using System;
using System.Text;
using System.Collections.Generic;
using Raft;
using Raft.Transport;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

using FileSplitter.Splitter;

namespace FileSplitter.Server
{
    public class RaftServer
    {
        private BasicSplitter splitter = new BasicSplitter();
        private TcpRaftNode raftNode;
        private int counter = 0;

        public RaftServer(
            string configPath,
            string path)
        {
            if(string.IsNullOrEmpty(configPath) || string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("One or more parameter is null");
            }

            raftNode = TcpRaftNode.GetFromConfig(1, configPath, path, 4250, new Logger(), splitter.WriteFile);

            raftNode.Start();
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
            var result = raftNode?.AddLogEntry(crap);
            Console.WriteLine(result.AddResult.ToString());
        }
    }

    class Logger : IWarningLog
    {
        private StringBuilder stringBuilder = new StringBuilder();
        public void Log(WarningLogEntry logEntry)
        {
            stringBuilder.Append($"{logEntry.LogType}: {logEntry.Description} at {logEntry.DateTime}");
            if (logEntry.Exception != null)
            {
                stringBuilder.Append($" with exception: {logEntry.Exception}");
            }
            
            Console.WriteLine(stringBuilder.ToString());
            stringBuilder.Clear();
        }
    }
}
