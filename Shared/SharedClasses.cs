using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Shared
{
    [Serializable()]
    public class Command
    {
        public CommandType commandType;
        public string content;

        public Command(CommandType ct, string c, string s = null)
        {
            commandType = ct;
            content = c;
        }
    }

    public class Friend 
    {
        public string name;
        public bool isOnline;

        public Friend(string n, bool status = false)
        {
            name = n;
            isOnline = status;
        }

        public override string ToString()
        {
            if (isOnline)
                return name + " | ONLINE |";
            else return name + " | OFFLINE |";
        }

        public string format()
        {
            string status = isOnline ? "online" : "offline";
            return name + '$' + status;
        }
    }

    public class Utils
    {
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        public static char[] Separator = "##".ToCharArray();
    }

    public enum CommandType
    {
        REGISTER,
        REGISTER_SUCCESS,
        REGISTER_ERROR,
        LOGIN,
        LOGGED_IN,
        LOGIN_ERROR,
        FRIEND_ONLINE,
        FRIEND_OFFILNE,
        INVITE,
        INVITE_NOTIFICATION,
        INVITE_ACCEPTED,
        INVITE_REJECTED,
        GET_MESSAGES,
        SEND_MESSAGE,
        RECIVE_MESSAGE,
        MESSAGE_NOTIFICATION,
        REMOVE_FRIEND,
        FRIEND_REMOVED
    }
}
