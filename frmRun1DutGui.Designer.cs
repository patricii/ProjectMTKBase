namespace ateRun
{
    partial class frmRun1DutGui
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRun1DutGui));
            this.frmRunDoc1TimerRecord = new System.Windows.Forms.Timer(this.components);
            this.tsToolButton = new System.Windows.Forms.ToolStrip();
            this.tsbRunCfg = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRunStartCalibrateMain = new System.Windows.Forms.ToolStripButton();
            this.tsbRunStop = new System.Windows.Forms.ToolStripButton();
            this.tsbRunExit = new System.Windows.Forms.ToolStripButton();
            this.tsbRunHelp = new System.Windows.Forms.ToolStripButton();
            this.tscbRunMode = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolComPortCfg = new System.Windows.Forms.ToolStrip();
            this.tslDutComPort = new System.Windows.Forms.ToolStripLabel();
            this.tsbDutPortMode0 = new System.Windows.Forms.ToolStripTextBox();
            this.tsbDutPortCom0 = new System.Windows.Forms.ToolStripTextBox();
            this.tsbDutPortCom1 = new System.Windows.Forms.ToolStripTextBox();
            this.tslFixtureComPort = new System.Windows.Forms.ToolStripLabel();
            this.tsbFixtureComPort0 = new System.Windows.Forms.ToolStripTextBox();
            this.tsSumInfo = new System.Windows.Forms.ToolStrip();
            this.tslDutPassNumPrompt = new System.Windows.Forms.ToolStripLabel();
            this.tstbDutPassNum = new System.Windows.Forms.ToolStripTextBox();
            this.tslDutFailNumPrompt = new System.Windows.Forms.ToolStripLabel();
            this.tstbDutFailNum = new System.Windows.Forms.ToolStripTextBox();
            this.tslDutFailRatePrompt = new System.Windows.Forms.ToolStripLabel();
            this.tstbDutFailRate = new System.Windows.Forms.ToolStripTextBox();
            this.ssRunStatus = new System.Windows.Forms.StatusStrip();
            this.tsslRunStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.labelProgress = new System.Windows.Forms.Label();
            this.labelTimeCost = new System.Windows.Forms.Label();
            this.labelSN = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.labelTestStatusDut0 = new System.Windows.Forms.Label();
            this.tcRunDut0 = new System.Windows.Forms.TabControl();
            this.tpRunStatDut0 = new System.Windows.Forms.TabPage();
            this.rtbRunItemDut0 = new System.Windows.Forms.RichTextBox();
            this.tpRunResDut0 = new System.Windows.Forms.TabPage();
            this.rtbRunLogDut0 = new System.Windows.Forms.RichTextBox();
            this.btRunStart = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.tsToolButton.SuspendLayout();
            this.toolComPortCfg.SuspendLayout();
            this.tsSumInfo.SuspendLayout();
            this.ssRunStatus.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.tcRunDut0.SuspendLayout();
            this.tpRunStatDut0.SuspendLayout();
            this.tpRunResDut0.SuspendLayout();
            this.SuspendLayout();
            // 
            // frmRunDoc1TimerRecord
            // 
            this.frmRunDoc1TimerRecord.Interval = 200;
            this.frmRunDoc1TimerRecord.Tick += new System.EventHandler(this.frmRunDoc1TimerRecord_Tick);
            // 
            // tsToolButton
            // 
            this.tsToolButton.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsToolButton.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRunCfg,
            this.toolStripSeparator2,
            this.tsbRunStartCalibrateMain,
            this.tsbRunStop,
            this.tsbRunExit,
            this.tsbRunHelp,
            this.tscbRunMode,
            this.toolStripSeparator1});
            this.tsToolButton.Location = new System.Drawing.Point(0, 0);
            this.tsToolButton.Name = "tsToolButton";
            this.tsToolButton.Size = new System.Drawing.Size(531, 31);
            this.tsToolButton.TabIndex = 26;
            this.tsToolButton.Text = "toolStrip1";
            // 
            // tsbRunCfg
            // 
            this.tsbRunCfg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRunCfg.Image = ((System.Drawing.Image)(resources.GetObject("tsbRunCfg.Image")));
            this.tsbRunCfg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunCfg.Name = "tsbRunCfg";
            this.tsbRunCfg.Size = new System.Drawing.Size(28, 28);
            this.tsbRunCfg.Text = "设置待测件测试属性";
            this.tsbRunCfg.Click += new System.EventHandler(this.tsbRunCfg_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbRunStartCalibrateMain
            // 
            this.tsbRunStartCalibrateMain.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRunStartCalibrateMain.Enabled = false;
            this.tsbRunStartCalibrateMain.Image = ((System.Drawing.Image)(resources.GetObject("tsbRunStartCalibrateMain.Image")));
            this.tsbRunStartCalibrateMain.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunStartCalibrateMain.Name = "tsbRunStartCalibrateMain";
            this.tsbRunStartCalibrateMain.Size = new System.Drawing.Size(28, 28);
            this.tsbRunStartCalibrateMain.Text = "开始校准";
            this.tsbRunStartCalibrateMain.Visible = false;
            // 
            // tsbRunStop
            // 
            this.tsbRunStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRunStop.Enabled = false;
            this.tsbRunStop.Image = ((System.Drawing.Image)(resources.GetObject("tsbRunStop.Image")));
            this.tsbRunStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunStop.Name = "tsbRunStop";
            this.tsbRunStop.Size = new System.Drawing.Size(28, 28);
            this.tsbRunStop.Text = "停止测试";
            this.tsbRunStop.Visible = false;
            this.tsbRunStop.Click += new System.EventHandler(this.tsbRunStop_Click);
            // 
            // tsbRunExit
            // 
            this.tsbRunExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRunExit.Enabled = false;
            this.tsbRunExit.Image = ((System.Drawing.Image)(resources.GetObject("tsbRunExit.Image")));
            this.tsbRunExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunExit.Name = "tsbRunExit";
            this.tsbRunExit.Size = new System.Drawing.Size(28, 28);
            this.tsbRunExit.Text = "退出测试";
            this.tsbRunExit.Visible = false;
            this.tsbRunExit.Click += new System.EventHandler(this.tsbRunExit_Click);
            // 
            // tsbRunHelp
            // 
            this.tsbRunHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRunHelp.Enabled = false;
            this.tsbRunHelp.Image = ((System.Drawing.Image)(resources.GetObject("tsbRunHelp.Image")));
            this.tsbRunHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunHelp.Name = "tsbRunHelp";
            this.tsbRunHelp.Size = new System.Drawing.Size(28, 28);
            this.tsbRunHelp.Text = "显示问题处理方法";
            this.tsbRunHelp.Visible = false;
            // 
            // tscbRunMode
            // 
            this.tscbRunMode.Items.AddRange(new object[] {
            "手机校准&非信令测试模式",
            "夹具线损校准模式"});
            this.tscbRunMode.Name = "tscbRunMode";
            this.tscbRunMode.Size = new System.Drawing.Size(200, 31);
            this.tscbRunMode.SelectedIndexChanged += new System.EventHandler(this.tscbRunMode_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolComPortCfg
            // 
            this.toolComPortCfg.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolComPortCfg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslDutComPort,
            this.tsbDutPortMode0,
            this.tsbDutPortCom0,
            this.tsbDutPortCom1,
            this.tslFixtureComPort,
            this.tsbFixtureComPort0});
            this.toolComPortCfg.Location = new System.Drawing.Point(0, 31);
            this.toolComPortCfg.Name = "toolComPortCfg";
            this.toolComPortCfg.Size = new System.Drawing.Size(531, 25);
            this.toolComPortCfg.TabIndex = 30;
            // 
            // tslDutComPort
            // 
            this.tslDutComPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tslDutComPort.Name = "tslDutComPort";
            this.tslDutComPort.Size = new System.Drawing.Size(34, 22);
            this.tslDutComPort.Text = "手机:";
            // 
            // tsbDutPortMode0
            // 
            this.tsbDutPortMode0.BackColor = System.Drawing.Color.Gray;
            this.tsbDutPortMode0.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.tsbDutPortMode0.ForeColor = System.Drawing.Color.Black;
            this.tsbDutPortMode0.Name = "tsbDutPortMode0";
            this.tsbDutPortMode0.ReadOnly = true;
            this.tsbDutPortMode0.Size = new System.Drawing.Size(65, 25);
            this.tsbDutPortMode0.Text = "USB";
            this.tsbDutPortMode0.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tsbDutPortCom0
            // 
            this.tsbDutPortCom0.BackColor = System.Drawing.Color.Gray;
            this.tsbDutPortCom0.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.tsbDutPortCom0.ForeColor = System.Drawing.Color.Black;
            this.tsbDutPortCom0.Name = "tsbDutPortCom0";
            this.tsbDutPortCom0.ReadOnly = true;
            this.tsbDutPortCom0.Size = new System.Drawing.Size(65, 25);
            this.tsbDutPortCom0.Text = "COM11";
            this.tsbDutPortCom0.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tsbDutPortCom0.ToolTipText = "Dut Preloader Com Port";
            // 
            // tsbDutPortCom1
            // 
            this.tsbDutPortCom1.BackColor = System.Drawing.Color.Gray;
            this.tsbDutPortCom1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.tsbDutPortCom1.ForeColor = System.Drawing.Color.Black;
            this.tsbDutPortCom1.Name = "tsbDutPortCom1";
            this.tsbDutPortCom1.ReadOnly = true;
            this.tsbDutPortCom1.Size = new System.Drawing.Size(65, 25);
            this.tsbDutPortCom1.Text = "COM12";
            this.tsbDutPortCom1.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tsbDutPortCom1.ToolTipText = "Dut Gadget Com Port";
            // 
            // tslFixtureComPort
            // 
            this.tslFixtureComPort.Name = "tslFixtureComPort";
            this.tslFixtureComPort.Size = new System.Drawing.Size(34, 22);
            this.tslFixtureComPort.Text = "夹具:";
            // 
            // tsbFixtureComPort0
            // 
            this.tsbFixtureComPort0.BackColor = System.Drawing.Color.Gray;
            this.tsbFixtureComPort0.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.tsbFixtureComPort0.ForeColor = System.Drawing.Color.Black;
            this.tsbFixtureComPort0.Name = "tsbFixtureComPort0";
            this.tsbFixtureComPort0.Size = new System.Drawing.Size(65, 25);
            this.tsbFixtureComPort0.Text = "COM18";
            this.tsbFixtureComPort0.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tsSumInfo
            // 
            this.tsSumInfo.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsSumInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslDutPassNumPrompt,
            this.tstbDutPassNum,
            this.tslDutFailNumPrompt,
            this.tstbDutFailNum,
            this.tslDutFailRatePrompt,
            this.tstbDutFailRate});
            this.tsSumInfo.Location = new System.Drawing.Point(0, 56);
            this.tsSumInfo.Name = "tsSumInfo";
            this.tsSumInfo.Size = new System.Drawing.Size(531, 25);
            this.tsSumInfo.TabIndex = 37;
            // 
            // tslDutPassNumPrompt
            // 
            this.tslDutPassNumPrompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tslDutPassNumPrompt.Name = "tslDutPassNumPrompt";
            this.tslDutPassNumPrompt.Size = new System.Drawing.Size(34, 22);
            this.tslDutPassNumPrompt.Text = "通过:";
            // 
            // tstbDutPassNum
            // 
            this.tstbDutPassNum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tstbDutPassNum.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.tstbDutPassNum.ForeColor = System.Drawing.Color.Black;
            this.tstbDutPassNum.Name = "tstbDutPassNum";
            this.tstbDutPassNum.ReadOnly = true;
            this.tstbDutPassNum.Size = new System.Drawing.Size(40, 25);
            this.tstbDutPassNum.Text = "0";
            this.tstbDutPassNum.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tslDutFailNumPrompt
            // 
            this.tslDutFailNumPrompt.Name = "tslDutFailNumPrompt";
            this.tslDutFailNumPrompt.Size = new System.Drawing.Size(34, 22);
            this.tslDutFailNumPrompt.Text = "失败:";
            // 
            // tstbDutFailNum
            // 
            this.tstbDutFailNum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.tstbDutFailNum.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.tstbDutFailNum.ForeColor = System.Drawing.Color.Black;
            this.tstbDutFailNum.Name = "tstbDutFailNum";
            this.tstbDutFailNum.Size = new System.Drawing.Size(40, 25);
            this.tstbDutFailNum.Text = "0";
            this.tstbDutFailNum.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tslDutFailRatePrompt
            // 
            this.tslDutFailRatePrompt.Name = "tslDutFailRatePrompt";
            this.tslDutFailRatePrompt.Size = new System.Drawing.Size(46, 22);
            this.tslDutFailRatePrompt.Text = "失败率:";
            // 
            // tstbDutFailRate
            // 
            this.tstbDutFailRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.tstbDutFailRate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.tstbDutFailRate.ForeColor = System.Drawing.Color.Black;
            this.tstbDutFailRate.Name = "tstbDutFailRate";
            this.tstbDutFailRate.Size = new System.Drawing.Size(80, 25);
            this.tstbDutFailRate.Text = "   0.00%";
            this.tstbDutFailRate.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ssRunStatus
            // 
            this.ssRunStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslRunStatus});
            this.ssRunStatus.Location = new System.Drawing.Point(0, 742);
            this.ssRunStatus.Name = "ssRunStatus";
            this.ssRunStatus.Size = new System.Drawing.Size(531, 22);
            this.ssRunStatus.TabIndex = 39;
            this.ssRunStatus.Text = "statusStrip1";
            // 
            // tsslRunStatus
            // 
            this.tsslRunStatus.AutoSize = false;
            this.tsslRunStatus.Name = "tsslRunStatus";
            this.tsslRunStatus.Size = new System.Drawing.Size(450, 17);
            this.tsslRunStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 81);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(531, 661);
            this.splitContainer1.SplitterDistance = 65;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 40;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.splitContainer7);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.labelSN);
            this.splitContainer4.Size = new System.Drawing.Size(531, 65);
            this.splitContainer4.SplitterDistance = 27;
            this.splitContainer4.SplitterWidth = 5;
            this.splitContainer4.TabIndex = 0;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.splitContainer6);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.labelTimeCost);
            this.splitContainer7.Size = new System.Drawing.Size(531, 27);
            this.splitContainer7.SplitterDistance = 396;
            this.splitContainer7.SplitterWidth = 5;
            this.splitContainer7.TabIndex = 21;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.pbProgress);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.labelProgress);
            this.splitContainer6.Size = new System.Drawing.Size(396, 27);
            this.splitContainer6.SplitterDistance = 263;
            this.splitContainer6.SplitterWidth = 5;
            this.splitContainer6.TabIndex = 21;
            // 
            // pbProgress
            // 
            this.pbProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbProgress.Location = new System.Drawing.Point(0, 0);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(263, 27);
            this.pbProgress.Step = 1;
            this.pbProgress.TabIndex = 19;
            // 
            // labelProgress
            // 
            this.labelProgress.BackColor = System.Drawing.Color.Black;
            this.labelProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProgress.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelProgress.ForeColor = System.Drawing.Color.Yellow;
            this.labelProgress.Location = new System.Drawing.Point(0, 0);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(128, 27);
            this.labelProgress.TabIndex = 20;
            this.labelProgress.Text = "0.00%";
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTimeCost
            // 
            this.labelTimeCost.BackColor = System.Drawing.Color.Black;
            this.labelTimeCost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTimeCost.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelTimeCost.ForeColor = System.Drawing.Color.Yellow;
            this.labelTimeCost.Location = new System.Drawing.Point(0, 0);
            this.labelTimeCost.Name = "labelTimeCost";
            this.labelTimeCost.Size = new System.Drawing.Size(130, 27);
            this.labelTimeCost.TabIndex = 21;
            this.labelTimeCost.Text = "00.000 s";
            this.labelTimeCost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSN
            // 
            this.labelSN.BackColor = System.Drawing.Color.Black;
            this.labelSN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSN.Font = new System.Drawing.Font("Arial", 12.25F, System.Drawing.FontStyle.Bold);
            this.labelSN.ForeColor = System.Drawing.Color.Yellow;
            this.labelSN.Location = new System.Drawing.Point(0, 0);
            this.labelSN.Name = "labelSN";
            this.labelSN.Size = new System.Drawing.Size(531, 33);
            this.labelSN.TabIndex = 22;
            this.labelSN.Text = "SN: ";
            this.labelSN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer5);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btRunStart);
            this.splitContainer2.Size = new System.Drawing.Size(531, 591);
            this.splitContainer2.SplitterDistance = 490;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.labelTestStatusDut0);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.tcRunDut0);
            this.splitContainer5.Size = new System.Drawing.Size(531, 490);
            this.splitContainer5.SplitterDistance = 158;
            this.splitContainer5.SplitterWidth = 5;
            this.splitContainer5.TabIndex = 29;
            // 
            // labelTestStatusDut0
            // 
            this.labelTestStatusDut0.BackColor = System.Drawing.Color.Black;
            this.labelTestStatusDut0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTestStatusDut0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTestStatusDut0.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.labelTestStatusDut0.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.labelTestStatusDut0.ForeColor = System.Drawing.Color.Yellow;
            this.labelTestStatusDut0.Location = new System.Drawing.Point(0, 0);
            this.labelTestStatusDut0.Name = "labelTestStatusDut0";
            this.labelTestStatusDut0.Size = new System.Drawing.Size(531, 158);
            this.labelTestStatusDut0.TabIndex = 19;
            this.labelTestStatusDut0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tcRunDut0
            // 
            this.tcRunDut0.Controls.Add(this.tpRunStatDut0);
            this.tcRunDut0.Controls.Add(this.tpRunResDut0);
            this.tcRunDut0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcRunDut0.Location = new System.Drawing.Point(0, 0);
            this.tcRunDut0.Name = "tcRunDut0";
            this.tcRunDut0.SelectedIndex = 0;
            this.tcRunDut0.Size = new System.Drawing.Size(531, 327);
            this.tcRunDut0.TabIndex = 28;
            // 
            // tpRunStatDut0
            // 
            this.tpRunStatDut0.Controls.Add(this.rtbRunItemDut0);
            this.tpRunStatDut0.Location = new System.Drawing.Point(4, 24);
            this.tpRunStatDut0.Name = "tpRunStatDut0";
            this.tpRunStatDut0.Size = new System.Drawing.Size(523, 299);
            this.tpRunStatDut0.TabIndex = 3;
            this.tpRunStatDut0.Text = "测试状态";
            this.tpRunStatDut0.UseVisualStyleBackColor = true;
            // 
            // rtbRunItemDut0
            // 
            this.rtbRunItemDut0.BackColor = System.Drawing.SystemColors.Info;
            this.rtbRunItemDut0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbRunItemDut0.Font = new System.Drawing.Font("Lucida Console", 8.25F);
            this.rtbRunItemDut0.Location = new System.Drawing.Point(0, 0);
            this.rtbRunItemDut0.Name = "rtbRunItemDut0";
            this.rtbRunItemDut0.ReadOnly = true;
            this.rtbRunItemDut0.Size = new System.Drawing.Size(523, 299);
            this.rtbRunItemDut0.TabIndex = 15;
            this.rtbRunItemDut0.Text = "";
            this.rtbRunItemDut0.WordWrap = false;
            // 
            // tpRunResDut0
            // 
            this.tpRunResDut0.Controls.Add(this.rtbRunLogDut0);
            this.tpRunResDut0.Location = new System.Drawing.Point(4, 22);
            this.tpRunResDut0.Name = "tpRunResDut0";
            this.tpRunResDut0.Padding = new System.Windows.Forms.Padding(3);
            this.tpRunResDut0.Size = new System.Drawing.Size(523, 301);
            this.tpRunResDut0.TabIndex = 2;
            this.tpRunResDut0.Text = "测试结果";
            this.tpRunResDut0.UseVisualStyleBackColor = true;
            // 
            // rtbRunLogDut0
            // 
            this.rtbRunLogDut0.BackColor = System.Drawing.SystemColors.Info;
            this.rtbRunLogDut0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbRunLogDut0.Font = new System.Drawing.Font("Lucida Console", 8.25F);
            this.rtbRunLogDut0.Location = new System.Drawing.Point(3, 3);
            this.rtbRunLogDut0.Name = "rtbRunLogDut0";
            this.rtbRunLogDut0.ReadOnly = true;
            this.rtbRunLogDut0.Size = new System.Drawing.Size(517, 295);
            this.rtbRunLogDut0.TabIndex = 14;
            this.rtbRunLogDut0.Text = "";
            this.rtbRunLogDut0.WordWrap = false;
            // 
            // btRunStart
            // 
            this.btRunStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btRunStart.Location = new System.Drawing.Point(0, 0);
            this.btRunStart.Name = "btRunStart";
            this.btRunStart.Size = new System.Drawing.Size(531, 96);
            this.btRunStart.TabIndex = 30;
            this.btRunStart.Text = "开始测试";
            this.btRunStart.UseVisualStyleBackColor = true;
            this.btRunStart.Click += new System.EventHandler(this.btRunStart_Click);
            // 
            // frmRun1DutGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 764);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ssRunStatus);
            this.Controls.Add(this.tsSumInfo);
            this.Controls.Add(this.toolComPortCfg);
            this.Controls.Add(this.tsToolButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRun1DutGui";
            this.Text = "测试信息";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRunDoc1_FormClosed);
            this.Load += new System.EventHandler(this.frmRunDoc1_Load);
            this.tsToolButton.ResumeLayout(false);
            this.tsToolButton.PerformLayout();
            this.toolComPortCfg.ResumeLayout(false);
            this.toolComPortCfg.PerformLayout();
            this.tsSumInfo.ResumeLayout(false);
            this.tsSumInfo.PerformLayout();
            this.ssRunStatus.ResumeLayout(false);
            this.ssRunStatus.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            this.splitContainer7.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.ResumeLayout(false);
            this.tcRunDut0.ResumeLayout(false);
            this.tpRunStatDut0.ResumeLayout(false);
            this.tpRunResDut0.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer frmRunDoc1TimerRecord;
        private System.Windows.Forms.ToolStrip tsToolButton;
        private System.Windows.Forms.ToolStripButton tsbRunCfg;
        private System.Windows.Forms.ToolStripButton tsbRunStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbRunStartCalibrateMain;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip toolComPortCfg;
        private System.Windows.Forms.ToolStripLabel tslDutComPort;
        
        private System.Windows.Forms.ToolStripLabel tslFixtureComPort;
        private System.Windows.Forms.ToolStripTextBox tsbFixtureComPort0;
        private System.Windows.Forms.ToolStripButton tsbRunExit;


        public System.Windows.Forms.ToolStripTextBox tsbDutPortMode0;
        private System.Windows.Forms.ToolStripButton tsbRunHelp;
        private System.Windows.Forms.ToolStripComboBox tscbRunMode;
        private System.Windows.Forms.ToolStrip tsSumInfo;
        private System.Windows.Forms.ToolStripLabel tslDutPassNumPrompt;
        private System.Windows.Forms.ToolStripTextBox tstbDutPassNum;
        private System.Windows.Forms.ToolStripLabel tslDutFailNumPrompt;
        private System.Windows.Forms.ToolStripTextBox tstbDutFailNum;
        private System.Windows.Forms.ToolStripLabel tslDutFailRatePrompt;
        private System.Windows.Forms.ToolStripTextBox tstbDutFailRate;
        public System.Windows.Forms.ToolStripTextBox tsbDutPortCom0;
        public System.Windows.Forms.ToolStripTextBox tsbDutPortCom1;
        private System.Windows.Forms.StatusStrip ssRunStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsslRunStatus;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Label labelTimeCost;
        private System.Windows.Forms.Label labelSN;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer5;
        public System.Windows.Forms.Label labelTestStatusDut0;
        private System.Windows.Forms.TabControl tcRunDut0;
        private System.Windows.Forms.TabPage tpRunStatDut0;
        private System.Windows.Forms.RichTextBox rtbRunItemDut0;
        private System.Windows.Forms.TabPage tpRunResDut0;
        private System.Windows.Forms.Button btRunStart;
        private System.Windows.Forms.RichTextBox rtbRunLogDut0;
        private System.IO.Ports.SerialPort serialPort1;






    }
}