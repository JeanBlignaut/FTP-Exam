using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FTP_Exam_Library
{
    public class FTPControlStream : Stream
    {

        protected Stream SocketStream;
        protected Socket Socket;

        public FTPControlStream()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 21);

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(localEndPoint);

            Socket.Listen(10);

            Socket.Accept();
        }

        private bool IsConnected => Socket?.IsBound ?? false && CanRead && CanWrite;

        public override bool CanRead => SocketStream?.CanRead ?? false;

        public override bool CanSeek => false;

        public override bool CanWrite => SocketStream?.CanWrite ?? false;

        public override long Length => SocketStream?.Length ?? 0;

        public override long Position { get => SocketStream?.Position ?? 0; set => throw new InvalidOperationException(); }

        public override void Flush()
        {
            if (IsConnected)
            {
                SocketStream?.Flush();
            }
            throw new InvalidOperationException("The ControlSocketStream is not connected.");
        }

        public override int Read(byte[] buffer, int offset, int count) => SocketStream?.Read(buffer, offset, count) ?? 0;
        

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException();
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException();
        }

        public override void Write(byte[] buffer, int offset, int count) => SocketStream?.Write(buffer, offset, count);
    }
}
