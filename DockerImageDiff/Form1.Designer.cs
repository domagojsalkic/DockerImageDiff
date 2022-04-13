using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace DockerImageDiff
{
    partial class DockerImageCompare
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockerImageCompare));
            this.differenceTreeView = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.selectImage1Button = new System.Windows.Forms.Button();
            this.dockerImageLabel = new System.Windows.Forms.Label();
            this.fileImage1TextBox = new System.Windows.Forms.TextBox();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.layerList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // differenceTreeView
            // 
            this.differenceTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.differenceTreeView.ImageIndex = 0;
            this.differenceTreeView.ImageList = this.imageList1;
            this.differenceTreeView.Location = new System.Drawing.Point(312, 40);
            this.differenceTreeView.Name = "differenceTreeView";
            this.differenceTreeView.SelectedImageIndex = 0;
            this.differenceTreeView.Size = new System.Drawing.Size(827, 656);
            this.differenceTreeView.StateImageList = this.imageList1;
            this.differenceTreeView.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.png");
            this.imageList1.Images.SetKeyName(1, "add-folder.png");
            this.imageList1.Images.SetKeyName(2, "folder-modified.png");
            this.imageList1.Images.SetKeyName(3, "delete-folder.png");
            this.imageList1.Images.SetKeyName(4, "file.png");
            this.imageList1.Images.SetKeyName(5, "add-file.png");
            this.imageList1.Images.SetKeyName(6, "modified-file.png");
            this.imageList1.Images.SetKeyName(7, "delete-file.png");
            // 
            // selectImage1Button
            // 
            this.selectImage1Button.Location = new System.Drawing.Point(217, 40);
            this.selectImage1Button.Name = "selectImage1Button";
            this.selectImage1Button.Size = new System.Drawing.Size(75, 23);
            this.selectImage1Button.TabIndex = 2;
            this.selectImage1Button.Text = "Select";
            this.selectImage1Button.UseVisualStyleBackColor = true;
            this.selectImage1Button.Click += new System.EventHandler(this.selectImage1Button_Click);
            // 
            // dockerImageLabel
            // 
            this.dockerImageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dockerImageLabel.AutoSize = true;
            this.dockerImageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dockerImageLabel.Location = new System.Drawing.Point(12, 26);
            this.dockerImageLabel.Name = "dockerImageLabel";
            this.dockerImageLabel.Size = new System.Drawing.Size(86, 13);
            this.dockerImageLabel.TabIndex = 3;
            this.dockerImageLabel.Text = "Docker Image";
            // 
            // fileImage1TextBox
            // 
            this.fileImage1TextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.fileImage1TextBox.Location = new System.Drawing.Point(15, 42);
            this.fileImage1TextBox.Name = "fileImage1TextBox";
            this.fileImage1TextBox.ReadOnly = true;
            this.fileImage1TextBox.Size = new System.Drawing.Size(196, 20);
            this.fileImage1TextBox.TabIndex = 1;
            // 
            // fileDialog
            // 
            this.fileDialog.FileName = "fileDialog";
            // 
            // layerList
            // 
            this.layerList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.layerList.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.layerList.FormattingEnabled = true;
            this.layerList.Location = new System.Drawing.Point(12, 81);
            this.layerList.Name = "layerList";
            this.layerList.Size = new System.Drawing.Size(280, 615);
            this.layerList.TabIndex = 6;
            this.layerList.SelectedIndexChanged += new System.EventHandler(this.LayerList_SelectedIndexChanged);
            // 
            // DockerImageCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 714);
            this.Controls.Add(this.layerList);
            this.Controls.Add(this.fileImage1TextBox);
            this.Controls.Add(this.dockerImageLabel);
            this.Controls.Add(this.selectImage1Button);
            this.Controls.Add(this.differenceTreeView);
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(650, 500);
            this.Name = "DockerImageCompare";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Docker Image Compare";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DockerImageCompare_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public void UpdateName(string name)
        {
            Text = "Docker Image Compare";
            Text += (" (" + name + ")");
        }

        private TreeView differenceTreeView;
        private Button selectImage1Button;
        private Label dockerImageLabel;
        private TextBox fileImage1TextBox;
        private OpenFileDialog fileDialog;
        private ListBox layerList;
        private ImageList imageList1;
    }
}

