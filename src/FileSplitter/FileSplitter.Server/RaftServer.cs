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
        private List<TcpClusterEndPoint> tcpClusterEndPoints = new List<TcpClusterEndPoint>();
        private List<RaftNodeSettings> nodeSettings = new List<RaftNodeSettings>();
        private BasicSplitter splitter = new BasicSplitter();
        private TcpRaftNode raftNode;

        public RaftServer(
            TcpClusterEndPoint endpoint,
            RaftNodeSettings settings,
            string path)
        {
            if(endpoint == null || settings == null || string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("One or more parameter is null");
            }

            tcpClusterEndPoints.Add(endpoint);
            nodeSettings.Add(settings);

            raftNode = new TcpRaftNode(tcpClusterEndPoints, nodeSettings, path, splitter.WriteFile, log: new Logger());

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
            => raftNode?.AddLogEntry(crap);
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
