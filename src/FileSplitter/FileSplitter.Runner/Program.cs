using System;
using Raft;
using Raft.Transport;
using System.Net;

using FileSplitter.Server;

namespace FileSplitter.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var endpoint = new TcpClusterEndPoint()
            {
                Host = IPAddress.Any.ToString(),
                Port = 4500
            };

            var setting = new RaftNodeSettings();

            var server = new RaftServer(endpoint, setting, @".\Temp\");

            Console.ReadLine();
        }
    }
}
