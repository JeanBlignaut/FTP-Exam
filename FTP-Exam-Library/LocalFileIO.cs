using System;
using System.Linq;
using System.IO.Abstractions;
using System.IO;
using System.Collections.Generic;

namespace FTP_Exam_Library
{
    public class LocalFileIO
    {

        private IFileSystem fileSystem { get; init; }

        public String CurrentWorkingDirectory { get; private set; }

        public LocalFileIO(IFileSystem filesystem)
        {
            fileSystem = filesystem;
        }

        public IEnumerable<string> List(string? path = null)
        {
            // Not abstracted properly
            //IDirectoryInfo dirInfo = (DirectoryInfoBase)new DirectoryInfo(path);
            //var dirs = dirInfo.EnumerateDirectories("*");
            //var files = dirInfo.EnumerateFiles("*");

            //return dirs.Select(d => d.Name)
            //    .Union(files.Select(f => f.Name));
        }

        public static void ChangeWorkingDirectory()
        {
            throw new NotImplementedException();
        }

        public static void Retrieve()
        {
            IFileInfo fi = (FileInfoBase)new FileInfo("test.txt");
            //return fi;
            throw new NotImplementedException();
        }

        public static void Store()
        {
            IFileInfo fi = (FileInfoBase)new FileInfo("test.txt");
            //return fi;
            throw new NotImplementedException();
        }
    }
}