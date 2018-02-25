namespace Controller_WinForms
{
    partial class FrmController
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
            this.LvClients = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CmsClients = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MiStartFileManagerSession = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.LblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.BtnStart = new System.Windows.Forms.Button();
            this.CmsClients.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LvClients
            // 
            this.LvClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.LvClients.ContextMenuStrip = this.CmsClients;
            this.LvClients.FullRowSelect = true;
            this.LvClients.GridLines = true;
            this.LvClients.Location = new System.Drawing.Point(12, 12);
            this.LvClients.MultiSelect = false;
            this.LvClients.Name = "LvClients";
            this.LvClients.Size = new System.Drawing.Size(696, 229);
            this.LvClients.TabIndex = 0;
            this.LvClients.UseCompatibleStateImageBehavior = false;
            this.LvClients.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Socket:";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "IP Address:";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "UserName:";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Operating System:";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 160;
            // 
            // CmsClients
            // 
            this.CmsClients.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.CmsClients.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MiStartFileManagerSession});
            this.CmsClients.Name = "CmsClients";
            this.CmsClients.Size = new System.Drawing.Size(165, 28);
            // 
            // MiStartFileManagerSession
            // 
            this.MiStartFileManagerSession.Name = "MiStartFileManagerSession";
            this.MiStartFileManagerSession.Size = new System.Drawing.Size(164, 24);
            this.MiStartFileManagerSession.Text = "File Manager";
            this.MiStartFileManagerSession.Click += new System.EventHandler(this.MiStartFileManagerSession_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 286);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(720, 25);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // LblStatus
            // 
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(172, 20);
            this.LblStatus.Text = "Status: Waiting for start...";
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(633, 247);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(75, 31);
            this.BtnStart.TabIndex = 2;
            this.BtnStart.Text = "Start";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // FrmController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 311);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.LvClients);
            this.Name = "FrmController";
            this.Text = "Controller [WinForms]";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.CmsClients.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView LvClients;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.ToolStripStatusLabel LblStatus;
        private System.Windows.Forms.ContextMenuStrip CmsClients;
        private System.Windows.Forms.ToolStripMenuItem MiStartFileManagerSession;
    }
}

