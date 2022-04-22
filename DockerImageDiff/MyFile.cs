using System;

public enum FileState
{
    None = 0,
    Added = 1,
    Modified = 2,
    Deleted = 3
}

namespace DockerImageDiff
{
    public class MyFile : IEquatable<MyFile>
    {
        public string Name { get; set; }
        public FileState FileState { get; set; }
        public string AbsPath { get; set; }
        public MyFile(string name)
        {
            Name = name;
            FileState = FileState.None;
            AbsPath = "";
        }
        public MyFile(MyFile file)
        {
            Name = file.Name;
            FileState = file.FileState;
            AbsPath = file.AbsPath;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MyFile objAsMyDirectory))
                return false;

            return Equals(objAsMyDirectory);
        }

        public bool Equals(MyFile myFile)
        {
            if (myFile == null)
                return false;
            return myFile.Name == Name;
        }
    }
}
