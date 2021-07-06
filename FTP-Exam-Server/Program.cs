using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Threading;
using FubarDev.FtpServer;
using FubarDev.FtpServer.FileSystem.DotNet;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace FTP_Exam_Server
{
    class Program
    {
        static void Main(string[] args)
        {

            var cts = new CancellationTokenSource();
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 21);

            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);

            listener.Listen(10);

            var handler = listener.Accept();

            // Incoming data from the client.    
            string data = null;
            byte[] bytes = null;


            handler.Send(System.Text.Encoding.ASCII.GetBytes("220 Hello old chap"));

            while (true)
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
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

            //// Setup dependency injection
            //var services = new ServiceCollection();

            //// use %TEMP%/TestFtpServer as root folder
            //services.Configure<DotNetFileSystemOptions>(opt => opt
            //    .RootPath = Path.Combine(Path.GetTempPath(), "TestFtpServer"));

            //// Add FTP server services
            //// DotNetFileSystemProvider = Use the .NET file system functionality
            //// AnonymousMembershipProvider = allow only anonymous logins
            //services.AddFtpServer(builder => builder
            //    //.UseDotNetFileSystem() // Use the .NET file system functionality
            //    .EnablePamAuthentication()
            //    .UsePamUserHome()
            //    .UseUnixFileSystem());
            //    //.EnableAnonymousAuthentication()); // allow anonymous logins

            //// Configure the FTP server
            //services.Configure<FtpServerOptions>(opt =>
            //{
            //    opt.ServerAddress = "127.0.0.1";
            //    //opt.Port = 2021;
            //});

            //// Build the service provider
            //using (var serviceProvider = services.BuildServiceProvider())
            //{
            //    // Initialize the FTP server
            //    var ftpServerHost = serviceProvider.GetRequiredService<IFtpServerHost>();

            //    // Start the FTP server
            //    ftpServerHost.StartAsync(CancellationToken.None).Wait();

            //    Console.WriteLine("Press ENTER/RETURN to close the test application.");
            //    Console.ReadLine();

            //    // Stop the FTP server
            //    ftpServerHost.StopAsync(CancellationToken.None).Wait();
            //}
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
            using (client)
            {
                var s = new ArraySegment<byte>();
                var buf = new byte[4096];
                var stream = client.ReceiveAsync(s, SocketFlags.None); //.GetStream();
                while (!ct.IsCancellationRequested)
                {
                    var timeoutTask = Task.Delay(TimeSpan.FromSeconds(15));
                    var amountReadTask = stream.ReadAsync(buf, 0, buf.Length, ct);
                    var completedTask = await Task.WhenAny(timeoutTask, amountReadTask)
                                                  .ConfigureAwait(false);
                    if (completedTask == timeoutTask)
                    {
                        var msg = Encoding.ASCII.GetBytes("Client timed out");
                        await stream.WriteAsync(msg, 0, msg.Length);
                        break;
                    }
                    var amountRead = amountReadTask.Result;
                    if (amountRead == 0) break; //end of stream.
                    await stream.WriteAsync(buf, 0, amountRead, ct)
                                .ConfigureAwait(false);
                }
            }
            Console.WriteLine("Client ({0}) disconnected", clientIndex);
        }
    }
}
