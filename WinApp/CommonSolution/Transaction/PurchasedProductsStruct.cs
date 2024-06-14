/********************************************************************************************
 * Project Name - LinkedPurchaseProductsStruct DTO
 * Description  - Data object of LinkedPurchaseProductsStruct 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70        22-Apr-2019   Guru S A          Moved the class from Product project to transaction project
 *2.70.2        12-Aug-2019   Deeksha           Added logger methods.
 *2.130.7       20-Jun-2022     Muaaz           Added min Combo child product quantity detials
 ********************************************************************************************/
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Transaction
{    
    /// <summary>
    /// This is the PurchasedProductsStruct data object class. This acts as data holder for the PurchasedProductsStruct business object
    /// </summary>
    public class PurchasedProductsStruct : ProductsStruct
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int quantity;
        private double amount;
        private string cardNumber;
        private int cardId;
        private string isPackage;
        private int puchaseReferenceId;
        private double taxAmount;
        private string remarks;
        //Added additional parameters to get the customer id in a transaction
        private int customerId;
        //Added AttractionScheduleId and ScheduleDate to enable Attraction Booking//
        private int attractionScheduleId;
        private DateTime scheduleDate;
        private DateTime scheduleToDate;
        private List<AttractionBookingDTO> attractionBookingList = new List<AttractionBookingDTO>();
        private string comboProductType;
        private int minComboProdQty;


        //Added the below parameter to update EntitlementReferenceDate based on the visit date  on Apr-6-2016//
        DateTime visitDate;

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public PurchasedProductsStruct()
            : base()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public PurchasedProductsStruct(int productId, string description)
            : base(productId, description)
        {
            log.LogMethodEntry(productId, description);
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public PurchasedProductsStruct(int productId, int quantity, int cardId) : base(productId)
        {
            log.LogMethodEntry(productId, quantity, cardId);
            this.quantity = quantity;
            this.cardId = cardId;
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public PurchasedProductsStruct(int productId, int quantity, int cardId, int purchaseLineId)
            : this(productId, quantity, cardId)
        {
            log.LogMethodEntry(productId, quantity, cardId, purchaseLineId);
            this.puchaseReferenceId = purchaseLineId;
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public PurchasedProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, int purchaseLineId, int quantity, double purchaseAmount, double taxAmount, int cardId, string cardNumber)
            : base(productId, productName, productDescription, productType, displayGroup)
        {
            log.LogMethodEntry(productId, productName, productDescription, productType, displayGroup, purchaseLineId, quantity, purchaseAmount, taxAmount, cardId, cardNumber);
            this.quantity = quantity;
            this.amount = purchaseAmount;
            this.taxAmount = taxAmount;
            this.cardId = cardId;
            this.cardNumber = cardNumber;
            this.puchaseReferenceId = purchaseLineId;
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public PurchasedProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, int quantity, double purchaseAmount, double taxAmount, string isPackage)
            : base(productId, productName, productDescription, productType, displayGroup)
        {
            log.LogMethodEntry(productId, productName, productDescription, productType, displayGroup, quantity, purchaseAmount, taxAmount, isPackage);
            this.quantity = quantity;
            this.amount = purchaseAmount;
            this.taxAmount = taxAmount;
            this.isPackage = isPackage;
            this.minComboProdQty = quantity > 0 ? quantity : 0;
            log.LogMethodExit();
        }


        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added AttractionScheduleId and ScheduleDate to enable Attraction Booking//
        public PurchasedProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, int quantity, double purchaseAmount, double taxAmount, string isPackage, int attractionScheduleId, DateTime scheduleDate, DateTime scheduleToDate)
            : base(productId, productName, productDescription, productType, displayGroup)
        {
            log.LogMethodEntry(productId, productName, productDescription, productType, displayGroup, quantity, purchaseAmount, taxAmount, isPackage, attractionScheduleId, scheduleDate, scheduleToDate);
            this.quantity = quantity;
            this.amount = purchaseAmount;
            this.taxAmount = taxAmount;
            this.isPackage = isPackage;
            this.attractionScheduleId = attractionScheduleId;
            this.scheduleDate = scheduleDate;
            this.scheduleToDate = scheduleToDate;
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added AttractionScheduleId and ScheduleDate to enable Attraction Booking && Visit Date May-17-2016//
        public PurchasedProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, int quantity, double purchaseAmount, double taxAmount, string isPackage, int attractionScheduleId, DateTime scheduleDate, DateTime visitDate, DateTime scheduleToDate)
            : base(productId, productName, productDescription, productType, displayGroup)
        {
            log.LogMethodEntry(productId, productName, productDescription, productType, displayGroup, quantity, purchaseAmount, taxAmount, isPackage, attractionScheduleId, scheduleDate, visitDate, scheduleToDate);
            this.quantity = quantity;
            this.amount = purchaseAmount;
            this.taxAmount = taxAmount;
            this.isPackage = isPackage;
            this.attractionScheduleId = attractionScheduleId;
            this.scheduleDate = scheduleDate;
            this.visitDate = visitDate;
            this.scheduleToDate = scheduleToDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// AddAttractionBooking method to add  AttractionBookingsDTO object to list.
        /// </summary>
        /// <param name="attractionBookingDTO">AttractionBookingsDTO attractionBookingsDTO</param>
        public void AddAttractionBooking(AttractionBookingDTO attractionBookingDTO)
        {
            log.LogMethodEntry(attractionBookingDTO);
            attractionBookingList.Add(attractionBookingDTO);
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the ProductQuantity field
        /// </summary>
        public int ProductQuantity { get { return quantity; } set { quantity = value; } }

        /// <summary>
        /// Get/Set method of the MinComboProductQuantity field
        /// </summary>
        public int MinComboProdQty { get { return minComboProdQty; } set { minComboProdQty = value; } }


        /// <summary>
        /// Get/Set method of the ProductQuantity field
        /// </summary>
        public double Amount { get { return amount; } set { amount = value; } }

        /// <summary>
        /// Get/Set method of the ProductQuantity field
        /// </summary>
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; } }


        /// <summary>
        /// Get/Set method of the ProductQuantity field
        /// </summary>
        [DefaultValue(-1)]
        public int CardId { get { return cardId; } set { cardId = value; } }


        /// <summary>
        /// Get/Set method of the ProductQuantity field
        /// </summary>
        [DefaultValue(-1)]
        public int PurchaseLineId { get { return puchaseReferenceId; } set { puchaseReferenceId = value; } }


        /// <summary>
        /// Get/Set method of the ProductQuantity field
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; } }

        /// <summary>
        /// Get/Set method of the ProductQuantity field
        /// </summary>
        public double TaxAmount { get { return taxAmount; } set { taxAmount = value; } }

        /// <summary>
        /// Get/Set method of the ProductQuantity field
        /// </summary>
        //Added to get the transaction details based on the transaction id
        [DefaultValue(-1)]
        public int CustomerId { get { return customerId; } set { customerId = value; } }

        /// <summary>
        /// Get/Set method of the ProductQuantity field
        /// </summary>
        //Added AttractionScheduleId and ScheduleDate to enable Attraction Booking//
        [DefaultValue(-1)]
        public int AttractionScheduleId { get { return attractionScheduleId; } set { attractionScheduleId = value; } }

        /// <summary>
        /// Get/Set method of the ScheduleDate field
        /// </summary>
        public DateTime ScheduleDate { get { return scheduleDate; } set { scheduleDate = value; } }

        /// <summary>
        /// Get/Set method of the scheduleToDate field
        /// </summary>
        public DateTime ScheduleToDate { get { return scheduleToDate; } set { scheduleToDate = value; } }
        

        /// <summary>
        /// Get/Set method of the ProductQuantity field
        /// </summary>
        // Added new variable remarks to assign visit date to EntitlementReferenceDate on on Apr-6-2016//
        [System.ComponentModel.DefaultValue(typeof(DateTime), "")]
        public DateTime VisitDate { get { return visitDate; } set { visitDate = value; } }

        /// <summary>
        ///   Get/Set method of the AttractionBooking field
        /// </summary>
        public List<AttractionBookingDTO> AttractionBookingList { get { return attractionBookingList; } set { attractionBookingList = value; } }

        /// <summary>
        /// Get/Set method of the ComboProductType field
        /// </summary>
        public string ComboProductType { get { return comboProductType; } set { comboProductType = value; } }
    }

    /// <summary>
    /// This is the LinkedPurchaseProductStruct data object class. This acts as data holder for the product business object
    /// </summary>
    public class LinkedPurchaseProductsStruct : PurchasedProductsStruct
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int linkLineId;
        private string transactionGuid;
        private int trxLineId;
        private bool isGroupedCombo;
        /// <summary>
        /// Default Constructor 
        /// </summary>
        public LinkedPurchaseProductsStruct() : base()
        {
            log.LogMethodEntry();
            this.linkLineId = -1;
            this.transactionGuid = "";
            this.trxLineId = -1;
            this.isGroupedCombo = false;
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public LinkedPurchaseProductsStruct(int productId, string description)
            : base(productId, description)
        {
            log.LogMethodEntry(productId, description);
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public LinkedPurchaseProductsStruct(int productId, int quantity, int linkLineId, int cardId) : base(productId, quantity, cardId)
        {
            log.LogMethodEntry(productId, quantity, linkLineId, cardId);
            this.linkLineId = linkLineId;
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public LinkedPurchaseProductsStruct(int productId, int quantity, int linkLineId, int cardId, int purchaseLineId)
            : base(productId, quantity, cardId, purchaseLineId)
        {
            log.LogMethodEntry(productId, quantity, linkLineId, cardId, purchaseLineId);
            this.linkLineId = linkLineId;
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public LinkedPurchaseProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, int purchaseLineId, int quantity, double purchaseAmount, double taxAmount, int linkLineId, int cardId, string cardNumber, string guid)
            : base(productId, productName, productDescription, productType, displayGroup, purchaseLineId, quantity, purchaseAmount, taxAmount, cardId, cardNumber)
        {
            log.LogMethodEntry(productId, productName, productDescription, productType, displayGroup, purchaseLineId, quantity, purchaseAmount, taxAmount, linkLineId, "cardId", "cardNumber", guid);
            this.linkLineId = linkLineId;
            Guid = guid;
            log.LogMethodExit();
        }
        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public LinkedPurchaseProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, int quantity, double purchaseAmount, double taxAmount, string isPackage, int linkLineId)
            : base(productId, productName, productDescription, productType, displayGroup, quantity, purchaseAmount, taxAmount, isPackage)
        {
            log.LogMethodEntry(productId, productName, productDescription, productType, displayGroup, quantity, purchaseAmount, taxAmount, isPackage, linkLineId);
            this.linkLineId = linkLineId;
            log.LogMethodExit();
        }
        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        //Added a parameter remarks to save Names of customers as line remarks on June 18,2015//
        public LinkedPurchaseProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, int quantity, double purchaseAmount, double taxAmount, string isPackage, int linkLineId, string remarks, string guid)
            : base(productId, productName, productDescription, productType, displayGroup, quantity, purchaseAmount, taxAmount, isPackage)
        {
            log.LogMethodEntry(productId, productName, productDescription, productType, displayGroup, quantity, purchaseAmount, taxAmount, isPackage, linkLineId, remarks, guid);
            this.linkLineId = linkLineId;
            Guid = guid;
            this.Remarks = remarks;
            log.LogMethodExit();
        }
        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        ///added rakshith
        public LinkedPurchaseProductsStruct(int productId, string productName, string productDescription, string productType, string displayGroup, int quantity, double purchaseAmount, double taxAmount, string isPackage, int linkLineId, string remarks, string guid, string quantityPrompt)
            : base(productId, productName, productDescription, productType, displayGroup, quantity, purchaseAmount, taxAmount, isPackage)
        {
            log.LogMethodEntry(productId, productName, productDescription, productType, displayGroup, quantity, purchaseAmount, taxAmount, isPackage, linkLineId, remarks, guid, quantityPrompt);
            this.linkLineId = linkLineId;
            Guid = guid;
            this.Remarks = remarks;
            this.QuantityPrompt = quantityPrompt;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LinkLineId field
        /// </summary>
        [DisplayName("LinkLineId")]
        [DefaultValue(-1)]
        public int LinkLineId { get { return linkLineId; } set { linkLineId = value; } }

        /// <summary>
        /// Get/Set method of the TransactionGuid field
        /// </summary>
        [DisplayName("TransactionGuid")]
        public string TransactionGuid { get { return transactionGuid; } set { transactionGuid = value; } }


        /// <summary>
        /// Get/Set method of the TrxLineId field
        /// </summary>
        [DisplayName("TrxLineId")]
        public int TrxLineId { get { return trxLineId; } set { trxLineId = value; } }


        /// <summary>
        /// Get/Set method of the IsGroupedCombo field
        /// </summary>
        [DisplayName("IsGroupedCombo")]
        public bool IsGroupedCombo { get { return isGroupedCombo; } set { isGroupedCombo = value; } }


  }


    /// <summary>
    /// LinkedPurchasedProducts class. 
    /// </summary>
    public class LinkedPurchasedProducts : PurchasedProducts
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string productLineIdentifier;
        private string linkedLineIdentifier;
        private int attractionScheduleId;
        private DateTime scheduleDate;
        private DateTime scheduleToDate;
        private string quantityPrompt;
        List<AttractionBookingDTO> attractionBookingList = new List<AttractionBookingDTO>();
        private string rowGuid;
        private string transactionGuid;
        private int categoryId;


        /// <summary>
        /// Default Constructor 
        /// </summary>
        public LinkedPurchasedProducts()
        {
            log.LogMethodEntry();
            this.attractionScheduleId = -1;
            this.rowGuid = "";
            this.transactionGuid = "";
            this.categoryId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public LinkedPurchasedProducts(int productId)
            : base(productId)
        {
            log.LogMethodEntry(productId);
            this.productLineIdentifier = "";
            this.linkedLineIdentifier = "";
            this.attractionScheduleId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public LinkedPurchasedProducts(int productId, string productDesc, int purchasedLineId, int productQuantity, double purchaseAmount, double taxAmount, string productLineIdentifier, string linkedLineIdentifier)
            : base(productId, productDesc, purchasedLineId, productQuantity, purchaseAmount, taxAmount)
        {
            log.LogMethodEntry(productId, productDesc,  purchasedLineId,  productQuantity,  purchaseAmount,  taxAmount,  productLineIdentifier,  linkedLineIdentifier);
            this.productLineIdentifier = productLineIdentifier;
            this.linkedLineIdentifier = linkedLineIdentifier;
            log.LogMethodExit();
        }
        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public LinkedPurchasedProducts(int productId, string productDesc, int purchasedLineId, int productQuantity, double purchaseAmount, double taxAmount, int purchaseCardId, string purchaseCardNumber, string productLineIdentifier, string linkedLineIdentifier, string remarks)
            : base(productId, productDesc, purchasedLineId, productQuantity, purchaseAmount, taxAmount, purchaseCardId, purchaseCardNumber, remarks)
        {
            log.LogMethodEntry(productId, productDesc, purchasedLineId, productQuantity, purchaseAmount, taxAmount, purchaseCardId, purchaseCardNumber, productLineIdentifier, linkedLineIdentifier, remarks);
            this.productLineIdentifier = productLineIdentifier;
            this.linkedLineIdentifier = linkedLineIdentifier;
            log.LogMethodExit();
        }

        /// <summary>
        ///   Constructor With parameter quantityPrompt 
        /// </summary>
        public LinkedPurchasedProducts(int productId, string productDesc, int purchasedLineId, int productQuantity, double purchaseAmount, double taxAmount, int purchaseCardId, string purchaseCardNumber, string productLineIdentifier, string linkedLineIdentifier, string remarks, string quantityPrompt)
            : base(productId, productDesc, purchasedLineId, productQuantity, purchaseAmount, taxAmount, purchaseCardId, purchaseCardNumber, remarks)
        {
            log.LogMethodEntry(productId, productDesc, purchasedLineId, productQuantity, purchaseAmount, taxAmount, purchaseCardId, purchaseCardNumber, productLineIdentifier, linkedLineIdentifier, remarks, quantityPrompt);
            this.productLineIdentifier = productLineIdentifier;
            this.linkedLineIdentifier = linkedLineIdentifier;
            this.quantityPrompt = quantityPrompt;
            log.LogMethodExit();
        }

        /// <summary>
        /// AddAttractionBooking method to add  AttractionBookingsDTO object to list.
        /// </summary>
        /// <param name="attractionBookingsDTO">AttractionBookingsDTO attractionBookingsDTO</param>
        public void AddAttractionBooking(AttractionBookingDTO attractionBookingsDTO)
        {
            log.LogMethodEntry(attractionBookingsDTO);
            attractionBookingList.Add(attractionBookingsDTO);
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the ProductLineIdentifier field
        /// </summary>
        [DisplayName("QuantityPrompt")]
        public string QuantityPrompt { get { return quantityPrompt; } set { quantityPrompt = value; } }


        /// <summary>
        /// Get/Set method of the CategoryId field
        /// </summary>
        [DisplayName("CategoryId")]
        public int CategoryId { get { return categoryId; } set { categoryId = value; } }


        /// <summary>
        /// Get/Set method of the ProductLineIdentifier field
        /// </summary>
        [DisplayName("ProductLineIdentifier")]
        public string ProductLineIdentifier { get { return productLineIdentifier; } set { productLineIdentifier = value; } }

        /// <summary>
        /// Get/Set method of the LinkedLineIdentifier field
        /// </summary>
        [DisplayName("LinkedLineIdentifier")]
        public string LinkedLineIdentifier { get { return linkedLineIdentifier; } set { linkedLineIdentifier = value; } }


        /// <summary>
        /// Get/Set method of the AttractionScheduleId field
        /// </summary>
        [DisplayName("AttractionScheduleId")]
        public int AttractionScheduleId { get { return attractionScheduleId; } set { attractionScheduleId = value; } }


        /// <summary>
        /// Get/Set method of the ScheduleDate field
        /// </summary>
        [DisplayName("ScheduleDate")]
        public DateTime ScheduleDate { get { return scheduleDate; } set { scheduleDate = value; } }

        /// <summary>
        /// Get/Set method of the scheduleToDate field
        /// </summary>
        [DisplayName("ScheduleToDate")]
        public DateTime ScheduleToDate { get { return scheduleToDate; } set { scheduleToDate = value; } }
        

        /// <summary>
        ///   Get/Set method of the AttractionBookingsList field
        /// </summary>
        public List<AttractionBookingDTO> AttractionBookingList { get { return attractionBookingList; } set { attractionBookingList = value; } }

        /// <summary>
        ///   Get/Set method of the RowGuid field
        /// </summary>
        public string RowGuid { get { return rowGuid; } set { rowGuid = value; } }


        /// <summary>
        /// Get/Set method of the TransactionGuid field
        /// </summary>
        [DisplayName("TransactionGuid")]
        public string TransactionGuid { get { return transactionGuid; } set { transactionGuid = value; } }
    }

}
