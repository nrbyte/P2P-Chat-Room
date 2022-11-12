using System;
using System.Windows;
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
        
        private List<ChatClient> clients;


        public void AddConnection(string ipAddress)
        {
            ChatClient cc = new ChatClient(ipAddress);
            clients.Add(cc);
        }

        public void SendMessage(string ipAddress, string device, string msg)
        {
            foreach (ChatClient clientConnection in clients)
            {
                if (clientConnection.ipAddress.ToString() == ipAddress)
                {
                    clientConnection.msgsToSend.Enqueue((device + msg));
                }
            }
        }

        public NetworkManager(ChatHandler ch)
        {
            cs = new ChatServer(ch);
            clients = new List<ChatClient>();

            this.networkBackgroundWorker = new BackgroundWorker();
        }
        
    }
}