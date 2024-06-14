using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.Win32.SafeHandles;
using System.Reflection;
using Semnox.Parafait.Device;

namespace Semnox.Parafait.KioskCore.CardDispenser
{
    public class D1801 : K720RF
    {
        public D1801(string serialPortNum, int carDispenserAdd) : base(serialPortNum, carDispenserAdd)
        {
            log.LogMethodEntry(serialPortNum, carDispenserAdd);
            log.LogMethodExit();
        }

        protected override IntPtr OpenCom(string port)
        {
            return D1801_CommOpen(port);
        }

        protected override int CloseCom(IntPtr handle)
        {
            return D1801_CommClose(handle);
        }

        protected override int SendCmd(IntPtr handle, byte addr, string p_Cmd, int CmdLen)
        {
            return D1801_SendCmd(handle, addr, p_Cmd, CmdLen);
        }

        protected override int SensorQuery(IntPtr handle, byte addr, byte[] StateInfo)
        {
            return D1801_SensorQuery(handle, addr, StateInfo);
        }

        #region DLL imports
        private const int VERSION_MAX_LEN = 32;
        public const string DLLNAME = @"D1801_DLL.dll";

        [DllImport(DLLNAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr D1801_CommOpen(string port);

        [DllImport(DLLNAME, CharSet = CharSet.Ansi)]
        private static extern int D1801_CommClose(IntPtr handle);

        [DllImport(DLLNAME, CharSet = CharSet.Ansi)]
        private static extern int D1801_GetSysVersion(IntPtr handle, byte MacAddr, [Out] StringBuilder version);
        
        [DllImport(DLLNAME, CharSet = CharSet.Ansi)]
        private static extern int D1801_SendCmd(IntPtr handle, byte MacAddr, string cmd, int cmdLen);

        [DllImport(DLLNAME, CharSet = CharSet.Ansi)]
        private static extern int D1801_Query(IntPtr handle, byte MacAddr, byte[] stateInfo);

        [DllImport(DLLNAME, CharSet = CharSet.Ansi)]
        private static extern int D1801_SensorQuery(IntPtr handle, byte MacAddr, byte[] stateInfo);

        #endregion DLL imports
    }
}
