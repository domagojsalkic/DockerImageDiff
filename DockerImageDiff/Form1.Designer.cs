namespace DockerImageDiff
{
    partial class DockerImageCompare
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.differenceTreeView = new System.Windows.Forms.TreeView();
            this.selectImage1Button = new System.Windows.Forms.Button();
            this.dockerImageLabel = new System.Windows.Forms.Label();
            this.fileImage1TextBox = new System.Windows.Forms.TextBox();
            this.loadButton = new System.Windows.Forms.Button();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.layerList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // differenceTreeView
            // 
            this.differenceTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.differenceTreeView.Location = new System.Drawing.Point(312, 12);
            this.differenceTreeView.Name = "differenceTreeView";
            this.differenceTreeView.Size = new System.Drawing.Size(310, 437);
            this.differenceTreeView.TabIndex = 0;
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
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(89, 68);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(122, 45);
            this.loadButton.TabIndex = 5;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // fileDialog
            // 
            this.fileDialog.FileName = "fileDialog";
            // 
            // layerList
            // 
            this.layerList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.layerList.FormattingEnabled = true;
            this.layerList.Location = new System.Drawing.Point(12, 120);
            this.layerList.Name = "layerList";
            this.layerList.Size = new System.Drawing.Size(280, 329);
            this.layerList.TabIndex = 6;
            this.layerList.SelectedIndexChanged += new System.EventHandler(this.layerList_SelectedIndexChanged);
            // 
            // DockerImageCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 461);
            this.Controls.Add(this.layerList);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.fileImage1TextBox);
            this.Controls.Add(this.dockerImageLabel);
            this.Controls.Add(this.selectImage1Button);
            this.Controls.Add(this.differenceTreeView);
            this.MaximumSize = new System.Drawing.Size(650, 500);
            this.MinimumSize = new System.Drawing.Size(650, 500);
            this.Name = "DockerImageCompare";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Docker Image Compare";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView differenceTreeView;
        private System.Windows.Forms.Button selectImage1Button;
        private System.Windows.Forms.Label dockerImageLabel;
        private System.Windows.Forms.TextBox fileImage1TextBox;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.OpenFileDialog fileDialog;
        private System.Windows.Forms.ListBox layerList;
    }
}

