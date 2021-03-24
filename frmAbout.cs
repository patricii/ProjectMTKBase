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
    public partial class frmAbout : Form
    {
        public string strToolVersion = "";
        public int iLanguageOpt = 1;
        public frmAbout()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            if (iLanguageOpt == 1) //English
            {
                labelSwVersion.Text = "SW Version:";
                labelWebSite.Text = "Website:";
                labelSwDisc.Text = "SW Discription:";
                tbSwDisc.Text = "This is a tool for MTK chipset calibration and verification.";
            }
            else   
            {
                labelSwVersion.Text = "软件版本:";
                labelWebSite.Text = "公司网址:";
                labelSwDisc.Text = "软件描述:";
                tbSwDisc.Text = "这是进行非信令校准和验证的程序。";
            }

            tbSwVersion.Text = strToolVersion;
        }
    }
}
