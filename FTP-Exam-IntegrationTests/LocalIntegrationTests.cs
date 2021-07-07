using System;
using Xunit;
using FTP_Exam_Library;
using System.IO.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FTP_Exam_IntegrationTests
{

    public class LocalIntegrationTestFixture : IDisposable
    {
        public string projectBasePath;
        public IFileSystem fileSystem;
        public string basePath;
        public LocalFileIO fileIO;

        public LocalIntegrationTestFixture()
        {
            projectBasePath = Environment.CurrentDirectory;
            fileSystem = new FileSystem();
            basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FTP-Exam-Library-LocalIntegrationTests");
            fileIO = new LocalFileIO(fileSystem, basePath);

            var di = new DirectoryInfo(basePath);
            if (!di.Exists)
                di.Create();
        }

        public void Dispose()
        {
            
        }
    }

    public class LocalIntegrationTests : IClassFixture<LocalIntegrationTestFixture>
    {
        public string projectBasePath;
        public IFileSystem fileSystem;
        public string basePath;
        public LocalFileIO fileIO;

        public LocalIntegrationTests(LocalIntegrationTestFixture data)
        {
            projectBasePath = data.projectBasePath;
            fileSystem = data.fileSystem;
            basePath = data.basePath;
            fileIO = data.fileIO;
        }

        [Fact]
        public void ListTest()
        {
            IEnumerable<string> result;

            result = fileIO.List(basePath);

            Assert.NotEmpty(result);

            Assert.Throws<DirectoryNotFoundException>(() => fileIO.List(Path.Combine(basePath, "DoesNotExist")));

        }

        [Fact]
        public void ChangeWorkingDirectoryTest()
        {
            var subDirName = "testSub";
            var di = new DirectoryInfo(basePath);
            var sub = di.CreateSubdirectory(subDirName);

            Assert.Equal(Path.Combine(basePath, subDirName), fileIO.ChangeWorkingDirectory(subDirName));

            Assert.Equal(basePath, fileIO.ChangeWorkingDirectory(".."));

            sub.Delete();

            Assert.Throws<DirectoryNotFoundException>(() => fileIO.ChangeWorkingDirectory("DoesNotExist"));

        }

        [Fact]
        public void RetrieveTest()
        {
            var testFileName = "test.txt";
            var fi = new FileInfo(Path.Combine(basePath, testFileName));

            if (fi.Exists)
            {
                fi.Delete();
            }

            Assert.Throws<FileNotFoundException>(() => fileIO.Retrieve(testFileName));

            if (!fi.Exists)
            {
                var path = Path.Combine(projectBasePath, testFileName);
                new FileInfo(path).CopyTo(Path.Combine(basePath, testFileName));
            }

            var stream = fileIO.Retrieve(testFileName);

            Assert.True(stream.CanRead);
            Assert.Equal(29, stream.Length);

            byte[] buffer = new byte[29];
            Span<byte> spanBuffer = new Span<byte>(buffer);

            Assert.Equal(29,stream.Read(spanBuffer));

        }


        [Fact]
        public void StoreTest()
        {
            var text = "Test a new file,\n to see if it works.";
            var testFileName = "test2.txt";
            var fi = new FileInfo(Path.Combine(basePath, testFileName));

            if (fi.Exists)
            {
                fi.Delete();
            }

            Assert.False(fi.Exists);

            fileIO.Store(testFileName, Encoding.ASCII.GetBytes(text));

            fi.Refresh();

            Assert.True(fi.Exists);
        }

    }
}
