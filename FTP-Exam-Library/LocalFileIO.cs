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

        private IDirectoryInfo _BaseDirectoryInfo { get; set; }
        public string BaseDirectory => _BaseDirectoryInfo.FullName;

        private IDirectoryInfo _CWDirectoryInfo { get; set; }
        public string CurrentWorkingDirectory => _CWDirectoryInfo.FullName;

        public LocalFileIO(IFileSystem filesystem, string homeDirectoryPath)
        {
            fileSystem = filesystem;
            _BaseDirectoryInfo = (DirectoryInfoBase) new DirectoryInfo(homeDirectoryPath);
            _CWDirectoryInfo = _BaseDirectoryInfo;
        }

        private IEnumerable<string> List(IDirectoryInfo di)
        {
            var dirs = di.EnumerateDirectories("*");
            var files = di.EnumerateFiles("*");

            return dirs.Select(d => d.Name)
                .Union(files.Select(f => f.Name));
        }

        public IEnumerable<string> List()
        {
            return List(_CWDirectoryInfo);
        }

        public IEnumerable<string> List(string path)
        {
            var fullPath = Path.Combine(CurrentWorkingDirectory, path);
            IDirectoryInfo di = (DirectoryInfoBase) new DirectoryInfo(fullPath);

            return List(di);
        }

        public string ChangeWorkingDirectory(string path)
        {
            Path.GetFullPath(_CWDirectoryInfo.FullName);
            var result = Path.Combine(_CWDirectoryInfo.FullName, path);
            return result;
        }

        public Stream Retrieve(string path)
        {
            IFileInfo fi = (FileInfoBase)new FileInfo("path");
            //if (fi.Exists)
            //{
                return fi.OpenRead();
            //}
        }

        public void Store()
        {
            IFileInfo fi = (FileInfoBase)new FileInfo("test.txt");
            //return fi;
            throw new NotImplementedException();
        }
    }
}