using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DockerImageDiff
{
    public class MyFile
    {
        public MyFile()
        {

        }
        public MyFile(string fileName, string sha)
        {
            FileName = fileName;
            Sha256 = sha;
        }
        public string FileName { get; set; }
        public string Sha256 { get; set; }
    }
}
