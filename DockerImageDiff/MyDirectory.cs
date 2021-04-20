using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DockerImageDiff
{
    public class MyDirectory
    {
        public MyDirectory(string directoryName)
        {
            DirectoryName = directoryName;
        }

        public string DirectoryName { get; set; }
        public List<MyDirectory> Directories = new List<MyDirectory>();
        public List<MyFile> Files = new List<MyFile>();

        public void Dive(string path)
        {
            foreach (var tempDir in Directory.GetDirectories(path))
            {

                MyDirectory subDir = new MyDirectory(Path.GetFileName(tempDir));
                Directories.Add(subDir);
                subDir.Dive(tempDir);
            }
            foreach (var tempFile in Directory.GetFiles(path))
            {
                using (FileStream stream = File.OpenRead(tempFile))
                {
                    var sha = new SHA256Managed();
                    byte[] checksum = sha.ComputeHash(stream);
                    MyFile file = new MyFile(Path.GetFileName(tempFile), BitConverter.ToString(checksum).Replace("-", String.Empty));
                    Files.Add(file);
                }
            }
        }
    }
}
