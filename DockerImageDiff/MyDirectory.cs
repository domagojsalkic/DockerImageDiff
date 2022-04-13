using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace DockerImageDiff
{
    public class MyDirectory
    {
        public string Name { get; set; }
        public int Position { get; set; }
        public bool Modified { get; set; }
        public bool Deleted { get; set; }
        public bool Added { get; set; }
        public string AbsPath { get; set; }

        private List<MyDirectory> _directories = new List<MyDirectory>();
        private List<MyFile> _files = new List<MyFile>();

        public MyDirectory(string name)
        {
            Name = name;
            Modified = false;
            Deleted = false;
            Added = false;
        }

        public MyDirectory(MyDirectory directory)
        {
            Name = directory.Name;
            Position = directory.Position;
            Modified = directory.Modified;
            Deleted = directory.Deleted;
            Added = directory.Added;
            AbsPath = directory.AbsPath;

            foreach (var f in directory.GetFiles)
            {
                _files.Add(new MyFile(f));
            }

            foreach (var dir in directory.GetDirectories)
            {
                _directories.Add(new MyDirectory(dir));
            }
        }

        public ref List<MyDirectory> GetDirectories => ref _directories;
        public ref List<MyFile> GetFiles => ref _files;

        public override bool Equals(object obj)
        {
            if (!(obj is MyDirectory objAsMyDirectory))
                return false;

            return Equals(objAsMyDirectory);
        }

        public bool Equals(MyDirectory obj)
        {
            return obj.Name == Name;
        }

        public void Dive(string path)
        {
            AbsPath = Path.GetFullPath(path);

            foreach (var tempFile in Directory.GetFiles(path))
            {
                using (var stream = File.OpenRead(tempFile))
                {
                    var file = new MyFile(Path.GetFileName(tempFile));
                    file.AbsPath = Path.GetFullPath(path);
                    if (file.Name.Contains(".wh."))
                    {
                        file.Name = file.Name.Replace(".wh.", "");
                        file.AbsPath += "\\" + file.Name;
                        file.Deleted = true;
                    }
                    if (file.Name != ".opq")
                        _files.Add(file);
                }
            }

            foreach (var tempDir in Directory.GetDirectories(path))
            {
                var subDir = new MyDirectory(Path.GetFileName(tempDir));
                if (subDir.Name.Contains(".wh."))
                {
                    subDir.Name = subDir.Name.Replace(".wh.", "");
                    subDir.Deleted = true;
                }
                _directories.Add(subDir);
                subDir.Dive(tempDir);
            }
        }

        private void AddedDir()
        {
            foreach (var myFile in GetFiles)
            {
                myFile.Added = true;
            }

            foreach (var myDirectory in GetDirectories)
            {
                myDirectory.Added = true;
            }
        }

        public void CleanPrevDiff()
        {
            foreach (var file in GetFiles)
            {

                if (file.Deleted)
                {
                    GetFiles.Remove(file);
                }
                else
                {
                    file.Added = false;
                    file.Modified = false;
                }
            }

            foreach (var directory in GetDirectories)
            {
                if (directory.Deleted)
                {
                    GetDirectories.Remove(directory);
                }
                else
                {

                    directory.Added = false;
                    directory.Modified = false;
                    directory.CleanPrevDiff();
                }
            }
        }

        public void DiffDive(MyDirectory layer)
        {
            foreach (var file in layer.GetFiles)
            {
                MyFile tempFile;
                bool deletedDir = false;

                if (GetDirectories.Any(d => d.Name == file.Name))
                {
                    GetDirectories.Find(d => d.Name == file.Name).Delete();
                    deletedDir = true;
                }


                if (GetFiles.Contains(file))
                {
                    if (file.Deleted)
                    {
                        GetFiles.Find(f => f.Name == file.Name).Deleted = true;
                    }
                    else
                    {
                        tempFile = GetFiles.Find(f => f.Name == file.Name);
                        tempFile.Modified = true;
                    }
                }
                else
                {
                    if (deletedDir) continue;
                    tempFile = new MyFile(file.Name)
                    {
                        Added = true
                    };
                    GetFiles.Add(tempFile);
                }

            }
            GetFiles = GetFiles.OrderBy(q => q.Name).ToList();

            foreach (var directory in layer.GetDirectories)
            {
                MyDirectory tempDir;

                if (GetDirectories.Contains(directory))
                {
                    if (directory.Deleted)
                    {
                        GetDirectories.Find(p => directory.Name == p.Name).Delete();
                    }
                    else
                    {
                        tempDir = GetDirectories.Find(p => directory.Name == p.Name);
                        tempDir.Modified = true;
                        tempDir.DiffDive(directory);
                    }
                }
                else
                {
                    tempDir = new MyDirectory(directory)
                    {
                        Added = true
                    };
                    tempDir.AddedDir();
                    GetDirectories.Add(tempDir);
                }
            }
            GetDirectories = GetDirectories.OrderBy(q => q.Name).ToList();
        }

        private void Delete()
        {
            Deleted = true;

            foreach (var file in GetFiles)
            {
                file.Deleted = true;
            }

            foreach (var directory in GetDirectories)
            {
                directory.Deleted = true;
                directory.Delete();
            }
        }
    }

}
