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
        private static RaftServer server;
        static void Main(string[] args)
        {
            server = new RaftServer(File.ReadAllText(@".\config.txt"), @".\Temp\");

            Run();
        }

        static void Run()
        {
            while (true)
            {
                PrintMenu();
                var line = Console.ReadLine();
                if (line == "q")
                {
                    break;
                }
                else if (line == "send")
                {
                    SendFile();
                }
                else if (line == "get")
                {

                }
                else
                {
                    Console.WriteLine("Command not supported");
                }
            }
        }

        static void SendFile()
        {
            Console.WriteLine("Write path to file");
            var path = Console.ReadLine();
            var result = server.SendFile(path);
            var message = result ? "Success" : "Failed";
            Console.WriteLine(message);
        }

        static void PrintMenu()
        {
            Console.WriteLine("Write \"q\" to exit program");
            Console.WriteLine("Write \"send\" to partition and send a file");
            Console.WriteLine("Write \"get\" to retrieve a file");
        }
    }
}
