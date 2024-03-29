﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public enum FolderState
{
    None = 0,
    Added = 1,
    Modified = 2,
    Deleted = 3
}

namespace DockerImageDiff
{
    public class Directory : IEquatable<Directory>
    {
        private List<Directory> _directories = new List<Directory>();
        private List<File> _files = new List<File>();

        public Directory(string name)
        {
            Name = name;
            FolderState = FolderState.None;
        }

        public Directory(Directory directory)
        {
            Name = directory.Name;
            Position = directory.Position;
            FolderState = directory.FolderState;
            AbsPath = directory.AbsPath;

            foreach (var f in directory.GetFiles) _files.Add(new File(f));

            foreach (var dir in directory.GetDirectories) _directories.Add(new Directory(dir));
        }

        public string Name { get; set; }
        public int Position { get; set; }
        public FolderState FolderState { get; set; }
        public string AbsPath { get; set; }

        public ref List<Directory> GetDirectories => ref _directories;
        public ref List<File> GetFiles => ref _files;

        public bool Equals(Directory obj)
        {
            if (obj == null)
                return false;
            return obj.Name == Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Directory objAsCustomDirectory))
                return false;

            return Equals(objAsCustomDirectory);
        }

        public void Dive(string path)
        {
            AbsPath = Path.GetFullPath(path);

            foreach (var tempFile in System.IO.Directory.GetFiles(path))
                using (var stream = System.IO.File.OpenRead(tempFile))
                {
                    var file = new File(Path.GetFileName(tempFile))
                    {
                        AbsPath = Path.GetFullPath(path)
                    };
                    if (file.Name.Contains(".wh."))
                    {
                        file.Name = file.Name.Replace(".wh.", "");
                        file.AbsPath += "\\" + file.Name;
                        file.FileState = FileState.Deleted;
                    }

                    if (file.Name != ".opq")
                        _files.Add(file);
                }

            foreach (var tempDir in System.IO.Directory.GetDirectories(path))
            {
                var subDir = new Directory(Path.GetFileName(tempDir));
                if (subDir.Name.Contains(".wh."))
                {
                    subDir.Name = subDir.Name.Replace(".wh.", "");
                    subDir.FolderState = FolderState.Deleted;
                }

                _directories.Add(subDir);
                subDir.Dive(tempDir);
            }
        }

        private void AddedDir()
        {
            foreach (var myFile in GetFiles) myFile.FileState = FileState.Added;

            foreach (var myDirectory in GetDirectories)
            {
                myDirectory.FolderState = FolderState.Added;
                myDirectory.AddedDir();
            }
        }

        public void CleanPrevDiff()
        {
            foreach (var file in GetFiles)
                if (file.FileState == FileState.Deleted)
                    GetFiles.Remove(file);
                else
                    file.FileState = FileState.None;

            foreach (var directory in GetDirectories)
                if (directory.FolderState == FolderState.Deleted)
                {
                    GetDirectories.Remove(directory);
                }
                else
                {
                    directory.FolderState = FolderState.None;
                    directory.CleanPrevDiff();
                }
        }

        public void DiffDive(Directory layer)
        {
            foreach (var file in layer.GetFiles)
            {
                File tempFile;
                var deletedDir = false;

                if (GetDirectories.Any(d => d.Name == file.Name))
                {
                    GetDirectories.Find(d => d.Name == file.Name).Delete();
                    deletedDir = true;
                }


                if (GetFiles.Contains(file))
                {
                    if (file.FileState == FileState.Deleted)
                    {
                        GetFiles.Find(f => f.Name == file.Name).FileState = FileState.Deleted;
                    }
                    else
                    {
                        tempFile = GetFiles.Find(f => f.Name == file.Name);
                        tempFile.FileState = FileState.Modified;
                    }
                }
                else
                {
                    if (deletedDir) continue;

                    tempFile = new File(file.Name)
                    {
                        FileState = FileState.Added
                    };

                    GetFiles.Add(tempFile);
                }
            }

            GetFiles = GetFiles.OrderBy(q => q.Name).ToList();

            foreach (var directory in layer.GetDirectories)
            {
                Directory tempDir;

                if (GetDirectories.Contains(directory))
                {
                    if (directory.FolderState == FolderState.Deleted)
                    {
                        GetDirectories.Find(p => directory.Name == p.Name).Delete();
                    }
                    else
                    {
                        tempDir = GetDirectories.Find(p => directory.Name == p.Name);
                        tempDir.FolderState = FolderState.Modified;
                        tempDir.DiffDive(directory);
                    }
                }
                else
                {
                    tempDir = new Directory(directory)
                    {
                        FolderState = FolderState.Added
                    };
                    tempDir.AddedDir();
                    GetDirectories.Add(tempDir);
                }
            }

            GetDirectories = GetDirectories.OrderBy(q => q.Name).ToList();
        }

        private void Delete()
        {
            FolderState = FolderState.Deleted;

            foreach (var file in GetFiles) file.FileState = FileState.Deleted;

            foreach (var directory in GetDirectories)
            {
                directory.FolderState = FolderState.Deleted;
                directory.Delete();
            }
        }
    }
}