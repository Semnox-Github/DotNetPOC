using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk
{
    static class Program
    {
        //static int restartCount = 0;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
                if (!StaticUtils.CheckIfProgramAlreadyRunning("Parafait Kiosk", true))
                    Application.Run(new FSKCoverPage());

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
            log.LogMethodExit();
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            log.LogMethodEntry();
            log.Error("Unhandled exception occured", e.Exception);
            log.LogMethodExit();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ExceptionObject is Exception)
            {
                log.Error("Unhandled exception occured", e.ExceptionObject as Exception);
            }
            else
            {
                log.Error("Unhandled exception occured" + e.ExceptionObject.ToString());
            }
            log.LogMethodExit();
        }
    }
}
