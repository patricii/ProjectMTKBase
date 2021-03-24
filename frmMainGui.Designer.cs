namespace ateRun
{
    partial class frmMainGui
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainGui));
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin2 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient8 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient9 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient5 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient10 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient11 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient12 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient6 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient13 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient14 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelpUserManual = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSetLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.niMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.msFrmMain = new System.Windows.Forms.MenuStrip();
            this.tsmiSet = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSetExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLanguageChs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLanguageEng = new System.Windows.Forms.ToolStripMenuItem();
            this.tsTool = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tslDutNum = new System.Windows.Forms.ToolStripLabel();
            this.tslDutIndex = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.msFrmMain.SuspendLayout();
            this.tsTool.SuspendLayout();
            this.SuspendLayout();
            this.LabelBZBench = new System.Windows.Forms.ToolStripLabel(); // Patricio
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHelpAbout,
            this.tsmiHelpUserManual});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(44, 21);
            this.tsmiHelp.Text = "帮助";
            // 
            // tsmiHelpAbout
            // 
            this.tsmiHelpAbout.Name = "tsmiHelpAbout";
            this.tsmiHelpAbout.Size = new System.Drawing.Size(124, 22);
            this.tsmiHelpAbout.Text = "关于软件";
            this.tsmiHelpAbout.Click += new System.EventHandler(this.tsmiHelpAboutSw_Click);
            // 
            // tsmiHelpUserManual
            // 
            this.tsmiHelpUserManual.Name = "tsmiHelpUserManual";
            this.tsmiHelpUserManual.Size = new System.Drawing.Size(124, 22);
            this.tsmiHelpUserManual.Text = "使用手册";
            // 
            // tsmiSetLogin
            // 
            this.tsmiSetLogin.Name = "tsmiSetLogin";
            this.tsmiSetLogin.Size = new System.Drawing.Size(124, 22);
            this.tsmiSetLogin.Text = "更改用户";
            this.tsmiSetLogin.Click += new System.EventHandler(this.tsmiUserLogin_Click);
            // 
            // niMain
            // 
            this.niMain.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.niMain.Icon = ((System.Drawing.Icon)(resources.GetObject("niMain.Icon")));
            this.niMain.Text = "niMain";
            this.niMain.DoubleClick += new System.EventHandler(this.niMain_DoubleClick);
            // 
            // msFrmMain
            // 
            this.msFrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSet,
            this.tsmiHelp,
            this.tsmiLanguage});
            this.msFrmMain.Location = new System.Drawing.Point(0, 0);
            this.msFrmMain.Name = "msFrmMain";
            this.msFrmMain.Size = new System.Drawing.Size(639, 25);
            this.msFrmMain.TabIndex = 33;
            this.msFrmMain.Text = "menuStrip1";
            // 
            // tsmiSet
            // 
            this.tsmiSet.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSetLogin,
            this.tsmiSetExit});
            this.tsmiSet.Name = "tsmiSet";
            this.tsmiSet.Size = new System.Drawing.Size(44, 21);
            this.tsmiSet.Text = "设置";
            // 
            // tsmiSetExit
            // 
            this.tsmiSetExit.Name = "tsmiSetExit";
            this.tsmiSetExit.Size = new System.Drawing.Size(124, 22);
            this.tsmiSetExit.Text = "退出程序";
            this.tsmiSetExit.Click += new System.EventHandler(this.tsmiSetExit_Click);
            // 
            // tsmiLanguage
            // 
            this.tsmiLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiLanguageChs,
            this.tsmiLanguageEng});
            this.tsmiLanguage.Name = "tsmiLanguage";
            this.tsmiLanguage.Size = new System.Drawing.Size(109, 21);
            this.tsmiLanguage.Text = "语言(Language)";
            // 
            // tsmiLanguageChs
            // 
            this.tsmiLanguageChs.Name = "tsmiLanguageChs";
            this.tsmiLanguageChs.Size = new System.Drawing.Size(152, 22);
            this.tsmiLanguageChs.Text = "中文";
            this.tsmiLanguageChs.Click += new System.EventHandler(this.tsmiLanguageChs_Click);
            // 
            // tsmiLanguageEng
            // 
            this.tsmiLanguageEng.Name = "tsmiLanguageEng";
            this.tsmiLanguageEng.Size = new System.Drawing.Size(152, 22);
            this.tsmiLanguageEng.Text = "English";
            this.tsmiLanguageEng.Click += new System.EventHandler(this.tsmiLanguageEng_Click);
            // 
            // tsTool
            // 
            this.tsTool.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tslDutNum,
            this.tslDutIndex,
            this.toolStripSeparator1,
            this.LabelBZBench}); // Patricio
            this.tsTool.Location = new System.Drawing.Point(0, 25);
            this.tsTool.Name = "tsTool";
            this.tsTool.Size = new System.Drawing.Size(639, 27);
            this.tsTool.TabIndex = 36;
            this.tsTool.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(0, 24);
            // 
            // tslDutNum
            // 
            this.tslDutNum.BackColor = System.Drawing.SystemColors.MenuText;
            this.tslDutNum.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.tslDutNum.ForeColor = System.Drawing.Color.Blue;
            this.tslDutNum.Name = "tslDutNum";
            this.tslDutNum.Size = new System.Drawing.Size(81, 24);
            this.tslDutNum.Text = "总数: 1";
            // 
            // tslDutIndex
            // 
            this.tslDutIndex.BackColor = System.Drawing.SystemColors.MenuText;
            this.tslDutIndex.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslDutIndex.ForeColor = System.Drawing.Color.Blue;
            this.tslDutIndex.Name = "tslDutIndex";
            this.tslDutIndex.Size = new System.Drawing.Size(81, 24);
            this.tslDutIndex.Text = "序号: 1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dockPanel.Location = new System.Drawing.Point(0, 52);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(639, 351);
            dockPanelGradient4.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient4.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin2.DockStripGradient = dockPanelGradient4;
            tabGradient8.EndColor = System.Drawing.SystemColors.Control;
            tabGradient8.StartColor = System.Drawing.SystemColors.Control;
            tabGradient8.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin2.TabGradient = tabGradient8;
            autoHideStripSkin2.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            dockPanelSkin2.AutoHideStripSkin = autoHideStripSkin2;
            tabGradient9.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient9.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient9.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient2.ActiveTabGradient = tabGradient9;
            dockPanelGradient5.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient5.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient2.DockStripGradient = dockPanelGradient5;
            tabGradient10.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient10.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient10.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient2.InactiveTabGradient = tabGradient10;
            dockPaneStripSkin2.DocumentGradient = dockPaneStripGradient2;
            dockPaneStripSkin2.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            tabGradient11.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient11.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient11.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient11.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient2.ActiveCaptionGradient = tabGradient11;
            tabGradient12.EndColor = System.Drawing.SystemColors.Control;
            tabGradient12.StartColor = System.Drawing.SystemColors.Control;
            tabGradient12.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient2.ActiveTabGradient = tabGradient12;
            dockPanelGradient6.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient6.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient2.DockStripGradient = dockPanelGradient6;
            tabGradient13.EndColor = System.Drawing.SystemColors.InactiveCaption;
            tabGradient13.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient13.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient13.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
            dockPaneStripToolWindowGradient2.InactiveCaptionGradient = tabGradient13;
            tabGradient14.EndColor = System.Drawing.Color.Transparent;
            tabGradient14.StartColor = System.Drawing.Color.Transparent;
            tabGradient14.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient2.InactiveTabGradient = tabGradient14;
            dockPaneStripSkin2.ToolWindowGradient = dockPaneStripToolWindowGradient2;
            dockPanelSkin2.DockPaneStripSkin = dockPaneStripSkin2;
            this.dockPanel.Skin = dockPanelSkin2;
            this.dockPanel.TabIndex = 38;
            // 
            // LabelBZBench // Patricio
            // 
            this.LabelBZBench.BackColor = System.Drawing.SystemColors.Control;
            this.LabelBZBench.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.LabelBZBench.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.LabelBZBench.ForeColor = System.Drawing.Color.Blue;
            this.LabelBZBench.Name = "LabelBZBench";
            this.LabelBZBench.Size = new System.Drawing.Size(0, 24);
            // 
            // frmMainGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 403);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.tsTool);
            this.Controls.Add(this.msFrmMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "frmMainGui";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WWTE预测软件: 校准+非信令测试软件";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMainGui_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.SizeChanged += new System.EventHandler(this.frmMain_SizeChanged);
            this.msFrmMain.ResumeLayout(false);
            this.msFrmMain.PerformLayout();
            this.tsTool.ResumeLayout(false);
            this.tsTool.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem tsmiSet;
        private System.Windows.Forms.ToolStripMenuItem tsmiSetExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiSetLogin;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelpUserManual;
        private System.Windows.Forms.NotifyIcon niMain;
        private System.Windows.Forms.MenuStrip msFrmMain;
        
        private System.Windows.Forms.ToolStripMenuItem tsmiLanguage;
        private System.Windows.Forms.ToolStripMenuItem tsmiLanguageChs;
        private System.Windows.Forms.ToolStripMenuItem tsmiLanguageEng;
        private System.Windows.Forms.ToolStrip tsTool;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel tslDutIndex;
        private System.Windows.Forms.ToolStripLabel tslDutNum;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel LabelBZBench; // Patricio
    }
}

