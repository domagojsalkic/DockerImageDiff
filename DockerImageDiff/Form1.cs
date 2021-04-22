using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DockerImageDiff;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DockerImageDiff
{
    public partial class DockerImageCompare : Form
    {
        public DockerImageCompare()
        {
            InitializeComponent();
        }

        private MyDirectory DockerImage = new MyDirectory("DockerImage");
        private List<MyDirectory> Layers = new List<MyDirectory>();
        private string FolderPath;

        private void selectImage1Button_Click(object sender, EventArgs e)
        {
            Layers.Clear();
            fileDialog = new OpenFileDialog();
            fileDialog.Filter = "TAR (*.tar)|*.tar;|All files (*.*)|*.*";
            fileDialog.ShowDialog();
            fileImage1TextBox.Text = fileDialog.SafeFileName;
            FolderPath = Path.Combine(Path.GetDirectoryName(fileDialog.FileName),  Path.GetFileNameWithoutExtension(fileDialog.FileName));


            ExtractFiles.ExtractFile(fileDialog.FileName);

            DirSearch(Path.Combine(Directory.GetParent(Path.GetFullPath(fileDialog.FileName)).FullName,Path.GetFileNameWithoutExtension(fileDialog.SafeFileName)));

            Layers = Layers.OrderBy(s => s.Position).ToList();

            DockerImage = Layers.First();

            foreach (var item in Layers.ToArray())
            {
                layerList.Items.Add(item.DirectoryName.Substring(0, 30)); 
            }
        }


        private void DirSearch(string path)
        {
            Dictionary<int, string> Positions = new Dictionary<int, string>();

            foreach (var tempFile in Directory.GetFiles(path))
            {
                using (FileStream stream = File.OpenRead(tempFile))
                {
                    if (Path.GetFileNameWithoutExtension(tempFile) == "manifest")
                    {
                        using (JsonTextReader reader = new JsonTextReader(File.OpenText(tempFile)))
                        {
                            JArray o2 = (JArray)JToken.ReadFrom(reader);
                            int i = 0;
                            foreach (var value in o2.First.Last.Values())
                            {
                                int index = value.ToString().IndexOf('/');
                                Positions.Add(i++, value.ToString().Substring(0,index));
                            }
                        }
                    }

                }
            }

            foreach (var dir in Directory.GetDirectories(path))
            {
                MyDirectory layer = new MyDirectory(Path.GetFileName(dir));
                if (Positions.ContainsValue(layer.DirectoryName))
                {
                    layer.Position = Positions.FirstOrDefault(s => s.Value == layer.DirectoryName).Key;
                }
                layer.Dive(dir);
                Layers.Add(layer);
            }
        }

        private void layerList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public static TreeNode CreateDirectoryNode(DirectoryInfo dirInfo)
        {
            TreeNode directoryNode = new TreeNode(dirInfo.Name);

            foreach (var dir in dirInfo.GetDirectories())
            {
                directoryNode.Nodes.Add(CreateDirectoryNode(dir));
            }
            
            foreach (var file in dirInfo.GetFiles())
            {
                directoryNode.Nodes.Add(new TreeNode(file.Name));
            }

            return directoryNode;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            differenceTreeView.Nodes.Clear();

            var rootDirInfo = new DirectoryInfo(Path.Combine(FolderPath, Layers.First(s=>s.Position == 0).DirectoryName));

            differenceTreeView.Nodes.Add(CreateDirectoryNode(rootDirInfo));
        }
    }
}
