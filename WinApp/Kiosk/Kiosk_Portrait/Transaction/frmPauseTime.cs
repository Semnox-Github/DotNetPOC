/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmPauseTime.cs
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80       09-Sep-2019      Deeksha            Added logger methods.
* 2.120      18-May-2021      Dakshakh Raj       Handling text box fore color changes.
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Windows.Forms;
using System.Drawing;

namespace Parafait_Kiosk.Transaction
{
    public partial class frmPauseTime : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string cardNumber;
        Card card;
        Utilities Utilities = KioskStatic.Utilities;
        public frmPauseTime(string message, string pCardNo)
        {
            log.LogMethodEntry(message, pCardNo);
            InitializeComponent();
            txtCardNo.Text = txtTimeRemaining.Text = " ";
            lblmsg.Text = message.ToString();
            if (pCardNo != null)
            {
                cardNumber = pCardNo;
            }
            KioskStatic.setDefaultFont(this);
            //SetTextBoxFontColors();
            SetCustomizedFontColors();
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }
        private void frmPauseTime_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtCardNo.Text = cardNumber.ToString();
            card = new Card(cardNumber, "", Utilities);

            if (card.CardStatus == "NEW")
            {
                frmOKMsg.ShowUserMessage(Utilities.MessageUtils.getMessage(459));
                log.LogMethodExit();
                return;
            }
            txtCardNo.Text = cardNumber;
            txtTimeRemaining.Text = (card.time + card.CreditPlusTime).ToString() + " " + Utilities.MessageUtils.getMessage("Minutes");
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            if (tickSecondsRemaining <= 60)
                tickSecondsRemaining = tickSecondsRemaining - 1;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining == 3)
            {
                btnOk.Enabled = false;
            }

            if (tickSecondsRemaining == 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                {
                    tickSecondsRemaining = 0;
                }
            }

            if (tickSecondsRemaining <= 0)
            {
                KioskStatic.logToFile("frmPauseTime Timed out");
                Application.DoEvents();
                this.Close();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            string message = "";
            bool sv = true;
            try
            {
                btnOk.Enabled = false;
                btnBack.Enabled = false;
                if (txtCardNo.Text.ToString().Length > 0)
                {
                    TaskProcs tp = new TaskProcs(KioskStatic.Utilities);
                    sv = tp.PauseTimeEntitlement(card.card_id, "Kiosk Pause Time ", ref message);
                    if (!sv)
                    {
                        frmOKMsg.ShowUserMessage(message);
                    }
                    else
                    {
                        setKioskTimerSecondsValue(10);
                        lblmsg.Text = Utilities.MessageUtils.getMessage(1388);
                        btnOk.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing btnOk_Click()" + ex.Message);
                frmOKMsg.ShowUserMessage(ex.ToString());
                KioskStatic.logToFile("Error in PauseTimeEntitlement");
            }
            finally
            {
                btnOk.Enabled = true;
                btnBack.Enabled = true;
            }            
            log.LogMethodExit();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }
        private void SetTextBoxFontColors()
        {
            log.LogMethodEntry();
            if (KioskStatic.CurrentTheme == null ||
               (KioskStatic.CurrentTheme != null && KioskStatic.CurrentTheme.TextForeColor == Color.White))
            {
                txtCardNo.ForeColor = Color.Black;
                txtTimeRemaining.ForeColor = Color.Black;
            }
            else
            {
                txtCardNo.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                txtTimeRemaining.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmPauseTime");
            try
            {
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.PauseTimeLblMessageTextForeColor;//Balance Time will be paused
                this.lblCardNumber.ForeColor = KioskStatic.CurrentTheme.PauseTimeCardNumberHeaderTextForeColor;//Card number Header
                this.txtCardNo.ForeColor = KioskStatic.CurrentTheme.PauseTimeCardNumberInfoTextForeColor;//Card number info
                this.lblTimeRemainingText.ForeColor = KioskStatic.CurrentTheme.PauseTimeTimeHeaderTextForeColor;//Time remaining header
                this.txtTimeRemaining.ForeColor = KioskStatic.CurrentTheme.PauseTimeTimeInfoTextForeColor;//Time remaining info
                //this.lbltxtEticket.ForeColor = KioskStatic.CurrentTheme.PauseTimeETicketHeaderTextForeColor;//e - Ticket Balance header
                //this.lblEticket.ForeColor = KioskStatic.CurrentTheme.PauseTimeETicketInfoTextForeColor;//e - Ticket Balance
                this.btnBack.ForeColor = KioskStatic.CurrentTheme.PauseTimeBackBtnTextForeColor;//Back button
                this.btnOk.ForeColor = KioskStatic.CurrentTheme.PauseTimeOkBtnTextForeColor;//Ok Button
                this.BackgroundImage = ThemeManager.CurrentThemeImages.PauseTimeBackgroundImage;
                btnOk.BackgroundImage =
                    btnBack.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.PauseTimeButtons);
                txtCardNo.BackColor = KioskStatic.CurrentTheme.TextBackGroundColor;
                txtTimeRemaining.BackColor = KioskStatic.CurrentTheme.TextBackGroundColor;
                //lblEticket.BackColor = KioskStatic.CurrentTheme.TextBackGroundColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmPauseTime: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
