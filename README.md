# FTP-Exam

Incomplete implementation:

* Client incomplete
* Server Incomplete
* Unit tests Incomplete

Uses DotNet 5.0 SDK

development done on both Macos and Windows 10 using visual studio 2019 and visualstudio for mac

cd into server sub dir and execure:
dotnet run

or on macos:
sudo dotnet run

cd into integration tests folder and run:
dotnet test

I encountered extensive cognitive disonance with this assignment.
While I decided to use C# as I'm vastly more comfortable with the language and development tools, 
 the formality of the code gave me great dificuilty when it came to implementing a naive FTP server / client.
Ultimately this was a mistake and should rather have attempted this in python as it would have granted a more informal paradyme.

### Sources (not an exhaustive list):
* https://github.com/sparkeh9/CoreFTP
* https://github.com/robinrodricks/FluentFTP
* https://github.com/taoyouh/FtpServer
* https://github.com/FubarDevelopment/FtpServer
* https://www.w3.org/Protocols/rfc959/5_Declarative.html
* https://www.ncftp.com/libncftp/doc/ftp_overview.html
* https://www.c-sharpcorner.com/article/socket-programming-in-C-Sharp/
* https://www.winsocketdotnetworkprogramming.com/clientserversocketnetworkcommunication8d.html
* https://stackoverflow.com/questions/12630827/using-net-4-5-async-feature-for-socket-programming
* https://www.c-sharpcorner.com/article/c-sharp-arraysegment/
* https://datatracker.ietf.org/doc/html/rfc959
* https://zetcode.com/csharp/listdirectory/
* https://developers.redhat.com/blog/2018/11/07/dotnet-special-folder-api-linux#environment_getfolderpath
* https://github.com/alvath96/simple-ftp/blob/master/ftpserver.py
* https://stackoverflow.com/questions/12976319/xunit-net-global-setup-teardown

