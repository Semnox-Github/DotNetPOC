/********************************************************************************************
* Project Name - Semnox.Parafait.KioskCore 
* Description  - clsKioskTransaction 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for MultiPoint changes & Tax display
*2.6.0       17-Mar-2019      Nitin Pai          Modified for Split Product -  to add card deposit and credit plus
*2.6.0       30-Apr-2019      Nitin Pai          Adding deposit to KIOSK transactions
*2.7.0       30-Jun-2019      Nitin Pai          Moving split product enhancement to AccountBL
*2.70.3      3-Sep-2019       Deeksha            Added logger methods.
*2.70.3      06-Nov-2019      Girish Kundar      Ticket printer integration enhancement
*2.70.3      06-Nov-2019      Dakshakh raj       Issued cards in Kiosk bin to be dispensed as issued card other than rejecting 
*2.90.0      30-Jun-2020      Dakshakh raj       Dynamic Payment Modes based on set up
*2.100.0     05-Aug-2020      Guru S A           Kiosk activity log changes
*2.100.0     21-Oct-2020      Guru S A           Kiosk is reporting few card payments as cash payments for particular user navigation
*2.100.0     10-Feb-2021      Deeksha            Issue Fix : Hiding cash button option on credit card payment cancel click
*2.110.0     23-Oct-2020      Mushahid Faizan    Passed execution context in Tax.
*2.120.0     09-Oct-2020      Guru S A           Wristband printing flow changes
*2.140.0     18-Oct-2021      Sathyavathi        Check-In Check-Out feature in Kiosk
*2.140.0     22-Oct-2021      Sathyavathi        CEC enhancement - Fund Raiser and Donations
*2.140.2     13-Jun-2022      Sathyavathi        VCAT integration in Kiosk reconciled from Fireball08
 ***********************************************************************************************/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Waiver;
using Semnox.Parafait.Device.PaymentGateway;
using System.Drawing.Printing;
using System.Printing;
using Semnox.Parafait.Product;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Printer;
using Semnox.Parafait.POS;
using Semnox.Parafait.Languages;
using System.Threading.Tasks;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.KioskCore
{
    public class clsKioskTransaction
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";
        string Function;
        public DataRow rowProduct;
        Card CurrentCard, FirstCard;
        public decimal ProductPrice;
        public decimal AmountRequired;
        public decimal ProductDeposit;
        bool wristBandPrintTag;
        bool TransactionSuccess = false;
        string loyaltyCardNumber = "";
        bool transactionHasCardToPrint = false;
        Semnox.Parafait.KioskCore.CoinAcceptor.CoinAcceptor coinAcceptor;
        Semnox.Parafait.KioskCore.BillAcceptor.BillAcceptor billAcceptor;

        KioskStatic.acceptance ac;
        KioskStatic.receipt_format rc = KioskStatic.rc;
        bool cardPrinterError = false;
        public int CardCount = 1;
        int DispensedCardCount = 0;
        PaymentModeDTO _PaymentModeDTO;
        bool printReceipt = false;
        private bool multiPointConversionRequired = false;
        private decimal productTaxAmount = 0;
        private decimal productPriceWithOutTax = 0;

        private decimal depositTaxAmount = 0;
        private decimal depositPriceWithOutTax = 0;
        public TransactionPaymentsDTO trxPaymentDTO = null;
        private POSPrinterDTO rfidPrinterDTO = null;
        private double ccSurchargeAmount = 0;

        //byte[] inp;

        Transaction.Transaction CurrentTrx;
        public Semnox.Parafait.Transaction.Transaction DummyCurrentTrx;

        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;

        public bool MultipleCardsInSingleProduct = false;
        CustomerDTO customerDTO = null;

        Button txtMessage;
        Form trxForm;
        Button btnCancel, btnDebitCard, btnCreditCard;
        FlowLayoutPanel fLPPModes;
        public bool cancelPressed = false;
        public delegate void dlgShowThankYou(bool receiptPrinted);
        dlgShowThankYou showThankYou;
        string selectedEntitlementType = "Credits";
        DiscountsSummaryDTO discountSummaryDTO = null;
        private const string DONATIONS_LOOKUP_VALUE = "Donations";
        List<KeyValuePair<string, ProductsDTO>> selectedFundsDonationsList;

        string qrCodeScanned;
        public string QRCodeScanned { set { qrCodeScanned = value; } get { return qrCodeScanned; } }
        string couponNumber = "";
        decimal discountAmount = 0;
        public string CouponNumber
        {
            get { return couponNumber; }
        }
        public delegate void dlgShowOK(string message, bool enableTimeOut = true);
        dlgShowOK showOK;

        public decimal ProductTaxAmount
        {
            get { return productTaxAmount; }
        }
        public decimal ProductPriceWithOutTax
        {
            get { return productPriceWithOutTax; }
        }

        public decimal DepositTaxAmount
        {
            get { return depositTaxAmount; }
        }
        public decimal DepositPriceWithOutTax
        {
            get { return depositPriceWithOutTax; }
        }

        public double CCSurchargeAmount
        {
            get { return ccSurchargeAmount; }
        }
        public Transaction.Transaction UpdatedTrx
        {
            get { return CurrentTrx; }
        }
        public clsKioskTransaction(Form _trxForm, string pFunction, DataRow prowProduct, Card rechargeCard, CustomerDTO customerDTO, PaymentModeDTO PaymentModeDTO, int inCardCount, string entitlementType, string loyaltyCardNo, DiscountsSummaryDTO discountSummaryDTO = null,
                            string couponNo = null, Transaction.Transaction Trx = null, List<KeyValuePair<string, ProductsDTO>> activeFundsDonationsList = null)
        {
            log.LogMethodEntry(_trxForm, pFunction, prowProduct, rechargeCard, customerDTO, PaymentModeDTO, inCardCount, entitlementType, loyaltyCardNo, discountSummaryDTO, couponNo, Trx, activeFundsDonationsList);
            trxForm = _trxForm;
            selectedEntitlementType = entitlementType;
            loyaltyCardNumber = loyaltyCardNo;
            CurrentTrx = Trx;
            selectedFundsDonationsList = activeFundsDonationsList;
            ProductPrice = Math.Round(Convert.ToDecimal(prowProduct["price"] == DBNull.Value ? 0 : prowProduct["price"]), 4, MidpointRounding.AwayFromZero);
            productPriceWithOutTax = Math.Round(Convert.ToDecimal(prowProduct["ProductPrice"] == DBNull.Value ? 0 : prowProduct["ProductPrice"]), 4, MidpointRounding.AwayFromZero);
            productTaxAmount = Math.Round(Convert.ToDecimal(prowProduct["taxAmount"] == DBNull.Value ? 0 : prowProduct["taxAmount"]), 4, MidpointRounding.AwayFromZero);
            string TaxInclusivePrice = prowProduct["TaxInclusivePrice"] == DBNull.Value ? "N" : prowProduct["TaxInclusivePrice"].ToString();
            Decimal taxPercentage = Math.Round(Convert.ToDecimal(prowProduct["taxPercentage"] == DBNull.Value ? 0 : prowProduct["taxPercentage"]), 4, MidpointRounding.AwayFromZero);
            rfidPrinterDTO = KioskStatic.GetRFIDPrinter(Utilities.ExecutionContext, Utilities.ParafaitEnv.POSMachineId);
            wristBandPrintTag = KioskStatic.IsWristBandPrintTag(Convert.ToInt32(prowProduct["product_id"]), rfidPrinterDTO); //prowProduct["AutoGenerateCardNumber"].ToString() == "N" || prowProduct["AutoGenerateCardNumber"] == DBNull.Value ? false : true;
            // modified 02/2019: BearCat Split product entitlement -  get the original product deposit and send that as deposit
            try
            {
                // if the product is game time or card sale, the deposit amount is added to the prow["price"] in the frmPaymentMode. No need to change here
                if (pFunction.Equals("R") || prowProduct["product_type"].ToString() == "VARIABLECARD" || pFunction.Equals("C"))
                {
                    // deposit will be zero on recharge
                    ProductDeposit = 0;
                }
                else
                {
                    ProductDeposit = Math.Round(Convert.ToDecimal(prowProduct["face_value"]), 4, MidpointRounding.AwayFromZero);
                }

                // get depot product details depositTaxAmount && depositPriceWithOutTax
                depositTaxAmount = 0;
                depositPriceWithOutTax = ProductDeposit;

                if (ProductDeposit > 0)
                {
                    ProductsDTO depositProduct = KioskStatic.GetDepositProduct();
                    if (depositProduct != null && depositProduct.Tax_id != -1)
                    {
                        Tax depositTax = new Tax(Utilities.ExecutionContext, depositProduct.Tax_id);
                        if (depositTax.GetTaxDTO() != null && depositTax.GetTaxDTO().TaxPercentage > 0)
                        {
                            if (depositProduct.TaxInclusivePrice.Equals("Y"))
                            {
                                depositPriceWithOutTax = Math.Round(Convert.ToDecimal(Convert.ToDouble(ProductDeposit) / (1.0 + Convert.ToDouble(depositTax.GetTaxDTO().TaxPercentage) / 100.0)), 4, MidpointRounding.AwayFromZero);
                                depositTaxAmount = ProductDeposit - depositPriceWithOutTax;
                            }
                            else
                            {
                                depositTaxAmount = Math.Round(Convert.ToDecimal((Convert.ToDouble(ProductDeposit) * Convert.ToDouble(depositTax.GetTaxDTO().TaxPercentage)) / 100.0), 4, MidpointRounding.AwayFromZero);
                                depositPriceWithOutTax = ProductDeposit;
                                ProductDeposit += depositTaxAmount;
                            }
                        }
                    }

                    if (TaxInclusivePrice.Equals("N"))
                    {
                        productTaxAmount = Math.Round(((ProductPrice - ProductDeposit) * (taxPercentage / 100)), 4, MidpointRounding.AwayFromZero);
                        productPriceWithOutTax = (ProductPrice - ProductDeposit);
                    }
                    else
                    {
                        productPriceWithOutTax = Math.Round(((ProductPrice - ProductDeposit) / (1 + taxPercentage / 100)), 4, MidpointRounding.AwayFromZero);
                        productTaxAmount = (ProductPrice - ProductDeposit - productPriceWithOutTax);
                    }
                }
            }
            catch (Exception ex)
            {

                KioskStatic.logToFile("Failed to convert deposit:" + ex.Message);
                ProductDeposit = 0.0M;
            }

            if (productPriceWithOutTax == 0 && prowProduct["product_type"].ToString() == "VARIABLECARD")
            {
                productPriceWithOutTax = ProductPrice;
            }

            if (discountSummaryDTO != null)
            {
                this.discountSummaryDTO = discountSummaryDTO;
                discountAmount = discountSummaryDTO.DiscountAmount;
            }

            if (couponNo != null)
                couponNumber = couponNo;

            if (pFunction.Equals("I"))
            {
                if (Convert.ToInt32(prowProduct["CardCount"]) > 1)
                    MultipleCardsInSingleProduct = true;

                CardCount = inCardCount;

                if (CardCount <= 0)
                    CardCount = 1;

                if (MultipleCardsInSingleProduct)
                    AmountRequired = ProductPrice + ((inCardCount - 1) * (ProductDeposit));
                else
                    AmountRequired = ProductPrice * CardCount;

                KioskStatic.logToFile("Card Count: " + CardCount.ToString());
                KioskStatic.logToFile("Multiple Cards In Single Product?: " + MultipleCardsInSingleProduct.ToString());
            }
            else if (pFunction.Equals("C"))
            {
                CardCount = inCardCount;
                AmountRequired = (CurrentTrx == null) ? (ProductPrice * CardCount) : (decimal)CurrentTrx.Net_Transaction_Amount;
                KioskStatic.logToFile("Guest Count: " + CardCount.ToString());
            }
            else
            {
                AmountRequired = ProductPrice;
            }

            _PaymentModeDTO = PaymentModeDTO;
            Function = pFunction;
            rowProduct = prowProduct;

            if (rechargeCard != null)
                CurrentCard = rechargeCard;

            if (discountSummaryDTO != null)
            {
                AmountRequired = AmountRequired - discountSummaryDTO.DiscountAmount;
            }

            if (!pFunction.Equals("C") && CurrentTrx != null)
            {
                if (activeFundsDonationsList != null && activeFundsDonationsList.Any())
                {
                    foreach (KeyValuePair<string, ProductsDTO> keyValuePair in selectedFundsDonationsList)
                    {
                        if (keyValuePair.Key == DONATIONS_LOOKUP_VALUE)
                        {
                            double lineAmount = 0;
                            lineAmount = CurrentTrx.TrxLines.Where(t => t.CategoryId == keyValuePair.Value.CategoryId).FirstOrDefault().LineAmount;
                            AmountRequired = AmountRequired + Convert.ToDecimal(lineAmount);
                        }
                    }
                }
            }

            this.customerDTO = customerDTO;
            log.LogMethodExit();
        }

        public void SetupKioskTransaction(bool _printReceipt,
                                            Semnox.Parafait.KioskCore.CoinAcceptor.CoinAcceptor _coinAcceptor,
                                            Semnox.Parafait.KioskCore.BillAcceptor.BillAcceptor _billAcceptor,
                                            KioskStatic.acceptance _ac,
                                            dlgShowThankYou _showThankYou,
                                            dlgShowOK _showOK)
        {
            log.LogMethodEntry(_printReceipt, _coinAcceptor, _billAcceptor, _ac, _showThankYou, _showOK);
            printReceipt = _printReceipt;
            coinAcceptor = _coinAcceptor;
            billAcceptor = _billAcceptor;
            ac = _ac;
            showThankYou = _showThankYou;
            showOK = _showOK;

            txtMessage = trxForm.Controls.Find("txtMessage", true).FirstOrDefault() as Button;
            btnCancel = trxForm.Controls.Find("btnCancel", true).FirstOrDefault() as Button;
            btnDebitCard = trxForm.Controls.Find("btnDebitCard", true).FirstOrDefault() as Button;
            btnCreditCard = trxForm.Controls.Find("btnCreditCard", true).FirstOrDefault() as Button;
            fLPPModes = trxForm.Controls.Find("fLPPModes", true).FirstOrDefault() as FlowLayoutPanel;

            if (txtMessage == null)
                KioskStatic.logToFile("unable to locate txtMessage");

            if (btnCancel == null)
                KioskStatic.logToFile("unable to locate btnCancel");

            if (fLPPModes == null)
                KioskStatic.logToFile("unable to locate fLPPModes");

            if (btnDebitCard == null)
                KioskStatic.logToFile("unable to locate btnDebitCard");

            if (btnCreditCard == null)
                KioskStatic.logToFile("unable to locate btnCreditCard");
            log.LogMethodExit();
        }

        public bool ApplyVoucher(Card card, ref string message)
        {
            return CurrentTrx.ApplyVoucher(card, ref message);
        }

        public bool rechargeCard()
        {
            log.LogMethodEntry();
            Audio.PlayAudio(Audio.WaitForCardTopUp);
            KioskStatic.logToFile("Recharging card: " + CurrentCard.CardNumber);
            CurrentCard.getCardDetails(CurrentCard.card_id); // refresh card
            if (createTransaction())
            {
                KioskStatic.logToFile("Recharge successful");
                KioskStatic.updateKioskActivityLog(-1, -1, CurrentCard.CardNumber, "TOP-UP", rowProduct["product_name"].ToString(), ac);

                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                }

                bool printed = false;
                if (KioskStatic.receipt == true)
                {
                    try
                    {
                        if (print_receipt())
                        {
                            displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 430), WARNING);
                            printed = true;
                        }
                        else
                            displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 432), WARNING);

                        Audio.PlayAudio(Audio.CollectReceipt, Audio.ThankYouEnjoyGame);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 431, ex.Message), ERROR);
                    }
                }
                else
                {
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 432), WARNING);
                    Audio.PlayAudio(Audio.ThankYouEnjoyGame);
                }

                ac.totalValue = 0;
                showThankYou(printed);

                log.LogMethodExit(true);
                return true;
            }
            else
            {
                if (btnCancel != null)
                    btnCancel.Enabled = true;
                abortAndExit(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Recharge failed"));
                log.LogMethodExit(false);
                return false;
            }
        }
        public bool doCheckIn()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Card Number: " + CurrentCard.CardNumber);
            CurrentCard.getCardDetails(CurrentCard.card_id); // refresh card 

            if (createCheckInTransaction())
            {
                KioskStatic.logToFile("Save Order successful");
                KioskStatic.updateKioskActivityLog(-1, -1, CurrentCard.CardNumber, "Check-In", rowProduct["product_name"].ToString(), ac);

                bool printed = false;
                if (KioskStatic.receipt == true)
                {
                    try
                    {
                        if (print_receipt())
                        {
                            displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4120), WARNING); //Transaction Successful. Please collect your receipt... Thank You.
                            printed = true;
                        }
                        else
                            displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4121), WARNING); //Transaction Successful. Thank You.

                        Audio.PlayAudio(Audio.CollectReceipt, Audio.ThankYouEnjoyGame);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4122, ex.Message), ERROR); //Transaction Successful. Print Error: &1
                        MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4122)); //Transaction Successful. Print Error: &1
                    }
                }
                else
                {
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4121), WARNING); //Transaction Successful. Thank You.
                    Audio.PlayAudio(Audio.ThankYouEnjoyGame);
                }

                ac.totalValue = 0;
                showThankYou(printed);

                log.LogMethodExit(true);
                return true;
            }
            else
            {
                if (btnCancel != null)
                    btnCancel.Enabled = true;
                abortAndExit(MessageUtils.getMessage("doCheckIn failed"));
                log.LogMethodExit(false);
                return false;
            }
        }
        public void abortAndExit(string errorMessage)
        {
            log.LogMethodEntry(errorMessage);
            KioskStatic.logToFile("AbortAndExit: " + errorMessage);

            if (coinAcceptor != null)
                coinAcceptor.disableCoinAcceptor();
            if (billAcceptor != null)
                billAcceptor.disableBillAcceptor();

            if (ac != null && ac.totalValue != 0)
            {
                decimal cashAmount = ac.totalValue;
                decimal ccAmount = 0;
                decimal gameCardAmount = 0;
                if (ac.totalValue > 0)
                {
                    if ((trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed)
                        || (trxPaymentDTO != null && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString())))
                    {
                        ccAmount = (decimal)trxPaymentDTO.Amount;
                        cashAmount -= ccAmount;
                    }
                    if (trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed == false
                        && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard)
                    {
                        gameCardAmount = (decimal)trxPaymentDTO.PaymentUsedCreditPlus;
                        cashAmount -= gameCardAmount;
                    }
                }
                ac.totalCCValue = ccAmount;
                ac.totalGameCardValue = gameCardAmount;

                KioskStatic.updateKioskActivityLog(-1, -1, "", KioskStatic.ACTIVITY_TYPE_ABORT, errorMessage, ac);

                KioskStatic.logToFile("Cash: " + cashAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                log.Info("Cash: " + cashAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                KioskStatic.logToFile("Credit Card: " + ccAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                log.Info("Credit Card: " + ccAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                KioskStatic.logToFile("Game Card: " + gameCardAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                log.Info("Game Card: " + gameCardAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));

                if (ccAmount > 0)
                {
                    string mes = "";
                    try
                    {
                        displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "REVERSING CREDIT CARD PAYMENT"), WARNING);
                        if (trxPaymentDTO != null && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString()))
                        {
                            ReverseFiscalization();
                            return;
                        }
                        if (CreditCardPaymentGateway.RefundAmount(trxPaymentDTO, Utilities, ref mes))
                        {
                            CreditCardPaymentGateway.PrintCCReceipt(trxPaymentDTO);
                            ac.totalValue -= ccAmount;
                        }
                        KioskStatic.logToFile("Credit Card Refund: " + mes);
                        log.Info("Credit Card Refund: " + mes);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        KioskStatic.logToFile("Credit Card Refund: " + ex.Message);
                        log.Info("Credit Card Refund: " + ex.Message);
                    }
                }

                if (gameCardAmount > 0)
                {
                    ac.totalValue -= gameCardAmount;
                    KioskStatic.logToFile("Game Card Amount adjusted from total");
                    log.Info("Game Card Amount adjusted from total");
                }

                if (KioskStatic.receipt)
                    print_receipt(true);

                string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Transaction could not be completed.");
                if (cashAmount > 0)
                {
                    message += (" " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Money inserted by you has been recognized."));
                }

                if (ccAmount > 0 && trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed == false)
                {
                    message += (" " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Your credit card has not been charged."));
                }

                if (gameCardAmount > 0 && trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed == false
                     && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard)
                {
                    message += (" " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Your Game card has not been debited."));
                }

                message += (" " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Please contact attendant with the receipt printed.") + " [" + errorMessage + "]");
                KioskStatic.logToFile(message);

                showOK(message);

                ac.totalValue = 0;
            }
            else
            {
                log.Info("AbortAndExit: No action as AC is null or Total Value is zero");
                KioskStatic.logToFile("AbortAndExit: No action as AC is null or Total Value is zero");
            }
            trxForm.Close();
            log.LogMethodExit();
        }

        // in case of new card issue with variable amount, create a carddeposit product before applying variable recharge
        bool variableNewCardCheck()
        {
            log.LogMethodEntry();
            if (Function.Equals("I") && rowProduct["product_type"].ToString() == "VARIABLECARD")
            {
                if (KioskStatic.Utilities.ParafaitEnv.CardDepositProductId < 0)
                {
                    displayMessageLine(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "CardDepositProduct not defined. Contact Manager"), ERROR);
                    throw new ApplicationException(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "CardDepositProduct not defined. Contact Manager"));
                }

                string message = "";
                if (CurrentTrx.createTransactionLine(CurrentCard, KioskStatic.Utilities.ParafaitEnv.CardDepositProductId, KioskStatic.Utilities.ParafaitEnv.CardFaceValue, 1, ref message) != 0)
                {
                    displayMessageLine(message, ERROR);
                    throw new ApplicationException(message);
                }
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        // in case of new card issue with variable amount, create a carddeposit product before applying variable recharge
        decimal variableNewCardCheck(decimal Amount)
        {
            log.LogMethodEntry(Amount);
            if (Function.Equals("I") && rowProduct["product_type"].ToString() == "VARIABLECARD")
            {
                string productType = "NEW";
                if (selectedEntitlementType == "Time")
                {
                    productType = "GAMETIME";
                }

                ProductsList productsList = new ProductsList(KioskStatic.Utilities.ExecutionContext);
                List<ProductsDTO> productsDTOList = productsList.getSplitProductList(Convert.ToDouble(Amount), productType);
                if (productsDTOList != null && productsDTOList.Count > 0)
                {
                    int prodId = productsDTOList[0].ProductId;
                    int externalRef = -1;
                    CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(KioskStatic.Utilities.ExecutionContext);
                    List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.NAME, "External System Identifier"));
                    List<CustomAttributesDTO> customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParameters);
                    if (customAttributesDTOList != null)
                    {
                        CustomDataListBL customDataListBL = new CustomDataListBL(Utilities.ExecutionContext);
                        List<KeyValuePair<CustomDataDTO.SearchByParameters, string>> searchCustomDataParameters = new List<KeyValuePair<CustomDataDTO.SearchByParameters, string>>();
                        searchCustomDataParameters.Add(new KeyValuePair<CustomDataDTO.SearchByParameters, string>(CustomDataDTO.SearchByParameters.CUSTOM_DATA_SET_ID, productsDTOList[0].CustomDataSetId.ToString()));
                        searchCustomDataParameters.Add(new KeyValuePair<CustomDataDTO.SearchByParameters, string>(CustomDataDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID, customAttributesDTOList[0].CustomAttributeId.ToString()));
                        List<CustomDataDTO> customDataDTOList = customDataListBL.GetCustomDataDTOList(searchCustomDataParameters);
                        if (customDataDTOList != null)
                        {
                            if (customDataDTOList[0].CustomDataNumber != null)
                                externalRef = Convert.ToInt32(customDataDTOList[0].CustomDataNumber);
                        }
                    }

                    string message = "";
                    double price = Convert.ToDouble(productsDTOList[0].Price);
                    if (CurrentTrx.createTransactionLine(CurrentCard, prodId, -1, 1, ref message) != 0)
                        throw new ApplicationException(message);

                    KioskStatic.logToFile("Created split product. Ext Ref: " + externalRef.ToString() + ". Parafait product: " + productsDTOList[0].ProductName);
                    multiPointConversionRequired = true;
                    log.LogMethodExit(price);
                    return (decimal)price;
                }
                else
                {
                    if (variableNewCardCheck())
                    {
                        decimal returnValue = (decimal)KioskStatic.Utilities.ParafaitEnv.CardFaceValue;
                        log.LogMethodExit(returnValue);
                        return returnValue;
                    }
                }
            }
            log.LogMethodExit();
            return 0;
        }

        int CreateSplitVariableProducts(int variableProductId, double Amount, ref string message)
        {
            log.LogMethodEntry(variableProductId, Amount, message);
            if (!KioskStatic.SPLIT_AND_MAP_VARIABLE_PRODUCT)
            {
                int returnValue = CurrentTrx.createTransactionLine(CurrentCard, variableProductId, Amount, 1, ref message);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            else
            {
                try
                {
                    while (Amount > 0)
                    {
                        string productType = "RECHARGE";
                        if (selectedEntitlementType == "Time")
                        {
                            productType = "GAMETIME";
                        }

                        ProductsList productsList = new ProductsList(KioskStatic.Utilities.ExecutionContext);
                        List<ProductsDTO> productsDTOList = productsList.getSplitProductList(Amount, productType);
                        if (productsDTOList != null && productsDTOList.Count > 0)
                        {
                            int prodId = productsDTOList[0].ProductId;
                            int externalRef = -1;
                            CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(KioskStatic.Utilities.ExecutionContext);
                            List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.NAME, "External System Identifier"));
                            List<CustomAttributesDTO> customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParameters);
                            if (customAttributesDTOList != null)
                            {
                                CustomDataListBL customDataListBL = new CustomDataListBL(Utilities.ExecutionContext);
                                List<KeyValuePair<CustomDataDTO.SearchByParameters, string>> searchCustomDataParameters = new List<KeyValuePair<CustomDataDTO.SearchByParameters, string>>();
                                searchCustomDataParameters.Add(new KeyValuePair<CustomDataDTO.SearchByParameters, string>(CustomDataDTO.SearchByParameters.CUSTOM_DATA_SET_ID, productsDTOList[0].CustomDataSetId.ToString()));
                                searchCustomDataParameters.Add(new KeyValuePair<CustomDataDTO.SearchByParameters, string>(CustomDataDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID, customAttributesDTOList[0].CustomAttributeId.ToString()));
                                List<CustomDataDTO> customDataDTOList = customDataListBL.GetCustomDataDTOList(searchCustomDataParameters);
                                if (customDataDTOList != null)
                                {
                                    if (customDataDTOList[0].CustomDataNumber != null)
                                        externalRef = Convert.ToInt32(customDataDTOList[0].CustomDataNumber);
                                }
                            }

                            double price = Convert.ToDouble(productsDTOList[0].Price);
                            if (CurrentTrx.createTransactionLine(CurrentCard, prodId, price, 1, ref message) != 0)
                                return -1;
                            Amount -= price;

                            KioskStatic.logToFile("Created split product. Ext Ref: " + externalRef.ToString() + ". Parafait product: " + productsDTOList[0].ProductName);
                            multiPointConversionRequired = true;
                        }
                        else
                        {
                            object oExternalRef = KioskStatic.getProductExternalSystemReference(variableProductId);
                            if (oExternalRef == DBNull.Value)
                            {
                                KioskStatic.logToFile("External System Reference for Variable product not found");
                            }
                            if (CurrentTrx.createTransactionLine(CurrentCard, variableProductId, Amount, 1, ref message) != 0)
                                KioskStatic.logToFile("Created split product. Ext Ref: " + oExternalRef.ToString() + ". Parafait variable product (" + Amount.ToString() + ")");
                            break;
                        }
                    }
                    log.LogMethodExit();
                    return 0;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    log.Error("Error occurred while executing CreateSplitVariableProducts()" + ex.Message);
                    log.LogMethodExit(-1);
                    return -1;
                }
            }
        }
        public bool ApplyCoupon(string couponNo)
        {
            log.LogMethodEntry(couponNo);
            try
            {
                if (CurrentTrx == null)
                {
                    couponNumber = "";
                    createDummyTransaction();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing createDummyTransaction()" + ex.Message);
                KioskStatic.logToFile(ex.Message);
            }
            try
            {
                if (CurrentTrx == null)
                {
                    DummyCurrentTrx.ApplyCoupon(couponNo);
                }
                else
                {
                    CurrentTrx.ApplyCoupon(couponNo);
                    DummyCurrentTrx = CurrentTrx;
                }
                couponNumber = couponNo;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing ApplyCoupon()" + ex.Message);
                KioskStatic.logToFile(ex.Message);
                couponNumber = "";
            }
            log.LogMethodExit(true);
            return true;
        }
        public bool createDummyTransaction()
        {
            log.LogMethodEntry();
            if (Function == "I")
            {
                string cardNumber;
                TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(KioskStatic.Utilities.ExecutionContext);
                if (tagNumberLengthList.Contains(10))
                {
                    cardNumber = "DUMMYCARDA";
                }
                else if (tagNumberLengthList.Contains(14))
                {
                    cardNumber = "DUMMYABCDEFGHI";
                }
                else
                {
                    cardNumber = "DUMMYABC";
                }
                CurrentCard = new Card(KioskStatic.DispenserReaderDevice, cardNumber, "External POS", Utilities);
            }
            if (CurrentCard == null)
            {
                displayMessageLine(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 504), ERROR);
                KioskStatic.logToFile(txtMessage.Text);
                return false;
            }

            if (DummyCurrentTrx == null)
            {
                DummyCurrentTrx = new Semnox.Parafait.Transaction.Transaction(KioskStatic.Utilities);
                DummyCurrentTrx.PaymentReference = "Kiosk Transaction";
                DummyCurrentTrx.PaymentMode = 1;
                KioskStatic.logToFile("New Trx object created");
            }

            decimal dummyTrxValue;
            dummyTrxValue = ProductPrice;
            string message = "";
            try
            {
                //if (DummyCurrentTrx.createTransactionLine(CurrentCard, Convert.ToInt32(rowProduct["product_id"]), (double)ProductPrice, 1, ref message) != 0)
                decimal lineProductPrice = ProductPrice;
                if (rowProduct["TaxInclusivePrice"].ToString() == "N")
                {
                    lineProductPrice = Convert.ToDecimal(rowProduct["ProductPrice"].ToString());
                }
                if (DummyCurrentTrx.createTransactionLine(CurrentCard, Convert.ToInt32(rowProduct["product_id"]), (double)lineProductPrice, MultipleCardsInSingleProduct == true ? 1 : CardCount, ref message) != 0)
                {
                    displayMessageLine(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Error") + ": " + message, ERROR);
                    KioskStatic.logToFile("Error TrxLine4: " + message);
                    log.LogMethodExit(false);
                    return false;
                }
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                displayMessageLine(message + ":" + ex.Message, ERROR);
                KioskStatic.logToFile(message + ":" + ex.Message);
                MessageBox.Show(ex.StackTrace);
                log.Error("Error occurred while executing createDummyTransaction" + ex.Message);
                log.LogMethodExit(false);
                return false;
            }
        }
        bool createCheckInTransaction()
        {
            log.LogMethodEntry();
            if (CurrentCard == null)
            {
                displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 504), ERROR);
                KioskStatic.logToFile(txtMessage.Text);
                log.LogMethodExit(false);
                return false;
            }
            CurrentTrx.SaveStartTime = KioskStatic.Utilities.getServerTime(); //object reference not set to an instance
            decimal trxValue;
            double ccSurcharge = 0;
            if (trxPaymentDTO != null && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsCreditCard && ccSurchargeAmount > 0)
            {
                ccSurcharge = ccSurchargeAmount;
            }

            trxValue = ac.totalValue - (decimal)ccSurcharge;

            string message = "";
            try
            {
                PaymentModeList paymentModeListBL = new PaymentModeList(KioskStatic.Utilities.ExecutionContext);
                List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                if ((trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed)
                    || (trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed == false
                       && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard)
                    || (trxPaymentDTO != null && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsCreditCard &&
                           ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString())))
                {
                    //if there is balance, then apply remaining as default Cash. THis needs to be verified as doesnt look right
                    CurrentTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                    double balance = Math.Abs((CurrentTrx.Net_Transaction_Amount + ccSurcharge) - trxPaymentDTO.Amount);
                    if (trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed == false
                       && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard)
                    {
                        log.Info("Gamecard used");
                        balance = Math.Abs(CurrentTrx.Net_Transaction_Amount - trxPaymentDTO.Amount - trxPaymentDTO.PaymentUsedCreditPlus);
                    }
                    log.Info("balance: " + balance.ToString());
                    if (balance > 0 && balance < 1)
                    {
                        if (paymentModeDTOList != null)
                        {
                            double balanceAmountValue = (CurrentTrx.Net_Transaction_Amount - trxPaymentDTO.Amount);
                            if (trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed == false
                                && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard)
                            {
                                log.Info("Gamecard used");
                                balanceAmountValue = (CurrentTrx.Net_Transaction_Amount - trxPaymentDTO.Amount - trxPaymentDTO.PaymentUsedCreditPlus);
                            }
                            log.Info("balanceAmountValue: " + balanceAmountValue.ToString());
                            TransactionPaymentsDTO trxBalanceCashPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId,
                                                                                  balanceAmountValue,
                                                                                  "", "", "", "", "", -1, "", -1, -1, "", "", false, -1, -1, "", Utilities.getServerTime(),
                                                                                   Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
                            trxBalanceCashPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                            CurrentTrx.TransactionPaymentsDTOList.Add(trxBalanceCashPaymentDTO);
                        }
                    }
                    else if (balance > 1)
                    {
                        KioskStatic.logToFile("Payment Remaining: " + (CurrentTrx.Net_Transaction_Amount - trxPaymentDTO.Amount).ToString() + ". Performing Abort and Exit");
                        abortAndExit(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Partial payment not allowed"));
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                //Check added for debit card transactions
                else if (trxPaymentDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard == true)
                {
                    CurrentTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                }
                else
                {
                    TransactionPaymentsDTO trxCashPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, (CurrentTrx.Net_Transaction_Amount),
                                                                                              "", "", "", "", "", -1, "", -1, -1, "", "", false, -1, -1, "", Utilities.getServerTime(),
                                                                                              Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
                    trxCashPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                    CurrentTrx.TransactionPaymentsDTOList.Add(trxCashPaymentDTO);
                }

                string trxMessage = "";
                int retcode = -1;
                {

                    if (CurrentTrx.TransactionPaymentsDTOList != null)
                    {
                        foreach (TransactionPaymentsDTO trxPaymentsDTO in CurrentTrx.TransactionPaymentsDTOList)
                        {
                            trxPaymentsDTO.TransactionId = -1;
                        }
                    }

                    if (ccSurcharge > 0)
                    {
                        CurrentTrx.PaymentCreditCardSurchargeAmount = ccSurcharge;
                    }

                    retcode = CurrentTrx.SaveOrder(ref trxMessage);

                    if (loyaltyCardNumber != string.Empty)
                        try
                        {
                            Utilities.executeNonQuery("update trx_header set External_System_Reference = @loyaltyCardNumber where trxId = @trxId",
                                                        new SqlParameter("@trxId", CurrentTrx.Trx_id),
                                                        new SqlParameter("@loyaltyCardNumber", loyaltyCardNumber));
                        }
                        catch (Exception ex)
                        {
                            KioskStatic.logToFile("Error While Updating External System Reference to trx_header Cloumn : " + ex.Message);
                        }
                }
                // Update the check in status
                if (retcode == 0 &&
                     CurrentTrx.TrxLines.Exists(x => x.ProductTypeCode == ProductTypeValues.CHECKIN && x.LineCheckInDTO != null))
                {
                    int checkInId = CurrentTrx.TrxLines.Where(x => x.ProductTypeCode == ProductTypeValues.CHECKIN && x.LineCheckInDTO != null).FirstOrDefault().LineCheckInDTO.CheckInId;
                    string checkInOptions = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CHECK_IN_OPTIONS_IN_POS");
                    bool cardMandatory = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "CARD_ISSUE_MANDATORY_FOR_CHECKIN_DETAILS");
                    CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext, checkInId, true, true);
                    ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(Utilities.ExecutionContext);
                    //  Update the status to Ordered when card is taped during direct check in. 
                    if (string.IsNullOrWhiteSpace(checkInOptions) == false && cardMandatory == false)
                    {
                        List<CheckInDetailDTO> checkInDetailDTOList = new List<CheckInDetailDTO>();

                        if (checkInOptions == "AUTO")
                        {
                            foreach (var checkInDetailDTO in checkInBL.CheckInDTO.CheckInDetailDTOList)
                            {
                                if (checkInDetailDTO != null && checkInDetailDTO.Status == CheckInStatus.PENDING)
                                {
                                    checkInDetailDTO.Status = CheckInStatus.CHECKEDIN;
                                    checkInDetailDTOList.Add(checkInDetailDTO);
                                }
                            }
                            if (checkInDetailDTOList.Any())
                            {
                                using (NoSynchronizationContextScope.Enter())
                                {
                                    Task<CheckInDTO> t = transactionUseCases.UpdateCheckInStatus(checkInBL.CheckInDTO.CheckInId, checkInDetailDTOList);
                                    t.Wait();
                                }
                            }
                        }
                    }
                }

                if (retcode != 0)
                {
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Error") + ": " + trxMessage, ERROR);

                    KioskStatic.logToFile("Error TrxSave: " + trxMessage);
                    MessageBox.Show(trxMessage);
                    log.LogMethodExit(false);
                    return false;
                }
                else
                {
                    CurrentTrx.SaveEndTime = KioskStatic.Utilities.getServerTime();

                    displayMessageLine(trxMessage, MESSAGE);
                    ac.TrxId = (int)CurrentTrx.Trx_id;
                    CurrentTrx.UpdateTrxHeaderSavePrintTime(CurrentTrx.Trx_id, CurrentTrx.SaveStartTime, CurrentTrx.SaveEndTime, null, null);
                    KioskStatic.logToFile("Success TrxSave: " + message);
                    log.LogMethodExit(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error while transaction SaveOrder");
                displayMessageLine(message + ":" + ex.Message, ERROR);
                KioskStatic.logToFile(message + ":" + ex.Message);
                MessageBox.Show(ex.StackTrace);
                log.LogMethodExit(false);
                return false;
            }
        }

        bool createTransaction()
        {
            log.LogMethodEntry();
            if (CurrentCard == null)
            {
                displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 504), ERROR);
                KioskStatic.logToFile(txtMessage.Text);
                log.LogMethodExit(false);
                return false;
            }

            if (CurrentTrx == null)
            {
                CurrentTrx = new Semnox.Parafait.Transaction.Transaction(KioskStatic.Utilities);
                CurrentTrx.PaymentReference = "Kiosk Transaction";
                CurrentTrx.PaymentMode = 1;
                KioskStatic.logToFile("New Trx object created");
            }
            CurrentTrx.SaveStartTime = KioskStatic.Utilities.getServerTime();
            decimal trxValue;
            double ccSurcharge = 0;
            if (trxPaymentDTO != null && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsCreditCard && ccSurchargeAmount > 0)
            {
                ccSurcharge = ccSurchargeAmount;
            }
            if (Function == "I")
            {
                if (DispensedCardCount == 1) // put the overpaid amount on first card
                {
                    FirstCard = CurrentCard;

                    if (ac.totalValue > (AmountRequired + (decimal)ccSurcharge))
                        trxValue = ProductPrice + ac.totalValue - (AmountRequired + (decimal)ccSurcharge);
                    else
                        trxValue = ProductPrice;
                }
                else
                    trxValue = ProductPrice;
            }
            else
            {
                FirstCard = CurrentCard;
                trxValue = ac.totalValue - (decimal)ccSurcharge;
            }

            string message = "";
            try
            {
                // single product is defined with a cardCount > 1. here the first card loads the actual product. other cards are issued 
                // with zero balance but later credits are transferred from first card.

                // or variable product is selected for new card issue
                if (MultipleCardsInSingleProduct && DispensedCardCount > 1)
                {
                    if (KioskStatic.Utilities.ParafaitEnv.CardDepositProductId < 0)
                    {
                        displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "CardDepositProduct not defined. Contact Manager"), ERROR);
                        log.LogMethodExit(false);
                        return false;
                    }
                    if (CurrentTrx.createTransactionLine(CurrentCard, KioskStatic.Utilities.ParafaitEnv.CardDepositProductId, Convert.ToDouble(ProductDeposit), 1, ref message) != 0)
                    {
                        displayMessageLine(message, ERROR);
                        KioskStatic.logToFile(message);
                        log.LogMethodExit(false);
                        return false;
                    }

                    trxValue += ProductDeposit;
                }
                else if (trxValue > ProductPrice && (selectedFundsDonationsList == null || selectedFundsDonationsList.Any() == false))
                {
                    if (KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId < 0)
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Variable Top-up product (Tax Inclusive) not found. Please contact Manager"));
                        log.LogMethodExit(false);
                        return false;
                    }
                    else
                    {
                        // create the highest possible variable product
                        decimal reducePrice = variableNewCardCheck(trxValue);

                        if (rowProduct["product_type"].ToString() == "VARIABLECARD")
                        {
                            if (trxValue - reducePrice > 0)
                            {
                                if (CreateSplitVariableProducts(Convert.ToInt32(rowProduct["product_id"]), (double)(trxValue - reducePrice), ref message) != 0)
                                {
                                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Error") + ": " + message, ERROR);
                                    KioskStatic.logToFile("Error TrxLine1: " + message);
                                    log.LogMethodExit(false);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            //if (CurrentTrx.createTransactionLine(CurrentCard, Convert.ToInt32(rowProduct["product_id"]), (double)ProductPrice, 1, ref message) != 0)
                            if (CurrentTrx.createTransactionLine(CurrentCard, Convert.ToInt32(rowProduct["product_id"]), -1, 1, ref message) != 0)
                            {
                                displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Error") + ": " + message, ERROR);
                                KioskStatic.logToFile("Error TrxLine1: " + message);
                                log.LogMethodExit(false);
                                return false;
                            }

                            message = "";
                            if (CreateSplitVariableProducts(KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId, (double)(trxValue - ProductPrice), ref message) != 0)
                            {
                                displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Error") + ": " + message, ERROR);
                                KioskStatic.logToFile("Error TrxVarLine2: " + message);
                                log.LogMethodExit(false);
                                return false;
                            }
                        }
                    }
                }
                else if (trxValue < ProductPrice && (trxValue != ProductPrice - discountAmount)) // this case occurs only for recharge
                {
                    if (KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId < 0)
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Variable Top-up product (Tax Inclusive) not found. Please contact Manager"));
                        log.LogMethodExit(false);
                        return false;
                    }
                    else
                    {
                        if (CreateSplitVariableProducts(KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId, (double)(trxValue), ref message) != 0)
                        {
                            displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Error") + ": " + message, ERROR);
                            KioskStatic.logToFile("Error TrxVarLine3: " + message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
                else
                {
                    decimal reducePrice = variableNewCardCheck(trxValue);
                    if (rowProduct["product_type"].ToString() == "VARIABLECARD")
                    {
                        if (trxValue - reducePrice > 0)
                        {
                            if (CreateSplitVariableProducts(Convert.ToInt32(rowProduct["product_id"]), (double)(trxValue - reducePrice), ref message) != 0)
                            {
                                displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Error") + ": " + message, ERROR);
                                KioskStatic.logToFile("Error TrxLine1: " + message);
                                log.LogMethodExit(false);
                                return false;
                            }
                        }
                    }
                    else
                    {

                        //if (CurrentTrx.createTransactionLine(CurrentCard, Convert.ToInt32(rowProduct["product_id"]), (double)ProductPrice, 1, ref message) != 0)
                        if (CurrentTrx.createTransactionLine(CurrentCard, Convert.ToInt32(rowProduct["product_id"]), -1, 1, ref message) != 0)
                        {
                            displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Error") + ": " + message, ERROR);
                            KioskStatic.logToFile("Error TrxLine4: " + message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
                try
                {
                    if (couponNumber != string.Empty && discountSummaryDTO != null)
                    {
                        CurrentTrx.ApplyCoupon(couponNumber);
                    }
                }
                catch (Exception ex)
                {
                    KioskStatic.logToFile("Error while applying discount " + ex.Message);
                }

                // either top-up trx or last card being issued
                if (Function != "I" || DispensedCardCount == CardCount)
                {
                    PaymentModeList paymentModeListBL = new PaymentModeList(KioskStatic.Utilities.ExecutionContext);
                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                    if ((trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed)
                        || (trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed == false
                           && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard)
                        || (trxPaymentDTO != null && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsCreditCard &&
                           ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString())))
                    {
                        //if there is balance, then apply remaining as default Cash. THis needs to be verified as doesnt look right
                        CurrentTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                        double balance = Math.Abs((CurrentTrx.Net_Transaction_Amount + ccSurcharge) - trxPaymentDTO.Amount);
                        if (trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed == false
                           && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard)
                        {
                            log.Info("Gamecard used");
                            balance = Math.Abs(CurrentTrx.Net_Transaction_Amount - trxPaymentDTO.Amount - trxPaymentDTO.PaymentUsedCreditPlus);
                        }
                        log.Info("balance: " + balance.ToString());
                        if (balance > 0 && balance < 1)
                        {
                            if (paymentModeDTOList != null)
                            {
                                double balanceAmountValue = (CurrentTrx.Net_Transaction_Amount - trxPaymentDTO.Amount);
                                if (trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed == false
                                    && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard)
                                {
                                    log.Info("Gamecard used");
                                    balanceAmountValue = (CurrentTrx.Net_Transaction_Amount - trxPaymentDTO.Amount - trxPaymentDTO.PaymentUsedCreditPlus);
                                }
                                log.Info("balanceAmountValue: " + balanceAmountValue.ToString());
                                TransactionPaymentsDTO trxBalanceCashPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId,
                                                                                      balanceAmountValue,
                                                                                      "", "", "", "", "", -1, "", -1, -1, "", "", false, -1, -1, "", Utilities.getServerTime(),
                                                                                       Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
                                trxBalanceCashPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                                //trxBalanceCashPaymentDTO.TransactionId = CurrentTrx.Trx_id;
                                CurrentTrx.TransactionPaymentsDTOList.Add(trxBalanceCashPaymentDTO);
                            }
                        }
                        else if (balance > 1)
                        {
                            KioskStatic.logToFile("Payment Remaining: " + (CurrentTrx.Net_Transaction_Amount - trxPaymentDTO.Amount).ToString() + ". Performing Abort and Exit");
                            abortAndExit(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Partial payment not allowed"));
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                    //Check added for debit card transactions
                    else if (trxPaymentDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard == true)
                    {
                        CurrentTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                    }
                    else
                    {
                        TransactionPaymentsDTO trxCashPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, (CurrentTrx.Net_Transaction_Amount),
                                                                                                  "", "", "", "", "", -1, "", -1, -1, "", "", false, -1, -1, "", Utilities.getServerTime(),
                                                                                                  Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
                        trxCashPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                        //trxCashPaymentDTO.TransactionId = CurrentTrx.Trx_id;
                        CurrentTrx.TransactionPaymentsDTOList.Add(trxCashPaymentDTO);
                        //CurrentTrx.StaticDataExchange.PaymentCashAmount = CurrentTrx.Net_Transaction_Amount;
                    }

                    string trxMessage = "";
                    int retcode = -1;
                    {
                        //if (CurrentTrx.WaiversSignedHistoryDTOList != null && CurrentTrx.WaiversSignedHistoryDTOList.Count > 0)
                        //{
                        //    foreach (WaiverSignatureDTO waiverSignedDTO in CurrentTrx.WaiversSignedHistoryDTOList)
                        //    {
                        //        waiverSignedDTO.IsWaiverSigned = true;
                        //    }
                        //}

                        if (CurrentTrx.TransactionPaymentsDTOList != null)
                        {
                            foreach (TransactionPaymentsDTO trxPaymentsDTO in CurrentTrx.TransactionPaymentsDTOList)
                            {
                                trxPaymentsDTO.TransactionId = -1;
                            }
                        }

                        if (ccSurcharge > 0)
                        {
                            CurrentTrx.PaymentCreditCardSurchargeAmount = ccSurcharge;
                        }

                        retcode = CurrentTrx.SaveTransacation(ref trxMessage);

                        if (loyaltyCardNumber != string.Empty)
                            try
                            {
                                Utilities.executeNonQuery("update trx_header set External_System_Reference = @loyaltyCardNumber where trxId = @trxId",
                                                            new SqlParameter("@trxId", CurrentTrx.Trx_id),
                                                            new SqlParameter("@loyaltyCardNumber", loyaltyCardNumber));
                            }
                            catch (Exception ex)
                            {
                                KioskStatic.logToFile("Error While Updating External System Reference to trx_header Cloumn : " + ex.Message);
                            }
                    }

                    if (retcode != 0)
                    {
                        displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Error") + ": " + trxMessage, ERROR);
                        KioskStatic.logToFile("Error TrxSave: " + trxMessage);
                        log.LogMethodExit(false);
                        return false;
                    }
                    else
                    {
                        CurrentTrx.SaveEndTime = KioskStatic.Utilities.getServerTime();
                        if (Function == "I" && customerDTO != null)
                        {
                            FirstCard.customerDTO = customerDTO;
                            FirstCard.updateCustomer();
                        }

                        // transfer credits from first card to other cards
                        // modified 02/2019: BearCat Split product entitlement -  get the original product deposit and send that as deposit
                        if (DispensedCardCount > 1 && MultipleCardsInSingleProduct)
                        {
                            double TotalTokens = 0;
                            double bonus = 0;
                            double courtesy = 0;
                            double tickets = 0;

                            int Trx_id = 0;
                            int Trx_line = 0;
                            List<int> childCardIds = new List<int>();

                            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in CurrentTrx.TrxLines)
                                tl.LineProcessed = false;

                            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in CurrentTrx.TrxLines)
                            {
                                if (FirstCard.Equals(tl.card))
                                {
                                    TotalTokens += tl.Credits;
                                    bonus += tl.Bonus;
                                    courtesy += tl.Courtesy;
                                    tickets += tl.Tickets;
                                    tl.LineProcessed = true;
                                    Trx_id = CurrentTrx.TransactionDTO.TransactionId;
                                    Trx_line = tl.DBLineId;
                                }
                                else
                                {
                                    if (tl.card != null)
                                        childCardIds.Add(tl.card.card_id);
                                }
                            }

                            if (FirstCard != null && childCardIds.Count > 0)
                            {
                                TaskProcs tp = new TaskProcs(Utilities);

                                KioskStatic.logToFile("Split entitlements: from " + FirstCard.card_id + " to " + String.Join(";", childCardIds) + " TrxId: " + Trx_id + " LineId:" + Trx_line);

                                try
                                {
                                    if (!tp.BalanceTransferWithCreditPlus(FirstCard.card_id, childCardIds, Convert.ToDecimal(TotalTokens.ToString()), Convert.ToDecimal(bonus.ToString()),
                                        Convert.ToDecimal(courtesy.ToString()), Convert.ToDecimal(tickets.ToString()), "", ref message, Trx_id, Trx_line, null))
                                    {
                                        displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Error") + ": " + message, ERROR);
                                        KioskStatic.logToFile("Failed to split product to child cards: from " + FirstCard.card_id + " to " + String.Join(";", childCardIds));
                                        KioskStatic.logToFile(" reason: " + message);
                                    }
                                    KioskStatic.logToFile("Split entitlements: from " + FirstCard.card_id + " to " + String.Join(";", childCardIds));
                                }
                                catch (Exception ex)
                                {
                                    string errMsg1 = "Failed to split product to child cards: from " + FirstCard.card_id + " to " + String.Join(";", childCardIds);
                                    KioskStatic.logToFile(errMsg1);
                                    KioskStatic.logToFile(" reason: " + message);
                                    KioskStatic.logToFile(message + ":" + ex.Message);
                                    log.Error(errMsg1);
                                    log.Error(ex);

                                    string errDisplayMsg1 = "Failed to split points to all the child cards.";
                                    string errDisplayMsg2 = MessageContainerList.GetMessage(Utilities.ExecutionContext, 3012);//Please contact attendant
                                    displayMessageLine(errDisplayMsg1, ERROR);
                                    if (KioskStatic.EnableTransfer)
                                    {
                                        errDisplayMsg2 += " or transfer the points from Transfer menu button in the home screen";
                                    }
                                    showOK(errDisplayMsg1 + Environment.NewLine + errDisplayMsg2);
                                }
                                foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in CurrentTrx.TrxLines)
                                {
                                    if (!tl.LineProcessed && tl.card != null)
                                    {
                                        tp.LinkChildCard(FirstCard.card_id, tl.card.card_id);

                                        foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tlc in CurrentTrx.TrxLines)
                                        {
                                            if (tl.card.Equals(tlc.card))
                                                tlc.LineProcessed = true;
                                        }
                                    }
                                }

                            }
                        }
                        if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0 && selectedEntitlementType == "Time")
                        {
                            TaskProcs tp = new TaskProcs(KioskStatic.Utilities);
                            int each = 0;
                            double firstEach = 0;
                            double TotalTokens = 0;
                            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in CurrentTrx.TrxLines)
                            {
                                TotalTokens += tl.Credits;
                                tl.LineProcessed = false;
                            }
                            if (Function == "I")
                            {
                                each = (int)(TotalTokens / DispensedCardCount);
                                firstEach = TotalTokens - (each * DispensedCardCount);
                            }
                            else
                                each = (int)(TotalTokens);

                            List<int> childCardIds = new List<int>();
                            //List<int> childCardIds = (CurrentTrx.TrxLines.FindAll(tl => !FirstCard.Equals(tl.card)).ToList();
                            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in CurrentTrx.TrxLines)
                            {
                                if (!FirstCard.Equals(tl.card))
                                {
                                    if (tl.card != null)
                                        childCardIds.Add(tl.card.card_id);
                                }
                            }
                            bool callSaveTrx = false;
                            try
                            {
                                foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in CurrentTrx.TrxLines)
                                {
                                    if (!tl.LineProcessed && tl.card != null)
                                    {
                                        Card convertCard = new Card(tl.card.CardNumber, KioskStatic.Utilities.ParafaitEnv.LoginID, KioskStatic.Utilities);

                                        bool sv = tp.ConvertCreditsForTime(convertCard, (FirstCard.Equals(tl.card)) ? (firstEach + each) : each, CurrentTrx.Trx_id, tl.DBLineId, multiPointConversionRequired, "Kiosk Trx: convert Credit to Time", ref message);
                                        if (!sv)
                                        {
                                            displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Error") + ": " + message, ERROR);
                                            KioskStatic.logToFile("Trx transfer: " + message);
                                        }

                                        callSaveTrx = true;
                                        foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tlc in CurrentTrx.TrxLines)
                                        {
                                            if (tl.card.Equals(tlc.card))
                                                tlc.LineProcessed = true;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string errMsg1 = "Failed to split product to child cards: from " + FirstCard.card_id + " to " + String.Join("; ", childCardIds);
                                KioskStatic.logToFile(errMsg1);
                                KioskStatic.logToFile(" reason: " + message);
                                KioskStatic.logToFile(message + ":" + ex.Message);
                                log.Error(errMsg1);
                                log.Error(ex);

                                string errDisplayMsg1 = "Failed to split time to all the child cards.";
                                string errDisplayMsg2 = MessageContainerList.GetMessage(Utilities.ExecutionContext, 3012); //Please contact attendant
                                displayMessageLine(errDisplayMsg1, ERROR);
                                if (KioskStatic.EnableTransfer)
                                {
                                    errDisplayMsg2 += " or transfer the time from Transfer menu button in the home screen";
                                }
                                showOK(errDisplayMsg1 + Environment.NewLine + errDisplayMsg2);
                            }
                            if (callSaveTrx)
                            {
                                bool returnValue = false;
                                returnValue = CurrentTrx.CompleteTransaction(null, ref trxMessage);

                                if (returnValue == false)
                                {
                                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Error") + ": " + trxMessage, ERROR);
                                    KioskStatic.logToFile("Error TrxSave: " + trxMessage);
                                    log.LogMethodExit(false);
                                    return false;
                                }
                            }
                        }

                        displayMessageLine(trxMessage, MESSAGE);
                        ac.TrxId = (int)CurrentTrx.Trx_id;
                        CurrentTrx.UpdateTrxHeaderSavePrintTime(CurrentTrx.Trx_id, CurrentTrx.SaveStartTime, CurrentTrx.SaveEndTime, null, null);
                        KioskStatic.logToFile("Success TrxSave: " + message);
                        log.LogMethodExit(true);
                        return true;
                    }
                }
                else
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(message + ":" + ex.Message, ERROR);
                KioskStatic.logToFile(message + ":" + ex.Message);
                MessageBox.Show(ex.StackTrace);
                log.LogMethodExit(false);
                return false;
            }
        }


        int cardDispenseRetryCount = 3;
        public void dispenseCards(Semnox.Parafait.KioskCore.CardDispenser.CardDispenser cardDispenser)
        {
            log.LogMethodEntry(cardDispenser);
            if (cancelPressed)
            {
                log.LogMethodExit();
                return;

            }

            KioskStatic.logToFile("Dispensing Cards");
            cardDispenseRetryCount = 3;
            string cardNumber = "";
            KioskStatic.logToFile("Card number for Wrist band Printer   :" + wristBandPrintTag);
            log.Info("Card number for Wrist band Printer  :" + wristBandPrintTag);
            bool newCardNumber;
            List<int> autoGenCardTrxLineInfo = new List<int>();
            try
            {
                if (Function == "I")
                {
                    if (ac.totalValue > 0 || ProductPrice == 0 || AmountRequired == 0)
                    {
                        Audio.PlayAudio(Audio.WaitForCardDispense);
                        string message = "";

                        while (true)
                        {
                            cardNumber = "";
                            bool succ = false;
                            Thread.Sleep(300);
                            newCardNumber = false;
                            if (wristBandPrintTag && CardCount != DispensedCardCount)
                            {

                                while (!newCardNumber)
                                {
                                    cardNumber = KioskStatic.GetTagNumber();
                                    AccountListBL accountBL = new AccountListBL(KioskStatic.Utilities.ExecutionContext);
                                    List<KeyValuePair<AccountDTO.SearchByParameters, string>> accountSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                                    accountSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.TAG_NUMBER, newCardNumber.ToString()));
                                    List<AccountDTO> accountListDTO = accountBL.GetAccountDTOList(accountSearchParameters, true, true);
                                    if (accountListDTO == null || accountListDTO.Count == 0)
                                    {
                                        CurrentCard = new Card(cardNumber, Utilities.ExecutionContext.GetUserId(), Utilities);
                                        newCardNumber = true;
                                        transactionHasCardToPrint = true;
                                        KioskStatic.logToFile("New card number is generated : " + cardNumber);
                                        log.Info("New card number is generated : " + cardNumber);
                                    }
                                }

                            }
                            else    //if (!autoGeneratedCardNumber) // other card products
                            {
                                if (KioskStatic.config.dispport == -1)
                                {
                                    log.Info("Card dispenser is disabled , dispport == -1");
                                    KioskStatic.logToFile("Card dispenser is disabled , dispport == -1");
                                    showOK(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2384));
                                    return;
                                }

                                succ = cardDispenser.doDispenseCard(ref cardNumber, ref message);
                                if (!succ)
                                {
                                    KioskStatic.logToFile(message);
                                    if (cardDispenser.criticalError)
                                    {
                                        showOK(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 441) +
                                          Environment.NewLine + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Dispenser Error") + ": " +
                                          message + Environment.NewLine +
                                          MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Please fix the error and press Close to continue"), false);
                                    }
                                    else
                                    {
                                        Thread.Sleep(1500);
                                        Application.DoEvents();
                                        KioskStatic.logToFile(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Dispenser Error") + ": " + message + MessageContainerList.GetMessage(Utilities.ExecutionContext, ". Retrying..."));
                                    }
                                    cardDispenseRetryCount--;
                                    if (cardDispenseRetryCount > 0)
                                    {
                                        txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Dispense Failed. Retrying") + " [" + (3 - cardDispenseRetryCount).ToString() + "]";
                                        KioskStatic.logToFile(txtMessage.Text);
                                        continue;
                                    }
                                    else
                                    {
                                        txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Unable to issue card after MAX retries. Contact Staff.");
                                        abortAndExit(txtMessage.Text + " " + message);
                                        log.LogMethodExit();
                                        return;
                                    }
                                }
                                else if (string.IsNullOrEmpty(cardNumber))
                                {
                                    txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Card Dispensed but not read. Rejecting");
                                    KioskStatic.logToFile(txtMessage.Text);
                                    Thread.Sleep(300);
                                    if (!cardDispenser.doRejectCard(ref message))
                                    {
                                        displayMessageLine(message, ERROR);
                                        abortAndExit(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Card dispenser error.") + " " + txtMessage.Text + "; " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Unable to reject card") + ": " + message);
                                        log.LogMethodExit();
                                        return;
                                    }
                                    cardDispenseRetryCount--;
                                    if (cardDispenseRetryCount > 0)
                                    {
                                        Thread.Sleep(2000);
                                        Application.DoEvents();
                                        continue;
                                    }
                                    else
                                    {
                                        string savMsg = txtMessage.Text;
                                        txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Unable to issue card after MAX retries. Contact Staff.");
                                        abortAndExit(savMsg + "; " + txtMessage.Text);
                                        log.LogMethodExit();
                                        return;
                                    }
                                }
                                else
                                {
                                    txtMessage.Text = "";
                                    try
                                    {
                                        if (KioskStatic.CardDispenserModel.Equals(Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models.SCT0M0))
                                            KioskStatic.DispenserReaderDevice = new DeviceClass();

                                        if (ParafaitEnv.MIFARE_CARD)
                                        {
                                            CurrentCard = new MifareCard(KioskStatic.DispenserReaderDevice, cardNumber, "External POS", Utilities);
                                        }
                                        else
                                        {
                                            CurrentCard = new Card(KioskStatic.DispenserReaderDevice, cardNumber, "External POS", Utilities);
                                        }

                                        if (CurrentCard != null)
                                        {
                                            if (KioskHelper.CardRoamingRemotingClient != null
                                                || (KioskStatic.Utilities.ParafaitEnv.ALLOW_ROAMING_CARDS == "Y"
                                                     && KioskStatic.Utilities.ParafaitEnv.ENABLE_ON_DEMAND_ROAMING == "Y"))
                                            {
                                                txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1607);//Refreshing Card from HQ. Please Wait...
                                            }
                                            //Application.DoEvents();
                                            if (!KioskHelper.refreshCardFromHQ(ref CurrentCard, ref message))
                                            {
                                                KioskStatic.logToFile("Unable refresh Card From HQ [" + CurrentCard.CardNumber + " ]" + " error: " + message);
                                                log.Info("Unable to refresh Card From HQ [" + CurrentCard.CardNumber + " ]" + " error: " + message);
                                                txtMessage.Text = message;
                                                throw new Exception(message);
                                            }
                                            if (CurrentCard.CardStatus.Equals("ISSUED") && CurrentCard.technician_card.Equals('N'))
                                            {
                                                TaskProcs tp = new TaskProcs(KioskStatic.Utilities);
                                                string refundMsg = "";
                                                if (!tp.RefundCard(CurrentCard, 0, 0, 0, "Deactivated By Kiosk", ref refundMsg, true))
                                                {
                                                    txtMessage.Text = refundMsg;
                                                    KioskStatic.logToFile("Unable to Deactivate card [" + CurrentCard.CardNumber + " ]" + " error: " + refundMsg);
                                                    log.Info("Unable to Deactivate card [" + CurrentCard.CardNumber + " ]" + " error: " + refundMsg);
                                                    CurrentCard = null;
                                                }
                                                else
                                                {
                                                    CurrentCard.invalidateCard(null);
                                                    CurrentCard = new Card(CurrentCard.CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
                                                }

                                            }
                                            if (CurrentCard.technician_card.Equals('Y'))
                                            {
                                                KioskStatic.logToFile("Technician cards cannot be dispensed. card number: [" + CurrentCard.CardNumber + " ]");
                                                log.Info("Technician cards cannot be dispensed. card number: [" + CurrentCard.CardNumber + " ]");
                                                CurrentCard = null;
                                            }
                                        }
                                        if (KioskStatic.CardDispenserModel.Equals(Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models.SCT0M0))
                                            KioskStatic.DispenserReaderDevice = null;
                                    }
                                    catch (Exception ex)
                                    {
                                        txtMessage.Text = ex.Message;
                                        KioskStatic.logToFile(ex.Message);
                                        log.Error(ex);
                                        CurrentCard = null;
                                    }
                                }
                            }
                            if (CurrentCard == null)
                                TransactionSuccess = false;
                            else
                                TransactionSuccess = true;

                            if (TransactionSuccess)
                            {
                                DispensedCardCount++;
                                TransactionSuccess = createTransaction();
                                KioskStatic.logToFile("CreateTransaction returned " + TransactionSuccess.ToString());
                                if (!TransactionSuccess)
                                    DispensedCardCount--;
                            }

                            if (TransactionSuccess)
                            {
                                if (!wristBandPrintTag)
                                {
                                    KioskStatic.updateKioskActivityLog(-1, -1, cardNumber, "NEWCARD", rowProduct["product_name"].ToString(), ac);
                                    Thread.Sleep(300);
                                    cardDispenser.doEjectCard(ref message);

                                    while (true)
                                    {
                                        Thread.Sleep(300);
                                        int cardPosition = 0;
                                        if (cardDispenser.checkStatus(ref cardPosition, ref message))
                                        {
                                            if (cardPosition >= 2)
                                            {
                                                message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 393);
                                                showOK(message);
                                                KioskStatic.logToFile("Card dispensed. Waiting to be removed.");
                                            }
                                            else
                                            {
                                                KioskStatic.logToFile("Card removed.");
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    Transaction.Transaction.TransactionLine cardLine = CurrentTrx.TrxLines.Find(tl => tl.LineValid && tl.CardNumber == cardNumber);
                                    if (cardLine != null)
                                    {
                                        int lineIndex = CurrentTrx.TrxLines.IndexOf(cardLine);
                                        autoGenCardTrxLineInfo.Add(lineIndex);
                                    }
                                }

                                if (CardCount == DispensedCardCount)
                                {

                                    bool printed = false;
                                    if (!printReceipt && transactionHasCardToPrint == true)
                                    {
                                        log.Debug("transactionHasCardToPrint == true");
                                        if (PrintWithoutTransactionReceipt())
                                        {
                                            displayMessageLine(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 435), WARNING);
                                            printed = true;
                                            log.LogVariableState("autoGenCardTrxLineInfo", autoGenCardTrxLineInfo);
                                            LogAutoGenCardActivity(autoGenCardTrxLineInfo);
                                        }
                                        else
                                        {
                                            // abortAndExit(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext,"PLEASE CONTACT ATTENDANT.") + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext,2386));
                                            GenerateCardPrintErrorReceipt("NEWCARD", rowProduct["product_name"].ToString(), autoGenCardTrxLineInfo);
                                            log.LogMethodExit();
                                            return;
                                        }
                                    }

                                    if (KioskStatic.receipt == true)
                                    {
                                        log.Debug("KioskStatic.receipt == true");
                                        try
                                        {
                                            if (print_receipt())
                                            {
                                                log.Debug("prints both wrist band card and receipt");
                                                if (cardPrinterError)
                                                {
                                                    log.Error(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2386));
                                                    GenerateCardPrintErrorReceipt("NEWCARD", rowProduct["product_name"].ToString(), autoGenCardTrxLineInfo);
                                                    // abortAndExit(MessageContainerList.GetMessage(Utilities.ExecutionContext,"PLEASE CONTACT ATTENDANT.") + MessageContainerList.GetMessage(Utilities.ExecutionContext,2386));
                                                    log.LogMethodExit();
                                                    return;
                                                }
                                                else
                                                {
                                                    displayMessageLine(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 435), WARNING);
                                                    printed = true;
                                                    log.LogVariableState("autoGenCardTrxLineInfo", autoGenCardTrxLineInfo);
                                                    LogAutoGenCardActivity(autoGenCardTrxLineInfo);
                                                }
                                            }
                                            else
                                                displayMessageLine(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 437), WARNING);

                                            Audio.PlayAudio(Audio.CollectCardAndReceipt, Audio.ThankYouEnjoyGame);
                                        }
                                        catch (Exception ex)
                                        {
                                            displayMessageLine(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 436, ex.Message), ERROR);
                                            Audio.PlayAudio(Audio.CollectCard, Audio.ThankYouEnjoyGame);
                                        }
                                    }
                                    else
                                    {
                                        displayMessageLine(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 437), WARNING);
                                    }
                                    ac.totalValue = 0;
                                    showThankYou(printed);
                                    log.LogMethodExit();
                                    return;
                                }
                                else
                                    cardDispenseRetryCount = 3;
                            }
                            else
                            {
                                cardDispenseRetryCount--;
                                KioskStatic.updateKioskActivityLog(-1, -1, cardNumber, "NEWCARD", rowProduct["product_name"].ToString() + ": Failed", ac);

                                if (!cardDispenser.doRejectCard(ref message))
                                {
                                    displayMessageLine(message, ERROR);
                                    abortAndExit(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Card dispenser error. Unable to reject card") + ": " + message);
                                    log.LogMethodExit();
                                    return;
                                }

                                if (cardDispenseRetryCount > 0)
                                {
                                    txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Card issue failed. Retrying") + " [" + (3 - cardDispenseRetryCount).ToString() + "]";
                                    KioskStatic.logToFile(txtMessage.Text);
                                    continue;
                                }
                                else
                                {
                                    txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Unable to issue card after MAX retries. Contact Staff.");
                                    abortAndExit(txtMessage.Text);
                                    log.LogMethodExit();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("CardDispense: " + ex.Message + "-" + ex.StackTrace);
                MessageBox.Show(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "PLEASE CONTACT ATTENDANT") + ": " + ex.Message);
                KioskStatic.updateKioskActivityLog(-1, -1, cardNumber, "NEWCARD", rowProduct["product_name"].ToString() + ":" + ex.Message, ac);
                if (btnCancel != null)
                    btnCancel.Enabled = true;
                abortAndExit(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "CardDispense Read") + ":" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LogAutoGenCardActivity(List<int> autoGenCardTrxLineInfo)
        {
            log.LogMethodEntry(autoGenCardTrxLineInfo);
            TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
            Semnox.Parafait.Transaction.Transaction trx = TransactionUtils.CreateTransactionFromDB(CurrentTrx.Trx_id, Utilities);
            for (int i = 0; i < autoGenCardTrxLineInfo.Count; i++)
            {
                string replacedCardNoForAutoGen = trx.TrxLines[autoGenCardTrxLineInfo[i]].CardNumber;
                KioskStatic.updateKioskActivityLog(-1, -1, replacedCardNoForAutoGen, "NEWCARD", rowProduct["product_name"].ToString(), ac);
            }
            log.LogMethodExit();
        }

        public void print_receipt(bool isAbort)
        {
            log.LogMethodEntry(isAbort);
            if (KioskStatic.isUSBPrinter)
                print_receiptUSB(isAbort);
            //else
            //    print_receiptSP(isAbort);
            log.LogMethodExit();
        }

        bool print_receipt()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("PrintReceipt: " + printReceipt.ToString());
            if (printReceipt)
                print_receipt(false);
            log.LogMethodExit(printReceipt);
            return printReceipt;
        }

        /// <summary>
        /// This method Skips the receipt printer from the EligiblePrintabletransactionLines list  
        /// </summary>
        /// <returns></returns>
        bool PrintWithoutTransactionReceipt()
        {
            log.LogMethodEntry();
            string message = string.Empty;
            KioskStatic.logToFile("Transaction has card/Ticket to print: " + transactionHasCardToPrint.ToString());
            PrintTransaction printTransaction = new PrintTransaction();
            printTransaction.PrintProgressUpdates = new PrintTransaction.ProgressUpdates(PrintProgressUpdates);
            printTransaction.SetCardPrinterErrorValue = new PrintTransaction.SetCardPrinterError(SetCardPrinterErrorValue);
            TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
            Semnox.Parafait.Transaction.Transaction trx = TransactionUtils.CreateTransactionFromDB(CurrentTrx.Trx_id, Utilities);
            if (trx.POSPrinterDTOList == null || trx.POSPrinterDTOList.Count == 0)
            {
                POSMachines posMachine = new POSMachines(trx.Utilities.ExecutionContext, trx.Utilities.ParafaitEnv.POSMachineId);
                trx.POSPrinterDTOList = posMachine.PopulatePrinterDetails();
            }
            POSPrinterListBL posPrinterBL = new POSPrinterListBL(CurrentTrx.Utilities.ExecutionContext);
            trx.POSPrinterDTOList = posPrinterBL.RemovePrinterType(trx.POSPrinterDTOList, PrinterDTO.PrinterTypes.ReceiptPrinter);
            trx.GetPrintableTransactionLines(trx.POSPrinterDTOList);
            try
            {
                if (!printTransaction.Print(trx, -1, ref message))
                {
                    string msg = MessageContainerList.GetMessage(CurrentTrx.Utilities.ExecutionContext, 2386) +
                        ": " + message;
                    displayMessageLine(msg, WARNING); // 2386 - Card printing error.
                    KioskStatic.logToFile("Cardprint Error: " + message);
                    KioskStatic.updateKioskActivityLog(-1, -1, string.Empty, "PRINT-ERROR", message, ac);
                    log.Info(msg);
                    showOK(msg);
                    log.LogMethodExit("PrintTransaction.Print failed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Cardprint Error: " + ex.Message);
                KioskStatic.updateKioskActivityLog(-1, -1, string.Empty, "PRINT-ERROR", ex.Message, ac);
                showOK(ex.Message);
                log.LogMethodExit("Unexpected error in PrintTransaction.Print");
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        void print_receiptUSB(bool isAbort)
        {
            log.LogMethodEntry(isAbort);
            KioskStatic.logToFile("print_receiptUSB; isAbort = " + isAbort.ToString());
            if (isAbort)
            {
                GenerateAbortPrint(Utilities.ExecutionContext, ParafaitEnv.POSMachine, ac, rc, trxPaymentDTO);
            }
            else
            {
                TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
                Semnox.Parafait.Transaction.Transaction trx = TransactionUtils.CreateTransactionFromDB(CurrentTrx.Trx_id, Utilities);
                PrintTransaction printTransaction = new PrintTransaction(KioskStatic.POSMachineDTO.PosPrinterDtoList);
                printTransaction.PrintProgressUpdates = new PrintTransaction.ProgressUpdates(PrintProgressUpdates);
                printTransaction.SetCardPrinterErrorValue = new PrintTransaction.SetCardPrinterError(SetCardPrinterErrorValue);
                string message = "";
                if (!printTransaction.Print(trx, ref message))
                {
                    log.Error(message);
                    KioskStatic.logToFile(message);
                    KioskStatic.updateKioskActivityLog(-1, -1, string.Empty, "PRINT-ERROR", message, ac);
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "PLEASE CONTACT ATTENDANT. ") + MessageContainerList.GetMessage(Utilities.ExecutionContext, 3010), WARNING);
                    showOK(MessageContainerList.GetMessage(Utilities.ExecutionContext, "PLEASE CONTACT ATTENDANT. ") + message);
                    // 3010 - Error while printing wrist bands
                }
                KioskStatic.logToFile("Print message: " + message);
                //List<int> trxIdList = new List<int>();

                //trxIdList.Add(CurrentTrx.Trx_id);


                //PrintMultipleTransactions printMultipleTransactions = new PrintMultipleTransactions(KioskStatic.Utilities);
                //if (!printMultipleTransactions.Print(trxIdList, false, ref message))
                //{
                //    log.Error(message);
                //    KioskStatic.logToFile(message);
                //    foreach (string errorCode in stimaPrinterErrorCode)
                //    {
                //        if (message.Contains(errorCode))
                //        {
                //            cardPrinterError = true;
                //            KioskStatic.logToFile("errorCode :" + errorCode);
                //            log.Error("cardPrinterError : " + errorCode);
                //            return;
                //        }
                //    }

                //    KioskStatic.logToFile(message);
                //    displayMessageLine(message, WARNING);
                //}
                //KioskStatic.logToFile("Print message: " + message);
            }
            log.LogMethodExit();
        }

        private static void PrintUSB(System.Drawing.Printing.PrintPageEventArgs e, List<string> input)
        {
            log.LogMethodEntry(input);
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

        private void displayMessageLine(string message, string msgType)
        {
            log.LogMethodEntry(message, msgType);
            Application.DoEvents();
            txtMessage.Text = message;
            log.LogMethodExit();
        }

        public void cancelCCPayment()
        {
            log.LogMethodEntry();
            if (trxPaymentDTO != null && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString()))
            {
                ReverseFiscalization();
                return;
            }
            if (trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed)
            {
                string lclmessage = "";
                if (CreditCardPaymentGateway.RefundAmount(trxPaymentDTO, Utilities, ref lclmessage))
                {
                    ac.totalValue -= (decimal)trxPaymentDTO.Amount;
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Credit Card Payment Refunded"), WARNING);
                    KioskStatic.logToFile("CC amount refunded: " + trxPaymentDTO.Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                }
                else
                {
                    KioskStatic.logToFile("CC refund failed: " + trxPaymentDTO.Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Unable to Refund Credit Card Payment"), WARNING);
                }

                Thread.Sleep(4000);
            }
            log.LogMethodExit();
        }

        //staticDataExchange.PaymentModeDetail paymentModeDetail = null;
        public void CreditCardPayment(bool IsDebitCard)
        {
            log.LogMethodEntry(IsDebitCard);
            ccSurchargeAmount = 0;
            double SurchargePercentage = (double)_PaymentModeDTO.CreditCardSurchargePercentage;
            ccSurchargeAmount = (double)(AmountRequired - ac.totalValue) * SurchargePercentage / 100.0; /*KioskStatic.ccPaymentModeDetails.SurchargePercentage / 100.0;*/
            ccSurchargeAmount = Math.Round(ccSurchargeAmount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
            if (ccSurchargeAmount < 0)
                ccSurchargeAmount = 0;
            trxPaymentDTO = new TransactionPaymentsDTO(-1, ac.KioskTrxId, _PaymentModeDTO.PaymentModeId, ((double)(AmountRequired - ac.totalValue) + ccSurchargeAmount)
                                                          , qrCodeScanned, "", "", "", "", -1, "", -1, -1, "", "", false, -1, -1, "", KioskStatic.Utilities.getServerTime(),
                                                       Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
            trxPaymentDTO.paymentModeDTO = _PaymentModeDTO;
            if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString()))
            {
                SendCreditCardTrxInfoToVCATDevice();
                return;
            }
            if (Enum.IsDefined(typeof(PaymentGateways), _PaymentModeDTO.GatewayLookUp))
                trxPaymentDTO.paymentModeDTO.GatewayLookUp = (PaymentGateways)Enum.Parse(typeof(PaymentGateways), _PaymentModeDTO.GatewayLookUp.ToString(), true);
            else
            {
                //   paymentModeDetail.Gateway = CreditCardPaymentGateway.Gateways.None;
                displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Payment Gateway set up missing. Please contact Manager."), ERROR);
                KioskStatic.logToFile("Payment Gateway for Credit Card is not selected. Set up Error.");
                log.LogMethodExit();
                return;
            }
            PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(trxPaymentDTO.paymentModeDTO.GatewayLookUp);
            paymentGateway.IsCreditCard = !IsDebitCard;
            paymentGateway.PrintReceipt = printReceipt;

            string message = "";

            if (paymentGateway.IsPrinterRequired)
            {
                bool Isprinter = false;//Stres the printer status
                Isprinter = printerStatus(ref message);//checks for printer status.
                if (!Isprinter)//without printer quest transactions are not allowed
                {
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Due to the printer error, transaction cannot be processed.") + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Error") + ":" + message, MessageContainerList.GetMessage(Utilities.ExecutionContext, "Printer error"));
                    KioskStatic.logToFile("Due to the printer error, transaction cannot be processed. Error:" + message);
                    log.LogMethodExit();
                    return;
                }
            }

            if (CreditCardPaymentGateway.MakePayment(trxPaymentDTO, Utilities, ref message))
            {
                CreditCardPaymentGateway.PrintCCReceipt(trxPaymentDTO);
                ac.totalValue += (decimal)trxPaymentDTO.Amount;
                if (KioskStatic.debugMode) MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Credit card payment success"));
                KioskStatic.logToFile("Credit card payment success");
                //Added check to see if Partial approval is not allowed and if credit card is partially approved then abort and exit
                //current transaction
                if (KioskStatic.Utilities.getParafaitDefaults("ALLOW_PARTIAL_APPROVAL").Equals("N") && ac.totalValue < AmountRequired)
                {
                    abortAndExit(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Partial Approval not allowed"));
                    log.LogMethodExit();
                    return;
                }
                else if (ac.totalValue > 0 && ac.totalValue < AmountRequired)
                {
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2854, ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                    displayMessageLine(msg, WARNING);
                    KioskStatic.logToFile(msg);
                    log.Info(msg);
                }
                if (paymentGateway.IsPartiallyApproved)
                {
                    if (ac.totalValue < AmountRequired)//Quest:Starts//Checking for difference amount
                    {
                        showOK(message);
                    }
                }
            }
            else
            {
                ccSurchargeAmount = 0;//reset surcharge on failures
                if (ac.totalValue > 0 == false)
                {
                    if (fLPPModes != null)
                    {
                        fLPPModes.Visible = true;
                        foreach (Control c in fLPPModes.Controls)
                        {
                            if (c.Text.ToLower().Contains("cash"))
                            {
                                fLPPModes.Controls.Remove(c);
                            }
                        }
                    }
                    //Modified as part of Check-In Check-Out feature in Kiosk
                    //implemented to go back to Payment Mode screen when Cancel is pressed. With this change, payment option in frmCardTransaction has no significance.
                    fLPPModes.Enabled = false;
                }
                displayMessageLine(message, WARNING);
                KioskStatic.logToFile(message);
                KioskStatic.logToFile("CC payment failure");
            }
            if (ac.totalValue > 0 && ac.totalValue < AmountRequired)
            {
                fLPPModes.Enabled = false;
            }
            log.LogMethodExit();
        }

        public void GameCardPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            trxPaymentDTO = transactionPaymentsDTO;
            ac.totalValue += (decimal)(trxPaymentDTO.Amount + trxPaymentDTO.PaymentUsedCreditPlus);
            log.LogMethodExit();
        }

        private bool printerStatus(ref string message)//Quest:Starts
        {
            log.LogMethodEntry(message);
            //This method will checks and retuns the printer status and status text will be placed in ref variable.
            try
            {
                PrinterSettings settings = new PrinterSettings();
                PrintQueueCollection printQueues = null;
                PrintServer printServer = new PrintServer();
                printQueues = printServer.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local });
                foreach (PrintQueue printQueue in printQueues)
                {
                    if (settings.PrinterName == printQueue.Name)//here settings.PrinterName always point to the defualt printer
                    {
                        //Checking for all the possible status.
                        if (printQueue.HasPaperProblem || printQueue.IsOutOfPaper ||
                            printQueue.IsOffline || printQueue.HasPaperProblem || !printQueue.HasToner ||
                            printQueue.IsNotAvailable || printQueue.IsBusy || printQueue.IsDoorOpened ||
                            printQueue.IsInitializing || printQueue.IsPaperJammed ||
                            printQueue.IsOutOfMemory || printQueue.IsTonerLow || printQueue.IsInError)
                        {
                            message = printQueue.QueueStatus.ToString();
                            printServer.Dispose();
                            printQueues.Dispose();
                            printQueue.Dispose();
                            log.LogMethodExit(false);
                            return false;
                        }
                        else
                        {
                            printServer.Dispose();
                            printQueues.Dispose();
                            printQueue.Dispose();
                        }
                    }
                }
            }
            catch
            {
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }//Quest:Ends

        private void PrintProgressUpdates(string message)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Print Progress Updates: " + message);
            log.Info("Print Progress Updates: " + message);
            txtMessage.Text = message;
            log.LogMethodExit();
        }

        private void GenerateCardPrintErrorReceipt(string activity, string productName, List<int> autoGenCardTrxLineInfo)
        {
            log.LogMethodEntry(activity, productName, autoGenCardTrxLineInfo);
            KioskStatic.logToFile("Generate Card Print Error Receipt");
            try
            {
                if (coinAcceptor != null)
                    coinAcceptor.disableCoinAcceptor();
                if (billAcceptor != null)
                    billAcceptor.disableBillAcceptor();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while disabling Coin/Bill Acceptors " + ex.Message);
            }
            try
            {
                PrintTransaction printTransaction = new PrintTransaction();
                TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
                Semnox.Parafait.Transaction.Transaction trx = TransactionUtils.CreateTransactionFromDB(CurrentTrx.Trx_id, Utilities);
                LogAutoGenCardActivity(trx, productName, activity, autoGenCardTrxLineInfo);
                if (trx.POSPrinterDTOList == null || trx.POSPrinterDTOList.Count == 0)
                {
                    POSMachines posMachine = new POSMachines(trx.Utilities.ExecutionContext, trx.Utilities.ParafaitEnv.POSMachineId);
                    trx.POSPrinterDTOList = posMachine.PopulatePrinterDetails();
                }
                //retain one receipt printer
                RetainReceiptPrinters(trx);
                TemporaryInvalidationOfPrintedLines(trx);
                trx.GetPrintableTransactionLines(trx.POSPrinterDTOList);
                int cardPrintErrorReceiptTemplate = ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "CARD_PRINT_ERROR_RECEIPT_TEMPLATE", -1);
                if (cardPrintErrorReceiptTemplate == -1)
                {
                    // CARD_PRINT_ERROR_RECEIPT_TEMPLATE is not found but still proceed with template defined by the printer.
                    txtMessage.Text = (MessageContainerList.GetMessage(Utilities.ExecutionContext, 2299, "CARD_PRINT_ERROR_RECEIPT_TEMPLATE"));
                    //Value is not defined for &1 
                    KioskStatic.logToFile(txtMessage.Text);
                    log.Info(txtMessage.Text);
                }
                else
                { // use CARD_PRINT_ERROR_RECEIPT_TEMPLATE
                    ReceiptPrintTemplateHeaderBL receiptPrintTemplateHeaderBL = new ReceiptPrintTemplateHeaderBL(Utilities.ExecutionContext, cardPrintErrorReceiptTemplate, true);
                    trx.POSPrinterDTOList[0].ReceiptPrintTemplateHeaderDTO = receiptPrintTemplateHeaderBL.ReceiptPrintTemplateHeaderDTO;
                }
                printTransaction.GenericReceiptPrint(trx, trx.POSPrinterDTOList[0]);

                string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 3010);
                //"Error while printing wrist bands" 
                message += " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 3011);
                //"Please contact attendant with the receipt printed."
                KioskStatic.logToFile(message);
                showOK(message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in Generate Card Print Error Receipt method: " + ex.Message);
                string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 3010);
                //"Error while printing wrist bands" 
                message += " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 3012);
                //Please contact attendant
                KioskStatic.logToFile(message);
                showOK(message);
            }
            finally
            {
                ac.totalValue = 0;
                trxForm.Close();
            }
            log.LogMethodExit();
        }

        private void RetainReceiptPrinters(Transaction.Transaction trx)
        {
            log.LogMethodEntry();
            if (trx.POSPrinterDTOList != null && trx.POSPrinterDTOList.Any())
            {
                for (int i = 0; i < trx.POSPrinterDTOList.Count; i++)
                {
                    if (trx.POSPrinterDTOList[i].PrinterDTO != null && trx.POSPrinterDTOList[i].PrinterDTO.PrinterType != PrinterDTO.PrinterTypes.ReceiptPrinter)
                    {
                        trx.POSPrinterDTOList.RemoveAt(i);
                        i = i - 1;
                    }
                }
                if ((trx.POSPrinterDTOList != null && trx.POSPrinterDTOList.Any()) == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 3013));
                    // "Unable to find receipt printer. Please check printer setup"
                }
            }
            log.LogMethodExit();
        }

        private void SetCardPrinterErrorValue(bool errorValue)
        {
            log.LogMethodEntry(errorValue);
            cardPrinterError = errorValue;
            log.LogMethodExit(cardPrinterError);
        }

        private void LogAutoGenCardActivity(Semnox.Parafait.Transaction.Transaction trx, string productName, string activity, List<int> autoGenCardTrxLineInfo)
        {
            log.LogMethodEntry(autoGenCardTrxLineInfo);
            for (int i = 0; i < autoGenCardTrxLineInfo.Count; i++)
            {
                string replacedCardNoForAutoGen = trx.TrxLines[autoGenCardTrxLineInfo[i]].CardNumber;
                KioskStatic.updateKioskActivityLog(-1, -1, replacedCardNoForAutoGen, activity, productName, ac);
            }
            log.LogMethodExit();
        }


        private void TemporaryInvalidationOfPrintedLines(Transaction.Transaction customerTransaction)
        {
            log.LogMethodEntry();
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine in customerTransaction.TrxLines)
            {
                //line is already printer or not a card line or a deposit line and active then mark them as inactive for print purpose
                if ((trxLine.ReceiptPrinted == true || string.IsNullOrWhiteSpace(trxLine.CardNumber) == true
                    || trxLine.ProductTypeCode.Equals("LOCKERDEPOSIT") || trxLine.ProductTypeCode.Equals("DEPOSIT")
                    || trxLine.ProductTypeCode.Equals("CARDDEPOSIT")) && trxLine.LineValid == true)
                {
                    trxLine.LineValid = false;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Credit Card Payment Amount
        /// </summary>
        /// <returns></returns>
        public decimal GetCreditCardPaymentAmount()
        {
            log.LogMethodEntry();
            decimal ccAmount = 0;
            log.LogVariableState("trxPaymentDTO", trxPaymentDTO);
            if (trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed)
            {
                ccAmount = (decimal)trxPaymentDTO.Amount;
            }
            log.LogMethodExit(ccAmount);
            return ccAmount;
        }
        /// <summary>
        /// Get Game Card Payment Amount
        /// </summary>
        /// <returns></returns>
        public decimal GetGameCardPaymentAmount()
        {
            log.LogMethodEntry();
            decimal gameCardAmount = 0;
            log.LogVariableState("trxPaymentDTO", trxPaymentDTO);
            if (trxPaymentDTO != null && trxPaymentDTO.GatewayPaymentProcessed == false
                && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard)
            {
                gameCardAmount = (decimal)trxPaymentDTO.Amount + (decimal)trxPaymentDTO.PaymentUsedCreditPlus;
            }
            log.LogMethodExit(gameCardAmount);
            return gameCardAmount;
        }

        public static string DirectCashAbortAndExit(Core.Utilities.ExecutionContext executionContext, string errorMessage, string posMachine, KioskStatic.acceptance billAcceptance)
        {
            log.LogMethodEntry(errorMessage, posMachine);
            string outputMsg = string.Empty;
            KioskStatic.logToFile("DirectCashAbortAndExit: " + errorMessage);

            if (billAcceptance != null && billAcceptance.totalValue != 0)
            {
                decimal cashAmount = billAcceptance.totalValue;
                KioskStatic.updateKioskActivityLog(-1, -1, "", KioskStatic.ACTIVITY_TYPE_ABORT, errorMessage, billAcceptance);
                string currencySymbol = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");
                KioskStatic.logToFile("Cash: " + cashAmount.ToString(currencySymbol));
                log.Info("Cash: " + cashAmount.ToString(currencySymbol));

                clsKioskTransaction.GenerateAbortPrint(executionContext, posMachine, billAcceptance, KioskStatic.rc, null);

                outputMsg = MessageContainerList.GetMessage(executionContext, "Transaction could not be completed.");
                if (cashAmount > 0)
                {
                    outputMsg += (" " + MessageContainerList.GetMessage(executionContext, "Money inserted by you has been recognized."));
                }

                outputMsg += (" " + MessageContainerList.GetMessage(executionContext, "Please contact attendant with the receipt printed.") + " [" + errorMessage + "]");

                billAcceptance.totalValue = 0;
            }
            else
            {
                log.Info("DirectCashAbortAndExit: No action as AC is null or Total Value is zero");
                KioskStatic.logToFile("DirectCashAbortAndExit: No action as AC is null or Total Value is zero");
            }
            log.LogMethodExit(outputMsg);
            KioskStatic.logToFile(outputMsg);
            return outputMsg;
        }

        private static void GenerateAbortPrint(Core.Utilities.ExecutionContext executionContext, string posMachine, KioskStatic.acceptance billAcceptance, KioskStatic.receipt_format recepitFormat, TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(executionContext, posMachine, transactionPaymentsDTO);
            string currencySymbol = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_WITH_CURRENCY_SYMBOL", "Rs N2");
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            List<string> printString = new List<string>();
            printString.Add(recepitFormat.head);
            if (!string.IsNullOrEmpty(recepitFormat.a1))
            {
                printString.Add(recepitFormat.a1.Replace("@Date", DateTime.Now.ToString("ddd, dd-MMM-yyyy h:mm tt")));
            }

            if (!string.IsNullOrEmpty(recepitFormat.a21))
            {
                printString.Add(recepitFormat.a21.Replace("@POS", posMachine));
            }

            printString.Add(Environment.NewLine);
            printString.Add("*******************");
            printString.Add(MessageContainerList.GetMessage(executionContext, 439)); //"TRANSACTION ABORTED";
            printString.Add("*******************");
            printString.Add(Environment.NewLine);

            printString.Add(MessageContainerList.GetMessage(executionContext, 440, billAcceptance.totalValue.ToString(currencySymbol)));
            printString.Add(Environment.NewLine);

            decimal cashAmount = billAcceptance.totalValue;
            decimal ccAmount = 0;
            decimal gameCardAmount = 0;
            if (billAcceptance.totalValue > 0)
            {
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.GatewayPaymentProcessed)
                {
                    ccAmount = (decimal)transactionPaymentsDTO.Amount;
                    cashAmount -= ccAmount;
                }
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.GatewayPaymentProcessed == false
                    && transactionPaymentsDTO.paymentModeDTO != null && transactionPaymentsDTO.paymentModeDTO.IsDebitCard)
                {
                    gameCardAmount = (decimal)transactionPaymentsDTO.PaymentUsedCreditPlus;
                    cashAmount -= gameCardAmount;
                }
            }
            printString.Add("Cash: " + cashAmount.ToString(currencySymbol));
            printString.Add("Credit Card: " + ccAmount.ToString(currencySymbol));
            printString.Add(Environment.NewLine);

            printString.Add(MessageContainerList.GetMessage(executionContext, 441));

            pd.PrintPage += (sender, e) => PrintUSB(e, printString);
            pd.Print();
        }
        private void SendCreditCardTrxInfoToVCATDevice()
        {
            log.LogMethodEntry();

            if (_PaymentModeDTO.Gateway > -1)
            {
                displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Payment Gateway unexpectedly Set. Please contact Manager."), ERROR);
                KioskStatic.logToFile("Error: Payment Gateway is not expected for VCAT.");
                log.LogMethodExit();
                return;
            }

            createDummyTransaction(); // Create dummy transaction as trx gets created in later stage
            FiscalPrinterFactory.GetInstance().Initialize(Utilities);
            bool unAttendedMode = true;
            FiscalPrinter fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER"), unAttendedMode);
            string Message = string.Empty;
            string printOption = string.Empty;
            FiscalizationRequest fiscalizationRequest = new FiscalizationRequest();
            List<PaymentInfo> payItemList = new List<PaymentInfo>();
            PaymentInfo paymentInfo = new PaymentInfo();
            paymentInfo.amount = Convert.ToDecimal(trxPaymentDTO.Amount);
            paymentInfo.paymentMode = "CreditCard";
            int installmentMonth = 0; //decided to default the Installment month to 0 in Kiosk
            paymentInfo.quantity = installmentMonth;
            payItemList.Add(paymentInfo);
            fiscalizationRequest.payments = payItemList.ToArray();

            Semnox.Parafait.Device.Printer.FiscalPrint.TransactionLine transactionLine = new TransactionLine();
            if (DummyCurrentTrx != null && DummyCurrentTrx.TrxLines != null && DummyCurrentTrx.TrxLines.Any())
            {
                transactionLine.VATRate = Convert.ToDecimal(DummyCurrentTrx.TrxLines.First().tax_percentage);
                log.Debug("VATRate :" + transactionLine.VATRate);
                if (transactionLine.VATRate > 0)
                {
                    transactionLine.VATAmount = (Convert.ToDecimal(trxPaymentDTO.Amount) * transactionLine.VATRate) / (100 + transactionLine.VATRate);
                    log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                    if (transactionLine.VATAmount % 1 > 0)
                    {
                        transactionLine.VATAmount = (decimal)(new Semnox.Core.GenericUtilities.CommonFuncs(Utilities)).RoundOff(Convert.ToDouble(transactionLine.VATAmount), Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                        log.Debug("transactionLine.VATAmount after rounding:" + transactionLine.VATAmount);
                    }
                }
                else
                {
                    transactionLine.VATAmount = 0;
                    log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                }
            }
            fiscalizationRequest.transactionLines = new TransactionLine[] { transactionLine };
            bool success = fiscalPrinter.PrintReceipt(fiscalizationRequest, ref Message);
            if (success)
            {
                if (fiscalPrinter != null && string.IsNullOrWhiteSpace(fiscalizationRequest.extReference) == false)
                {
                    trxPaymentDTO.CreditCardAuthorization = fiscalizationRequest.extReference;
                    trxPaymentDTO.ExternalSourceReference = fiscalizationRequest.payments[0].description;
                    trxPaymentDTO.CreditCardName = fiscalizationRequest.transactionLines.ToList().First().description;
                    if (installmentMonth.ToString().Length < 2)
                    {
                        trxPaymentDTO.Reference = "0" + installmentMonth;
                        log.LogVariableState("installtionMonth after appending 0 : ", trxPaymentDTO.Reference);
                    }
                    else
                    {
                        trxPaymentDTO.Reference = installmentMonth.ToString();
                    }
                    trxPaymentDTO.CreditCardNumber = fiscalizationRequest.payments[0].description;
                }
                else
                {
                    log.Error("Credit card Payment Failed");
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Credit card Payment Failed."), ERROR);
                    KioskStatic.logToFile("Credit card Payment Failed.");
                    log.LogMethodExit();
                    return;
                }

                ac.totalValue += (decimal)trxPaymentDTO.Amount;
                if (KioskStatic.debugMode) MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Credit card payment success"));
                KioskStatic.logToFile("Credit card payment success");
                //Added check to see if Partial approval is not allowed and if credit card is partially approved then abort and exit
                //current transaction
                if (KioskStatic.Utilities.getParafaitDefaults("ALLOW_PARTIAL_APPROVAL").Equals("N") && ac.totalValue < AmountRequired)
                {
                    abortAndExit(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Partial Approval not allowed"));
                    log.LogMethodExit();
                    return;
                }
                else if (ac.totalValue > 0 && ac.totalValue < AmountRequired)
                {
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2854, ac.totalValue.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                    displayMessageLine(msg, WARNING);
                    KioskStatic.logToFile(msg);
                    log.Info(msg);
                }
            }
            else
            {
                log.Error("Credit card Payment Failed");
                displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Credit card Payment Failed."), ERROR);
                KioskStatic.logToFile("Credit card Payment Failed.");
                log.LogMethodExit();
                return;
            }
            log.LogMethodExit();
        }

        private void ReverseFiscalization()
        {
            log.LogMethodEntry();
            createDummyTransaction();
            log.Debug("FiscalPrinters.SmartroKorea  Reversal");
            FiscalPrinterFactory.GetInstance().Initialize(Utilities);
            bool unAttendedMode = true;
            FiscalPrinter fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(Utilities.getParafaitDefaults("FISCAL_PRINTER"), unAttendedMode);
            string Message = string.Empty;
            FiscalizationRequest fiscalizationRequest = new FiscalizationRequest();
            List<PaymentInfo> payItemList = new List<PaymentInfo>();
            PaymentInfo paymentInfo = new PaymentInfo();
            if (trxPaymentDTO.paymentModeDTO.IsCreditCard)
            {
                paymentInfo.paymentMode = "CreditCard";
                paymentInfo.quantity = Convert.ToInt32(trxPaymentDTO.Reference);  // Installment month integer value 01,02...
                paymentInfo.amount = Convert.ToDecimal(trxPaymentDTO.Amount) * -1;
            }
            paymentInfo.reference = trxPaymentDTO.CreditCardAuthorization;
            paymentInfo.description = trxPaymentDTO.PaymentDate.ToString("yyyyMMdd");
            log.LogVariableState("paymentInfo", paymentInfo);
            payItemList.Add(paymentInfo);
            fiscalizationRequest.payments = payItemList.ToArray();
            fiscalizationRequest.isReversal = true;
            fiscalizationRequest.extReference = trxPaymentDTO.Reference;
            log.LogVariableState("fiscalizationRequest", fiscalizationRequest);
            Semnox.Parafait.Device.Printer.FiscalPrint.TransactionLine transactionLine = new Semnox.Parafait.Device.Printer.FiscalPrint.TransactionLine();
            if (DummyCurrentTrx != null && DummyCurrentTrx.TrxLines != null && DummyCurrentTrx.TrxLines.Any())
            {
                transactionLine.VATRate = Convert.ToDecimal(DummyCurrentTrx.TrxLines.First().tax_percentage);
                log.Debug("VATRate :" + transactionLine.VATRate);
                if (transactionLine.VATRate > 0)
                {
                    //creditCardAmount is inclusive of tax amount. 
                    transactionLine.VATAmount = (Convert.ToDecimal(paymentInfo.amount) * transactionLine.VATRate) / (100 + transactionLine.VATRate);
                    log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                    if (transactionLine.VATAmount % 1 > 0)
                    {
                        transactionLine.VATAmount = (decimal)(new Semnox.Core.GenericUtilities.CommonFuncs(Utilities)).RoundOff(Convert.ToDouble(transactionLine.VATAmount), Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                        log.Debug("transactionLine.VATAmount after rounding:" + transactionLine.VATAmount);
                    }
                }
                else
                {
                    transactionLine.VATAmount = 0;
                    log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                }
            }
            fiscalizationRequest.transactionLines = new Semnox.Parafait.Device.Printer.FiscalPrint.TransactionLine[] { transactionLine };
            bool success = fiscalPrinter.PrintReceipt(fiscalizationRequest, ref Message);
            if (success)
            {
                if (fiscalPrinter != null && string.IsNullOrWhiteSpace(fiscalizationRequest.extReference) == false)
                {
                    trxPaymentDTO.CreditCardNumber = fiscalizationRequest.payments[0].description;
                    trxPaymentDTO.CreditCardAuthorization = fiscalizationRequest.extReference;
                    trxPaymentDTO.ExternalSourceReference = fiscalizationRequest.payments[0].description;
                }
                else
                {
                    log.Error("Payment reversal Failed");
                    fiscalPrinter.ClosePort();
                    throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4279) + Message);
                }
            }
            else
            {
                log.Error("Payment reversal Failed");
                fiscalPrinter.ClosePort();
                throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4279) + Message);
            }

            ac.totalValue -= (decimal)trxPaymentDTO.Amount;
            displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Credit Card smartro Payment Refunded"), WARNING);
            KioskStatic.logToFile("CC smartro amount refunded: " + trxPaymentDTO.Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
            log.LogMethodExit();
        }
    }
}
