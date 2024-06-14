/********************************************************************************************
* Project Name - Parafait_Kiosk - frmOKMsg
* Description  - frmOKMsg 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
*2.155.0     23-Aug-2023      Vignesh Bhat        Modified to support Smaller popup message 
 ********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmOKMsg : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal string SetButtonText { set { this.btnClose.Text = value; } }

        public frmOKMsg(string message, bool enableTimeOut = true, bool reduceHeight = false)
        {
            log.LogMethodEntry(message, enableTimeOut);
            InitializeComponent();
            this.BackgroundImage = ThemeManager.GetPopupBackgroundImage(ThemeManager.CurrentThemeImages.OkMsgBox);//Modification on 17-Dec-2015 for introducing new theme
            btnClose.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.OkMsgButtons);//Modification on 17-Dec-2015 for introducing new theme
            this.Size = this.BackgroundImage.Size;
            if (reduceHeight)
            {
                RepositionUIElements();
            }
            KioskStatic.setDefaultFont(this);//Modification on 17-Dec-2015 for introducing new theme

            lblmsg.Text = message;
            //if (enableTimeOut)
            //    timer1.Start();
            if (!enableTimeOut)
            {
                StopKioskTimer();
            }
            SetCustomizedFontColors();
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void RepositionUIElements()
        {
            log.LogMethodEntry();
            this.SuspendLayout();
            this.BackgroundImage = ThemeManager.CurrentThemeImages.OkMsgBoxShort;
            this.Size = this.BackgroundImage.Size;
            int btnHeight = (int)(btnClose.Height * 0.7);
            btnClose.Size = new System.Drawing.Size(btnClose.Width, btnHeight);
            int closeBtnYLoc = this.Height - btnClose.Height - 20;
            btnClose.Location = new System.Drawing.Point((this.Size.Width / 2 - this.btnClose.Width / 2) - 5, closeBtnYLoc);
            lblmsg.Size = new System.Drawing.Size((this.Size.Width - 20), (this.Height - btnClose.Height - 50));
            this.lblmsg.Location = new System.Drawing.Point(this.lblmsg.Location.X - 2, this.lblmsg.Location.Y);
            this.ResumeLayout(true);
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
            {
                lblmsg.Text = this.Text;
            }
            Application.DoEvents();
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmOkMsg");
            try
            {
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.OkMsgScreenHeaderTextForeColor;
                this.btnClose.ForeColor = KioskStatic.CurrentTheme.OkMsgScreenBtnCloseTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmOkMsg: " + ex.Message);
            }
            log.LogMethodExit();
        }

        public static void ShowUserMessage(string message)
        {
            using (frmOKMsg frm = new frmOKMsg(message))
            {
                frm.ShowDialog();
            }
        }
        public static void ShowOkMessage(string message, bool enableTimeOut)
        {
            using (frmOKMsg frm = new frmOKMsg(message, enableTimeOut))
            {
                frm.ShowDialog();
            }
        }

        public static void ShowShortUserMessage(string message, string buttonText)
        {
            using (frmOKMsg frm = new frmOKMsg(message, true, true))
            {
                frm.SetButtonText = buttonText;
                frm.ShowDialog();
            }
        }
    }
}
