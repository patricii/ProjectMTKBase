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
using System.IO.Ports;
using System.Collections.Specialized;
using Microsoft.Win32;
using System.Security.Permissions;
using System.Management;
using Bz_Handler; // Patricio
using TPWrapper.CheckStatusParameters; // Patricio
using TPWrapper; // Patricio
using TPWrapper.LogParameters; // Patricio
using Ivi.Visa.Interop; // SCPI functions





namespace ateRun
{
    public partial class frmRun1DutGui : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public const int USER = 0x0400;
        public const int WM_RUN_CFG = USER + 101;
        public const int WM_RUN_SUM = USER + 102;
        public const int WM_RUN_LOG = USER + 103;
        public const int WM_RUN_CTRL = USER + 201;
        public const int WM_RUN_PROC = USER + 301;
        public const int WM_RUN_ERR = USER + 401;

        public const int ERR_GUI = -9000;
        public const int ERR_PRE_VAL = -410000;

        public const int ERR_GUI_COMPORT_FIND_TIMEOUT = ERR_PRE_VAL + (ERR_GUI - 10);
        public const int ERR_GUI_DUT_ENTER_META_FAILED = ERR_PRE_VAL + (ERR_GUI - 11);
        public const int ERR_GUI_DUT_NO_CAL_GOLDEN = ERR_PRE_VAL + (ERR_GUI - 12);
        public const int ERR_GUI_DUT_SN_NOVOLID = ERR_PRE_VAL + (ERR_GUI - 20);
        public const int ERR_GUI_DUT_SN_GET_FAILED = ERR_PRE_VAL + (ERR_GUI - 23);
        public const int ERR_GUI_FIXTURE_OPEN_FAIL = ERR_PRE_VAL + (ERR_GUI - 30);
        public const int ERR_GUI_FIXTURE_CLOSE_FAIL = ERR_PRE_VAL + (ERR_GUI - 31);
        public const int ERR_GUI_FIXTURE_CHECK_FAIL = ERR_PRE_VAL + (ERR_GUI - 32);
        public const int ERR_GUI_POWER_ON_FAIL = ERR_PRE_VAL + (ERR_GUI - 40);
        public const int ERR_GUI_POWER_OFF_FAIL = ERR_PRE_VAL + (ERR_GUI - 41);
        public const int ERR_GUI_RF_SWITCH_FAIL = ERR_PRE_VAL + (ERR_GUI - 50);
        public const int ERR_GUI_PROC_RF_STOP = ERR_PRE_VAL + (ERR_GUI - 100);
        public const int ERR_GUI_PROC_TIMEOUT = ERR_PRE_VAL + (ERR_GUI - 101);

        public const int ERR_GUI_SW_SWITCH_FAIL = ERR_PRE_VAL + (ERR_GUI - 33);

        clCfgCtrlRun sCfgCtrlRun = new clCfgCtrlRun();
        public clCfgCtrlSet g_sCurCfgCtrlSet = new clCfgCtrlSet();
        private clExeCtrl sExeCtrl = new clExeCtrl();
        private clMsgCtrl sMsgCtrl = new clMsgCtrl();
        private clInstrCtrl sInstrCtrl = new clInstrCtrl();
        
        StreamWriter streamTraceLogFile;
        StreamWriter streamResLogFile;
        StreamWriter streamSumLogFile;
        StringBuilder sbRetCmdMsg = new StringBuilder(40960);
        StringBuilder sbRetCmdBuf = new StringBuilder(40960);
        Process g_proIntelEngineCalibrateMain;
        Process g_proRunBbTest; // Patricio
        /*********************************************************************************
         * 0:初始状态  
         * 1:完成测试并成功
         * 101:完成测试，FBR校准失败
         * 
         * 201:未完成测试，连接仪表出错 
         * 202:未完成测试，控制手机出错
         * 
        *********************************************************************************/       

        public IntPtr g_hwndFrmRun1DutGui;

        //0:   还未开始  
        //11:  查看串口是否存在
        //12:  已经找到串口
        //21:  进入测试
        //22:  无可执行文件
        //23:  已经启动测试
        //50:  开始连接手机
        //51:  已经连接上手机
        //11:  无执行文件
        //100: 完成测试      


        //private bool g_bShowSumLog = false;

        public string strOrgSumLogFile = ".\\Log\\SumLog.txt";
        public string strOrgResLogFile = ".\\Log\\ResLog.txt";
        public string strOrgTraceLogFile = ".\\Log\\TraceLog.txt";

        public bool g_bAppRebootFlag = false;          
        public int g_iResetCnt = 0;

        //"BZ" change - Timer for StartScan // Patricio
        private System.Windows.Forms.Timer StartScanTimer = new System.Windows.Forms.Timer();
        public const string mutex_id = "Global\\{ATERUN_SCAN}";
        private static Mutex ScanMutex = new Mutex(false, mutex_id);
        bool iOwnTheMutex = false;
        ///Patricio Mutex
        public const string mutex_id_METACON = "Global\\{ATERUN_METACON}";
        private static Mutex METACONMutex = new Mutex(false, mutex_id_METACON);
        bool iOwnTheMutex_METACON = false;
        ///Patricio Mutex end

        ///Patricio Mutex
        public const string mutex_id_ADBCONN = "Global\\{ATERUN_ADBCONN}";
        private static Mutex ADBCONNMutex = new Mutex(false, mutex_id_ADBCONN);
        bool iOwnTheMutex_ADBCONN = false;
        ///Patricio Mutex end
        ///
        BzLogResult LogResult = new BzLogResult(); // Class log MQS Patricio
        int MEASCODE = 0;

        private SerialPort _serialPort; //Patricio AT
        private Ivi.Visa.Interop.FormattedIO488 ioTestSet; //DTV

        private void Close_LOG_EXIT()
        {
                int nStatus = -1;
                StringBuilder strErrorMessage = new StringBuilder(256);
                nStatus = Bz_Handler.CI2cControl.ReleaseGPIBMutex();
                labelTestStatusDut0.Text = "ReleaseGPIBMutex \n Test Set Liberado";
                g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                g_sCurCfgCtrlSet.sRunAllInfo.iDutFailNum++;

                if (g_sCurCfgCtrlSet.sRunAllInfo.iRestartATEDemo == 1)
                {
                    g_sCurCfgCtrlSet.sRunAllInfo.iContinueFailCnt = 0;
                }
                frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);  // Finalizando app MTK_atedemo.exe
                frmRun1DutGui_WriteResFiles(g_sCurCfgCtrlSet);
                frmRun1DutGui_WriteInfoFiles(g_sCurCfgCtrlSet);
                frmRun1DutGui_CloseLogFiles();   // Fechando Log MTK
                g_bAppRebootFlag = false;
                g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus = -1;
                frmRunDoc1TimerRecord.Enabled = false;
                nStatus = LogResult.LogResult(g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail, strErrorMessage.ToString());
                Thread.Sleep(100);
                while (nStatus != 0)
                {
                    MessageBox.Show("Error to finish log result");
                    MessageBox.Show(strErrorMessage.ToString());
                }
                nStatus = Bz_Handler.CJagLocalFucntions.CloseTunerDVM();

                if (Bz_Handler.CItemListEquip.IsOpenJigEnable() == 1)
                {
                    if (nStatus == 0)
                    {
                        double dVoltage = -999;
                        int nCloseJigCount = 0;
                        string strPreviewMessage;
                        strPreviewMessage = labelTestStatusDut0.Text.ToString();
                        labelTestStatusDut0.Text = "!!!REMOVA O TELEFONE DO FIXTURE!!! \n \n" + strPreviewMessage;
                        Application.DoEvents();
                        nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_CHARLES_CLOSE");
                        if (nStatus == 0)
                        {
                            //while (dVoltage < 0.2)
                                while (dVoltage < 2)
                            {
                                dVoltage = Bz_Handler.CItemListEquip.ReadDVM1Voltage();

                                if (nCloseJigCount++ % 2 == 0)
                                {
                                    labelTestStatusDut0.ForeColor = System.Drawing.Color.White;
                                    labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                                    labelTestStatusDut0.Text = "!!!DTV RSSI TEST FAILED!!!";
                                }
                                else
                                {
                                    labelTestStatusDut0.ForeColor = System.Drawing.Color.Red;
                                    labelTestStatusDut0.BackColor = System.Drawing.Color.White;
                                    labelTestStatusDut0.Text = "!!!REMOVA O TELEFONE DO FIXTURE!!! \n \n" + strPreviewMessage;
                                }
                                Application.DoEvents();
                            }
                        }
                        nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_CHARLES_OPEN");
                        nStatus = Bz_Handler.CJagLocalFucntions.OpenTunerDVM();

                            labelTestStatusDut0.Text = " !!! DTV RSSI TEST FAILED !!! ";
                            labelTestStatusDut0.BackColor = System.Drawing.Color.Red;                        
                            labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                            Application.DoEvents();
                    }
                }
                StartScanTimer.Enabled = true;
                return;
             }
        private void DTV_RSSI_ON()  //Patricio PowerOn Current function
        {
            try
            {

                ioTestSet = new FormattedIO488();  //DTV

                try
                {
                    ResourceManager grm = new ResourceManager();
                    ioTestSet.IO = (IMessage)grm.Open("RS_CMW500", AccessMode.NO_LOCK, 2000, "");
                }
                catch
                {
                    ioTestSet.IO = null;
                }

                string strOpMessage = string.Empty;
                string strCommand = string.Empty;
                string strReturn = string.Empty;

                //double freq = Bz_Handler.CItemListEquip.GetTestSpecFromDataBase("DTV_RSSI", "LOWSPEC");
                double amplitudedtv = Bz_Handler.CItemListEquip.GetTestSpecFromDataBase("DTV_RSSI", "HIGHSPEC");

                labelTestStatusDut0.Text = "...DTV RSSI TEST...";
                labelTestStatusDut0.BackColor = System.Drawing.Color.Black;
                Thread.Sleep(200);
                Application.DoEvents();

                ioTestSet.WriteString("*IDN?", true);
                Thread.Sleep(3000);

                //Verify if DTV wave form can be found                
                ioTestSet.WriteString("MMEM:CAT? 'D:\\Rohde-Schwarz\\CMW\\Data\\waveform\\'", true);
                Thread.Sleep(300);
                strReturn = ioTestSet.ReadString();

                if (strReturn.Contains("ISDB-Tb_Digital_TV_withPR.wv") == false)
                {
                    MessageBox.Show("Wave Form: \r\n D:\\Rohde-Schwarz\\CMW\\Data\\waveform\\ISDB-Tb_Digital_TV_withPR.wv \r\n Não encontrada !!!", "Wave Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ioTestSet.WriteString("SOUR:GPRF:GEN:ARB:FILE 'D:\\Rohde-Schwarz\\CMW\\Data\\waveform\\ISDB-Tb_Digital_TV_withPR.wv';*OPC?", true);
                Thread.Sleep(300);
                strReturn = ioTestSet.ReadString();

                ioTestSet.WriteString("SOUR:GPRF:GEN:ARB:FILE?", true);

                ioTestSet.WriteString("SOUR:GPRF:GEN:BBM ARB;:SOUR:GPRF:GEN:ARB:REP CONT;:SOUR:GPRF:GEN:LIST OFF;:TRIG:GPRF:GEN:ARB:RETR ON;:TRIG:GPRF:GEN:ARB:AUT ON;*OPC?", true);
                Thread.Sleep(300);
                strReturn = ioTestSet.ReadString();

                ioTestSet.WriteString("SYST:ERR?", true);
                Thread.Sleep(300);

                ioTestSet.WriteString("*CLS", true);
                Thread.Sleep(300);

                ioTestSet.WriteString("ROUT:GPRF:GEN:SCEN:SAL RFBC, TX1;*OPC?", true);
                Thread.Sleep(300);
                strReturn = ioTestSet.ReadString();

                ioTestSet.WriteString("SOUR:GPRF:GEN1:RFS:FREQ 587.143 MHz;*OPC?", true); // Set frequência //587.143
                Thread.Sleep(300);
                strReturn = ioTestSet.ReadString();

                //ioTestSet.WriteString("CONF:BASE:FDC:CTAB:CRE 'Rx_RF2C_instr0' ,58714300,15.00;*OPC?", true); // Cableloss
                //strReturn = ioTestSet.ReadString();             

                ioTestSet.WriteString("SOUR:GPRF:GEN1:STAT ON;*OPC?", true); // Ligando a transmissão
                Thread.Sleep(300);
                strReturn = ioTestSet.ReadString();

                ioTestSet.WriteString("SOUR:GPRF:GEN1:RFS:LEV " + amplitudedtv.ToString() + " dBm;*OPC?", true); // Set amplitude //-40
                Thread.Sleep(300);
                strReturn = ioTestSet.ReadString();
                Thread.Sleep(13000);
                do
                {
                    ioTestSet.WriteString("SOUR:GPRF:GEN1:STAT?", true);
                    strReturn = ioTestSet.ReadString();
                }
                while (strReturn.Contains("OFF") == true);
                g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "PASS";

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: DTV RSSI Signal ON failed");
                g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
            }
        }
        private void REBOOTADBMODE()
        {
            try
            {
                int metaportid = -1;
                int nstatus = -1;
                StringBuilder str = new StringBuilder(6);
                Bz_Handler.CItemListEquip.GetI2cSide(str);
                string strI2CSide = str.ToString();
                string dir_ap;
                labelTestStatusDut0.Text = "..SEND ADB REBOOT..WAIT 60s..";
                Thread.Sleep(200);
                Application.DoEvents();

                if (strI2CSide == "LEFT")
                    dir_ap = "C:\\prod\\LACROSSE\\tools\\dut0\\mtk\\cfg\\mode_dut_cal_ver\\AP_Database";
                else
                    dir_ap = "C:\\prod\\LACROSSE\\tools\\dut1\\mtk\\cfg\\mode_dut_cal_ver\\AP_Database";

                MTK_UUT_Driver_15v2.UUT_DriverClass _metaporttoadb = new MTK_UUT_Driver_15v2.UUT_DriverClass();

                nstatus = _metaporttoadb.InitAllMetaHandle();

                if (nstatus == 0)
                    nstatus = _metaporttoadb.InitAP_NVRAM(dir_ap);
                else
                    MessageBox.Show("Error to load AP file");

                if (nstatus == 0)
                    nstatus = _metaporttoadb.SetupMetaModeParameters(2);
                else
                    MessageBox.Show("Error to initialize setup metamode");

                if (strI2CSide == "LEFT")
                    metaportid = 5;
                else
                    metaportid = 7;

                if (nstatus == 0)
                    nstatus = _metaporttoadb.SPMetaConnectInMeta(metaportid, 20000);
                else
                    MessageBox.Show("Error to connect with Kernel");

                if (nstatus == 0)
                    nstatus = _metaporttoadb.SPMetaReboot(1);
                else
                    MessageBox.Show("Reboot Command Failed");

                _metaporttoadb.SPMetaDisconnect();
                _metaporttoadb.MetaAppDisconnect();
                _metaporttoadb.DeInitAllMetaHandle();

                Thread.Sleep(45000); //Wait ADB boot
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: ADB REBOOT FAILED failed");
            }

        }
        private void SEND_ADB_Devices()
        {
            string adbcmd = "devices";
            try
            {

                Process adbcommand = new Process();
                ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.FileName = "adb.exe";
                startInfo.Arguments = adbcmd;
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                adbcommand.StartInfo = startInfo;
                adbcommand.Start();
                string adbResponse = adbcommand.StandardOutput.ReadToEnd();
                adbcommand.WaitForExit();
                if (adbResponse.Contains("List of devices attached"))
                {
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "PASS";
                }
                else
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: SEND ADBDEVICES FAILED");            
            }
        }
        private void SEND_ADB_STARTCQA()
        {
            String errMessage = string.Empty;
            string adbcmd;
            StringBuilder str = new StringBuilder(6);
            Bz_Handler.CItemListEquip.GetI2cSide(str);
            string strI2CSide = str.ToString();

            if (strI2CSide == "LEFT")
                adbcmd = "push C:\\prod\\LACROSSE\\tools\\dut0\\CQA_commands.sh /mnt/sdcard/CQATest/CQA_commands.sh";
            else
                adbcmd = "push C:\\prod\\LACROSSE\\tools\\dut1\\CQA_commands.sh /mnt/sdcard/CQATest/CQA_commands.sh";
            try
            {
                Process adbcommand = new Process();
                ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.FileName = "adb.exe";
                startInfo.Arguments = adbcmd;
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                adbcommand.StartInfo = startInfo;
                adbcommand.Start();
                string adbResponse = adbcommand.StandardOutput.ReadToEnd();
                adbcommand.WaitForExit();

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: START CQA.sh FAILED");
                g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
            }
        }
         private void SEND_ADB_DTVON()
        {
            String errMessage = string.Empty;
            string adbcmd = "shell sh /mnt/sdcard/CQATest/CQA_commands.sh DTV_On";
            try
            {

                Process adbcommand = new Process();
                ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.FileName = "adb.exe";
                startInfo.Arguments = adbcmd;
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                adbcommand.StartInfo = startInfo;
                adbcommand.Start();
                string adbResponse = adbcommand.StandardOutput.ReadToEnd();
                adbcommand.WaitForExit();
                 if (adbResponse.Contains("RETURN=PASS"))
                    {
                        frmRun1DutGui_ShowCurStatus("DTV_ON PASS");
                        LogResult.AddLogResult("DTV_ON_DTV", "DTV_ON_DTV", "0", "0", "0", "0", "0", 0, "", "");
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "PASS";
                    }
                else
                    {
                        frmRun1DutGui_ShowCurStatus("DTV_ON FAIL");
                        LogResult.AddLogResult("DTV_ON_DTV", "DTV_ON_DTV", "0", "0", "0", "0", "0", 1, "", "");
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
   
                    }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: SEND ADBMODE COMMANDS failed");            
            }
        }
        private void SEND_ADB_DTV_MEAS()
        {
            String errMessage = string.Empty;
            string adbcmd = "shell sh /mnt/sdcard/CQATest/CQA_commands.sh DTV_MEAS";
            try
            {

                Process adbcommand = new Process();
                ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.FileName = "adb.exe";
                startInfo.Arguments = adbcmd;
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                adbcommand.StartInfo = startInfo;
                adbcommand.Start();
                string adbResponse = adbcommand.StandardOutput.ReadToEnd();  
                adbcommand.WaitForExit();
                //MessageBox.Show(adbResponse);//to debug

                // MODE 1 FAIL "timeout pid 2494 signal 15\r\nRETURN=FAIL - cannot set Channel\r\n"  // SEM SINAL

                // MODE 2 FAIL "Lock : 0x0, CN: 0, RSSI : -92\r\nRETURN=PASS\r\n"  //COM SINAL SEM LOCK

                // PASS "Lock : 0x1, CN: 0, RSSI : -62\r\nRETURN=PASS\r\n" //COM SINAL COM LOCK

                if (adbResponse.Contains("RETURN=PASS") && adbResponse.Contains("Lock : 0x1")) 
                {
                    string[] _response = adbResponse.Split(new[] { "Lock : 0x1, CN: 0, RSSI :", "\r\nRETURN=PASS\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string _RssIValue = _response[0].ToString();
                    frmRun1DutGui_ShowCurStatus("DTV_RSSI PASS");
                    LogResult.AddLogResult("DTV_RSSI_DTV", "DTV_RSSI_DTV", _RssIValue,"-67", "-50", "0", "0", 0, "", "");
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "PASS";
                }
                else if (adbResponse.Contains("RETURN=PASS") && adbResponse.Contains("Lock : 0x0"))
                {
                    string[] _response = adbResponse.Split(new[] { "Lock : 0x0, CN: 0, RSSI :", "\r\nRETURN=PASS\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string _RssIValue = _response[0].ToString();
                    frmRun1DutGui_ShowCurStatus("DTV_RSSI FAIL");
                    LogResult.AddLogResult("DTV_RSSI_DTV", "DTV_RSSI_DTV", _RssIValue, "-67", "-50", "0", "0", 1, "", "");
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";                                      
                }
                else
                {
                    frmRun1DutGui_ShowCurStatus("DTV_RSSI FAIL");
                    LogResult.AddLogResult("DTV_RSSI_DTV", "DTV_RSSI_DTV", "-999", "-67", "-50", "0", "0", 1, "", "");
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";         
                }

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: SEND ADB DTV_MEAS FAILED");            
            }
        }
        private void SEND_ADB_DTV_OFF()
        {
            String errMessage = string.Empty;
            string adbcmd = "shell sh /mnt/sdcard/CQATest/CQA_commands.sh DTV_Off";
            try
            {

                Process adbcommand = new Process();
                ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.FileName = "adb.exe";
                startInfo.Arguments = adbcmd;
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                adbcommand.StartInfo = startInfo;
                adbcommand.Start();
                string adbResponse = adbcommand.StandardOutput.ReadToEnd();
                adbcommand.WaitForExit();
                if (adbResponse.Contains("RETURN=PASS"))
                {
                    frmRun1DutGui_ShowCurStatus("DTV_OFF PASS");
                    LogResult.AddLogResult("DTV_OFF", "DTV_OFF", "0", "0", "0", "0", "0", 0, "", "");
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "PASS";
                }
                else
                {
                    frmRun1DutGui_ShowCurStatus("DTV_OFF FAIL");
                    LogResult.AddLogResult("DTV_OFF", "DTV_OFF", "0", "0", "0", "0", "0", 1, "", "");
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";

                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: SEND ADB DTV_OFF FAILED");            
            }
        }

        private void SEND_ADB_COMMANDS()
        {
            int count = 0;
            labelTestStatusDut0.Text = "..SEND ADB COMMANDS..";
            Thread.Sleep(200);

            if (!iOwnTheMutex_ADBCONN)
                iOwnTheMutex_ADBCONN = ADBCONNMutex.WaitOne(); //Acquire ADB

            Application.DoEvents();

            SEND_ADB_Devices();
            if (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "FAIL")
            {
                do
                {
                    SEND_ADB_Devices();
                    Thread.Sleep(3000);
                    count++;
                }
                while (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "FAIL" && count < 3);
            }
            if (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS")
            SEND_ADB_STARTCQA();
            if (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS")
            SEND_ADB_DTVON();
            if(g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS")
            SEND_ADB_DTV_MEAS();
            if (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "FAIL")
            {
                int countDTV = 0;
                do
                {
                    SEND_ADB_DTV_OFF();
                    Thread.Sleep(500);
                    SEND_ADB_DTVON();
                    Thread.Sleep(500);
                    SEND_ADB_DTV_MEAS();
                    Thread.Sleep(500);
                    countDTV ++;
                }
                while (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "FAIL" && countDTV <3);
            }

            if(g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS")                            
            SEND_ADB_DTV_OFF();

            if (iOwnTheMutex_ADBCONN) //Release ADB
            {
                ADBCONNMutex.ReleaseMutex();
                iOwnTheMutex_ADBCONN = false;
            }
        }
        private void DTV_RSSI_TEST()
        {
            try
            {
                DTV_RSSI_ON();
                if (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS")
                SEND_ADB_COMMANDS();
                if (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS")
                    MetaModeReboot();
                else
                    Close_LOG_EXIT();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: DTV_TEST");
            }


        }

        private void MetaModeReboot()
        {
            StringBuilder str = new StringBuilder(6);
            Bz_Handler.CItemListEquip.GetI2cSide(str);
            string strI2CSide = str.ToString();
            int prelportid = -1;
            int nstatus = -1;

            labelTestStatusDut0.Text = "..SEND META REBOOT..";
            Thread.Sleep(200);
            Application.DoEvents();

            MTK_UUT_Driver_15v2.UUT_DriverClass metaport = new MTK_UUT_Driver_15v2.UUT_DriverClass();

            if (strI2CSide == "LEFT")
                prelportid = 4;
            else
                prelportid = 6;

            int count = 0;

            do
            {
                Thread.Sleep(200);
                Bz_Handler.CItemListEquip.Output2StateOFF(); // add function to OPEN I2C CHARGER  
                Thread.Sleep(200);
                Bz_Handler.CItemListEquip.Output1StateOFF(); // add function to OPEN I2C BATT 
                Thread.Sleep(500);
                Bz_Handler.CItemListEquip.Output1StateON(); // add function to CLOSE I2C BATT
                Thread.Sleep(1000);
                Bz_Handler.CItemListEquip.Output2StateON(); //add function to CLOSE I2C CHARGER
                nstatus = metaport.ConnectWithPreloaderByPort(prelportid, 1, "VID_0e8d&PID_2000", "VID_0e8d&PID_2006", 2); ;
                count++;
            }
            while (nstatus != 0 && count < 3);
            Thread.Sleep(15000);

        }
        private void CHARGER_ATCOMMAND()
        {
                  StringBuilder str = new StringBuilder(6);
            Bz_Handler.CItemListEquip.GetI2cSide(str);
            string strI2CSide = str.ToString();

            if (strI2CSide == "LEFT")
             _serialPort = new SerialPort("COM5", 115200);
            else
            _serialPort = new SerialPort("COM7", 115200);

            Thread.Sleep(500);
            _serialPort.Open(); //OPEN
            _serialPort.RtsEnable = true; 
            _serialPort.DtrEnable = true;  
            Thread.Sleep(500);
            _serialPort.WriteLine("ATE0" + "\r\n");
            Thread.Sleep(500);
            _serialPort.WriteLine("AT+START" + "\r\n");
            Thread.Sleep(500);
            _serialPort.WriteLine("AT+CHARGER=1" + "\r\n");
            Thread.Sleep(500);
            _serialPort.WriteLine("AT+CHARGER" + "\r\n");
            //string status1 = _serialPort.ReadLine(); //for response
            //MessageBox.Show(status1);          
            _serialPort.Close(); //CLOSE
        }

        private void STANDBY_ATCOMMAND()
        {
            StringBuilder str = new StringBuilder(6);
            Bz_Handler.CItemListEquip.GetI2cSide(str);
            string strI2CSide = str.ToString();
          
            //Add SP_META functions
            int nstatus = -1;
            int portid = -1;
            MTK_UUT_Driver_15v2.UUT_DriverClass teste = new MTK_UUT_Driver_15v2.UUT_DriverClass();

            if (!iOwnTheMutex_METACON)
                iOwnTheMutex_METACON = METACONMutex.WaitOne();

            teste.InitAllMetaHandle();
            if (strI2CSide == "LEFT")
                portid = 4;
            else
                portid = 6;

            int count = 0;

            do
            {
                Thread.Sleep(200);
                Bz_Handler.CItemListEquip.Output2StateOFF(); // add function to OPEN I2C CHARGER  
                Thread.Sleep(200);
                Bz_Handler.CItemListEquip.Output1StateOFF(); // add function to OPEN I2C BATT 
                Thread.Sleep(500);
                Bz_Handler.CItemListEquip.Output1StateON(); // add function to CLOSE I2C BATT
                Thread.Sleep(1000);
                Bz_Handler.CItemListEquip.Output2StateON(); //add function to CLOSE I2C CHARGER
                nstatus = teste.ConnectFactoryPreldr(portid, 20000); //Search Preloader

                count++;
            }
            while (nstatus != 0 && count < 3);

            if (strI2CSide == "LEFT")
                _serialPort = new SerialPort("COM5", 115200);
            else
                _serialPort = new SerialPort("COM7", 115200);

            Thread.Sleep(18000);
            _serialPort.Open();  //OPEN
            Thread.Sleep(500);
            _serialPort.WriteLine("AT+START" + "\r\n");
            Thread.Sleep(500);
            _serialPort.WriteLine("AT" + "\r\n");
            Thread.Sleep(500);
            _serialPort.WriteLine("AT+ESLP=1" + "\r\n"); //SwitchTo SLPMODE
            Thread.Sleep(500);
            _serialPort.WriteLine("ATE0" + "\r\n");
            Thread.Sleep(500);
            _serialPort.WriteLine("AT+IDLE" + "\r\n"); //Standby AT commnad 
            Thread.Sleep(500);
            _serialPort.Close();  //CLOSE

            teste.SPMetaDisconnect();
            teste.MetaAppDisconnect();
            teste.DeInitAllMetaHandle();

            if (iOwnTheMutex_METACON)
            {
                METACONMutex.ReleaseMutex();
                iOwnTheMutex_METACON = false;
            }
        }

        public frmRun1DutGui()
        {  
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;
        }

        private void frmRun1DutGui_AddMeasResMsg(string strResMsg)
        {
            bool bFindSetResName = true;
            bool bFindPreResName = false;
            string[] strarySetNameUnit = new string[60];
            string[] straryCurNameUnit = new string[60];
            string[] straryPreNameUnit = new string[60];
            int iResIndex = 0;
            string strLogMsg = "";
            if (strResMsg.Contains(":") && strResMsg.Contains("."))
            {
                return;
            }
            try
            {
                foreach (clCfgResSetUnit sCurResSetUnit in g_sCurCfgCtrlSet.listResMeasSet)
                {
                    if (sCurResSetUnit.strVal == "1")
                    {
                        bFindSetResName = true;
                        clResLogUnit sResLogUnit = new clResLogUnit();
                        sResLogUnit.iModemIndex = g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex;
                        sResLogUnit.strModemIndex = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex);
                        sResLogUnit.strName = strResMsg.Remove(strResMsg.IndexOf(","));
                        sResLogUnit.strVal = strResMsg.Substring(sResLogUnit.strName.Length + 1, strResMsg.Length - sResLogUnit.strName.Length - 1);
                        sResLogUnit.strLevel = sCurResSetUnit.strLevel;
                        sResLogUnit.strNameOpt = sCurResSetUnit.strNameOpt;

                        strarySetNameUnit = sCurResSetUnit.strName.Split(new char[1] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string strSetNameUnit in strarySetNameUnit)
                        {
                            if (!sResLogUnit.strName.Contains(strSetNameUnit))
                            {
                                bFindSetResName = false;
                                break;
                            }
                        }                        

                        if (bFindSetResName)
                        {
                            bFindPreResName = false;
                            foreach (clResLogUnit sPreResLogUnit in g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogRfVerify)
                            {                                
                                straryPreNameUnit = sPreResLogUnit.strVal.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                straryCurNameUnit = sResLogUnit.strVal.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                if ((sResLogUnit.strName == sPreResLogUnit.strName)
                                    &&(straryPreNameUnit[0] == straryCurNameUnit[0])
                                    &&(straryPreNameUnit[1] == straryCurNameUnit[1])
                                     &&(straryPreNameUnit[2] == straryCurNameUnit[2])
                                     &&(straryPreNameUnit[3] == straryCurNameUnit[3])
                                     &&(straryPreNameUnit[4] == straryCurNameUnit[4])
                                     &&(straryPreNameUnit[5] == straryCurNameUnit[5]))
                                {
                                    bFindPreResName = true;
                                    break;
                                }

                                iResIndex++;
                            }

                            if (bFindPreResName)
                            {
                                g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogRfVerify[iResIndex].strVal = sResLogUnit.strVal;
                            }
                            else
                            {
                                if (sResLogUnit.strNameOpt == "0")
                                {
                                    sResLogUnit.strDisc = sResLogUnit.strName;
                                }
                                else
                                {
                                    sResLogUnit.strDisc = sCurResSetUnit.strDisc;
                                }

                                g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogRfVerify.Add(sResLogUnit);
                            }
                            iResIndex = g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogRfVerify.Count;
                            strLogMsg = string.Format("{0}, {1}", g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogRfVerify[iResIndex - 1].strDisc, g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogRfVerify[iResIndex - 1].strVal);
                            frmRun1DutGui_ShowLogMsg(strLogMsg);
                            if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ") // Patricio log MQS
                            {
                                StringBuilder strErrorMessage = new StringBuilder(256);
                                int nStatus = -1;

                                nStatus = LogResult.AddTestResult(strLogMsg, strErrorMessage.ToString());
                                if (nStatus != 0)
                                {
                                    while (nStatus != 0)
                                    {
                                        MessageBox.Show("Error to add log resut");
                                        MessageBox.Show(strErrorMessage.ToString());
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: frmRun1DutGui_AddMeasResMsg");
            }
        }

        private void frmRun1DutGui_AddCalConfigInfo(string strCalResLogLine)
        {
            int iBand = 0;
            string strSingleBand = "";

            try
            {   
                string[] strAryUnit = new string[128];
                strAryUnit = strCalResLogLine.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (strAryUnit[0].Contains("CAL_RES_") && strAryUnit[0].Contains("_VAL"))
                {
                    strSingleBand = strAryUnit[1].Trim();
                    
                    if (strSingleBand.Contains("WCDMA_B")
                        || strSingleBand.Contains("LTE_B")
                        || strSingleBand.Contains("GSM")
                        || strSingleBand.Contains("DCS")
                        || strSingleBand.Contains("PCS")
                        || strSingleBand.Contains("C2K_BC")
                        || strSingleBand.Contains("TDSCDMA_B"))
                    {
                        if (strSingleBand.Contains("WCDMA_B"))
                        {
                            string strSign = "WCDMA_B";
                            int pos = strSingleBand.IndexOf(strSign);
                            string iStr = strSingleBand.Substring(pos + strSign.Length);
                            iBand = int.Parse(iStr);

                            if (iBand <= 0)
                            {
                                return;
                            }
                        }
                        else if (strSingleBand.Contains("LTE_B"))
                        {
                            string strSign = "LTE_B";
                            int pos = strSingleBand.IndexOf(strSign);
                            string iStr = strSingleBand.Substring(pos + strSign.Length);
                            iBand = int.Parse(iStr);

                            if (iBand <= 0)
                            {
                                return;
                            }
                        }
                        else if (strSingleBand.Contains("C2K_BC"))
                        {
                            string strSign = "C2K_BC";
                            int pos = strSingleBand.IndexOf(strSign);
                            string iStr = strSingleBand.Substring(pos + strSign.Length);
                            iBand = int.Parse(iStr);

                            if (iBand < 0)
                            {
                                return;
                            }
                        }
                        else if (strSingleBand.Contains("TDSCDMA_B"))
                        {
                            string strSign = "TDSCDMA_B";
                            int pos = strSingleBand.IndexOf(strSign);
                            string iStr = strSingleBand.Substring(pos + strSign.Length);
                            iBand = int.Parse(iStr);

                            if (iBand <= 0)
                            {
                                return;
                            }
                        }
                        else if (strSingleBand.Contains("PCS"))
                        {
                            strSingleBand = "GSM1900";
                        }
                        else if (strSingleBand.Contains("DCS"))
                        {
                            strSingleBand = "GSM1800";
                        }

                        if (g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex > 0)
                        {
                            strSingleBand += "_C2";
                        }

                        if (g_sCurCfgCtrlSet.sRunAllInfo.strCalBandsConfigInfo.Length > 0)
                        {
                            if (!g_sCurCfgCtrlSet.sRunAllInfo.strCalBandsConfigInfo.Contains(strSingleBand))
                            {
                                g_sCurCfgCtrlSet.sRunAllInfo.strCalBandsConfigInfo += "," + strSingleBand;
                            }
                        }
                        else
                        {
                            g_sCurCfgCtrlSet.sRunAllInfo.strCalBandsConfigInfo = strSingleBand;
                        }
                        
                    }

                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: frmRun1DutGui_AddCalConfigInfo");
            }
        }

        private void frmRun1DutGui_AddCalResMsg(string strResMsg)
        {
            bool bFindReturnResName = true;
            string strLogMsg = "";
            clResLogUnit sResLogUnit = new clResLogUnit();

            if (strResMsg.Contains(":") && strResMsg.Contains("."))
            {
                return;
            }

            try
            {
                foreach (clCfgResSetUnit sCurResSetUnit in g_sCurCfgCtrlSet.listResCalSet)
                {
                    if (sCurResSetUnit.strVal == "1")
                    {
                        bFindReturnResName = true;
                        sResLogUnit.iModemIndex = g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex;
                        sResLogUnit.strModemIndex = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex);
                        sResLogUnit.strName = strResMsg.Remove(strResMsg.IndexOf(","));
                        sResLogUnit.strVal = strResMsg.Substring(sResLogUnit.strName.Length + 1, strResMsg.Length - sResLogUnit.strName.Length - 1);
                        sResLogUnit.strLevel = sCurResSetUnit.strLevel;
                        sResLogUnit.strNameOpt = sCurResSetUnit.strNameOpt;                     

                        string[] straryNameUnit = new string[60];
                        straryNameUnit = sCurResSetUnit.strName.Split(new char[1] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string strNameUnit in straryNameUnit)
                        {
                            if (!sResLogUnit.strName.Contains(strNameUnit))
                            {
                                bFindReturnResName = false;
                                break;
                            }
                        }

                        //if (sResLogUnit.strName.Contains("CAL_RES_LTE"))
                        {
                            if (bFindReturnResName)
                            {
                                if (sResLogUnit.strNameOpt == "0")
                                {
                                    sResLogUnit.strDisc = sResLogUnit.strName;
                                }
                                else
                                {
                                    sResLogUnit.strDisc = sCurResSetUnit.strDisc;
                                }

                                g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogRfCalibrate.Add(sResLogUnit);
                                strLogMsg = string.Format("{0}, {1}", sResLogUnit.strDisc, sResLogUnit.strVal);
                                frmRun1DutGui_ShowLogMsg(strLogMsg);
                                if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ")  // Patricio LOG MQS
                                {
                                    StringBuilder strErrorMessage = new StringBuilder(256);
                                    int nStatus = -1;

                                    nStatus = LogResult.AddTestResult(strLogMsg, strErrorMessage.ToString());
                                    if (nStatus != 0)
                                    {
                                        while (nStatus != 0)
                                        {
                                            MessageBox.Show("Error to add log resut");
                                            MessageBox.Show(strErrorMessage.ToString());
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: frmRun1DutGui_AddCalResMsg");
            }
        }
        private void PowerOnCurrent()  //Patricio PowerOn Current function
        {
          try 
           {
               if (Bz_Handler.CItemListEquip.GetDoTestFromDataBase("POWERON_CURR") == 1)
               {

                   bool bStatus = false;
                   int nCurrentTimeFrame = 10;
                   int nTry = 0;
                   double dPowerOnCurrent = 0;
                   double dMaxCurrent = 0;
                   string strOpMessage = string.Empty;

                   labelTestStatusDut0.Text = "...POWERON TEST...";
                   labelTestStatusDut0.BackColor = System.Drawing.Color.Black;
                   frmRun1DutGui_ShowCurStatus("..POWER_ON_CURRENT TEST..");
                   Thread.Sleep(2000);
                   labelTestStatusDut0.Text = "..POWERON TEST..";
                   Application.DoEvents();

                   Bz_Handler.CItemListEquip.SetRangeMAX();
                   Thread.Sleep(300);

                   do
                   {

                       Bz_Handler.CJagLocalFucntions.I2CTurnPhoneOnByVBAT();

                       
                       double dPowerOnLowLimit = Bz_Handler.CItemListEquip.GetTestSpecFromDataBase("POWERON_CURR", "LOWSPEC");
                       double dPowerOnHighLimit = Bz_Handler.CItemListEquip.GetTestSpecFromDataBase("POWERON_CURR", "HIGHSPEC");

                      
                       Stopwatch sTestTimeFrame = Stopwatch.StartNew();
                       do
                       {
                           dPowerOnCurrent = 1000 * Bz_Handler.CItemListEquip.MeasPS1Current();  //function to read a power supply current measurement
                           if (dPowerOnCurrent > dMaxCurrent) dMaxCurrent = dPowerOnCurrent; //get max current

                       }
                       while (sTestTimeFrame.ElapsedMilliseconds < nCurrentTimeFrame * 1000);
                       dPowerOnCurrent = -dPowerOnCurrent;
                       if ((dPowerOnHighLimit < (dPowerOnCurrent) || dPowerOnLowLimit > (dPowerOnCurrent)))
                       {
                           bStatus = false;
                           frmRun1DutGui_ShowCurStatus("POWERON_FAIL");
                           string cutlogPowerOn = String.Format("{0:F20}", dPowerOnCurrent).Remove(7);
                           LogResult.AddLogResult("PowerOn_Current", "PowerOn_Current", cutlogPowerOn, dPowerOnHighLimit.ToString(), dPowerOnLowLimit.ToString(), "0", "0", 1, "", "");
                           g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                           Bz_Handler.CJagLocalFucntions.I2CDisconnectAll(); // Open all I2C Connections
                           Thread.Sleep(2000);
                       }
                       else
                       {
                           bStatus = true;
                           frmRun1DutGui_ShowCurStatus("POWERON_PASS");
                           string cutlogPowerOn = String.Format("{0:F20}", dPowerOnCurrent).Remove(7);
                           LogResult.AddLogResult("PowerOn_Current", "PowerOn_Current", cutlogPowerOn, dPowerOnHighLimit.ToString(), dPowerOnLowLimit.ToString(), "0", "0", 0, "", "");
                           g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "PASS";
                       }
                   }
                   while (!bStatus && ++nTry < 3);
               }
        }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: StandbyCurrent");
            }
        }
        private void StandByCurrent() // Patricio StandbyCurrentTest function
        {
            try
            {
                if (Bz_Handler.CItemListEquip.GetDoTestFromDataBase("STANDBY_CURR") == 1)
                {
                    labelTestStatusDut0.Text = "...STANDBY TEST...";
                    labelTestStatusDut0.BackColor = System.Drawing.Color.Black;
                    frmRun1DutGui_ShowCurStatus("..STANDBY_CURRENT TEST..");
                    Application.DoEvents();

                    double StandbyLowLimit = Bz_Handler.CItemListEquip.GetTestSpecFromDataBase("STANDBY_CURR", "LOWSPEC");
                    double StandbyHighLimit = Bz_Handler.CItemListEquip.GetTestSpecFromDataBase("STANDBY_CURR", "HIGHSPEC");              

                    STANDBY_ATCOMMAND(); //Atcommand FactoryMode

                    //Bz_Handler.CItemListEquip.SetRangeMIN();
                    
                    int status = 0;
                    frmRun1DutGui_ShowCurStatus("..STANDBY_CURRENT_TEST..");

                    Bz_Handler.CItemListEquip.Output2StateOFF(); // add function to OPEN I2C CHARGER  after command
                    Thread.Sleep(300);

                    double StandbyCurrent = 0.0;
                    int m_IntTestCount = 0;
 
                    do
                    {
                        StandbyCurrent = Bz_Handler.CItemListEquip.MeasPS1Current();
                        m_IntTestCount++;
                        StandbyCurrent = StandbyCurrent / 20;
                        Thread.Sleep(300);
                    }
                    while (((StandbyHighLimit < (StandbyCurrent)) || (StandbyLowLimit > (StandbyCurrent))) && m_IntTestCount < 30);
                    if ((StandbyHighLimit < (StandbyCurrent) || StandbyLowLimit > (StandbyCurrent)))
                    {
                        //FAIL
                        frmRun1DutGui_ShowCurStatus("STANDBY_CURRENT_FAIL");
                        string cutlogPowerOff = String.Format("{0:F20}", StandbyCurrent).Remove(7);
                        LogResult.AddLogResult("STANDBY_CURRENT", "STANDBY_CURR", cutlogPowerOff, StandbyHighLimit.ToString(), StandbyLowLimit.ToString(), "0", "0", 1, "", "");
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                        status = 1;
                    }
                    else if (status == 0)
                    {
                        //PASS  
                        frmRun1DutGui_ShowCurStatus("STANDBY_CURRENT_PASS");
                        string cutlogPowerOff = String.Format("{0:F20}", StandbyCurrent).Remove(7);
                        LogResult.AddLogResult("STANDBY_CURRENT", "STANDBY_CURR", cutlogPowerOff, StandbyHighLimit.ToString(), StandbyLowLimit.ToString(), "0", "0", 0, "", "");
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "PASS";
                    }                  

                }
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: StandbyCurrent");
            }
        
        
        }
        private void ChargerCurrent() // Patricio Charger CurrentTest function
        {
            try
            {
                if (Bz_Handler.CItemListEquip.GetDoTestFromDataBase("CHARGER_CURR") == 1)
                {
                    labelTestStatusDut0.Text = "...CHARGER TEST...";
                    labelTestStatusDut0.BackColor = System.Drawing.Color.Black;

                    double ChargeLowLimit1 = Bz_Handler.CItemListEquip.GetTestSpecFromDataBase("CHARGER_CURR", "LOWSPEC");
                    double ChargeHighLimit1 = Bz_Handler.CItemListEquip.GetTestSpecFromDataBase("CHARGER_CURR", "HIGHSPEC");
                    

                    int status = 0;
                    frmRun1DutGui_ShowCurStatus("..CHARGER_CURRENT_TEST..");
                    Bz_Handler.CItemListEquip.Output2StateOFF(); // add function to OPEN I2C CHARGER  
                    Thread.Sleep(300);
                    Bz_Handler.CItemListEquip.Output1StateOFF(); // add function to OPEN I2C BATT 
                    Thread.Sleep(300);
                    Bz_Handler.CItemListEquip.Output1StateON(); // add function to CLOSE I2C BATT
                    Thread.Sleep(300);
                    Bz_Handler.CItemListEquip.Output2StateON(); //add function to CLOSE I2C CHARGER
                    Thread.Sleep(18000);

                    double ChargerCurrent = 0.0;
                    int m_IntTestCount = 0;

                    double ChargeHighLimit = -ChargeHighLimit1; // to negative spec
                    double ChargeLowLimit = -ChargeLowLimit1; // to negative spec
                    do
                    {
                        ChargerCurrent = Bz_Handler.CItemListEquip.MeasPS1Current(); //function to read a power supply negative current measure
                        m_IntTestCount++;
                        Thread.Sleep(300);
                    }
                    while (((ChargeHighLimit > (ChargerCurrent)) || (ChargeLowLimit < (ChargerCurrent))) && m_IntTestCount < 30);
                    if ((ChargeHighLimit > (ChargerCurrent) || ChargeLowLimit < (ChargerCurrent)))
                    {                      
                       //FAIL
                       frmRun1DutGui_ShowCurStatus("CHARGER_CURRENT_FAIL");
                       string cutlogPowerOff = String.Format("{0:F20}", ChargerCurrent).Remove(7);
                       LogResult.AddLogResult("CHARGER_CURRENT", "CHARGER_CURR", cutlogPowerOff, ChargeHighLimit.ToString(), ChargeLowLimit.ToString(), "0", "0", 1, "", "");
                       g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                       status = 1;
                    }
                    else if (status == 0)
                    {
                        //PASS  
                        frmRun1DutGui_ShowCurStatus("CHARGER_CURRENT_PASS");
                        string cutlogPowerOff = String.Format("{0:F20}", ChargerCurrent).Remove(7);
                        LogResult.AddLogResult("CHARGER_CURRENT", "CHARGER_CURR", cutlogPowerOff, ChargeHighLimit.ToString(), ChargeLowLimit.ToString(), "0", "0", 0, "", "");
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "PASS";
                    }
                }
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: ChargerCurrent");
            }
        }

        private void PowerOffCurrent() //Patricio CurrentTest function
        {
            try
            {
                if (Bz_Handler.CItemListEquip.GetDoTestFromDataBase("POWER_OFF_CURR") == 1)
                {
                    labelTestStatusDut0.Text = "...POWEROFF TEST...";
                    labelTestStatusDut0.BackColor = System.Drawing.Color.Black;

                    double PowerOffLowLimit1 = Bz_Handler.CItemListEquip.GetTestSpecFromDataBase("POWER_OFF_CURR", "LOWSPEC");
                    double PowerOffhighLimit = Bz_Handler.CItemListEquip.GetTestSpecFromDataBase("POWER_OFF_CURR", "HIGHSPEC");

                    int status = 0;
                    frmRun1DutGui_ShowCurStatus("..POWER_OFF_CURRENT TEST..");
                    Bz_Handler.CItemListEquip.Output2StateOFF(); // add function to OPEN I2C CHARGER  
                    Thread.Sleep(500);
                    Bz_Handler.CItemListEquip.Output1StateOFF(); // add function to OPEN I2C BATT 
                    Thread.Sleep(4000);
                    Bz_Handler.CItemListEquip.Output1StateON(); // add function to CLOSE I2C BATT 
                    Thread.Sleep(500);
                    double poweroffCurrent = 0.0;
                    int m_IntTestCount = 0;

                    double PowerOffLowLimit = -PowerOffLowLimit1;
                    do
                    {
                        poweroffCurrent = Bz_Handler.CItemListEquip.MeasPS1Current(); //function to read a power supply current measure
                        m_IntTestCount++;
                        Thread.Sleep(100);
                    }
                    while (((PowerOffhighLimit < (poweroffCurrent)) || (PowerOffLowLimit > (poweroffCurrent))) && m_IntTestCount < 30);
                    if ((PowerOffhighLimit < (poweroffCurrent) || PowerOffLowLimit > (poweroffCurrent)))
                    {
                        //FAIL
                        frmRun1DutGui_ShowCurStatus("POWER_OFF_CURRENT_FAIL");
                        string cutlogPowerOff = String.Format("{0:F20}", poweroffCurrent).Remove(7);
                        LogResult.AddLogResult("POWEROFF_CURRENT", "POWER_OFF_CURR", cutlogPowerOff, PowerOffhighLimit.ToString(), PowerOffLowLimit.ToString(), "0", "0", 1, "", "");
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                        status = 1;
                    }
                    else if (status == 0)
                    {
                        //PASS  
                        frmRun1DutGui_ShowCurStatus("POWER_OFF_CURRENT_PASS");
                        string cutlogPowerOff = String.Format("{0:F20}", poweroffCurrent).Remove(7);
                        LogResult.AddLogResult("POWEROFF_CURRENT", "POWER_OFF_CURR", cutlogPowerOff, PowerOffhighLimit.ToString(), PowerOffLowLimit.ToString(), "0", "0", 0, "", "");
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "PASS";
                    }
                }

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: PowerOffCurrent");
            }

        }

        // ///////BB Logs  Patricio 
        private void frmRun1DutGui_AddBbResMsg(string strResMsg)
        {
            bool bFindRetResName = true;
            string strLogMsg = "";
            clResLogUnit sResLogUnit = new clResLogUnit();

            if (strResMsg.Contains(":") && strResMsg.Contains("."))
            {
                return;
            }

            try
            {
                foreach (clCfgResSetUnit sCurResSetUnit in g_sCurCfgCtrlSet.listResBbSet)
                {
                    if (sCurResSetUnit.strVal == "1")
                    {
                        bFindRetResName = true;
                        sResLogUnit.iModemIndex = g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex;
                        sResLogUnit.strModemIndex = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex);
                        sResLogUnit.strName = strResMsg.Remove(strResMsg.IndexOf(","));
                        sResLogUnit.strVal = strResMsg.Substring(sResLogUnit.strName.Length + 1, strResMsg.Length - sResLogUnit.strName.Length - 1);
                        sResLogUnit.strLevel = sCurResSetUnit.strLevel;
                        sResLogUnit.strNameOpt = sCurResSetUnit.strNameOpt;

                        string[] straryNameUnit = new string[60];
                        straryNameUnit = sCurResSetUnit.strName.Split(new char[1] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string strNameUnit in straryNameUnit)
                        {
                            if (!sResLogUnit.strName.Contains(strNameUnit))
                            {
                                bFindRetResName = false;
                                break;
                            }
                        }

                        if (bFindRetResName)
                        {
                            if (sResLogUnit.strNameOpt == "0")
                            {
                                sResLogUnit.strDisc = sResLogUnit.strName;
                            }
                            else
                            {
                                sResLogUnit.strDisc = sCurResSetUnit.strDisc;
                            }

                            g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogBB.Add(sResLogUnit);
                            strLogMsg = string.Format("{0}, {1}", sResLogUnit.strDisc, sResLogUnit.strVal);
                            frmRun1DutGui_ShowLogMsg(strLogMsg);

                            if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ")
                            {
                                StringBuilder strErrorMessage = new StringBuilder(256);
                                int nStatus = -1;

                                nStatus = LogResult.AddTestResult(strLogMsg, strErrorMessage.ToString());
                                if (nStatus != 0)
                                {
                                    while (nStatus != 0)
                                    {
                                        MessageBox.Show("Error to add log resut");
                                        MessageBox.Show(strErrorMessage.ToString());
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error: frmRun1DutGui_AddBbResMsg");
            }
        }
        /// ////BB End Patricio
        public bool IsGoldenDut(clCfgCtrlSet sCurCfgCtrlSet, string strSN)
        {
            bool bIsGoldenDut = false;

            foreach (clCfgGolenDutUnit sGoldenDut in sCurCfgCtrlSet.listGoldenDut)
            {
                if (strSN == sGoldenDut.strSN)
                {
                    bIsGoldenDut = true;
                }
            }
            return bIsGoldenDut;
        }
        
        private void frmRunDoc1_StartProcessCalibrateMain()
        {
            string strPromptMsg = "";
            try
            {
                if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ")  // Patricio
                {
                    Thread.Sleep(1000);
                    int nStatus = 0;
                    labelTestStatusDut0.Text = "AcquireGPIBMutex \n Aguardando Test Set";
                    Application.DoEvents();
                    nStatus = Bz_Handler.CI2cControl.AcquireGPIBMutex();
                    if (nStatus == 0)
                    {                       
                        labelTestStatusDut0.Text = "Checking TestSet Options";
                        Application.DoEvents();
                        nStatus = Bz_Handler.CJagLocalFucntions.CheckTestOptions();
                        if (nStatus != 0)
                        {
                            while (nStatus != 0)
                            {
                                MessageBox.Show("Equipamento Sem licença KV113!!");
                            }
                        }

                        labelTestStatusDut0.Text = "Checking TestSet Firmware";
                        Application.DoEvents();
                        nStatus = Bz_Handler.CJagLocalFucntions.CheckTestSetFWVersion();
                        if (nStatus != 0)
                        {
                            while (nStatus != 0)
                            {
                                MessageBox.Show("Versão de Firmware Incorreto");
                            }
                        }

                        labelTestStatusDut0.Text = "Checking 10MHz Status";
                        Application.DoEvents();
                        nStatus = Bz_Handler.CJagLocalFucntions.Check10Mhz();
                        if (nStatus != 0)
                        {
                            while (nStatus != 0)
                            {
                                MessageBox.Show("Fail to lock 10MHz signal.");
                            }
                        }
                    }
                    if (Bz_Handler.CItemListEquip.GetDoTestFromDataBase("DTV_RSSI") == 1)
                    {
                        if (nStatus == 0)
                            nStatus = Bz_Handler.CI2cControl.SendI2cCommand("FC_RF.IN1_TO_OUT2_ENG");
                        if (nStatus != 0)
                        {
                            MessageBox.Show("Fail to Close RF Switch");
                            Environment.Exit(0);
                        }
                        DTV_RSSI_TEST(); // DTV TEST
                    }
                    if (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail != "FAIL")
                    {
                        MEASCODE = 0;
                        if (nStatus == 0)
                            nStatus = Bz_Handler.CI2cControl.SendI2cCommand("FC_RF.MAIN_ENG");
                        if (nStatus == 0)
                            nStatus = Bz_Handler.CJagLocalFucntions.ClosePowerKey();
                        if (nStatus != 0)
                        {
                            MessageBox.Show("Fail to Close RF Switch");
                            Environment.Exit(0);
                        }
                    }
                }                                                                        

                if (!File.Exists(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath))
                {
                    strPromptMsg = string.Format("选择路径下{0}可执行文件不存在，请检查!", g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);
                    frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
                    return;
                }
                else if (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS")
                {
                    if (g_bAppRebootFlag == false)
                    {
                        frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);                    
                        g_proIntelEngineCalibrateMain = new Process();
                        g_proIntelEngineCalibrateMain.StartInfo.UseShellExecute = false;
                        g_proIntelEngineCalibrateMain.StartInfo.RedirectStandardInput = true;
                        g_proIntelEngineCalibrateMain.StartInfo.RedirectStandardOutput = true;
                        g_proIntelEngineCalibrateMain.StartInfo.RedirectStandardError = true;
                        g_proIntelEngineCalibrateMain.StartInfo.CreateNoWindow = true;
                        g_proIntelEngineCalibrateMain.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        g_proIntelEngineCalibrateMain.StartInfo.FileName = g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath;
                        g_proIntelEngineCalibrateMain.StartInfo.WorkingDirectory = g_sCurCfgCtrlSet.sRunAllInfo.strCurExeWorkPath;
                        g_proIntelEngineCalibrateMain.StartInfo.Arguments = "";
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ExeThreadCalibrateMain), g_proIntelEngineCalibrateMain);

                    }
                    else
                    {  // Second inicialization Patricio End
                        string strWriteLine = string.Format("{0:HH:mm:ss}##########CMD_ATEDEMO_PROCEDURE_START##########", DateTime.Now);
                        g_proIntelEngineCalibrateMain.StandardInput.WriteLine(strWriteLine);
                    }
                }
            }
            catch (Exception exc)
            {
                strPromptMsg = string.Format("{0}:{1}", "frmStart_RunExe Failed!", exc.Message);
                frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
            }
        }


        private void ExeThreadCalibrateMain(object obj)
        {
            string strPromptMsg = "";
            try
            {
                Process proCmdCalMain = obj as Process;
                proCmdCalMain.Start();
                proCmdCalMain.OutputDataReceived += new DataReceivedEventHandler(ExeCalMainThread_OutputDataReceived);

                proCmdCalMain.BeginOutputReadLine();

                Application.DoEvents();

                proCmdCalMain.WaitForExit();

                if (proCmdCalMain.ExitCode != 0)
                {
                    //ShowCalMainMessage(cmd.StandardOutput.ReadToEnd());
                }
                proCmdCalMain.Close();

                g_bAppRebootFlag = false;
            }
            catch (Exception exc)
            {
                strPromptMsg = string.Format("{0}:{1}", "ExeThreadCalibrateMain Failed", exc.Message);
                frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
            }
        }
        
        ///BB start test by Patricio
        private void frmRun1DutGui_StartProcessBbTest()
        {
            StringBuilder str = new StringBuilder(6);
            Bz_Handler.CItemListEquip.GetI2cSide(str);
            string strI2CSide = str.ToString();
            if (strI2CSide == "LEFT")
            {
                g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0BbExeFilePath;
                g_sCurCfgCtrlSet.sRunAllInfo.strCurBbCfgFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0BbCfgFilePath;
                if (Bz_Handler.CItemListEquip.IsFqaVerify() == 1) // To FQA ENTER in META MODE!!!
                {
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurBbSeqFilePath = "C:\\prod\\LACROSSE\\tools\\dut0\\mtk\\bb\\TestSequence_FQA.tsq";
                }
                else
                g_sCurCfgCtrlSet.sRunAllInfo.strCurBbSeqFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0BbSeqFilePath;
                g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeWorkPath = g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath.Remove(g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath.LastIndexOf("\\")); //  Patricio
            }
            else
            {
                g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0BbExeFilePath;
                g_sCurCfgCtrlSet.sRunAllInfo.strCurBbCfgFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0BbCfgFilePath;
                g_sCurCfgCtrlSet.sRunAllInfo.strCurBbSeqFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0BbSeqFilePath;
                g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeWorkPath = g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath.Remove(g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath.LastIndexOf("\\")); //  Patricio
            }
            string strPromptMsg = "";

            //strPromptMsg = "Enter BaseBand testing process";

            frmRun1DutGui_ShowCurItem(strPromptMsg);
            frmRun1DutGui_ShowItemMsg(strPromptMsg);

            frmRun1DutGui_InitCurBbTestParas();
            frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath); // Close BB.exe
            g_sCurCfgCtrlSet.sRunAllInfo.strRetDutSerialNumber = Bz_Handler.CJagLocalFucntions.GetTrackId();
            g_sCurCfgCtrlSet.sRunAllInfo.strLogDutSerialNumber = Bz_Handler.CJagLocalFucntions.GetTrackId();
            //frmRun1DutGui_ShowSN(g_sCurCfgCtrlSet.sRunAllInfo.strRetDutSerialNumber);

            g_proRunBbTest = new Process();
            g_proRunBbTest.StartInfo.UseShellExecute = false;
            g_proRunBbTest.StartInfo.RedirectStandardInput = true;
            g_proRunBbTest.StartInfo.RedirectStandardOutput = true;
            g_proRunBbTest.StartInfo.RedirectStandardError = true;
            g_proRunBbTest.StartInfo.CreateNoWindow = true;
            g_proRunBbTest.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            g_proRunBbTest.StartInfo.FileName = g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath;
            g_proRunBbTest.StartInfo.WorkingDirectory = g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeWorkPath;
            g_proRunBbTest.StartInfo.Arguments = string.Format("{0} {1} {2} {3}",
            g_sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex,
            g_sCurCfgCtrlSet.sRunAllInfo.strInputDutSerialNumber,
            g_sCurCfgCtrlSet.sRunAllInfo.strCurBbCfgFilePath,
            g_sCurCfgCtrlSet.sRunAllInfo.strCurBbSeqFilePath);

            ThreadPool.QueueUserWorkItem(new WaitCallback(ExeThreadBbTest), g_proRunBbTest);       
        }

        private void ExeThreadBbTest(object obj)
        {
            string strPromptMsg = "";
            try
            {
                Process proCmdBbTest = obj as Process;
                proCmdBbTest.Start();
                proCmdBbTest.OutputDataReceived += new DataReceivedEventHandler(ExeBbTestThread_OutputDataReceived);
                proCmdBbTest.BeginOutputReadLine();
                Application.DoEvents();
                proCmdBbTest.WaitForExit();
                if (proCmdBbTest.ExitCode != 0)
                {
                    //ShowRfTestMessage(cmd.StandardOutput.ReadToEnd());
                }
                proCmdBbTest.Close();
            }
            catch (Exception exc)
            {
                strPromptMsg = string.Format("{0}:{1}", "ExeThreadBbTest Failed", exc.Message);
                frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
            }
            //if (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus == 0)
            //{
            //    frmRunDoc1_StartProcessCalibrateMain();
            //}              

        }
        void ExeBbTestThread_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            ShowBbTestMessage(e.Data);
        }
        
        private void ShowBbTestMessage(string msg)
        {
            string strPromptMsg = "";
            string strErrMsg = "";
            string strErrName = "";
            string strErrVal = "";
            string[] straryRetErrBufAll = new string[60];
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowMessageHandler(ShowBbTestMessage), new object[] { msg });
            }
            else
            {
                if (msg != null)
                {
                    if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ")
                    {
                        if (msg.Contains("#lenovo_log#BB#DELAY"))
                        {                          
                            Bz_Handler.CJagLocalFucntions.Set_USB_PS2_5V();
                            Thread.Sleep(2000);
                        }
                        if (Bz_Handler.CItemListEquip.IsOpenJigEnable() == 1)
                        {
                            if (msg.Contains("FAIL") && msg.Contains("#lenovo_log#BB#ENTERTESTMODE"))
                                Bz_Handler.CJagLocalFucntions.SetBBRecycle(true);
                        }                      
                        if (msg.Contains("PASS") && msg.Contains("#lenovo_log#BB#Receiver"))
                        {
                            if (Bz_Handler.CItemListEquip.GetDoTestFromDataBase("DTV_RSSI") == 1)
                            {
                                REBOOTADBMODE();
                            }

                            frmRunDoc1_StartProcessCalibrateMain(); //Start Calibration                            
                        }
      
                    }

                    if (streamTraceLogFile != null)
                    {
                        if (msg.Length > 0)
                        {
                            streamTraceLogFile.WriteLine(msg);
                        }
                    }

                    if ((msg.Contains("#lenovo_log#")) && (msg.Contains("FAIL")))
                    {
                        g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus = -1;
                    }

                    frmRun1DutGui_GetInstrInfo(msg);
                    frmRun1DutGui_GetCfgInfo(msg);
                    frmRun1DutGui_GetResInfo(msg);


                    if (msg.Contains("Total Result:"))
                    {
                        g_sCurCfgCtrlSet.sRunAllInfo.bBbTestDone = true;
                        strErrMsg = msg;
                        strErrName = strErrMsg.Substring(0, strErrMsg.LastIndexOf(":"));
                        strErrVal = strErrMsg.Substring(strErrMsg.LastIndexOf(":"), strErrMsg.Length - strErrMsg.LastIndexOf(":"));
                    }
                }
                else
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iEnablePowerSupplyControl == 1)
                    {
                        strPromptMsg = "关闭程控电源开始";
                        if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ")
                            strPromptMsg = "TurnOff Power Supply";
                        frmRun1DutGui_ShowCurStatus(strPromptMsg);
                        frmRun1DutGui_ShowItemMsg(strPromptMsg);

                        Thread ntExeCtrlPowerOff = new Thread(new ThreadStart(sExeCtrl.clExeCtrl_PowerOff));
                        ntExeCtrlPowerOff.IsBackground = true;
                        ntExeCtrlPowerOff.Priority = ThreadPriority.Normal;
                        ntExeCtrlPowerOff.Start();
                        ntExeCtrlPowerOff.Join();

                        if (sExeCtrl.bPowerOff)
                        {
                            strPromptMsg = "关闭程控电源成功";
                            if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ")
                                strPromptMsg = "Closing Power Supply";
                            frmRun1DutGui_ShowCurStatus(strPromptMsg);
                            frmRun1DutGui_ShowItemMsg(strPromptMsg);

                        }
                        else
                        {
                            frmRun1DutGui_SetErrMsg(ERR_GUI_POWER_ON_FAIL, "打开程控电源失败");
                            return;
                        }
                    }

                    
                    if ((g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != 0) || (!g_sCurCfgCtrlSet.sRunAllInfo.bBbTestDone))
                    {
                        if (Bz_Handler.CItemListEquip.IsOpenJigEnable() == 1)
                        {
                            if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ") // Patricio BB recycle
                            {
                                #region BzBBRecyle
                                g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryNum = 2;
                                if (Bz_Handler.CJagLocalFucntions.GetBBRecyle())
                                {
                                    Bz_Handler.CJagLocalFucntions.SetBBRecycle(false);

                                    //if ((g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex > 0) && (g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex < g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryNum))
                                    if ((g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex >= 0) && (g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex < g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryNum))
                                    {
                                        #region OperatorOpenCloseJig
                                        Thread.Sleep(2000);
                                        int nStatus = -1;
                                        double dVoltage = -999;
                                        int nCloseJigCount = 0;
                                        this.Focus();
                                        labelTestStatusDut0.Text = "!!!!!!! ATENCAO !!!!!!!\n \n BASEBAND RECYCLE \n \n Abra a tampa do Fixture";
                                        labelTestStatusDut0.ForeColor = System.Drawing.Color.Purple;
                                        labelTestStatusDut0.BackColor = System.Drawing.Color.White;
                                        Application.DoEvents();
                                        Thread.Sleep(2000);
                                        nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_CHARLES_CLOSE");

                                        if (nStatus == 0)
                                        {
                                            Thread.Sleep(2000);
                                            //while (dVoltage < 0.2)
                                            while (dVoltage < 2)
                                            {
                                                dVoltage = Bz_Handler.CItemListEquip.ReadDVM1Voltage();

                                                if (nCloseJigCount++ % 2 == 0)
                                                {
                                                    labelTestStatusDut0.ForeColor = System.Drawing.Color.Purple;
                                                    labelTestStatusDut0.BackColor = System.Drawing.Color.White;
                                                }
                                                else
                                                {
                                                    labelTestStatusDut0.ForeColor = System.Drawing.Color.White;
                                                    labelTestStatusDut0.BackColor = System.Drawing.Color.Purple;
                                                }
                                                Application.DoEvents();
                                            }
                                        }

                                        labelTestStatusDut0.Text = "FECHE A TAMPA DO FIXTURE";
                                        Application.DoEvents();

                                        if (nStatus == 0)
                                        {
                                           // while (dVoltage > 0.2)
                                                while (dVoltage > 2)
                                            {
                                                dVoltage = Bz_Handler.CItemListEquip.ReadDVM1Voltage();

                                                if (nCloseJigCount++ % 2 == 0)
                                                {
                                                    labelTestStatusDut0.ForeColor = System.Drawing.Color.Purple;
                                                    labelTestStatusDut0.BackColor = System.Drawing.Color.White;
                                                }
                                                else
                                                {
                                                    labelTestStatusDut0.ForeColor = System.Drawing.Color.White;
                                                    labelTestStatusDut0.BackColor = System.Drawing.Color.Purple;
                                                }
                                                Application.DoEvents();
                                            }
                                        }

                                        nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_CHARLES_OPEN");
                                        labelTestStatusDut0.ForeColor = System.Drawing.Color.Yellow;
                                        labelTestStatusDut0.BackColor = System.Drawing.Color.Black;
                                        #endregion
                                    }

                                    if (g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex < g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryNum)
                                    {
                                        sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_bb_test");
                                        g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex++;
                                    }
                                    else
                                    {
                                        sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_release");
                                    }
                                }
                                else
                                {
                                    sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_release");
                                }

                                #endregion
                            }
                        }
                        else
                        {
                            if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableRetryMode == 0)
                            {
                                g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex = g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryNum;
                            }

                            if (g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex < g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryNum)
                            {
                                sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_bb_test");
                                g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex++;
                            }
                            else
                            {
                                sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_release");
                            }
                        }
                    }
                    
                }
            }
        }
        ///BB End Patricio

        
        void ExeCalMainThread_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //ShowCalMainMessage(e.Data);
            Thread CalMainMessageThread = new Thread(new ParameterizedThreadStart(DoThreadWork));
            CalMainMessageThread.IsBackground = true;
            CalMainMessageThread.Priority = ThreadPriority.Normal;
            CalMainMessageThread.Start(e.Data);
            CalMainMessageThread.Join();
        }

        public void DoThreadWork(object data)
        {
            string Message = "";

            if (data == null)
            {
                Message = null;
            }
            else
            {
                Message = data.ToString();
            }

            ShowCalMainMessage(Message);           
        }

        private delegate void ShowMessageHandler(string msg);

        private void ShowCalMainMessage(string msg)
        {
            string strPromptMsg = "";
            int iErrCode = 0;
            string strErrMsg = "";
            string strErrName = "";
            string strErrVal = "";
            int iErrIndex = 0;
            string[] straryRetErrBufAll = new string[60];
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowMessageHandler(ShowCalMainMessage), new object[] { msg });
            }
            else
            {
                if (msg != null)
                {
                    //if (!msg.Contains("lenovo_log#"))
                    {
                        if (streamTraceLogFile != null)
                        {
                            if (msg.Length > 0)
                            {
                                streamTraceLogFile.WriteLine(msg);
                                //clLogUnit sLogUnit = new clLogUnit();
                                //sLogUnit.strLog = msg;
                                //sRunLogInfo.listRunTraceLog.Add(sLogUnit);
                            }
                        }
                    }

                    if (msg.Contains("CMD_ATEDEMO_INIT_FINISHED") && (g_bAppRebootFlag == false))
                    {
                        string strWriteLine = string.Format("{0:HH:mm:ss}##########CMD_ATEDEMO_PROCEDURE_START##########", DateTime.Now);
                        g_proIntelEngineCalibrateMain.StandardInput.WriteLine(strWriteLine);                        

                        if(g_sCurCfgCtrlSet.sRunAllInfo.iEnablePowerSupplyControl == 1)                        
                        {
                            if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)                                
                            {
                                strPromptMsg = "PSU power on begin";                               
                            }
                            else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                            {
                                strPromptMsg = "打开程控电源开始";
                            }       
                            frmRun1DutGui_ShowCurStatus(strPromptMsg);                                    
                            frmRun1DutGui_ShowItemMsg(strPromptMsg);

                            Thread ntExeCtrlPowerOn = new Thread(new ThreadStart(sExeCtrl.clExeCtrl_PowerOn));
                            ntExeCtrlPowerOn.IsBackground = true;
                            ntExeCtrlPowerOn.Priority = ThreadPriority.Normal;
                            ntExeCtrlPowerOn.Start();
                            ntExeCtrlPowerOn.Join();

                            if (sExeCtrl.bPowerOn)
                            {
                                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)                                
                                {
                                    strPromptMsg = "PSU power on successful";                               
                                }
                                else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                {
                                    strPromptMsg = "打开程控电源成功";
                                }
                                frmRun1DutGui_ShowCurStatus(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);                              
                            }
                            else
                            {
                                frmRun1DutGui_SetErrMsg(ERR_GUI_POWER_ON_FAIL, "打开程控电源失败");
                                return;
                            }                                                 
                        }

                        this.labelTestStatusDut0.ResetText();
                        this.labelTestStatusDut0.BackColor = System.Drawing.Color.Black;
                        this.labelTestStatusDut0.ForeColor = System.Drawing.Color.Yellow;  
                        
                        if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)  //Patricio to first initialize unit!!                              
                        {
                            strPromptMsg = "Software inicializado!";      
                            //if (Bz_Handler.CItemListEquip.IsFqaVerify() == 1) // Meta Calibration handle in METAMODE
                            //{
                            //Thread.Sleep(200);
                            //Bz_Handler.CItemListEquip.Output2StateOFF(); // add function to OPEN I2C CHARGER  
                            //Thread.Sleep(200);
                            //Bz_Handler.CItemListEquip.Output1StateOFF(); // add function to OPEN I2C BATT 
                            //Thread.Sleep(500);
                            //Bz_Handler.CItemListEquip.Output1StateON(); // add function to CLOSE I2C BATT
                            //Thread.Sleep(1000);
                            //Bz_Handler.CItemListEquip.Output2StateON(); //add function to CLOSE I2C CHARGER
                            //Thread.Sleep(200);
                            //}
                        }
                        else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                        {
                            strPromptMsg = "程序就绪，准备测试";
                        }
                        frmRun1DutGui_ShowCurItem(strPromptMsg);
                        frmRun1DutGui_ShowItemMsg(strPromptMsg);
                    }
                   //////////////
                    else if (msg.Contains("Error Code:") && msg.Contains("WCDMA_GET_CAPABILITY_FAILED") || msg.Contains("Error Code:") && msg.Contains("WCDMA_FHC_APC_WRITE_INIT_VALUE_FAILED") || msg.Contains("Error Code:") && msg.Contains("WCDMA_FHC_AGC_PARSE_CONFIG_FAILED") || msg.Contains("Error Code:") && msg.Contains("GSM_AGC_WCoef_GET_RUNTIME_FAILED"))
                    {
                        int nStatus = 0;
                        nStatus = Bz_Handler.CI2cControl.ReleaseGPIBMutex();
                        labelTestStatusDut0.Text = "ReleaseGPIBMutex \n Test Set Liberado";
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                        g_sCurCfgCtrlSet.sRunAllInfo.iDutFailNum++;
                        if (g_sCurCfgCtrlSet.sRunAllInfo.iRestartATEDemo == 1)
                        {
                            g_sCurCfgCtrlSet.sRunAllInfo.iContinueFailCnt = 0;
                        }
                        frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);  // Finalizando app MTK_atedemo.exe
                        frmRun1DutGui_WriteResFiles(g_sCurCfgCtrlSet);
                        frmRun1DutGui_WriteInfoFiles(g_sCurCfgCtrlSet);
                        frmRun1DutGui_CloseLogFiles();   // Fechando Log MTK
                        g_bAppRebootFlag = false;
                        nStatus = Bz_Handler.CJagLocalFucntions.CloseTunerDVM();
                        if (Bz_Handler.CItemListEquip.IsOpenJigEnable() == 1)
                        {
                            if (nStatus == 0)
                            {
                                double dVoltage = -999;
                                int nCloseJigCount = 0;
                                string strPreviewMessage;
                                strPreviewMessage = labelTestStatusDut0.Text.ToString();
                                labelTestStatusDut0.Text = "!!!REMOVA O TELEFONE DO FIXTURE!!! \n \n" + strPreviewMessage;
                                Application.DoEvents();
                                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_CHARLES_CLOSE");
                                nStatus = Bz_Handler.CJagLocalFucntions.CloseTunerDVM();
                                if (nStatus == 0)
                                {
                                   // while (dVoltage < 0.2)
                                        while (dVoltage < 2)
                                    {
                                        dVoltage = Bz_Handler.CItemListEquip.ReadDVM1Voltage();

                                        if (nCloseJigCount++ % 2 == 0)
                                        {
                                            labelTestStatusDut0.ForeColor = System.Drawing.Color.Red;
                                            labelTestStatusDut0.BackColor = System.Drawing.Color.Black;
                                        }
                                        else
                                        {
                                            labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                                            labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                                        }
                                        Application.DoEvents();
                                    }
                                }
                                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_CHARLES_OPEN");
                                nStatus = Bz_Handler.CJagLocalFucntions.OpenTunerDVM();

                                labelTestStatusDut0.Text = "!!! WCDMA GET CAPABILITY FAIL !!! ";
                                labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                                labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                                Application.DoEvents();
                            }
                        }
                        if (nStatus == 0)
                            Thread.Sleep(100);
                        nStatus = Bz_Handler.CJagLocalFucntions.ExitHandlerTest(g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail);

                        while (nStatus != 0)
                        {
                            MessageBox.Show("Exit Handler Error");
                        }
                        MEASCODE = 1;
                        msg = "CMD_ATEDEMO_PROCEDURE_FINISHED";
                        sMsgCtrl.SendMsg(g_hwndFrmRun1DutGui, WM_RUN_PROC, "run_proc", "run_release");
                        sMsgCtrl.SendMsg(g_hwndFrmRun1DutGui, WM_RUN_PROC, "run_proc", "run_stop");
                        StartScanTimer.Enabled = true;
                        return;
                    }
                        //////////////
                    else if (msg.Contains("CMD_ATEDEMO_PROCEDURE_FINISHED"))
                    {
                        g_bAppRebootFlag = true;

                       frmRunDoc1_ProcessTestResult();

                        //sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_release");

                       if (g_sCurCfgCtrlSet.sRunAllInfo.iATERebootEveryTime == 1)
                        {
                            g_sCurCfgCtrlSet.sRunAllInfo.iRestartATEDemo = 0;

                            g_bAppRebootFlag = false;
                         

                            g_proIntelEngineCalibrateMain.StandardInput.WriteLine(@"##########CMD_ATEDEMO_APP_QUIT##########");
               
                            Thread.Sleep(700);    
            
                            frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);                           
                        }
                    }
                    else if (msg.Contains("CMD_ATEDEMO_APP_QUIT_FINISHED"))
                    {
                        strPromptMsg = "ATE_DEMO EXIT"; 
                        frmRun1DutGui_ShowCurStatus(strPromptMsg);
                    }

                    if (msg.Contains("Get Available Handle and Please reset Target"))
                    {
                        if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)                                
                        {
                            strPromptMsg = string.Format("{0:yyyy/MM/dd HH:mm:ss.fff} Search Preloader port begin", DateTime.Now);                               
                        }
                        else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                        {
                            strPromptMsg = string.Format("{0:yyyy/MM/dd HH:mm:ss.fff} 开始检测Preloader口", DateTime.Now);
                        }

                        frmRun1DutGui_ShowCurStatus(strPromptMsg);
                        frmRun1DutGui_ShowItemMsg(strPromptMsg);

                        g_iResetCnt++; 
                            
                        if ((g_sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode == 1)
                                && (g_iResetCnt > 1))                            
                        {
                            //string strWriteLine = string.Format("{0}##########CMD_ATEDEMO_APP_RUN##########", DateTime.Now.ToString("HH:mm:ss"));
                            //g_proIntelEngineCalibrateMain.StandardInput.WriteLine(strWriteLine);

                            //if (g_sCurCfgCtrlSet.sRunAllInfo.iEnablePowerSupplyControl == 1)
                            //{
                            //    strPromptMsg = "打开程控电源开始";
                            //    frmRun1DutGui_ShowCurStatus(strPromptMsg);
                            //    frmRun1DutGui_ShowItemMsg(strPromptMsg);

                            //    Thread ntExeCtrlPowerOn = new Thread(new ThreadStart(sExeCtrl.clExeCtrl_PowerOn));
                            //    ntExeCtrlPowerOn.IsBackground = true;
                            //    ntExeCtrlPowerOn.Priority = ThreadPriority.Normal;
                            //    ntExeCtrlPowerOn.Start();
                            //    ntExeCtrlPowerOn.Join();

                            //    if (sExeCtrl.bPowerOn)
                            //    {
                            //        strPromptMsg = "打开程控电源成功";
                            //        frmRun1DutGui_ShowCurStatus(strPromptMsg);
                            //        frmRun1DutGui_ShowItemMsg(strPromptMsg);

                            //        //strPromptMsg = "等待手机进入测试模式";
                            //        //frmRun1DutGui_ShowCurItem(strPromptMsg);
                            //        //frmRun1DutGui_ShowItemMsg(strPromptMsg);
                            //    }
                            //    else
                            //    {
                            //        frmRun1DutGui_SetErrMsg(ERR_GUI_POWER_ON_FAIL, "打开程控电源失败");
                            //        return;
                            //    }
                            //}
                            //else if (g_sCurCfgCtrlSet.sRunAllInfo.iEnablePowerSupplyControl == 0)
                            //{
                            //    strPromptMsg = "请对手机进行上下电，使手机进入校准模式！";
                            //    frmRun1DutGui_ShowCurItem(strPromptMsg);
                            //    frmRun1DutGui_ShowItemMsg(strPromptMsg);
                            //}                            
                        }  
                    }

                    if (msg.Contains("Search Preloader USB port") && msg.Contains("begin"))
                    {
                        string strWriteLine = string.Format("{0:HH:mm:ss}##########CMD_ATEDEMO_RESEARCH_PRELOADER_PARAMS--{{{1}}}--##########", DateTime.Now,
                        (g_sCurCfgCtrlSet.sRunAllInfo.iRepeatModeOpt == 1) ? "2000" : "INFINITE");
                        g_proIntelEngineCalibrateMain.StandardInput.WriteLine(strWriteLine);
                    }

                    if (msg.Contains("Search Kernel USB port") && msg.Contains("begin"))
                    {                        
                        if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)                                
                        {
                            strPromptMsg = string.Format("{0:yyyy/MM/dd HH:mm:ss.fff} Search Kernel port begin", DateTime.Now);                               
                        }
                        else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                        {
                            strPromptMsg = string.Format("{0:yyyy/MM/dd HH:mm:ss.fff} 开始检测Kernel口", DateTime.Now);
                        }

                        frmRun1DutGui_ShowCurStatus(strPromptMsg);
                        frmRun1DutGui_ShowItemMsg(strPromptMsg);                        
                    }

                    if (msg.Contains("SP_BootMode_Complete"))
                    {
                        if (g_sCurCfgCtrlSet.sRunAllInfo.iRepeatModeOpt == 1)
                        {
                            //frmRun1DutGui_InitToolUI();

                            string strWriteLine = string.Format("{0}##########CMD_ATEDEMO_APP_RUN##########", DateTime.Now.ToString("HH:mm:ss"));
                            g_proIntelEngineCalibrateMain.StandardInput.WriteLine(strWriteLine);
                        }
                        else if (g_sCurCfgCtrlSet.sRunAllInfo.iRepeatModeOpt == 2)
                        {
                            if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableFixtureControl == 1
                                && g_sCurCfgCtrlSet.sRunAllInfo.iCheckFixtureClosed == 1)
                            {
                                strPromptMsg = "检查夹具状态";
                                strPromptMsg = string.Format("{0:yyyy/MM/dd HH:mm:ss.fff} 检查夹具状态", DateTime.Now);

                                frmRun1DutGui_ShowCurStatus(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);

                                for (int iCnt = 0; iCnt < 5; iCnt++)
                                {
                                    Thread ntExeCtrlCheckFixtureClose = new Thread(new ThreadStart(sExeCtrl.clExeCtrl_QueryFixtureStatus));
                                    ntExeCtrlCheckFixtureClose.IsBackground = true;
                                    ntExeCtrlCheckFixtureClose.Priority = ThreadPriority.Normal;
                                    ntExeCtrlCheckFixtureClose.Start();
                                    ntExeCtrlCheckFixtureClose.Join();

                                    if (sExeCtrl.bCheckFixtureClosed)
                                    {
                                        strPromptMsg = "夹具已关闭";
                                        strPromptMsg = string.Format("{0:yyyy/MM/dd HH:mm:ss.fff} 夹具已关闭", DateTime.Now);

                                        frmRun1DutGui_ShowCurStatus(strPromptMsg);
                                        frmRun1DutGui_ShowItemMsg(strPromptMsg);

                                        if (iCnt == 4)
                                        {
                                            frmRun1DutGui_InitToolUI();
                                            
                                            if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                                            {
                                                strPromptMsg = "Enter into calibration && NSFT procedure";
                                            }
                                            else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                            {
                                                strPromptMsg = "进入射频校准 && 非信令测试流程";
                                            }
                                           
                                            frmRun1DutGui_ShowCurItem(strPromptMsg);
                                            frmRun1DutGui_ShowItemMsg(strPromptMsg);

                                            string strWriteLine = string.Format("{0}##########CMD_ATEDEMO_APP_RUN##########", DateTime.Now.ToString("HH:mm:ss"));
                                            g_proIntelEngineCalibrateMain.StandardInput.WriteLine(strWriteLine);
                                        }
                                    }
                                    else
                                    {
                                        g_iResetCnt = 0;
                                        string strWriteLine = string.Format("{0}##########CMD_ATEDEMO_APP_PRELOADER_RESTART_DETECT##########", DateTime.Now.ToString("HH:mm:ss.fff"));
                                        g_proIntelEngineCalibrateMain.StandardInput.WriteLine(strWriteLine);
                                        break;
                                    }

                                    Thread.Sleep(500);
                                }
                            }
                            else
                            {
                                frmRun1DutGui_InitToolUI();

                                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                                {
                                    strPromptMsg = "Enter into calibration && NSFT procedure";
                                }
                                else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                {
                                    strPromptMsg = "进入射频校准 && 非信令测试流程";
                                }

                                frmRun1DutGui_ShowCurItem(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);

                                string strWriteLine = string.Format("{0}##########CMD_ATEDEMO_APP_RUN##########", DateTime.Now.ToString("HH:mm:ss"));
                                g_proIntelEngineCalibrateMain.StandardInput.WriteLine(strWriteLine);                            
                            }
                        }                       

                        //string strWriteLine = string.Format("{0}##########CMD_ATEDEMO_APP_RUN##########", DateTime.Now.ToString("HH:mm:ss"));
                        //g_proIntelEngineCalibrateMain.StandardInput.WriteLine(strWriteLine);

                        //if (g_iResetCnt == 1)
                        //{
                        //    frmRun1DutGui_InitToolUI();
                        //}
                    }

                    if (msg.Contains("Open Preloader comport timeout."))
                    {
                        //if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableFixtureControl == 1)
                        //{
                        //    strPromptMsg = "检查夹具状态";
                        //    frmRun1DutGui_ShowCurStatus(strPromptMsg);
                        //    frmRun1DutGui_ShowItemMsg(strPromptMsg);

                        //    Thread ntExeCtrlCheckFixtureClose = new Thread(new ThreadStart(sExeCtrl.clExeCtrl_QueryFixtureStatus));
                        //    ntExeCtrlCheckFixtureClose.IsBackground = true;
                        //    ntExeCtrlCheckFixtureClose.Priority = ThreadPriority.Normal;
                        //    ntExeCtrlCheckFixtureClose.Start();
                        //    ntExeCtrlCheckFixtureClose.Join();

                        //    if (sExeCtrl.bCheckFixtureClosed)
                        //    {
                        //        strPromptMsg = "夹具已关闭";
                        //        frmRun1DutGui_ShowCurStatus(strPromptMsg);
                        //        frmRun1DutGui_ShowItemMsg(strPromptMsg);

                        //        frmRun1DutGui_InitToolUI();

                        //        string strWriteLine = string.Format("{0}##########CMD_ATEDEMO_APP_RUN##########", DateTime.Now.ToString("HH:mm:ss"));
                        //        g_proIntelEngineCalibrateMain.StandardInput.WriteLine(strWriteLine);
                        //    }
                        //    else
                        //    { 
                        //        g_iResetCnt = 0;
                        //        string strWriteLine = string.Format("{0}##########CMD_ATEDEMO_APP_PRELOADER_CONTINUE_DETECT##########", DateTime.Now.ToString("HH:mm:ss.fff"));
                        //        g_proIntelEngineCalibrateMain.StandardInput.WriteLine(strWriteLine);
                        //    }
                        //}
                        //else
                        //{
                        //    frmRun1DutGui_InitToolUI();

                        //    string strWriteLine = string.Format("{0}##########CMD_ATEDEMO_APP_RUN##########", DateTime.Now.ToString("HH:mm:ss"));
                        //    g_proIntelEngineCalibrateMain.StandardInput.WriteLine(strWriteLine);                        
                        //}                        
                    }

                    if (msg.Contains("SP_BootMode_Fail_Restart_PSU"))
                    {
                        frmRun1DutGui_SetErrMsgDefualt();

                        if (g_sCurCfgCtrlSet.sRunAllInfo.iEnablePowerSupplyControl == 1)
                        {
                             if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                            {
                                strPromptMsg = "PSU power off begin";
                            }
                             else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                             {
                                 strPromptMsg = "关闭程控电源开始";
                             }

                            frmRun1DutGui_ShowCurStatus(strPromptMsg);
                            frmRun1DutGui_ShowItemMsg(strPromptMsg);

                            Thread ntExeCtrlPowerOff = new Thread(new ThreadStart(sExeCtrl.clExeCtrl_PowerOff));
                            ntExeCtrlPowerOff.IsBackground = true;
                            ntExeCtrlPowerOff.Priority = ThreadPriority.Normal;
                            ntExeCtrlPowerOff.Start();
                            ntExeCtrlPowerOff.Join();

                            if (sExeCtrl.bPowerOff)
                            {
                                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                                {
                                    strPromptMsg = "PSU power off successful";
                                }
                                else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                {
                                    strPromptMsg = "关闭程控电源成功";
                                }
                                frmRun1DutGui_ShowCurStatus(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);
                            }
                            else
                            {
                                frmRun1DutGui_SetErrMsg(ERR_GUI_POWER_OFF_FAIL, "关闭程控电源失败");
                                //sMsgCtrl.SendMsg(this.Handle, WM_RUN_ERR, "run_err", "power_off_failed");
                                //return;
                            }

                            Thread.Sleep(2000);

                            if (g_sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode == 1)
                            {
                                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                                {
                                    strPromptMsg = "PSU power on begin";
                                }
                                else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                {
                                    strPromptMsg = "打开程控电源开始";
                                }
                                frmRun1DutGui_ShowCurStatus(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);

                                Thread ntExeCtrlPowerOn = new Thread(new ThreadStart(sExeCtrl.clExeCtrl_PowerOn));
                                ntExeCtrlPowerOn.IsBackground = true;
                                ntExeCtrlPowerOn.Priority = ThreadPriority.Normal;
                                ntExeCtrlPowerOn.Start();
                                ntExeCtrlPowerOn.Join();

                                if (sExeCtrl.bPowerOn)
                                {
                                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                                    {
                                        strPromptMsg = "PSU power on successful";
                                    }
                                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                    {
                                        strPromptMsg = "打开程控电源成功";
                                    }

                                    frmRun1DutGui_ShowCurStatus(strPromptMsg);
                                    frmRun1DutGui_ShowItemMsg(strPromptMsg);

                                    //strPromptMsg = "等待手机进入测试模式";
                                    //frmRun1DutGui_ShowCurItem(strPromptMsg);
                                    //frmRun1DutGui_ShowItemMsg(strPromptMsg);
                                }
                                else
                                {
                                    frmRun1DutGui_SetErrMsg(ERR_GUI_POWER_ON_FAIL, "关闭程控电源失败");
                                    return;
                                }
                            }
                        }
                    }

                    if (msg.Contains("Enter into META Mode OK"))
                    {
                        g_sCurCfgCtrlSet.sRunAllInfo.bCalEnterMeta = true;

                        if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                        {
                            strPromptMsg = "Enter into test mode";

                        }
                        else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                        {
                            strPromptMsg = "手机已经进入测试模式";
                        }

                        frmRun1DutGui_ShowCurItem(strPromptMsg);
                        
                         if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                        {
                            strPromptMsg = string.Format("{0:yyyy/MM/dd HH:mm:ss.fff} Enter into test mode", DateTime.Now);
                        }
                         else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                         {
                             strPromptMsg = string.Format("{0:yyyy/MM/dd HH:mm:ss.fff} 手机已经进入测试模式", DateTime.Now);
                         }

                        frmRun1DutGui_ShowItemMsg(strPromptMsg);
                        //frmRun1DutGui_ShowLogMsg(strPromptMsg);

                        g_sCurCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin = DateTime.Now;
                        g_sCurCfgCtrlSet.sRunAllInfo.tsRunBegin = new TimeSpan(g_sCurCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Ticks);                        
                    }

                    if ((msg.Contains("lenovo_log#")) && (msg.Contains("RET_SN=")))
                    {
                        g_sCurCfgCtrlSet.sRunAllInfo.bPrintSNInfo = true;

                        if (msg.Contains("PASS"))
                        {
                            string[] straryRetBufAll = new string[60];
                            straryRetBufAll = msg.Split(new char[1] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                            if (straryRetBufAll.Length > 1)
                            {
                                string[] straryRetBufUnitAll = new string[60];
                                straryRetBufUnitAll = straryRetBufAll[1].Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                                if (straryRetBufUnitAll.Length > 1)
                                {
                                    g_sCurCfgCtrlSet.sRunAllInfo.strRetDutSerialNumber = straryRetBufUnitAll[1];
                                    frmRun1DutGui_ShowSN(g_sCurCfgCtrlSet.sRunAllInfo.strRetDutSerialNumber);

                                    if (g_sCurCfgCtrlSet.sRunAllInfo.iDesignType == 0)
                                    {
                                        g_sCurCfgCtrlSet.sRunAllInfo.strLogDutSerialNumber = g_sCurCfgCtrlSet.sRunAllInfo.strRetDutSerialNumber.Substring(0, g_sCurCfgCtrlSet.sRunAllInfo.strRetDutSerialNumber.Length - 2);
                                    }
                                    else if ((g_sCurCfgCtrlSet.sRunAllInfo.iDesignType == 1) || (g_sCurCfgCtrlSet.sRunAllInfo.iDesignType == 2))
                                    {
                                        g_sCurCfgCtrlSet.sRunAllInfo.strLogDutSerialNumber = g_sCurCfgCtrlSet.sRunAllInfo.strRetDutSerialNumber;
                                    }
                                }
                                if (g_sCurCfgCtrlSet.sRunAllInfo.strLogDutSerialNumber.Length < 2)
                                {
                                    frmRun1DutGui_SetErrMsg(ERR_GUI_DUT_SN_NOVOLID, "返回SN无效");
                                    //sMsgCtrl.SendMsg(this.Handle, WM_RUN_ERR, "run_err", "dut_sn_no_volid");
                                    return;
                                }

                                //如果是Golden Dut, 则不能进行校准
                                if (IsGoldenDut(g_sCurCfgCtrlSet, g_sCurCfgCtrlSet.sRunAllInfo.strRetDutSerialNumber)
                                    && (g_sCurCfgCtrlSet.sRunAllInfo.iRunMode == 0))
                                {
                                    //frmRun1DutGui_SetErrMsg( ERR_GUI_DUT_NO_CAL_GOLDEN, "不能校准金板");
                                    sMsgCtrl.SendMsg(this.Handle, WM_RUN_ERR, "run_err", "dut_no_cal_golden");

                                    return;
                                }
                            }
                        }
                        else if (msg.Contains("FAIL"))
                        {
                            frmRun1DutGui_SetErrMsg(ERR_GUI_DUT_SN_GET_FAILED, "读取SN失败");
                            //sMsgCtrl.SendMsg(this.Handle, WM_RUN_ERR, "run_err", "dut_get_sn_failed");
                            //return;
                        }
                    }

                    frmRun1DutGui_GetInstrInfo(msg);
                    frmRun1DutGui_GetCfgInfo(msg);
                    frmRun1DutGui_GetResInfo(msg);
                    frmRun1DutGui_GetItemInfo(msg);

                    if (msg.Contains("Set Clean Boot Successfully.")
                        || msg.Contains("write cal&final test status to barcode string pass!"))
                    {
                        g_sCurCfgCtrlSet.sRunAllInfo.bCalMainDone = true;
                    }
                    else if (msg.Contains("RF Stop failed: (Possibly non-recoverable error occurred)"))
                    //|| msg.Contains("[*Error*]")
                    {
                        //frmRun1DutGui_SetErrMsg( ERR_GUI_PROC_RF_STOP, "测试过程出错");
                        //g_sCurCfgCtrlSet.sRunAllInfo.bCalMainFail = true;
                        //sMsgCtrl.SendMsg(this.Handle, WM_RUN_ERR, "run_err", "dut_get_sn_failed");
                    }
                    else if (msg.Contains("Error Code:") && msg.Contains("=>"))
                    {
                        string strMeasDescription = ""; // Patricio
                        string ErrorCode = ""; // Patricio
                        strErrMsg = msg;
                        strErrName = strErrMsg.Substring(0, strErrMsg.LastIndexOf("="));
                        strErrVal = strErrMsg.Substring(strErrMsg.LastIndexOf(">"), strErrMsg.Length - strErrMsg.LastIndexOf(">"));

                        straryRetErrBufAll = strErrMsg.Split(new char[3] { ':', '=', '>' }, StringSplitOptions.RemoveEmptyEntries);

                        if (straryRetErrBufAll.Length > 1)
                        {
                            strErrMsg = "";
                            iErrIndex = 0;
                            iErrCode = int.Parse(straryRetErrBufAll[straryRetErrBufAll.Length - 1]);
                            foreach (string strAddErrMsg in straryRetErrBufAll)
                            {
                                if ((iErrIndex != 0) || (iErrIndex != straryRetErrBufAll.Length - 1))
                                {
                                    strErrMsg += strAddErrMsg;
                                }
                                if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ") // Patricio
                                {
                                    if (iErrIndex == 2) //BZ error log
                                        ErrorCode = strAddErrMsg;

                                    if (iErrIndex == 1) //BZ error log
                                    {
                                        strMeasDescription = strAddErrMsg;
                                        strMeasDescription.Trim();
                                    }
                                }
                                iErrIndex++;
                            }
                        }
                        if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ") // Patricio
                            LogResult.AddLogResult("", strMeasDescription, ErrorCode, "0", "0", "0", "0", 1, "", "");

                        frmRun1DutGui_SetErrMsg(iErrCode, strErrMsg);
                    }

                    if (msg.Contains("SW Switch FAIL"))
                    {
                        if(g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt ==2)
                        {                         
                            frmRun1DutGui_SetErrMsg(ERR_GUI_SW_SWITCH_FAIL, "切换软体失败");
                        }
                        else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                        {
                            frmRun1DutGui_SetErrMsg(ERR_GUI_SW_SWITCH_FAIL, "SW switch fail");
                        }
                        
                    }

                    if (msg.Contains("ATE Tool Version:"))
                    {
                        frmRun1DutGui_GetAteVersion(msg);
                    }
                }
                else
                {
                    frmRunDoc1_ProcessTestResult();                    
                }
            }
        }

        private void frmRun1DutGui_GetAteVersion(string strMsg)
        {
            string startMark = "ATE Tool Version:";
            string endMark = "Part Number:";

            int iStartPos = strMsg.IndexOf(startMark);
            int iEndPos = strMsg.IndexOf(endMark);

            if (iEndPos==-1)
            {
                iEndPos = strMsg.Length - 1;
            }

            string strVersion = strMsg.Substring(iStartPos + startMark.Length, iEndPos - iStartPos - startMark.Length);
           
            string[] strArray = strVersion.Split(new char []{'.'});

            g_sCurCfgCtrlSet.sRunAllInfo.strAteVersion = strArray[1];
        }

        private void frmRunDoc1_ProcessTestResult()
        {        
            if ((g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != 0) || (!g_sCurCfgCtrlSet.sRunAllInfo.bCalMainDone))                    
            {                
                if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableRetryMode == 0)
                {
                    g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex = g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryNum;
                }

                if (g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex < g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryNum)
                {
                    g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex++;
                    sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_cal_ver");                    
                }
                else
                {
                    if (g_bAppRebootFlag)
                    {
                       sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_release");
                    }
                }                    
            }                    
            else
            {
                if (g_bAppRebootFlag)
                {
                    sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_release");
                }
            }
            //runCtrl_ExeCmd("exe_cmd:run_ctrl:exit_share_resource", sbRetCmdBuf, sbRetCmdMsg);
            //frmRun1DutGui_ShowLogMsg(sbRetCmdMsg.ToString());
            //g_sCurCfgCtrlSet.sRunAllInfo.dtCurrentRunEnd = DateTime.Now;
            //g_sCurCfgCtrlSet.sRunAllInfo.tsRunEnd = new TimeSpan(g_sCurCfgCtrlSet.sRunAllInfo.dtCurrentRunEnd.Ticks);
            //TimeSpan ts = g_sCurCfgCtrlSet.sRunAllInfo.tsRunEnd.Subtract(g_sCurCfgCtrlSet.sRunAllInfo.tsRunBegin).Duration();
            //labelTimeCost.Text = string.Format("{0:000.000}s", ts.TotalSeconds);

            //frmRun1DutGui_ShowProgressVal(g_sCurCfgCtrlSet.sRunAllInfo.iItemNum - 1, g_sCurCfgCtrlSet.sRunAllInfo.iItemNum);

            //if ((g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != 0) || (!g_sCurCfgCtrlSet.sRunAllInfo.bCalMainDone))
            //{
            //    strPromptMsg = "射频校准 && 非信令测试出错";
            //    frmRun1DutGui_ShowCurItem(strPromptMsg);
            //    frmRun1DutGui_ShowItemMsg(strPromptMsg);
            //    //frmRun1DutGui_SetErrMsg(0, 3, 9098, strPromptMsg);
            //    sMsgCtrl.SendMsg(g_hwndFrmRun1DutGui, WM_RUN_ERR, "run_err", "err_happened");
            //}
            //else
            //{
            //    strPromptMsg = "完成射频校准 && 非信令测试流程";
            //    frmRun1DutGui_ShowCurItem(strPromptMsg);
            //    frmRun1DutGui_ShowItemMsg(strPromptMsg);
            //    sMsgCtrl.SendMsg(g_hwndFrmRun1DutGui, WM_RUN_PROC, "run_proc", "run_release");
            //}   
        }

        private void frmRun1DutGui_ShowMsgPrompt(int iMsgMode, string strMsgPrompt)
        {
            frmMsgPrompt wlMsgPrompt = new frmMsgPrompt();
            wlMsgPrompt.iGuiLeft = g_sCurCfgCtrlSet.sRunAllInfo.iGuiLeft;
            wlMsgPrompt.iGuiWidth = g_sCurCfgCtrlSet.sRunAllInfo.iGuiWidth;
            wlMsgPrompt.iMsgMode = iMsgMode;
            wlMsgPrompt.strMsgPrompt = strMsgPrompt;
            if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
            {
                wlMsgPrompt.Text = "Hint infomation";
                wlMsgPrompt.btOK.Text="Confirm";
            }
            wlMsgPrompt.ShowDialog(this);           
            
        }

        private void frmRun1DutGui_GetInstrInfo(string strMsg)
        {
            int iBeginPos = 0;
            int iEndPos = 0;

            if (g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Contains("MT8870"))
            {
                g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterMode = "MT8870";
                if (strMsg.Contains("ANRITSU") && strMsg.Contains("MT8870A"))
                {
                    string strLogMsg = strMsg;
                    string[] straryRetBufAll = new string[60];
                    straryRetBufAll = strLogMsg.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (straryRetBufAll.Length > 3)
                    {
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterMode = "MT8870";
                        iBeginPos = straryRetBufAll[2].IndexOf(":") + 1;
                        iEndPos = straryRetBufAll[2].Length;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterSN = straryRetBufAll[2].Substring(iBeginPos, iEndPos - iBeginPos);
                        iBeginPos = straryRetBufAll[3].IndexOf(":") + 1;
                        iEndPos = straryRetBufAll[3].Length;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterFW = straryRetBufAll[3].Substring(iBeginPos, iEndPos - iBeginPos);
                    }
                }
            }
            else if (g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Contains("CMW500"))
            {
                g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterMode = "CMW500";
                if (strMsg.Contains("CMW") && strMsg.Contains("Rohde&Schwarz"))
                {
                    string strLogMsg = strMsg;
                    string[] straryRetBufAll = new string[60];
                    straryRetBufAll = strLogMsg.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    iBeginPos = straryRetBufAll[2].IndexOf("/") + 1;
                    iEndPos = straryRetBufAll[2].Length;
                    if (straryRetBufAll.Length > 3)
                    {
                        if (strMsg.Contains("1201.0002k50"))
                        {
                            g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterMode = "CMW500";
                        }
                        else
                        {
                            g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterMode = "CMW280";
                        }
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterSN = straryRetBufAll[2].Substring(iBeginPos, iEndPos - iBeginPos);
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterFW = straryRetBufAll[3];
                    }
                }
            }
            else if (g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Contains("IQXSTREAM"))
            {
                g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterMode = "IQxstream";
                if (strMsg.Contains("LitePoint") && strMsg.Contains("IQxstream"))
                {
                    string strLogMsg = strMsg;
                    string[] straryRetBufAll = new string[60];
                    straryRetBufAll = strLogMsg.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (straryRetBufAll.Length > 3)
                    {
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterMode = "IQxstream";
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterSN = straryRetBufAll[2];
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterFW = straryRetBufAll[3];
                    }
                }
            }
        }

        private void frmRun1DutGui_GetItemInfo(string strMsg)
        {
            string strPromptMsg = "";
            if (strMsg.Contains("Multi-RAT Temperature Sensor start")
                        || strMsg.Contains("Multi-RAT Temperature Sensor end")
                        || strMsg.Contains("GSM & EDGE DTS FHC Enhancement start")
                        || strMsg.Contains("GSM & EDGE DTS FHC Enhancement end")
                        || strMsg.Contains("GSM & EDGE UTS FHC extension start")
                        || strMsg.Contains("GSM & EDGE UTS FHC extension end")
                        || strMsg.Contains("GSM Path Test start")
                        || strMsg.Contains("GSM Path Test end")
                        || strMsg.Contains("GSM NSFT start")
                        || strMsg.Contains("GSM NSFT end")
                        || strMsg.Contains("EDGE NSFT start")
                        || strMsg.Contains("EDGE NSFT end")
                        || strMsg.Contains("TDSCDMA (AST) FHC start")
                        || strMsg.Contains("TDSCDMA (AST) FHC end")
                        || strMsg.Contains("TDSCDMA NSFT start")
                        || strMsg.Contains("TDSCDMA NSFT end")
                        || strMsg.Contains("WCDMA FHC start")
                        || strMsg.Contains("WCDMA FHC end")
                        || strMsg.Contains("WCDMA R99 NSFT start")
                        || strMsg.Contains("WCDMA R99 NSFT end")
                        || strMsg.Contains("LTE FHC start")
                        || strMsg.Contains("LTE FHC end")
                        || strMsg.Contains("LTE Path Test start")
                        || strMsg.Contains("LTE Path Test end")
                        || strMsg.Contains("LTE ETM start")
                        || strMsg.Contains("LTE ETM end")
                        || strMsg.Contains("LTE NSFT start")
                        || strMsg.Contains("LTE NSFT end")
                        || strMsg.Contains("C2K COMMON start")
                        || strMsg.Contains("C2K COMMON end")
                        || strMsg.Contains("C2K DCXO AFC start")
                        || strMsg.Contains("C2K DCXO AFC end")
                        || strMsg.Contains("C2K FHC start")
                        || strMsg.Contains("C2K FHC end")
                        || strMsg.Contains("C2K CS0011 NSFT start"))
            {
                if (strMsg.Contains("Multi-RAT Temperature Sensor start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" calibration is running...", "Multi-RAT Temperature Sensor");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) 
                    {
                        strPromptMsg = string.Format("...\"{0}\"校准中...", "Multi-RAT Temperature Sensor");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("GSM & EDGE DTS FHC Enhancement start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" is running...", "GSM & EDGE DTS FHC Enhancement");                    
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2) 
                    {
                        strPromptMsg = string.Format("...\"{0}\"校准中...", "GSM & EDGE DTS FHC Enhancement");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("GSM & EDGE UTS FHC extension start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" is running...", "GSM & EDGE UTS FHC extension");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2) 
                    {
                        strPromptMsg = string.Format("...\"{0}\"校准中...", "GSM & EDGE UTS FHC extension");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("GSM Path Test start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" is running...", "GSM Path Test");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"校准中...", "GSM Path Test");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("GSM NSFT start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" NSFT is running...", "GSM");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"非信令测试中...", "GSM");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("EDGE NSFT start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" NSFT is running...", "EDGE");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"非信令测试中...", "EDGE");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("TDSCDMA (AST) FHC start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" is running...", "TDSCDMA FHC");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"校准中...", "TDSCDMA FHC");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("TDSCDMA NSFT start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" NSFT is running...", "TDSCDMA");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"非信令测试中...", "TDSCDMA");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("WCDMA FHC start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" is running...", "WCDMA FHC");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"校准中...", "WCDMA FHC");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("WCDMA R99 NSFT start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" NSFT is running...", "WCDMA R99");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"非信令测试中...", "WCDMA R99");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("LTE FHC start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" is running...", "LTE FHC");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"校准中...", "LTE FHC");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("LTE Path Test start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" calibration is running...", "LTE Path");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"校准中...", "LTE Path");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("LTE ETM start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" calibration is running...", "LTE ETM");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"校准中...", "LTE ETM");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("LTE NSFT start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" NSFT is running...", "LTE");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"非信令测试中...", "LTE");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("C2K COMMON start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" calibration is running...", "C2K COMMON");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"校准中...", "C2K COMMON");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("C2K DCXO AFC start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" is running...", "C2K DCXO AFC");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"校准中...", "C2K DCXO AFC");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("C2K FHC start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" is running...", "C2K FHC");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"校准中...", "C2K FHC");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                else if (strMsg.Contains("C2K CS0011 NSFT start"))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                    {
                        strPromptMsg = string.Format("...\"{0}\" is running...", "C2K CS0011 NSFT");
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {
                        strPromptMsg = string.Format("...\"{0}\"非信令测试中...", "C2K CS0011 NSFT");
                    }
                    g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex++;
                }
                frmRun1DutGui_ShowProgressVal(g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex, g_sCurCfgCtrlSet.sRunAllInfo.iItemNum);
                if (strPromptMsg.Length > 0)
                {
                    frmRun1DutGui_ShowCurItem(strPromptMsg);
                }
            }

            //测试步骤
            if (strMsg.Contains("-------") 
                && (strMsg.Contains("start") || strMsg.Contains("end") || strMsg.Contains("begin")))
            {
                //frmRun1DutGui_GetSubProcessStatus(ref strMsg);
                frmRun1DutGui_ShowItemMsg(strMsg);
            }
        }

        private void frmRun1DutGui_GetSubProcessStatus(ref string strMsg)
        {
            int FirAppear =0;                   
            string subSting = "";
            
            string strProcessStatus = strMsg;

            if (strProcessStatus.Contains("lenovo_log#"))
            {
                subSting = string.Format("{0:d4}/{1:d2}/{2:d2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    
                FirAppear = strProcessStatus.IndexOf(subSting, 0);
           
                if (FirAppear !=-1)
                {
                    strMsg =strProcessStatus.Substring(FirAppear);            
                }           
            }                   
        }

        private void frmRun1DutGui_GetCfgInfo(string strMsg)
        {
            //if ((strMsg.Contains("GSM & EDGE DTS FHC Enhancement start")))  
            if ((strMsg.Contains("===== Begin Calibration ====")))
            {
                g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex++;
            }

            if ((strMsg.Contains("lenovo_log#")) && (!strMsg.Contains("Report Generation Time")) && (!strMsg.Contains("2014")))
            {
                string strLogMsg = strMsg;
                string[] straryRetBufAll = new string[60];
                straryRetBufAll = strLogMsg.Split(new char[1] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                if (straryRetBufAll.Length > 1)
                {
                    if (strMsg.Contains("CFG_FILE_PATH"))
                    {
                        string[] straryRetBufUnitAll = new string[60];
                        straryRetBufUnitAll = straryRetBufAll[1].Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        //g_sCurCfgCtrlSet.sRunAllInfo.strCurCalFilePath = straryRetBufUnitAll[1];
                        bool bCurCfgFileExist = false;

                        foreach (string strCurCfgFilePath in g_sCurCfgCtrlSet.sRunAllInfo.straryCurCfgFilePath)
                        {
                            if (strCurCfgFilePath == null)
                            {
                                break;
                            }
                            else
                            {
                                if (strCurCfgFilePath.Contains(straryRetBufUnitAll[1]) && straryRetBufUnitAll[1].Length > 1)
                                {
                                    bCurCfgFileExist = true;
                                    break;
                                }
                            }
                        }

                        if (bCurCfgFileExist == false)
                        {
                            g_sCurCfgCtrlSet.sRunAllInfo.iCurCfgFileNum++;
                            g_sCurCfgCtrlSet.sRunAllInfo.iCurCfgFileIndex++;
                        }
                    }
                    else if (strMsg.Contains("CAL_FILE_PATH"))
                    {
                        string[] straryRetBufUnitAll = new string[60];
                        straryRetBufUnitAll = straryRetBufAll[1].Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        //g_sCurCfgCtrlSet.sRunAllInfo.strCurCalFilePath = straryRetBufUnitAll[1];
                        bool bCurCalFileExist = false;

                        foreach (string strCurCalFilePath in g_sCurCfgCtrlSet.sRunAllInfo.straryCurCalFilePath)
                        {
                            if (strCurCalFilePath == null)
                            {
                                break;
                            }
                            else
                            {
                                if (strCurCalFilePath.Contains(straryRetBufUnitAll[1]) && straryRetBufUnitAll[1].Length > 1)
                                {
                                    bCurCalFileExist = true;
                                    break;
                                }
                            }
                        }

                        if (bCurCalFileExist == false)
                        {
                            g_sCurCfgCtrlSet.sRunAllInfo.straryCurCalFilePath[g_sCurCfgCtrlSet.sRunAllInfo.iCurCalFileIndex] = straryRetBufUnitAll[1];
                            g_sCurCfgCtrlSet.sRunAllInfo.iCurCalFileNum++;
                            g_sCurCfgCtrlSet.sRunAllInfo.iCurCalFileIndex++;
                        }
                    }
                }
            }
        }

        private void frmRun1DutGui_GetResInfo(string strMsg)
        {
            if ((strMsg.Contains("lenovo_log#")) && (!strMsg.Contains("Report Generation Time")) && (!strMsg.Contains("2014")))
            {
                string strLogMsg = strMsg;
                string[] straryRetBufAll = new string[60];
                straryRetBufAll = strLogMsg.Split(new char[1] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                if (straryRetBufAll.Length > 1)
                {
                    clResLogUnit sResUnit = new clResLogUnit();
                    sResUnit.iModemIndex = g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex;
                    sResUnit.strModemIndex = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex);
                    sResUnit.strVal = straryRetBufAll[1];

                    if (straryRetBufAll.Length > 1)
                    {
                        if (strMsg.Contains("CAL") || strMsg.Contains("cal"))
                        {
                            frmRun1DutGui_AddCalConfigInfo(straryRetBufAll[1]);

                            if (g_sCurCfgCtrlSet.sRunAllInfo.iLogCalRes == 1)
                            {
                                frmRun1DutGui_AddCalResMsg(straryRetBufAll[1]);                               
                            }  
                        }
                            //BB Patricio
                        else if (strMsg.ToUpper().Contains("#BB"))
                        {
                            if (g_sCurCfgCtrlSet.sRunAllInfo.iLogBbRes == 1)
                            {
                                frmRun1DutGui_AddBbResMsg(straryRetBufAll[2]);
                                frmRun1DutGui_ShowCurItem(straryRetBufAll[2]);
                                frmRun1DutGui_ShowItemMsg(straryRetBufAll[2]);
                            }
                        }
                            //BB end Patricio
                        else if (strMsg.Contains("NSFT") || strMsg.Contains("nsft"))
                        {
                            if (g_sCurCfgCtrlSet.sRunAllInfo.iLogMeasRes == 1)
                            {
                                if (g_sCurCfgCtrlSet.sRunAllInfo.iRunMode == 0)
                                {
                                    frmRun1DutGui_AddMeasResMsg(straryRetBufAll[1]);
                                }
                                else if (g_sCurCfgCtrlSet.sRunAllInfo.iRunMode == 1)
                                {
                                    if (strMsg.Contains("TXP") || strMsg.Contains("RSRP") || strMsg.Contains("MaxPwr"))
                                    {
                                        frmRun1DutGui_AddMeasResMsg(straryRetBufAll[1]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void tsbRunCfg_Click(object sender, EventArgs e)
        {
            int iStatus = 0;

            try
            {
            switch (g_sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex)
            {
                case 0:
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup0DutComPort;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup0FixturePort;
                    break;
                case 1:
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup1DutComPort;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup1FixturePort;
                    break;
                case 2:
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup2DutComPort;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup2FixturePort;
                    break;
                case 3:
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup3DutComPort;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup3FixturePort;
                    break;
            }

            frmRun1DutCfg wlRunCfg = new frmRun1DutCfg();
            wlRunCfg.strCurExeFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath;
            wlRunCfg.strCurCfgFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strCurCfgFilePath;
            wlRunCfg.strCurEattFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strCurEattFilePath;
            wlRunCfg.straryAllTesterType = g_sCurCfgCtrlSet.sRunAllInfo.straryAllTesterType;
            wlRunCfg.straryAllTesterPort = g_sCurCfgCtrlSet.sRunAllInfo.straryAllTesterPort;
            wlRunCfg.strCurPowerSupplyType = g_sCurCfgCtrlSet.sRunAllInfo.strCurPowerSupplyType;

            wlRunCfg.straryAllDutPortModeSupport = g_sCurCfgCtrlSet.sRunAllInfo.straryAllDutPortModeSupport;
            wlRunCfg.iCurDutPortMode = g_sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode;
            wlRunCfg.iDutPreloaderComPort = g_sCurCfgCtrlSet.sRunAllInfo.iDutPreloaderComPort;
            wlRunCfg.iDutGadgetComPort = g_sCurCfgCtrlSet.sRunAllInfo.iDutGadgetComPort;

            wlRunCfg.straryAllPowerSupplyType = g_sCurCfgCtrlSet.sRunAllInfo.straryAllPowerSupplyType;
            wlRunCfg.straryAllSupportResLevelType = g_sCurCfgCtrlSet.sRunAllInfo.straryAllSupportResLevelType;
            wlRunCfg.strCurResLevel = g_sCurCfgCtrlSet.sRunAllInfo.strCurResLevel;
            wlRunCfg.strCurTesterType = g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterType;
            wlRunCfg.strCurTesterPort = g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterPort;
            wlRunCfg.strCurTesterAddr = g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterAddr;
            wlRunCfg.iLanguageOpt = g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt;
            wlRunCfg.strCurPowerSupplyAddr = g_sCurCfgCtrlSet.sRunAllInfo.strCurPowerSupplyAddr;
            wlRunCfg.iCurPowerChanIndex = g_sCurCfgCtrlSet.sRunAllInfo.iCurPowerChanIndex;
            wlRunCfg.dCurCh0PowerVoltage = g_sCurCfgCtrlSet.sRunAllInfo.dCurCh0PowerVoltage;
            wlRunCfg.dCurCh0PowerCurrent = g_sCurCfgCtrlSet.sRunAllInfo.dCurCh0PowerCurrent;
            wlRunCfg.dCurCh1PowerVoltage = g_sCurCfgCtrlSet.sRunAllInfo.dCurCh1PowerVoltage;
            wlRunCfg.dCurCh1PowerCurrent = g_sCurCfgCtrlSet.sRunAllInfo.dCurCh1PowerCurrent;
            wlRunCfg.strAttnSourceCfgFile1 = g_sCurCfgCtrlSet.sRunAllInfo.strAttnSourceCfgFile1;
            wlRunCfg.strAttnSourceCfgFile2 = g_sCurCfgCtrlSet.sRunAllInfo.strAttnSourceCfgFile2;
            wlRunCfg.strAttnTargetCfgFile1 = g_sCurCfgCtrlSet.sRunAllInfo.strAttnTargetCfgFile1;
            wlRunCfg.strAttnTargetCfgFile2 = g_sCurCfgCtrlSet.sRunAllInfo.strAttnTargetCfgFile2;
            wlRunCfg.iEnableAttnSourceCfgFile1 = g_sCurCfgCtrlSet.sRunAllInfo.iEnableAttnSourceCfgFile1;
            wlRunCfg.iEnableAttnTargetCfgFile1 = g_sCurCfgCtrlSet.sRunAllInfo.iEnableAttnTargetCfgFile1;
            wlRunCfg.iEnableAttnSourceCfgFile2 = g_sCurCfgCtrlSet.sRunAllInfo.iEnableAttnSourceCfgFile2;
            wlRunCfg.iEnableAttnTargetCfgFile2 = g_sCurCfgCtrlSet.sRunAllInfo.iEnableAttnTargetCfgFile2;
            wlRunCfg.iEnablePingPang = g_sCurCfgCtrlSet.sRunAllInfo.iEnablePingPang;
            wlRunCfg.iTimeoutPingPang = g_sCurCfgCtrlSet.sRunAllInfo.iTimeoutPingPang;

            wlRunCfg.dCurAttnMaxValue = g_sCurCfgCtrlSet.sRunAllInfo.dCurAttnMaxValue;
            wlRunCfg.dCurAttnMinValue = g_sCurCfgCtrlSet.sRunAllInfo.dCurAttnMinValue;
            wlRunCfg.dGsmBandDiffMaxValue = g_sCurCfgCtrlSet.sRunAllInfo.dGsmBandDiffMaxValue;
            wlRunCfg.dDeltaMaxValue = g_sCurCfgCtrlSet.sRunAllInfo.dDeltaMaxValue;
            wlRunCfg.iRunCableCalRepeatNum = g_sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatNum;
            wlRunCfg.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort;
            
            wlRunCfg.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort;
            wlRunCfg.iCurDutGroupIndex = g_sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex;
            wlRunCfg.iEnableFixtureControl = g_sCurCfgCtrlSet.sRunAllInfo.iEnableFixtureControl;
            wlRunCfg.iFixtureType = g_sCurCfgCtrlSet.sRunAllInfo.iFixtureType;
            wlRunCfg.iEnablePowerSupplyControl = g_sCurCfgCtrlSet.sRunAllInfo.iEnablePowerSupplyControl;
            wlRunCfg.iGuiWidth = g_sCurCfgCtrlSet.sRunAllInfo.iGuiWidth;
            wlRunCfg.iGuiLeft = g_sCurCfgCtrlSet.sRunAllInfo.iGuiLeft;
            wlRunCfg.iRepeatModeOpt = g_sCurCfgCtrlSet.sRunAllInfo.iRepeatModeOpt;
            wlRunCfg.iFixtureCloseModeOpt = g_sCurCfgCtrlSet.sRunAllInfo.iFixtureCloseModeOpt;
            wlRunCfg.iStopModeOpt = g_sCurCfgCtrlSet.sRunAllInfo.iStopModeOpt;
            wlRunCfg.iRunDutCalDelayTime = g_sCurCfgCtrlSet.sRunAllInfo.iRunDutCalDelayTime;
            wlRunCfg.iRunDutCalRepeatNum = g_sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatNum;
            wlRunCfg.iRunCableCalRepeatNum = g_sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatNum;

            wlRunCfg.ShowDialog(this);
            if (wlRunCfg.bRunDoc1CfgOK == true)
            {
                frmRunDoc1_ExitATE_Demo();

                g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterType = wlRunCfg.strCurTesterType;
                g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterPort = wlRunCfg.strCurTesterPort;
                g_sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode = wlRunCfg.iCurDutPortMode;
                g_sCurCfgCtrlSet.sRunAllInfo.iDutPreloaderComPort = wlRunCfg.iDutPreloaderComPort;
                g_sCurCfgCtrlSet.sRunAllInfo.iDutGadgetComPort = wlRunCfg.iDutGadgetComPort;

                g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterAddr = wlRunCfg.strCurTesterAddr;
                g_sCurCfgCtrlSet.sRunAllInfo.strCurPowerSupplyType = wlRunCfg.strCurPowerSupplyType;
                g_sCurCfgCtrlSet.sRunAllInfo.strCurPowerSupplyAddr = wlRunCfg.strCurPowerSupplyAddr;
                g_sCurCfgCtrlSet.sRunAllInfo.iCurPowerChanIndex = wlRunCfg.iCurPowerChanIndex;
                g_sCurCfgCtrlSet.sRunAllInfo.dCurCh0PowerVoltage = wlRunCfg.dCurCh0PowerVoltage;
                g_sCurCfgCtrlSet.sRunAllInfo.dCurCh0PowerCurrent = wlRunCfg.dCurCh0PowerCurrent;
                g_sCurCfgCtrlSet.sRunAllInfo.dCurCh1PowerVoltage = wlRunCfg.dCurCh1PowerVoltage;
                g_sCurCfgCtrlSet.sRunAllInfo.dCurCh1PowerCurrent = wlRunCfg.dCurCh1PowerCurrent;

                g_sCurCfgCtrlSet.sRunAllInfo.strAttnSourceCfgFile1 = wlRunCfg.strAttnSourceCfgFile1;
                g_sCurCfgCtrlSet.sRunAllInfo.strAttnSourceCfgFile2 = wlRunCfg.strAttnSourceCfgFile2;
                g_sCurCfgCtrlSet.sRunAllInfo.strAttnTargetCfgFile1 = wlRunCfg.strAttnTargetCfgFile1;
                g_sCurCfgCtrlSet.sRunAllInfo.strAttnTargetCfgFile2 = wlRunCfg.strAttnTargetCfgFile2;

                g_sCurCfgCtrlSet.sRunAllInfo.iEnableAttnSourceCfgFile1 = wlRunCfg.iEnableAttnSourceCfgFile1;
                g_sCurCfgCtrlSet.sRunAllInfo.iEnableAttnTargetCfgFile1 = wlRunCfg.iEnableAttnTargetCfgFile1;
                g_sCurCfgCtrlSet.sRunAllInfo.iEnableAttnSourceCfgFile2 = wlRunCfg.iEnableAttnSourceCfgFile2;
                g_sCurCfgCtrlSet.sRunAllInfo.iEnableAttnTargetCfgFile2 = wlRunCfg.iEnableAttnTargetCfgFile2;
                g_sCurCfgCtrlSet.sRunAllInfo.iEnablePingPang = wlRunCfg.iEnablePingPang;
                g_sCurCfgCtrlSet.sRunAllInfo.iTimeoutPingPang = wlRunCfg.iTimeoutPingPang;

                g_sCurCfgCtrlSet.sRunAllInfo.dCurAttnMaxValue = wlRunCfg.dCurAttnMaxValue;
                g_sCurCfgCtrlSet.sRunAllInfo.dCurAttnMinValue = wlRunCfg.dCurAttnMinValue;
                g_sCurCfgCtrlSet.sRunAllInfo.dGsmBandDiffMaxValue = wlRunCfg.dGsmBandDiffMaxValue;
                g_sCurCfgCtrlSet.sRunAllInfo.dDeltaMaxValue = wlRunCfg.dDeltaMaxValue;
                g_sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatNum = wlRunCfg.iRunCableCalRepeatNum;
                g_sCurCfgCtrlSet.sRunAllInfo.iRepeatModeOpt = wlRunCfg.iRepeatModeOpt;
                g_sCurCfgCtrlSet.sRunAllInfo.iFixtureCloseModeOpt = wlRunCfg.iFixtureCloseModeOpt;
                g_sCurCfgCtrlSet.sRunAllInfo.iStopModeOpt = wlRunCfg.iStopModeOpt;
                g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort = wlRunCfg.strCurDutComPort;
                g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort = wlRunCfg.strCurFixtureComPort;
                g_sCurCfgCtrlSet.sRunAllInfo.iRunDutCalDelayTime = wlRunCfg.iRunDutCalDelayTime;
                g_sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatNum = wlRunCfg.iRunDutCalRepeatNum;
                g_sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatNum = wlRunCfg.iRunCableCalRepeatNum;
                g_sCurCfgCtrlSet.sRunAllInfo.iEnableFixtureControl = wlRunCfg.iEnableFixtureControl;
                g_sCurCfgCtrlSet.sRunAllInfo.iFixtureType = wlRunCfg.iFixtureType;
                g_sCurCfgCtrlSet.sRunAllInfo.iEnablePowerSupplyControl = wlRunCfg.iEnablePowerSupplyControl;
                g_sCurCfgCtrlSet.sRunAllInfo.straryAllSupportResLevelType = wlRunCfg.straryAllSupportResLevelType;
                g_sCurCfgCtrlSet.sRunAllInfo.strCurResLevel = wlRunCfg.strCurResLevel;
               

                switch (g_sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex)
                {
                    case 0:
                        g_sCurCfgCtrlSet.sRunAllInfo.strGroup0DutComPort = wlRunCfg.strCurDutComPort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strGroup0FixturePort = wlRunCfg.strCurFixtureComPort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0ExeFilePath = wlRunCfg.strCurExeFilePath;
                        g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0CfgFilePath = wlRunCfg.strCurCfgFilePath;
                        g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0EattFilePath = wlRunCfg.strCurEattFilePath;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup0DutComPort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup0FixturePort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0BbExeFilePath; //  Patricio
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurBbCfgFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0BbCfgFilePath;  // Patricio
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurBbSeqFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0BbSeqFilePath; // Patricio
                        break;
                    case 1:
                        g_sCurCfgCtrlSet.sRunAllInfo.strGroup1DutComPort = wlRunCfg.strCurDutComPort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strGroup1FixturePort = wlRunCfg.strCurFixtureComPort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0ExeFilePath = wlRunCfg.strCurExeFilePath;
                        g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0CfgFilePath = wlRunCfg.strCurCfgFilePath;
                        g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0EattFilePath = wlRunCfg.strCurEattFilePath;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup1DutComPort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup1FixturePort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0BbExeFilePath; //  Patricio
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurBbCfgFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0BbCfgFilePath;  // Patricio
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurBbSeqFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0BbSeqFilePath; // Patricio
                        break;
                    case 2:
                        g_sCurCfgCtrlSet.sRunAllInfo.strGroup2DutComPort = wlRunCfg.strCurDutComPort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strGroup2FixturePort = wlRunCfg.strCurFixtureComPort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0ExeFilePath = wlRunCfg.strCurExeFilePath;
                        g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0CfgFilePath = wlRunCfg.strCurCfgFilePath;
                        g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0EattFilePath = wlRunCfg.strCurEattFilePath;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup2DutComPort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup2FixturePort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0BbExeFilePath; //  Patricio
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurBbCfgFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0BbCfgFilePath;  // Patricio
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurBbSeqFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0BbSeqFilePath; // Patricio
                        break;
                    case 3:
                        g_sCurCfgCtrlSet.sRunAllInfo.strGroup3DutComPort = wlRunCfg.strCurDutComPort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strGroup3FixturePort = wlRunCfg.strCurFixtureComPort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0ExeFilePath = wlRunCfg.strCurExeFilePath;
                        g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0CfgFilePath = wlRunCfg.strCurCfgFilePath;
                        g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0EattFilePath = wlRunCfg.strCurEattFilePath;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup3DutComPort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup3FixturePort;
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0BbExeFilePath; //  Patricio
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurBbCfgFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0BbCfgFilePath;  // Patricio
                        g_sCurCfgCtrlSet.sRunAllInfo.strCurBbSeqFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0BbSeqFilePath; // Patricio
                        break;
                }
                g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeWorkPath = g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath.Remove(g_sCurCfgCtrlSet.sRunAllInfo.strCurBbExeFilePath.LastIndexOf("\\")); //  Patricio

                if (g_sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode == 1)
                {
                    tsbDutPortMode0.Text = "USB";
                    tsbDutPortCom0.Enabled = true;
                    tsbDutPortCom0.Visible = true;
                    tsbDutPortCom0.Text = string.Format("COM{0}", g_sCurCfgCtrlSet.sRunAllInfo.iDutPreloaderComPort);
                    tsbDutPortCom0.ToolTipText = "Dut Preloader Com Port";
                    tsbDutPortCom1.Enabled = true;
                    tsbDutPortCom1.Visible = true;
                    tsbDutPortCom1.Text = string.Format("COM{0}", g_sCurCfgCtrlSet.sRunAllInfo.iDutGadgetComPort);
                    tsbDutPortCom1.ToolTipText = "Dut Gadget Com Port";
                }
                else if (g_sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode == 2)
                {
                    tsbDutPortMode0.Text = "UART";
                    tsbDutPortCom0.Enabled = true;
                    tsbDutPortCom0.Visible = true;
                    tsbDutPortCom0.Text = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort);
                    tsbDutPortCom0.ToolTipText = "Dut Uart Com Port";
                    tsbDutPortCom1.Enabled = false;
                    tsbDutPortCom1.Visible = false;
                }

                tsbFixtureComPort0.Text = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort);
                if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableFixtureControl == 0)
                {
                    tslFixtureComPort.Enabled = false;
                    tslFixtureComPort.Visible = false;
                    tsbFixtureComPort0.Enabled = false;
                    tsbFixtureComPort0.Visible = false;
                    tsbFixtureComPort0.Text = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort);
                }
                else if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableFixtureControl == 1)
                {
                    tslFixtureComPort.Enabled = true;
                    tslFixtureComPort.Visible = true;
                    tsbFixtureComPort0.Enabled = true;
                    tsbFixtureComPort0.Visible = true;
                    tsbFixtureComPort0.Text = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort);
                }

                iStatus = sCfgCtrlRun.SaveMainCfgXml(g_sCurCfgCtrlSet);
                if (iStatus != 0)
                {
                    return;
                }

                iStatus = sCfgCtrlRun.UpdateMtkCustomerSetupTxt(g_sCurCfgCtrlSet);
                if (iStatus != 0)
                {
                    return;
                }
                iStatus = sCfgCtrlRun.UpdateMtkSetupIni_All(g_sCurCfgCtrlSet);
                if (iStatus != 0)
                {
                    return;
                }
                iStatus = sCfgCtrlRun.UpdateMtkCfgTxt(g_sCurCfgCtrlSet);
                if (iStatus != 0)
                {
                    return;
                }
                if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableBbTest == 1) // Patricio BB
                {
                    iStatus = sCfgCtrlRun.UpdateBbSetupIni_All(g_sCurCfgCtrlSet);
                    if (iStatus != 0)
                    {
                        return;
                    }
                }
            }
        }
            catch(Exception exep)
            {
                MessageBox.Show(exep.Message, "tsbRunCfg_Click Failed");            
            }
        }

        public int frmRunDoc1_SaveSettings(string strName, string strValue)
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

        public int frmRunDoc1_FetchSettings(string strName, ref string strValue)
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

        private void frmRunDoc1_Load(object sender, EventArgs e)
        {
            string strPromptMsg = "";
            int iStatus = 0;
            if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
            {
                this.tpRunStatDut0.Text = "Current test status";
                this.tpRunResDut0.Text = "Detail test result";
                this.tslDutPassNumPrompt.Text = "PASS:";
                this.tslDutFailNumPrompt.Text = "FAIL:";
                this.tslDutFailRatePrompt.Text = "  FR:";
                this.tslDutComPort.Text = "Phone:";
                this.tslFixtureComPort.Text = "Fixture:";
                this.btRunStart.Text = "Start";      
            }
            else
            {
                this.tpRunStatDut0.Text = "测试状态";
                this.tpRunResDut0.Text = "详细结果";
                this.tslDutPassNumPrompt.Text = "通过:";
                this.tslDutFailNumPrompt.Text = "失败:";
                this.tslDutFailRatePrompt.Text = "失败率:";
                this.tslDutComPort.Text = "手机:";
                this.tslFixtureComPort.Text = "夹具:";
                this.btRunStart.Text = "开始测试"; 
            }           

            tsbRunExit.Enabled = false;
            tsbRunStop.Enabled = false;
            tsbRunStartCalibrateMain.Enabled = false;

            if (!Directory.Exists(".\\log"))
            {
                Directory.CreateDirectory(".\\log");
            }

            if (!Directory.Exists(".\\log\\mtk"))
            {
                Directory.CreateDirectory(".\\log\\mtk");
            }

            if (!Directory.Exists(".\\mtk"))
            {
                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                {
                    MessageBox.Show("There is no mtk folder, please check!");
                }
                else
                {
                    MessageBox.Show("测试工具中没有包括mtk工具包，请检查!");
                }
                Application.Exit();
                return;
            }

            if (!Directory.Exists(".\\mtk\\cfg"))
            {
                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                {
                    MessageBox.Show("There is no mtk cfg folder, please check!");
                }
                else
                {
                    MessageBox.Show("mtk工具包没有包括cfg文件夹，请检查!");
                }
                Application.Exit();
                return;
            }

            if (!Directory.Exists(".\\mtk\\cfg\\mode_dut_cal_ver"))
            {
                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                {
                    MessageBox.Show("There is no mtk cfg mode_dut_cal_ver folder, please check!");
                }
                else
                {
                    MessageBox.Show("cfg文件夹没有包括mode_dut_cal_ver文件夹，请检查!");
                }
                Application.Exit();
                return;
            }

            frmRunDoc1_FetchSettings("login_db", ref g_sCurCfgCtrlSet.sRunAllInfo.strLoginDb);
            frmRunDoc1_FetchSettings("db_connect", ref g_sCurCfgCtrlSet.sRunAllInfo.strConnectDb);

            if (g_sCurCfgCtrlSet.sRunAllInfo.strLoginDb.Length < 1)
            {
                g_sCurCfgCtrlSet.sRunAllInfo.strLoginDb = "0";
            }

            if (g_sCurCfgCtrlSet.sRunAllInfo.strConnectDb.Length < 1)
            {
                g_sCurCfgCtrlSet.sRunAllInfo.strConnectDb = "0";
            }
          
            g_sCurCfgCtrlSet.sRunAllInfo.strLoginDb = "1";
            g_sCurCfgCtrlSet.sRunAllInfo.strConnectDb = "1";

            if ((Int32.Parse(g_sCurCfgCtrlSet.sRunAllInfo.strLoginDb) == 1) && (Int32.Parse(g_sCurCfgCtrlSet.sRunAllInfo.strConnectDb) == 0))
            {
                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
                {
                    strPromptMsg = string.Format("{0}:{1}", "error", "can't access the database");
                }
                else
                {
                    strPromptMsg = string.Format("{0}:{1}", "错误提示", "未成功登陆数据库");
                }
                frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
                this.Close();
            }

            tscbRunMode.Items.Clear();
            if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
            {
                tscbRunMode.Items.Add("cal & ver mode ");
                tscbRunMode.Items.Add("cal cableloss mode");
            }
            else
            {
                tscbRunMode.Items.Add("手机校准&非信令测试模式");
                tscbRunMode.Items.Add("夹具线损校准模式");
            }
            tscbRunMode.SelectedIndex = 0;          //0

            frmRunDoc1_FetchSettings("db_account", ref g_sCurCfgCtrlSet.sRunAllInfo.strDbAccount);
            frmRunDoc1_FetchSettings("db_password", ref g_sCurCfgCtrlSet.sRunAllInfo.strDbPassword);
            frmRunDoc1_FetchSettings("db_name", ref g_sCurCfgCtrlSet.sRunAllInfo.strDbName);
            frmRunDoc1_FetchSettings("db_rank", ref g_sCurCfgCtrlSet.sRunAllInfo.strDbRank);
            frmRunDoc1_FetchSettings("db_database_name", ref g_sCurCfgCtrlSet.sRunAllInfo.strDbDatabaseName);
            frmRunDoc1_FetchSettings("db_group", ref g_sCurCfgCtrlSet.sRunAllInfo.strDbGroup);
            frmRunDoc1_FetchSettings("db_pc_name", ref g_sCurCfgCtrlSet.sRunAllInfo.strDbPcName);
            frmRunDoc1_FetchSettings("db_ms_type", ref g_sCurCfgCtrlSet.sRunAllInfo.strDbMsType);
            frmRunDoc1_FetchSettings("db_ms_type_name", ref g_sCurCfgCtrlSet.sRunAllInfo.strDbMsTypeName);

            g_hwndFrmRun1DutGui = this.Handle;

            switch (g_sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex)
            {
                case 0:
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0ExeFilePath;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurCfgFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0CfgFilePath;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurEattFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0EattFilePath;
                    break;
                case 1:
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0ExeFilePath;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurCfgFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0CfgFilePath;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurEattFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0EattFilePath;
                    break;
                case 2:
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0ExeFilePath;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurCfgFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0CfgFilePath;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurEattFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0EattFilePath;
                    break;
                case 3:
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0ExeFilePath;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurCfgFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0CfgFilePath;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurEattFilePath = g_sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0EattFilePath;
                    break;
            }
            g_sCurCfgCtrlSet.sRunAllInfo.strCurExeWorkPath = g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath.Remove(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath.LastIndexOf("\\"));

            if (g_sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode == 1)
            {
                tsbDutPortMode0.Text = "USB";
                tsbDutPortCom0.Enabled = true;
                tsbDutPortCom0.Visible = true;
                tsbDutPortCom0.Text = string.Format("COM{0}", g_sCurCfgCtrlSet.sRunAllInfo.iDutPreloaderComPort);
                tsbDutPortCom0.ToolTipText = "Dut Preloader Com Port";
                tsbDutPortCom1.Enabled = true;
                tsbDutPortCom1.Visible = true;
                tsbDutPortCom1.Text = string.Format("COM{0}", g_sCurCfgCtrlSet.sRunAllInfo.iDutGadgetComPort);
                tsbDutPortCom1.ToolTipText = "Dut Gadget Com Port";
            }
            else if (g_sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode == 2)
            {
                tsbDutPortMode0.Text = "UART";
                tsbDutPortCom0.Enabled = true;
                tsbDutPortCom0.Visible = true;
                tsbDutPortCom0.Text = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort);
                tsbDutPortCom0.ToolTipText = "Dut Uart Com Port";
                tsbDutPortCom1.Enabled = false;
                tsbDutPortCom1.Visible = false;
            }

            tsbFixtureComPort0.Text = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort);
            if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableFixtureControl == 0)
            {
                tslFixtureComPort.Enabled = false;
                tslFixtureComPort.Visible = false;
                tsbFixtureComPort0.Enabled = false;
                tsbFixtureComPort0.Visible = false;
                tsbFixtureComPort0.Text = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort);
            }
            else if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableFixtureControl == 1)
            {
                tslFixtureComPort.Enabled = true;
                tslFixtureComPort.Visible = true;
                tsbFixtureComPort0.Enabled = true;
                tsbFixtureComPort0.Visible = true;
                tsbFixtureComPort0.Text = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort);
            }

            frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);

            iStatus = sCfgCtrlRun.UpdateMtkCustomerSetupTxt(g_sCurCfgCtrlSet);

            if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ")  // Patricio
            {
                StringBuilder strErrorMessage = new StringBuilder(256);
                strErrorMessage.Insert(0, "Fail to initialize BZ Equipments");

                if (iStatus == 0)
                    iStatus = LogResult.LoadLogResult(strErrorMessage.ToString());

                if (iStatus != 0)
                {
                    MessageBox.Show("Fail to initialize LOG RESULT");
                    this.Close();
                    return;
                }

                else
                    StartTimer();  // Entrar no LABEL VERIFY & Close JIG Patricio
            }

            if (iStatus != 0)
            {
                return;
            }
            iStatus = sCfgCtrlRun.UpdateMtkSetupIni_All(g_sCurCfgCtrlSet);
            if (iStatus != 0)
            {
                return;
            }
            iStatus = sCfgCtrlRun.UpdateMtkCfgTxt(g_sCurCfgCtrlSet);
            if (iStatus != 0)
            {
                return;
            }

            tsslRunStatus.AutoSize = true;
           
        }

        private void frmRunDoc1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (iOwnTheMutex) // "BZ" Scan Mutex release  // Patricio
                ScanMutex.ReleaseMutex();

            if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ")
            {
                Bz_Handler.CI2cControl.ReleaseGPIBMutex();
                labelTestStatusDut0.Text = "ReleaseGPIBMutex \n Test Set Liberado";
                Application.DoEvents();
            }
            //frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);
            //sExeCtrl.bfixtureContinueDetect = false;

            frmRunDoc1_ExitATE_Demo();
        }

        private void frmRunDoc1_ExitATE_Demo()
        {
            if (g_bAppRebootFlag)
            {
                g_bAppRebootFlag = false;
                g_proIntelEngineCalibrateMain.StandardInput.WriteLine(@"##########CMD_ATEDEMO_APP_QUIT##########");

                Thread.Sleep(500);
            }

            frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath); 
        }

        static TimeSpan waitTime = new TimeSpan(0, 0, 10);

        private void frmRun1DutGui_CheckRepeatTime(clCfgCtrlSet sCurCfgCtrlSet)
        {
            if (sCurCfgCtrlSet.sRunAllInfo.iRunMode == 0)                     //正常测试模式
            {
                if (sCurCfgCtrlSet.sRunAllInfo.iRepeatModeOpt == 1)           //单次测试模式
                {
                    frmRun1DutGui_ShowProgressVal(g_sCurCfgCtrlSet.sRunAllInfo.iItemNum, g_sCurCfgCtrlSet.sRunAllInfo.iItemNum);
                    tsbRunCfg.Enabled = true;
                    btRunStart.Enabled = true;
                    g_sCurCfgCtrlSet.sRunAllInfo.bFirstRun = true;
                    sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatIndex = 0;
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.iRepeatModeOpt == 2)      //重复测试模式
                {
                    if (sCurCfgCtrlSet.sRunAllInfo.iStopModeOpt == 1)          //失败后停止测试
                    {
                        if ((sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatIndex < sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatNum)
                            && ((g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != 0) && (g_sCurCfgCtrlSet.sRunAllInfo.bCalMainDone)))
                        {
                            Thread.Sleep(g_sCurCfgCtrlSet.sRunAllInfo.iRunDutCalDelayTime * 1000);
                            //Thread.Sleep(2000);
							tsbRunCfg.Enabled = true;
                            btRunStart.Enabled = true;
                            btRunStart.PerformClick();
                            g_sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatIndex++;
                        }
                        else
                        {
                            frmRun1DutGui_ShowProgressVal(g_sCurCfgCtrlSet.sRunAllInfo.iItemNum, g_sCurCfgCtrlSet.sRunAllInfo.iItemNum);
                            tsbRunCfg.Enabled = true;
                            btRunStart.Enabled = true;
                            g_sCurCfgCtrlSet.sRunAllInfo.bFirstRun = true;
                            sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatIndex = 0;
                        }
                    }
                    else if (sCurCfgCtrlSet.sRunAllInfo.iStopModeOpt == 2)     //失败后继续测试
                    {
                        if (sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatIndex < sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatNum)
                        {
                            Thread.Sleep(sCurCfgCtrlSet.sRunAllInfo.iRunDutCalDelayTime * 1000);
                            //Thread.Sleep(2000);
							tsbRunCfg.Enabled = true;
                            btRunStart.Enabled = true;
                            btRunStart.PerformClick();
                            sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatIndex++;

                            string strPromptMsg = string.Format("{0:yyyy/MM/dd HH:mm:ss.fff}  开始下一次测试", DateTime.Now);
                            
                            frmRun1DutGui_ShowCurStatus(strPromptMsg);
                            frmRun1DutGui_ShowItemMsg(strPromptMsg);
                        }
                        else
                        {
                            frmRun1DutGui_ShowProgressVal(g_sCurCfgCtrlSet.sRunAllInfo.iItemNum, g_sCurCfgCtrlSet.sRunAllInfo.iItemNum);
                            tsbRunCfg.Enabled = true;
                            btRunStart.Enabled = true;
                            g_sCurCfgCtrlSet.sRunAllInfo.bFirstRun = true;
                            sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatIndex = sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatNum;
                        }
                    }
                }
            }
            else if (sCurCfgCtrlSet.sRunAllInfo.iRunMode == 1)       //线损校准模式
            {
                if (sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatIndex < sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatNum)
                {
                    if (sCurCfgCtrlSet.sRunAllInfo.bRunCableCalPassed)
                    {
                        if (sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatIndex == 0)
                        {
                            frmRun1DutGui_DeleteAttnResFiles();
                        }

                        frmRun1DutGui_UpdateAttnResFiles();
                        sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatIndex++;
                    }

                    //Thread.Sleep(sCurCfgCtrlSet.sRunAllInfo.iRunDutCalDelayTime * 1000);
                    //btRunStart.Enabled = true;
                    //btRunStart.PerformClick();

                    if (sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatIndex == sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatNum)
                    {
                        tsbRunCfg.Enabled = true;
                        btRunStart.Enabled = true;
                        g_sCurCfgCtrlSet.sRunAllInfo.bFirstRun = true;
                        frmRun1DutGui_UpdateAttnCfgFiles();
                        sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatIndex = 0;
                        frmRun1DutGui_ShowProgressVal(g_sCurCfgCtrlSet.sRunAllInfo.iItemNum, g_sCurCfgCtrlSet.sRunAllInfo.iItemNum);
                    }
                    else
                    {
                        //Thread.Sleep(sCurCfgCtrlSet.sRunAllInfo.iRunDutCalDelayTime * 1000);
                        Thread.Sleep(2000); 
						btRunStart.Enabled = true;
                        btRunStart.PerformClick();
                    }
                }
                //else if (sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatIndex == sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatNum - 1)
                //{
                //    if (sCurCfgCtrlSet.sRunAllInfo.bRunCableCalPassed)
                //    {
                //        frmRun1DutGui_UpdateAttnResFiles();
                //        sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatIndex++;
                //    }                    
                //    return;
                //}                
            }
            return;
        }

        private void frmRun1DutGui_SetErrMsg(int iErrCode, string strErrMsg)
        {
            if (iErrCode != 0)
            {
                if ((-410000 < iErrCode) && (iErrCode < 0))
                {
                    iErrCode = iErrCode - 410000;
                }
                else if (0 < iErrCode)
                {
                    iErrCode = (-1) * iErrCode - 410000;
                }
            }

            g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus = iErrCode;

            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.iErrCode = iErrCode;

            foreach (clCfgErrSetUnit sErrSetUnit in g_sCurCfgCtrlSet.listErrComSet)
            {
                if (Int32.Parse(sErrSetUnit.strLenovoErrorCode) == g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.iErrCode)
                {
                    g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strChipErrorCode = sErrSetUnit.strChipErrorCode;
                    g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strLenovoErrorCode = sErrSetUnit.strLenovoErrorCode;
                    g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscEng = sErrSetUnit.strDiscEng;
                    g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscChs = sErrSetUnit.strDiscChs;
                }
            }

            if (g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscEng.Length == 0)
            {
                g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscEng = strErrMsg;
            }
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.iErrUpdate = 1;
        }

        private void frmRun1DutGui_SetErrMsgDefualt()
        {     
            g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus = 0;
            
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.iErrCode = 0;          
             
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strChipErrorCode = "";           
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strLenovoErrorCode = "";             
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscEng = "";             
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscChs = "";    
        }

        private void frmRun1DutGui_ShowFailMsg()
        {
            string strCurErrMsg = "";
            string strErrDisc = "";

            if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
            {
                strErrDisc = string.Format("{0}", g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscEng);
            }
            else
            {
                if (g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscChs.Length>0)
                {
                    if(g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscChs.Contains("err_comport_fail"))
                    {                
                        strErrDisc = string.Format("{0}", g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscEng);
                    }
                    else
                    {                
                        strErrDisc = string.Format("{0}", g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscChs);
                    }
                }
                else
                {                       
                    strErrDisc = string.Format("{0}", g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscEng);
                }
            }

            if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
            {
                strCurErrMsg = string.Format("FAIL\r\nError Code:{0}->{1}", g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.iErrCode, strErrDisc);
            }
            else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2) 
            {
                strCurErrMsg = string.Format("失败\r\n错误代码:{0}->{1}", g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.iErrCode, strErrDisc);
            }

            labelTestStatusDut0.Text = strCurErrMsg;
            labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
            labelTestStatusDut0.BackColor = System.Drawing.Color.Red;

            frmRun1DutGui_ShowSummaryVal(g_sCurCfgCtrlSet.sRunAllInfo.iDutPassNum, g_sCurCfgCtrlSet.sRunAllInfo.iDutFailNum);

            frmRun1DutGui_ShowCurStatus("Process finished");
        }

        private void frmRun1DutGui_ShowPassMsg()
        {
            if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) //English
            {
                labelTestStatusDut0.Text = "PASS";
            }
            else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2) 
            {
                labelTestStatusDut0.Text = "通过";
            }
            labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
            labelTestStatusDut0.BackColor = System.Drawing.Color.Green;

            frmRun1DutGui_ShowSummaryVal(g_sCurCfgCtrlSet.sRunAllInfo.iDutPassNum, g_sCurCfgCtrlSet.sRunAllInfo.iDutFailNum);

            frmRun1DutGui_ShowCurStatus("Process finished");
        }

        private void frmRun1DutGui_ShowCurItem(string strStatusMsg)
        {
            labelTestStatusDut0.Text = strStatusMsg;

            Application.DoEvents();

            //Thread.Sleep(100);
            
            labelTestStatusDut0.Refresh();

            frmRun1DutGui_ShowCurStatus(strStatusMsg);            
        }

        private void frmRun1DutGui_ShowCurStatus(string strStatusMsg)
        {
            tsslRunStatus.Text = strStatusMsg;

            Application.DoEvents();

            //Thread.Sleep(100);

            this.Refresh();
        }

        private void frmRun1DutGui_ShowItemMsg(string strItemMsg)
        {
            strItemMsg = strItemMsg.Replace("\r\n", "");
            if (strItemMsg.Length > 0)
            {
                rtbRunItemDut0.AppendText(strItemMsg);
                rtbRunItemDut0.AppendText("\r\n");
                rtbRunItemDut0.ScrollToCaret();
                clSumUnit sSumUnit = new clSumUnit();
                sSumUnit.strLog = strItemMsg;
                g_sCurCfgCtrlSet.sRunLogInfo.listRunSumLog.Add(sSumUnit);
                if (streamSumLogFile != null)
                {
                    streamSumLogFile.WriteLine(sSumUnit.strLog);
                }
                frmRun1DutGui_ShowLogMsg(strItemMsg);
            }
        }

        private void frmRun1DutGui_ShowLogMsg(string strLogMsg)
        {
            strLogMsg = strLogMsg.Replace("\r\n", "");
            if (strLogMsg.Length > 0)
            {
                if (!strLogMsg.Contains("CAL_RES"))
                {
                    rtbRunLogDut0.AppendText(strLogMsg);
                    rtbRunLogDut0.AppendText("\r\n");
                    rtbRunLogDut0.ScrollToCaret();
                }
            }
        }
        
        private void frmRun1DutGui_ShowSN(string strSN)
        {
              
            if (strSN == frmBzModel.strTrackId.ToUpper()) // Patricio Compare Trackid
            {
                LogResult.AddLogResult("TRCKIDRFY", "COMPARE_TRACKID", "0", "0", "0", "0", "0", 0, "", "");
                g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "PASS";
                labelSN.Text = string.Format("SN: {0}", strSN);
            }
            else
            {
                int nStatus = 0;
                labelSN.Text = string.Format("SN: {0}", strSN);
                string strPromptMsg = "";
                nStatus = Bz_Handler.CI2cControl.ReleaseGPIBMutex();
                labelTestStatusDut0.Text = "ReleaseGPIBMutex \n Test Set Liberado";
               
                // MQS log
                //string strErrorMessage = string.Empty;
                //TPWrapper.LogDataAcquisition logProcess = new TPWrapper.LogDataAcquisition();
                //EndRecipeParameters parameters = new EndRecipeParameters();
                //LogResult.AddLogResult("TRCKIDRFY", "COMPARE_TRACKID", "0", "0", "0", "0", "0", 1, "", "");
                //g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                //parameters.trackId = Bz_Handler.CJagLocalFucntions.GetTrackId();
                //parameters.masterTestResult = TestResult.Fail;
                //logProcess.EndRecipe(parameters, out strErrorMessage);
                // MQS log end
                g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                g_sCurCfgCtrlSet.sRunAllInfo.iDutFailNum++;
                if (g_sCurCfgCtrlSet.sRunAllInfo.iRestartATEDemo == 1)
                {
                    g_sCurCfgCtrlSet.sRunAllInfo.iContinueFailCnt = 0;
                }
                frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);  // Finalizando app MTK_atedemo.exe
                frmRun1DutGui_WriteResFiles(g_sCurCfgCtrlSet);
                frmRun1DutGui_WriteInfoFiles(g_sCurCfgCtrlSet);
                frmRun1DutGui_CloseLogFiles();   // Fechando Log MTK
                g_bAppRebootFlag = false;
                nStatus = Bz_Handler.CJagLocalFucntions.CloseTunerDVM();
                strPromptMsg = string.Format("!!! TRACKID INTERNO DIFERENTE DO ESCANEADO RETIRE A UNIDADE DO FIXTURE !!!");
                frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
                if (Bz_Handler.CItemListEquip.IsOpenJigEnable() == 1)
                {
                    if (nStatus == 0)
                    {
                        double dVoltage = -999;
                        int nCloseJigCount = 0;
                        string strPreviewMessage;
                        strPreviewMessage = labelTestStatusDut0.Text.ToString();
                        labelTestStatusDut0.Text = "!!!REMOVA O TELEFONE DO FIXTURE!!! \n \n" + strPreviewMessage;
                        Application.DoEvents();
                        nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_CHARLES_CLOSE");
                        if (nStatus == 0)
                        {
                            //while (dVoltage < 0.2)
                                while (dVoltage < 2)
                            {
                                dVoltage = Bz_Handler.CItemListEquip.ReadDVM1Voltage();

                                if (nCloseJigCount++ % 2 == 0)
                                {
                                    labelTestStatusDut0.ForeColor = System.Drawing.Color.White;
                                    labelTestStatusDut0.BackColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                                    labelTestStatusDut0.BackColor = System.Drawing.Color.White;
                                }
                                Application.DoEvents();
                            }
                        }
                        nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_CHARLES_OPEN");
                        nStatus = Bz_Handler.CJagLocalFucntions.OpenTunerDVM();

                            labelTestStatusDut0.Text = "!!!TRACKID INTERNO DIFERENTE DO ESCANEADO!!! ";
                            labelTestStatusDut0.BackColor = System.Drawing.Color.Red;                        
                            labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                            Application.DoEvents();
                    }
                }
                StartScanTimer.Enabled = true;
                return;
             }
                     //labelSN.Text = string.Format("SN: {0}", strSN);
        } // Patricio End Compare Trackid

        private void frmRun1DutGui_ShowProgressVal(int iCurTestItemIndex, int iCurAllItemNum)
        {
            int iProgressValue = (100 * iCurTestItemIndex) / iCurAllItemNum;
            if (iProgressValue < 0)
            {
                pbProgress.Value = 0;
            }
            else if (iProgressValue > 100)
            {
                pbProgress.Value = 100;
            }
            else
            {
                pbProgress.Value = (100 * iCurTestItemIndex) / iCurAllItemNum;
            }
            labelProgress.Text = string.Format("{0:000.00}%", pbProgress.Value);
        }

        private void frmRun1DutGui_ShowSummaryVal(int iDutPassNum, int iDutFailNum)
        {
            double dDutFailRate = 0.00;
            if ((iDutFailNum + iDutPassNum) == 0)
            {
                dDutFailRate = 0.00;
            }
            else
            {
                dDutFailRate = double.Parse(iDutFailNum.ToString()) * 100.00 / (double.Parse(iDutFailNum.ToString()) + double.Parse(iDutPassNum.ToString()));
            }
            tstbDutPassNum.Text = string.Format("{0}", iDutPassNum);
            tstbDutFailNum.Text = string.Format("{0}", iDutFailNum);
            tstbDutFailRate.Text = string.Format("{0:000.00}%", dDutFailRate);
        }

        private int frmRun1DutGui_InitLogFiles()
        {
            string strCurrentDirectory = "";
            string strPromptMsg = "";
            int iStatus = 0;
            if (streamTraceLogFile != null)
            {
                streamTraceLogFile.Close();
                streamTraceLogFile = null;
            }
            if (streamResLogFile != null)
            {
                streamResLogFile.Close();
                streamResLogFile = null;
            }
            if (streamSumLogFile != null)
            {
                streamSumLogFile.Close();
                streamSumLogFile = null;
            }
            try
            {
                strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                strOrgSumLogFile = strCurrentDirectory + "\\Log\\SumLog.txt";
                strOrgResLogFile = strCurrentDirectory + "\\Log\\ResLog.txt";
                strOrgTraceLogFile = strCurrentDirectory + "\\Log\\TraceLog.txt";
                streamTraceLogFile = new StreamWriter(strOrgTraceLogFile, false, System.Text.Encoding.Default);
                streamResLogFile = new StreamWriter(strOrgResLogFile, false, System.Text.Encoding.Default);
                streamSumLogFile = new StreamWriter(strOrgSumLogFile, false, System.Text.Encoding.Default);
            }
            catch (Exception exc)
            {
                if (streamTraceLogFile == null)
                {
                    strPromptMsg = string.Format("{0}:{1}", "Init trace file failed!", exc.Message);
                    frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
                }
                if (streamResLogFile == null)
                {
                    strPromptMsg = string.Format("{0}:{1}", "Init result file failed!", exc.Message);
                    frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
                }
                if (streamSumLogFile == null)
                {
                    strPromptMsg = string.Format("{0}:{1}", "Init summary file failed!", exc.Message);
                    frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
                }
                iStatus = -1;
            }

            return iStatus;
        }

        private int frmRun1DutGui_InsertCfgCableLossSet(clCfgCtrlSet sCfgCtrlSet, string strLine)
        {
            int iStatus = 0;
            clResCfgUnit sResCfgUnit = new clResCfgUnit();
            sResCfgUnit.iModemIndex = g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex;
            sResCfgUnit.strModemIndex = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex);
            sResCfgUnit.strVal = string.Format("CFG_SET_CABLELOSS, {0}", strLine);
            sResCfgUnit.strVal = sResCfgUnit.strVal.Replace("=", ",");
            g_sCurCfgCtrlSet.sRunLogInfo.listRunCfgKeySet.Add(sResCfgUnit);
            frmRun1DutGui_ShowLogMsg(sResCfgUnit.strVal);
            return iStatus;
        }

        private int frmRun1DutGui_InsertCfgKeySet(clCfgCtrlSet sCfgCtrlSet, string strLine)
        {
            int iStatus = 0;
            clResCfgUnit sResCfgUnit = new clResCfgUnit();
            sResCfgUnit.iModemIndex = g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex;
            sResCfgUnit.strModemIndex = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex);
            sResCfgUnit.strVal = string.Format("CFG_SET_CAL, {0}", strLine);
            sResCfgUnit.strVal = sResCfgUnit.strVal.Replace("=", ",");
            g_sCurCfgCtrlSet.sRunLogInfo.listRunCfgKeySet.Add(sResCfgUnit);
            frmRun1DutGui_ShowLogMsg(sResCfgUnit.strVal);
            return iStatus;
        }

        private int frmRun1DutGui_ReadCfgFiles(clCfgCtrlSet sCfgCtrlSet)
        {
            int iStatus = 0;
            try
            {
                string strLine = "";
                foreach (string strCurCfgFilePath in g_sCurCfgCtrlSet.sRunAllInfo.straryCurCfgFilePath)
                {
                    if (File.Exists(strCurCfgFilePath))
                    {
                        StreamReader streamCalFile = new StreamReader(strCurCfgFilePath, false);
                        if (streamCalFile != null)
                        {
                            clResCfgUnit sResCfgUnit = new clResCfgUnit();
                            sResCfgUnit.iModemIndex = g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex;
                            sResCfgUnit.strModemIndex = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex);
                            sResCfgUnit.strVal = string.Format("CFG_FILE, {0}", strCurCfgFilePath);
                            g_sCurCfgCtrlSet.sRunLogInfo.listRunCfgKeySet.Add(sResCfgUnit);
                            frmRun1DutGui_ShowLogMsg(sResCfgUnit.strVal);

                            string strLineUpper = "";
                            string strLineOrg = "";
                            bool bFindCableLossSet2G3G = false;
                            bool bFindCableLossSet4G = false;
                            while ((strLine = streamCalFile.ReadLine()) != null)
                            {
                                strLineUpper = strLine.ToUpper();
                                strLineOrg = strLine;

                                if (g_sCurCfgCtrlSet.sRunAllInfo.iLogCfgLoss == 1)
                                {
                                    //read cable loss  

                                    if (strLineUpper.Contains("[")
                                        && strLineUpper.Contains("]")
                                        && strLineUpper.Contains("INITIALIZATION")
                                        && (strLineUpper.Contains(g_sCurCfgCtrlSet.sRunAllInfo.strCurTesterType) == true))
                                    {
                                        bFindCableLossSet2G3G = true;
                                        frmRun1DutGui_InsertCfgCableLossSet(sCfgCtrlSet, strLine);
                                        continue;
                                    }
                                    if (strLineOrg.Contains("[LTE Cable Attenuation]")
                                        || strLineOrg.Contains("[LTE Cable Attenuation Mapping]"))
                                    {
                                        bFindCableLossSet4G = true;
                                        frmRun1DutGui_InsertCfgCableLossSet(sCfgCtrlSet, strLine);
                                        continue;
                                    }

                                    if (bFindCableLossSet2G3G)
                                    {
                                        if (strLine.Contains("cable loss"))
                                        {
                                            frmRun1DutGui_InsertCfgCableLossSet(sCfgCtrlSet, strLine);
                                        }
                                    }

                                    if (bFindCableLossSet4G && (strLine.Contains("Port") || strLine.Contains("Map_Table")))
                                    {
                                        frmRun1DutGui_InsertCfgCableLossSet(sCfgCtrlSet, strLine);
                                    }

                                    if (bFindCableLossSet2G3G && strLineUpper.Contains("[") && strLineUpper.Contains("]"))
                                    {
                                        bFindCableLossSet2G3G = false;
                                    }

                                    if (bFindCableLossSet4G && strLineUpper.Contains("[") && strLineUpper.Contains("]"))
                                    {
                                        bFindCableLossSet4G = false;
                                    }
                                }

                                //read key set
                                if (g_sCurCfgCtrlSet.sRunAllInfo.iLogCfgSet == 1)
                                {
                                    if (strLine.Contains("MAX_INIT_AFC_DAC")
                                        || strLine.Contains("MIN_INIT_AFC_DAC")
                                        || strLine.Contains("MAX_AFC_SLOPE")
                                        || strLine.Contains("MIN_AFC_SLOPE")
                                        || strLine.Contains("MAX_CRYSTAL_CAL_CAP_ID")
                                        || strLine.Contains("MIN_CRYSTAL_CAL_CAP_ID")
                                        || strLine.Contains("CRYSTAL_AFC_MAX_CAP_ID")
                                        || strLine.Contains("CRYSTAL_AFC_MIN_CAP_ID")
                                        || strLine.Contains("MAX_RX_LOSS")
                                        || strLine.Contains("MIN_RX_LOSS"))
                                    {
                                        frmRun1DutGui_InsertCfgKeySet(sCfgCtrlSet, strLine);
                                    }
                                }
                            }

                            if (streamCalFile != null)
                            {
                                streamCalFile.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "frmRun1DutGui_ReadCfgFiles Failed!");
                iStatus = -1;
            }
            return iStatus;
        }

        private int frmRun1DutGui_ReadMtkCalFiles(clCfgCtrlSet sCfgCtrlSet)
        {
            int iStatus = 0;
            string strLogMsg = "";
            try
            {
                string strLine = "";
                foreach (string strCurCalFilePath in g_sCurCfgCtrlSet.sRunAllInfo.straryCurCalFilePath)
                {
                    if (File.Exists(strCurCalFilePath))
                    {
                        StreamReader streamCalFile = new StreamReader(strCurCalFilePath, false);
                        if (streamCalFile != null)
                        {
                            clResLogUnit sResLogUnit = new clResLogUnit();
                            sResLogUnit.iModemIndex = g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex;
                            sResLogUnit.strModemIndex = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex);
                            sResLogUnit.strVal = string.Format("CAL_FILE, {0}", strCurCalFilePath);
                            g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogRfCalibrate.Add(sResLogUnit);
                            frmRun1DutGui_ShowLogMsg(sResLogUnit.strVal);

                            while ((strLine = streamCalFile.ReadLine()) != null)
                            {
                                sResLogUnit = new clResLogUnit();
                                sResLogUnit.iModemIndex = g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex;
                                sResLogUnit.strModemIndex = string.Format("{0}", g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex);
                                sResLogUnit.strName = "CAL_DATA";
                                sResLogUnit.strDisc = "CAL_DATA";
                                sResLogUnit.strVal = strLine;
                                sResLogUnit.strVal = sResLogUnit.strVal.Replace("=", ",");
                                g_sCurCfgCtrlSet.sRunLogInfo.listRunDataLogRfCalibrate.Add(sResLogUnit);
                                strLogMsg = string.Format("{0}, {1}", sResLogUnit.strDisc, sResLogUnit.strVal);
                                frmRun1DutGui_ShowLogMsg(strLogMsg);
                            }
                            if (streamCalFile != null)
                            {
                                streamCalFile.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "frmRun1DutGui_ReadMtkCalFiles Failed!");
                iStatus = -1;
            }
            return iStatus;
        }

        private int frmRun1DutGui_WriteResFiles(clCfgCtrlSet sCfgCtrlSet)
        {
            int iStatus = 0;

            if (streamResLogFile != null)
            {
                String strLogInfo = "";
                sCfgCtrlSet.sRunAllInfo.dtCurrentRunEnd = DateTime.Now;
                sCfgCtrlSet.sRunAllInfo.tsRunEnd = new TimeSpan(sCfgCtrlSet.sRunAllInfo.dtCurrentRunEnd.Ticks);
                TimeSpan tsCostTime = sCfgCtrlSet.sRunAllInfo.tsRunEnd.Subtract(sCfgCtrlSet.sRunAllInfo.tsRunBegin).Duration();
                sCfgCtrlSet.sRunAllInfo.iCurCostTime = Convert.ToInt32(tsCostTime.TotalSeconds);
                streamResLogFile.WriteLine("----------------------------------------------------------------");
                streamResLogFile.WriteLine("ResultType:,TotalResult");

                if (sCfgCtrlSet.sRunAllInfo.iLogErrCode == 0)
                {
                    streamResLogFile.WriteLine("SN, DATETIME, OPERATOR, STATION, FIXTURE, COST, INSTRUMENT, RESULT");
                    strLogInfo = string.Format("{0}, {1}-{2}-{3} {4}:{5}:{6}, {7}, {8}_{9}, {10}, {11}, {12}|{13}|{14}, {15}",
                        sCfgCtrlSet.sRunAllInfo.strLogDutSerialNumber,
                        sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Year,
                        sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Month,
                        sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Day,
                        sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Hour,
                        sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Minute,
                        sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Second,
                        sCfgCtrlSet.sRunAllInfo.strDbAccount,
                        sCfgCtrlSet.sRunAllInfo.strDbPcName,
                        sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex+1,
                        sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex+1,
                        sCfgCtrlSet.sRunAllInfo.iCurCostTime,
                        sCfgCtrlSet.sRunAllInfo.strCurTesterMode,
                        sCfgCtrlSet.sRunAllInfo.strCurTesterSN,
                        sCfgCtrlSet.sRunAllInfo.strCurTesterFW,
                        sCfgCtrlSet.sRunAllInfo.strCurRunPassFail);
                }
                else if (sCfgCtrlSet.sRunAllInfo.iLogErrCode == 1)
                {
                    streamResLogFile.WriteLine("SN, DATETIME, OPERATOR, STATION, FIXTURE, COST, INSTRUMENT, RESULT, ERROR_CODE");
                        strLogInfo = string.Format("{0}, {1}-{2}-{3} {4}:{5}:{6}, {7}, {8}_{9}, {10}, {11}, {12}|{13}|{14}, {15}, {16}",
                            sCfgCtrlSet.sRunAllInfo.strLogDutSerialNumber,
                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Year,
                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Month,
                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Day,
                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Hour,
                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Minute,
                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Second,
                            sCfgCtrlSet.sRunAllInfo.strDbAccount,
                            sCfgCtrlSet.sRunAllInfo.strDbPcName,
                            sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex+1,
                            sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex+1,
                            sCfgCtrlSet.sRunAllInfo.iCurCostTime,
                            sCfgCtrlSet.sRunAllInfo.strCurTesterMode,
                            sCfgCtrlSet.sRunAllInfo.strCurTesterSN,
                            sCfgCtrlSet.sRunAllInfo.strCurTesterFW,
                            sCfgCtrlSet.sRunAllInfo.strCurRunPassFail,
                            sCfgCtrlSet.sRunLogInfo.sCurRunErrLog.iErrCode);                     
                }
                streamResLogFile.WriteLine(strLogInfo);

                streamResLogFile.WriteLine("----------------------------------------------------------------");
                streamResLogFile.WriteLine("ResultType:,Station Settings");
                strLogInfo = string.Format("GI__ToolVersion, {0}.{1}", sCfgCtrlSet.sRunAllInfo.strToolVersion, sCfgCtrlSet.sRunAllInfo.strAteVersion);
                streamResLogFile.WriteLine(strLogInfo);
                strLogInfo = string.Format("GI__PcName, {0}", sCfgCtrlSet.sRunAllInfo.strDbPcName);
                streamResLogFile.WriteLine(strLogInfo);
                strLogInfo = string.Format("GI__TestSetAddr, {0}", sCfgCtrlSet.sRunAllInfo.strCurTesterAddr);
                streamResLogFile.WriteLine(strLogInfo);
                strLogInfo = string.Format("GI__TestSetInfo, {0}|{1}|{2}",
                    sCfgCtrlSet.sRunAllInfo.strCurTesterMode,
                    sCfgCtrlSet.sRunAllInfo.strCurTesterSN,
                    sCfgCtrlSet.sRunAllInfo.strCurTesterFW);
                streamResLogFile.WriteLine(strLogInfo);
                strLogInfo = string.Format("GI__PowerSuplyAddr, {0}", sCfgCtrlSet.sRunAllInfo.strCurPowerSupplyAddr);
                streamResLogFile.WriteLine(strLogInfo);
                strLogInfo = string.Format("GI__PowerSuplyInfo, {0}", sCfgCtrlSet.sRunAllInfo.strCurPowerSupplyType);
                streamResLogFile.WriteLine(strLogInfo);
                strLogInfo = string.Format("GI__FixtureNumber, {0}", sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex+1);
                streamResLogFile.WriteLine(strLogInfo);
                strLogInfo = string.Format("GI__MobilePort, {0}", sCfgCtrlSet.sRunAllInfo.strCurDutComPort);
                streamResLogFile.WriteLine(strLogInfo);

                strLogInfo = string.Format("GI__RfCalBands, {0}", sCfgCtrlSet.sRunAllInfo.strCalBandsConfigInfo);
                streamResLogFile.WriteLine(strLogInfo);

                if (sCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS")
                {
                    strLogInfo = string.Format("GI__ErrCode, {0}", 0);
                    streamResLogFile.WriteLine(strLogInfo);
                    strLogInfo = string.Format("GI__ErrDiscription, {0}", "");
                    streamResLogFile.WriteLine(strLogInfo);
                }
                else
                {
                    strLogInfo = string.Format("GI__ErrCode, {0}", sCfgCtrlSet.sRunLogInfo.sCurRunErrLog.iErrCode);
                    streamResLogFile.WriteLine(strLogInfo);
                    strLogInfo = string.Format("GI__ErrDiscription, {0}", sCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscEng);
                    streamResLogFile.WriteLine(strLogInfo);
                }

                    streamResLogFile.WriteLine("----------------------------------------------------------------");
                    streamResLogFile.WriteLine("ResultType:,Rf Settings");
                if ((g_sCurCfgCtrlSet.sRunAllInfo.iLogCfgSet == 1) || (g_sCurCfgCtrlSet.sRunAllInfo.iLogCfgLoss == 1))
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.bCalEnterMeta)
                    {
                        frmRun1DutGui_ReadCfgFiles(sCfgCtrlSet);
                    }
                    foreach (clResCfgUnit sResCfgUnit in g_sCurCfgCtrlSet.sRunLogInfo.listRunCfgKeySet)
                    {
                        strLogInfo = string.Format("{0}", sResCfgUnit.strVal);
                        streamResLogFile.WriteLine(strLogInfo);
                    }
                }
                streamResLogFile.WriteLine("----------------------------------------------------------------"); // Patricio BB
                streamResLogFile.WriteLine("ResultType:,BB Test Result");
                if (g_sCurCfgCtrlSet.sRunAllInfo.iLogBbRes == 1)
                {
                    foreach (clResLogUnit sResLogUnit in g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogBB)
                    {
                        strLogInfo = string.Format("{0}, {1}", sResLogUnit.strDisc, sResLogUnit.strVal);
                        streamResLogFile.WriteLine(strLogInfo);
                    }
                }

                    streamResLogFile.WriteLine("----------------------------------------------------------------");
                    streamResLogFile.WriteLine("ResultType:,Rf Calibrate Result");
                if (g_sCurCfgCtrlSet.sRunAllInfo.iLogCalRes == 1)
                {
                    foreach (clResLogUnit sResLogUnit in g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogRfCalibrate)
                    {
                        strLogInfo = string.Format("{0}, {1}", sResLogUnit.strDisc, sResLogUnit.strVal);
                        streamResLogFile.WriteLine(strLogInfo);
                    }
                }

                    streamResLogFile.WriteLine("----------------------------------------------------------------");
                    streamResLogFile.WriteLine("ResultType:,Rf Calibrate Data");

                if (g_sCurCfgCtrlSet.sRunAllInfo.iLogCalData == 1)
                {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.bCalEnterMeta)
                    {
                        frmRun1DutGui_ReadMtkCalFiles(sCfgCtrlSet);
                    }
                    foreach (clResLogUnit sResLogUnit in g_sCurCfgCtrlSet.sRunLogInfo.listRunDataLogRfCalibrate)
                    {
                        strLogInfo = string.Format("{0}, {1}", sResLogUnit.strDisc, sResLogUnit.strVal);
                        streamResLogFile.WriteLine(strLogInfo);
                    }
                }
                //string[] straryRetBufAll = new string[60];

                //foreach (clResLogUnit sResUnit in sRunLogInfo.listRunResLogRfCalibrate)
                //{
                //    //strLogInfo = string.Format("{0}_{1}", sResUnit.strModemIndex, sResUnit.strVal);
                //    if (sResUnit.iModemIndex > 0)
                //    {
                //        straryRetBufAll = sResUnit.strVal.Split(new char[1] { ',' });
                //        if (straryRetBufAll.Length > 1)
                //        {
                //            string strTempVal = sResUnit.strVal;
                //            int iPos1 = straryRetBufAll[0].Length + straryRetBufAll[1].Length + 2;
                //            int iPos2 = strTempVal.Length - iPos1;

                //            strTempVal = strTempVal.Substring(iPos1, iPos2);
                //            sResUnit.strVal = string.Format("{0},{1}_C{2},{3}", straryRetBufAll[0], straryRetBufAll[1], sResUnit.iModemIndex + 1, strTempVal);
                //            strLogInfo = string.Format("{0}", sResUnit.strVal);
                //            streamResLogFile.WriteLine(strLogInfo);
                //        }
                //    }
                //    else
                //    {
                //        strLogInfo = string.Format("{0}", sResUnit.strVal);
                //        streamResLogFile.WriteLine(strLogInfo);
                //    }                    
                //}


                    streamResLogFile.WriteLine("----------------------------------------------------------------");
                    streamResLogFile.WriteLine("ResultType:,Rf Verify Result");
                if (g_sCurCfgCtrlSet.sRunAllInfo.iLogMeasRes == 1)
                {
                    foreach (clResLogUnit sResLogUnit in g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogRfVerify)
                    {
                        //strLogInfo = string.Format("{0}_{1}", sResUnit.strModemIndex, sResUnit.strVal);
                        if (sResLogUnit.iModemIndex > 0)
                        {
                            strLogInfo = string.Format("{0}, {1}", sResLogUnit.strDisc, sResLogUnit.strVal);
                            if (sResLogUnit.strLevel.Contains(g_sCurCfgCtrlSet.sRunAllInfo.strCurResLevel))
                            {
                                streamResLogFile.WriteLine(strLogInfo);
                            }
                        }
                        else
                        {
                            strLogInfo = string.Format("{0}, {1}", sResLogUnit.strDisc, sResLogUnit.strVal);
                            if (sResLogUnit.strLevel.Contains(g_sCurCfgCtrlSet.sRunAllInfo.strCurResLevel))
                            {
                                streamResLogFile.WriteLine(strLogInfo);
                            }
                        }
                    }
                }
            }

            return iStatus;
        }

        private int frmRun1DutGui_WriteInfoFiles(clCfgCtrlSet sCfgCtrlSet)
        {
            int iStatus = 0;
            string strPromptMsg = "";
            string strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
            try
            {
                if (streamSumLogFile != null)
                {
                    streamSumLogFile.Close();
                    streamSumLogFile = null;

                    sCfgCtrlSet.sRunAllInfo.strCurSumLogFile = string.Format("\\Log\\{0}_{1}_{2}-{3}-{4}_{5}.{6}.{7}.{8}_SUM_Module{9}.txt",
                                            sCfgCtrlSet.sRunAllInfo.strCurRunPassFail,
                                            sCfgCtrlSet.sRunAllInfo.strRetDutSerialNumber,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Year,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Month,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Day,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Hour,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Minute,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Second,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Millisecond,
                                            sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex);

                    sCfgCtrlSet.sRunAllInfo.strCurSumLogFile = strCurrentDirectory + sCfgCtrlSet.sRunAllInfo.strCurSumLogFile;

                    if (File.Exists(strOrgSumLogFile))
                    {
                        System.IO.File.Move(strOrgSumLogFile, sCfgCtrlSet.sRunAllInfo.strCurSumLogFile);
                    }
                }

                if (streamTraceLogFile != null)
                {
                    streamTraceLogFile.Close();
                    streamTraceLogFile = null;

                    sCfgCtrlSet.sRunAllInfo.strCurTraceLogFile = string.Format("\\Log\\{0}_{1}_{2}-{3}-{4}_{5}.{6}.{7}.{8}_Trace_Module{9}.txt",
                                            sCfgCtrlSet.sRunAllInfo.strCurRunPassFail,
                                            sCfgCtrlSet.sRunAllInfo.strRetDutSerialNumber,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Year,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Month,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Day,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Hour,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Minute,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Second,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Millisecond,
                                            sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex);

                    sCfgCtrlSet.sRunAllInfo.strCurTraceLogFile = strCurrentDirectory + sCfgCtrlSet.sRunAllInfo.strCurTraceLogFile;

                    if (File.Exists(strOrgTraceLogFile))
                    {
                        System.IO.File.Move(strOrgTraceLogFile, sCfgCtrlSet.sRunAllInfo.strCurTraceLogFile);
                    }
                }

                if (streamResLogFile != null)
                {
                    streamResLogFile.Close();
                    streamResLogFile = null;

                    sCfgCtrlSet.sRunAllInfo.strCurResLogFile = string.Format("\\Log\\{0}_{1}_{2}-{3}-{4}_{5}.{6}.{7}.{8}_Res_Module{9}.csv",
                                            sCfgCtrlSet.sRunAllInfo.strCurRunPassFail,
                                            sCfgCtrlSet.sRunAllInfo.strRetDutSerialNumber,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Year,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Month,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Day,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Hour,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Minute,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Second,
                                            sCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Millisecond,
                                            sCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex);

                    sCfgCtrlSet.sRunAllInfo.strCurResLogFile = strCurrentDirectory + sCfgCtrlSet.sRunAllInfo.strCurResLogFile;

                    if (File.Exists(strOrgResLogFile))
                    {
                        System.IO.File.Move(strOrgResLogFile, sCfgCtrlSet.sRunAllInfo.strCurResLogFile);
                    }
                }
            }
            catch (Exception exc)
            {
                strPromptMsg = string.Format("{0}:{1}", "frmRun1DutGui_WriteInfoFiles Failed!", exc.Message);
                frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
            }
            return iStatus;
        }

        private int frmRun1DutGui_CopyUploadResFiles()
        {
            int iStatus = 0;
            string strPromptMsg = "";
            string strUploadResLogFile = "";
            try
            {
                if (File.Exists(g_sCurCfgCtrlSet.sRunAllInfo.strCurResLogFile))
                {
                    int iBeginPos = g_sCurCfgCtrlSet.sRunAllInfo.strCurResLogFile.LastIndexOf("\\") + 1;
                    int iEndPos = g_sCurCfgCtrlSet.sRunAllInfo.strCurResLogFile.Length;

                    strUploadResLogFile = g_sCurCfgCtrlSet.sRunAllInfo.strCurResLogFile.Substring(iBeginPos, iEndPos - iBeginPos);
                    strUploadResLogFile = "..\\start\\TestResult\\Pretest\\" + strUploadResLogFile;
                    if (!File.Exists(strUploadResLogFile) && (g_sCurCfgCtrlSet.sRunAllInfo.bCopyFailResult == false))
                    {
                        System.IO.File.Copy(g_sCurCfgCtrlSet.sRunAllInfo.strCurResLogFile, strUploadResLogFile);
                        g_sCurCfgCtrlSet.sRunAllInfo.bCopyFailResult = true;
                    }
                }
            }
            catch (Exception exc)
            {
                strPromptMsg = string.Format("frmRun1DutGui_CopyUploadResFiles: {0} -> {1} Failed!", g_sCurCfgCtrlSet.sRunAllInfo.strCurResLogFile, strUploadResLogFile);
                frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
                return -1;
            }

            return iStatus;
        }

        private void frmRun1DutGui_RunAttnExe(string strExePath)
        {
            string strPromptMsg = "";
            try
            {
                if (!File.Exists(strExePath))
                {
                    strPromptMsg = string.Format("选择路径下{0}可执行文件不存在，请检查!", strExePath);
                    frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
                    return;
                }
                else
                {
                    Process proAttnExeFile;
                    proAttnExeFile = new Process();
                    proAttnExeFile.StartInfo.UseShellExecute = false;
                    proAttnExeFile.StartInfo.RedirectStandardInput = true;
                    proAttnExeFile.StartInfo.RedirectStandardOutput = true;
                    proAttnExeFile.StartInfo.RedirectStandardError = true;
                    proAttnExeFile.StartInfo.CreateNoWindow = true;
                    proAttnExeFile.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    proAttnExeFile.StartInfo.FileName = strExePath;
                    proAttnExeFile.StartInfo.WorkingDirectory = strExePath.Remove(strExePath.LastIndexOf("\\"));
                    proAttnExeFile.StartInfo.Arguments = "";
                    proAttnExeFile.Start();
                }
            }
            catch (Exception exc)
            {
                strPromptMsg = string.Format("{0}:{1}", "执行测试软件失败!", exc.Message);
                frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
            }
            return;
        }

        private int frmRun1DutGui_DeleteAttnResFiles()
        {
            int iStatus = 0;
            string strPromptMsg = "";
            string strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
            g_sCurCfgCtrlSet.sRunAllInfo.strAttnResLogFolder = string.Format("{0}{1}", strCurrentDirectory, "\\attn\\ref\\res");
            try
            {

                if (System.IO.Directory.Exists(g_sCurCfgCtrlSet.sRunAllInfo.strAttnResLogFolder))
                {
                    //获得文件夹数组
                    string[] strDirs = System.IO.Directory.GetDirectories(g_sCurCfgCtrlSet.sRunAllInfo.strAttnResLogFolder);
                    // 获得文件数组
                    string[] strFiles = System.IO.Directory.GetFiles(g_sCurCfgCtrlSet.sRunAllInfo.strAttnResLogFolder);
                    // 遍历所有子文件夹
                    foreach (string strFile in strFiles)
                    {
                        //删除文件夹
                        System.IO.File.Delete(strFile);
                    }
                    //遍历所有文件
                    foreach (string strdir in strDirs)
                    {
                        //删除文件  
                        System.IO.Directory.Delete(strdir, true);
                    }
                }
            }
            catch (Exception exc)// 异常处理
            {
                strPromptMsg = string.Format("{0}:{1}", "frmRun1DutGui_DeleteAttnResFiles ERROR", exc.Message);
                frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
                return -1;
            }
            return iStatus;
        }

        private int frmRun1DutGui_UpdateAttnResFiles()
        {
            int iStatus = 0;
            string strPromptMsg = "";
            string strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
            string strNewResLogFile = "";

            try
            {
                if (File.Exists(g_sCurCfgCtrlSet.sRunAllInfo.strCurResLogFile))
                {
                    strNewResLogFile = g_sCurCfgCtrlSet.sRunAllInfo.strAttnResLogFolder + g_sCurCfgCtrlSet.sRunAllInfo.strCurResLogFile.Substring(g_sCurCfgCtrlSet.sRunAllInfo.strCurResLogFile.LastIndexOf("\\"), g_sCurCfgCtrlSet.sRunAllInfo.strCurResLogFile.Length - g_sCurCfgCtrlSet.sRunAllInfo.strCurResLogFile.LastIndexOf("\\")); ;
                    System.IO.File.Copy(g_sCurCfgCtrlSet.sRunAllInfo.strCurResLogFile, strNewResLogFile);
                }
            }
            catch (Exception exc)
            {
                strPromptMsg = string.Format("{0}:{1}", "frmRun1DutGui_DeleteAttnResFiles ERROR", exc.Message);
                frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
                return -1;
            }

            return iStatus;
        }

        private int frmRun1DutGui_UpdateAttnCfgFiles()
        {
            int iStatus = 0;
            string strPromptMsg = "";
            string strCurrentDirectory = "";
            string strAttnExeFile = "";
            string strAttnLogFile = "";
            try
            {
                iStatus = sCfgCtrlRun.UpdateAttnIni(g_sCurCfgCtrlSet);
                if (iStatus != 0)
                {
                    return iStatus;
                }

                strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                strAttnExeFile = strCurrentDirectory + "\\attn\\csv_EXE_Call.exe";
                strAttnLogFile = strCurrentDirectory + "\\attn\\Log.txt";

                frmRun1DutGui_RunAttnExe(strAttnExeFile);

                Thread.Sleep(5000);

                string strReadLine = "";
                bool bCvtCableLossPass = false;
                StreamReader streamAttnLog = new StreamReader(strAttnLogFile, false);
                if (streamAttnLog != null)
                {
                    if(g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                    {
                        strPromptMsg = "Cableloss tool infomation\r\n";
                    }
                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                    {                    
                        strPromptMsg = "线损工具log信息显示:\r\n";
                    }
                    rtbRunItemDut0.AppendText(strPromptMsg);
                    rtbRunItemDut0.AppendText("\r\n");
                    rtbRunItemDut0.ScrollToCaret();

                    while ((strReadLine = streamAttnLog.ReadLine()) != null)
                    {
                        if (strReadLine.Contains("result") && strReadLine.Contains("pass"))
                        {
                            bCvtCableLossPass = true;
                        }
                        else if (strReadLine.Contains("result") && strReadLine.Contains("fail"))
                        {
                            bCvtCableLossPass = false;
                        }
                        rtbRunItemDut0.AppendText(strReadLine);
                        rtbRunItemDut0.AppendText("\r\n");
                        rtbRunItemDut0.ScrollToCaret();

                        rtbRunLogDut0.AppendText(strReadLine);
                        rtbRunLogDut0.AppendText("\r\n");
                        rtbRunLogDut0.ScrollToCaret();
                    }
                    if (bCvtCableLossPass == true)
                    {
                        if(g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt== 1)
                        {
                            labelTestStatusDut0.Text = "Cableloss calibration pass\r\n";
                        }
                        else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                        {
                            labelTestStatusDut0.Text = "校准线损通过";
                        }
                        labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                        labelTestStatusDut0.BackColor = System.Drawing.Color.Green;
                    }
                    else if (bCvtCableLossPass == false)
                    {
                        if(g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt== 1)
                        {
                            labelTestStatusDut0.Text = "Cableloss calibration fail\r\n";
                        }
                        else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                        {
                            labelTestStatusDut0.Text = "校准线损失败";
                        }
                        labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                        labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                    }
                    if (streamAttnLog != null)
                    {
                        streamAttnLog.Close();
                    }
                }
            }
            catch (Exception exc)
            {
                strPromptMsg = string.Format("{0}:{1}", "frmRun1DutGui_UpdateAttnCfgFiles ERROR", exc.Message);
                frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
                return -1;
            }

            return iStatus;
        }

        public static void frmRun1DutGui_DeleteMtkLog(string strLogFolder)
        {
            try
            {
                if (strLogFolder[strLogFolder.Length - 1] != Path.DirectorySeparatorChar)
                {
                    strLogFolder += Path.DirectorySeparatorChar;
                }
                string[] fileList = Directory.GetFileSystemEntries(strLogFolder);
                foreach (string file in fileList)
                {
                    if (Path.GetFileName(file).Contains(".log")
                        || Path.GetFileName(file).Contains(".muxraw"))
                    {
                        continue;
                    }
                    else
                    {
                        File.Delete(strLogFolder + Path.GetFileName(file));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private int frmRun1DutGui_CloseLogFiles()
        {
            int iStatus = 0;

            if (streamTraceLogFile != null)
            {
                streamTraceLogFile.Close();
                streamTraceLogFile = null;
            }
            if (streamResLogFile != null)
            {
                streamResLogFile.Close();
                streamResLogFile = null;
            }
            if (streamSumLogFile != null)
            {
                streamSumLogFile.Close();
                streamSumLogFile = null;
            }

            if (g_sCurCfgCtrlSet.sRunAllInfo.iDeleteMtkLog == 1)
            {
                frmRun1DutGui_DeleteMtkLog(".\\log\\mtk");
            }

            return iStatus;
        }

        private void frmRunDoc1TimerRecord_Tick(object sender, EventArgs e)
        {
            g_sCurCfgCtrlSet.sRunAllInfo.dtCurrentRunEnd = DateTime.Now;
            g_sCurCfgCtrlSet.sRunAllInfo.tsRunEnd = new TimeSpan(g_sCurCfgCtrlSet.sRunAllInfo.dtCurrentRunEnd.Ticks);
            TimeSpan ts = g_sCurCfgCtrlSet.sRunAllInfo.tsRunEnd.Subtract(g_sCurCfgCtrlSet.sRunAllInfo.tsRunBegin).Duration();
            labelTimeCost.Text = string.Format("{0:000.000}s", ts.TotalSeconds);

            if (ts.TotalSeconds > g_sCurCfgCtrlSet.sRunAllInfo.iRunTimeout)
            {
                sMsgCtrl.SendMsg(this.Handle, WM_RUN_ERR, "run_err", "run_timeout");
            }
        }


        private void frmRunDoc1_Resize(object sender, EventArgs e)
        {
            splitContainer6.Height = 25;
        }

        private void tsbRunStop_Click(object sender, EventArgs e)
        {
            //g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus = -1;
            sMsgCtrl.SendMsg(g_hwndFrmRun1DutGui, WM_RUN_PROC, "run_proc", "run_stop");
        }

        private int frmRun1DutGui_CloseApp(string strCurExeFilePath)
        {
            int iStatus = 0;
            string strProcessNameUpper = "";
            string strExeProcessUpper = strCurExeFilePath.Substring(strCurExeFilePath.LastIndexOf("\\") + 1, strCurExeFilePath.Length - strCurExeFilePath.LastIndexOf("\\") - 1);
            strExeProcessUpper = strExeProcessUpper.ToUpper();
            if (!strExeProcessUpper.Contains(".EXE"))
            {
                return iStatus;
            }
            try
            {

                int id = Process.GetCurrentProcess().Id;
                Process[] prcMain = Process.GetProcesses();
                foreach (Process pr in prcMain)
                {
                    strProcessNameUpper = pr.ProcessName.ToUpper();
                    if (strExeProcessUpper.Contains(strProcessNameUpper))
                    {
                        pr.Kill();
                        Thread.Sleep(2000);
                    }
                }
            }
            catch (Exception exc)
            {
                //strPromptMsg = string.Format("{0}:{1}", "frmRun1DutGui_CloseApp ERROR", exc.Message);
                //frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
                return -1;
            }
            return iStatus;
        }

        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            int iStatus = 0;
            string strMsgName = "";
            string strMsgVal = "";
            string strPromptMsg = "";
            switch (m.Msg)
            {
                case WM_RUN_CFG:

                    break;
                case WM_RUN_LOG:
                    strMsgName = string.Format("{0}", Marshal.PtrToStringAnsi(m.WParam).ToString());
                    strMsgVal = string.Format("{0}", Marshal.PtrToStringAnsi(m.LParam).ToString());
                    if (strMsgName == "run_status")
                    {
                        frmRun1DutGui_ShowCurStatus(strMsgVal);
                        if (strMsgVal.Contains("夹具已关闭"))
                        {
                            this.labelTestStatusDut0.Text = strMsgVal;
                            this.labelTestStatusDut0.BackColor = System.Drawing.Color.Black;
                            this.labelTestStatusDut0.ForeColor = System.Drawing.Color.Yellow;
                        }
                    }
                    else if (strMsgName == "run_item")
                    {
                        frmRun1DutGui_ShowItemMsg(strMsgVal);
                    }
                    else if (strMsgName == "run_log")
                    {
                        frmRun1DutGui_ShowLogMsg(strMsgVal);
                    }
                    //Application.DoEvents();
                    //System.Threading.Thread.Sleep(1);
                    break;
                case WM_RUN_ERR:
                    strMsgName = string.Format("{0}", Marshal.PtrToStringAnsi(m.WParam).ToString());
                    strMsgVal = string.Format("{0}", Marshal.PtrToStringAnsi(m.LParam).ToString());

                    if (strMsgName == "run_err")
                    {
                        if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableFixtureControl == 1)
                        {
                            strPromptMsg = "打开屏蔽箱开始";
                            frmRun1DutGui_ShowCurStatus(strPromptMsg);
                            frmRun1DutGui_ShowItemMsg(strPromptMsg);

                            sExeCtrl.strCurRunPassFail = g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail;
                            Thread ntExeCtrlFixtureOpen = new Thread(new ThreadStart(sExeCtrl.clExeCtrl_FixtureOpen));
                            ntExeCtrlFixtureOpen.IsBackground = true;
                            ntExeCtrlFixtureOpen.Priority = ThreadPriority.Normal;
                            ntExeCtrlFixtureOpen.Start();
                            ntExeCtrlFixtureOpen.Join();

                            if (sExeCtrl.bFixtureOpen)
                            {
                                strPromptMsg = "打开屏蔽箱成功";

                                frmRun1DutGui_ShowCurStatus(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);
                            }
                            else
                            {
                                frmRun1DutGui_SetErrMsg(ERR_GUI_FIXTURE_OPEN_FAIL, "打开屏蔽箱失败");
                                //sMsgCtrl.SendMsg(this.Handle, WM_RUN_ERR, "run_err", "fixture_open_failed");
                            }
                        }

                        if ((strMsgVal == "comport_find_failed")
                           || (strMsgVal == "power_on_failed")
                            || (strMsgVal == "power_off_failed")
                            || (strMsgVal == "fixture_open_failed")
                            || (strMsgVal == "fixture_close_failed")
                            || (strMsgVal == "dut_get_sn_failed")
                            || (strMsgVal == "dut_sn_no_volid")
                            || (strMsgVal == "dut_no_cal_golden"))
                        {
                            if (strMsgVal == "comport_find_failed")
                            {
                                strPromptMsg = "手机串口未正常打开";
                                frmRun1DutGui_ShowCurItem(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);
                                frmRun1DutGui_SetErrMsg( ERR_GUI_COMPORT_FIND_TIMEOUT, strPromptMsg);
                            }
                            else if (strMsgVal == "power_on_failed")
                            {
                                strPromptMsg = "打开电源失败";
                                frmRun1DutGui_ShowCurItem(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);
                                frmRun1DutGui_SetErrMsg( ERR_GUI_POWER_ON_FAIL, strPromptMsg);
                            }
                            else if (strMsgVal == "power_off_failed")
                            {
                                strPromptMsg = "打开电源失败";
                                frmRun1DutGui_ShowCurItem(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);
                                frmRun1DutGui_SetErrMsg( ERR_GUI_POWER_OFF_FAIL, strPromptMsg);
                            }
                            else if (strMsgVal == "fixture_open_failed")
                            {
                                strPromptMsg = "打开夹具失败";
                                frmRun1DutGui_ShowCurItem(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);
                                frmRun1DutGui_SetErrMsg( ERR_GUI_FIXTURE_OPEN_FAIL, strPromptMsg);
                            }
                            else if (strMsgVal == "fixture_close_failed")
                            {
                                strPromptMsg = "关闭夹具失败";
                                frmRun1DutGui_ShowCurItem(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);
                                frmRun1DutGui_SetErrMsg( ERR_GUI_FIXTURE_CLOSE_FAIL, strPromptMsg);
                            }
                            else if (strMsgVal == "dut_get_sn_failed")
                            {
                                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                                {
                                    strPromptMsg = "Get SN fail";
                                }
                                else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                {
                                    strPromptMsg = "读取SN失败";
                                }
                                frmRun1DutGui_ShowCurItem(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);
                                frmRun1DutGui_SetErrMsg( ERR_GUI_DUT_SN_GET_FAILED, strPromptMsg);
                            }                       
                            else if (strMsgVal == "dut_sn_no_volid")
                            {
                                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                                {
                                    strPromptMsg = "Get illegal SN";
                                }
                                else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                {
                                    strPromptMsg = "返回SN无效";
                                }
                                frmRun1DutGui_ShowCurItem(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);
                                frmRun1DutGui_SetErrMsg( ERR_GUI_DUT_SN_NOVOLID, strPromptMsg);
                            }                           
                            else if (strMsgVal == "dut_no_cal_golden")
                            {
                                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                                {
                                    strPromptMsg = "Do not calibrate golden sample";
                                }
                                else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                {
                                    strPromptMsg = "不能校准金板";
                                }
                                frmRun1DutGui_ShowCurItem(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);
                                frmRun1DutGui_SetErrMsg( ERR_GUI_DUT_NO_CAL_GOLDEN, strPromptMsg);                                
                            }

                            frmRun1DutGui_ShowFailMsg();
                            if (strMsgVal == "dut_no_cal_golden")
                            {
                                frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);
                            }
                            frmRunDoc1TimerRecord.Enabled = false;
                        }
                        else if (strMsgVal == "run_timeout")
                        {
                            if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                            {
                                strPromptMsg = "test timeout";
                            }
                            else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                            {
                                strPromptMsg = "测试过程超时";
                            }

                            frmRun1DutGui_ShowCurItem(strPromptMsg);
                            frmRun1DutGui_ShowItemMsg(strPromptMsg);
                            frmRun1DutGui_SetErrMsg( ERR_GUI_PROC_TIMEOUT, strPromptMsg);
                            frmRun1DutGui_ShowFailMsg();
                            frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);
                            frmRunDoc1TimerRecord.Enabled = false;

                            g_bAppRebootFlag = false;
                           
                        }
                        else if (strMsgVal == "run_proc_rf_stop")
                        {
                            strPromptMsg = "测试过程出错";
                            frmRun1DutGui_ShowCurItem(strPromptMsg);
                            frmRun1DutGui_ShowItemMsg(strPromptMsg);
                            frmRun1DutGui_SetErrMsg( ERR_GUI_PROC_RF_STOP, strPromptMsg);
                            frmRunDoc1TimerRecord.Enabled = false;
                        }
                       
                        //else if (strMsgVal == "err_happened")
                        //{
                            //if (g_sCurCfgCtrlSet.sRunAllInfo.bCalEnterMeta)
                            //{
                            //    g_sCurCfgCtrlSet.sRunAllInfo.iDutFailNum++;
                            //    frmRun1DutGui_ShowFailMsg();
                            //    g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                            //    frmRun1DutGui_WriteResFiles(g_sCurCfgCtrlSet);
                            //    frmRun1DutGui_WriteInfoFiles(g_sCurCfgCtrlSet);
                            //    if ((Int32.Parse(g_sCurCfgCtrlSet.sRunAllInfo.strLoginDb) == 1)
                            //        && (Int32.Parse(g_sCurCfgCtrlSet.sRunAllInfo.strConnectDb) == 1)
                            //            && (g_sCurCfgCtrlSet.sRunAllInfo.iRunMode == 0))
                            //    {
                            //        frmRun1DutGui_CopyUploadResFiles();
                            //    }
                            //    if (g_sCurCfgCtrlSet.sRunAllInfo.iEnablePowerSupplyControl == 1)
                            //    {
                            //        strPromptMsg = "关闭程控电源开始";
                            //        frmRun1DutGui_ShowCurStatus(strPromptMsg);
                            //        frmRun1DutGui_ShowItemMsg(strPromptMsg);

                            //        Thread ntExeCtrlPowerOff = new Thread(new ThreadStart(sExeCtrl.clExeCtrl_PowerOff));
                            //        ntExeCtrlPowerOff.IsBackground = true;
                            //        ntExeCtrlPowerOff.Priority = ThreadPriority.Normal;
                            //        ntExeCtrlPowerOff.Start();
                            //        ntExeCtrlPowerOff.Join();

                            //        if (sExeCtrl.bPowerOff)
                            //        {
                            //            strPromptMsg = "关闭程控电源成功";
                            //            frmRun1DutGui_ShowCurStatus(strPromptMsg);
                            //            frmRun1DutGui_ShowItemMsg(strPromptMsg);

                            //        }
                            //        else
                            //        {
                            //            frmRun1DutGui_SetErrMsg( ERR_GUI_POWER_OFF_FAIL, "关闭程控电源失败");
                            //            //sMsgCtrl.SendMsg(this.Handle, WM_RUN_ERR, "run_err", "power_off_failed");
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                            //    strPromptMsg = "未进入测试模式，请检查手机，电源，串口等连接是否异常";
                            //    frmRun1DutGui_ShowCurItem(strPromptMsg);
                            //    frmRun1DutGui_ShowItemMsg(strPromptMsg);
                            //    frmRun1DutGui_SetErrMsg( ERR_GUI_DUT_ENTER_META_FAILED, strPromptMsg);
                            //    frmRun1DutGui_ShowFailMsg();
                            //    frmRun1DutGui_WriteResFiles(g_sCurCfgCtrlSet);
                            //    frmRun1DutGui_WriteInfoFiles(g_sCurCfgCtrlSet);
                            //}
                        //}

                        g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                        g_sCurCfgCtrlSet.sRunAllInfo.iDutFailNum++;                        
                        if (g_sCurCfgCtrlSet.sRunAllInfo.iRestartATEDemo == 1)
                        {         
                            g_sCurCfgCtrlSet.sRunAllInfo.iContinueFailCnt = 0;      
                        }
                        frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);
                        frmRun1DutGui_WriteResFiles(g_sCurCfgCtrlSet);
                        frmRun1DutGui_WriteInfoFiles(g_sCurCfgCtrlSet);
                        frmRun1DutGui_CloseLogFiles();                    
                        g_bAppRebootFlag = false;

                        //if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableFixtureControl == 1)
                        //{
                        //    strPromptMsg = "打开屏蔽箱开始";
                        //    frmRun1DutGui_ShowCurStatus(strPromptMsg);
                        //    frmRun1DutGui_ShowItemMsg(strPromptMsg);

                        //    sExeCtrl.strCurRunPassFail = g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail;
                        //    Thread ntExeCtrlFixtureOpen = new Thread(new ThreadStart(sExeCtrl.clExeCtrl_FixtureOpen));
                        //    ntExeCtrlFixtureOpen.IsBackground = true;
                        //    ntExeCtrlFixtureOpen.Priority = ThreadPriority.Normal;
                        //    ntExeCtrlFixtureOpen.Start();
                        //    ntExeCtrlFixtureOpen.Join();

                        //    if (sExeCtrl.bFixtureOpen)
                        //    {
                        //        strPromptMsg = "打开屏蔽箱成功";

                        //        frmRun1DutGui_ShowCurStatus(strPromptMsg);
                        //        frmRun1DutGui_ShowItemMsg(strPromptMsg);
                        //    }
                        //    else
                        //    {
                        //        frmRun1DutGui_SetErrMsg( ERR_GUI_FIXTURE_OPEN_FAIL, "打开屏蔽箱失败");
                        //        //sMsgCtrl.SendMsg(this.Handle, WM_RUN_ERR, "run_err", "fixture_open_failed");
                        //    }
                        //}

                        if (g_sCurCfgCtrlSet.sRunAllInfo.iRunMode == 0)
                        {
                            g_sCurCfgCtrlSet.sRunAllInfo.bRunDutCalPassed = false;
                        }
                        else if (g_sCurCfgCtrlSet.sRunAllInfo.iRunMode == 1)
                        {
                            g_sCurCfgCtrlSet.sRunAllInfo.bRunCableCalPassed = false;
                        }

                        frmRunDoc1TimerRecord.Enabled = false;
                        frmRun1DutGui_CheckRepeatTime(g_sCurCfgCtrlSet);


                  }
                    break;
                case WM_RUN_PROC:
                    strMsgName = string.Format("{0}", Marshal.PtrToStringAnsi(m.WParam).ToString());
                    strMsgVal = string.Format("{0}", Marshal.PtrToStringAnsi(m.LParam).ToString());
                    if (strMsgName == "run_proc")
                    {
                        if (strMsgVal == "run_init")
                        {                           
                            frmRun1DutGui_InitCurRunParas();
                            frmRun1DutGui_InitRetryRunParas();
                            frmRun1DutGui_InitLogFiles();
                            frmRun1DutGui_InitUI();
                            frmRun1DutGui_InitCurBbTestParas(); // Patricio BB

                        }
                        else if (strMsgVal == "run_bb_test") // Patricio BB
                        {
                            frmRun1DutGui_StartProcessBbTest();
                        }
                        else if (strMsgVal == "run_prepare")
                        {
                            if (g_sCurCfgCtrlSet.sRunAllInfo.iRunMode == 1)
                            {
                                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                                {
                                    strPromptMsg = "Cableloss Calibrating，power on again！";
                                    frmRun1DutGui_ShowMsgPrompt(0, strPromptMsg);
                                }
                                else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                {
                                    //frmRunDoc1TimerRecord.Enabled = false;       
                                    strPromptMsg = "请重新对夹具进行开关，已确保线损校准顺利进行！";

                                    frmRun1DutGui_ShowMsgPrompt(0, strPromptMsg);
                                }
                            }

                            //if (g_sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode == 1)
                            //{
                            //    frmRunDoc1_StartProcessCalibrateMain();
                            //}

                            //if (Bz_Handler.CItemListEquip.IsFqaVerify() == 1)
                            //{
                            //  frmRunDoc1_StartProcessCalibrateMain(); // Start MainCalibration Patricio to FQA Mode
                            //}

                            if (g_sCurCfgCtrlSet.sRunAllInfo.iRepeatModeOpt == 1)
                            {
                                frmRun1DutGui_InitToolUI();                             
                            }
 
                            if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ") // FQA-Golden Process - change enviroment color 
                            {
                                if (Bz_Handler.CItemListEquip.IsFqaVerify() == 1)
                                {
                                    labelTestStatusDut0.ForeColor = System.Drawing.Color.Green;
                                    labelTestStatusDut0.BackColor = System.Drawing.Color.White;
                                    labelSN.BackColor = System.Drawing.Color.Green;
                                    rtbRunItemDut0.BackColor = System.Drawing.Color.Green;
                                    Application.DoEvents();
                                }
                            }
                            
                        }                       
                        else if (strMsgVal == "run_cal_ver")
                        {
                            if (g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex != 0)
                            {
                                frmRun1DutGui_InitRetryRunParas();
                                g_sCurCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin = DateTime.Now;
                                g_sCurCfgCtrlSet.sRunAllInfo.tsRunBegin = new TimeSpan(g_sCurCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Ticks);
                            }
                            
                            if (g_sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode == 2)                            
                            {                               
                                if (g_sCurCfgCtrlSet.sRunAllInfo.iEnablePowerSupplyControl == 1)
                                {
                                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                                    {
                                        strPromptMsg = "PSU power off begin";
                                    }
                                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                    {
                                        strPromptMsg = "关闭程控电源开始";
                                    }
                                    frmRun1DutGui_ShowCurStatus(strPromptMsg);
                                    frmRun1DutGui_ShowItemMsg(strPromptMsg);

                                    Thread ntExeCtrlPowerOff = new Thread(new ThreadStart(sExeCtrl.clExeCtrl_PowerOff));
                                    ntExeCtrlPowerOff.IsBackground = true;
                                    ntExeCtrlPowerOff.Priority = ThreadPriority.Normal;
                                    ntExeCtrlPowerOff.Start();
                                    ntExeCtrlPowerOff.Join();

                                if (sExeCtrl.bPowerOff)
                                    {
                                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                                    {
                                        strPromptMsg = "PSU power off successful";
                                    }
                                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                    {
                                        strPromptMsg = "关闭程控电源成功";
                                    }
                                    frmRun1DutGui_ShowCurStatus(strPromptMsg);
                                    frmRun1DutGui_ShowItemMsg(strPromptMsg);

                                    }
                                    else
                                    {
                                        frmRun1DutGui_SetErrMsg(ERR_GUI_POWER_ON_FAIL, "关闭程控电源失败");
                                        //sMsgCtrl.SendMsg(this.Handle, WM_RUN_ERR, "run_err", "power_off_failed");
                                        return;
                                    }
                                }
                               
                                frmRunDoc1_StartProcessCalibrateMain();
                            }
                            
                        }
                        else if (strMsgVal == "run_release")
                        {  
                            if (g_sCurCfgCtrlSet.sRunAllInfo.bCalEnterMeta || g_sCurCfgCtrlSet.sRunAllInfo.bPrintSNInfo)
                            {
                                if ((g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus == 0) && (g_sCurCfgCtrlSet.sRunAllInfo.bCalMainDone) && (g_sCurCfgCtrlSet.sRunAllInfo.iEnableBbTest == 1)) // Patricio
                                {
                                    g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "PASS";
                                    g_sCurCfgCtrlSet.sRunAllInfo.iDutPassNum++;
                                    //frmRun1DutGui_ShowPassMsg();

                                    if (g_sCurCfgCtrlSet.sRunAllInfo.iRestartATEDemo == 1)
                                    {      
                                        g_sCurCfgCtrlSet.sRunAllInfo.iContinueFailCnt = 0;                                           
                                    }
                                }
                                else 
                                {
                                    g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";

                                    if (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != ERR_GUI_DUT_SN_NOVOLID)
                                    {
                                        g_sCurCfgCtrlSet.sRunAllInfo.iDutFailNum++;
                                    }
                                else
                                {
                                    g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                                }
                            
                                     //frmRun1DutGui_ShowFailMsg();

                                    if(g_sCurCfgCtrlSet.sRunAllInfo.iRestartATEDemo == 1)
                                    {
                                        g_sCurCfgCtrlSet.sRunAllInfo.iContinueFailCnt ++;   

                                        if (g_sCurCfgCtrlSet.sRunAllInfo.iContinueFailCnt == g_sCurCfgCtrlSet.sRunAllInfo.iContinueFailMax)
                                        {
                                            g_sCurCfgCtrlSet.sRunAllInfo.iContinueFailCnt = 0;

                                            frmRunDoc1_ExitATE_Demo();
                                            //g_bAppRebootFlag = false;
                                            //Thread.Sleep(2000);
                                            //frmRunDoc1_StartProcessCalibrateMain(); 
                                        }                                       
                                    }
                                }

                                frmRun1DutGui_WriteResFiles(g_sCurCfgCtrlSet);
                                frmRun1DutGui_WriteInfoFiles(g_sCurCfgCtrlSet);                                

                                if (g_sCurCfgCtrlSet.sRunAllInfo.iRunMode == 0)
                                {
                                    if ((Int32.Parse(g_sCurCfgCtrlSet.sRunAllInfo.strLoginDb) == 1)
                                        && (Int32.Parse(g_sCurCfgCtrlSet.sRunAllInfo.strConnectDb) == 1))
                                    {
                                        if ((g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != ERR_GUI_COMPORT_FIND_TIMEOUT)
                                            && (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != ERR_GUI_DUT_ENTER_META_FAILED)
                                            && (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != ERR_GUI_DUT_NO_CAL_GOLDEN)
                                            && (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != ERR_GUI_FIXTURE_OPEN_FAIL)
                                            && (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != ERR_GUI_FIXTURE_CLOSE_FAIL)
                                            && (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != ERR_GUI_FIXTURE_CHECK_FAIL)
                                            && (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != ERR_GUI_POWER_ON_FAIL)
                                            && (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != ERR_GUI_POWER_OFF_FAIL)
                                            && (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != ERR_GUI_RF_SWITCH_FAIL)
                                            && (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != ERR_GUI_DUT_SN_NOVOLID)
                                            && (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != -410026)
                                            /*&& (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus != -410012)*/)
                                        {
                                            if (Bz_Handler.CJagLocalFucntions.GetFactory() != "BZ") // Remove copy Lenovo log files at the end of teh recipe
                                            frmRun1DutGui_CopyUploadResFiles();
                                        }
                                    }
                                }
                                else
                                {
                                    if ((g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus == 0) && (g_sCurCfgCtrlSet.sRunAllInfo.bCalMainDone))
                                    {
                                        g_sCurCfgCtrlSet.sRunAllInfo.bRunCableCalPassed = true;
                                    }
                                    else
                                    {
                                        g_sCurCfgCtrlSet.sRunAllInfo.bRunCableCalPassed = false;
                                    }
                                }
                            }
                            else
                            {
                                g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1) // Patricio to Close MTK_demo on BB fail
                                {
                                    strPromptMsg = "Enter test mode fail,check DUT,PSU,UART connection is correct";
                                    LogResult.AddLogResult("ENTER_META_MODE", "ENTER_META_MODE", "0", "0", "0", "0", "0", 1, "", "");
                                    g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "FAIL";
                                    //g_proIntelEngineCalibrateMain.StandardInput.WriteLine(@"##########CMD_ATEDEMO_APP_QUIT##########"); // CRASH ENTER TEST MODE HERE
                                   // Thread.Sleep(700);
                                    frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);
                                }
                                else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                {
                                    strPromptMsg = "未进入测试模式，请检查手机，电源，串口等连接是否异常";
                                }
                                frmRun1DutGui_ShowCurItem(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);
                                frmRun1DutGui_SetErrMsg( ERR_GUI_DUT_ENTER_META_FAILED, strPromptMsg);
                                frmRun1DutGui_ShowFailMsg();

                                frmRun1DutGui_WriteResFiles(g_sCurCfgCtrlSet);
                                frmRun1DutGui_WriteInfoFiles(g_sCurCfgCtrlSet);

                                if (g_sCurCfgCtrlSet.sRunAllInfo.iRestartATEDemo == 1)
                                {
                                    g_sCurCfgCtrlSet.sRunAllInfo.iContinueFailCnt += 1;

                                    if (g_sCurCfgCtrlSet.sRunAllInfo.iContinueFailCnt == g_sCurCfgCtrlSet.sRunAllInfo.iContinueFailMax)
                                    {
                                        g_sCurCfgCtrlSet.sRunAllInfo.iContinueFailCnt = 0;

                                        frmRunDoc1_ExitATE_Demo();
                                        //g_bAppRebootFlag = false;

                                        //Thread.Sleep(2000);

                                        //frmRunDoc1_StartProcessCalibrateMain(); 
                                    }                                  
                                }
                            }

                            if ((g_sCurCfgCtrlSet.sRunAllInfo.iEnablePowerSupplyControl == 1)
                                && (g_sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode == 2))
                            {

                                if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                                {
                                    strPromptMsg = "PSU power off begin";
                                }
                                else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                {
                                    strPromptMsg = "关闭程控电源开始";
                                }
                                frmRun1DutGui_ShowCurStatus(strPromptMsg);
                                frmRun1DutGui_ShowItemMsg(strPromptMsg);

                                Thread ntExeCtrlPowerOff = new Thread(new ThreadStart(sExeCtrl.clExeCtrl_PowerOff));
                                ntExeCtrlPowerOff.IsBackground = true;
                                ntExeCtrlPowerOff.Priority = ThreadPriority.Normal;
                                ntExeCtrlPowerOff.Start();
                                ntExeCtrlPowerOff.Join();

                                if (sExeCtrl.bPowerOff)
                                {
                                    if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 1)
                                    {
                                        strPromptMsg = "PSU power off successful";
                                    }
                                    else if (g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt == 2)
                                    {
                                        strPromptMsg = "关闭程控电源成功";
                                    }
                                    frmRun1DutGui_ShowCurStatus(strPromptMsg);
                                    frmRun1DutGui_ShowItemMsg(strPromptMsg);

                                }
                                else
                                {
                                    frmRun1DutGui_SetErrMsg(ERR_GUI_POWER_OFF_FAIL, "关闭程控电源失败");
                                    //sMsgCtrl.SendMsg(this.Handle, WM_RUN_ERR, "run_err", "power_off_failed");
                                    //return;
                                }
                            }

                            if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableFixtureControl == 1)
                            {
                                string strLogMsg = string.Format("{0:yyyy/MM/dd HH:mm:ss.fff}  打开屏蔽箱开始", DateTime.Now);

                                frmRun1DutGui_ShowCurStatus(strLogMsg);
                                frmRun1DutGui_ShowItemMsg(strLogMsg);

                                sExeCtrl.strCurRunPassFail = g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail;
                                Thread ntExeCtrlFixtureOpen = new Thread(new ThreadStart(sExeCtrl.clExeCtrl_FixtureOpen));
                                ntExeCtrlFixtureOpen.IsBackground = true;
                                ntExeCtrlFixtureOpen.Priority = ThreadPriority.Normal;
                                ntExeCtrlFixtureOpen.Start();
                                ntExeCtrlFixtureOpen.Join();

                                if (sExeCtrl.bFixtureOpen)
                                {
                                    //strLogMsg = "打开屏蔽箱成功";
                                    strLogMsg = string.Format("{0:yyyy/MM/dd HH:mm:ss.fff}  打开屏蔽箱成功", DateTime.Now);
                                    frmRun1DutGui_ShowCurStatus(strLogMsg);
                                    frmRun1DutGui_ShowItemMsg(strLogMsg);
                                }
                                else
                                {
                                    frmRun1DutGui_SetErrMsg(ERR_GUI_FIXTURE_OPEN_FAIL, "打开屏蔽箱失败");
                                    //sMsgCtrl.SendMsg(this.Handle, WM_RUN_ERR, "run_err", "fixture_open_failed");
                                    //return;
                                }
                            }
                            
                            frmRun1DutGui_CloseLogFiles();

                            //////////////_CURRENT_TESTS_BZ
                            
                            if ((g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS") && (Bz_Handler.CItemListEquip.IsFqaVerify() == 0))
                            {
                                    Bz_Handler.CI2cControl.ReleaseGPIBMutex();
                                    PowerOnCurrent();
                            }                   
                            if ((g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS") && (Bz_Handler.CItemListEquip.IsFqaVerify() == 0))
                            {
                                    ChargerCurrent();                                
                            }
                            if ((g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS") && (Bz_Handler.CItemListEquip.IsFqaVerify() == 0))
                            {
                                    StandByCurrent();

                            }
                            if ((g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS") && (Bz_Handler.CItemListEquip.IsFqaVerify() == 0))
                            {
                                    PowerOffCurrent();
                                    
                            }                            
                                                     
                            //////////////_CURRENT_TESTS END

                            if(g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS")
                            {
                                frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);  // Finalizando app MTK_atedemo.exe
                                frmRun1DutGui_ShowPassMsg();
                                Bz_Handler.CI2cControl.SendI2cCommand("PASS_LAMP_ON");

                            }
                            else
                            {
                                frmRun1DutGui_CloseApp(g_sCurCfgCtrlSet.sRunAllInfo.strCurExeFilePath);  // Finalizando app MTK_atedemo.exe
                                frmRun1DutGui_ShowFailMsg();
                                Bz_Handler.CI2cControl.SendI2cCommand("FAIL_LAMP_ON");
                            }

                            frmRunDoc1TimerRecord.Enabled = false;
                            frmRun1DutGui_CheckRepeatTime(g_sCurCfgCtrlSet);
                        
                            if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ" && MEASCODE == 0) // Patricio
                            {
                                int nStatus = -1;
                                Application.DoEvents();
                                StringBuilder strErrorMessage = new StringBuilder(256);

                                //FQA-Golden Process - Clean FQA directory
                                if(Bz_Handler.CItemListEquip.IsFqaVerify() == 1)                                
                                    foreach (string file in Directory.GetFiles(@"c:\prod\FQATemp\")) File.Delete(file);

                                nStatus = LogResult.LogResult(g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail, strErrorMessage.ToString());
                                //Thread.Sleep(100);
                                while (nStatus != 0)
                                {
                                    MessageBox.Show("Error to finish log result");
                                    MessageBox.Show(strErrorMessage.ToString());
                                }

                                // FQA-Golden Process - Run Calitool from Wrapper                               
                                if ((Bz_Handler.CItemListEquip.IsFqaVerify() == 1) && (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS"))
                                    nStatus = Bz_Handler.CItemListEquip.FQAVerifyAteRun();

                                if (nStatus == 0)
                                    nStatus = Bz_Handler.CI2cControl.ReleaseGPIBMutex(); 
                                  //labelTestStatusDut0.Text = "ReleaseGPIBMutex \n Test Set Liberado";

                                if (Bz_Handler.CItemListEquip.IsOpenJigEnable() == 1)
                                {
                                    if (nStatus == 0)
                                    {
                                        double dVoltage = -999;
                                        int nCloseJigCount = 0;
                                        string strPreviewMessage;
                                        strPreviewMessage = labelTestStatusDut0.Text.ToString();
                                        labelTestStatusDut0.Text = "Remova a placa do Fixture \n \n" + strPreviewMessage;
                                        nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_CHARLES_CLOSE");
                                        Thread.Sleep(100);
                                        nStatus = Bz_Handler.CJagLocalFucntions.CloseTunerDVM();
                                        Thread.Sleep(100);
                                        if (nStatus == 0)
                                        {
                                              //while (dVoltage < 0.2)
                                                  while (dVoltage < 2)
                                                {
                                                    dVoltage = Bz_Handler.CItemListEquip.ReadDVM1Voltage();
                                                    if (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS")
                                                    {
                                                        if (nCloseJigCount++ % 2 == 0)
                                                        {
                                                            labelTestStatusDut0.ForeColor = System.Drawing.Color.White;
                                                            labelTestStatusDut0.BackColor = System.Drawing.Color.Green;
                                                        }
                                                        else
                                                        {
                                                            labelTestStatusDut0.ForeColor = System.Drawing.Color.Green;
                                                            labelTestStatusDut0.BackColor = System.Drawing.Color.White;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (nCloseJigCount++ % 2 == 0)
                                                        {
                                                            labelTestStatusDut0.ForeColor = System.Drawing.Color.White;
                                                            labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                                                        }
                                                        else
                                                        {
                                                            labelTestStatusDut0.ForeColor = System.Drawing.Color.Red;
                                                            labelTestStatusDut0.BackColor = System.Drawing.Color.White;
                                                        }

                                                    }
                                                    Application.DoEvents();
                                                }
                                            }
                                                                                     
                                        nStatus = Bz_Handler.CJagLocalFucntions.OpenTunerDVM();
                                        nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_CHARLES_OPEN");


                                        if (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS")
                                        {
                                            labelTestStatusDut0.Text = "TELEFONE PASSOU NO TESTE";
                                            labelTestStatusDut0.BackColor = System.Drawing.Color.Green;
                                        }
                                        else
                                        {
                                            labelTestStatusDut0.Text = "Telefone " + Bz_Handler.CJagLocalFucntions.GetTrackId() + " falhou ! \n" + strPreviewMessage;
                                            labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                                        }
                                        labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                                        Application.DoEvents();
                                    }
                                }

                                if (nStatus == 0)
                                Thread.Sleep(100);
                                nStatus = Bz_Handler.CJagLocalFucntions.ExitHandlerTest(g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail);

                                while (nStatus != 0)
                                {
                                    MessageBox.Show("Exit Handler Error");
                                }
                                
                                // FQA-Golden Process - Close program if FQA process pass                               
                                if((Bz_Handler.CItemListEquip.IsFqaVerify() == 1)  && (g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail == "PASS"))
                                    Environment.Exit(0);

                                StartScanTimer.Enabled = true;
                            }
                        }
                        else if (strMsgVal == "run_stop")
                        {
                            ;
                        }
                        else if (strMsgVal == "run_exit")
                        {
                            ;
                        }
                    }
                    break;
                case WM_RUN_SUM:

                    break;
                case WM_RUN_CTRL:
                    strMsgName = string.Format("{0}", Marshal.PtrToStringAnsi(m.WParam).ToString());
                    strMsgVal = string.Format("{0}", Marshal.PtrToStringAnsi(m.LParam).ToString());
                    if (strMsgName == "run_ctrl")
                    {
                        if (strMsgVal == "operator")
                        {
                            tscbRunMode.Enabled = false;
                            tscbRunMode.SelectedIndex = 0;
                            g_sCurCfgCtrlSet.sRunAllInfo.iDutPassNum = 0;
                            g_sCurCfgCtrlSet.sRunAllInfo.iDutFailNum = 0;
                        }
                        else if (strMsgVal == "engineer")
                        {
                            tscbRunMode.Enabled = true;
                            tscbRunMode.SelectedIndex = 0;
                            g_sCurCfgCtrlSet.sRunAllInfo.iDutPassNum = 0;
                            g_sCurCfgCtrlSet.sRunAllInfo.iDutFailNum = 0;
                        }
                        else if (strMsgVal == "developer")
                        {
                            tscbRunMode.Enabled = true;
                            tscbRunMode.SelectedIndex = 0;
                            g_sCurCfgCtrlSet.sRunAllInfo.iDutPassNum = 0;
                            g_sCurCfgCtrlSet.sRunAllInfo.iDutFailNum = 0;
                        }
                        frmRun1DutGui_ShowSummaryVal(g_sCurCfgCtrlSet.sRunAllInfo.iDutPassNum, g_sCurCfgCtrlSet.sRunAllInfo.iDutFailNum);
                    }
                    break;
                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        private void btRunStart_Click(object sender, EventArgs e)
        {
            if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ") // Patricio
                return;
            sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_init");
                       
            if (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus == 0)
            {
                    if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableBbTest == 1) // BB Patricio
                    {
                        sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_bb_test");
                    }
            }
            if (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus == 0)
            {
                sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_prepare");

            }
        }
        
        private void frmRun1DutGui_InitToolUI()
        {
            frmRunDoc1TimerRecord.Enabled = true;
            g_sCurCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin = DateTime.Now;
            g_sCurCfgCtrlSet.sRunAllInfo.tsRunBegin = new TimeSpan(g_sCurCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Ticks);
                            
            this.labelSN.ResetText();
            this.labelSN.Text = "SN: ";

            this.tsbDutPortMode0.BackColor = System.Drawing.Color.Gray;
            this.tsbFixtureComPort0.BackColor = System.Drawing.Color.Gray;

            this.rtbRunItemDut0.ResetText();
            this.rtbRunItemDut0.BackColor = System.Drawing.SystemColors.Info;
            this.rtbRunItemDut0.ForeColor = System.Drawing.Color.Black;

            this.rtbRunLogDut0.ResetText();
            this.rtbRunLogDut0.BackColor = System.Drawing.SystemColors.Info;
            this.rtbRunLogDut0.ForeColor = System.Drawing.Color.Black;

            this.labelTestStatusDut0.ResetText();
            this.labelTestStatusDut0.BackColor = System.Drawing.Color.Black;
            this.labelTestStatusDut0.ForeColor = System.Drawing.Color.Yellow;

            this.labelTimeCost.Text = "000.000 s";
            this.labelTimeCost.ForeColor = System.Drawing.Color.Yellow;

            //string strPromptMsg = "进入射频校准 && 非信令测试流程";
            //frmRun1DutGui_ShowCurItem(strPromptMsg);
            //frmRun1DutGui_ShowItemMsg(strPromptMsg);
        }

        private void frmRun1DutGui_InitCurRunParas()
        {
            g_sCurCfgCtrlSet.sRunAllInfo.iRunRetryIndex = 0;
            g_sCurCfgCtrlSet.sRunAllInfo.iCurCalFileIndex = 0;
            g_sCurCfgCtrlSet.sRunAllInfo.iCurCalFileNum = 0;
            g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus = 0;
            sExeCtrl.g_ipFrmDut = this.Handle;
            sExeCtrl.iCurDutGroupIndex = g_sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex;
            sExeCtrl.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort;
            sExeCtrl.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort;
            sExeCtrl.iCurDutPortMode = g_sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode;
            sExeCtrl.iLanguageOpt = g_sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt;
            sExeCtrl.iFixtureType = g_sCurCfgCtrlSet.sRunAllInfo.iFixtureType;
            sExeCtrl.strDutSerialPortBaudRate = g_sCurCfgCtrlSet.sRunAllInfo.strDutSerialPortPreBaudRate;
            sExeCtrl.strDutSerialPortDataBits = g_sCurCfgCtrlSet.sRunAllInfo.strDutSerialPortDataBits;
            sExeCtrl.strDutSerialPortReadTimeout = g_sCurCfgCtrlSet.sRunAllInfo.strDutSerialPortReadTimeout;
            sExeCtrl.strDutSerialPortParity = g_sCurCfgCtrlSet.sRunAllInfo.strDutSerialPortParity;
            sExeCtrl.strDutSerialPortEncoding = g_sCurCfgCtrlSet.sRunAllInfo.strDutSerialPortEncoding;
            sExeCtrl.iRecodeCalFlag = g_sCurCfgCtrlSet.sRunAllInfo.iRecodeCalFlag;
            sExeCtrl.iDutResetAfterCal = g_sCurCfgCtrlSet.sRunAllInfo.iDutResetAfterCal;

            sExeCtrl.strCurRfSwitchComPort = g_sCurCfgCtrlSet.sRunAllInfo.strRfSwitchCom;

            sExeCtrl.strCurPowerSupplyType = g_sCurCfgCtrlSet.sRunAllInfo.strCurPowerSupplyType;
            sExeCtrl.strCurPowerSupplyAddr = g_sCurCfgCtrlSet.sRunAllInfo.strCurPowerSupplyAddr;
            sExeCtrl.dCurCh0PowerVoltage = g_sCurCfgCtrlSet.sRunAllInfo.dCurCh0PowerVoltage;
            sExeCtrl.dCurCh0PowerCurrent = g_sCurCfgCtrlSet.sRunAllInfo.dCurCh0PowerCurrent;
            sExeCtrl.dCurCh1PowerVoltage = g_sCurCfgCtrlSet.sRunAllInfo.dCurCh1PowerVoltage;
            sExeCtrl.dCurCh1PowerCurrent = g_sCurCfgCtrlSet.sRunAllInfo.dCurCh1PowerCurrent;
            sExeCtrl.iCurPowerChanIndex = g_sCurCfgCtrlSet.sRunAllInfo.iCurPowerChanIndex;

            sExeCtrl.strAtCmdLenovoGsn = g_sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoGsn;
            sExeCtrl.strAtCmdLenovoCheckVersion = g_sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoCheckVersion;
            sExeCtrl.strAtCmdLenovoEnterCalMode = g_sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoEnterCalMode;
            sExeCtrl.strAtCmdLenovoExitCalMode = g_sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoExitCalMode;
            sExeCtrl.strAtCmdLenovoBackupModemData = g_sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoBackupModemData;
            sExeCtrl.strAtCmdLenovoModemReset = g_sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoModemReset;
            sExeCtrl.strAtCmdLenovoModemLock = g_sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoModemLock;
            sExeCtrl.strAtCmdLenovoModemUnLock = g_sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoModemUnLock;
            sExeCtrl.strAtCmdIntelSetNvmCfg = g_sCurCfgCtrlSet.sRunAllInfo.strAtCmdIntelSetNvmCfg;
            sExeCtrl.strAtCmdIntelFixUsb = g_sCurCfgCtrlSet.sRunAllInfo.strAtCmdIntelFixUsb;
            sExeCtrl.strAtCmdIntelStoreNvmSync = g_sCurCfgCtrlSet.sRunAllInfo.strAtCmdIntelStoreNvmSync;
            sExeCtrl.strAtCmdIntelModemReset = g_sCurCfgCtrlSet.sRunAllInfo.strAtCmdIntelModemReset;

            g_sCurCfgCtrlSet.sRunAllInfo.bCopyFailResult = false;
            g_sCurCfgCtrlSet.sRunAllInfo.iItemNum = g_sCurCfgCtrlSet.sRunAllInfo.iStepNumCal + g_sCurCfgCtrlSet.sRunAllInfo.iStepNumVerify;            
        }

        private void frmRun1DutGui_InitRetryRunParas()
        {
            g_sCurCfgCtrlSet.sRunAllInfo.iCurCalFileIndex = 0;
            g_sCurCfgCtrlSet.sRunAllInfo.iCurCalFileNum = 0;           

            g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus = 0;
            g_sCurCfgCtrlSet.sRunAllInfo.iModemIndex = -1;
            g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex = 0;
            
            g_sCurCfgCtrlSet.sRunAllInfo.bRunCableCalPassed = false;
            g_sCurCfgCtrlSet.sRunAllInfo.bCalMainDone = false;
            g_sCurCfgCtrlSet.sRunAllInfo.bCalEnterMeta = false;

            g_sCurCfgCtrlSet.sRunAllInfo.bPrintSNInfo = false;

            for (g_sCurCfgCtrlSet.sRunAllInfo.iCurCfgFileIndex = 0; g_sCurCfgCtrlSet.sRunAllInfo.iCurCfgFileIndex < 4; g_sCurCfgCtrlSet.sRunAllInfo.iCurCfgFileIndex++)
            {
                g_sCurCfgCtrlSet.sRunAllInfo.straryCurCalFilePath[g_sCurCfgCtrlSet.sRunAllInfo.iCurCfgFileIndex] = "";
                g_sCurCfgCtrlSet.sRunAllInfo.straryCurCfgFilePath[g_sCurCfgCtrlSet.sRunAllInfo.iCurCfgFileIndex] = "";
            }

            g_sCurCfgCtrlSet.sRunAllInfo.iCurCfgFileIndex = 0;
            g_sCurCfgCtrlSet.sRunAllInfo.iCurCfgFileNum = 0;

            g_sCurCfgCtrlSet.sRunLogInfo.listRunCfgKeySet.Clear();
            g_sCurCfgCtrlSet.sRunLogInfo.listRunDataLogRfCalibrate.Clear();
            g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogRfCalibrate.Clear();
            g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogRfVerify.Clear();
            g_sCurCfgCtrlSet.sRunLogInfo.listRunSumLog.Clear();

            g_sCurCfgCtrlSet.sRunAllInfo.strCalBandsConfigInfo = "";

            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.iErrUpdate = 0;
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.iErrCode = 0;
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscChs = "";
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscEng = "";
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strChipErrorCode = "";
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strLenovoErrorCode = "";
            g_sCurCfgCtrlSet.sRunAllInfo.strRetDutSerialNumber = "";
            g_sCurCfgCtrlSet.sRunAllInfo.strLogDutSerialNumber = "";

            g_iResetCnt = 0;
          
        }
        //BB Patricio
        private void frmRun1DutGui_InitCurBbTestParas()
        {
            g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus = 0;
            g_sCurCfgCtrlSet.sRunAllInfo.bBbTestDone = false;
            g_sCurCfgCtrlSet.sRunLogInfo.listCfgResLogBbTest.Clear();
            g_sCurCfgCtrlSet.sRunLogInfo.listRunResLogBB.Clear();

            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.iErrUpdate = 0;
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.iErrCode = 0;
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscChs = "";
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strDiscEng = "";
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strChipErrorCode = "";
            g_sCurCfgCtrlSet.sRunLogInfo.sCurRunErrLog.strLenovoErrorCode = "";
        }
        //BB End Patricio

        private void frmRun1DutGui_InitUI()
        {
            frmRun1DutGui_ShowProgressVal(g_sCurCfgCtrlSet.sRunAllInfo.iItemIndex, g_sCurCfgCtrlSet.sRunAllInfo.iItemNum);

            this.tsbRunCfg.Enabled = false;
            this.btRunStart.Enabled = false;
            this.tsbRunStop.Enabled = false;
            this.tsbRunExit.Enabled = false;
            this.tsbRunStartCalibrateMain.Enabled = false;

            if (g_sCurCfgCtrlSet.sRunAllInfo.bFirstRun)
            {
                //frmRun1DutGui_ShowCurStatus("...等待夹具关闭...");
                g_sCurCfgCtrlSet.sRunAllInfo.bFirstRun = false;
            }

            g_sCurCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin = DateTime.Now;
            g_sCurCfgCtrlSet.sRunAllInfo.tsRunBegin = new TimeSpan(g_sCurCfgCtrlSet.sRunAllInfo.dtCurrentRunBegin.Ticks);

            //frmRunDoc1TimerRecord.Enabled = true;
            tsbRunStartCalibrateMain.Enabled = false;

        }
        private void tscbRunMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iStatus = 0;
            
            frmRunDoc1_ExitATE_Demo();

            g_sCurCfgCtrlSet.sRunAllInfo.iRunMode = tscbRunMode.SelectedIndex;

            switch (g_sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex)
            {
                case 0:
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup0DutComPort;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup0FixturePort;
                    break;
                case 1:
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup1DutComPort;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup1FixturePort;
                    break;
                case 2:
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup2DutComPort;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup2FixturePort;
                    break;
                case 3:
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup3DutComPort;
                    g_sCurCfgCtrlSet.sRunAllInfo.strCurFixtureComPort = g_sCurCfgCtrlSet.sRunAllInfo.strGroup3FixturePort;
                    break;
            }

            iStatus = sCfgCtrlRun.UpdateMtkCustomerSetupTxt(g_sCurCfgCtrlSet);
            if (iStatus != 0)
            {
                return;
            }
            iStatus = sCfgCtrlRun.UpdateMtkSetupIni_All(g_sCurCfgCtrlSet);
            if (iStatus != 0)
            {
                return;
            }
            iStatus = sCfgCtrlRun.UpdateMtkCfgTxt(g_sCurCfgCtrlSet);
            if (iStatus != 0)
            {
                return;
            }
        }

        private void tsbRunExit_Click(object sender, EventArgs e)
        {
            sMsgCtrl.SendMsg(g_hwndFrmRun1DutGui, WM_RUN_PROC, "run_proc", "run_exit");
        }
        public void StartTimer() // Patricio
        {
            StartScanTimer.Interval = 1000;
            StartScanTimer.Tick += new EventHandler(StartScan_Tick);
            StartScanTimer.Enabled = true;
         }

        private void StartScan_Tick(object sender, EventArgs e) //  Patricio
        {
            //Timer used to start scan process without press button start
            if ((Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ") && (Bz_Handler.CJagLocalFucntions.IsOkToScan()))
            {
                btRunStart.Enabled = false;

                StartScanTimer.Enabled = false;

                Bz_Handler.CJagLocalFucntions.SetTrackId("");

                Bz_Handler.CJagLocalFucntions.SetBzScanMode("FIRST_SCAN");

                if (!iOwnTheMutex)
                    iOwnTheMutex = ScanMutex.WaitOne();

                frmBzModel BzModelForm = new frmBzModel();
                BzModelForm.ShowDialog(this);
                g_sCurCfgCtrlSet.sRunAllInfo.strInputDutSerialNumber = Bz_Handler.CJagLocalFucntions.GetTrackId();

                if (iOwnTheMutex)
                {
                    ScanMutex.ReleaseMutex();
                    iOwnTheMutex = false;
                }
                if ((Bz_Handler.CJagLocalFucntions.GetTrackId() == "") || (Bz_Handler.CJagLocalFucntions.GetTrackId().Length != 10))
                    return;


                g_sCurCfgCtrlSet.sRunAllInfo.strRetDutSerialNumber = g_sCurCfgCtrlSet.sRunAllInfo.strInputDutSerialNumber;
                g_sCurCfgCtrlSet.sRunAllInfo.strLogDutSerialNumber = g_sCurCfgCtrlSet.sRunAllInfo.strInputDutSerialNumber;
                Bz_Handler.CJagLocalFucntions.SetTrackId(g_sCurCfgCtrlSet.sRunAllInfo.strInputDutSerialNumber);
                Bz_Handler.CItemListEquip.SetTrackId(g_sCurCfgCtrlSet.sRunAllInfo.strInputDutSerialNumber);

                labelTestStatusDut0.ResetText();
                labelTestStatusDut0.BackColor = System.Drawing.Color.Black;
                labelTestStatusDut0.ForeColor = System.Drawing.Color.Yellow;
                labelTestStatusDut0.Text = "CHECK STATUS TEST";
                Application.DoEvents();

                // GetUnitInfo GPS Function Patricio
                if (Bz_Handler.CItemListEquip.IsChecStatusEnable() == 1) // Check Status!!! Patricio
                {                    
                    Bz_Handler.CJagLocalFucntions.SetTrackId(frmBzModel.strTrackId.ToString().Substring(0, 10));

                    if (Bz_Handler.CItemListEquip.IsChecStatusEnable() == 1)
                    {
                        int nStatus = -1;
                        string errorMessage = string.Empty;

                        string strStationCode;
                        string strStnId;
                        string strStnLine;
                        StringBuilder strStationId = new StringBuilder(16);
                        StringBuilder strStationLine = new StringBuilder(16);
                        StringBuilder strStationType = new StringBuilder(16);
                        StringBuilder strServerURL = new StringBuilder(256);

                        Bz_Handler.CItemListEquip.GetStationLine(strStationLine);
                        Bz_Handler.CItemListEquip.GetStationType(strStationType);
                        Bz_Handler.CItemListEquip.GetStationID(strStationId);
                        Bz_Handler.CItemListEquip.GetFlowControlQueryURL(strServerURL);

                        strStnId = strStationId.ToString();
                        strStnLine = strStationLine.ToString();
                        strStationCode = strStnLine + "-" + strStnId;

                        RequestParameters request = new RequestParameters();
                        request.URI = @strServerURL.ToString();
                        //request.URI = @"http://jagnt001.americas.ad.flextronics.com/FF_Http_AutoTester/default.aspx"; // FLEXFLOW
                        request.orderNumber = "";
                        request.stationCode = strStationCode;
                        request.stationType = strStationType.ToString();
                        request.testMachineId = "";
                        request.unitTrackId = frmBzModel.strTrackId.ToString().Substring(0, 10);

                        string responseString = string.Empty;
                        nStatus = TPWrapper.GpsAcquisition.Request(request, out responseString, out errorMessage);

                        if (nStatus != 0)
                        {
                            MessageBox.Show(errorMessage);
                            labelTestStatusDut0.Text = "CHECK STATUS TEST FAIL";
                            labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                            StartScanTimer.Enabled = true;
                            return;
                        }
                        else
                        {
                            ResponseParameters responseParameters = TPWrapper.GpsAcquisition.GetParametersValuesFromStringResponse(responseString);

                            Bz_Handler.CJagLocalFucntions.SetCheckStatusResult("Init");
                            // Response
                            string batteryPartNumber = responseParameters.batteryPartNumber;
                            string boardKitNumber = responseParameters.boardKitNumber;
                            string cfcSvn = responseParameters.cfcSvn;
                            string endoPartNumber = responseParameters.endoPartNumber;
                            string errorMsg = responseParameters.errorMessage;
                            string facSvn = responseParameters.facSvn;
                            string fvcNumber = responseParameters.fvcNumber;
                            string orderNumber = responseParameters.orderNumber;
                            string permission = responseParameters.permission;
                            string salesModel = responseParameters.salesModel;
                            string transceiverNumber = responseParameters.transceiverNumber;
                            string unitTrackId = responseParameters.unitTrackId;

                            if (permission.Equals("0"))
                            {
                                nStatus = 0;
                                Bz_Handler.CJagLocalFucntions.SetCheckStatusResult("PASS");
                            }
                            else if (permission.Equals("1"))
                            {
                                nStatus = -1;
                                MessageBox.Show(errorMsg);
                                labelTestStatusDut0.Text = "CHECK_STATUS_FAIL";
                                labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                                labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                                StartScanTimer.Enabled = true;
                                return;

                            }
                            else
                            {
                                nStatus = -1;
                                MessageBox.Show("Application does not know if the phone is OK to test");
                                labelTestStatusDut0.Text = "CHECK_STATUS_FAIL";
                                labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                                labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                                StartScanTimer.Enabled = true;
                                return;
                            }
                        }

                        if (nStatus != 0)
                        {
                            Bz_Handler.CJagLocalFucntions.SetCheckStatusResult("FAIL");
                            labelTestStatusDut0.Text = "CHECK_STATUS_FAIL";
                            labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                            labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                            StartScanTimer.Enabled = true;
                            return;
                        }
                    }
                    else
                    {
                        Bz_Handler.CJagLocalFucntions.SetCheckStatusResult("PASS");
                    }                 
                        
                }    

                if (Bz_Handler.CJagLocalFucntions.EntryHandlerTest() != 0)
                {
                    labelTestStatusDut0.Text = "Entry_Handler_FAIL";
                    labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                    labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                    StartScanTimer.Enabled = true;
                    return;
                }
                // Performing Close Jig Test Patricio
                #region CloseJig

                if (Bz_Handler.CItemListEquip.IsOpenJigEnable() == 1)
                {
                      
                    int nStatus = -1;
                    double dVoltage = 999;
                    int nCloseJigCount = 0;

                    labelTestStatusDut0.Text = "CLOSE JIG TEST \n ATENCAO:Insira a unidade no Fixture!!!";
                    Application.DoEvents();

                    nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_CHARLES_CLOSE");

                    if (nStatus == 0)
                    {
                        //while (dVoltage > 0.2 && nCloseJigCount < 50)
                            while (dVoltage > 2 && nCloseJigCount < 50)
                        {
                            dVoltage = Bz_Handler.CItemListEquip.ReadDVM1Voltage();

                            if (nCloseJigCount++ % 2 == 0)
                            {
                                labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                                labelTestStatusDut0.BackColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                labelTestStatusDut0.ForeColor = System.Drawing.Color.White;
                                labelTestStatusDut0.BackColor = System.Drawing.Color.Black;
                            }
                            Application.DoEvents();
                        }
                    }

                    nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_CHARLES_OPEN");

                    if (dVoltage > 2)
                    {
                        labelTestStatusDut0.Text = "CLOSE_JIG_FAIL";
                        labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                        labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                        Bz_Handler.CI2cControl.SendI2cCommand("FAIL_LAMP_ON");
                        StartScanTimer.Enabled = true;
                        return;
                    }

                    labelTestStatusDut0.Text = "FIXTURE FECHADO !!! \n Iniciando Teste";
                    labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                    //labelTestStatusDut0.BackColor = System.Drawing.Color.Yellow;
                    Application.DoEvents();
                }                 

                #endregion


                if (Bz_Handler.CItemListEquip.IsFqaVerify() == 1)
                {
                    //string strPromptMsg = "";
                    //strPromptMsg = string.Format("                       !!! BANCADA EM FQA VERIFY !!!");
                    //frmRun1DutGui_ShowMsgPrompt(-1, strPromptMsg);
                    //labelTestStatusDut0.ResetText();
                    labelTestStatusDut0.Text = "FQA VERIFY !!! \n Não usar para produção!";
                    labelTestStatusDut0.ForeColor = System.Drawing.Color.Green;
                    labelTestStatusDut0.BackColor = System.Drawing.Color.White;
                    labelSN.BackColor = System.Drawing.Color.Green;
                    rtbRunItemDut0.BackColor = System.Drawing.Color.Green;
                    Application.DoEvents();
                    Thread.Sleep(1500);
                }

                if (Bz_Handler.CItemListEquip.IsFqaGoldenGenerate() == 1) // Gerar Arq.cal (GOLDEN) no prod\autocal
                {
                    labelTestStatusDut0.ResetText();
                    labelTestStatusDut0.Text = "FQA GOLDEN GENERATE !!! \n Nao usar para producao \n Arquivo sera gerado em V:\\Autocal";
                    labelTestStatusDut0.ForeColor = System.Drawing.Color.Blue;
                    labelTestStatusDut0.BackColor = System.Drawing.Color.White;
                    labelSN.BackColor = System.Drawing.Color.Green;
                    rtbRunItemDut0.BackColor = System.Drawing.Color.Green;
                    Application.DoEvents();
                    Thread.Sleep(3000);
                }

                //if (Bz_Handler.CJagLocalFucntions.EntryHandlerTest() != 0)
                //{
                //    labelTestStatusDut0.Text = "Entry_Handler_FAIL";
                //    labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                //    labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                //    StartScanTimer.Enabled = true;
                //    return;
                //}
                StringBuilder strErrormessage = new StringBuilder(256);
                if (LogResult.StartLogResult(strErrormessage.ToString()) != 0)
                {
                    labelTestStatusDut0.Text = "LogResult Start Failed";
                    labelTestStatusDut0.ForeColor = System.Drawing.Color.Black;
                    labelTestStatusDut0.BackColor = System.Drawing.Color.Red;
                    MessageBox.Show(strErrormessage.ToString());
                    StartScanTimer.Enabled = true;
                    return;
                }
                
                //Starting Test

                sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_init");

                if (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus == 0)
                {
                    //if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableBbTest == 1 && Bz_Handler.CItemListEquip.IsFqaVerify() == 0)                    
                    sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_bb_test"); // BB start
                    
                }  

                if (g_sCurCfgCtrlSet.sRunAllInfo.iRunStatus == 0)
                {                
                     sMsgCtrl.SendMsg(this.Handle, WM_RUN_PROC, "run_proc", "run_prepare"); // Calibration Start

                     
                     if (g_sCurCfgCtrlSet.sRunAllInfo.iEnableBbTest == 1)
                          {                                
                          }  
                           else
                             {
                                g_sCurCfgCtrlSet.sRunAllInfo.strCurRunPassFail = "PASS";
                                sMsgCtrl.SendMsg(g_hwndFrmRun1DutGui, WM_RUN_PROC, "run_proc", "run_release");
                      }                                                      
                 }               
            }
        }
    }
}
