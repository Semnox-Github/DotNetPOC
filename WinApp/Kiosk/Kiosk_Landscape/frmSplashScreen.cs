/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - frmSPlashScreen.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.80        4-Sep-2019       Deeksha             Added logger methods.
********************************************************************************************/
using Semnox.Parafait.KioskCore;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmSplashScreen : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmSplashScreen()
        {
            log.LogMethodEntry();
            InitializeComponent();

            if (ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage != null)
            {
                this.BackgroundImage = ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage;
            }
            if (ThemeManager.CurrentThemeImages.SplashScreenImage != null)
            {
                if (ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage == null)
                    this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;
                pbImage.Image = ThemeManager.CurrentThemeImages.SplashScreenImage;
            }//Ends:Modification on 17-Dec-2015 for introducing new theme
            KioskStatic.logToFile("frmSplashScreen()");
            log.LogMethodExit();
        }

        private void frmSplashScreen_MouseClick(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            KioskStatic.logToFile("frmSplashScreen_MouseClick()");
            log.LogMethodExit();
        }
    }
}
