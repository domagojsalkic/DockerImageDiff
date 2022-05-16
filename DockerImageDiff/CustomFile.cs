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
    public class CustomFile : IEquatable<CustomFile>
    {
        public CustomFile(string name)
        {
            Name = name;
            FileState = FileState.None;
            AbsPath = "";
        }

        public CustomFile(CustomFile file)
        {
            Name = file.Name;
            FileState = file.FileState;
            AbsPath = file.AbsPath;
        }

        public string Name { get; set; }
        public FileState FileState { get; set; }
        public string AbsPath { get; set; }

        public bool Equals(CustomFile customFile)
        {
            if (customFile == null)
                return false;
            return customFile.Name == Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CustomFile objAsCustomFile))
                return false;

            return Equals(objAsCustomFile);
        }
    }
}