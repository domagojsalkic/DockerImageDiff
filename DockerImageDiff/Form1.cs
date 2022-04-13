using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DockerImageDiff
{
    public enum ICON
    {
        FOLDER = 0,
        FOLDER_ADDED,
        FOLDER_MODIFIED,
        FOLDER_DELETED,
        FILE,
        FILE_ADDED,
        FILE_MODIFIED,
        FILE_DELETED
    }

    public partial class DockerImageCompare : Form
    {
        public DockerImageCompare()
        {
            InitializeComponent();
        }

        private List<MyDirectory> _layers = new List<MyDirectory>();
        private readonly List<MyDirectory> _diffLayers = new List<MyDirectory>();

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

            DirSearch(Path.Combine(Directory.GetParent(Path.GetFullPath(fileDialog.FileName))?.FullName ?? throw new InvalidOperationException(),
                Path.GetFileNameWithoutExtension(fileDialog.SafeFileName) ?? throw new InvalidOperationException()));

            _layers = _layers.OrderBy(s => s.Position).ToList();

            foreach (var layer in _layers)
            {
                MyDirectory diffLayer;
                if (_diffLayers.Count == 0)
                {
                    diffLayer = new MyDirectory(layer);
                    _diffLayers.Add(diffLayer);
                }
                else
                {
                    diffLayer = new MyDirectory(_diffLayers.Last())
                    {
                        Name = layer.Name
                    };
                    diffLayer.CleanPrevDiff();
                    diffLayer.DiffDive(layer);
                    _diffLayers.Add(diffLayer);
                }
            }

            layerList.Items.Clear();

            foreach (var item in _layers.ToArray())
            {
                layerList.Items.Add(item.Name.Substring(0, 30));
            }

            ShowTreeView(_diffLayers.First().Position);
        }

        private void DirSearch(string path)
        {
            var positions = new Dictionary<int, string>();

            foreach (var tempFile in Directory.GetFiles(path))
            {
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
            }

            foreach (var dir in Directory.GetDirectories(path))
            {
                var layer = new MyDirectory(Path.GetFileName(dir));
                if (positions.ContainsValue(layer.Name))
                {
                    layer.Position = positions.FirstOrDefault(s => s.Value == layer.Name).Key;
                }

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

        public static TreeNode CreateTreeNode(MyDirectory dirInfo)
        {
            var directoryNode = new TreeNode(dirInfo.Name);

            if (dirInfo.Deleted)
            {
                directoryNode.ImageIndex = (int)ICON.FOLDER_DELETED;
                directoryNode.SelectedImageIndex = (int)ICON.FOLDER_DELETED;
                directoryNode.Expand();
            }
            else if (dirInfo.Modified)
            {
                directoryNode.ImageIndex = (int)ICON.FOLDER_MODIFIED;
                directoryNode.SelectedImageIndex = (int)ICON.FOLDER_MODIFIED;
                directoryNode.Expand();
            }
            else if (dirInfo.Added)
            {
                directoryNode.ImageIndex = (int)ICON.FOLDER_ADDED;
                directoryNode.SelectedImageIndex = (int)ICON.FOLDER_ADDED;
                directoryNode.Expand();
            }
            else
            {
                directoryNode.ImageIndex = (int)ICON.FOLDER;
                directoryNode.SelectedImageIndex = (int)ICON.FOLDER;
            }

            foreach (var dir in dirInfo.GetDirectories)
            {
                directoryNode.Nodes.Add(CreateTreeNode(dir));
            }

            foreach (var file in dirInfo.GetFiles)
            {
                var treeNode = new TreeNode(file.Name);

                if (file.Deleted)
                {
                    treeNode.ImageIndex = (int)ICON.FILE_DELETED;
                    treeNode.SelectedImageIndex = (int)ICON.FILE_DELETED;
                }
                else if (file.Modified)
                {
                    treeNode.ImageIndex = (int)ICON.FILE_MODIFIED;
                    treeNode.SelectedImageIndex = (int)ICON.FILE_MODIFIED;
                }
                else if (file.Added)
                {
                    treeNode.ImageIndex = (int)ICON.FILE_ADDED;
                    treeNode.SelectedImageIndex = (int)ICON.FILE_ADDED;
                }
                else
                {
                    treeNode.ImageIndex = (int)ICON.FILE;
                    treeNode.SelectedImageIndex = (int)ICON.FILE;
                }

                
                directoryNode.Nodes.Add(treeNode);

            }

            return directoryNode;
        }

        private void DiffLayerTreeView(int index)
        {
            differenceTreeView.Nodes.Add(CreateTreeNode(_diffLayers[index].GetDirectories.Find(d=>d.Name == "layer")));
        }

        private void DockerImageCompare_FormClosed(object sender, FormClosedEventArgs e)
        {
            ExtractFiles.DeleteExtractedFiles();
        }
    }
}
