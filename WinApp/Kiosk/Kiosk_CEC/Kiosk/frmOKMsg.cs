/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - frmOKMsg.cs
 * 
 **************
 **Version Log
 ************** 
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.80.1     02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 *2.130.0    30-Jun-2021      Dakshak             Theme changes to support customized Font ForeColor
 ********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk
{
    public partial class frmOKMsg : BaseForm
    {
        //int ticks = 0;
        public frmOKMsg(string message, bool enableTimeOut = true)
        {
            log.LogMethodEntry(message, enableTimeOut);
            InitializeComponent();
            this.Size = this.BackgroundImage.Size;
            KioskStatic.Utilities.setLanguage(this);
            Semnox.Parafait.KioskCore.KioskStatic.setDefaultFont(this);
            this.BackgroundImage = KioskStatic.CurrentTheme.TapCardBackgroundImage;
            lblmsg.Text = message;
            //if (enableTimeOut)
            //timer1.Start();
            //--Legacy/Open Port Cleanup
            if(!enableTimeOut)
            {
                StopKioskTimer();
            }
            SetCustomizedFontColors();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            if (tickSecondsRemaining < 20)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }
            else
                setKioskTimerSecondsValue(tickSecondsRemaining - 1);
            log.LogMethodExit();
        }

        private void btnClose_MouseDown(object sender, MouseEventArgs e)
        {
            //btnClose.BackgroundImage = Properties.Resources.cancel_btn_pressed;
        }

        private void btnClose_MouseUp(object sender, MouseEventArgs e)
        {
            //btnClose.BackgroundImage = Properties.Resources.cancel_btn;
        }

        private void frmOKMsg_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            //timer1.Stop();
            //if (!GetKioskTimer())
            //{
            //    StartKioskTimer();
            //}
            StopKioskTimer();
            log.LogMethodExit();
        }

        private void frmOKMsg_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(lblmsg.Text))
                lblmsg.Text = this.Text;
            log.LogMethodEntry();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.OkMsgScreenHeaderTextForeColor;
                this.btnClose.ForeColor = KioskStatic.CurrentTheme.OkMsgScreenBtnCloseTextForeColor;
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
