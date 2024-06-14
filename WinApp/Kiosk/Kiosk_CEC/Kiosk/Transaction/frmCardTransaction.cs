/********************************************************************************************
* Project Name - Parafait_Kiosk -frmCardTransaction
* Description  - frmCardTransaction 
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A            Modified for MultiPoint, Timeout crash, Validty msg changes & Tax display
*2.70        1-Jul-2019       Lakshminarayana     Modified to add support for ULC cards 
*2.80        20-Sep-2019      Archana             Modified to remove debit card option 
*2.80.1      10-Nov-2020      Deeksha             Modified to display entitlement information message for payment mode / transaction screen
*2.80.1      02-Feb-2021      Deeksha             Theme changes to support customized Images/Font
*2.90.0      30-Jun-2020      Dakshakh raj        Dynamic Payment Modes based on set up
*2.100.0     05-Aug-2020      Guru S A            Kiosk activity log changes
*2.100.0     27-Oct-2020      Dakshakh Raj        Payment Buttons image issue fix
*2.130.0     30-Jun-2021      Dakshak             Theme changes to support customized Font ForeColor
*2.140.0     22-Oct-2021      Sathyavathi         CEC enhancement - Fund Raiser and Donations
*2.130.9     12-Jun-2022      Sathyavathi         Removed hard coded amount/number format
*2.150.0.0   16-Nov-2022      Vignesh Bhat        Kiosk Printing receipts with enabled configuration settings
*2.150.0.0   13-Oct-2022      Sathyavathi         Mask card number
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.KioskCore.CoinAcceptor;
using Semnox.Parafait.KioskCore.BillAcceptor;
using Semnox.Parafait.KioskCore.CardDispenser;
using Semnox.Parafait.KioskCore;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Device.PaymentGateway;
using System.Text.RegularExpressions;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.POS;
using Parafait_Kiosk.Transaction;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Parafait_Kiosk
{
    public partial class frmCardTransaction : BaseFormKiosk
    {
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

        private decimal depositTaxAmount;
        private decimal depositPriceWithOutTax;

        CoinAcceptor coinAcceptor;
        BillAcceptor billAcceptor;
        CardDispenser cardDispenser;

        KioskStatic.acceptance ac;
        KioskStatic.configuration config = KioskStatic.config;

        int CardCount = 1;
        PaymentModeDTO _PaymentModeDTO;
        public TransactionPaymentsDTO trxPaymentDTO = null;
        string couponNumber = "";
        CustomerDTO _customerDTO;
        string selectedLoyaltyCardNo = "";
        Card _rechargeCard;

        int billAcceptorTimeout = 0;
        int coinAcceptorTimeout = 0;

        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;

        bool MultipleCardsInSingleProduct = false;

        Font savTimeOutFont;
        Font timeoutFont;

        clsKioskTransaction kioskTransaction;

        string selectedEntitlementType = "Credits";
        string previousCardNumber = "";
        DiscountsSummaryDTO discountSummaryDTO = null;
        private readonly TagNumberParser tagNumberParser;
        private List<KeyValuePair<string, ProductsDTO>> selectedFundsAndDonationsList = null;
        private Semnox.Parafait.Transaction.Transaction currentTrx;
        private string amountFormat;
        private System.Windows.Forms.Timer noteCoinActionTimer;
        private bool canPerformNoteCoinReceivedAction = false;

        public frmCardTransaction(string pFunction, DataRow prowProduct, Card rechargeCard, CustomerDTO customerDTO, 
                                    PaymentModeDTO selectedPaymentModeDTO, int inCardCount, string entitlementType, string loyaltyCardNo, 
                                    DiscountsSummaryDTO discountSummaryDTO = null, string couponNo = null, 
                                    List<KeyValuePair<string, ProductsDTO>> fundDonationProductsDTOList = null,
                                    Semnox.Parafait.Transaction.Transaction inTrx = null) // I for issue, R for recharge
        {
            log.LogMethodEntry(pFunction, prowProduct, rechargeCard, customerDTO, selectedPaymentModeDTO, inCardCount, entitlementType, 
                                loyaltyCardNo, discountSummaryDTO, couponNo, fundDonationProductsDTOList, inTrx);
            currentTrx = inTrx;
            selectedFundsAndDonationsList = fundDonationProductsDTOList;
            InitializeComponent();
            initializeForm();
            amountFormat = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "AMOUNT_FORMAT");
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            KioskStatic.formatMessageLine(txtMessage, 26, Properties.Resources.bottom_bar);
            InitializefrmCardTransaction(pFunction, prowProduct, rechargeCard, customerDTO, selectedPaymentModeDTO, inCardCount, entitlementType, loyaltyCardNo, discountSummaryDTO = null, couponNo = null, currentTrx);
            log.LogMethodExit();
        }

        private void InitializefrmCardTransaction(string pFunction, DataRow prowProduct, Card rechargeCard, CustomerDTO customerDTO, 
                                                    PaymentModeDTO selectedPaymentModeDTO, int inCardCount, string entitlementType, 
                                                    string loyaltyCardNo, DiscountsSummaryDTO discountSummaryDTO = null, string couponNo = null,
                                                    Semnox.Parafait.Transaction.Transaction Trx = null)
        {
            log.LogMethodEntry(pFunction, prowProduct, rechargeCard, customerDTO, selectedPaymentModeDTO, inCardCount, entitlementType, 
                                loyaltyCardNo, discountSummaryDTO, couponNo, Trx);
            Audio.Stop();
            selectedEntitlementType = entitlementType;
            this.discountSummaryDTO = discountSummaryDTO;
            kioskTransaction = new clsKioskTransaction(this, pFunction, prowProduct, rechargeCard, customerDTO, selectedPaymentModeDTO, 
                                                        inCardCount, selectedEntitlementType, loyaltyCardNo, discountSummaryDTO, couponNo, 
                                                        Trx, selectedFundsAndDonationsList);

            ProductPrice = kioskTransaction.ProductPrice;
            CardCount = kioskTransaction.CardCount;
            MultipleCardsInSingleProduct = kioskTransaction.MultipleCardsInSingleProduct;
            ProductPrice = kioskTransaction.ProductPrice;

            if (pFunction != "C")
                AmountRequired = kioskTransaction.AmountRequired; //this is because, trx line is not created for the actual product yet in case of new card/top up
            else
                AmountRequired = (Trx == null) ? kioskTransaction.AmountRequired : (decimal)Trx.Net_Transaction_Amount;

            productTaxAmount = kioskTransaction.ProductTaxAmount;
            productPriceWithOutTax = kioskTransaction.ProductPriceWithOutTax;

            depositTaxAmount = kioskTransaction.DepositTaxAmount;
            depositPriceWithOutTax = kioskTransaction.DepositPriceWithOutTax;
            Decimal totalCardDeposit = kioskTransaction.ProductDeposit * (CardCount);
            _customerDTO = customerDTO;
            _PaymentModeDTO = selectedPaymentModeDTO;
            Function = pFunction;
            rowProduct = prowProduct;
            _rechargeCard = rechargeCard;
            selectedLoyaltyCardNo = loyaltyCardNo;

            TimerMoney.Enabled = false;
            KioskTimerSwitch(false);
            Utilities.setLanguage(this);


            displayMessageLine("", MESSAGE);
            lblTimeOut.Text = KioskStatic.MONEY_SCREEN_TIMEOUT.ToString("#0");

            savTimeOutFont = lblTimeOut.Font;
            timeoutFont = lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            if (rechargeCard != null)
                CurrentCard = rechargeCard;

            //bool enableAcceptors = true;
            //CommonBillAcceptor commonBA = (Application.OpenForms[0] as frmHome).commonBillAcceptor;
            //if (commonBA != null && commonBA.GetAcceptance().totalValue > 0)
            //{
            //    KioskStatic.ac = ac = commonBA.GetAcceptance();
            //    if (prowProduct["product_type"].ToString().Equals("VARIABLECARD"))
            //    {
            //        AmountRequired = ProductPrice = ac.totalValue;
            //    }
            //    enableAcceptors = false;
            //}

            this.ShowInTaskbar = true;

            if (discountSummaryDTO != null)
            {
                panelDiscount.Visible = true;
                txtDiscountName.Text = discountSummaryDTO.DiscountName;
                txtDiscAmount.Text = discountSummaryDTO.DiscountAmount.ToString(amountFormat);
                txtDiscPerc.Text = discountSummaryDTO.DiscountPercentage.ToString() + "%";
            }


            lblPackage.Text = ProductPrice.ToString(amountFormat) +
                                Environment.NewLine + "(" +
                                prowProduct["product_name"].ToString() + ")";
            if (pFunction == "I")
                lblCardnumber.Text = MessageUtils.getMessage("NEW");
            else
                lblCardnumber.Text = KioskHelper.GetMaskedCardNumber(rechargeCard.CardNumber);
            string message = KioskStatic.GetEntitlementsMessage(prowProduct, CardCount, selectedEntitlementType, MultipleCardsInSingleProduct, rechargeCard);
            lblQuantity.Text = message;
            BuildToolTipForlblQuantity(message);


            lblBal.Text = lblTotalToPay.Text = (AmountRequired).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            lblTotal.Text = (AmountRequired - totalCardDeposit).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
            lblPrice.Text = (ProductPrice - productTaxAmount - kioskTransaction.ProductDeposit).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
            lblTaxAmount.Text = (productTaxAmount - depositTaxAmount).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);

            UpdateFundDonationPanelText(totalCardDeposit);

            lblBal.Text = lblTotalToPay.Text = (AmountRequired).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);

            if (kioskTransaction.ProductDeposit > 0)
            {
                lblDepositQty.Text = CardCount.ToString();
                lblDepositTotal.Text = totalCardDeposit.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
                lblDepositName.Text = String.IsNullOrEmpty(KioskStatic.GetDepositProduct().TranslatedProductName) ? KioskStatic.GetDepositProduct().ProductName : KioskStatic.GetDepositProduct().TranslatedProductName;
                lblDepositPrice.Text = depositPriceWithOutTax.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
                lblDepositTax.Text = depositTaxAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
            }

            //if (_PaymentModeDTO != null && _PaymentModeDTO.IsCash == true && enableAcceptors)
            //{

            //    if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB))
            //    {
            //        billAcceptor = KioskStatic.getBillAcceptor(KioskStatic.config.baport.ToString());
            //        ((NV9USB)billAcceptor).dataReceivedEvent = billAcceptorDataReceived;
            //        billAcceptor.initialize();
            //    }
            //    else if (config.baport != 0)
            //    {
            //        billAcceptor = KioskStatic.getBillAcceptor(KioskStatic.config.baport.ToString());
            //        KioskStatic.baReceiveAction = billAcceptorDataReceived;
            //        if (billAcceptor.spBillAcceptor != null)
            //        {
            //            billAcceptor.spBillAcceptor.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(spBillAcceptor_DataReceived);
            //        }
            //        billAcceptor.initialize();
            //        billAcceptor.requestStatus();
            //    }

            //    if (billAcceptor != null)
            //    {
            //        ac = KioskStatic.ac = billAcceptor.acceptance;
            //    }

            //    if (config.coinAcceptorport > 0)
            //    {
            //        coinAcceptor = KioskStatic.getCoinAcceptor();
            //        coinAcceptor.spCoinAcceptor = KioskStatic.spCoinAcceptor;
            //        if (coinAcceptor.set_acceptance())
            //        {
            //            KioskStatic.caReceiveAction = coinAcceptorDataReceived;
            //            log.Info("KioskStatic.caReceiveAction = coinAcceptorDataReceived");
            //        }
            //        else
            //        {
            //            log.Info("KioskStatic.caReceiveAction = null");
            //            KioskStatic.caReceiveAction = null;
            //        }
            //    }
            //    if (coinAcceptor != null)
            //        coinAcceptor.acceptance = ac;
            //}

            //if (ac == null)
            //    ac = KioskStatic.ac = new KioskStatic.acceptance();

            //ac.productPrice = AmountRequired;
            DisplayValidtyMsg(Convert.ToInt32(rowProduct["product_id"]));

            for (int i = 0; i < this.Controls.Count; i++)
            {
                this.Controls[i].Visible = false;
            }
            SetCustomizedFontColors();
            RefreshTotals();
            log.LogMethodExit();
        }

        private void frmNewcard_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
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
                KioskStatic.logToFile("Closing form: billAcceptor stillProcessing. reset timer clock");
                log.Info("billAcceptor stillProcessing. reset timer cock");
                log.LogMethodExit();
                return;
            }
            KioskStatic.logToFile("FormClosing: True");
            this.Hide();
            TimerMoney.Stop();
            if(noteCoinActionTimer != null)
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
                KioskStatic.logToFile(this.Name + ": Dispenser Reader unregistered");
                log.Info(this.Name + ": Dispenser Reader unregistered");
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
                this.BackgroundImage = KioskStatic.CurrentTheme.TransactionBackgroundImage;
                lblTimeOut.BackgroundImage = KioskStatic.CurrentTheme.MoneyScreenTimerButtonImage;
                KioskStatic.setDefaultFont(this);

            }
            catch { }
            log.LogMethodExit();
        }

        private void frmNewcard_Load(System.Object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FrmCardTransactionLoad(true);
            log.LogMethodExit();
        }

        private void FrmCardTransactionLoad(bool firstLoad)
        {
            log.LogMethodEntry(firstLoad);
            for (int i = 0; i < this.Controls.Count; i++)//12-06-2015:Starts
            {
                this.Controls[i].Visible = true;
            }//12-06-2015:Ends

            //btnDebitCard.Visible = btnCreditCard.Visible = false;

            lblSiteName.Text = KioskStatic.SiteHeading;

            //txtMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            //txtMessage.ForeColor = KioskStatic.CurrentTheme.TextForeColor;

            Application.DoEvents();
            if (discountSummaryDTO != null)
            {
                panelDiscount.Visible = true;
            }
            else
            {
                panelDiscount.Visible = false;
            }

            UpdateFundDonationPanelVisibility();

            if (kioskTransaction.ProductDeposit > 0)
            {
                panelDeposit.Visible = true;
            }
            else
            {
                panelDeposit.Visible = false;
            }
            KioskStatic.logToFile("Enter Money screen");
            log.Info("Enter Money screen");
            KioskStatic.logToFile(rowProduct["product_name"].ToString());
            log.Info(rowProduct["product_name"].ToString());
            KioskStatic.logToFile("Amount required: " + AmountRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
            log.Info("Amount required: " + AmountRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
            bool printReceipt = false;
            string option = Utilities.getParafaitDefaults("TRX_AUTO_PRINT_AFTER_SAVE");
            if (fLPPModes != null && fLPPModes.Controls.Count > 0)
            {
                fLPPModes.Controls.Clear();
            }
            loadPaymentModes(true);
            if (option.Equals("A"))
            {
                using (frmYesNo fyn = new frmYesNo(MessageUtils.getMessage(933)))
                {
                    if (fyn.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                        printReceipt = true;
                }
            }
            else if (option.Equals("Y"))
            {
                printReceipt = true;
            }

            if (firstLoad)
            {
                InitializeBillAndNoteAcceptors();
                if (ac == null)
                {
                    ac = KioskStatic.ac = new KioskStatic.acceptance();
                }
                ac.productPrice = AmountRequired;
                InitiateNoteCoinActionTimer();
            }
            TimerMoney.Start();
            RefreshTotals();
            kioskTransaction.SetupKioskTransaction(printReceipt, coinAcceptor, billAcceptor, ac, showThankYou, showOK);

            Audio.PlayAudio(Audio.InsertExactAmount);
            if (_PaymentModeDTO != null)
            {
                fLPPModes.Visible = false;
                verticalScrollBarView2.Visible = false;
                btnCancel.Enabled = true;
                Button btnPayment = new Button();
                btnPayment.Tag = _PaymentModeDTO;
                TimerMoney.Start();
                btnPaymentMode_Click(btnPayment, null);
            }
            else
            {
                fLPPModes.Visible = true;
                verticalScrollBarView2.Visible = true;
                fLPPModes.Enabled = true;
                displaybtnCancel(true);
                displaybtnPrev(false);
                btnCancel.Enabled = true;
            }
            previousCardNumber = "";
            log.LogMethodExit();
        }

        private void InitializeBillAndNoteAcceptors()
        {
            log.LogMethodEntry();
            try
            {
                bool enableAcceptors = true;
                CommonBillAcceptor commonBA = (Application.OpenForms[0] as FSKCoverPage).commonBillAcceptor;
                if (commonBA != null && commonBA.GetAcceptance().totalValue > 0)
                {
                    KioskStatic.ac = ac = commonBA.GetAcceptance();
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
                    if (billAcceptor != null)
                    {
                        ac = KioskStatic.ac = billAcceptor.acceptance;
                    }
                    if (config.coinAcceptorport > 0)
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
                }
                if (ac == null)
                {
                    ac = KioskStatic.ac = new KioskStatic.acceptance();
                }
                ac.productPrice = AmountRequired;
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
        private void loadPaymentModes(bool loadPaymentmodes)
        {
            log.LogMethodEntry(loadPaymentmodes);
            List<POSPaymentModeInclusionDTO> pOSInclusionDTOList = null;
            pOSInclusionDTOList = KioskStatic.pOSPaymentModeInclusionDTOList;
            List<POSPaymentModeInclusionDTO> pOSPaymentModeInclusionDTOList = null;

            if (pOSInclusionDTOList != null && pOSInclusionDTOList.Any())
            {
                pOSPaymentModeInclusionDTOList = pOSInclusionDTOList;//.FindAll(x => x.PaymentModeDTO.IsCash == false);
            }
            string imageFolder = KioskStatic.Utilities.getParafaitDefaults("IMAGE_DIRECTORY");
            bool temp = true;
            if (pOSPaymentModeInclusionDTOList != null && pOSPaymentModeInclusionDTOList.Any())
            {
                foreach (POSPaymentModeInclusionDTO pOSPaymentModeInclusionDTO in pOSPaymentModeInclusionDTOList)
                {
                    PaymentModeDTO paymentModeDTO;
                    paymentModeDTO = pOSPaymentModeInclusionDTO.PaymentModeDTO;
                    Button btnPayment = new Button();
                    String btnText = "";

                    if (String.IsNullOrEmpty(pOSPaymentModeInclusionDTO.FriendlyName))
                    {
                        btnText = paymentModeDTO.PaymentMode;
                    }
                    else
                    {
                        btnText = pOSPaymentModeInclusionDTO.FriendlyName;
                    }
                    if (paymentModeDTO.PaymentMode == "Cash")
                    {
                        btnPayment.Text = "Cash\r\nNo Change Made";
                    }
                    else
                    {
                        btnPayment.Text = btnText;
                    }

                    if (pOSPaymentModeInclusionDTOList.Count > 2 && pOSPaymentModeInclusionDTOList.Count % 2 == 0)
                    {
                        btnPayment.Margin = new System.Windows.Forms.Padding(15, 5, 5, 5);
                        btnPayment.Size = new System.Drawing.Size(480, 198);
                    }
                    else if (pOSPaymentModeInclusionDTOList.Count <= 2)
                    {
                        btnPayment.Margin = new System.Windows.Forms.Padding(10, 10, 0, 10);
                        btnPayment.Size = new System.Drawing.Size(1015, 197);
                    }
                    else
                    {
                        if (temp == true)
                        {
                            btnPayment.Margin = new System.Windows.Forms.Padding(10, 10, 0, 10);
                            btnPayment.Size = new System.Drawing.Size(1015, 197);
                            temp = false;
                        }
                        else
                        {
                            btnPayment.Margin = new System.Windows.Forms.Padding(15, 5, 5, 5);
                            btnPayment.Size = new System.Drawing.Size(480, 198);
                        }

                    }
                    if (String.IsNullOrEmpty(paymentModeDTO.ImageFileName))
                    {
                        if (pOSPaymentModeInclusionDTOList.Count <= 2)
                        {
                            btnPayment.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Cash_Button;
                        }
                        else
                        {
                            btnPayment.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.credit_debit_button;
                        }
                        btnPayment.Text = btnText;
                    }
                    else
                    {
                        try
                        {
                            object o = KioskStatic.Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                    new SqlParameter("@FileName", imageFolder + "\\" + paymentModeDTO.ImageFileName));

                            btnPayment.BackgroundImage = KioskStatic.Utilities.ConvertToImage(o);
                            btnPayment.Text = "";
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            KioskStatic.logToFile(ex.Message + ": " + imageFolder + "\\" + paymentModeDTO.ImageFileName);
                            btnPayment.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.credit_debit_button;
                        }
                    }


                    btnPayment.BackColor = System.Drawing.Color.Transparent;
                    btnPayment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
                    btnPayment.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
                    btnPayment.FlatAppearance.BorderSize = 0;
                    btnPayment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                    btnPayment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                    btnPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btnPayment.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btnPayment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
                    btnPayment.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
                    //btn.Location = new System.Drawing.Point(10, 10);
                    //btn.Margin = new System.Windows.Forms.Padding(10, 10, 0, 10);
                    btnPayment.Name = "btn" + btnText;
                    //btn.Size = new System.Drawing.Size(1015, 197);
                    btnPayment.TabIndex = 12;
                    btnPayment.UseVisualStyleBackColor = false;
                    btnPayment.Tag = paymentModeDTO;
                    btnPayment.Click += new System.EventHandler(this.btnPayment_click);

                    if (paymentModeDTO.IsDebitCard == true && KioskStatic.Utilities.getParafaitDefaults("SHOW_DEBIT_CARD_BUTTON") == "N")
                    {
                        btnPayment.Visible = false;
                    }
                    fLPPModes.Controls.Add(btnPayment);
                }
            }
            SetCustomizedFontColors();
        }

        private void btnPayment_click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Button btn = (Button)sender;
            PaymentModeDTO paymentModeDTO = (PaymentModeDTO)btn.Tag;
            TimerMoney.Stop();
            if (paymentModeDTO.IsDebitCard == true)
            {
                TransactionPaymentsDTO transactionPaymentsDTO = null;
                using (frmPaymentGameCard frmg = new frmPaymentGameCard(Function, rowProduct, _rechargeCard, _customerDTO, paymentModeDTO, CardCount, selectedEntitlementType, discountSummaryDTO == null ? null : discountSummaryDTO, couponNumber == string.Empty ? null : couponNumber, selectedFundsAndDonationsList, currentTrx))
                {
                    frmg.ShowDialog();
                    if (frmg.debitTrxPaymentDTO != null)
                    {
                        transactionPaymentsDTO = frmg.debitTrxPaymentDTO;
                    }
                }
                this.Close();
            }
            else
            {
                InitializefrmCardTransaction(Function, rowProduct, _rechargeCard, _customerDTO, paymentModeDTO, CardCount, selectedEntitlementType, selectedLoyaltyCardNo, discountSummaryDTO == null ? null : discountSummaryDTO, couponNumber == string.Empty ? null : couponNumber, currentTrx);
                FrmCardTransactionLoad(false);
            }
        }

        private void btnPaymentMode_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            PaymentModeDTO paymentModeDTO = (PaymentModeDTO)btn.Tag;
            if (paymentModeDTO != null)
            {
                if (paymentModeDTO.IsCreditCard == true)
                {
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
                        KioskStatic.logToFile("btnCreditCard_Click Event. ");
                        TimerMoney.Stop();
                        Audio.Stop();
                        //fLPPModes.Visible = false;
                        fLPPModes.Enabled = false;
                        btnCancel.Enabled = false;
                        displayMessageLine(Utilities.MessageUtils.getMessage("Credit Card Payment.") + " " + Utilities.MessageUtils.getMessage("Please wait..."), WARNING);
                        Application.DoEvents();
                        if (paymentModeDTO.GatewayLookUp.Equals(PaymentGateways.NCR.ToString()))//starts: Modification on 2016-06-13 For adding NCR gateway
                        {
                            try
                            {
                                PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentModeDTO.GatewayLookUp);
                                paymentGateway.BeginOrder();
                            }
                            catch (Exception ex)
                            {
                                log.Error("Payment Gateway error :" + ex.Message);
                            }
                            //KioskStatic.NCRAdapter.BeginOrder();
                        }
                        if (paymentModeDTO.IsQRCode.Equals("Y"))
                        {
                            using (frmScanCoupon scanCoupon = new frmScanCoupon("WeChat", "Scan QRCode", "QR Code", "", ""))
                            {
                                scanCoupon.ShowDialog();
                                if (!string.IsNullOrEmpty(scanCoupon.CodeScaned))
                                {
                                    kioskTransaction.QRCodeScanned = scanCoupon.CodeScaned;
                                }
                                else
                                {
                                    kioskTransaction.QRCodeScanned = "";
                                    throw new Exception(KioskStatic.Utilities.MessageUtils.getMessage(1552));
                                }
                                //scanCoupon.Dispose();
                            }
                        }
                        else
                        {
                            kioskTransaction.QRCodeScanned = "";
                        }
                        kioskTransaction.CreditCardPayment((sender as Control).Equals(paymentModeDTO.IsCreditCard) ? true : false);
                        totSecs = 0;
                        //TimerMoney.Start();
                        checkMoneyStatus();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        fLPPModes.Visible = true;
                        string errMsg = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4650);
                        using (frmOKMsg frm = new frmOKMsg(ex.Message + Environment.NewLine + errMsg, true))
                        {
                            frm.ShowDialog();
                        }
                        displayMessageLine(errMsg, ERROR);
                        KioskStatic.logToFile("btnCreditCard_Click method" + errMsg + ex.Message);
                        KioskStatic.logToFile("btnCreditCard_Click method" + ex.StackTrace);
                        Close();
                        log.LogMethodExit();
                    }
                    finally
                    {
                        btnCancel.Enabled = true;
                        this.Focus();
                    }

                }
                else if (paymentModeDTO.IsDebitCard == true)
                {
                    if (AmountRequired - ac.totalValue <= 0)
                    {
                        displayMessageLine("Nothing to pay for", WARNING);
                        log.LogMethodExit("Nothing to pay for");
                        return;
                    }

                    KioskStatic.logToFile("Debit card clicked");
                    log.Info("Debit card clicked");
                    try
                    {
                        TimerMoney.Stop();
                        Audio.Stop();
                        //btnDebitCard.Enabled =
                        //btnCreditCard.Enabled = false;
                        displayMessageLine(Utilities.MessageUtils.getMessage("Game Card Payment.") + " " + Utilities.MessageUtils.getMessage("Please wait..."), WARNING);
                        Application.DoEvents();
                        //if (btnDebitCard.Tag != null)
                        {
                            //TransactionPaymentsDTO trxPaymentDTO = (TransactionPaymentsDTO)btnDebitCard.Tag;
                            if (trxPaymentDTO.PaymentModeId != -1)
                            {
                                if ((sender as Control).Equals(btn) ? true : false)
                                {
                                    kioskTransaction.GameCardPayment(trxPaymentDTO);
                                    totSecs = 0;
                                    TimerMoney.Start();
                                    checkMoneyStatus();
                                }
                            }
                        }

                    }

                    catch (Exception ex)
                    {
                        log.Error(ex);
                        fLPPModes.Visible = true;
                        fLPPModes.Enabled = true;
                        displayMessageLine(ex.Message, ERROR);
                        KioskStatic.logToFile("btnDebitCard_Click method" + ex.Message);
                        KioskStatic.logToFile("btnDebitCard_Click method" + ex.StackTrace);
                        MessageBox.Show("Error : " + ex.Message + "\n" + ex.StackTrace);
                    }

                }
                else
                {
                    checkMoneyStatus();
                }
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
                    KioskStatic.logToFile("Bill Acceptor note received: " + billMessage);
                    log.Info("Bill Acceptor note received: " + billMessage);
                    log.LogVariableState("Ac", ac);
                    KioskStatic.updateKioskActivityLog(billAcceptor.ReceivedNoteDenomination, -1, (CurrentCard == null ? "" : CurrentCard.CardNumber), "BILL-IN", "Bill Inserted", ac);
                    billAcceptor.ReceivedNoteDenomination = 0;

                    System.Threading.Thread.Sleep(300);
                    billAcceptor.requestStatus();
                    canPerformNoteCoinReceivedAction = true;
                    KioskStatic.logToFile("IsHandleCreated: " + this.IsHandleCreated.ToString());
                    KioskStatic.logToFile("Set canPerformNoteCoinReceivedAction as true");
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
                KioskStatic.logToFile("Coin Acceptor: " + coinMessage);
                log.Info("Coin Acceptor: " + coinMessage);
                if (coinRecd)
                {
                    totSecs = 0;
                    log.LogVariableState("Ac", ac);
                    KioskStatic.updateKioskActivityLog(-1, coinAcceptor.ReceivedCoinDenomination, (CurrentCard == null ? "" : CurrentCard.CardNumber), "COIN-IN", "Coin Inserted", ac);
                    coinAcceptor.ReceivedCoinDenomination = 0;
                    System.Threading.Thread.Sleep(20);
                    canPerformNoteCoinReceivedAction = true;
                    KioskStatic.logToFile("IsHandleCreated: " + this.IsHandleCreated.ToString());
                    KioskStatic.logToFile("Set canPerformNoteCoinReceivedAction as true");
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

        void RefreshTotals()
        {
            log.LogMethodEntry();
            try
            {
                lblPaid.Text = (ac != null && ac.totalValue != 0 ? ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)
                                                             : (0).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));


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
                    if (billAcceptorTimeout > 30)
                    {
                        billMessage = MessageUtils.getMessage(423);
                        KioskStatic.logToFile("Bill acceptor timeout / offline");
                        log.Error("Bill acceptor timeout / offline");
                    }

                    if (billAcceptorTimeout % 5 == 0)
                    {
                        billAcceptor.requestStatus();
                        if (billAcceptor.Working == false)
                        { restartValidators(); }
                    }
                }

                if (!KioskStatic.coinAcceptorDatareceived && coinAcceptor != null && KioskStatic.caReceiveAction != null)
                {
                    coinAcceptorTimeout++;
                    if (coinAcceptorTimeout > 30 && KioskStatic.spCoinAcceptor.IsOpen)
                    {
                        coinMessage = MessageUtils.getMessage(424);
                        KioskStatic.logToFile("Coin acceptor timeout / offline");
                        log.Error("Coin acceptor timeout / offline");
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
                                KioskStatic.logToFile("billAcceptor stillProcessing. reset timer clock");
                                log.Info("billAcceptor stillProcessing. reset timer cock");
                                TimerMoney.Start();
                                log.LogMethodExit();
                                return;
                            }

                            if (billAcceptor != null)
                                billAcceptor.disableBillAcceptor();
                            if (coinAcceptor != null)
                                coinAcceptor.disableCoinAcceptor();
                            fLPPModes.Enabled = false;

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
                            btnCancel.Enabled = false;
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
                        KioskStatic.logToFile("Timer Money Tick: ac.totalValue <= 0 is false, restartValidators");
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
                        if (ServerDateTime.Now.Millisecond / 500 > 0)
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
                                if (Function == "I")
                                    displayMessageLine(MessageUtils.getMessage(419, AmountRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), MESSAGE);
                                else
                                    displayMessageLine(MessageUtils.getMessage(420, AmountRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), MESSAGE);
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
                                if (Function == "I")
                                    displayMessageLine(MessageUtils.getMessage(419, AmountRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), MESSAGE);
                                else
                                    displayMessageLine(MessageUtils.getMessage(420, AmountRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), MESSAGE);
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
                        KioskStatic.logToFile("checkMoneyStatus() - ac.totalValue = " + ac.totalValue.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) + ", AmountRequired = " + ProductPrice.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT));
                        log.Info("Ac.totalValue = " + ac.totalValue.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) + ", AmountRequired = " + ProductPrice.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT));
                        TimerMoney.Stop();

                        if (kioskTransaction.cancelPressed)
                        {
                            KioskStatic.logToFile("checkMoneyStatus() - Cancel button Pressed");
                            log.Info("Cancel button Pressed");
                            log.LogMethodExit();
                            return;
                        }

                        btnCancel.Enabled = false;
                        Application.DoEvents();

                        KioskStatic.logToFile("Valid amount inserted: " + ac.totalValue.ToString(amountFormat) + "; " + "Required: " + ProductPrice.ToString(amountFormat));
                        log.Info("Valid amount inserted: " + ac.totalValue.ToString(amountFormat) + "; " + "Required: " + ProductPrice.ToString(amountFormat));

                        if (billAcceptor != null)
                        {
                            KioskStatic.logToFile("checkMoneyStatus() - disabling billAcceptor");
                            log.Info("Disabling billAcceptor");
                            System.Threading.Thread.Sleep(300);
                            billAcceptor.disableBillAcceptor();
                        }
                        if (coinAcceptor != null)
                        {
                            KioskStatic.logToFile("checkMoneyStatus() - disabling billAcceptor");
                            log.Info("Disabling billAcceptor");
                            System.Threading.Thread.Sleep(300);
                            coinAcceptor.disableCoinAcceptor();
                        }

                        if (Function == "I")
                        {
                            KioskStatic.logToFile("checkMoneyStatus() - Inside Fuction=I");
                            log.Info("Inside Fuction=I");
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
                            KioskStatic.logToFile("checkMoneyStatus() - Before calling rechargeCard()");
                            log.Info("Before calling rechargeCard()");
                            displayMessageLine(MessageUtils.getMessage(503), MESSAGE);
                            Application.DoEvents();
                            kioskTransaction.rechargeCard();
                            KioskStatic.logToFile("checkMoneyStatus() - After rechargeCard() process");
                            log.Info("After rechargeCard() process");
                        }
                        AmountRequired = 0;
                    }
                    else
                    {
                        KioskStatic.logToFile("checkMoneyStatus() - TotalValue greater than or equls to AmountRequired condition failed- ac.totalValue = " + ac.totalValue.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) + ", AmountRequired = " + ProductPrice.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT));
                        log.Info("TotalValue greater than or equls to AmountRequired condition failed- ac.totalValue = " + ac.totalValue.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) + ", AmountRequired = " + ProductPrice.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    displayMessageLine(ex.Message, ERROR);
                    KioskStatic.logToFile("checkMoneyStatus():" + ex.Message + "-" + ex.StackTrace);
                    btnCancel.Enabled = true;
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
                try
                {
                    handleCardRead(CardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
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
            if (_PaymentModeDTO != null && _PaymentModeDTO.IsCreditCard != true)
            {
                if ((AmountRequired - ac.totalValue) > 0)
                {
                    if (billAcceptor != null)
                    {
                        if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB))
                        {
                            KioskStatic.logToFile("RestartValidators - DisableBillAcceptor");
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
                else
                {
                    KioskStatic.logToFile("RestartValidators - skip restart, balance amount is 0");
                }
            }

            //btnDebitCard.Enabled = btnCreditCard.Enabled = true;
            if (ac.totalValue > 0 && ac.totalValue < AmountRequired)
            {
                fLPPModes.Enabled = false;
            }
            else
            {
                fLPPModes.Enabled = true;
            }
            btnCancel.Enabled = true;

            TimerMoney.Start();
            log.LogMethodExit();
        }

        int cancelPressCount = 0;
        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                kioskTransaction.cancelPressed = true;
                btnCancel.Enabled = false;
                fLPPModes.Enabled = false;
                //btnDebitCard.Enabled = btnCreditCard.Enabled = false;

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
                    btnCancel.Enabled = true;
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4682), ERROR);
                    log.Info("billAcceptor.StillProcessing");
                    KioskStatic.logToFile("billAcceptor.StillProcessing");
                    log.LogMethodExit();
                    return;
                }
                KioskStatic.logToFile("Cancel button Disabling bill acceptor ");

                if (billAcceptor != null)
                    billAcceptor.disableBillAcceptor();
                if (coinAcceptor != null)
                    coinAcceptor.disableCoinAcceptor();

                // btnDebitCard.Enabled = btnCreditCard.Enabled = false;

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
                                message = MessageUtils.getMessage(934, (ac.totalValue - (decimal)kioskTransaction.CCSurchargeAmount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                f = new frmYesNo(message);
                                DialogResult dr = f.ShowDialog();
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
                                                                            new SqlParameter("@amount", (ac.totalValue - (decimal)kioskTransaction.CCSurchargeAmount)));

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
                                        if (Math.Ceiling((ac.totalValue - (decimal)kioskTransaction.CCSurchargeAmount) / CardCount) < ProductPrice)
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
                                        rowProduct["Price"] = kioskTransaction.ProductPrice = AmountRequired = ac.productPrice = ProductPrice = (ac.totalValue - (decimal)kioskTransaction.CCSurchargeAmount); //added KioskTransaction.ProductPrice
                                    else
                                    {
                                        ProductPrice = (ac.totalValue - (decimal)kioskTransaction.CCSurchargeAmount) / CardCount;
                                        rowProduct["Price"] = kioskTransaction.ProductPrice = ProductPrice; //added KioskTransaction.ProductPrice
                                        AmountRequired = ac.productPrice = (ac.totalValue - (decimal)kioskTransaction.CCSurchargeAmount);
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
                            KioskStatic.logToFile("Cancel button, variable new card option not selected, restartValidators");
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
                                        KioskStatic.logToFile("Cancel button, Decided not to exit, restartValidators");
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
            KioskStatic.logToFile("TImer click - still processing check ");
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
            using (frmThankYou frm = new frmThankYou(receiptPrinted))
            {
                frm.ShowDialog();
            }
            KioskStatic.logToFile("Exit money screen");
            Close();
            log.LogMethodEntry("Exit money screen");
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



        private void frmCardTransaction_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Card Transaction form closed event");
            if (_PaymentModeDTO != null && _PaymentModeDTO.GatewayLookUp.Equals(PaymentGateways.NCR.ToString()))
            {
                try
                {
                    PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(_PaymentModeDTO.GatewayLookUp);
                    paymentGateway.EndOrder();
                }
                catch (Exception ex)
                {
                    log.Error("Payment Gateway error :" + ex.Message);
                }
            }
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
            selectedEntitlementType = "";
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

        private void BuildToolTipForlblQuantity(string message)
        {
            log.LogMethodEntry(message);
            ToolTip msgToolTip = new ToolTip();
            msgToolTip.SetToolTip(lblQuantity, message);
            log.LogMethodExit();
        }

        private void lblQuantity_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ToolTip msgToolTip = new ToolTip();
            msgToolTip.Show(lblQuantity.Text, lblQuantity);
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void lblProductCPValidityMsg_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ToolTip msgToolTip = new ToolTip();
            msgToolTip.Show(lblProductCPValidityMsg.Text, lblProductCPValidityMsg);
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                foreach (Control c in panelSummary.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CardTransactionSummaryHeaderTextForeColor;//Summary Panel header
                    }
                }
                foreach (Control c in fLPPModes.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CardTransactionPaymentButtonsTextForeColor;//Summary Panel header
                    }
                }
                foreach (Control c in panelDeposit.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CardTransactionDepositeHeaderTextForeColor;//Deposite panel header
                    }
                }
                foreach (Control c in panelDiscount.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CardTransactionDiscountHeaderTextForeColor;//Discount panel header
                    }
                }
                foreach (Control c in panelFund.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CardTransactionFundHeaderTextForeColor;//Fund panel header
                    }
                }
                foreach (Control c in panelDonation.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CardTransactionDonationHeaderTextForeColor;//Donation panel header
                    }
                }
                foreach (Control c in flowLayoutPanel4.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CardTransactionSummaryInfoTextForeColor;// Summary Info 
                    }
                }
                foreach (Control c in flpDepositLine.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CardTransactionDepositeInfoTextForeColor;// Deposite Info
                    }
                }
                foreach (Control c in panelDiscountInfo.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CardTransactionDiscountInfoTextForeColor;// Discount Info 
                    }
                }
                foreach (Control c in panelFundInfo.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CardTransactionFundInfoTextForeColor;// Fund Raiser Info 
                    }
                }
                foreach (Control c in panelDonationInfo.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CardTransactionDonationInfoTextForeColor;// Donation Info 
                    }
                }
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.CardTransactionCancelButtonTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.CardTransactionFooterTextForeColor;
                this.Label1.ForeColor = KioskStatic.CurrentTheme.PaymentModeTotalToPayHeaderTextForeColor;//Total to pay header label
                this.lblProductCPValidityMsg.ForeColor = KioskStatic.CurrentTheme.CardTransactionCPVlaiditTextForeColor;//product cp validity
                this.lblTotalToPay.ForeColor = KioskStatic.CurrentTheme.CardTransactionTotalToPayInfoTextForeColor;//Total to pay info label
                this.Label8.ForeColor = KioskStatic.CurrentTheme.CardTransactionAmountPaidHeaderTextForeColor;//Total to pay info label
                this.lblPaid.ForeColor = KioskStatic.CurrentTheme.CardTransactionAmountPaidInfoTextForeColor;//Total to pay info label
                this.Label9.ForeColor = KioskStatic.CurrentTheme.CardTransactionBalanceToPayHeaderTextForeColor;//Total to pay info label
                this.lblBal.ForeColor = KioskStatic.CurrentTheme.CardTransactionBalanceToPayInfoTextForeColor;//Total to pay info label
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void UpdateFundDonationPanelVisibility()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Updating Fund or Donation Panel visibility");
            panelFund.Visible = false;
            panelDonation.Visible = false;
            if ((selectedFundsAndDonationsList != null) && selectedFundsAndDonationsList.Any() == true)
            {
                for (int i = 0; i < selectedFundsAndDonationsList.Count; i++)
                {
                    if (selectedFundsAndDonationsList[i].Key == KioskStatic.FUND_RAISER_LOOKUP_VALUE)
                    {
                        panelFund.Visible = true;
                    } 
                    else if (selectedFundsAndDonationsList[i].Key == KioskStatic.DONATIONS_LOOKUP_VALUE)
                    {
                        panelDonation.Visible = true;
                    } 
                }
            }
            log.LogMethodExit();
        }

        private void UpdateFundDonationPanelText(Decimal totalCardDeposit)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Updating Fund or Donation Panel Text");
            if ((selectedFundsAndDonationsList != null) && selectedFundsAndDonationsList.Any())
            {
                for (int i = 0; i < selectedFundsAndDonationsList.Count; i++)
                {
                    if (selectedFundsAndDonationsList[i].Key == KioskStatic.FUND_RAISER_LOOKUP_VALUE)
                    {
                        panelFund.Visible = true;
                        txtFundName.Text = selectedFundsAndDonationsList[i].Value.ProductName;
                    }
                    else if (selectedFundsAndDonationsList[i].Key == KioskStatic.DONATIONS_LOOKUP_VALUE)
                    {
                        panelDonation.Visible = true;
                        txtDonationName.Text = selectedFundsAndDonationsList[i].Value.ProductName;
                        txtDonationAmount.Text = selectedFundsAndDonationsList[i].Value.Price.ToString(ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "AMOUNT_FORMAT"));
                        if (currentTrx != null)
                        {
                            txtDonationTax.Text = currentTrx.TrxLines.Where(t => t.CategoryId == selectedFundsAndDonationsList[i].Value.CategoryId).FirstOrDefault().tax_amount.ToString(ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "AMOUNT_FORMAT"));
                            double donationLineAmount = currentTrx.TrxLines.Where(t => t.CategoryId == selectedFundsAndDonationsList[i].Value.CategoryId).FirstOrDefault().LineAmount;
                            lblTotal.Text = (AmountRequired - totalCardDeposit - Convert.ToDecimal(donationLineAmount)).ToString(ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "AMOUNT_FORMAT"));
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
