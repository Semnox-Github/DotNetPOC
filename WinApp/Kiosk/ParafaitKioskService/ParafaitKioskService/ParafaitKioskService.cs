using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Timers;
using System.IO;
using System.Configuration;
using Semnox.Parafait.JobUtils;
using System.Threading;

namespace ParafaitKiosk
{
    public partial class ParafaitKioskService : ServiceBase
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string className, string windowTitle);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int index);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        private const int GWL_STYLE = -16;
        private const int WS_VISIBLE = 0x10000000;

        private EventLog eventLog;
        private string applicationWithPath;
        private string applicationName;
        private string applicationDirectory;
        private System.Timers.Timer timer;
        private System.Timers.Timer keyBoardTimer;
        public ParafaitKioskService()
        {
            log.LogMethodEntry();
            InitializeComponent();
            this.ServiceName = "Parafait Kiosk Service";
            timer = new System.Timers.Timer(30000);  //30000 milliseconds = 30 seconds//300000 milliseconds = 5min
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(StartProcesses);
            keyBoardTimer = new System.Timers.Timer(5000); //5 seconds
            keyBoardTimer.AutoReset = true;
            keyBoardTimer.Elapsed += new ElapsedEventHandler(CheckExeRunStatus);
            // These Flags set whether or not to handle that specific
            //  type of event. Set to true if you need it, false otherwise.
            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
            log.LogMethodExit();
        }

        protected override void OnStart(string[] args)
        {
            log.LogMethodEntry(args);
            log.LogVariableState("args", args);
            timer.Start();            
            eventLog = new EventLog();
            eventLog.Source = ServiceName;
            eventLog.Log = "Application";
            if(args != null && args.Length > 0)
                applicationWithPath = args[0];
            else
            {
                applicationWithPath = ConfigurationManager.AppSettings["StartUpPath"];
            }
            log.LogVariableState("applicationWithPath", applicationWithPath);
            keyBoardTimer.Start();
            log.LogMethodExit();
        }

        protected override void OnStop()
        {
            log.LogMethodEntry();
            try
            {
                WriteLog("Service stopped", EventLogEntryType.Error);
                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                }
                if (keyBoardTimer != null)
                {
                    keyBoardTimer.Stop();
                    keyBoardTimer.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        
        private void StartProcesses(object sender, ElapsedEventArgs e)
        {
            try
            {
                //Debugger.Launch();
                WriteLog("Service Started", EventLogEntryType.Information);
                applicationName = System.IO.Path.GetFileNameWithoutExtension(applicationWithPath);
                if (!string.IsNullOrEmpty(applicationName))
                {
                    WriteLog("Application Name to start " + applicationName, EventLogEntryType.Information);
                    applicationDirectory = System.IO.Path.GetDirectoryName(applicationWithPath);
                    WriteLog("Application Path " + applicationDirectory, EventLogEntryType.Information);
                    if (!isProcessRunning(applicationName))
                    {
                        try
                        {
                            ApplicationLoader.PROCESS_INFORMATION procInfo;
                            ApplicationLoader.StartProcessAndBypassUAC(applicationWithPath, out procInfo);
                            int processId = Convert.ToInt32(procInfo.dwProcessId);
                            if (processId > 0)
                            {
                                string msg = String.Format(
                              "Parafait Kiosk Application Started {0} {1} Procees Id {2}", applicationName, "\"" + applicationDirectory + "\"", processId.ToString());
                                WriteLog(msg, EventLogEntryType.Information);
                                log.Info(msg);
                            }
                            else
                            {
                                throw new Exception("Error starting Kiosk "+ applicationName+ " from path " + applicationName);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                else
                {
                    WriteLog("Start up argument is empty, please pass Kiosk Application folder path.", EventLogEntryType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in StartProcesses", ex);
                WriteLog("Error while calling StartProcesses, " + ex, EventLogEntryType.Error);
                this.ExitCode = -1;
                Environment.Exit(-1);
            }
        }
        private void WriteLog(string message, EventLogEntryType logType)
        {
            eventLog.WriteEntry(message, logType);
        }

        private bool isProcessRunning(string processName)
        {
            WriteLog("Check if Process " + processName + " is running.", EventLogEntryType.Information);
            Process[] procs = Process.GetProcessesByName(processName);
            if (procs.Length == 0)
            {
                WriteLog(processName + " is not running", EventLogEntryType.Information);
                return false;
            }
            else if (procs.Length > 1)
            {
                WriteLog("Multiple instances of " + processName + "found.", EventLogEntryType.Error);
                return true;
            }
            else
            {
                WriteLog("Process " + processName + " is running.", EventLogEntryType.Information);
                return true;
            }
        }


        private void CheckExeRunStatus(object sender, ElapsedEventArgs e)
        {
            //log.LogMethodEntry();
            try
            {
                if (!string.IsNullOrEmpty(applicationName))
                {
                    if (!isProcessRunning(applicationName))
                    {
                        ShowTaskbar();
                    }
                }
                else
                {
                    ShowTaskbar();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            //log.LogMethodExit();
        }
        private void ShowTaskbar()
        {
            log.LogMethodEntry();
            try
            {
                IntPtr taskbarHWnd = FindWindow("Shell_TrayWnd", null);
                if (taskbarHWnd != IntPtr.Zero)
                {
                    int style = GetWindowLong(taskbarHWnd, GWL_STYLE);
                    bool taskbarVisible = (style & WS_VISIBLE) != 0;
                    if (taskbarVisible == false)
                    {
                        ShowWindow(taskbarHWnd, SW_SHOW);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
