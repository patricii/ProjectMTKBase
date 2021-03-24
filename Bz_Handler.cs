using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using TPWrapper;
using TPWrapper.LogParameters;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Collections; // Patricio

namespace Bz_Handler
{
    public class CItemListEquip
    {

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_InitializeEquipments")]
        public static extern int InitItemListEquip();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_LoadBZConfig")]
        public static extern void LoadBZConfig();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_SetMotorolaModel")]
        public static extern void SetMotorolaModel([MarshalAs(UnmanagedType.LPStr)] string strMotorolaModel);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetMotorolaModel")]
        public static extern void GetMotorolaModel(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_IsChecStatusEnable")]
        public static extern int IsChecStatusEnable();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_IsFqaVerify")]
        public static extern int IsFqaVerify();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_IsLogData")]
        public static extern int IsLogData();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_IsFqaGoldenGenerate")]
        public static extern int IsFqaGoldenGenerate();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_CheckStatus")]
        public static extern int CheckStatus();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_CheckStatusByStation")]
        public static extern int CheckStatusByStation([MarshalAs(UnmanagedType.LPStr)] string strStationId);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_SetTrackId")]
        public static extern void SetTrackId([MarshalAs(UnmanagedType.LPStr)] string strTrackId);
        
        [DllImport(@"C:\prod\bin\wrapperItemList.dll", EntryPoint = "InitItemList")]
        public static extern int InitItemList();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetMotModelfromModelFile")]
        public static extern string GetMotModelfromModelFile([MarshalAs(UnmanagedType.LPStr)] string strFactoryID, StringBuilder retorno);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetI2cSide")]
        public static extern void GetI2cSide(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetMotName")]
        public static extern void GetMotName(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetMQSDataFeedLogPath")]
        public static extern void GetMQSDataFeedLogPath(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetMQSDataTempLogPath")]
        public static extern void GetMQSDataTempLogPath(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetStationID")]
        public static extern void GetStationID(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetStationLine")]
        public static extern void GetStationLine(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_SetStationID")]
        public static extern void SetStationID([MarshalAs(UnmanagedType.LPStr)] string strStationId);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_IsEngMode")]
        public static extern int IsEngMode();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_CheckUserPass")]
        public static extern int CheckUserPass([MarshalAs(UnmanagedType.LPStr)] string strUserId,
                                                  [MarshalAs(UnmanagedType.LPStr)] string strPassWord);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetSofVerfromModelFile")]
        public static extern void GetSofVerfromModelFile(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetRFBandIdfromModelFile")]
        public static extern void GetRFBandIdfromModelFile(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetHdVerfromModelFile")]
        public static extern void GetHdVerfromModelFile(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetOdmName")]
        public static extern void GetOdmName(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetProjectCode")]
        public static extern void GetProjectCode(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetStationType")]
        public static extern void GetStationType(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetComPort")]
        public static extern void GetComPort(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetPS1GpibAddress")]
        public static extern void GetPS1GpibAddress(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetPS2GpibAddress")]
        public static extern void GetPS2GpibAddress(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "GetTEST_SETGpibAddress")]
        public static extern void GetTEST_SETGpibAddress(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetTestSetIPAddress")]
        public static extern void GetTestSetIPAddress(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetTestSetInst")]
        public static extern void GetTestSetInst(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetChCableLoss")]
        public static extern void GetChCableLoss( [MarshalAs(UnmanagedType.LPStr)] string strBand,
                                                    [MarshalAs(UnmanagedType.LPStr)] string strChannel,
                                                    StringBuilder strbReturnData);
        
        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_IsOpenJigEnable")]
        public static extern int IsOpenJigEnable();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetFamily")]
        public static extern void GetFamily(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetTecnology")]
        public static extern void GetTecnology(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_Get10MhzSetting")]
        public static extern void Get10MhzSetting(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetPowerSupplyModel")]
        public static extern void GetPowerSupplyModel(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetApSwfromModelFile")]
        public static extern void GetApSwfromModelFile(StringBuilder strbReturnData);
        
        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetBpSwfromModelFile")]
        public static extern void GetBpSwfromModelFile(StringBuilder strbReturnData);
        
        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetFisrtCfgFilefromModelFile")]
        public static extern void GetFisrtCfgFilefromModelFile(StringBuilder strbReturnData);
        
        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetFisrtIniFilefromModelFile")]
        public static extern void GetFisrtIniFilefromModelFile(StringBuilder strbReturnData);
        
        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetComPortRight")]
        public static extern void GetComPortRight(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetComPortLeft")]
        public static extern void GetComPortLeft(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetKpLogPath")]
        public static extern void GetKpLogPath(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_UpdateCableLossDataOnMTKConfigFile")]
        public static extern int UpdateCableLossDataOnMTKConfigFile();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_UpdateCableLossDataOnMTKConfigFile_SUMO")] // Patricio
        public static extern int UpdateCableLossDataOnMTKConfigFile_SUMO();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_FQAVerifyAteRun")]
        public static extern int FQAVerifyAteRun();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_ReadDVM1Voltage")]
        public static extern double ReadDVM1Voltage();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetFreqCableLoss")] 
        public static extern void GetFreqCableLoss([MarshalAs(UnmanagedType.LPStr)] string strPath,
                                                   [MarshalAs(UnmanagedType.LPStr)] string strFrequency,
                                                   [MarshalAs(UnmanagedType.LPStr)] string strLevel,
                                                   [MarshalAs(UnmanagedType.LPStr)] string strTxRx,
                                                   StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetAddTestLogFormat")] // Patricio
        public static extern void GetAddTestLogFormat(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetAddTestURL")] // Patricio
        public static extern void GetAddTestURL(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetFlowControlQueryURL")] // Patricio
        public static extern void GetFlowControlQueryURL(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_Output1StateON")] //BATT ON
        public static extern int Output1StateON();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_Output1StateOFF")] //BATT OFF
        public static extern int Output1StateOFF();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_Output2StateON")] //CHARGER ON
        public static extern int Output2StateON();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_Output2StateOFF")] //CHARGER OFF
        public static extern int Output2StateOFF();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_MeasPS2Current")]
        public static extern double MeasPS2Current();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_MeasPS1Current")]
        public static extern double MeasPS1Current();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetStandbyHiSpec")] //Spec no DataBase
        public static extern void GetStandbyHiSpec(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetStandbyLoSpec")] //Spec no DataBase
        public static extern void GetStandbyLoSpec(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetChargerLoSpec")] //Spec no DataBase
        public static extern void GetChargerLoSpec(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetChargerHiSpec")] //Spec no DataBase
        public static extern void GetChargerHiSpec(StringBuilder strbReturnData);
    
        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetPoweroffHiSpec")] //Spec no DataBase
        public static extern void GetPoweroffHiSpec(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetPoweroffLoSpec")] //Spec no DataBase
        public static extern void GetPoweroffLoSpec(StringBuilder strbReturnData);


        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetDoTestFromDataBase")] // TOKEN CURRENT TESTS
        public static extern int GetDoTestFromDataBase([MarshalAs(UnmanagedType.LPStr)] string MeasCode);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetTestSetFirmwareVersion")] // versão no database
        public static extern void GetTestSetFirmwareVersion(StringBuilder strbReturnData);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_SetRangeMAX")] //Set Range 2304 /2306
        public static extern int SetRangeMAX();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_SetRangeMIN")] //Set Range 2304 /2306
        public static extern int SetRangeMIN();


        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_GetTestSpecFromDataBase")]
        public static extern double GetTestSpecFromDataBase([MarshalAs(UnmanagedType.LPStr)] string strMeasCode, [MarshalAs(UnmanagedType.LPStr)] string strSpecType);
     
    }

    public class CI2cControl
    {
        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_SendI2cCommand")]
        public static extern int SendI2cCommand([MarshalAs(UnmanagedType.LPStr)] string strI2cCommand);

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_AcquireGPIBMutex")]
        public static extern int AcquireGPIBMutex();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_ReleaseGPIBMutex")]
        public static extern int ReleaseGPIBMutex();

    }

    public class CJagTests
    {
        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_CloseJig")]
        public static extern int CloseJig();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_OpenJig")]
        public static extern int OpenJig();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_InitPhone")]
        public static extern int InitPhone();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_InitPhoneByCloseJig")]
        public static extern int InitPhoneByCloseJig();
        
        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_SetUSB_PS2_5V")]
        public static extern int SetUSB_PS2_5V();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_SetBattery_PS1_4V")] // Patricio
        public static extern int SetBattery_PS1_4V();

        [DllImport(@"C:\\prod\\bin\\wrapper_Handler_Bz.dll", EntryPoint = "EXPORT_SetPowerSupply")] // Patricio
        public static extern int SetPowerSupply(int nChannel,
                                                [MarshalAs(UnmanagedType.LPStr)] string strVoltage,
                                                [MarshalAs(UnmanagedType.LPStr)] string strCurrent,
                                                [MarshalAs(UnmanagedType.LPStr)] string strState); 


    }

    public class CJagLocalFucntions
    {
        static string m_strCheckStatusResult; // Patricio
        static string m_meascode; // Patricio
        static string m_strFactory;
        static bool m_OkToScan = false;
        static string m_strTrackId;
       // static string m_strTrackIdDoubleCheck;
        static string m_strToolVersion;
        static TestSet TestEquip = new TestSet();
        static string m_strBzModelMode;
        static bool m_BBRecycle = false;  //Patricio

        public static string GetPowerSupplyModel()
        {
            StringBuilder strPowerSupply = new StringBuilder(20);
            string strPowerSupplyModel;
            Bz_Handler.CItemListEquip.GetPowerSupplyModel(strPowerSupply);

            if (strPowerSupply.ToString().Contains("2306"))
                strPowerSupplyModel = "KET2306";
            else if (strPowerSupply.ToString().Contains("66319"))
                strPowerSupplyModel = "AG66319B";
            else if (strPowerSupply.ToString().Contains("2304"))
                strPowerSupplyModel = "KET2304";
            else
            {
                MessageBox.Show("Power Supply model not found", "GetPowerSupplyModel", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
                strPowerSupplyModel = "NULL";
            }

            return strPowerSupplyModel;
        }

        public static string GetApSw()
        {
            StringBuilder strData = new StringBuilder(256);

            Bz_Handler.CItemListEquip.GetApSwfromModelFile(strData);

            return strData.ToString();

        }

        public static string GetBpSw()
        {
            StringBuilder strData = new StringBuilder(256);

            Bz_Handler.CItemListEquip.GetBpSwfromModelFile(strData);

            return strData.ToString();

        }

        public static string GetFisrtCfgFile()
        {
            StringBuilder strData = new StringBuilder(256);

            Bz_Handler.CItemListEquip.GetFisrtCfgFilefromModelFile(strData);

            return strData.ToString();

        }

        public static string GetBzToolVersion()
        {                        
            StringBuilder strData = new StringBuilder(256);

            string[] strFileNameSplit;

            string strCFGFileName = null;            

            Bz_Handler.CItemListEquip.GetFisrtCfgFilefromModelFile(strData);

            strFileNameSplit = strData.ToString().Split('\\');

            strCFGFileName = strFileNameSplit[strFileNameSplit.Length - 1];

            strFileNameSplit = strCFGFileName.Split('.');
  
            strCFGFileName = "BZ_V1.0_" + strFileNameSplit[0]; // Versão do SW Patricio

            if (strCFGFileName.Length > 49)
                strCFGFileName = strCFGFileName.Substring(0, 49);

            return strCFGFileName.ToString();
        }

        public static string GetFisrtIniFile()
        {
            StringBuilder strData = new StringBuilder(256);

            Bz_Handler.CItemListEquip.GetFisrtIniFilefromModelFile(strData);

            return strData.ToString();
        }
        
        public static string GetComPortLeft()
        {
            StringBuilder strData = new StringBuilder(256);

            Bz_Handler.CItemListEquip.GetComPortLeft(strData);

            return strData.ToString();
        }
        
        public static string GetComPortRight()
        {
            StringBuilder strData = new StringBuilder(256);

            Bz_Handler.CItemListEquip.GetComPortRight(strData);

            return strData.ToString();
        }
        
        public static string GetTestSetAddress()
        {
            StringBuilder strData = new StringBuilder(256);
            StringBuilder strInstrument = new StringBuilder(256);
            
            string strDataReturned;

            Bz_Handler.CItemListEquip.GetTestSetIPAddress(strData);
            Bz_Handler.CItemListEquip.GetTestSetInst(strInstrument);

            strDataReturned = ("TCPIP0::" + strData.ToString() + "::" + strInstrument.ToString() + "::INSTR");

            return strDataReturned;
        }

        public static string GetPowerSupplyAddress()
        {
            StringBuilder strData = new StringBuilder(256);
            string strDataReturned;

            Bz_Handler.CItemListEquip.GetPS1GpibAddress(strData);

            strDataReturned = ("GPIB0::" + strData.ToString() + "::INSTR");

            return strDataReturned;
        }
        public static int CheckTestSetFWVersion()  //Check TestSet Firmware Version Patricio
        {
            int nStatus = -1;
            string strTestSetResponse;
            StringBuilder TestSetFirmwareVersion = new StringBuilder(50);
            Bz_Handler.CItemListEquip.GetTestSetFirmwareVersion(TestSetFirmwareVersion);
            nStatus = TestEquip.SendCommandToTestSet("*IDN?");
            Thread.Sleep(200);
            strTestSetResponse = TestEquip.ReadfromTestSet();
            if (!strTestSetResponse.Contains(TestSetFirmwareVersion.ToString()))
            {
                nStatus = -1;
                MessageBox.Show("Test Set com Versão de Firmware Incorreto, Versão correta: " + TestSetFirmwareVersion.ToString());
            }

            return nStatus;
        }

        public static int CheckTestOptions() // Check TestSet Option to MTK Patricio
        {
            int nStatus = -1;
            string strTestSetResponse;
            nStatus = TestEquip.SendCommandToTestSet(":SYST:BASE:OPT:LIST? SWOP");
            Thread.Sleep(200);
            strTestSetResponse = TestEquip.ReadfromTestSet();
            if (!strTestSetResponse.Contains("KV113"))
            {
                nStatus = -1;
                MessageBox.Show("Test Set sem Licença KV113");
            }

            return nStatus;                    
        }
        public static int Check10Mhz()
        {
            int nStatus = -1;
            int nReferenceLock = 0;
            StringBuilder strTestSetIpAddress = new StringBuilder(20);
            StringBuilder strInstrument = new StringBuilder(256);
            StringBuilder str10MhzSetting = new StringBuilder(10);
            string strTestSetResponse;

           
            Bz_Handler.CItemListEquip.Get10MhzSetting(str10MhzSetting);
            Bz_Handler.CItemListEquip.GetTestSetIPAddress(strTestSetIpAddress);
            Bz_Handler.CItemListEquip.GetTestSetInst(strInstrument);

            if (strInstrument.ToString().Equals("inst0"))
                nStatus = TestEquip.ConnectTestSet(strTestSetIpAddress.ToString(), 5025);
            else if (strInstrument.ToString().Equals("inst1"))
                nStatus = TestEquip.ConnectTestSet(strTestSetIpAddress.ToString(), 5026);
            else
            {
                nStatus = -1;
                MessageBox.Show("Instrument: " + strInstrument.ToString() + " not configured properly", "Check10Mhz");
            }
            
            Thread.Sleep(350);
            if (nStatus == 0)
            {
                if (str10MhzSetting.ToString().Contains("EXTERNAL"))
                {
                    nStatus = TestEquip.SendCommandToTestSet("SYST:REF:FREQ:SOUR EXT");
                    //nStatus = TestEquip.SendCommandToTestSet("SYST:REF:FREQ:SOUR EXT;*OPC");
                    
                    Thread.Sleep(350);
                    if (nStatus == 0)
                        nStatus = TestEquip.SendCommandToTestSet("SYST:REF:FREQ:SOUR EXT");
                        //nStatus = TestEquip.SendCommandToTestSet("SYST:REF:FREQ:SOUR EXT;*OPC");

                    Thread.Sleep(350);
                    if (nStatus == 0)
                        nStatus = TestEquip.SendCommandToTestSet("SYST:REF:FREQ 10E+6 Hz");


                    nStatus = -1;
                    Thread.Sleep(350);
                    while (nStatus!=0 && nReferenceLock <5)
                    {
                        nStatus = TestEquip.SendCommandToTestSet("SENS:BASE:REF:FREQ:LOCK?");
                    
                        if (nStatus == 0)
                        {
                            strTestSetResponse = TestEquip.ReadfromTestSet();
                            if (!strTestSetResponse.Contains("1"))
                            {
                                nStatus = -1;
                                Thread.Sleep(1000);
                                nReferenceLock++;
                                
                            }
                        }
                    }
                }
                else if (str10MhzSetting.ToString().Contains("INTERNAL"))
                {
                    Thread.Sleep(350);

                    nStatus = TestEquip.SendCommandToTestSet("SYST:REF:FREQ:SOUR INT;*OPC");

                    nStatus = TestEquip.SendCommandToTestSet("SYST:REF:FREQ:SOUR INT;*OPC");
                }
            }
  
            TestEquip.Disconnect();

            return nStatus;
        }
       
        public static void SetToolVersion(string strToolVersion)
        {
            m_strToolVersion = strToolVersion;
        }

        public static String GetToolVersion()
        {
            return m_strToolVersion;
        }

        public static void SetTrackId(string strTrackId)
        {
            m_strTrackId = strTrackId;
        }

        public static String GetTrackId()
        {
            return m_strTrackId;
        }

        public static void SetCheckStatusResult(string strCheckStatusResult) // Patricio
        {
            m_strCheckStatusResult = strCheckStatusResult;
        }
                
        public static bool IsOkToScan()
        {
            return m_OkToScan;
        }
   
        public static string GetFactory()
        {
            return m_strFactory;
        }

        public static void SetFactory(string strFactory)
        {
            m_strFactory = strFactory;
        }

        public static string GetBzScanMode()
        {
            return m_strBzModelMode;
        }

        public static void SetBzScanMode(string strMode)
        {
            m_strBzModelMode = strMode;
        }
        
        public static int EntryHandlerSystem()
        {
            
            int nStatus = 0;
            nStatus = Bz_Handler.CI2cControl.SendI2cCommand("PASS_LAMP_OFF");

            if (nStatus == 0)
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("FAIL_LAMP_OFF");

            if (nStatus == 0)
            {
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("D+_D-_OPEN");
                Thread.Sleep(200);
            }

            if (nStatus == 0)
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("ID_TO_VBUS_OPEN");

            if (nStatus == 0)
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("PSU2_OPEN"); // WIND PAtricio

            if (nStatus == 0)
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("PSU1_OPEN");

            string PowerSupplyBB = Bz_Handler.CJagLocalFucntions.GetPowerSupplyModel();


            
            if (PowerSupplyBB.Contains("2304")) // Power_Supply 2304 to BB
            {
                Bz_Handler.CJagTests.SetBattery_PS1_4V(); // Patricio
            }
            if (nStatus == 0)
            {
                nStatus = Bz_Handler.CJagLocalFucntions.ConfigTestSet2Instr();
                if (nStatus != 0)
                {
                    while (nStatus != 0)
                    {
                        MessageBox.Show("ATENCAO: ERRO AO CONFIGURAR QUANTIDADE DE EQUIPAMENTOS (DEVICE->ONE/TWO Instrument)  - Chamar a manutenção !");
                    }
                }
            }

            //if (nStatus == 0) // Patricio To ANDY
            //{
            //    nStatus = Bz_Handler.CJagTests.SetUSB_PS2_5V();
            //    Thread.Sleep(200);
            //}

            if (nStatus == 0)
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("D+_D-_OPEN");

            if (nStatus == 0)
                nStatus = OpenPowerKey();

            if (nStatus == 0)
                nStatus = OpenTunerDVM();


            if (nStatus == 0)
                m_OkToScan = true;
        
             return nStatus;
            
        }

        public static int EntryHandlerTest()
        {
            int nStatus = 0;

            nStatus = Bz_Handler.CI2cControl.SendI2cCommand("PASS_LAMP_OFF");

            if (nStatus == 0)
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("FAIL_LAMP_OFF");

            if (nStatus == 0)
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("D+_D-_CLOSE");

            if (nStatus == 0)
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("PSU2_OPEN");

            if (nStatus == 0)
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("PSU1_CLOSE");

            if (nStatus == 0)
                nStatus = Bz_Handler.CJagLocalFucntions.CloseTunerDVM();
                       
            return nStatus;

        }

        public static int ExitHandlerTest(string strPassFail)
        {
            int nStatus = 0;

            if (strPassFail == "PASS")
            {
                Thread.Sleep(200);
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("PASS_LAMP_ON");
            }
            else
            {
                Thread.Sleep(200);
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("FAIL_LAMP_ON");
            }

            if (nStatus == 0)
                nStatus = Bz_Handler.CI2cControl.ReleaseGPIBMutex();
            if (nStatus == 0)
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("D+_D-_OPEN");

            if (nStatus == 0)
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("PSU2_OPEN");

            if (nStatus == 0)
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("PSU1_OPEN");
            if (nStatus == 0)
            {
                nStatus = Bz_Handler.CI2cControl.SendI2cCommand("PASS_LAMP_ON");
                if (nStatus == 0)
                    nStatus = Bz_Handler.CI2cControl.SendI2cCommand("FAIL_LAMP_ON");
            }
            
            return nStatus;

        }

        public static int Set_USB_PS2_5V()
        {
            int nStatus = 0;
            nStatus = SendI2CCommand("D+_D-_CLOSE");
            nStatus = Bz_Handler.CJagTests.SetUSB_PS2_5V();

            return nStatus;
        }      

        public static int ClosePowerKey()
        {
            int nStatus = 0;
            
            nStatus = Bz_Handler.CI2cControl.SendI2cCommand("CHLS_PW_KEY_CLOSE");
            
            return nStatus;
        }

        public static int OpenPowerKey()
        {
            int nStatus = 0;

            nStatus = Bz_Handler.CI2cControl.SendI2cCommand("CHLS_PW_KEY_OPEN");

            return nStatus;
        }

        public static int CloseTunerDVM()
        {
            int nStatus = 0;

            nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_TUNER_CLOSE");

            return nStatus;
        }

        public static int OpenTunerDVM()
        {
            int nStatus = 0;

            nStatus = Bz_Handler.CI2cControl.SendI2cCommand("DVM1_TUNER_OPEN");

            return nStatus;
        }

        public static int SendI2CCommand(string strI2CCommand)
        {
            
            return Bz_Handler.CI2cControl.SendI2cCommand(strI2CCommand);
        }

        public static int SetPowerSupply(int nChannel, string strVoltage, string strCurrent, string strState)
        {
            
            return Bz_Handler.CJagTests.SetPowerSupply(nChannel, strVoltage, strCurrent, strState);
        }

       
        public static int I2CDisconnectAll()
        {
            int nStatus = 0;

            if (nStatus == 0)
                nStatus = SendI2CCommand("CHLS_PW_KEY_OPEN");
      
            if (nStatus == 0)
                nStatus = SendI2CCommand("PSU1_OPEN");

            if (nStatus == 0)
                nStatus = SendI2CCommand("PSU2_OPEN");

            if (nStatus == 0)
                nStatus = SendI2CCommand("D+_D-_OPEN");

            return nStatus;
        }
        public static int I2CTurnPhoneOnByVBAT()
        {
            int nStatus = 0;

            if (nStatus == 0)
                SetPowerSupply(1, "4", "2", "ON");

            if (nStatus == 0)
                nStatus = SendI2CCommand("PSU1_CLOSE");

            if (nStatus == 0)
                nStatus = SendI2CCommand("CHLS_PW_KEY_CLOSE");

            Thread.Sleep(3000);

            if (nStatus == 0)
                nStatus = SendI2CCommand("CHLS_PW_KEY_OPEN");

            return nStatus;
        }
        public static int ConfigTestSet2Instr()
        {
            int nStatus = -1;         
            StringBuilder strTestSetIpAddress = new StringBuilder(20);
            StringBuilder strInstrument = new StringBuilder(256);
            string strTestSetResponse;
           
            Bz_Handler.CItemListEquip.GetTestSetIPAddress(strTestSetIpAddress);
            Bz_Handler.CItemListEquip.GetTestSetInst(strInstrument);

            if (strInstrument.ToString().Equals("inst0"))
                nStatus = TestEquip.ConnectTestSet(strTestSetIpAddress.ToString(), 5025);
            else if (strInstrument.ToString().Equals("inst1"))
                nStatus = TestEquip.ConnectTestSet(strTestSetIpAddress.ToString(), 5026);
            
            while (nStatus != 0)
            {
                MessageBox.Show("Instrument: " + strInstrument.ToString() + " not configured properly \n CHAMAR MANUTENCAO", "ConfigTestSet2Instr");
            }

            Thread.Sleep(300);
            if (nStatus == 0)
            {                
                nStatus = TestEquip.SendCommandToTestSet("SYSTem:BASE:DEVice:SUBinst?");

                if (nStatus == 0)
                {
                    strTestSetResponse = TestEquip.ReadfromTestSet();
                    if (!strTestSetResponse.Contains("2"))
                    {                        
                        nStatus = TestEquip.SendCommandToTestSet("*CLS");
                        Thread.Sleep(300);

                        nStatus = TestEquip.SendCommandToTestSet("SYSTem:BASE:DEVice:COUNt 2");
                        Thread.Sleep(300);

                        nStatus = TestEquip.SendCommandToTestSet("SYSTem:BASE:DEVice:RESet");
                        Thread.Sleep(300);                       
                    }
                }                                             
            }

            TestEquip.Disconnect();

            while (nStatus != 0)
            {
                Bz_Handler.CI2cControl.SendI2cCommand("PASS_LAMP_ON");
                Bz_Handler.CI2cControl.SendI2cCommand("FAIL_LAMP_ON");
                MessageBox.Show("ATENÇÃO: ERRO AO CONFIGURAR QUANTIDADE DE EQUIPAMENTOS (DEVICE->ONE/TWO Instrument)  - Chamar a manutenção !", "ConfigTestSet2Instr");
            }

            return nStatus;
        }

        public static void SetBBRecycle(bool bBBRecycle) // Patricio
        {
            m_BBRecycle = bBBRecycle;
        }

        public static bool GetBBRecyle()
        {
            return m_BBRecycle;
        }
       
    }
    public class BzLogResult
    {
        TPWrapper.LogDataAcquisition logProcess = new TPWrapper.LogDataAcquisition();

        public StringBuilder strFinalResult = new StringBuilder();

        public int LoadLogResult(string strErrorMessage)
        {
            strErrorMessage = string.Empty;
            int nStatus = 0;
            string strStationCode;
            string strStnId;
            string strStnLine;
            StringBuilder strStationId = new StringBuilder(56);
            StringBuilder strStationLine = new StringBuilder(56);
            StringBuilder strStationType = new StringBuilder(56);
            StringBuilder strMQSDataFeedLogPath = new StringBuilder(56);
            StringBuilder strMQSDataTempLogPath = new StringBuilder(56);
            StringBuilder strLogFormat = new StringBuilder(256); // Patricio
            StringBuilder strServerURL = new StringBuilder(256); //Patricio
            
            Bz_Handler.CItemListEquip.GetStationLine(strStationLine);
            Bz_Handler.CItemListEquip.GetStationType(strStationType);
            Bz_Handler.CItemListEquip.GetStationID(strStationId);
            Bz_Handler.CItemListEquip.GetMQSDataFeedLogPath(strMQSDataFeedLogPath);
            Bz_Handler.CItemListEquip.GetMQSDataTempLogPath(strMQSDataTempLogPath);
            Bz_Handler.CItemListEquip.GetAddTestLogFormat(strLogFormat); // Patricio
            Bz_Handler.CItemListEquip.GetAddTestURL(strServerURL); // Patricio

            strStnId = strStationId.ToString();
            strStnLine = strStationLine.ToString();
            strStationCode = strStnLine + "-" + strStnId;


            InitParameters parameters = new InitParameters();
            parameters.computerMacAddress = "00000000000000000";
            parameters.fixtureId = "*";
            parameters.processCode = strStationType.ToString();
            parameters.siteCode = "JAG";
            parameters.softwareReleaseVersion = Bz_Handler.CJagLocalFucntions.GetToolVersion().ToString();
            parameters.stationCode = strStationCode;
            parameters.headerVersion = "TH4";
            parameters.testHeaderVersion = "TR1";
            parameters.logFormat = LogFileFormat.MQS;
            parameters.feedPath = strMQSDataFeedLogPath.ToString();
            parameters.temporaryPath = strMQSDataTempLogPath.ToString();

            // FQA-Golden Process - Debug!!! change log result path to C:\prod\FQATemp

            if (Bz_Handler.CItemListEquip.IsFqaVerify() == 1)
                parameters.feedPath = @"C:\prod\FQATemp";
                
            if (Bz_Handler.CItemListEquip.IsLogData() == 0)
                parameters.feedPath = @"C:\Prod\temp";

            //Patricio

            if (strLogFormat.ToString().Equals("MQS"))
                parameters.logFormat = LogFileFormat.MQS;
            else if (strLogFormat.ToString().Equals("MQS_AND_HTTP_POST"))
                parameters.logFormat = LogFileFormat.MQS_AND_HTTP_POST;
            else if (strLogFormat.ToString().Equals("HTTP_POST"))
                parameters.logFormat = LogFileFormat.HTTP_POST;
            else
            {
                MessageBox.Show("FUNCTION LoadLogResult() Error:\n\rLog Format not valid-->" + strLogFormat.ToString());
                Application.Exit();
            }

            if (strLogFormat.ToString().Contains("HTTP"))
            {
                if (strServerURL.Length == 0)
                {
                    MessageBox.Show("FUNCTION LoadLogResult() Error:\n\rHTTP POST URL Not Valid-->" + strServerURL.ToString());
                    Application.Exit();
                }

                parameters.serverUri = @strServerURL.ToString();
                //parameters.serverUri = @"http://jagnt001.americas.ad.flextronics.com/FF_Http_AutoTester/default.aspx";
            }
            nStatus = logProcess.Init(parameters, out strErrorMessage);
            return nStatus;
        }

        public int StartLogResult(string strErrorMessage)
        {
            strErrorMessage = string.Empty;
            int nStatus = 0;

            StringBuilder strMotorolaModel = new StringBuilder(16);
            StringBuilder strMotorolaName = new StringBuilder(16);
            StringBuilder strTecnology = new StringBuilder(16);
            StringBuilder strFamily = new StringBuilder(16);

            Bz_Handler.CItemListEquip.GetMotName(strMotorolaName);
            Bz_Handler.CItemListEquip.GetMotorolaModel(strMotorolaModel);
            Bz_Handler.CItemListEquip.GetFamily(strFamily);
            Bz_Handler.CItemListEquip.GetTecnology(strTecnology);

            string strRecipe = strMotorolaName + "_BRD";
            
            StartRecipeParameters parameters = new StartRecipeParameters();
            parameters.equipmentId = "";
            parameters.family = strFamily.ToString();
            parameters.motorolaModel = strMotorolaModel.ToString();
            parameters.recipeName = strRecipe;
            parameters.shopOrder = "";
            parameters.softwareId = "";
            parameters.technology = strTecnology.ToString();
            parameters.XcvrNumber = "P1B";

            nStatus = logProcess.StartRecipe(parameters, out strErrorMessage);
            return nStatus;
        }

        public int AddLogResult(string strMeasCode, string strMeasDescription,string strTestResult,
                                         string strHighLimit, string strLowLimit, string strYHighLimit, 
                                         string strYLowLimit, int nPassFail, string strUnitsstring, string strErrorMessage)
        {
            strErrorMessage = string.Empty;
            int nStatus = 0;

            EvalTestParameters parameters = new EvalTestParameters();
            parameters.attempts = 1;
            parameters.testErrorMessage = "";
            parameters.testFailCode = "";
            if(nPassFail==0)
                parameters.testResult = TestResult.Pass;
            else
                parameters.testResult = TestResult.Fail;
            //parameters.testResult = TestResult.Pass;
            parameters.testValue = Convert.ToDouble(strTestResult);
            parameters.highLimit = Convert.ToDouble(strHighLimit);
            parameters.lowLimit = Convert.ToDouble(strLowLimit);
            parameters.testCode = "";//strMeasCode;
            parameters.testGroup = "Data_No_Stat_Analysis";
            parameters.testName = strMeasDescription;
            parameters.testSpecName = "";
            parameters.testSubGroup = "";
            parameters.testTextValue = "";
            parameters.retestFlag = 0;

            nStatus = logProcess.EvalTest(parameters, out strErrorMessage);
            return nStatus;
        }
        public int LogResult(string strPassFail, string strErrorMessage)
        {
            strErrorMessage = string.Empty;
            int nStatus = 0;
            EndRecipeParameters parameters = new EndRecipeParameters();
            parameters.trackId = Bz_Handler.CJagLocalFucntions.GetTrackId();
            if (strPassFail == "FAIL")
                parameters.masterTestResult = TestResult.Fail;
            else
                parameters.masterTestResult = TestResult.Pass;

            nStatus = logProcess.EndRecipe(parameters, out strErrorMessage);
            return nStatus;
        }

        public int StartTestTimeBlock(string strTestName, string strErrorMessage)
        {

            strErrorMessage = string.Empty;
            int nStatus = 0;

            EvalTestParameters parameters = new EvalTestParameters();
            parameters.attempts = 1;
            parameters.testErrorMessage = "";
            parameters.testFailCode = "";
            parameters.testResult = TestResult.Pass;
            parameters.testValue = 0;
            parameters.highLimit = 0;
            parameters.lowLimit = 0;
            parameters.testCode = "TM_SBLTH";
            parameters.testGroup = "Data_No_Stat_Analysis";
            parameters.testName = strTestName;
            parameters.testSpecName = "";
            parameters.testSubGroup = "";
            parameters.testTextValue = "";

            nStatus = logProcess.StartNewTimeBlock(strTestName, out strErrorMessage);

            if (nStatus == 0)
                nStatus = logProcess.EvalTest(parameters, out strErrorMessage);
            return nStatus;
        }

        public int FinishTestTimeBlock(string strTestName, string strErrorMessage)
        {
            strErrorMessage = string.Empty;
            int nStatus = 0;
            double testValue;

            EvalTestParameters parameters = new EvalTestParameters();
            parameters.attempts = 1;
            parameters.testErrorMessage = "";
            parameters.testFailCode = "";
            parameters.testResult = TestResult.Pass;
            parameters.highLimit = 9999999;
            parameters.lowLimit = 0;
            parameters.testCode = "TM_EBLTH";
            parameters.testGroup = "Data_No_Stat_Analysis";
            parameters.testName = strTestName;
            parameters.testSpecName = "";
            parameters.testSubGroup = "";
            parameters.testTextValue = "";

            nStatus = logProcess.StopTimeBlock(parameters.testName, out strErrorMessage, out testValue);
                     
            parameters.testValue = testValue;

            if (nStatus == 0)
                nStatus = logProcess.EvalTest(parameters, out strErrorMessage);
            return nStatus;
        }



        public int AddTestResult(string strTestResult, String ErrorMessage)
        {
            int nPassFail = -1; //Pass = 0, Fail = 1
            string strMeasDesc = "";
            int nStatus = -1;

            try
            {
                //Set Pass/Fail flag
                if (strTestResult.Contains("PASS"))
                    nPassFail = 0;
                else if (strTestResult.Contains("FAIL"))
                    nPassFail = 1;

                string[] strTestResultValues = strTestResult.Split(',');

                //BB Test Result: Example: BB_RECEIVER,1,,,,,,,,,,,,,PASS

                if (strTestResult.StartsWith("BB_"))
                {
                    if (strTestResultValues[7].Length > 0) // Test with low/high spec
                        nStatus = AddLogResult(strTestResultValues[0], strTestResultValues[0], strTestResultValues[8], strTestResultValues[9], strTestResultValues[7], strTestResultValues[9], strTestResultValues[7], nPassFail, "NA", ErrorMessage);
                    else //Test without spec
                        nStatus = AddLogResult(strTestResultValues[0], strTestResultValues[0], strTestResultValues[1], strTestResultValues[1], strTestResultValues[1], strTestResultValues[1], strTestResultValues[1], nPassFail, "NA", ErrorMessage);
                }
                //Calibration MAIN Results   // Patricio
                if (strTestResult.StartsWith("CAL_"))
                {
                    if (!strTestResult.Contains("HEADER"))
                    {

                        if ((strTestResultValues[11].Contains("PASS")) || (strTestResultValues[11].Contains("FAIL")))
                        {
                            strMeasDesc = strTestResultValues[0] +
                                        ((strTestResultValues[1].Trim().Length > 0) ? ("_" + strTestResultValues[1].Trim()) : "") +
                                        ((strTestResultValues[2].Trim().Length > 0) ? ("_" + strTestResultValues[2].Trim()) : "") +
                                        ((strTestResultValues[3].Trim().Length > 0) ? ("_" + strTestResultValues[3].Trim()) : "") +
                                        ((strTestResultValues[4].Trim().Length > 0) ? ("_" + strTestResultValues[4].Trim()) : "") +
                                        ((strTestResultValues[5].Trim().Length > 0) ? ("_" + strTestResultValues[5].Trim()) : "") +
                                        ((strTestResultValues[6].Trim().Length > 0) ? ("_" + strTestResultValues[6].Trim()) : "");

                            nStatus = AddLogResult(strMeasDesc.ToUpper(), strMeasDesc.ToUpper(), strTestResultValues[7], strTestResultValues[10], strTestResultValues[9], strTestResultValues[10], strTestResultValues[9], nPassFail, strTestResultValues[6], ErrorMessage);
                        }

                        if (strTestResult.StartsWith("CAL_RES_LTE_TX_TPC_ALL_VAL"))
                        {

                            strMeasDesc = strTestResultValues[0] +
                                        ((strTestResultValues[1].Trim().Length > 0) ? ("_" + strTestResultValues[1].Trim()) : "") +
                                        ((strTestResultValues[2].Trim().Length > 0) ? ("_" + strTestResultValues[2].Trim()) : "") +
                                        ((strTestResultValues[3].Trim().Length > 0) ? ("_" + strTestResultValues[3].Trim()) : "") +
                   //!!!MQS Issue!!!   // ((strTestResultValues[4].Trim().Length > 0) ? ("_" + strTestResultValues[4].Trim()) : "") +   fix MQS issue
                                        ((strTestResultValues[5].Trim().Length > 0) ? ("_" + strTestResultValues[5].Trim()) : "") +
                                        ((strTestResultValues[6].Trim().Length > 0) ? ("_" + strTestResultValues[6].Trim()) : "") +
                                        ((strTestResultValues[7].Trim().Length > 0) ? ("_" + strTestResultValues[7].Trim()) : "");

                            //pd_report
                            nStatus = AddLogResult((strMeasDesc.ToUpper() + "_PDREPORT"), (strMeasDesc.ToUpper() + "_PDREPORT"), strTestResultValues[8].Trim(), "9999", "-9999", "9999", "-9999", 0, "DAC", ErrorMessage); //just log - always pass

                            //pd_offset
                            if (nStatus == 0)
                                nStatus = AddLogResult((strMeasDesc.ToUpper() + "_PDOFFSET"), (strMeasDesc.ToUpper() + "_PDOFFSET"), strTestResultValues[9].Trim(), "9999", "-9999", "9999", "-9999", 0, "NA", ErrorMessage); //just log - always pass

                            //tx_power
                            if (nStatus == 0)
                                nStatus = AddLogResult((strMeasDesc.ToUpper() + "_TXPOWER"), (strMeasDesc.ToUpper() + "_TXPOWER"), strTestResultValues[10].Trim(), "9999", "-9999", "9999", "-9999", 0, "dbm", ErrorMessage); //just log - always pass

                            //diffence
                            if (nStatus == 0)
                                nStatus = AddLogResult((strMeasDesc.ToUpper() + "_DIFFERENCE"), (strMeasDesc.ToUpper() + "_DIFFERENCE"), strTestResultValues[11].Trim(), "9999", "-9999", "9999", "-9999", 0, "NA", ErrorMessage); //just log - always pass

                            //back_off
                            if (nStatus == 0)
                                nStatus = AddLogResult((strMeasDesc.ToUpper() + "_BACKOFF"), (strMeasDesc.ToUpper() + "_BACKOFF"), strTestResultValues[12].Trim(), "9999", "-9999", "9999", "-9999", 0, "NA", ErrorMessage); //just log - always pass

                            //result
                            if (nStatus == 0)
                                nStatus = AddLogResult((strMeasDesc.ToUpper() + "_RESULT"), (strMeasDesc.ToUpper() + "_RESULT"), strTestResultValues[13].Trim(), "9999", "-9999", "9999", "-9999", 0, "NA", ErrorMessage); //just log - always pass
                        }

                        if (strTestResult.StartsWith("CAL_RES_LTE_RX_PATHLOSS_RSSI_VAL"))
                        {
                            if (!strTestResult.Contains("band"))
                            {
                                strMeasDesc = strTestResultValues[0] +
                                            ((strTestResultValues[1].Trim().Length > 0) ? ("_" + strTestResultValues[1].Trim()) : "") +
                                            ((strTestResultValues[2].Trim().Length > 0) ? ("_" + strTestResultValues[2].Trim()) : "") +
                                            ((strTestResultValues[3].Trim().Length > 0) ? ("_" + strTestResultValues[3].Trim()) : "");

                                if (strTestResultValues[3].Trim().Length > 0)
                                {
                                    //rssi1
                                    nStatus = AddLogResult((strMeasDesc.ToUpper() + "_RSSI1"), (strMeasDesc.ToUpper() + "_RSSI1"), strTestResultValues[4].Trim(), "9999", "-9999", "9999", "-9999", 0, "dBm", ErrorMessage); //just log - always pass

                                    //rssi2
                                    if (nStatus == 0)
                                        nStatus = AddLogResult((strMeasDesc.ToUpper() + "_RSSI2"), (strMeasDesc.ToUpper() + "_RSSI2"), strTestResultValues[5].Trim(), "9999", "-9999", "9999", "-9999", 0, "dBm", ErrorMessage); //just log - always pass

                                    // ini1
                                    if (nStatus == 0)
                                        nStatus = AddLogResult((strMeasDesc.ToUpper() + "_INI1"), (strMeasDesc.ToUpper() + "_INI1"), strTestResultValues[6].Trim(), "9999", "-9999", "9999", "-9999", 0, "NA", ErrorMessage); //just log - always pass

                                    // ini2
                                    if (nStatus == 0)
                                        nStatus = AddLogResult((strMeasDesc.ToUpper() + "_INI2"), (strMeasDesc.ToUpper() + "_INI2"), strTestResultValues[7].Trim(), "9999", "-9999", "9999", "-9999", 0, "NA", ErrorMessage); //just log - always pass

                                    //result1
                                    if (nStatus == 0)
                                        nStatus = AddLogResult((strMeasDesc.ToUpper() + "_RESULT1"), (strMeasDesc.ToUpper() + "_RESULT1"), strTestResultValues[8].Trim(), "9999", "-9999", "9999", "-9999", 0, "NA", ErrorMessage); //just log - always pass

                                    //result2
                                    if (nStatus == 0)
                                        nStatus = AddLogResult((strMeasDesc.ToUpper() + "_RESULT2"), (strMeasDesc.ToUpper() + "_RESULT2"), strTestResultValues[9].Trim(), "9999", "-9999", "9999", "-9999", 0, "NA", ErrorMessage); //just log - always pass

                                    //lna_mode1
                                    if (nStatus == 0)
                                        nStatus = AddLogResult((strMeasDesc.ToUpper() + "_LNA_MODE1"), (strMeasDesc.ToUpper() + "_LNA_MODE1"), strTestResultValues[10].Trim(), "9999", "-9999", "9999", "-9999", 0, "NA", ErrorMessage); //just log - always pass

                                    //lna_mode2
                                    if (nStatus == 0)
                                        nStatus = AddLogResult((strMeasDesc.ToUpper() + "_LNA_MODE2"), (strMeasDesc.ToUpper() + "_LNA_MODE2"), strTestResultValues[11].Trim(), "9999", "-9999", "9999", "-9999", 0, "NA", ErrorMessage); //just log - always pass
                                }
                                else
                                {
                                    nStatus = AddLogResult((strMeasDesc.ToUpper()), (strMeasDesc.ToUpper()), strTestResultValues[7].Trim(), strTestResultValues[10].Trim(), strTestResultValues[9].Trim(), strTestResultValues[10].Trim(), strTestResultValues[9].Trim(), nPassFail, "dBm", ErrorMessage);
                                }
                            }
                            else
                                nStatus = 0;
                        }

                        if (strTestResult.StartsWith("CAL_RES_GSM_W_COEF_REAL_VAL"))
                        {
                            strMeasDesc = strTestResultValues[0];
                            nStatus = 0;

                            for (int nCount = 1; nCount < strTestResultValues.Length; nCount++)
                            {
                                if (strTestResultValues[nCount].Trim().Length > 0)
                                {
                                    if (nStatus == 0)
                                        nStatus = AddLogResult((strMeasDesc.ToUpper() + "_Test_" + nCount), (strMeasDesc.ToUpper() + "_Test_" + nCount), strTestResultValues[nCount].Trim(), "9999", "-9999", "9999", "-9999", 0, "NA", ErrorMessage); //just log - always pass
                                    else
                                        break;
                                }

                            }
                        }

                        if (strTestResult.StartsWith("CAL_RES_GSM_W_COEF_IMAGE_VAL"))
                        {
                            strMeasDesc = strTestResultValues[0];

                            nStatus = 0;

                            for (int nCount = 1; nCount < strTestResultValues.Length; nCount++)
                            {
                                if (strTestResultValues[nCount].Trim().Length > 0)
                                {
                                    if (nStatus == 0)
                                        nStatus = AddLogResult((strMeasDesc.ToUpper() + "_Test_" + nCount), (strMeasDesc.ToUpper() + "_Test_" + nCount), strTestResultValues[nCount].Trim(), "9999", "-9999", "9999", "-9999", 0, "NA", ErrorMessage); //just log - always pass
                                    else break;
                                }
                            }
                        }

                    }
                    else
                        nStatus = 0;

                }

                //Rf Verify Result: Example: NSFT_GSM_TXP,  GSM850, 128, -85, 5, , dBm, 32.548, 32.750, 31.500, 34.000, PASS
                if (strTestResult.StartsWith("NSFT_"))
                {
                    //Create MeasDesc
                    strMeasDesc = strTestResultValues[0] +
                        ((strTestResultValues[1].Trim().Length > 0) ? ("_" + strTestResultValues[1].Trim()) : "") +
                        ((strTestResultValues[2].Trim().Length > 0) ? ("_" + strTestResultValues[2].Trim()) : "") +
                        ((strTestResultValues[3].Trim().Length > 0) ? ("_" + strTestResultValues[3].Trim()) : "") +
                        ((strTestResultValues[4].Trim().Length > 0) ? ("_" + strTestResultValues[4].Trim()) : "") +
                        ((strTestResultValues[5].Trim().Length > 0) ? ("_" + strTestResultValues[5].Trim()) : "");
                    
                    if(strTestResult.Contains("NSFT_LTE_BER_QPSK") || strTestResult.Contains("NSFT_LTETDD_BER_QPSK"))
                    {
                        //Set Pass/Fail flag
                        double dResult;
                        dResult = Convert.ToDouble(strTestResultValues[7].ToString());
                        if (dResult == 0)
                            nPassFail = 0;
                        else 
                            nPassFail = 1;
                    }
                    
                    nStatus = AddLogResult(strMeasDesc.ToUpper(), strMeasDesc.ToUpper(), strTestResultValues[7], strTestResultValues[10], strTestResultValues[9], strTestResultValues[10], strTestResultValues[9], nPassFail, strTestResultValues[6], ErrorMessage);
                }
                if (nStatus!=0)
                {
                    string teste;
                    teste = "1";
                }

            }
            catch (Exception exc)
            {
                string str = exc.Message;
                nStatus = -1;
            }

            return nStatus;
        }

    }
    public class TestSet
    {
        
        private string IpAddr;
        private int PortNumber;
        private IPEndPoint ip = null;
        Socket soket = null;

        public IPEndPoint Ip
        {
            get
            {
                if (ip == null)
                    ip = new IPEndPoint(IPAddress.Parse(this.IpAddr), this.PortNumber);
                return ip;
            }
            set { ip = value; }
        }

        public Socket Soket
        {
            get
            {
                if (soket == null)
                {
                    soket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        if (!soket.Connected)
                            soket.Connect(this.Ip);
                    }
                    //Throw an exception if not connected
                    catch (SocketException e)
                    {
                        Console.WriteLine("Unable to connect to server.");
                        throw new ApplicationException("Instrument at " + this.Ip.Address + ":" + this.Ip.Port + " is not connected");
                    }
                }
                return soket;
            }
            set { soket = value; }
        }
       
        public int ConnectTestSet(string instrIPAddress, int instrPortNo)
        {
            this.IpAddr = instrIPAddress;
            this.PortNumber = instrPortNo;
            int nStatus = -1;            

            if (Soket.Connected)
                nStatus = 0;

            return nStatus;
                //throw new ApplicationException("Instrument at " + this.Ip.Address + ":" + this.Ip.Port + " is not connected");
        }

        
        public int SendCommandToTestSet(string command)
        {
            int nStatus = -1;
            string strTestSetResponse;
            bool bReadFromTestSet = false;

            nStatus = Soket.Send(Encoding.ASCII.GetBytes(command + "\n"));

            nStatus = 0;

            return nStatus;
        }
        
        public string ReadfromTestSet()
        {
            byte[] data = new byte[1024];
            int receivedDataLength = Soket.Receive(data);
            return Encoding.ASCII.GetString(data, 0, receivedDataLength);
        }

        public int Disconnect()
        {
            int nStatus = -1;

            soket.Disconnect(false);
            soket = null;
            
            nStatus = 0;

            return nStatus;
        }

    }
}
