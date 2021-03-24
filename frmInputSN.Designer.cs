namespace ateRun
{
    partial class frmInputSN
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInputSN));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.labelSetPromptMsg = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tb1stInputSN = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tb2ndInputSN = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.labelSetPromptMsg);
            this.splitContainer2.Panel1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(334, 94);
            this.splitContainer2.SplitterDistance = 29;
            this.splitContainer2.TabIndex = 4;
            this.splitContainer2.TabStop = false;
            // 
            // labelSetPromptMsg
            // 
            this.labelSetPromptMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSetPromptMsg.Location = new System.Drawing.Point(0, 0);
            this.labelSetPromptMsg.Name = "labelSetPromptMsg";
            this.labelSetPromptMsg.Size = new System.Drawing.Size(334, 29);
            this.labelSetPromptMsg.TabIndex = 1;
            this.labelSetPromptMsg.Text = "请输入第一个手机SN号码";
            this.labelSetPromptMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.splitContainer1.Panel1.Controls.Add(this.tb1stInputSN);
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tb2ndInputSN);
            this.splitContainer1.Panel2.Controls.Add(this.textBox2);
            this.splitContainer1.Size = new System.Drawing.Size(334, 61);
            this.splitContainer1.SplitterDistance = 27;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // tb1stInputSN
            // 
            this.tb1stInputSN.BackColor = System.Drawing.SystemColors.Control;
            this.tb1stInputSN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb1stInputSN.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb1stInputSN.ForeColor = System.Drawing.Color.Black;
            this.tb1stInputSN.Location = new System.Drawing.Point(94, 0);
            this.tb1stInputSN.Name = "tb1stInputSN";
            this.tb1stInputSN.Size = new System.Drawing.Size(240, 24);
            this.tb1stInputSN.TabIndex = 0;
            this.tb1stInputSN.Text = "8S5P69A6MWDGHA6045V00S";
            this.tb1stInputSN.TextChanged += new System.EventHandler(this.tb1stInputSN_TextChanged);
            this.tb1stInputSN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb1stInputSN_KeyPress);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBox1.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(94, 27);
            this.textBox1.TabIndex = 0;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "第一次输入:";
            // 
            // tb2ndInputSN
            // 
            this.tb2ndInputSN.BackColor = System.Drawing.SystemColors.Control;
            this.tb2ndInputSN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb2ndInputSN.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb2ndInputSN.ForeColor = System.Drawing.Color.Black;
            this.tb2ndInputSN.Location = new System.Drawing.Point(94, 0);
            this.tb2ndInputSN.Name = "tb2ndInputSN";
            this.tb2ndInputSN.Size = new System.Drawing.Size(240, 24);
            this.tb2ndInputSN.TabIndex = 0;
            this.tb2ndInputSN.Text = "8S5P69A6MWDGHA6045V00S";
            this.tb2ndInputSN.TextChanged += new System.EventHandler(this.tb2ndInputSN_TextChanged);
            this.tb2ndInputSN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb2ndInputSN_KeyPress);
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBox2.Font = new System.Drawing.Font("SimSun", 10.5F);
            this.textBox2.Location = new System.Drawing.Point(0, 0);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(94, 30);
            this.textBox2.TabIndex = 1;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "第二次输入:";
            // 
            // frmInputSN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 94);
            this.Controls.Add(this.splitContainer2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInputSN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "输入SN界面";
            this.Load += new System.EventHandler(this.frmInputSN_Load);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox tb1stInputSN;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox tb2ndInputSN;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label labelSetPromptMsg;



    }
}