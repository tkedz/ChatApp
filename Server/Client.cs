using Shared;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Client
    {
        public TcpClient tcpClient;
        public string username;
        public List<Friend> friendList;

        public Client(string un, TcpClient tc, List<Friend> fl)
        {
            username = un;
            tcpClient = tc;
            friendList = fl;
        }
    }
}
