/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmPauseTime.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80        4-Sep-2019      Deeksha             Added logger methods.
 *2.80.1     02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 *2.130.0     30-Jun-2021      Dakshak             Theme changes to support customized Font ForeColor
********************************************************************************************/
using System;
using Semnox.Parafait.Transaction;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk.Transaction
{
   
    public partial class frmPauseTime : BaseForm
    {
        public string cardNumber;
        Card card;
        Utilities Utilities = KioskStatic.Utilities;
        public frmPauseTime(string message,string pCardNo)
        {
            log.LogMethodEntry(message, pCardNo);
            InitializeComponent();
            txtCardNo.Text = txtTimeRemaining.Text = " ";
            lblmsg.Text = message.ToString();
            if (pCardNo != null)
            {
                cardNumber = pCardNo;
            }
            KioskStatic.Utilities.setLanguage(this);
            KioskStatic.setDefaultFont(this);
            SetCustomizedFontColors();

            log.LogMethodExit();
        }
        private void frmPauseTime_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtCardNo.Text = cardNumber.ToString();
            card = new Card(cardNumber, "", Utilities);

            if (card.CardStatus == "NEW")
            {
                frmOKMsg f = new frmOKMsg(Utilities.MessageUtils.getMessage(459));
                f.ShowDialog();
                log.LogMethodExit();
                return;
            }
            txtCardNo.Text = cardNumber;
            txtTimeRemaining.Text = (card.time + card.CreditPlusTime).ToString() + " " + Utilities.MessageUtils.getMessage("Minutes");
            lblEticket.Text= (card.CreditPlusTickets + card.ticket_count).ToString() + " " + Utilities.MessageUtils.getMessage("Tickets");
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
                if (txtCardNo.Text.ToString().Length > 0)
                {
                    TaskProcs tp = new TaskProcs(KioskStatic.Utilities);
                    sv = tp.PauseTimeEntitlement(card.card_id, "Kiosk Pause Time ", ref message);
                    if (!sv)
                    {
                        frmOKMsg f = new frmOKMsg(message);
                        f.ShowDialog();
                    }
                    else
                    {
                        setKioskTimerSecondsValue(10);
                        lblmsg.Text = Utilities.MessageUtils.getMessage(1388);
                        btnOk.Visible = false;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                KioskStatic.logToFile("Error in PauseTimeEntitlement");
            }
            log.LogMethodExit();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.PauseTimeLblMessageTextForeColor;//Balance Time will be paused
                this.lblCardNumber.ForeColor = KioskStatic.CurrentTheme.PauseTimeCardNumberHeaderTextForeColor;//Card number Header
                this.txtCardNo.ForeColor = KioskStatic.CurrentTheme.PauseTimeCardNumberInfoTextForeColor;//Card number info
                this.lblTimeRemainingText.ForeColor = KioskStatic.CurrentTheme.PauseTimeTimeHeaderTextForeColor;//Time remaining header
                this.txtTimeRemaining.ForeColor = KioskStatic.CurrentTheme.PauseTimeTimeInfoTextForeColor;//Time remaining info
                this.lbltxtEticket.ForeColor = KioskStatic.CurrentTheme.PauseTimeETicketHeaderTextForeColor;//e - Ticket Balance header
                this.lblEticket.ForeColor = KioskStatic.CurrentTheme.PauseTimeETicketInfoTextForeColor;//e - Ticket Balance
                this.btnBack.ForeColor = KioskStatic.CurrentTheme.PauseTimeBackBtnTextForeColor;//Back button
                this.btnOk.ForeColor = KioskStatic.CurrentTheme.PauseTimeOkBtnTextForeColor;//Ok Button
                this.BackgroundImage = KioskStatic.CurrentTheme.PauseTimeBackgroundImage;
                txtCardNo.BackColor = KioskStatic.CurrentTheme.TextBackGroundColor;
                txtTimeRemaining.BackColor = KioskStatic.CurrentTheme.TextBackGroundColor;
                lblEticket.BackColor = KioskStatic.CurrentTheme.TextBackGroundColor;
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
