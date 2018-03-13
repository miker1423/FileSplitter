using System;
using System.Net;

using FileSplitter.Server;
using System.IO;

namespace FileSplitter.Runner
{
    class Program
    {
        private static TcpServer server;
        private static Client client;
        static void Main(string[] args)
        {
            Console.WriteLine("Start as (s)erver or (c)lient");
            var startState = Console.ReadLine();
            if(startState == "s")
            {
                server = new TcpServer();

                RunServer();
            }
            else if(startState == "c")
            {
                Console.WriteLine("IP server");
                var ipAddress = Console.ReadLine();
                client = new Client(ipAddress, 25000);
            }
        }

        static void RunServer()
        {
            while (true)
            {
                PrintServerMenu();
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

        static void RunClient()
        {
            while (true)
            {
                PrintClientMenu();
                var line = Console.ReadLine();
                if(line == "send")
                {
                    SendString();
                }
                else
                {
                    Console.WriteLine("Command not supported");
                }
            }
        }

        static void SendString()
        {
            Console.WriteLine("Write string to send");
            var line = Console.ReadLine();
            client.SendString(line);
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

        static void PrintClientMenu()
        {
            Console.WriteLine("Write \"send\" to send something");
        }

        static void PrintServerMenu()
        {
            Console.WriteLine("Write \"q\" to exit program");
            Console.WriteLine("Write \"send\" to partition and send a file");
            Console.WriteLine("Write \"get\" to retrieve a file");
        }
    }
}
