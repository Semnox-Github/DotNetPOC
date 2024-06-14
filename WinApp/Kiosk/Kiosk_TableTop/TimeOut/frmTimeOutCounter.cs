/********************************************************************************************
* Project Name - Parafait_Kiosk - frmTimeOut
* Description  - frmTimeOut.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmTimeOutCounter : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Timer timer;
        int timeOut = 21;

        public frmTimeOutCounter()
        {
            log.LogMethodEntry();
            InitializeComponent();
            timer = new Timer();
            this.BackgroundImage = ThemeManager.CurrentThemeImages.CountdownTimer;
            KioskStatic.setDefaultFont(this);//Starts:Modification on 17-Dec-2015 for introducing new theme
            SetCustomizedFontColors();
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmTimeOutCounter_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            timer.Interval = 500;
            timer.Tick += timer_Tick;
            timer.Start();

            lblTimeOut.Text = (timeOut / 2).ToString();
            log.LogMethodExit();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            timeOut--;

            if (timeOut == 1)
            {
                this.Hide();
                DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }

            lblTimeOut.Text = (timeOut / 2).ToString();
            log.LogMethodExit();
        }

        private void frmTimeOutCounter_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            timer.Stop();
            log.LogMethodExit();
        }

        private void frmTimeOutCounter_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = System.Windows.Forms.DialogResult.Abort;
            Close();
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmTimeoutCounter");
            try
            {
                this.label1.ForeColor = KioskStatic.CurrentTheme.TimeOutCounterHeader1TextForeColor;//(Do you need more time?)
                this.lblTimeOut.ForeColor = KioskStatic.CurrentTheme.TimeOutCounterTimerTextForeColor;// --(Time)
                this.label2.ForeColor = KioskStatic.CurrentTheme.TimeOutCounterHeader2TextForeColor;//(Touch anywhere on screen to continue)
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmTimeoutCounter: " + ex.Message);
            }
            log.LogMethodExit();
        }

    }
}
