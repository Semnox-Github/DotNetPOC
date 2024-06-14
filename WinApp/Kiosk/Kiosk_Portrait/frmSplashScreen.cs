/********************************************************************************************
* Project Name - Parafait_Kiosk - frmSplashScreen
* Description  - frmSplashScreen 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.80        05-Sep-2019      Deeksha            Added logger methods.
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Parafait.KioskCore;
using System;
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
            this.BackgroundImage = (ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage != null
                                          ? ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage 
                                          : ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage);
            pbForeImage.Image = ThemeManager.CurrentThemeImages.SplashScreenImage;
            label1.Visible = false;
            SetCustomizedFontColors();
            log.LogMethodExit();
        }

        private void frmSplashScreen_MouseClick(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            KioskStatic.logToFile("frmSplashScreen_MouseClick()");
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmSplashScreen");
            try
            {
                this.label1.ForeColor = KioskStatic.CurrentTheme.FrmSplashScreenLabel1TextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmSplashScreens: " + ex.Message);
            }
            log.LogMethodExit();
        }

    }
}
