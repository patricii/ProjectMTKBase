using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.IO;
using System.Collections.Specialized;
using System.Runtime.InteropServices;


namespace ateRun
{
    public class clCaseInputPara
    {
        public string strEnable;
        public string strDisc;
        public string strName;
        public string strUnit;
        public string strVal;
        public string strPrompt;
        public int iCurParaIndex;
        public clCaseInputPara()
        {
            strEnable = "";
            strDisc = "";
            strName = "";
            strUnit = "";
            strVal = "";
            strPrompt = "";
        }
    }

    public class clCaseOutputRes
    {
        public string strEnable;
        public string strDisc;
        public string strName;
        public string strOpt;
        public string strUnit;
        public string strVal;
        public string strPrompt;
        public string strLowerLimit;
        public string strUpperLimit;
        public string strCheck;
        public int iCurResIndex;
        public clCaseOutputRes()
        {
            strEnable = "";
            strDisc = "";
            strName = "";
            strUnit = "";
            strVal = "";
            strPrompt = "";
            strLowerLimit = "";
            strUpperLimit = "";
            strCheck = "PASS";
        }
    }

    public class clSeqCase
    {
        public string strEnable;
        public string strDisc;
        public string strName;
        public string strCheck;
        public string strPrompt;
        public string strCostTime;

        public int iCurCaseIndex;
        public int iCurCasePassFail;      //1:PASS 0:FAIL

        public int iResPassNum;
        public int iResFailNum;

        public string strParaNum;
        public List<clCaseInputPara> listInputPara = new List<clCaseInputPara>();

        public string strResNum;
        public List<clCaseOutputRes> listOutputRes = new List<clCaseOutputRes>();

        public clSeqCase()
        {
            iCurCaseIndex = 0;
        }
    }

    public class clCfgMainSetUnit
    {
        public string strIndex;
        public string strEnable;
        public string strDisc;
        public string strName;
        public string strUnit;
        public string strVal;
        public string strPrompt;
        public clCfgMainSetUnit()
        {
            strIndex = "";
            strEnable = "";
            strDisc = "";
            strName = "";
            strUnit = "";
            strVal = "";
            strPrompt = "";
        }
    }

    public class clCfgResSetUnit
    {
        public string strIndex;
        public string strEnable;
        public string strDisc;
        public string strName;
        public string strUnit;
        public string strVal;
        public string strLevel;
        public string strPrompt;
        public string strNameOpt;
        public clCfgResSetUnit()
        {
            strIndex = "";
            strEnable = "";
            strDisc = "";
            strName = "";
            strUnit = "";
            strVal = "";
            strPrompt = "";
        }
    }

    public class clCfgErrSetUnit
    {
        public string strIndex;
        public string strUpload;
        public string strChipErrorCode;
        public string strLenovoErrorCode;
        public string strDiscEng;
        public string strDiscChs;
        public string strEnable;
        public string strName;
        public string strUnit;
        public string strVal;
        public string strPrompt;
        public string strHelpDoc;        

        public clCfgErrSetUnit()
        {
            strIndex = "";
            strEnable = "";
            strName = "";
            strUnit = "";
            strVal = "";
            strPrompt = "";
        }
    }

    public class clResCfgUnit
    {
        public string strIndex;
        public string strModemIndex;
        public int iModemIndex;
        public string strName;
        public string strVal;
        public string strPrompt;
        public clResCfgUnit()
        {
            strIndex = "";
            strModemIndex = "";
            strVal = "";
            strPrompt = "";
        }
    }

    public class clResLogUnit
    {
        public string strIndex;
        public string strModemIndex;
        public int iModemIndex;
        public string strName;
        public string strDisc;
        public string strVal;
        public string strLevel;
        public string strPrompt;
        public string strNameOpt;
        public clResLogUnit()
        {
            strIndex = "";
            strModemIndex = "";
            strVal = "";
            strPrompt = "";
        }
    }

    public class clSumUnit
    {
        public string strIndex;
        public string strLog;
        public clSumUnit()
        {
            strIndex = "";
            strLog = "";
        }
    }

    public class clErrUnit
    {
        public int iErrUpdate;
        public int iErrCode;
        public string strChipErrorCode;
        public string strLenovoErrorCode;
        public string strDiscEng;
        public string strDiscChs;
        public clErrUnit()
        {
            strDiscEng = "";
        }
    }

    public class clLogUnit
    {
        public string strIndex;
        public string strLog;
        public clLogUnit()
        {
            strIndex = "";
            strLog = "";
        }
    }

    public class clRunLogInfo
    {
        
        public List<clResCfgUnit> listCfgResLogWlanTest = new List<clResCfgUnit>();
        public List<clResCfgUnit> listRunCfgKeySet = new List<clResCfgUnit>();
        public List<clResCfgUnit> listCfgResKeySet = new List<clResCfgUnit>();
        public List<clResLogUnit> listRunDataLogRfCalibrate = new List<clResLogUnit>();
        public List<clResCfgUnit> listCfgResLogBbTest = new List<clResCfgUnit>(); // Patricio
        public List<clResLogUnit> listRunResLogBB = new List<clResLogUnit>(); // Patricio
        public List<clResLogUnit> listRunResLogRfCalibrate = new List<clResLogUnit>();
        public List<clResLogUnit> listRunResLogRfVerify = new List<clResLogUnit>();
        public clErrUnit sCurRunErrLog = new clErrUnit();
        public List<clSumUnit> listRunSumLog = new List<clSumUnit>();
        //public List<clLogUnit> listRunTraceLog = new List<clLogUnit>();

        public clRunLogInfo()
        {
            listCfgResLogWlanTest.Clear();
            listCfgResKeySet.Clear();
            listRunResLogRfCalibrate.Clear();
            listRunResLogRfVerify.Clear();
            listRunSumLog.Clear();
            listCfgResLogBbTest.Clear(); // Patricio
            listRunResLogBB.Clear(); // Patricio
            //listRunTraceLog.Clear();
        }
    }


    public class clRunAllInfo
    {
        public int iDeleteMtkLog = 0;
        public int iTestMode;
        public int iLoginDB;
        public int iEnableBbTest = 0; // Patricio
        public int iBbSnOpt = 0; // Patricio
        public string strPhoneType = "Blade2_10W";
        public int iInputSn = 0;
        public int iOnlySn = 0;
        public int iRecodeCalFlag = 0;
        public int iEnableFixtureControl = 0;
        public int iCheckFixtureClosed = 1;
        public int iEnablePlcControl = 0;
        public int iFixtureType = 0;
        public int iEnablePowerSupplyControl = 0;
        public int iEnableRetryMode = 0;
        public int iGuiWidth = 0;
        public int iGuiLeft = 0;
        public int iUnclockModem = 0;
        public string strDutCtrlDll;
        public string strDutCtrlApi;
        public int iDutExeMode;
        public int iDutResetAfterCal = 1;
        public int iDutIndex;
        public int iCurCostTime;
        public bool bCopyFailResult = false;
        public string strCurSumLogFile = "";
        public string strCurResLogFile = "";
        public string strCurTraceLogFile = "";
        public string strAttnResLogFolder = "";
        public int iRunDutCalRepeatIndex = 0;
        public int iRunDutCalRepeatNum = 0;
        public int iRunDutCalDelayTime = 0;
        public int iRunCableCalRepeatIndex = 0;
        public int iRunCableCalRepeatNum = 4;

        public int iModemIndex = -1;
        public int iItemIndex = 0;
        public int iItemNum = 18;
        public int iRunStatus = 0;
        public bool bCalMainDone = false;
        public bool bBbTestDone = false; // Patricio
        public bool bCalEnterMeta = false;
        public bool bPrintSNInfo = false;
        public int iDutPassNum = 0;
        public int iDutFailNum = 0;
        public int iRunRetryIndex = 0;
        public int iRunRetryNum = 1;
        public bool bFirstRun = true;
        public double dDutFailRate = 0.00;

        public bool bRunCableCalPassed = false;
        public bool bRunDutCalPassed = false;

        public string strCurTesterType;
        public string strCurTesterMode;
        public string strCurTesterSN;
        public string strCurTesterFW;
        public string strCurRunPassFail = "INIT";
        public string strInputDutSerialNumber;
        public string strRetDutSerialNumber;
        public string strLogDutSerialNumber;
        //public string strAteVersion = "1612";
        public string strAteVersion = "LACROSSEBZ";
        public string strToolVersion = "v1.1"; //  Patricio
        public int iDutGroup0Num;
        public int iDutGroup1Num;
        public int iDutGroup2Num;
        public int iDutGroup3Num;
        public string[] straryAllDutPortModeSupport = new string[4];
        //public string strCurDutPortMode;
        public int iDutPreloaderComPort;
        public int iDutGadgetComPort;
        public int iCurDutGroupIndex;
        public int iAllDutGroupNum;
        //public int iAllDutGroupNum;
        public int iStart;
        public int iTotalRunAllNum;
        public string strLoginDb = "";
        public string strConnectDb = "";
        public string strDbAccount = "xxh";
        public string strDbPassword = "0";
        public string strDbName = "";
        public string strDbRank = "";
        public string strDbDatabaseName = "";
        public string strDbGroup = "";
        public string strDbPcName = "LENOVO-PC";
        public string strDbMsType = "";
        public string strDbMsTypeName = "";
        public string strDutSerialPortPreBaudRate = "115200";
        public string strDutSerialPortCurBaudRate = "115200";
        public string strDutSerialPortDataBits = "";
        public string strDutSerialPortReadTimeout = "";
        public string strDutSerialPortWriteTimeout = "";
        public string strDutSerialPortStopBits = "";
        public string strDutSerialPortParity = "";
        public string strDutSerialPortEncoding = "";
        public string strSupportSwVersion = "";
        public string strAtCmdLenovoGsn = "";
        public string strAtCmdLenovoCheckVersion = "";
        public string strAtCmdLenovoSetTestBaudRate = "";
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
        public string strDutGroup0Port0EattFilePath = "";
        public string strDutGroup0Port1EattFilePath = "";
        public string strDutGroup0Port2EattFilePath = "";
        public string strDutGroup0Port3EattFilePath = "";
        public string strDutGroup1Port0EattFilePath = "";
        public string strDutGroup1Port1EattFilePath = "";
        public string strDutGroup1Port2EattFilePath = "";
        public string strDutGroup1Port3EattFilePath = "";
        public string strDutGroup2Port0EattFilePath = "";
        public string strDutGroup2Port1EattFilePath = "";
        public string strDutGroup2Port2EattFilePath = "";
        public string strDutGroup2Port3EattFilePath = "";
        public string strDutGroup3Port0EattFilePath = "";
        public string strDutGroup3Port1EattFilePath = "";
        public string strDutGroup3Port2EattFilePath = "";
        public string strDutGroup3Port3EattFilePath = "";
        public string strDutGroup0Port0ExeFilePath = "";
        public string strDutGroup0Port1ExeFilePath = "";
        public string strDutGroup0Port2ExeFilePath = "";
        public string strDutGroup0Port3ExeFilePath = "";
        public string strDutGroup1Port0ExeFilePath = "";
        public string strDutGroup1Port1ExeFilePath = "";
        public string strDutGroup1Port2ExeFilePath = "";
        public string strDutGroup1Port3ExeFilePath = "";
        public string strDutGroup2Port0ExeFilePath = "";
        public string strDutGroup2Port1ExeFilePath = "";
        public string strDutGroup2Port2ExeFilePath = "";
        public string strDutGroup2Port3ExeFilePath = "";
        public string strDutGroup3Port0ExeFilePath = "";
        public string strDutGroup3Port1ExeFilePath = "";
        public string strDutGroup3Port2ExeFilePath = "";
        public string strDutGroup3Port3ExeFilePath = "";
        public string strDutGroup0Port0ExeWorkPath = "";
        public string strDutGroup0Port1ExeWorkPath = "";
        public string strDutGroup0Port2ExeWorkPath = "";
        public string strDutGroup0Port3ExeWorkPath = "";
        public string strDutGroup1Port0ExeWorkPath = "";
        public string strDutGroup1Port1ExeWorkPath = "";
        public string strDutGroup1Port2ExeWorkPath = "";
        public string strDutGroup1Port3ExeWorkPath = "";
        public string strDutGroup2Port0ExeWorkPath = "";
        public string strDutGroup2Port1ExeWorkPath = "";
        public string strDutGroup2Port2ExeWorkPath = "";
        public string strDutGroup2Port3ExeWorkPath = "";
        public string strDutGroup3Port0ExeWorkPath = "";
        public string strDutGroup3Port1ExeWorkPath = "";
        public string strDutGroup3Port2ExeWorkPath = "";
        public string strDutGroup3Port3ExeWorkPath = "";
        public string strDutGroup0Port0CfgFilePath = "";
        public string strDutGroup0Port1CfgFilePath = "";
        public string strDutGroup0Port2CfgFilePath = "";
        public string strDutGroup0Port3CfgFilePath = "";
        public string strDutGroup1Port0CfgFilePath = "";
        public string strDutGroup1Port1CfgFilePath = "";
        public string strDutGroup1Port2CfgFilePath = "";
        public string strDutGroup1Port3CfgFilePath = "";
        public string strDutGroup2Port0CfgFilePath = "";
        public string strDutGroup2Port1CfgFilePath = "";
        public string strDutGroup2Port2CfgFilePath = "";
        public string strDutGroup2Port3CfgFilePath = "";
        public string strDutGroup3Port0CfgFilePath = "";
        public string strDutGroup3Port1CfgFilePath = "";
        public string strDutGroup3Port2CfgFilePath = "";
        public string strDutGroup3Port3CfgFilePath = "";
        public string strDutGroup0Port0BbExeFilePath = ""; // Patricio
        public string strDutGroup1Port0BbExeFilePath = ""; // Patricio
        public string strDutGroup2Port0BbExeFilePath = ""; // Patricio
        public string strDutGroup3Port0BbExeFilePath = ""; // Patricio
        public string strDutGroup0Port0BbExeWorkPath = ""; // Patricio
        public string strDutGroup1Port0BbExeWorkPath = ""; // Patricio
        public string strDutGroup2Port0BbExeWorkPath = ""; // Patricio
        public string strDutGroup3Port0BbExeWorkPath = ""; // Patricio
        public string strDutGroup0Port0BbCfgFilePath = ""; // Patricio
        public string strDutGroup1Port0BbCfgFilePath = ""; // Patricio
        public string strDutGroup2Port0BbCfgFilePath = ""; // Patricio
        public string strDutGroup3Port0BbCfgFilePath = ""; // Patricio
        public string strDutGroup0Port0BbSeqFilePath = ""; // Patricio
        public string strDutGroup1Port0BbSeqFilePath = ""; // Patricio
        public string strDutGroup2Port0BbSeqFilePath = ""; // Patricio
        public string strDutGroup3Port0BbSeqFilePath = ""; // Patricio
        public string strAttnSourceCfgFile1 = "";
        public string strAttnSourceCfgFile2 = "";
        public string strAttnTargetCfgFile1 = "";
        public string strAttnTargetCfgFile2 = "";
        public int iEnableAttnSourceCfgFile1 = 0;
        public int iEnableAttnSourceCfgFile2 = 0;
        public int iEnableAttnTargetCfgFile1 = 0;
        public int iEnableAttnTargetCfgFile2 = 0;
        public int iEnablePingPang = 0;
        public int iTimeoutPingPang = 300;
        public string strRetDutSwVersion = "";
        public int iRemoteOption;
        public int iPrintOption;
        public int iRepeatTime;
        public int iRunTimeout;
        public int iDutConnectTimeout;
        public int iComPortConnectTimeout;
        public int iRepeatModeOpt;        
        public int iFixtureCloseModeOpt;
        public int iEnableRfSwitch;
        public string strRfSwitchPort;
        public string strRfSwitchCom;
        public int iStopModeOpt;
        public int iStopOpt;
        public int iCurDutPortMode;
        public int iLanguageOpt;
        public int iLogErrCode;
        public int iDesignType;           //0:lenovo|1:longcheer|2:huaqin
        public string strResCfgFile = "";
        public string strErrCfgFile = "";
        public int iSerialMode;
        public string strHwIdValue;
        public string strHwIdFile;
        public string strHwIdMode;
        public string strHwIdDefault;
        public int iDebugMode;
        public int iLogMode;
        public int iInstrStatus;
        public int iFailDutNum;
        public string strErrMsg;
        public string strIntelTestItemCalMain;
        public string strIntelTestItemCalRd;
        public string strIntelTestItemRfVerify;
        public string strCfgFileGroup0Port0CalMain;
        public string strCfgFileGroup0Port0CalRd;
        public string strCfgFileGroup0Port0RfVerify;
        public string strCfgFileGroup1Port0CalMain;
        public string strCfgFileGroup1Port0CalRd;
        public string strCfgFileGroup1Port0RfVerify;
        public string strCfgFileGroup2Port0CalMain;
        public string strCfgFileGroup2Port0CalRd;
        public string strCfgFileGroup2Port0RfVerify;
        public string strCfgFileGroup3Port0CalMain;
        public string strCfgFileGroup3Port0CalRd;
        public string strCfgFileGroup3Port0RfVerify;
        public string strIntelLogFolder;
        public string strLenovoLogFolder;
        public string strStepLogStampCalFbrBegin;
        public string strStepLogStampCalFbrEnd;
        public string strStepLogStampCalFastFbrBegin;
        public string strStepLogStampCalFastFbrEnd;
        public string strStepLogStampCalAfcBegin;
        public string strStepLogStampCalAfcEnd;
        public string strStepLogStampCal2gRxBegin;
        public string strStepLogStampCal2gRxEnd;
        public string strStepLogStampCal2gTxBegin;
        public string strStepLogStampCal2gTxEnd;
        public string strStepLogStampCal2gFastBegin;
        public string strStepLogStampCal2gFastEnd;
        public string strStepLogStampCal3gRxBegin;
        public string strStepLogStampCal3gRxEnd;
        public string strStepLogStampCal3gTxBegin;
        public string strStepLogStampCal3gTxEnd;
        public string strStepLogStampCal3gFastBegin;
        public string strStepLogStampCal3gFastEnd;
        public string strStepLogStampCal4gRxBegin;
        public string strStepLogStampCal4gRxEnd;
        public string strStepLogStampCal4gTxBegin;
        public string strStepLogStampCal4gTxEnd;
        public string strStepLogStampCal4gFastBegin;
        public string strStepLogStampCal4gFastEnd;
        public string strSumLogStampCalBegin;
        public string strSumLogStampCalEnd;
        public string strSumLogStampRfVerifyBegin;
        public string strSumLogStampRfVerifyEnd;
        public int iCurAllItemNum = 1;
        public int iCurTestItemIndex = 0;
        public int iStepNumVerify = 3;
        public int iStepNumCal = 3;
        public double dCurAttnMinValue = 7.00;
        public double dCurAttnMaxValue = 10.00;
        public double dGsmBandDiffMaxValue = 1.00;
        public double dDeltaMaxValue = 0.50;

        public int iEnableGsm;
        public int iEnableWcdma;
        public int iEnableLte;
        public int iEnableWlan;
        public int iEnableBt;
        public int iEnableGps;

        public string strPreSeqCmd;
        public string strCurSeqCmd;

        public int iSimMode;
        public string[] straryEattPort = new string[4];
        public string[] straryCurEattPort = new string[4];
        public string[] straryCurDutEnable = new string[4];
        public string[] straryCurDutComPort = new string[4];
        public string[] straryCurFixturePort = new string[4];
        public string[] straryCurDutRfPort = new string[4];
        public string strGroup0EattPort = "";
        public string strGroup0DutComPort = "";
        public string strGroup0FixturePort = "";
        public string strGroup1EattPort = "";
        public string strGroup1DutComPort = "";
        public string strGroup1FixturePort = "";
        public string strGroup2EattPort = "";
        public string strGroup2DutComPort = "";
        public string strGroup2FixturePort = "";
        public string strGroup3EattPort = "";
        public string strGroup3DutComPort = "";
        public string strGroup3FixturePort = "";
        public string strCurDutComPort;
        public string strCurFixtureComPort;
        public string strCurExeFilePath = "";
        public string strCurExeWorkPath = "";
        public string strCurCfgFilePath = "";
        public string strCurEattFilePath = "";
        public string strCurBbExeFileName = ""; //Patricio
        public string strCurBbExeFilePath = ""; //Patricio
        public string strCurBbExeWorkPath = ""; //Patricio
        public string strCurBbCfgFilePath = ""; //Patricio
        public string strCurBbSeqFilePath = ""; //Patricio
        public string strApdbFile = ""; //Patricio
        public string str1stDbFile = ""; //Patricio
        public int iCurCalFileIndex = 0;
        public int iCurCalFileNum = 0;
        public int iCurCfgFileIndex = 0;
        public int iCurCfgFileNum = 0;
        public int iModemNum = 0;
        public string[] straryCurCalFilePath = new string[4];
        public string[] straryCurCfgFilePath = new string[4];
        public string strCurDutGroup0Port0ExeFilePath;
        public string strCurDutGroup1Port0ExeFilePath;
        public string strCurDutGroup2Port0ExeFilePath;
        public string strCurDutGroup3Port0ExeFilePath;
       
        public string strDutRfPort;
        public string strDutGroup0;
        public string strDutGroup1;
        public string strDutGroup2;
        public string strDutGroup3;
        public int iConnectMode;
        public string[] straryAllTesterType = new string[4];
        public string[] straryAllTesterPort = new string[4];
        public string[] straryAllPowerSupplyType = new string[4];
        public string[] straryAllSupportResLevelType = new string[4];
        public int iRunMode = 0;
        public string strCurTesterPort;
        public string strCurPowerSupplyType;
        public string strCurTesterAddr;
        public string strCurPowerSupplyAddr;
        public int iCurPowerChanIndex;
        public double dCurCh0PowerVoltage;
        public double dCurCh0PowerCurrent;
        public double dCurCh1PowerVoltage;
        public double dCurCh1PowerCurrent;

        //public string strDutRfPort;
        //public string strDutGroup0;
        //public string strDutGroup1;
        //public string strDutGroup2;
        //public string strDutGroup3;
        //public int iConnectMode;
        public string strConnectAddr;
        public string strCmwCtrlDll;
        public string strCmwCtrlApi;
        public int iCmwExeMode;
        public string strVersionInfo;
        public string strLogFile;
        public string strCurResLevel;
        public int iLogCalRes;
        public int iLogCalData;
        public int iLogCfgSet;
        public int iLogCfgLoss;
        public int iLogMeasRes;
        public int iLogBbRes; // Patricio
        public int iCurChanIndex;
        public string strCurInstrEattMode;
        public string strPreInstrEattMode;
        public string strCurIntrEattTx1;
        public string strCurIntrEattTx2;
        public string strCurIntrEattTx3;
        public string strCurIntrEattTx4;
        public string strCurIntrEattRx1;
        public string strCurIntrEattRx2;
        public string strCurIntrEattRx3;
        public string strCurIntrEattRx4;
        public string strPreIntrEattTx1;
        public string strPreIntrEattTx2;
        public string strPreIntrEattTx3;
        public string strPreIntrEattTx4;
        public string strPreIntrEattRx1;
        public string strPreIntrEattRx2;
        public string strPreIntrEattRx3;
        public string strPreIntrEattRx4;
        public string strPreInstrPort;
        public bool bIsConnectCMW;

        public DateTime dtTotalRunBegin;
        public DateTime dtTotalRunEnd;
        public DateTime dtCurrentRunBegin;
        public DateTime dtCurrentRunEnd;
        public TimeSpan tsRunBegin;
        public TimeSpan tsRunEnd;
        public TimeSpan tsRunCost;

        public int iCalRdStatus = 2;                                 //0: no start  1:wait for serial port 2:find serial port 3: started, still going 4: finished
        public int iRfVerifyStatus = 2;                              //0: no start  1:wait for serial port 2:find serial port 3: started, still going 4: finished 5: skipped
        public bool bCalRdFail = false;
        public bool bRfVerifyFail = false;
        public bool bCalRdDone = false;
        public bool bRfVerifyDone = false;
        public bool bShowSumLog = false;

        public string strIntelCfgFileCalMain = "";
        public string strIntelCfgFileCalRd = "";
        public string strIntelCfgFileRfVerify = "";

        public int iRestartATEDemo = 0;
        public int iContinueFailCnt = 0;
        public int iContinueFailMax = 5;

        public int iATERebootEveryTime = 0;

        public string strCalBandsConfigInfo = "";

        public clRunAllInfo()
        {

        }
    }

    public class clRunDutInfo
    {
        public int iTestMode;
        public int iDutIndex;
        public int iDutNum;

        public string strCheck;

        public string strPreSeqCmd;
        public string strCurSeqCmd;

        public int iConnectMode;

        public string strLogFile;
        public int iDutStatus;

        public clRunDutInfo()
        {

        }
    }

    public class clMtkCfgTxtFile
    {
        public string strIndex;
        public string strFile;
        public clMtkCfgTxtFile()
        {
            strIndex = "";
            strFile = "";
        }
    }


    public class clCfgFileUnit
    {
        public string strIndex;
        public string strLine;
        public clCfgFileUnit()
        {
            strIndex = "";
            strLine = "";
        }
    }

    public class clCfgGolenDutUnit
    {
        public string strIndex;
        public string strSN;
        public clCfgGolenDutUnit()
        {
            strIndex = "";
            strSN = "";
        }
    }

    public class clCfgCtrlSet
    {
        public string strMainCfgFile = "";
        public clRunAllInfo sRunAllInfo = new clRunAllInfo();
        public clRunLogInfo sRunLogInfo = new clRunLogInfo();
        public List<clCfgMainSetUnit> listMainComSet = new List<clCfgMainSetUnit>();
        public List<clCfgMainSetUnit> listMainInstrSet = new List<clCfgMainSetUnit>();
        public List<clCfgMainSetUnit> listMainDutSet = new List<clCfgMainSetUnit>();
        public List<clCfgMainSetUnit> listMainSeqSet = new List<clCfgMainSetUnit>();
        public List<clCfgMainSetUnit> listMainSumSet = new List<clCfgMainSetUnit>();
        public List<clCfgErrSetUnit> listErrComSet = new List<clCfgErrSetUnit>();
        public List<clCfgResSetUnit> listResComSet = new List<clCfgResSetUnit>();
        public List<clCfgResSetUnit> listResCfgSet = new List<clCfgResSetUnit>();
        public List<clCfgResSetUnit> listResMeasSet = new List<clCfgResSetUnit>();
        public List<clCfgResSetUnit> listResCalSet = new List<clCfgResSetUnit>();
        public List<clCfgResSetUnit> listResBbSet = new List<clCfgResSetUnit>(); // Patricio

        public List<clMtkCfgTxtFile> listMtkCfgTxtFile = new List<clMtkCfgTxtFile>();
        //public List<clCfgFileUnit> listOrgMtkCustomerSetupTxt = new List<clCfgFileUnit>();
        //public List<clCfgFileUnit> listNewMtkCustomerSetupTxt = new List<clCfgFileUnit>();
        //public List<clCfgFileUnit> listOrgMtkSetupIni = new List<clCfgFileUnit>();
        //public List<clCfgFileUnit> listNewMtkSetupIni = new List<clCfgFileUnit>();
        public List<clCfgFileUnit> listOrgMtkCfg = new List<clCfgFileUnit>();
        //public List<clCfgFileUnit> listNewMtkCfg = new List<clCfgFileUnit>();
        //public List<clCfgFileUnit> listOrgAttnIni = new List<clCfgFileUnit>();
        //public List<clCfgFileUnit> listNewAttnIni = new List<clCfgFileUnit>();
        public List<clCfgGolenDutUnit> listGoldenDut = new List<clCfgGolenDutUnit>();

        public clCfgCtrlSet()
        {
            listMainComSet.Clear();
            listMainInstrSet.Clear();
            listMainDutSet.Clear();
            listMainSeqSet.Clear();
            listMainSumSet.Clear();
            listErrComSet.Clear();
            listResComSet.Clear();
            listResCfgSet.Clear();
            listResMeasSet.Clear();
            listResCalSet.Clear();
            listResBbSet.Clear(); // Patricio
        }
    }


    class clCfgCtrlRun
    {
        #region API函数声明

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        #endregion


        private const string STR_CFG = "cfg";
        private const string STR_SETTING = "setting";
        private const string STR_SET_COM = "com_setting";
        private const string STR_SET_INSTR = "instr_setting";
        private const string STR_SET_DUT = "dut_setting";
        private const string STR_SET_SUM = "sum_setting";
        private const string STR_SET_SEQ = "seq_setting";
        private const string STR_SET_ITEM = "item";
        private const string STR_CASE = "case";
        private const string STR_RETRY = "retry";
        private const string STR_ENABLE = "enable";
        private const string STR_DISC = "disc";
        private const string STR_NAME = "name";
        private const string STR_OPT = "opt";
        private const string STR_LEVEL = "level";
        private const string STR_UNIT = "unit";
        private const string STR_VALUE = "value";
        private const string STR_PROMPT = "prompt";
        private const string STR_NAME_OPT = "name_opt";
        private const string STR_LOWER_LIMIT = "lower_limit";
        private const string STR_UPPER_LIMIT = "upper_limit";
        private const string STR_OUTPUT_RES = "output_res";
        private const string STR_INPUT_PARA = "input_para";

        private const string STR_RUN = "run";
        private const string STR_SEQ_FILE = "file";
        private const string STR_SEQ = "test_seqence";

        private const string STR_SET_RES_CFG_COM = "com_setting";
        private const string STR_SET_RES_CFG_TEST = "test_cfg_setting";
        private const string STR_SET_RES_BB = "bb_res_setting"; // Patricio
        private const string STR_SET_RES_CAL = "cal_res_setting";
        private const string STR_SET_RES_MEAS = "meas_res_setting";

        private const string STR_ERR_SET = "err_setting";
        private const string STR_ERR_UPLOAD = "upload";
        private const string STR_ERR_CHIP_ERROR_CODE = "chip_error_code";
        private const string STR_ERR_LENOVO_ERROR_CODE = "lenovo_error_code";
        private const string STR_ERR_DISC_ENG = "disc_eng";
        private const string STR_ERR_DISC_CHS = "disc_chs";
        private const string STR_ERR_HELP_DOC = "helpdoc";

        public int LoadMainCfgFile(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;
            try
            {
                XmlDocument docSetup = new XmlDocument();
                docSetup.Load(sCurCfgCtrlSet.strMainCfgFile);
                XmlElement rootNode = docSetup.DocumentElement;
                foreach (XmlNode nodes_layer1 in rootNode.ChildNodes)
                {
                    if (nodes_layer1.Name == STR_SETTING)
                    {
                        foreach (XmlNode nodes_layer2 in nodes_layer1)
                        {
                            if (nodes_layer2.Name == STR_SET_COM)
                            {
                                foreach (XmlNode nodes_layer3 in nodes_layer2)
                                {
                                    clCfgMainSetUnit sCfgSet = new clCfgMainSetUnit();
                                    if (nodes_layer3.Name == STR_SET_ITEM)
                                    {
                                        foreach (XmlNode nodes_layer4 in nodes_layer3)
                                        {
                                            switch (nodes_layer4.Name)
                                            {
                                                case STR_ENABLE:
                                                    sCfgSet.strEnable = nodes_layer4.InnerText;
                                                    break;
                                                case STR_DISC:
                                                    sCfgSet.strDisc = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME:
                                                    sCfgSet.strName = nodes_layer4.InnerText;
                                                    break;
                                                case STR_VALUE:
                                                    sCfgSet.strVal = nodes_layer4.InnerText;
                                                    break;
                                                case STR_UNIT:
                                                    sCfgSet.strUnit = nodes_layer4.InnerText;
                                                    break;
                                                case STR_PROMPT:
                                                    sCfgSet.strPrompt = nodes_layer4.InnerText;
                                                    break;
                                            }
                                        }
                                        sCurCfgCtrlSet.listMainComSet.Add(sCfgSet);
                                    }
                                }

                            }
                            else if (nodes_layer2.Name == STR_SET_INSTR)
                            {
                                foreach (XmlNode nodes_layer3 in nodes_layer2)
                                {
                                    clCfgMainSetUnit sCfgSet = new clCfgMainSetUnit();
                                    if (nodes_layer3.Name == STR_SET_ITEM)
                                    {
                                        foreach (XmlNode nodes_layer4 in nodes_layer3)
                                        {
                                            switch (nodes_layer4.Name)
                                            {
                                                case STR_ENABLE:
                                                    sCfgSet.strEnable = nodes_layer4.InnerText;
                                                    break;
                                                case STR_DISC:
                                                    sCfgSet.strDisc = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME:
                                                    sCfgSet.strName = nodes_layer4.InnerText;
                                                    break;
                                                case STR_VALUE:
                                                    sCfgSet.strVal = nodes_layer4.InnerText;
                                                    break;
                                                case STR_UNIT:
                                                    sCfgSet.strUnit = nodes_layer4.InnerText;
                                                    break;
                                                case STR_PROMPT:
                                                    sCfgSet.strPrompt = nodes_layer4.InnerText;
                                                    break;
                                            }
                                        }
                                        sCurCfgCtrlSet.listMainInstrSet.Add(sCfgSet);
                                    }
                                }
                            }
                            else if (nodes_layer2.Name == STR_SET_DUT)
                            {
                                foreach (XmlNode nodes_layer3 in nodes_layer2)
                                {
                                    clCfgMainSetUnit sCfgSet = new clCfgMainSetUnit();
                                    if (nodes_layer3.Name == STR_SET_ITEM)
                                    {
                                        foreach (XmlNode nodes_layer4 in nodes_layer3)
                                        {
                                            switch (nodes_layer4.Name)
                                            {
                                                case STR_ENABLE:
                                                    sCfgSet.strEnable = nodes_layer4.InnerText;
                                                    break;
                                                case STR_DISC:
                                                    sCfgSet.strDisc = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME:
                                                    sCfgSet.strName = nodes_layer4.InnerText;
                                                    break;
                                                case STR_VALUE:
                                                    sCfgSet.strVal = nodes_layer4.InnerText;
                                                    break;
                                                case STR_UNIT:
                                                    sCfgSet.strUnit = nodes_layer4.InnerText;
                                                    break;
                                                case STR_PROMPT:
                                                    sCfgSet.strPrompt = nodes_layer4.InnerText;
                                                    break;
                                            }
                                        }
                                        sCurCfgCtrlSet.listMainDutSet.Add(sCfgSet);
                                    }
                                }
                            }
                            else if (nodes_layer2.Name == STR_SET_SUM)
                            {
                                foreach (XmlNode nodes_layer3 in nodes_layer2)
                                {
                                    clCfgMainSetUnit sCfgSet = new clCfgMainSetUnit();
                                    if (nodes_layer3.Name == STR_SET_ITEM)
                                    {
                                        foreach (XmlNode nodes_layer4 in nodes_layer3)
                                        {
                                            switch (nodes_layer4.Name)
                                            {
                                                case STR_ENABLE:
                                                    sCfgSet.strEnable = nodes_layer4.InnerText;
                                                    break;
                                                case STR_DISC:
                                                    sCfgSet.strDisc = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME:
                                                    sCfgSet.strName = nodes_layer4.InnerText;
                                                    break;
                                                case STR_VALUE:
                                                    sCfgSet.strVal = nodes_layer4.InnerText;
                                                    break;
                                                case STR_UNIT:
                                                    sCfgSet.strUnit = nodes_layer4.InnerText;
                                                    break;
                                                case STR_PROMPT:
                                                    sCfgSet.strPrompt = nodes_layer4.InnerText;
                                                    break;
                                            }
                                        }
                                        sCurCfgCtrlSet.listMainSumSet.Add(sCfgSet);
                                    }
                                }
                            }
                            else if (nodes_layer2.Name == STR_SET_SEQ)
                            {
                                foreach (XmlNode nodes_layer3 in nodes_layer2)
                                {
                                    clCfgMainSetUnit sCfgSet = new clCfgMainSetUnit();
                                    if (nodes_layer3.Name == STR_SET_ITEM)
                                    {
                                        foreach (XmlNode nodes_layer4 in nodes_layer3)
                                        {
                                            switch (nodes_layer4.Name)
                                            {
                                                case STR_ENABLE:
                                                    sCfgSet.strEnable = nodes_layer4.InnerText;
                                                    break;
                                                case STR_DISC:
                                                    sCfgSet.strDisc = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME:
                                                    sCfgSet.strName = nodes_layer4.InnerText;
                                                    break;
                                                case STR_VALUE:
                                                    sCfgSet.strVal = nodes_layer4.InnerText;
                                                    break;
                                                case STR_UNIT:
                                                    sCfgSet.strUnit = nodes_layer4.InnerText;
                                                    break;
                                                case STR_PROMPT:
                                                    sCfgSet.strPrompt = nodes_layer4.InnerText;
                                                    break;
                                            }
                                        }
                                        sCurCfgCtrlSet.listMainSeqSet.Add(sCfgSet);
                                    }
                                }
                            }
                        }
                    }
                }

                iStatus = InitMainCfgSet(sCurCfgCtrlSet);
                if (iStatus != 0)
                {
                    return iStatus;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "LoadMainCfgFile Failed!");
                iStatus = -1;
            }
            return iStatus;
        }

        public int LoadResCfgFile(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;
            try
            {
                XmlDocument docSetup = new XmlDocument();
                docSetup.Load(sCurCfgCtrlSet.sRunAllInfo.strResCfgFile);
                XmlElement rootNode = docSetup.DocumentElement;
                foreach (XmlNode nodes_layer1 in rootNode.ChildNodes)
                {
                    if (nodes_layer1.Name == STR_SETTING)
                    {
                        foreach (XmlNode nodes_layer2 in nodes_layer1)
                        {
                            if (nodes_layer2.Name == STR_SET_RES_CFG_COM)
                            {
                                foreach (XmlNode nodes_layer3 in nodes_layer2)
                                {
                                    clCfgResSetUnit sCfgSet = new clCfgResSetUnit();
                                    if (nodes_layer3.Name == STR_SET_ITEM)
                                    {
                                        foreach (XmlNode nodes_layer4 in nodes_layer3)
                                        {
                                            switch (nodes_layer4.Name)
                                            {
                                                case STR_ENABLE:
                                                    sCfgSet.strEnable = nodes_layer4.InnerText;
                                                    break;
                                                case STR_DISC:
                                                    sCfgSet.strDisc = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME:
                                                    sCfgSet.strName = nodes_layer4.InnerText;
                                                    break;
                                                case STR_VALUE:
                                                    sCfgSet.strVal = nodes_layer4.InnerText;
                                                    break;
                                                case STR_UNIT:
                                                    sCfgSet.strUnit = nodes_layer4.InnerText;
                                                    break;
                                                case STR_PROMPT:
                                                    sCfgSet.strPrompt = nodes_layer4.InnerText;
                                                    break;
                                            }
                                        }
                                        sCurCfgCtrlSet.listResComSet.Add(sCfgSet);
                                    }
                                }

                            }
                            else if (nodes_layer2.Name == STR_SET_RES_CFG_TEST)
                            {
                                foreach (XmlNode nodes_layer3 in nodes_layer2)
                                {
                                    clCfgResSetUnit sCfgSet = new clCfgResSetUnit();
                                    if (nodes_layer3.Name == STR_SET_ITEM)
                                    {
                                        foreach (XmlNode nodes_layer4 in nodes_layer3)
                                        {
                                            switch (nodes_layer4.Name)
                                            {
                                                case STR_ENABLE:
                                                    sCfgSet.strEnable = nodes_layer4.InnerText;
                                                    break;
                                                case STR_DISC:
                                                    sCfgSet.strDisc = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME:
                                                    sCfgSet.strName = nodes_layer4.InnerText;
                                                    break;
                                                case STR_VALUE:
                                                    sCfgSet.strVal = nodes_layer4.InnerText;
                                                    break;
                                                case STR_UNIT:
                                                    sCfgSet.strUnit = nodes_layer4.InnerText;
                                                    break;
                                                case STR_PROMPT:
                                                    sCfgSet.strPrompt = nodes_layer4.InnerText;
                                                    break;
                                            }
                                        }
                                        sCurCfgCtrlSet.listResCfgSet.Add(sCfgSet);
                                    }
                                }
                            }

                                //BB  Patricio
                            else if (nodes_layer2.Name == STR_SET_RES_BB)
                            {
                                foreach (XmlNode nodes_layer3 in nodes_layer2)
                                {
                                    clCfgResSetUnit sCfgSet = new clCfgResSetUnit();
                                    if (nodes_layer3.Name == STR_SET_ITEM)
                                    {
                                        foreach (XmlNode nodes_layer4 in nodes_layer3)
                                        {
                                            switch (nodes_layer4.Name)
                                            {
                                                case STR_ENABLE:
                                                    sCfgSet.strEnable = nodes_layer4.InnerText;
                                                    break;
                                                case STR_DISC:
                                                    sCfgSet.strDisc = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME:
                                                    sCfgSet.strName = nodes_layer4.InnerText;
                                                    break;
                                                case STR_VALUE:
                                                    sCfgSet.strVal = nodes_layer4.InnerText;
                                                    break;
                                                case STR_UNIT:
                                                    sCfgSet.strUnit = nodes_layer4.InnerText;
                                                    break;
                                                case STR_LEVEL:
                                                    sCfgSet.strLevel = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME_OPT:
                                                    sCfgSet.strNameOpt = nodes_layer4.InnerText;
                                                    break;
                                                case STR_PROMPT:
                                                    sCfgSet.strPrompt = nodes_layer4.InnerText;
                                                    break;
                                            }
                                        }
                                        sCurCfgCtrlSet.listResBbSet.Add(sCfgSet);
                                    }
                                }
                            }
                                //BB end Patricio

                            else if (nodes_layer2.Name == STR_SET_RES_CAL)
                            {
                                foreach (XmlNode nodes_layer3 in nodes_layer2)
                                {
                                    clCfgResSetUnit sCfgSet = new clCfgResSetUnit();
                                    if (nodes_layer3.Name == STR_SET_ITEM)
                                    {
                                        foreach (XmlNode nodes_layer4 in nodes_layer3)
                                        {
                                            switch (nodes_layer4.Name)
                                            {
                                                case STR_ENABLE:
                                                    sCfgSet.strEnable = nodes_layer4.InnerText;
                                                    break;
                                                case STR_DISC:
                                                    sCfgSet.strDisc = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME:
                                                    sCfgSet.strName = nodes_layer4.InnerText;
                                                    break;
                                                case STR_VALUE:
                                                    sCfgSet.strVal = nodes_layer4.InnerText;
                                                    break;
                                                case STR_UNIT:
                                                    sCfgSet.strUnit = nodes_layer4.InnerText;
                                                    break;
                                                case STR_LEVEL:
                                                    sCfgSet.strLevel = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME_OPT:
                                                    sCfgSet.strNameOpt = nodes_layer4.InnerText;
                                                    break;
                                                case STR_PROMPT:
                                                    sCfgSet.strPrompt = nodes_layer4.InnerText;
                                                    break;
                                            }
                                        }
                                        sCurCfgCtrlSet.listResCalSet.Add(sCfgSet);
                                    }
                                }
                            }
                            else if (nodes_layer2.Name == STR_SET_RES_MEAS)
                            {
                                foreach (XmlNode nodes_layer3 in nodes_layer2)
                                {
                                    clCfgResSetUnit sCfgSet = new clCfgResSetUnit();
                                    if (nodes_layer3.Name == STR_SET_ITEM)
                                    {
                                        foreach (XmlNode nodes_layer4 in nodes_layer3)
                                        {
                                            switch (nodes_layer4.Name)
                                            {
                                                case STR_ENABLE:
                                                    sCfgSet.strEnable = nodes_layer4.InnerText;
                                                    break;
                                                case STR_DISC:
                                                    sCfgSet.strDisc = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME:
                                                    sCfgSet.strName = nodes_layer4.InnerText;
                                                    break;
                                                case STR_VALUE:
                                                    sCfgSet.strVal = nodes_layer4.InnerText;
                                                    break;
                                                case STR_UNIT:
                                                    sCfgSet.strUnit = nodes_layer4.InnerText;
                                                    break;
                                                case STR_LEVEL:
                                                    sCfgSet.strLevel = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME_OPT:
                                                    sCfgSet.strNameOpt = nodes_layer4.InnerText;
                                                    break;
                                                case STR_PROMPT:
                                                    sCfgSet.strPrompt = nodes_layer4.InnerText;
                                                    break;
                                            }
                                        }
                                        sCurCfgCtrlSet.listResMeasSet.Add(sCfgSet);
                                    }
                                }
                            }
                        }
                    }
                }

                iStatus = InitResCfgSet(sCurCfgCtrlSet);
                if (iStatus != 0)
                {
                    return iStatus;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "LoadResCfgFile Failed!");
                iStatus = -1;
            }
            return iStatus;
        }

        public int LoadErrCfgFile(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;
            try
            {
                XmlDocument docSetup = new XmlDocument();
                docSetup.Load(sCurCfgCtrlSet.sRunAllInfo.strErrCfgFile);
                XmlElement rootNode = docSetup.DocumentElement;
                foreach (XmlNode nodes_layer1 in rootNode.ChildNodes)
                {
                    if (nodes_layer1.Name == STR_SETTING)
                    {
                        foreach (XmlNode nodes_layer2 in nodes_layer1)
                        {
                            if (nodes_layer2.Name == STR_ERR_SET)
                            {
                                foreach (XmlNode nodes_layer3 in nodes_layer2)
                                {
                                    clCfgErrSetUnit sErrSet = new clCfgErrSetUnit();
                                    if (nodes_layer3.Name == STR_SET_ITEM)
                                    {
                                        foreach (XmlNode nodes_layer4 in nodes_layer3)
                                        {
                                            switch (nodes_layer4.Name)
                                            {
                                                case STR_ENABLE:
                                                    sErrSet.strEnable = nodes_layer4.InnerText;
                                                    break;
                                                case STR_ERR_UPLOAD:
                                                    sErrSet.strUpload = nodes_layer4.InnerText;
                                                    break;
                                                case STR_ERR_DISC_ENG:
                                                    sErrSet.strDiscEng = nodes_layer4.InnerText;
                                                    break;
                                                case STR_ERR_DISC_CHS:
                                                    sErrSet.strDiscChs = nodes_layer4.InnerText;
                                                    break;
                                                case STR_ERR_CHIP_ERROR_CODE:
                                                    sErrSet.strChipErrorCode = nodes_layer4.InnerText;
                                                    break;
                                                case STR_ERR_LENOVO_ERROR_CODE:
                                                    sErrSet.strLenovoErrorCode = nodes_layer4.InnerText;
                                                    break;
                                                case STR_ERR_HELP_DOC:
                                                    sErrSet.strHelpDoc = nodes_layer4.InnerText;
                                                    break;
                                                case STR_NAME:
                                                    sErrSet.strName = nodes_layer4.InnerText;
                                                    break;
                                                case STR_VALUE:
                                                    sErrSet.strVal = nodes_layer4.InnerText;
                                                    break;
                                                case STR_PROMPT:
                                                    sErrSet.strPrompt = nodes_layer4.InnerText;
                                                    break;
                                            }
                                        }
                                        sCurCfgCtrlSet.listErrComSet.Add(sErrSet);
                                    }
                                }
                            }
                        }
                    }
                }

                iStatus = InitErrCfgSet(sCurCfgCtrlSet);
                if (iStatus != 0)
                {
                    return iStatus;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "LoadErrCfgFile Failed!");
                iStatus = -1;
            }
            return iStatus;
        }

        public int LoadAllCfgFile(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;
            try
            {
                iStatus = LoadGoldenDutList(sCurCfgCtrlSet);
                if (iStatus != 0)
                {
                    return iStatus;
                }

                iStatus = LoadMainCfgFile(sCurCfgCtrlSet);
                if (iStatus != 0)
                {
                    return iStatus;
                }

                iStatus = LoadResCfgFile(sCurCfgCtrlSet);
                if (iStatus != 0)
                {
                    return iStatus;
                }

                iStatus = LoadErrCfgFile(sCurCfgCtrlSet);
                if (iStatus != 0)
                {
                    return iStatus;
                }

                if (sCurCfgCtrlSet.sRunAllInfo.strHwIdMode == "1")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strHwIdValue = sCurCfgCtrlSet.sRunAllInfo.strHwIdDefault;
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.strHwIdMode == "2")
                {
                    iStatus = LoadHwIdDb(sCurCfgCtrlSet);
                    if (iStatus != 0)
                    {
                        return iStatus;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "LoadAllCfgFile Failed!");
                iStatus = -1;
            }
            return iStatus;
        }

        public int LoadHwIdDb(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;
            try
            {
                string strLine = "";
                StreamReader streamCfgDb = new StreamReader(sCurCfgCtrlSet.sRunAllInfo.strHwIdFile, false);
                if (streamCfgDb != null)
                {
                    while ((strLine = streamCfgDb.ReadLine()) != null)
                    {
                        if (strLine.Contains("HardWareCode"))
                        {
                            string[] straryLineVal = new string[10];
                            straryLineVal = strLine.Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            {
                                sCurCfgCtrlSet.sRunAllInfo.strHwIdValue = straryLineVal[1];
                            }
                        }
                    }
                    if (streamCfgDb != null)
                    {
                        streamCfgDb.Close();
                    }
                }
                if (sCurCfgCtrlSet.sRunAllInfo.strHwIdValue.Length == 2)
                {
                    iStatus = 0;
                }
                else
                {
                    iStatus = 1;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "LoadHwIdDb Failed!");
                iStatus = -1;
            }
            return iStatus;
        }

        public int SaveMainCfgXml(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;
            string strLine = "";
            string strProptMsg = "";
            string strMainCfgFile = string.Format("{0,1}", sCurCfgCtrlSet.strMainCfgFile);
            try
            {
                StreamWriter streamCfgFile = new StreamWriter(strMainCfgFile, false);
                if (streamCfgFile == null)
                {
                    strProptMsg = string.Format("{0}文件不存在!", strMainCfgFile);
                    MessageBox.Show("{}", "SaveMainCfgXml Failed!");
                    return -1;
                }
                streamCfgFile.WriteLine("<?xml version=\"1.0\" encoding=\"gb2312\" ?>");
                streamCfgFile.WriteLine("<!-- main configuration for all testing -->");
                streamCfgFile.WriteLine("<cfg>");
                streamCfgFile.WriteLine("  <setting>");

                streamCfgFile.WriteLine("    <com_setting>");
                foreach (clCfgMainSetUnit sCfgSet in sCurCfgCtrlSet.listMainComSet)
                {
                    if (sCfgSet.strName.Contains("enable_fixture"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iEnableFixtureControl,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("fixture_type"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iFixtureType,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }

                    else if (sCfgSet.strName.Contains("enable_power_supply"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iEnablePowerSupplyControl,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("fixture_close_mode"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iFixtureCloseModeOpt,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }                        
                    else if (sCfgSet.strName.Contains("repeat_mode_opt"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iRepeatModeOpt,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("stop_mode_opt"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iStopModeOpt,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("repeat_num"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatNum,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("delay_time"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iRunDutCalDelayTime,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("attn_min_value"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3:F2}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.dCurAttnMinValue,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("attn_max_value"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3:F2}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.dCurAttnMaxValue,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("attn_gsm_band_diff_max_value"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3:F2}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.dGsmBandDiffMaxValue,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("attn_delta_max_value"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3:F2}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.dDeltaMaxValue,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("attn_meas_num"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatNum,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("restart_ateDemo_enable"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iRestartATEDemo,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("continue_fail_max"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iContinueFailMax,
                            "default 5 max");
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("ate_reboot_every_time"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iATERebootEveryTime,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("language_opt"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCfgSet.strVal,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                }
                streamCfgFile.WriteLine("    </com_setting>");

                streamCfgFile.WriteLine("    <instr_setting>");
                foreach (clCfgMainSetUnit sCfgSet in sCurCfgCtrlSet.listMainInstrSet)
                {
                    if (sCfgSet.strName.Contains("cur_tester_type"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.strCurTesterType,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("cur_tester_port"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.strCurTesterPort,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("tester_connect_addr"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.strCurTesterAddr,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("cur_power_type"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.strCurPowerSupplyType,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("enable_pingpang"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iEnablePingPang,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                       else if (sCfgSet.strName.Contains("timeout_pingpang"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iTimeoutPingPang,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("power_connect_addr"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.strCurPowerSupplyAddr,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("power_chan_index"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iCurPowerChanIndex,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("power_ch0_voltage"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3:F2}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.dCurCh0PowerVoltage,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("power_ch0_current"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3:F2}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.dCurCh0PowerCurrent,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("power_ch1_voltage"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3:F2}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.dCurCh1PowerVoltage,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("power_ch1_current"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3:F2}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.dCurCh1PowerCurrent,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("path_source_cfg_file1"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.strAttnSourceCfgFile1,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("enable_source_cfg_file1"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iEnableAttnSourceCfgFile1,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("path_source_cfg_file2"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.strAttnSourceCfgFile2,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("enable_source_cfg_file2"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iEnableAttnSourceCfgFile2,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("path_target_cfg_file1"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.strAttnTargetCfgFile1,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("enable_target_cfg_file1"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iEnableAttnTargetCfgFile1,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("path_target_cfg_file2"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.strAttnTargetCfgFile2,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("enable_target_cfg_file2"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCurCfgCtrlSet.sRunAllInfo.iEnableAttnTargetCfgFile2,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                            sCfgSet.strEnable,
                            sCfgSet.strDisc,
                            sCfgSet.strName,
                            sCfgSet.strVal,
                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }

                }
                streamCfgFile.WriteLine("    </instr_setting>");

                streamCfgFile.WriteLine("    <dut_setting>");
                foreach (clCfgMainSetUnit sCfgSet in sCurCfgCtrlSet.listMainDutSet)
                {
                    if (sCfgSet.strName.Contains("dut_group0_comport"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCurCfgCtrlSet.sRunAllInfo.strGroup0DutComPort,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("dut_group1_comport"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCurCfgCtrlSet.sRunAllInfo.strGroup1DutComPort,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("dut_group2_comport"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCurCfgCtrlSet.sRunAllInfo.strGroup2DutComPort,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("dut_group3_comport"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCurCfgCtrlSet.sRunAllInfo.strGroup3DutComPort,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("fixture_group0_comport"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCurCfgCtrlSet.sRunAllInfo.strGroup0FixturePort,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("fixture_group1_comport"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCurCfgCtrlSet.sRunAllInfo.strGroup1FixturePort,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("fixture_group2_comport"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCurCfgCtrlSet.sRunAllInfo.strGroup2FixturePort,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("fixture_group3_comport"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCurCfgCtrlSet.sRunAllInfo.strGroup3FixturePort,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("dut_cur_port_mode"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("dut_preloader_com_port"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCurCfgCtrlSet.sRunAllInfo.iDutPreloaderComPort,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("dut_gadget_com_port"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCurCfgCtrlSet.sRunAllInfo.iDutGadgetComPort,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("dut_all_group_num"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCurCfgCtrlSet.sRunAllInfo.iAllDutGroupNum,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else if (sCfgSet.strName.Contains("dut_cur_group_index"))
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }
                    else
                    {
                        strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><value>{3}</value><prompt>{4}</prompt></item>",
                                            sCfgSet.strEnable,
                                            sCfgSet.strDisc,
                                            sCfgSet.strName,
                                            sCfgSet.strVal,
                                            sCfgSet.strPrompt);
                        streamCfgFile.WriteLine(strLine);
                    }

                }
                streamCfgFile.WriteLine("    </dut_setting>");

                streamCfgFile.WriteLine("    <sum_setting>");
                foreach (clCfgMainSetUnit sCfgSet in sCurCfgCtrlSet.listMainSumSet)
                {
                    strLine = string.Format("      <item><enable>{0}</enable><disc>{1}</disc><name>{2}</name><prompt>{3}</prompt></item>",
                        sCfgSet.strEnable,
                        sCfgSet.strDisc,
                        sCfgSet.strName,
                        sCfgSet.strPrompt);
                    streamCfgFile.WriteLine(strLine);
                }
                streamCfgFile.WriteLine("    </sum_setting>");
                streamCfgFile.WriteLine("    <seq_setting>");
                streamCfgFile.WriteLine("    </seq_setting>");
                streamCfgFile.WriteLine("  </setting>");
                streamCfgFile.WriteLine("</cfg>");
                if (streamCfgFile != null)
                {
                    streamCfgFile.Close();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "SaveMainCfgXml Failed!");
                iStatus = -1;
            }

            return iStatus;
        }

        //修改指定section的key的值
        public bool EditIniKey(string filePath, string section, string key, string value)
        {
            bool flag = true;
            try
            {
                if (section.Trim().Length <= 0 || key.Trim().Length <= 0)
                {
                    flag = false;
                }
                else
                {
                    if (WritePrivateProfileString(section, key, value, filePath) == 0)
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public bool ReadIniKey(string filePath, string section, string key, ref string value)
        {
            StringBuilder sbRetCmdMsg = new StringBuilder(1024);
            bool flag = true;
            try
            {
                if (section.Trim().Length <= 0 || key.Trim().Length <= 0)
                {
                    flag = false;
                }
                else
                {
                    if (GetPrivateProfileString(section, key, "", sbRetCmdMsg, 1024, filePath) == 0)
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                        value = sbRetCmdMsg.ToString();
                    }
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public int UpdateMtkCustomerSetupTxt(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;

            try
            {
                string strReadName = "";
                string strReadVal = "";
                string strCurrentDirectory = "";
                string strMtkSetupFile = "";
                strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                strMtkSetupFile = strCurrentDirectory + "\\mtk\\Customer_Setup.txt";

                strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                if (sCurCfgCtrlSet.sRunAllInfo.iRunMode == 0)
                {
                    if (Bz_Handler.CItemListEquip.IsFqaVerify() == 1) //  Patricio FQA Verify Setup
                    {
                        strReadVal = string.Format("\"{0}\\mtk\\cfg\\mode_dut_cal_ver\\MTK_SETUP_Verify.ini\"", strCurrentDirectory);
                    }
                    else
                    strReadVal = string.Format("\"{0}\\mtk\\cfg\\mode_dut_cal_ver\\MTK_SETUP.ini\"", strCurrentDirectory);
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.iRunMode == 1)
                {
                    if (Bz_Handler.CItemListEquip.IsFqaVerify() == 1) //  Patricio FQA Verify Setup
                    {
                        strReadVal = string.Format("\"{0}\\mtk\\cfg\\mode_dut_cal_ver\\MTK_SETUP_Verify.ini\"", strCurrentDirectory);
                    }
                    else
                    strReadVal = string.Format("\"{0}\\mtk\\cfg\\mode_cl_cal\\MTK_SETUP.ini\"", strCurrentDirectory);
                }

                strReadVal = strReadVal.Replace("\\", "\\\\");

                EditIniKey(strMtkSetupFile, "Customer Define Setup", "Test Setup file", strReadVal);
                strReadVal = string.Format("0", sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex);
                EditIniKey(strMtkSetupFile, "Customer Define Setup", "Dut Index", strReadVal);
                if (sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode == 1)
                {
                    EditIniKey(strMtkSetupFile, "Calibration Setup", "COM PORT", "9999");
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode == 2)
                {
                    if (sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort.Length > 3)
                    {
                        strReadVal = sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort;
                        strReadVal = strReadVal.Substring(3, strReadVal.Length - 3);
                        EditIniKey(strMtkSetupFile, "Calibration Setup", "COM PORT", strReadVal);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "UpdateMtkCustomerSetupTxt Failed!");
                iStatus = -1;
            }
            return iStatus;
        }

        public int UpdateMtkSetupIni_FilePath(int iRunMode, string strMtkSetupFile, string strKeyName)
        {
            int iStatus = 0;
            string strFileName = "";
            string strKeyVal = "";
            string strCurrentDirectory = "";
            if (strKeyName.Contains("APBB database file")
                || strKeyName.Contains("Second FDM database file")
                || strKeyName.Contains("Second Calibration file")
                || strKeyName.Contains("Second Config file")
                || strKeyName.Contains("MD2 FDM database file")
                || strKeyName.Contains("MD2 Calibration file")
                || strKeyName.Contains("MD2 Config file")
                || strKeyName.Contains("FDM database file")
                || strKeyName.Contains("Config file")
                || strKeyName.Contains("Report file path")
                || strKeyName.Contains("Calibration file")
                || strKeyName.Contains("C2K Config file")
                || strKeyName.Contains("C2K Calibration file"))
            {
                ReadIniKey(strMtkSetupFile, "System Setting", strKeyName, ref strKeyVal);
                if (strKeyVal.Length > 3)
                {
                    strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                    strFileName = strKeyVal.Substring(strKeyVal.LastIndexOf("\\"), strKeyVal.Length - strKeyVal.LastIndexOf("\\"));
                    strFileName = strFileName.Replace("\"", "");
                    if (iRunMode == 0)
                    {
                        strKeyVal = string.Format("\"{0}\\mtk\\cfg\\mode_dut_cal_ver{1}\"", strCurrentDirectory, strFileName);
                    }
                    else if (iRunMode == 1)
                    {
                        strKeyVal = string.Format("\"{0}\\mtk\\cfg\\mode_cl_cal{1}\"", strCurrentDirectory, strFileName);
                    }
                    strKeyVal = strKeyVal.Replace("\\", "\\\\");
                    EditIniKey(strMtkSetupFile, "System Setting", strKeyName, strKeyVal);
                }
            }

            return iStatus;
        }

        public int UpdateMtkSetupIni_InstrType(string strMtkSetupFile, string strInstrType)
        {
            int iStatus = 0;
            if (strInstrType.Equals("MT8870"))
            {
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument", "\"MT8870\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument WiFi", "\"MT8870\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument WCDMA", "\"MT8870\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument LTE", "\"MT8870\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument FM", "\"MT8870\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument TD-SCDMA", "\"MT8870\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument BT", "\"MT8870\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument C2K", "\"MT8870\"");
            }
            else if (strInstrType.Equals("CMW500"))
            {
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument WiFi", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument WCDMA", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument LTE", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument FM", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument TD-SCDMA", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument BT", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument C2K", "\"CMW500\"");
            }
            else if (strInstrType.Equals("IQXSTREAM"))
            {
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument", "\"IQXstream\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument WiFi", "\"IQXstream\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument WCDMA", "\"IQXstream\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument LTE", "\"IQXstream\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument FM", "\"IQXstream\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument TD-SCDMA", "\"IQXstream\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument BT", "\"IQXstream\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument C2K", "\"IQXstream\"");
            }
            else if (strInstrType.Equals("SP6010"))
            {
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument", "\"SP6010\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument WiFi", "\"SP6010\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument WCDMA", "\"SP6010\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument LTE", "\"SP6010\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument FM", "\"SP6010\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument TD-SCDMA", "\"SP6010\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument BT", "\"SP6010\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument C2K", "\"SP6010\"");
            }
            else if (strInstrType.Equals("AG8960"))
            {
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument", "\"AG8960\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument WiFi", "\"AG8960\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument WCDMA", "\"AG8960\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument LTE", "\"AG8960\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument FM", "\"AG8960\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument TD-SCDMA", "\"AG8960\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument BT", "\"AG8960\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument C2K", "\"AG8960\"");
            }
            else
            {
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument WiFi", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument WCDMA", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument LTE", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument FM", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument TD-SCDMA", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument BT", "\"CMW500\"");
                EditIniKey(strMtkSetupFile, "System Setting", "Instrument C2K", "\"CMW500\"");
            }
            return iStatus;
        }

        public int UpdateMtkSetupIni_All(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;
            //bool bFindComPort = false;
            try
            {
                string strKeyName = "";
                string strKeyVal = "";
                string strFileName = "";
                string strLine = "";
                string strCurrentDirectory = "";
                string strMtkSetupFile = "";
                strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                if (sCurCfgCtrlSet.sRunAllInfo.iRunMode == 0)
                {
                    if (Bz_Handler.CItemListEquip.IsFqaVerify() == 1) //  Patricio FQA Verify Setup
                    {
                      strMtkSetupFile = strCurrentDirectory + "\\mtk\\cfg\\mode_dut_cal_ver\\MTK_SETUP_Verify.ini";
                    }
                    else
                    strMtkSetupFile = strCurrentDirectory + "\\mtk\\cfg\\mode_dut_cal_ver\\MTK_SETUP.ini";
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.iRunMode == 1)
                {
                    if (Bz_Handler.CItemListEquip.IsFqaVerify() == 1) //  Patricio FQA Verify Setup
                    {
                        strMtkSetupFile = strCurrentDirectory + "\\mtk\\cfg\\mode_dut_cal_ver\\MTK_SETUP_Verify.ini";
                    }
                    else
                    strMtkSetupFile = strCurrentDirectory + "\\mtk\\cfg\\mode_cl_cal\\MTK_SETUP.ini";
                }

                if (sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode == 1)
                {
                    EditIniKey(strMtkSetupFile, "Calibration Setup", "COM PORT", "9999");
                    EditIniKey(strMtkSetupFile, "Calibration Setup", "Preloader COM PORT", sCurCfgCtrlSet.sRunAllInfo.iDutPreloaderComPort.ToString());
                    EditIniKey(strMtkSetupFile, "Calibration Setup", "Gadget VCOM PORT", sCurCfgCtrlSet.sRunAllInfo.iDutGadgetComPort.ToString());
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode == 2)
                {
                    if (sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort.Length > 3)
                    {
                        strKeyVal = sCurCfgCtrlSet.sRunAllInfo.strCurDutComPort;
                        strKeyVal = strKeyVal.Substring(3, strKeyVal.Length - 3);
                        EditIniKey(strMtkSetupFile, "Calibration Setup", "COM PORT", strKeyVal);
                    }
                }
                strKeyVal = string.Format("\"{0}\"", sCurCfgCtrlSet.sRunAllInfo.strCurTesterType);
                EditIniKey(strMtkSetupFile, "Calibration Setup", "Instrument", strKeyVal);
                strKeyVal = string.Format("\"{0}\"", sCurCfgCtrlSet.sRunAllInfo.strCurTesterPort);
                EditIniKey(strMtkSetupFile, "Calibration Setup", "RF Port", strKeyVal);
                if(sCurCfgCtrlSet.sRunAllInfo.iEnablePingPang == 0)
                {
                    EditIniKey(strMtkSetupFile, "Calibration Setup", "EnablePingPang", "0");                   
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.iEnablePingPang == 1)
                {
                    EditIniKey(strMtkSetupFile, "Calibration Setup", "EnablePingPang", "1");
                }
                EditIniKey(strMtkSetupFile, "Calibration Setup", "TimoutPingPang", sCurCfgCtrlSet.sRunAllInfo.iTimeoutPingPang.ToString());

                EditIniKey(strMtkSetupFile, "Signalling Measurement", "Stop Condition", "1");
                EditIniKey(strMtkSetupFile, "Signalling Measurement", "Write pass status to Target", "1");

                UpdateMtkSetupIni_InstrType(strMtkSetupFile, sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper());

                UpdateMtkSetupIni_FilePath(sCurCfgCtrlSet.sRunAllInfo.iRunMode, strMtkSetupFile, "APBB database file");
                UpdateMtkSetupIni_FilePath(sCurCfgCtrlSet.sRunAllInfo.iRunMode, strMtkSetupFile, "Second FDM database file");
                UpdateMtkSetupIni_FilePath(sCurCfgCtrlSet.sRunAllInfo.iRunMode, strMtkSetupFile, "Second Calibration file");
                UpdateMtkSetupIni_FilePath(sCurCfgCtrlSet.sRunAllInfo.iRunMode, strMtkSetupFile, "Second Config file");
                UpdateMtkSetupIni_FilePath(sCurCfgCtrlSet.sRunAllInfo.iRunMode, strMtkSetupFile, "MD2 FDM database file");
                UpdateMtkSetupIni_FilePath(sCurCfgCtrlSet.sRunAllInfo.iRunMode, strMtkSetupFile, "MD2 Calibration file");
                UpdateMtkSetupIni_FilePath(sCurCfgCtrlSet.sRunAllInfo.iRunMode, strMtkSetupFile, "MD2 Config file");
                UpdateMtkSetupIni_FilePath(sCurCfgCtrlSet.sRunAllInfo.iRunMode, strMtkSetupFile, "FDM database file");
                UpdateMtkSetupIni_FilePath(sCurCfgCtrlSet.sRunAllInfo.iRunMode, strMtkSetupFile, "Config file");
                UpdateMtkSetupIni_FilePath(sCurCfgCtrlSet.sRunAllInfo.iRunMode, strMtkSetupFile, "Report file path");
                UpdateMtkSetupIni_FilePath(sCurCfgCtrlSet.sRunAllInfo.iRunMode, strMtkSetupFile, "Calibration file");

                UpdateMtkSetupIni_FilePath(sCurCfgCtrlSet.sRunAllInfo.iRunMode, strMtkSetupFile, "C2K Calibration file");
                UpdateMtkSetupIni_FilePath(sCurCfgCtrlSet.sRunAllInfo.iRunMode, strMtkSetupFile, "C2K Config file");

                strKeyVal = string.Format("\"{0}\\log\\mtk\"", strCurrentDirectory);
                strKeyVal = strKeyVal.Replace("\\", "\\\\");
                EditIniKey(strMtkSetupFile, "System Setting", "Report file path", strKeyVal);

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "UpdateMtkSetupIni_All Failed!");
                iStatus = -1;
            }
            return iStatus;
        }
        // BB Patricio
        public int UpdateBbSetupIni_All(clCfgCtrlSet sSetCtrl)
        {
            int iStatus = 0;
            //bool bFindComPort = false;
            try
            {
                string strKeyName = "";
                string strKeyVal = "";
                string strCurrentDirectory = "";
                string strBbSetupFile = "";

                strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                strBbSetupFile = strCurrentDirectory + "\\mtk\\bb\\MTK_SETUP.ini";
                if (sSetCtrl.sRunAllInfo.iCurDutPortMode == 1)
                {
                    strKeyName = string.Format("Preloader_port{0}", sSetCtrl.sRunAllInfo.iCurDutGroupIndex + 1);
                    strKeyVal = sSetCtrl.sRunAllInfo.iDutPreloaderComPort.ToString();
                    EditIniKey(strBbSetupFile, "MTK_USB", strKeyName, strKeyVal);
                    strKeyName = string.Format("Kernel_port{0}", sSetCtrl.sRunAllInfo.iCurDutGroupIndex + 1);
                    strKeyVal = sSetCtrl.sRunAllInfo.iDutGadgetComPort.ToString();
                    EditIniKey(strBbSetupFile, "MTK_USB", strKeyName, strKeyVal);
                }
                else if (sSetCtrl.sRunAllInfo.iCurDutPortMode == 2)
                {
                    if (sSetCtrl.sRunAllInfo.strCurDutComPort.Length > 3)
                    {
                        strKeyName = string.Format("DUT{0}", sSetCtrl.sRunAllInfo.iCurDutGroupIndex + 1);
                        strKeyVal = sSetCtrl.sRunAllInfo.strCurDutComPort;
                        EditIniKey(strBbSetupFile, "RS232_MsCtrl", strKeyName, strKeyVal);
                    }
                }
                strKeyName = string.Format("DUT{0}", sSetCtrl.sRunAllInfo.iCurDutGroupIndex + 1);
                strKeyVal = string.Format("{0}&{1}",
                    sSetCtrl.sRunAllInfo.strCurPowerSupplyAddr,
                    sSetCtrl.sRunAllInfo.iCurPowerChanIndex + 1);
                EditIniKey(strBbSetupFile, "DC_POWER", strKeyName, strKeyVal);

                strKeyName = "Modem";
                strKeyVal = string.Format("{0}{1}", strCurrentDirectory, sSetCtrl.sRunAllInfo.str1stDbFile);
                EditIniKey(strBbSetupFile, "DataBase", strKeyName, strKeyVal);

                strKeyName = "Ap";
                strKeyVal = string.Format("{0}{1}", strCurrentDirectory, sSetCtrl.sRunAllInfo.strApdbFile);
                EditIniKey(strBbSetupFile, "DataBase", strKeyName, strKeyVal);

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "UpdateBbSetupIni_All Failed!");
                iStatus = -1;
            }
            return iStatus;
        }
        // BB end Patricio
        public int UpdateMtkCfgTxt(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;
            string[] straryCfgSetLineVal = new string[10];

            try
            {
                string strWriteLine = "";
                string strLine = "";
                string strCurrentDirectory = "";
                string strMtkSetupFile = "";

                strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                if (sCurCfgCtrlSet.sRunAllInfo.iRunMode == 0)
                {
                    if (Bz_Handler.CItemListEquip.IsFqaVerify() == 1) //  Patricio FQA Verify Setup
                    {
                        strMtkSetupFile = strCurrentDirectory + "\\mtk\\cfg\\mode_dut_cal_ver\\MTK_SETUP_Verify.ini";
                    }
                    else
                    strMtkSetupFile = strCurrentDirectory + "\\mtk\\cfg\\mode_dut_cal_ver\\MTK_SETUP.ini";
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.iRunMode == 1)
                {
                    if (Bz_Handler.CItemListEquip.IsFqaVerify() == 1) //  Patricio FQA Verify Setup
                    {
                        strMtkSetupFile = strCurrentDirectory + "\\mtk\\cfg\\mode_dut_cal_ver\\MTK_SETUP_Verify.ini";
                    }
                    else
                    strMtkSetupFile = strCurrentDirectory + "\\mtk\\cfg\\mode_cl_cal\\MTK_SETUP.ini";
                }
                sCurCfgCtrlSet.listOrgMtkCfg.Clear();
                StreamReader streamOrgMtkSetupIniFile = new StreamReader(strMtkSetupFile, Encoding.GetEncoding("gb2312"), false);
                if (streamOrgMtkSetupIniFile != null)
                {
                    while ((strLine = streamOrgMtkSetupIniFile.ReadLine()) != null)
                    {
                        clCfgFileUnit sNewCfgFileSet = new clCfgFileUnit();
                        sNewCfgFileSet.strIndex = string.Format("{0}", sCurCfgCtrlSet.listOrgMtkCfg.Count);
                        sNewCfgFileSet.strLine = strLine;
                        sCurCfgCtrlSet.listOrgMtkCfg.Add(sNewCfgFileSet);
                    }
                    streamOrgMtkSetupIniFile.Close();
                }

                sCurCfgCtrlSet.listMtkCfgTxtFile.Clear();

                foreach (clCfgFileUnit sNewCfgFileSet in sCurCfgCtrlSet.listOrgMtkCfg)
                {
                    if (sNewCfgFileSet.strLine.Contains("Config file") && (sNewCfgFileSet.strLine.Contains(".cfg") || sNewCfgFileSet.strLine.Contains(".CFG")))
                    {
                        straryCfgSetLineVal = sNewCfgFileSet.strLine.Split(new char[2] { '=', '"' });
                        if (straryCfgSetLineVal.Length > 3)
                        {
                            clMtkCfgTxtFile sMtkCfgTxtFile = new clMtkCfgTxtFile();
                            sMtkCfgTxtFile.strIndex = string.Format("{0}", sCurCfgCtrlSet.listMtkCfgTxtFile.Count);
                            sMtkCfgTxtFile.strFile = straryCfgSetLineVal[2];
                            sMtkCfgTxtFile.strFile = sMtkCfgTxtFile.strFile.Replace("\\\\", "\\");
                            sCurCfgCtrlSet.listMtkCfgTxtFile.Add(sMtkCfgTxtFile);
                        }
                    }
                }

                foreach (clMtkCfgTxtFile sMtkCfgTxtFile in sCurCfgCtrlSet.listMtkCfgTxtFile)
                {
                    if (!File.Exists(sMtkCfgTxtFile.strFile))
                    {
                        continue;
                    }

                    sCurCfgCtrlSet.listOrgMtkCfg.Clear();
                    StreamReader streamOrgMtkCfgFile = new StreamReader(sMtkCfgTxtFile.strFile, Encoding.GetEncoding("gb2312"), false);
                    if (streamOrgMtkCfgFile != null)
                    {
                        while ((strLine = streamOrgMtkCfgFile.ReadLine()) != null)
                        {
                            clCfgFileUnit sNewCfgFileSet = new clCfgFileUnit();
                            sNewCfgFileSet.strIndex = string.Format("{0}", sCurCfgCtrlSet.listOrgMtkCfg.Count);
                            sNewCfgFileSet.strLine = strLine;
                            sCurCfgCtrlSet.listOrgMtkCfg.Add(sNewCfgFileSet);
                        }
                        streamOrgMtkCfgFile.Close();
                    }

                    StreamWriter streamNewCfgFile = new StreamWriter(sMtkCfgTxtFile.strFile, false, Encoding.GetEncoding("gb2312"));
                    foreach (clCfgFileUnit sNewCfgFileSet in sCurCfgCtrlSet.listOrgMtkCfg)
                    {
                        if (sNewCfgFileSet.strLine.Contains("GPIB Address") 
                            || sNewCfgFileSet.strLine.Contains("IP Address")
                            || sNewCfgFileSet.strLine.Contains("RF Port"))
                        {
                            strWriteLine = "";
                            if (sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Equals("AG8960"))
                            {
                                if (sNewCfgFileSet.strLine.Contains("8960 GPIB Address"))
                                {
                                    strWriteLine = "8960 GPIB Address = " + sCurCfgCtrlSet.sRunAllInfo.strCurTesterAddr;
                                }
                                else if (sNewCfgFileSet.strLine.Contains("8960 RF Port"))
                                {
                                    strWriteLine = "8960 RF Port = " + sCurCfgCtrlSet.sRunAllInfo.strCurTesterPort;
                                }
                            }
                            else if (sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Equals("MT8870"))
                            {
                                if (sNewCfgFileSet.strLine.Contains("MT8870 GPIB Address"))
                                {
                                    strWriteLine = "MT8870 GPIB Address = " + sCurCfgCtrlSet.sRunAllInfo.strCurTesterAddr;
                                }
                                else if (sNewCfgFileSet.strLine.Contains("MT8870 RF Port"))
                                {
                                    strWriteLine = "MT8870 RF Port = " + sCurCfgCtrlSet.sRunAllInfo.strCurTesterPort;
                                }
                            }
                            else if (sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Equals("IQXSTREAM"))
                            {
                                if (sNewCfgFileSet.strLine.Contains("IQxstream IP Address"))
                                {
                                    strWriteLine = "IQxstream IP Address = " + sCurCfgCtrlSet.sRunAllInfo.strCurTesterAddr;
                                }
                                else if (sNewCfgFileSet.strLine.Contains("IQxstream RF Port"))
                                {
                                    strWriteLine = "IQxstream RF Port = " + sCurCfgCtrlSet.sRunAllInfo.strCurTesterPort;
                                }
                                else if (sNewCfgFileSet.strLine.Contains("Connect Port"))
                                {
                                    if ((sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 0) || (sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 2))
                                    {
                                        strWriteLine = "Connect Port = 24100";
                                    }
                                    else if ((sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 1) || (sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex == 3))
                                    {
                                        strWriteLine = "Connect Port = 24200";
                                    }
                                }
                            }

                            else if (sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Equals("EXT"))
                            {
                                if (sNewCfgFileSet.strLine.Contains("EXT GPIB Address"))
                                {
                                    strWriteLine = "EXT GPIB Address = " + sCurCfgCtrlSet.sRunAllInfo.strCurTesterAddr;
                                }
                                else if (sNewCfgFileSet.strLine.Contains("EXT RF Port"))
                                {
                                    strWriteLine = "EXT RF Port = " + sCurCfgCtrlSet.sRunAllInfo.strCurTesterPort;
                                }
                            }
                            else if (sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Equals("CMW500"))
                            {
                                if (sNewCfgFileSet.strLine.Contains("CMW500 GPIB Address"))
                                {
                                    strWriteLine = "CMW500 GPIB Address = " + sCurCfgCtrlSet.sRunAllInfo.strCurTesterAddr;
                                }
                                else if (sNewCfgFileSet.strLine.Contains("CMW500 RF Port"))
                                {
                                    strWriteLine = "CMW500 RF Port = " + sCurCfgCtrlSet.sRunAllInfo.strCurTesterPort;
                                }
                            }
                            else if (sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Equals("CMU200"))
                            {
                                if (sNewCfgFileSet.strLine.Contains("CMU200 GPIB Address"))
                                {
                                    strWriteLine = "CMU200 GPIB Address = " + sCurCfgCtrlSet.sRunAllInfo.strCurTesterAddr;
                                }
                                else if (sNewCfgFileSet.strLine.Contains("CMU200 RF Port"))
                                {
                                    strWriteLine = "CMU200 RF Port = " + sCurCfgCtrlSet.sRunAllInfo.strCurTesterPort;
                                }
                            }
                            if (strWriteLine.Length > 1)
                            {
                                streamNewCfgFile.WriteLine(strWriteLine);
                            }
                            else
                            {
                                streamNewCfgFile.WriteLine(sNewCfgFileSet.strLine);
                            }
                        }
                        else
                        {
                            streamNewCfgFile.WriteLine(sNewCfgFileSet.strLine);
                        }
                    }
                    streamNewCfgFile.Close();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "UpdateMtkCfgTxt Failed!");
                iStatus = -1;
            }
            return iStatus;
        }

        public int UpdateMtkCfg(clCfgCtrlSet sCurCfgCtrlSet, string strMtkCfgFile)
        {
            int iStatus = 0;

            try
            {
                ;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "UpdateMtkCfg Failed!");
                iStatus = -1;
            }
            return iStatus;
        }



        public int LoadGoldenDutList(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;

            try
            {
                string strCurrentDirectory = "";
                string strAttnIniFile = "";
                int iGoldenDutNum = 0;
                int iGoldenDutIndex = 0;
                string strKeyName = "";
                string strKeyVal = "";

                sCurCfgCtrlSet.listGoldenDut.Clear();
                strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                strAttnIniFile = strCurrentDirectory + "\\attn\\csv_EXE_Call.ini";
                ReadIniKey(strAttnIniFile, "Golden Dut List", "Golden Dut Number", ref strKeyVal);
                if (strKeyVal.Length > 0)
                {
                    iGoldenDutNum = Int32.Parse(strKeyVal);
                }
                for (iGoldenDutIndex = 0; iGoldenDutIndex < iGoldenDutNum; iGoldenDutIndex++)
                {
                    strKeyName = string.Format("Golden Dut SN {0}", iGoldenDutIndex);
                    ReadIniKey(strAttnIniFile, "Golden Dut List", strKeyName, ref strKeyVal);
                    if (strKeyVal.Length > 0)
                    {
                        clCfgGolenDutUnit sGoldenDut = new clCfgGolenDutUnit();
                        sGoldenDut.strIndex = string.Format("{0}", iGoldenDutIndex);
                        sGoldenDut.strSN = strKeyVal;
                        sCurCfgCtrlSet.listGoldenDut.Add(sGoldenDut);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "LoadGoldenDutList Failed!");
                iStatus = -1;
            }
            return iStatus;
        }

        public int UpdateAttnIni(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;

            try
            {
                string strCurrentDirectory = "";
                string strGoldenFolder = "";
                string strResFolder = "";
                string strCfgFile = "";
                string strAttnIniFile = "";

                strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                strAttnIniFile = strCurrentDirectory + "\\attn\\csv_EXE_Call.ini";

                strGoldenFolder = strCurrentDirectory + "\\attn\\ref\\gold";
                strResFolder = strCurrentDirectory + "\\attn\\ref\\res";

                strGoldenFolder = strGoldenFolder.Replace("\\", "\\\\");
                EditIniKey(strAttnIniFile, "Config", "Gold csv folder", strGoldenFolder);

                strResFolder = strResFolder.Replace("\\", "\\\\");
                EditIniKey(strAttnIniFile, "Config", "Test csv folder", strResFolder);

                if (sCurCfgCtrlSet.sRunAllInfo.iEnableAttnSourceCfgFile1 == 0)
                {
                    EditIniKey(strAttnIniFile, "Config", "Gold cfg(Moderm 1) file", "");
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.iEnableAttnSourceCfgFile1 == 1)
                {
                    strCfgFile = strCurrentDirectory + sCurCfgCtrlSet.sRunAllInfo.strAttnSourceCfgFile1;
                    strCfgFile = strCfgFile.Replace("\\", "\\\\");
                    EditIniKey(strAttnIniFile, "Config", "Gold cfg(Moderm 1) file", strCfgFile);
                }

                if (sCurCfgCtrlSet.sRunAllInfo.iEnableAttnTargetCfgFile1 == 0)
                {
                    EditIniKey(strAttnIniFile, "Config", "Test cfg(Moderm 1) file", "");
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.iEnableAttnTargetCfgFile1 == 1)
                {
                    strCfgFile = strCurrentDirectory + sCurCfgCtrlSet.sRunAllInfo.strAttnTargetCfgFile1;
                    strCfgFile = strCfgFile.Replace("mode_cl_cal", "mode_dut_cal_ver");
                    strCfgFile = strCfgFile.Replace("\\", "\\\\");
                    EditIniKey(strAttnIniFile, "Config", "Test cfg(Moderm 1) file", strCfgFile);
                }

                if (sCurCfgCtrlSet.sRunAllInfo.iEnableAttnSourceCfgFile2 == 0)
                {
                    EditIniKey(strAttnIniFile, "Config", "Gold cfg(Moderm 2) file", "");
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.iEnableAttnSourceCfgFile2 == 1)
                {
                    strCfgFile = strCurrentDirectory + sCurCfgCtrlSet.sRunAllInfo.strAttnSourceCfgFile2;
                    strCfgFile = strCfgFile.Replace("\\", "\\\\");
                    EditIniKey(strAttnIniFile, "Config", "Gold cfg(Moderm 2) file", strCfgFile);
                }

                if (sCurCfgCtrlSet.sRunAllInfo.iEnableAttnTargetCfgFile2 == 0)
                {
                    EditIniKey(strAttnIniFile, "Config", "Test cfg(Moderm 2) file", "");
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.iEnableAttnTargetCfgFile2 == 1)
                {
                    strCfgFile = strCurrentDirectory + sCurCfgCtrlSet.sRunAllInfo.strAttnTargetCfgFile2;
                    strCfgFile = strCfgFile.Replace("mode_cl_cal", "mode_dut_cal_ver");
                    strCfgFile = strCfgFile.Replace("\\", "\\\\");
                    EditIniKey(strAttnIniFile, "Config", "Test cfg(Moderm 2) file", strCfgFile);
                }

                if (sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Equals("MT8870"))
                {
                    EditIniKey(strAttnIniFile, "MTK Cableloss add setting", "Instrument Type", "0");
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Equals("CMW500"))
                {
                    EditIniKey(strAttnIniFile, "MTK Cableloss add setting", "Instrument Type", "1");
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Equals("IQXSTREAM"))
                {
                    EditIniKey(strAttnIniFile, "MTK Cableloss add setting", "Instrument Type", "2");
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Equals("CMU200"))
                {
                    EditIniKey(strAttnIniFile, "MTK Cableloss add setting", "Instrument Type", "3");
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Equals("EXT"))
                {
                    EditIniKey(strAttnIniFile, "MTK Cableloss add setting", "Instrument Type", "4");
                }
                else if (sCurCfgCtrlSet.sRunAllInfo.strCurTesterType.ToUpper().Equals("AG8960"))
                {
                    EditIniKey(strAttnIniFile, "MTK Cableloss add setting", "Instrument Type", "5");
                }
                else
                {
                    EditIniKey(strAttnIniFile, "MTK Cableloss add setting", "Instrument Type", "0");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "UpdateAttnIni Failed!");
                iStatus = -1;
            }
            return iStatus;
        }

        public int InitMainCfgSet(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;
            int iSetNum = 0;
            int iDutIndex = 0;
            int iSetIndex = 0;

            iSetNum = sCurCfgCtrlSet.listMainComSet.Count;
            for (iSetIndex = 0; iSetIndex < iSetNum; iSetIndex++)
            {
                if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "ate_version")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAteVersion = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "log_file")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strLogFile = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "err_cfg_file")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strErrCfgFile = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "res_cfg_file")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strResCfgFile = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "design_type")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iDesignType = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "log_err_code")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iLogErrCode = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "serial_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iSerialMode = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "hw_id_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strHwIdMode = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "hw_id_default")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strHwIdDefault = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "hw_id_file")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strHwIdFile = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "test_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iTestMode = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "phone_type")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strPhoneType = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "input_sn")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iInputSn = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "bb_sn_opt") // Patricio
                {
                    sCurCfgCtrlSet.sRunAllInfo.iBbSnOpt = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "only_sn")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iOnlySn = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "recode_cal_flag")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iRecodeCalFlag = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "enable_retry_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iEnableRetryMode = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "enable_fixture")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iEnableFixtureControl = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "check_fixture_closed")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iCheckFixtureClosed = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "enable_plc")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iEnablePlcControl = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "fixture_type")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iFixtureType = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "enable_power_supply")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iEnablePowerSupplyControl = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "unclock_modem")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iUnclockModem = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "debug_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iDebugMode = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "log_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iLogMode = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "repeat_num")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iRunDutCalRepeatNum = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "delay_time")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iRunDutCalDelayTime = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "run_timeout")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iRunTimeout = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "comport_connect_timeout")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iComPortConnectTimeout = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "dut_connect_timeout")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iDutConnectTimeout = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "remote_option")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iRemoteOption = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "print_option")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iPrintOption = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "repeat_mode_opt")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iRepeatModeOpt = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "fixture_close_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iFixtureCloseModeOpt = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "enable_rf_switch")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iEnableRfSwitch = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "rf_switch_port")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strRfSwitchPort = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "rf_switch_com")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strRfSwitchCom = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "stop_mode_opt")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iStopModeOpt = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "language_opt")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iLanguageOpt = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "enable_bb_test")  // Patricio
                {
                     sCurCfgCtrlSet.sRunAllInfo.iEnableBbTest = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "test_item_cal_main")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strIntelTestItemCalMain = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "test_item_cal_rd")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strIntelTestItemCalRd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "test_item_verify")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strIntelTestItemRfVerify = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "lenovo_log_folder")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strLenovoLogFolder = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "intel_log_folder")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strIntelLogFolder = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_fer_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCalFbrBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_fer_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCalFbrEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_fast_fbr_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCalFastFbrBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_fast_fbr_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCalFastFbrEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_afc_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCalAfcBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_afc_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCalAfcEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_2g_rx_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal2gRxBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_2g_rx_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal2gRxEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_2g_tx_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal2gTxBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_2g_tx_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal2gTxEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_2g_fast_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal2gFastBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_2g_fast_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal2gFastEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_3g_rx_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal3gRxBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_3g_rx_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal3gRxEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_3g_tx_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal3gTxBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_3g_tx_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal3gTxEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_3g_fast_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal3gFastBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_3g_fast_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal3gFastEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_4g_rx_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal4gRxBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_4g_rx_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal4gRxEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_4g_tx_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal4gTxBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_4g_tx_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal4gTxEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_4g_fast_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal4gFastBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_log_stamp_cal_4g_fast_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strStepLogStampCal4gFastEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "sum_log_stamp_cal_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strSumLogStampCalBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "sum_log_stamp_cal_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strSumLogStampCalEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "sum_log_stamp_rf_verify_begin")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strSumLogStampRfVerifyBegin = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "sum_log_stamp_rf_verify_end")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strSumLogStampRfVerifyEnd = sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_num_verify")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iStepNumVerify = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "step_num_cal")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iStepNumCal = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "attn_min_value")
                {
                    sCurCfgCtrlSet.sRunAllInfo.dCurAttnMinValue = Double.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "attn_max_value")
                {
                    sCurCfgCtrlSet.sRunAllInfo.dCurAttnMaxValue = Double.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "attn_gsm_band_diff_max_value")
                {
                    sCurCfgCtrlSet.sRunAllInfo.dGsmBandDiffMaxValue = Double.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "attn_delta_max_value")
                {
                    sCurCfgCtrlSet.sRunAllInfo.dDeltaMaxValue = Double.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "attn_meas_num")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iRunCableCalRepeatNum = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "restart_ateDemo_enable")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iRestartATEDemo = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "continue_fail_max")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iContinueFailMax = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "delete_mtk_log")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iDeleteMtkLog = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainComSet[iSetIndex].strName == "ate_reboot_every_time")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iATERebootEveryTime = Int32.Parse(sCurCfgCtrlSet.listMainComSet[iSetIndex].strVal);
                }
            }

            iSetNum = sCurCfgCtrlSet.listMainInstrSet.Count;
            for (iSetIndex = 0; iSetIndex < iSetNum; iSetIndex++)
            {
                if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "all_tester_support")
                {
                    string strAllTesterSupport = "";
                    strAllTesterSupport = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                    sCurCfgCtrlSet.sRunAllInfo.straryAllTesterType = strAllTesterSupport.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "cur_tester_type")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCurTesterType = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "enable_pingpang")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iEnablePingPang = Int32.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "timeout_pingpang")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iTimeoutPingPang = Int32.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "tester_connect_addr")
                {
                    if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ") // Patricio
                        sCurCfgCtrlSet.sRunAllInfo.strCurTesterAddr = Bz_Handler.CJagLocalFucntions.GetTestSetAddress();
                    else
                        sCurCfgCtrlSet.sRunAllInfo.strCurTesterAddr = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "all_tester_port")
                {
                    string strAllTesterPort = "";
                    strAllTesterPort = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                    sCurCfgCtrlSet.sRunAllInfo.straryAllTesterPort = strAllTesterPort.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "cur_tester_port")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCurTesterPort = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "all_power_support")
                {
                    string strAllTesterSupport = "";
                    strAllTesterSupport = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                    sCurCfgCtrlSet.sRunAllInfo.straryAllPowerSupplyType = strAllTesterSupport.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "cur_power_type")
                {
                    if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ") // Patricio
                        sCurCfgCtrlSet.sRunAllInfo.strCurPowerSupplyType = Bz_Handler.CJagLocalFucntions.GetPowerSupplyModel();
                    else
                        sCurCfgCtrlSet.sRunAllInfo.strCurPowerSupplyType = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "power_connect_addr")
                {
                    if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ")  // Patricio
                        sCurCfgCtrlSet.sRunAllInfo.strCurPowerSupplyAddr = Bz_Handler.CJagLocalFucntions.GetPowerSupplyAddress();
                    else
                        sCurCfgCtrlSet.sRunAllInfo.strCurPowerSupplyAddr = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                   
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "power_chan_index")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iCurPowerChanIndex = Int32.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "power_ch0_voltage") // Desligado no Main.XML
                {                 
                    sCurCfgCtrlSet.sRunAllInfo.dCurCh0PowerVoltage = Double.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "power_ch0_current")
                {
                        sCurCfgCtrlSet.sRunAllInfo.dCurCh0PowerCurrent = Double.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "power_ch1_voltage")
                {
                    sCurCfgCtrlSet.sRunAllInfo.dCurCh1PowerVoltage = Double.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "power_ch1_current")
                {
                    sCurCfgCtrlSet.sRunAllInfo.dCurCh1PowerCurrent = Double.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "path_source_cfg_file1")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAttnSourceCfgFile1 = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "path_source_cfg_file2")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAttnSourceCfgFile2 = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "path_target_cfg_file1")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAttnTargetCfgFile1 = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "path_target_cfg_file2")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAttnTargetCfgFile2 = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "enable_source_cfg_file1")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iEnableAttnSourceCfgFile1 = Int32.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "enable_target_cfg_file1")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iEnableAttnTargetCfgFile1 = Int32.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "enable_source_cfg_file2")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iEnableAttnSourceCfgFile2 = Int32.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "enable_target_cfg_file2")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iEnableAttnTargetCfgFile2 = Int32.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "connect_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iConnectMode = Int32.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "cmw_ctrl_dll")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCmwCtrlDll = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "cmw_ctrl_api")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCmwCtrlApi = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "cmw_exe_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iCmwExeMode = Int32.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "sim_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iSimMode = Int32.Parse(sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "instr_eatt_port0")
                {
                    sCurCfgCtrlSet.sRunAllInfo.straryEattPort[0] = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "instr_eatt_port1")
                {
                    sCurCfgCtrlSet.sRunAllInfo.straryEattPort[1] = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "instr_eatt_port2")
                {
                    sCurCfgCtrlSet.sRunAllInfo.straryEattPort[2] = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "instr_eatt_port3")
                {
                    sCurCfgCtrlSet.sRunAllInfo.straryEattPort[3] = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "instr_eatt_port4")
                {
                    sCurCfgCtrlSet.sRunAllInfo.straryEattPort[4] = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "instr_eatt_port5")
                {
                    sCurCfgCtrlSet.sRunAllInfo.straryEattPort[5] = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "instr_eatt_port6")
                {
                    sCurCfgCtrlSet.sRunAllInfo.straryEattPort[6] = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strName == "instr_eatt_port7")
                {
                    sCurCfgCtrlSet.sRunAllInfo.straryEattPort[7] = sCurCfgCtrlSet.listMainInstrSet[iSetIndex].strVal;
                }
            }

            iSetNum = sCurCfgCtrlSet.listMainDutSet.Count;
            for (iSetIndex = 0; iSetIndex < iSetNum; iSetIndex++)
            {
                if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_ctrl_dll")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutCtrlDll = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_ctrl_api")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutCtrlApi = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_exe_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iDutExeMode = Int32.Parse(sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_reset_after_cal")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iDutResetAfterCal = Int32.Parse(sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_serial_port_pre_baudrate")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutSerialPortPreBaudRate = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_serial_port_cur_baudrate")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutSerialPortCurBaudRate = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_serial_port_stopbits")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutSerialPortStopBits = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_serial_port_parity")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutSerialPortParity = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_serial_port_encoding")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutSerialPortEncoding = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_serial_port_datebits")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutSerialPortDataBits = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_serial_port_read_timeout")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutSerialPortReadTimeout = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_serial_port_write_timeout")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutSerialPortWriteTimeout = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_cmd_gsn")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoGsn = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_support_sw_version")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strSupportSwVersion = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_cmd_lenovo_check_sw_version")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoCheckVersion = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_cmd_lenovo_set_test_baudrate")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoSetTestBaudRate = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_cmd_lenovo_enter_cal_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoEnterCalMode = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_cmd_lenovo_exit_cal_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoExitCalMode = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_cmd_lenovo_backup_modem_data")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoBackupModemData = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_cmd_lenovo_modem_reset")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoModemReset = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_cmd_lenovo_modem_lock")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoModemLock = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_cmd_lenovo_modem_unlock")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAtCmdLenovoModemUnLock = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_cmd_intel_set_nvm_cfg")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAtCmdIntelSetNvmCfg = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_cmd_intel_fix_usb")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAtCmdIntelFixUsb = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_cmd_intel_store_nvm_sync")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAtCmdIntelStoreNvmSync = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "at_cmd_intel_modem_reset")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strAtCmdIntelModemReset = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "all_dut_port_mode_support")
                {
                    string strAllDutPortModeSupport = "";
                    strAllDutPortModeSupport = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                    sCurCfgCtrlSet.sRunAllInfo.straryAllDutPortModeSupport = strAllDutPortModeSupport.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_cur_port_mode")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iCurDutPortMode = Int32.Parse(sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_preloader_com_port")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iDutPreloaderComPort = Int32.Parse(sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_gadget_com_port")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iDutGadgetComPort = Int32.Parse(sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_all_group_num")
                {
                    //if (sCurCfgCtrlSet.sRunAllInfo.iStart == 0)
                    {
                        sCurCfgCtrlSet.sRunAllInfo.iAllDutGroupNum = Int32.Parse(sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    }
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_cur_group_index")
                {
                    //if (sCurCfgCtrlSet.sRunAllInfo.iStart == 0)
                    {
                        sCurCfgCtrlSet.sRunAllInfo.iCurDutGroupIndex = Int32.Parse(sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    }
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port0_cfg_file_cal_main")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCfgFileGroup0Port0CalMain = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port0_cfg_file_cal_rd")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCfgFileGroup0Port0CalRd = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port0_cfg_file_rf_verify")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCfgFileGroup0Port0RfVerify = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port0_cfg_file_cal_main")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCfgFileGroup1Port0CalMain = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port0_cfg_file_cal_rd")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCfgFileGroup1Port0CalRd = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port0_cfg_file_rf_verify")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCfgFileGroup1Port0RfVerify = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port0_cfg_file_cal_main")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCfgFileGroup2Port0CalMain = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port0_cfg_file_cal_rd")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCfgFileGroup2Port0CalRd = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port0_cfg_file_rf_verify")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCfgFileGroup2Port0RfVerify = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port0_cfg_file_cal_main")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCfgFileGroup3Port0CalMain = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port0_cfg_file_cal_rd")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCfgFileGroup3Port0CalRd = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port0_cfg_file_rf_verify")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCfgFileGroup3Port0RfVerify = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port0_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port1_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port1EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port2_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port2EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port3_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port3EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port0_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port1_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port1EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port2_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port2EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port3_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port3EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port0_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port1_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port1EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port2_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port2EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port3_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port3EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port0_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port1_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port1EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port2_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port2EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port3_pathloss")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port3EattFilePath = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port0_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port1_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port1ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port1ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port1ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port1ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port2_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port2ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port2ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port2ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port2ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port3_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port3ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port3ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port3ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port3ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port0_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port1_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port1ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port1ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port1ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port1ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port2_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port2ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port2ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port2ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port2ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port3_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port3ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port3ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port3ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port3ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port0_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port1_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port1ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port1ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port1ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port1ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port2_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port2ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port2ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port2ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port2ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port3_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port3ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port3ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port3ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port3ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port0_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port1_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port1ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port1ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port1ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port1ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port2_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port2ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port2ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port2ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port2ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port3_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port3ExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port3ExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port3ExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port3ExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port0_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port1_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port1CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port2_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port2CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port3_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port3CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port0_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port1_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port1CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port2_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port2CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port3_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port3CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port0_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port1_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port1CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port2_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port2CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port3_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port3CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port0_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port1_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port1CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port2_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port2CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port3_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port3CfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                    // BB Patricio Carregando Parametros BB TEST
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port0_bb_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0BbExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0BbExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0BbExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0BbExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port0_bb_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0BbExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0BbExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0BbExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0BbExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port0_bb_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0BbExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0BbExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0BbExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0BbExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port0_bb_exe_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0BbExeFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0BbExeWorkPath = sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0BbExeFilePath.Remove(sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0BbExeFilePath.LastIndexOf("\\"));
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port0_bb_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0BbCfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port0_bb_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0BbCfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port0_bb_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0BbCfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port0_bb_cfg_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0BbCfgFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_port0_bb_seq_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup0Port0BbSeqFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_port0_bb_seq_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup1Port0BbSeqFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_port0_bb_seq_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup2Port0BbSeqFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_port0_bb_seq_path")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strDutGroup3Port0BbSeqFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory(), sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal);
                } 
                    // BB end Patricio
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group0_comport")
                {
                    //if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ") // Patricio
                    //    sCurCfgCtrlSet.sRunAllInfo.strGroup0DutComPort = Bz_Handler.CJagLocalFucntions.GetComPortLeft();
                    //else
                        sCurCfgCtrlSet.sRunAllInfo.strGroup0DutComPort = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                    
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group1_comport")
                {
                    //if (Bz_Handler.CJagLocalFucntions.GetFactory() == "BZ") // Patricio
                    //    sCurCfgCtrlSet.sRunAllInfo.strGroup1DutComPort = Bz_Handler.CJagLocalFucntions.GetComPortRight();
                    //else
                        sCurCfgCtrlSet.sRunAllInfo.strGroup1DutComPort = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;

                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group2_comport")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strGroup2DutComPort = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "dut_group3_comport")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strGroup3DutComPort = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "fixture_group0_comport")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strGroup0FixturePort = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "fixture_group1_comport")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strGroup1FixturePort = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "fixture_group2_comport")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strGroup2FixturePort = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listMainDutSet[iSetIndex].strName == "fixture_group3_comport")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strGroup3FixturePort = sCurCfgCtrlSet.listMainDutSet[iSetIndex].strVal;
                }
            }

            return iStatus;
        }

        public int InitResCfgSet(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;
            int iSetNum = 0;
            int iSetIndex = 0;

            iSetNum = sCurCfgCtrlSet.listResComSet.Count;
            for (iSetIndex = 0; iSetIndex < iSetNum; iSetIndex++)
            {
                if (sCurCfgCtrlSet.listResComSet[iSetIndex].strName == "LOG_RES_LEVEL")
                {
                    sCurCfgCtrlSet.sRunAllInfo.strCurResLevel = sCurCfgCtrlSet.listResComSet[iSetIndex].strVal;
                }
                else if (sCurCfgCtrlSet.listResComSet[iSetIndex].strName == "LOG_CAL_RES")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iLogCalRes = Int32.Parse(sCurCfgCtrlSet.listResComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listResComSet[iSetIndex].strName == "LOG_CAL_DATA")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iLogCalData = Int32.Parse(sCurCfgCtrlSet.listResComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listResComSet[iSetIndex].strName == "LOG_CFG_SET")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iLogCfgSet = Int32.Parse(sCurCfgCtrlSet.listResComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listResComSet[iSetIndex].strName == "LOG_CFG_LOSS")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iLogCfgLoss = Int32.Parse(sCurCfgCtrlSet.listResComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listResComSet[iSetIndex].strName == "LOG_MEAS_RES")
                {
                    sCurCfgCtrlSet.sRunAllInfo.iLogMeasRes = Int32.Parse(sCurCfgCtrlSet.listResComSet[iSetIndex].strVal);
                }
                else if (sCurCfgCtrlSet.listResComSet[iSetIndex].strName == "LOG_BB_RES") // BB Patricio
                {
                    sCurCfgCtrlSet.sRunAllInfo.iLogBbRes = Int32.Parse(sCurCfgCtrlSet.listResComSet[iSetIndex].strVal);
                }        
                else if (sCurCfgCtrlSet.listResComSet[iSetIndex].strName == "SUPPORT_RES_LEVEL")
                {
                    string strAllSupportResLevel = "";
                    strAllSupportResLevel = sCurCfgCtrlSet.listResComSet[iSetIndex].strVal;
                    sCurCfgCtrlSet.sRunAllInfo.straryAllSupportResLevelType = strAllSupportResLevel.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            return iStatus;
        }

        public int InitErrCfgSet(clCfgCtrlSet sCurCfgCtrlSet)
        {
            int iStatus = 0;

            return iStatus;
        }
    }
}
