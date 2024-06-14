using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    static class Program
    {
        //static int restartCount = 0;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                if (!StaticUtils.CheckIfProgramAlreadyRunning("Parafait FnB Kiosk", true))
                {
                    Common.log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace);
                    Logger.setRootLogLevel(Common.log);
                    Common.logToFile("Start Program"); 
                    Common.initEnv();
                    Application.Run(new frmSplash());
                }
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                //if (restartCount++ < 3)
                //{
                //    Common.logToFile("Restarting...");
                //    Application.Restart();
               // }
            }
        }
    }
}
