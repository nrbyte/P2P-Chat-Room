using System;

using System.Collections.Generic;
using System.Linq;

namespace P2PChatRoom
{
    public class NetworkManager : ChatHandler
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

        public List<DirectMessage> directMessages;

        private ChatServer cs;

        public void showMessage(string sender, string messageRecieved)
        {
            directMessages.Where(x => x.contactName == sender).First().RecieveMessage(sender, messageRecieved);
        }

        public NetworkManager(ChatHandler ch)
        {
            directMessages = new List<DirectMessage>();

            cs = new ChatServer(this);
        }

        public void SendMessageOutward(string sendTo, string deviceName, string msg)
        {
            directMessages.Where(x => x.contactName == sendTo).First().SendMessageOutward(deviceName, msg);
        }
        
        public void AddDirectMessage(DirectMessage directMessage)
        {
            directMessages.Add(directMessage);
        }
    }
}