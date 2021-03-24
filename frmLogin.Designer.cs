namespace ateRun
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cbUserName = new System.Windows.Forms.ComboBox();
            this.labelUserType = new System.Windows.Forms.Label();
            this.cbUserType = new System.Windows.Forms.ComboBox();
            this.tbPassWord = new System.Windows.Forms.TextBox();
            this.labelUserDisc = new System.Windows.Forms.Label();
            this.labelPassWord = new System.Windows.Forms.Label();
            this.labelUserName = new System.Windows.Forms.Label();
            this.tbDisc = new System.Windows.Forms.TextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cbUserName);
            this.splitContainer1.Panel1.Controls.Add(this.labelUserType);
            this.splitContainer1.Panel1.Controls.Add(this.cbUserType);
            this.splitContainer1.Panel1.Controls.Add(this.tbPassWord);
            this.splitContainer1.Panel1.Controls.Add(this.labelUserDisc);
            this.splitContainer1.Panel1.Controls.Add(this.labelPassWord);
            this.splitContainer1.Panel1.Controls.Add(this.labelUserName);
            this.splitContainer1.Panel1.Controls.Add(this.tbDisc);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(334, 322);
            this.splitContainer1.SplitterDistance = 276;
            this.splitContainer1.TabIndex = 1;
            // 
            // cbUserName
            // 
            this.cbUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbUserName.FormattingEnabled = true;
            this.cbUserName.Location = new System.Drawing.Point(121, 48);
            this.cbUserName.Name = "cbUserName";
            this.cbUserName.Size = new System.Drawing.Size(200, 20);
            this.cbUserName.TabIndex = 46;
            // 
            // labelUserType
            // 
            this.labelUserType.Location = new System.Drawing.Point(16, 14);
            this.labelUserType.Name = "labelUserType";
            this.labelUserType.Size = new System.Drawing.Size(100, 14);
            this.labelUserType.TabIndex = 14;
            this.labelUserType.Text = "用户类型:";
            this.labelUserType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbUserType
            // 
            this.cbUserType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbUserType.FormattingEnabled = true;
            this.cbUserType.Items.AddRange(new object[] {
            "操作员",
            "产线管理员",
            "工具开发员"});
            this.cbUserType.Location = new System.Drawing.Point(121, 12);
            this.cbUserType.Name = "cbUserType";
            this.cbUserType.Size = new System.Drawing.Size(201, 20);
            this.cbUserType.TabIndex = 0;
            this.cbUserType.SelectedIndexChanged += new System.EventHandler(this.cbUserType_SelectedIndexChanged);
            // 
            // tbPassWord
            // 
            this.tbPassWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassWord.Location = new System.Drawing.Point(120, 82);
            this.tbPassWord.Name = "tbPassWord";
            this.tbPassWord.PasswordChar = '*';
            this.tbPassWord.Size = new System.Drawing.Size(201, 21);
            this.tbPassWord.TabIndex = 2;
            // 
            // labelUserDisc
            // 
            this.labelUserDisc.Location = new System.Drawing.Point(16, 122);
            this.labelUserDisc.Name = "labelUserDisc";
            this.labelUserDisc.Size = new System.Drawing.Size(100, 14);
            this.labelUserDisc.TabIndex = 9;
            this.labelUserDisc.Text = "说    明:";
            this.labelUserDisc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPassWord
            // 
            this.labelPassWord.Location = new System.Drawing.Point(16, 86);
            this.labelPassWord.Name = "labelPassWord";
            this.labelPassWord.Size = new System.Drawing.Size(100, 14);
            this.labelPassWord.TabIndex = 6;
            this.labelPassWord.Text = "登陆密码:";
            this.labelPassWord.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelUserName
            // 
            this.labelUserName.Location = new System.Drawing.Point(16, 50);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(100, 14);
            this.labelUserName.TabIndex = 5;
            this.labelUserName.Text = "用户名称:";
            this.labelUserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDisc
            // 
            this.tbDisc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDisc.Location = new System.Drawing.Point(120, 119);
            this.tbDisc.Multiline = true;
            this.tbDisc.Name = "tbDisc";
            this.tbDisc.ReadOnly = true;
            this.tbDisc.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbDisc.Size = new System.Drawing.Size(201, 140);
            this.tbDisc.TabIndex = 4;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btOK);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btCancel);
            this.splitContainer2.Size = new System.Drawing.Size(334, 42);
            this.splitContainer2.SplitterDistance = 163;
            this.splitContainer2.TabIndex = 0;
            // 
            // btOK
            // 
            this.btOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btOK.Location = new System.Drawing.Point(0, 0);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(163, 42);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "确定";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btCancel.Location = new System.Drawing.Point(0, 0);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(167, 42);
            this.btCancel.TabIndex = 0;
            this.btCancel.Text = "取消";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // frmLogin
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 322);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登陆界面";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.Shown += new System.EventHandler(this.frmLogin_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox tbPassWord;
        private System.Windows.Forms.Label labelUserDisc;
        private System.Windows.Forms.Label labelPassWord;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.TextBox tbDisc;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label labelUserType;
        private System.Windows.Forms.ComboBox cbUserType;
        private System.Windows.Forms.ComboBox cbUserName;

    }
}