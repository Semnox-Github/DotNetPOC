/********************************************************************************************
* Project Name - Parafait_Kiosk -frmRedeemTokens
* Description  - frmRedeemTokens 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards
 * 2.80        5-Sep-2019      Deeksha             Added logger methods.
 *2.80        14-Nov-2019      Girish Kundar       Modified: As part of ticket printer integration
 *2.150.1     22-Feb-2023      Guru S A            Kiosk Cart Enhancements
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
        //bool TransactionSuccess = false;
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
        private KioskTransaction kioskTransaction;
        //private bool canPerformNoteCoinReceivedAction = false;
        private System.Windows.Forms.Timer noteCoinActionTimer;
        public frmRedeemTokens(bool AllowNewCard, CustomerDTO customerDTO = null)
        {
            log.LogMethodEntry(AllowNewCard, customerDTO);
            Utilities.setLanguage(this);
            InitializeComponent();
            kioskTransaction = new KioskTransaction(Utilities);
            //TimerMoney.Enabled = exitTimer.Enabled = false;
            TimerMoney.Enabled = false;
            KioskTimerSwitch(false);
            AMOUNT_WITH_CURRENCY_SYMBOL = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");
            _allowNewCard = AllowNewCard;

            btnCompleteNEWCard.BackgroundImage = ThemeManager.CurrentThemeImages.NewCardButtonSmall;//Starts:Modification on 17-Dec-2015 for introducing new theme
            btnCompleteReload.BackgroundImage = ThemeManager.CurrentThemeImages.LoadExistingImage;
            btnCompleteNEWCard.Size = btnCompleteNEWCard.BackgroundImage.Size;
            btnCompleteReload.Size = btnCompleteReload.BackgroundImage.Size;
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnHome.Size = btnHome.BackgroundImage.Size;
            panelGrid.BackgroundImage = ThemeManager.CurrentThemeImages.RedeemTable;
            lblTimeOut.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
            panel2.BackgroundImage = ThemeManager.CurrentThemeImages.TextEntryBox;//Ends:Modification on 17-Dec-2015 for introducing new theme

            Utilities.setLanguage(this);
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            if (KioskStatic.CurrentTheme.ThemeId != 0)
                KioskStatic.setDefaultFont(this);//Starts:Modification on 17-Dec-2015 for introducing new theme
            if (KioskStatic.CurrentTheme.TextForeColor != Color.White)
            {
                txtAvlblTokens.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            else
            {
                txtAvlblTokens.ForeColor = Color.DarkOrchid;
            }
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);//Ends:Modification on 17-Dec-2015 for introducing new theme
            DisplayMessageLine("");

            lblTimeOut.Text = KioskStatic.MONEY_SCREEN_TIMEOUT.ToString("#0");
            savTimeOutFont = lblTimeOut.Font;
            lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            this.customerDTO = customerDTO;

            this.ShowInTaskbar = false;
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
                DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext,"Thank You"));
            }

            KioskStatic.logToFile("Exiting...");

            Audio.Stop();
            //if (coinAcceptor != null)
            //    coinAcceptor.CloseComm();
            log.LogMethodExit("Exiting...");
        }

        void initializeForm()
        {
            log.LogMethodEntry();
            try
            {
                this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing initializeForm()" + ex.Message);
            }

            //btnCompleteNEWCard.Location = new Point((this.Width - btnCompleteNEWCard.Width) / 2, btnCompleteNEWCard.Location.Y);
            //panelGrid.Location = new Point((this.Width - panelGrid.Width) / 2, panelGrid.Location.Y);

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

        void handleCardReadDummy()
        {
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
            initializeForm();

            lblSiteName.Text = KioskStatic.SiteHeading;
            lblGreeting1.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext,796);
             DisplayMessageLine(lblGreeting1.Text);

            Application.DoEvents();

            KioskStatic.logToFile("Enter Redeem Tokens screen");
            log.Info("Enter Redeem Tokens screen");
            Audio.PlayAudio(Audio.RedeemInsertToken);
            btnCompleteNEWCard.Visible = (_allowNewCard && !KioskStatic.DisableNewCard);
            if (!btnCompleteNEWCard.Visible)
            {
                btnCompleteReload.Left = btnCompleteReload.Left - (btnCompleteNEWCard.Width / 2);
            } 

            TimerMoney.Start();
            InitiateNoteCoinActionTimer();
            log.LogMethodExit();
        }

        void coinAcceptorDataReceived()
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
                    decimal netTrxAmount = kioskTransaction.GetTrxNetAmount();
                    kioskTransaction.AddCoinPayment(denomination);
                    decimal amountReceived = kioskTransaction.GetTotalPaymentsReceived();
                    coinAcceptor.ReceivedCoinDenomination = 0;
                    decimal balance = netTrxAmount - amountReceived;
                    coinAcceptor.AmountRemainingToBeCollected = (double)balance;
                    KioskStatic.ReceivedDenominationToActivityLog(Utilities.ExecutionContext, kioskTransaction.GetTransactionId,
                                                                  (currentCard == null ? "" : currentCard.CardNumber),
                                                                   coinAcceptor.ReceivedCoinDenomination, KioskTransaction.GETCOININ,
                                                                KioskTransaction.GETCOININMSG, KioskStatic.COIN);
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
            log.LogMethodEntry();
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
                            DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext,425));
                            lblTimeOut.Font = savTimeOutFont;
                            lblTimeOut.Text = "Time Out";
                            btnPrev.Enabled = false;
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
                            lblTimeOut.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext,800);
                    }

                    btnPrev.Enabled = false;
                    btnCompleteReload.Enabled = btnCompleteNEWCard.Enabled = true;
                }

                if (coinMessage.EndsWith("accepted") || coinMessage.EndsWith("rejected"))
                {
                    DisplayMessageLine(coinMessage.Replace("accepted", MessageContainerList.GetMessage(Utilities.ExecutionContext,"accepted"))
                                                  .Replace("rejected", MessageContainerList.GetMessage(Utilities.ExecutionContext,"rejected"))
                                                  .Replace("Denomination", MessageContainerList.GetMessage(Utilities.ExecutionContext,"Denomination")));
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
                        DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext,coinMessage));
                    else
                        DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext,796));
                }

                Application.DoEvents();

                if (kioskTransaction.GetTotalPaymentsReceived() >= ProductPrice)
                {
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
                        DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext,429, ""));
                        Application.DoEvents();
                        //dispenseCards();
                    }
                    else
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext,503));
                        Application.DoEvents(); 
                    }
                    kioskTransaction.ExecuteRedeemTokenTransaction(cardDispenser, currentCard, DisplayMessageLine, frmOKMsg.ShowUserMessage, ShowThankYou, KioskStatic.receipt);
                    if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.cardAcceptor.EjectCardFront();
                        KioskStatic.cardAcceptor.BlockAllCards();
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
            btnCompleteReload.Enabled = btnCompleteNEWCard.Enabled = false;
            function = KioskTransaction.GETNEWCARDTYPE;
            ProductPrice = kioskTransaction.GetTotalPaymentsReceived();//ac.productPrice = ac.totalValue;
            log.LogMethodExit();
        }

        private void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            Application.DoEvents(); 
            txtMessage.Text = message;
            log.LogMethodExit();
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
                    message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 952,
                                                                   (cardLine != null ? cardLine.ProductName : ""));
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

        private void btnPrev_Click(object sender, EventArgs e)
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
            try
            {
                Audio.PlayAudio(Audio.RedeemTokenTapCard);
                frmTapCard ftc = new frmTapCard();
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

                btnCompleteReload.Enabled = btnCompleteNEWCard.Enabled = false;
                function = KioskTransaction.GETRECHAREGETYPE;
            ProductPrice = kioskTransaction.GetTotalPaymentsReceived();// ac.productPrice = ac.totalValue;
            log.LogMethodExit();
            }
            catch (Exception ex)
            {
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile(ex.Message);
                log.Error(ex);
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

