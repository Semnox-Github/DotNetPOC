/********************************************************************************************
* Project Name - Semnox.Parafait.KioskCore
* Description  - KioskTransaction 
* 
**************
**Version Log
**************
*Version         Date             Modified By           Remarks          
*********************************************************************************************
*2.150.1.0       28-Nov-2022      Guru S A              Added for Cart feature in Kiosk 
*2.150.3.0       28-Apr-2023      Vignesh Bhat          Modified for TableTop Kiosk Changes
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.KioskCore.CardDispenser;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.TransactionFunctions;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.KioskCore
{
    public class KioskTransaction
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected Transaction.Transaction kioskTrx;
        private int globalKioskTrxId;
        protected ExecutionContext executionContext;
        protected Utilities utilities;
        private bool showCartInKiosk = false;
        private readonly object notifyingObjectToProcessCash = new Object();
        public delegate void ProgressUpdates(string statusMessage);
        protected ProgressUpdates KioskProgressUpdates;
        public delegate void PopupAlerts(string statusMessage);
        protected PopupAlerts KioskPopupAlerts;
        public delegate void ShowThankYou(bool receiptPrinted, bool receiptEmailed);
        protected ShowThankYou KioskShowThankYou;
        public delegate void AbortTransactionEvent();
        protected AbortTransactionEvent kioskAbortTransactionEvent;
        private bool cardPrinterError = false;
        protected KioskReceiptDeliveryMode receciptDeliveryMode = KioskReceiptDeliveryMode.NONE;
        private string loyaltyCardNumber = string.Empty; //used for third Party Loyalty Points. Like ALOHA..
        protected const string NEWCARDTYPE = "I";
        protected const string RECHAREGETYPE = "R";
        protected const string CHECKINCHECKOUTTYPE = "C";
        protected const string FNBTYPE = "F";
        protected const string ATTRACTIONSTYPE = "A";
        protected const string REDEEM = "REDEEM";
        protected const string CARDPRINT = "CARDPRINT";
        protected const string CARDDISPENSER = "CARDDISPENSER";
        protected const string CARD_PRINT_ERROR_RECEIPT_TEMPLATE = "CARD_PRINT_ERROR_RECEIPT_TEMPLATE";
        protected const string CARD_DISPENSER_ERROR_RECEIPT_TEMPLATE = "CARD_DISPENSER_ERROR_RECEIPT_TEMPLATE";
        protected const string EXTERNAL_SYS_IDENTIFIER = "External System Identifier";
        protected const string NEWCARD = "NEWCARD";
        protected const string TOPUP = "TOP-UP";
        protected const string CHECKIN = "CHECK-IN";
        protected const string ATTRACTION = "ATTRACTION";
        protected const string KIOSKSALE = "KIOSKSALE";
        protected const string NEWCARDMSG = "New card";
        protected const string TOPUPMSG = "Recharge";
        protected const string CHECKINMSG = "Check in";
        protected const string ATTRACTIONMSG = "Attraction";
        protected const string PRINTERROR = "PRINT-ERROR";
        protected const string KIOSKSALEMSG = "Kiosk Sale";
        protected const string REDEEMTOKENMSG = "Redeem token";
        protected const string REDEEMTOKENPRODUCTNAME = "Redeem Token Product";
        protected const string ABORTNEWCARDISSUE = "Abort New Card Issue";
        protected const string ABORTRECHAREGE = "Abort Recharge";
        protected const string ABORTCHECKIN = "Abort Check-In";
        protected const string ABORTKIOSKTRXMSG = "Abort Kiosk Transaction";
        protected static string REPRINT = "RE-PRINT";
        protected static string REPRINTERROR = "RE-PRINT-ERROR";
        protected static string ABORTTRANSACTION = "ABORT_TRANSACTION";
        protected static string ABORTCASH = "ABORT";
        protected static string ABORTCREDITCARD = "ABORT_CC";
        protected static string ABORTGAMECARD = "ABORT_GAMECARD";
        protected static string REFUNDTRANSACTION = "REFUND";
        protected static string BILLIN = "BILL-IN";
        protected static string BILLINMSG = "Bill Inserted";
        protected static string DIRECTCASH_BILLINMSG = "DIRECTCASH: Bill Inserted";
        protected static string COININ = "COIN-IN";
        protected static string COININMSG = "Coin Inserted";
        protected static string DIRECTCASH_COININMSG = "DIRECTCASH: Coin Inserted";
        protected static string TOKENIN = "TOKEN-IN";
        protected static string TOKENINMSG = "Token Inserted";
        protected static string TIME = "Time";
        protected static string CREDITS = "Credits";
        protected static string KIOSKLAUNCH = "KIOSK_LAUNCH";
        protected static string EXITKIOSK = "EXIT_KIOSK";
        protected static string PAYMENT_MODE_ERROR_EXIT_KIOSK = "PAYMENT_MODE_ERROR_EXIT_KIOSK";
        protected string AMOUNTFORMAT;
        protected string NUMBERFORMAT;
        protected string AMOUNTFORMATWITHCURRENCYSYMBOL;
        protected string selectedProductType = string.Empty;
        protected PaymentModesContainerDTO preSelectedPaymentModeDTO;
        protected List<PaymentModeDisplayGroupsDTO> eligibleDisplayGroupDTOList;
        protected List<ProductsDTO> fundRaiserProductList = null;
        protected List<ProductsDTO> donationProductList = null;
        protected List<MasterScheduleBL> masterScheduleBLList = null;
        private string selectedEntitlement = string.Empty;
        private bool isOnlineTransaction = false;

        public bool IsOnlineTransaction { set { isOnlineTransaction = value; } get { return isOnlineTransaction; } }

        public enum KioskReceiptDeliveryMode
        {
            /// <summary>
            /// Print
            /// </summary>
            PRINT,
            /// <summary>
            /// Email
            /// </summary>
            EMAIL,
            /// <summary>
            /// None
            /// </summary>
            NONE,
            /// <summary>
            /// ASK
            /// </summary>
            ASK
        }
        #region static
        public static string GETCHECKINCHECKOUTTYPE { get { return CHECKINCHECKOUTTYPE; } }
        public static string GETRECHAREGETYPE { get { return RECHAREGETYPE; } }
        public static string GETNEWCARDTYPE { get { return NEWCARDTYPE; } }
        public static string GETFNBTYPE { get { return FNBTYPE; } }
        public static string GETATTRACTIONSTYPE { get { return ATTRACTIONSTYPE; } }
        public static string GETABORTNEWCARDISSUE { get { return ABORTNEWCARDISSUE; } }
        public static string GETABORTCHECKIN { get { return ABORTCHECKIN; } }
        public static string GETABORTRECHAREGE { get { return ABORTRECHAREGE; } }
        public static string GETABORTTRANSACTION { get { return ABORTTRANSACTION; } }
        public static string GETABORTCASH { get { return ABORTCASH; } }
        public static string GETABORTGAMECARD { get { return ABORTGAMECARD; } }
        public static string GETABORTCREDITCARD { get { return ABORTCREDITCARD; } }
        public static string GETREFUNDTRANSACTION { get { return REFUNDTRANSACTION; } }
        public static string GETBILLIN { get { return BILLIN; } }
        public static string GETBILLINMSG { get { return BILLINMSG; } }
        public static string GETDIRECTCASH_BILLINMSG { get { return DIRECTCASH_BILLINMSG; } }
        public static string GETCOININ { get { return COININ; } }
        public static string GETCOININMSG { get { return COININMSG; } }
        public static string GETDIRECTCASH_COININMSG { get { return DIRECTCASH_COININMSG; } }
        public static string GETTOKENIN { get { return TOKENIN; } }
        public static string GETTOKENINMSG { get { return TOKENINMSG; } }
        public static string GETREPRINT { get { return REPRINT; } }
        public static string GETREPRINTERROR { get { return REPRINTERROR; } }
        public static string GETREDEEM { get { return REDEEM; } }
        public static string LAUNCHKIOSKMSG { get { return KIOSKLAUNCH; } }
        public static string EXITKIOSKMSG { get { return EXITKIOSK; } }
        public static string PAYMENTMODE_ERRROR_EXITKIOSK_MSG { get { return PAYMENT_MODE_ERROR_EXIT_KIOSK; } }
        public List<MasterScheduleBL> MasterScheduleBLList { get { return masterScheduleBLList; } set { masterScheduleBLList = value; } }

        #endregion
        #region Class Properties
        public virtual int GetTransactionId { get { return GetTrxId(); } }
        public string GetTransactionStatus { get { return GetTrxStatus(); } }
        public bool ShowCartInKiosk
        {
            get
            {
                return showCartInKiosk;
            }
        }
        public List<DiscountsSummaryDTO> GetTransactionDiscountsSummaryList { get { return GetTrxDiscountSummary(); } }
        public List<Transaction.Transaction.TransactionLine> GetActiveTransactionLines { get { return GetActiveTrxLines(); } }
        public List<Transaction.Transaction.TransactionLine> GetCardDepositTransactionLines { get { return GetCardDepositTrxLines(); } }
        public Transaction.Transaction.TransactionLine GetFirstNonDepositAndFnDTransactionLine { get { return GetFirstNonDepositAndFnDTrxLine(); } }
        public List<TransactionPaymentsDTO> GetTransactionPaymentsDTOList { get { return GetTrxTransactionPaymentsDTOList(); } }
        public string LoyaltyCardNumber { get { return loyaltyCardNumber; } set { loyaltyCardNumber = value; } } //used for third Party Loyalty Points. Like ALOHA...
        public KioskReceiptDeliveryMode RececiptDeliveryMode { get { return receciptDeliveryMode; } set { receciptDeliveryMode = value; } }
        public Card GetTransactionPrimaryCard { get { return GetTrxPrimaryCard(); } }
        public string GetTransactionNumber { get { return GetTrxNumber(); } }
        public decimal GetCCSurchargeAmount { get { return GetTrxCCSurchargeAmount(); } }
        public string SelectedProductType { get { return selectedProductType; } set { selectedProductType = value; } }
        public PaymentModesContainerDTO PreSelectedPaymentModeDTO { get { return preSelectedPaymentModeDTO; } set { preSelectedPaymentModeDTO = value; } }
        public List<PaymentModeDisplayGroupsDTO> EligibleDisplayGroupDTOList { get { return eligibleDisplayGroupDTOList; } set { eligibleDisplayGroupDTOList = value; } }
        public static string CREDITS_ENTITLEMENT { get { return CREDITS; } }
        public static string TIME_ENTITLEMENT { get { return TIME; } }
        public int GetCartItemCount { get { return GetTrxCartItemCount(); } }
        public int GetGlobalKioskTrxId { get { return globalKioskTrxId; } }
        public virtual Transaction.Transaction KioskTransactionObject { get { return kioskTrx; } }
        public string GetSelectedEntitlementType { get { return selectedEntitlement; } }

        #endregion
        /// <summary>
        /// KioskTransaction constructor
        /// </summary>
        /// <param name="utils"></param>
        public KioskTransaction(Utilities utils, int trxId = -1)
        {
            log.LogMethodEntry("utils", trxId);
            KioskStatic.logToFile("In KioskTransaction()");
            this.utilities = utils;
            this.executionContext = utils.ExecutionContext;
            if (trxId == -1)
            {
                this.kioskTrx = new Transaction.Transaction(utils);
            }
            else
            {
                Transaction.TransactionUtils transactionUtils = new TransactionUtils(utils);
                this.kioskTrx = transactionUtils.CreateTransactionFromDB(trxId, utils);
            }
            this.globalKioskTrxId = KioskStatic.GetKioskGlobalTransactionId(null);
            this.cardPrinterError = false;
            this.loyaltyCardNumber = string.Empty;
            SetShowCardInKioskFlag();
            AMOUNTFORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            AMOUNTFORMATWITHCURRENCYSYMBOL = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");
            NUMBERFORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");
            log.LogMethodExit();
        }
        /// <summary>
        /// ~KioskTransaction destructor
        /// </summary>
        ~KioskTransaction()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Dispose KioskTransaction()");
            Dispose();
            log.LogMethodExit();
        }
        private void Dispose()
        {
            log.LogMethodEntry();
            this.globalKioskTrxId = -1;
            this.kioskTrx = null;
            this.utilities = null;
            this.executionContext = null;
            this.cardPrinterError = false;
            this.loyaltyCardNumber = string.Empty;
            this.KioskProgressUpdates = null;
            this.KioskPopupAlerts = null;
            this.KioskShowThankYou = null;
            this.kioskAbortTransactionEvent = null;
            this.selectedProductType = string.Empty;
            log.LogMethodExit();
        }
        #region Public Methods
        /// <summary>
        /// Add new card product
        /// </summary> 
        public void AddNewCardProduct(int productId, int quantity, double? overidePrice)
        {
            log.LogMethodEntry(productId, quantity, overidePrice);
            KioskStatic.logToFile("AddNewCardProduct: " + productId);
            //TransactionIsNotNull();
            ProductsContainerDTO containerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext, productId);
            if (containerDTO.ProductType != ProductTypeValues.NEW && containerDTO.ProductType != ProductTypeValues.CARDSALE
                && containerDTO.ProductType != ProductTypeValues.GAMETIME)
            {
                ValidationException validationException = new ValidationException(MessageContainerList.GetMessage(executionContext, 4442) + " " + containerDTO.ProductType);
                //Invalid product type.
                log.Error("Error while adding new card product", validationException);
                throw validationException;
            }
            ClearOldActiveLinesForNoCartTransaction(); //Cancells previously added lines when cart option is disabled
            if (containerDTO.CardCount > 1)
            {
                CreateNewCardTrxLinesForMultipleCardsInSingleProduct(containerDTO, quantity, overidePrice);
            }
            else
            {
                CreateNewCardTrxLines(containerDTO, quantity, overidePrice);
            }
            kioskTrx.SetServiceCharges(null);
            kioskTrx.SetAutoGratuityAmount(null);
            log.LogMethodExit();
        }
        /// <summary>
        /// Add recharge card product
        /// </summary> 
        public void AddRechargeCardProduct(Card rechargeCard, int productId, int quantity, double? overidePrice)
        {
            log.LogMethodEntry(rechargeCard, productId, quantity, overidePrice);
            KioskStatic.logToFile("AddRechargeCardProduct: " + productId);

            if (rechargeCard == null)
            {
                //Please Tap Card
                ValidationException validationException = new ValidationException(MessageContainerList.GetMessage(executionContext, 257));
                log.Error("Error while adding recharge product", validationException);
                throw validationException;
            }
            ProductsContainerDTO containerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext, productId);
            if (containerDTO.ProductType != ProductTypeValues.RECHARGE && containerDTO.ProductType != ProductTypeValues.CARDSALE
                && containerDTO.ProductType != ProductTypeValues.GAMETIME)
            {
                ValidationException validationException = new ValidationException(MessageContainerList.GetMessage(executionContext, 4442) + " " + containerDTO.ProductType);
                //Invalid product type.
                log.Error("Error in AddRechargeCardProduct", validationException);
                throw validationException;
            }
            if (rechargeCard.technician_card.Equals('Y'))
            {
                ValidationException validationException = new ValidationException(MessageContainerList.GetMessage(executionContext, 197, rechargeCard.CardNumber));
                //Technician Card (&1) not allowed for Transaction
                log.Error("Error in AddRechargeCardProduct", validationException);
                throw validationException;
            }
            ClearOldActiveLinesForNoCartTransaction(); //Cancells previously added lines when cart option is disabled

            while (quantity > 0)
            {
                string msg = string.Empty;
                double finalPrice = (overidePrice == null ? -1 : (double)overidePrice);
                int returnValue = kioskTrx.createTransactionLine(rechargeCard, productId, finalPrice, 1, ref msg);
                if (returnValue != 0)
                {
                    ValidationException validationException = new ValidationException(msg);
                    log.Error("Error in createTransactionLine", validationException);
                    throw validationException;
                }
                quantity--;
            }

            SetPrimaryCard(rechargeCard);
            kioskTrx.SetServiceCharges(null);
            kioskTrx.SetAutoGratuityAmount(null);
            log.LogMethodExit();
        }
        /// <summary>
        /// Add check in product
        /// </summary> 
        public List<Semnox.Parafait.Transaction.Transaction.TransactionLine> AddCheckinCheckOutProduct(int productId, PurchaseProductDTO purchaseProductDTO, Card parentCard, bool applyCardCreditPlusConsumption)
        {
            log.LogMethodEntry(productId, purchaseProductDTO, parentCard, applyCardCreditPlusConsumption);
            KioskStatic.logToFile("AddCheckinCheckOutProduct: " + productId);
            ProductsContainerDTO comboProductsContainerDTO = IsValidCheckinCheckoutProduct(productId);
            ClearOldActiveLinesForNoCartTransaction(); //Cancells previously added lines when cart option is disabled
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> addLineList = new List<Transaction.Transaction.TransactionLine>();
            if (comboProductsContainerDTO.ProductType == ProductTypeValues.COMBO)
            {
                addLineList = CreateTrxLinesForCombo(comboProductsContainerDTO, purchaseProductDTO, parentCard, applyCardCreditPlusConsumption);
            }
            else
            {
                addLineList = CreateTrxLinesForCheckIn(purchaseProductDTO, parentCard, applyCardCreditPlusConsumption);
            }
            kioskTrx.SetServiceCharges(null);
            kioskTrx.SetAutoGratuityAmount(null);
            log.LogMethodExit();
            return addLineList;
        }

        /// <summary>
        /// Add attraction product
        /// </summary> 
        public List<Semnox.Parafait.Transaction.Transaction.TransactionLine> AddAttractiontProduct(KioskAttractionDTO kioskAttractionDTO)
        {
            log.LogMethodEntry(kioskAttractionDTO);
            KioskStatic.logToFile("AddAttractiontProduct: " + (kioskAttractionDTO != null ? kioskAttractionDTO.ProductId : -1));
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> addLineList = new List<Transaction.Transaction.TransactionLine>();
            ProductsContainerDTO productsContainerDTO = IsValidAttractionProduct(kioskAttractionDTO);
            ClearOldActiveLinesForNoCartTransaction(); //Cancells previously added lines when cart option is disabled
            if (productsContainerDTO.ProductType == ProductTypeValues.ATTRACTION)
            {
                addLineList = AddSingleAttractionProduct(kioskAttractionDTO);
            }
            else if (productsContainerDTO.ProductType == ProductTypeValues.COMBO)
            {
                addLineList = AddComboAttractionProduct(kioskAttractionDTO);
            }
            kioskTrx.SetServiceCharges(null);
            kioskTrx.SetAutoGratuityAmount(null);

            log.LogMethodExit(addLineList);
            return addLineList;
        }

        /// <summary>
        /// Is Valid Checkin Checkout Product
        /// </summary> 
        public ProductsContainerDTO IsValidCheckinCheckoutProduct(int productId)
        {
            log.LogMethodEntry(productId);
            ProductsContainerDTO selectedProductContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext.SiteId, productId);
            if (selectedProductContainerDTO == null)
            {                   //Sorry, cannot to proceed. Unable to find product details
                string errMsg = MessageContainerList.GetMessage(executionContext, 2165) + " "
                                    + MessageContainerList.GetMessage(executionContext, "Product Id") + ": " + productId;
                log.Error(errMsg);
                KioskStatic.logToFile("ValidCheckinCheckoutProduct: " + errMsg);
                throw new ValidationException(errMsg);

            }
            //Price is not supported at COMBO Level.
            if (selectedProductContainerDTO.ProductType == ProductTypeValues.COMBO && selectedProductContainerDTO.Price > 0)
            {

                string errMsg = MessageContainerList.GetMessage(executionContext, 4466) //Product setup Error: 
                                  + MessageContainerList.GetMessage(executionContext, 5019);
                // "Currently price at combo level is NOT supported"
                log.Error(errMsg);
                KioskStatic.logToFile("ValidCheckinCheckoutProduct: " + errMsg);
                throw new ValidationException(errMsg);
            }
            if (selectedProductContainerDTO.ProductType == ProductTypeValues.CHECKIN
                && selectedProductContainerDTO.CheckInFacilityId < 0)
            {
                //Product setup Error: Facility is not mapped to the check-in product(product name)
                string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4466)
                    + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4817)
                    + "(" + selectedProductContainerDTO.ProductName + ")";
                log.Error(errMsg);
                KioskStatic.logToFile(errMsg);
                throw new ValidationException(errMsg);
            }

            if (selectedProductContainerDTO.ProductType == ProductTypeValues.COMBO)
            {
                if (selectedProductContainerDTO.ComboProductContainerDTOList != null && selectedProductContainerDTO.ComboProductContainerDTOList.Any())
                {
                    if (selectedProductContainerDTO.ComboProductContainerDTOList.Exists(x => x.ChildProductType == ProductTypeValues.CHECKIN) == false)
                    {
                        string errMsg = MessageContainerList.GetMessage(executionContext, 4466) //Product setup Error: 
                                          + MessageContainerList.GetMessage(executionContext, 5020, ProductTypeValues.CHECKIN);
                        //'Combo must include atleast 1 Check-In product'
                        log.Error(errMsg);
                        KioskStatic.logToFile("ValidCheckinCheckoutProduct: " + errMsg);
                        throw new ValidationException(errMsg);
                    }
                    List<int> listOfChildProductIds = selectedProductContainerDTO.ComboProductContainerDTOList.Select(p => p.ChildProductId).ToList();
                    if (listOfChildProductIds.Count != listOfChildProductIds.Distinct().Count())
                    {
                        string errMsg = MessageContainerList.GetMessage(executionContext, 4466)//Product setup Error: 
                                           + MessageContainerList.GetMessage(executionContext, 5021);
                        // 'Duplicate products exist in the combo selected'
                        log.Error(errMsg);
                        KioskStatic.logToFile("ValidCheckinCheckoutProduct: " + errMsg);
                        throw new ValidationException(errMsg);
                    }

                    foreach (ComboProductContainerDTO comboChildProduct in selectedProductContainerDTO.ComboProductContainerDTOList)
                    {
                        ProductsContainerDTO childProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext, comboChildProduct.ChildProductId);
                        if (childProductsContainerDTO.IsActive == false)
                        {
                            string errMsg = MessageContainerList.GetMessage(executionContext, 4466) //Product setup Error: 
                                                + MessageContainerList.GetMessage(executionContext, 5022);
                            //"Inactive product in the combo is not supported"
                            log.Error(errMsg);
                            KioskStatic.logToFile("ValidCheckinCheckoutProduct: " + errMsg);
                            throw new ValidationException(errMsg);
                        }

                        if (childProductsContainerDTO.ProductType == ProductTypeValues.CHECKIN
                            && childProductsContainerDTO.CheckInFacilityId < 0)
                        {
                            string errMsg = MessageContainerList.GetMessage(executionContext, 4466)//Product setup Error: 
                                              + MessageContainerList.GetMessage(executionContext, 5023)
                                               + "(" + childProductsContainerDTO.ProductName + ")";
                            //Facility is not mapped to the check-in product
                            log.Error(errMsg);
                            KioskStatic.logToFile("ValidCheckinCheckoutProduct: " + errMsg);
                            throw new ValidationException(errMsg);
                        }
                    }
                }
            }
            log.LogMethodExit(selectedProductContainerDTO);
            return selectedProductContainerDTO;
        }
        /// <summary>
        /// Is Valid Attraction Product
        /// </summary> 
        public ProductsContainerDTO IsValidAttractionProduct(int productId)
        {
            log.LogMethodEntry(productId);
            ProductsContainerDTO selectedProductContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext.SiteId, productId);
            if (selectedProductContainerDTO == null)
            {                   //Sorry, cannot to proceed. Unable to find product details
                string errMsg = MessageContainerList.GetMessage(executionContext, 2165) + " "
                                    + MessageContainerList.GetMessage(executionContext, "Product Id") + ": " + productId;
                log.Error(errMsg);
                KioskStatic.logToFile("ValidCheckinCheckoutProduct: " + errMsg);
                throw new ValidationException(errMsg);
            }
            //validate quantity other than combo level
            //validate price
            if (selectedProductContainerDTO.ProductType == ProductTypeValues.COMBO)
            {
                if (selectedProductContainerDTO.ComboProductContainerDTOList != null && selectedProductContainerDTO.ComboProductContainerDTOList.Any())
                {
                    if (selectedProductContainerDTO.ComboProductContainerDTOList.Exists(x => x.ChildProductType == ProductTypeValues.ATTRACTION) == false)
                    {
                        string errMsg = MessageContainerList.GetMessage(executionContext, 4466) //Product setup Error: 
                                          + MessageContainerList.GetMessage(executionContext, 5020, ProductTypeValues.ATTRACTION);
                        //'Combo must include atleast 1 Attraction product'
                        log.Error(errMsg);
                        KioskStatic.logToFile("ValidAttractionProduct: " + errMsg);
                        throw new ValidationException(errMsg);
                    }

                    bool hasOtherChildProducts = selectedProductContainerDTO.ComboProductContainerDTOList.Exists(x => x.ChildProductType != ProductTypeValues.ATTRACTION
                                                                                         && x.ChildProductType != ProductTypeValues.MANUAL
                                                                                         && x.CategoryId == -1);
                    if (hasOtherChildProducts)
                    {
                        string errMsg = MessageContainerList.GetMessage(executionContext, 4466)//Product setup Error: 
                                           + MessageContainerList.GetMessage(executionContext, 5180); //Kiosk supports Combo with child products of type Attraction and FnB only"
                        // "Kiosk supports Combo with child products of type Attraction and F&B only"
                        log.Error(errMsg);
                        KioskStatic.logToFile("ValidAttractionProduct: " + errMsg);
                        throw new ValidationException(errMsg);

                    }
                    List<ComboProductContainerDTO> listOfCategoryChildren = selectedProductContainerDTO.ComboProductContainerDTOList.Where(cp => cp.CategoryId != -1).ToList();
                    if (listOfCategoryChildren != null && listOfCategoryChildren.Any())
                    {

                        string errMsg = MessageContainerList.GetMessage(executionContext, 4466)//Product setup Error: 
                                           + MessageContainerList.GetMessage(executionContext, "Combo with category products is not supported in Kiosk");
                        // "Combo with category products is not supported in Kiosk"
                        log.Error(errMsg);
                        KioskStatic.logToFile("ValidAttractionProduct: " + errMsg);
                        throw new ValidationException(errMsg);
                    }
                    //List<int> listOfChildProductIds = selectedProductContainerDTO.ComboProductContainerDTOList.Select(p => p.ChildProductId).ToList();
                    //if (listOfChildProductIds.Count != listOfChildProductIds.Distinct().Count())
                    //{
                    //    string errMsg = MessageContainerList.GetMessage(executionContext, 4466)//Product setup Error: 
                    //                       + MessageContainerList.GetMessage(executionContext, 5021);
                    //    // 'Duplicate products exist in the combo selected'
                    //    log.Error(errMsg);
                    //    KioskStatic.logToFile("ValidAttractionProduct: " + errMsg);
                    //    throw new ValidationException(errMsg);
                    //}

                    foreach (ComboProductContainerDTO comboChildProduct in selectedProductContainerDTO.ComboProductContainerDTOList)
                    {
                        ProductsContainerDTO childProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext, comboChildProduct.ChildProductId);
                        if (childProductsContainerDTO.IsActive == false)
                        {
                            string errMsg = MessageContainerList.GetMessage(executionContext, 4466) //Product setup Error: 
                                                + MessageContainerList.GetMessage(executionContext, 5022);
                            //"Inactive product in the combo is not supported"
                            log.Error(errMsg);
                            KioskStatic.logToFile("ValidAttractionProduct: " + errMsg);
                            throw new ValidationException(errMsg);
                        }
                        //validate facility map
                    }
                }
            }
            log.LogMethodExit(selectedProductContainerDTO);
            return selectedProductContainerDTO;
        }

        private ProductsContainerDTO IsValidAttractionProduct(KioskAttractionDTO kioskAttractionDTO)
        {
            log.LogMethodEntry(kioskAttractionDTO);
            if (kioskAttractionDTO == null)
            {
                string errMsg = MessageContainerList.GetMessage(executionContext, 1831) + " - kioskAttractionDTO";
                //The Parameter should not be empty
                log.Error(errMsg);
                KioskStatic.logToFile("ValidAttractionProduct: " + errMsg);
                throw new ValidationException(errMsg);
            }
            ProductsContainerDTO selectedProductContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext.SiteId, kioskAttractionDTO.ProductId);
            if (selectedProductContainerDTO != null && selectedProductContainerDTO.ProductType == ProductTypeValues.ATTRACTION
                && kioskAttractionDTO.AttractionBookingDTO == null)
            {
                string errMsg = MessageContainerList.GetMessage(executionContext, "Attraction DTO information is missing for " + selectedProductContainerDTO.ProductName);
                log.Error(errMsg);
                KioskStatic.logToFile("ValidAttractionProduct: " + errMsg);
                throw new ValidationException(errMsg);
            }
            else if (selectedProductContainerDTO != null && selectedProductContainerDTO.ProductType == ProductTypeValues.COMBO
               && (kioskAttractionDTO.ChildAttractionBookingDTOList == null || kioskAttractionDTO.ChildAttractionBookingDTOList.Any() == false))
            {
                string errMsg = MessageContainerList.GetMessage(executionContext, "Child Product information is missing for " + selectedProductContainerDTO.ProductName);
                log.Error(errMsg);
                KioskStatic.logToFile("ValidAttractionProduct: " + errMsg);
                throw new ValidationException(errMsg);
            }
            selectedProductContainerDTO = IsValidAttractionProduct(kioskAttractionDTO.ProductId);
            log.LogMethodExit(selectedProductContainerDTO);
            return selectedProductContainerDTO;
        }

        /// <summary>
        /// Add Variable Card Product
        /// </summary> 
        public void AddVariableCardProduct(int productId, double variableAmount, string selectedEntitlementType, Card rechargedCard, int quantity)
        {
            log.LogMethodEntry(productId, variableAmount, selectedEntitlementType, (rechargedCard != null ? rechargedCard.CardNumber : ""), quantity);
            KioskStatic.logToFile("AddVariableCardProduct: " + productId + " amount: " + variableAmount.ToString());
            //TransactionIsNotNull();
            ProductsContainerDTO containerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext, productId);
            if (containerDTO.ProductType != ProductTypeValues.VARIABLECARD)
            {
                ValidationException validationException = new ValidationException(MessageContainerList.GetMessage(executionContext, 4442) + " " + containerDTO.ProductType);
                //Invalid product type.
                log.Error("Error while adding new card product", validationException);
                throw validationException;
            }
            ValidateVariableAmount(variableAmount);
            ClearOldActiveLinesForNoCartTransaction(); //Cancells previously added lines when cart option is disabled
            double finalVariableAmount = variableAmount / quantity;
            int quantityValue = quantity;
            while (quantityValue > 0)
            {
                CreateVariableCardTrxLines(containerDTO, (double)finalVariableAmount, selectedEntitlementType, rechargedCard);
                quantityValue--;
            }
            if (kioskTrx.PrimaryCard == null)
            {
                SetPrimaryCard(rechargedCard);
            }
            kioskTrx.SetServiceCharges(null);
            kioskTrx.SetAutoGratuityAmount(null);
            log.LogMethodExit();
        }
        /// <summary>
        /// Add Donation Or Fundraiser Products
        /// </summary>
        public void AddDonationOrFundraiserProducts(List<KeyValuePair<string, ProductsDTO>> selectedFundsList)
        {
            log.LogMethodEntry(selectedFundsList);
            KioskStatic.logToFile("AddDonationOrFundraiserProducts()");
            //TransactionIsNotNull();
            string message = "";
            if (selectedFundsList != null && selectedFundsList.Any())
            {
                Card rechargeCard = null;
                foreach (KeyValuePair<string, ProductsDTO> keyValuePair in selectedFundsList)
                {
                    if (keyValuePair.Value != null && keyValuePair.Value.ProductType == ProductTypeValues.MANUAL)
                    {
                        int returnValue = kioskTrx.createTransactionLine(rechargeCard, keyValuePair.Value.ProductId,
                                                                         Convert.ToDouble(keyValuePair.Value.Price), 1, ref message);
                        if (returnValue != 0)
                        {
                            ValidationException validationException = new ValidationException(message);
                            log.Error("Error in AddDonationOrFundraiserProducts", validationException);
                            throw validationException;
                        }
                    }
                    else
                    {
                        KioskStatic.logToFile("Product Setup Error: Donation/Fundraise should be a MANUAL Product");
                        message = MessageContainerList.GetMessage(executionContext, 5191); //"Setup Issue: Donation/Fundraise product is having invalid product type."
                        ValidationException validationException = new ValidationException(message);
                        log.Error("Error in AddDonationOrFundraiserProducts", validationException);
                        throw validationException;
                    }
                }
                kioskTrx.SetServiceCharges(null);
                kioskTrx.SetAutoGratuityAmount(null);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Add Card Deposit Product
        /// </summary>
        public void AddCardDepositProduct(int productId, double depositAmount, Card rechargeCard)
        {
            log.LogMethodEntry(productId, depositAmount, rechargeCard);
            KioskStatic.logToFile("AddCardDepositProduct: " + productId);
            //TransactionIsNotNull();
            ProductsContainerDTO containerDTO = ProductsContainerList.GetSystemProductContainerDTO(executionContext.SiteId, productId);
            if (containerDTO.ProductType != ProductTypeValues.CARDDEPOSIT)
            {
                ValidationException validationException = new ValidationException(MessageContainerList.GetMessage(executionContext, 4442) + " " + containerDTO.ProductType);
                //Invalid product type.
                log.Error("Error while Adding Card Deposit Product", validationException);
                throw validationException;
            }
            if (rechargeCard.technician_card.Equals('Y'))
            {
                ValidationException validationException = new ValidationException(MessageContainerList.GetMessage(executionContext, 197, rechargeCard.CardNumber));
                //Technician Card (&1) not allowed for Transaction
                log.Error("Error in AddCardDepositProduct", validationException);
                throw validationException;
            }
            string msg = string.Empty;
            int returnValue = kioskTrx.createTransactionLine(rechargeCard, productId, depositAmount, 1, ref msg);
            if (returnValue != 0)
            {
                ValidationException validationException = new ValidationException(msg);
                log.Error("Error in AddCardDepositProduct", validationException);
                throw validationException;
            }
            kioskTrx.SetServiceCharges(null);
            kioskTrx.SetAutoGratuityAmount(null);
            log.LogMethodExit();
        }
        /// <summary>
        /// Add Manual Product
        /// </summary>
        public void AddManualProduct(int productId, double amount, int quantity)
        {
            log.LogMethodEntry(productId, amount);
            KioskStatic.logToFile("AddManualProduct: " + productId);
            //TransactionIsNotNull();
            ProductsContainerDTO containerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext, productId);
            if (containerDTO.ProductType != ProductTypeValues.MANUAL)
            {
                ValidationException validationException = new ValidationException(MessageContainerList.GetMessage(executionContext, 4442) + " " + containerDTO.ProductType);
                //Invalid product type.
                log.Error("Error while Adding Card Deposit Product", validationException);
                throw validationException;
            }
            ClearOldActiveLinesForNoCartTransaction(); //Cancells previously added lines when cart option is disabled
            while (quantity > 0)
            {
                string msg = string.Empty;
                int returnValue = kioskTrx.createTransactionLine(null, productId, amount, 1, ref msg);
                if (returnValue != 0)
                {
                    ValidationException validationException = new ValidationException(msg);
                    log.Error("Error in AddManualProduct", validationException);
                    throw validationException;
                }
                quantity--;
            }
            kioskTrx.SetServiceCharges(null);
            kioskTrx.SetAutoGratuityAmount(null);
            log.LogMethodExit();
        }
        /// <summary>
        /// Apply Discount Coupon
        /// </summary>
        public void ApplyDiscountCoupon(string couponNumber)
        {
            log.LogMethodEntry(couponNumber);
            //TransactionIsNotNull();
            kioskTrx.ApplyCoupon(couponNumber);
            log.LogMethodExit();
        }
        /// <summary>
        /// Apply Card Voucher
        /// </summary>
        public void ApplyCardVoucher(Card card)
        {
            log.LogMethodEntry(card);
            //TransactionIsNotNull();
            string message = string.Empty;
            if (kioskTrx.ApplyVoucher(card, ref message) == false)
            {
                ValidationException validationException = new ValidationException(message);
                log.Error("Error in ApplyCardVoucher", validationException);
                throw validationException;
            }
            log.LogMethodExit();
        } 
        /// <summary>
        /// Save Payments
        /// </summary>
        public void SavePayments()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In SavePayments ");
            //TransactionIsNotNull();
            if (kioskTrx != null)
            {
                try
                {
                    if (kioskTrx.TransactionPaymentsDTOList == null)
                    {
                        kioskTrx.TransactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
                    } 
                    foreach (TransactionPaymentsDTO trxPaymentsDTO in kioskTrx.TransactionPaymentsDTOList)
                    {
                        if (kioskTrx.Trx_id > 0 && trxPaymentsDTO.TransactionId != kioskTrx.Trx_id)
                        {
                            trxPaymentsDTO.TransactionId = kioskTrx.Trx_id;
                        }
                        else
                        {
                            trxPaymentsDTO.TransactionId = -1;
                        }
                    }
                    bool hasPaymentLinesToProcess = kioskTrx.TransactionPaymentsDTOList != null
                                                         && kioskTrx.TransactionPaymentsDTOList.Exists(tp => tp.PaymentId == -1);
                    log.LogVariableState("hasPaymentLinesToProcess", hasPaymentLinesToProcess);
                    if (kioskTrx.isSavedTransaction() == false)
                    {
                        SaveTransaction();
                    }
                    else
                    {
                        KioskStatic.logToFile("SavePayments --> CreatePaymentInfo() ");
                        string message = string.Empty;
                        bool payAdded = kioskTrx.CreatePaymentInfo(null, ref message);
                        if (payAdded == false)
                        {
                            ValidationException validationException = new ValidationException(message);
                            log.Error(validationException);
                            throw validationException;
                        }
                    }
                    if (hasPaymentLinesToProcess && kioskTrx.Trx_id > 0)
                    {
                        //Reload data. In game card scenario. Saved data is not reflected back into transaction object
                        TransactionPaymentsListBL trxPaymentsListBL = new TransactionPaymentsListBL();
                        List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> trxPaymentSearchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                        trxPaymentSearchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, kioskTrx.Trx_id.ToString()));
                        List<TransactionPaymentsDTO> transactionPaymentsDTOList = trxPaymentsListBL.GetTransactionPaymentsDTOList(trxPaymentSearchParameters, null);
                        if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Count > 0)
                        {
                            kioskTrx.TransactionPaymentsDTOList = transactionPaymentsDTOList;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error in SavePayments: " + ex.Message);
                    throw;
                }
            }
            KioskStatic.logToFile("Exit SavePayments ");
            log.LogMethodExit();
        }
        /// <summary>
        /// Set Transaction Customer
        /// </summary> 
        public void SetTransactionCustomer(CustomerDTO customerDTO)
        {
            log.LogMethodEntry(customerDTO);
            //TransactionIsNotNull();
            kioskTrx.customerDTO = customerDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Set Guest Email Id
        /// </summary> 
        public virtual void SetGuestEmailId(string emailId)
        {
            log.LogMethodEntry(emailId);
            kioskTrx.SetGuestContactInfo(emailId, string.Empty);
            log.LogMethodExit();
        }
        /// <summary>
        /// Get Transaction Customer
        /// </summary> 
        public virtual List<string> GetTransactionCustomerIdentifierList()
        {
            log.LogMethodEntry();
            List<string> idList = kioskTrx.customerIdentifiersList;
            log.LogMethodExit(idList);
            return idList;
        }
        /// <summary>
        /// Get Transaction Customer
        /// </summary> 
        public virtual CustomerDTO GetTransactionCustomer()
        {
            log.LogMethodEntry();
            CustomerDTO customerDTO = kioskTrx.customerDTO;
            log.LogMethodExit(customerDTO);
            return customerDTO;
        }
        /// <summary>
        /// Set Primary Card
        /// </summary> 
        public void SetPrimaryCard(Transaction.Card card)
        {
            log.LogMethodEntry(card);
            //TransactionIsNotNull();
            if (card != null)
            {
                kioskTrx.PrimaryCard = card;
                if (card.customerDTO != null && kioskTrx.customerDTO == null)
                {
                    SetTransactionCustomer(card.customerDTO);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Remove Product
        /// </summary> 
        public void RemoveProduct(int productId, int quantity)
        {
            log.LogMethodEntry(productId, quantity);
            KioskStatic.logToFile("RemoveProduct: " + productId);
            //TransactionIsNotNull();
            if (kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                List<Transaction.Transaction.TransactionLine> productTrxLines = kioskTrx.TrxLines.Where(tl => tl.ProductID == productId && tl.LineValid).ToList();
                if (productTrxLines != null && productTrxLines.Any())
                {
                    int lineIndex = productTrxLines.Count - 1;
                    while (quantity > 0)
                    {
                        Transaction.Transaction.TransactionLine lineRec = productTrxLines[lineIndex];
                        int recordIndex = kioskTrx.TrxLines.IndexOf(lineRec);
                        if (kioskTrx.CancelTransactionLine(recordIndex) == false)
                        {
                            string msg = MessageContainerList.GetMessage(executionContext, 5024);
                            //Unexexpected error, unable to cancel the transaction line
                            ValidationException validationException = new ValidationException(msg);
                            log.Error("Error in RemoveProduct", validationException);
                            throw validationException;
                        }
                        quantity--;
                        lineIndex--;
                    }
                    kioskTrx.SetServiceCharges(null);
                    kioskTrx.SetAutoGratuityAmount(null);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Remove Specific Transaction Line
        /// </summary> 
        public void RemoveSpecificTransactionLine(Semnox.Parafait.Transaction.Transaction.TransactionLine trxLineRec)
        {
            log.LogMethodEntry(trxLineRec);
            //TransactionIsNotNull();
            if (kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                int lineIndex = kioskTrx.TrxLines.IndexOf(trxLineRec);
                if (lineIndex > -1)
                {
                    //Transaction.Transaction.TransactionLine lineRec = kioskTrx.TrxLines[lineIndex];
                    //int recordIndex = kioskTrx.TrxLines.IndexOf(lineRec);
                    if (kioskTrx.CancelTransactionLine(lineIndex) == false)
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, 5024);
                        // "Unexexpected error, unable to cancel the transaction line"
                        ValidationException validationException = new ValidationException(msg);
                        log.Error("Error in RemoveSpecificTransactionLine", validationException);
                        throw validationException;
                    }
                    kioskTrx.SetServiceCharges(null);
                    kioskTrx.SetAutoGratuityAmount(null);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Remove Discount Coupon
        /// </summary> 
        public void RemoveDiscountCoupon(string couponNumber)
        {
            log.LogMethodEntry(couponNumber);
            //TransactionIsNotNull();
            if (kioskTrx.DiscountsSummaryDTOList != null && kioskTrx.DiscountsSummaryDTOList.Any())
            {
                DiscountsSummaryDTO summaryDTO = kioskTrx.DiscountsSummaryDTOList.Find(d => d.CouponNumbers != null && d.CouponNumbers.Contains(couponNumber));
                if (summaryDTO != null)
                {
                    bool retVal = kioskTrx.cancelDiscountLine(summaryDTO.DiscountId);
                    if (retVal == false)
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, 5025);
                        //Unexexpected error, unable to cancel coupon discount
                        ValidationException validationException = new ValidationException(msg);
                        log.Error("Error in RemoveDiscountCoupon", validationException);
                        throw validationException;
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Save Transaction
        /// </summary> 
        public void SaveTransaction()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In SaveTransaction()");
            //TransactionIsNotNull();
            string msg = string.Empty;
            int retValue = kioskTrx.SaveOrder(ref msg);
            if (retValue != 0)
            {
                ValidationException validationException = new ValidationException(msg);
                log.Error("Error in SaveOrder", validationException);
                throw validationException;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Clear Transactionn
        /// </summary> 
        public void ClearTransaction(PopupAlerts popupAlerts)
        {
            log.LogMethodEntry("popupAlerts");
            this.KioskPopupAlerts = popupAlerts;
            try
            {
                AbortKioskTransaction();
            }
            finally
            {
                this.KioskPopupAlerts = null;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Execute Transaction
        /// </summary> 
        public void ExecuteTransaction(CardDispenser.CardDispenser cardDispenser, ProgressUpdates progressUpdates, PopupAlerts popupAlerts,
            ShowThankYou showThankYou, AbortTransactionEvent kioskAbortTransactionEvent, bool generateReceipt)
        {
            log.LogMethodEntry(cardDispenser, "progressUpdates", "popupAlerts", "ShowThankYou", "kioskAbortTransactionEvent", generateReceipt);
            KioskStatic.logToFile("In ExecuteTransaction()");
            this.KioskProgressUpdates = progressUpdates;
            this.KioskPopupAlerts = popupAlerts;
            this.KioskShowThankYou = showThankYou;
            this.kioskAbortTransactionEvent = kioskAbortTransactionEvent;
            try
            {
                TransactionHasItems();
                try
                {
                    ProcessOverpaidAmount();
                    AddRoundOffAmount();
                    kioskTrx.AutoAssignSingleTrxPOSPrinterOverrideRule(null);
                    kioskTrx.AutomaticWaiverMapping(null);
                    if (kioskTrx.IsWaiverSignaturePending() || HasCheckinProducts())
                    {
                        SaveTransaction();
                        kioskTrx.SaveTrxPOSPrinterOverrideRulesDTOList(null); //save over ride options as well
                    }
                    else
                    {
                        MarkTransactionAsComplete();
                    }
                    try
                    {
                        AddKioskActivityLogEntries();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        KioskStatic.logToFile("Error in AddKioskActivityLogEntries: " + ex.Message);
                        this.SendKioskProgressUpdates(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error in MarkTransactionAsComplete: " + ex.Message);
                    this.SendKioskPopupAlerts(ex.Message);
                    string msg = MessageContainerList.GetMessage(executionContext, 5026);
                    //Aborting transaction due to error. Please wait
                    this.SendKioskProgressUpdates(ex.Message);
                    PerformAbortTrxAction();//AbortKioskTransaction();//Perform refund etc.
                    SendkioskAbortTransactionEvent();
                    log.LogMethodExit();
                    return;
                }
                try
                {
                    UpdateLoyaltyCardNumber();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error in UpdateLoyaltyCardNumber: " + ex.Message);
                    this.SendKioskProgressUpdates(ex.Message);
                }
                IssueNewCards(cardDispenser);
                bool printed = false;
                bool emailed = false;
                bool printOnReceiptPrinter = (generateReceipt && receciptDeliveryMode == KioskReceiptDeliveryMode.PRINT);
                printed = PrintTheTransaction(printOnReceiptPrinter);
                emailed = EmailTransactionReceipt(generateReceipt);
                Audio.PlayAudio(Audio.CollectCardAndReceipt, Audio.ThankYouEnjoyGame);
                //ac.totalValue = 0;
                SendKioskShowThankYou(printed, emailed);
            }
            finally
            {
                this.KioskProgressUpdates = null;
                this.KioskPopupAlerts = null;
                this.KioskShowThankYou = null;
                this.kioskAbortTransactionEvent = null;
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///Execute Pending Temp Cards
        /// </summary> 
        public void ExecutePendingTempCards(CardDispenser.CardDispenser cardDispenser,
            ProgressUpdates progressUpdates, PopupAlerts popupAlerts,
            ShowThankYou showThankYou, bool printReceipt)
        {
            log.LogMethodEntry(cardDispenser, "progressUpdates", "popupAlerts", "showThankYou", printReceipt);
            KioskStatic.logToFile("In ExecutePendingTempCards()");
            this.KioskProgressUpdates = progressUpdates;
            this.KioskPopupAlerts = popupAlerts;
            this.KioskShowThankYou = showThankYou;
            try
            {
                IssueNewCards(cardDispenser);
                bool printed = false;
                bool emailed = false;
                //bool printOnReceiptPrinter = (printReceipt && receciptDeliveryMode == KioskReceiptDeliveryMode.PRINT);
                //printed = PrintTheTransaction(printOnReceiptPrinter);
                //emailed = EmailTransactionReceipt(printReceipt);
                Audio.PlayAudio(Audio.CollectCardAndReceipt, Audio.ThankYouEnjoyGame);
                SendKioskShowThankYou(printed, emailed);
            }
            finally
            {
                this.KioskProgressUpdates = null;
                this.KioskPopupAlerts = null;
                this.KioskShowThankYou = null;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Apply Credit Card Payment
        /// </summary> 
        public void ApplyCreditCardPayment(bool isDebitCard, PaymentModeDTO paymentModeDTO, string qrCodeScanned, bool printReceipt, AbortTransactionEvent kioskAbortTransactionEvent, PopupAlerts popupAlerts)
        {
            log.LogMethodEntry(isDebitCard, paymentModeDTO, qrCodeScanned, printReceipt, kioskAbortTransactionEvent, popupAlerts);
            try
            {
                KioskStatic.logToFile("In ApplyCreditCardPayment()");
                this.kioskAbortTransactionEvent = kioskAbortTransactionEvent;
                this.KioskPopupAlerts = popupAlerts;
                double ccSurchargeAmount = 0;
                double surchargePercentage = (double)paymentModeDTO.CreditCardSurchargePercentage;
                double totalAmountReceived = (double)GetTotalPaymentsReceived();
                double amountToPay = kioskTrx.Net_Transaction_Amount - totalAmountReceived;
                //amountToPay = 5;
                ccSurchargeAmount = ((amountToPay * surchargePercentage) / 100.0); //(double)(AmountRequired - ac.totalValue) * SurchargePercentage / 100.0;
                ccSurchargeAmount = Math.Round(ccSurchargeAmount, utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                if (ccSurchargeAmount < 0)
                {
                    ccSurchargeAmount = 0;
                }
                TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTO.PaymentModeId, amountToPay + ccSurchargeAmount, qrCodeScanned, "", "", "", "",
                                                  -1, "", -1, -1, "", "", false, -1, -1, "", ServerDateTime.Now, executionContext.GetUserId(), -1, null, 0, -1,
                                                   executionContext.POSMachineName, -1, "", null);
                trxPaymentDTO.paymentModeDTO = paymentModeDTO;
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString()))
                {
                    SendCreditCardTrxInfoToVCATDevice(paymentModeDTO, trxPaymentDTO, amountToPay, ccSurchargeAmount);
                    log.LogMethodExit();
                    return;
                }
                if (Enum.IsDefined(typeof(PaymentGateways), paymentModeDTO.GatewayLookUp))
                {
                    trxPaymentDTO.paymentModeDTO.GatewayLookUp = (PaymentGateways)Enum.Parse(typeof(PaymentGateways), paymentModeDTO.GatewayLookUp.ToString(), true);
                }
                else
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 5027);
                    //Payment Gateway set up missing. Please contact Manager.
                    KioskStatic.logToFile(msg);
                    this.SendKioskPopupAlerts(msg);
                    log.LogMethodExit(msg);
                    return;
                }
                PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(trxPaymentDTO.paymentModeDTO.GatewayLookUp);
                paymentGateway.IsCreditCard = !isDebitCard;
                paymentGateway.PrintReceipt = printReceipt;

                string message = "";

                if (paymentGateway.IsPrinterRequired)
                {
                    try
                    {
                        DeviceContainer.PrinterStatus(executionContext);//checks for printer status.
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        SendKioskPopupAlerts(ex.Message);
                        KioskStatic.logToFile("Error while getting printer status:" + ex.Message);
                        return;
                    }
                }

                if (CreditCardPaymentGateway.MakePayment(trxPaymentDTO, utilities, ref message))
                {
                    try
                    {
                        AddCCSurchargeAmount(ccSurchargeAmount);
                        ProcessReceivedMoney(trxPaymentDTO);
                    }
                    finally
                    {
                        CreditCardPaymentGateway.PrintCCReceipt(trxPaymentDTO);
                    }
                    KioskStatic.logToFile("Credit card payment success");
                    if (ccSurchargeAmount > 0)
                    {
                        ReloadTransactionObject();
                    }
                    //Added check to see if Partial approval is not allowed and if credit card is partially approved then abort and exit
                    //current transaction
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_PARTIAL_APPROVAL", false) == false
                          && (amountToPay + ccSurchargeAmount) > trxPaymentDTO.Amount)
                    {
                        PerformAbortTrxAction();
                        SendkioskAbortTransactionEvent();
                        log.LogMethodExit();
                        return;
                    }
                    else if ((amountToPay + ccSurchargeAmount) > trxPaymentDTO.Amount)
                    {
                        //Partial amount &1 is approved.Press cancel button to proceed                        
                        string msg = MessageContainerList.GetMessage(executionContext, 2854, trxPaymentDTO.Amount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                        SendKioskPopupAlerts(msg);
                        KioskStatic.logToFile(msg);
                    }
                    if (paymentGateway.IsPartiallyApproved)
                    {
                        if ((amountToPay + ccSurchargeAmount) > trxPaymentDTO.Amount)
                        {
                            SendKioskPopupAlerts(message);
                        }
                    }
                }
                else
                {
                    ValidationException validationException = new ValidationException(message);
                    log.Error("CC payment failure", validationException);
                    throw validationException;
                }
            }
            finally
            {
                this.kioskAbortTransactionEvent = null;
                this.KioskPopupAlerts = null;
            }
            log.LogMethodExit();
        }
        public void ExecuteRedeemTokenTransaction(CardDispenser.CardDispenser cardDispenser, Card rechargeCard, ProgressUpdates progressUpdates,
            PopupAlerts popupAlerts, ShowThankYou showThankYou, bool generateReceipt)
        {
            log.LogMethodEntry(cardDispenser, rechargeCard, generateReceipt);
            KioskStatic.logToFile("In ExecuteRedeemTokenTransaction()");
            try
            {
                this.KioskProgressUpdates = progressUpdates;
                this.KioskPopupAlerts = popupAlerts;
                this.KioskShowThankYou = showThankYou;
                bool newCardScenario = (rechargeCard == null);
                if (rechargeCard == null)
                {
                    try
                    {
                        rechargeCard = GetCardFromDispenser(cardDispenser);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        KioskStatic.logToFile("Card dispenser error: " + ex.Message);
                        this.SendKioskProgressUpdates(ex.Message);
                        try
                        {
                            AddRedeemTokenTrxProducts(rechargeCard);
                        }
                        catch (Exception exx)
                        {
                            log.Error(exx);
                        }
                        string msg = MessageContainerList.GetMessage(executionContext, 441) + ". " + MessageContainerList.GetMessage(executionContext, 460);
                        //Please contact our staff. Problem in Card Dispenser. Cannot issue new card.
                        this.SendKioskPopupAlerts(msg);
                        this.SendKioskProgressUpdates(msg);
                        PerformAbortTrxAction();//Perform refund etc.
                        SendKioskShowThankYou(false, false);
                    }
                }
                if (rechargeCard != null)
                {
                    try
                    {
                        CompleteRedeemTokenTransaction(rechargeCard);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        if (newCardScenario)
                        {
                            RejectCard(cardDispenser, rechargeCard, REDEEMTOKENMSG);
                        }
                        KioskStatic.logToFile("Error in CompleteRedeemTokenTransaction: " + ex.Message);
                        string message = MessageContainerList.GetMessage(executionContext, 5028, ex.Message);
                        //"There was an error processing your request. The tokens inserted by you is recognized and will be refunded to you. Please contact our staff with the receipt. [" + ex.Message + "]"
                        this.SendKioskPopupAlerts(message);
                        PerformAbortTrxAction();//Perform refund etc.
                        SendKioskShowThankYou(false, false);
                        log.LogMethodExit();
                        return;
                    }
                    if (newCardScenario)
                    {
                        EjjectCard(cardDispenser, rechargeCard, REDEEMTOKENMSG);
                    }
                    else
                    {
                        KioskStatic.UpdateKioskActivityLog(executionContext, TOPUP, REDEEMTOKENMSG, rechargeCard.CardNumber, GetTransactionId, globalKioskTrxId);
                    }
                    if (generateReceipt)
                    {
                        try
                        {
                            GenerateRedeemTokenReceipt(KioskStatic.rc, generateReceipt);
                            Audio.PlayAudio(Audio.CollectReceipt, Audio.ThankYouEnjoyGame);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            SendKioskProgressUpdates(MessageContainerList.GetMessage(executionContext, 431, ex.Message));
                            Audio.PlayAudio(Audio.ThankYouEnjoyGame);
                        }
                    }
                    else
                    {
                        Audio.PlayAudio(Audio.ThankYouEnjoyGame);
                    }

                    //ac.totalValue = 0;

                    if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.cardAcceptor.EjectCardFront();
                        KioskStatic.cardAcceptor.BlockAllCards();
                    }
                }
            }
            finally
            {
                this.KioskProgressUpdates = null;
                this.KioskPopupAlerts = null;
                this.KioskShowThankYou = null;
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Get Total Payments Received
        /// </summary> 
        public decimal GetTotalPaymentsReceived()
        {
            log.LogMethodEntry();
            decimal totalPaymentReceived = 0;
            if (kioskTrx != null && kioskTrx.TransactionPaymentsDTOList != null && kioskTrx.TransactionPaymentsDTOList.Any())
            {
                for (int i = 0; i < kioskTrx.TransactionPaymentsDTOList.Count; i++)
                {
                    TransactionPaymentsDTO trxPaymentDTO = kioskTrx.TransactionPaymentsDTOList[i];
                    totalPaymentReceived = totalPaymentReceived + Convert.ToDecimal(trxPaymentDTO.Amount)
                                           + Convert.ToDecimal((trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard
                                               ? trxPaymentDTO.PaymentUsedCreditPlus : 0));
                } 
            }
            log.LogMethodExit(totalPaymentReceived);
            return totalPaymentReceived;
        }
        /// <summary>
        /// ReceivedMoneyToProcess
        /// </summary>
        /// <returns></returns>
        public bool HasMoneyToProcess()
        {
            log.LogMethodEntry();
            bool hasMoneyToProcess = false;
            //if (kioskTrx != null && acceptedCashDTOList != null && acceptedCashDTOList.Any() && acceptedCashDTOList.Exists(c => c.IsProcesed == false))
            //{
            //    hasCashToProcess = true;
            //}
            if (kioskTrx != null && kioskTrx.TransactionPaymentsDTOList != null && kioskTrx.TransactionPaymentsDTOList.Any()
                && kioskTrx.TransactionPaymentsDTOList.Exists( tp => tp.PaymentId == -1))
            {
                hasMoneyToProcess = true;
            }
            log.LogMethodExit(hasMoneyToProcess);
            return hasMoneyToProcess;

        }
        /// <summary>
        /// ProcessReceivedMoney
        /// </summary>
        public void ProcessReceivedMoney(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            KioskStatic.logToFile("In ProcessReceivedMoney: " + (transactionPaymentsDTO != null? "transactionPaymentsDTO": "null"));
            if (kioskTrx != null 
                && (kioskTrx.TransactionPaymentsDTOList != null && kioskTrx.TransactionPaymentsDTOList.Any()
               && (kioskTrx.TransactionPaymentsDTOList.Exists(tp => tp.PaymentId == -1)) || (transactionPaymentsDTO != null && transactionPaymentsDTO.PaymentId == -1)))
            {
                lock (notifyingObjectToProcessCash)
                {
                    AddToTransactionPaymentDTOList(transactionPaymentsDTO);
                    SavePayments();
                }
            }
            log.LogMethodExit(); 
        }
        /// <summary>
        /// Get CreditCard Payments Received
        /// </summary> 
        public decimal GetCreditCardPaymentAmount()
        {
            log.LogMethodEntry();
            decimal ccPaymentAmount = 0;
            if (kioskTrx != null && kioskTrx.TransactionPaymentsDTOList != null && kioskTrx.TransactionPaymentsDTOList.Any())
            {
                for (int i = 0; i < kioskTrx.TransactionPaymentsDTOList.Count; i++)
                {
                    TransactionPaymentsDTO trxPaymentDTO = kioskTrx.TransactionPaymentsDTOList[i];
                    if (trxPaymentDTO != null && trxPaymentDTO.PaymentModeDTO != null
                        && trxPaymentDTO.PaymentModeDTO.IsCreditCard == true
                        && (trxPaymentDTO.GatewayPaymentProcessed || trxPaymentDTO.PaymentId > -1))
                    {
                        ccPaymentAmount = ccPaymentAmount + Convert.ToDecimal(trxPaymentDTO.Amount);
                    }
                }
            }
            log.LogMethodExit(ccPaymentAmount);
            return ccPaymentAmount;
        }
        /// <summary>
        /// Get Game card Payments Received
        /// </summary> 
        public decimal GetGameCardPaymentAmount()
        {
            log.LogMethodEntry();
            decimal gameCardAmount = 0;
            if (kioskTrx != null && kioskTrx.TransactionPaymentsDTOList != null && kioskTrx.TransactionPaymentsDTOList.Any())
            {
                for (int i = 0; i < kioskTrx.TransactionPaymentsDTOList.Count; i++)
                {
                    TransactionPaymentsDTO trxPaymentDTO = kioskTrx.TransactionPaymentsDTOList[i];
                    if (trxPaymentDTO != null && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard)
                    {
                        gameCardAmount = gameCardAmount + Convert.ToDecimal(trxPaymentDTO.Amount) + Convert.ToDecimal(trxPaymentDTO.PaymentUsedCreditPlus);
                    }
                }
            }
            log.LogMethodExit(gameCardAmount);
            return gameCardAmount;
        }
        /// <summary>
        /// Get cash Payments Received
        /// </summary> 
        public decimal GetCashPaymentAmount()
        {
            log.LogMethodEntry();
            decimal cashPaymentAmount = 0;
            if (kioskTrx != null && kioskTrx.TransactionPaymentsDTOList != null && kioskTrx.TransactionPaymentsDTOList.Any())
            {
                for (int i = 0; i < kioskTrx.TransactionPaymentsDTOList.Count; i++)
                {
                    TransactionPaymentsDTO trxPaymentDTO = kioskTrx.TransactionPaymentsDTOList[i];
                    if (trxPaymentDTO != null && trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsCash)
                    {
                        cashPaymentAmount = cashPaymentAmount + Convert.ToDecimal(trxPaymentDTO.Amount);
                    }
                }
            }
            log.LogMethodExit(cashPaymentAmount);
            return cashPaymentAmount;
        }
        /// <summary>
        /// Has temp cards
        /// </summary> 
        public bool HasTempCards()
        {
            log.LogMethodEntry();
            bool hasTempCards = false;
            if (kioskTrx != null && kioskTrx.TrxLines.Exists(tl => tl.LineValid && string.IsNullOrWhiteSpace(tl.CardNumber) == false && tl.CardNumber.StartsWith("T")))
            {
                hasTempCards = true;
            }
            log.LogMethodExit(hasTempCards);
            return hasTempCards;
        }
        /// <summary>
        /// Has Unsaved Items
        /// </summary> 
        public bool HasUnsavedItems()
        {
            log.LogMethodEntry();
            bool hasUnsavedData = false;
            if (kioskTrx != null && kioskTrx.Trx_id == 0 && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any() &&
                kioskTrx.TrxLines.Exists(tl => tl.LineValid))
            {
                hasUnsavedData = true;
            }
            log.LogMethodExit(hasUnsavedData);
            return hasUnsavedData;
        }
        /// <summary>
        /// CardCreditPlusConsumptionIsUsed
        /// </summary>
        /// <returns></returns>
        public bool CardCreditPlusConsumptionIsUsed()
        {
            log.LogMethodEntry();
            bool consumptionUsed = false;
            if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any()
                && kioskTrx.TrxLines.Exists(tl => tl.LineValid && tl.CreditPlusConsumptionId > -1))
            {
                consumptionUsed = true;
            }
            log.LogMethodExit(consumptionUsed);
            return consumptionUsed;
        }

        /// <summary>
        /// Has Active Transaction Lines
        /// </summary>
        /// <returns></returns>
        public bool HasActiveTransactionLines()
        {
            log.LogMethodEntry();
            bool hasActiveLines = false;
            if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any()
                && kioskTrx.TrxLines.Exists(tl => tl.LineValid))
            {
                hasActiveLines = true;
            }
            log.LogMethodExit(hasActiveLines);
            return hasActiveLines;
        }
        /// <summary>
        /// Has Active Non FundRaiser Or DonationProducts
        /// </summary>
        /// <returns></returns>
        public bool HasActiveNonFundRaiserOrDonationOrChargeProducts()
        {
            log.LogMethodEntry();
            bool hasActiveProducts = false;
            if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any()
                && kioskTrx.TrxLines.Exists(tl => tl.LineValid
                && tl.ProductTypeCode.Equals(ProductTypeValues.SERVICECHARGE) == false
                && tl.ProductTypeCode.Equals(ProductTypeValues.GRATUITY) == false
                && NotAFundRaiserOorDonationProduct(tl.ProductID) == true))
            {
                hasActiveProducts = true;
            }
            log.LogMethodExit(hasActiveProducts);
            return hasActiveProducts;
        }
        /// <summary>
        /// Has Customer Record
        /// </summary> 
        public bool HasCustomerRecord()
        {
            log.LogMethodEntry();
            bool hasCustomer = false;
            if (kioskTrx != null && kioskTrx.customerDTO != null)
            {
                hasCustomer = true;
            }
            log.LogMethodExit(hasCustomer);
            return hasCustomer;
        }
        /// <summary>
        /// Get Trx NetAmount
        /// </summary>
        public decimal GetTrxNetAmount()
        {
            log.LogMethodEntry();
            decimal netAmount = (kioskTrx != null ? Convert.ToDecimal(kioskTrx.Net_Transaction_Amount) : 0);
            log.LogMethodExit(netAmount);
            return netAmount;
        }
        /// <summary>
        /// Get Trx Tax Amount
        /// </summary>
        public double GetTrxTaxAmount()
        {
            log.LogMethodEntry();
            double taxAmount = (kioskTrx != null ? kioskTrx.Tax_Amount : 0);
            log.LogMethodExit(taxAmount);
            return taxAmount;
        }
        /// <summary>
        /// Get Transaction Amount
        /// </summary>
        public double GetTransactionAmount()
        {
            log.LogMethodEntry();
            double ttxAmount = (kioskTrx != null ? kioskTrx.Transaction_Amount : 0);
            log.LogMethodExit(ttxAmount);
            return ttxAmount;
        }
        /// <summary>
        /// Get Transaction ServiceCharges
        /// </summary>
        public double GetTransactionServiceCharges()
        {
            log.LogMethodEntry();
            double serviceCharges = 0;
            if (kioskTrx != null)
            {
                for (int i = 0; i < kioskTrx.TrxLines.Count; i++)
                {
                    if (kioskTrx.TrxLines[i].LineValid && kioskTrx.TrxLines[i].ProductTypeCode == ProductTypeValues.SERVICECHARGE)
                    {
                        serviceCharges += kioskTrx.TrxLines[i].LineAmount;
                    }
                }
            }
            log.LogMethodExit(serviceCharges);
            return serviceCharges;
        }
        /// <summary>
        /// Get Transaction Discounts Amount
        /// </summary>
        /// <returns></returns>
        public double GetTransactionDiscountsAmount()
        {
            log.LogMethodEntry();
            double discountAmount = 0;
            if (kioskTrx != null)
            {
                if (kioskTrx.DiscountsSummaryDTOList != null && kioskTrx.DiscountsSummaryDTOList.Any())
                {
                    for (int i = 0; i < kioskTrx.DiscountsSummaryDTOList.Count; i++)
                    {
                        if (kioskTrx.DiscountsSummaryDTOList[i].DiscountAmount > 0)
                        {
                            discountAmount = discountAmount + (double)kioskTrx.DiscountsSummaryDTOList[i].DiscountAmount;
                        }
                    }
                }
            }
            log.LogMethodExit(discountAmount);
            return discountAmount;
        }

        /// <summary>
        /// Get Transaction Gratuity Amount
        /// </summary>
        public double GetTransactionGratuityAmount()
        {
            log.LogMethodEntry();
            double gratuityCharges = 0;
            if (kioskTrx != null)
            {
                for (int i = 0; i < kioskTrx.TrxLines.Count; i++)
                {
                    if (kioskTrx.TrxLines[i].LineValid && kioskTrx.TrxLines[i].ProductTypeCode == ProductTypeValues.GRATUITY)
                    {
                        gratuityCharges += kioskTrx.TrxLines[i].LineAmount;
                    }
                }
            }
            log.LogMethodExit(gratuityCharges);
            return gratuityCharges;
        }
        public double GetTaxAmountOnCharges()
        {
            log.LogMethodEntry();
            double taxAmount = 0;
            if (kioskTrx != null)
            {
                for (int i = 0; i < kioskTrx.TrxLines.Count; i++)
                {
                    if (kioskTrx.TrxLines[i].LineValid
                        && (kioskTrx.TrxLines[i].ProductTypeCode == ProductTypeValues.SERVICECHARGE
                        || kioskTrx.TrxLines[i].ProductTypeCode == ProductTypeValues.GRATUITY))
                    {
                        taxAmount += kioskTrx.TrxLines[i].tax_amount;
                    }
                }
            }
            log.LogMethodExit(taxAmount);
            return taxAmount;
        }
        /// <summary>
        /// Add Coin Payment
        /// </summary> 
        public void AddCoinPayment(int coinIndex)
        {
            log.LogMethodEntry(coinIndex);
            if (coinIndex > 0)
            {
                decimal coinValue = KioskStatic.config.Coins[coinIndex].Value;
                AddCashPaymentEntry(coinValue);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Add Cash Note Payment
        /// </summary> 
        public void AddCashNotePayment(int noteIndex)
        {
            log.LogMethodEntry(noteIndex);
            if (noteIndex > 0)
            {
                decimal noteValue = KioskStatic.config.Notes[noteIndex].Value;
                AddCashPaymentEntry(noteValue); 
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Transaction Has Items
        /// </summary> 
        public void TransactionHasItems()
        {
            log.LogMethodEntry();
            //TransactionIsNotNull();
            if (kioskTrx == null)
            {
                string msg = MessageContainerList.GetMessage(executionContext, 2907, "Transaction");
                //&1 details are missing
                ValidationException validationException = new ValidationException(msg);
                log.Error("", validationException);
                throw validationException;
            }
            if (kioskTrx.TrxLines == null || kioskTrx.TrxLines.Any() == false)
            {
                ValidationException validationException = new ValidationException(MessageContainerList.GetMessage(executionContext,
                       5029));
                //Please add Items to the transaction
                log.Error("", validationException);
                throw validationException;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Containes Line For Any Of These Products
        /// </summary> 
        public bool ContainesLineForAnyOfTheseProducts(List<ProductsDTO> activeProducts)
        {
            log.LogMethodEntry();
            bool alreadyAdded = false;
            if (activeProducts != null && activeProducts.Any() && kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                alreadyAdded = kioskTrx.TrxLines.Exists(tl => tl.LineValid && activeProducts.Exists(p => p.ProductId == tl.ProductID));
            }
            log.LogMethodExit(alreadyAdded);
            return alreadyAdded;
        }
        /// <summary>
        /// Containes Line For Any Of These Products
        /// </summary> 
        public bool ContainesLineForAnyOfTheseProducts(List<ProductsContainerDTO> activeProductContainerList)
        {
            log.LogMethodEntry();
            bool alreadyAdded = false;
            if (activeProductContainerList != null && activeProductContainerList.Any() && kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                alreadyAdded = kioskTrx.TrxLines.Exists(tl => tl.LineValid && activeProductContainerList.Exists(p => p.ProductId == tl.ProductID));
            }
            log.LogMethodExit(alreadyAdded);
            return alreadyAdded;
        }
        /// <summary>
        /// Containes Line For The Product
        /// </summary> 
        public bool ContainesLineForTheProduct(int productId, int comboProductId = -1)
        {
            log.LogMethodEntry(productId, comboProductId);
            bool alreadyAdded = false;
            if (productId > -1 && kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                alreadyAdded = kioskTrx.TrxLines.Exists(tl => tl.LineValid
                                && productId == tl.ProductID
                                && (comboProductId == tl.ComboproductId));
            }
            log.LogMethodExit(alreadyAdded);
            return alreadyAdded;
        }
        /// <summary>
        /// Has Some Discounts
        /// </summary> 
        public bool HasSomeDiscounts()
        {
            log.LogMethodEntry();
            bool applied = false;
            applied = (kioskTrx != null && kioskTrx.DiscountsSummaryDTOList != null && kioskTrx.DiscountsSummaryDTOList.Any());
            log.LogMethodExit(applied);
            return applied;
        }
        /// <summary>
        /// Get Applied Discount Summary For Coupon
        /// </summary> 
        /// <returns></returns>
        public DiscountsSummaryDTO GetAppliedDiscountSummaryForCoupon(string couponNumber)
        {
            log.LogMethodEntry(couponNumber);
            DiscountsSummaryDTO trxDiscountSummary = null;
            if (kioskTrx != null && kioskTrx.DiscountsSummaryDTOList != null && kioskTrx.DiscountsSummaryDTOList.Any())
            {
                trxDiscountSummary = kioskTrx.DiscountsSummaryDTOList.Find(tds => tds.CouponNumbers != null
                                                     && tds.CouponNumbers.Any() && tds.CouponNumbers.Contains(couponNumber));
            }
            log.LogMethodExit(trxDiscountSummary);
            return trxDiscountSummary;

        }
        /// <summary>
        /// Get Applied coupon Discount Summary
        /// </summary> 
        /// <returns></returns>
        public DiscountsSummaryDTO GetAppliedCouponDiscountSummary()
        {
            log.LogMethodEntry();
            DiscountsSummaryDTO trxDiscountSummary = null;
            if (kioskTrx != null && kioskTrx.DiscountsSummaryDTOList != null && kioskTrx.DiscountsSummaryDTOList.Any())
            {
                trxDiscountSummary = kioskTrx.DiscountsSummaryDTOList.Find(tds => tds.CouponNumbers != null
                                                     && tds.CouponNumbers.Any());
            }
            log.LogMethodExit(trxDiscountSummary);
            return trxDiscountSummary;

        }
        /// <summary>
        /// Validate Variable Amount
        /// </summary> 
        /// <returns></returns>
        public double ValidateVariableAmount(double varAmount)
        {
            log.LogMethodEntry(varAmount);
            if (varAmount <= 0)
            {
                string errMsg = MessageContainerList.GetMessage(executionContext, 5030);
                //Variable amount cannot be less than or equal to Zero
                ValidationException validationException = new ValidationException(errMsg);
                throw validationException;
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_DECIMALS_IN_VARIABLE_RECHARGE", false))
            {
                varAmount = Math.Round(varAmount, 2);
            }
            else
            {
                if (varAmount != Math.Round(varAmount, 0))
                {
                    string errMsg = MessageContainerList.GetMessage(executionContext, 932);//Please enter a whole amount. 
                    ValidationException validationException = new ValidationException(errMsg);
                    throw validationException;
                }
            }
            double maxVariableRechargeAmount = ParafaitDefaultContainerList.GetParafaitDefault<double>(executionContext, "MAX_VARIABLE_RECHARGE_AMOUNT", 100);
            if (varAmount > maxVariableRechargeAmount)
            {
                string errMsg = MessageContainerList.GetMessage(executionContext, 930, maxVariableRechargeAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                //Maximum allowed amount is &1 
                ValidationException validationException = new ValidationException(errMsg);
                throw validationException;
            }
            log.LogMethodExit(varAmount);
            return varAmount;
        }
        /// <summary> 
        /// Cancel Existing Products And Add New Variable Card
        /// </summary> 
        /// <returns></returns>
        public void CancelExistingProductsAndAddNewVariableCard()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In CancelExistingProductsAndAddNewVariableCard()");
            CancelOldActiveTransactionLines();
            decimal variableAmount = GetTotalPaymentsReceived() - GetCCSurchargeAmount;
            int variableProductId = utilities.ParafaitEnv.VariableRechargeProductId;
            selectedProductType = KioskTransaction.GETNEWCARDTYPE;
            int quantity = 1;
            AddVariableCardProduct(variableProductId, (double)variableAmount, selectedProductType, null, quantity);
            selectedProductType = string.Empty;
            log.LogMethodExit();
        }
        /// <summary> 
        /// Cancel Existing Products And Add Variable RechargeCard
        /// </summary> 
        /// <returns></returns>

        public void CancelExistingProductsAndAddVariableRechargeCard()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In CancelExistingProductsAndAddVariableRechargeCard()");
            Card card = GetTrxPrimaryCard();
            CancelOldActiveTransactionLines();
            decimal variableAmount = GetTotalPaymentsReceived() - GetCCSurchargeAmount;
            int variableProductId = utilities.ParafaitEnv.VariableRechargeProductId;
            selectedProductType = KioskTransaction.GETRECHAREGETYPE;
            int quantity = 1;
            AddVariableCardProduct(variableProductId, (double)variableAmount, selectedProductType, card, quantity);
            selectedProductType = string.Empty;
            log.LogMethodExit();
        }
        /// <summary>
        /// ClearOldActiveLinesForNoCartTransaction
        /// </summary>
        public void ClearOldActiveLinesForNoCartTransaction()
        {
            log.LogMethodEntry();
            if (ShowCartInKiosk == false)
            {
                KioskStatic.logToFile("In ClearOldActiveLinesForNoCartTransaction()");
                if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
                {
                    //clear any previously added lines
                    if (HasActiveTransactionLines())
                    {
                        CancelOldActiveTransactionLines();
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// GetNumOfCardCreditPlusBalanceAvailable
        /// </summary> 
        public int GetNumOfCardCreditPlusBalanceAvailable(List<ProductsContainerDTO> childProductsContainerDTOList, string inCardNumber)
        {
            log.LogMethodEntry();
            int consumptionBalance = 0;
            try
            {
                Card primaryCard = GetTransactionPrimaryCard;
                if (primaryCard != null || string.IsNullOrEmpty(inCardNumber) == false)
                {
                    AccountBL accountBL = new AccountBL(KioskStatic.Utilities.ExecutionContext, (primaryCard != null) ? primaryCard.CardNumber : inCardNumber);
                    if (accountBL != null)
                    {
                        int totalConsumptionBalance = accountBL.GetApplicableCardCPConsumptionsBalance(childProductsContainerDTOList);
                        int usedBalance = 0;
                        List<Transaction.Transaction.TransactionLine> trxLines = GetActiveTrxLines();
                        if (trxLines != null && trxLines.Any())
                        {
                            usedBalance = trxLines.Count(t => t.CreditPlusConsumptionId > -1);
                        }

                        consumptionBalance = (totalConsumptionBalance - usedBalance) > 0 ? (totalConsumptionBalance - usedBalance) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while fetching Passport Coupons: " + ex.Message);
            }
            log.LogMethodExit(consumptionBalance);
            return consumptionBalance;
        }
        /// <summary>
        /// Waiver Mapping Is Pending
        /// </summary>
        /// <returns></returns>
        public bool WaiverMappingIsPending()
        {
            log.LogMethodEntry();
            bool waiverMappingIsPending = false;
            if (kioskTrx != null)
            {
                waiverMappingIsPending = kioskTrx.IsWaiverSignaturePending();
            }
            log.LogMethodExit(waiverMappingIsPending);
            return waiverMappingIsPending;
        }
        public bool HasCheckinProducts()
        {
            log.LogMethodEntry();
            bool hasCheckInProducts = false;
            if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                hasCheckInProducts = kioskTrx.TrxLines.Exists(tl => tl.LineValid && tl.ProductTypeCode == ProductTypeValues.CHECKIN);
            }
            log.LogMethodExit(hasCheckInProducts);
            return hasCheckInProducts;
        }
        public bool HasAttractionProducts()
        {
            log.LogMethodEntry();
            bool hasAttractionProducts = false;
            if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                hasAttractionProducts = kioskTrx.TrxLines.Exists(tl => tl.LineValid && tl.ProductTypeCode == ProductTypeValues.ATTRACTION);
            }
            log.LogMethodExit(hasAttractionProducts);
            return hasAttractionProducts;
        }

        /// <summary>
        /// GenerateAttractionCards
        /// </summary> 
        public KioskAttractionDTO GenerateAttractionCards(KioskAttractionDTO kioskAttrcationDTO)
        {
            log.LogMethodEntry(executionContext);
            int qty = kioskAttrcationDTO.Quantity;
            ProductsContainerDTO pContainer = ProductsContainerList.GetProductsContainerDTO(executionContext, kioskAttrcationDTO.ProductId);
            if (kioskAttrcationDTO.ChildAttractionBookingDTOList != null && kioskAttrcationDTO.ChildAttractionBookingDTOList.Any())
            {
                kioskAttrcationDTO = GenerateAttractionCardsForCombCHilds(kioskAttrcationDTO, qty, pContainer);
            }
            else
            {
                kioskAttrcationDTO = GenerateAttractionCardsForSingleProduct(kioskAttrcationDTO, qty, pContainer);
            }
            log.LogMethodExit(kioskAttrcationDTO);
            return kioskAttrcationDTO;
        }

        public KioskAttractionDTO ClearTemporarySlots(KioskAttractionDTO kioskAttractionDTO)
        {
            log.LogMethodEntry(kioskAttractionDTO);
            KioskStatic.logToFile("In ClearTemporarySlots()");

            if (kioskAttractionDTO != null && isOnlineTransaction == false)
            {
                if (kioskAttractionDTO.ChildAttractionBookingDTOList != null && kioskAttractionDTO.ChildAttractionBookingDTOList.Any())
                {
                    foreach (KioskAttractionChildDTO item in kioskAttractionDTO.ChildAttractionBookingDTOList)
                    {
                        if (item.ChildAttractionBookingDTO != null &&
                            item.ChildAttractionBookingDTO.BookingId > -1)
                        {
                            try
                            {
                                AttractionBooking atb = new AttractionBooking(executionContext, item.ChildAttractionBookingDTO);
                                atb.Expire();
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                    }
                }
                else
                {
                    if (kioskAttractionDTO.AttractionBookingDTO != null &&
                            kioskAttractionDTO.AttractionBookingDTO.BookingId > -1)
                    {
                        try
                        {
                            AttractionBooking atb = new AttractionBooking(executionContext, kioskAttractionDTO.AttractionBookingDTO);
                            atb.Expire();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                }
                kioskAttractionDTO = null;
            }
            log.LogMethodExit();
            return kioskAttractionDTO;
        }
        /// <summary>
        /// GetExecuteOnlineReceipt
        /// </summary> 
        /// <returns></returns>
        public virtual Printer.ReceiptClass GetExecuteOnlineReceipt(POSPrinterDTO posPrinterDTO, int cardCount)
        {
            log.LogMethodEntry(posPrinterDTO, cardCount);
            log.LogMethodExit();
            throw new NotImplementedException();
        }
        /// <summary>
        /// GetExecuteOnlineErrorReceipt
        /// </summary> 
        /// <returns></returns>
        public virtual Printer.ReceiptClass GetExecuteOnlineErrorReceipt(POSPrinterDTO posPrinterDTO, int cardCount)
        {
            log.LogMethodEntry(posPrinterDTO, cardCount);
            log.LogMethodExit();
            throw new NotImplementedException();
        }

        /// <summary>
        /// GetEntitlementReferenceDate
        /// </summary>
        /// <returns>EntitlementReferenceDate</returns>
        public DateTime GetEntitlementReferenceDate()
        {
            log.LogMethodEntry();
            DateTime entitlementReferenceDate = kioskTrx.EntitlementReferenceDate;
            log.LogMethodExit(entitlementReferenceDate);
            return entitlementReferenceDate;
        }
        /// <summary>
        /// CheckLicenseForCustomerAndCard
        /// </summary> 
        /// <returns></returns>
        public string CheckLicenseForCustomerAndCard(AccountDTO selectedAccountDTO, String LicenseType, DateTime entitlementConsumptionDate, int customerId)
        {
            log.LogMethodEntry();
            string licenseType = kioskTrx.CheckLicenseForCustomerAndCard(selectedAccountDTO, LicenseType, entitlementConsumptionDate, customerId, null);
            log.LogMethodExit(licenseType);
            return licenseType;
        }

        /// <summary>
        /// MapCustomerWaiversToLine
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="customerDTO"></param>
        public void MapCustomerWaiversToLine(int lineId, CustomerDTO customerDTO)
        {
            log.LogMethodEntry();
            if (isOnlineTransaction)
            {
                SaveWaiversSignedViaExecuteOnline(lineId, customerDTO);
            }
            else
            {
                kioskTrx.MapCustomerWaiversToLine(lineId, customerDTO);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// GetTrxLines
        /// </summary>
        /// <returns></returns>
        public List<Transaction.Transaction.TransactionLine> GetTrxLines()
        {
            log.LogMethodEntry();
            List<Transaction.Transaction.TransactionLine> transactionLineList = new List<Transaction.Transaction.TransactionLine>();
            if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                transactionLineList = kioskTrx.TrxLines;
            }
            log.LogMethodExit(transactionLineList);
            return transactionLineList;

        }
        /// <summary>
        /// SetSelectedEntitlementType
        /// </summary>
        /// <param name="entitlementType"></param>
        public void SetSelectedEntitlementType(string entitlementType)
        {
            log.LogMethodEntry();
            if (this.showCartInKiosk == false)
            {
                selectedEntitlement = entitlementType;
            }
            log.LogMethodExit();
        }
        #endregion
        #region Private methods
        private void CancelOldActiveTransactionLines()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In CancelOldActiveTransactionLines()");
            if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                for (int i = 0; i < kioskTrx.TrxLines.Count; i++)
                {
                    if (kioskTrx.TrxLines[i].LineValid
                        && kioskTrx.TrxLines[i].ProductID != utilities.ParafaitEnv.CreditCardSurchargeProductId
                        && NotAFundRaiserOorDonationProduct(kioskTrx.TrxLines[i].ProductID))
                    {
                        bool retVal = kioskTrx.CancelTransactionLine(i);
                        if (retVal == false)
                        {
                            string msg = MessageContainerList.GetMessage(executionContext, 5024);
                            // "Unexexpected error, unable to cancel the transaction line"
                            ValidationException validationException = new ValidationException(msg);
                            log.Error("Error in CancelActiveTransactionLines", validationException);
                            throw validationException;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private bool NotAFundRaiserOorDonationProduct(int productID)
        {
            log.LogMethodEntry(productID);
            bool notAFundOrDonationProduct = true;
            if (fundRaiserProductList == null)
            {
                fundRaiserProductList = KioskHelper.GetActiveFundRaiserProducts(executionContext);
                if (fundRaiserProductList == null) //next time no need to check again
                {
                    fundRaiserProductList = new List<ProductsDTO>();
                }
            }
            if (donationProductList == null)
            {
                donationProductList = KioskHelper.GetActiveDonationProducts(executionContext);
                if (donationProductList == null)//next time no need to check again
                {
                    donationProductList = new List<ProductsDTO>();
                }
            }
            if (fundRaiserProductList != null && fundRaiserProductList.Any())
            {
                notAFundOrDonationProduct = fundRaiserProductList.Exists(p => p.ProductId == productID) ? false : true;
            }
            if (notAFundOrDonationProduct == true && donationProductList != null && donationProductList.Any())
            {
                notAFundOrDonationProduct = donationProductList.Exists(p => p.ProductId == productID) ? false : true;
            }
            log.LogMethodExit(notAFundOrDonationProduct);
            return notAFundOrDonationProduct;
        }

        private void AbortKioskTransaction()
        {
            log.LogMethodEntry();
            if (kioskTrx != null)
            {
                KioskStatic.logToFile("In AbortKioskTransaction()");
                bool waiverSignaturePedning = kioskTrx.IsWaiverSignaturePending();
                bool hasCheckInProducts = HasCheckinProducts();
                decimal amountReceived = GetTotalPaymentsReceived();
                decimal amountRequired = GetTrxNetAmount();
                if (kioskTrx.Status == Transaction.Transaction.TrxStatus.CLOSED
                    || kioskTrx.Status == Transaction.Transaction.TrxStatus.CANCELLED
                    || kioskTrx.Status == Transaction.Transaction.TrxStatus.SYSTEMABANDONED
                    || (kioskTrx.Trx_id > 0 && kioskTrx.Status == Transaction.Transaction.TrxStatus.OPEN
                           && (waiverSignaturePedning || hasCheckInProducts) && amountReceived == amountRequired))
                {
                    string statusValue = waiverSignaturePedning ? kioskTrx.Status + "-Pending waiver mapping"
                                                               : (hasCheckInProducts ? kioskTrx.Status + "-with CheckIn Products" : kioskTrx.Status.ToString());
                    string msg = "Transaction is already in " + statusValue + " status. Skippng abort transaction action.";
                    kioskTrx = null;
                    KioskStatic.logToFile(msg);
                    log.LogMethodExit(msg);
                    return;
                }
                //CancelCCPayment();
                PerformAbortTrxAction();
            }
            log.LogMethodExit();
        }

        private void PerformAbortTrxAction()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In PerformAbortTrxAction()");
            bool redeemTokenTrx = false;
            decimal totalAmount = 0;
            decimal cashAmount = 0;
            decimal creditCardAmount = 0;
            decimal gameCardAmount = 0;
            bool disableAutoPrintCheck = false;
            if (kioskTrx.Trx_id > 0
                || kioskTrx.TransactionPaymentsDTOList != null && kioskTrx.TransactionPaymentsDTOList.Any()
               )
            {
                ProductsContainerDTO redeemTokenProductDTO = GetRedeemTokenProduct();
                if (kioskTrx.TrxLines != null && kioskTrx.TrxLines.Exists(tl => tl.ProductID == redeemTokenProductDTO.ProductId))
                {
                    redeemTokenTrx = true;
                }
                log.LogVariableState("redeemTokenTrx", redeemTokenTrx);
                totalAmount = GetTotalPaymentsReceived();
                cashAmount = GetCashPaymentAmount();
                creditCardAmount = GetCreditCardPaymentAmount();
                gameCardAmount = GetGameCardPaymentAmount();
                if (receciptDeliveryMode == KioskReceiptDeliveryMode.EMAIL)
                {
                    disableAutoPrintCheck = true;
                }
            }
            bool succ = false;
            if (kioskTrx.Trx_id > 0)
            {
                string message = string.Empty;
                TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                succ = TransactionUtils.reverseTransaction(kioskTrx.Trx_id, -1, true, executionContext.POSMachineName, executionContext.GetUserId(),
                    executionContext.GetUserPKId(), executionContext.GetUserId(), "Kiosk Transaction Reversal", ref message, disableAutoPrintCheck);
                if (succ == false)
                {
                    ValidationException validationException = new ValidationException(message);
                    log.Error("Error in AbortKioskTransaction", validationException);
                    throw validationException;
                }
                else
                {
                    GenerateTrxReceipt(redeemTokenTrx, totalAmount, cashAmount, creditCardAmount, gameCardAmount);
                }
                kioskTrx = null;
            }
            else
            {
                try
                {
                    if (kioskTrx.TransactionPaymentsDTOList != null && kioskTrx.TransactionPaymentsDTOList.Any())
                    {
                        RefundUnsavedCreditCardPayment();
                        GenerateTrxReceipt(redeemTokenTrx, totalAmount, cashAmount, creditCardAmount, gameCardAmount);
                    }
                    kioskTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
                    kioskTrx.ClearUnSavedSchedules(null);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                kioskTrx = null;
            }
            log.LogMethodExit();
        }

        private void RefundUnsavedCreditCardPayment()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In RefundUnsavedCreditCardPayment()");

            if (kioskTrx.Trx_id <= 0 && kioskTrx.TransactionPaymentsDTOList != null && kioskTrx.TransactionPaymentsDTOList.Any()
                && kioskTrx.TransactionPaymentsDTOList.Exists(tp => tp.PaymentModeDTO != null && tp.PaymentModeDTO.IsCreditCard))
            {
                foreach (TransactionPaymentsDTO item in kioskTrx.TransactionPaymentsDTOList.Where(tp => tp.PaymentModeDTO != null && tp.PaymentModeDTO.IsCreditCard))
                {

                    string mes = "";
                    try
                    {
                        //SendKioskPopupAlerts(message);
                        mes = MessageContainerList.GetMessage(executionContext, "REVERSING CREDIT CARD PAYMENT");
                        if (item != null && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString()))
                        {
                            ReverseFiscalization(item);
                            continue; //Skip cc refund call
                        }
                        mes = string.Empty;
                        if (CreditCardPaymentGateway.RefundAmount(item, utilities, ref mes))
                        {
                            CreditCardPaymentGateway.PrintCCReceipt(item);
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
            }

            log.LogMethodExit();
        }
        private void GenerateTrxReceipt(bool redeemTokenTrx, decimal totalAmount, decimal cashAmount, decimal creditCardAmount, decimal gameCardAmount)
        {
            log.LogMethodEntry(redeemTokenTrx, totalAmount, cashAmount, creditCardAmount, gameCardAmount);
            if (redeemTokenTrx)
            {
                GenerateRedeemTokenAbortReceipt(KioskStatic.rc);
            }
            else
            {
                GenerateAbortReceipt(ABORTKIOSKTRXMSG, totalAmount, cashAmount, gameCardAmount, creditCardAmount);
            }
            log.LogMethodExit();
        }
        private void GenerateAbortReceipt(string errorMessage, decimal totalAmount, decimal cashAmount, decimal gameCardAmount, decimal creditCardAmount)
        {
            log.LogMethodEntry(errorMessage, cashAmount, gameCardAmount, creditCardAmount);
            try
            {
                KioskStatic.logToFile("GenerateAbortReceipt- AbortAndExit: " + errorMessage);
                if (cashAmount > 0)
                {
                    KioskStatic.logToFile("Cash: " + cashAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                    log.Info("Cash: " + cashAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                    KioskStatic.UpdateKioskActivityLog(executionContext, GETABORTCASH, errorMessage, null, GetTransactionId, globalKioskTrxId, (double)cashAmount);
                }
                if (creditCardAmount > 0)
                {
                    KioskStatic.logToFile("Credit Card: " + creditCardAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                    log.Info("Credit Card: " + creditCardAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                    KioskStatic.UpdateKioskActivityLog(executionContext, GETABORTCREDITCARD, errorMessage, null, GetTransactionId, globalKioskTrxId, (double)creditCardAmount);
                }
                if (gameCardAmount > 0)
                {
                    KioskStatic.logToFile("Game Card: " + gameCardAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                    log.Info("Game Card: " + gameCardAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                    KioskStatic.UpdateKioskActivityLog(executionContext, GETABORTGAMECARD, errorMessage, null, GetTransactionId, globalKioskTrxId, (double)gameCardAmount);
                }
                GenerateAbortPrint(KioskStatic.rc, totalAmount, cashAmount, gameCardAmount, creditCardAmount);

                string message = MessageContainerList.GetMessage(executionContext, "Transaction could not be completed.");
                if (cashAmount > 0)
                {
                    message += (" " + MessageContainerList.GetMessage(executionContext, "Money inserted by you has been recognized."));
                }

                if (creditCardAmount > 0)
                {
                    message += (" " + MessageContainerList.GetMessage(executionContext, "Your credit card has not been charged."));
                }

                if (gameCardAmount > 0)
                {
                    message += (" " + MessageContainerList.GetMessage(executionContext, "Your Game card has not been debited."));
                }

                message += (" " + MessageContainerList.GetMessage(executionContext, "Please contact attendant with the receipt printed.") + " [" + errorMessage + "]");
                KioskStatic.logToFile(message);

                SendKioskPopupAlerts(message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected Error while Generating Transaction Abort Receipt: " + ex.Message);
                string actionType = MessageContainerList.GetMessage(executionContext, "Generating Transaction Abort Receipt");
                string msg = MessageContainerList.GetMessage(executionContext, 5078, actionType, ex.Message); //Unexpected error occured while  &1. Error : &2
                SendKioskPopupAlerts(msg);
            }
            log.LogMethodExit();
        }
        private void GenerateAbortPrint(KioskStatic.receipt_format recepitFormat, decimal totalAmount, decimal cashAmount, decimal gameCardAmount, decimal creditCardAmount)
        {
            log.LogMethodEntry(totalAmount, cashAmount, gameCardAmount, creditCardAmount);
            KioskStatic.logToFile("In GenerateAbortPrint: " + RececiptDeliveryMode.ToString());
            if (RececiptDeliveryMode == KioskReceiptDeliveryMode.PRINT || RececiptDeliveryMode == KioskReceiptDeliveryMode.NONE)
            {
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
                    printString.Add(recepitFormat.a21.Replace("@POS", executionContext.POSMachineName));
                }

                printString.Add(Environment.NewLine);
                printString.Add("*******************");
                printString.Add(MessageContainerList.GetMessage(executionContext, 439)); //"TRANSACTION ABORTED";
                printString.Add("*******************");
                printString.Add(Environment.NewLine);

                printString.Add(MessageContainerList.GetMessage(executionContext, 440, totalAmount.ToString(currencySymbol)));
                printString.Add(Environment.NewLine);

                printString.Add("Cash: " + cashAmount.ToString(currencySymbol));
                printString.Add("Credit Card: " + creditCardAmount.ToString(currencySymbol));
                printString.Add(Environment.NewLine);
                printString.Add(MessageContainerList.GetMessage(executionContext, 441));
                pd.PrintPage += (sender, e) => PrintUSB(e, printString);
                pd = SetReceiptPrinter(pd);
                pd.Print();

            }
            else if (RececiptDeliveryMode == KioskReceiptDeliveryMode.EMAIL)
            {
                bool emailed = EmailTransactionAbortReceipt();
                if (emailed == true)
                {
                    string actionType = MessageContainerList.GetMessage(executionContext, "Transaction");
                    string msg = MessageContainerList.GetMessage(executionContext, 5079, actionType);  //&1 abort receipt is emailed to you
                    KioskStatic.logToFile(msg);
                    SendKioskPopupAlerts(msg);
                }
                else
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 5076); //Send Email Failed
                    KioskStatic.logToFile(msg);
                    SendKioskPopupAlerts(msg);
                }
            }
            log.LogMethodExit();
        }
        private void GenerateRedeemTokenAbortReceipt(KioskStatic.receipt_format rc)
        {
            log.LogMethodEntry(rc);
            try
            {
                KioskStatic.logToFile("In GenerateRedeemTokenAbortReceipt()");
                if (RececiptDeliveryMode == KioskReceiptDeliveryMode.PRINT || RececiptDeliveryMode == KioskReceiptDeliveryMode.NONE)
                {
                    System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                    List<string> printString = new List<string>();

                    printString.Add(rc.head);
                    if (!string.IsNullOrEmpty(rc.a1))
                    {
                        printString.Add(rc.a1.Replace("@Date", DateTime.Now.ToString("ddd, dd-MMM-yyyy h:mm tt")));
                    }

                    if (!string.IsNullOrEmpty(rc.a21))
                    {
                        printString.Add(rc.a21.Replace("@POS", executionContext.POSMachineName));
                    }

                    printString.Add(Environment.NewLine);
                    printString.Add(Environment.NewLine);
                    printString.Add("*******************");
                    printString.Add(Environment.NewLine);
                    printString.Add(MessageContainerList.GetMessage(executionContext, 439)); //"TRANSACTION ABORTED";
                    printString.Add(Environment.NewLine);
                    printString.Add("*******************");
                    printString.Add("*******************");
                    printString.Add(Environment.NewLine);
                    decimal amount = GetTotalPaymentsReceived();
                    printString.Add(MessageContainerList.GetMessage(executionContext, 440, amount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL)));
                    printString.Add(Environment.NewLine);
                    printString.Add(MessageContainerList.GetMessage(executionContext, 441));
                    pd.PrintPage += (sender, e) => PrintUSB(e, printString);
                    pd = SetReceiptPrinter(pd);
                    pd.Print();
                }
                else if (RececiptDeliveryMode == KioskReceiptDeliveryMode.EMAIL)
                {
                    bool emailed = EmailRedeemTokenAbortReceipt();
                    if (emailed == true)
                    {
                        string actionType = MessageContainerList.GetMessage(executionContext, "Redeem token");
                        string msg = MessageContainerList.GetMessage(executionContext, 5079, actionType);  //&1 abort receipt is emailed to you
                        KioskStatic.logToFile(msg);
                        SendKioskPopupAlerts(msg);
                    }
                    else
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, 5076); //Send Email Failed
                        KioskStatic.logToFile(msg);
                        SendKioskPopupAlerts(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected Error while Generating Redeem Token Abort Receipt: " + ex.Message);
                string actionType = MessageContainerList.GetMessage(executionContext, "Generating Redeem Token Abort Receipt");
                string msg = MessageContainerList.GetMessage(executionContext, 5078, actionType, ex.Message); //Unexpected error occured while &1. Error : &2
                SendKioskPopupAlerts(msg);
            }
            log.LogMethodExit();
        }

        private static System.Drawing.Printing.PrintDocument SetReceiptPrinter(System.Drawing.Printing.PrintDocument pd)
        {
            log.LogMethodEntry(pd);
            List<POSPrinterDTO> posPrinterDTOList = new List<POSPrinterDTO>();
            if (KioskStatic.POSMachineDTO.PosPrinterDtoList != null)
            {
                for (int i = 0; i < KioskStatic.POSMachineDTO.PosPrinterDtoList.Count; i++)
                {
                    POSPrinterDTO posPrinterDTO = KioskStatic.POSMachineDTO.PosPrinterDtoList[i];
                    if (posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter)
                    {
                        posPrinterDTOList.Add(posPrinterDTO);
                        break;
                    }
                }
            }
            if (posPrinterDTOList != null && posPrinterDTOList.Any() && posPrinterDTOList[0].PrinterDTO != null
                && posPrinterDTOList[0].PrinterDTO.PrinterName != "Default")
            {
                pd.PrinterSettings.PrinterName = String.IsNullOrEmpty(posPrinterDTOList[0].PrinterDTO.PrinterLocation)
                                                   ? posPrinterDTOList[0].PrinterDTO.PrinterName : posPrinterDTOList[0].PrinterDTO.PrinterLocation;
            }
            log.LogMethodExit(pd.PrinterSettings.PrinterName);
            return pd;
        }

        private void SetShowCardInKioskFlag()
        {
            log.LogMethodEntry();
            try
            {
                this.showCartInKiosk = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SHOW_CART_IN_KIOSK", false);
            }
            catch (Exception ex)
            {
                this.showCartInKiosk = false;
                log.Error("Unable to fetch SHOW_CART_IN_KIOSK details", ex);
            }
            log.LogMethodExit();
        }
        private void CreateNewCardTrxLinesForMultipleCardsInSingleProduct(ProductsContainerDTO containerDTO, int quantity, double? overidePrice)
        {
            log.LogMethodEntry(containerDTO.ProductId, quantity, overidePrice);
            KioskStatic.logToFile("In CreateNewCardTrxLinesForMultipleCardsInSingleProduct()");
            int i = 0;
            Semnox.Parafait.Transaction.Transaction.TransactionLine parentLine = new Semnox.Parafait.Transaction.Transaction.TransactionLine();
            SubscriptionHeaderDTO subscriptionHeaderDTO = null;
            while (quantity > 0)
            {
                string finalCardNumber = GetNewCardNumber(containerDTO);
                Transaction.Card card = new Transaction.Card(finalCardNumber, executionContext.GetUserId(), utilities);
                int finalProductId = containerDTO.ProductId;
                double finalPrice = -1;
                int returnValue = 0;
                string msg = string.Empty;
                if (i == 0)
                {
                    finalPrice = (overidePrice == null ? -1 : (double)overidePrice);
                    i++;
                    returnValue = kioskTrx.createTransactionLine(card, containerDTO.ProductId, finalPrice, 1, ref msg, parentLine, true, null, -1, null, subscriptionHeaderDTO);
                }
                else
                {
                    if (card.CardStatus == "NEW")
                    {
                        finalPrice = (double)containerDTO.FaceValue;
                        finalProductId = GetCardDepositProductId(executionContext);
                    }
                    else
                    {
                        finalPrice = 0;
                        finalProductId = GetKioskVariableTopupProductId();
                    }
                    returnValue = kioskTrx.createTransactionLine(card, finalProductId, finalPrice, 1, parentLine, ref msg);
                }
                if (returnValue != 0)
                {
                    ValidationException validationException = new ValidationException(msg);
                    log.Error("Error in createTransactionLine", validationException);
                    throw validationException;
                }
                quantity--;
            }
            log.LogMethodExit();
        }
        private void CreateNewCardTrxLines(ProductsContainerDTO containerDTO, int quantity, double? overidePrice)
        {
            log.LogMethodEntry(containerDTO.ProductId, quantity, overidePrice);
            while (quantity > 0)
            {
                string finalCardNumber = GetNewCardNumber(containerDTO);
                Transaction.Card card = new Transaction.Card(finalCardNumber, executionContext.GetUserId(), utilities);
                string msg = string.Empty;
                double finalPrice = (overidePrice == null ? -1 : (double)overidePrice);
                int returnValue = kioskTrx.createTransactionLine(card, containerDTO.ProductId, finalPrice, 1, ref msg);
                if (returnValue != 0)
                {
                    ValidationException validationException = new ValidationException(msg);
                    log.Error("Error in createTransactionLine", validationException);
                    throw validationException;
                }
                quantity--;
            }
            log.LogMethodExit();
        }
        private static int GetCardDepositProductId(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            int finalProductId;
            if (KioskStatic.Utilities.ParafaitEnv.CardDepositProductId < 0)
            {
                string errMsg = MessageContainerList.GetMessage(executionContext, 5031, "Card Deposit Product");
                //CardDepositProduct not defined. Contact staff
                ValidationException validationException = new ValidationException(errMsg);
                log.Error("Error in GetCardDepositProductId", validationException);
                throw validationException;
            }
            else
            {
                finalProductId = KioskStatic.Utilities.ParafaitEnv.CardDepositProductId;
            }
            log.LogMethodExit(finalProductId);
            return finalProductId;
        }
        private int GetKioskVariableTopupProductId()
        {
            log.LogMethodEntry();
            int finalProductId = -1;
            try
            {
                finalProductId = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "KIOSK_VARIABLE_TOPUP_PRODUCT", -1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string errMsg = MessageContainerList.GetMessage(executionContext, 2646);
                //"Variable product is not set up.Please set up the Product"
                ValidationException validationException = new ValidationException(errMsg);
                log.Error("Error in GetKioskVariableTopupProductId", validationException);
                throw validationException;
            }
            if (finalProductId == -1)
            {
                string errMsg = MessageContainerList.GetMessage(executionContext, 2646);
                //"Variable product is not set up.Please set up the Product"
                ValidationException validationException = new ValidationException(errMsg);
                log.Error("Error in GetKioskVariableTopupProductId", validationException);
                throw validationException;
            }
            log.LogMethodExit(finalProductId);
            return finalProductId;
        }
        private string GetNewCardNumber(ProductsContainerDTO containerDTO)
        {
            log.LogMethodEntry();
            bool newCardNumber = false;
            string cardNumber = string.Empty;
            while (newCardNumber == false)
            {
                cardNumber = (containerDTO.AutoGenerateCardNumber == "Y" ? KioskStatic.GetTagNumber() : KioskStatic.GetTempTagNumber());
                AccountListBL accountBL = new AccountListBL(executionContext);
                List<KeyValuePair<AccountDTO.SearchByParameters, string>> accountSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                accountSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.TAG_NUMBER, newCardNumber.ToString()));
                List<AccountDTO> accountListDTO = accountBL.GetAccountDTOList(accountSearchParameters, false, true);
                if (accountListDTO == null || accountListDTO.Count == 0)
                {
                    newCardNumber = true;
                }
            }
            log.LogMethodExit(cardNumber);
            return cardNumber;
        }
        private void CreateVariableCardTrxLines(ProductsContainerDTO containerDTO, double variableAmount, string selectedEntitlementType, Card rechargedCard)
        {
            log.LogMethodEntry(containerDTO.ProductId, variableAmount, selectedEntitlementType, rechargedCard);
            string message = string.Empty;
            string finalCardNumber = string.Empty;
            Transaction.Card card = null;
            if (rechargedCard == null)
            {
                finalCardNumber = GetNewCardNumber(containerDTO);
                card = new Transaction.Card(finalCardNumber, executionContext.GetUserId(), utilities);
                CreateVariableCardTrxLinesForNewCard(containerDTO, variableAmount, selectedEntitlementType, card);
            }
            else
            {
                card = rechargedCard;
                CreateVariableCardTrxLinesForRechargeCard(containerDTO, variableAmount, selectedEntitlementType, card, null);
            }

            log.LogMethodExit();
        }
        private void CreateVariableCardTrxLinesForNewCard(ProductsContainerDTO containerDTO, double variableAmount, string selectedEntitlementType, Card newCard)
        {
            log.LogMethodEntry("containerDTO", variableAmount, selectedEntitlementType, newCard);
            KioskStatic.logToFile("In CreateVariableCardTrxLinesForNewCard: " + variableAmount.ToString());
            string productType = ProductTypeValues.NEW;
            if (selectedEntitlementType == TIME)
            {
                productType = ProductTypeValues.GAMETIME;
            }
            double productPrice = 0;
            ProductsList productsList = new ProductsList(executionContext);
            List<ProductsDTO> productsDTOList = productsList.getSplitProductList(variableAmount, productType);
            string convertToTimeGuid = string.Empty;
            if (productsDTOList != null && productsDTOList.Count > 0)
            {
                int prodId = productsDTOList[0].ProductId;
                int customDataSetId = productsDTOList[0].CustomDataSetId;
                int externalRef = GetExternalRefFromCustomAttributes(customDataSetId);
                string message = "";
                productPrice = Convert.ToDouble(productsDTOList[0].Price);
                Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine = new Transaction.Transaction.TransactionLine();
                if (kioskTrx.createTransactionLine(newCard, prodId, -1, 1, ref message, trxLine) != 0)
                {
                    ValidationException validationException = new ValidationException(message);
                    log.Error("Error in CreateVariableCardTrxLinesForNewCard (1) ", validationException);
                    throw validationException;
                }
                int timeInMinPerCredit = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "TIME_IN_MINUTES_PER_CREDIT", 0);
                if (timeInMinPerCredit > 0 && selectedEntitlementType == TIME)
                {
                    trxLine.MultiPointConversionRequired = true;
                    convertToTimeGuid = Convert.ToString(Guid.NewGuid());
                    trxLine.ConvertToTimeGuid = convertToTimeGuid;
                }
                KioskStatic.logToFile("Created split product. Ext Ref: " + externalRef.ToString() + ". Parafait product: " + productsDTOList[0].ProductName);
                // multiPointConversionRequired = true;
            }
            else
            {
                int cardDepositProductId = GetCardDepositProductId(executionContext);
                float cardFaceValue = ParafaitDefaultContainerList.GetParafaitDefault<float>(executionContext, "CARD_FACE_VALUE", 0);
                string message = "";
                if (kioskTrx.createTransactionLine(newCard, cardDepositProductId, cardFaceValue, 1, ref message) != 0)
                {
                    ValidationException validationException = new ValidationException(message);
                    log.Error("Error in CreateVariableCardTrxLinesForNewCard (2) ", validationException);
                    throw validationException;
                }
                KioskStatic.logToFile("Created card desposit product with card face value amount");
                productPrice = cardFaceValue;
            }
            if ((variableAmount - productPrice) > 0)
            {
                CreateVariableCardTrxLinesForRechargeCard(containerDTO, (variableAmount - productPrice), selectedEntitlementType, newCard, convertToTimeGuid);
            }
            log.LogMethodExit();
        }
        private int GetExternalRefFromCustomAttributes(int customDataSetId)
        {
            log.LogMethodEntry(customDataSetId);
            int externalRef = -1;
            CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext);
            List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.NAME, EXTERNAL_SYS_IDENTIFIER));
            List<CustomAttributesDTO> customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParameters);
            if (customAttributesDTOList != null)
            {
                CustomDataListBL customDataListBL = new CustomDataListBL(executionContext);
                List<KeyValuePair<CustomDataDTO.SearchByParameters, string>> searchCustomDataParameters = new List<KeyValuePair<CustomDataDTO.SearchByParameters, string>>();
                searchCustomDataParameters.Add(new KeyValuePair<CustomDataDTO.SearchByParameters, string>(CustomDataDTO.SearchByParameters.CUSTOM_DATA_SET_ID, customDataSetId.ToString()));
                searchCustomDataParameters.Add(new KeyValuePair<CustomDataDTO.SearchByParameters, string>(CustomDataDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID, customAttributesDTOList[0].CustomAttributeId.ToString()));
                List<CustomDataDTO> customDataDTOList = customDataListBL.GetCustomDataDTOList(searchCustomDataParameters);
                if (customDataDTOList != null)
                {
                    if (customDataDTOList[0].CustomDataNumber != null)
                        externalRef = Convert.ToInt32(customDataDTOList[0].CustomDataNumber);
                }
            }
            log.LogMethodExit(externalRef);
            return externalRef;
        }
        private void CreateVariableCardTrxLinesForRechargeCard(ProductsContainerDTO containerDTO, double amount, string selectedEntitlementType, Card rechargeCard,
            string convertToTimeGuid)
        {
            log.LogMethodEntry("containerDTO", amount, selectedEntitlementType, rechargeCard, convertToTimeGuid);
            KioskStatic.logToFile("In CreateVariableCardTrxLinesForRechargeCard: " + amount.ToString());
            try
            {
                string message = string.Empty;
                int returnValue = 0;
                bool isSplitAndMapProduct = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SPLIT_AND_MAP_VARIABLE_PRODUCT", false);
                if (isSplitAndMapProduct == false)
                {
                    returnValue = kioskTrx.createTransactionLine(rechargeCard, containerDTO.ProductId, amount, 1, ref message);
                }
                else
                {
                    while (amount > 0)
                    {
                        string productType = ProductTypeValues.RECHARGE;
                        if (selectedEntitlementType == TIME)
                        {
                            productType = ProductTypeValues.GAMETIME;
                        }

                        ProductsList productsList = new ProductsList(executionContext);
                        List<ProductsDTO> productsDTOList = productsList.getSplitProductList(amount, productType);
                        if (productsDTOList != null && productsDTOList.Count > 0)
                        {
                            int prodId = productsDTOList[0].ProductId;
                            int customDataSetId = productsDTOList[0].CustomDataSetId;
                            int externalRef = GetExternalRefFromCustomAttributes(customDataSetId);
                            double price = Convert.ToDouble(productsDTOList[0].Price);
                            Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine = new Transaction.Transaction.TransactionLine();
                            if (kioskTrx.createTransactionLine(rechargeCard, prodId, price, 1, ref message, trxLine) != 0)
                            {
                                ValidationException validationException = new ValidationException(message);
                                log.Error("Error in CreateVariableCardTrxLinesForRechargeCard (1) ", validationException);
                                throw validationException;
                            }
                            amount -= price;
                            KioskStatic.logToFile("Created split product. Ext Ref: " + externalRef.ToString() + ". Parafait product: " + productsDTOList[0].ProductName);
                            //multiPointConversionRequired = true; 
                            int timeInMinPerCredit = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "TIME_IN_MINUTES_PER_CREDIT", 0);
                            if (timeInMinPerCredit > 0 && selectedEntitlementType == TIME)
                            {
                                trxLine.MultiPointConversionRequired = true;
                                if (string.IsNullOrWhiteSpace(convertToTimeGuid))
                                {
                                    convertToTimeGuid = Convert.ToString(Guid.NewGuid());
                                }
                                trxLine.ConvertToTimeGuid = convertToTimeGuid;
                            }
                        }
                        else
                        {
                            object oExternalRef = KioskStatic.getProductExternalSystemReference(containerDTO.ProductId);
                            if (oExternalRef == DBNull.Value)
                            {
                                KioskStatic.logToFile("External System Reference for Variable product not found");
                            }
                            if (kioskTrx.createTransactionLine(rechargeCard, containerDTO.ProductId, amount, 1, ref message) != 0)
                            {
                                ValidationException validationException = new ValidationException(message);
                                log.Error("Error in CreateVariableCardTrxLinesForRechargeCard (2) ", validationException);
                                throw validationException;
                            }
                            KioskStatic.logToFile("Created split product. Ext Ref: " + (oExternalRef != null ? oExternalRef.ToString() : "")
                                                      + ". Parafait variable product (" + amount.ToString() + ")");
                            amount = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing CreateVariableCardTrxLinesForRechargeCard (3)", ex);
                KioskStatic.logToFile("Error in CreateVariableCardTrxLinesForRechargeCard: " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        private void ProcessOverpaidAmount()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In ProcessOverpaidAmount()");
            //kioskTrx.PaymentCreditCardSurchargeAmount = 0.1;
            if (kioskTrx.TransactionPaymentsDTOList != null && kioskTrx.TransactionPaymentsDTOList.Any())
            {
                decimal amountReceived = GetTotalPaymentsReceived();
                //double ccSurchargeAmount = GetCCSurchargeAmount; 
                if (amountReceived > (decimal)kioskTrx.Net_Transaction_Amount)
                {
                    KioskStatic.logToFile("In ProcessOverpaidAmount, Received: " + amountReceived.ToString());
                    KioskStatic.logToFile("In ProcessOverpaidAmount, Net: " + kioskTrx.Net_Transaction_Amount.ToString());
                    decimal overPaidAmount = (amountReceived - (decimal)kioskTrx.Net_Transaction_Amount);
                    KioskStatic.logToFile("In ProcessOverpaidAmount, Diff: " + overPaidAmount);
                    Card rechargeCard = null;
                    rechargeCard = GetACardFromTrx();
                    int variableProductId = GetKioskVariableTopupProductId();
                    ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext, variableProductId);
                    if (rechargeCard != null)
                    {
                        CreateVariableCardTrxLinesForRechargeCard(productsContainerDTO, (double)overPaidAmount, CREDITS, rechargeCard, string.Empty);
                        SaveTransaction();
                    }
                    else
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 5072, kioskTrx.Net_Transaction_Amount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL), amountReceived.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL), overPaidAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                        ValidationException validationException = new ValidationException(message);
                        log.Error("Error in ProcessOverpaidAmount ", validationException);
                        throw validationException;
                    }

                }
            }
            log.LogMethodExit();
        }
        private Card GetACardFromTrx()
        {
            log.LogMethodEntry();
            Card rechargeCard = null;
            if (kioskTrx.PrimaryCard != null)
            {
                rechargeCard = kioskTrx.PrimaryCard;
            }
            else
            {
                for (int i = 0; i < kioskTrx.TrxLines.Count; i++)
                {
                    if (kioskTrx.TrxLines[i].card != null && string.IsNullOrWhiteSpace(kioskTrx.TrxLines[i].CardNumber) == false)
                    {
                        rechargeCard = kioskTrx.TrxLines[i].card;
                        break;
                    }
                }
            }
            log.LogMethodExit();
            return rechargeCard;
        }
        private void AddRoundOffAmount()
        {
            log.LogMethodEntry();
            if (kioskTrx.TransactionPaymentsDTOList != null && kioskTrx.TransactionPaymentsDTOList.Any())
            {
                KioskStatic.logToFile("In AddRoundOffAmount()");
                //double ccSurchargeAmount = GetCCSurchargeAmount;
                decimal amountReceived = GetTotalPaymentsReceived();
                decimal balance = Math.Abs((decimal)kioskTrx.Net_Transaction_Amount - amountReceived);
                log.Info("balance: " + balance.ToString());
                if (balance > 0 && balance < 1)
                {
                    KioskStatic.logToFile("In AddRoundOffAmount: " + balance.ToString());
                    PaymentModeDTO cashPaymentMode = GetCashPaymentMode();
                    if (cashPaymentMode != null)
                    {
                        decimal balanceAmountValue = balance;
                        log.Info("balanceAmountValue: " + balanceAmountValue.ToString());
                        TransactionPaymentsDTO trxBalanceCashPaymentDTO = new TransactionPaymentsDTO(-1, -1, cashPaymentMode.PaymentModeId,
                                                                              (double)balanceAmountValue,
                                                                              "", "", "", "", "", -1, "", -1, -1, "", "", false, -1, -1, "", utilities.getServerTime(),
                                                                               utilities.ParafaitEnv.LoginID, -1, null, 0, -1, utilities.ParafaitEnv.POSMachine, -1, "", null);
                        trxBalanceCashPaymentDTO.paymentModeDTO = cashPaymentMode;
                        //trxBalanceCashPaymentDTO.TransactionId = CurrentTrx.Trx_id;
                        kioskTrx.TransactionPaymentsDTOList.Add(trxBalanceCashPaymentDTO);
                        string message = string.Empty;
                        bool succ = kioskTrx.CreatePaymentInfo(null, ref message);
                        if (succ == false)
                        {
                            ValidationException validationException = new ValidationException(message);
                            log.Error("Error in AddRoundOffAmount", validationException);
                            throw validationException;
                        }
                    }
                }
                else if (balance > 1)
                {
                    KioskStatic.logToFile("In AddRoundOffAmount: " + balance.ToString());
                    string message = MessageContainerList.GetMessage(executionContext, "Partial payment not allowed");
                    ValidationException validationException = new ValidationException(message);
                    log.Error("Error in AddRoundOffAmount", validationException);
                    throw validationException;
                }
            }
            else
            {
                if (kioskTrx.Net_Transaction_Amount > 0)
                {
                    string message = MessageContainerList.GetMessage(executionContext, 238);//Enter Payment Details before saving
                    ValidationException validationException = new ValidationException(message);
                    log.Error("Error in AddRoundOffAmount", validationException);
                    throw validationException;
                }

            }
            log.LogMethodExit();
        }
        private void MarkTransactionAsComplete()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In MarkTransactionAsComplete()");
            //TransactionIsNotNull();
            string msg = string.Empty;
            int retValue = kioskTrx.SaveTransacation(ref msg);
            if (retValue != 0)
            {
                ValidationException validationException = new ValidationException(msg);
                log.Error("Error in SaveTransacation", validationException);
                throw validationException;
            }
            log.LogMethodExit();
        }
        private void UpdateLoyaltyCardNumber()
        {
            log.LogMethodEntry();
            try
            {
                kioskTrx.UpdateLoyaltyCardNumber(this.loyaltyCardNumber, null);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in UpdateLoyaltyCardNumber: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private int GetTrxId()
        {
            log.LogMethodEntry();
            int trxId = (kioskTrx != null && kioskTrx.Trx_id > 0 ? kioskTrx.Trx_id : -1);
            log.LogMethodExit(trxId);
            return trxId;
        }
        private string GetTrxStatus()
        {
            log.LogMethodEntry();
            string trxStatus = (kioskTrx != null ? kioskTrx.Status.ToString() : string.Empty);
            log.LogMethodExit(trxStatus);
            return trxStatus;
        }
        private List<Transaction.Transaction.TransactionLine> GetActiveTrxLines()
        {
            log.LogMethodEntry();
            List<Transaction.Transaction.TransactionLine> transactionLineList = new List<Transaction.Transaction.TransactionLine>();
            if (kioskTrx != null && kioskTrx.TrxLines != null
                && kioskTrx.TrxLines.Any() && kioskTrx.TrxLines.Exists(tl => tl.LineValid))
            {
                transactionLineList = kioskTrx.TrxLines.Where(tl => tl.LineValid).ToList();
            }
            log.LogMethodExit(transactionLineList);
            return transactionLineList;

        }
        private List<DiscountsSummaryDTO> GetTrxDiscountSummary()
        {
            log.LogMethodEntry();
            List<DiscountsSummaryDTO> trxDiscountSummary = new List<DiscountsSummaryDTO>();
            if (kioskTrx != null && kioskTrx.DiscountsSummaryDTOList != null && kioskTrx.DiscountsSummaryDTOList.Any())
            {
                trxDiscountSummary = kioskTrx.DiscountsSummaryDTOList;
            }
            log.LogMethodExit(trxDiscountSummary);
            return trxDiscountSummary;

        }
        private List<Transaction.Transaction.TransactionLine> GetCardDepositTrxLines()
        {
            log.LogMethodEntry();
            List<Transaction.Transaction.TransactionLine> transactionLineList = new List<Transaction.Transaction.TransactionLine>();
            if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any()
                     && kioskTrx.TrxLines.Exists(tl => tl.LineValid && tl.ProductTypeCode == ProductTypeValues.CARDDEPOSIT))
            {
                transactionLineList = kioskTrx.TrxLines.Where(tl => tl.LineValid && tl.ProductTypeCode == ProductTypeValues.CARDDEPOSIT).ToList();
            }
            log.LogMethodExit(transactionLineList);
            return transactionLineList;
        }
        private Transaction.Transaction.TransactionLine GetFirstNonDepositAndFnDTrxLine()
        {
            log.LogMethodEntry();
            Transaction.Transaction.TransactionLine firstNonDepositTransactionLine = null;
            List<Transaction.Transaction.TransactionLine> transactionLineList = new List<Transaction.Transaction.TransactionLine>();
            if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                for (int i = 0; i < kioskTrx.TrxLines.Count; i++)
                {
                    Transaction.Transaction.TransactionLine transactionLine = kioskTrx.TrxLines[i];
                    if (transactionLine.LineValid && transactionLine.ProductTypeCode != ProductTypeValues.CARDDEPOSIT
                        && (NotAFundRaiserOorDonationProduct(transactionLine.ProductID) == true))
                    {
                        firstNonDepositTransactionLine = kioskTrx.TrxLines[i];
                        break;
                    }
                }
            }
            log.LogMethodExit(firstNonDepositTransactionLine);
            return firstNonDepositTransactionLine;
        }
        private List<TransactionPaymentsDTO> GetTrxTransactionPaymentsDTOList()
        {
            log.LogMethodEntry();
            List<TransactionPaymentsDTO> trxPaymentDTOList = null;
            if (kioskTrx != null && kioskTrx.TransactionPaymentsDTOList != null && kioskTrx.TransactionPaymentsDTOList.Any())
            {
                trxPaymentDTOList = new List<TransactionPaymentsDTO>(kioskTrx.TransactionPaymentsDTOList);
            }
            log.LogMethodExit(trxPaymentDTOList);
            return trxPaymentDTOList;
        }
        private Card GetTrxPrimaryCard()
        {
            log.LogMethodEntry();
            Card primaryCard = null;
            if (kioskTrx != null)
            {
                if (kioskTrx.PrimaryCard != null)
                {
                    primaryCard = kioskTrx.PrimaryCard;
                }
                else if (kioskTrx.TrxLines.Exists(tl => tl.LineValid && tl.card != null))
                {
                    primaryCard = kioskTrx.TrxLines.Find(tl => tl.LineValid && tl.card != null).card;
                }
            }
            log.LogMethodExit(primaryCard);
            return primaryCard;
        }
        private string GetTrxNumber()
        {
            log.LogMethodEntry();
            string trxNumber = string.Empty;
            if (kioskTrx != null && kioskTrx.Trx_id > 0)
            {
                trxNumber = kioskTrx.Trx_No;
            }
            log.LogMethodExit(trxNumber);
            return trxNumber;
        }
        private int GetTrxCartItemCount()
        {
            log.LogMethodEntry();
            int itemCount = 0;
            if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                //consider individual products and combo parents only
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> prodLines
                      = kioskTrx.TrxLines.Where(tl => tl.LineValid
                                              && tl.ProductTypeCode != ProductTypeValues.CARDDEPOSIT
                                              && tl.ProductTypeCode != ProductTypeValues.DEPOSIT
                                              && tl.ProductTypeCode != ProductTypeValues.LOCKERDEPOSIT
                                              && tl.ProductTypeCode != ProductTypeValues.LOYALTY
                                              && tl.ProductTypeCode != ProductTypeValues.SERVICECHARGE
                                              && tl.ProductTypeCode != ProductTypeValues.GRATUITY
                                              && tl.ParentLine == null).ToList();
                //for split and map products
                for (int i = 0; i < kioskTrx.TrxLines.Count; i++)
                {
                    if (!string.IsNullOrEmpty(kioskTrx.TrxLines[i].CardNumber) && kioskTrx.TrxLines[i].LineValid)
                    {
                        if (kioskTrx.TrxLines[i].ParentLine != null && kioskTrx.TrxLines[i].ParentLine.card != null
                            && (kioskTrx.TrxLines[i].ProductTypeCode == ProductTypeValues.CARDDEPOSIT
                                 || kioskTrx.TrxLines[i].ProductTypeCode == ProductTypeValues.VARIABLECARD))
                        {

                            if (prodLines.Exists(tl => tl == kioskTrx.TrxLines[i]) == false)
                                prodLines.Add(kioskTrx.TrxLines[i]);
                        }
                    }
                }
                if (prodLines == null || prodLines.Any() == false)
                { //only deposit line scenario
                    prodLines = kioskTrx.TrxLines.Where(tl => tl.LineValid
                                              && tl.ProductTypeCode != ProductTypeValues.LOYALTY
                                              && tl.ProductTypeCode != ProductTypeValues.SERVICECHARGE
                                              && tl.ProductTypeCode != ProductTypeValues.GRATUITY
                                              && tl.ParentLine == null).ToList();
                }
                if (prodLines != null)
                {
                    itemCount = prodLines.Count;
                }
            }
            log.LogMethodExit(itemCount);
            return itemCount;
        }
        private decimal GetTrxCCSurchargeAmount()
        {
            log.LogMethodEntry();
            decimal ccSurchargeAmount = 0;
            if (kioskTrx != null)
            {
                ccSurchargeAmount = Convert.ToDecimal(kioskTrx.GetCCSurchargeAmount(null));
            }
            log.LogMethodExit(ccSurchargeAmount);
            return ccSurchargeAmount;
        }
        private void AddCCSurchargeAmount(double ccSurchargeAmount)
        {
            log.LogMethodEntry();
            if (ccSurchargeAmount > 0 && kioskTrx != null)
            {
                kioskTrx.PaymentCreditCardSurchargeAmount = kioskTrx.PaymentCreditCardSurchargeAmount + ccSurchargeAmount;
            }
            log.LogMethodExit();
        }
        private void SendCreditCardTrxInfoToVCATDevice(PaymentModeDTO paymentModeDTO, TransactionPaymentsDTO trxPaymentDTO, double amountToPay, double ccSurchargeAmount)
        {
            log.LogMethodEntry("paymentModeDTO", trxPaymentDTO, amountToPay, ccSurchargeAmount);
            KioskStatic.logToFile("In SendCreditCardTrxInfoToVCATDevice()");

            if (paymentModeDTO.Gateway > -1)
            {
                string msg = MessageContainerList.GetMessage(executionContext, "Payment Gateway unexpectedly Set. Please contact Manager.");
                SendKioskPopupAlerts(msg);
                KioskStatic.logToFile("Error: Payment Gateway is not expected for VCAT.");
                log.Error(msg);
                return;
            }
            //createDummyTransaction(); // Create dummy transaction as trx gets created in later stage
            FiscalPrinterFactory.GetInstance().Initialize(utilities);
            bool unAttendedMode = true;
            FiscalPrinter fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER"), unAttendedMode);
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
            if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                transactionLine.VATRate = Convert.ToDecimal(kioskTrx.TrxLines.First().tax_percentage);
                log.Debug("VATRate :" + transactionLine.VATRate);
                if (transactionLine.VATRate > 0)
                {
                    transactionLine.VATAmount = (Convert.ToDecimal(trxPaymentDTO.Amount) * transactionLine.VATRate) / (100 + transactionLine.VATRate);
                    log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                    if (transactionLine.VATAmount % 1 > 0)
                    {
                        transactionLine.VATAmount = (decimal)(new Semnox.Core.GenericUtilities.CommonFuncs(utilities)).RoundOff(Convert.ToDouble(transactionLine.VATAmount), utilities.ParafaitEnv.RoundOffAmountTo, utilities.ParafaitEnv.RoundingPrecision, utilities.ParafaitEnv.RoundingType);
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
                    string msg = MessageContainerList.GetMessage(executionContext, "Credit card Payment Failed.");
                    SendKioskPopupAlerts(msg);
                    KioskStatic.logToFile(msg);
                    return;
                }
                ProcessReceivedMoney(trxPaymentDTO);
                //ac.totalValue += (decimal)trxPaymentDTO.Amount;
                //if (KioskStatic.debugMode) MessageBox.Show(MessageContainerList.GetMessage(executionContext, "Credit card payment success"));
                KioskStatic.logToFile("Credit card payment success");
                //Added check to see if Partial approval is not allowed and if credit card is partially approved then abort and exit
                //current transaction
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_PARTIAL_APPROVAL", false) == false
                                 && (amountToPay + ccSurchargeAmount) > trxPaymentDTO.Amount)
                {
                    PerformAbortTrxAction();
                    SendkioskAbortTransactionEvent();
                    log.LogMethodExit();
                    return;
                }
                else if ((amountToPay + ccSurchargeAmount) > trxPaymentDTO.Amount)
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 2854, trxPaymentDTO.Amount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                    SendKioskPopupAlerts(msg);
                    KioskStatic.logToFile(msg);
                }
            }
            else
            {
                log.Error("Credit card Payment Failed");
                SendKioskProgressUpdates(MessageContainerList.GetMessage(executionContext, "Credit card Payment Failed."));
                KioskStatic.logToFile("Credit card Payment Failed.");
                log.LogMethodExit();
                return;
            }
            log.LogMethodExit();
        }
        private void ReverseFiscalization(TransactionPaymentsDTO trxPaymentDTO)
        {
            log.LogMethodEntry(trxPaymentDTO);
            KioskStatic.logToFile("In ReverseFiscalization()");
            log.Debug("FiscalPrinters.SmartroKorea  Reversal");
            FiscalPrinterFactory.GetInstance().Initialize(utilities);
            bool unAttendedMode = true;
            string fiscalPrinterName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER");
            FiscalPrinter fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(fiscalPrinterName, unAttendedMode);
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
            if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                transactionLine.VATRate = Convert.ToDecimal(kioskTrx.TrxLines.First().tax_percentage);
                log.Debug("VATRate :" + transactionLine.VATRate);
                if (transactionLine.VATRate > 0)
                {
                    //creditCardAmount is inclusive of tax amount. 
                    transactionLine.VATAmount = (Convert.ToDecimal(paymentInfo.amount) * transactionLine.VATRate) / (100 + transactionLine.VATRate);
                    log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                    if (transactionLine.VATAmount % 1 > 0)
                    {
                        transactionLine.VATAmount = (decimal)(new Semnox.Core.GenericUtilities.CommonFuncs(utilities)).RoundOff(Convert.ToDouble(transactionLine.VATAmount), utilities.ParafaitEnv.RoundOffAmountTo, utilities.ParafaitEnv.RoundingPrecision, utilities.ParafaitEnv.RoundingType);
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
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 4279) + Message);
                }
            }
            else
            {
                log.Error("Payment reversal Failed");
                fiscalPrinter.ClosePort();
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4279) + Message);
            }

            //ac.totalValue -= (decimal)trxPaymentDTO.Amount;
            string msg = MessageContainerList.GetMessage(executionContext, "Credit Card smartro Payment Refunded") + ": " + trxPaymentDTO.Amount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
            SendKioskProgressUpdates(msg);
            KioskStatic.logToFile(msg);
            log.LogMethodExit(msg);
        }


        private void AddCashPaymentEntry(decimal cashValue)
        {
            log.LogMethodEntry(cashValue);
            KioskStatic.logToFile("In AddCashPaymentEntry: " + cashValue.ToString());
            PaymentModeDTO cashPaymentMode = GetCashPaymentMode();
            double tenderedAmount = 0;
            double tipAmount = 0;
            int transactionId = kioskTrx.Trx_id > 0 ? kioskTrx.Trx_id : -1;
            int paymentModeId = (cashPaymentMode != null ? cashPaymentMode.PaymentModeId : -1);
            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(-1, transactionId, paymentModeId,
                            (double)cashValue, null, null, null, null, null, -1, null, -1, -1, null, null, false, executionContext.GetSiteId(),
                            -1, null, ServerDateTime.Now, executionContext.GetUserId(), -1, tenderedAmount, tipAmount, -1, executionContext.POSMachineName,
                            -1, null, null);
            transactionPaymentsDTO.PaymentModeDTO = cashPaymentMode;
            AddToTransactionPaymentDTOList(transactionPaymentsDTO); 
            log.LogMethodExit();
        }

        private void AddToTransactionPaymentDTOList(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            KioskStatic.logToFile("In AddToTransactionPaymentDTOList:");
            if (kioskTrx != null && transactionPaymentsDTO != null)
            {
                KioskStatic.logToFile("AddToTransactionPaymentDTOList: " + (transactionPaymentsDTO.PaymentModeDTO != null ? transactionPaymentsDTO.PaymentModeDTO.PaymentMode : ""));
                try
                {
                    if (kioskTrx.TransactionPaymentsDTOList == null)
                    {
                        kioskTrx.TransactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
                    }
                    kioskTrx.TransactionPaymentsDTOList.Add(transactionPaymentsDTO);
                    if (transactionPaymentsDTO.paymentModeDTO == null)
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, "Unable to fetch payment mode details. ");
                        ValidationException ve = new ValidationException(msg);
                        log.Error(ve);
                        throw ve;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error in AddToTransactionPaymentDTOList: " + ex.Message);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        private PaymentModeDTO GetCashPaymentMode()
        {
            log.LogMethodEntry();
            PaymentModeDTO cashMode = null;
            List<POSPaymentModeInclusionDTO> posPaymentModeInclusionDTOList = KioskStatic.pOSPaymentModeInclusionDTOList;
            if (posPaymentModeInclusionDTOList != null && posPaymentModeInclusionDTOList.Any())
            {
                POSPaymentModeInclusionDTO cashDTO = posPaymentModeInclusionDTOList.Find(pm => pm.PaymentModeDTO != null
                                             && pm.PaymentModeDTO.IsCash && pm.PaymentModeDTO.IsActive);
                if (cashDTO != null)
                {
                    cashMode = cashDTO.PaymentModeDTO;
                }
            }
            else
            {
                PaymentModesContainerDTOCollection dTOCollection = PaymentModesContainerList.GetPaymentModeContainerDTOCollection(executionContext.SiteId);
                if (dTOCollection != null && dTOCollection.PaymentModesContainerDTOList != null
                    && dTOCollection.PaymentModesContainerDTOList.Any())
                {
                    PaymentModesContainerDTO cpDTO = dTOCollection.PaymentModesContainerDTOList.Find(p => p.IsCash);
                    if (cpDTO != null)
                    {
                        PaymentMode pM = new PaymentMode(executionContext, cpDTO.PaymentModeId, null, true);
                        cashMode = pM.GetPaymentModeDTO;
                    }
                }
            }
            log.LogMethodExit(cashMode);
            return cashMode;
        }
        private void IssueNewCards(CardDispenser.CardDispenser cardDispenser)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In IssueNewCards()");
            try
            {
                List<Transaction.Transaction.TransactionLine> tempCardLines = kioskTrx.TrxLines.Where(tl => tl.LineValid
                                                                                && string.IsNullOrWhiteSpace(tl.CardNumber) == false
                                                                                && tl.CardNumber.StartsWith("T")).ToList();
                if (tempCardLines != null && tempCardLines.Any())
                {
                    List<string> tempCardNumberList = tempCardLines.Select(tl => tl.CardNumber).Distinct().ToList();
                    for (int i = 0; i < tempCardNumberList.Count; i++)
                    {
                        string sourceCardNumber = tempCardNumberList[i];
                        KioskStatic.logToFile("IssueNewCard for " + sourceCardNumber);
                        Transaction.Card destinationCard = null;
                        try
                        {
                            destinationCard = GetCardFromDispenser(cardDispenser);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            KioskStatic.logToFile("Card dispenser error: " + ex.Message);
                            this.SendKioskProgressUpdates(ex.Message);
                            string msg = MessageContainerList.GetMessage(executionContext, 441) + ". " + MessageContainerList.GetMessage(executionContext, 460);
                            //Please contact our staff. Problem in Card Dispenser. Cannot issue new card.
                            this.SendKioskPopupAlerts(msg);
                            this.SendKioskProgressUpdates(msg);
                            break;//stop do not proceed with next card
                        }
                        if (destinationCard != null)
                        {
                            try
                            {
                                DoTransferEntitlements(sourceCardNumber, destinationCard);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                RejectCard(cardDispenser, destinationCard, NEWCARDMSG);
                                KioskStatic.logToFile("Transfer Entitlements error: " + ex.Message);
                                this.SendKioskProgressUpdates(ex.Message); //Sorry unexpected error. Please contact our staff
                                string msg = MessageContainerList.GetMessage(executionContext, "Sorry unexpected error")
                                                  + ". " + MessageContainerList.GetMessage(executionContext, 441);
                                this.SendKioskPopupAlerts(msg);
                                this.SendKioskProgressUpdates(msg);
                                break;//stop do not proceed with next card
                            }
                            EjjectCard(cardDispenser, destinationCard, NEWCARDMSG);
                        }
                    }
                    List<Transaction.Transaction.TransactionLine> pendingTempCardLines = kioskTrx.TrxLines.Where(tl => tl.LineValid
                                                                                && string.IsNullOrWhiteSpace(tl.CardNumber) == false
                                                                                && tl.CardNumber.StartsWith("T")).ToList();
                    if (pendingTempCardLines != null && pendingTempCardLines.Any())
                    {
                        GenerateErrorReceipt(CARDDISPENSER);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected Error while Issuing New Cards: " + ex.Message);
                string actionType = MessageContainerList.GetMessage(executionContext, "Issuing New Cards");
                string msg = MessageContainerList.GetMessage(executionContext, 5078, actionType, ex.Message); //Unexpected error occured while &1. Error : &2
                this.SendKioskPopupAlerts(msg);
            }
            KioskStatic.logToFile("Exit IssueNewCards()");
            log.LogMethodExit();
        }
        private void RejectCard(CardDispenser.CardDispenser cardDispenser, Card destinationCard, string msg)
        {
            log.LogMethodEntry("cardDispenser", (destinationCard != null ? destinationCard.CardNumber : "-1"), msg);
            KioskStatic.logToFile("In RejectCard: " + msg);
            try
            {
                string productName = GetProductDetailsFromTrxLine(destinationCard.CardNumber);
                KioskStatic.UpdateKioskActivityLog(executionContext, "NEWCARD", productName + " " + msg + ": Failed", destinationCard.CardNumber, GetTransactionId, globalKioskTrxId);
                string message = string.Empty;
                if (!cardDispenser.doRejectCard(ref message))
                {
                    KioskStatic.logToFile("Error while rejecting card: " + message);
                    log.Error("Error during rejecting card: " + message);
                    SendKioskPopupAlerts(message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("RejectCard error: " + ex.Message);
                this.SendKioskProgressUpdates(ex.Message);
            }
            log.LogMethodExit();
        }
        private void EjjectCard(CardDispenser.CardDispenser cardDispenser, Card destinationCard, string msg)
        {
            log.LogMethodEntry("cardDispenser", (destinationCard != null ? destinationCard.CardNumber : "-1"), msg);
            KioskStatic.logToFile("In EjjectCard: " + (destinationCard != null ? destinationCard.CardNumber : "null") + " : " + msg);
            try
            {
                string productName = GetProductDetailsFromTrxLine(destinationCard.CardNumber) + " " + msg;
                KioskStatic.UpdateKioskActivityLog(executionContext, NEWCARD, productName, destinationCard.CardNumber, GetTransactionId, globalKioskTrxId);
                System.Threading.Thread.Sleep(300);
                string message = string.Empty;
                cardDispenser.doEjectCard(ref message);
                while (true)
                {
                    System.Threading.Thread.Sleep(300);
                    int cardPosition = 0;
                    if (cardDispenser.checkStatus(ref cardPosition, ref message))
                    {
                        if (cardPosition >= 2)
                        {
                            message = MessageContainerList.GetMessage(executionContext, 393);
                            //Please Collect Your Card...
                            SendKioskPopupAlerts(message);
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
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("EjjectCard error: " + ex.Message);
                this.SendKioskProgressUpdates(ex.Message);
            }
            KioskStatic.logToFile("Exit EjjectCard: " + (destinationCard != null ? destinationCard.CardNumber : "null"));
            log.LogMethodExit();
        }
        private string GetProductDetailsFromTrxLine(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            StringBuilder productNames = new StringBuilder("");
            List<Transaction.Transaction.TransactionLine> pTrxLines = kioskTrx.TrxLines.Where(tl => tl.LineValid
                                                                           && tl.CardNumber == cardNumber
                                                                           && tl.ProductTypeCode != ProductTypeValues.CARDDEPOSIT).ToList();
            if (pTrxLines != null && pTrxLines.Any())
            {
                List<string> prodNameList = pTrxLines.Select(tl => tl.ProductName).Distinct().ToList();
                if (prodNameList != null && prodNameList.Any())
                {
                    for (int i = 0; i < prodNameList.Count; i++)
                    {
                        productNames.Append(prodNameList[i]);
                        if (i < prodNameList.Count - 1)
                        { productNames.Append(","); }
                    }
                }
            }
            log.LogMethodExit(productNames);
            return productNames.ToString();
        }
        private Card GetCardFromDispenser(CardDispenser.CardDispenser cardDispenser)
        {
            log.LogMethodEntry();
            Card currentCard = null;
            int cardDispenseRetryCount = 3;
            int dispenserPort = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "CARD_DISPENSER_PORT", -1);
            if (dispenserPort == -1)
            {
                KioskStatic.logToFile("Card dispenser is disabled , dispport == -1");
                string errMsg = MessageContainerList.GetMessage(executionContext, 2384);
                //Card dispenser is Disabled.Sorry you cannot proceed
                ValidationException validationException = new ValidationException(errMsg);
                log.Error(validationException);
                throw validationException;
            }
            string cardNumber = string.Empty;
            string message = string.Empty;
            while (true)
            {
                bool succ = cardDispenser.doDispenseCard(ref cardNumber, ref message);
                KioskStatic.logToFile("cardDispenser.doDispenseCard: Success - " + (succ ? "Yes" : "No") + " : Card No# " + cardNumber);
                if (!succ)
                {
                    KioskStatic.logToFile(message);
                    if (cardDispenser.criticalError)
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, 441) +
                          Environment.NewLine + MessageContainerList.GetMessage(executionContext, "Dispenser Error") + ": " +
                          message + Environment.NewLine +
                          MessageContainerList.GetMessage(executionContext, "Please fix the error and press Close to continue");
                        KioskStatic.logToFile("Card dispenser Error: " + msg);
                        SendKioskPopupAlerts(msg);
                    }
                    else
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, "Dispenser Error") + ": " + message + MessageContainerList.GetMessage(executionContext, ". Retrying...");
                        KioskStatic.logToFile(msg);
                        SendKioskProgressUpdates(msg);
                        System.Threading.Thread.Sleep(1500);
                    }
                    if (KioskStatic.CardDispenserModel.Equals(Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models.SCT0M0))
                    {
                        // If Sankyo dispenser is unable to read the Card, then reject it
                        try
                        {
                            if (!cardDispenser.doRejectCard(ref message))
                            {
                                KioskStatic.logToFile(message);
                                ValidationException validationException = new ValidationException(message);
                                log.Error(validationException);
                                throw validationException;
                            }
                        }
                        catch (Exception ex)
                        {
                            string actionType = MessageContainerList.GetMessage(executionContext, "rejecting card for Sankyo dispenser");
                            string errorMsg = MessageContainerList.GetMessage(executionContext, 5078, actionType, ex.Message); //Unexpected error occured while &1. Error : &2
                            KioskStatic.logToFile(errorMsg);
                            log.Error(errorMsg, ex);
                        }
                    }
                    cardDispenseRetryCount--;
                    if (cardDispenseRetryCount > 0)
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, "Dispense Failed. Retrying") + " [" + (3 - cardDispenseRetryCount).ToString() + "]";
                        KioskStatic.logToFile(msg);
                        SendKioskProgressUpdates(msg);
                        continue;
                    }
                    else
                    {
                        string errMsg = MessageContainerList.GetMessage(executionContext, "Unable to issue card after MAX retries. Contact Staff.");
                        KioskStatic.logToFile(errMsg);
                        ValidationException validationException = new ValidationException(errMsg);
                        log.Error(validationException);
                        throw validationException;
                    }
                }
                else
                {
                    if (KioskStatic.CardDispenserModel.Equals(Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models.SCT0M0) == false)
                    {
                        int counter = 0;
                        while (string.IsNullOrWhiteSpace(cardNumber))
                        {
                            string newCardNumber = KioskStatic.DispenserReaderDevice.readValidatedCardNumber();
                            cardNumber = KioskHelper.ValidateDispenserCardNumber(executionContext, newCardNumber);
                            if (string.IsNullOrWhiteSpace(cardNumber) == false)
                            {
                                break;
                            }
                            string msg = MessageContainerList.GetMessage(executionContext, "Unable to read card number. Retrying...");
                            KioskStatic.logToFile(msg);
                            log.Error(msg);
                            counter++;
                            if (counter > 2)
                            {
                                break;
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(cardNumber))
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, "Card Dispensed but not read. Rejecting");
                        KioskStatic.logToFile(msg);
                        SendKioskProgressUpdates(msg);
                        System.Threading.Thread.Sleep(300);
                        if (!cardDispenser.doRejectCard(ref message))
                        {
                            KioskStatic.logToFile(message);
                            ValidationException validationException = new ValidationException(message);
                            log.Error(validationException);
                            throw validationException;
                        }
                        cardDispenseRetryCount--;
                        if (cardDispenseRetryCount > 0)
                        {
                            System.Threading.Thread.Sleep(2000);
                            //Application.DoEvents();
                            continue;
                        }
                        else
                        {
                            string errMsg = MessageContainerList.GetMessage(executionContext, "Unable to issue card after MAX retries. Contact Staff.");
                            KioskStatic.logToFile(errMsg);
                            ValidationException validationException = new ValidationException(errMsg);
                            log.Error(validationException);
                            throw validationException;
                        }
                    }
                    else
                    {
                        if (KioskStatic.CardDispenserModel.Equals(Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models.SCT0M0))
                        {
                            KioskStatic.DispenserReaderDevice = new DeviceClass();
                        }
                        if (utilities.ParafaitEnv.MIFARE_CARD)
                        {
                            currentCard = new MifareCard(KioskStatic.DispenserReaderDevice, cardNumber, "External POS", utilities);
                        }
                        else
                        {
                            currentCard = new Card(KioskStatic.DispenserReaderDevice, cardNumber, "External POS", utilities);
                        }

                        if (currentCard != null)
                        {
                            if (KioskHelper.CardRoamingRemotingClient != null
                                || (KioskStatic.Utilities.ParafaitEnv.ALLOW_ROAMING_CARDS == "Y"
                                     && KioskStatic.Utilities.ParafaitEnv.ENABLE_ON_DEMAND_ROAMING == "Y"))
                            {
                                string msg = MessageContainerList.GetMessage(executionContext, 1607);//Refreshing Card from HQ. Please Wait...
                                KioskStatic.logToFile(msg);
                                SendKioskProgressUpdates(msg);
                            }
                            if (!KioskHelper.refreshCardFromHQ(ref currentCard, ref message))
                            {
                                string msg = "Unable refresh Card From HQ [" + currentCard.CardNumber + " ]" + " error: " + message;
                                RejectCardWithIssues(cardDispenser, currentCard, msg);
                            }
                            if (currentCard.CardStatus.Equals("ISSUED") && currentCard.technician_card.Equals('N'))
                            {
                                string msg1 = "Refunding issued card [" + currentCard.CardNumber + "]";
                                KioskStatic.logToFile(msg1);
                                TaskProcs tp = new TaskProcs(KioskStatic.Utilities);
                                string refundMsg = "";
                                if (!tp.RefundCard(currentCard, 0, 0, 0, "Deactivated By Kiosk", ref refundMsg, true))
                                {
                                    string msg = ("Unable to refund card [" + currentCard.CardNumber + " ]" + " error: " + refundMsg);
                                    RejectCardWithIssues(cardDispenser, currentCard, msg);
                                }
                                else
                                {
                                    string msg2 = "Marking card [" + currentCard.CardNumber + "] as new";
                                    KioskStatic.logToFile(msg2);
                                    currentCard.invalidateCard(null);
                                    currentCard = new Card(currentCard.CardNumber, utilities.ParafaitEnv.LoginID, utilities);
                                }

                            }
                            if (currentCard.technician_card.Equals('Y'))
                            {
                                string errMsg = "Technician cards cannot be dispensed. card number: [" + currentCard.CardNumber + " ]";
                                RejectCardWithIssues(cardDispenser, currentCard, errMsg);
                            }
                            break;
                        }
                        if (KioskStatic.CardDispenserModel.Equals(Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models.SCT0M0))
                        {
                            KioskStatic.DispenserReaderDevice = null;
                        }
                    }
                }
            }
            if (currentCard != null)
            {
                string msg2 = "Card from dispenser [" + currentCard.CardNumber + "]";
                KioskStatic.logToFile(msg2);
            }
            else
            {
                string msg2 = "Failed to receive card from dispenser";
                KioskStatic.logToFile(msg2);
            }
            log.LogMethodExit(currentCard);
            return currentCard;
        }

        private static void RejectCardWithIssues(CardDispenser.CardDispenser cardDispenser, Card currentCard, string msg)
        {
            log.LogMethodEntry(cardDispenser, currentCard, msg);
            KioskStatic.logToFile("RejectCardWithIssues: " + msg);
            System.Threading.Thread.Sleep(300);
            string rejMsg = string.Empty;
            if (!cardDispenser.doRejectCard(ref rejMsg))
            {
                currentCard = null;
                KioskStatic.logToFile("unable to RejectCardWithIssues:" + rejMsg);
                ValidationException validationException = new ValidationException(rejMsg);
                log.Error(validationException);
                throw validationException;
            }
            else
            {
                KioskStatic.logToFile("Done with RejectCardWithIssues");
                currentCard = null;
                ValidationException validationException = new ValidationException(msg);
                log.Error(validationException);
                throw validationException;
            }
            log.LogMethodExit();
        }

        private void SendKioskProgressUpdates(string message)
        {
            log.LogMethodEntry(message);
            if (KioskProgressUpdates != null)
            {
                KioskProgressUpdates(message);
            }
            else
            {
                log.Info("KioskProgressUpdates is not defined. Hence no message sent back");
            }
            log.LogMethodExit();
        }
        private void SendKioskPopupAlerts(string message)
        {
            log.LogMethodEntry(message);
            if (KioskPopupAlerts != null)
            {
                KioskPopupAlerts(message);
            }
            else
            {
                log.Info("KioskPopupAlerts is not defined. Hence no message sent back");
            }
            log.LogMethodExit();
        }
        private void SendKioskShowThankYou(bool printed, bool receiptEmailed)
        {
            log.LogMethodEntry(printed, receiptEmailed);
            if (KioskShowThankYou != null)
            {
                KioskStatic.logToFile("In SendKioskShowThankYou()");
                KioskShowThankYou(printed, receiptEmailed);
            }
            else
            {
                log.Info("KioskShowThankYou is not defined. Hence no message sent back");
            }
            log.LogMethodExit();
        }
        private void SendkioskAbortTransactionEvent()
        {
            log.LogMethodEntry();
            if (kioskAbortTransactionEvent != null)
            {
                KioskStatic.logToFile("In SendkioskAbortTransactionEvent()");
                kioskAbortTransactionEvent();
            }
            else
            {
                log.Info("kioskAbortTransactionEvent is not defined. Hence no message sent back");
            }
            log.LogMethodExit();
        }

        private void DoTransferEntitlements(string sourceCardNumber, Card destinationCard)
        {
            log.LogMethodEntry(sourceCardNumber, (destinationCard != null ? destinationCard.card_id + " : " + destinationCard.CardNumber : "-1"));
            KioskStatic.logToFile("In DoTransferEntitlements(): sourceCardNumber - " + sourceCardNumber
                + " destinationCard: " + (destinationCard != null ? destinationCard.CardNumber : "Null"));
            TaskProcs tp = new TaskProcs(utilities);
            CardUtils cardUtils = new CardUtils(utilities);
            List<string> cardNumberList = new List<string>() { sourceCardNumber };
            List<Card> cardObjectList = cardUtils.GetCardList(cardNumberList, "", null);
            if (cardObjectList != null && cardObjectList.Any())
            {
                Card sourceCard = cardObjectList[0];
                KioskStatic.logToFile("sourceCard: " + (sourceCard != null ? sourceCard.CardNumber : "null"));
                if (destinationCard.CardStatus == "ISSUED")
                {
                    List<Card> cards = new List<Card>() { sourceCard, destinationCard };
                    KioskStatic.logToFile("Start Consolidate for " + destinationCard.CardNumber);
                    log.Debug("Start Consolidate for " + destinationCard.CardNumber);
                    string message = string.Empty;
                    if (!tp.Consolidate(cards, 2, "Kiosk Transaction", ref message, null, true, true))
                    {
                        KioskStatic.logToFile("Error during Consolidate cards: " + message);
                        ValidationException validationException = new ValidationException(message);
                        log.Error("Error during Consolidate cards: ", validationException);
                        throw validationException;
                    }
                    log.Debug("End Consolidate for " + destinationCard.CardNumber);
                }
                else
                {
                    log.Debug("Start transferCard for " + destinationCard.CardNumber);
                    KioskStatic.logToFile("Start transferCard for " + destinationCard.CardNumber);
                    string message = string.Empty;
                    if (!tp.transferCard(sourceCard, destinationCard, "Kiosk Transaction", ref message, null, kioskTrx.Trx_id))
                    {
                        KioskStatic.logToFile("Error during transferCard: " + message);
                        ValidationException validationException = new ValidationException(message);
                        log.Error("Error during transferCard: ", validationException);
                        throw validationException;
                    }
                    log.Debug("End transferCard for " + destinationCard.CardNumber);
                }
                RefreshCardInfoOnTransaction(sourceCardNumber, destinationCard);
            }
            else
            {
                string msgPart1 = MessageContainerList.GetMessage(executionContext, "Card");
                string errMsg = MessageContainerList.GetMessage(executionContext, 2757, msgPart1, sourceCardNumber);
                KioskStatic.logToFile(errMsg);
                ValidationException validationException = new ValidationException(errMsg);
                log.Error("Error in DoTransferEntitlements: ", validationException);
                throw validationException;
            }
            log.LogMethodExit();
        }
        private void RefreshCardInfoOnTransaction(string sourceCardNumber, Card destinationCard)
        {
            log.LogMethodEntry(sourceCardNumber, (destinationCard != null ? destinationCard.card_id + " : " + destinationCard.CardNumber : "-1"));
            KioskStatic.logToFile("In RefreshCardInfoOnTransaction");
            if (kioskTrx != null && kioskTrx.TrxLines != null && kioskTrx.TrxLines.Any())
            {
                StringBuilder data = new StringBuilder("");
                for (int i = 0; i < kioskTrx.TrxLines.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(kioskTrx.TrxLines[i].CardNumber) == false
                         && kioskTrx.TrxLines[i].CardNumber == sourceCardNumber)
                    {
                        data.Append("Line " + i + " is updated. ");
                        kioskTrx.TrxLines[i].card = destinationCard;
                        kioskTrx.TrxLines[i].CardNumber = destinationCard.CardNumber;
                    }
                }
                KioskStatic.logToFile(data.ToString());
            }
            log.LogMethodExit();
        }
        private bool PrintTheTransaction(bool printReceipt)
        {
            log.LogMethodEntry(printReceipt);
            KioskStatic.logToFile("In PrintTheTransaction()");
            bool printed = true;
            this.cardPrinterError = false;
            try
            {
                string message = string.Empty;
                List<POSPrinterDTO> posPrinterDTOList = new List<POSPrinterDTO>();
                for (int i = 0; i < KioskStatic.POSMachineDTO.PosPrinterDtoList.Count; i++)
                {
                    POSPrinterDTO posPrinterDTO = KioskStatic.POSMachineDTO.PosPrinterDtoList[i];
                    posPrinterDTOList.Add(posPrinterDTO);
                    if (printReceipt == false && posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter)
                    {
                        posPrinterDTOList.Remove(posPrinterDTO);
                    }
                }
                PrintTransaction printTransaction = new PrintTransaction(posPrinterDTOList);
                printTransaction.PrintProgressUpdates = new PrintTransaction.ProgressUpdates(SendKioskProgressUpdates);
                printTransaction.SetCardPrinterErrorValue = new PrintTransaction.SetCardPrinterError(SetCardPrinterErrorValue);
                TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                Transaction.Transaction trx = TransactionUtils.CreateTransactionFromDB(kioskTrx.Trx_id, utilities);
                bool kotPrintOnly = (printReceipt == false ? true : false);
                if (!printTransaction.Print(trx, -1, ref message, kotPrintOnly))
                {
                    printed = false;
                    string msg = MessageContainerList.GetMessage(executionContext, "Transaction Print Error: ") + message;
                    SendKioskProgressUpdates(msg); // 2386 - Card printing error.
                    KioskStatic.logToFile(msg);
                    log.Error(msg);
                    SendKioskPopupAlerts(msg);
                    KioskStatic.UpdateKioskActivityLog(executionContext, PRINTERROR, msg, null, GetTransactionId, globalKioskTrxId);
                    if (cardPrinterError)
                    {
                        GenerateErrorReceipt(CARDPRINT);
                    }
                }
                if (printReceipt == false)
                {
                    printed = false;
                }
                KioskStatic.logToFile("Print message: " + message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SendKioskProgressUpdates(ex.Message);
                KioskStatic.logToFile("Unexpected Error while Printing Transaction Receipt : " + ex.Message);
                string actionType = MessageContainerList.GetMessage(executionContext, "Printing Transaction Receipt");
                string msg = MessageContainerList.GetMessage(executionContext, 5078, actionType, ex.Message); //Unexpected error occured while &1. Error : &2
                this.SendKioskPopupAlerts(msg);
            }
            log.LogMethodExit(printed);
            return printed;
        }
        private void SetCardPrinterErrorValue(bool errorValue)
        {
            log.LogMethodEntry(errorValue);
            cardPrinterError = errorValue;
            log.LogMethodExit(cardPrinterError);
        }
        private void GenerateErrorReceipt(string mode)
        {
            log.LogMethodEntry(mode);
            KioskStatic.logToFile("Generate Error Receipt: " + mode);
            try
            {
                PrintTransaction printTransaction = new PrintTransaction();
                TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                Transaction.Transaction trx = TransactionUtils.CreateTransactionFromDB(kioskTrx.Trx_id, utilities);
                //LogAutoGenCardActivity(trx, productName, activity, autoGenCardTrxLineInfo);
                if (trx.POSPrinterDTOList == null || trx.POSPrinterDTOList.Count == 0)
                {
                    POSMachines posMachine = new POSMachines(trx.Utilities.ExecutionContext, trx.Utilities.ParafaitEnv.POSMachineId);
                    trx.POSPrinterDTOList = posMachine.PopulatePrinterDetails();
                }
                //retain one receipt printer
                RetainReceiptPrinter(trx);
                TemporaryInvalidationOfTrxLines(trx, mode);
                trx.GetPrintableTransactionLines(trx.POSPrinterDTOList);
                string templateName = GetTemplateName(mode);
                int errorReceiptTemplate = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, templateName, -1);
                if (errorReceiptTemplate == -1)
                {
                    // templateName is not found but still proceed with template defined by the printer.
                    string errMsg = (MessageContainerList.GetMessage(executionContext, 2299, templateName));
                    //Value is not defined for &1 
                    KioskStatic.logToFile(errMsg);
                    log.Error(errMsg);
                    SendKioskProgressUpdates(errMsg);
                }
                else
                { // using templateName
                    ReceiptPrintTemplateHeaderBL receiptPrintTemplateHeaderBL = new ReceiptPrintTemplateHeaderBL(executionContext, errorReceiptTemplate, true);
                    trx.POSPrinterDTOList[0].ReceiptPrintTemplateHeaderDTO = receiptPrintTemplateHeaderBL.ReceiptPrintTemplateHeaderDTO;
                }
                if (RececiptDeliveryMode == KioskReceiptDeliveryMode.EMAIL)
                {
                    EmailErrorReceipt(mode, trx);
                }
                else if (RececiptDeliveryMode == KioskReceiptDeliveryMode.PRINT)
                {
                    printTransaction.GenericReceiptPrint(trx, trx.POSPrinterDTOList[0]);
                }
                string message = GetFinalErrorPrintMessage(mode);
                //KioskStatic.logToFile(message);
                //SendKioskPopupAlerts(message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in Generate Error Receipt method: " + ex.Message);
                string message = GetUnexpectedErrorPrintMessage(mode);
                KioskStatic.logToFile(message);
                SendKioskPopupAlerts(message);
            }
            log.LogMethodExit();
        }
        private void RetainReceiptPrinter(Transaction.Transaction trx)
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
                    PrinterDTO printerDTO = new PrinterDTO(-1, "Default", "Default", 0, true, DateTime.Now, "", DateTime.Now, "", "", "", -1, PrinterDTO.PrinterTypes.ReceiptPrinter, -1, "", false, -1, -1, 0);
                    POSPrinterDTO posPrinterDTO = new POSPrinterDTO(-1, -1, -1, -1, -1, -1, -1, printerDTO, null, null, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1, -1);
                    trx.POSPrinterDTOList.Add(posPrinterDTO);
                }
                else
                {   //retain one receipt printer
                    for (int i = 0; i < trx.POSPrinterDTOList.Count; i++)
                    {
                        if (i > 0)
                        {
                            trx.POSPrinterDTOList.RemoveAt(i);
                            i = i - 1;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void TemporaryInvalidationOfTrxLines(Transaction.Transaction customerTransaction, string mode)
        {
            log.LogMethodEntry(customerTransaction.Trx_id, mode);
            switch (mode)
            {
                case CARDPRINT:

                    foreach (Transaction.Transaction.TransactionLine trxLine in customerTransaction.TrxLines)
                    {
                        //line is already printed, or it is Tcard or not a card line or a deposit line and active then mark them as inactive for print purpose
                        if ((trxLine.ReceiptPrinted == true || string.IsNullOrWhiteSpace(trxLine.CardNumber) == true
                            || (string.IsNullOrWhiteSpace(trxLine.CardNumber) == false && trxLine.CardNumber.StartsWith("T"))
                            || trxLine.ProductTypeCode.Equals("LOCKERDEPOSIT") || trxLine.ProductTypeCode.Equals("DEPOSIT")
                            || trxLine.ProductTypeCode.Equals("CARDDEPOSIT")) && trxLine.LineValid == true)
                        {
                            trxLine.LineValid = false;
                        }
                    }
                    break;
                case CARDDISPENSER:
                    foreach (Transaction.Transaction.TransactionLine trxLine in customerTransaction.TrxLines)
                    {
                        //Other than Temp card lines mark rest as invalid
                        if (false == (string.IsNullOrWhiteSpace(trxLine.CardNumber) == false && trxLine.CardNumber.StartsWith("T") && trxLine.LineValid == true))
                        {
                            trxLine.LineValid = false;
                        }
                    }
                    break;
            }
            log.LogMethodExit();
        }
        private string GetTemplateName(string mode)
        {
            log.LogMethodEntry(mode);
            string templateName = string.Empty;
            switch (mode)
            {
                case CARDPRINT:
                    templateName = CARD_PRINT_ERROR_RECEIPT_TEMPLATE;
                    break;
                case CARDDISPENSER:
                    templateName = CARD_DISPENSER_ERROR_RECEIPT_TEMPLATE;
                    break;
            }
            log.LogMethodExit(templateName);
            return templateName;
        }
        private string GetFinalErrorPrintMessage(string mode)
        {
            log.LogMethodEntry(mode);
            string templateName = string.Empty;
            switch (mode)
            {
                case CARDPRINT:
                    templateName = MessageContainerList.GetMessage(executionContext, 3010) + ". " + MessageContainerList.GetMessage(executionContext, 3011);
                    //"Error while printing wrist bands" "Please contact attendant with the receipt printed."
                    break;
                case CARDDISPENSER:
                    templateName = MessageContainerList.GetMessage(executionContext, 380) + ". " + MessageContainerList.GetMessage(executionContext, 3011);
                    //"Error in Dispensing the Card" "Please contact attendant with the receipt printed."
                    break;
            }
            log.LogMethodExit(templateName);
            return templateName;

        }
        private string GetUnexpectedErrorPrintMessage(string mode)
        {
            log.LogMethodEntry(mode);
            string templateName = string.Empty;
            switch (mode)
            {
                case CARDPRINT:
                    templateName = MessageContainerList.GetMessage(executionContext, 3010) + ". " + MessageContainerList.GetMessage(executionContext, 3012);
                    //"Error while printing wrist bands" "Please contact attendant"
                    break;
                case CARDDISPENSER:
                    templateName = MessageContainerList.GetMessage(executionContext, 380) + ". " + MessageContainerList.GetMessage(executionContext, 3012);
                    //"Error in Dispensing the Card" "Please contact attendant"
                    break;
            }
            log.LogMethodExit(templateName);
            return templateName;
        }
        private bool EmailTransactionReceipt(bool generateReceipt)
        {
            log.LogMethodEntry(generateReceipt);
            KioskStatic.logToFile("In EmailTransactionReceipt1()");
            bool emailed = false;
            try
            {
                if (receciptDeliveryMode == KioskReceiptDeliveryMode.EMAIL && generateReceipt)
                {
                    TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                    Transaction.Transaction trx = TransactionUtils.CreateTransactionFromDB(kioskTrx.Trx_id, utilities);
                    TransactionEventsBL transactionEventsBL = new TransactionEventsBL(executionContext, utilities, Communication.ParafaitFunctionEvents.PURCHASE_EVENT, trx);
                    transactionEventsBL.SendMessage(Communication.MessagingClientDTO.MessagingChanelType.EMAIL);
                    emailed = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SendKioskProgressUpdates(ex.Message);
                KioskStatic.logToFile("Unexpected Error while Emailing Transaction Receipt : " + ex.Message);
                string actionType = MessageContainerList.GetMessage(executionContext, "Emailing Transaction Receipt");
                string msg = MessageContainerList.GetMessage(executionContext, 5078, actionType, ex.Message); //Unexpected error occured while &1. Error : &2
                this.SendKioskPopupAlerts(msg);
            }
            log.LogMethodExit(emailed);
            return emailed;
        }
        public bool EmailTransactionReceipt(TransactionEventContactsDTO transactionEventContactsDTO, PopupAlerts popupAlerts)
        {
            log.LogMethodEntry(transactionEventContactsDTO, popupAlerts);
            KioskStatic.logToFile("In EmailTransactionReceipt2()");
            this.KioskPopupAlerts = popupAlerts;
            bool emailed = false;
            try
            {
                if (receciptDeliveryMode == KioskReceiptDeliveryMode.EMAIL)
                {
                    TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                    Transaction.Transaction trx = TransactionUtils.CreateTransactionFromDB(kioskTrx.Trx_id, utilities);
                    TransactionEventsBL transactionEventsBL = new TransactionEventsBL(executionContext, utilities, Communication.ParafaitFunctionEvents.PURCHASE_EVENT, trx, transactionEventContactsDTO);
                    transactionEventsBL.SendMessage(Communication.MessagingClientDTO.MessagingChanelType.EMAIL);
                    emailed = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SendKioskProgressUpdates(ex.Message);
                KioskStatic.logToFile("Unexpected Error while Emailing Transaction Receipt : " + ex.Message);
                string actionType = MessageContainerList.GetMessage(executionContext, "Emailing Transaction Receipt");
                string msg = MessageContainerList.GetMessage(executionContext, 5078, actionType, ex.Message); //Unexpected error occured while &1. Error : &2
                this.SendKioskPopupAlerts(msg);
            }
            finally
            {
                this.KioskPopupAlerts = null;
            }
            log.LogMethodExit(emailed);
            return emailed;
        }
        public bool EmailRedeemTokenTransactionReceipt(bool generateReceipt)
        {
            log.LogMethodEntry(generateReceipt);
            KioskStatic.logToFile("In EmailRedeemTokenTransactionReceipt()");
            bool emailed = false;
            try
            {
                if (generateReceipt)
                {
                    TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                    Transaction.Transaction trx = TransactionUtils.CreateTransactionFromDB(kioskTrx.Trx_id, utilities);
                    TransactionEventsBL transactionEventsBL = new TransactionEventsBL(executionContext, utilities, Communication.ParafaitFunctionEvents.REDEEM_TOKEN_TRANSACTION_EVENT, trx);
                    transactionEventsBL.SendMessage(Communication.MessagingClientDTO.MessagingChanelType.EMAIL);
                    emailed = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected Error while Emailing Redeem Token Transaction Receipt : " + ex.Message);
                string actionType = MessageContainerList.GetMessage(executionContext, "Emailing Redeem Token Transaction Receipt");
                string msg = MessageContainerList.GetMessage(executionContext, 5078, actionType, ex.Message); //Unexpected error occured while &1. Error : &2
                SendKioskPopupAlerts(msg);
            }
            log.LogMethodExit(emailed);
            return emailed;
        }
        public bool EmailTransactionAbortReceipt()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In EmailTransactionAbortReceipt()");
            bool emailed = false;
            try
            {
                Transaction.Transaction trx = null;
                if (kioskTrx.Trx_id > 0)
                {
                    TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                    trx = TransactionUtils.CreateTransactionFromDB(kioskTrx.Trx_id, utilities);
                }
                else
                {
                    trx = kioskTrx;
                    if (string.IsNullOrWhiteSpace(trx.customerIdentifier) == false)
                    {
                        string decryptVal = Encryption.Decrypt(trx.customerIdentifier);
                        if (string.IsNullOrWhiteSpace(decryptVal))
                        {   //Encrypt if info is not already encrypted.
                            trx.customerIdentifier = Encryption.Encrypt(trx.customerIdentifier);
                        }
                    }
                }
                TransactionEventsBL transactionEventsBL = new TransactionEventsBL(executionContext, utilities, Communication.ParafaitFunctionEvents.ABORT_TRANSACTION_EVENT, trx);
                transactionEventsBL.SendMessage(Communication.MessagingClientDTO.MessagingChanelType.EMAIL);
                emailed = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SendKioskProgressUpdates(ex.Message);
            }
            log.LogMethodExit(emailed);
            return emailed;
        }
        public bool EmailRedeemTokenAbortReceipt()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In EmailRedeemTokenAbortReceipt()");
            bool emailed = false;
            try
            {
                Transaction.Transaction trx = null;
                if (kioskTrx.Trx_id > 0)
                {
                    TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                    trx = TransactionUtils.CreateTransactionFromDB(kioskTrx.Trx_id, utilities);
                }
                else
                {
                    trx = kioskTrx;
                    if (string.IsNullOrWhiteSpace(trx.customerIdentifier) == false)
                    {
                        string decryptVal = Encryption.Decrypt(trx.customerIdentifier);
                        if (string.IsNullOrWhiteSpace(decryptVal))
                        {   //Encrypt if info is not already encrypted.
                            trx.customerIdentifier = Encryption.Encrypt(trx.customerIdentifier);
                        }
                    }
                }
                TransactionEventsBL transactionEventsBL = new TransactionEventsBL(executionContext, utilities, Communication.ParafaitFunctionEvents.ABORT_REDEEM_TOKEN_TRANSACTION_EVENT, trx);
                transactionEventsBL.SendMessage(Communication.MessagingClientDTO.MessagingChanelType.EMAIL);
                emailed = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SendKioskProgressUpdates(ex.Message);
            }
            log.LogMethodExit(emailed);
            return emailed;
        }
        public void EmailErrorReceipt(string mode, Transaction.Transaction trx)
        {
            log.LogMethodEntry(mode);
            KioskStatic.logToFile("In EmailErrorReceipt()");
            TransactionEventsBL transactionEventsBL = null;
            switch (mode)
            {
                case CARDPRINT:
                    transactionEventsBL = new TransactionEventsBL(executionContext, utilities, Communication.ParafaitFunctionEvents.KiOSK_WRISTBAND_PRINT_ERROR, trx);
                    transactionEventsBL.SendMessage(Communication.MessagingClientDTO.MessagingChanelType.EMAIL);
                    break;
                case CARDDISPENSER:
                    transactionEventsBL = new TransactionEventsBL(executionContext, utilities, Communication.ParafaitFunctionEvents.KIOSK_CARD_DISPENSER_ERROR_EVENT, trx);
                    transactionEventsBL.SendMessage(Communication.MessagingClientDTO.MessagingChanelType.EMAIL);
                    break;
            }
            log.LogMethodExit();
        }
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> CreateTrxLinesForCombo(ProductsContainerDTO comboProductsContainerDTO, PurchaseProductDTO purchaseProductDTO, Card parentCard,
                                            bool applyCardCreditPlusConsumption)
        {
            log.LogMethodEntry(comboProductsContainerDTO, purchaseProductDTO, parentCard, applyCardCreditPlusConsumption);
            var productMapping = purchaseProductDTO.ProductQtyMappingDTOs.Where(p => p.ProductsContainerDTO.ProductType == ProductTypeValues.CHECKIN).FirstOrDefault();

            int checkInFacilityId = productMapping == null ? -1 : productMapping.ProductsContainerDTO.CheckInFacilityId;
            CustomerDTO parentCustomerDTO = parentCard.customerDTO;
            CheckInDTO checkInDTO = null;
            if (kioskTrx.TrxLines.Exists(x => x.LineCheckInDTO != null)) // same transation without save
            {
                checkInDTO = kioskTrx.TrxLines.Where(x => x.LineCheckInDTO != null).FirstOrDefault().LineCheckInDTO;
            }
            else
            {
                checkInDTO = new CheckInDTO(-1, parentCustomerDTO.Id, null, string.Empty, null,
                                    null, (parentCard == null ? -1 : parentCard.card_id), -1, -1, checkInFacilityId,
                                     -1, -1, parentCustomerDTO, true);
            }
            CheckInBL checkInBL = new CheckInBL(executionContext, checkInDTO);
            checkInDTO.CheckInDetailDTOList.Clear();  //CheckInDTO will only have header checkin 

            if (applyCardCreditPlusConsumption == false)
            {
                kioskTrx.PrimaryCard = null;
            }
            else
            {
                if (parentCard != null)
                {
                    SetPrimaryCard(parentCard);
                }
            }

            List<ComboCheckInDetailDTO> comboCheckInDetailDTOs = new List<ComboCheckInDetailDTO>();
            List<Transaction.Transaction.ComboManualProduct> comboManualProducts = new List<Transaction.Transaction.ComboManualProduct>();
            foreach (ProductQtyMappingDTO productQtyMappingDTO in purchaseProductDTO.ProductQtyMappingDTOs)
            {
                if (productQtyMappingDTO.ProductsContainerDTO.ProductType == ProductTypeValues.CHECKIN)
                {
                    ComboCheckInDetailDTO comboCheckInDetailDTO = new ComboCheckInDetailDTO();
                    comboCheckInDetailDTO.CheckInProductId = productQtyMappingDTO.ProductsContainerDTO.ProductId;
                    comboCheckInDetailDTO.CreateLinesWithCheckInDetails = false;
                    for (int i = 0; i < productQtyMappingDTO.Quantity; i++)
                    {
                        CheckInDetailDTO dummyCheckInDetailDTO = new CheckInDetailDTO();
                        comboCheckInDetailDTO.CheckInDetailDTOList.Add(dummyCheckInDetailDTO);
                    }
                    comboCheckInDetailDTOs.Add(comboCheckInDetailDTO);
                }
                else if (productQtyMappingDTO.ProductsContainerDTO.ProductType == ProductTypeValues.MANUAL)
                {
                    Transaction.Transaction.ComboManualProduct comboManualProduct = new Transaction.Transaction.ComboManualProduct();
                    comboManualProduct.ComboProductId = comboProductsContainerDTO.ProductId;
                    comboManualProduct.ChildProductId = productQtyMappingDTO.ProductsContainerDTO.ProductId;
                    comboManualProduct.ChildProductName = productQtyMappingDTO.ProductsContainerDTO.ProductName;
                    comboManualProduct.Quantity = productQtyMappingDTO.Quantity;
                    comboManualProducts.Add(comboManualProduct);
                }
            }
            List<KeyValuePair<int, int>> categoryProducts = new List<KeyValuePair<int, int>>();
            string msg = string.Empty;
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> addedLines = new List<Transaction.Transaction.TransactionLine>();
            Semnox.Parafait.Transaction.Transaction.TransactionLine comboParentLine = new Transaction.Transaction.TransactionLine();
            int retcode = kioskTrx.CreateComboProduct(ProductId: comboProductsContainerDTO.ProductId, Price: -1, Quantity: 1, message: ref msg, ComboParentLine: comboParentLine,
                              CardProducts: null, CategoryProducts: categoryProducts, CreateChildLines: true, isGroupMeal: false, comboProductId: -1,
                              parentTrxLine: null, purchasedProductList: null, atbList: null, checkInDTO: checkInDTO, customCheckInDetailDTOList: comboCheckInDetailDTOs,
                              comboManualProductsList: comboManualProducts);
            if (retcode != 0)
            {
                log.Error(msg);
                KioskStatic.logToFile("Error creating Combo Product: " + msg);
                throw new Exception(msg);
            }
            else
            {
                int lineIndex = kioskTrx.TrxLines.IndexOf(comboParentLine);
                if (lineIndex > -1)
                {
                    for (int i = lineIndex; i < kioskTrx.TrxLines.Count; i++)
                    {
                        addedLines.Add(kioskTrx.TrxLines[i]);
                    }
                }
            }
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> finalSetOfLines = new List<Transaction.Transaction.TransactionLine>();
            if (addedLines != null && addedLines.Any())
            {
                finalSetOfLines = new List<Transaction.Transaction.TransactionLine>(addedLines);
            }
            log.LogVariableState("finalSetOfLines", finalSetOfLines);
            if (parentCard != null)
            {
                SetPrimaryCard(parentCard);
            }
            log.LogMethodExit();
            return finalSetOfLines;
        }
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> CreateTrxLinesForCheckIn(PurchaseProductDTO purchaseProductDTO, Card parentCard, bool applyCardCreditPlusConsumption)
        {
            log.LogMethodEntry("purchaseProductDTO", "parentCard", applyCardCreditPlusConsumption);
            string message = "";
            var productMapping = purchaseProductDTO.ProductQtyMappingDTOs.Where(p => p.ProductsContainerDTO.ProductType == ProductTypeValues.CHECKIN).FirstOrDefault();
            int checkInFacilityId = productMapping.ProductsContainerDTO.CheckInFacilityId;
            CustomerDTO parentCustomerDTO = parentCard.customerDTO;
            CheckInDTO checkInDTO = null;
            if (kioskTrx.TrxLines.Exists(x => x.LineCheckInDTO != null)) // same transation without save
            {
                checkInDTO = kioskTrx.TrxLines.Where(x => x.LineCheckInDTO != null).FirstOrDefault().LineCheckInDTO;
            }
            else
            {
                checkInDTO = new CheckInDTO(-1, parentCustomerDTO.Id, null, string.Empty, null,
                          null, (parentCard == null ? -1 : parentCard.card_id), -1, -1, checkInFacilityId,
                         -1, -1, parentCustomerDTO, true);
            }
            CheckInBL checkInBL = new CheckInBL(executionContext, checkInDTO);
            checkInDTO.CheckInDetailDTOList.Clear();  //CheckInDTO will be only have header checkin
            if (applyCardCreditPlusConsumption == false) //do not link card to the trx before creating lines to avoid transaction engine applying credit plus consumption automatically.
            {
                kioskTrx.PrimaryCard = null;
            }
            else
            {
                if (parentCard != null)
                {
                    SetPrimaryCard(parentCard);
                }
            }
            CheckInDetailDTO checkInDetailDTO = new CheckInDetailDTO();

            Transaction.Transaction.TransactionLine checkInParentLine = new Transaction.Transaction.TransactionLine();
            int retCode = -1;
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> addedLines = new List<Transaction.Transaction.TransactionLine>();
            for (int i = 0; i < productMapping.Quantity; i++)
            {
                //CheckInDTO only in first line. Other lines have only CheckInDetailDTO. Link all lines using parent line id (of first line)
                if (i == 0)
                {
                    retCode = kioskTrx.createTransactionLine(inCard: null, productId: productMapping.ProductsContainerDTO.ProductId, checkInDTO: checkInDTO
                        , checkInDetailDTO: checkInDetailDTO
                        , in_price: productMapping.ProductsContainerDTO.Price < 0 ? 0 : Convert.ToDouble(productMapping.ProductsContainerDTO.Price)
                        , in_quantity: 1, message: ref message, subscriptionHeaderDTO: null);

                    if (retCode != 0)
                    {
                        log.Error(message);
                        KioskStatic.logToFile("Error creating check in transaction line: " + message);
                        throw new Exception(message);
                    }

                    checkInParentLine = kioskTrx.TrxLines[kioskTrx.TrxLines.Count - 1];
                    //kioskTrx.TrxLines[kioskTrx.TrxLines.Count - 1].ParentLine = checkInParentLine;
                }
                else
                {
                    retCode = kioskTrx.createTransactionLine(inCard: null, productId: productMapping.ProductsContainerDTO.ProductId, checkInDTO: null, checkInDetailDTO: checkInDetailDTO
                        , in_price: productMapping.ProductsContainerDTO.Price < 0 ? 0 : Convert.ToDouble(productMapping.ProductsContainerDTO.Price)
                        , in_quantity: 1, message: ref message, subscriptionHeaderDTO: null);

                    if (retCode != 0)
                    {
                        log.Error(message);
                        KioskStatic.logToFile("Error creating check in transaction line: " + message);
                        throw new Exception(message);
                    }

                    kioskTrx.TrxLines[kioskTrx.TrxLines.Count - 1].ParentLine = checkInParentLine;
                    kioskTrx.TrxLines[kioskTrx.TrxLines.Count - 1].ProductName = checkInParentLine.ProductName;
                }
            }
            int lineIndex = kioskTrx.TrxLines.IndexOf(checkInParentLine);
            if (lineIndex > -1)
            {
                for (int i = lineIndex; i < kioskTrx.TrxLines.Count; i++)
                {
                    addedLines.Add(kioskTrx.TrxLines[i]);
                }
            }
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> finalSetOfLines = new List<Transaction.Transaction.TransactionLine>();
            if (addedLines != null && addedLines.Any())
            {
                finalSetOfLines = new List<Transaction.Transaction.TransactionLine>(addedLines);
            }
            log.LogVariableState("finalSetOfLines", finalSetOfLines);
            if (parentCard != null)
            {
                SetPrimaryCard(parentCard);
            }
            log.LogMethodExit();
            return finalSetOfLines;
        }

        private List<Transaction.Transaction.TransactionLine> AddSingleAttractionProduct(KioskAttractionDTO kioskAttractionDTO)
        {
            log.LogMethodEntry(kioskAttractionDTO);
            List<Transaction.Transaction.TransactionLine> trxLineEntry = new List<Transaction.Transaction.TransactionLine>();

            int productId = kioskAttractionDTO.ProductId;
            int quantity = kioskAttractionDTO.Quantity;
            AttractionBooking atb = new AttractionBooking(executionContext, kioskAttractionDTO.AttractionBookingDTO);
            string msg = string.Empty;
            int lineIndex = (kioskTrx != null && kioskTrx.TrxLines != null ? kioskTrx.TrxLines.Count - 1 : 0);
            bool attractionIsCardSale = KioskHelper.AttractionIsOfTypeCardSale(executionContext);
            if (attractionIsCardSale == false)
            {
                atb.cardList = null;
            }
            if (atb.cardList != null && atb.cardList.Any())
            {
                atb.cardNumberList = new List<string>();
                foreach (Card item in atb.cardList)
                {
                    atb.cardNumberList.Add(item.CardNumber);
                }
            }
            int retCode = kioskTrx.CreateAttractionProduct(productId, -1, quantity, -1, atb, kioskAttractionDTO.CardList, ref msg);
            if (retCode != 0)
            {
                log.Error(msg);
                KioskStatic.logToFile("Error creating attraction transaction line: " + msg);
                throw new Exception(msg);
            }
            else
            {
                int k = (lineIndex == 0 ? 0 : lineIndex + 1);
                for (int i = k; i < kioskTrx.TrxLines.Count; i++)
                {
                    trxLineEntry.Add(kioskTrx.TrxLines[i]);
                }
            }
            log.LogMethodExit(trxLineEntry);
            return trxLineEntry;
        }

        private List<Transaction.Transaction.TransactionLine> AddComboAttractionProduct(KioskAttractionDTO kioskAttractionDTO)
        {
            log.LogMethodEntry(kioskAttractionDTO);
            List<Transaction.Transaction.TransactionLine> trxLineEntry = new List<Transaction.Transaction.TransactionLine>();
            int productId = kioskAttractionDTO.ProductId;
            int quantity = kioskAttractionDTO.Quantity;
            string message = string.Empty;
            List<Transaction.Transaction.ComboCardProduct> cardProductList = null;
            List<KeyValuePair<int, int>> CategoryProductList = null;
            List<KeyValuePair<int, PurchasedProducts>> purcProductList = null;
            List<AttractionBooking> atbList = null;
            List<Transaction.Transaction.ComboManualProduct> comboManualProductsList = null;
            List<List<AttractionBooking>> masterList = GenerateATBList(kioskAttractionDTO);
            int lineIndex = (kioskTrx != null && kioskTrx.TrxLines != null ? kioskTrx.TrxLines.Count - 1 : 0);
            int i = 0;
            while (quantity > 0)
            {
                atbList = masterList[i];
                Transaction.Transaction.TransactionLine parentTrxLine = null;
                int flag = kioskTrx.CreateComboProduct(ProductId: productId, Price: -1, Quantity: 1, message: ref message, ComboParentLine: parentTrxLine,
                                        CardProducts: cardProductList, CategoryProducts: CategoryProductList,
                                        CreateChildLines: true, isGroupMeal: false, comboProductId: -1,
                                        parentTrxLine: null, purchasedProductList: purcProductList,
                                        atbList: atbList, checkInDTO: null, customCheckInDetailDTOList: null,
                                        comboManualProductsList: comboManualProductsList);
                if (flag != 0)
                {
                    log.Error(message);
                    KioskStatic.logToFile("Error creating combo attraction transaction line: " + message);
                    throw new Exception(message);
                }
                else
                {
                    quantity--;
                }
                i++;
            }
            int k = (lineIndex == 0 ? 0 : lineIndex + 1);
            for (int j = k; j < kioskTrx.TrxLines.Count; j++)
            {
                trxLineEntry.Add(kioskTrx.TrxLines[j]);
            }
            log.LogMethodExit(trxLineEntry);
            return trxLineEntry;
        }

        private List<List<AttractionBooking>> GenerateATBList(KioskAttractionDTO kioskAttractionDTO)
        {
            log.LogMethodEntry(kioskAttractionDTO);
            KioskAttractionDTO cloneObject = new KioskAttractionDTO(kioskAttractionDTO);
            bool attractionIsCardSale = KioskHelper.AttractionIsOfTypeCardSale(executionContext);
            List<List<AttractionBooking>> masterList = new List<List<AttractionBooking>>();
            int comboqty = cloneObject.Quantity;
            while (comboqty > 0)
            {
                List<AttractionBooking> childList = new List<AttractionBooking>();
                foreach (KioskAttractionChildDTO item in cloneObject.ChildAttractionBookingDTOList)
                {
                    if (item.ChildProductType == ProductTypeValues.ATTRACTION)
                    {
                        int bookedQty = item.ChildProductQuantity;
                        AttractionBooking tempProd = new AttractionBooking(executionContext, item.ChildAttractionBookingDTO);
                        tempProd.cardList = item.CardList;
                        if (tempProd.cardList != null && tempProd.cardList.Any())
                        {
                            tempProd.cardNumberList = new List<string>();
                            foreach (Card cardItem in tempProd.cardList)
                            {
                                tempProd.cardNumberList.Add(cardItem.CardNumber);
                            }
                        }
                        while (bookedQty > 0)
                        {
                            AttractionBooking comboATB = new AttractionBooking(executionContext);
                            comboATB.CloneObject(tempProd, item.ChildProductQuantity);
                            comboATB.AttractionBookingDTO.AttractionProductId = tempProd.AttractionBookingDTO.AttractionProductId;
                            comboATB.AttractionBookingDTO.Identifier = item.ComboProductId;
                            if (tempProd.cardList != null && tempProd.cardList.Any())
                            {
                                comboATB.cardList = new List<Card>();
                                comboATB.cardNumberList = new List<string>();
                                for (int i = 0; i < tempProd.cardList.Count; i++)
                                {
                                    if (i < item.ChildProductQuantity)
                                    {
                                        comboATB.cardList.Add(tempProd.cardList[i]);
                                        comboATB.cardNumberList.Add(tempProd.cardList[i].CardNumber);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            try
                            {

                                if (tempProd.AttractionBookingDTO.BookedUnits == item.ChildProductQuantity)
                                {
                                    tempProd.AttractionBookingDTO.BookedUnits = 0;
                                }
                                else
                                {
                                    tempProd.AttractionBookingDTO.BookedUnits -= item.ChildProductQuantity;
                                    if (tempProd.cardList != null)
                                        tempProd.cardList.RemoveRange(0, Math.Min(item.ChildProductQuantity, comboATB.cardList.Count));
                                    if (tempProd.cardNumberList != null)
                                        tempProd.cardNumberList.RemoveRange(0, Math.Min(item.ChildProductQuantity, comboATB.cardNumberList.Count));
                                }
                                if (tempProd.AttractionBookingDTO.BookedUnits == 0)
                                {
                                    tempProd.Expire();
                                }
                                else
                                {
                                    tempProd.Save(kioskTrx.PrimaryCard == null ? -1 : kioskTrx.PrimaryCard.card_id);
                                }
                                comboATB.Save(kioskTrx.PrimaryCard == null ? -1 : kioskTrx.PrimaryCard.card_id);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                throw;
                            }
                            bookedQty = bookedQty - (item.ChildProductQuantity);
                            if (attractionIsCardSale == false)
                            {
                                comboATB.cardList = null;
                            }
                            childList.Add(comboATB);
                        }
                    }
                }
                comboqty = comboqty - 1;
                if (childList != null && childList.Any())
                {
                    masterList.Add(childList);
                }
            }
            log.LogMethodExit(masterList);
            return masterList;
        }

        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> createAttractionProduct(int productId, double price, int quantity, int parentLineId, AttractionBooking atb, List<Card> cardList, ref string message)
        {
            log.LogMethodEntry(productId, "atb");

            Transaction.Transaction.TransactionLine parentLine = new Transaction.Transaction.TransactionLine();
            int retCode = -1;
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> addedLines = new List<Transaction.Transaction.TransactionLine>();
            for (int i = 0; i < quantity; i++)
            {
                parentLine = kioskTrx.TrxLines[kioskTrx.TrxLines.Count - 1];
            }

            retCode = kioskTrx.CreateAttractionProduct(productId, price, quantity, parentLineId, atb, cardList, ref message, -1);
            if (retCode != 0)
            {
                log.Error(message);
                KioskStatic.logToFile("Error creating attraction transaction line: " + message);
                throw new Exception(message);
            }

            if (atb != null)
                atb.Expire();

            KioskStatic.logToFile(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Schedule is blocked sucessfully"));
            log.Debug("MESSAGE" + "Schedule is blocked sucessfully");

            int lineIndex = kioskTrx.TrxLines.IndexOf(parentLine);
            if (lineIndex > -1)
            {
                for (int i = lineIndex; i < kioskTrx.TrxLines.Count; i++)
                {
                    addedLines.Add(kioskTrx.TrxLines[i]);
                }
            }
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> finalSetOfLines = new List<Transaction.Transaction.TransactionLine>();
            if (addedLines != null && addedLines.Any())
            {
                finalSetOfLines = new List<Transaction.Transaction.TransactionLine>(addedLines);
            }
            log.LogMethodExit();
            return finalSetOfLines;
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
        private void ReloadTransactionObject()
        {
            log.LogMethodEntry();
            if (this.kioskTrx != null && kioskTrx.Trx_id > 0)
            {
                KioskStatic.logToFile("In ReloadTransactionObject()");
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                kioskTrx = transactionUtils.CreateTransactionFromDB(kioskTrx.Trx_id, utilities);
            }
            log.LogMethodExit();
        }
        private void CompleteRedeemTokenTransaction(Card currentCard)
        {
            log.LogMethodEntry(currentCard);
            KioskStatic.logToFile("In CompleteRedeemTokenTransaction()");
            AddRedeemTokenTrxProducts(currentCard);
            currentCard.getCardDetails(currentCard.CardNumber); // refresh
            TaskProcs tasks = new TaskProcs(KioskStatic.Utilities);
            int sourceTrxId = GetTransactionId;
            List<TransactionPaymentsDTO> paymentList = GetTransactionPaymentsDTOList;
            int tokenCount = (paymentList != null ? paymentList.Count : 0);
            double amountReceived = (double)GetTotalPaymentsReceived();
            string message = string.Empty;
            if (!tasks.exchangeTokenForCredit(currentCard, tokenCount, amountReceived, "Redeem tokens from Kiosk", ref message, sourceTrxId))
            {
                log.LogMethodExit("Error: " + message);
                ValidationException ve = new ValidationException(message);
                throw ve;
            }
            log.LogMethodExit();
        }

        private void AddRedeemTokenTrxProducts(Card currentCard)
        {
            log.LogMethodEntry(currentCard);
            KioskStatic.logToFile("In AddRedeemTokenTrxProducts()");
            double amountReceived = (double)GetTotalPaymentsReceived();
            try
            {
                if (currentCard != null && currentCard.CardStatus.Equals("NEW"))
                {
                    AddCardDepositProduct(utilities.ParafaitEnv.CardDepositProductId, 0, currentCard);
                }
                //REDEEMTOKENPRODUCTNAME
                ProductsContainerDTO productsContainerDTO = null;
                productsContainerDTO = GetRedeemTokenProduct();
                int genericSaleProductForRedeemToken = productsContainerDTO.ProductId;
                //AddManualProduct(genericSaleProductForRedeemToken, amountReceived, 1);
                AddGenericProduct(genericSaleProductForRedeemToken, amountReceived, 1);
                MarkTransactionAsComplete();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in AddRedeemTokenTrxProducts: " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        private ProductsContainerDTO GetRedeemTokenProduct()
        {
            log.LogMethodEntry();
            ProductsContainerDTO productsContainerDTO = null;
            List<ProductsContainerDTO> productsContainerDTOList = ProductsContainerList.GetSystemProductsContainerDTOList(executionContext.GetSiteId());
            if (productsContainerDTOList != null && productsContainerDTOList.Any())
            {
                productsContainerDTO = productsContainerDTOList.Find(p => p.ProductName == REDEEMTOKENPRODUCTNAME);
            }
            if (productsContainerDTO == null)
            {
                string msg = MessageContainerList.GetMessage(executionContext, 4498, REDEEMTOKENPRODUCTNAME);
                //Please check the product setup for &1
                ValidationException validationException = new ValidationException(msg);
                log.Error(validationException);
                throw validationException;
            }
            log.LogMethodExit();
            return productsContainerDTO;
        }

        /// <summary>
        /// Add Generic Product
        /// </summary>
        public void AddGenericProduct(int productId, double amount, int quantity)
        {
            log.LogMethodEntry(productId, amount);
            KioskStatic.logToFile("In AddGenericProduct: " + productId.ToString());
            //TransactionIsNotNull();
            ProductsContainerDTO containerDTO = ProductsContainerList.GetSystemProductContainerDTO(executionContext.SiteId, productId);
            if (containerDTO.ProductType != ProductTypeValues.GENERICSALE)
            {
                ValidationException validationException = new ValidationException(MessageContainerList.GetMessage(executionContext, 4442) + " " + containerDTO.ProductType);
                //Invalid product type.
                log.Error("Error while Adding generic sale product", validationException);
                throw validationException;
            }
            //ClearOldActiveLinesForNoCartTransaction(); //Cancells previously added lines when cart option is disabled
            while (quantity > 0)
            {
                string msg = string.Empty;
                int returnValue = kioskTrx.createTransactionLine(null, productId, amount, 1, ref msg);
                if (returnValue != 0)
                {
                    ValidationException validationException = new ValidationException(msg);
                    log.Error("Error in AddGenericProduct", validationException);
                    throw validationException;
                }
                quantity--;
            }
            kioskTrx.SetServiceCharges(null);
            kioskTrx.SetAutoGratuityAmount(null);
            log.LogMethodExit();
        }

        private void GenerateRedeemTokenReceipt(KioskStatic.receipt_format rc, bool generateReceipt)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In GenerateRedeemTokenReceipt()");
            bool printed = false;
            bool emailed = false;
            try
            {
                if (receciptDeliveryMode == KioskReceiptDeliveryMode.PRINT)
                {
                    System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                    List<string> printString = new List<string>();
                    printString.Add(rc.head);
                    if (!string.IsNullOrEmpty(rc.a1))
                    {
                        printString.Add(rc.a1.Replace("@Date", DateTime.Now.ToString("ddd, dd-MMM-yyyy h:mm tt")));
                    }

                    if (!string.IsNullOrEmpty(rc.a21))
                    {
                        printString.Add(rc.a21.Replace("@POS", executionContext.POSMachineName));
                    }

                    printString.Add(Environment.NewLine);
                    printString.Add(Environment.NewLine);
                    printString.Add("*******************");
                    printString.Add(Environment.NewLine);
                    printString.Add(MessageContainerList.GetMessage(executionContext, 797)); //"Redeem Successful";
                    printString.Add(Environment.NewLine);
                    printString.Add("*******************");
                    printString.Add("*******************");
                    printString.Add(Environment.NewLine);

                    List<TransactionPaymentsDTO> paymentsDTOList = GetTransactionPaymentsDTOList;
                    int count = (paymentsDTOList != null ? paymentsDTOList.Count : 0);
                    printString.Add(MessageContainerList.GetMessage(executionContext, "Total Tokens inserted") + ": " + count.ToString(NUMBERFORMAT));
                    printString.Add(MessageContainerList.GetMessage(executionContext, "Points Loaded") + ": " + GetTotalPaymentsReceived().ToString(NUMBERFORMAT));
                    printString.Add(Environment.NewLine);
                    printString.Add(Environment.NewLine);
                    printString.Add(MessageContainerList.GetMessage(executionContext, 499));

                    pd.PrintPage += (sender, e) => PrintUSB(e, printString);
                    pd.Print();
                    printed = true;
                    SendKioskProgressUpdates(MessageContainerList.GetMessage(executionContext, 498));
                    //PLEASE COLLECT THE RECEIPT.
                }
                else if (receciptDeliveryMode == KioskReceiptDeliveryMode.EMAIL)
                {

                    emailed = EmailRedeemTokenTransactionReceipt(generateReceipt);
                    SendKioskProgressUpdates(MessageContainerList.GetMessage(executionContext, 5013));
                    //Transaction receipt is emailed to you
                }
                else
                {
                    string msg = MessageContainerList.GetMessage(executionContext, "Receipt Delivery Mode is not set");
                    SendKioskProgressUpdates(msg);
                    KioskStatic.logToFile("Error in GenerateRedeemTokenReceipt:" + msg);
                }
                SendKioskShowThankYou(printed, emailed);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected Error while Generating Redeem Token Receipt: " + ex.Message);
                string actionType = MessageContainerList.GetMessage(executionContext, "Generating Redeem Token Receipt");
                string msg = MessageContainerList.GetMessage(executionContext, 5078, actionType, ex.Message); //Unexpected error occured while &1. Error : &2
                this.SendKioskPopupAlerts(msg);
            }
            log.LogMethodExit();
        }

        private void AddKioskActivityLogEntries()
        {
            log.LogMethodEntry();
            Semnox.Parafait.Transaction.Transaction.TransactionLine firstLine = GetFirstNonDepositAndFnDTransactionLine;
            string msg = string.Empty;
            Card card = GetTransactionPrimaryCard;
            string cardNumber = (card != null ? card.CardNumber : string.Empty);
            switch (selectedProductType)
            {
                case NEWCARDTYPE:
                    msg = (firstLine != null ? firstLine.ProductName : NEWCARDMSG);
                    KioskStatic.UpdateKioskActivityLog(executionContext, NEWCARD, msg, cardNumber, GetTransactionId, GetGlobalKioskTrxId);
                    break;
                case RECHAREGETYPE:
                    msg = (firstLine != null ? firstLine.ProductName : TOPUPMSG);
                    KioskStatic.UpdateKioskActivityLog(executionContext, TOPUP, msg, cardNumber, GetTransactionId, GetGlobalKioskTrxId);
                    break;
                case CHECKINCHECKOUTTYPE:
                    msg = (firstLine != null ? firstLine.ProductName : CHECKINMSG);
                    KioskStatic.UpdateKioskActivityLog(executionContext, CHECKIN, msg, cardNumber, GetTransactionId, GetGlobalKioskTrxId);
                    break;
                case ATTRACTIONSTYPE:
                    msg = (firstLine != null ? firstLine.ProductName : ATTRACTIONMSG);
                    KioskStatic.UpdateKioskActivityLog(executionContext, ATTRACTION, msg, cardNumber, GetTransactionId, GetGlobalKioskTrxId);
                    break;
                default:
                    msg = (firstLine != null ? firstLine.ProductName : KIOSKSALEMSG);
                    KioskStatic.UpdateKioskActivityLog(executionContext, KIOSKSALE, msg, cardNumber, GetTransactionId, GetGlobalKioskTrxId);
                    break;
            }
            log.LogMethodExit();
        }

        private KioskAttractionDTO GenerateAttractionCardsForSingleProduct(KioskAttractionDTO kioskAttrcationDTO, int qty, ProductsContainerDTO pContainer)
        {
            log.LogMethodEntry(kioskAttrcationDTO, qty, "pContainer");
            if (kioskAttrcationDTO.CardList == null)
            {
                kioskAttrcationDTO.CardList = new List<Card>();
            }
            if (pContainer.LoadToSingleCard)
            {
                string cardNUmber = GetNewCardNumber(pContainer);
                Card card = new Transaction.Card(cardNUmber, executionContext.GetUserId(), utilities);

                while (qty > 0)
                {
                    kioskAttrcationDTO.CardList.Add(card);
                    qty--;
                }
            }
            else
            {
                while (qty > 0)
                {
                    string cardNUmber = GetNewCardNumber(pContainer);
                    Card card = new Transaction.Card(cardNUmber, executionContext.GetUserId(), utilities);
                    kioskAttrcationDTO.CardList.Add(card);
                    qty--;
                }
            }
            log.LogMethodExit(kioskAttrcationDTO);
            return kioskAttrcationDTO;
        }

        private KioskAttractionDTO GenerateAttractionCardsForCombCHilds(KioskAttractionDTO kioskAttrcationDTO, int qty, ProductsContainerDTO pContainer)
        {
            log.LogMethodEntry(kioskAttrcationDTO, qty, "pContainer");
            if (pContainer.LoadToSingleCard)
            {
                string cardNUmber = GetNewCardNumber(pContainer);
                Card card = new Transaction.Card(cardNUmber, executionContext.GetUserId(), utilities);
                foreach (KioskAttractionChildDTO item in kioskAttrcationDTO.ChildAttractionBookingDTOList)
                {
                    if (item.ChildProductType == ProductTypeValues.ATTRACTION)
                    {
                        if (item.CardList == null)
                        {
                            item.CardList = new List<Card>();
                        }
                        int childqty = item.ChildProductQuantity * qty;
                        while (childqty > 0)
                        {
                            item.CardList.Add(card);
                            childqty--;
                        }
                    }
                }
            }
            else
            {
                while (qty > 0)
                {
                    foreach (KioskAttractionChildDTO item in kioskAttrcationDTO.ChildAttractionBookingDTOList)
                    {
                        if (item.ChildProductType == ProductTypeValues.ATTRACTION)
                        {
                            ProductsContainerDTO pChildContainer = ProductsContainerList.GetProductsContainerDTO(executionContext, item.ChildProductId);
                            if (item.CardList == null)
                            {
                                item.CardList = new List<Card>();
                            }
                            int childqty = item.ChildProductQuantity;
                            if (pChildContainer.LoadToSingleCard)
                            {
                                string cardNUmber = GetNewCardNumber(pChildContainer);
                                Card card = new Transaction.Card(cardNUmber, executionContext.GetUserId(), utilities);
                                while (childqty > 0)
                                {
                                    item.CardList.Add(card);
                                    childqty--;
                                }
                            }
                            else
                            {
                                while (childqty > 0)
                                {
                                    string cardNUmber = GetNewCardNumber(pChildContainer);
                                    Card card = new Transaction.Card(cardNUmber, executionContext.GetUserId(), utilities);
                                    item.CardList.Add(card);
                                    childqty--;
                                }
                            }
                        }
                    }
                    qty--;
                }

            }
            log.LogMethodExit(kioskAttrcationDTO);
            return kioskAttrcationDTO;
        }

        /// <summary>
        /// productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool IsWaiverSigningRequiredForTheProduct(int productId)
        {
            log.LogMethodEntry(productId, "kioskTransaction");
            bool isSignReq = kioskTrx.IsWaiverSigningIsRequiredForTheProduct(productId);
            log.LogMethodExit(isSignReq);
            return isSignReq;
        }

        /// <summary>
        /// GetCountOfParticipantsRequireWiaverSigning
        /// </summary> 
        /// <returns></returns>
        public int GetCountOfParticipantsRequireWiaverSigning(int productId)
        {
            log.LogMethodEntry(productId);
            int quantity = kioskTrx.GetCountOfParticipantsRequireWiaverSigning(productId);
            log.LogMethodExit(quantity);
            return quantity;
        }

        /// <summary>
        /// GetTrxDate
        /// </summary>
        /// <returns></returns>
        public DateTime GetTrxDate()
        {
            log.LogMethodEntry();
            DateTime trxDatevalue = kioskTrx.GetTrxDate();
            log.LogMethodExit(trxDatevalue);
            return trxDatevalue;
        }

        /// <summary>
        /// HasActiveAttendeeList
        /// </summary>
        /// <returns></returns>
        public bool HasActiveAttendeeList()
        {
            log.LogMethodEntry();
            bool hasList = kioskTrx.HasActiveAttendeeList();
            log.LogMethodExit(hasList);
            return hasList;
        }

        /// <summary>
        /// WaiverRequiredLines
        /// </summary>
        /// <returns></returns>
        public List<Semnox.Parafait.Transaction.Transaction.TransactionLine> WaiverRequiredLines()
        {
            log.LogMethodEntry();
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> waiverRequiredLines = kioskTrx.WaiverRequiredLines();
            log.LogMethodExit(waiverRequiredLines);
            return waiverRequiredLines;
        }

        /// <summary>
        /// GetAttendeeCustomerIds
        /// </summary>
        /// <returns></returns>
        public List<int> GetAttendeeCustomerIds()
        {
            log.LogMethodEntry();
            List<int> customerIdList = kioskTrx.GetAttendeeCustomerIds();
            log.LogMethodExit(customerIdList);
            return customerIdList;
        }

        /// <summary>
        /// GetMappedCustomerIdListForWaiver
        /// </summary>
        /// <returns></returns>
        public List<int> GetMappedCustomerIdListForWaiver()
        {
            log.LogMethodEntry();
            List<int> custIdList = kioskTrx.GetMappedCustomerIdListForWaiver();
            log.LogMethodExit(custIdList);
            return custIdList;
        }

        /// <summary>
        /// GetWaiverPendingLinesForTheProduct
        /// </summary>
        /// <returns></returns>
        public List<Semnox.Parafait.Transaction.Transaction.TransactionLine> GetWaiverPendingLinesForTheProduct(int productId)
        {
            log.LogMethodEntry();
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> waiverPendingLinesOftheProduct = kioskTrx.GetWaiverPendingLinesForTheProduct(productId);
            log.LogMethodExit(waiverPendingLinesOftheProduct);
            return waiverPendingLinesOftheProduct;
        }

        /// <summary>
        /// ValidateLicenseTypeInWaiver
        /// </summary>
        /// <param name="trxLineEntry"></param>
        /// <param name="selectedCustomerDTO"></param>
        /// <returns></returns>
        public bool ValidateLicenseTypeInWaiver(Semnox.Parafait.Transaction.Transaction.TransactionLine trxLineEntry, CustomerDTO selectedCustomerDTO)// return bool and calling function should take action
        {
            log.LogMethodEntry();
            bool validLicenseType = kioskTrx.ValidateLicenseTypeInWaiver(trxLineEntry, selectedCustomerDTO);
            log.LogMethodExit(validLicenseType);
            return validLicenseType;
        }

        /// <summary>
        /// LinkCustomerToTheLineCard
        /// </summary>
        /// <param name="line"></param>
        /// <param name="customerId"></param>
        public void LinkCustomerToTheLineCard(Semnox.Parafait.Transaction.Transaction.TransactionLine line, int customerId)
        {
            log.LogMethodEntry(line, customerId);
            if (/*pContainer.AutoGenerateCardNumber == "N" &&*/
                line.LineValid == true &&
                line.card != null &&
                line.card.customer_id == -1)
            {
                line.card.customer_id = customerId;
                AccountBL accountBL = new AccountBL(KioskStatic.Utilities.ExecutionContext, line.card.card_id);
                string msg = string.Empty;
                if (accountBL != null && accountBL.AccountDTO != null && accountBL.CanAccountLinkToCustomer(ref msg) == true)
                {
                    accountBL.AccountDTO.CustomerId = customerId;
                    accountBL.Save(null);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Over ride Pending Waivers
        /// </summary>
        /// <param name="mgrId"></param>
        /// <param name="approvalTime"></param>
        public void SaveWaiversSignedViaExecuteOnline(int lineIndex, CustomerDTO customerDTO)
        {
            log.LogMethodEntry(lineIndex, customerDTO);
            if (kioskTrx.TrxLines != null)
            {
                for (int i = 0; i < kioskTrx.TrxLines[lineIndex].WaiverSignedDTOList.Count; i++)
                {
                    if (kioskTrx.TrxLines[lineIndex].WaiverSignedDTOList[i].CustomerSignedWaiverId == -1
                        && kioskTrx.TrxLines[lineIndex].WaiverSignedDTOList[i].IsActive)
                    {
                        kioskTrx.TrxLines[lineIndex].WaiverSignedDTOList[i].IsActive = false;
                        kioskTrx.TrxLines[lineIndex].WaiverSignedDTOList[i].IsOverriden = true;
                        if (kioskTrx.Trx_id > 0 && kioskTrx.TrxLines[lineIndex].DBLineId > 0)
                        {
                            SaveWaiverSignedData(kioskTrx.Trx_id, kioskTrx.TrxLines[lineIndex]);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// ShowAlertForCustomerSignedWaiverExpiryDate
        /// </summary>
        /// <param name="mappedCustomerDTO"></param>
        /// <param name="lineId"></param>
        public void ShowAlertForCustomerSignedWaiverExpiryDate(CustomerDTO mappedCustomerDTO, int lineId)
        {
            log.LogMethodEntry(lineId);

            List<ValidationError> validationErrorList = kioskTrx.ValidateCustomerSignedWaiverExpiryDate(mappedCustomerDTO, lineId);
            if (validationErrorList != null && validationErrorList.Any())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Expiry Date"), validationErrorList);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// SaveWaiverSignedData
        /// </summary>
        /// <param name="TrxId"></param>
        /// <param name="inTrxLine"></param>
        /// <param name="SQLTrx"></param>
        private void SaveWaiverSignedData(int TrxId, Transaction.Transaction.TransactionLine inTrxLine)
        {
            log.LogMethodEntry(TrxId, inTrxLine);

            int unSignedWaiverCount = 0;
            foreach (var signedDTO in inTrxLine.WaiverSignedDTOList)
            {
                if (signedDTO.CustomerSignedWaiverId == -1 && signedDTO.IsActive)
                {
                    unSignedWaiverCount++;
                }

                if (signedDTO.TrxId != TrxId)
                {
                    signedDTO.TrxId = TrxId;
                }
                if (signedDTO.LineId != inTrxLine.DBLineId)
                {
                    signedDTO.LineId = inTrxLine.DBLineId;
                }
                if (signedDTO.Site_id != (KioskStatic.Utilities.ParafaitEnv.IsCorporate ? KioskStatic.Utilities.ParafaitEnv.SiteId : -1))
                {
                    signedDTO.Site_id = KioskStatic.Utilities.ParafaitEnv.IsCorporate ? KioskStatic.Utilities.ParafaitEnv.SiteId : -1;
                }
                WaiverSignatureBL waiverSignedBL = new WaiverSignatureBL(KioskStatic.Utilities.ExecutionContext, signedDTO);
                waiverSignedBL.Save();
            }
            if (inTrxLine.lineApprovals != null && inTrxLine.lineApprovals.Any())
            {
                ApprovalAction overrideWaiver = inTrxLine.GetApproval(ApprovalAction.ApprovalActionType.OVERRIDE_WAIVER.ToString());
                if (overrideWaiver != null && overrideWaiver.ApprovalTime != null)//&& string.IsNullOrEmpty(overrideWaiver.ApproverId) == false)
                {
                    kioskTrx.InsertTrxLogs(TrxId, inTrxLine.DBLineId, KioskStatic.Utilities.ParafaitEnv.LoginID, ApprovalAction.ApprovalActionType.OVERRIDE_WAIVER.ToString(), "Override Waiver", null, overrideWaiver.ApproverId, overrideWaiver.ApprovalTime);
                    inTrxLine.lineApprovals.Remove(ApprovalAction.ApprovalActionType.OVERRIDE_WAIVER.ToString());
                }
                ApprovalAction resetOverrideWaiver = inTrxLine.GetApproval(ApprovalAction.ApprovalActionType.RESET_OVERRIDEN_WAIVER.ToString());
                if (resetOverrideWaiver != null && resetOverrideWaiver.ApprovalTime != null)
                {
                    kioskTrx.InsertTrxLogs(TrxId, inTrxLine.DBLineId, KioskStatic.Utilities.ParafaitEnv.LoginID, ApprovalAction.ApprovalActionType.OVERRIDE_WAIVER.ToString(), "Reset Overriden Waiver", null, resetOverrideWaiver.ApproverId, resetOverrideWaiver.ApprovalTime);
                    inTrxLine.lineApprovals.Remove(ApprovalAction.ApprovalActionType.RESET_OVERRIDEN_WAIVER.ToString());
                }
            }

            log.LogMethodExit();
        }
        #endregion
    }
}
