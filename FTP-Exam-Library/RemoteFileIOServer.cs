using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FTP_Exam_Library
{
    public class RemoteFileIOServer
    {
        private Socket client;
        private LocalFileIO fileIO;
        private int _dataPort;
        private IPAddress _ipAddress;
        private Socket clientData;

        public RemoteFileIOServer(Socket connectedClient, LocalFileIO localFileIO, IPAddress ipAddress, int dataPort)
        {
            client = connectedClient;
            fileIO = localFileIO;
            _ipAddress = ipAddress;
            _dataPort = dataPort;
        }

        private void clientSendHelper(StatusCodes statusCode, string message)
        {
            var response = Encoding.ASCII.GetBytes($"{statusCode} {message.Trim()}\n");
            client.Send(response);
        }

        private void clientDataOpenHelper()
        {
            try
            {
                var clientData = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientData.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                var dataEndPoint = new IPEndPoint(_ipAddress, _dataPort);
                clientData.Bind(dataEndPoint);
                clientData.Listen(5);

                Console.WriteLine($"Data socket bound and listening on {_ipAddress}:{_dataPort}");
                clientSendHelper(StatusCodes.DataAlreadyOpen, "Data connection open and listening");
            }
            catch(SocketException sex)
            {
                clientDataCloseHelper();
                clientSendHelper(StatusCodes.CantOpenData, $"Can't bind data socket on {_ipAddress}:{_dataPort} Error: {sex.Message}");
            }
        }

        private void clientDataCloseHelper()
        {
            try
            {
                clientData?.Close();
            }
            catch
            {
                Console.WriteLine("DataSocket doesn't exist or already closed");
            }
        }

        public void USER()
        {
            throw new NotImplementedException();
        }

        public void QUIT()
        {
            try
            {
                clientSendHelper(StatusCodes.ClosingControl,"Terminating Server...");
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

        public void RETR(string? path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(Path.GetFileName(path)))
            {
                clientSendHelper(StatusCodes.ArgumentSyntaxError, "invalid filename argument.");
                return;
            }

            try
            {
                using var stream = fileIO.Retrieve(path);

                clientDataOpenHelper();

                byte[] buffer = new byte[1024];
                int offset = 0;
                while (offset < stream.Length)
                {
                    stream.Read(buffer, offset, 1024);
                    clientData.Send(buffer);
                    offset += 1024;
                }
                clientSendHelper(StatusCodes.ClosingData, "File transfer complete.");
            }
            catch (FileNotFoundException fnfex)
            {
                clientSendHelper(StatusCodes.ActionNotTakenFileUnavailable, fnfex.Message);
            }
            catch (IOException ioex)
            {
                Console.WriteLine(ioex.Message);
                throw ioex;
            }
            finally
            {
                clientDataCloseHelper();
            }
        }

        public void STOR(string path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(Path.GetFileName(path)))
            {
                clientSendHelper(StatusCodes.ArgumentSyntaxError, "invalid filename argument.");
                return;
            }

            Stream? fileStream = null;

            try
            {

                byte[] data = new byte[1024];

                clientDataOpenHelper();

                fileStream = fileIO.Store(path);

                while (clientData.Receive(data) > 0)
                {
                    fileStream?.Write(data);
                }
               
                clientSendHelper(StatusCodes.ClosingData, "File transfer complete.");
            }
            catch (System.IO.IOException ioex)
            {
                clientSendHelper(StatusCodes.ActionNotTakenFileUnavailable, ioex.Message);
            }
            finally
            {
                clientDataCloseHelper();
                fileStream?.Close();
            }
        }

        public void CWD(string path)
        {
            try
            {
                var cwd = fileIO.ChangeWorkingDirectory(path);
                clientSendHelper(StatusCodes.FileActionOK, $"current working directory changed to: {cwd}");
            }
            catch(DirectoryNotFoundException dnfe)
            {
                Console.WriteLine(dnfe.Message);
                clientSendHelper(StatusCodes.ActionNotTakenFileUnavailable, dnfe.Message);
            }
        }

        public void LIST(string path)
        {
            throw new NotImplementedException();
        }
    }
}
