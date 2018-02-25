namespace Controller_WinForms
{
    partial class FrmFileManager
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
            this.components = new System.ComponentModel.Container();
            this.CbDrives = new System.Windows.Forms.ComboBox();
            this.TxtPath = new System.Windows.Forms.TextBox();
            this.LvFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.downloadFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CbDrives
            // 
            this.CbDrives.FormattingEnabled = true;
            this.CbDrives.Location = new System.Drawing.Point(12, 12);
            this.CbDrives.Name = "CbDrives";
            this.CbDrives.Size = new System.Drawing.Size(121, 24);
            this.CbDrives.TabIndex = 0;
            // 
            // TxtPath
            // 
            this.TxtPath.Location = new System.Drawing.Point(139, 12);
            this.TxtPath.Name = "TxtPath";
            this.TxtPath.Size = new System.Drawing.Size(498, 22);
            this.TxtPath.TabIndex = 1;
            // 
            // LvFiles
            // 
            this.LvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader6,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.LvFiles.Location = new System.Drawing.Point(12, 42);
            this.LvFiles.Name = "LvFiles";
            this.LvFiles.Size = new System.Drawing.Size(625, 199);
            this.LvFiles.TabIndex = 2;
            this.LvFiles.UseCompatibleStateImageBehavior = false;
            this.LvFiles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name:";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Type:";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader6.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Size:";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Status:";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Download speed:";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 120;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Upload speed:";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 120;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadFolderToolStripMenuItem,
            this.uploadFileToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(192, 52);
            // 
            // downloadFolderToolStripMenuItem
            // 
            this.downloadFolderToolStripMenuItem.Name = "downloadFolderToolStripMenuItem";
            this.downloadFolderToolStripMenuItem.Size = new System.Drawing.Size(191, 24);
            this.downloadFolderToolStripMenuItem.Text = "Download folder";
            // 
            // uploadFileToolStripMenuItem
            // 
            this.uploadFileToolStripMenuItem.Name = "uploadFileToolStripMenuItem";
            this.uploadFileToolStripMenuItem.Size = new System.Drawing.Size(191, 24);
            this.uploadFileToolStripMenuItem.Text = "Upload file";
            // 
            // FrmFileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 253);
            this.Controls.Add(this.LvFiles);
            this.Controls.Add(this.TxtPath);
            this.Controls.Add(this.CbDrives);
            this.Name = "FrmFileManager";
            this.Text = "File-Manager [Admin@127.0.0.1:1234]";
            this.Load += new System.EventHandler(this.FrmFileManager_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CbDrives;
        private System.Windows.Forms.TextBox TxtPath;
        private System.Windows.Forms.ListView LvFiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ToolStripMenuItem downloadFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadFileToolStripMenuItem;
    }
}