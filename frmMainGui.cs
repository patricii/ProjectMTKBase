using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Collections.Specialized;
using Microsoft.Win32;
using System.Security.Permissions;

using WeifenLuo.WinFormsUI.Docking;

namespace ateRun
{
    public partial class frmMainGui : Form
    {
        public DateTime g_dtCurrentRunBegin;
        public DateTime g_dtCurrentRunEnd;
        public TimeSpan g_tsRunBegin;
        public TimeSpan tsRunEnd;

        public const int USER = 0x0400;
        public const int WM_RUN_CFG = USER + 101;
        public const int WM_RUN_SUM = USER + 102;
        public const int WM_RUN_LOG = USER + 103;
        public const int WM_RUN_CTRL = USER + 201;
        public const int WM_RUN_PROC = USER + 301;
        public const int WM_RUN_INFO = USER + 401;
        clMsgCtrl sMsgCtrl = new clMsgCtrl();

        clCfgCtrlRun sCfgCtrlRun = new clCfgCtrlRun();
        clCfgCtrlSet sCfgCtrlSet = new clCfgCtrlSet();


        //frmRunSum wlRunSum = new frmRunSum();
        frmRun1DutGui wlRunDoc11 = new frmRun1DutGui();  //第1个进程，每个1个线程


        const string STR_PATH_CFG_ALL = ".\\cfg\\mainCfgAll.xml";

        //创建NotifyIcon对象
        #region
        //NotifyIcon notifyicon = new NotifyIcon();
        ////创建托盘图标对象 
        //Icon ico = new Icon(".\\ico\\rs.ico");
        //创建托盘菜单对象 
        ContextMenu notifyContextMenu = new ContextMenu();
        #endregion

        public frmMainGui()
        {
            //sCfgCtrlSet.sRunAllInfo.strLoginDb ="0";
            //sCfgCtrlSet.sRunAllInfo.strConnectDb = "0";

            InitializeComponent();
        }

        private string[] args = null;

        public int frmMainGui_SaveSettings(string strName, string strValue)
        {
            int iStatus = 0;
            string strNamePath = "SOFTWARE\\Start\\" + strName;
            try
            {
                RegistryKey rsg = null;

                if (Registry.LocalMachine.OpenSubKey("SOFTWARE\\Start") == null)
                {
                    rsg = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Start");
                }
                rsg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Start", true);

                rsg.SetValue(strName, strValue);

                rsg.Close();
            }
            catch (Exception ex)
            {
                ;
            }
            return iStatus;
        }

        public int frmMainGui_FetchSettings(string strName, ref string strValue)
        {
            int iStatus = 0;
            try
            {
                RegistryKey rsg = null;
                rsg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Start", true);
                if (rsg == null)
                {
                    return -1;
                }
                else
                {
                    strValue = rsg.GetValue(strName).ToString();
                }
            }
            catch (Exception ex)
            {
                ;
            }
            return iStatus;
        }

        private void frmMainGui_ShowMsgPrompt(int iMsgMode, string strMsgTitle, string strMsgPrompt)
        {
            frmMsgPrompt wlMsgPrompt = new frmMsgPrompt();
            wlMsgPrompt.iGuiLeft = sCfgCtrlSet.sRunAllInfo.iGuiLeft;
            wlMsgPrompt.iGuiWidth = sCfgCtrlSet.sRunAllInfo.iGuiWidth;
            wlMsgPrompt.iMsgMode = iMsgMode;
            wlMsgPrompt.strMsgTitle = strMsgTitle;
            wlMsgPrompt.strMsgPrompt = strMsgPrompt;
            wlMsgPrompt.ShowDialog(this);
        }

        public frmMainGui(string[] args)
        {
            InitializeComponent();

            frmMainGui_FetchSettings("login_db", ref sCfgCtrlSet.sRunAllInfo.strLoginDb);
            frmMainGui_FetchSettings("db_connect", ref sCfgCtrlSet.sRunAllInfo.strConnectDb);

            if (sCfgCtrlSet.sRunAllInfo.strLoginDb.Length < 1)
            {
                sCfgCtrlSet.sRunAllInfo.strLoginDb = "0";
            }

            if (sCfgCtrlSet.sRunAllInfo.strConnectDb.Length < 1)
            {
                sCfgCtrlSet.sRunAllInfo.strConnectDb = "0";
            }

            if ((Int32.Parse(sCfgCtrlSet.sRunAllInfo.strLoginDb) == 1) && (Int32.Parse(sCfgCtrlSet.sRunAllInfo.strConnectDb) == 0))
            {
                frmMainGui_ShowMsgPrompt(-1, "错误提示", "未成功登陆数据库");
                this.Close();
            }

            frmMainGui_FetchSettings("db_account", ref sCfgCtrlSet.sRunAllInfo.strDbAccount);
            frmMainGui_FetchSettings("db_password", ref sCfgCtrlSet.sRunAllInfo.strDbPassword);
            frmMainGui_FetchSettings("db_name", ref sCfgCtrlSet.sRunAllInfo.strDbName);
            frmMainGui_FetchSettings("db_rank", ref sCfgCtrlSet.sRunAllInfo.strDbRank);
            frmMainGui_FetchSettings("db_database_name", ref sCfgCtrlSet.sRunAllInfo.strDbDatabaseName);
            frmMainGui_FetchSettings("db_group", ref sCfgCtrlSet.sRunAllInfo.strDbGroup);
            frmMainGui_FetchSettings("db_pc_name", ref sCfgCtrlSet.sRunAllInfo.strDbPcName);
            frmMainGui_FetchSettings("db_ms_type", ref sCfgCtrlSet.sRunAllInfo.strDbMsType);
            frmMainGui_FetchSettings("db_ms_type_name", ref sCfgCtrlSet.sRunAllInfo.strDbMsTypeName);
            
            foreach (string strCmd in args)
            {
                string[] strarySubCmd = new string[10];
                strarySubCmd = strCmd.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string strSubCmd in strarySubCmd)
                {
                    string[] straryLineVal = new string[10];
                    string strName = "";
                    string strValue = "";
                    straryLineVal = strSubCmd.Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (straryLineVal.Length == 2)
                    {
                        strName = straryLineVal[0];
                        strValue = straryLineVal[1];
                        if (strName == "dut_all_group_num")
                        {
                            sCfgCtrlSet.sRunAllInfo.iAllDutGroupNum = Int32.Parse(strValue);
                        }
                        else if (strName == "dut_cur_group_index")
                        {
                            sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex = Int32.Parse(strValue);
                        }
                    }
                }
            }
        }

        private void CloseAllSubFrm()
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                {
                    form.Close();
                }
            }
            else
            {
                for (int index = dockPanel.Contents.Count - 1; index >= 0; index--)
                {
                    if (dockPanel.Contents[index] is IDockContent)
                    {
                        IDockContent content = (IDockContent)dockPanel.Contents[index];
                        //if ((content != wlRunSum))
                        {
                            content.DockHandler.Close();
                        }

                    }
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            int iStatus = 0;
            //this.ShowInTaskbar = true;
            //niMain.Visible = false;

            //string name = Process.GetCurrentProcess().ProcessName;

            //int id = Process.GetCurrentProcess().Id;
            //Process[] prcMain = Process.GetProcesses();
            //foreach (Process pr in prcMain)
            //{
            //    if ((name == pr.ProcessName) && (pr.Id != id))
            //    {
            //        //if (MessageBox.Show("对不起, 本地已经有系统正在运行, 是否关闭之前程序并继续!\n.", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            //        {
            //            pr.Kill();
            //        }
            //        //else
            //        //{
            //        //    System.Environment.Exit(0);
            //        //}
            //    }
            //}            

            //string strEngineName = "Engine";
            StringBuilder sbRetCmdMsg = new StringBuilder(40960);
            StringBuilder sbRetCmdBuf = new StringBuilder(40960);

            //int iEngineId = Process.GetCurrentProcess().Id;
            //Process[] prcEngine = Process.GetProcesses();
            //foreach (Process pr in prcEngine)
            //{
            //    if (((("Engine" == pr.ProcessName) && (pr.Id != id)))||((("Engine_d" == pr.ProcessName) && (pr.Id != id))))
            //    {
            //        pr.Kill();
            //    }
            //}
            if (File.Exists(".\\station.ini")) // Patricio
            {
                StringBuilder strErrorMessage = new StringBuilder(256);

                strErrorMessage.Insert(0, "Fail to initialize BZ Equipments");

                Bz_Handler.CJagLocalFucntions.SetFactory("BZ");
                if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ")
                {
                    Bz_Handler.CItemListEquip.LoadBZConfig();
                    iStatus = Bz_Handler.CItemListEquip.InitItemListEquip();

                    if (iStatus == 0)
                    {
                        Bz_Handler.CJagLocalFucntions.SetBzScanMode("NO_SCAN");
                        frmBzModel BzModelForm = new frmBzModel();
                        BzModelForm.ShowDialog(this);
                    }

                    if (iStatus == 0)
                        iStatus = Bz_Handler.CJagLocalFucntions.EntryHandlerSystem();
                   

                    if (iStatus == 0)  // Carregando as perdas do calinfo.cfg no arquivo .cfg MTK!!! PATRICIO
                    {
                        iStatus = Bz_Handler.CItemListEquip.UpdateCableLossDataOnMTKConfigFile_SUMO();

                        if (iStatus != 0)
                        {
                            MessageBox.Show("Update MTK Cable Loss FAIL", "CABLE LOSS");
                        }
                    }

                    if (iStatus == 0)
                    {
                        string strBzToolVersion;
                        strBzToolVersion = Bz_Handler.CJagLocalFucntions.GetBzToolVersion();
                        Bz_Handler.CJagLocalFucntions.SetToolVersion(strBzToolVersion);
                    }

  
                    if (iStatus != 0)
                    {
                        MessageBox.Show("Fail to initialize BZ Equipments");
                        this.Close();
                        return;
                    }
                 }

            }
            sCfgCtrlSet.strMainCfgFile = STR_PATH_CFG_ALL;
            iStatus = sCfgCtrlRun.LoadAllCfgFile(sCfgCtrlSet);
            if (iStatus != 0)
            {
                this.Close();
                return;
            }           

            Rectangle rect = System.Windows.Forms.SystemInformation.VirtualScreen;
            int iWidth = rect.Width;
            int iHeight = rect.Height;
            int iPosX = 0;
            int iPosY = 0;
            if (sCfgCtrlSet.sRunAllInfo.iAllDutGroupNum == 1)
            {
                if (sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 0)
                {
                    iPosX = 0;
                    iPosY = 0;
                    this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(iPosX, iPosY);
                    this.Width = rect.Width;
                    this.Height = rect.Height - 40;
                }
            }
            else if (sCfgCtrlSet.sRunAllInfo.iAllDutGroupNum == 2)
            {
                if (sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 0)
                {
                    iPosX = 0;
                    iPosY = 0;
                    this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(iPosX, iPosY);
                    this.Width = rect.Width / 2;
                    this.Height = rect.Height - 40;
                }
                else if (sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 1)
                {
                    iPosX = rect.Width / 2;
                    iPosY = 0;
                    this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(iPosX, iPosY);
                    this.Width = rect.Width / 2;
                    this.Height = rect.Height - 40;
                }
            }
            else if (sCfgCtrlSet.sRunAllInfo.iAllDutGroupNum == 3)
            {
                if (sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 0)
                {
                    iPosX = 0;
                    iPosY = 0;
                    this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(iPosX, iPosY);
                    this.Width = rect.Width / 3;
                    this.Height = rect.Height - 40;
                }
                else if (sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 1)
                {
                    iPosX = rect.Width / 3;
                    iPosY = 0;
                    this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(iPosX, iPosY);
                    this.Width = rect.Width / 3;
                    this.Height = rect.Height - 40;
                }
                else if (sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 2)
                {
                    iPosX = rect.Width * 2 / 3;
                    iPosY = 0;
                    this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(iPosX, iPosY);
                    this.Width = rect.Width / 3;
                    this.Height = rect.Height - 40;
                }
            }
            else if (sCfgCtrlSet.sRunAllInfo.iAllDutGroupNum == 4)
            {
                if (sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 0) //  Patricio 2UP Screen
                {
                    StringBuilder str = new StringBuilder(6);
                    Bz_Handler.CItemListEquip.GetI2cSide(str);
                    string strI2CSide = str.ToString();

                    if (strI2CSide == "LEFT")
                    {
                        iPosX = 0;
                        iPosY = 0;
                        this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                        this.Location = new System.Drawing.Point(iPosX, iPosY);
                        this.Width = rect.Width / 2;
                        this.Height = rect.Height - 40;

                    }
                    else
                    {
                        iPosX = rect.Width / 2;
                        iPosY = 0;
                        this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                        this.Location = new System.Drawing.Point(iPosX, iPosY);
                        this.Width = rect.Width / 2;
                        this.Height = rect.Height - 40;
                    }
                  }
                else if (sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 1)
                {
                    iPosX = rect.Width / 4;
                    iPosY = 0;
                    this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(iPosX, iPosY);
                    this.Width = rect.Width / 4;
                    this.Height = rect.Height - 40;
                }
                else if (sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 2)
                {
                    iPosX = rect.Width * 2 / 4;
                    iPosY = 0;
                    this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(iPosX, iPosY);
                    this.Width = rect.Width / 4;
                    this.Height = rect.Height - 40;
                }
                else if (sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 3)
                {
                    iPosX = rect.Width * 3 / 4;
                    iPosY = 0;
                    this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(iPosX, iPosY);
                    this.Width = rect.Width / 4;
                    this.Height = rect.Height - 40;
                }
            }

            //frmLogin wlLogin = new frmLogin();
            //wlLogin.sCfgCtrlSet = sCfgCtrlSet;
            //wlLogin.ShowDialog(this);
            //if (wlLogin.bLoginOK == true)
            //{

            //}

            CloseAllSubFrm();
            //wlRunSum.sCfgCtrlSet = sCfgCtrlSet;
            //wlRunSum.Show(dockPanel, DockState.Document);
            //wlRunSum.Text = "统计信息";         
            
            string strExeCaption = "";
            
            wlRunDoc11.g_sCurCfgCtrlSet = sCfgCtrlSet;

            sCfgCtrlSet.sRunAllInfo.iCurAllItemNum = sCfgCtrlSet.sRunAllInfo.iStepNumCal + sCfgCtrlSet.sRunAllInfo.iStepNumVerify;

            frmMain_UpdateDoc();
            if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ") // Patricio
            {
                StringBuilder strbenchID = new StringBuilder(16);
                Bz_Handler.CItemListEquip.GetStationID(strbenchID);
                LabelBZBench.Text = strbenchID.ToString();

            }

            if (sCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
            {
                this.Text = "Automatic cal&ver tool";
                this.tsmiSet.Text = "Set";
                this.tsmiSetLogin.Text = "Change login operator";
                this.tsmiSetExit.Text = "Exit the tool";
                this.tsmiHelp.Text = "Ajuda";
                this.tsmiHelpUserManual.Text = "User manual";
                this.tsmiHelpAbout.Text = "About the tool";
                this.tsmiLanguage.Text = "Idioma";
                this.tsmiLanguageChs.Checked = false;
                this.tsmiLanguageEng.Checked = true;
                this.tslDutNum.Text = string.Format("all: {0}", sCfgCtrlSet.sRunAllInfo.iAllDutGroupNum);
                this.tslDutIndex.Text = string.Format("index: {0}", sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex);
                strExeCaption = string.Format("WWTE Cal & Ver [{0}.{1}]", sCfgCtrlSet.sRunAllInfo.strToolVersion, sCfgCtrlSet.sRunAllInfo.strAteVersion);
                if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ")  // Patricio
                {
                    this.Text = Bz_Handler.CJagLocalFucntions.GetBzToolVersion();
                }
            }
            else
            {
                this.Text = "自动化测试软件";
                this.tsmiSet.Text = "设置";
                this.tsmiSetLogin.Text = "更改用户";                
                this.tsmiSetExit.Text = "退出程序";
                this.tsmiHelp.Text = "帮助";
                this.tsmiHelpUserManual.Text = "使用手册";
                this.tsmiHelpAbout.Text = "关于软件";
                this.tsmiLanguage.Text = "语言(Language)";
                this.tsmiLanguageChs.Checked = true;
                this.tsmiLanguageEng.Checked = false;
                this.tslDutNum.Text = string.Format("总数: {0}", sCfgCtrlSet.sRunAllInfo.iAllDutGroupNum);
                this.tslDutIndex.Text = string.Format("序号: {0}", sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex);
                strExeCaption = string.Format("WWTE预测软件[{0}.{1}]", sCfgCtrlSet.sRunAllInfo.strToolVersion, sCfgCtrlSet.sRunAllInfo.strAteVersion);
                
            }           

            this.Text = strExeCaption;

        }

        private void tsmiSetExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_UpdateDoc()
        {
            if (sCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
            {
                wlRunDoc11.Text = "test info";
            }
            else
            {
                wlRunDoc11.Text = "测试信息";
            }
            wlRunDoc11.Show(dockPanel, DockState.Document);

            sCfgCtrlSet.sRunAllInfo.iGuiWidth = this.Width;
            sCfgCtrlSet.sRunAllInfo.iGuiLeft = this.Left;

            dockPanel.ResumeLayout(true, true);
        }


        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            ////判断是否选择的是最小化按钮 
            //if (WindowState == FormWindowState.Minimized)
            //{
            //    //托盘显示图标等于托盘图标对象 
            //    //注意notifyIcon1是控件的名字而不是对象的名字 
            //    //notifyicon.Icon = ico;
            //    //隐藏任务栏区图标 
            //    this.ShowInTaskbar = false;
            //    //图标显示在托盘区 
            //    niMain.Visible = true;
            //}
        }

        private void niMain_DoubleClick(object sender, EventArgs e)
        {
            ////判断是否已经最小化于托盘 
            //if (WindowState == FormWindowState.Minimized)
            //{
            //    //还原窗体显示 
            //    WindowState = FormWindowState.Normal;
            //    //激活窗体并给予它焦点 
            //    this.Activate();
            //    //任务栏区显示图标 
            //    this.ShowInTaskbar = true;
            //    //托盘区图标隐藏 
            //    niMain.Visible = false;
            //}
        }

        private void tsmiNiMainMax_Click(object sender, EventArgs e)
        {
            ////还原窗体显示 
            //WindowState = FormWindowState.Maximized;
            ////激活窗体并给予它焦点 
            //this.Activate();
            ////任务栏区显示图标 
            //this.ShowInTaskbar = true;
            ////托盘区图标隐藏 
            //niMain.Visible = false;
            //dockPanel.Visible = true;
        }

        private void tsmiNiMainExit_Click(object sender, EventArgs e)
        {
            //tsmiSetExit_Click(sender, e);
        }

        private void tsmiOptSettingOption_Click(object sender, EventArgs e)
        {

        }

        private void tsbRunCfg_Click(object sender, EventArgs e)
        {
            //frmMainCfg wlMainCfg = new frmMainCfg();
            //wlMainCfg.strIntelLogFolder = sCfgCtrlSet.sRunAllInfo.strIntelLogFolder;
            //wlMainCfg.strIntelTestItemCalMain = sCfgCtrlSet.sRunAllInfo.strIntelTestItemCalMain;
            //wlMainCfg.ShowDialog(this);

            //if (wlMainCfg.bMainCfgOK == true)
            //{
            //    ;
            //}
        }

        private void tsmiHelpAboutSw_Click(object sender, EventArgs e)
        {
            frmAbout wlAbout = new frmAbout();
            wlAbout.iLanguageOpt = sCfgCtrlSet.sRunAllInfo.iLanguageOpt;
            wlAbout.strToolVersion = string.Format("{0}.{1}", sCfgCtrlSet.sRunAllInfo.strToolVersion, sCfgCtrlSet.sRunAllInfo.strAteVersion); 
            wlAbout.ShowDialog(this);
        }

        private void frmMainGui_FormClosed(object sender, FormClosedEventArgs e)
        {
            //int iStatus = 0;
            //iStatus = sCfgCtrlRun.SaveMainCfgXml(sCfgCtrlSet);
            //if (iStatus != 0)
            //{
            //    return;
            //}
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {

        }

        private void tsmiOpenCfg_Click(object sender, EventArgs e)
        {

        }

        private void tsmiUserLogin_Click(object sender, EventArgs e)
        {
            frmLogin wlLogin = new frmLogin();
            wlLogin.sCfgCtrlSet = sCfgCtrlSet;
            //wlLogin.iGuiWidth = sCfgCtrlSet.sRunAllInfo.iGuiWidth;
            //wlLogin.iGuiLeft = sCfgCtrlSet.sRunAllInfo.iGuiLeft;
            wlLogin.ShowDialog(this);
            if (wlLogin.bLoginOK == true)
            {
                if (wlLogin.iUserType == 0)
                {
                    sMsgCtrl.SendMsg(wlRunDoc11.Handle, WM_RUN_CTRL, "run_ctrl", "operator");
                }
                else if (wlLogin.iUserType == 1)
                {
                    sMsgCtrl.SendMsg(wlRunDoc11.Handle, WM_RUN_CTRL, "run_ctrl", "engineer");
                }
                else if (wlLogin.iUserType == 2)
                {
                    sMsgCtrl.SendMsg(wlRunDoc11.Handle, WM_RUN_CTRL, "run_ctrl", "developer");
                }
           }
        }

        private void tsmiHelpUserDocs_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("..\\doc\\WWTE MTK平台校准&非信令测试软件操作手册.pdf");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "打开帮助文件失败");
            }
        }

        private void tsmiLanguageChs_Click(object sender, EventArgs e)
        {
            sCfgCtrlSet.sRunAllInfo.iLanguageOpt = 2;
            MessageBox.Show("已经设置为中文，需要重启软件之后，进行语言切换。", "提示");
        }

        private void tsmiLanguageEng_Click(object sender, EventArgs e)
        {
            sCfgCtrlSet.sRunAllInfo.iLanguageOpt = 1;
            MessageBox.Show("Aready set to english version, still need the restart the tool to change.", "Prompt");
        }

    }
}
