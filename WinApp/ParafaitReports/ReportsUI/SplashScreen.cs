/********************************************************************************************
* Project Name - Parafait Report
* Description  - SplashScreen 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80       26-Aug-2019      Jinto Thomas        Added logger into methods
********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Parafait.Report.Reports
{

    /// <summary>
    /// SplashScreen Class
    /// </summary>
    public partial class SplashScreen : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SplashScreen
        /// </summary>
        public SplashScreen()
        {
            log.LogMethodEntry();
            InitializeComponent();
            try
            {
                this.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\Resources\\Parafait-Start-Splash.png");
                this.ClientSize = this.BackgroundImage.Size;
                log.LogMethodExit();
            }
            catch { }
        }

        /// <summary>
        /// CloseSplash() method
        /// </summary>
        public void CloseSplash()
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }
    }
}