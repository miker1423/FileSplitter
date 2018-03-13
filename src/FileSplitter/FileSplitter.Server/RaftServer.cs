using System;
using System.Text;
using System.Collections.Generic;
using Raft;
using Raft.Transport;
using System.IO;
using FileSplitter.Splitter;

namespace FileSplitter.Server
{
    public class RaftServer
    {
        private BasicSplitter splitter = new BasicSplitter();
        private TcpRaftNode raftNode;

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

        public bool SendFile(string path)
        {
            var data = File.ReadAllBytes(path);
            var splitted = splitter.Split(data.AsSpan(), 3);

            foreach (var span in splitted)
            {
                var result = Send(span);
            }

            throw new NotImplementedException();
        }

        public AddLogEntryResult Send(byte[] crap)
        {
            var result = raftNode?.AddLogEntry(crap);
            Console.WriteLine(result.AddResult.ToString());

            return result;
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
