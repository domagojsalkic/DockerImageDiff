using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DockerImageDiff
{
    public enum Icon
    {
        Folder = 0,
        FolderAdded,
        FolderModified,
        FolderDeleted,
        File,
        FileAdded,
        FileModified,
        FileDeleted
    }

    public partial class DockerImageCompare : Form
    {
        private readonly List<CustomDirectory> _diffLayers = new List<CustomDirectory>();

        private List<CustomDirectory> _layers = new List<CustomDirectory>();

        public DockerImageCompare()
        {
            InitializeComponent();
        }

        private void selectImage1Button_Click(object sender, EventArgs e)
        {
            fileDialog = new OpenFileDialog();
            fileDialog.Filter = @"TAR (*.tar)|*.tar;|All files (*.*)|*.*";
            fileDialog.ShowDialog();
            fileImage1TextBox.Text = fileDialog.SafeFileName;
            if (string.Empty == fileDialog.FileName) return;
            if (fileDialog == null) return;

            _layers.Clear();
            _diffLayers.Clear();

            UpdateName(fileDialog.SafeFileName);

            ExtractFiles.ExtractFile(fileDialog.FileName);

            DirSearch(Path.Combine(
                Directory.GetParent(Path.GetFullPath(fileDialog.FileName))?.FullName ??
                throw new InvalidOperationException(),
                Path.GetFileNameWithoutExtension(fileDialog.SafeFileName) ?? throw new InvalidOperationException()));

            ExtractFiles.DeleteExtractedFiles();

            _layers = _layers.OrderBy(s => s.Position).ToList();

            foreach (var layer in _layers)
            {
                CustomDirectory diffLayer;
                if (_diffLayers.Count == 0)
                {
                    diffLayer = new CustomDirectory(layer);
                    _diffLayers.Add(diffLayer);
                }
                else
                {
                    diffLayer = new CustomDirectory(_diffLayers.Last())
                    {
                        Name = layer.Name
                    };
                    diffLayer.CleanPrevDiff();
                    diffLayer.DiffDive(layer);
                    _diffLayers.Add(diffLayer);
                }
            }

            layerList.Items.Clear();

            foreach (var item in _layers.ToArray()) layerList.Items.Add(item.Name.Substring(0, 30));

            ShowTreeView(_diffLayers.First().Position);
        }

        private void DirSearch(string path)
        {
            var positions = new Dictionary<int, string>();

            foreach (var tempFile in Directory.GetFiles(path))
                using (var stream = File.OpenRead(tempFile))
                {
                    if (Path.GetFileNameWithoutExtension(tempFile) != "manifest") continue;

                    using (var reader = new JsonTextReader(File.OpenText(tempFile)))
                    {
                        var o2 = (JArray)JToken.ReadFrom(reader);
                        var i = 0;
                        Debug.Assert(o2.First != null, "o2.First != null");
                        Debug.Assert(o2.First.Last != null, "o2.First.Last != null");
                        foreach (var value in o2.First.Last.Values())
                        {
                            var index = value.ToString().IndexOf('/');
                            positions.Add(i++, value.ToString().Substring(0, index));
                        }
                    }
                }

            foreach (var dir in Directory.GetDirectories(path))
            {
                var layer = new CustomDirectory(Path.GetFileName(dir));
                if (positions.ContainsValue(layer.Name))
                    layer.Position = positions.FirstOrDefault(s => s.Value == layer.Name).Key;

                layer.Dive(dir);
                _layers.Add(layer);
            }
        }

        private void ShowTreeView(int index)
        {
            differenceTreeView.Nodes.Clear();
            DiffLayerTreeView(index);
        }


        private void LayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (layerList.SelectedIndex >= _layers.Count || layerList.SelectedIndex <= -1) return;

            ShowTreeView(layerList.SelectedIndex);
        }


        public static TreeNode CreateTreeNode(CustomDirectory dirInfo)
        {
            var directoryNode = new TreeNode(dirInfo.Name);

            switch (dirInfo.FolderState)
            {
                case FolderState.Deleted:
                    directoryNode.ImageIndex = (int)DockerImageDiff.Icon.FolderDeleted;
                    directoryNode.SelectedImageIndex = (int)DockerImageDiff.Icon.FolderDeleted;
                    directoryNode.BackColor = Color.LightPink;
                    directoryNode.Expand();
                    break;
                case FolderState.Modified:
                    directoryNode.ImageIndex = (int)DockerImageDiff.Icon.FolderModified;
                    directoryNode.SelectedImageIndex = (int)DockerImageDiff.Icon.FolderModified;
                    directoryNode.BackColor = Color.LightSkyBlue;
                    directoryNode.Expand();
                    break;
                case FolderState.Added:
                    directoryNode.ImageIndex = (int)DockerImageDiff.Icon.FolderAdded;
                    directoryNode.SelectedImageIndex = (int)DockerImageDiff.Icon.FolderAdded;
                    directoryNode.BackColor = Color.PaleGreen;
                    directoryNode.Expand();
                    break;
                default:
                    directoryNode.ImageIndex = (int)DockerImageDiff.Icon.Folder;
                    directoryNode.SelectedImageIndex = (int)DockerImageDiff.Icon.Folder;
                    break;
            }

            foreach (var dir in dirInfo.GetDirectories) directoryNode.Nodes.Add(CreateTreeNode(dir));

            foreach (var file in dirInfo.GetFiles)
            {
                var fileNode = new TreeNode(file.Name);

                switch (file.FileState)
                {
                    case FileState.Deleted:
                        fileNode.ImageIndex = (int)DockerImageDiff.Icon.FileDeleted;
                        fileNode.SelectedImageIndex = (int)DockerImageDiff.Icon.FileDeleted;
                        fileNode.BackColor = Color.LightPink;
                        break;
                    case FileState.Modified:
                        fileNode.ImageIndex = (int)DockerImageDiff.Icon.FileModified;
                        fileNode.SelectedImageIndex = (int)DockerImageDiff.Icon.FileModified;
                        fileNode.BackColor = Color.LightSkyBlue;
                        break;
                    case FileState.Added:
                        fileNode.ImageIndex = (int)DockerImageDiff.Icon.FileAdded;
                        fileNode.SelectedImageIndex = (int)DockerImageDiff.Icon.FileAdded;
                        fileNode.BackColor = Color.PaleGreen;
                        break;
                    default:
                        fileNode.ImageIndex = (int)DockerImageDiff.Icon.File;
                        fileNode.SelectedImageIndex = (int)DockerImageDiff.Icon.File;
                        break;
                }

                directoryNode.Nodes.Add(fileNode);
            }

            return directoryNode;
        }

        private void DiffLayerTreeView(int index)
        {
            differenceTreeView.Nodes.Add(CreateTreeNode(_diffLayers[index].GetDirectories
                .Find(d => d.Name == "layer")));
        }
    }
}