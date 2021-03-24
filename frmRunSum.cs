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
    public partial class frmRunSum : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public clCfgCtrlSet sCfgCtrlSet = new clCfgCtrlSet();
        public frmRunSum()
        {
            InitializeComponent();
        }
        public int frmRunSum_UpdateSumInfo(string strMsgName, string strMsgVal)
        {
            int iStatus = 0;
            //int iListItemIndex = 0;
            //ListViewItem lvItem;
            //ListViewItem.ListViewSubItem lvSubItem;

            //if (strMsgName.Contains("run_summary"))
            //{
            //    string[] strSumName = strMsgName.Split('=');
            //    string[] strSumVal = strMsgVal.Split('=');
            //    if (strSumVal[0].Contains("total_run"))
            //    {
            //        for (iListItemIndex = 0; iListItemIndex < lvTotalSumInfo.Items.Count; iListItemIndex++)
            //        {
            //            if (strSumName[1] == lvTotalSumInfo.Items[iListItemIndex].SubItems[1].Text)
            //            {
            //                lvTotalSumInfo.Items[iListItemIndex].SubItems[2].Text = strSumVal[1];
            //                break;
            //            }
            //        }

            //        if (iListItemIndex == lvTotalSumInfo.Items.Count)
            //        {
            //            lvItem = new ListViewItem();
            //            lvItem.Text = string.Format("{0}", lvTotalSumInfo.Items.Count);
            //            lvTotalSumInfo.Items.Add(lvItem);

            //            lvSubItem = new ListViewItem.ListViewSubItem();
            //            lvSubItem.Text = strSumName[1];
            //            lvItem.SubItems.Add(lvSubItem);

            //            lvSubItem = new ListViewItem.ListViewSubItem();
            //            lvSubItem.Text = strSumVal[1];
            //            lvItem.SubItems.Add(lvSubItem);
            //        }
            //    }
            //}
            return iStatus;
        }

        private void frmRunSum_Load(object sender, EventArgs e)
        {
            if (sCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
            {
                this.labelTitleTotalRun.Text = "total run statistic";
                this.Text = "statistic";
            }
            else 
            {
                this.labelTitleTotalRun.Text = "全部测试情况统计";
                this.Text = "统计";
            }
        }
    }
}
