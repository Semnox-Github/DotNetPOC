/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmPaymentGameCard 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 *2.90.0     12-June-2020     Dakshakh Raj       Created: As part of Payment Modes based on 
 *                                                        Kiosk Configuration set up enhancement
*2.140.0     22-Oct-2021      Sathyavathi        CEC enhancement - Fund Raiser and Donations
*2.150.0.0   13-Oct-2022      Sathyavathi        Mask card number
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using Semnox.Parafait.Product;

namespace Parafait_Kiosk.Transaction
{
    public partial class frmPaymentGameCard : BaseFormKiosk
    {
        string Function;
        DataRow rowProduct;
        decimal ProductPrice;
        decimal AmountRequired;
        private decimal productTaxAmount;
        private decimal productPriceWithOutTax;

        private decimal depositTaxAmount;
        private decimal depositPriceWithOutTax;


        KioskStatic.acceptance ac;
        KioskStatic.configuration config = KioskStatic.config;

        int CardCount = 1;
        PaymentModeDTO _paymentModeDTO = new PaymentModeDTO();

        double creditPlusAmount = 0;//begin modificatoin on 01-Dec-2016
        string _pFunction; DataRow _prowProduct; Card _rechargeCard; CustomerDTO customerDTO; int _cardCount;
        string selectedLoyaltyCardNo = "";
        double usedCredits = 0;
        double usedAmount = 0;
        double usedCreditPlus = 0;
        double balance = 0;

        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;
        private Semnox.Core.Utilities.Utilities Utilities = KioskStatic.Utilities;

        bool MultipleCardsInSingleProduct = false;

        private readonly TagNumberParser tagNumberParser;
        public string cardNumber;
        public Card Card;
        public TransactionPaymentsDTO debitTrxPaymentDTO = null;
        clsKioskTransaction kioskTransaction;
        string selectedEntitlementType = "Credits";
        DiscountsSummaryDTO discountSummaryDTO = null;
        Semnox.Parafait.Transaction.Transaction inTrx;       
        private List<KeyValuePair<string, ProductsDTO>> selectedFundsAndDonationsList = null;
        
        public frmPaymentGameCard(string pFunction, DataRow prowProduct, Card rechargeCard, CustomerDTO customerDTO, PaymentModeDTO selectedPaymentModeDTO, int inCardCount, string entitlementType, DiscountsSummaryDTO discountSummaryDTO = null, string couponNo = null, List<KeyValuePair<string, ProductsDTO>> activeFundsDonationsDTOList = null, Semnox.Parafait.Transaction.Transaction Trx = null) // I for issue, R for recharge
        {
            log.LogMethodEntry(pFunction, prowProduct, rechargeCard, customerDTO, selectedPaymentModeDTO, inCardCount, entitlementType, discountSummaryDTO, couponNo, activeFundsDonationsDTOList);
	        _pFunction = pFunction;
            _rechargeCard = rechargeCard;
            _prowProduct = prowProduct;
            this.customerDTO = customerDTO;
            _cardCount = CardCount;  
            inTrx = Trx;          
            selectedEntitlementType = entitlementType;
            this.discountSummaryDTO = discountSummaryDTO;
            selectedFundsAndDonationsList = activeFundsDonationsDTOList;
            kioskTransaction = new clsKioskTransaction(this, pFunction, prowProduct, rechargeCard, customerDTO, selectedPaymentModeDTO, inCardCount, selectedEntitlementType, "", discountSummaryDTO, couponNo, inTrx, selectedFundsAndDonationsList);

            ProductPrice = kioskTransaction.ProductPrice;
            CardCount = kioskTransaction.CardCount;
            MultipleCardsInSingleProduct = kioskTransaction.MultipleCardsInSingleProduct;
            ProductPrice = kioskTransaction.ProductPrice;
            AmountRequired = kioskTransaction.AmountRequired;
            productTaxAmount = kioskTransaction.ProductTaxAmount;
            productPriceWithOutTax = kioskTransaction.ProductPriceWithOutTax;

            depositTaxAmount = kioskTransaction.DepositTaxAmount;
            depositPriceWithOutTax = kioskTransaction.DepositPriceWithOutTax;
            decimal totalCardDeposit = kioskTransaction.ProductDeposit * (CardCount);
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            //btnApply.Enabled = false;
            this.ShowInTaskbar = true;

            _paymentModeDTO = selectedPaymentModeDTO;
            Function = pFunction;
            rowProduct = prowProduct;

            InitializeComponent();
        }


        private void frmPaymentGameCard_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            btnApply.Enabled = false;
            //lblTotalToPay.Text = Convert.ToString(AmountRequired);
            //lblTotaltoPayText.Text = MessageUtils.getMessage("Total to Pay");
            btnApply.Text = MessageUtils.getMessage("Apply");
            btnCancel.Text = MessageUtils.getMessage("Cancel");
            lblAvailableCreditsText.Text = MessageUtils.getMessage("Available Credits") + ":";
            lblBalanceCreditsText.Text = MessageUtils.getMessage("Balance Credits") + ":";
            lblPurchaseValueText.Text = MessageUtils.getMessage("Purchase Value") + ":";
            lblCardNumberText.Text = MessageUtils.getMessage("Card Number") + ":";
            lblPaymentText.Text = MessageUtils.getMessage("Payment - Gamecard");
            lblTapCardText.Text = MessageUtils.getMessage("Tap card to pay for the purchase");
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
            KioskStatic.logToFile("frmPaymentGameCard loaded");
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
                Close();
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
                        txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(528);
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
                txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(1607);//Refreshing Card from HQ. Please Wait...
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
                    txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(528);
                else
                    txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(459);

                Card = null;
                cardNumber = "";
                refreshLabels();
                this.btnApply.Enabled = false;
                //ticks = 0;
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
                txtMessage.Text = MessageUtils.getMessage(197, Card.CardNumber);
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
                    CreditPlus creditPlus = new CreditPlus(Utilities);
                    creditPlusAmount = creditPlus.getCreditPlusForPOS(Card.card_id, Utilities.ParafaitEnv.POSTypeId, kioskTransaction);
                }
                txtMessage.Text = "";
                if (ValidateGameCards() != 0)
                {
                    txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(183);
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
            creditPlusAvailable = Math.Round(Convert.ToDouble(creditPlusAmount), ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
            credits = Math.Round(Convert.ToDouble(Card.credits), ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
            creditAmountAvailable = credits;
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
                lblBalanceCredits.Text = Convert.ToString((credits + creditPlusAvailable) - (double)AmountRequired);
                lblPurchaseValue.Text = Convert.ToString(AmountRequired);
                lblCardNumber.Text = KioskHelper.GetMaskedCardNumber(Card.CardNumber);
                lblAvailableCredits.Text = Convert.ToString(creditPlusAvailable + creditAmountAvailable);
                this.btnApply.Enabled = true;
            }
            else
            {

                txtMessage.Text = "";
                refreshLabels();
                if (localGameCardAmount != 0)
                {
                    this.btnApply.Enabled = false;
                    txtMessage.Text = MessageUtils.getMessage(183);
                    log.Debug("Ends-ValidateGameCards() as Insufficient Credits on Game Card(s)");//Modified for Adding logger feature on 08-Mar-2016
                    return localGameCardAmount;
                }
            }

            log.LogMethodExit("Ends-ValidateGameCards()");//Modified for Adding logger feature on 08-Mar-2016
            return 0;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            StopKioskTimer();
            debitTrxPaymentDTO = new TransactionPaymentsDTO();
            debitTrxPaymentDTO.PaymentModeId = _paymentModeDTO.PaymentModeId;
            debitTrxPaymentDTO.paymentModeDTO = _paymentModeDTO;
            debitTrxPaymentDTO.Amount = usedCredits;
            debitTrxPaymentDTO.CardId = Card.card_id;
            debitTrxPaymentDTO.CardEntitlementType = "C";
            debitTrxPaymentDTO.PaymentUsedCreditPlus = usedCreditPlus;
            debitTrxPaymentDTO.PaymentCardNumber = Card.CardNumber;

            this.Close();
            log.LogMethodExit();
        }
        private void refreshLabels()
        {
            log.LogMethodEntry();
            lblBalanceCredits.Text = "";
            lblPurchaseValue.Text = "";
            lblCardNumber.Text = "";
            lblAvailableCredits.Text = "";
            log.LogMethodExit();
        }

        private void butttonCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Cancel pressed");
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
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
                using (frmCardTransaction frms = new frmCardTransaction(_pFunction, _prowProduct, _rechargeCard, customerDTO, _paymentModeDTO, _cardCount, selectedEntitlementType, selectedLoyaltyCardNo, discountSummaryDTO == null ? null : discountSummaryDTO, null))
                {
                    frms.trxPaymentDTO = debitTrxPaymentDTO;
                    frms.ShowDialog();
                }
            }
            log.LogVariableState("KioskStatic.TopUpReaderDevice", KioskStatic.TopUpReaderDevice);
        }
    }

}
