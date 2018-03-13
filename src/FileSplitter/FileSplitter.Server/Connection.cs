using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace FileSplitter.Server
{
    class Connection
    {
        private Socket sock;
        private Encoding encoding = Encoding.UTF8;
        private byte[] dataRcvBuf;

        public Connection(Socket s)
        {
            this.sock = s;
            dataRcvBuf = new byte[254];
            this.BeginReceive();
        }

        private void BeginReceive()
        {
            sock.BeginReceive(
                    dataRcvBuf, 0,
                    dataRcvBuf.Length,
                    SocketFlags.None,
                    new AsyncCallback(OnBytesReceived),
                    this);
        }
        
        protected void OnBytesReceived(IAsyncResult result)
        {
            int nBytesRec = sock.EndReceive(result);
            if (nBytesRec <= 0)
            {
                sock.Close();
                return;
            }
            string strReceived = encoding.GetString(
                dataRcvBuf, 0, nBytesRec);

            sock.BeginReceive(
                dataRcvBuf, 0,
                dataRcvBuf.Length,
                SocketFlags.None,
                new AsyncCallback(OnBytesReceived),
                this);

        }
    }
}
