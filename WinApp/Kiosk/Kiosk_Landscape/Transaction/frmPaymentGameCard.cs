/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmPaymentGameCard 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 *2.80        12-June-2020     Dakshakh Raj       Created: As part of Payment Modes based on 
 *                                                        Kiosk Configuration set up enhancement
 *2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/

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
using Semnox.Parafait.POS;
using System.Collections.Generic;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk.Transaction
{

    public partial class frmPaymentGameCard : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        decimal AmountRequired; 
        KioskStatic.configuration config = KioskStatic.config;
         
        public Card Card;

        Utilities utilities = KioskStatic.Utilities;
        private readonly TagNumberParser tagNumberParser;
        bool MultipleCardsInSingleProduct = false;
        public string cardNumber;
        PaymentModeDTO _paymentModeDTO = new PaymentModeDTO();
        public TransactionPaymentsDTO debitTrxPaymentDTO = null;

        double creditPlusAmount = 0;//begin modificatoin on 01-Dec-2016

        double usedCredits = 0;
        double usedAmount = 0;
        double usedCreditPlus = 0;
        double balance = 0;

        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmPaymentGameCard(KioskTransaction kioskTransactionIn, PaymentModeDTO selectedPaymentModeDTO)  
        {
            log.LogMethodEntry("kioskTransactionIn", selectedPaymentModeDTO );
            Audio.Stop();
            this.kioskTransaction = kioskTransactionIn;
            AmountRequired = (decimal)kioskTransaction.GetTrxNetAmount(); //kioskTransaction.AmountRequired;
            
            _paymentModeDTO = selectedPaymentModeDTO;


            InitializeComponent();
            //TimerMoney.Enabled = exitTimer.Enabled = false;
            KioskTimerSwitch(false);
            utilities.setLanguage(this);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);//Starts:Modification on 17-Dec-2015 for introducing new theme           
            KioskStatic.setDefaultFont(this);//Ends:Modification on 17-Dec-2015 for introducing new theme

            tagNumberParser = new TagNumberParser(utilities.ExecutionContext);

            displayMessageLine("");
     
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
       }
        private void frmPaymentGameCard_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            btnApply.Enabled = false;
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
            KioskStatic.logToFile("frmTapCard loaded");
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
                txtMessage.Text = "Refreshing Card from HQ. Please Wait...";
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
                txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 197, Card.CardNumber);
                Card = null;
                cardNumber = "";
                refreshLabels();
		        this.btnApply.Enabled = false;
                //ticks = 0;
                ResetKioskTimer();
                //PoleDisplay.writeLines(txtMessage.Text, SwipedCardNumber);
                //log.Info(SwipedCardNumber + " is tech card");
                //return false;
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
                    creditPlusAmount = creditPlus.getCreditPlusForPOS(Card.card_id, utilities.ParafaitEnv.POSTypeId, kioskTransaction);
                }

                //dgvGameCards.Rows.Add(new object[] { card.card_id, SwipedCardNumber, credits + creditPlusAmount, null, null, credits, 0, creditPlusAmount, 0, card.last_update_time });
                if (ValidateGameCards() != 0)
                {
                    txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(183);
                }
                //kioskTransaction. TransactionInfo.PrimaryPaymentCardNumber = SwipedCardNumber;
                //PoleDisplay.writeLines("Card: " + SwipedCardNumber, "Balance: " + (credits + creditPlusAmount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));

                //if ((credits + creditPlusAmount) < tobePaidAmount && Utilities.getParafaitDefaults("ALLOW_PARENTCARD_PAYMENT_WITHOUT_CARD").Equals("Y"))
                //{
                //    object parentCardNumber = Utilities.executeScalar(@"select top 1 card_number 
                //                                                     from cards c, ParentChildCards pcc 
                //                                                    where pcc.ParentCardId = c.card_id 
                //                                                    and pcc.ChildCardId = @cardId
                //                                                    and pcc.ActiveFlag = 1 
                //                                                    and c.valid_flag = 'Y'",
                //                                                    new SqlParameter("@cardId", card.card_id));
                //    if (parentCardNumber != null)
                //        InsertCardDetails(parentCardNumber.ToString(), readerDevice);
                //}

                //log.Debug("Ends-InsertCardDetails(" + SwipedCardNumber + ",readerDevice)");//Modified for Adding logger feature on 08-Mar-2016
            }

            log.LogMethodExit();
        }

        double ValidateGameCards()
        {
            log.LogMethodEntry();//Modified for Adding logger feature on 08-Mar-2016
            double credits = 0;
            double localGameCardAmount = (double)AmountRequired; ;
            double creditAmountAvailable = 0;
            double creditPlusAvailable = 0;
            creditPlusAvailable = Math.Round(Convert.ToDouble(creditPlusAmount), utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
            credits = Math.Round(Convert.ToDouble(Card.credits), utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
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
                lblCardNumber.Text = Convert.ToString(Card.CardNumber);
                lblAvailableCredits.Text = Convert.ToString(creditPlusAvailable + creditAmountAvailable);
                this.btnApply.Enabled = true;
                //localGameCardAmount = 0;

            }
            else
            {
                //txtTotalDebitCardPayment.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", getDebitCardPaymentAmount());
                txtMessage.Text = "";
                refreshLabels();
                if (localGameCardAmount != 0)
                {
                    this.btnApply.Enabled = false;
                    txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 183);
                    log.Debug("Ends-ValidateGameCards() as Insufficient Credits on Game Card(s)");//Modified for Adding logger feature on 08-Mar-2016
                    return localGameCardAmount;
                }
            }

            log.LogMethodExit("Ends-ValidateGameCards()");//Modified for Adding logger feature on 08-Mar-2016
            return 0;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (_paymentModeDTO != null)
            {
                ResetKioskTimer();
                StopKioskTimer();
                DialogResult = System.Windows.Forms.DialogResult.OK;
                debitTrxPaymentDTO = new TransactionPaymentsDTO();
                debitTrxPaymentDTO.PaymentModeId = _paymentModeDTO.PaymentModeId;
                debitTrxPaymentDTO.paymentModeDTO = _paymentModeDTO;
                debitTrxPaymentDTO.Amount = usedCredits;
                debitTrxPaymentDTO.CardId = Card.card_id;
                debitTrxPaymentDTO.CardEntitlementType = "C";
                debitTrxPaymentDTO.PaymentUsedCreditPlus = usedCreditPlus;
                debitTrxPaymentDTO.PaymentCardNumber = Card.CardNumber;
                this.Close();
            }
            log.LogMethodExit();
        }


        private void displayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            Application.DoEvents();
            txtMessage.Text = message;
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

        private void btnCancel_Click(object sender, EventArgs e)
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
            StopKioskTimer();
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
                StopKioskTimer();
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

    }
}
