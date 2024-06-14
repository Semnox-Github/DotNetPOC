/********************************************************************************************
* Project Name - Parafait_Kiosk - frmSuccessMsg
* Description  - frmSuccessMsg 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.80        05-Sep-2019      Deeksha            Added logger methods.
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.140.0     18-Oct-2021      Sathyavathi        Check-In Check-Out feature in Kiosk
*2.150.0.0   02-Dec-2022      Sathyavathi        Check-In feature Phase-2 Additional features
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{//Starts:Modification on 17-Dec-2015 for introducing new theme 
    public partial class frmSuccessMsg : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //int ticks = 0;
        public frmSuccessMsg(string message, string Action, string Balance, string point, string pass, string Source, string trxNo)
        {
            log.LogMethodEntry(message, Action, Balance, point, pass, Source);
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
            if (!string.IsNullOrEmpty(Action))
            {
                lblAction.Text = Action;
                lblAction.Visible = true;
            }
            if (!string.IsNullOrEmpty(Balance))
            {
                lblBalanceMsg.Text = Balance;
                lblBalanceMsg.Visible = true;
            }
            if (!string.IsNullOrEmpty(point))
            {
                lblPoint.Text = point;
                lblPoint.Visible = true;
            }
            if (!string.IsNullOrEmpty(pass))
            {
                lblPasNo.Text = pass;
                lblPasNo.Visible = true;
            }
            if (!string.IsNullOrEmpty(Source))
            {
                switch (Source)
                {
                    case "NEW":
                        pbSuccess.Image = ThemeManager.CurrentThemeImages.SucessAddImage;
                        pbSuccess.Visible = true;
                        break;
                    case "Recharge":
                        pbSuccess.Image = ThemeManager.CurrentThemeImages.SucessRechargeImage;
                        pbSuccess.Visible = true;
                        break;
                    case "Redeem":
                        pbSuccess.Image = ThemeManager.CurrentThemeImages.SucessRedeemImage;
                        pbSuccess.Visible = true;
                        break;
                    case "Register":
                        pbSuccess.Image = ThemeManager.CurrentThemeImages.SucessRegister;
                        pbSuccess.Visible = true;
                        break;
                    case "Check-In":
                        pbSuccess.Image = ThemeManager.CurrentThemeImages.SucessCheckIn;
                        pbSuccess.Visible = true;
                        break;
                }
            }
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

        private void frmOKMsg_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timer1.Stop();
            // StopKioskTimer();
        }

        private void frmOKMsg_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(lblmsg.Text))
                lblmsg.Text = this.Text;
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmSuccessMsg");
            try
            {
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgBtnHomeTextForeColor;
                this.lblHeading.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgLblHeadingTextForeColor;
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgLblmsgTextForeColor;
                this.lblBalanceMsg.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgLblBalanceMsgTextForeColor;
                this.lblPoint.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgLblPointTextForeColor;
                this.lblPasNo.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgLblPasNoTextForeColor;
                this.btnClose.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgBtnCloseTextForeColor;
                this.lblTrxNumber.ForeColor = KioskStatic.CurrentTheme.FrmSuccessMsgLblTrxNumberTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmSuccessMsg: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}//Ends:Modification on 17-Dec-2015 for introducing new theme
