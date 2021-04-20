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

namespace DockerImageDiff
{
    public partial class DockerImageCompare : Form
    {
        public DockerImageCompare()
        {
            InitializeComponent();
        }

        private List<MyDirectory> Layers = new List<MyDirectory>();

        private void selectImage1Button_Click(object sender, EventArgs e)
        {
            Layers.Clear();
            fileDialog = new OpenFileDialog();
            fileDialog.Filter = "TAR (*.tar)|*.tar;|All files (*.*)|*.*";
            fileDialog.ShowDialog();
            fileImage1TextBox.Text = fileDialog.SafeFileName;

            ExtractFiles.ExtractFile(fileDialog.FileName);

            DirSearch(Path.Combine(Directory.GetParent(Path.GetFullPath(fileDialog.FileName)).FullName,Path.GetFileNameWithoutExtension(fileDialog.SafeFileName)));

            foreach (var item in Layers.ToArray())
            {
                layerList.Items.Add(item.DirectoryName.Substring(0, 30)); 
            }
        }

        private void DirSearch(string path)
        {
            foreach (var dir in Directory.GetDirectories(path))
            {
                MyDirectory layer = new MyDirectory(Path.GetFileName(dir));
                layer.Dive(dir);
                Layers.Add(layer);
            }
        }

        private void layerList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
