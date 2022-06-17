using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandType = Shared.CommandType;

namespace Client
{
    public partial class Form1 : Form
    {
        private static string loggedUser = String.Empty;
        private static List<Friend> friendList = null;
        private static List<string> invites = null;
        private static List<Message> messages = null;
        private static Friend chatWith = null;
        private static string newMessageFromNotification = "";
        private static TcpClient client;
        private static NetworkStream clientStream;
        private static BinaryReader reader;
        private static BinaryWriter writer;
        public Form1()
        {
            try
            {
                InitializeComponent();
                client = new TcpClient("127.0.0.1", 9000);
                clientStream = client.GetStream();
                reader = new BinaryReader(clientStream);
                writer = new BinaryWriter(clientStream);
                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.RunWorkerAsync();
            }
            catch(Exception e)
            {
                panelError.Visible = true;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while(client.Connected)
            {
                try
                {
                    int bytesToRead = reader.ReadInt32();
                    byte[] recievedCommand = reader.ReadBytes(bytesToRead);
                    Command command = (Command)Utils.ByteArrayToObject(recievedCommand);
                    string[] content = command.content.Split(Utils.Separator, StringSplitOptions.RemoveEmptyEntries);

                    switch (command.commandType)
                    {
                        case CommandType.LOGGED_IN:
                            loggedUser = content[0];
                        
                            friendList = new List<Friend>();
                            if(content.Length > 1 && content[1].Replace("FRIENDS:", String.Empty) != String.Empty)
                            {
                                //recreate FriendList
                                content[1] = content[1].Replace("FRIENDS:", String.Empty);
                                string[] tmp = content[1].Split('$');
                                for(int i = 0; i < tmp.Length; i+=2)
                                {
                                    bool status = tmp[i+1] == "online" ? true : false;
                                    friendList.Add(new Friend(tmp[i], status));
                                }
                            }

                            invites = new List<string>();
                            if(content.Length > 2 && content[2].Replace("INVITES:", String.Empty) != String.Empty)
                            {
                                //recreate invitesList
                                content[2] = content[2].Replace("INVITES:", String.Empty);
                                string[] tmp = content[2].Split('$');
                                for(int i = 0; i < tmp.Length; i++)
                                {
                                    invites.Add(tmp[i]);
                                }
                            }

                        
                            backgroundWorker1.ReportProgress(3,  CommandType.LOGGED_IN );
                            break;
                        case CommandType.LOGIN_ERROR:
                            backgroundWorker1.ReportProgress(3,  CommandType.LOGIN_ERROR );
                            break;
                        case CommandType.REGISTER_SUCCESS:
                            backgroundWorker1.ReportProgress(3, CommandType.REGISTER_SUCCESS);
                            break;
                        case CommandType.REGISTER_ERROR:
                            backgroundWorker1.ReportProgress(3, CommandType.REGISTER_ERROR);
                            break;
                        case CommandType.FRIEND_ONLINE:
                            int index = friendList.FindIndex(f => f.name == content[0]);
                            if (index > -1)
                            {
                                friendList[index].isOnline = true;
                                backgroundWorker1.ReportProgress(3, CommandType.FRIEND_ONLINE);
                            }
                            break;
                        case CommandType.FRIEND_OFFILNE:
                            int index1 = friendList.FindIndex(f => f.name == content[0]);
                            if (index1 > -1)
                            {
                                friendList[index1].isOnline = false;
                                backgroundWorker1.ReportProgress(3, CommandType.FRIEND_OFFILNE);
                            }
                            break;
                        case CommandType.INVITE_NOTIFICATION:
                            invites.Add(content[0]);
                            backgroundWorker1.ReportProgress(3, CommandType.INVITE_NOTIFICATION);
                            break;
                        case CommandType.INVITE_ACCEPTED:
                            int index2 = invites.FindIndex(i => i == content[0]);
                            if(index2 > -1) invites.RemoveAt(index2);
                            bool isOnline = content[1] == "online" ? true : false;
                            friendList.Add(new Friend(content[0], isOnline));
                            backgroundWorker1.ReportProgress(3, CommandType.INVITE_ACCEPTED);
                            break;
                        case CommandType.INVITE_REJECTED:
                            int index3 = invites.FindIndex(i => i == content[0]);
                            if (index3 > -1) invites.RemoveAt(index3);
                            backgroundWorker1.ReportProgress(3, CommandType.INVITE_REJECTED);
                            break;
                        case CommandType.GET_MESSAGES:
                            messages = new List<Message>();
                            if(content.Length > 2)
                            {
                                string[] tmp1 = content[2].Split('$');
                                for(int i=0;i<tmp1.Length;i+=2)
                                {
                                    messages.Add(new Message(tmp1[i], tmp1[i + 1]));
                                }

                            }
                            backgroundWorker1.ReportProgress(3, CommandType.GET_MESSAGES);
                            break;
                        case CommandType.RECIVE_MESSAGE:
                            if(chatWith != null && chatWith.name == content[0])
                            {
                                messages.Add(new Message(content[0], content[1]));
                                backgroundWorker1.ReportProgress(3, CommandType.GET_MESSAGES);
                            }
                            else
                            {
                                newMessageFromNotification = content[0];
                                backgroundWorker1.ReportProgress(3, CommandType.MESSAGE_NOTIFICATION);
                            
                            }
                            break;
                        case CommandType.FRIEND_REMOVED:
                            chatWith = null;
                            int index4 = friendList.FindIndex(f => f.name == content[0]);
                            if (index4 > -1) friendList.RemoveAt(index4);
                            backgroundWorker1.ReportProgress(3, CommandType.FRIEND_REMOVED);
                            break;
                    }
                }
                catch
                {
                    MessageBox.Show("Stracono polaczenie z serwerem, zrestartuj aplikacje");
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CommandType commandType = (CommandType)e.UserState;

            switch(commandType)
            {
                case CommandType.LOGGED_IN:
                    label1.Text = "jestem?";
                    refreshFriendList();
                    refreshInvites();
                    label1.Visible = false;
                    panelStart.Visible = false;
                    panelMain.Visible = true;
                    break;
                case CommandType.LOGIN_ERROR:
                    label1.Visible = true;
                    label1.Text = "Logowanie nieudane";
                    break;
                case CommandType.REGISTER_SUCCESS:
                    label1.Visible = true;
                    label1.Text = "Rejestracja udana, mozesz sie zalogowac";
                    break;
                case CommandType.REGISTER_ERROR:
                    label1.Visible = true;
                    label1.Text = "Podany login jest zajety";
                    break;
                case CommandType.FRIEND_ONLINE:
                    refreshFriendList();
                    break;
                case CommandType.FRIEND_OFFILNE:
                    refreshFriendList();
                    break;
                case CommandType.INVITE_NOTIFICATION:
                    refreshInvites();
                    break;
                case CommandType.INVITE_ACCEPTED:
                    refreshInvites();
                    refreshFriendList();
                    break;
                case CommandType.INVITE_REJECTED:
                    refreshInvites();
                    break;
                case CommandType.GET_MESSAGES:
                    refreshMessages();
                    break;
                case CommandType.MESSAGE_NOTIFICATION:
                    showTooltip("Nowa wiadomosc od " + newMessageFromNotification);
                    break;
                case CommandType.FRIEND_REMOVED:
                    panelChat.Visible = false;
                    refreshFriendList();
                    break;
            }
        }

        private void showTooltip(string message)
        {
            Label l = new Label();
            l.Text = message;
            l.Width = l.Width * 2;
            panelNotifications.Controls.Add(l);
            Timer t = new Timer();
            t.Interval = 5000;
            t.Tick += (s, e) =>
            {
                panelNotifications.Controls.Remove(l);
                t.Stop();
            };
            t.Start();
        }

        private void refreshFriendList()
        {
           if(friendList != null)
            {
                listBoxFriends.Items.Clear();
                foreach (var friend in friendList)
                {
                    listBoxFriends.Items.Add(friend);
                }
            }
        }

        private void refreshInvites()
        {
            if(invites != null)
            {
                listBoxInvites.Items.Clear();
                
                foreach(string inv in invites)
                {
                    listBoxInvites.Items.Add(inv);
                }
                
            }
        }

        private void refreshMessages()
        {
            if(messages != null)
            {
                listBoxMessages.Items.Clear();
                foreach (var message in messages)
                {
                    listBoxMessages.Items.Add(message);
                }
                panelChat.Visible = true;
            }
        }

        private bool send(Command command)
        {
            try
            {
                byte[] commandToTransport = Utils.ObjectToByteArray(command);
                writer.Write(commandToTransport.Length);
                writer.Flush();
                writer.Write(commandToTransport);
                writer.Flush();
                return true;
            } 
            catch
            {
                MessageBox.Show("Stracono połączenie z serwerem, zrestartuj aplikacje");
                return false;
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (UsernameTextBox.Text != String.Empty && PasswordTextBox.Text != String.Empty)
            {
                Command command = new Command(CommandType.LOGIN, UsernameTextBox.Text + new string(Utils.Separator) + PasswordTextBox.Text);
                send(command);
            }
            else
            {
                label1.Visible = true;
                label1.Text = "Oba pola wymagane";
            }
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            if (UsernameTextBox.Text != String.Empty && PasswordTextBox.Text != String.Empty)
            {
                Command command = new Command(CommandType.REGISTER, UsernameTextBox.Text + new string(Utils.Separator) + PasswordTextBox.Text);
                send(command);
            }
            else
            {
                label1.Visible = true;
                label1.Text = "Oba pola wymagane";
            }
        }

        private void buttonSendInvite_Click(object sender, EventArgs e)
        {
            string user = textBoxInvite.Text;
            if(user != String.Empty && user != loggedUser)
            {
                Command command = new Command(CommandType.INVITE, loggedUser + new string(Utils.Separator) + user);
                if (send(command))
                {
                    MessageBox.Show("Wysłano zaproszenie");
                    textBoxInvite.Text = String.Empty;
                }
            }
        }

        private void buttonAcceptInvite_Click(object sender, EventArgs e)
        {
            string user = (string)listBoxInvites.SelectedItem;
            if(user!=String.Empty)
            {
                Command command = new Command(CommandType.INVITE_ACCEPTED, loggedUser + new string(Utils.Separator) + user);
                send(command);
            }
        }

        private void buttonRejectInvite_Click(object sender, EventArgs e)
        {
            string user = (string)listBoxInvites.SelectedItem;
            if (user != String.Empty)
            {
                Command command = new Command(CommandType.INVITE_REJECTED, loggedUser + new string(Utils.Separator) + user);
                send(command);
            }
        }

        private void listBoxFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            Friend f = (Friend)listBoxFriends.SelectedItem;
            if(f != null)
            {
                chatWith = f;
                labelChatWith.Text = chatWith.name.ToUpper();
                Command command = new Command(CommandType.GET_MESSAGES, loggedUser + new string(Utils.Separator) + f.name);
                send(command);
            }
        }

        private void buttonSendMessage_Click(object sender, EventArgs e)
        {
            string message = textBoxMessage.Text;
            if(chatWith != null && message != String.Empty)
            {
                Command command = new Command(CommandType.SEND_MESSAGE, loggedUser + new string(Utils.Separator) + chatWith.name + new string(Utils.Separator) + message);
                if(send(command))
                {
                    messages.Add(new Message(loggedUser, message));
                    refreshMessages();
                    textBoxMessage.Text = String.Empty;
                }
            }
        }

        private void textBoxMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return) buttonSendMessage_Click(null, null);
        }

        private void buttonRemoveFriend_Click(object sender, EventArgs e)
        {
            if(chatWith != null)
            {
                Command command = new Command(CommandType.REMOVE_FRIEND, loggedUser +new string(Utils.Separator)+chatWith.name);
                send(command);
            }
        }
    }
}
