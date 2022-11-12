using System.Collections.Generic;

namespace P2PChatRoom
{
    public class DirectMessage
    {
        private ChatHandler ch;

        private ChatClient chatClient;
        public List<string> messages;

        public string contactName { get; }

        public DirectMessage(ChatHandler ch, string contactName, string ipAddress)
        {
            this.ch = ch;

            chatClient = new ChatClient(ipAddress);
            messages = new List<string>();

            this.contactName = contactName;
        }

        public void RecieveMessage(string sender, string msg)
        {
            ch.showMessage(sender, msg);
        }

        public void SendMessageOutward(string deviceName, string msg)
        {
            chatClient.msgsToSend.Enqueue((deviceName + msg));
        }

    }
}
