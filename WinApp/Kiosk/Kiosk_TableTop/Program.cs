/********************************************************************************************
* Project Name - Parafait_Kiosk - Program
* Description  - Program 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk
{
    static class Program
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string className, string windowTitle);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        //static int restartCount = 0;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        static void Main()
        {
            log.LogMethodEntry();

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                HideTaskbar();
                if (!StaticUtils.CheckIfProgramAlreadyRunning("Parafait Kiosk", true))
                {
                    try
                    {
                        VirtualWindowsKeyboardController.ClearHelpPaneSettings();
                        Application.Run(new frmHome());
                    }
                    finally
                    {
                        VirtualWindowsKeyboardController.RestoreHelpPaneSettings();
                        ShowTaskbar();
                    }
                    //Application.Run(new frmHome());
                }
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message);
                KioskStatic.logToFile(ex.StackTrace);
                //if (restartCount++ < 3)
                //{
                //    KioskStatic.logToFile("Restarting...");
                //    Application.Restart();
                //}
            }
            finally
            {
                ShowTaskbar();
            }
            log.LogMethodExit();
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            log.LogMethodEntry();
            log.Error("Unhandled exception occured", e.Exception);
            try
            {
                if (e.Exception is Exception)
                {
                    KioskStatic.logToFile("Application_ThreadException occured:" + e.Exception.Message);
                    KioskStatic.logToFile(" Application_ThreadException occured:" + e.Exception.StackTrace);
                }
            }
            catch (Exception ex)
            {
                log.Error("In Application_ThreadException", ex);
            }
            log.LogMethodExit();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.ExceptionObject is Exception)
                {
                    log.Error("Unhandled exception occured", e.ExceptionObject as Exception);
                    try
                    {

                        KioskStatic.logToFile("CurrentDomain_UnhandledException occured:" + (e.ExceptionObject as Exception).Message);
                        KioskStatic.logToFile("CurrentDomain_UnhandledException occured:" + (e.ExceptionObject as Exception).StackTrace);
                    }
                    catch (Exception ex)
                    {
                        log.Error("In CurrentDomain_UnhandledException", ex);
                    }
                }
                else
                {
                    log.Error("Unhandled exception occured" + e.ExceptionObject.ToString());
                    try
                    {

                        KioskStatic.logToFile("CurrentDomain_UnhandledException occured:" + e.ExceptionObject.ToString());
                    }
                    catch (Exception ex)
                    {
                        log.Error("In CurrentDomain_UnhandledException", ex);
                    }
                }
                ShowTaskbar();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }        
            log.LogMethodExit();
        }

        private static void HideTaskbar()
        {
            log.LogMethodEntry();
            try
            {
                IntPtr hWnd = FindWindow("Shell_TrayWnd", "");
                ShowWindow(hWnd, SW_HIDE);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        internal static void ShowTaskbar()
        {
            log.LogMethodEntry();
            try
            {
                IntPtr hWnd = FindWindow("Shell_TrayWnd", "");
                ShowWindow(hWnd, SW_SHOW);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
