using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace FileSplitter.Server
{
    class Connection
    {
        private Socket _socket;
        private Encoding encoding = Encoding.UTF8;
        private byte[] dataRcvBuf;

        public Connection(Socket socket)
        {
            _socket = socket;
            dataRcvBuf = new byte[254];
            BeginReceive();
        }

        private void BeginReceive()
        {
            _socket.BeginReceive(
                    dataRcvBuf, 0,
                    dataRcvBuf.Length,
                    SocketFlags.None,
                    new AsyncCallback(OnBytesReceived),
                    this);
        }
        
        protected void OnBytesReceived(IAsyncResult result)
        {
            int nBytesRec = _socket.EndReceive(result);
            if (nBytesRec <= 0)
            {
                _socket.Close();
                return;
            }
            string strReceived = encoding.GetString(
                dataRcvBuf, 0, nBytesRec);

            Console.WriteLine(strReceived);

            _socket.BeginReceive(
                dataRcvBuf, 0,
                dataRcvBuf.Length,
                SocketFlags.None,
                new AsyncCallback(OnBytesReceived),
                this);

        }
    }
}
