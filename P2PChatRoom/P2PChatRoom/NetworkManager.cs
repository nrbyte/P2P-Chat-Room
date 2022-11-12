using System;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;

using System.Collections.Generic;

namespace P2PChatRoom
{
    public class NetworkManager
    {

        public struct Constants
        {
            public static int MAX_CONN = 16;
            public static int MAX_MESSAGE_SIZE = 512;

            // Used by the client
            public static int SEND_MSG_PORT = 1984;
            // Used by the server
            public static int RECV_MSG_PORT = 1984;
        }

        private BackgroundWorker networkBackgroundWorker;

        private ChatServer cs;
        
        private Dictionary<String, ChatClient> clientContacts;


        public void AddConnection(string name, string ipAddress)
        {
            ChatClient cc = new ChatClient(ipAddress);
            clientContacts.Add(name, cc);
        }

        public void SendMessage(string name, string device, string msg)
        {
            clientContacts[name].msgsToSend.Enqueue(device + msg);
        }

        public NetworkManager(ChatHandler ch)
        {
            cs = new ChatServer(ch);
            clientContacts = new Dictionary<String, ChatClient>();

            this.networkBackgroundWorker = new BackgroundWorker();
        }
        
    }
}