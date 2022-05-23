using System.Diagnostics;
using System.IO;
using DockerImageDiff.ExtractTarGz;

namespace DockerImageDiff
{
    public class ExtractFiles
    {
        public static void ExtractFile(string path)
        {
            var extension = Path.GetExtension(path);
            var fileName = Path.GetFileName(path);
            var currentPath = Path.GetFullPath(path);
            currentPath = System.IO.Directory.GetParent(currentPath)?.FullName;

            switch (extension.ToLower())
            {
                case ".gz":
                    Tar.ExtractTarGz(path, currentPath);
                    break;
                case ".tar":
                    Tar.ExtractTar(path, currentPath);
                    break;
                default:
                    Debug.Assert(false, "Something went wrong.");
                    break;
            }
        }

        public static void DeleteExtractedFiles()
        {
            Tar.RemoveExtractedData();
        }
    }
}