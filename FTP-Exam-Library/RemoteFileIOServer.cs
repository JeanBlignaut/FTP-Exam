using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace FTP_Exam_Library
{
    public class RemoteFileIOServer
    {
        private Socket client;

        public RemoteFileIOServer(Socket connectedClient)
        {
            client = connectedClient;
        }

        public void USER()
        {
            throw new NotImplementedException();
        }

        public void QUIT()
        {
            try
            {
                var response = $"{StatusCodes.ClosingControl}";
                var buffer = Encoding.ASCII.GetBytes(response);
                client.Send(buffer);
            }
            finally
            {
                Console.WriteLine($"Closing connection from {client.RemoteEndPoint}...");
                client.Close();
            }
        }

        public void PORT()
        {
            throw new NotImplementedException();
        }

        public void TYPE()
        {
            throw new NotImplementedException();
        }

        public void MODE()
        {
            throw new NotImplementedException();
        }

        public void STRU()
        {
            throw new NotImplementedException();
        }

        public void RETR()
        {
            throw new NotImplementedException();
        }

        public void STOR()
        {
            throw new NotImplementedException();
        }

        public void CWD()
        {
            throw new NotImplementedException();
        }

        public void LIST(string path)
        {
            throw new NotImplementedException();
        }
    }
}
