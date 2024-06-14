/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - frmSplashScreen.cs
 * 
 **************
 **Version Log
 ************** 
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
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
            if (KioskStatic.CurrentTheme.SplashScreenImage != null)
            {
                this.BackgroundImage = KioskStatic.CurrentTheme.SplashScreenImage;
            }
            else
            {
                this.BackgroundImage = KioskStatic.CurrentTheme.DefaultBackgroundImage;
            }
            KioskStatic.logToFile("frmSplashScreen()");
            log.LogMethodExit();
        } 

        protected override CreateParams CreateParams
        {
            //this method is used to avoid the screen flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
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
