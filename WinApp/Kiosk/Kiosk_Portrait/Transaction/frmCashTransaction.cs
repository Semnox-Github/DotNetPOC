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
*2.80        09-Sep-2019      Deeksha            Added logger methods.
*2.90.0      30-Jun-2020      Dakshakh raj       Dynamic Payment Modes based on set up
*2.100.0     05-Aug-2020      Guru S A           Kiosk activity log changes
*2.100.0     10-Feb-2021      Deeksha            Isseu Fix : Cash payment fails if we insert amout when timer is about to end.
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.140.0     18-Oct-2021      Sathyavathi        Check-In Check-Out feature in Kiosk
*2.150.0.0   21-Jun-2022      Vignesh Bhat       Back and Cancel button changes
*2.130.9     12-Jun-2022      Sathyavathi        Removed hard coded amount/number format
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
*2.155.0     20-Jun-2023      Sathyavathi        Attraction Sale in Kiosk
********************************************************************************************/

using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
//using ParafaitAlohaIntegration;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using Semnox.Parafait.KioskCore.CardDispenser;
using Semnox.Parafait.KioskCore.CoinAcceptor;
using Semnox.Parafait.Transaction;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Semnox.Parafait.POS;
using System.Collections.Generic;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

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
        private decimal productTaxAmount;
        private decimal productPriceWithOutTax;

        CoinAcceptor coinAcceptor;
        BillAcceptor billAcceptor;
        CardDispenser cardDispenser;

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
        private System.Windows.Forms.Timer noteCoinActionTimer;
        private bool canPerformNoteCoinReceivedAction = false;

        bool MultipleCardsInSingleProduct = false;

        Font savTimeOutFont;
        Font timeoutFont;
        clsKioskTransaction kioskTransaction;
        DiscountsSummaryDTO discountSummaryDTO = null;
        string selectedEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
        string previousCardNumber = "";
        private List<KeyValuePair<string, ProductsDTO>> selectedFundsAndDonationsList = null;
        private string amountFormat;
        private KioskTransaction kioskTransactionObj;
        public KioskTransaction GetKioskTransaction { get { return kioskTransactionObj; } }

        public frmCashTransaction(KioskTransaction kioskTransactionObj, string pFunction, DataRow prowProduct, Card rechargeCard, CustomerDTO customerDTO,
                                    PaymentModeDTO paymentModeDTO, int inCardCount, bool Isprint, string entitlementType,
                                    DiscountsSummaryDTO discountSummaryDTO = null, string couponNo = null,
                                    Semnox.Parafait.Transaction.Transaction Trx = null,
                                    List<KeyValuePair<string, ProductsDTO>> fundDonationProductsDTOList = null) // I for issue, R for recharge, C for Check-In
        {
            log.LogMethodEntry("kioskTransaction", pFunction, prowProduct, rechargeCard, customerDTO, paymentModeDTO, inCardCount, Isprint, entitlementType, discountSummaryDTO, couponNo, Trx, fundDonationProductsDTOList);
            Audio.Stop();
            this.discountSummaryDTO = discountSummaryDTO;
            selectedEntitlementType = entitlementType;
            this.kioskTransactionObj = kioskTransactionObj;
            amountFormat = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "AMOUNT_FORMAT");
            kioskTransaction = new clsKioskTransaction(this, pFunction, prowProduct, rechargeCard, customerDTO, paymentModeDTO, inCardCount, selectedEntitlementType, "", discountSummaryDTO, couponNo, Trx, fundDonationProductsDTOList);

            ProductPrice = kioskTransaction.ProductPrice;
            CardCount = kioskTransaction.CardCount;
            MultipleCardsInSingleProduct = kioskTransaction.MultipleCardsInSingleProduct;
            ProductPrice = kioskTransaction.ProductPrice;
            //AmountRequired = (Trx == null) ? kioskTransaction.AmountRequired : (decimal)Trx.Net_Transaction_Amount;

            if (pFunction != "C")
                AmountRequired = kioskTransaction.AmountRequired; //this is because, trx line is not created for the actual product yet in case of new card/top up
            else
                AmountRequired = (Trx == null) ? kioskTransaction.AmountRequired : (decimal)Trx.Net_Transaction_Amount;

            productTaxAmount = kioskTransaction.ProductTaxAmount;
            productPriceWithOutTax = kioskTransaction.ProductPriceWithOutTax;

            _PaymentModeDTO = paymentModeDTO;
            Function = pFunction;
            rowProduct = prowProduct;
            selectedFundsAndDonationsList = fundDonationProductsDTOList;
            printReceipt = Isprint;//Modification on 17-Dec-2015 for introducing new theme
            InitializeComponent();
            //TimerMoney.Enabled = exitTimer.Enabled = false;
            TimerMoney.Enabled = false;
            KioskTimerSwitch(false);
            Utilities.setLanguage(this);
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            if (KioskStatic.CurrentTheme.ThemeId != 0)
                KioskStatic.setDefaultFont(this);

            displayMessageLine("", MESSAGE);
            lblTimeOut.Text = KioskStatic.MONEY_SCREEN_TIMEOUT.ToString("#0");

            savTimeOutFont = lblTimeOut.Font;
            timeoutFont = lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            if (rechargeCard != null)
            {
                CurrentCard = rechargeCard;
                log.LogVariableState("CurrentCard", CurrentCard);
            }
            //bool enableAcceptors = true;
            //CommonBillAcceptor commonBA = (Application.OpenForms[0] as FSKCoverPage).commonBillAcceptor;
            //if (commonBA != null && commonBA.GetAcceptance().totalValue > 0)
            //{
            //    KioskStatic.ac = ac = commonBA.GetAcceptance();
            //    if (prowProduct["product_type"].ToString().Equals("VARIABLECARD"))
            //    {
            //        AmountRequired = ProductPrice = ac.totalValue;
            //    }
            //    enableAcceptors = false;
            //}

            //this.ShowInTaskbar = true;

            string amountFormatWithCurrencySymbol = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");
            lblPackage.Text = ProductPrice.ToString(amountFormatWithCurrencySymbol) +
                                Environment.NewLine + "(" +
                                prowProduct["product_name"].ToString() + ")";
            if (pFunction == "I")
                lblCardnumber.Text = MessageUtils.getMessage("NEW");
            else
                lblCardnumber.Text = rechargeCard.CardNumber;
            double credits = 0;
            //int credits = 0;
            if (prowProduct["product_type"].ToString().Equals("VARIABLECARD"))
                credits = KioskStatic.GetCreditsOnSplitVariableProduct((double)ProductPrice, selectedEntitlementType, rechargeCard);
            else
                credits = Convert.ToInt32(prowProduct["Credits"]);
            log.LogVariableState("credits", credits);
            lblQuantity.Text = (CardCount <= 1 ? "1 " + MessageUtils.getMessage("Card") : CardCount.ToString() + " " + MessageUtils.getMessage("Cards"));// + Environment.NewLine +
                                                                                                                                                         //"(" + credits / (CardCount == 0 ? 1 : CardCount) + " " + MessageUtils.getMessage("points per card)");

            lblBal.Text = lblTotalToPay.Text = lblTotal.Text = AmountRequired.ToString(ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL"));
            lblPrice.Text = productPriceWithOutTax.ToString(amountFormat);
            lblTax.Text = productTaxAmount.ToString(amountFormat);
            lblPaid.Text = "0.00";

            DisplayValidtyMsg(Convert.ToInt32(rowProduct["product_id"]));

            for (int i = 0; i < this.Controls.Count; i++)
            {
                this.Controls[i].Visible = false;
            }//12-06-2015:Ends

            SetCustomizedFontColors();
            RefreshTotals();
            log.LogMethodExit();
        }

        private void frmNewcard_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            Application.DoEvents();
            if (ac.totalValue > 0)
            {
                e.Cancel = true;
                KioskStatic.logToFile("FormClosing: Inserted value > 0. Cannot exit. restartValidators");
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
            if (noteCoinActionTimer != null)
            {
                noteCoinActionTimer.Stop();
            }


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
                KioskStatic.logToFile("FormClosing: Inserted value > 0. Cannot exit. restartValidators");
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
            Audio.Stop();
            log.LogMethodExit("Exiting money screen...");
        }

        void initializeForm()
        {
            log.LogMethodEntry();
            try
            {
                this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                pbDownArrow.Image = ThemeManager.CurrentThemeImages.InsertCashAnimation;
                btnCreditCard.BackgroundImage = btnDebitCard.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                lblTimeOut.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
            }
            catch (Exception ex) { log.Error(ex); }

            log.LogMethodExit();
        }

        void handleCardReadDummy()
        {
        }

        void RefreshTotals()
        {
            log.LogMethodEntry();
            try
            {
                lblPaid.Text = (ac != null && ac.totalValue != 0 ? ac.totalValue.ToString(ParafaitEnv.AMOUNT_FORMAT)
                                                             : "0.00");
                if (ac != null && (AmountRequired - ac.totalValue) > 0)
                {
                    lblBal.Text = (AmountRequired - ac.totalValue).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                }
                else
                {
                    lblBal.Text = (0).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                }
                //Application.DoEvents();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in refreshTotals: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmNewcard_Load(System.Object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            initializeForm();

            for (int i = 0; i < this.Controls.Count; i++)//12-06-2015:Starts
            {
                if (!(this.Controls[i].Name.Equals("btnPrev") ||
                    this.Controls[i].Name.Equals("btnCart")))
                {
                    this.Controls[i].Visible = true;
                }
            }//12-06-2015:Ends

            lblSiteName.Visible = panelAmountPaid.Visible = btnDebitCard.Visible = btnCreditCard.Visible = panelSummary.Visible = false;//Starts:Modification on 17-Dec-2015 for introducing new theme

            lblSiteName.Text = KioskStatic.SiteHeading;

            Application.DoEvents();

            KioskStatic.logToFile("Enter Money screen");
            log.Info("Enter Money screen");
            KioskStatic.logToFile(rowProduct["product_name"].ToString());
            log.Info(rowProduct["product_name"].ToString());
            KioskStatic.logToFile("Amount required: " + AmountRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
            log.Info("Amount required: " + AmountRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
            
            InitializeBillAndNoteAcceptors();
            if (ac == null)
            {
                //ac = KioskStatic.ac = new KioskStatic.acceptance();
                ac = new KioskStatic.acceptance();
            }
            ac.productPrice = AmountRequired;
            InitiateNoteCoinActionTimer();
            TimerMoney.Start();
            RefreshTotals();
            kioskTransaction.SetupKioskTransaction(printReceipt, coinAcceptor, billAcceptor, ac, showThankYou, showOK);

            Audio.PlayAudio(Audio.InsertExactAmount); 
            log.Info("_PaymentModeDTO: " + _PaymentModeDTO);
            if (_PaymentModeDTO.IsCreditCard == true)
            {
                btnCreditCard_Click(btnCreditCard, null);
            }
            else if (_PaymentModeDTO.IsDebitCard == true)
            {
                btnCreditCard_Click(btnDebitCard, null);
            }
            else
            {
                checkMoneyStatus();
            } 
            log.LogMethodExit();
        }
        private void InitializeBillAndNoteAcceptors()
        {
            log.LogMethodEntry();
            try
            {
                bool enableAcceptors = true;
                CommonBillAcceptor commonBA = (Application.OpenForms[0] as frmHome).commonBillAcceptor;
                if (commonBA != null && kioskTransactionObj.GetTotalPaymentsReceived() > 0)
                {
                    //KioskStatic.ac = ac = commonBA.GetAcceptance();
                    if (rowProduct["product_type"].ToString().Equals("VARIABLECARD"))
                    {
                        AmountRequired = ProductPrice = ac.totalValue;
                    }
                    enableAcceptors = false;
                }
                if (_PaymentModeDTO != null && _PaymentModeDTO.IsCash == true && enableAcceptors)

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
                    //if (billAcceptor != null)
                    //{
                    //    ac = KioskStatic.ac = billAcceptor.acceptance;
                    //}
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
                    //if (coinAcceptor != null)
                    //{ coinAcceptor.acceptance = ac; }
                }
                if (ac == null)
                {
                    //ac = KioskStatic.ac = new KioskStatic.acceptance();
                    ac = new KioskStatic.acceptance();
                }
                ac.productPrice = AmountRequired;
                lblNoChange.Text = MessageUtils.getMessage(507);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in InitializeBillAndNoteAcceptors: " + ex.Message);
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
                RefreshTotals();
                if (canPerformNoteCoinReceivedAction)
                {
                    canPerformNoteCoinReceivedAction = false;
                    checkMoneyStatus();
                    KioskStatic.logToFile("Performing NoteCoinActionTimer action");
                    log.Debug("Performing NoteCoinActionTimer action");
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
            try
            {
                billMessage = "";
                KioskStatic.billAcceptorDatareceived = true;
                bool noteRecd = billAcceptor.ProcessReceivedData(KioskStatic.billAcceptorRec, ref billMessage);
                if (noteRecd)
                {
                    totSecs = 0;
                    log.Info("noteRecd: " + noteRecd.ToString());
                    KioskStatic.logToFile("Bill Acceptor note received: " + billMessage); 
                    log.LogVariableState("Ac", ac);
                    log.LogVariableState("BillAcceptor.ReceivedNoteDenomination", billAcceptor.ReceivedNoteDenomination);
                    KioskStatic.updateKioskActivityLog(billAcceptor.ReceivedNoteDenomination, -1, (CurrentCard == null ? "" : CurrentCard.CardNumber), "BILL-IN", "Bill Inserted", ac);
                    kioskTransactionObj.ReceiveCashNoteForPayment(billAcceptor.ReceivedNoteDenomination); 
                    billAcceptor.ReceivedNoteDenomination = 0; 
                    System.Threading.Thread.Sleep(400);
                    KioskStatic.logToFile("Before calling requestStatus");
                    log.Info("Before calling requestStatus");
                    billAcceptor.requestStatus();
                    canPerformNoteCoinReceivedAction = true; 
                    KioskStatic.logToFile("IsHandleCreated: " + this.IsHandleCreated.ToString());
                    KioskStatic.logToFile("Set canPerformNoteCoinReceivedAction as true");
                    log.Info("Set canPerformNoteCoinReceivedAction as true");
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error in billAcceptorDataReceived: " + ex.Message);
                log.Error(ex);
                displayMessageLine(ex.Message, ERROR);
            }
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
                if (coinRecd)
                {
                    totSecs = 0;
                    log.Info("coinRecd: " + coinRecd.ToString());
                    log.LogVariableState("Ac", ac);
                    log.LogVariableState("CoinAcceptor.ReceivedCoinDenomination", coinAcceptor.ReceivedCoinDenomination);
                    KioskStatic.updateKioskActivityLog(-1, coinAcceptor.ReceivedCoinDenomination, (CurrentCard == null ? "" : CurrentCard.CardNumber), "COIN-IN", "Coin Inserted", ac);
                    kioskTransactionObj.ReceiveCoinForPayment(coinAcceptor.ReceivedCoinDenomination);
                    coinAcceptor.ReceivedCoinDenomination = 0;
                    System.Threading.Thread.Sleep(20);
                    canPerformNoteCoinReceivedAction = true;
                    KioskStatic.logToFile("IsHandleCreated: " + this.IsHandleCreated.ToString());
                    KioskStatic.logToFile("Set canPerformNoteCoinReceivedAction as true");
                    log.Info("Set canPerformNoteCoinReceivedAction as true");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile("Error in coinAcceptorDataReceived: " + ex.Message);
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
                if (billAcceptor != null && (KioskStatic.billAcceptorDatareceived == false || billAcceptor.Working == false))
                {
                    billAcceptorTimeout++;
                    //if (billAcceptorTimeout > 30 && KioskStatic.spBillAcceptor.IsOpen)
                    if (billAcceptorTimeout > 30)
                    {
                        billMessage = MessageUtils.getMessage(423);
                        KioskStatic.logToFile("Bill acceptor timeout / offline");
                        log.Info("Bill acceptor timeout / offline: billAcceptorTimeout - " + billAcceptorTimeout.ToString());
                    }

                    if (billAcceptorTimeout % 5 == 0)
                    {
                        billAcceptor.requestStatus();
                        if (billAcceptor.Working == false)
                        { restartValidators(); }
                    }
                } 
                if (!KioskStatic.coinAcceptorDatareceived && coinAcceptor != null)
                {
                    coinAcceptorTimeout++;
                    if (coinAcceptorTimeout > 30 && KioskStatic.spCoinAcceptor.IsOpen)
                    {
                        coinMessage = MessageUtils.getMessage(424);
                        KioskStatic.logToFile("Coin acceptor timeout / offline");
                        log.Info("Coin acceptor timeout / offline: coinAcceptorTimeout - " + coinAcceptorTimeout.ToString());
                    }

                    if (coinAcceptorTimeout % 5 == 0)
                    {
                        if (KioskStatic.spCoinAcceptor.IsOpen)
                        {
                            if (coinAcceptor != null)
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
                                TimerMoney.Start();
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
                            {
                                totSecs = 0;
                            }
                            else
                            {
                                totSecs = KioskStatic.MONEY_SCREEN_TIMEOUT - 3;
                            }
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
                            btnCancel.Enabled = false;
                            KioskTimerSwitch(true);
                            StartKioskTimer();
                            KioskStatic.logToFile("Operation Timed out...");
                            log.Info("Operation Timed out...");
                            log.LogMethodExit();
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
                        KioskStatic.logToFile("Timer Money Tick: ac.totalValue <= 0 is false, restartValidators");
                        restartValidators();
                        log.LogMethodExit(ac.totalValue, "ac.totalValue > 0");
                        return;
                    }
                }
                else
                {
                    if (Function == "I")
                    {
                        lblTimeOut.Text = "";
                    }
                    else if (Function == "C")
                    {
                        lblTimeOut.Font = savTimeOutFont;
                        if (ServerDateTime.Now.Millisecond / 500 > 0)
                            lblTimeOut.Text = "";
                        else
                            lblTimeOut.Text = "Transaction in progress";
                    }
                    else
                    {
                        lblTimeOut.Font = savTimeOutFont;
                        if (ServerDateTime.Now.Millisecond / 500 > 0)
                            lblTimeOut.Text = "";
                        else
                            lblTimeOut.Text = MessageUtils.getMessage(427);
                    }
                }

                if (billMessage.EndsWith("inserted") || billMessage.EndsWith("accepted") || billMessage.EndsWith("rejected"))
                {
                    log.Info(billMessage);
                    displayMessageLine(billMessage.Replace("inserted", MessageUtils.getMessage("inserted"))
                                                  .Replace("accepted", MessageUtils.getMessage("accepted"))
                                                  .Replace("rejected", MessageUtils.getMessage("rejected"))
                                                  .Replace("Bill", MessageUtils.getMessage("Bill"))
                                                  .Replace("Denomination", MessageUtils.getMessage("Denomination")), MESSAGE);
                    Audio.Stop();
                }
                else if (coinMessage.EndsWith("accepted") || coinMessage.EndsWith("rejected"))
                {
                    log.Info(coinMessage);
                    displayMessageLine(coinMessage.Replace("accepted", MessageUtils.getMessage("accepted"))
                                                  .Replace("rejected", MessageUtils.getMessage("rejected"))
                                                  .Replace("Denomination", MessageUtils.getMessage("Denomination")), MESSAGE);
                    Audio.Stop();
                }
                else if (billMessage.StartsWith("Insert") && coinMessage.StartsWith("Insert"))
                {
                    log.Info(billMessage + " " + MessageUtils.getMessage(428));
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
                                    log.Info("billAcceptor.criticalError:" + billMessage);
                                    using (frmOKMsg f = new frmOKMsg(MessageUtils.getMessage(billMessage) + ". " + Utilities.MessageUtils.getMessage(441)))
                                    {
                                        f.ShowDialog();
                                    }
                                }
                                else if (billAcceptor.overpayReject)
                                {
                                    log.Info("billAcceptor.overpayReject:" + billMessage);
                                    billAcceptor.overpayReject = false;
                                    using (frmOKMsg f = new frmOKMsg("Bill refused. Please insert exact amount"))
                                    {
                                        f.ShowDialog();
                                    }
                                }
                                else
                                {
                                    log.Info(billMessage);
                                    displayMessageLine(MessageUtils.getMessage(billMessage), MESSAGE);
                                }
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
                                    log.Info("coinAcceptor.criticalError:" + coinMessage);
                                    using (frmOKMsg f = new frmOKMsg(MessageUtils.getMessage(coinMessage) + ". " + Utilities.MessageUtils.getMessage(441)))
                                    {
                                        f.ShowDialog();
                                    }
                                }
                                else
                                {
                                    log.Info(coinMessage);
                                    displayMessageLine(MessageUtils.getMessage(coinMessage), MESSAGE);
                                }
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
                {
                    TimerMoney.Start();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                TimerMoney.Start();
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile(ex.Message + "-" + ex.StackTrace);
                btnHome.Enabled = true;
                btnCancel.Enabled = true;
            }
            log.LogMethodExit();
        }

        private Object thisLock = new Object();
        void checkMoneyStatus()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("checkMoneyStatus() - enter");
            TimeOut.Abort();
            KioskStatic.logToFile("checkMoneyStatus() -Abort TimeOut");
            lock (thisLock)
            {
                try
                {
                    KioskStatic.logToFile("checkMoneyStatus()- before checking TotalValue greater than or equals to AmountRequired");
                    log.Info("Before checking TotalValue greater than or equals to AmountRequired");
                    if (ac.totalValue >= AmountRequired)
                    {
                        KioskStatic.logToFile("checkMoneyStatus() - ac.totalValue = " + ac.totalValue.ToString(amountFormat) + ", AmountRequired = " + ProductPrice.ToString(amountFormat));
                        log.Info("Ac.totalValue = " + ac.totalValue.ToString(amountFormat) + ", AmountRequired = " + ProductPrice.ToString(amountFormat));
                        TimerMoney.Stop();

                        if (kioskTransaction.cancelPressed)
                        {
                            KioskStatic.logToFile("CHS: Cancel button Pressed");
                            log.LogMethodExit("Cancel button Pressed");
                            return;
                        }


                        btnHome.Enabled = false;
                        btnCancel.Enabled = false;
                        Application.DoEvents();

                        KioskStatic.logToFile("Valid amount inserted: " + ac.totalValue.ToString(amountFormat) + "; " + "Required: " + ProductPrice.ToString(amountFormat));
                        log.Info("Valid amount inserted: " + ac.totalValue.ToString(amountFormat) + "; " + "Required: " + ProductPrice.ToString(amountFormat));

                        if (billAcceptor != null)
                        {
                            KioskStatic.logToFile("checkMoneyStatus() - disabling billAcceptor");
                            log.Info("Calling billAcceptor.disableBillAcceptor");
                            System.Threading.Thread.Sleep(300);
                            billAcceptor.disableBillAcceptor();
                        }
                        if (coinAcceptor != null)
                        {
                            KioskStatic.logToFile("checkMoneyStatus() - disabling coinAcceptor");
                            log.Info("Calling coinAcceptor.disableBillAcceptor");
                            System.Threading.Thread.Sleep(300);
                            coinAcceptor.disableCoinAcceptor();
                        }

                        log.Info("Function: " + Function);
                        if (Function == "I")
                        {
                            KioskStatic.logToFile("checkMoneyStatus() - Inside Fuction=I");
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
                            //kioskTransaction.dispenseCards(cardDispenser);
                            kioskTransactionObj.ExecuteTransaction(cardDispenser, DisplayMessageLine, frmOKMsg.ShowUserMessage, showThankYou, printReceipt);
                            //this.Close();
                        }
                        else if (Function == "C")
                        {
                            KioskStatic.logToFile("checkMoneyStatus() - Before calling doCheckIn()");
                            log.Info(MessageUtils.getMessage(1008));
                            displayMessageLine(MessageUtils.getMessage(1008), MESSAGE);
                            Application.DoEvents();
                            kioskTransaction.doCheckIn();
                            KioskStatic.logToFile("checkMoneyStatus() - After doCheckIn() process");
                            log.Info("checkMoneyStatus() - After doCheckIn() process");
                        }
                        else
                        {
                            log.Info(MessageUtils.getMessage(503));
                            displayMessageLine(MessageUtils.getMessage(503), MESSAGE);
                            Application.DoEvents();
                            kioskTransaction.rechargeCard();
                        }

                        AmountRequired = 0;
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        KioskStatic.logToFile("checkMoneyStatus() - TotalValue greater than or equls to AmountRequired condition failed- ac.totalValue = " + ac.totalValue.ToString(amountFormat) + ", AmountRequired = " + ProductPrice.ToString(amountFormat));
                        log.Info("TotalValue greater than or equls to AmountRequired condition failed- ac.totalValue = " + ac.totalValue.ToString(amountFormat) + ", AmountRequired = " + ProductPrice.ToString(amountFormat));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    displayMessageLine(ex.Message, ERROR);
                    KioskStatic.logToFile(ex.Message + "-" + ex.StackTrace);
                    btnHome.Enabled = true;
                    btnCancel.Enabled = true;
                }
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
                string scannedTagNumber = checkScannedEvent.Message;
                DeviceClass encryptedTagDevice = sender as DeviceClass;
                if (tagNumberParser.IsTagDecryptApplicable(encryptedTagDevice, checkScannedEvent.Message.Length))
                {
                    string decryptedTagNumber = string.Empty;
                    try
                    {
                        decryptedTagNumber = tagNumberParser.GetDecryptedTagData(encryptedTagDevice, checkScannedEvent.Message);
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number result: ", ex);
                        displayMessageLine(ex.Message, ERROR);
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        displayMessageLine(ex.Message, ERROR);
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        displayMessageLine(ex.Message, ERROR);
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
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
                    displayMessageLine(ex.Message, ERROR);
                    log.Error(ex.Message);
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
            log.Info("_PaymentModeDTO: " + _PaymentModeDTO);
            if (_PaymentModeDTO.IsCreditCard != true)
            {
                KioskStatic.logToFile("_PaymentMode != CREDITCARD");
                if ((AmountRequired - ac.totalValue) > 0)
                {
                    if (billAcceptor != null)
                    {
                        KioskStatic.logToFile("Bill acceptor is not equls to null");
                        log.LogVariableState("KioskStatic.BillAcceptorModel", KioskStatic.BillAcceptorModel);
                        if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB))
                        {
                            billAcceptor.disableBillAcceptor();
                            KioskStatic.logToFile("Before initializing bill acceptor");
                            billAcceptor.initialize();
                            KioskStatic.logToFile("After initializing bill acceptor");
                        }
                        else
                        {
                            billAcceptor.requestStatus();
                        }
                    }

                    if (coinAcceptor != null)
                        coinAcceptor.set_acceptance();
                }
                else
                {
                    KioskStatic.logToFile("RestartValidators - skip restart, balance amount is 0");
                }
            }

            btnDebitCard.Enabled = btnCreditCard.Enabled = true;
            btnHome.Enabled = true;
            btnCancel.Enabled = true;


            TimerMoney.Start();
            log.LogMethodExit();
        }

        int cancelPressCount = 0;
        public override void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CancelAction();
            log.LogMethodExit();
        }
        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CancelAction();
            log.LogMethodExit();
        }
        private void CancelAction()
        {
            log.LogMethodEntry();
            try
            {
                kioskTransaction.cancelPressed = true;
                btnHome.Enabled = false;
                btnCancel.Enabled = false;
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
                    || (coinAcceptor != null && coinAcceptor.ReceivedCoinDenomination > 0)
                    )
                {
                    kioskTransaction.cancelPressed = false;
                    btnHome.Enabled = true;
                    btnCancel.Enabled = true;
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
                    btnCancel.Enabled = true;
                    log.Info("billAcceptor.StillProcessing");
                    KioskStatic.logToFile("btnHome_Click: billAcceptor.StillProcessing");
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4682), ERROR);
                    log.LogMethodExit();
                    return;
                }
                KioskStatic.logToFile("Cancel button Disabling bill acceptor ");

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
                        using (frmYesNo f = new frmYesNo(message))
                        {
                            if (f.ShowDialog() == System.Windows.Forms.DialogResult.No)
                            {
                                if (Utilities.getParafaitDefaults("ALLOW_VARIABLE_NEW_CARD_ISSUE").Equals("Y"))
                                {
                                    message = MessageUtils.getMessage(934, ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    using (frmYesNo ff = new frmYesNo(message))
                                    {
                                        DialogResult dr = ff.ShowDialog();
                                        if (dr == System.Windows.Forms.DialogResult.Cancel) // time out
                                        {
                                            kioskTransaction.cancelPressed = false;
                                            KioskStatic.logToFile("Cancel button, timeout on abort option, restartValidators");
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
                                                {
                                                    using (frmOKMsg fom = new frmOKMsg(MessageUtils.getMessage(935, CardCount, savCardCount)))
                                                    {
                                                        fom.ShowDialog();
                                                    }
                                                }
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
                                            {
                                                rowProduct["Price"] = kioskTransaction.ProductPrice = AmountRequired = ac.productPrice = ProductPrice = ac.totalValue; //added KioskTransaction.ProductPrice
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
                                }
                                else
                                {
                                    abort = true;
                                }
                            }
                            else
                            {
                                kioskTransaction.cancelPressed = false;
                                KioskStatic.logToFile("Cancel button, variable new card option not selected, restartValidators");
                                restartValidators();
                            }
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
                            {
                                frmok.ShowDialog();
                            }

                            ac.totalValue = 0;
                            closeForm();
                        }
                    }
                    else if (Function.Equals("C"))
                    {
                        message = MessageUtils.getMessage(442, ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                        using (frmYesNo f = new frmYesNo(message))
                        {
                            if (f.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                            {
                                kioskTransaction.doCheckIn();
                                closeForm();
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
                                        KioskStatic.logToFile("Abort Check-In... Money entered: " + ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                        log.Info("Abort Check-In... Money entered: " + ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                        if (KioskStatic.receipt)
                                            kioskTransaction.print_receipt(true);

                                        using (frmOKMsg frmok = new frmOKMsg(MessageUtils.getMessage(441)))
                                        {
                                            frmok.ShowDialog();
                                        }

                                        ac.totalValue = 0;
                                        closeForm();
                                    }
                                    else
                                    {
                                        kioskTransaction.cancelPressed = false;
                                        KioskStatic.logToFile("Cancel button, Decided not to exit, restartValidators");
                                        restartValidators();
                                    }
                                }
                            }
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
                                closeForm();
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
                                        {
                                            frmok.ShowDialog();
                                        }

                                        ac.totalValue = 0;
                                        closeForm();
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
                    StopKioskTimer();
                    DialogResult = DialogResult.Cancel;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("btnCancel_Click(): " + ex.Message + ":" + ex.StackTrace);
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
            if (ac.totalValue > 0)
            {
                KioskStatic.logToFile("KioskTimer_Tick, ac.totalValue > 0, restartValidators");
                restartValidators();
                log.LogMethodExit();
                return;
            }
            if (billAcceptor != null && billAcceptor.StillProcessing())
            {
                totSecs = 0;
                KioskStatic.logToFile("billAcceptor stillProcessing. reset timer clock");
                log.Info("billAcceptor stillProcessing. reset timer cock");
                log.LogMethodExit();
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

            string Source = "";
            if (Function == "I")
            {
                Source = "NEW";
            }
            else if (Function == "C")
            {
                Source = "Check-In";
            }
            else
            {
                Source = "Recharge";
            }

            string message = "";
            string printMsg;
            string trxNumber = "";

            if (Function.Equals("C"))
            {
                message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4149, rowProduct["product_name"].ToString());
                printMsg = (receiptPrinted) ? MessageContainerList.GetMessage(Utilities.ExecutionContext, 4148) : "";
                trxNumber = kioskTransaction.UpdatedTrx.Trx_No;
            }
            else
            {
                message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 952, rowProduct["product_name"].ToString());
                printMsg = (receiptPrinted) ? MessageContainerList.GetMessage(Utilities.ExecutionContext, 498) : "";
            }

            using (frmSuccessMsg frm = new frmSuccessMsg(message, "", "", printMsg, "", Source, kioskTransaction.UpdatedTrx.Trx_No))
            {
                frm.ShowDialog();
            }

            KioskStatic.logToFile("Exit money screen");
            log.Info("Exit money screen");
            Close();
            log.LogMethodExit();
        }

        void showOK(string message, bool enableTimeOut = true)
        {
            log.LogMethodEntry(message, enableTimeOut);
            KioskStatic.logToFile(message);
            using (frmOKMsg frm = new frmOKMsg(message, enableTimeOut))
            {
                frm.ShowDialog();
            }
            log.LogMethodExit();
        }

        private void btnCreditCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
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

        private void closeForm()//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            StopKioskTimer();
            DialogResult = System.Windows.Forms.DialogResult.OK;
            log.LogMethodExit();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            log.LogMethodEntry(msg, keyData);
            log.LogMethodExit(true);
            return true;
        }

        private void frmCashTransaction_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Card Transaction form closed event");
            try
            {
                if (billAcceptor != null)
                    billAcceptor.disableBillAcceptor();
                if (coinAcceptor != null)
                    coinAcceptor.disableCoinAcceptor();
            }
            catch (Exception ex)
            {
                log.Error("Bill/Coin Acceptor disable Error:", ex);
            }
            this.Close();
            log.LogMethodExit();
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
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionBtnHomeTextForeColor;//PLEASE TAP THE CARD THAT POINTS WILL BE ADDED TO
                this.button1.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionButton1TextForeColor;//Prev button(Terms)
                this.lblTimeOut.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionLblTimeOutTextForeColor;//Next button(Terms)
                this.label6.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionLabel6TextForeColor;//Footer text message
                this.Label1.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionLabel1TextForeColor;//Footer text message
                this.lblTotalToPay.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionLblTotalToPayTextForeColor;//Footer text message
                this.Label8.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionLabel8TextForeColor;//Footer text message
                this.Label9.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionLabel9TextForeColor;//Footer text message
                this.lblPaid.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionLblPaidTextForeColor;//Footer text message
                this.lblBal.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionLblBalTextForeColor;//Footer text message
                this.lblProductCPValidityMsg.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionLblProductCPValidityMsgTextForeColor;//Footer text message
                this.lblNoChange.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionLblNoChangeTextForeColor;//Footer text message
                this.btnCreditCard.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionBtnCreditCardTextForeColor;//Footer text message
                this.btnDebitCard.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionBtnDebitCardTextForeColor;//Footer text message
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionTxtMessageTextForeColor;//Footer text message
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.FrmCashTransactionLblSiteNameTextForeColor;
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
