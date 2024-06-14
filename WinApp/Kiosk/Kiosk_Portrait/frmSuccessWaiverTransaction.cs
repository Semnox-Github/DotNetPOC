﻿/********************************************************************************************
* Project Name - Parafait_Kiosk - frmSuccessWaiverTransaction
* Description  - Image illustration of waiver signing pending 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.1.0   28-Dec-2023      Sathyavathi        Enable waiver product sale in Kiosk
********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmSuccessWaiverTransaction : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //int ticks = 0;
        public frmSuccessWaiverTransaction(string message, string point, string trxNo)
        {
            log.LogMethodEntry(message, point, trxNo);
            InitializeComponent();
            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;

            this.Size = this.BackgroundImage.Size;
            btnClose.BackgroundImage = ThemeManager.CurrentThemeImages.DoneButton;
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            KioskStatic.setDefaultFont(this);
            KioskStatic.Utilities.setLanguage(this);
            SetCustomizedFontColors();

            if (!string.IsNullOrEmpty(message))
            {
                lblmsg.Text = message;
                lblmsg.Visible = true;
            }

            if (!string.IsNullOrEmpty(point))
            {
                lblPoint.Text = point;
                lblPoint.Visible = true;
            }
            lblHeading.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "SUCCESS");
            pbSuccess.Image = KioskStatic.CurrentTheme.WaiverSigningInstructions;
            pbSuccess.Visible = true;
            if (!string.IsNullOrEmpty(trxNo))
            {
                lblTrxNumber.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4470, trxNo); //Trx Number : &1
                lblTrxNumber.Visible = true;
            }
            //timer1.Start();

            int successScreenTimeout = ParafaitDefaultContainerList.GetParafaitDefault<int>(KioskStatic.Utilities.ExecutionContext, "SUCCESS_SCREEN_TIMEOUT", 5);
            KioskTimerInterval(successScreenTimeout * 1000); //parafait default valuee will be in sec, we need to pass it in ms

            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void frmSuccessWaiverTransaction_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timer1.Stop();
            // StopKioskTimer();
        }

        private void frmSuccessWaiverTransaction_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(lblmsg.Text))
                lblmsg.Text = this.Text;
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            try
            {
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgBtnHomeTextForeColor;
                this.lblHeading.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgLblHeadingTextForeColor;
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgLblmsgTextForeColor;
                this.lblPoint.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgLblPointTextForeColor;
                this.btnClose.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgBtnCloseTextForeColor;
                this.lblTrxNumber.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgLblTrxNumberTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmSuccessWaiverTransaction : " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
