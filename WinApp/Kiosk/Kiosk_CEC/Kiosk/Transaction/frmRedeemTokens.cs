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
 * 2.80        4-Sep-2019      Deeksha             Added logger methods.
 *2.80        14-Nov-2019      Girish Kundar  Modified: As part of ticket printer integration
 *2.80.1      02-Feb-2021      Deeksha            Theme changes to support customized Images/Font
 *2.130.0     30-Jun-2021      Dakshak            Theme changes to support customized Font ForeColor
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

namespace Parafait_Kiosk
{
    public partial class frmRedeemTokens : BaseForm
    {
        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";
        string Function;
        Card CurrentCard;
        decimal ProductPrice;
        bool TransactionSuccess = false;
        bool _allowNewCard = true;

        CoinAcceptor coinAcceptor;
        CardDispenser cardDispenser;

        KioskStatic.acceptance ac;
        KioskStatic.configuration config = KioskStatic.config;
        KioskStatic.receipt_format rc = KioskStatic.rc;
        private readonly TagNumberParser tagNumberParser;

        //byte[] inp;

        int coinAcceptorTimeout = 0;

        Semnox.Parafait.Transaction.Transaction CurrentTrx;
        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;

        CustomerDTO customerDTO = null;

        Font savTimeOutFont;
        public frmRedeemTokens(bool AllowNewCard, CustomerDTO customerDTO = null)
        {
            log.LogMethodEntry(AllowNewCard, customerDTO);
            Utilities.setLanguage();
            InitializeComponent();
            //TimerMoney.Enabled = exitTimer.Enabled = false;
            TimerMoney.Enabled = false;
            KioskTimerSwitch(false);
            Utilities.setLanguage(this);
            //KioskStatic.setDefaultFont(this);
            _allowNewCard = AllowNewCard;
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);

            KioskStatic.formatMessageLine(txtMessage, 26, Properties.Resources.bottom_bar);
            displayMessageLine("", MESSAGE);

            lblTimeOut.Text = KioskStatic.MONEY_SCREEN_TIMEOUT.ToString("#0");
            savTimeOutFont = lblTimeOut.Font;
            lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            this.customerDTO = customerDTO;

            this.ShowInTaskbar = false;
            //--Legacy/open port cleanup
            //if (KioskStatic.CoinAcceptorModel.Equals(CoinAcceptor.Models.MICROCOIN_SP))
            //{
            //    coinAcceptor = KioskStatic.getCoinAcceptor();
            //    ((MicrocoinSP)coinAcceptor).dataReceivedEvent = coinAcceptorDataReceived;
            //    coinAcceptor.set_acceptance(true);    
            //}
            //else 
            if (config.coinAcceptorport > 0)
            {
                //coinAcceptor = KioskStatic.getCoinAcceptor(config.coinAcceptorport.ToString());
                coinAcceptor = KioskStatic.getCoinAcceptor();
                log.LogVariableState("KioskStatic.config.Coins", KioskStatic.config.Coins);
                log.LogVariableState("coinAcceptor",coinAcceptor);
                log.LogVariableState("KioskStatic.spCoinAcceptor", KioskStatic.spCoinAcceptor);
                coinAcceptor.spCoinAcceptor = KioskStatic.spCoinAcceptor;
                if (coinAcceptor.set_acceptance(true))
                {
                    KioskStatic.caReceiveAction = coinAcceptorDataReceived;
                    log.Info("KioskStatic.caReceiveAction = coinAcceptorDataReceived");
                }
                else
                {
                    log.Info("KioskStatic.caReceiveAction = null");
                    KioskStatic.caReceiveAction = null;
                }
            }

            if (coinAcceptor != null)
                ac = KioskStatic.ac = coinAcceptor.acceptance;
            else
                ac = KioskStatic.ac = new KioskStatic.acceptance();

            btnCompleteReload.Enabled = btnCompleteNEWCard.Enabled = false;
            SetCustomizedFontColors();

            log.LogMethodExit();
        }

        private void frmNewcard_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (ac.totalValue > 0)
            {
                e.Cancel = true;
                btnCompleteReload.Enabled = true;
                btnCompleteNEWCard.Enabled = (_allowNewCard && !KioskStatic.DisableNewCard);
                refreshGridTotals();
                TimerMoney.Start();
                log.LogMethodExit("ac.totalValue > 0");
                return;
            }

            TimerMoney.Stop();

            if (coinAcceptor != null)
                coinAcceptor.disableCoinAcceptor();

            KioskStatic.caReceiveAction = null;
            KioskStatic.coinAcceptorDatareceived = false;

            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
            {
                KioskStatic.cardAcceptor.EjectCardFront();
                KioskStatic.cardAcceptor.BlockAllCards();
            }

            if (Function == "I" && KioskStatic.DispenserReaderDevice != null)
            {
                KioskStatic.DispenserReaderDevice.UnRegister();
                KioskStatic.logToFile(this.Name + ": Dispenser Reader unregistered");
                log.Info(this.Name + ": Dispenser Reader unregistered");
            }

            if (ac.totalValue <= 0)
            {
                displayMessageLine(MessageUtils.getMessage("Thank You"), MESSAGE);
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
                this.BackgroundImage = KioskStatic.CurrentTheme.RedeemBackgroundImage;
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

            refreshGridTotals();

            ProductPrice = 999999999;
            ac.productPrice = ProductPrice;

            dgvCashInserted.DefaultCellStyle.Font = new Font(dgvCashInserted.DefaultCellStyle.Font.FontFamily, 24);
            dgvCashInserted.ColumnHeadersVisible = false;
            log.LogMethodExit();
        }

        void refreshGridTotals()
        {
            log.LogMethodEntry();
            for (int i = 0; i < dgvCashInserted.Rows.Count; i++)
            {
                dgvCashInserted["Quantity", i].Value = 0;
                dgvCashInserted["Amount", i].Value = 0;
            }

            for (int i = 0; i < ac.coinCount; i++)
            {
                int index = getGridRowIndex("C" + ac.coinDenominations[i].ToString());
                if (index < 0)
                    continue;

                dgvCashInserted["Quantity", index].Value = Convert.ToInt32(dgvCashInserted["Quantity", index].Value) + 1;
                dgvCashInserted["Amount", index].Value = Convert.ToDecimal(dgvCashInserted["Amount", index].Value) + config.Coins[ac.coinDenominations[i]].Value;
                dgvCashInserted.Rows[index].DefaultCellStyle.BackColor = Color.AliceBlue;
            }

            dgvCashInserted.Refresh();

            txtAvlblTokens.Text = ac.coinCount.ToString();

            Application.DoEvents();
            log.LogMethodExit();
        }

        int getGridRowIndex(string denomination)
        {
            log.LogMethodEntry(denomination);
            foreach (DataGridViewRow dr in dgvCashInserted.Rows)
            {
                if (dr.Tag == null)
                    continue;

                if (dr.Tag.ToString() == denomination)
                    return dr.Index;
            }

            log.LogMethodExit(-1);
            return -1;
        }

        private void frmRedeemTokens_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            initializeForm();

            lblSiteName.Text = KioskStatic.SiteHeading;
            lblGreeting1.Text = MessageUtils.getMessage(796);

            lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            //txtMessage.ForeColor = lblGreeting1.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;
            //txtMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            //txtMessage.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            displayMessageLine(lblGreeting1.Text, MESSAGE);

            Application.DoEvents();

            KioskStatic.logToFile("Enter Redeem Tokens screen");
            log.Info("Enter Redeem Tokens screen");
            btnCompleteNEWCard.Visible = (_allowNewCard && !KioskStatic.DisableNewCard);
            if (!btnCompleteNEWCard.Visible)
            {
                btnCompleteReload.Left = btnCompleteReload.Left - (btnCompleteNEWCard.Width / 2);
            }
            if (_allowNewCard == false && !KioskStatic.DisableNewCard)
            {
                string mes = Utilities.MessageUtils.getMessage(460);
                KioskStatic.logToFile(mes);
                frmOKMsg f = new frmOKMsg(mes);
                f.ShowDialog();
            }

            Audio.PlayAudio(Audio.RedeemInsertToken);

            TimerMoney.Start();
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
                    KioskStatic.updateKioskActivityLog(-1, coinAcceptor.ReceivedCoinDenomination, (CurrentCard == null ? "" : CurrentCard.CardNumber), "COIN-IN", "Coin Inserted", ac);
                    coinAcceptor.ReceivedCoinDenomination = 0;

                    Invoke((MethodInvoker)delegate
                    {
                        TimeOut.Abort();
                        refreshGridTotals();

                        if (ac.coinCount >= ac.coinDenominations.Length)
                        {
                            coinAcceptor.disableCoinAcceptor();
                            frmOKMsg fok = new frmOKMsg("You have inserted maximum allowed coins per transaction. Please press Complete to redeem inserted tokens.");
                            fok.ShowDialog();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in : " + ex.Message);
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
                if (!KioskStatic.coinAcceptorDatareceived)
                {
                    coinAcceptorTimeout++;
                    if (coinAcceptorTimeout > 30 && KioskStatic.spCoinAcceptor.IsOpen)
                    {
                        coinMessage = MessageUtils.getMessage(424);
                        KioskStatic.logToFile("Coin acceptor timeout / offline");
                        log.Info("Coin acceptor timeout / offline");
                    }

                    if (coinAcceptorTimeout % 5 == 0)
                    {
                        log.Info("KioskStatic.spCoinAcceptor.IsOpen : " + KioskStatic.spCoinAcceptor.IsOpen);
                        if (KioskStatic.spCoinAcceptor.IsOpen)
                        {
                            if (coinAcceptor != null)
                                coinAcceptor.checkStatus();
                        }
                    }
                }

                if (ac.totalValue <= 0)
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
                    if (ac.totalValue <= 0) // check again if any money has been inserted at the last minute
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
                            displayMessageLine(MessageUtils.getMessage(425), ERROR);
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
                    if (Function == "I")
                        lblTimeOut.Text = "";
                    else
                    {
                        lblTimeOut.Font = savTimeOutFont;
                        if (ServerDateTime.Now.Millisecond / 500 > 0)
                            lblTimeOut.Text = "";
                        else
                            lblTimeOut.Text = MessageUtils.getMessage(800);
                    }

                    btnPrev.Enabled = false;
                    btnCompleteReload.Enabled = true;
                    btnCompleteNEWCard.Enabled = (_allowNewCard && !KioskStatic.DisableNewCard);
                }

                if (coinMessage.EndsWith("accepted") || coinMessage.EndsWith("rejected"))
                {
                    displayMessageLine(coinMessage.Replace("accepted", MessageUtils.getMessage("accepted"))
                                                  .Replace("rejected", MessageUtils.getMessage("rejected"))
                                                  .Replace("Denomination", MessageUtils.getMessage("Denomination")), MESSAGE);
                    Audio.Stop();
                }
                else
                {
                    if (coinAcceptor != null && coinAcceptor.criticalError)
                    {
                        frmOKMsg f = new frmOKMsg(MessageUtils.getMessage(coinMessage) + ". " + Utilities.MessageUtils.getMessage(441));
                        f.ShowDialog();
                        abortAndExit(coinMessage);
                        log.LogMethodExit(coinMessage);
                        return;
                    }
                    else if (string.IsNullOrEmpty(coinMessage) == false)
                        displayMessageLine(MessageUtils.getMessage(coinMessage), MESSAGE);
                    else
                        displayMessageLine(MessageUtils.getMessage(796), MESSAGE);
                }

                Application.DoEvents();

                if (ac.totalValue >= ProductPrice)
                {
                    TimerMoney.Stop();
                    refreshGridTotals();
                    Application.DoEvents();

                    KioskStatic.logToFile("Valid Tokens inserted: " + ac.totalValue.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) + "; " + "Required: " + ProductPrice.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT));
                    log.Info("Valid Tokens inserted: " + ac.totalValue.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) + "; " + "Required: " + ProductPrice.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT));
                    if (coinAcceptor != null)
                    {
                        System.Threading.Thread.Sleep(300);
                        coinAcceptor.disableCoinAcceptor();
                        KioskStatic.logToFile("Coin acceptor disabled");
                        log.Info("Coin acceptor disabled");
                    }

                    if (Function == "I")
                    {
                        if (KioskStatic.DispenserReaderDevice != null)
                        {
                            KioskStatic.DispenserReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));
                            KioskStatic.logToFile("Dispenser Reader registered: " + KioskStatic.DispenserReaderDevice.GetType().ToString());
                            log.Info("Dispenser Reader registered: " + KioskStatic.DispenserReaderDevice.GetType().ToString());
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
                        displayMessageLine(MessageUtils.getMessage(429, ""), MESSAGE);
                        Application.DoEvents();
                        dispenseCards();
                    }
                    else
                    {
                        displayMessageLine(MessageUtils.getMessage(503), MESSAGE);
                        Application.DoEvents();
                        redeemTokens();
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
                displayMessageLine(ex.Message, ERROR);
                MessageBox.Show(ex.Message + "-" + ex.StackTrace, "Error - Contact Manager");
            }
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    displayMessageLine(message, ERROR);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string CardNumber = tagNumber.Value;
                try
                {
                    handleCardRead(CardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    displayMessageLine(ex.Message, ERROR);
                }
            }
            log.LogMethodExit();
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            if (cardDispenser != null)
                cardDispenser.HandleDispenserCardRead(inCardNumber);
            log.LogMethodExit();
        }
        //Begin Timer Cleanup
        //protected override CreateParams CreateParams
        //{
        //    //this method is used to avoid the able layout flickering.
        //    get
        //    {
        //        CreateParams CP = base.CreateParams;
        //        CP.ExStyle = CP.ExStyle | 0x02000000;
        //        return CP;
        //    }
        //} 
        //End Timer Cleanup

        void redeemTokens()
        {
            log.LogMethodEntry();
            btnCompleteReload.Enabled = btnCompleteNEWCard.Enabled = false;
            Audio.PlayAudio(Audio.WaitForCardTopUp);
            KioskStatic.logToFile("Redeeming Tokens to card: " + CurrentCard.CardNumber);
            log.Info("Redeeming Tokens to card: " + CurrentCard.CardNumber);
            if (createTransaction())
            {
                KioskStatic.logToFile("Redeem successful");
                log.Info("Redeem successful");
                KioskStatic.updateKioskActivityLog(-1, -1, CurrentCard.CardNumber, "TOP-UP", "Redeem token", ac);

                displayMessageLine(MessageUtils.getMessage(797), WARNING);

                if (KioskStatic.receipt == true)
                {
                    try
                    {
                        print_receipt();
                        displayMessageLine(MessageUtils.getMessage(498), WARNING);
                        Audio.PlayAudio(Audio.CollectReceipt, Audio.ThankYouEnjoyGame);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        displayMessageLine(MessageUtils.getMessage(431, ex.Message), ERROR);
                        Audio.PlayAudio(Audio.ThankYouEnjoyGame);
                    }
                }
                else
                {
                    Audio.PlayAudio(Audio.ThankYouEnjoyGame);
                }

                ac.totalValue = 0;

                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                }

                showThankYou();
            }
            else
            {
                btnCompleteReload.Enabled = true;
                btnCompleteNEWCard.Enabled = (_allowNewCard && !KioskStatic.DisableNewCard);
                abortAndExit("Redeem failed");
            }
            log.LogMethodExit();
        }

        void abortAndExit(string errorMessage)
        {
            log.LogMethodEntry(errorMessage);
            KioskStatic.logToFile("AbortAndExit: " + errorMessage);
            TimerMoney.Stop();

            if (coinAcceptor != null)
                coinAcceptor.disableCoinAcceptor();

            if (KioskStatic.receipt)
                print_receipt(true);

            string message = "There was an error processing your request. The tokens inserted by you is recognized and will be refunded to you. Please contact our staff with the receipt. [" + errorMessage + "]";
            frmOKMsg frm = new frmOKMsg(message);
            frm.ShowDialog();

            ac.totalValue = 0;

            Close();
            log.LogMethodExit();
        }

        bool createTransaction()
        {
            log.LogMethodEntry();
            if (CurrentCard == null)
            {
                displayMessageLine(MessageUtils.getMessage(504), ERROR);
                log.LogMethodExit(MessageUtils.getMessage(504));
                return false;
            }

            string message = "";
            try
            {
                if (CurrentCard.CardStatus.Equals("NEW"))
                {
                    CurrentTrx = new Semnox.Parafait.Transaction.Transaction(KioskStatic.Utilities);
                    CurrentTrx.PaymentReference = "Kiosk Transaction";
                    CurrentTrx.PaymentMode = 1;

                    if (CurrentTrx.createTransactionLine(CurrentCard, KioskStatic.Utilities.ParafaitEnv.CardDepositProductId, 0, 1, ref message) != 0)
                    {
                        displayMessageLine("Error: " + message, ERROR);
                        KioskStatic.logToFile("Error TrxVarLine1: " + message);
                        log.LogMethodExit("Error TrxVarLine1: " + message);
                        return false;
                    }


                    int retcode = CurrentTrx.SaveTransacation(ref message);
                    if (retcode != 0)
                    {
                        displayMessageLine("Error: " + message, ERROR);
                        KioskStatic.logToFile("Error TrxSave: " + message);
                        log.LogMethodExit("Error TrxSave: " + message);
                        return false;
                    }
                    else
                    {
                        displayMessageLine(message, MESSAGE);
                        ac.TrxId = (int)CurrentTrx.Trx_id;
                        KioskStatic.logToFile("Success TrxSave: " + message);
                    }
                }

                CurrentCard.getCardDetails(CurrentCard.CardNumber); // refresh
                TaskProcs tasks = new TaskProcs(KioskStatic.Utilities);
                int sourceTrxId = (CurrentTrx != null && CurrentTrx.Trx_id > 0 ? CurrentTrx.Trx_id : -1); 
                if (!tasks.exchangeTokenForCredit(CurrentCard, ac.totalCount, (double)ac.totalValue, "Redeem tokens from Kiosk", ref message, sourceTrxId))
                {
                    displayMessageLine("Error: " + message, ERROR);
                    log.LogMethodExit("Error: " + message);
                    return false;
                }

                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(message + ":" + ex.Message, ERROR);
                KioskStatic.logToFile(message + ":" + ex.Message);
                MessageBox.Show(ex.StackTrace);
                log.LogMethodExit(false);
                return false;
            }
        }

        int cardDispenseRetryCount = 3;
        void dispenseCards()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Dispensing Cards");
            cardDispenseRetryCount = 3;
            string cardNumber = "";
            try
            {
                if (Function == "I")
                {
                    if (ac.totalValue > 0 || ProductPrice == 0)
                    {
                        Audio.PlayAudio(Audio.WaitForCardDispense);
                        string message = "";

                        while (true)
                        {
                            cardNumber = "";
                            bool succ = false;
                            Thread.Sleep(300);

                            succ = cardDispenser.doDispenseCard(ref cardNumber, ref message);
                            if (!succ)
                            {
                                KioskStatic.logToFile(message);
                                cardDispenseRetryCount--;
                                if (cardDispenseRetryCount > 0)
                                {
                                    txtMessage.Text = "Dispense Failed. Retrying [" + (3 - cardDispenseRetryCount).ToString() + "]";
                                    KioskStatic.logToFile(txtMessage.Text);
                                    log.Info(txtMessage.Text);
                                    continue;
                                }
                                else
                                {
                                    txtMessage.Text = "Unable to issue card after MAX retries. Contact Staff.";
                                    btnPrev.Enabled = true;

                                    abortAndExit(txtMessage.Text + " " + message);
                                    log.LogMethodExit(txtMessage.Text + " " + message);
                                    return;
                                }
                            }
                            else if (string.IsNullOrEmpty(cardNumber))
                            {
                                txtMessage.Text = "Card Dispensed but not read. Rejecting";
                                KioskStatic.logToFile(txtMessage.Text);
                                Thread.Sleep(300);
                                if (!cardDispenser.doRejectCard(ref message))
                                {
                                    displayMessageLine(message, ERROR);
                                    abortAndExit("Card dispenser error. Unable to reject card: " + message);
                                    log.LogMethodExit("Card dispenser error. Unable to reject card: " + message);
                                    return;
                                }
                                cardDispenseRetryCount--;
                                if (cardDispenseRetryCount > 0)
                                    continue;
                                else
                                {
                                    txtMessage.Text = "Unable to issue card after MAX retries. Contact Staff.";

                                    abortAndExit(txtMessage.Text);
                                    log.LogMethodExit(txtMessage.Text);
                                    return;
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (KioskStatic.CardDispenserModel.Equals(CardDispenser.Models.SCT0M0))
                                        KioskStatic.DispenserReaderDevice = new DeviceClass();

                                    if (ParafaitEnv.MIFARE_CARD)
                                    {
                                        CurrentCard = new MifareCard(KioskStatic.DispenserReaderDevice, cardNumber, "External POS", Utilities);
                                    }
                                    else
                                    {
                                        CurrentCard = new Card(KioskStatic.DispenserReaderDevice, cardNumber, "External POS", Utilities);
                                    }

                                    if (KioskStatic.CardDispenserModel.Equals(CardDispenser.Models.SCT0M0))
                                        KioskStatic.DispenserReaderDevice = null;
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                    txtMessage.Text = ex.Message;
                                    KioskStatic.logToFile(ex.Message);
                                    CurrentCard = null;
                                    if (KioskStatic.CardDispenserModel.Equals(CardDispenser.Models.SCT0M0))
                                        KioskStatic.DispenserReaderDevice = null;
                                }

                                if (CurrentCard == null)
                                    TransactionSuccess = false;
                                else
                                    TransactionSuccess = true;

                                if (TransactionSuccess)
                                {
                                    TransactionSuccess = createTransaction();
                                    KioskStatic.logToFile("CreateTransaction returned " + TransactionSuccess.ToString());
                                    log.Info("CreateTransaction returned " + TransactionSuccess.ToString());
                                }

                                if (TransactionSuccess)
                                {
                                    if (customerDTO != null)
                                    {
                                        CurrentCard.customerDTO = customerDTO;
                                        CurrentCard.updateCustomer();
                                    }
                                    KioskStatic.updateKioskActivityLog(-1, -1, cardNumber, "NEWCARD", "Redeem Token", ac);
                                    ac.totalValue = 0;

                                    Thread.Sleep(300);
                                    cardDispenser.doEjectCard(ref message);

                                    if (KioskStatic.receipt == true)
                                    {
                                        try
                                        {
                                            print_receipt();
                                            displayMessageLine(MessageUtils.getMessage(435), WARNING);
                                            Audio.PlayAudio(Audio.CollectCardAndReceipt, Audio.ThankYouEnjoyGame);
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                            displayMessageLine(MessageUtils.getMessage(436, ex.Message), ERROR);
                                            Audio.PlayAudio(Audio.CollectCard, Audio.ThankYouEnjoyGame);
                                        }
                                    }
                                    else
                                        displayMessageLine(MessageUtils.getMessage(437), WARNING);

                                    showThankYou();
                                    log.LogMethodExit("showThankYou()");
                                    return;
                                }
                                else
                                {
                                    cardDispenseRetryCount--;
                                    KioskStatic.updateKioskActivityLog(-1, -1, cardNumber, "NEWCARD", "Redeem Token Failed", ac);

                                    if (!cardDispenser.doRejectCard(ref message))
                                    {
                                        displayMessageLine(message, ERROR);
                                        abortAndExit("Card dispenser error. Unable to reject card: " + message);
                                        log.LogMethodExit("Card dispenser error. Unable to reject card: " + message);
                                        return;
                                    }

                                    if (cardDispenseRetryCount > 0)
                                    {
                                        txtMessage.Text = "Card issue failed. Retrying [" + (3 - cardDispenseRetryCount).ToString() + "]";
                                        KioskStatic.logToFile(txtMessage.Text);
                                        log.Info(txtMessage.Text);
                                        continue;
                                    }
                                    else
                                    {
                                        txtMessage.Text = "Unable to issue card after MAX retries. Contact Staff.";
                                        abortAndExit(txtMessage.Text);
                                        log.LogMethodExit("Unable to issue card after MAX retries. Contact Staff.");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show("PLEASE CONTACT ADMIN: " + ex.Message);
                KioskStatic.updateKioskActivityLog(-1, -1, cardNumber, "NEWCARD", "Redeem Token:" + ex.Message, ac);
                btnPrev.Enabled = true;
                abortAndExit(txtMessage.Text);
            }
            log.LogMethodExit();
        }

        void print_receipt(bool isAbort)
        {
            log.LogMethodEntry(isAbort);
            if (KioskStatic.isUSBPrinter)
                print_receiptUSB(isAbort);
            //else
            //{
            //    print_receiptSP(isAbort);
            //}
            log.LogMethodExit();
        }

        void print_receipt()
        {
            log.LogMethodEntry();
            print_receipt(false);
            log.LogMethodExit();
        }

        void print_receiptUSB(bool isAbort)
        {
            log.LogMethodEntry(isAbort);
            if (isAbort)
            {
                System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                List<string> printString = new List<string>();

                printString.Add(rc.head);
                if (!string.IsNullOrEmpty(rc.a1))
                {
                    printString.Add(rc.a1.Replace("@Date", DateTime.Now.ToString("ddd, dd-MMM-yyyy h:mm tt")).Replace("Date:", MessageUtils.getMessage("Date:")));
                }

                if (!string.IsNullOrEmpty(rc.a21))
                {
                    printString.Add(rc.a21.Replace("@POS", ParafaitEnv.POSMachine).Replace("Kiosk:", MessageUtils.getMessage("Kiosk:")));
                }

                printString.Add(Environment.NewLine);
                printString.Add(Environment.NewLine);
                printString.Add("*******************");
                printString.Add(Environment.NewLine);
                printString.Add(MessageUtils.getMessage(439)); //"TRANSACTION ABORTED";
                printString.Add(Environment.NewLine);
                printString.Add("*******************");
                printString.Add("*******************");
                printString.Add(Environment.NewLine);

                printString.Add(MessageUtils.getMessage("Total Tokens Inserted") + ": " + ac.totalCount.ToString(ParafaitEnv.NUMBER_FORMAT));
                printString.Add(Environment.NewLine);
                printString.Add(MessageUtils.getMessage(441));

                pd.PrintPage += (sender, e) => PrintUSB(e, printString);
                pd.Print();
            }
            else
            {
                System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                List<string> printString = new List<string>();

                printString.Add(rc.head);
                if (!string.IsNullOrEmpty(rc.a1))
                {
                    printString.Add(rc.a1.Replace("@Date", DateTime.Now.ToString("ddd, dd-MMM-yyyy h:mm tt")).Replace("Date:", MessageUtils.getMessage("Date:")));
                }

                if (!string.IsNullOrEmpty(rc.a21))
                {
                    printString.Add(rc.a21.Replace("@POS", ParafaitEnv.POSMachine).Replace("Kiosk:", MessageUtils.getMessage("Kiosk:")));
                }

                printString.Add(Environment.NewLine);
                printString.Add(Environment.NewLine);
                printString.Add("*******************");
                printString.Add(Environment.NewLine);
                printString.Add(MessageUtils.getMessage(797)); //"Redeem Successful";
                printString.Add(Environment.NewLine);
                printString.Add("*******************");
                printString.Add("*******************");
                printString.Add(Environment.NewLine);

                printString.Add(MessageUtils.getMessage("Total Tokens Inserted") + ": " + ac.totalCount.ToString(ParafaitEnv.NUMBER_FORMAT));
                printString.Add(MessageUtils.getMessage("Points Loaded") + ": " + ProductPrice.ToString(ParafaitEnv.NUMBER_FORMAT));//Modified - 06Jun2015 - Replaced
                printString.Add(Environment.NewLine);
                printString.Add(Environment.NewLine);
                printString.Add(MessageUtils.getMessage(499));

                pd.PrintPage += (sender, e) => PrintUSB(e, printString);
                pd.Print();
            }
            log.LogMethodExit();
        }

        void PrintUSB(System.Drawing.Printing.PrintPageEventArgs e, List<string> input)
        {
            log.LogMethodEntry(e, input);
            Font f = new Font("Verdana", 10f);
            float height = e.Graphics.MeasureString("ABCD", f).Height;
            float locY = 10;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            foreach (string s in input)
            {
                e.Graphics.DrawString(s, f, Brushes.Black, new Rectangle(0, (int)locY, e.PageBounds.Width, (int)height), sf);
                locY += height;
            }
            log.LogMethodExit();
        }

        private void btnCompleteNEWCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnCompleteReload.Enabled = btnCompleteNEWCard.Enabled = false;
            Function = "I";
            ProductPrice = ac.productPrice = ac.totalValue;
            log.LogMethodExit();
        }

        private void displayMessageLine(string message, string msgType)
        {
            log.LogMethodEntry(message, msgType);
            Application.DoEvents();

            txtMessage.Text = message;
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Application.DoEvents();
            StopKioskTimer();
            if (ac.totalValue > 0)
            {
                if (coinAcceptor != null)
                    coinAcceptor.set_acceptance(true);
                btnPrev.Enabled = false;
                btnCompleteNEWCard.Enabled = (_allowNewCard && !KioskStatic.DisableNewCard);
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
        void showThankYou()
        {
            log.LogMethodEntry();
            using (frmThankYou frm = new frmThankYou(true))
            {
                frm.ShowDialog();
            }
            KioskStatic.logToFile("Exit Redeem Tokens screen");
            log.Info("Exit Redeem Tokens screen");
            Close();
            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnPrev.Enabled = false;
            Audio.Stop();
            refreshGridTotals();
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

            if (ac.totalValue > 0)
            {
                btnPrev.Enabled = false;
                btnCompleteNEWCard.Enabled = (_allowNewCard && !KioskStatic.DisableNewCard);
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
            log.LogMethodEntry();
            log.LogMethodExit();
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
                    CurrentCard = ftc.Card;
                }
                else // time out
                {
                    ftc.Dispose();
                    log.LogMethodExit(" ftc.Dispose();");
                    return;
                }

                ftc.Dispose();

                btnCompleteReload.Enabled = btnCompleteNEWCard.Enabled = false;
                Function = "R";
                ProductPrice = ac.productPrice = ac.totalValue;
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblGreeting1.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenHeaderTextForeColor;//Please insert CEC tokens and press complete
                this.label1.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenTokenInsertedTextForeColor;//Tokens Inserted
                this.txtAvlblTokens.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenAvialableTokensTextForeColor;//Available tokens 
                this.btnCompleteNEWCard.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenBtnNewCardTextForeColor;//Button new card
                this.btnCompleteReload.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenBtnLoadTextForeColor;//(Load points to existing card)- 
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenBtnBackTextForeColor;//
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenFooterTextForeColor;//RedeemTokensScreenFooterTextForeColor
                this.label2.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenDenominationTextForeColor;//Denomination
                this.label3.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenQuantityTextForeColor;//Quantity
                this.label4.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenPointsTextForeColor;//Points
                this.lblTimeOut.ForeColor = KioskStatic.CurrentTheme.RedeemTokensScreenTimeOutTextForeColor;//Time out
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

