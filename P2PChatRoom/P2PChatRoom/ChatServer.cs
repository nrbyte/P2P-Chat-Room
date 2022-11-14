using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Documents;

using System.Collections.Generic;

namespace P2PChatRoom
{
    
    public interface ChatSorter
    {
        void sortMessage(IPAddress senderIP, string messageReceived);
    }

    public interface ChatHandler
    {
        void showMessage(string sender, string messageReceived);
    }

    public class ChatServer
    {

        private ChatSorter chatSorter;

        public ChatServer(ChatSorter chatSorter)
        {
            this.chatSorter = chatSorter;

            Thread thread = new Thread(new ThreadStart(AcceptConnections));
            thread.Start();
        }

        public void RunServer(Socket handler)
        {
            Trace.WriteLine("ChatServer.cs: Got a connection!");

            string dataReceived = "";
            byte[] bytes = new byte[NetworkManager.Constants.MAX_MESSAGE_SIZE];

            bool serverRunning = true;
            while (serverRunning)
            {
                bytes = new byte[NetworkManager.Constants.MAX_MESSAGE_SIZE];

                int bytesReceived = handler.Receive(bytes);
                Trace.WriteLine("Recieved Message");
                dataReceived = Encoding.ASCII.GetString(bytes, 0, bytesReceived);

                string[] dataReceivedSplit = dataReceived.Split("|");

                string sender = dataReceivedSplit[0];

                string msg = dataReceivedSplit[1];
                Console.WriteLine($"{sender}: {msg}");

                System.Windows.Application.Current.Dispatcher.Invoke(() => {
                    Trace.WriteLine($"Recieved message from {(handler.RemoteEndPoint as IPEndPoint).Address}");
                    chatSorter.sortMessage((handler.RemoteEndPoint as IPEndPoint).Address, msg);
                });

            }

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        private void AcceptConnections()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, NetworkManager.Constants.RECV_MSG_PORT);

            List<Thread> allConnections = new List<Thread>();

            try {

                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(localEndPoint);

                listener.Listen(NetworkManager.Constants.MAX_CONN);
                
                Console.WriteLine("ChatServer.cs: Waiting for connections!");

                while (true)
                {
                    Socket handler = listener.Accept();

                    Thread thread = new Thread(() => { RunServer(handler); });
                    thread.Start();

                    allConnections.Add(thread);
                }

            } catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }
    }
}