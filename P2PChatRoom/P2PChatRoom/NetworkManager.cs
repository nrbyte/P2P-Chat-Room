using System;
using System.Windows;
using System.ComponentModel;

using System.Threading;

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

        public ChatServer cs;
        public ChatClient cc;

        public void AddMessage(string device, string msg)
        {
            cc.msgsToSend.Enqueue((device + msg));
        }


        public NetworkManager(ChatHandler ch)
        {
            cs = new ChatServer(ch);
            cc = new ChatClient();

            this.networkBackgroundWorker = new BackgroundWorker();
        }
        
    }
}