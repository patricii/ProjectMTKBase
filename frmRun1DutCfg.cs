using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Math;
using System.Management;

namespace ateRun
{
    public partial class frmRun1DutCfg : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public IntPtr g_ipFrmMain;
        public const int USER = 0x0400;
        public const int WM_RUN_CFG = USER + 101;
        public const int WM_RUN_SUM = USER + 102;
        public const int WM_RUN_LOG = USER + 103;
        public const int WM_RUN_CTRL = USER + 201;
        public int iCurDutGroupIndex = 0;
        public int iEnableFixtureControl = 0;
        public string strCurDutComPort;
        public string strCurFixtureComPort;
        public int iEnablePowerSupplyControl = 0;
        public int iGuiWidth = 0;
        public int iGuiLeft = 0;
        public string strDutNum = "";
        public int iLanguageOpt = 1;
        
        public bool bRunDoc1CfgOK = false;
        public string[] straryAllDutPortModeSupport = new string[4];
        public int iCurDutPortMode;
        public int iDutPreloaderComPort;
        public int iDutGadgetComPort;
        public string[] straryAllTesterType = new string[4];
        public string[] straryAllTesterPort = new string[4];
        public string[] straryAllPowerSupplyType = new string[4];
        public string[] straryAllSupportResLevelType = new string[4];
        public string strCurExeFilePath = "";
        public string strCurCfgFilePath = "";
        public string strCurEattFilePath = "";
        public string strCurResLevel = "";
        public string strCurTesterType = "IQXSTREAM";
       
        public string strCurTesterPort = "RF1C";
        public string strCurTesterAddr = "GPIB0::10::INSTR";
        public string strCurPowerSupplyAddr = "GPIB0::8::INSTR";
        public string strCurPowerSupplyType = "AG66319B";
        public int iTimeoutPingPang = 300;
        public int iCurPowerChanIndex;
        public double dCurCh0PowerVoltage;
        public double dCurCh0PowerCurrent;
        public double dCurCh1PowerVoltage;
        public double dCurCh1PowerCurrent;
        public int iRepeatModeOpt = 0;
        public int iFixtureCloseModeOpt = 0;
        public int iStopModeOpt = 0;
        public int iRunDutCalDelayTime = 0;
        public int iRunDutCalRepeatNum = 0;
        public int iRunCableCalRepeatNum = 5;
        public double dCurAttnMinValue = 7.00;
        public double dCurAttnMaxValue = 10.00;
        public double dGsmBandDiffMaxValue = 1.00;
        public double dDeltaMaxValue = 0.50;
        public string strAttnSourceCfgFile1 = "";
        public string strAttnSourceCfgFile2 = "";
        public string strAttnTargetCfgFile1 = "";
        public string strAttnTargetCfgFile2 = "";
        public int iEnableAttnSourceCfgFile1 = 0;
        public int iEnableAttnSourceCfgFile2 = 0;
        public int iEnableAttnTargetCfgFile1 = 0;
        public int iEnableAttnTargetCfgFile2 = 0;
        public int iEnablePingPang = 0;

        public int iFixtureType = 0;
        /// <summary>
        /// 枚举win32 api
        /// </summary>
        public enum HardwareEnum
        {
            // 硬件
            Win32_Processor, // CPU 处理器
            Win32_PhysicalMemory, // 物理内存条
            Win32_Keyboard, // 键盘
            Win32_PointingDevice, // 点输入设备，包括鼠标。
            Win32_FloppyDrive, // 软盘驱动器
            Win32_DiskDrive, // 硬盘驱动器
            Win32_CDROMDrive, // 光盘驱动器
            Win32_BaseBoard, // 主板
            Win32_BIOS, // BIOS 芯片
            Win32_ParallelPort, // 并口
            Win32_SerialPort, // 串口
            Win32_SerialPortConfiguration, // 串口配置
            Win32_SoundDevice, // 多媒体设置，一般指声卡。
            Win32_SystemSlot, // 主板插槽 (ISA & PCI & AGP)
            Win32_USBController, // USB 控制器
            Win32_NetworkAdapter, // 网络适配器
            Win32_NetworkAdapterConfiguration, // 网络适配器设置
            Win32_Printer, // 打印机
            Win32_PrinterConfiguration, // 打印机设置
            Win32_PrintJob, // 打印机任务
            Win32_TCPIPPrinterPort, // 打印机端口
            Win32_POTSModem, // MODEM
            Win32_POTSModemToSerialPort, // MODEM 端口
            Win32_DesktopMonitor, // 显示器
            Win32_DisplayConfiguration, // 显卡
            Win32_DisplayControllerConfiguration, // 显卡设置
            Win32_VideoController, // 显卡细节。
            Win32_VideoSettings, // 显卡支持的显示模式。

            // 操作系统
            Win32_TimeZone, // 时区
            Win32_SystemDriver, // 驱动程序
            Win32_DiskPartition, // 磁盘分区
            Win32_LogicalDisk, // 逻辑磁盘
            Win32_LogicalDiskToPartition, // 逻辑磁盘所在分区及始末位置。
            Win32_LogicalMemoryConfiguration, // 逻辑内存配置
            Win32_PageFile, // 系统页文件信息
            Win32_PageFileSetting, // 页文件设置
            Win32_BootConfiguration, // 系统启动配置
            Win32_ComputerSystem, // 计算机信息简要
            Win32_OperatingSystem, // 操作系统信息
            Win32_StartupCommand, // 系统自动启动程序
            Win32_Service, // 系统安装的服务
            Win32_Group, // 系统管理组
            Win32_GroupUser, // 系统组帐号
            Win32_UserAccount, // 用户帐号
            Win32_Process, // 系统进程
            Win32_Thread, // 系统线程
            Win32_Share, // 共享
            Win32_NetworkClient, // 已安装的网络客户端
            Win32_NetworkProtocol, // 已安装的网络协议
            Win32_PnPEntity,//all device
        }
        /// <summary>
        /// WMI取硬件信息
        /// </summary>
        /// <param name="hardType"></param>
        /// <param name="propKey"></param>
        /// <returns></returns>
        public static string[] MulGetDutComPortInfo(int iCurDutPortMode, HardwareEnum hardType, string propKey)
        {
            string[] straryRetBufAll = new string[60];
            string strComPortInfo = "";
            List<string> strs = new List<string>();
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + hardType))
                {
                    var hardInfos = searcher.Get();
                    foreach (var hardInfo in hardInfos)
                    {
                        if (hardInfo.Properties[propKey].Value.ToString().Contains("COM") && hardInfo.Properties[propKey].Value.ToString().Contains("Serial") && hardInfo.Properties[propKey].Value.ToString().Contains("Port"))
                        {
                            strComPortInfo = hardInfo.Properties[propKey].Value.ToString();
                            straryRetBufAll = strComPortInfo.Split(new char[2] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                            if (straryRetBufAll.Length > 1)
                            {
                                foreach (string strComPort in straryRetBufAll)
                                {
                                    if (strComPort.Contains("COM"))
                                    {
                                        strs.Add(strComPort);
                                    }
                                }
                            }
                        }
                    }
                    searcher.Dispose();
                }
                return strs.ToArray();
            }
            catch
            {
                return null;
            }
            finally
            {
                strs = null; 
            }
            return strs.ToArray();
        }

        public static string[] MulGetFixtureComPortInfo(int iCurDutPortMode, HardwareEnum hardType, string propKey)
        {
            string[] straryRetBufAll = new string[60];
            string strComPortInfo = "";
            List<string> strs = new List<string>();
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + hardType))
                {
                    var hardInfos = searcher.Get();
                    foreach (var hardInfo in hardInfos)
                    {
                        if (hardInfo.Properties[propKey].Value.ToString().Contains("COM"))
                        {
                            strComPortInfo = hardInfo.Properties[propKey].Value.ToString();
                            straryRetBufAll = strComPortInfo.Split(new char[2] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                            if (straryRetBufAll.Length > 1)
                            {
                                foreach (string strComPort in straryRetBufAll)
                                {
                                    if (strComPort.Contains("COM"))
                                    {
                                        strs.Add(strComPort);
                                    }
                                }
                            }
                        }

                    }
                    searcher.Dispose();
                }
                return strs.ToArray();
            }
            catch
            {
                return null;
            }
            finally
            {
                strs = null;
            }
            return strs.ToArray();
        }

        public frmRun1DutCfg()
        {            
            InitializeComponent();            
        }

        private void frmRunSet_Shown(object sender, EventArgs e)
        {
        }

        private void tsbRunCfgDefault_Click(object sender, EventArgs e)
        {

        }

        private void tsbRunCfgSave_Click(object sender, EventArgs e)
        {

        }

        private void tsbRunDoc1CfgOK_Click(object sender, EventArgs e)
        {
            
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            string[] straryRetBufAll = new string[60];
            string strTextVal = "";
            strCurExeFilePath = tbExeFilePath.Text;  
            
            bRunDoc1CfgOK = true;

            strCurTesterType = cbMeasInstrType.Text;
            strCurTesterPort = cbMeasInstrPort.Text;
            strCurTesterAddr = tbTesterAddr.Text;
            iTimeoutPingPang = Int32.Parse(tbPingPangTimeOutS.Text);
            strCurPowerSupplyType = cbPowerSupplyType.Text;
            strCurPowerSupplyAddr = tbPowerSupplyAddr.Text;
            iCurPowerChanIndex = Int32.Parse(cbPowerSupplyChan.Text);
            if (iCurPowerChanIndex == 0)
            {
                dCurCh0PowerVoltage = Double.Parse(tbPowerSupplyVoltage.Text);
                dCurCh0PowerCurrent = Double.Parse(tbPowerSupplyCurrent.Text);
            }
            else if (iCurPowerChanIndex == 1)
            {
                dCurCh1PowerVoltage = Double.Parse(tbPowerSupplyVoltage.Text);
                dCurCh1PowerCurrent = Double.Parse(tbPowerSupplyCurrent.Text);
            }
            dCurAttnMinValue = Double.Parse(tbAttnMinVal.Text);
            dCurAttnMaxValue = Double.Parse(tbAttnMaxVal.Text);
            dGsmBandDiffMaxValue = Double.Parse(tbAttnGsmDiffMaxVal.Text);
            dDeltaMaxValue = Double.Parse(tbAttnChanDiffMaxVal.Text);
            iRunCableCalRepeatNum = Int32.Parse(tbAttnMeasNum.Text);
            iRepeatModeOpt = cbRepeatMode.SelectedIndex + 1;
            iFixtureCloseModeOpt = cbFixtureCloseMode.SelectedIndex + 1;
            iStopModeOpt = cbStopMode.SelectedIndex + 1;
            iRunDutCalRepeatNum = Int32.Parse(tbRepeatNum.Text);
            iRunDutCalDelayTime = Int32.Parse(tbDelayTime.Text);
            iCurDutPortMode = cbDutPortMode.SelectedIndex + 1;
            iDutPreloaderComPort = Int32.Parse(tbDutPreloaderCom.Text);
            iDutGadgetComPort = Int32.Parse(tbDutGadgetVCom.Text);
            iFixtureType = cbFixtureType.SelectedIndex;

            strTextVal = cbDutComPort.Text;
            strCurDutComPort = cbDutComPort.Text;

            if (cbEnableFixtureControl.Checked)
            {
                strCurFixtureComPort = cbFixtureComPort.Text;
                if (!strTextVal.Contains("COM"))
                {
                    bRunDoc1CfgOK = false;
                    this.Close();
                }
            }

            if (cbEnablePowerSupplyControl.Checked
                && (cbPowerSupplyType.Text.Length < 1)
                && (cbPowerSupplyChan.Text.Length < 1)
                && (tbPowerSupplyAddr.Text.Length < 1)
                && (tbPowerSupplyVoltage.Text.Length < 1)
                && (tbPowerSupplyCurrent.Text.Length < 1))
            {
                bRunDoc1CfgOK = false;
                this.Close();
            }

            bRunDoc1CfgOK = true;
            this.Close();
            
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            bRunDoc1CfgOK = false;
            this.Close();
        }

        private void btSelectTestExePath_Click(object sender, EventArgs e)
        {

        }

        private void btSelectTestPlanPath_Click(object sender, EventArgs e)
        {

        }

        private void btSelectEattFilePath_Click(object sender, EventArgs e)
        {

        }

        private void btSelectTestResultPath_Click(object sender, EventArgs e)
        {

        }

        private void frmRunDoc1Cfg_Load(object sender, EventArgs e)
        {
            int iDutPortIndex = 0;
            int iInstrTypeIndex = 0;
            int iInstrPortIndex = 0;
            int iPowerSupplyTypeIndex = 0;
            int iResLevelTypeIndex = 0;           

            string[] straryDutComPort;            

            this.Width = iGuiWidth - 40;
            this.Left = iGuiLeft + 20;

            try
            {
            tbExeFilePath.Text = strCurExeFilePath;  
            cbMeasInstrType.Text = strCurTesterType;
            cbMeasInstrPort.Text = strCurTesterPort;
            tbTesterAddr.Text = strCurTesterAddr;
            tbPingPangTimeOutS.Text = string.Format("{0}", iTimeoutPingPang);
            tbAttnMinVal.Text = string.Format("{0:F2}", dCurAttnMinValue);
            tbAttnMaxVal.Text = string.Format("{0:F2}", dCurAttnMaxValue);
            tbAttnGsmDiffMaxVal.Text = string.Format("{0:F2}", dGsmBandDiffMaxValue);
            tbAttnChanDiffMaxVal.Text = string.Format("{0:F2}", dDeltaMaxValue);
            tbAttnMeasNum.Text = string.Format("{0}", iRunCableCalRepeatNum);

            cbMeasInstrType.Items.Clear();
            cbMeasInstrPort.Items.Clear();
            cbPowerSupplyType.Items.Clear();
            cbDutComPort.Items.Clear();
            cbRepeatMode.Items.Clear();
            cbFixtureCloseMode.Items.Clear();
            cbResLevelOpt.Items.Clear();
            cbStopMode.Items.Clear();
            cbFixtureType.Items.Clear();

            if (iLanguageOpt == 1) //English
            {
                this.Text = "Dut Settings";
                this.labelSetPromptMsg.Text = "Please set the dut configurations";
                this.cbRepeatMode.Items.Add("Normal Mode");
                this.cbRepeatMode.Items.Add("Reapte Mode");
                this.cbFixtureCloseMode.Items.Add("Manually Close");
                this.cbFixtureCloseMode.Items.Add("Automatic Close");
                this.cbStopMode.Items.Add("Stop On Fail");
                this.cbStopMode.Items.Add("Continue On Fail");
                this.cbFixtureType.Items.Add("R_H");
                this.cbFixtureType.Items.Add("B_P");
                this.cbFixtureType.Items.Add("B_J");
                this.cbFixtureType.Items.Add("J_Z");

                this.labelInstrPortSelect.Text = "Port:";
                this.labelSetPrompt.Text = "Setting Prompt:";

                this.labelInstrAddress.Text = "Address:";
                this.labelInstrOpt.Text = "Select:";
                this.labelPingPangTimeout.Text = "Timeout:";
                this.cbEnablePingPang.Text = "Enable Pingpang";

                this.labelSelectComPort.Text = "Com Port Select:";

                this.btSelectAttnTargetCfgFile1.Text = "Target Cfg File1:";
                this.btSelectAttnSourceCfgFile1.Text = "Source Cfg File1:";
                this.btSelectAttnTargetCfgFile2.Text = "Target Cfg File2:";
                this.btSelectAttnSourceCfgFile2.Text = "Source Cfg File2:";

                this.tpCfgDut0.Text = "Dut Settings";
                this.labelPreloaderPort.Text = "Preloader Port:";
                this.labelGadgetVcomPort.Text = "Gadget VCOM Port:";
                this.labelSelectPortType.Text = "Com Port Select:";


                this.labelExeFilePath.Text = "Execute File Path:";


                this.tpCfgPower0.Text = "Supply Power Setting";
                this.labelPowerType.Text = "Supply Power Type:";
                this.labelPowerAddress.Text = "Supply Power Address:";
                this.labelPowerChannelSelect.Text = "Supply Power Channel:";
                this.labelPowerCurrentSet.Text = "Supply Power Current:";
                this.labelPowerVoltageSet.Text = "Supply Power Voltage:";


                this.btOK.Text = "OK";
                this.btCancel.Text = "Cancel";

                this.tpCfgRun0.Text = "Common Test Setting";
                this.labelDataLevel.Text = "Data Level:";
                this.labelWaitTime.Text = "Wait Time:";
                this.labelRetryMode.Text = "Retry Mode:";
                this.labelTestFunction.Text = "Test Function:";
                this.labelRetryTime.Text = "Retry Mode:";

                this.labelMaxFreqDiff.Text = "Max Freq CL Diff:";
                this.labelMaxBandDiff.Text = "Max Band CL Diff:";
                this.labelCLMeasNum.Text = "CL Meas Num:";
                this.labelCLMaxValue.Text = "CL Max Value:";
                this.labelCLMinValue.Text = "CL Min Value:";

                this.tpCfgAttn0.Text = "CL Calibration";
                this.cbEnablePowerSupplyControl.Text = "Enable Power Supply Control";

                this.labelFixtureType.Text = "Fixture Type Select:";
                this.tpCfgFixture0.Text = "Fixture Setting";
                this.cbEnableFixtureControl.Text = "Enable Fixture Control";
                this.labelFixtureComPort.Text = "Fixture Com Port:";
                this.labelFixtureOperateMode.Text = "Operate Mode:";

                this.tpCfgInstr0.Text = "Instrument";

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
                this.Text = "待测件设置";
                this.labelSetPromptMsg.Text = "请设置手机参数";
                this.cbRepeatMode.Items.Add("常规测试模式");
                this.cbRepeatMode.Items.Add("重复测试模式");
                this.cbFixtureCloseMode.Items.Add("手动关闭");
                this.cbFixtureCloseMode.Items.Add("程控关闭");
                this.cbStopMode.Items.Add("失败后停止");
                this.cbStopMode.Items.Add("失败后继续");
                this.cbFixtureType.Items.Add("R_H");
                this.cbFixtureType.Items.Add("B_P");
                this.cbFixtureType.Items.Add("B_J");
                this.cbFixtureType.Items.Add("J_Z");

                this.tpCfgInstr0.Text = "测试仪表设置";

                this.labelInstrPortSelect.Text = "端口选择:";
                this.labelSetPrompt.Text = "设置提示:";

                this.labelInstrAddress.Text = "地址:";
                this.labelInstrOpt.Text = "选择:";
                this.labelPingPangTimeout.Text = "超时:";
                this.cbEnablePingPang.Text = "启用乒乓测试";

                this.labelSelectComPort.Text = "串口选择:";
                
                this.btSelectAttnTargetCfgFile1.Text = "目标配置文件1:";
                this.btSelectAttnSourceCfgFile1.Text = "源配置文件1:";
                this.btSelectAttnTargetCfgFile2.Text = "目标配置文件2:";
                this.btSelectAttnSourceCfgFile2.Text = "源配置文件2:";

                this.tpCfgDut0.Text = "待测件配置";
                this.labelPreloaderPort.Text = "Preloader口:";
                this.labelGadgetVcomPort.Text = "Gadget VCOM口:";
                this.labelSelectPortType.Text = "端口类型选择:";
                

                this.labelExeFilePath.Text = "执行程序路径:";
                

                this.tpCfgPower0.Text = "程控电源设置";
                this.labelPowerType.Text = "类型选择:";
                this.labelPowerAddress.Text = "电源地址:";
                this.labelPowerChannelSelect.Text = "通道选择:";
                this.labelPowerCurrentSet.Text = "电流设置:";
                this.labelPowerVoltageSet.Text = "电压设置:";
                

                this.btOK.Text = "确定";
                this.btCancel.Text = "取消";

                this.tpCfgRun0.Text = "通用测试设置";
                this.labelDataLevel.Text = "数据级别:";
                this.labelWaitTime.Text = "等待时间";
                this.labelRetryMode.Text = "循环模式:";
                this.labelTestFunction.Text = "测试功能:";
                this.labelRetryTime.Text = "循环次数";

                this.labelMaxFreqDiff.Text = "频点差异最大值:";
                this.labelMaxBandDiff.Text = "频段差异最大值:";
                this.labelCLMeasNum.Text = "线损测量次数:";
                this.labelCLMaxValue.Text = "线损最大值:";
                this.labelCLMinValue.Text = "线损最小值:";

                this.tpCfgAttn0.Text = "线损校准设置";
                this.cbEnablePowerSupplyControl.Text = "启用电源控制";

                this.labelFixtureType.Text = "夹具类型选择:";
                this.tpCfgFixture0.Text = "自动夹具设置";
                this.cbEnableFixtureControl.Text = "启用夹具控制";
                this.labelFixtureComPort.Text = "夹具串口选择:";
                this.labelFixtureOperateMode.Text = "操作方式选择:";

                switch (iCurDutGroupIndex)
                {
                    case 0:
                        labelSetPromptMsg.Text = "请设置第一个手机参数";
                        break;
                    case 1:
                        labelSetPromptMsg.Text = "请设置第二个手机参数";
                        break;
                    case 2:
                        labelSetPromptMsg.Text = "请设置第三个手机参数";
                        break;
                    case 3:
                        labelSetPromptMsg.Text = "请设置第四个手机参数";
                        break;
                }

            }       


            cbRepeatMode.SelectedIndex = iRepeatModeOpt - 1;
            cbFixtureCloseMode.SelectedIndex = iFixtureCloseModeOpt - 1;
            cbStopMode.SelectedIndex = iStopModeOpt - 1;

            tbDelayTime.Text = string.Format("{0}", iRunDutCalDelayTime);
            tbRepeatNum.Text = string.Format("{0}", iRunDutCalRepeatNum);
            tbAttnMeasNum.Text = string.Format("{0}", iRunCableCalRepeatNum);

            //更新仪表选择
            foreach (string strInstr in straryAllTesterType)
            {
                if (strInstr != null)
                {
                    cbMeasInstrType.Items.Add(strInstr);
                }
            }

            foreach (string strInstr in straryAllTesterType)
            {
                if (strInstr.Contains(strCurTesterType))
                {
                    break;
                }
                else
                {
                    iInstrTypeIndex++;
                }
            }

            if (iInstrTypeIndex == straryAllTesterType.Length)
            {
                iInstrTypeIndex = 0;
            }

            if (cbMeasInstrType.Items.Count > 0)
            {
                cbMeasInstrType.SelectedIndex = iInstrTypeIndex;
            }

            //更新仪表测量端口选择
            foreach (string strPort in straryAllTesterPort)
            {
                if (strPort != null)
                {
                    cbMeasInstrPort.Items.Add(strPort);
                }
            }

            foreach (string strRfPort in straryAllTesterPort)
            {
                if (strRfPort.ToUpper().Equals(strCurTesterPort.ToUpper()))
                {
                    break;
                }
                else
                {
                    iInstrPortIndex++;
                }
            }

            if (iInstrPortIndex == straryAllTesterPort.Length)
            {
                iInstrPortIndex = 0;
            }

            if (cbMeasInstrPort.Items.Count > 0)
            {
                cbMeasInstrPort.SelectedIndex = iInstrPortIndex;
            }

            //更新芯片控制端口类型选择
            foreach (string strDutPortMode in straryAllDutPortModeSupport)
            {
                if (strDutPortMode != null)
                {
                    cbDutPortMode.Items.Add(strDutPortMode);
                }
            }

            if (iCurDutPortMode <= 0)
            {
                cbDutPortMode.SelectedIndex = 0;
            }
            else if (iCurDutPortMode > cbDutPortMode.Items.Count)
            {
                cbDutPortMode.SelectedIndex = cbDutPortMode.Items.Count - 1;
            }
            else
            {
                cbDutPortMode.SelectedIndex = iCurDutPortMode - 1;
            }

            //通过WMI获取Dut ComPort端口
            straryDutComPort = MulGetDutComPortInfo(iCurDutPortMode, HardwareEnum.Win32_PnPEntity, "Name");
            foreach (string strComPort in straryDutComPort)
            {
                cbDutComPort.Items.Add(strComPort);
            }

            for (iDutPortIndex = 0; iDutPortIndex < cbDutComPort.Items.Count; iDutPortIndex++)
            {
                string strComPort = cbDutComPort.GetItemText(cbDutComPort.Items[iDutPortIndex]);
                if (strComPort.ToUpper().Equals(strCurDutComPort) && (strCurDutComPort.Length > 3))
                {
                    break;
                }
            }

            if (iDutPortIndex == cbDutComPort.Items.Count)
            {
                iDutPortIndex = 0;
            }

            if (cbDutComPort.Items.Count > 0)
            {
                cbDutComPort.SelectedIndex = iDutPortIndex;
            }


            cbFixtureType.SelectedIndex = iFixtureType;

        

            if (iEnableFixtureControl == 1)
            {
                cbEnableFixtureControl.Checked = true;
            }
            else
            {
                cbEnableFixtureControl.Checked = false;
            }

            if (iEnablePowerSupplyControl == 1)
            {
                cbEnablePowerSupplyControl.Checked = true;
            }
            else
            {
                cbEnablePowerSupplyControl.Checked = false;
            }

            if (iEnablePingPang == 1)
            {
                cbEnablePingPang.Checked = true;
            }
            else
            {
                cbEnablePingPang.Checked = false;
            }

            //cbPowerSupplyType.Text = strCurExeFilePath;
            //cbPowerSupplyChan.Text = string.Format("{0}", );
            //tbPowerSupplyAddr.Text = strCurEattFilePath;
            //cbMeasInstrType.Text = strCurTesterType;

            //更新仪表选择
            foreach (string strPowerSupplyType in straryAllPowerSupplyType)
            {
                if (strPowerSupplyType != null)
                {
                    cbPowerSupplyType.Items.Add(strPowerSupplyType);
                }
            }

            foreach (string strPowerSupplyType in straryAllPowerSupplyType)
            {
                if (strPowerSupplyType.Contains(strCurPowerSupplyType))
                {
                    break;
                }
                else
                {
                    iPowerSupplyTypeIndex++;
                }
            }

            if (iPowerSupplyTypeIndex == straryAllPowerSupplyType.Length)
            {
                iPowerSupplyTypeIndex = 0;
            }

            if (cbPowerSupplyType.Items.Count > 0)
            {
                cbPowerSupplyType.SelectedIndex = iPowerSupplyTypeIndex;
            }


            //测试结果层次选择
            foreach (string strResLevelType in straryAllSupportResLevelType)
            {
                if (strResLevelType != null)
                {
                    cbResLevelOpt.Items.Add(strResLevelType);
                }
            }

            foreach (string strResLevelType in straryAllSupportResLevelType)
            {
                if (strResLevelType.Contains(strCurResLevel))
                {
                    break;
                }
                else
                {
                    iResLevelTypeIndex++;
                }
            }

            if (iResLevelTypeIndex == straryAllSupportResLevelType.Length)
            {
                iResLevelTypeIndex = 0;
            }

            if (cbResLevelOpt.Items.Count > 0)
            {
                cbResLevelOpt.SelectedIndex = iResLevelTypeIndex;
            }

            tbPowerSupplyAddr.Text = strCurPowerSupplyAddr;
            cbPowerSupplyChan.Text = string.Format("{0}", iCurPowerChanIndex);
            if (iCurPowerChanIndex == 0)
            {
                tbPowerSupplyVoltage.Text = string.Format("{0:F2}", dCurCh0PowerVoltage);
                tbPowerSupplyCurrent.Text = string.Format("{0:F2}", dCurCh0PowerCurrent);
            }
            else if (iCurPowerChanIndex == 1)
            {
                tbPowerSupplyVoltage.Text = string.Format("{0:F2}", dCurCh1PowerVoltage);
                tbPowerSupplyCurrent.Text = string.Format("{0:F2}", dCurCh1PowerCurrent);
            }

            tbAttnSourceCfgFile1.Text = strAttnSourceCfgFile1;
            tbAttnSourceCfgFile2.Text = strAttnSourceCfgFile2;
            tbAttnTargetCfgFile1.Text = strAttnTargetCfgFile1;
            tbAttnTargetCfgFile2.Text = strAttnTargetCfgFile2;

            if (iEnableAttnSourceCfgFile1 == 0)
                cbEnableAttnSourceCfgFile1.Checked = false;
            else
                cbEnableAttnSourceCfgFile1.Checked = true;

            if (iEnableAttnTargetCfgFile1 == 0)
                cbEnableAttnTargetCfgFile1.Checked = false;
            else
                cbEnableAttnTargetCfgFile1.Checked = true;

            if (iEnableAttnSourceCfgFile2 == 0)
                cbEnableAttnSourceCfgFile2.Checked = false;
            else
                cbEnableAttnSourceCfgFile2.Checked = true;            

            if (iEnableAttnTargetCfgFile2 == 0)
                cbEnableAttnTargetCfgFile2.Checked = false;
            else
                cbEnableAttnTargetCfgFile2.Checked = true;

            tbDutPreloaderCom.Text = string.Format("{0}", iDutPreloaderComPort);
            tbDutGadgetVCom.Text = string.Format("{0}", iDutGadgetComPort);

            cbEnableAttnSourceCfgFile1_CheckStatedChanged(sender, e);
            cbEnableAttnSourceCfgFile2_CheckStatedChanged(sender, e);
            cbEnableAttnTargetCfgFile1_CheckStatedChanged(sender, e);
            cbEnableAttnTargetCfgFile2_CheckStatedChanged(sender, e);
            cbDutPortMode_SelectedIndexChanged(sender, e);

            cbFixtureCloseMode.SelectedIndex = iFixtureCloseModeOpt - 1;
            cbRepeatMode.SelectedIndex = iRepeatModeOpt - 1;
            cbStopMode.SelectedIndex = iStopModeOpt - 1;
            cbEnableFixtureControl_CheckedStateChanged(sender, e);
            cbEnablePowerSupplyControl_CheckedStateChanged(sender, e);            
            cbEnableFixtureControl_CheckedChanged(sender, e);
            cbEnablePingPang_CheckStateChanged(sender, e);
            cbRunMode_SelectedIndexChanged(sender, e);

            Application.DoEvents();

            this.Refresh();
        }
            catch(Exception exep)
            {
                MessageBox.Show(exep.Message, "frmRunDoc1Cfg_Load Failed");
            
            }
        }

        private void btResSelect_Click(object sender, EventArgs e)
        {

        }

        private void cbEnableFixtureControl_CheckedStateChanged(object sender, EventArgs e)
        {
            int iFixturePortIndex = 0;
            string[] straryFixtureComPort;

            if (cbEnableFixtureControl.Checked)
            {
                iEnableFixtureControl = 1;
                cbFixtureComPort.Enabled = true;
                cbFixtureComPort.Items.Clear();
                straryFixtureComPort = MulGetFixtureComPortInfo(iCurDutPortMode, HardwareEnum.Win32_PnPEntity, "Name");
                foreach (string strComPort in straryFixtureComPort)
                {
                    cbFixtureComPort.Items.Add(strComPort);
                }

                foreach (string strComPort in straryFixtureComPort)
                {
                    if (strComPort.ToUpper().Equals(strCurFixtureComPort.ToUpper()))
                    {
                        break;
                    }
                    else
                    {
                        iFixturePortIndex++;
                    }
                }
                if (iFixturePortIndex == straryFixtureComPort.Length)
                {
                    iFixturePortIndex = 0;
                }

                if (cbFixtureComPort.Items.Count > 0)
                {
                    cbFixtureComPort.SelectedIndex = iFixturePortIndex;
                }
            }
            else
            {
                iEnableFixtureControl = 0;
                cbFixtureComPort.Enabled = false;
            }
        }

        private void cbEnablePowerSupplyControl_CheckedStateChanged(object sender, EventArgs e)
        {
            if (cbEnablePowerSupplyControl.Checked)
            {
                cbPowerSupplyType.Enabled = true;
                cbPowerSupplyChan.Enabled = true;
                tbPowerSupplyAddr.Enabled = true;
                tbPowerSupplyVoltage.Enabled = true;
                tbPowerSupplyCurrent.Enabled = true;
                iEnablePowerSupplyControl = 1;
            }
            else
            {
                cbPowerSupplyType.Enabled = false;
                cbPowerSupplyChan.Enabled = false;
                tbPowerSupplyAddr.Enabled = false;
                tbPowerSupplyVoltage.Enabled = false;
                tbPowerSupplyCurrent.Enabled = false;
                iEnablePowerSupplyControl = 0;
            }
        }

        private void cbMeasInstrType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cbMeasInstrType.Text.Contains("MT8870"))||(cbMeasInstrType.Text.Contains("CMW500")))
            {
                if (iLanguageOpt == 1) //English
                {
                    tbTesterAddrPrompt.Text = "Instrument Address Setting:\r\n1st: GPIB0::10::INSTR\r\n2nd: GPIB0::12::INSTR\r\n3rd: GPIB1::14::INSTR\r\n4th: GPIB1::16::INSTR";
                }
                else
                {
                    tbTesterAddrPrompt.Text = "仪表地址设置方法:\r\n第一通道: GPIB0::10::INSTR\r\n第二通道: GPIB0::12::INSTR\r\n第三通道: GPIB1::14::INSTR\r\n第四通道: GPIB1::16::INSTR";
                }
                if (cbMeasInstrType.Text.Contains("CMW500"))
                {
                    cbEnablePingPang.Enabled = true;
                    tbPingPangTimeOutS.Enabled = true;
                }
                else
                {
                    cbEnablePingPang.Enabled = false;
                    tbPingPangTimeOutS.Enabled = false;
                }
            }
            else if (cbMeasInstrType.Text.Contains("IQXSTREAM"))
            {
                if (iLanguageOpt == 1) //English
                {
                    tbTesterAddrPrompt.Text = "Instrument Address Setting:\r\n1st: 192.168.100.254\r\n2nd: 192.168.100.254";
                }
                else
                {
                    tbTesterAddrPrompt.Text = "仪表地址设置方法:\r\n第一通道: 192.168.100.254\r\n第二通道: 192.168.100.254";
                }
            }
        }

        private void cbEnableFixtureControl_CheckedChanged(object sender, EventArgs e)
        {
            if (cbEnableFixtureControl.Checked == false)
            {
                cbFixtureType.Enabled = false;
                cbFixtureCloseMode.Enabled = false;
                cbFixtureComPort.Enabled = false;
                iEnableFixtureControl = 0;
            }
            else if (cbEnableFixtureControl.Checked == true)
            {
                cbFixtureType.Enabled = true;
                cbFixtureCloseMode.Enabled = true;
                cbFixtureComPort.Enabled = true;
                iEnableFixtureControl = 1;
            }
        }

        private void cbRunMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRepeatMode.SelectedIndex == 0)
            {
                cbStopMode.Enabled = false;
                tbRepeatNum.Enabled = false;
                tbDelayTime.Enabled = false;
            }
            else if (cbRepeatMode.SelectedIndex == 1)
            {
                cbStopMode.Enabled = true;
                tbRepeatNum.Enabled = true;
                tbDelayTime.Enabled = true;
            }
        }

        private void cbPowerSupplyChan_SelectedIndexChanged(object sender, EventArgs e)
        {
            iCurPowerChanIndex = Int32.Parse(cbPowerSupplyChan.Text);
            if (iCurPowerChanIndex == 0)
            {
                tbPowerSupplyVoltage.Text = string.Format("{0:F2}", dCurCh0PowerVoltage);
                tbPowerSupplyCurrent.Text = string.Format("{0:F2}", dCurCh0PowerCurrent);
            }
            else if (iCurPowerChanIndex == 1)
            {
                tbPowerSupplyVoltage.Text = string.Format("{0:F2}", dCurCh1PowerVoltage);
                tbPowerSupplyCurrent.Text = string.Format("{0:F2}", dCurCh1PowerCurrent);
            }
        }

        private int frmRunDoc1Cfg_SelectCfgFile(ref string strCfgFile)
        {
            int iStatus = 0;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择配置文件路径";
            openFileDialog.Filter = "配置文件|*.cfg|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return -1;
            }
            else
            {
                strCfgFile = openFileDialog.FileName;
                strCfgFile = strCfgFile.Substring(strCfgFile.LastIndexOf("\\")+1, strCfgFile.Length - strCfgFile.LastIndexOf("\\")-1);
            }
            return iStatus;
        }

        private void btSelectAttnSourceCfgFile1_Click(object sender, EventArgs e)
        {
            string strCfgFile = "";
            int iStatus = 0;
            iStatus = frmRunDoc1Cfg_SelectCfgFile(ref strCfgFile);
            if (iStatus == 0)
            {
                strAttnSourceCfgFile1 = string.Format("\\mtk\\cfg\\mode_cl_cal\\{0}", strCfgFile);
                tbAttnSourceCfgFile1.Text = strAttnSourceCfgFile1;
            }
            return;
        }

        private void btSelectAttnTargetCfgFile1_Click(object sender, EventArgs e)
        {
            string strCfgFile = "";
            int iStatus = 0;
            iStatus = frmRunDoc1Cfg_SelectCfgFile(ref strCfgFile);
            if (iStatus == 0)
            {
                strAttnTargetCfgFile1 = string.Format("\\mtk\\cfg\\mode_dut_cal_ver\\{0}", strCfgFile);
                tbAttnTargetCfgFile1.Text = strAttnTargetCfgFile1;
            }
            return;
        }

        private void btSelectAttnSourceCfgFile2_Click(object sender, EventArgs e)
        {
            string strCfgFile = "";
            int iStatus = 0;
            iStatus = frmRunDoc1Cfg_SelectCfgFile(ref strCfgFile);
            if (iStatus == 0)
            {
                strAttnSourceCfgFile2 = string.Format("\\mtk\\cfg\\mode_cl_cal\\{0}", strCfgFile);
                tbAttnSourceCfgFile2.Text = strAttnSourceCfgFile2;
            }
            return;
        }

        private void btSelectAttnTargetCfgFile2_Click(object sender, EventArgs e)
        {
            string strCfgFile = "";
            int iStatus = 0;
            iStatus = frmRunDoc1Cfg_SelectCfgFile(ref strCfgFile);
            if (iStatus == 0)
            {
                strAttnTargetCfgFile2 = string.Format("\\mtk\\cfg\\mode_dut_cal_ver\\{0}", strCfgFile);
                tbAttnTargetCfgFile2.Text = strAttnTargetCfgFile2;
            }
            return;
        }

        private void cbEnableAttnSourceCfgFile1_CheckStatedChanged(object sender, EventArgs e)
        {
            if (cbEnableAttnSourceCfgFile1.Checked == false)
            {
                tbAttnSourceCfgFile1.Enabled = false;
                btSelectAttnSourceCfgFile1.Enabled = false;
                iEnableAttnSourceCfgFile1 = 0;
            }
            else
            {
                tbAttnSourceCfgFile1.Enabled = true;
                btSelectAttnSourceCfgFile1.Enabled = true;
                iEnableAttnSourceCfgFile1 = 1;
            }

            
        }

        private void cbEnableAttnTargetCfgFile1_CheckStatedChanged(object sender, EventArgs e)
        {
            if (cbEnableAttnTargetCfgFile1.Checked == false)
            {
                tbAttnTargetCfgFile1.Enabled = false;
                btSelectAttnTargetCfgFile1.Enabled = false;
                iEnableAttnTargetCfgFile1 = 0;
            }
            else
            {
                tbAttnTargetCfgFile1.Enabled = true;
                btSelectAttnTargetCfgFile1.Enabled = true;
                iEnableAttnTargetCfgFile1 = 1;
            }
        }

        private void cbEnableAttnSourceCfgFile2_CheckStatedChanged(object sender, EventArgs e)
        {
            if (cbEnableAttnSourceCfgFile2.Checked == false)
            {
                tbAttnSourceCfgFile2.Enabled = false;
                btSelectAttnSourceCfgFile2.Enabled = false;
                iEnableAttnSourceCfgFile2 = 0;
            }
            else
            {
                tbAttnSourceCfgFile2.Enabled = true;
                btSelectAttnSourceCfgFile2.Enabled = true;
                iEnableAttnSourceCfgFile2 = 1;
            }            
        }

        private void cbEnableAttnTargetCfgFile2_CheckStatedChanged(object sender, EventArgs e)
        {
            if (cbEnableAttnTargetCfgFile2.Checked == false)
            {
                tbAttnTargetCfgFile2.Enabled = false;
                btSelectAttnTargetCfgFile2.Enabled = false;
                iEnableAttnTargetCfgFile2 = 0;
            }
            else
            {
                tbAttnTargetCfgFile2.Enabled = true;
                btSelectAttnTargetCfgFile2.Enabled = true;
                iEnableAttnTargetCfgFile2 = 1;
            }
        }

        private void cbDutPortMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDutPortMode.SelectedIndex == 0)
            {
                cbDutComPort.Enabled = false;
                tbDutPreloaderCom.Enabled = true;
                tbDutGadgetVCom.Enabled = true;  
            }
            else if (cbDutPortMode.SelectedIndex == 1)
            {
                cbDutComPort.Enabled = true;
                tbDutPreloaderCom.Enabled = false;
                tbDutGadgetVCom.Enabled = false;
            }
        }
            
        private void cbEnablePingPang_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbEnablePingPang.Checked == false)
            {
                iEnablePingPang = 0;
                tbPingPangTimeOutS.Enabled = false;
            }
            else
            {
                iEnablePingPang = 1;
                tbPingPangTimeOutS.Enabled = true;
            } 
        }

    }

}
