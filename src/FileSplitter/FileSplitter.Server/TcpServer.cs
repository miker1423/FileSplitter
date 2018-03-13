using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

using FileSplitter.Splitter;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;

namespace FileSplitter.Server
{
    public class TcpServer
    {
        private Socket socket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        private BasicSplitter splitter = new BasicSplitter();
        private ConcurrentBag<NetworkStream> Clients = new ConcurrentBag<NetworkStream>();
        private int counter = 0;

        public TcpServer()
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, 25000));
            socket.Listen(5);
            socket.BeginAccept(OnConnectRequest, socket);
        }

        private void OnConnectRequest(IAsyncResult result)
        {
            var clientSocket = result.AsyncState as Socket;
            var connection = new Connection(socket.EndAccept(result));
            socket.BeginAccept(OnConnectRequest, socket);
        }

        public void GetFile(string fileName)
        {
            var files = splitter.GetFile(fileName);
            var json = JsonConvert.SerializeObject(files);
            Send(Encoding.UTF8.GetBytes(json));
        }

        public void SendFile(string path)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            var fileExtension = Path.GetExtension(path);
            var data = File.ReadAllBytes(path);
            var splitted = splitter.Split(data, 3);

            foreach (var span in splitted)
            {
                var fileDescriptor = new FileDescriptor()
                {
                    Content = span.ToArray(),
                    Extension = fileExtension,
                    Name = fileName,
                    Part = counter
                };
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(fileDescriptor));
                Send(bytes);

                counter++;
            }

            counter = 0;
        }

        public void Send(byte[] crap)
        {
        }
    }
}
