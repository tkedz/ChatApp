namespace Client
{
    class Message
    {
        public string sender;
        public string msg;

        public Message(string s, string m)
        {
            sender = s;
            msg = m;
        }

        public override string ToString()
        {
            return sender + ": " + msg;
        }
    }
}
