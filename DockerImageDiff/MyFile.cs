using System.Security.Cryptography;

namespace DockerImageDiff
{
    public class MyFile
    {
        public MyFile(string name)
        {
            Name = name;
        }  
        public MyFile(MyFile file)
        {
            Name = file.Name;
            Modified= file.Modified;
            Deleted = file.Deleted;
            Added = file.Added;
            AbsPath = file.AbsPath;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MyFile objAsMyDirectory))
                return false;

            return Equals(objAsMyDirectory);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public bool Equals(MyFile myFile)
        {
            return myFile.Name == Name;
        }

        public string Name { get; set; }
        public bool Modified { get; set; }
        public bool Deleted { get; set; }
        public bool Added { get; set; }
        public string AbsPath { get; set; }
    }
}
