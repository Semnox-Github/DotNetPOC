/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmPauseTime
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80        5-Sep-2019       Deeksha            Added logger methods.
 * 2.150.1    22-Feb-2023       Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Windows.Forms;

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
                cardNumber = pCardNo;
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
            catch (Exception ex)
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
    }
}
