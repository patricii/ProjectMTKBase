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


namespace ateRun
{
    class clExeCtrl
    {
        public const int USER = 0x0400;
        public const int WM_RUN_CFG = USER + 101;
        public const int WM_RUN_SUM = USER + 102;
        public const int WM_RUN_LOG = USER + 103;
        public const int WM_RUN_CTRL = USER + 201;
        public const int WM_RUN_PROC = USER + 301;
        public const int WM_RUN_ERR = USER + 401;

        public string strCurPowerSupplyAddr;
        public int iCurPowerChanIndex;
        public double dCurCh0PowerVoltage;
        public double dCurCh0PowerCurrent;
        public double dCurCh1PowerVoltage;
        public double dCurCh1PowerCurrent;
        private clInstrCtrl sInstrCtrl = new clInstrCtrl();

        public DateTime dtCurrentRunBegin;
        public int iCurDutGroupIndex;
        public int iFixtureType;   //0:R_H|1:B_P|2:B_J|3:J_Z

        public string strCurPowerSupplyType;
        public int iLanguageOpt = 1;
        public string strCurDutComPort;
        public string strCurFixtureComPort;
        public string strCurRfSwitchComPort;
        public string strCurRfSwitchRfPort;
        public bool bFindComPort;
        public bool bCheckFixtureClosed = false;
        public bool bRunProcPrepare;

        public bool bfixtureContinueDetect;
      
        private int iComPortSearchMaxFindNum;
        private int iComPortSearchMaxFindIndex;
        private int iComPortSearchMaxRetryNum;
        private int iComPortSearchMaxRetryIndex;
        private int iDutSearchMaxRetryNum = 100;
        private int iDutSearchMaxRetryIndex = 0;
        private int iWaiteForStartMaxRetryNum;
        private int iWaiteForStartMaxRetryIndex;

        public int iCurDutPortMode;
        public string strDutSerialPortBaudRate;
        public string strDutSerialPortDataBits;
        public string strDutSerialPortReadTimeout;
        public string strDutSerialPortWriteTimeout;
        public string strDutSerialPortParity;
        public string strDutSerialPortEncoding;
        public string strFixtureSerialPortBaudRate;
        public string strFixtureSerialPortDataBits;
        public string strFixtureSerialPortReadTimeout;
        public string strFixtureSerialPortWriteTimeout;
        public string strFixtureSerialPortParity;
        public string strFixtureSerialPortEncoding;
        //public int iFindUsbPortRetryNum;
        //public int iFindUsbPortRetryIndex;
        public string strDutSerialPortPreBaudRate = "115200";
        public string strDutSerialPortCurBaudRate = "115200";
        public string g_strInputDutSerialNumber = "";
        public string g_strRetDutSerialNumber = "";
        public string strAtCmdLenovoSetTestBaudRate = "";
        public string strRetDutSwVersion = "";

        public string strAtCmdLenovoGsn = "";
        public string strAtCmdLenovoCheckVersion = "";
        public string strAtCmdLenovoEnterCalMode = "";
        public string strAtCmdLenovoExitCalMode = "";
        public string strAtCmdLenovoBackupModemData = "";
        public string strAtCmdLenovoModemReset = "";
        public string strAtCmdLenovoModemLock = "";
        public string strAtCmdLenovoModemUnLock = "";
        public string strAtCmdIntelSetNvmCfg = "";
        public string strAtCmdIntelFixUsb = "";
        public string strAtCmdIntelStoreNvmSync = "";
        public string strAtCmdIntelModemReset = "";
        public bool bFixtureOpen = false;
        public bool bFixtureChange = false;
        public bool bFixturePowerOn = false;
        public bool bFixturePowerOff = false;
        public bool bReadyForStart = false;
        public bool bFixtureClose = false;
        public bool bRfSwitch = false;
        public bool bPowerOn = false;
        public bool bPowerOff = false;
        public string strCurRunPassFail = "INIT";
        public int iRecodeCalFlag = 0;
        public int iPassCalFlag = 0;
        public int iDutResetAfterCal = 0;

        public string strRfSwitchSerialPortBaudRate;
        public string strRfSwitchSerialPortDataBits;
        public string strRfSwitchSerialPortReadTimeout;
        public string strRfSwitchSerialPortWriteTimeout;
        public string strRfSwitchSerialPortParity;
        public string strRfSwitchSerialPortEncoding;

        StringBuilder sbRetCmdMsg = new StringBuilder(40960);
        StringBuilder sbRetCmdBuf = new StringBuilder(40960);

        public IntPtr g_ipFrmDut;

        clMsgCtrl sMsgCtrl = new clMsgCtrl();


        [DllImport("portCtrlSerial.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int portCtrlSerial_ExeCmd(String strWriteCmd, [MarshalAs(UnmanagedType.LPStr)] StringBuilder strRetBuf, [MarshalAs(UnmanagedType.LPStr)] StringBuilder strRetMsg);

        [DllImport("runCtrlDll.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int runCtrl_ExeCmd(String strWriteCmd, [MarshalAs(UnmanagedType.LPStr)] StringBuilder strRetBuf, [MarshalAs(UnmanagedType.LPStr)] StringBuilder strRetMsg);


        public clExeCtrl()
        {
            strCurDutComPort = "COM10";
            iComPortSearchMaxRetryNum = 50;
            iComPortSearchMaxFindNum = 3;
            //iDutSearchMaxRetryNum = 100;
            iCurDutPortMode = 1;
            strDutSerialPortBaudRate = "115200";
            strDutSerialPortDataBits = "none";
            strDutSerialPortReadTimeout = "90000";
            strDutSerialPortParity = "none";
            strDutSerialPortEncoding = "ascii";

            strCurRfSwitchRfPort = "RF1C";
            strCurRfSwitchComPort = "COM123";

            strRfSwitchSerialPortBaudRate = "9600";
            strRfSwitchSerialPortDataBits = "8";
            strRfSwitchSerialPortReadTimeout = "90000";
            strRfSwitchSerialPortWriteTimeout = "90000";
            strRfSwitchSerialPortParity = "none";
            strRfSwitchSerialPortEncoding = "ascii";

            bFindComPort = false;
            bCheckFixtureClosed = false;

            iFixtureType = 0;
            strFixtureSerialPortBaudRate = "9600";
            strFixtureSerialPortDataBits = "8";
            strFixtureSerialPortReadTimeout = "90000";
            strFixtureSerialPortWriteTimeout = "90000";
            strFixtureSerialPortParity = "none";
            strFixtureSerialPortEncoding = "ascii";
          
            /*          iFixtureType = 3;

                        if (iFixtureType == 0)
                        {
                            strFixtureSerialPortBaudRate = "9600";
                            strFixtureSerialPortDataBits = "8";
                            strFixtureSerialPortReadTimeout = "5000";
                            strFixtureSerialPortWriteTimeout = "5000";
                            strFixtureSerialPortParity = "none";
                            strFixtureSerialPortEncoding = "ascii";
                        }
                        else if (iFixtureType == 1)
                        {
                            strFixtureSerialPortBaudRate = "9600";
                            strFixtureSerialPortDataBits = "8";
                            strFixtureSerialPortReadTimeout = "5000";
                            strFixtureSerialPortWriteTimeout = "5000";
                            strFixtureSerialPortParity = "none";
                            strFixtureSerialPortEncoding = "ascii";
                        }
                        else if (iFixtureType == 2)
                        {
                            strFixtureSerialPortBaudRate = "115200";
                            strFixtureSerialPortDataBits = "8";
                            strFixtureSerialPortReadTimeout = "5000";
                            strFixtureSerialPortWriteTimeout = "5000";
                            strFixtureSerialPortParity = "none";
                            strFixtureSerialPortEncoding = "ascii";
                        }
                        else if (iFixtureType == 3)
                        {
                            strFixtureSerialPortBaudRate = "9600";
                            strFixtureSerialPortDataBits = "8";
                            strFixtureSerialPortReadTimeout = "5000";
                            strFixtureSerialPortWriteTimeout = "5000";
                            strFixtureSerialPortParity = "none";
                            strFixtureSerialPortEncoding = "ascii";
                        }
            */
        }

        private int clExeCtrl_SerialPortCheckExist(int iCurDutPortMode,
            string strCurDutComPort,
            string strDutSerialPortBaudRate,
            string strDutSerialPortDataBits,
            string strDutSerialPortReadTimeout,
            string strDutSerialPortParity,
            string strDutSerialPortEncoding)
        {
            int iStatus = 0;
            SerialPort comport = new SerialPort();
            try
            {
                comport.PortName = strCurDutComPort;
                comport.BaudRate = int.Parse(strDutSerialPortBaudRate);
                comport.Open();

                if (comport.IsOpen)
                {
                    iStatus = 1;
                    comport.Close();
                }
                else
                {
                    iStatus = 0;
                    comport.Close();
                }
            }
            catch (Exception exp)
            {
                comport.Close();
            }
            return iStatus;

        }

        public void clExeCtrl_CheckComPortExist()
        {
            string strMsgVal = "";
            bFindComPort = false;
            iComPortSearchMaxRetryNum = 30;
            iComPortSearchMaxRetryIndex = 0;
            iComPortSearchMaxFindNum = 3;
            iComPortSearchMaxFindIndex = 0;

            try
            {
                if (iCurDutPortMode == 1)
                {
                    bFindComPort = true;
                    if (iLanguageOpt == 1) //English
                    {
                        strMsgVal = string.Format("...Find USB Port, start test...", strCurDutComPort);
                    }
                    else
                    {
                        strMsgVal = string.Format("...找到手机USB,开始测试...", strCurDutComPort);
                    }
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_status", strMsgVal);
                    return;
                }

                do
                {
                    switch (iComPortSearchMaxRetryIndex % 7)
                    {
                        case 0:
                            if (strCurDutComPort.Contains("COM"))
                            {
                                if (iLanguageOpt == 1) //English
                                {
                                    strMsgVal = strMsgVal = string.Format("Searching serial port\"{0}\"......", strCurDutComPort);
                                }
                                else
                                {
                                    strMsgVal = strMsgVal = string.Format("寻找串口\"{0}\"中......", strCurDutComPort);
                                }
                            }
                            break;
                        case 1:
                            if (strCurDutComPort.Contains("COM"))
                            {
                                if (iLanguageOpt == 1) //English
                                {
                                    strMsgVal = string.Format(".Searching serial port\"{0}\".....", strCurDutComPort);
                                }
                                else
                                {
                                    strMsgVal = strMsgVal = string.Format(".寻找串口\"{0}\"中.....", strCurDutComPort);
                                }
                            }
                            break;
                        case 2:
                            if (strCurDutComPort.Contains("COM"))
                            {
                                if (iLanguageOpt == 1) //English
                                {
                                    strMsgVal = string.Format("..Searching serial port\"{0}\"....", strCurDutComPort);
                                }
                                else
                                {
                                    strMsgVal = strMsgVal = string.Format("..寻找串口\"{0}\"中....", strCurDutComPort);
                                }
                            }
                            break;
                        case 3:
                            if (strCurDutComPort.Contains("COM"))
                            {
                                if (iLanguageOpt == 1) //English
                                {
                                    strMsgVal = string.Format("...Searching serial port\"{0}\"...", strCurDutComPort);
                                }
                                else
                                {
                                    strMsgVal = strMsgVal = string.Format("...寻找串口\"{0}\"中...", strCurDutComPort);
                                }
                            }
                            break;
                        case 4:
                            if (strCurDutComPort.Contains("COM"))
                            {
                                if (iLanguageOpt == 1) //English
                                {
                                    strMsgVal = string.Format("....Searching serial port\"{0}\"..", strCurDutComPort);
                                }
                                else
                                {
                                    strMsgVal = strMsgVal = string.Format("....寻找串口\"{0}\"中..", strCurDutComPort);
                                }
                            }
                            break;
                        case 5:
                            if (strCurDutComPort.Contains("COM"))
                            {
                                if (iLanguageOpt == 1) //English
                                {
                                    strMsgVal = string.Format(".....Searching serial port\"{0}\".", strCurDutComPort);
                                }
                                else
                                {
                                    strMsgVal = strMsgVal = string.Format(".....寻找串口\"{0}\"中.", strCurDutComPort);
                                }
                            }
                            break;
                        case 6:
                            if (strCurDutComPort.Contains("COM"))
                            {
                                if (iLanguageOpt == 1) //English
                                {
                                    strMsgVal = string.Format("......Searching serial port\"{0}\"", strCurDutComPort);
                                }
                                else
                                {
                                    strMsgVal = strMsgVal = string.Format("......寻找串口\"{0}\"中", strCurDutComPort);
                                }
                            }
                            break;
                    }

                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_status", strMsgVal);

                    if (strCurDutComPort.Contains("COM"))
                    {
                        iComPortSearchMaxFindIndex += clExeCtrl_SerialPortCheckExist(iCurDutPortMode,
                                                                        strCurDutComPort,
                                                                        strDutSerialPortBaudRate,
                                                                        strDutSerialPortDataBits,
                                                                        strDutSerialPortReadTimeout,
                                                                        strDutSerialPortParity,
                                                                        strDutSerialPortEncoding);


                        if (iComPortSearchMaxFindIndex > iComPortSearchMaxFindNum)
                        {
                            bFindComPort = true;
                            if (iLanguageOpt == 1) //English
                            {
                                strMsgVal = string.Format("...Find serial port\"{0}\", start test...", strCurDutComPort);
                            }
                            else
                            {
                                strMsgVal = string.Format("...找到串口\"{0}\",开始测试...", strCurDutComPort);
                            }
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_status", strMsgVal);
                            return;
                        }
                    }
                    else
                    {

                    }

                    iComPortSearchMaxRetryIndex++;
                    //Thread.Sleep(200);

                } while (iComPortSearchMaxRetryNum > iComPortSearchMaxRetryIndex);

                if (iComPortSearchMaxRetryNum <= iComPortSearchMaxRetryIndex)
                {
                    bFindComPort = false;
                    return;
                }
                else
                {
                    bFindComPort = true;
                    return;
                }
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message,"clExeCtrl_CheckComPortExist fail");
                return;            
            }
        }

        private int clExeCtrl_SerialPortInit(int iCurDutPortMode,
            string strCurDutComPort,
            string strDutSerialPortBaudRate,
            string strDutSerialPortDataBits,
            string strDutSerialPortReadTimeout,
            string strDutSerialPortWriteTimeout,
            string strDutSerialPortParity,
            string strDutSerialPortEncoding)
        {
            int iStatus = 0;
            string strWriteCmd = "";
            //string strRetBuf = "";
            strWriteCmd = string.Format("set_parameter:global:{0}={1}", "dut_port_mode", iCurDutPortMode);
            portCtrlSerial_ExeCmd(strWriteCmd, sbRetCmdBuf, sbRetCmdMsg);

            strWriteCmd = string.Format("set_parameter:global:{0}={1}", "serial_port_name", strCurDutComPort);
            portCtrlSerial_ExeCmd(strWriteCmd, sbRetCmdBuf, sbRetCmdMsg);

            strWriteCmd = string.Format("set_parameter:global:{0}={1}", "serial_port_baudrate", strDutSerialPortBaudRate);
            portCtrlSerial_ExeCmd(strWriteCmd, sbRetCmdBuf, sbRetCmdMsg);

            strWriteCmd = string.Format("set_parameter:global:{0}={1}", "serial_port_datebits", strDutSerialPortDataBits);
            portCtrlSerial_ExeCmd(strWriteCmd, sbRetCmdBuf, sbRetCmdMsg);

            strWriteCmd = string.Format("set_parameter:global:{0}={1}", "serial_port_read_timeout", strDutSerialPortReadTimeout);
            portCtrlSerial_ExeCmd(strWriteCmd, sbRetCmdBuf, sbRetCmdMsg);

            strWriteCmd = string.Format("set_parameter:global:{0}={1}", "serial_port_write_timeout", strDutSerialPortWriteTimeout);
            portCtrlSerial_ExeCmd(strWriteCmd, sbRetCmdBuf, sbRetCmdMsg);

            strWriteCmd = string.Format("set_parameter:global:{0}={1}", "serial_port_parity", strDutSerialPortParity);
            portCtrlSerial_ExeCmd(strWriteCmd, sbRetCmdBuf, sbRetCmdMsg);

            strWriteCmd = string.Format("set_parameter:global:{0}={1}", "serial_port_encoding", strDutSerialPortEncoding);
            portCtrlSerial_ExeCmd(strWriteCmd, sbRetCmdBuf, sbRetCmdMsg);
            return iStatus;
        }

        private int clExeCtrl_SerialPortConnect(string strCurDutComPort)
        {
            int iStatus = 0;
            string strItemMsg = "";
            //sbRetCmdBuf.Clear();
            //sbRetCmdMsg.Clear();
            iStatus = portCtrlSerial_ExeCmd("exe_cmd:port:connect", sbRetCmdBuf, sbRetCmdMsg);
            if (iStatus != 0)
            {
                strItemMsg = string.Format("连接串口\"{0}\"失败!", strCurDutComPort);

            }
            return iStatus;
        }

        private int clExeCtrl_SerialPortDisconnect()
        {
            int iStatus = 0;
            string strItemMsg = "";
            //sbRetCmdBuf.Clear();
            //sbRetCmdMsg.Clear();
            iStatus = portCtrlSerial_ExeCmd("exe_cmd:port:disconnect", sbRetCmdBuf, sbRetCmdMsg);
            if (iStatus != 0)
            {
                strItemMsg = string.Format("断开串口连接\"{0}\"失败!", strCurDutComPort);
                //clExeCtrl_AddErrMsg(100, strItemMsg);
            }
            return iStatus;
        }

        public void clExeCtrl_CheckFixtureClose()
        {
            int iStatus = 0;
            //string strRetAllMsg = "";
            //string strRetAddMsg = "";
            string strMsgVal = "";

            string strWriteCmd = "";
            string strRetBuf = "";
            
            //int iTryConnectSerialPort = 0;
            string strCmdVal1 = "";
            string strCmdVal2 = "";

            bCheckFixtureClosed = false;
            iDutSearchMaxRetryIndex = 0;

            bool bVBATT_ON = false;

            try
            {
                if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 1))
                {
                    strCmdVal1 = string.Format("enter_share_resource:resource_name={0}:time_out={1}", "fixture0", 10000);
                    strCmdVal2 = string.Format("exit_share_resource:resource_name={0}", "fixture0");
                }
                else if ((iCurDutGroupIndex == 2) || (iCurDutGroupIndex == 3))
                {
                    strCmdVal1 = string.Format("enter_share_resource:resource_name={0}:time_out={1}", "fixture1", 10000);
                    strCmdVal2 = string.Format("exit_share_resource:resource_name={0}", "fixture1");
                }

                //sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_status", "...等待夹具关闭...");

                do
                {
                    switch (iDutSearchMaxRetryIndex % 7)
                    {
                        case 0:
                            strMsgVal = string.Format("等待夹具关闭......");
                            break;
                        case 1:
                            strMsgVal = string.Format(".等待夹具关闭.....", strCurDutComPort);
                            break;
                        case 2:
                            strMsgVal = string.Format("..等待夹具关闭....", strCurDutComPort);
                            break;
                        case 3:
                            strMsgVal = string.Format("...等待夹具关闭...", strCurDutComPort);
                            break;
                        case 4:
                            strMsgVal = string.Format("....等待夹具关闭..", strCurDutComPort);
                            break;
                        case 5:
                            strMsgVal = string.Format(".....等待夹具关闭.", strCurDutComPort);
                            break;
                        case 6:
                            strMsgVal = string.Format("......等待夹具关闭", strCurDutComPort);
                            break;
                    }

                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_status", strMsgVal);


                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal1);
                    runCtrl_ExeCmd(strCmdVal1, sbRetCmdBuf, sbRetCmdMsg);
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                    iStatus = clExeCtrl_SerialPortInit(1,
                                                   strCurFixtureComPort,
                                                   strFixtureSerialPortBaudRate,
                                                   strFixtureSerialPortDataBits,
                                                   strFixtureSerialPortReadTimeout,
                                                   strFixtureSerialPortWriteTimeout,
                                                   strFixtureSerialPortParity,
                                                   strFixtureSerialPortEncoding);

                    iStatus = clExeCtrl_SerialPortConnect(strCurFixtureComPort);
                    if (iStatus != 0)
                    {
                        sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                        runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                        sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                        bCheckFixtureClosed = false;
                        Thread.Sleep(100);
                        continue;
                    }

                    if (iFixtureType == 0)
                    {
                        if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 2))
                        {
                            if (bVBATT_ON == false)
                            {
                                //strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "VBATT_ON");
                                strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "VBATT_ON_1");
                                iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);

                                bVBATT_ON = true;
                            }

                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS_1?");
                        }
                        else if ((iCurDutGroupIndex == 1) || (iCurDutGroupIndex == 3))
                        {
                            if (bVBATT_ON == false)
                            {
                                //strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "VBATT_OFF");
                                strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "VBATT_ON_2");
                                iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);

                                bVBATT_ON = true;
                            }

                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS_2?");
                        }

                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    }
                    else if (iFixtureType == 1)
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS");
                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    }
                    else if (iFixtureType == 2)
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS?");
                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    }
                    else if (iFixtureType == 3)
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS");
                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    }

                    strRetBuf = strRetBuf.ToString().ToUpper();
                    iStatus = clExeCtrl_SerialPortDisconnect();

                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                    runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                    if (strRetBuf.Contains("CLOSE"))
                    {
                        //sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_status", "...夹具已关闭...");
                        break;
                    }
                    iDutSearchMaxRetryIndex++;

                    Application.DoEvents();

                    Thread.Sleep(950);

                } while (/*true*/bfixtureContinueDetect == true);
                //} while (iDutSearchMaxRetryNum > iDutSearchMaxRetryIndex);
                bCheckFixtureClosed = true;

                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_status", "...夹具已关闭...");

                return;
            }
            catch(Exception exp)
            {  
                MessageBox.Show(exp.Message,"clExeCtrl_CheckFixtureClose fail");
                return;
            }
        }

        public void clExeCtrl_QueryFixtureStatus()
        {           
            int iStatus = 0;
          
            string strMsgVal = "";

            string strWriteCmd = "";
            string strRetBuf = "";

            //int iTryConnectSerialPort = 0;
            string strCmdVal1 = "";
            string strCmdVal2 = "";

            bCheckFixtureClosed = false;        
         
            try
            {               
                if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 1))
                {
                    strCmdVal1 = string.Format("enter_share_resource:resource_name={0}:time_out={1}", "fixture0", 10000);
                    strCmdVal2 = string.Format("exit_share_resource:resource_name={0}", "fixture0");
                }
                else if ((iCurDutGroupIndex == 2) || (iCurDutGroupIndex == 3))
                {
                    strCmdVal1 = string.Format("enter_share_resource:resource_name={0}:time_out={1}", "fixture1", 10000);
                    strCmdVal2 = string.Format("exit_share_resource:resource_name={0}", "fixture1");
                }   
                    
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal1);
                runCtrl_ExeCmd(strCmdVal1, sbRetCmdBuf, sbRetCmdMsg);
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                    
                iStatus = clExeCtrl_SerialPortInit(1,
                                                   strCurFixtureComPort,
                                                   strFixtureSerialPortBaudRate,
                                                   strFixtureSerialPortDataBits,
                                                   strFixtureSerialPortReadTimeout,
                                                   strFixtureSerialPortWriteTimeout,
                                                   strFixtureSerialPortParity,
                                                   strFixtureSerialPortEncoding);                    
                
                iStatus = clExeCtrl_SerialPortConnect(strCurFixtureComPort);
                if (iStatus != 0)
                {
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                    runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                    bCheckFixtureClosed = false;

                    iStatus = clExeCtrl_SerialPortDisconnect();

                    return;                                        
                }
                
                if (iFixtureType == 0)
                {
                    if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 2))
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS_1?");
                    }
                    else if ((iCurDutGroupIndex == 1) || (iCurDutGroupIndex == 3))
                    {     
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS_2?");
                    }

                    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                }
                //else if (iFixtureType == 1)
                //{
                //    strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS");
                //    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                //}
                //else if (iFixtureType == 2)
                //{
                //    strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS?");
                //    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                //}
                //else if (iFixtureType == 3)
                //{
                //    strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS");
                //    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                //}
                    
                strRetBuf = strRetBuf.ToString().ToUpper(); 
                   
                iStatus = clExeCtrl_SerialPortDisconnect();
                    
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);                    
                runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);                    
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());
                    
                if (strRetBuf.Contains("CLOSE"))                    
                {                        
                    bCheckFixtureClosed = true;  
                }
                 
                return;           
            }           
            catch (Exception exp)
            {                
                MessageBox.Show(exp.Message, "clExeCtrl_CheckFixtureClose fail");
                return;
            }
        }

        private int clExeCtrl_SerialPortSendCmd(string strWriteCmd, ref string strRetBuf)
        {
            int iStatus = 0;
            string strLogMsg = "";
            string strItemMsg = "";
            int iRetryIndex = 0;
            int iRetryNum = 5;

            if (strWriteCmd.Contains(strAtCmdLenovoExitCalMode) && (strAtCmdLenovoExitCalMode.Length > 1))
            {
                iRetryNum = 1;
            }
            else
            {
                iRetryNum = 3;
            }

            for (iRetryIndex = 0; iRetryIndex < iRetryNum; iRetryIndex++)
            {
                iStatus = 0;
                //sbRetCmdBuf.Clear();
                //sbRetCmdMsg.Clear();
                portCtrlSerial_ExeCmd(strWriteCmd, sbRetCmdBuf, sbRetCmdMsg);
                strRetBuf = sbRetCmdBuf.ToString();

                strLogMsg = string.Format("{0}:->{1}", "serial_port", strWriteCmd);
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strLogMsg);
                strLogMsg = string.Format("{0}:<-{1}", "serial_port", strRetBuf);
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strLogMsg);

                string[] straryRetCmdBuf = new string[4];
                straryRetCmdBuf = strRetBuf.Split(new char[3] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);

                if (strWriteCmd.Contains(strAtCmdLenovoCheckVersion) && (strAtCmdLenovoCheckVersion.Length > 1)
                    || strWriteCmd.Contains(strAtCmdLenovoGsn) && (strAtCmdLenovoGsn.Length > 1)
                    || strWriteCmd.Contains(strAtCmdLenovoEnterCalMode) && (strAtCmdLenovoEnterCalMode.Length > 1)
                    || strWriteCmd.Contains(strAtCmdLenovoExitCalMode) && (strAtCmdLenovoExitCalMode.Length > 1)
                    || strWriteCmd.Contains(strAtCmdLenovoBackupModemData) && (strAtCmdLenovoBackupModemData.Length > 1)
                    || strWriteCmd.Contains(strAtCmdLenovoModemReset) && (strAtCmdLenovoModemReset.Length > 1)
                    || strWriteCmd.Contains(strAtCmdLenovoModemLock) && (strAtCmdLenovoModemLock.Length > 1)
                    || strWriteCmd.Contains(strAtCmdLenovoModemUnLock) && (strAtCmdLenovoModemUnLock.Length > 1)
                    || strWriteCmd.Contains(strAtCmdLenovoSetTestBaudRate) && (strAtCmdLenovoSetTestBaudRate.Length > 1))
                {
                    if (strWriteCmd.Contains(strAtCmdLenovoSetTestBaudRate) && (strAtCmdLenovoSetTestBaudRate.Length > 1))
                    {
                        strItemMsg = string.Format("设置串口速率为:{0}", strAtCmdLenovoSetTestBaudRate);
                        sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strItemMsg);
                        break;
                    }

                    if (strWriteCmd.Contains(strAtCmdLenovoCheckVersion))
                    {
                        if (strRetBuf.Contains("(") && strRetBuf.Contains(")") && strRetBuf.Contains("1") && (straryRetCmdBuf.Length > 2))
                        {
                            strRetDutSwVersion = straryRetCmdBuf[2];
                            strItemMsg = string.Format("获得当前软件版本信息:{0}", strRetDutSwVersion);
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", strItemMsg);
                        }
                        else
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "未获取到有效的软件版本信息，请检查指令是否正确!");
                            iStatus = 102;
                        }
                    }
                    else if (strWriteCmd.Contains(strAtCmdLenovoGsn))
                    {
                        if (strRetBuf.Contains("(") && strRetBuf.Contains(")") && strRetBuf.Contains("1") && (straryRetCmdBuf.Length > 2))
                        {
                            g_strRetDutSerialNumber = straryRetCmdBuf[2];
                            strItemMsg = string.Format("获得当前待测件SN信息:{0}", g_strRetDutSerialNumber);
                            g_strInputDutSerialNumber = g_strRetDutSerialNumber;
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", strItemMsg);
                            break;
                        }
                        else
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "未获取到有效的SN，请检查SN是否有效写入或者该指令是否有效!");
                            iStatus = 103;
                        }
                    }
                    else if (strWriteCmd.Contains(strAtCmdLenovoEnterCalMode))
                    {
                        if (strRetBuf.Contains("(") && strRetBuf.Contains(")") && strRetBuf.Contains("1") && (straryRetCmdBuf.Length > 2))
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "已经进入校准模式.");
                            break;
                        }
                        else
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "进入校准模式失败，请检查该指令是否有效!");
                            iStatus = 104;
                        }
                    }
                    else if (strWriteCmd.Contains(strAtCmdLenovoExitCalMode))
                    {
                        //if (strRetBuf.Contains("(") && strRetBuf.Contains(")") && strRetBuf.Contains("1") && (straryRetCmdBuf.Length > 2))
                        if (strRetBuf.Contains("(") && strRetBuf.Contains(")") && (straryRetCmdBuf.Length > 2))
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "已经退出校准模式.");
                            break;
                        }
                        else
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "退出校准模式失败，请检查该指令是否有效!");
                            iStatus = 105;
                        }
                    }
                    else if (strWriteCmd.Contains(strAtCmdLenovoBackupModemData))
                    {
                        if (strRetBuf.Contains("(") && strRetBuf.Contains(")") && strRetBuf.Contains("1") && (straryRetCmdBuf.Length > 2))
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "已经备份校准数据.");
                            break;
                        }
                        else
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "备份校准数据失败，请检查该指令是否有效!");
                            iStatus = 106;
                        }
                    }
                    else if (strWriteCmd.Contains(strAtCmdLenovoModemReset))
                    {
                        if (strRetBuf.Contains("(") && strRetBuf.Contains(")") && strRetBuf.Contains("1") && (straryRetCmdBuf.Length > 2))
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "已经重启待测件.");
                            break;
                        }
                        else
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "重启待测件失败，请检查该指令是否有效!");
                            iStatus = 107;
                            break;
                        }
                    }
                    else if (strWriteCmd.Contains(strAtCmdLenovoModemLock))
                    {
                        if (strRetBuf.Contains("(") && strRetBuf.Contains(")") && strRetBuf.Contains("1") && (straryRetCmdBuf.Length > 2))
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "已经锁上待测件.");
                            break;
                        }
                        else
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "锁上待测件失败，请检查该指令是否有效!");
                            iStatus = 108;
                        }
                    }
                    else if (strWriteCmd.Contains(strAtCmdLenovoModemUnLock))
                    {
                        if (strRetBuf.Contains("(") && strRetBuf.Contains(")") && strRetBuf.Contains("1") && (straryRetCmdBuf.Length > 2))
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "已经对待测件解锁.");
                            break;
                        }
                        else
                        {
                            sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", "对待测件解锁失败，请检查该指令是否有效!");
                            iStatus = 109;
                        }
                    }

                    if (iRetryIndex != 0)
                    {
                        strItemMsg = string.Format("发送工程指令失败，第{0}次重新发送:{1}", iRetryIndex, strWriteCmd);
                        sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", strItemMsg);
                    }
                }
                else if (strWriteCmd.Contains("at"))
                {
                    if (strRetBuf.Contains("OK"))
                    {
                        break;
                    }
                    if (iRetryIndex != 0)
                    {
                        strItemMsg = string.Format("发送AT指令失败，第{0}次重新发送:{1}", iRetryIndex, strWriteCmd);
                        sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", strItemMsg);
                    }
                }
                else if (strWriteCmd.Contains("OPEN")
                    || strWriteCmd.Contains("CLOSE")
                    || strWriteCmd.Contains("STATUS")
                    || strWriteCmd.Contains("STATUS?")
                    || strWriteCmd.Contains("GLED_ON")
                    || strWriteCmd.Contains("GLED_OFF")
                    || strWriteCmd.Contains("GLED_FLASH")
                    || strWriteCmd.Contains("YLED_ON")
                    || strWriteCmd.Contains("YLED_OFF")
                    || strWriteCmd.Contains("YLED_FLASH")
                    || strWriteCmd.Contains("RLED_ON")
                    || strWriteCmd.Contains("RLED_OFF")
                    || strWriteCmd.Contains("RLED_FLASH")
                    || strWriteCmd.Contains("RESET")
                    || strWriteCmd.Contains("SELF_CHECK")
                    || strWriteCmd.Contains("TESTMODE_ON")
                    || strWriteCmd.Contains("TESTMODE_OFF")
                    || strWriteCmd.Contains("UUT1LTE")
                    || strWriteCmd.Contains("UUT13G")
                    || strWriteCmd.Contains("UUT2LTE")
                    || strWriteCmd.Contains("UUT23G")
                    || strWriteCmd.Contains("PASS")
                    || strWriteCmd.Contains("FAIL")
                    || strWriteCmd.Contains("SWITCHRF")
                    || strWriteCmd.Contains("T1InitOK")
                    || strWriteCmd.Contains("T1Open")
                    || strWriteCmd.Contains("T1Place")
                    || strWriteCmd.Contains("T1Close"))
                {
                    if (iFixtureType == 0)
                    {
                        if (strRetBuf.Contains("OK") || strRetBuf.Contains("OPEN") || strRetBuf.Contains("CLOSE") || strRetBuf.Contains("READY"))
                        {
                            break;
                        }
                    }
                    else if (iFixtureType == 1)
                    {
                        if (strRetBuf.Contains("OK") || strRetBuf.Contains("OPEN") || strRetBuf.Contains("CLOSE") || strRetBuf.Contains("READY"))
                        {
                            break;
                        }
                    }
                    else if (iFixtureType == 2)
                    {
                        if (strRetBuf.Contains("OK") || strRetBuf.Contains("OPEN") || strRetBuf.Contains("CLOSE") || strRetBuf.Contains("READY"))
                        {
                            break;
                        }
                    }
                    else if (iFixtureType == 3)
                    {
                        if (strWriteCmd.Contains("CLOSE") || strRetBuf.Contains("OK") || strRetBuf.Contains("OPEN") || strRetBuf.Contains("CLOSE") || strRetBuf.Contains("READY"))
                        {
                            break;
                        }
                    }
                    if (iRetryIndex != 0)
                    {
                        strItemMsg = string.Format("发送AT指令失败，第{0}次重新发送:{1}", iRetryIndex, strWriteCmd);
                        sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_item", strItemMsg);
                    }
                }
            }

            if (iRetryIndex == iRetryNum)
            {
                if (iStatus == 102)
                {
                    ////clExeCtrl_AddErrMsg(iStatus, "未获取到有效的软件版本信息，请检查指令是否正确!");
                }
                else if (iStatus == 103)
                {
                    //clExeCtrl_AddErrMsg(iStatus, "未获取到有效的SN，请检查SN是否有效写入或者该指令是否有效!");
                }
                else if (iStatus == 104)
                {
                    //clExeCtrl_AddErrMsg(iStatus, "进入校准模式失败，请检查该指令是否有效!");
                }
                else if (iStatus == 105)
                {
                    //clExeCtrl_AddErrMsg(iStatus, "退出校准模式失败，请检查该指令是否有效!");
                }
                else if (iStatus == 106)
                {
                    //clExeCtrl_AddErrMsg(iStatus, "备份校准数据失败，请检查该指令是否有效!");
                }
                else if (iStatus == 107)
                {
                    //clExeCtrl_AddErrMsg(iStatus, "重启待测件失败，请检查该指令是否有效!");
                }
                else if (iStatus == 108)
                {
                    //clExeCtrl_AddErrMsg(iStatus, "锁上待测件失败，请检查该指令是否有效!");
                }
                else if (iStatus == 109)
                {
                    //clExeCtrl_AddErrMsg(iStatus, "对待测件解锁失败，请检查该指令是否有效!");
                }
                else
                {
                    //clExeCtrl_AddErrMsg(198, "工程指令无有效返回，请检查该指令是否有效!");
                }
                iStatus = 198;
            }

            return iStatus;
        }

        public void clExeCtrl_WaitForStart()
        {
            //string strWriteCmd = "";
            //string strRetBuf = "";
            bReadyForStart = false;

            do
            {
                dtCurrentRunBegin = DateTime.Now;
                if ((iCurDutGroupIndex * 2.5 <= (dtCurrentRunBegin.Second % 10))
                    && ((dtCurrentRunBegin.Second % 10) <= (iCurDutGroupIndex + 1) * 2.5))
                {
                    break;
                }
                else
                {
                    //Thread.Sleep(1000);
                }
                iWaiteForStartMaxRetryIndex++;
            } while (iWaiteForStartMaxRetryIndex < iWaiteForStartMaxRetryNum);

            if (iWaiteForStartMaxRetryIndex < iWaiteForStartMaxRetryNum)
            {
                bReadyForStart = true;
            }
            else
            {
                bReadyForStart = false;
            }
            return;
        }

        public void clExeCtrl_FixtureOpen()
        {
            int iStatus = 0;
            string strWriteCmd = "";
            string strRetBuf = "";
            bFixtureOpen = false;
            int iRetryIndex = 0;
            int iRetryNum = 3;

            string strCmdVal1 = "";
            string strCmdVal2 = "";

            try
            {

                if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 1))
                {
                    strCmdVal1 = string.Format("enter_share_resource:resource_name={0}:time_out={1}", "fixture0", 10000);
                    strCmdVal2 = string.Format("exit_share_resource:resource_name={0}", "fixture0");
                }
                else if ((iCurDutGroupIndex == 2) || (iCurDutGroupIndex == 3))
                {
                    strCmdVal1 = string.Format("enter_share_resource:resource_name={0}:time_out={1}", "fixture1", 10000);
                    strCmdVal2 = string.Format("exit_share_resource:resource_name={0}", "fixture1");
                }

                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal1);
                runCtrl_ExeCmd(strCmdVal1, sbRetCmdBuf, sbRetCmdMsg);
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                iStatus = clExeCtrl_SerialPortInit(1,
                                                    strCurFixtureComPort,
                                                    strFixtureSerialPortBaudRate,
                                                    strFixtureSerialPortDataBits,
                                                    strFixtureSerialPortReadTimeout,
                                                    strFixtureSerialPortWriteTimeout,
                                                    strFixtureSerialPortParity,
                                                    strFixtureSerialPortEncoding);

                iStatus = clExeCtrl_SerialPortConnect(strCurFixtureComPort);
                if (iStatus != 0)
                {
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                    runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                    return;
                }
                if (iFixtureType == 0)
                {
                    if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 2))
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS_1?");
                    }
                    else if ((iCurDutGroupIndex == 1) || (iCurDutGroupIndex == 3))
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS_2?");
                    }
                    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    strRetBuf = strRetBuf.ToString().ToUpper();
                    if (strRetBuf.Contains("CLOSE"))
                    {
                        if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 2))
                        {
                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "OPEN_1");
                        }
                        else if ((iCurDutGroupIndex == 1) || (iCurDutGroupIndex == 3))
                        {
                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "OPEN_2");
                        }
                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    }
                }
                else if (iFixtureType == 1)
                {
                    for (iRetryIndex = 0; iRetryIndex < iRetryNum; iRetryIndex++)
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS");
                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                        strRetBuf = strRetBuf.ToString().ToUpper();
                        if (strRetBuf.Contains("OPEN"))
                        {
                            break;
                        }
                        else if (strRetBuf.Contains("CLOSE"))
                        {
                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "OPEN");
                            iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                        }
                    }
                }
                else if (iFixtureType == 2)
                {
                    for (iRetryIndex = 0; iRetryIndex < iRetryNum; iRetryIndex++)
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS?");
                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                        strRetBuf = strRetBuf.ToString().ToUpper();
                        if (strRetBuf.Contains("OPEN"))
                        {
                            break;
                        }
                        else if (strRetBuf.Contains("CLOSE"))
                        {
                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "OPEN");
                            iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                        }
                    }
                }
                else if (iFixtureType == 3)
                {
                    strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS");
                    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    strRetBuf = strRetBuf.ToString().ToUpper();
                    if (strRetBuf.Contains("CLOSE"))
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "OPEN");
                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    }
                }

                //LED
                if (iFixtureType == 0)
                {
                    if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 2))
                    {
                        if (strCurRunPassFail == "PASS")
                        {
                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "PASS_1");
                        }
                        else if (strCurRunPassFail == "FAIL")
                        {
                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "FAIL_1");
                        }
                        else if (strCurRunPassFail == "INIT")
                        {
                            ;
                        }
                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    }
                    else if ((iCurDutGroupIndex == 1) || (iCurDutGroupIndex == 3))
                    {
                        if (strCurRunPassFail == "PASS")
                        {
                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "PASS_2");
                        }
                        else if (strCurRunPassFail == "FAIL")
                        {
                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "FAIL_2");
                        }
                        else if (strCurRunPassFail == "INIT")
                        {
                            ;
                        }
                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    }
                }
                else if (iFixtureType == 1)
                {
                    //if (strCurRunPassFail == "PASS")
                    //{
                    //    strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "PASS");
                    //    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    //}
                    //else if (strCurRunPassFail == "FAIL")
                    //{
                    //    strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "FAIL");
                    //    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    //}
                    //else if (strCurRunPassFail == "INIT")
                    //{
                    //    ;
                    //}
                }
                else if (iFixtureType == 2)
                {
                    //if (strCurRunPassFail == "PASS")
                    //{
                    //    strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "PASS");
                    //    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    //}
                    //else if (strCurRunPassFail == "FAIL")
                    //{
                    //    strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "FAIL");
                    //    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    //}
                    //else if (strCurRunPassFail == "INIT")
                    //{
                    //    ;
                    //}
                }
                else if (iFixtureType == 3)
                {
                    //if (strCurRunPassFail == "PASS")
                    //{
                    //    strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "PASS");
                    //    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    //}
                    //else if (strCurRunPassFail == "FAIL")
                    //{
                    //    strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "FAIL");
                    //    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    //}
                    //else if (strCurRunPassFail == "INIT")
                    //{
                    //    ;
                    //}
                }

                iStatus = clExeCtrl_SerialPortDisconnect();
                //runCtrl_ExeCmd("exe_cmd:run_ctrl:exit_share_resource", sbRetCmdBuf, sbRetCmdMsg);
                //sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                bFixtureOpen = true;
                return;
            }
            catch(Exception exp)
            {  
                MessageBox.Show(exp.Message,"clExeCtrl_FixtureOpen fail");
                return;
            }
        }

        public void clExeCtrl_FixtureClose()
        {
            int iStatus = 0;
            string strWriteCmd = "";
            string strRetBuf = "";
            bFixtureClose = false;
            int iRetryIndex = 0;
            int iRetryNum = 3;

            string strMsgVal = "";

            string strCmdVal1 = "";
            string strCmdVal2 = "";

            try
            {
                if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 1))
                {
                    strCmdVal1 = string.Format("enter_share_resource:resource_name={0}:time_out={1}", "fixture0", 10000);
                    strCmdVal2 = string.Format("exit_share_resource:resource_name={0}", "fixture0");
                }
                else if ((iCurDutGroupIndex == 2) || (iCurDutGroupIndex == 3))
                {
                    strCmdVal1 = string.Format("enter_share_resource:resource_name={0}:time_out={1}", "fixture1", 10000);
                    strCmdVal2 = string.Format("exit_share_resource:resource_name={0}", "fixture1");
                }

                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal1);
                runCtrl_ExeCmd(strCmdVal1, sbRetCmdBuf, sbRetCmdMsg);
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());


                iStatus = clExeCtrl_SerialPortInit(1,
                                                    strCurFixtureComPort,
                                                    strFixtureSerialPortBaudRate,
                                                    strFixtureSerialPortDataBits,
                                                    strFixtureSerialPortReadTimeout,
                                                    strFixtureSerialPortWriteTimeout,
                                                    strFixtureSerialPortParity,
                                                    strFixtureSerialPortEncoding);

                iStatus = clExeCtrl_SerialPortConnect(strCurFixtureComPort);
                if (iStatus != 0)
                {
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                    runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                    strMsgVal = string.Format("Serial Port {0:d} Connect Fail", strCurFixtureComPort);
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strMsgVal);

                    bFixtureClose = false;

                    return;
                }

                if (iFixtureType == 0)
                {
                    if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 2))
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS_1?");
                    }
                    else if ((iCurDutGroupIndex == 1) || (iCurDutGroupIndex == 3))
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS_2?");
                    }

                    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);

                    if (strRetBuf.Contains("OPEN"))
                    {
                        if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 2))
                        {
                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "CLOSE_1");
                        }
                        else if ((iCurDutGroupIndex == 1) || (iCurDutGroupIndex == 3))
                        {
                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "CLOSE_2");
                        }

                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    }
                }
                else if (iFixtureType == 1)
                {
                    for (iRetryIndex = 0; iRetryIndex < iRetryNum; iRetryIndex++)
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS");

                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);

                        strRetBuf = strRetBuf.ToString().ToUpper();
                        if (strRetBuf.Contains("CLOSE"))
                        {
                            break;
                        }
                        else if (strRetBuf.Contains("OPEN"))
                        {
                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "CLOSE");
                            iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                        }
                    }
                }
                else if (iFixtureType == 2)
                {
                    for (iRetryIndex = 0; iRetryIndex < iRetryNum; iRetryIndex++)
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS?");

                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);

                        strRetBuf = strRetBuf.ToString().ToUpper();
                        if (strRetBuf.Contains("CLOSE"))
                        {
                            break;
                        }
                        else if (strRetBuf.Contains("OPEN"))
                        {
                            strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "CLOSE");
                            iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                        }
                    }
                }
                else if (iFixtureType == 3)
                {
                    strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "STATUS");
                    iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    strRetBuf = strRetBuf.ToString().ToUpper();
                    if (strRetBuf.Contains("OPEN"))
                    {
                        strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "CLOSE");
                        iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);
                    }
                }

                iStatus = clExeCtrl_SerialPortDisconnect();

                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                bFixtureClose = true;
                return;
            }              
            catch(Exception exp)
            {  
                MessageBox.Show(exp.Message,"clExeCtrl_FixtureClose fail");
                return;
            }
        }

        public void clExeCtrl_RfSwitch()
        {
            int iStatus = 0;
            string strWriteCmd = "";
            string strRetBuf = "";
            bRfSwitch = false;

            if (iFixtureType == 1)
            {
                //if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 1))
                //{
                //    runCtrl_ExeCmd("set_parameter:global:resource_name=fixture0", sbRetCmdBuf, sbRetCmdMsg);
                //    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());
                //}
                //else if ((iCurDutGroupIndex == 2) || (iCurDutGroupIndex == 3))
                //{
                //    runCtrl_ExeCmd("set_parameter:global:resource_name=fixture1", sbRetCmdBuf, sbRetCmdMsg);
                //    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());
                //}
                //runCtrl_ExeCmd("set_parameter:global:time_out=10000", sbRetCmdBuf, sbRetCmdMsg);
                //sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());
                //runCtrl_ExeCmd("exe_cmd:run_ctrl:enter_share_resource", sbRetCmdBuf, sbRetCmdMsg);
                //sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());
                iStatus = clExeCtrl_SerialPortInit(1,
                                                    strCurRfSwitchComPort,
                                                    strRfSwitchSerialPortBaudRate,
                                                    strRfSwitchSerialPortDataBits,
                                                    strRfSwitchSerialPortReadTimeout,
                                                    strRfSwitchSerialPortWriteTimeout,
                                                    strRfSwitchSerialPortParity,
                                                    strRfSwitchSerialPortEncoding);

                iStatus = clExeCtrl_SerialPortConnect(strCurRfSwitchComPort);
                if (iStatus != 0)
                {
                    //runCtrl_ExeCmd("exe_cmd:run_ctrl:exit_share_resource", sbRetCmdBuf, sbRetCmdMsg);
                    //sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());
                    return;
                }

                strWriteCmd = string.Format("exe_cmd:port:write:{0}={1}\r\n", "cmd", "SWITCHRF1");
                iStatus = clExeCtrl_SerialPortSendCmd(strWriteCmd, ref strRetBuf);

                iStatus = clExeCtrl_SerialPortDisconnect();
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());
                //runCtrl_ExeCmd("exe_cmd:run_ctrl:exit_share_resource", sbRetCmdBuf, sbRetCmdMsg);
                //sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());
            }
            bRfSwitch = true;
            return;
        }

        public void clExeCtrl_PowerOn()
        {
            int iStatus = 0;
            string strCmdVal1 = "";
            string strCmdVal2 = "";
            bPowerOn = false;           

            sInstrCtrl.dCurCh0PowerVoltage = dCurCh0PowerVoltage;
            sInstrCtrl.dCurCh0PowerCurrent = dCurCh0PowerCurrent;
            sInstrCtrl.dCurCh1PowerVoltage = dCurCh1PowerVoltage;
            sInstrCtrl.dCurCh1PowerCurrent = dCurCh1PowerCurrent;

            try
            {
                if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 1))
                {
                    strCmdVal1 = string.Format("enter_share_resource:resource_name={0}:time_out={1}", "power0", 10000);
                    strCmdVal2 = string.Format("exit_share_resource:resource_name={0}", "power0");
                }
                else if ((iCurDutGroupIndex == 2) || (iCurDutGroupIndex == 3))
                {
                    strCmdVal1 = string.Format("enter_share_resource:resource_name={0}:time_out={1}", "power1", 10000);
                    strCmdVal2 = string.Format("exit_share_resource:resource_name={0}", "power1");
                }

                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal1);
                runCtrl_ExeCmd(strCmdVal1, sbRetCmdBuf, sbRetCmdMsg);
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                iStatus = sInstrCtrl.clInstrCtrl_Connect(strCurPowerSupplyType, strCurPowerSupplyAddr, iCurPowerChanIndex);
                if (iStatus != 0)
                {
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                    runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());
                    return;
                }
                iStatus = sInstrCtrl.clInstrCtrl_PowerOn(strCurPowerSupplyType, iCurPowerChanIndex);
                if (iStatus != 0)
                {
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                    runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());
                    return;
                }
                iStatus = sInstrCtrl.clInstrCtrl_Disconnect();
                if (iStatus != 0)
                {
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                    runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());
                    return;
                }

                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                bPowerOn = true;
                return;
            }            
            catch(Exception exp)
            {  
                MessageBox.Show(exp.Message,"clExeCtrl_PowerOn fail");
                return;
            }
        }

        public void clExeCtrl_PowerOff()
        {
            int iStatus = 0;

            string strCmdVal1 = "";
            string strCmdVal2 = "";

            bPowerOff = false;

            try
            {
                if ((iCurDutGroupIndex == 0) || (iCurDutGroupIndex == 1))
                {
                    strCmdVal1 = string.Format("enter_share_resource:resource_name={0}:time_out={1}", "power0", 10000);
                    strCmdVal2 = string.Format("exit_share_resource:resource_name={0}", "power0");
                }
                else if ((iCurDutGroupIndex == 2) || (iCurDutGroupIndex == 3))
                {
                    strCmdVal1 = string.Format("enter_share_resource:resource_name={0}:time_out={1}", "power1", 10000);
                    strCmdVal2 = string.Format("exit_share_resource:resource_name={0}", "power1");
                }

                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal1);
                runCtrl_ExeCmd(strCmdVal1, sbRetCmdBuf, sbRetCmdMsg);
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());


                iStatus = sInstrCtrl.clInstrCtrl_Connect(strCurPowerSupplyType, strCurPowerSupplyAddr, iCurPowerChanIndex);
                if (iStatus != 0)
                {
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                    runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());
                    return;
                }
                iStatus = sInstrCtrl.clInstrCtrl_PowerOff(strCurPowerSupplyType, iCurPowerChanIndex);
                if (iStatus != 0)
                {
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                    runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());
                    return;
                }
                iStatus = sInstrCtrl.clInstrCtrl_Disconnect();
                if (iStatus != 0)
                {
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                    runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                    sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                    return;
                }

                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", strCmdVal2);
                runCtrl_ExeCmd(strCmdVal2, sbRetCmdBuf, sbRetCmdMsg);
                sMsgCtrl.SendMsg(g_ipFrmDut, WM_RUN_LOG, "run_log", sbRetCmdMsg.ToString());

                bPowerOff = true;
                return;
            }             
            catch(Exception exp)
            {  
                MessageBox.Show(exp.Message,"clExeCtrl_PowerOff fail");
                return;
            }
        }
    }
}
