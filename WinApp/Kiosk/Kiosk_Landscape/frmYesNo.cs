/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmYesNo 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80        4-Sep-2019      Deeksha             Added logger methods.
 *2.90.0       23-Jun-2020     Dakshakh raj       Payment Modes based on Kiosk Configuration set up
*2.150.1       22-Feb-2023     Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Data;
using System.Windows.Forms;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Device.PaymentGateway;

namespace Parafait_Kiosk
{
    public partial class frmYesNo : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public frmYesNo(string message, string additionalMessage = "")
        {
            log.LogMethodEntry(message, additionalMessage);
            InitializeComponent();
            this.BackgroundImage = ThemeManager.CurrentThemeImages.YesorNoFormBackgroundBox;          
            btnNo.BackgroundImage = btnYes.BackgroundImage = ThemeManager.CurrentThemeImages.CloseButton;           
            KioskStatic.setDefaultFont(this);
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
            try
            {
                this.btnNo.Enabled = false;
                this.btnYes.Enabled = false;
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }
            finally
            {
                btnNo.Enabled = btnYes.Enabled = true;
            }
            log.LogMethodExit();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.btnNo.Enabled = false;
                this.btnYes.Enabled = false;
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                Close();
            }
            finally
            {
                btnNo.Enabled = btnYes.Enabled = true;
            }
            log.LogMethodExit();
        }

        private void btnYes_MouseDown(object sender, MouseEventArgs e)
        {
            //btnYes.BackgroundImage = Properties.Resources.button_pressed;
        }

        private void btnNo_MouseDown(object sender, MouseEventArgs e)
        {
            //btnNo.BackgroundImage = Properties.Resources.button_pressed;
        }

        private void btnYes_MouseUp(object sender, MouseEventArgs e)
        {
            //btnYes.BackgroundImage = Properties.Resources.button_normal;
        }

        private void btnNo_MouseUp(object sender, MouseEventArgs e)
        {
            //btnNo.BackgroundImage = Properties.Resources.button_normal;
        }

    }
}
