using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Threading;

using System.Collections.Concurrent;

namespace P2PChatRoom
{
    public class ChatClient
    {
 
        public ConcurrentQueue<string> msgsToSend = new ConcurrentQueue<string>();

        public ChatClient()
        {
            Thread thread = new Thread(new ThreadStart(RunClient));
            thread.Start();
        }

        public void AddMessage(string device, string msg)
        {
            msgsToSend.Enqueue((device + msg));
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
                    string? msg = null;
                    if (msgsToSend.TryDequeue(out msg))
                    {
                        byte[] msgInBytes = Encoding.ASCII.GetBytes(msg);
                        sender.Send(msgInBytes);
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
            
        }
    }
}