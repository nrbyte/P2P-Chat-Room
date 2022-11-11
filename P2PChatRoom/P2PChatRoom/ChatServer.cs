using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace P2PChatRoom
{
    public class ChatServer
    {
        public ChatServer()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
        }
    }
}