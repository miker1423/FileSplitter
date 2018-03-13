using System;
using Raft;
using Raft.Transport;
using System.Net;

using FileSplitter.Server;
using System.IO;

namespace FileSplitter.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new RaftServer(File.ReadAllText(@".\config.txt"), @".\Temp\");


            while (true)
            {
                var line = Console.ReadLine();
                if(line == "q")
                {
                    break;
                }

                server.Send(new byte[] { 29 });
            }
        }
    }
}
