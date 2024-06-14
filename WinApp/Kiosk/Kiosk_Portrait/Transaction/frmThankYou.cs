/********************************************************************************************
* Project Name - Parafait_Kiosk -ThankYou.cs
* Description  - ThankYou 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
*2.130.0       09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.150.1       22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmThankYou : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmThankYou(bool receiptPrinted, string message = "")
        {
            log.LogMethodEntry(receiptPrinted);
            InitializeComponent();
            this.Size = this.BackgroundImage.Size;
            button1.Text = KioskStatic.Utilities.MessageUtils.getMessage(499);

            if (receiptPrinted)
                lblMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(498);
            else
                lblMessage.Text = "";

            if (!string.IsNullOrEmpty(message))
            {
                string option = KioskStatic.Utilities.getParafaitDefaults("TRX_AUTO_PRINT_AFTER_SAVE");
                KioskStatic.logToFile("TRX_AUTO_PRINT_AFTER_SAVE: " + option);
                if (!option.Equals("A"))
                {
                    lblMessage.Text = message;
                }
            }

            KioskStatic.setDefaultFont(this);//Starts:Modification on 17-Dec-2015 for introducing new theme
            KioskStatic.Utilities.setLanguage(this);
            this.BackgroundImage = ThemeManager.CurrentThemeImages.TapCardBox;
            btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.CloseButton;//Ends:Modification on 17-Dec-2015 for introducing new theme
            KioskTimerInterval(5000);
            SetCustomizedFontColors();

            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            Dispose();
            log.LogMethodExit();
        }

        private void btnPrev_MouseUp(object sender, MouseEventArgs e)
        {
            //btnPrev.BackgroundImage = Properties.Resources.cancel_btn;
        }

        private void btnPrev_MouseDown(object sender, MouseEventArgs e)
        {
           // btnPrev.BackgroundImage = Properties.Resources.cancel_btn_pressed;
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmThankYou");
            try
            {
                this.lblMessage.ForeColor = KioskStatic.CurrentTheme.ThankYouScreenHeader1TextForeColor;
                this.button1.ForeColor = KioskStatic.CurrentTheme.ThankYouScreenHeader2TextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.ThankYouScreenBtnPrevTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmThankYou: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
