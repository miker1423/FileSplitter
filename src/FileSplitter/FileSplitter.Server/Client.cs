using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileSplitter.Server
{
    public class Client
    {
        private Encoding encoding = Encoding.UTF8;
        private Socket client;
        private int MaxAttemps = 5;
        private byte[] buffer = new byte[254];

        public Client(string server, int port)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            int attemps = 0;
            while (attemps < MaxAttemps)
            {
                try
                {
                    client.Connect(new IPEndPoint(IPAddress.Parse(server), port));
                    client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnBytesReceived), this);
                }
                catch (Exception)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    attemps++;
                }
            }
        }

        public void SendString(string data)
        {
            var bytes = encoding.GetBytes(data);
            client.Send(bytes);
        }

        public void OnBytesReceived(IAsyncResult result)
        {

        }
    }
}
