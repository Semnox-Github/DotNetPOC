/********************************************************************************************
* Project Name - Parafait_Kiosk - frmYesNo
* Description  - frmYesNo 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Data;
using System.Windows.Forms;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Product;
using System.Collections.Generic;

namespace Parafait_Kiosk
{
    public partial class frmYesNo : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string SetYesButtonText { set { this.btnYes.Text = value; } }
        public string SetNoButtonText { set { this.btnNo.Text = value; } }
        public frmYesNo(string message, string additionalMessage = "", int timeout = 10)
        {
            log.LogMethodEntry(message, additionalMessage);
            InitializeComponent();
            this.BackgroundImage = ThemeManager.GetPopupBackgroundImage(ThemeManager.CurrentThemeImages.YesNoBox);
            //this.Size = this.BackgroundImage.Size;
            btnNo.BackgroundImage = btnYes.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.YesNoButtons);
            KioskStatic.setDefaultFont(this); 

            if (string.IsNullOrEmpty(additionalMessage))
            {
                lblmsg.Location = new System.Drawing.Point(lblmsg.Location.X, 10);
                lblmsg.MinimumSize = new System.Drawing.Size(968, 116);
                lblmsg.MaximumSize = new System.Drawing.Size(968, 0); 
                lblmsg.AutoSize = true;
                lblmsg.BringToFront();
            }
            lblmsg.Text = message;
            if (string.IsNullOrEmpty(additionalMessage))
            {
                lblmsg.AutoSize = false;
                lblmsg.MinimumSize = new System.Drawing.Size(968, 116);
                lblmsg.Size = new System.Drawing.Size(968, lblmsg.Height + lblAdditionalMessage.Height);
                lblAdditionalMessage.Visible = false;
                lblmsg.BringToFront();
            }
            lblmsg.Text = message;
            lblAdditionalMessage.Text = additionalMessage;
            //timer1.Start();
            KioskTimerInterval(timeout * 1000);
            SetCustomizedFontColors();
            KioskStatic.Utilities.setLanguage(this);
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
                this.btnNo.Enabled = true;
                this.btnYes.Enabled = true;
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
                this.btnNo.Enabled = true;
                this.btnYes.Enabled = true;
            }
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
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmYesNo");
            try
            {
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.YesNoScreenMessageTextForeColor;//Would you like to register?
                this.btnYes.ForeColor = KioskStatic.CurrentTheme.YesNoScreenBtnYesTextForeColor;//Button Yes
                this.btnNo.ForeColor = KioskStatic.CurrentTheme.YesNoScreenBtnNoTextForeColor;//Button No
                this.lblAdditionalMessage.ForeColor = KioskStatic.CurrentTheme.YesNoScreenLblAdditionalMessageTextForeColor;//Additional Message
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmYesNo: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
