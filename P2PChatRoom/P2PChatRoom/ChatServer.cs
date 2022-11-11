using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace P2PChatRoom
{
    public class ChatServer
    {
        public ChatServer()
        {
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

                string dataRecieved = "";
                byte[] bytes = new byte[NetworkManager.Constants.MAX_MESSAGE_SIZE];

                bool serverRunning = true;
                while (serverRunning)
                {
                    bytes = new byte[NetworkManager.Constants.MAX_MESSAGE_SIZE];

                    int bytesRecieved = handler.Receive(bytes);
                    dataRecieved = Encoding.ASCII.GetString(bytes, 0, bytesRecieved);

                    string deviceName = dataRecieved.Substring(0, 10);
                    string msg = dataRecieved.Substring(10);
                    Console.WriteLine($"{deviceName}: {msg}");

                    if (msg == "SHUTDOWN${$}") {
                        serverRunning = false;
                    }
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