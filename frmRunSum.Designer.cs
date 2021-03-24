namespace ateRun
{
    partial class frmRunSum
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRunSum));
            this.labelTitleTotalRun = new System.Windows.Forms.Label();
            this.tabCtrlSumInfo = new System.Windows.Forms.TabControl();
            this.tpSumRunInfo = new System.Windows.Forms.TabPage();
            this.tpSumErrInfo = new System.Windows.Forms.TabPage();
            this.lvSumRunInfo = new System.Windows.Forms.ListView();
            this.colSumIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSumUutSn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSumUutRes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSumUutInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSumTestBegin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSumTestEnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSumTestCost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvSumErrInfo = new System.Windows.Forms.ListView();
            this.colSumErrInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSumErrCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSumErrUutNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSumErrUutSn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabCtrlSumInfo.SuspendLayout();
            this.tpSumRunInfo.SuspendLayout();
            this.tpSumErrInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitleTotalRun
            // 
            this.labelTitleTotalRun.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTitleTotalRun.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitleTotalRun.Location = new System.Drawing.Point(0, 0);
            this.labelTitleTotalRun.Name = "labelTitleTotalRun";
            this.labelTitleTotalRun.Size = new System.Drawing.Size(641, 24);
            this.labelTitleTotalRun.TabIndex = 12;
            this.labelTitleTotalRun.Text = "全部测试情况统计";
            this.labelTitleTotalRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabCtrlSumInfo
            // 
            this.tabCtrlSumInfo.Controls.Add(this.tpSumRunInfo);
            this.tabCtrlSumInfo.Controls.Add(this.tpSumErrInfo);
            this.tabCtrlSumInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlSumInfo.Location = new System.Drawing.Point(0, 24);
            this.tabCtrlSumInfo.Name = "tabCtrlSumInfo";
            this.tabCtrlSumInfo.SelectedIndex = 0;
            this.tabCtrlSumInfo.Size = new System.Drawing.Size(641, 436);
            this.tabCtrlSumInfo.TabIndex = 13;
            // 
            // tpSumRunInfo
            // 
            this.tpSumRunInfo.Controls.Add(this.lvSumRunInfo);
            this.tpSumRunInfo.Location = new System.Drawing.Point(4, 22);
            this.tpSumRunInfo.Name = "tpSumRunInfo";
            this.tpSumRunInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpSumRunInfo.Size = new System.Drawing.Size(633, 410);
            this.tpSumRunInfo.TabIndex = 0;
            this.tpSumRunInfo.Text = "测试情况统计";
            this.tpSumRunInfo.UseVisualStyleBackColor = true;
            // 
            // tpSumErrInfo
            // 
            this.tpSumErrInfo.Controls.Add(this.lvSumErrInfo);
            this.tpSumErrInfo.Location = new System.Drawing.Point(4, 22);
            this.tpSumErrInfo.Name = "tpSumErrInfo";
            this.tpSumErrInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpSumErrInfo.Size = new System.Drawing.Size(633, 410);
            this.tpSumErrInfo.TabIndex = 1;
            this.tpSumErrInfo.Text = "失败情况统计";
            this.tpSumErrInfo.UseVisualStyleBackColor = true;
            // 
            // lvSumRunInfo
            // 
            this.lvSumRunInfo.BackColor = System.Drawing.Color.Black;
            this.lvSumRunInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSumIndex,
            this.colSumTestCost,
            this.colSumUutSn,
            this.colSumUutRes,
            this.colSumUutInfo,
            this.colSumTestBegin,
            this.colSumTestEnd});
            this.lvSumRunInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSumRunInfo.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Bold);
            this.lvSumRunInfo.ForeColor = System.Drawing.Color.Yellow;
            this.lvSumRunInfo.FullRowSelect = true;
            this.lvSumRunInfo.GridLines = true;
            this.lvSumRunInfo.Location = new System.Drawing.Point(3, 3);
            this.lvSumRunInfo.Name = "lvSumRunInfo";
            this.lvSumRunInfo.Size = new System.Drawing.Size(627, 404);
            this.lvSumRunInfo.TabIndex = 16;
            this.lvSumRunInfo.UseCompatibleStateImageBehavior = false;
            this.lvSumRunInfo.View = System.Windows.Forms.View.Details;
            // 
            // colSumIndex
            // 
            this.colSumIndex.Text = "序号";
            this.colSumIndex.Width = 50;
            // 
            // colSumUutSn
            // 
            this.colSumUutSn.Text = "待测件序列号";
            this.colSumUutSn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colSumUutSn.Width = 189;
            // 
            // colSumUutRes
            // 
            this.colSumUutRes.Text = "通过/失败";
            this.colSumUutRes.Width = 76;
            // 
            // colSumUutInfo
            // 
            this.colSumUutInfo.Text = "详细信息";
            this.colSumUutInfo.Width = 213;
            // 
            // colSumTestBegin
            // 
            this.colSumTestBegin.Text = "开始时间";
            this.colSumTestBegin.Width = 75;
            // 
            // colSumTestEnd
            // 
            this.colSumTestEnd.Text = "结束时间";
            this.colSumTestEnd.Width = 78;
            // 
            // colSumTestCost
            // 
            this.colSumTestCost.Text = "耗时(s)";
            this.colSumTestCost.Width = 66;
            // 
            // lvSumErrInfo
            // 
            this.lvSumErrInfo.BackColor = System.Drawing.Color.Black;
            this.lvSumErrInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSumErrInfo,
            this.colSumErrCode,
            this.colSumErrUutNum,
            this.colSumErrUutSn});
            this.lvSumErrInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSumErrInfo.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Bold);
            this.lvSumErrInfo.ForeColor = System.Drawing.Color.Yellow;
            this.lvSumErrInfo.FullRowSelect = true;
            this.lvSumErrInfo.GridLines = true;
            this.lvSumErrInfo.Location = new System.Drawing.Point(3, 3);
            this.lvSumErrInfo.Name = "lvSumErrInfo";
            this.lvSumErrInfo.Size = new System.Drawing.Size(627, 404);
            this.lvSumErrInfo.TabIndex = 17;
            this.lvSumErrInfo.UseCompatibleStateImageBehavior = false;
            this.lvSumErrInfo.View = System.Windows.Forms.View.Details;
            // 
            // colSumErrInfo
            // 
            this.colSumErrInfo.Text = "序号";
            this.colSumErrInfo.Width = 50;
            // 
            // colSumErrCode
            // 
            this.colSumErrCode.Text = "错误代码";
            this.colSumErrCode.Width = 75;
            // 
            // colSumErrUutNum
            // 
            this.colSumErrUutNum.Text = "失败次数";
            this.colSumErrUutNum.Width = 74;
            // 
            // colSumErrUutSn
            // 
            this.colSumErrUutSn.Text = "待测件SN";
            this.colSumErrUutSn.Width = 172;
            // 
            // frmRunSum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(641, 460);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.tabCtrlSumInfo);
            this.Controls.Add(this.labelTitleTotalRun);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRunSum";
            this.Text = "统计信息";
            this.Load += new System.EventHandler(this.frmRunSum_Load);
            this.tabCtrlSumInfo.ResumeLayout(false);
            this.tpSumRunInfo.ResumeLayout(false);
            this.tpSumErrInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelTitleTotalRun;
        private System.Windows.Forms.TabControl tabCtrlSumInfo;
        private System.Windows.Forms.TabPage tpSumRunInfo;
        private System.Windows.Forms.ListView lvSumRunInfo;
        private System.Windows.Forms.ColumnHeader colSumIndex;
        private System.Windows.Forms.ColumnHeader colSumTestBegin;
        private System.Windows.Forms.ColumnHeader colSumTestEnd;
        private System.Windows.Forms.ColumnHeader colSumTestCost;
        private System.Windows.Forms.ColumnHeader colSumUutSn;
        private System.Windows.Forms.ColumnHeader colSumUutRes;
        private System.Windows.Forms.ColumnHeader colSumUutInfo;
        private System.Windows.Forms.TabPage tpSumErrInfo;
        private System.Windows.Forms.ListView lvSumErrInfo;
        private System.Windows.Forms.ColumnHeader colSumErrInfo;
        private System.Windows.Forms.ColumnHeader colSumErrCode;
        private System.Windows.Forms.ColumnHeader colSumErrUutNum;
        private System.Windows.Forms.ColumnHeader colSumErrUutSn;



    }
}