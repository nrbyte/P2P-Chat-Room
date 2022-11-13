using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Threading;

using System.Collections.Concurrent;
using System.Diagnostics;

namespace P2PChatRoom
{
    public class ChatClient
    {
 
        public ConcurrentQueue<string> msgsToSend = new ConcurrentQueue<string>();

        public IPAddress ipAddress {get; private set;}


        public ChatClient(string ip)
        {
            IPHostEntry host = Dns.GetHostEntry(ip);
            this.ipAddress = host.AddressList[0];
            Thread thread = new Thread(new ThreadStart(RunClient));
            thread.Start();
        }

        public void AddMessage(string device, string msg)
        {
            msgsToSend.Enqueue((device + msg));
        }
        
        private void RunClient()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, NetworkManager.Constants.SEND_MSG_PORT);

            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            bool successful = false;

            for (int i = 0; i < 10; i++)
            {
                if (successful) continue;
                try
                {
                    sender.Connect(remoteEndPoint);
                    Trace.WriteLine($"ChatClient.cs: Connected to {(sender.RemoteEndPoint as IPEndPoint).Address}");

                    while (true)
                    {
                        successful = true;
                        string? msg = null;
                        if (msgsToSend.TryDequeue(out msg))
                        {
                            Trace.WriteLine($"Sending message: {msg} to {(sender.RemoteEndPoint as IPEndPoint).Address}");
                            byte[] msgInBytes = Encoding.ASCII.GetBytes(msg.PadRight(NetworkManager.Constants.MAX_MESSAGE_SIZE));
                            sender.Send(msgInBytes);
                            Trace.WriteLine($"Sent message");
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.ToString());
                }
            }

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
            
        }
    }
}