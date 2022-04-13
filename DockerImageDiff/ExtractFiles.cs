using System.Diagnostics;
using System.IO;

namespace DockerImageDiff
{
    public class ExtractFiles
    {
        public static void ExtractFile(string path)
        {
            var extension = Path.GetExtension(path);
            var fileName = Path.GetFileName(path);
            var currentPath = Path.GetFullPath(path);
            currentPath = Directory.GetParent(currentPath)?.FullName;

            switch (extension.ToLower())
            {
                case ".gz":
                    ExtractTarGz.Tar.ExtractTarGz(path, currentPath);
                    break;
                case ".tar":
                     ExtractTarGz.Tar.ExtractTar(path, currentPath);
                    break;
                default:
                    Debug.Assert(false, "Something went wrong.");
                    break;
            }
        }
        public static void DeleteExtractedFiles()
        {
            ExtractTarGz.Tar.RemoveExtractedData();
        }
    }
}
