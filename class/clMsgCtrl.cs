using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace ateRun
{
    class clMsgCtrl
    {
        public const int USER = 0x0400;
        public const int WM_RUN_CFG = USER + 101;
        public const int WM_RUN_SUM = USER + 102;
        public const int WM_RUN_LOG = USER + 103;
        public const int WM_RUN_CTRL = USER + 201;
        public const int WM_RUN_PROC = USER + 301;
        public const int WM_RUN_ERR = USER + 401;


        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);

        public void SendMsg(IntPtr hwnd, uint wMsg, string wParam, string lParam)
        {
            SendMessage(hwnd, wMsg, Marshal.StringToHGlobalAnsi(wParam).ToInt32(), Marshal.StringToHGlobalAnsi(lParam).ToInt32());
        }
    }
}
