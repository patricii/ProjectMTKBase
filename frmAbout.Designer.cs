namespace ateRun
{
    partial class frmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.labelSwVersion = new System.Windows.Forms.Label();
            this.tbSwVersion = new System.Windows.Forms.TextBox();
            this.linkWebSite = new System.Windows.Forms.LinkLabel();
            this.tbSwDisc = new System.Windows.Forms.TextBox();
            this.labelWebSite = new System.Windows.Forms.Label();
            this.labelSwDisc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelSwVersion
            // 
            this.labelSwVersion.AutoSize = true;
            this.labelSwVersion.Location = new System.Drawing.Point(26, 28);
            this.labelSwVersion.Name = "labelSwVersion";
            this.labelSwVersion.Size = new System.Drawing.Size(59, 12);
            this.labelSwVersion.TabIndex = 0;
            this.labelSwVersion.Text = "软件版本:";
            // 
            // tbSwVersion
            // 
            this.tbSwVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSwVersion.Location = new System.Drawing.Point(89, 25);
            this.tbSwVersion.Name = "tbSwVersion";
            this.tbSwVersion.ReadOnly = true;
            this.tbSwVersion.Size = new System.Drawing.Size(265, 21);
            this.tbSwVersion.TabIndex = 1;
            // 
            // linkWebSite
            // 
            this.linkWebSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.linkWebSite.AutoSize = true;
            this.linkWebSite.LinkVisited = true;
            this.linkWebSite.Location = new System.Drawing.Point(89, 70);
            this.linkWebSite.Name = "linkWebSite";
            this.linkWebSite.Size = new System.Drawing.Size(89, 12);
            this.linkWebSite.TabIndex = 2;
            this.linkWebSite.TabStop = true;
            this.linkWebSite.Text = "www.lenovo.com";
            // 
            // tbSwDisc
            // 
            this.tbSwDisc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSwDisc.Location = new System.Drawing.Point(89, 110);
            this.tbSwDisc.Multiline = true;
            this.tbSwDisc.Name = "tbSwDisc";
            this.tbSwDisc.ReadOnly = true;
            this.tbSwDisc.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbSwDisc.Size = new System.Drawing.Size(265, 95);
            this.tbSwDisc.TabIndex = 3;
            this.tbSwDisc.Text = "这是进行非信令校准和验证的程序。";
            // 
            // labelWebSite
            // 
            this.labelWebSite.AutoSize = true;
            this.labelWebSite.Location = new System.Drawing.Point(26, 70);
            this.labelWebSite.Name = "labelWebSite";
            this.labelWebSite.Size = new System.Drawing.Size(59, 12);
            this.labelWebSite.TabIndex = 4;
            this.labelWebSite.Text = "公司网址:";
            // 
            // labelSwDisc
            // 
            this.labelSwDisc.AutoSize = true;
            this.labelSwDisc.Location = new System.Drawing.Point(26, 113);
            this.labelSwDisc.Name = "labelSwDisc";
            this.labelSwDisc.Size = new System.Drawing.Size(59, 12);
            this.labelSwDisc.TabIndex = 5;
            this.labelSwDisc.Text = "软件描述:";
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 242);
            this.Controls.Add(this.labelSwDisc);
            this.Controls.Add(this.labelWebSite);
            this.Controls.Add(this.tbSwDisc);
            this.Controls.Add(this.linkWebSite);
            this.Controls.Add(this.tbSwVersion);
            this.Controls.Add(this.labelSwVersion);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "软件信息";
            this.Load += new System.EventHandler(this.frmAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSwVersion;
        private System.Windows.Forms.TextBox tbSwVersion;
        private System.Windows.Forms.LinkLabel linkWebSite;
        private System.Windows.Forms.TextBox tbSwDisc;
        private System.Windows.Forms.Label labelWebSite;
        private System.Windows.Forms.Label labelSwDisc;
    }
}