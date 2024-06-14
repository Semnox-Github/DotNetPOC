/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmYesNo 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.80.1     02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 *2.130.0    30-Jun-2021      Dakshak              Theme changes to support customized Font ForeColor
********************************************************************************************/
using System;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmYesNo : BaseForm
    {
        //int ticks = 0;
        public frmYesNo(string message, string additionalMessage = "")
        {
            log.LogMethodEntry(message, additionalMessage);
            InitializeComponent();
            this.Size = this.BackgroundImage.Size;
            Semnox.Parafait.KioskCore.KioskStatic.setDefaultFont(this);
            this.BackgroundImage = Semnox.Parafait.KioskCore.KioskStatic.CurrentTheme.TapCardBackgroundImage;
            SetCustomizedFontColors();

            lblmsg.Text = message;
            lblAdditionalMessage.Text = additionalMessage;
            //timer1.Start();
            KioskTimerInterval(5000);
            log.LogMethodExit();
        }

     
        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }
        private void btnNo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            Close();
            log.LogMethodExit();
        }

        private void btnYes_MouseDown(object sender, MouseEventArgs e)
        {
            // btnYes.BackgroundImage = Properties.Resources.button_pressed;
        }

        private void btnNo_MouseDown(object sender, MouseEventArgs e)
        {
            // btnNo.BackgroundImage = Properties.Resources.button_pressed;
        }

        private void btnYes_MouseUp(object sender, MouseEventArgs e)
        {
            // btnYes.BackgroundImage = Properties.Resources.button_normal;
        }

        private void btnNo_MouseUp(object sender, MouseEventArgs e)
        {
            // btnNo.BackgroundImage = Properties.Resources.button_normal;
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            Semnox.Parafait.KioskCore.KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblmsg.ForeColor = Semnox.Parafait.KioskCore.KioskStatic.CurrentTheme.YesNoScreenMessageTextForeColor;//Would you like to register?
                this.btnYes.ForeColor = Semnox.Parafait.KioskCore.KioskStatic.CurrentTheme.YesNoScreenBtnYesTextForeColor;//Button Yes
                this.btnNo.ForeColor = Semnox.Parafait.KioskCore.KioskStatic.CurrentTheme.YesNoScreenBtnNoTextForeColor;//Button No
                this.lblAdditionalMessage.ForeColor = Semnox.Parafait.KioskCore.KioskStatic.CurrentTheme.YesNoScreenLblAdditionalMessageTextForeColor;//Additional Message
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                Semnox.Parafait.KioskCore.KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
