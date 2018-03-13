using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FileSplitter.Server
{
    public class Client
    {
        TcpClient client;
        NetworkStream stream;
        public Client(string server, int port)
        {
            client = new TcpClient(server, port);
            stream = client.GetStream();
        }

        public async Task ReadMessage()
        {
        } 
    }
}
