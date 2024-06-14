/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Program cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Semnox.Parafait.KioskCore;
using System.Threading;

namespace Redemption_Kiosk
{
    static class Program
    {
        //static int restartCount = 0;
        static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string className, string windowTitle);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
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
                if (!StaticUtils.CheckIfProgramAlreadyRunning("Redemption Kiosk", true))
                {
                    try
                    {
                        Logger.setRootLogLevel(log);
                        log.Info("Start Program");
                        Common.InitEnv();
                        Application.Run(new frmRedemptionKioskSplashScreen());
                    }
                    finally
                    {
                        ShowTaskbar();
                    }
                }
            }
            catch (Exception ex)
            {
               log.Error(ex);
                //if (restartCount++ < 3)
               // {
               //     log.Info("Restarting...");
               //     Application.Restart();
               // }
            }
            finally
            {
                ShowTaskbar();
            }
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
