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
    public partial class frmMsgPrompt : Form
    {
        public int iMsgMode = 0;
        public int iGuiWidth = 0;
        public int iGuiLeft = 0;
        public string strMsgPrompt = "";
        public string strMsgTitle = "";
        public frmMsgPrompt()
        {
            InitializeComponent();
        }
        
        private void btOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMsgPrompt_Load(object sender, EventArgs e)
        {
            this.Width = iGuiWidth - 40;
            this.Left = iGuiLeft + 20;
        }

        private void frmMsgPrompt_Shown(object sender, EventArgs e)
        {
            tbMsgPrompt.Text = strMsgPrompt;
        }
    }
}
