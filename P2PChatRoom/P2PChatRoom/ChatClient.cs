using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Threading;

namespace P2PChatRoom
{
    public class ChatClient
    {
        public ChatClient()
        {
            Thread thread = new Thread(new ThreadStart(RunClient));
            thread.Start();
        }
        private void RunClient()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, NetworkManager.Constants.SEND_MSG_PORT);

            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try {
                sender.Connect(remoteEndPoint);
                Console.WriteLine("ChatClient.cs: Connected!");

                while (true)
                {
                    byte[] msg = Encoding.ASCII.GetBytes("TEST MESSAGE!");

                    int bytesSent = sender.Send(msg);
                }

            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}