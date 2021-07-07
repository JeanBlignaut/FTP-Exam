using System;
using Xunit;
using FTP_Exam_Library;
using System.IO.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FTP_Exam_IntegrationTests
{
    public class LocalIntegrationTests
    {
        [Fact]
        public void ListTest()
        {
            // Arrange
            IFileSystem fileSystem = new FileSystem();

            //string path = Path.GetTempPath();

            var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FTP-Exam-Library-LocalIntegrationTests");

            var fileIO = new LocalFileIO(fileSystem, basePath);

            IEnumerable<string> result;

            try
            {
                result = fileIO.List(basePath);
            }
            catch (DirectoryNotFoundException dnfe)
            {
                var di = new DirectoryInfo(basePath);
                di.Create();
                result = fileIO.List(basePath);
            }

            Assert.Empty(result);

            Assert.Throws<DirectoryNotFoundException>(() => fileIO.List(Path.Combine(basePath, "DoesNotExist")));

        }

        [Fact]
        public void ChangeWorkingDirectoryTest()
        {
            //LocalFileIO.ChangeWorkingDirectory();
            throw new NotImplementedException();
        }

        [Fact]
        public void RetrieveTest()
        {
            //LocalFileIO.Retrieve();
            throw new NotImplementedException();
        }


        [Fact]
        public void StoreTest()
        {
            //LocalFileIO.Store();
            throw new NotImplementedException();
        }

    }
}
