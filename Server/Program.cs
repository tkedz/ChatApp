using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared;

namespace Server
{
    class Program
    {
        private static TcpListener server;
        private static List<Client> connectedClients = new List<Client>();
        private static MongoClient dbClient;
        private static IMongoCollection<BsonDocument> usersCollection;
        private static IMongoCollection<BsonDocument> messagesCollection;

        static void Main(string[] args)
        {
            try
            {
                dbClient = new MongoClient("mongodb://localhost:27017/");
                usersCollection = dbClient.GetDatabase("ChatApp").GetCollection<BsonDocument>("users");
                messagesCollection = dbClient.GetDatabase("ChatApp").GetCollection<BsonDocument>("messages");
                server = new TcpListener(IPAddress.Any, 9000);
                server.Start();

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Thread clientThread = new Thread(HandleClient);
                    clientThread.Start(client);
                }
            }
            catch
            {
                Console.WriteLine("brak połączenia z bazą danych / port na serwer zajęty");
            }
        }

        private static void HandleClient(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            BinaryReader reader = new BinaryReader(clientStream);
            BinaryWriter writer = new BinaryWriter(clientStream);

            try
            {
                Console.WriteLine("Client connected");
                List<Command> commandsQueue = new List<Command>();
                while (tcpClient.Connected)
                {
                    int bytesToRead = reader.ReadInt32();
                    Console.WriteLine(bytesToRead);
                    byte[] recievedCommand = reader.ReadBytes(bytesToRead);
                    Command command = (Command)Utils.ByteArrayToObject(recievedCommand);
                    Console.WriteLine(command.commandType + " " + command.content);

                    string[] content = command.content.Split(Utils.Separator);
                    Command c = null;

                    if (command.commandType == CommandType.REGISTER)
                    {
                        if(content[0] != String.Empty && content[2] != String.Empty)
                        {
                            c = register(content[0], content[2]);
                        }
                    }
                    else if (command.commandType == CommandType.LOGIN)
                    {
                        if(content[0]!=String.Empty && content[2]!=String.Empty)
                        {
                            c = login(content[0], content[2], tcpClient);
                            notifyFriends(tcpClient, true);
                        }
                    }
                    else if (command.commandType == CommandType.INVITE)
                    {
                        bool result = saveInviteToDb(content[0], content[2]);
                        if (result) notifyAboutInvite(content[0], content[2]);
                    }
                    else if(command.commandType == CommandType.INVITE_ACCEPTED)
                    {
                        c = inviteAccepted(content[0], content[2]);
                    }
                    else if(command.commandType == CommandType.INVITE_REJECTED)
                    {
                        c = inviteRejected(content[0], content[2]);
                    }
                    else if(command.commandType == CommandType.GET_MESSAGES)
                    {
                        c = getMessages(content[0], content[2]);
                    }
                    else if(command.commandType == CommandType.SEND_MESSAGE)
                    {
                        sendMessage(content[0], content[2], content[4]);
                    }
                    else if(command.commandType == CommandType.REMOVE_FRIEND)
                    {
                        c = removeFriend(content[0], content[2]);
                    }

                    if(c!=null)
                    {
                        byte[] convertedCommand = Utils.ObjectToByteArray(c);
                        writer.Write(convertedCommand.Length);
                        writer.Flush();
                        writer.Write(convertedCommand);
                        writer.Flush();
                    }
                }
                Console.WriteLine(tcpClient.ToString() + " closed");
                tcpClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Client disconnected");
                Console.WriteLine(e.Message);
                logout(tcpClient);
                tcpClient.Close();
            }
        }

        private static void send(Client to, Command command)
        {
            try
            {
                NetworkStream stream = to.tcpClient.GetStream();
                BinaryWriter writer = new BinaryWriter(stream);
                byte[] convertedCommand = Utils.ObjectToByteArray(command);
                writer.Write(convertedCommand.Length);
                writer.Flush();
                writer.Write(convertedCommand);
                writer.Flush();
            }
            catch
            {
                Console.WriteLine("Blad, komunikat do " + to.username + " niewyslany");
            }
        }

        private static Command login(string username, string password, TcpClient tcpClient)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filt = builder.Eq("username", username) & builder.Eq("password", password);
            var userDocument = usersCollection.Find(filt).FirstOrDefault();

            if (userDocument == null)
                return new Command(CommandType.LOGIN_ERROR, "Logowanie nieudane");

            //get friends of logged user
            List<Friend> friendList = getFriendsOf(userDocument);
            string friends = "";
            for (int i = 0; i < friendList.Count; i++)
            {
                if (i != friendList.Count - 1) friends += friendList[i].format() + '$';
                else friends += friendList[i].format();
            }

            //get pending invites 
            string invites = getInvites(userDocument);

            connectedClients.Add(new Client(username, tcpClient, friendList));
            return new Command(CommandType.LOGGED_IN, username + new string(Utils.Separator) + "FRIENDS:"+friends + new string(Utils.Separator) + "INVITES:"+invites);
        }

        private static void logout(TcpClient tcpClient)
        {
            notifyFriends(tcpClient, false);
            int index = connectedClients.FindIndex(c => c.tcpClient == tcpClient);
            if (index > -1)
            {
                connectedClients.RemoveAt(index);
            }
        }

        private static Command register(string username, string password)
        {
            var userDocument = usersCollection.Find(Builders<BsonDocument>.Filter.Eq("username", username)).FirstOrDefault();
            if (userDocument == null)
            {
                var newUser = new BsonDocument {
                            {"username", username }, {"password", password}, { "friends", new BsonArray { } }, { "pendingInvites", new BsonArray { } }
                        };

                usersCollection.InsertOne(newUser);
                return new Command(CommandType.REGISTER_SUCCESS, "Rejestracja udana");
            }
            return new Command(CommandType.REGISTER_ERROR, "Login " + username + " jest zajęty");
        }

        private static List<Friend> getFriendsOf(BsonDocument user)
        {
            List<Friend> result = new List<Friend>();
            var friendList = user["friends"].AsBsonArray.Select(f => f.AsString).ToArray();
            foreach (var friend in friendList)
            {
                bool status = connectedClients.Any(c => c.username == friend);
                result.Add(new Friend(friend, status));
            }
            return result;
        }

        private static void notifyFriends(TcpClient tcpClient, bool status)
        {
            Client client = connectedClients.Find(c => c.tcpClient == tcpClient);
            if (client == null) return;
            foreach (Friend f in client.friendList)
            {
                Client friend = connectedClients.Find(c => c.username == f.name);
                if (friend != null)
                {
                    Command command;
                    if (status)
                        command = new Command(CommandType.FRIEND_ONLINE, client.username);
                    else command = new Command(CommandType.FRIEND_OFFILNE, client.username);
                    send(friend, command);
                }
            }
        }

        private static bool saveInviteToDb(string from, string to)
        {
            var userDocument = usersCollection.Find(Builders<BsonDocument>.Filter.Eq("username", to)).FirstOrDefault();
            if(userDocument != null)
            {
                var friends = userDocument["friends"].AsBsonArray.Select(f => f.AsString).ToList();
                if (!friends.Contains(from))
                {
                    int index = userDocument["pendingInvites"].AsBsonArray.IndexOf(from);
                    if(index == -1)
                    {
                        userDocument["pendingInvites"].AsBsonArray.Add(from);
                        usersCollection.ReplaceOne(Builders<BsonDocument>.Filter.Eq("username", to), userDocument);
                        return true;
                    }
                }
            }
            return false;
        }

        private static string getInvites(BsonDocument user)
        {
            List<string> invitesList = user["pendingInvites"].AsBsonArray.Select(f => f.AsString).ToList();
            string invites = "";
            for (int i = 0; i < invitesList.Count; i++)
            {
                if (i != invitesList.Count - 1) invites += invitesList[i] + '$';
                else invites += invitesList[i];
            }
            return invites;
        }

        private static void notifyAboutInvite(string from, string to)
        {
            Client client = connectedClients.Find(c => c.username == to);
            if (client == null) return;
            Command command = new Command(CommandType.INVITE_NOTIFICATION, from);
            send(client, command);
        }

        private static Command inviteAccepted(string acceptedBy, string recivedFrom)
        {
            //update database
            var acceptedByDoc = usersCollection.Find(Builders<BsonDocument>.Filter.Eq("username", acceptedBy)).FirstOrDefault();
            var recivedFromDoc = usersCollection.Find(Builders<BsonDocument>.Filter.Eq("username", recivedFrom)).FirstOrDefault();
            if (acceptedByDoc == null || recivedFromDoc == null) return null;
            //remove invite
            acceptedByDoc["pendingInvites"].AsBsonArray.Remove(recivedFrom);
            recivedFromDoc["pendingInvites"].AsBsonArray.Remove(acceptedBy);
            //add new friend
            acceptedByDoc["friends"].AsBsonArray.Add(recivedFrom);
            recivedFromDoc["friends"].AsBsonArray.Add(acceptedBy);

            //update db
            usersCollection.ReplaceOne(Builders<BsonDocument>.Filter.Eq("username", acceptedBy), acceptedByDoc);
            usersCollection.ReplaceOne(Builders<BsonDocument>.Filter.Eq("username", recivedFrom), recivedFromDoc);

            //check if sender of invite is online and notify him about accepted invitation
            Client client = connectedClients.Find(c => c.username == recivedFrom);
            string status = "offline";
            bool _status = false;
            if(client != null)
            {
                status = "online";
                _status = true;
                client.friendList.Add(new Friend(acceptedBy, _status));
                Command command = new Command(CommandType.INVITE_ACCEPTED, acceptedBy + new string(Utils.Separator) + status);
                send(client, command);
            }
            client = connectedClients.Find(c => c.username == acceptedBy);
            client.friendList.Add(new Friend(recivedFrom, _status));
            return new Command(CommandType.INVITE_ACCEPTED, recivedFrom+new string(Utils.Separator)+status);
        }

        private static Command inviteRejected(string rejectedBy, string recivedFrom)
        {
            var rejectedByDoc = usersCollection.Find(Builders<BsonDocument>.Filter.Eq("username", rejectedBy)).FirstOrDefault();
            if (rejectedByDoc == null) return null;
            rejectedByDoc["pendingInvites"].AsBsonArray.Remove(recivedFrom);
            usersCollection.ReplaceOne(Builders<BsonDocument>.Filter.Eq("username", rejectedBy), rejectedByDoc);
            return new Command(CommandType.INVITE_REJECTED, recivedFrom);
        }

        private static Command removeFriend(string removing, string removedFriend)
        {
            //update database
            var removingDoc = usersCollection.Find(Builders<BsonDocument>.Filter.Eq("username", removing)).FirstOrDefault();
            var removedFriendDoc = usersCollection.Find(Builders<BsonDocument>.Filter.Eq("username", removedFriend)).FirstOrDefault();
            if (removingDoc == null || removedFriendDoc == null) return null;
            //remove invite
            removingDoc["friends"].AsBsonArray.Remove(removedFriend);
            removedFriendDoc["friends"].AsBsonArray.Remove(removing);

            //update db
            usersCollection.ReplaceOne(Builders<BsonDocument>.Filter.Eq("username", removing), removingDoc);
            usersCollection.ReplaceOne(Builders<BsonDocument>.Filter.Eq("username", removedFriend), removedFriendDoc);

            //usuniecie przyjaciela z listy przechowywanej na serwerze
            Client client = connectedClients.Find(c => c.username == removing);
            int index = client.friendList.FindIndex(f => f.name == removedFriend);
            if (index > -1) client.friendList.RemoveAt(index);

            //sprawdzenie czy usuniety przyjaciel jest online i wyslanie mu info
            client = connectedClients.Find(c => c.username == removedFriend);
            if (client != null)
            {
                index = client.friendList.FindIndex(f => f.name == removing);
                if (index > -1) client.friendList.RemoveAt(index);
                Command command = new Command(CommandType.FRIEND_REMOVED, removing);
                send(client, command);
            }

            return new Command(CommandType.FRIEND_REMOVED, removedFriend);
        }

        private static Command getMessages(string user1, string user2)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("user1", user1) & builder.Eq("user2", user2);
            var messagesDocument = messagesCollection.Find(filter).FirstOrDefault();
            if (messagesDocument == null)
            {
                filter = builder.Eq("user1", user2) & builder.Eq("user2", user1);
                messagesDocument = messagesCollection.Find(filter).FirstOrDefault();
                if (messagesDocument == null) return new Command(CommandType.GET_MESSAGES, user1 + new string(Utils.Separator) + user2 + new string(Utils.Separator) + "");
            }

            List<BsonDocument> messagesList = messagesDocument["messages"].AsBsonArray.Select(m => m.AsBsonDocument).ToList();

            string messages = "";
            for(int i=0; i<messagesList.Count;i++)
            {
                if (i != messagesList.Count - 1)
                    messages += messagesList[i]["sender"].AsString + '$' + messagesList[i]["message"].AsString + '$';
                else messages += messagesList[i]["sender"].AsString + '$' + messagesList[i]["message"].AsString;
            }

            if (messages == "") return new Command(CommandType.GET_MESSAGES, user1 + new string(Utils.Separator) + user2 + new string(Utils.Separator) + "");

            return new Command(CommandType.GET_MESSAGES, user1 + new string(Utils.Separator) + user2 + new string(Utils.Separator) + messages);
        }

        private static void sendMessage(string from, string to, string message)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("user1", from) & builder.Eq("user2", to);
            var messagesDocument = messagesCollection.Find(filter).FirstOrDefault();
            //pierwsza proba - nie znaleziono
            if (messagesDocument == null)
            {
                filter = builder.Eq("user1", to) & builder.Eq("user2", from);
                messagesDocument = messagesCollection.Find(filter).FirstOrDefault();
                
                //pierwsza wiadomosc pomiedzy tymi uzytkownikami, wiec tworzymy nowy wpis w bazie
                if (messagesDocument == null)
                {
                    var newMessage = new BsonDocument {
                            {"user1", from }, {"user2", to}, { "messages", new BsonArray { new BsonDocument { {"sender", from }, {"message", message } } } }
                        };

                    messagesCollection.InsertOne(newMessage);
                }
                else
                {
                    messagesDocument["messages"].AsBsonArray.Add(new BsonDocument { { "sender", from }, { "message", message } });
                    messagesCollection.ReplaceOne(filter, messagesDocument);
                }
            }
            else
            {
                messagesDocument["messages"].AsBsonArray.Add(new BsonDocument { { "sender", from }, { "message", message } });
                messagesCollection.ReplaceOne(filter, messagesDocument);
            }

            //wyslanie wiadomosci socketem, jesli odbiorca jest online
            Client client = connectedClients.Find(c => c.username == to);
            if (client == null) return;
            Command command = new Command(CommandType.RECIVE_MESSAGE, from + new string(Utils.Separator) + message);
            send(client, command);
        }
    }
}
