namespace ateRun
{
    partial class frmWaitDut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWaitDut));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labelMsgPrompt = new System.Windows.Forms.Label();
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
            this.splitContainer1.Panel1.Controls.Add(this.labelMsgPrompt);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(284, 143);
            this.splitContainer1.SplitterDistance = 100;
            this.splitContainer1.TabIndex = 0;
            // 
            // labelMsgPrompt
            // 
            this.labelMsgPrompt.BackColor = System.Drawing.Color.Black;
            this.labelMsgPrompt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelMsgPrompt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMsgPrompt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.labelMsgPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.labelMsgPrompt.ForeColor = System.Drawing.Color.Lime;
            this.labelMsgPrompt.Location = new System.Drawing.Point(0, 0);
            this.labelMsgPrompt.Name = "labelMsgPrompt";
            this.labelMsgPrompt.Size = new System.Drawing.Size(284, 100);
            this.labelMsgPrompt.TabIndex = 20;
            this.labelMsgPrompt.Text = "请换另一个待测件进行测试!";
            this.labelMsgPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.splitContainer2.Size = new System.Drawing.Size(284, 39);
            this.splitContainer2.SplitterDistance = 141;
            this.splitContainer2.TabIndex = 0;
            // 
            // btOK
            // 
            this.btOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btOK.Location = new System.Drawing.Point(0, 0);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(141, 39);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "确定";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            this.btCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btCancel.Location = new System.Drawing.Point(0, 0);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(139, 39);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "取消";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // frmWaitDut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 143);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWaitDut";
            this.Text = "提示信息:";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        public System.Windows.Forms.Label labelMsgPrompt;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
    }
}