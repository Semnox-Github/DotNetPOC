/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmPaymentGameCard 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 *******************************************************************************************/
using iTextSharp.text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmPaymentGameCard : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private decimal AmountRequired;

        private PaymentModeDTO _paymentModeDTO = new PaymentModeDTO();

        private double creditPlusAmount = 0;
        private double usedCredits = 0;
        private double usedAmount = 0;
        private double usedCreditPlus = 0;
        private double balance = 0; 
        private Semnox.Core.Utilities.Utilities utilities = KioskStatic.Utilities;

        private readonly TagNumberParser tagNumberParser;
        public string cardNumber;
        public Card Card;
        public TransactionPaymentsDTO debitTrxPaymentDTO = null; 
        private int availableFreeEntries = 0;
        private bool applyCardCreditPlusConsumption;
        private bool hasCheckinProducts = false;
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        public frmPaymentGameCard(KioskTransaction kioskTransactionIn, PaymentModeDTO selectedPaymentModeDTO)
        {
            log.LogMethodEntry("kioskTransactionIn", selectedPaymentModeDTO); 
            this.kioskTransaction = kioskTransactionIn; 
            //KioskStatic.setDefaultFont(this);
            AmountRequired = (decimal)kioskTransaction.GetTrxNetAmount();
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            _paymentModeDTO = selectedPaymentModeDTO;
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            lblPaymentText.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;
            hasCheckinProducts = kioskTransaction.HasCheckinProducts();
            DisplaybtnCancel(true);
            this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.PaymentGameCardBackgroundImage);
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            panel1.BackgroundImage = ThemeManager.CurrentThemeImages.PurchaseSummaryTableImage;
            btnApply.BackgroundImage = btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            SetCustomizedFontColors();
            if (hasCheckinProducts == false)
            {
                DisplayCardCPConsumptionDetails(false);
            }
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmPaymentGameCard_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            btnApply.Enabled = false;
            lblTotalToPay.Text = AmountRequired.ToString(ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL"));
            btnApply.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Apply");
            btnCancel.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Cancel Payment");
            lblAvailableCreditsText.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Available Credits") + ":";
            lblBalanceCreditsText.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Balance Credits") + ":";
            lblPurchaseValueText.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Total To Pay") + ":";
            lblCardNumberText.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Card Number") + ":";
            lblAvailableFreeEntriesText.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Available Free Entries") + ":";
            lblBalanceFreeEntriesText.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Balance Free Entries") + ":";
            lblTotaltoPayText.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Total to Pay");
            lblPaymentText.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Payment - Gamecard");
            lblTapCardText.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Tap card to pay for the purchase");

            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.TopUpReaderDevice != null)
            {
                if (KioskStatic.cardAcceptor != null)
                    KioskStatic.cardAcceptor.AllowAllCards();
            }
            log.LogVariableState("KioskStatic.TopUpReaderDevice", KioskStatic.TopUpReaderDevice);
            if (KioskStatic.TopUpReaderDevice != null)
            {
                log.Info("Doing KioskStatic.TopUpReaderDevice.Register");
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));
            }
            StartKioskTimer();
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining == 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                    tickSecondsRemaining = 0;
            }
            if (tickSecondsRemaining <= 0)
            {
                Application.DoEvents();
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                base.CloseForms();
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
                        txtMessage.Text = ex.Message;
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, KioskStatic.Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        txtMessage.Text = ex.Message;
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        txtMessage.Text = ex.Message;
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    txtMessage.Text = message;
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string lclCardNumber = tagNumber.Value;
                lclCardNumber = KioskStatic.ReverseTopupCardNumber(lclCardNumber);
                try
                {
                    handleCardDetails(lclCardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    txtMessage.Text = ex.Message;
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        void handleCardDetails(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            cardNumber = inCardNumber;
            ResetKioskTimer();
            KioskStatic.logToFile("Tapped card " + cardNumber);
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
            {
                try
                {
                    Card = new MifareCard(KioskStatic.TopUpReaderDevice, cardNumber, "External POS", KioskStatic.Utilities);
                }
                catch (Exception ex)
                {
                    if (KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.cardAcceptor.EjectCardFront();
                        txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 528);
                        log.Info(txtMessage.Text);
                        log.LogMethodExit();
                        return;
                    }
                    log.Error("Error occurred while executing handleCardRead()" + ex.Message);
                }
            }
            else
            {
                Card = new Card(readerDevice, cardNumber, "External POS", KioskStatic.Utilities);

                string message = "";
                txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1607);//Refreshing Card from HQ. Please Wait...
                Application.DoEvents();
                if (!KioskHelper.refreshCardFromHQ(ref Card, ref message))
                {
                    txtMessage.Text = message;
                    Card = null;
                    log.LogMethodExit();
                    return;
                }
            }

            log.LogVariableState("Card.CardStatus", Card.CardStatus);
            if (Card.CardStatus == "NEW")
            {
                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                    txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 528);
                else
                    txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 459);

                Card = null;
                cardNumber = "";
                refreshLabels();
                this.btnApply.Enabled = false;
                ResetKioskTimer();
                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                }

                KioskStatic.logToFile("NEW card tapped. Rejected.");
                log.Info("NEW card tapped. Rejected.");
            }
            else if (Card.technician_card.Equals('N') == false)
            {
                txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 197, Card.CardNumber);
                Card = null;
                cardNumber = "";
                refreshLabels();
                this.btnApply.Enabled = false;
                ResetKioskTimer();
            }
            else
            {
                double credits = Card.credits;
                creditPlusAmount = 0;
                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                    creditPlusAmount = Card.CreditPlusCardBalance;
                else
                {
                    CreditPlus creditPlus = new CreditPlus(utilities);
                    creditPlusAmount = creditPlus.getCreditPlusForPOS(Card.card_id, utilities.ParafaitEnv.POSTypeId, kioskTransaction.GetTransactionId);//GGG
                }
                txtMessage.Text = "";
                if (ValidateGameCards() != 0)
                {
                    txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 183);
                }
            }
            log.LogMethodExit();
        }

        double ValidateGameCards()
        {
            log.LogMethodEntry();
            double credits = 0;
            double localGameCardAmount = (double)AmountRequired;
            double creditAmountAvailable = 0;
            double creditPlusAvailable = 0;
            double balanceToPay = 0;
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines = kioskTransaction.GetActiveTransactionLines;
            int usedFreeEntriesFromMemberCard = (trxLines == null) ? 0 : trxLines.FindAll(t => t.CreditPlusConsumptionId > -1).Count;

            creditPlusAvailable = Math.Round(Convert.ToDouble(creditPlusAmount), utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
            credits = Math.Round(Convert.ToDouble(Card.credits), utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
            creditAmountAvailable = credits;
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            string amountFormat = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "AMOUNT_FORMAT");

            if (hasCheckinProducts)
            {
                availableFreeEntries = CheckCreditPlusBalance();

                int usedFreeEntries = 0;
                if (availableFreeEntries > 0)
                {
                    applyCardCreditPlusConsumption = ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_APPLY_CARD_CREDITPLUS_CONSUMPTION", false);
                    if (!applyCardCreditPlusConsumption)
                    {
                        Card rechargeCard = kioskTransaction.GetTransactionPrimaryCard;
                        if (usedFreeEntriesFromMemberCard > 0 && cardNumber == rechargeCard.CardNumber)
                        {
                            availableFreeEntries = availableFreeEntries - usedFreeEntriesFromMemberCard;
                            log.Debug("available Free Entries : " + availableFreeEntries);
                        }
                        if (availableFreeEntries > 0)
                        {
                            int screenTimeout = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault<int>(utilities.ExecutionContext, "BALANCE_SCREEN_TIMEOUT", 30); //30 seconds
                            string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, 4343, availableFreeEntries); //You have &1 voucher free of charge. Would you like to use for this purchase?
                            using (frmYesNo frmYesNo = new frmYesNo(msg, string.Empty, screenTimeout))
                            {
                                DialogResult dr = frmYesNo.ShowDialog();
                                if (dr == DialogResult.Yes)
                                {
                                    applyCardCreditPlusConsumption = true;

                                    int payableQty = 0;
                                    foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in trxLines)
                                    {
                                        if (tl.LineAmount > 0)
                                        {
                                            bool isEligible = IsEligibleToApplyVoucher(tl);
                                            if (isEligible)
                                            {
                                                CreditPlus creditPlus = new CreditPlus(utilities);
                                                localGameCardAmount -= (tl.Price + tl.tax_amount);
                                                payableQty++;

                                                if (payableQty == availableFreeEntries)
                                                    break;
                                            }
                                        }
                                    }
                                    usedFreeEntries = payableQty;
                                }
                            }
                        }
                    }
                    else
                    {
                        int nonPayableQty = 0;
                        foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in trxLines)
                        {
                            if (tl.LineAmount > 0 && IsEligibleToApplyVoucher(tl))
                            {
                                localGameCardAmount -= (tl.Price + tl.tax_amount);
                                nonPayableQty++;
                            }
                        }
                        usedFreeEntries = nonPayableQty;
                    }
                }

                lblAvailableFreeEntries.Text = availableFreeEntries.ToString();
                int remainingFreeEntries = availableFreeEntries - usedFreeEntries;
                if (remainingFreeEntries < 0)
                {
                    remainingFreeEntries = 0;
                }
                lblBalanceFreeEntries.Text = remainingFreeEntries.ToString();
                balanceToPay = localGameCardAmount;

                double availableCredits = creditPlusAvailable + creditAmountAvailable;
                if (availableCredits < balanceToPay)
                {
                    txtMessage.Text = "";
                    refreshLabels();
                    if (localGameCardAmount != 0)
                    {
                        this.btnApply.Enabled = false;
                        string errorMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 183);
                        txtMessage.Text = errorMsg;
                        frmOKMsg.ShowUserMessage(errorMsg);
                        string errMsg = "Ends-ValidateGameCards() as Insufficient Credits on Game Card(s)";
                        log.Debug(errMsg);
                        KioskStatic.logToFile(errMsg);
                        return localGameCardAmount;
                    }
                }
            }

            if ((creditPlusAvailable + creditAmountAvailable) >= localGameCardAmount)
            {
                if (localGameCardAmount > creditPlusAvailable)
                {
                    usedCreditPlus = creditPlusAvailable;
                    localGameCardAmount -= creditPlusAvailable;

                    if (localGameCardAmount > creditAmountAvailable)
                    {
                        balance = 0;
                        lblBalanceCredits.Text = Convert.ToString(0);
                        usedCredits = credits;
                        usedAmount = creditAmountAvailable + creditPlusAvailable;
                        localGameCardAmount -= creditAmountAvailable;
                    }
                    else
                    {
                        double usedCreditsValue = localGameCardAmount;
                        balance = (credits - usedCreditsValue);
                        lblBalanceCredits.Text = Convert.ToString(credits - usedCreditsValue);
                        usedCredits = usedCreditsValue;
                        usedAmount = usedCreditsValue + creditPlusAvailable;
                        localGameCardAmount = 0;
                    }
                }
                else
                {
                    if (localGameCardAmount >= 0)
                    {
                        usedCreditPlus = localGameCardAmount;
                        usedCredits = 0;
                        lblPurchaseValue.Text = Convert.ToString(localGameCardAmount);
                    }
                    else
                    {
                        usedCreditPlus = 0;
                        usedCredits = localGameCardAmount;
                    }

                    localGameCardAmount = 0;
                }

                double balanceCredits = hasCheckinProducts ? (credits + creditPlusAvailable) - balanceToPay
                                            : ((credits + creditPlusAvailable) - (double)AmountRequired);
                lblBalanceCredits.Text = balanceCredits.ToString(amountFormat);
                lblPurchaseValue.Text = AmountRequired.ToString(amountFormat);
                lblCardNumber.Text = KioskHelper.GetMaskedCardNumber(cardNumber);
                lblAvailableCredits.Text = (creditPlusAvailable + creditAmountAvailable).ToString(amountFormat);

                if (hasCheckinProducts)
                {
                    lblPurchaseValue.Text = balanceToPay.ToString(amountFormat);
                    lblTotalToPay.Text = balanceToPay.ToString(ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL"));
                }
                this.btnApply.Enabled = true;
            }
            else
            {

                txtMessage.Text = "";
                refreshLabels();
                if (localGameCardAmount != 0)
                {
                    this.btnApply.Enabled = false;
                    string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 183); //Insufficient Credits on Game Card(s)
                    txtMessage.Text = errMsg;
                    frmOKMsg.ShowUserMessage(errMsg);
                    log.Debug("Ends-ValidateGameCards() as Insufficient Credits on Game Card(s)");
                    return localGameCardAmount;
                }
            }
            log.LogMethodExit("Ends-ValidateGameCards()");
            return 0;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                StopKioskTimer();
                if (hasCheckinProducts && applyCardCreditPlusConsumption == true)
                {
                    try
                    {
                        kioskTransaction.ApplyCardVoucher(Card);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5102); //Card Voucher can not be applied
                        ValidationException validationException = new ValidationException(errMsg + " :" + ex.Message);
                        KioskStatic.logToFile("Card Voucher is NOT applied. " + ex.Message);
                        throw validationException;
                    }
                }
                debitTrxPaymentDTO = new TransactionPaymentsDTO();
                debitTrxPaymentDTO.PaymentModeId = _paymentModeDTO.PaymentModeId;
                debitTrxPaymentDTO.paymentModeDTO = _paymentModeDTO;
                debitTrxPaymentDTO.Amount = usedCredits;
                debitTrxPaymentDTO.CardId = Card.card_id;
                debitTrxPaymentDTO.CardEntitlementType = KioskTransaction.GETCHECKINCHECKOUTTYPE;
                debitTrxPaymentDTO.PaymentUsedCreditPlus = usedCreditPlus;
                debitTrxPaymentDTO.PaymentCardNumber = Card.CardNumber;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            this.Close();
            log.LogMethodExit();
        }

        private void refreshLabels()
        {
            log.LogMethodEntry();
            lblBalanceCredits.Text = "";
            lblPurchaseValue.Text = "";
            lblCardNumber.Text = "";
            lblAvailableFreeEntries.Text = "";
            lblBalanceFreeEntries.Text = "";
            lblAvailableCredits.Text = "";
            log.LogMethodExit();
        }

        private void frmPaymentGameCard_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Audio.Stop();

            log.LogVariableState("KioskStatic.TopUpReaderDevice", KioskStatic.TopUpReaderDevice);
            if (KioskStatic.TopUpReaderDevice != null)
            {
                log.Info("Doing KioskStatic.TopUpReaderDevice.UnRegister");
                KioskStatic.TopUpReaderDevice.UnRegister();
            }
            if (ActiveControl.Name != "btnHome")
            {
                if (debitTrxPaymentDTO == null)
                {
                    _paymentModeDTO = null;
                }
                if (DialogResult != DialogResult.Cancel)
                {
                    using (frmCardTransaction frms = new frmCardTransaction(kioskTransaction, _paymentModeDTO))
                    {
                        frms.trxPaymentDTO = debitTrxPaymentDTO;
                        DialogResult dr = frms.ShowDialog();
                    }
                }
            }
            log.LogVariableState("KioskStatic.TopUpReaderDevice", KioskStatic.TopUpReaderDevice);
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmPaymentGameCard");
            try
            {
                this.lblPaymentText.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblPaymentTextTextForeColor;//Total to pay header label
                this.lblTotaltoPayText.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblTotaltoPayTextTextForeColor;//product cp validity
                this.lblTotalToPay.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblTotalToPayTextForeColor;//Total to pay info label
                this.label8.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLabel8TextForeColor;//Total to pay info label
                this.label7.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLabel7TextForeColor;//Total to pay info label
                this.lblTapCardText.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblTapCardTextTextForeColor;//Total to pay info label
                this.lblCardNumberText.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblCardNumberTextTextForeColor;//Total to pay info label
                this.lblCardNumber.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblCardNumberTextForeColor;//Total to pay info label
                this.lblAvailableCreditsText.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblAvailableCreditsTextTextForeColor;//Total to pay info label
                this.lblAvailableCredits.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblAvailableCreditsTextForeColor;//Total to pay info label
                this.lblAvailableFreeEntriesText.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblAvailableFreeEntriesTextTextForeColor;//lbl Available Free Entries info label
                this.lblAvailableFreeEntries.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblAvailableFreeEntriesTextForeColor;////Available Free Entries info label
                this.lblPurchaseValueText.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblPurchaseValueTextTextForeColor;//Total to pay info label
                this.lblPurchaseValue.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblPurchaseValueTextForeColor;//Total to pay info label
                this.lblBalanceCreditsText.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblBalanceCreditsTextTextForeColor;//Total to pay info label
                this.lblBalanceCredits.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblBalanceCreditsTextForeColor;//Total to pay info label
                this.lblBalanceFreeEntriesText.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblBalanceFreeEntriesTextTextForeColor;//Balance Free Entries Text info label
                this.lblBalanceFreeEntries.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardLblBalanceFreeEntriesTextForeColor;//Balance Free Entries Text info label
                this.btnApply.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardBtnApplyTextForeColor;//Total to pay info label
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardBtnCancelTextForeColor;//Total to pay info label
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardTxtMessageTextForeColor;//Total to pay info label
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.PaymentGameCardBtnHomeTextForeColor;//Total to pay info label
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmPaymentGameCard: " + ex.Message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Checks for Passport Voucher
        /// </summary>
        /// <returns></returns>
        private int CheckCreditPlusBalance()
        {
            log.LogMethodEntry();
            int consumptionBalance = 0;

            try
            {
                AccountBL accountBL = new AccountBL(KioskStatic.Utilities.ExecutionContext, Card.card_id);
                if (accountBL != null)
                {
                    List<ProductsContainerDTO> productsContainerDTOList = new List<ProductsContainerDTO>();
                    HashSet<int> trxProductIds = new HashSet<int>(kioskTransaction.GetActiveTransactionLines.Select(t => t.ProductID).ToList().Distinct());
                    if (trxProductIds != null && trxProductIds.Any())
                    {
                        foreach (int productId in trxProductIds)
                        {
                            ProductsContainerDTO selectedProductContainerDTO = ProductsContainerList.GetProductsContainerDTO(utilities.ExecutionContext.SiteId, productId);
                            productsContainerDTOList.Add(selectedProductContainerDTO);
                        }
                        consumptionBalance = accountBL.GetApplicableCardCPConsumptionsBalance(productsContainerDTOList);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while fetching CheckCreditPlusBalance(): " + ex.Message);
            }
            log.LogMethodExit(consumptionBalance);
            return consumptionBalance;
        }

        private bool IsEligibleToApplyVoucher(Semnox.Parafait.Transaction.Transaction.TransactionLine tl)
        {
            log.LogMethodEntry(tl);
            bool isEligible = false;

            try
            {
                AccountBL accountBL = new AccountBL(utilities.ExecutionContext, Card.card_id);
                if (accountBL != null && accountBL.AccountDTO != null)
                {
                    isEligible = accountBL.IsEligibleToApplyCardCPConsumptionBalance(tl.CategoryId, tl.ProductID);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in IsEligibleToApplyVoucher(): " + ex.Message);
            }
            log.LogMethodExit(isEligible);
            return isEligible;
        }

        private void DisplayCardCPConsumptionDetails(bool switchValue)
        {
            log.LogMethodEntry(switchValue);
            lblAvailableFreeEntriesText.Visible = switchValue;
            lblBalanceFreeEntriesText.Visible = switchValue;
            lblAvailableFreeEntries.Visible = switchValue;
            lblBalanceFreeEntries.Visible = switchValue;
            log.LogMethodExit();
        }
    }
}






















