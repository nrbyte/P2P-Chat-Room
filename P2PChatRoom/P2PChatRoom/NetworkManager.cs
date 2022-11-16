using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using System.Net;


namespace P2PChatRoom
{
    public class NetworkManager : ChatSorter
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

        public ChatServer cs;

        public void sortMessage(IPAddress senderIP, string messageReceived)
        {
            DirectMessage directMessage = directMessages.Where(x => x.chatClient.ipAddress.Equals(senderIP)).First();
            directMessage.ReceiveMessage(directMessage.contactName, messageReceived);
        }

        public NetworkManager(ChatHandler ch, NewDMHandler newDMHandler)
        {
            directMessages = new List<DirectMessage>();

            cs = new ChatServer(this, newDMHandler);
        }

        public bool SendMessageOutward(string recipient, string sender, string msg)
        {
            try
            {
                directMessages.Where(x => x.contactName == recipient).First().SendMessageOutward(sender, msg);
                return true;
            }
            catch
            {
                MessageBox.Show("Contact doesn't exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        
        public void AddDirectMessage(DirectMessage directMessage)
        {
            directMessages.Add(directMessage);
        }
    }
}