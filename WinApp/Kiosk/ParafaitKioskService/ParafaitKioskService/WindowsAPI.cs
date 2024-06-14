using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ParafaitKiosk
{
    public class WindowsAPI
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFO
        {
            public uint cb;
            public string reserved;
            public string desktop;
            public string title;
            public uint x;
            public uint y;
            public uint xSize;
            public uint ySize;
            public uint xCountChars;
            public uint yCountChars;
            public uint fillAttribute;
            public uint flags;
            public short showWindow;
            public short reserved2;
            public IntPtr reserved3;
            public IntPtr stdInput;
            public IntPtr stdOutput;
            public IntPtr stdError;
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint WTSGetActiveConsoleSessionId();

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSQueryUserToken(UInt32 sessionId, out IntPtr Token);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CreateProcessAsUser(
                            IntPtr hToken,
                            string lpApplicationName,
                            string lpCommandLine,
                            ref SECURITY_ATTRIBUTES lpProcessAttributes,
                            ref SECURITY_ATTRIBUTES lpThreadAttributes,
                            bool bInheritHandles,
                            uint dwCreationFlags,
                            IntPtr lpEnvironment,
                            string lpCurrentDirectory,
                            ref STARTUPINFO lpStartupInfo,
                            out PROCESS_INFORMATION lpProcessInformation);
    }
}
