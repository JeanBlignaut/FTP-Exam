using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CoreFtp;

namespace FTP_Exam_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 21);

            var sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(localEndPoint);

            var bytes = new byte[3];
            sender.Receive(bytes);

            Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytes.Length));

            //Task.Run(async () =>
            //{
            //    using (var ftpClient = new FtpClient(new FtpClientConfiguration
            //    {
            //        Host = "localhost",
            //        Username = "user",
            //        Password = "password"
            //    }))
            //    {
            //        var fileinfo = new FileInfo("C:\\test.png");
            //        await ftpClient.LoginAsync();

            //        using (var writeStream = await ftpClient.OpenFileWriteStreamAsync("test.png"))
            //        {
            //            var fileReadStream = fileinfo.OpenRead();
            //            await fileReadStream.CopyToAsync(writeStream);
            //        }
            //    }
            //});

            Console.ReadLine();
            
        }
    }
}
