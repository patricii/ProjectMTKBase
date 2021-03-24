namespace ateRun
{
    partial class frmBzModel
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.LabelBenchId = new System.Windows.Forms.Label();
            this.textBoxScanTrackid = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ComboModelName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(246, 191);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(55, 17);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "Focus";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // LabelBenchId
            // 
            this.LabelBenchId.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelBenchId.ForeColor = System.Drawing.Color.Blue;
            this.LabelBenchId.Location = new System.Drawing.Point(170, 9);
            this.LabelBenchId.Name = "LabelBenchId";
            this.LabelBenchId.Size = new System.Drawing.Size(270, 32); // Patricio TR Number
            this.LabelBenchId.TabIndex = 14;
            this.LabelBenchId.Text = "BENCH";
            this.LabelBenchId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxScanTrackid
            // 
            this.textBoxScanTrackid.Location = new System.Drawing.Point(126, 165);
            this.textBoxScanTrackid.Name = "textBoxScanTrackid";
            this.textBoxScanTrackid.Size = new System.Drawing.Size(288, 20);
            this.textBoxScanTrackid.TabIndex = 13;
            this.textBoxScanTrackid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxScanTrackid_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(172, 56);
            this.label1.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 24);
            this.label1.TabIndex = 12;
            this.label1.Text = "Escolha o Modelo";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ComboModelName
            // 
            this.ComboModelName.FormattingEnabled = true;
            this.ComboModelName.Location = new System.Drawing.Point(196, 138);
            this.ComboModelName.Name = "ComboModelName";
            this.ComboModelName.Size = new System.Drawing.Size(176, 21);
            this.ComboModelName.TabIndex = 11;
            this.ComboModelName.SelectionChangeCommitted += new System.EventHandler(this.ComboModelName_SelectionChangeCommitted);
            // 
            // frmBzModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 214);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.LabelBenchId);
            this.Controls.Add(this.textBoxScanTrackid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ComboModelName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "frmBzModel";
            this.Text = "frmBzModel";
            this.Load += new System.EventHandler(this.frmBzModel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label LabelBenchId;
        private System.Windows.Forms.TextBox textBoxScanTrackid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ComboModelName;
    }
}