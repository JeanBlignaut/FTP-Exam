using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FTP_Exam_Library;

namespace FTP_Exam_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            //Get IP from dns lookup
            //IPHostEntry host = Dns.GetHostEntry("localhost");
            //IPAddress ipAddress = host.AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            // Bind to all ipAdresses
            IPAddress ipAddress = new IPAddress(new byte[4] { 0, 0, 0, 0 });
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 21);

            int dataPort = 2020; // this is meant to be a range of ports
            string baseFsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FTP-Exam-Library-LocalIntegrationTests");

            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);

            listener.Listen(10);

            Console.WriteLine($"Bound to {listener.LocalEndPoint}");

            var handler = listener.Accept();

            Console.WriteLine($"Connection received from {handler.RemoteEndPoint}");

            RemoteFileIOServerSide remoteFileIOServerSide = new RemoteFileIOServerSide(handler, new LocalFileIO(baseFsPath), ipAddress, dataPort);

            // Incoming data from the client.    
            string data = string.Empty;
            byte[] bytes = new byte[1024];

            //handler.Send(System.Text.Encoding.ASCII.GetBytes($"{StatusCodes.SendUserCommand} Hello old chap"));
            remoteFileIOServerSide.clientSendHelper(StatusCodes.SendUserCommand, $"Hello {handler.RemoteEndPoint} this is {handler.LocalEndPoint}");

            while (true)
            {
                data = string.Empty;

                while (handler.Receive(bytes) > 0 || data.IndexOf("<EOL>") > -1 || data.IndexOf("<EOF>") > -1)
                {
                    data += Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                }

                Console.WriteLine($"Received: {data}");

                switch (Enum.Parse<MinimalCommands>(data.Substring(0, 4).Trim()))
                {
                    case MinimalCommands.USER:
                        remoteFileIOServerSide.clientSendHelper(StatusCodes.AccountNeeded, "Only anonamous implemented");
                        break;
                    case MinimalCommands.LIST:
                        remoteFileIOServerSide.LIST(data.Substring(4).Trim());
                        break;
                    case MinimalCommands.STOR:
                        remoteFileIOServerSide.STOR(data.Substring(4).Trim());
                        break;
                    case MinimalCommands.RETR:
                        remoteFileIOServerSide.RETR(data.Substring(4).Trim());
                        break;
                    case MinimalCommands.CWD:
                        remoteFileIOServerSide.CWD(data.Substring(4).Trim());
                        break;
                    default :
                        remoteFileIOServerSide.clientSendHelper(StatusCodes.CommandNotImplemented, "Command not implemented");
                        break;
                }

                if (data.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }

            Console.WriteLine("Text received : {0}", data);

            byte[] msg = Encoding.ASCII.GetBytes(data);
            handler.Send(msg);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        async Task AcceptClientsAsync(Socket listener, CancellationToken ct)
        {
            var clientCounter = 0;
            while (!ct.IsCancellationRequested)
            {
                var client = await listener.AcceptAsync()
                                                    .ConfigureAwait(false);
                clientCounter++;
                EchoAsync(client, clientCounter, ct);
            }
        }

        async Task EchoAsync(Socket clientConnection,
                     int clientIndex,
                     CancellationToken ct)
        {
            Console.WriteLine("New client ({0}) connected", clientIndex);
            //using (client)
            //{
            //    var s = new ArraySegment<byte>();
            //    var buf = new byte[4096];
            //    var stream = client.ReceiveAsync(s, SocketFlags.None); //.GetStream();
            //    while (!ct.IsCancellationRequested)
            //    {
            //        var timeoutTask = Task.Delay(TimeSpan.FromSeconds(15));
            //        var amountReadTask = stream.ReadAsync(buf, 0, buf.Length, ct);
            //        var completedTask = await Task.WhenAny(timeoutTask, amountReadTask)
            //                                      .ConfigureAwait(false);
            //        if (completedTask == timeoutTask)
            //        {
            //            var msg = Encoding.ASCII.GetBytes("Client timed out");
            //            await stream.WriteAsync(msg, 0, msg.Length);
            //            break;
            //        }
            //        var amountRead = amountReadTask.Result;
            //        if (amountRead == 0) break; //end of stream.
            //        await stream.WriteAsync(buf, 0, amountRead, ct)
            //                    .ConfigureAwait(false);
            //    }
            //}
            Console.WriteLine("Client ({0}) disconnected", clientIndex);
        }
    }
}
