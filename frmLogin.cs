using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ateRun
{
    public partial class frmLogin : Form
    {
        public clCfgCtrlSet sCfgCtrlSet = new clCfgCtrlSet();
        public int iUserType = 0;
        public string strDutType;
        public string strUserType;
        public string strUserName;
        private string strPassWord;
        private string strDiscription;
        //public int iGuiWidth = 0;
        //public int iGuiLeft = 0;

        public bool bLoginOK = false;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btEditCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogin_Shown(object sender, EventArgs e)
        {
            cbUserType.SelectedIndex = 0;
            cbUserName.SelectedIndex = 0;
        }

        private void frmLogin_ShowMsgPrompt(int iMsgMode, string strMsgTitle, string strMsgPrompt)
        {
            frmMsgPrompt wlMsgPrompt = new frmMsgPrompt();
            wlMsgPrompt.iGuiLeft = sCfgCtrlSet.sRunAllInfo.iGuiLeft;
            wlMsgPrompt.iGuiWidth = sCfgCtrlSet.sRunAllInfo.iGuiWidth;
            wlMsgPrompt.iMsgMode = iMsgMode;
            wlMsgPrompt.strMsgTitle = strMsgTitle;
            wlMsgPrompt.strMsgPrompt = strMsgPrompt;
            wlMsgPrompt.ShowDialog(this);
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            strUserType = cbUserType.Text;
            strUserName = cbUserName.Text;
            strPassWord = tbPassWord.Text;
            strDiscription = tbDisc.Text;
            iUserType = cbUserType.SelectedIndex;
            if (sCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
            {
                switch (cbUserType.SelectedIndex)
                {
                    case 0:
                        frmLogin_ShowMsgPrompt(0, "Welcome", "“Operator”access sucessfully.");
                        bLoginOK = true;
                        this.Close();
                        break;
                    case 1:
                        if ((strUserName == "Oni") && (strPassWord == "123"))
                        {
                            frmLogin_ShowMsgPrompt(0, "Welcome", "“Operator”access sucessfully.");
                            bLoginOK = true;
                            this.Close();
                        }
                        else
                        {
                            frmLogin_ShowMsgPrompt(-1, "Welcome", "“Engineer”access failure.");
                            bLoginOK = false;
                        }
                        break;
                    case 2:
                        if ((strUserName == "Dragon") && (strPassWord == "123456"))
                        {
                            frmLogin_ShowMsgPrompt(-1, "Welcome", "“Developer”access sucessfully.");
                            bLoginOK = true;
                            this.Close();
                        }
                        else
                        {
                            frmLogin_ShowMsgPrompt(-1, "Welcome", "“Developer”access failure.");
                            bLoginOK = false;
                        }
                        break;
                }
            }
            else
            {
                switch (cbUserType.SelectedIndex)
                {
                    case 0:
                        frmLogin_ShowMsgPrompt(0, "欢迎访问本系统", "“操作员”身份登录成功");
                        bLoginOK = true;
                        this.Close();
                        break;
                    case 1:
                        if ((strUserName == "李明")  && (strPassWord == "123"))
                        {
                            frmLogin_ShowMsgPrompt(0, "欢迎访问本系统", "“产线管理员”身份登录成功");
                            bLoginOK = true;
                            this.Close();
                        }
                        else
                        {
                            frmLogin_ShowMsgPrompt(-1, "欢迎访问本系统", "“产线管理员”身份登录失败");
                            bLoginOK = false;
                        }
                        break;
                    case 2:
                        if ((strUserName == "杨龙") && (strPassWord == "123456"))
                        {
                            frmLogin_ShowMsgPrompt(-1, "欢迎访问本系统", "“工具开发员”身份登录成功");
                            bLoginOK = true;
                            this.Close();
                        }
                        else
                        {
                            frmLogin_ShowMsgPrompt(-1, "欢迎访问本系统", "“工具开发员”身份登录失败");
                            bLoginOK = false;
                        }
                        break;
                }
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
            {
                switch (cbUserType.SelectedIndex)
                {
                    case 0:
                        tbDisc.Text = "Support the basic test function.";
                        tbPassWord.Enabled = false;
                        break;
                    case 1:
                        tbDisc.Text = "Support more functions to configure the system.";
                        tbPassWord.Enabled = true;
                        break;
                    case 2:
                        tbDisc.Text = "Support debug functions to development.";
                        tbPassWord.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (cbUserType.SelectedIndex)
                {
                    case 0:
                        tbDisc.Text = "该账户只能进行测试的最基本的设置和操作。";
                        tbPassWord.Enabled = false;
                        break;
                    case 1:
                        tbDisc.Text = "该账户能进行测试的产线管理的设置和操作。";
                        tbPassWord.Enabled = true;
                        break;
                    case 2:
                        tbDisc.Text = "该账户能进行测试的最大权限的设置和操作。";
                        tbPassWord.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (sCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
            {
                labelUserType.Text = "User Type:";
                labelUserName.Text = "User Name:";
                labelPassWord.Text = "User Key:";
                labelUserDisc.Text = "Discription:";
                cbUserName.Items.Clear();
                cbUserName.Items.Add("JingJing");
                cbUserName.Items.Add("Oni");
                cbUserName.Items.Add("Dragon");
                cbUserName.SelectedIndex = 0;
                cbUserType.Items.Clear();
                cbUserType.Items.Add("Operator");
                cbUserType.Items.Add("engineer");
                cbUserType.Items.Add("Developer");
                cbUserType.SelectedIndex = 0;
                this.Text = "Login";
                btOK.Text = "OK";
                btCancel.Text = "Cancel";
            }
            else
            {
                labelUserType.Text = "用户类型:";
                labelUserName.Text = "用户名称:";
                labelPassWord.Text = "登陆密码:";
                labelUserDisc.Text = "说    明:";
                cbUserName.Items.Clear();
                cbUserName.Items.Add("王晶");
                cbUserName.Items.Add("李明");
                cbUserName.Items.Add("杨龙");
                cbUserName.SelectedIndex = 0;
                cbUserType.Items.Clear();
                cbUserType.Items.Add("操作员");
                cbUserType.Items.Add("产线管理员");
                cbUserType.Items.Add("工具开发员");
                cbUserType.SelectedIndex = 0;
                this.Text = "登陆";
                btOK.Text = "确定";
                btCancel.Text = "取消";
            }



            this.Width = sCfgCtrlSet.sRunAllInfo.iGuiWidth - 40;
            this.Left = sCfgCtrlSet.sRunAllInfo.iGuiLeft + 20;
            cbUserType.Focus();
        }
    }
}
