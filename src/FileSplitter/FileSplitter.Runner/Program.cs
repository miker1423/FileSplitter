using System;
using System.Net;

using FileSplitter.Server;
using System.IO;

namespace FileSplitter.Runner
{
    class Program
    {
        private static TcpServer server;
        static void Main(string[] args)
        {
            server = new TcpServer();

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
                    GetFile();
                }
                else
                {
                    Console.WriteLine("Command not supported");
                }
            }
        }

        static void GetFile()
        {
            Console.WriteLine("Write the file name");
            var name = Console.ReadLine();
            server.GetFile(name);
        }

        static void SendFile()
        {
            Console.WriteLine("Write path to file");
            var path = Console.ReadLine();
            server.SendFile(path);
        }

        static void PrintMenu()
        {
            Console.WriteLine("Write \"q\" to exit program");
            Console.WriteLine("Write \"send\" to partition and send a file");
            Console.WriteLine("Write \"get\" to retrieve a file");
        }
    }
}
