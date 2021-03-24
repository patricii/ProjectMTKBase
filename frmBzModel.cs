using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bz_Handler;
using TPWrapper.CheckStatusParameters;
using System.Threading; // Patricio
using System.Runtime.InteropServices;  // Patricio
using System.Diagnostics; // Patricio


namespace ateRun
{
    public partial class frmBzModel : Form  // Patricio
    {

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        internal static readonly IntPtr InvalidHandleValue = IntPtr.Zero;

        clCfgCtrlRun sCfgCtrlRun = new clCfgCtrlRun();
        public clCfgCtrlSet g_sCurCfgCtrlSet = new clCfgCtrlSet();
        
        private System.Windows.Forms.Timer FocusTimer = new System.Windows.Forms.Timer();

        public void FocusMaster()
        {
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr hWnd = currentProcess.MainWindowHandle;
            if (hWnd != frmBzModel.InvalidHandleValue)
            {
                frmBzModel.SetForegroundWindow(hWnd);
            }
        }

        public int nTimerFormColorStatus = -1;
        public int nDoubleScanTentative = 0;

        public frmBzModel()
        {
            InitializeComponent();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                FocusTimer.Enabled = true;
            else
                FocusTimer.Enabled = false;
        }

        public static string strTrackId = string.Empty; // Patricio - variavel pública acessada em outros forms (Compare_TRackId)***

        public void textBoxScanTrackid_KeyPress(object sender, KeyPressEventArgs e)
        {
             

            if (e.KeyChar == (char)Keys.Enter)
            {
                strTrackId = textBoxScanTrackid.Text;
                if (strTrackId.Length == 10)
                { 
                    if (Bz_Handler.CJagLocalFucntions.GetBzScanMode() == "FIRST_SCAN")
                    {                                 
                        Bz_Handler.CJagLocalFucntions.SetTrackId(strTrackId);
                        FocusTimer.Enabled = false;
                        this.Close();
                    }                   
                }
                                 
                textBoxScanTrackid.Text = "";
            }
        }

        private void ComboModelName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Bz_Handler.CItemListEquip.SetMotorolaModel(ComboModelName.SelectedItem.ToString());

            this.Close();
        }

        public void StartTimer()
        {
            FocusTimer.Interval = 500;
            FocusTimer.Tick += new EventHandler(StartFocus_Tick);
            FocusTimer.Enabled = true;            
        }
        private void StartFocus_Tick(object sender, EventArgs e)
        {
            this.BringToFront();
            this.Activate();
            this.Focus();
            FocusMaster();
            textBoxScanTrackid.Select();
            textBoxScanTrackid.Focus();

            nTimerFormColorStatus = nTimerFormColorStatus * -1;

            if (nTimerFormColorStatus == 1)
                this.BackColor = System.Drawing.Color.DarkOrange;
            else
                this.BackColor = System.Drawing.Color.LightGray;
        }

        private void frmBzModel_Load(object sender, EventArgs e)
        {

            int iPosX;
            int iPosY;
            StringBuilder str = new StringBuilder(6);
            Bz_Handler.CItemListEquip.GetI2cSide(str);
            Rectangle rect = System.Windows.Forms.SystemInformation.VirtualScreen;
            string strI2CSide = str.ToString();

            StringBuilder strbenchID = new StringBuilder(16);
            Bz_Handler.CItemListEquip.GetStationID(strbenchID);
            LabelBenchId.Text = strbenchID.ToString();
            #region NO_SCAN
            if (Bz_Handler.CJagLocalFucntions.GetBzScanMode() == "NO_SCAN")
            {
                if (strI2CSide == "LEFT")
                {
                    iPosX = 0;
                    iPosY = 0;
                    this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(iPosX, iPosY);
                    this.Width = rect.Width / 2;
                    // this.Height = rect.Height - 40;

                }
                else
                {
                    iPosX = rect.Width / 2;
                    iPosY = 0;
                    this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(iPosX, iPosY);
                    this.Width = rect.Width / 2;
                    // this.Height = rect.Height - 40;
                }

                checkBox1.Hide();
                ComboModelName.Enabled = true;
                label1.Text = "SELECIONE O MODELO";
                ComboModelName.Items.Clear();
                textBoxScanTrackid.Enabled = false;
                textBoxScanTrackid.Hide();
              
                StringBuilder strMotModelName = new StringBuilder(16);

                Bz_Handler.CItemListEquip.GetMotName(strMotModelName); // The Motorola Model Name is set in the config.ini 

                StringBuilder strbMotPartNumberList = new StringBuilder(256);

                Bz_Handler.CItemListEquip.GetMotModelfromModelFile(strMotModelName.ToString(), strbMotPartNumberList);

                string strPartNumber = strbMotPartNumberList.ToString();

                string[] straTemp = strPartNumber.Split(';');

                foreach (string strWord in straTemp)
                {
                    string[] straTemp1 = strWord.Split(',');
                    if ((ComboModelName.Items.Contains(straTemp1[0])) == false) ComboModelName.Items.Add(straTemp1[0]);//Prevent duplicate items
                }
            }
            #endregion           
            #region FIRST_SCAN
            else if (Bz_Handler.CJagLocalFucntions.GetBzScanMode() == "FIRST_SCAN")
            {
                if (strI2CSide == "LEFT")
                {
                    iPosX = 0;
                    iPosY = 0;
                    this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(iPosX, iPosY);
                    this.Width = (rect.Width / 2);
                    //this.Height = rect.Height - 40;
                }
                else
                {
                    iPosX = rect.Width / 2;
                    iPosY = 0;
                    this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(iPosX, iPosY);
                    this.Width = rect.Width / 2;
                    //this.Height = rect.Height - 40;
                }
                ComboModelName.Enabled = false;
                ComboModelName.Hide();
                label1.Text = "ESCANEIE O BARCODE E INSIRA O TELEFONE NO FIXTURE";                
                textBoxScanTrackid.Enabled = true;
                textBoxScanTrackid.Show();
                textBoxScanTrackid.Focus();
                this.BackColor = System.Drawing.Color.WhiteSmoke;
                this.StartTimer();

            }
            #endregion

            // Centralize labels and texts
            LabelBenchId.Left = (this.ClientSize.Width - LabelBenchId.Size.Width) / 2;
            textBoxScanTrackid.Left = (this.ClientSize.Width - textBoxScanTrackid.Size.Width) / 2;
            ComboModelName.Left = (this.ClientSize.Width - ComboModelName.Size.Width) / 2;            
            checkBox1.Left = (this.ClientSize.Width - checkBox1.Size.Width) / 2;
            label1.MaximumSize = new Size((sender as Control).ClientSize.Width - label1.Left, 10000);
            label1.Left = (this.ClientSize.Width - label1.Size.Width) / 2;
        }       
    }
}
