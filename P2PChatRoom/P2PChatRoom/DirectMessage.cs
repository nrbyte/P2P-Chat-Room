using System.Collections.Generic;

namespace P2PChatRoom
{
    public class DirectMessage
    {
        private ChatHandler ch;

        public ChatClient chatClient;
        public List<string> messages;

        public string contactName { get; set; }

        public DirectMessage(ChatHandler ch, string contactName, string ipAddress)
        {
            this.ch = ch;

            chatClient = new ChatClient(ipAddress);
            messages = new List<string>();

            this.contactName = contactName;
        }

        public void ReceiveMessage(string sender, string msg)
        {
            ch.showMessage(sender, msg);
        }

        public void SendMessageOutward(string deviceName, string msg)
        {
            chatClient.msgsToSend.Enqueue((deviceName + "|" + msg));
        }

    }
}
