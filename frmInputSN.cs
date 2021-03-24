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
    public partial class frmInputSN : Form
    {
        public frmInputSN()
        {
            InitializeComponent();
        }
        public int iCurDutGroupIndex = 0;
        public bool bInputSnOK = false;
        private int iRetryNum = 3;
        private int iRetryIndex = 0;
        public string str1stInputSN = "";
        public string str2ndInputSN = "";
        public int iLanguageOpt = 1;
        private void tb1stInputSN_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void tb1stInputSN_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void tb2ndInputSN_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void frmInputSN_Load(object sender, EventArgs e)
        {
            iRetryIndex = 0;
            iRetryNum = 3;
            bInputSnOK = false;
            if (iLanguageOpt == 1) //English
            {
                switch (iCurDutGroupIndex)
                {
                    case 0:
                        labelSetPromptMsg.Text = "Please set 1st dut parameters";
                        break;
                    case 1:
                        labelSetPromptMsg.Text = "Please set 2nd dut parameters";
                        break;
                    case 2:
                        labelSetPromptMsg.Text = "Please set 3rd dut parameters";
                        break;
                    case 3:
                        labelSetPromptMsg.Text = "Please set 4th dut parameters";
                        break;
                }
            }
            else
            {
                switch (iCurDutGroupIndex)
                {
                    case 0:
                        labelSetPromptMsg.Text = "请输入第一个手机SN号码";
                        break;
                    case 1:
                        labelSetPromptMsg.Text = "请输入第二个手机SN号码";
                        break;
                    case 2:
                        labelSetPromptMsg.Text = "请输入第三个手机SN号码";
                        break;
                    case 3:
                        labelSetPromptMsg.Text = "请输入第四个手机SN号码";
                        break;
                }
            }

            tb1stInputSN.Text = "";
            tb2ndInputSN.Text = "";
            tb1stInputSN.Focus();
        }

        private void tb1stInputSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                str1stInputSN = tb1stInputSN.Text;
                if (str1stInputSN.Length == 23)
                {
                    tb2ndInputSN.Text = "";
                    tb2ndInputSN.Focus();
                }
                else
                {
                    bInputSnOK = false;
                    return;
                }
            }
        }

        private void tb2ndInputSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                str1stInputSN = tb1stInputSN.Text;
                str2ndInputSN = tb2ndInputSN.Text;
                if ((str2ndInputSN == str1stInputSN)
                    && (str1stInputSN.Length == 23)
                        && (str2ndInputSN.Length == 23))
                {
                    bInputSnOK = true;
                    this.Close();
                }
                else
                {
                    tb1stInputSN.Text = "";
                    tb1stInputSN.Focus();
                    bInputSnOK = false;
                }
            }
        }
    }
}
