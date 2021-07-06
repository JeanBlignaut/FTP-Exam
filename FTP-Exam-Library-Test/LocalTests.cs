using System;
using Xunit;
using FTP_Exam_Library;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Collections.Generic;

namespace FTP_Exam_IntegrationTests
{
    using XFS = MockUnixSupport;
    public class LocalTests
    {
        [Fact]
        public void ListTest()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\myfile.txt", new MockFileData("Testing is meh.") },
                { @"c:\demo\jQuery.js", new MockFileData("some js") },
                { @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
            });

            var fileIO = new LocalFileIO(fileSystem);
            string path = XFS.Path(@"c:\");
            var result = fileIO.List(path);
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
