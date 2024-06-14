/********************************************************************************************
 * Project Name - JobUtils
 * Description  - Aplication Loader
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/
using System;
using System.Security;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Class that allows running applications with full admin rights. In
    /// addition the application launched will bypass the Vista UAC prompt.
    /// </summary>
    public class ApplicationLoader
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Structures
        /// <summary>
        /// SECURITY_ATTRIBUTES
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            /// <summary>
            /// Length field
            /// </summary>
            public int Length;
            /// <summary>
            /// lpSecurityDescriptor field
            /// </summary>
            public IntPtr lpSecurityDescriptor;
            /// <summary>
            /// bInheritHandle field
            /// </summary>
            public bool bInheritHandle;
        }
        /// <summary>
        /// STARTUPINFO
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            /// <summary>
            /// cb field
            /// </summary>
            public int cb;
            /// <summary>
            /// lpReserved field
            /// </summary>
            public String lpReserved;
            /// <summary>
            /// lpDesktop field
            /// </summary>
            public String lpDesktop;
            /// <summary>
            /// lpTitle title field
            /// </summary>
            public String lpTitle;
            /// <summary>
            /// dwx field
            /// </summary>
            public uint dwX;
            /// <summary>
            /// dwy field
            /// </summary>
            public uint dwY;
            /// <summary>
            /// dwXSize field
            /// </summary>
            public uint dwXSize;
            /// <summary>
            /// dwYSize field
            /// </summary>
            public uint dwYSize;
            /// <summary>
            /// dwXCountChars field
            /// </summary>
            public uint dwXCountChars;
            /// <summary>
            /// dwYCountChars field
            /// </summary>
            public uint dwYCountChars;
            /// <summary>
            /// dwFillAttribute field
            /// </summary>
            public uint dwFillAttribute;
            /// <summary>
            /// dwFlags field
            /// </summary>
            public uint dwFlags;
            /// <summary>
            /// wShowWindow field
            /// </summary>
            public short wShowWindow;
            /// <summary>
            /// cbReserved2 field
            /// </summary>
            public short cbReserved2;
            /// <summary>
            /// lpReserved2 field
            /// </summary>
            public IntPtr lpReserved2;
            /// <summary>
            /// hStdInput field
            /// </summary>
            public IntPtr hStdInput;
            /// <summary>
            /// hStdOutput field
            /// </summary>
            public IntPtr hStdOutput;
            /// <summary>
            /// hStdError field
            /// </summary>
            public IntPtr hStdError;
        }
        /// <summary>
        /// PROCESS_INFORMATION
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            /// <summary>
            /// hProcess
            /// </summary>
            public IntPtr hProcess;
            /// <summary>
            /// hThread
            /// </summary>
            public IntPtr hThread;
            /// <summary>
            /// dwProcessId
            /// </summary>
            public uint dwProcessId;
            /// <summary>
            /// dwThreadId
            /// </summary>
            public uint dwThreadId;
        }

        #endregion

        #region Enumerations

        enum TOKEN_TYPE : int
        {
            TokenPrimary = 1,
            TokenImpersonation = 2
        }

        enum SECURITY_IMPERSONATION_LEVEL : int
        {
            SecurityAnonymous = 0,
            SecurityIdentification = 1,
            SecurityImpersonation = 2,
            SecurityDelegation = 3,
        }

        #endregion

        #region Constants
        /// <summary>
        /// TOKEN_DUPLICATE
        /// </summary>
        public const int TOKEN_DUPLICATE = 0x0002;
        /// <summary>
        /// MAXIMUM_ALLOWED
        /// </summary>
        public const uint MAXIMUM_ALLOWED = 0x2000000;
        /// <summary>
        /// CREATE_NEW_CONSOLE
        /// </summary>
        public const int CREATE_NEW_CONSOLE = 0x00000010;

        /// <summary>
        /// IDLE_PRIORITY_CLASS
        /// </summary>
        public const int IDLE_PRIORITY_CLASS = 0x40;
        /// <summary>
        /// NORMAL_PRIORITY_CLASS
        /// </summary>
        public const int NORMAL_PRIORITY_CLASS = 0x20;
        /// <summary>
        /// HIGH_PRIORITY_CLASS
        /// </summary>
        public const int HIGH_PRIORITY_CLASS = 0x80;
        /// <summary>
        /// REALTIME_PRIORITY_CLASS
        /// </summary>
        public const int REALTIME_PRIORITY_CLASS = 0x100;

        #endregion

        #region Win32 API Imports

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hSnapshot);

        [DllImport("kernel32.dll")]
        public static extern uint WTSGetActiveConsoleSessionId();

        /// <summary>
        /// CreateProcessAsUser
        /// </summary>
        /// <returns></returns>
        [DllImport("advapi32.dll", EntryPoint = "CreateProcessAsUser", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static bool CreateProcessAsUser(IntPtr hToken, String lpApplicationName, String lpCommandLine, ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandle, int dwCreationFlags, IntPtr lpEnvironment,
            String lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);
         
        [DllImport("kernel32.dll")]
        static extern bool ProcessIdToSessionId(uint dwProcessId, ref uint pSessionId);
        /// <summary>
        /// DuplicateTokenEx
        /// </summary>
        /// <returns></returns>
        [DllImport("advapi32.dll", EntryPoint = "DuplicateTokenEx")]
        public extern static bool DuplicateTokenEx(IntPtr ExistingTokenHandle, uint dwDesiredAccess,
            ref SECURITY_ATTRIBUTES lpThreadAttributes, int TokenType,
            int ImpersonationLevel, ref IntPtr DuplicateTokenHandle);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("advapi32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        static extern bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, ref IntPtr TokenHandle);

        #endregion

        /// <summary>
        /// Launches the given application with full admin rights, and in addition bypasses the Vista UAC prompt
        /// </summary>
        /// <param name="applicationName">The name of the application to launch</param>
        /// <param name="procInfo">Process information regarding the launched application that gets returned to the caller</param>
        /// <returns></returns>
        public static bool StartProcessAndBypassUAC(String applicationName, out PROCESS_INFORMATION procInfo)
        {
            log.LogMethodEntry(applicationName);
            uint winlogonPid = 0;
            IntPtr hUserTokenDup = IntPtr.Zero, hPToken = IntPtr.Zero, hProcess = IntPtr.Zero;
            procInfo = new PROCESS_INFORMATION();
            log.LogVariableState("procInfo", procInfo);

            // obtain the currently active session id; every logged on user in the system has a unique session id
            uint dwSessionId = WTSGetActiveConsoleSessionId();
            log.Debug("Session Id " + dwSessionId);
            // obtain the process id of the winlogon process that is running within the currently active session
            Process[] processes = Process.GetProcessesByName("winlogon");
            log.LogVariableState("processes", processes);

            foreach (Process p in processes)
            {
                log.Debug("Process Session Id" + (uint)p.SessionId);
                if ((uint)p.SessionId == dwSessionId)
                {
                    winlogonPid = (uint)p.Id;
                    log.Debug("winlogonPid Id " + winlogonPid + " for Process Id " + (uint)p.Id);
                }
            }

            // obtain a handle to the winlogon process
            hProcess = OpenProcess(MAXIMUM_ALLOWED, false, winlogonPid);
            log.LogVariableState("hProcess", hProcess);
            // obtain a handle to the access token of the winlogon process
            if (!OpenProcessToken(hProcess, TOKEN_DUPLICATE, ref hPToken))
            {
                log.Debug("OpenProcessToken Failure");
                CloseHandle(hProcess);
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                log.Debug("OpenProcessToken Success");
            }
            log.LogVariableState("OpenProcessToken ref HPToken", hPToken);
            // Security attibute structure used in DuplicateTokenEx and CreateProcessAsUser
            // I would prefer to not have to use a security attribute variable and to just 
            // simply pass null and inherit (by default) the security attributes
            // of the existing token. However, in C# structures are value types and therefore
            // cannot be assigned the null value.
            SECURITY_ATTRIBUTES sa = new SECURITY_ATTRIBUTES();
            sa.Length = Marshal.SizeOf(sa);
            log.LogVariableState("SECURITY_ATTRIBUTES", sa);
            // copy the access token of the winlogon process; the newly created token will be a primary token
            if (!DuplicateTokenEx(hPToken, MAXIMUM_ALLOWED, ref sa, (int)SECURITY_IMPERSONATION_LEVEL.SecurityIdentification, (int)TOKEN_TYPE.TokenPrimary, ref hUserTokenDup))
            {
                log.Debug("DuplicateTokenEx Failure");
                CloseHandle(hProcess);
                CloseHandle(hPToken);
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                log.Debug("DuplicateTokenEx Success");
            }

            // By default CreateProcessAsUser creates a process on a non-interactive window station, meaning
            // the window station has a desktop that is invisible and the process is incapable of receiving
            // user input. To remedy this we set the lpDesktop parameter to indicate we want to enable user 
            // interaction with the new process.
            STARTUPINFO si = new STARTUPINFO();
            si.cb = (int)Marshal.SizeOf(si);
            si.lpDesktop = @"winsta0\default"; // interactive window station parameter; basically this indicates that the process created can display a GUI on the desktop

            // flags that specify the priority and creation method of the process
            int dwCreationFlags = NORMAL_PRIORITY_CLASS | CREATE_NEW_CONSOLE;
            log.Debug("dwCreationFlags" + dwCreationFlags);
            // create a new process in the current user's logon session
            bool result = CreateProcessAsUser(hUserTokenDup,        // client's access token
                                            null,                   // file to execute
                                            applicationName,        // command line
                                            ref sa,                 // pointer to process SECURITY_ATTRIBUTES
                                            ref sa,                 // pointer to thread SECURITY_ATTRIBUTES
                                            false,                  // handles are not inheritable
                                            dwCreationFlags,        // creation flags
                                            IntPtr.Zero,            // pointer to new environment block 
                                            null,                   // name of current directory 
                                            ref si,                 // pointer to STARTUPINFO structure
                                            out procInfo            // receives information about new process
                                            );
            int iError = Marshal.GetLastWin32Error();
            log.Debug("CreateProcessAsUser Error Code " + iError);
            log.Debug("CreateProcessAsUser Result " + result);
            log.LogVariableState("CreateProcessAsUser STARTUPINFO ref result", si);
            log.LogVariableState("CreateProcessAsUser output result", procInfo);
            // invalidate the handles
            CloseHandle(hProcess);
            CloseHandle(hPToken);
            CloseHandle(hUserTokenDup);

            log.LogMethodExit(result);
            return result; // return the result
        } 
        /// <summary>
        /// IsAdministrator
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            log.LogMethodEntry();
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            log.LogMethodExit(isAdmin);
            return isAdmin;
        }
    }
}