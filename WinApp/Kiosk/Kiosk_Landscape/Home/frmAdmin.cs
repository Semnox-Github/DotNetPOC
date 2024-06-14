/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmAdmin.cs
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80      4-Sep-2019       Deeksha              Added logger methods.
 *2.100.0    05-Aug-2020      Guru S A             Kiosk activity log changes
 *2.150.1    22-Feb-2023      Guru S A             Kiosk Cart Enhancements
 *******************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmAdmin : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmAdmin()
        {
            log.LogMethodEntry();
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            KioskStatic.Utilities.setLanguage(this);
            //Starts:Modification on 17-Dec-2015 for introducing new theme
            btnCancel.BackgroundImage = btnExit.BackgroundImage = btnKioskActivity.BackgroundImage = btnLoadBonus.BackgroundImage =
                btnPrintSummary.BackgroundImage = btnSetup.BackgroundImage = ThemeManager.CurrentThemeImages.CloseButton;//Ends:Modification on 17-Dec-2015 for introducing new theme
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Cancel pressed");
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            ResetKioskTimer();
            KioskStatic.logToFile("SetUp pressed");
            using (SetUp sf = new SetUp())
            {
                sf.ShowDialog();
            }
            StartKioskTimer();
            log.LogMethodExit();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Exit pressed");
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void btnPrintSummary_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                ResetKioskTimer();
                KioskStatic.logToFile("Print Summary pressed");
                DataTable dt = KioskStatic.Utilities.executeDataTable(@"select 
                                                                    isnull(sum(Amount * CashRatio), 0) Cash, 
                                                                    isnull(sum(Amount * CreditCardRatio), 0) CreditCard, 
                                                                    isnull(sum(Amount * GameCardRatio), 0) GameCard, 
                                                                    --isnull(aborted.aborted, 0) Aborted,
                                                                    --isnull(sum(Amount), 0) + isnull(aborted.aborted, 0) Total,
                                                                    isnull(sum(Amount), 0) Total,
                                                                    count(distinct trxId) [TotalOps],
                                                                    count(distinct new_card_id) [NewCards]
                                                                  from TransactionView v --, 
                                                                   -- (select sum(value) aborted
                                                                   --     from KioskActivityLog 
                                                                   --     where Activity in ('ABORT','ABORT_CC','ABORT_GAMECARD')
                                                                   --     and TimeStamp >= case when datepart(hh, getdate()) < 6
                                                                   --                     then DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate() - 1), 0)) 
                                                                   --                     else DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate()), 0)) end
                                                                   --     and KioskName = @pos) aborted
                                                                  where trxdate >= case when datepart(hh, getdate()) < 6
                                                                                        then DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate() - 1), 0)) 
                                                                                        else DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate()), 0)) end
                                                                  and pos_machine = @pos
																  --group by aborted.aborted",
                                                                  new SqlParameter("@pos", KioskStatic.Utilities.ParafaitEnv.POSMachine));

                DataTable dtAborted = KioskStatic.Utilities.executeDataTable(@"select Activity, sum(value) aborted
                                                                                 from KioskActivityLog 
                                                                                where Activity in ( 'ABORT' ,'ABORT_CC', 'ABORT_GAMECARD') 
                                                                                  and TimeStamp >= case when datepart(hh, getdate()) < 6
                                                                                        then DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate() - 1), 0)) 
                                                                                        else DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate()), 0)) end
                                                                                  and KioskName = @pos
                                                                                group by Activity",
                                                                     new SqlParameter("@pos", KioskStatic.Utilities.ParafaitEnv.POSMachine));

                DataTable dtMoney = KioskStatic.Utilities.executeDataTable(@"select NoteCoinFlag, Value, sum(value) Total, count(value) Quantity 
                                                                         from kioskactivitylog
                                                                         where NoteCoinFlag is not null
                                                                           and TimeStamp >= case when datepart(hh, getdate()) < 6
                                                                                        then DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate() - 1), 0)) 
                                                                                        else DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate()), 0)) end
                                                                           and KioskName = @pos
                                                                           group by NoteCoinFlag, value
                                                                           order by NoteCoinFlag, value",
                                                                      new SqlParameter("@pos", KioskStatic.Utilities.ParafaitEnv.POSMachine));

                //DataTable dtRefund = KioskStatic.Utilities.executeDataTable(@" select ISNULL(sum(value),0) Refunded
                //                                                                from KioskActivityLog 
                //                                                                where Activity = 'REFUND'
                //                                                                and TimeStamp >= case when datepart(hh, getdate()) < 6
                //                                                                                then DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate() - 1), 0)) 
                //                                                                                else DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate()), 0)) end
                //                                                                and KioskName = @pos ",
                //                                                    new SqlParameter("@pos", KioskStatic.Utilities.ParafaitEnv.POSMachine));

                string formattedReceipt = "*---KIOSK Activity Summary---*" + Environment.NewLine;
                formattedReceipt += KioskStatic.Utilities.ParafaitEnv.SiteName + Environment.NewLine + Environment.NewLine;
                formattedReceipt += "Date: " + DateTime.Now.ToString(KioskStatic.Utilities.ParafaitEnv.DATETIME_FORMAT) + Environment.NewLine;
                formattedReceipt += "Kiosk: " + KioskStatic.Utilities.ParafaitEnv.POSMachine + Environment.NewLine + Environment.NewLine;

                //decimal refundedAmount = 0;
                //if (dtRefund.Rows.Count > 0)
                //{
                //    refundedAmount = Convert.ToDecimal(dtRefund.Rows[0]["Refunded"]);
                //}

                decimal abortedCash = 0;
                decimal abortedCreditCard = 0;
                decimal abortedGameCard = 0;
                if (dtAborted.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAborted.Rows.Count; i++)
                    {
                        if (dtAborted.Rows[i]["Activity"].ToString() == KioskTransaction.GETABORTCASH.ToString())
                        {
                            abortedCash = Convert.ToDecimal(dtAborted.Rows[0]["Aborted"]);
                        }
                        else if (dtAborted.Rows[i]["Activity"].ToString() == KioskTransaction.GETABORTCREDITCARD.ToString())
                        {
                            abortedCreditCard = Convert.ToDecimal(dtAborted.Rows[0]["Aborted"]);
                        }
                        else if (dtAborted.Rows[i]["Activity"].ToString() == KioskTransaction.GETABORTGAMECARD.ToString())
                        {
                            abortedGameCard = Convert.ToDecimal(dtAborted.Rows[0]["Aborted"]);
                        }
                    }
                }
                decimal cashAmount = 0;
                decimal cardAmount = 0;
                decimal gameCardAmount = 0;
                decimal totalTrxAmount = 0;
                decimal totalOps = 0;
                decimal newCards = 0;
                if (dt.Rows.Count > 0)
                {
                    cashAmount = Convert.ToDecimal(dt.Rows[0]["Cash"]);
                    cardAmount = Convert.ToDecimal(dt.Rows[0]["CreditCard"]);
                    gameCardAmount = Convert.ToDecimal(dt.Rows[0]["GameCard"]);
                    totalTrxAmount = Convert.ToDecimal(dt.Rows[0]["Total"]);
                    totalOps = Convert.ToDecimal(dt.Rows[0]["TotalOps"]);
                    newCards = Convert.ToDecimal(dt.Rows[0]["NewCards"]);
                }
                formattedReceipt += "Cash: " + cashAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                formattedReceipt += "Credit Card: " + cardAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                if (gameCardAmount > 0)
                {
                    formattedReceipt += "Game Card: " + gameCardAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                }

                formattedReceipt += "Aborted Cash: " + abortedCash.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                formattedReceipt += "Aborted Credit: " + abortedCreditCard.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                if (abortedGameCard > 0)
                {
                    formattedReceipt += "Aborted Game Card: " + abortedGameCard.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                }

                //formattedReceipt += "Refunded: " + refundedAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;

                formattedReceipt += "Total Trx: " + totalTrxAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine + Environment.NewLine;
                formattedReceipt += "#Ops: " + totalOps.ToString(KioskStatic.Utilities.ParafaitEnv.NUMBER_FORMAT) + Environment.NewLine;
                formattedReceipt += "#New Cards: " + newCards.ToString(KioskStatic.Utilities.ParafaitEnv.NUMBER_FORMAT) + Environment.NewLine + Environment.NewLine;


                decimal insertedTotal = 0;
                foreach (DataRow dr in dtMoney.Rows)
                {
                    if (dr["NoteCoinFlag"].ToString().Equals("T"))
                    {
                        formattedReceipt += ("  " + "Token").PadRight(12) + " #" + dr["Quantity"].ToString() + Environment.NewLine;
                    }
                    else
                    {
                        insertedTotal += Convert.ToDecimal(dr["Total"]);
                        formattedReceipt += (KioskStatic.Utilities.ParafaitEnv.CURRENCY_SYMBOL + " " + Convert.ToDouble(dr["Value"]).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) + " " + (dr["NoteCoinFlag"].ToString().Equals("N") ? "Bill" : "Coin")).PadRight(12) + " #" + dr["Quantity"].ToString() + ", " + Convert.ToDecimal(dr["Total"]).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                    }
                }
                formattedReceipt += "Total Inserted: " + insertedTotal.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;

                formattedReceipt += Environment.NewLine;
                formattedReceipt += "-".PadRight(10, '-').PadLeft(20, ' ');
                log.LogVariableState("formattedReceipt", formattedReceipt);
                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += (s, args) =>
                {
                    args.Graphics.DrawString(formattedReceipt, new System.Drawing.Font("Courier New", 9, FontStyle.Bold), System.Drawing.Brushes.Black, 12, 20);
                };
                printDocument.Print();
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (KioskStatic.Utilities.getParafaitDefaults("ENABLE_LOAD_BONUS_IN_ADMIN_SCREEN").Equals("Y"))
                btnLoadBonus.Enabled = true;
            else
                btnLoadBonus.Enabled = false;
            KioskStatic.logToFile("Admin Screen opened");
            this.FormClosed += frmAdmin_FormClosed;
            log.LogMethodExit();
        }
        void frmAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            //timeOut.Stop();
            log.LogMethodEntry();
            KioskStatic.logToFile("frmAdmin_FormClosed");
            log.LogMethodExit();
        }
        
        private void btnLoadBonus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                ResetKioskTimer();
                KioskStatic.logToFile("Load Bonus pressed");
                frmTapCard ftc = new frmTapCard();
                ftc.ShowDialog();
                if (ftc.Card == null)
                {
                    ftc.Dispose();
                    log.LogMethodExit();
                    return;
                }
                ftc.Dispose();

                if (ftc.Card.technician_card == 'Y')
                {
                    frmLoadBonus flb = new frmLoadBonus(ftc.Card);
                    flb.ShowDialog();
                }
                else
                {
                    frmOKMsg fok = new frmOKMsg("Please tap your staff card");
                    fok.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
            }
            finally
            { 
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining < 10)
            {
                KioskStatic.logToFile("Time-out exit");
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            log.LogMethodExit();
        }

        private void btnKioskActivity_Click(object sender, EventArgs e)//25-06-2015:Starts
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("KioskActivity pressed");
            //25-06-2015:Changed to add the Kiosk Activity Detail report
            KioskActivityDetails kioskactivity = new KioskActivityDetails();
            kioskactivity.ShowDialog();//Modification on 17-Dec-2015 for introducing new theme
            log.LogMethodExit();
        }

        private void btnReboot_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("btnReboot_Click()");
            System.Diagnostics.Process.Start("shutdown.exe", "-r -f -t 00");
            log.LogMethodExit();
        }
    }
}
