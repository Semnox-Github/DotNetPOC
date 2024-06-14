/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - frmThankYou 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.80        4-Sep-2019       Deeksha             Added logger methods.
 *2.80.1     02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 *2.130.0    30-Jun-2021     Dakshak             Theme changes to support customized Font ForeColor
********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmThankYou : BaseForm
    {
        public frmThankYou(bool receiptPrinted)
        {
            log.LogMethodEntry(receiptPrinted);
            InitializeComponent();
            this.Size = this.BackgroundImage.Size;
            this.BackgroundImage = KioskStatic.CurrentTheme.TapCardBackgroundImage;
            button1.Text = KioskStatic.Utilities.MessageUtils.getMessage(499);

            if (receiptPrinted)
                lblMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(498);
            else
                lblMessage.Text = "";

            KioskStatic.setDefaultFont(this);
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

        //private void timeoutTimer_Tick(object sender, EventArgs e)
        //{
        //    Close();
        //    Dispose();
        //}
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
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblMessage.ForeColor = KioskStatic.CurrentTheme.ThankYouScreenHeader1TextForeColor;
                this.button1.ForeColor = KioskStatic.CurrentTheme.ThankYouScreenHeader2TextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.ThankYouScreenBtnPrevTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
