/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmRedeemTokens.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.KioskCore.CoinAcceptor;
using Semnox.Parafait.KioskCore.CardDispenser;
using Semnox.Parafait.KioskCore;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device.PaymentGateway;
using System.Linq;
using Semnox.Parafait.Product;

namespace Parafait_Kiosk
{
    public partial class frmRedeemTokens : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string function;
        private Card currentCard;
        private decimal ProductPrice;
        private bool _allowNewCard = true;
        private CoinAcceptor coinAcceptor;
        private CardDispenser cardDispenser;
        private KioskStatic.configuration config = KioskStatic.config;
        private readonly TagNumberParser tagNumberParser;
        private string AMOUNT_WITH_CURRENCY_SYMBOL = string.Empty;
        private int coinAcceptorTimeout = 0;
        private Utilities Utilities = KioskStatic.Utilities;
        private CustomerDTO customerDTO = null;
        private Font savTimeOutFont;
        private const decimal LARGEAMOUNT = 999999999;
        private Button btnTest = new Button();
        //private bool canPerformNoteCoinReceivedAction = false;
        private System.Windows.Forms.Timer noteCoinActionTimer;
        public frmRedeemTokens(bool AllowNewCard, CustomerDTO customerDTO = null)
        {
            log.LogMethodEntry(AllowNewCard, customerDTO);
            Utilities.setLanguage(this);
            InitializeComponent();
            kioskTransaction = new KioskTransaction(Utilities);
            TimerMoney.Enabled = false;
            KioskTimerSwitch(false);
            AMOUNT_WITH_CURRENCY_SYMBOL = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");
            _allowNewCard = AllowNewCard;
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            SetTheme();

            KioskStatic.setDefaultFont(this);
            DisplayMessageLine("");

            lblTimeOut.Text = KioskStatic.MONEY_SCREEN_TIMEOUT.ToString("#0");
            savTimeOutFont = lblTimeOut.Font;
            lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            this.customerDTO = customerDTO;

            //this.ShowInTaskbar = false;
            if (config.coinAcceptorport != 0 && config.coinAcceptorport > -1)
            {
                try
                {
                    coinAcceptor = KioskStatic.getCoinAcceptor(KioskStatic.config.coinAcceptorport);
                    log.LogVariableState("KioskStatic.config.Coins", KioskStatic.config.Coins);
                    log.LogVariableState("coinAcceptor", coinAcceptor);
                    //log.LogVariableState("KioskStatic.spCoinAcceptor", KioskStatic.spCoinAcceptor);
                    //coinAcceptor.spCoinAcceptor = KioskStatic.spCoinAcceptor;
                    if (coinAcceptor.set_acceptance(true))
                    {
                        coinAcceptor.AmountRemainingToBeCollected = (double)LARGEAMOUNT;
                        if (KioskStatic.CoinAcceptorModel.Equals(CoinAcceptor.Models.MICROCOIN_SP))
                        {
                            coinAcceptor.dataReceivedEvent = coinAcceptorDataReceived;
                        }
                        else
                        {
                            KioskStatic.caReceiveAction = coinAcceptorDataReceived;
                            log.Info("KioskStatic.caReceiveAction = coinAcceptorDataReceived");
                        }
                        coinAcceptor.disableCoinAcceptor();
                    }
                    else
                    {
                        log.Info("KioskStatic.caReceiveAction = null");
                        KioskStatic.caReceiveAction = null;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error while Initializing Coin Acceptor: " + ex.Message);
                    throw;
                }
            }
            btnCompleteReload.Enabled = btnCompleteNEWCard.Enabled = false;
            SetCustomizedFontColors();
            DisplaybtnPrev(true);
            DisplaybtnCancel(true);
            Utilities.setLanguage(this);
            //AddTestBtn();
            log.LogMethodExit();
        }

        private void frmNewcard_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (kioskTransaction.GetTotalPaymentsReceived() > 0
                && kioskTransaction.GetTotalPaymentsReceived() < ProductPrice)
            {
                e.Cancel = true;
                btnCompleteReload.Enabled = btnCompleteNEWCard.Enabled = true;
                RefreshGridTotals();
                TimerMoney.Start();
                log.LogMethodExit("ac.totalValue > 0");
                return;
            }

            TimerMoney.Stop();

            if (noteCoinActionTimer != null)
            {
                noteCoinActionTimer.Stop();
            }
            if (coinAcceptor != null)
                coinAcceptor.disableCoinAcceptor();

            KioskStatic.caReceiveAction = null;
            KioskStatic.coinAcceptorDatareceived = false;

            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
            {
                KioskStatic.cardAcceptor.EjectCardFront();
                KioskStatic.cardAcceptor.BlockAllCards();
            } 

            if (kioskTransaction.GetTotalPaymentsReceived() >= ProductPrice)
            {
                DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Thank You"));
            }

            KioskStatic.logToFile("Exiting Reedeem Token.");

            Audio.Stop();

            log.LogMethodExit("Exiting...");
        }

        private void InitializeForm()
        {
            log.LogMethodEntry();            
            dgvCashInserted.Columns["Image"].Visible = false;

            int rowcount = 0;
            for (int i = 1; i < config.Coins.Length; i++)
            {
                if (config.Coins[i] != null && config.Coins[i].isToken)
                    rowcount++;
            }

            if (rowcount > 0)
                dgvCashInserted.RowTemplate.Height = (dgvCashInserted.Height) / (rowcount);

            rowcount = 0;
            for (int i = 1; i < config.Coins.Length; i++)
            {
                if (config.Coins[i] == null || !config.Coins[i].isToken)
                    continue;
                dgvCashInserted.Rows.Add(config.Coins[i].Image, config.Coins[i].Name, 0, 0);
                dgvCashInserted.Rows[rowcount++].Tag = "C" + i.ToString();
            }

            dgvCashInserted.Columns["Amount"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dgvCashInserted.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCashInserted.Columns["Amount"].DefaultCellStyle.Format = KioskStatic.KIOSK_CARD_VALUE_FORMAT;

            RefreshGridTotals();

            ProductPrice = LARGEAMOUNT;

            dgvCashInserted.DefaultCellStyle.Font = new Font(dgvCashInserted.DefaultCellStyle.Font.FontFamily, 24);
            dgvCashInserted.ColumnHeadersVisible = false;
            log.LogMethodExit();
        }

        private void RefreshGridTotals()
        {
            log.LogMethodEntry();
            for (int i = 0; i < dgvCashInserted.Rows.Count; i++)
            {
                dgvCashInserted["Quantity", i].Value = 0;
                dgvCashInserted["Amount", i].Value = 0;
            }
            List<TransactionPaymentsDTO> transactionPaymentsDTOList = kioskTransaction.GetTransactionPaymentsDTOList;
            int totalCount = 0;
            if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Any())
            {
                totalCount = transactionPaymentsDTOList.Count;
                for (int i = 0; i < transactionPaymentsDTOList.Count; i++)
                {
                    int index = GetGridRowIndex("C" + transactionPaymentsDTOList[i].PaymentId.ToString());
                    if (index < 0)
                        continue;

                    dgvCashInserted["Quantity", index].Value = Convert.ToInt32(dgvCashInserted["Quantity", index].Value) + 1;
                    dgvCashInserted["Amount", index].Value = Convert.ToDouble(dgvCashInserted["Amount", index].Value) + transactionPaymentsDTOList[i].Amount;
                    dgvCashInserted.Rows[index].DefaultCellStyle.BackColor = Color.AliceBlue;
                }
            }

            dgvCashInserted.Refresh();

            txtAvlblTokens.Text = totalCount.ToString();//ac.coinCount.ToString();

            Application.DoEvents();
            log.LogMethodExit();
        }

        private int GetGridRowIndex(string denomination)
        {
            log.LogMethodEntry(denomination);
            foreach (DataGridViewRow dr in dgvCashInserted.Rows)
            {
                if (dr.Tag == null)
                    continue;

                if (dr.Tag.ToString() == denomination)
                {
                    int ret = dr.Index;
                    log.LogMethodExit(ret);
                    return ret;
                }
            }
            log.LogMethodExit(-1);
            return -1;
        }

        private void frmRedeemTokens_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            InitializeForm();

            lblGreeting1.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 796);
            DisplayMessageLine(lblGreeting1.Text);

            Application.DoEvents();

            KioskStatic.logToFile("Enter Redeem Tokens screen");
            log.Info("Enter Redeem Tokens screen");

            btnCompleteNEWCard.Visible = (_allowNewCard && !KioskStatic.DisableNewCard);
            if (!btnCompleteNEWCard.Visible)
            {
                btnCompleteReload.Location = new Point(btnCompleteNEWCard.Location.X, btnCompleteNEWCard.Location.Y);
            }

            Audio.PlayAudio(Audio.RedeemInsertToken);
            if (coinAcceptor != null)
                coinAcceptor.set_acceptance(true);
            TimerMoney.Start();
            InitiateNoteCoinActionTimer();
            log.LogMethodExit();
        }

        private void coinAcceptorDataReceived()
        {
            log.LogMethodEntry();
            try
            {
                coinMessage = "";
                KioskStatic.coinAcceptorDatareceived = true;
                bool coinRecd = coinAcceptor.ProcessReceivedData(KioskStatic.coinAcceptor_rec, ref coinMessage);
                KioskStatic.logToFile("Coin Acceptor: " + coinMessage);
                log.Info("Coin Acceptor: " + coinMessage);
                if (coinRecd)
                {
                    totSecs = 0;
                    log.LogVariableState("CoinAcceptor.ReceivedCoinDenomination", coinAcceptor.ReceivedCoinDenomination);
                    int denomination = coinAcceptor.ReceivedCoinDenomination;
                    Card currentCard = kioskTransaction.GetTransactionPrimaryCard;
                    //double netTrxAmount = kioskTransaction.GetTrxNetAmount();
                    kioskTransaction.AddCoinPayment(denomination);
                    decimal amountReceived = kioskTransaction.GetTotalPaymentsReceived();
                    coinAcceptor.ReceivedCoinDenomination = 0;
                    //double balance = netTrxAmount - amountReceived;
                    //coinAcceptor.AmountRemainingToBeCollected = balance;
                    KioskStatic.ReceivedDenominationToActivityLog(Utilities.ExecutionContext, kioskTransaction.GetTransactionId,
                                                                  (currentCard == null ? "" : currentCard.CardNumber),
                                                                   denomination, KioskTransaction.GETTOKENIN,
                                                                KioskTransaction.GETTOKENINMSG, KioskStatic.TOKEN);
                    System.Threading.Thread.Sleep(20);
                    KioskStatic.logToFile("IsHandleCreated: " + this.IsHandleCreated.ToString());
                    //canPerformNoteCoinReceivedAction = true;
                    //KioskStatic.logToFile("Set canPerformNoteCoinReceivedAction as true");
                    //Invoke((MethodInvoker)delegate
                    //{
                    //    TimeOut.Abort();
                    //    RefreshGridTotals();
                    //    List<TransactionPaymentsDTO> transactionPaymentsDTOList = kioskTransaction.GetTransactionPaymentsDTOList;
                    //    if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Count >= config.Coins.Length)
                    //    {
                    //        System.Threading.Thread.Sleep(100);
                    //        coinAcceptor.disableCoinAcceptor();
                    //        using (frmOKMsg fok = new frmOKMsg("You have inserted maximum allowed coins per transaction. Please press Complete to redeem inserted tokens."))
                    //        { fok.ShowDialog(); }
                    //    }
                    //});
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile("Error in coinAcceptorDataReceived: " + ex.Message);
            }
            log.LogMethodExit();
        }

        string coinMessage = "";
        double totSecs = 0;
        private void TimerMoney_Tick(System.Object sender, System.EventArgs e)
        {
            //log.LogMethodEntry();
            TimerMoney.Stop();
            try
            {
                //bool microSPDevice = (KioskStatic.CoinAcceptorModel.Equals(CoinAcceptor.Models.MICROCOIN_SP));
                //if (!KioskStatic.coinAcceptorDatareceived && coinAcceptor != null
                //&& (KioskStatic.caReceiveAction != null || microSPDevice))
                //{
                //    coinAcceptorTimeout++;
                //    if (coinAcceptorTimeout > 300
                //        && ((KioskStatic.spCoinAcceptor != null && KioskStatic.spCoinAcceptor.IsOpen)
                //              || microSPDevice))
                //    {
                //        coinMessage = MessageContainerList.GetMessage(Utilities.ExecutionContext, 424);
                //        KioskStatic.logToFile("Coin acceptor timeout / offline");
                //        log.Info("Coin acceptor timeout / offline");
                //    }

                //    if (coinAcceptorTimeout % 5 == 0)
                //    {
                //        log.Info("KioskStatic.spCoinAcceptor.IsOpen : "
                //            + (KioskStatic.spCoinAcceptor != null && KioskStatic.spCoinAcceptor.IsOpen));
                //        if ((KioskStatic.spCoinAcceptor != null && KioskStatic.spCoinAcceptor.IsOpen)
                //             || microSPDevice)
                //        {
                //            if (coinAcceptor != null)
                //                coinAcceptor.checkStatus();
                //        }
                //    }
                //}

                if (kioskTransaction.GetTotalPaymentsReceived() <= 0
                    && ProductPrice > kioskTransaction.GetTotalPaymentsReceived())
                {
                    if (coinAcceptor == null || coinAcceptor.ReceivedCoinDenomination == 0)
                    {
                        totSecs += TimerMoney.Interval / 1000.0;
                        if (totSecs > KioskStatic.MONEY_SCREEN_TIMEOUT)
                        {
                            log.Info("totSecs : " + totSecs);
                            if (coinAcceptor != null)
                            {
                                coinAcceptor.disableCoinAcceptor();
                                KioskStatic.logToFile("Coin acceptor disabled");
                                log.Info("Coin acceptor disabled");
                            }
                        }
                    }
                    Application.DoEvents();
                    if (kioskTransaction.GetTotalPaymentsReceived() <= 0
                    && ProductPrice > kioskTransaction.GetTotalPaymentsReceived()) // check again if any money has been inserted at the last minute
                    {
                        int secondsRemaining = KioskStatic.MONEY_SCREEN_TIMEOUT - (int)totSecs;
                        if (secondsRemaining == 10)
                        {
                            lblTimeOut.Text = secondsRemaining.ToString("#0");
                            if (TimeOut.AbortTimeOut(this))
                                totSecs = 0;
                            else
                                totSecs = KioskStatic.MONEY_SCREEN_TIMEOUT - 2;
                        }

                        if (totSecs > KioskStatic.MONEY_SCREEN_TIMEOUT)
                        {
                            log.Info("totSecs : " + totSecs);
                            DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 425));
                            lblTimeOut.Font = savTimeOutFont;
                            lblTimeOut.Text = "Time Out";
                            btnPrev.Enabled = false;
                            //exitTimer.Interval = 2000;
                            //exitTimer.Enabled = true;
                            //exitTimer.Start();
                            KioskTimerSwitch(true);
                            KioskTimerInterval(2000);
                            StartKioskTimer();
                            KioskStatic.logToFile("Operation Timed out...");
                            log.LogMethodExit("Operation Timed out...");
                            return;
                        }
                        else
                        {
                            if (secondsRemaining > 0)
                            {
                                lblTimeOut.Text = secondsRemaining.ToString("#0");
                            }
                        }
                    }
                    else
                    {
                        if (coinAcceptor != null)
                            coinAcceptor.set_acceptance(true);
                        btnPrev.Enabled = true;
                        TimerMoney.Start();
                        log.LogMethodExit("TimerMoney.Start()");
                        return;
                    }
                }
                else
                {
                    if (function == KioskTransaction.GETNEWCARDTYPE)
                        lblTimeOut.Text = "";
                    else
                    {
                        lblTimeOut.Font = savTimeOutFont;
                        if (ServerDateTime.Now.Millisecond / 500 > 0)
                            lblTimeOut.Text = "";
                        else
                            lblTimeOut.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 800);
                    }
                    if (kioskTransaction.GetTotalPaymentsReceived() < ProductPrice)
                    {
                        btnPrev.Enabled = false;
                        btnCompleteReload.Enabled = btnCompleteNEWCard.Enabled = true;
                    }
                }

                if (coinMessage.EndsWith("accepted") || coinMessage.EndsWith("rejected"))
                {
                    DisplayMessageLine(coinMessage.Replace("accepted", MessageContainerList.GetMessage(Utilities.ExecutionContext, "accepted"))
                                                  .Replace("rejected", MessageContainerList.GetMessage(Utilities.ExecutionContext, "rejected"))
                                                  .Replace("Denomination", MessageContainerList.GetMessage(Utilities.ExecutionContext, "Denomination")));
                    Audio.Stop();
                }
                else
                {
                    if (coinAcceptor != null && coinAcceptor.criticalError)
                    {
                        using (frmOKMsg f = new frmOKMsg(MessageContainerList.GetMessage(Utilities.ExecutionContext, coinMessage) + ". " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 441)))
                        { f.ShowDialog(); }
                        AbortAndExit(coinMessage);
                        log.LogMethodExit(coinMessage);
                        return;
                    }
                    else if (string.IsNullOrEmpty(coinMessage) == false)
                        DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, coinMessage));
                    else
                        DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 796));
                }

                Application.DoEvents();

                if (kioskTransaction.GetTotalPaymentsReceived() >= ProductPrice)
                {
                    try
                    {
                        DisableButtons();
                        TimerMoney.Stop();
                        RefreshGridTotals();
                        Application.DoEvents();

                        string amountFormat = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "AMOUNT_FORMAT");
                        string msg = "Valid Tokens inserted: " + kioskTransaction.GetTotalPaymentsReceived().ToString(amountFormat) + "; " + "Required: " + ProductPrice.ToString(amountFormat);
                        KioskStatic.logToFile(msg);
                        log.Info(msg);
                        if (coinAcceptor != null)
                        {
                            System.Threading.Thread.Sleep(300);
                            coinAcceptor.disableCoinAcceptor();
                            KioskStatic.logToFile("Coin acceptor disabled");
                            log.Info("Coin acceptor disabled");
                        }

                        if (function == KioskTransaction.GETNEWCARDTYPE)
                        {
                            currentCard = null;
                            if (KioskStatic.DispenserReaderDevice != null)
                            {
                                KioskStatic.logToFile("Dispenser Reader is present: " + KioskStatic.DispenserReaderDevice.GetType().ToString());
                                log.Info("Dispenser Reader is present: " + KioskStatic.DispenserReaderDevice.GetType().ToString());
                            }
                            else
                            {
                                KioskStatic.logToFile("Dispenser Reader not present");
                                log.Info("Dispenser Reader not present");
                            }

                            //cardDispenser = KioskStatic.getCardDispenser(KioskStatic.spCardDispenser); ---Legacy/open port Cleanup
                            if (KioskStatic.config.dispport == -1)//&& autoGeneratedCardNumber == false)
                            {
                                log.Info("Card dispenser is disabled and product with auto generated card number set to Y is exists");
                                KioskStatic.logToFile("Card dispenser is disabled and product with auto generated card number set to Y is exists");
                                return;
                            }
                            cardDispenser = KioskStatic.getCardDispenser(KioskStatic.config.dispport.ToString());
                            DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 429, ""));
                            Application.DoEvents();
                        }
                        else
                        {
                            DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 503));
                            Application.DoEvents();
                        }
                        kioskTransaction.ExecuteRedeemTokenTransaction(cardDispenser, currentCard, DisplayMessageLine, frmOKMsg.ShowUserMessage, ShowThankYou, KioskStatic.receipt);
                        if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                        {
                            KioskStatic.cardAcceptor.EjectCardFront();
                            KioskStatic.cardAcceptor.BlockAllCards();
                        }
                    }
                    catch (Exception exp)
                    {
                        log.Error(exp);
                        KioskStatic.logToFile(exp.Message);
                    }
                    finally
                    {
                        EnableButtons();
                    }
                }
                else
                {
                    TimerMoney.Start();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                TimerMoney.Stop();
                DisplayMessageLine(ex.Message);
                MessageBox.Show(ex.Message + "-" + ex.StackTrace, "Error - Contact Manager");
            }
            //log.LogMethodExit();
        }
        public override void Form_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //StopKioskTimer();
            log.LogMethodExit();
        }
        public override void Form_Activated(object sender, EventArgs e)//Playpas1:starts
        {
            log.LogMethodEntry();
            //ResetKioskTimer();
            //StartKioskTimer();
            log.LogMethodExit();
        }
         

        protected override CreateParams CreateParams
        {
            //this method is used to avoid the able layout flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }

        private void AbortAndExit(string errorMessage)
        {
            log.LogMethodEntry(errorMessage);
            KioskStatic.logToFile("AbortAndExit: " + errorMessage);
            TimerMoney.Stop();

            if (coinAcceptor != null)
                coinAcceptor.disableCoinAcceptor();

            kioskTransaction.ClearTransaction(frmOKMsg.ShowUserMessage);

            string message = "There was an error processing your request. The tokens inserted by you is recognized and will be refunded to you. Please contact our staff with the receipt. [" + errorMessage + "]";
            frmOKMsg.ShowUserMessage(message);
            Close();
            log.LogMethodExit();
        }
        private void btnComplete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetReceiptPrintOptions(kioskTransaction);
            btnCompleteReload.Enabled = btnCompleteNEWCard.Enabled = false;
            function = KioskTransaction.GETNEWCARDTYPE;
            ProductPrice = kioskTransaction.GetTotalPaymentsReceived();//ac.productPrice = ac.totalValue;
            log.LogMethodExit();
        }

        private void DisplayMessageLine(string message)
        {
            //log.LogMethodEntry(message);
            //Application.DoEvents();
            txtMessage.Text = message;
            //log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Application.DoEvents();
            StopKioskTimer();
            if (kioskTransaction.GetTotalPaymentsReceived() > 0
               && kioskTransaction.GetTotalPaymentsReceived() < ProductPrice)
            {
                if (coinAcceptor != null)
                    coinAcceptor.set_acceptance(true);
                btnPrev.Enabled = false;
                btnCompleteNEWCard.Enabled = true;
                TimerMoney.Start();
                log.LogMethodExit();
                return;
            }
            TimerMoney.Stop();
            KioskStatic.logToFile("Exit Timer ticked");
            log.Info("Exit Timer ticked");
            Close();
            log.LogMethodExit();
        }

        private void ShowThankYou(bool receiptPrinted, bool receiptEmailed)
        {
            log.LogMethodEntry(receiptPrinted, receiptEmailed);
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines = kioskTransaction.GetActiveTransactionLines;
            string trxNumber = kioskTransaction.GetTransactionNumber;
            if (string.IsNullOrWhiteSpace(trxNumber) == false)
            {
                string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4121);//Transaction Successful. Thank You.
                string printMsg = (receiptPrinted) ? MessageContainerList.GetMessage(Utilities.ExecutionContext, 498) //PLEASE COLLECT THE RECEIPT
                            : MessageContainerList.GetMessage(Utilities.ExecutionContext, 5000);//Please note down Trx Number on screen for future reference

                if (kioskTransaction.SelectedProductType == KioskTransaction.GETNEWCARDTYPE
                         || kioskTransaction.SelectedProductType == KioskTransaction.GETRECHAREGETYPE)
                {
                    Semnox.Parafait.Transaction.Transaction.TransactionLine cardLine = trxLines.Find(tl => tl.LineValid && tl.card != null
                    && tl.ProductTypeCode != ProductTypeValues.CARDDEPOSIT);
                    string productName = KioskHelper.GetProductName(cardLine.ProductID);
                    message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 952,
                                                                   (cardLine != null ? productName : ""));
                    printMsg = (receiptPrinted) ? MessageContainerList.GetMessage(Utilities.ExecutionContext, 498) //PLEASE COLLECT THE RECEIPT
                        : MessageContainerList.GetMessage(Utilities.ExecutionContext, 5000); //Please note down Trx Number on screen for future reference
                }
                if (receiptEmailed)//override printMsg
                {
                    printMsg = MessageContainerList.GetMessage(Utilities.ExecutionContext, 5013);
                    // "Transaction receipt is emailed to you"
                }
                using (frmTransactionSuccess frm = new frmTransactionSuccess(message, printMsg, trxNumber, "Redeem", false))
                {
                    frm.ShowDialog();
                }
            }
            KioskStatic.logToFile("Exit Redeem Tokens screen");
            log.Info("Exit Redeem Tokens screen");
            this.Close();
            log.LogMethodExit();
        }

        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnPrev.Enabled = false;
            Audio.Stop();
            RefreshGridTotals();
            Application.DoEvents();
            btnPrev.Enabled = true;

            KioskStatic.logToFile("Cancel pressed");
            log.Info("Cancel pressed");

            if (coinAcceptor != null && coinAcceptor.ReceivedCoinDenomination > 0)
            {
                log.LogMethodExit("coinAcceptor.ReceivedCoinDenomination > 0");
                return;
            }

            TimerMoney.Stop();
            btnPrev.Enabled = false;
            Application.DoEvents();

            if (coinAcceptor != null)
                coinAcceptor.disableCoinAcceptor();

            Application.DoEvents();

            if (kioskTransaction.GetTotalPaymentsReceived() > 0
                && kioskTransaction.GetTotalPaymentsReceived() < ProductPrice)
            {
                btnPrev.Enabled = false;
                btnCompleteNEWCard.Enabled = true;
                TimerMoney.Start();
                if (coinAcceptor != null)
                    coinAcceptor.set_acceptance(true);
            }
            else
            {
                this.Close();
            }
            log.LogMethodExit();
        }

        private void frmRedeemTokens_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void btnCompleteReload_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Audio.PlayAudio(Audio.RedeemTokenTapCard);
            using (frmTapCard ftc = new frmTapCard())
            {
                DialogResult dr = ftc.ShowDialog();
                Audio.Stop();

                if (ftc.Card != null) // valid card tapped
                {
                    currentCard = ftc.Card;
                }
                else // time out
                {
                    ftc.Dispose();
                    log.LogMethodExit(" ftc.Dispose();");
                    return;
                }

                ftc.Dispose();
            }
            SetReceiptPrintOptions(kioskTransaction);
            btnCompleteReload.Enabled = btnCompleteNEWCard.Enabled = false;
            function = KioskTransaction.GETRECHAREGETYPE;
            ProductPrice = kioskTransaction.GetTotalPaymentsReceived();// ac.productPrice = ac.totalValue;
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmRedeemTokens");
            try
            {
                this.lblGreeting1.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenHeaderTextForeColor;//Please insert CEC tokens and press complete
                this.label1.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenTokenInsertedTextForeColor;//Tokens Inserted
                this.txtAvlblTokens.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenAvialableTokensTextForeColor;//Available tokens 
                this.btnCompleteNEWCard.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenBtnNewCardTextForeColor;//Button new card
                this.btnCompleteReload.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenBtnLoadTextForeColor;//(Load points to existing card)- 
                this.btnPrev.ForeColor = this.btnCancel.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenBtnBackTextForeColor;//
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenFooterTextForeColor;//RedeemTokensScreenFooterTextForeColor
                this.label2.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenDenominationTextForeColor;//Denomination
                this.label3.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenQuantityTextForeColor;//Quantity
                this.label4.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenPointsTextForeColor;//Points
                this.lblTimeOut.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenTimeOutTextForeColor;//Time out
                this.button1.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenButton1extForeColor;//Time out
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenBtnHomeTextForeColor;//Time out
                btnPrev.BackgroundImage = btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage; //KioskStatic.CurrentTheme.BackButtonImage;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmRedeemTokens: " + ex.Message);
            }
            log.LogMethodExit();
        }
        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                KioskStatic.logToFile("Cancel Button Pressed : Triggering Home Button Action ");
                base.btnHome_Click(sender, e);
            }
            catch (Exception ex)
            {
                log.Error("Error in btnCancel_Click", ex);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }
        private void EnableButtons()
        {
            log.LogMethodEntry();
            btnCancel.Enabled = true;
            btnCompleteNEWCard.Enabled = true;
            btnCompleteReload.Enabled = true;
            log.LogMethodExit();
        }
        private void DisableButtons()
        {
            log.LogMethodEntry();
            btnCancel.Enabled = false;
            btnCompleteNEWCard.Enabled = false;
            btnCompleteReload.Enabled = false;
            log.LogMethodExit();
        }
        private void SetTheme()
        {
            log.LogMethodEntry();
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.RedeemTokensBackgroundImage);//Modification on 17-Dec-2015 for introducing new theme
            }
            catch (Exception ex)
            {
                log.Error("Error occurred in SetTheme" , ex);
            }
            btnCompleteNEWCard.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.RedeemTokenButtonTextAlignment);
            btnCompleteReload.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.RedeemTokenButtonTextAlignment);
            btnCompleteNEWCard.BackgroundImage = ThemeManager.CurrentThemeImages.NewPlayPassButtonBig;
            btnCompleteReload.BackgroundImage = ThemeManager.CurrentThemeImages.RechargePlayPassButtonBig;
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            panelGrid.BackgroundImage = ThemeManager.CurrentThemeImages.RedeemTable;
            lblTimeOut.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
            panel2.BackgroundImage = ThemeManager.CurrentThemeImages.TextEntryBox;
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            log.LogMethodExit();
        }

        private void SetReceiptPrintOptions(KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry();
            string deliveryMode = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "KIOSK_RECEIPT_DELIVERY_MODE");
            kioskTransaction.RececiptDeliveryMode = KioskTransaction.KioskReceiptDeliveryMode.NONE;
            if (deliveryMode == KioskTransaction.KioskReceiptDeliveryMode.ASK.ToString())
            {
                using (frmReceiptDeliveryModeOptions frdmo = new frmReceiptDeliveryModeOptions(Utilities.ExecutionContext, kioskTransaction))
                {
                    frdmo.ShowDialog();
                    kioskTransaction = frdmo.GetKioskTransaction;
                }
            }
            else if (deliveryMode == KioskTransaction.KioskReceiptDeliveryMode.PRINT.ToString())
            {
                kioskTransaction.RececiptDeliveryMode = KioskTransaction.KioskReceiptDeliveryMode.PRINT;
            }
            else if (deliveryMode == KioskTransaction.KioskReceiptDeliveryMode.EMAIL.ToString())
            {
                using (frmGetEmailDetails fged = new frmGetEmailDetails(Utilities.ExecutionContext, kioskTransaction))
                {
                    fged.ShowDialog();
                    this.kioskTransaction = fged.GetKioskTransaction;
                }
            }
            log.LogMethodExit();
        }

        private void InitiateNoteCoinActionTimer()
        {
            log.LogMethodEntry();
            noteCoinActionTimer = new System.Windows.Forms.Timer(this.components);
            noteCoinActionTimer.Interval = 100;
            noteCoinActionTimer.Tick += new EventHandler(NoteCoinActionTimerTick);
            noteCoinActionTimer.Start();
            log.LogMethodExit();
        }
        private void NoteCoinActionTimerTick(object sender, EventArgs e)
        {
            try
            {
                noteCoinActionTimer.Stop();
                bool hasCashToProcess = kioskTransaction.HasMoneyToProcess();
                if (hasCashToProcess)
                {
                    KioskStatic.logToFile("Performing NoteCoinActionTimer action");
                    log.Debug("Performing NoteCoinActionTimer action");
                    kioskTransaction.ProcessReceivedMoney(null);
                    TimeOut.Abort();
                    RefreshGridTotals();
                    List<TransactionPaymentsDTO> transactionPaymentsDTOList = kioskTransaction.GetTransactionPaymentsDTOList;
                    if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Count >= config.Coins.Length)
                    { 
                        coinAcceptor.disableCoinAcceptor();
                        using (frmOKMsg fok = new frmOKMsg("You have inserted maximum allowed coins per transaction. Please press Complete to redeem inserted tokens."))
                        { fok.ShowDialog(); }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in NoteCoinActionTimerTick: " + ex.Message);
            }
            finally
            {
                noteCoinActionTimer.Start();
            }
        }
    }
}

