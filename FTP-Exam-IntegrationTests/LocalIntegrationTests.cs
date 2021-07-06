using System;
using Xunit;
using FTP_Exam_Library;
using System.IO.Abstractions;
using System.Collections.Generic;
using System.IO;

namespace FTP_Exam_IntegrationTests
{
    public class LocalIntegrationTests
    {
        [Fact]
        public void ListTest()
        {
            // Arrange
            IFileSystem fileSystem = new FileSystem();
            string path = Path.GetTempPath();
            var fileIO = new LocalFileIO(fileSystem);

            var curdir = Environment.CurrentDirectory;
            var tmppath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            try
            {
                var result = fileIO.List(path);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        [Fact]
        public void ChangeWorkingDirectoryTest()
        {
            LocalFileIO.ChangeWorkingDirectory();
        }

        [Fact]
        public void RetrieveTest()
        {
            LocalFileIO.Retrieve();
        }


        [Fact]
        public void StoreTest()
        {
            LocalFileIO.Store();
        }

    }
}
