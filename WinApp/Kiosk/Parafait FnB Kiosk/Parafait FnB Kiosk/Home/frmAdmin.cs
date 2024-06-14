/********************************************************************************************
* Project Name - Parafait_Kiosk -frmAdmin.cs
* Description  - frmAdmin 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
*2.80        09-Sep-2019      Deeksha            Added logger methods.
*2.100.0     05-Aug-2020      Guru S A           Kiosk activity log changes
********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    public partial class frmAdmin : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities = Common.utils;
        DeviceClass TopUpReaderDevice = null;
        public frmAdmin()
        {
            log.LogMethodEntry();
            InitializeComponent();

            btnExit.Enabled =
                        btnPrintSummary.Enabled =
                        btnRebootComputer.Enabled =
                        btnSetup.Enabled = false;

            this.FormClosing += frmAdmin_FormClosing;
            log.LogMethodExit();
        }

        void frmAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            if (TopUpReaderDevice != null)
            {
                TopUpReaderDevice.UnRegister();
                TopUpReaderDevice.Dispose();
            }
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                try
                {
                    TagNumber tagNumber;
                    TagNumberParser tagNumberParser = new TagNumberParser(Common.utils.ExecutionContext);
                    if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                    {
                        string message = tagNumberParser.Validate(checkScannedEvent.Message);
                        Common.logToFile(message);
                        log.LogMethodExit();
                        return;
                    }
                    string lclCardNumber = tagNumber.Value.ToString();
                    lclCardNumber = ReverseTopupCardNumber(lclCardNumber);
                    handleCardRead(lclCardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    Common.logException(ex);
                }
            }
            log.LogMethodExit();
        }

        public string ReverseTopupCardNumber(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            bool REVERSE_KIOSK_TOPUP_CARD_NUMBER = Common.utils.getParafaitDefaults("REVERSE_KIOSK_TOPUP_CARD_NUMBER").Equals("Y");

            if (REVERSE_KIOSK_TOPUP_CARD_NUMBER == false)
            {
                log.LogMethodExit(cardNumber);
                return cardNumber;
            }
            else
            {
                try
                {
                    char[] arr = cardNumber.ToCharArray();

                    for (int i = 0; i < cardNumber.Length / 2; i += 2)
                    {
                        char x = arr[i];
                        char y = arr[i + 1];

                        arr[i] = arr[cardNumber.Length - i - 2];
                        arr[i + 1] = arr[cardNumber.Length - i - 1];

                        arr[cardNumber.Length - i - 2] = x;
                        arr[cardNumber.Length - i - 1] = y;
                    }
                    string ret = new string(arr);
                    log.LogMethodExit(ret);
                    return ret;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while executing ReverseTopupCardNumber()" + ex.Message);
                    log.LogMethodExit(cardNumber);
                    return cardNumber;
                }
            }
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            seconds = 0;
            Card Card = new Card(readerDevice, inCardNumber, "External POS", Utilities);

            string message = "";
            Application.DoEvents();
            if (!Helper.refreshCardFromHQ(ref Card, ref message))
            {
                return;
            }

            if (Card.technician_card == 'N')
            {
                Common.ShowAlert("Please tap a valid Staff Card");
            }
            else
            {
                btnExit.Enabled =
                    btnPrintSummary.Enabled =
                    btnRebootComputer.Enabled =
                    btnSetup.Enabled = true;
            }
            log.LogMethodExit();
        }

        Semnox.Core.Utilities.KeyPads.Kiosk.frmNumberPad numPad = null;
        Panel NumberPadVarPanel;
        private void ShowKeyPad()
        {
            log.LogMethodEntry();
            if (numPad == null)
            {
                numPad = new Semnox.Core.Utilities.KeyPads.Kiosk.frmNumberPad();
                numPad.Init("N0", 0);
                NumberPadVarPanel = numPad.NumPadPanel();
                NumberPadVarPanel.Controls["btnClose"].Visible = false;
                NumberPadVarPanel.Controls["btnCloseX"].Visible = false;
                NumberPadVarPanel.Location = new Point(17, 17);
                this.Controls.Add(NumberPadVarPanel);

                numPad.setReceiveAction = EventnumPadOKReceived;
                numPad.setKeyAction = EventnumPadKeyPressReceived;

                this.KeyPreview = true;

                this.KeyPress += new KeyPressEventHandler(FormNumPad_KeyPress);
            }

            numPad.NewEntry = true;
            NumberPadVarPanel.Visible = true;
            NumberPadVarPanel.BringToFront();
            log.LogMethodExit();
        }

        void FormNumPad_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyChar == (char)Keys.Escape)
                NumberPadVarPanel.Visible = false;
            else
            {
                numPad.GetKey(e.KeyChar);
                seconds = 0;
            }
            log.LogMethodExit();
        }

        private void EventnumPadOKReceived()
        {
            log.LogMethodEntry();
            double n = numPad.ReturnNumber;
            if (n.ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT) == Properties.Settings.Default.ExitCode)
            {
                btnExit.Enabled =
                    btnPrintSummary.Enabled =
                    btnRebootComputer.Enabled =
                    btnSetup.Enabled = true;
            }
            log.LogMethodExit();
        }

        void EventnumPadKeyPressReceived()
        {
            log.LogMethodEntry();
            double n = numPad.ReturnNumber;
            if (n.ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT) == Properties.Settings.Default.ExitCode)
            {
                btnExit.Enabled =
                        btnPrintSummary.Enabled =
                        btnRebootComputer.Enabled =
                        btnSetup.Enabled = true;
            }
            seconds = 0;
            log.LogMethodExit();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Common.logEnter();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            seconds = 0;
            timeOut.Stop();

            Common.logEnter();
            Parafait_Kiosk.SetUp sf = new Parafait_Kiosk.SetUp();
            sf.ShowDialog();
            timeOut.Start();
            log.LogMethodExit();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Common.logEnter();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void btnPrintSummary_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                timeOut.Stop();
                seconds = 0;
                Common.logEnter();

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
                        if (dtAborted.Rows[i]["Activity"].ToString() == KioskStatic.ACTIVITY_TYPE_ABORT.ToString())
                        {
                            abortedCash = Convert.ToDecimal(dtAborted.Rows[0]["Aborted"]);
                        }
                        else if (dtAborted.Rows[i]["Activity"].ToString() == KioskStatic.ACTIVITY_TYPE_ABORT_CC.ToString())
                        {
                            abortedCreditCard = Convert.ToDecimal(dtAborted.Rows[0]["Aborted"]);
                        }
                        else if (dtAborted.Rows[i]["Activity"].ToString() == KioskStatic.ACTIVITY_TYPE_ABORT_GAMECARD.ToString())
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
                    args.Graphics.DrawString(formattedReceipt, new System.Drawing.Font("Courier New", 9), System.Drawing.Brushes.Black, 12, 20);
                };
                printDocument.Print();
            }
            finally
            {
                timeOut.Start();
            }
            log.LogMethodExit();
        }

        Timer timeOut = new Timer();
        int seconds = 0;
        private void frmAdmin_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Common.logEnter();
            ShowKeyPad();


            try
            {
                Common.log.Info("Registering top up reader");
                TopUpReaderDevice = DeviceContainer.RegisterUSBCardReader(Utilities.ExecutionContext, this, CardScanCompleteEventHandle);
                Common.log.Info("Top up reader is registered");
            }
            catch (Exception ex)
            {
                Common.log.Error("Error registering top up reader", ex);
                Common.ShowMessage(ex.Message + ". " + Utilities.MessageUtils.getMessage(441));
            }


            timeOut.Tick += timeOut_Tick;
            timeOut.Interval = 1000;
            this.FormClosed += frmAdmin_FormClosed;
            timeOut.Start();
            log.LogMethodExit();
        }

        void frmAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            Common.logEnter();
            timeOut.Stop();
            Common.logExit();
            log.LogMethodExit();
        }

        void timeOut_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            seconds++;
            if (seconds > 30)
            {
                Common.logExit();
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            log.LogMethodExit();
        }

        private void btnRebootComputer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Common.logEnter();
            System.Diagnostics.Process.Start("shutdown.exe", "-r -f -t 00");
            log.LogMethodExit();
        }
    }
}
