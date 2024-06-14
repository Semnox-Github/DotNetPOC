/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmCashTransaction 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for MultiPoint, Timeout crash, Validty msg changes & tax display
*2.70        1-Jul-2019       Lakshminarayana    Modified to add support for ULC cards 
*2.90.0      30-Jun-2020      Dakshakh raj       Dynamic Payment Modes based on set up
*2.100.0     05-Aug-2020      Guru S A           Kiosk activity log changes
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 *********************************************************************************************/

using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.KioskCore.CoinAcceptor;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using Semnox.Parafait.Customer;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using System.Text.RegularExpressions;
using Semnox.Core.GenericUtilities;
using System.Collections.Generic;

namespace Parafait_Kiosk
{
    public partial class frmCashTransaction : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";
        string Function;
        DataRow rowProduct;
        Card CurrentCard;
        decimal ProductPrice;
        decimal AmountRequired;
        private decimal productPriceWithOutTax;
        private decimal productTaxAmount;

        CoinAcceptor coinAcceptor;
        BillAcceptor billAcceptor;
        Semnox.Parafait.KioskCore.CardDispenser.CardDispenser cardDispenser;

        KioskStatic.acceptance ac;
        KioskStatic.configuration config = KioskStatic.config;

        int CardCount = 1;
        PaymentModeDTO _PaymentModeDTO;

        int billAcceptorTimeout = 0;
        int coinAcceptorTimeout = 0;
        bool printReceipt = false;
        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;
        private readonly TagNumberParser tagNumberParser;
        bool MultipleCardsInSingleProduct = false;

        Font savTimeOutFont;
        Font timeoutFont;
        clsKioskTransaction kioskTransaction;
        DiscountsSummaryDTO discountSummaryDTO = null;
        string selectedEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
        string previousCardNumber = "";
        public frmCashTransaction(string pFunction, DataRow prowProduct, Card rechargeCard, CustomerDTO customerDTO, PaymentModeDTO PaymentModeDTO, int inCardCount, bool Isprint, string entitlementType, DiscountsSummaryDTO discountSummaryDTO = null, string couponNo = null) // I for issue, R for recharge
        {
            log.LogMethodEntry(pFunction, prowProduct, rechargeCard, customerDTO, PaymentModeDTO, inCardCount, Isprint, entitlementType, discountSummaryDTO, couponNo);
            Audio.Stop();
            this.discountSummaryDTO = discountSummaryDTO;
            selectedEntitlementType = entitlementType;
            kioskTransaction = new clsKioskTransaction(this, pFunction, prowProduct, rechargeCard, customerDTO, PaymentModeDTO, inCardCount, selectedEntitlementType, "", discountSummaryDTO, couponNo);
            //selectedEntitlementType = entitlementType;
            ProductPrice = kioskTransaction.ProductPrice;
            CardCount = kioskTransaction.CardCount;
            MultipleCardsInSingleProduct = kioskTransaction.MultipleCardsInSingleProduct;
            ProductPrice = kioskTransaction.ProductPrice;
            AmountRequired = kioskTransaction.AmountRequired;
            productTaxAmount = kioskTransaction.ProductTaxAmount;
            productPriceWithOutTax = kioskTransaction.ProductPriceWithOutTax;

            _PaymentModeDTO = PaymentModeDTO;
            Function = pFunction;
            rowProduct = prowProduct;
            printReceipt = Isprint;//Modification on 17-Dec-2015 for introducing new theme
            InitializeComponent();
            //TimerMoney.Enabled = exitTimer.Enabled = false;
            TimerMoney.Enabled = false;
            KioskTimerSwitch(false);
            Utilities.setLanguage(this);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);//Starts:Modification on 17-Dec-2015 for introducing new theme           
            KioskStatic.setDefaultFont(this);//Ends:Modification on 17-Dec-2015 for introducing new theme
            
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);

            displayMessageLine("", MESSAGE);
            lblTimeOut.Text = KioskStatic.MONEY_SCREEN_TIMEOUT.ToString("#0");

            savTimeOutFont = lblTimeOut.Font;
            timeoutFont = lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            if (rechargeCard != null)
                CurrentCard = rechargeCard;

            bool enableAcceptors = true;
            CommonBillAcceptor commonBA = (Application.OpenForms[0] as frmHome).commonBillAcceptor;
            if (commonBA != null && commonBA.GetAcceptance().totalValue > 0)
            {
                KioskStatic.ac = ac = commonBA.GetAcceptance();
                AmountRequired = ProductPrice = ac.totalValue;
                enableAcceptors = false;
            }

            this.ShowInTaskbar = true;

            lblPackage.Text = KioskStatic.Utilities.ParafaitEnv.CURRENCY_SYMBOL + ProductPrice.ToString("N0") +
                                Environment.NewLine + "(" +
                                prowProduct["product_name"].ToString() + ")";
            if (pFunction == "I")
                lblCardnumber.Text = MessageUtils.getMessage("NEW");
            else
                lblCardnumber.Text = rechargeCard.CardNumber;

            int credits = 0;
            if (prowProduct["product_type"].ToString().Equals("VARIABLECARD"))
                credits = (int)KioskStatic.GetCreditsOnSplitVariableProduct((double)ProductPrice, selectedEntitlementType, rechargeCard);
            else
                credits = Convert.ToInt32(prowProduct["Credits"]);
            log.LogVariableState("credits", credits);
            lblQuantity.Text = (CardCount <= 1 ? "1 " + MessageUtils.getMessage("Card") : CardCount.ToString() + " " + MessageUtils.getMessage("Cards")) + Environment.NewLine +
                                "(" + credits / (CardCount == 0 ? 1 : CardCount) + " " + MessageUtils.getMessage("points per card)");

            lblBal.Text = lblTotalToPay.Text = lblTotal.Text = AmountRequired.ToString("N2");
            lblPrice.Text = productPriceWithOutTax.ToString("N2");
            lblTax.Text = productTaxAmount.ToString("N2");
            lblPaid.Text = "0.00";

            if (_PaymentModeDTO != null && _PaymentModeDTO.IsCash && enableAcceptors)
            {

                if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB))
                {
                    billAcceptor = KioskStatic.getBillAcceptor(KioskStatic.config.baport.ToString());
                    ((NV9USB)billAcceptor).dataReceivedEvent = billAcceptorDataReceived;
                    billAcceptor.initialize();
                }
                else if (config.baport != 0)
                {
                    billAcceptor = KioskStatic.getBillAcceptor(KioskStatic.config.baport.ToString()); 
                    KioskStatic.baReceiveAction = billAcceptorDataReceived;
                    if (billAcceptor.spBillAcceptor != null)
                    {
                        billAcceptor.spBillAcceptor.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(spBillAcceptor_DataReceived);
                    }
                    billAcceptor.initialize();
                    billAcceptor.requestStatus();
                }

                if (billAcceptor != null)
                    ac = KioskStatic.ac = billAcceptor.acceptance;

                if (config.coinAcceptorport != 0)
                {
                    coinAcceptor = KioskStatic.getCoinAcceptor();
                    coinAcceptor.spCoinAcceptor = KioskStatic.spCoinAcceptor;
                    if (coinAcceptor.set_acceptance())
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
                    coinAcceptor.acceptance = ac;

                lblNoChange.Text = MessageUtils.getMessage(507);
            }

            if (ac == null)
                ac = KioskStatic.ac = new KioskStatic.acceptance();

            ac.productPrice = AmountRequired;
            DisplayValidtyMsg(Convert.ToInt32(rowProduct["product_id"]));

            for (int i = 0; i < this.Controls.Count; i++)
            {
                this.Controls[i].Visible = false;
            }
            log.LogMethodExit();
        }

        private void frmNewcard_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Application.DoEvents();
            if (ac.totalValue > 0)
            {
                e.Cancel = true;
                KioskStatic.logToFile("FormClosing: Inserted value > 0. Cannot exit.");
                restartValidators();
                log.LogMethodExit("FormClosing: Inserted value > 0. Cannot exit.");
                return;
            }
            if (billAcceptor != null && billAcceptor.StillProcessing())
            {
                totSecs = 0;
                e.Cancel = true;
                KioskStatic.logToFile("Timer Money: billAcceptor stillProcessing. reset timer clock");
                log.Info("billAcceptor stillProcessing. reset timer cock");
                log.LogMethodExit();
                return;
            }

            this.Hide();
            TimerMoney.Stop();

            if (billAcceptor != null)
                billAcceptor.disableBillAcceptor();
            if (coinAcceptor != null)
                coinAcceptor.disableCoinAcceptor();

            KioskStatic.baReceiveAction = null;
            KioskStatic.caReceiveAction = null;
            KioskStatic.billAcceptorDatareceived = false;
            KioskStatic.coinAcceptorDatareceived = false;

            if (ac.totalValue <= 0)
            {
                displayMessageLine(MessageUtils.getMessage("Thank You"), MESSAGE);
            }
            else
            {
                Application.DoEvents();
                e.Cancel = true;
                KioskStatic.logToFile("FormClosing: Inserted value > 0. Cannot exit.");
                restartValidators();
                log.LogMethodExit("FormClosing: Inserted value > 0. Cannot exit.");
                return;
            }

            if (Function != "I" && KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
            {
                KioskStatic.cardAcceptor.EjectCardFront();
                KioskStatic.cardAcceptor.BlockAllCards();
            }
             

            if (Function == "I" && KioskStatic.DispenserReaderDevice != null)
            {
                KioskStatic.DispenserReaderDevice.UnRegister();
            }

            KioskStatic.logToFile("Exiting money screen...");
            log.Info("Exiting money screen...");
            Audio.Stop();
            if (coinAcceptor != null)
                coinAcceptor.CloseComm();

            log.LogMethodExit();
        }

        void initializeForm()
        {
            log.LogMethodEntry();
            try
            {//Starts:Modification on 17-Dec-2015 for introducing new theme
                this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnHome.Size = btnHome.BackgroundImage.Size;
                pbDownArrow.Image = ThemeManager.CurrentThemeImages.InsertCashAnimation;
                lblTimeOut.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
                btnCreditCard.BackgroundImage = btnDebitCard.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex){ log.Error(ex); } 

            //if (KioskStatic.ccPaymentModeDetails != null)
            //{
            //    //      btnCreditCard.Text = KioskStatic.ccPaymentModeDetails.PaymentMode;
            //}
            log.LogMethodExit();
        }

        void refreshTotals()
        {
            log.LogMethodEntry();
            lblPaid.Text = ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            if ((AmountRequired - ac.totalValue) > 0)
            {
                lblBal.Text = (AmountRequired - ac.totalValue).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            }
            else
            {
                lblBal.Text = (0).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            }
            Application.DoEvents();
            log.LogMethodExit();
        }

        private void frmNewcard_Load(System.Object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            initializeForm();

            for (int i = 0; i < this.Controls.Count; i++)//12-06-2015:Starts
            {
                this.Controls[i].Visible = true;
            }//12-06-2015:Ends

            lblSiteName.Visible = panelAmountPaid.Visible = btnDebitCard.Visible = btnCreditCard.Visible = panelSummary.Visible = false;//NewTheme:Changed to add new theme 

            lblSiteName.Text = KioskStatic.SiteHeading; 
            Application.DoEvents();

            KioskStatic.logToFile("Enter Money screen");
            log.Info("Enter Money screen");
            KioskStatic.logToFile(rowProduct["product_name"].ToString());
            log.Info(rowProduct["product_name"].ToString());
            KioskStatic.logToFile("Amount required: " + AmountRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
            log.Info("Amount required: " + AmountRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));

            //receipt         
            kioskTransaction.SetupKioskTransaction(printReceipt, coinAcceptor, billAcceptor, ac, showThankYou, showOK);

            Audio.PlayAudio(Audio.InsertExactAmount);

            TimerMoney.Start();

            if (_PaymentModeDTO!= null && _PaymentModeDTO.IsCreditCard)
            {
                btnCreditCard_Click(btnCreditCard, null);
            }
            else if (_PaymentModeDTO != null && _PaymentModeDTO.IsDebitCard)
            {
                btnCreditCard_Click(btnDebitCard, null);
            }
            else
            { checkMoneyStatus(); }
            previousCardNumber = "";
            log.LogMethodExit();
        }  
        
        private void spBillAcceptor_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            log.LogMethodEntry();
            if (billAcceptor.spBillAcceptor != null)
            {
                System.Threading.Thread.Sleep(20);
                billAcceptor.spBillAcceptor.Read(KioskStatic.billAcceptorRec, 0, billAcceptor.spBillAcceptor.BytesToRead);
                KioskStatic.billAcceptorDatareceived = true;

                if (KioskStatic.baReceiveAction != null)
                    KioskStatic.baReceiveAction.Invoke();
            }
            log.LogMethodExit();
        }

        void billAcceptorDataReceived()
        {
            log.LogMethodEntry();
            billMessage = "";
            KioskStatic.billAcceptorDatareceived = true;
            bool noteRecd = billAcceptor.ProcessReceivedData(KioskStatic.billAcceptorRec, ref billMessage);
            if (noteRecd)
            {
                totSecs = 0;
                KioskStatic.logToFile("Bill Acceptor note received: " + billMessage);
                log.Info("Bill Acceptor note received: " + billMessage);
                log.LogVariableState("Ac", ac);
                KioskStatic.updateKioskActivityLog(billAcceptor.ReceivedNoteDenomination, -1, (CurrentCard == null ? "" : CurrentCard.CardNumber), "BILL-IN", "Bill Inserted", ac);
                billAcceptor.ReceivedNoteDenomination = 0;

                System.Threading.Thread.Sleep(300);
                billAcceptor.requestStatus();

                this.BeginInvoke((MethodInvoker)delegate
                {
                    refreshTotals();
                    checkMoneyStatus();
                });
            }
            log.LogMethodExit();
        }

        void coinAcceptorDataReceived()
        {
            log.LogMethodEntry();
            coinMessage = "";
            KioskStatic.coinAcceptorDatareceived = true;
            bool coinRecd = coinAcceptor.ProcessReceivedData(KioskStatic.coinAcceptor_rec, ref coinMessage);
            KioskStatic.logToFile("Coin Acceptor: " + coinMessage);
            log.Info("Coin Acceptor: " + coinMessage);
            if (coinRecd)
            {
                totSecs = 0;
                log.LogVariableState("Ac", ac);
                KioskStatic.updateKioskActivityLog(-1, coinAcceptor.ReceivedCoinDenomination, (CurrentCard == null ? "" : CurrentCard.CardNumber), "COIN-IN", "Coin Inserted", ac);
                coinAcceptor.ReceivedCoinDenomination = 0;
                System.Threading.Thread.Sleep(20);

                this.BeginInvoke((MethodInvoker)delegate
                {
                    refreshTotals();
                    checkMoneyStatus();
                });
            }
            log.LogMethodExit();
        }

        int toggleAcceptorMessage = 0;
        string billMessage = "";
        string coinMessage = "";
        double totSecs = 0;
        private void TimerMoney_Tick(System.Object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            TimerMoney.Stop();

            try
            {
                if (!KioskStatic.billAcceptorDatareceived && billAcceptor != null)
                {
                    billAcceptorTimeout++;
                    // if (billAcceptorTimeout > 30 && KioskStatic.spBillAcceptor.IsOpen)
                    if (billAcceptorTimeout > 30)
                    {
                        billMessage = MessageUtils.getMessage(423);
                        KioskStatic.logToFile("Bill acceptor timeout / offline");
                        log.Error("Bill acceptor timeout / offline");
                    }

                    if (billAcceptorTimeout % 5 == 0)
                    {                      
                        billAcceptor.requestStatus(); 
                    }
                }

                if (!KioskStatic.coinAcceptorDatareceived && coinAcceptor != null)
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
                        if (KioskStatic.spCoinAcceptor.IsOpen)
                        {
                            coinAcceptor.checkStatus();
                        }
                    }
                }

                if (ac.totalValue <= 0)
                {
                    if ((billAcceptor == null || billAcceptor.ReceivedNoteDenomination == 0)
                        && (coinAcceptor == null || coinAcceptor.ReceivedCoinDenomination == 0))
                    {
                        totSecs += TimerMoney.Interval / 1000.0;
                        if (totSecs > KioskStatic.MONEY_SCREEN_TIMEOUT)
                        {
                            if (billAcceptor != null && billAcceptor.StillProcessing())
                            {
                                totSecs = 0;
                                KioskStatic.logToFile("Timer Money: billAcceptor stillProcessing. reset timer clock");
                                log.Info("billAcceptor stillProcessing. reset timer cock");
                                log.LogMethodExit();
                                return;
                            }
                            if (billAcceptor != null)
                                billAcceptor.disableBillAcceptor();
                            if (coinAcceptor != null)
                                coinAcceptor.disableCoinAcceptor();

                            btnDebitCard.Enabled =
                            btnCreditCard.Enabled = false;
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
                                totSecs = KioskStatic.MONEY_SCREEN_TIMEOUT - 3;
                        }

                        if (totSecs > KioskStatic.MONEY_SCREEN_TIMEOUT)
                        {
                            if (billAcceptor != null && billAcceptor.StillProcessing())
                            {
                                totSecs = 0;
                                KioskStatic.logToFile("billAcceptor stillProcessing. reset timer clock");
                                log.Info("billAcceptor stillProcessing. reset timer cock");
                                log.LogMethodExit();
                                return;
                            }
                            displayMessageLine(MessageUtils.getMessage(425), ERROR);
                            lblTimeOut.Font = savTimeOutFont;
                            lblTimeOut.Text = "Time Out";
                            btnHome.Enabled = false; 
                            KioskTimerSwitch(true);
                            StartKioskTimer();
                            KioskStatic.logToFile("Operation Timed out...");
                            log.LogMethodExit("Operation Timed out...");
                            return;
                        }
                        else
                        {
                            if (secondsRemaining > 0)
                            {
                                lblTimeOut.Font = timeoutFont;
                                lblTimeOut.Text = secondsRemaining.ToString("#0");
                            }
                        }
                    }
                    else
                    {
                        restartValidators();
                        log.LogMethodExit();
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
                        if (DateTime.Now.Millisecond / 500 > 0)
                            lblTimeOut.Text = "";
                        else
                            lblTimeOut.Text = MessageUtils.getMessage(427);
                    }
                }

                if (billMessage.EndsWith("inserted") || billMessage.EndsWith("accepted") || billMessage.EndsWith("rejected"))
                {
                    log.LogVariableState("billMessage", billMessage);
                    displayMessageLine(billMessage.Replace("inserted", MessageUtils.getMessage("inserted"))
                                                  .Replace("accepted", MessageUtils.getMessage("accepted"))
                                                  .Replace("rejected", MessageUtils.getMessage("rejected"))
                                                  .Replace("Bill", MessageUtils.getMessage("Bill"))
                                                  .Replace("Denomination", MessageUtils.getMessage("Denomination")), MESSAGE);
                    Audio.Stop();
                }
                else if (coinMessage.EndsWith("accepted") || coinMessage.EndsWith("rejected"))
                {
                    log.LogVariableState("coinMessage", coinMessage);
                    displayMessageLine(coinMessage.Replace("accepted", MessageUtils.getMessage("accepted"))
                                                  .Replace("rejected", MessageUtils.getMessage("rejected"))
                                                  .Replace("Denomination", MessageUtils.getMessage("Denomination")), MESSAGE);
                    Audio.Stop();
                }
                else if (billMessage.StartsWith("Insert") && coinMessage.StartsWith("Insert"))
                {
                    displayMessageLine(MessageUtils.getMessage(428), MESSAGE); //"Insert Bank Notes and Coins"
                }
                else
                {
                    if (toggleAcceptorMessage > 0)
                    {
                        if (billAcceptor != null)
                        {
                            if (billMessage != "")
                            {
                                if (billAcceptor.criticalError)
                                {
                                    using (frmOKMsg f = new frmOKMsg(MessageUtils.getMessage(billMessage) + ". " + Utilities.MessageUtils.getMessage(441)))
                                    { f.ShowDialog(); }
                                }
                                else if (billAcceptor.overpayReject)
                                {
                                    billAcceptor.overpayReject = false;
                                    using (frmOKMsg f = new frmOKMsg("Bill refused. Please insert exact amount"))
                                    { f.ShowDialog(); }
                                }
                                else
                                    displayMessageLine(MessageUtils.getMessage(billMessage), MESSAGE);
                            }
                            else
                            {
                                displayMessageLine(MessageUtils.getMessage(419, AmountRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), MESSAGE);
                            }
                        }
                    }
                    else
                    {
                        if (coinAcceptor != null)
                        {
                            if (coinMessage != "")
                            {
                                if (coinAcceptor.criticalError)
                                {
                                    using (frmOKMsg f = new frmOKMsg(MessageUtils.getMessage(coinMessage) + ". " + Utilities.MessageUtils.getMessage(441)))
                                    { f.ShowDialog(); }
                                }
                                else
                                    displayMessageLine(MessageUtils.getMessage(coinMessage), MESSAGE);
                            }
                            else
                            {
                                displayMessageLine(MessageUtils.getMessage(419, AmountRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), MESSAGE);
                            }
                        }
                    }

                    toggleAcceptorMessage++;
                    if (toggleAcceptorMessage > 40)
                        toggleAcceptorMessage = -40;
                }

                if (ac.totalValue < AmountRequired)
                    TimerMoney.Start();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                TimerMoney.Start();
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile(ex.Message + "-" + ex.StackTrace);
                btnHome.Enabled = true;
            }
            log.LogMethodExit();
        }

        private Object thisLock = new Object();
        void checkMoneyStatus()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("checkMoneyStatus() - enter");
            TimeOut.Abort();
            lock (thisLock)
            {
                try
                {
                    if (ac.totalValue >= AmountRequired)
                    {
                        TimerMoney.Stop();

                        if (kioskTransaction.cancelPressed)
                        {
                            log.LogMethodExit();
                            return;
                        }

                        btnHome.Enabled = false;
                        Application.DoEvents();

                        KioskStatic.logToFile("Valid amount inserted: " + ac.totalValue.ToString("N2") + "; " + "Required: " + ProductPrice.ToString("N2"));
                        log.Info("Valid amount inserted: " + ac.totalValue.ToString("N2") + "; " + "Required: " + ProductPrice.ToString("N2"));
                        if (billAcceptor != null)
                        {
                            System.Threading.Thread.Sleep(300);
                            billAcceptor.disableBillAcceptor();
                        }
                        if (coinAcceptor != null)
                        {
                            System.Threading.Thread.Sleep(300);
                            coinAcceptor.disableCoinAcceptor();
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

                            cardDispenser = KioskStatic.getCardDispenser(KioskStatic.config.dispport.ToString());
                            KioskStatic.logToFile("Dispenser Type: " + cardDispenser.GetType().ToString());
                            log.Info("Dispenser Type: " + cardDispenser.GetType().ToString());
                            displayMessageLine(MessageUtils.getMessage(429, ""), MESSAGE);
                            Application.DoEvents();
                            kioskTransaction.dispenseCards(cardDispenser);
                        }
                        else
                        {
                            displayMessageLine(MessageUtils.getMessage(503), MESSAGE);
                            Application.DoEvents();
                            kioskTransaction.rechargeCard();
                        }

                        AmountRequired = 0;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    displayMessageLine(ex.Message, ERROR);
                    KioskStatic.logToFile(ex.Message + "-" + ex.StackTrace);
                    btnHome.Enabled = true;
                }
                finally
                {
                    KioskStatic.logToFile("checkMoneyStatus() - exit");
                }
            }
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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
                log.LogVariableState("CardNumber", CardNumber);
                try
                {
                    handleCardRead(CardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    displayMessageLine(ex.Message, ERROR);
                }
            }
            log.LogMethodExit();
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            if (cardDispenser != null)
            {
                KioskStatic.logToFile("handleCardRead- inCardNumber : " + inCardNumber);
                KioskStatic.logToFile("handleCardRead- previousCardNumber : " + previousCardNumber);
                log.Info("inCardNumber : " + inCardNumber);
                log.Info("previousCardNumber : " + previousCardNumber);
                if (string.IsNullOrEmpty(previousCardNumber) || (!string.IsNullOrEmpty(previousCardNumber) && previousCardNumber != inCardNumber))
                {
                    previousCardNumber = inCardNumber;
                    KioskStatic.logToFile("Calling cardDispenser.HandleDispenserCardRead");
                    log.Info("Calling cardDispenser.HandleDispenserCardRead");
                    cardDispenser.HandleDispenserCardRead(inCardNumber);
                }
            }
            log.LogMethodExit();
        }

        void restartValidators()
        {
            log.LogMethodEntry();
            //if (_PaymentMode != "CREDITCARD")
            if (_PaymentModeDTO != null && _PaymentModeDTO.IsCreditCard != true)
            {
                if (billAcceptor != null)
                {
                    if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB))
                    {
                        billAcceptor.disableBillAcceptor();
                        billAcceptor.initialize();
                    } 
                    else
                    {
                        billAcceptor.requestStatus();
                    }
                }

                if (coinAcceptor != null)
                    coinAcceptor.set_acceptance();
            }

            btnDebitCard.Enabled = btnCreditCard.Enabled = true;
            btnHome.Enabled = true;

            TimerMoney.Start();
            log.LogMethodExit();
        }

        int cancelPressCount = 0;
        public override void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            kioskTransaction.cancelPressed = true;
            btnHome.Enabled = false;

            Audio.Stop();
            int iters = 100;
            while (iters-- > 0)
            {
                Thread.Sleep(20);
                Application.DoEvents();
            }

            KioskStatic.logToFile("Cancel pressed");
            log.Info("Cancel pressed");

            if ((billAcceptor != null && billAcceptor.ReceivedNoteDenomination > 0) // in the process of accepting
                || (coinAcceptor != null && coinAcceptor.ReceivedCoinDenomination > 0))
            {
                kioskTransaction.cancelPressed = false;
                btnHome.Enabled = true;
                if (cancelPressCount++ < 3)
                {
                    log.LogMethodExit();
                    return;
                }
            }

            if (billAcceptor != null && billAcceptor.StillProcessing())
            {
                kioskTransaction.cancelPressed = false;
                btnHome.Enabled = true;
                log.Info("billAcceptor.StillProcessing");
                KioskStatic.logToFile("btnHome_Click: billAcceptor.StillProcessing");
                log.LogMethodExit();
                return;
            }

            if (billAcceptor != null)
                billAcceptor.disableBillAcceptor();
            if (coinAcceptor != null)
                coinAcceptor.disableCoinAcceptor();

            btnDebitCard.Enabled = btnCreditCard.Enabled = false;

            TimerMoney.Stop();
            Application.DoEvents();

            if (ac.totalValue > 0)
            {
                string message;
                if (Function == "I")
                {
                    bool abort = false;
                    message = MessageUtils.getMessage(442, ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                    frmYesNo f = new frmYesNo(message);
                    if (f.ShowDialog() == System.Windows.Forms.DialogResult.No)
                    {
                        if (Utilities.getParafaitDefaults("ALLOW_VARIABLE_NEW_CARD_ISSUE").Equals("Y"))
                        {
                            message = MessageUtils.getMessage(934, ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                            f = new frmYesNo(message);
                            DialogResult dr = f.ShowDialog();
                            if (dr == System.Windows.Forms.DialogResult.Cancel) // time out
                            {
                                kioskTransaction.cancelPressed = false;
                                restartValidators();
                            }
                            else if (dr == System.Windows.Forms.DialogResult.No)
                            {
                                abort = true;
                            }
                            else
                            {
                                kioskTransaction.cancelPressed = false;

                                if (MultipleCardsInSingleProduct)
                                {
                                    object o = Utilities.executeScalar(@"select top 1 isnull(CardCount, 1)
                                                                    from products p, product_type pt 
                                                                    where p.product_type_id = pt.product_type_id 
                                                                    and pt.product_type in ('NEW', 'CARDSALE')
                                                                    and price <= @amount 
                                                                    order by price desc",
                                                                        new SqlParameter("@amount", ac.totalValue));

                                    int savCardCount = CardCount;
                                    if (o == null)
                                        CardCount = 1;
                                    else
                                        CardCount = Math.Min(CardCount, Convert.ToInt32(o));

                                    kioskTransaction.CardCount = CardCount; //added KioskTransaction.CardCount

                                    if (CardCount != savCardCount)
                                        (new frmOKMsg(MessageUtils.getMessage(935, CardCount, savCardCount))).ShowDialog();
                                }
                                else
                                {
                                    if (Math.Ceiling(ac.totalValue / CardCount) < ProductPrice)
                                    {
                                        kioskTransaction.CardCount = CardCount = 1;
                                    }
                                }

                                rowProduct = KioskStatic.getProductDetails(KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId).Rows[0];
                                kioskTransaction.rowProduct = rowProduct; //Assign new row product to KIoskTransaction
                                if (rowProduct != null)
                                {
                                    DisplayValidtyMsg(Convert.ToInt32(rowProduct["product_id"]));
                                }

                                if (MultipleCardsInSingleProduct)
                                {   rowProduct["Price"] = kioskTransaction.ProductPrice = AmountRequired = ac.productPrice = ProductPrice = ac.totalValue; //added KioskTransaction.ProductPrice
                                }
                                else
                                {
                                    ProductPrice = ac.totalValue / CardCount; 
                                    rowProduct["Price"] = kioskTransaction.ProductPrice = ProductPrice; //added KioskTransaction.ProductPrice
                                    AmountRequired = ac.productPrice = ac.totalValue;
                                }

                                checkMoneyStatus();
                            }
                        }
                        else
                        {
                            abort = true;
                        }
                    }
                    else
                    {
                        kioskTransaction.cancelPressed = false;
                        restartValidators();
                    }

                    if (abort)
                    {
                        decimal ccTotalValue = kioskTransaction.GetCreditCardPaymentAmount();
                        if (ccTotalValue > 0)
                        {
                            ac.totalCCValue = ccTotalValue;
                        }
                        kioskTransaction.cancelCCPayment();
                        decimal gameCardTotalValue = kioskTransaction.GetGameCardPaymentAmount();
                        if (gameCardTotalValue > 0)
                        {
                            ac.totalGameCardValue = gameCardTotalValue;
                        }
                        KioskStatic.updateKioskActivityLog(-1, -1, "", KioskStatic.ACTIVITY_TYPE_ABORT, "Abort New Card Issue", ac);
                        KioskStatic.logToFile("Abort new card issue... Money entered: " + ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                        log.Info("Abort new card issue... Money entered: " + ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                        if (KioskStatic.receipt)
                            kioskTransaction.print_receipt(true);

                        using (frmOKMsg frmok = new frmOKMsg(MessageUtils.getMessage(441)))
                        { frmok.ShowDialog(); }

                        ac.totalValue = 0;
                        this.Close();
                    }
                }
                else
                {
                    message = MessageUtils.getMessage(443, ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                    using (frmYesNo f = new frmYesNo(message))
                    {
                        if (f.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                        {
                            kioskTransaction.rechargeCard();
                        }
                        else
                        {
                            message = "Are you sure you want to exit?";
                            using (frmYesNo fop = new frmYesNo(message))
                            {
                                if (fop.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                                {
                                    decimal ccTotalValue = kioskTransaction.GetCreditCardPaymentAmount();
                                    if (ccTotalValue > 0)
                                    {
                                        ac.totalCCValue = ccTotalValue;
                                    }
                                    kioskTransaction.cancelCCPayment();
                                    decimal gameCardTotalValue = kioskTransaction.GetGameCardPaymentAmount();
                                    if (gameCardTotalValue > 0)
                                    {
                                        ac.totalGameCardValue = gameCardTotalValue;
                                    }
                                    KioskStatic.updateKioskActivityLog(-1, -1, "", KioskStatic.ACTIVITY_TYPE_ABORT, "Abort Recharge", ac);
                                    KioskStatic.logToFile("Abort recharge... Money entered: " + ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    log.Info("Abort recharge... Money entered: " + ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    if (KioskStatic.receipt)
                                        kioskTransaction.print_receipt(true);

                                    using (frmOKMsg frmok = new frmOKMsg(MessageUtils.getMessage(441)))
                                    { frmok.ShowDialog(); }

                                    ac.totalValue = 0;
                                    this.Close();
                                }
                                else
                                {
                                    kioskTransaction.cancelPressed = false;
                                    restartValidators();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                this.Close();
            }
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
                restartValidators();
                return;
            }
            TimerMoney.Stop();

            KioskStatic.logToFile("Exit Timer ticked");
            log.Info("Exit Timer ticked");
            Close();
            log.LogMethodExit();
        }

        void showThankYou(bool receiptPrinted)
        {
            log.LogMethodEntry(receiptPrinted);
            using (frmThankYou frm = new frmThankYou(receiptPrinted))
            {
                frm.ShowDialog();
            }
            KioskStatic.logToFile("Exit money screen");
            Close();
            log.LogMethodExit("Exit money screen");
        }

        void showOK(string message, bool enableTimeOut = true)
        {
            log.LogMethodEntry(message, enableTimeOut);
            using (frmOKMsg frm = new frmOKMsg(message, enableTimeOut))
            {
                frm.ShowDialog();
            }
            log.LogMethodExit();
        }

        private void btnCreditCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (AmountRequired - ac.totalValue <= 0)
            {
                displayMessageLine("Nothing to pay for", WARNING);
                log.LogMethodExit("Nothing to pay for");
                return;
            }

            KioskStatic.logToFile("Credit card clicked");
            log.Info("Credit card clicked");
            try
            {
                TimerMoney.Stop();
                Audio.Stop();
                btnDebitCard.Enabled =
                btnCreditCard.Enabled = false;
                btnHome.Enabled = false;
                displayMessageLine(Utilities.MessageUtils.getMessage("Credit Card Payment.") + " " + Utilities.MessageUtils.getMessage("Please wait..."), WARNING);
                Application.DoEvents();
                //To be tested and analyzed
                //if (KioskStatic.ccPaymentModeDetails.Gateway.Equals(PaymentGateways.NCR.ToString()))//starts: Modification on 2016-06-13 For adding NCR gateway
                //{
                //    PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(KioskStatic.ccPaymentModeDetails.Gateway);
                //    paymentGateway.BeginOrder();
                //}

                kioskTransaction.CreditCardPayment((sender as Control).Equals(btnDebitCard) ? true : false);
                totSecs = 0;
                TimerMoney.Start();
                checkMoneyStatus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                btnDebitCard.Visible = btnCreditCard.Visible = true;
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile("btnCreditCard_Click method" + ex.Message);
                KioskStatic.logToFile("btnCreditCard_Click method" + ex.StackTrace);
                MessageBox.Show(ex.StackTrace);
            }
            finally
            {
                btnHome.Enabled = true;
                if (Utilities.getParafaitDefaults("SHOW_DEBIT_CARD_BUTTON").Equals("N"))
                {
                    btnDebitCard.Visible = false;
                }
                else
                    btnDebitCard.Enabled = true;
                btnCreditCard.Enabled = true;
            }
            log.LogMethodExit();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            log.LogMethodEntry(msg, keyData);
            log.LogMethodExit(true);
            return true;
        }
        private void DisplayValidtyMsg(int productId)
        {
            log.LogMethodEntry(productId);
            string validityMsg = "";
            validityMsg = KioskStatic.GetProductCreditPlusValidityMessage(productId, selectedEntitlementType);
            int lineCount = Regex.Split(validityMsg, "\r\n").Count();
            if (lineCount > 2)
            {
                string[] validityMsgList = Regex.Split(validityMsg, "\r\n");
                lblProductCPValidityMsg.Text = validityMsgList[0] + "\r\n" + validityMsgList[1];
                System.Windows.Forms.ToolTip validtyMsgToolTip = new ToolTip();
                validtyMsgToolTip.SetToolTip(lblProductCPValidityMsg, validityMsg);
            }
            else
            {
                lblProductCPValidityMsg.Text = validityMsg;
            }

            log.LogMethodExit();
        }
    }
}
