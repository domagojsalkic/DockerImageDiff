using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerImageDiff
{
    public class ExtractFiles
    {
        public static void ExtractFile(string path)
        {
            var extension = Path.GetExtension(path);
            var fileName = Path.GetFileName(path);
            var currentPath = Path.GetFullPath(path);
            currentPath = Directory.GetParent(currentPath).FullName;

            switch (extension.ToLower())
            {
                case ".gz":
                    DockerImageDiff.ExtractTarGz.Tar.ExtractTarGz(path, currentPath);
                    break;
                case ".tar":
                    DockerImageDiff.ExtractTarGz.Tar.ExtractTar(path, currentPath);
                    break;
                default:
                    Debug.Assert(false, "Something went wrong.");
                    break;
            }
        }
    }
}
