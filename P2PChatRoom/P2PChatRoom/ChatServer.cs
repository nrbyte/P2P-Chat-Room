using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace P2PChatRoom
{
    
    public interface ChatHandler
    {
        void showMessage(string sender, string messageReceived);
    }

    public class ChatServer
    {

        private ChatHandler chatHandler;

        public ChatServer(ChatHandler chatHandler)
        {
            this.chatHandler = chatHandler;

            Thread thread = new Thread(new ThreadStart(RunServer));
            thread.Start();
        }
        private void RunServer()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, NetworkManager.Constants.RECV_MSG_PORT);

            try {

                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(localEndPoint);

                listener.Listen(NetworkManager.Constants.MAX_CONN);
                
                Console.WriteLine("ChatServer.cs: Waiting for connections!");

                Socket handler = listener.Accept();

                Console.WriteLine("ChatServer.cs: Got a connection!");

                string dataReceived = "";
                byte[] bytes = new byte[NetworkManager.Constants.MAX_MESSAGE_SIZE];

                bool serverRunning = true;
                while (serverRunning)
                {
                    bytes = new byte[NetworkManager.Constants.MAX_MESSAGE_SIZE];

                    int bytesReceived = handler.Receive(bytes);
                    dataReceived = Encoding.ASCII.GetString(bytes, 0, bytesReceived);

                    string[] dataReceivedSplit = dataReceived.Split("|");

                    string sender = dataReceivedSplit[0];
                    string msg = dataReceivedSplit[1];
                    Console.WriteLine($"{sender}: {msg}");

                    System.Windows.Application.Current.Dispatcher.Invoke(() => {
                        chatHandler.showMessage(sender, msg);
                    });

                }

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}