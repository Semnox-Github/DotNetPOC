using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// /// <summary>
    /// This is the product Summary data object class. This acts as data holder for the product Summary business object
    /// </summary>
    /// </summary>
    public class ProductSummaryDTO
    {

         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        int productId;
        string code;
        string description;
        double cost;
        double lastPurchasePrice;
        int taxId;
        double taxAmount;
        double quantity;
        string taxInclusiveCost;
        double lowerLimitCost;
        double upperLimitCost;
        double costVariancePercentage;
        int expiryDays;
        int defaultLocationId;
        bool lotControlled;
        bool marketListItem;
        string expiryType;
        int uomId;
        double taxPercentage;
        double subtotal;
        double cpoSubtotal;
        int requisitionId;
        int requisitionLineId;
        int requestedQnty;
        DateTime expectedReceiptDate;
        double unitPrice;
        int purchaseOrderLineId;
        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductSummaryDTO()
        {
            log.Debug("Starts-ProductSummaryDTO() default constructor.");
            productId = -1;
            defaultLocationId = -1;
            uomId = -1;
            taxId = -1;
            lotControlled = false;
            marketListItem = false;
            expiryType = "N";
            expiryDays = 0;
            log.Debug("Ends-ProductSummaryDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductSummaryDTO(int productId, string code, string description,  int defaultLocationId, int uomId, double cost,
                                    int taxId, double taxAmount, string taxInclusiveCost, double lowerLimitCost, double upperLimitCost, 
                                    double costVariancePercentage, bool lotControlled, bool marketListItem, string expiryType, double taxPercentage,
                                    double subtotal, double cpoSubtotal, int expiryDays, int requisitionId, int requisitionLineId,
                                    int requestedQnty, DateTime expectedReceiptDate, double quantity, double unitPrice, int purchaseOrderLineId)
        {
            log.Debug("Starts-ProductDTO(with all the data fields) Parameterized constructor.");
            this.productId = productId;
            this.code = code;
            this.description = description;
            this.defaultLocationId = defaultLocationId;
            this.uomId = uomId;
            this.cost = cost;
            this.taxId = taxId;
            this.taxAmount = taxAmount;
            this.taxInclusiveCost = taxInclusiveCost;
            this.lowerLimitCost = lowerLimitCost;
            this.upperLimitCost = upperLimitCost;
            this.costVariancePercentage = costVariancePercentage;
            this.lotControlled = lotControlled;
            this.marketListItem = marketListItem;
            this.expiryType = expiryType;
            this.taxPercentage = taxPercentage;
            this.subtotal = subtotal;
            this.cpoSubtotal = cpoSubtotal;
            this.expiryDays = expiryDays;
            this.requisitionId = requisitionId;
            this.requisitionLineId = requisitionLineId;
            this.requestedQnty = requestedQnty;
            this.expectedReceiptDate = expectedReceiptDate;
            this.quantity = quantity;
            this.unitPrice = unitPrice;
            this.purchaseOrderLineId = purchaseOrderLineId;
            log.Debug("Ends-ProductDTO(with all the data fields) Parameterized constructor.");
        }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("Product Id")]
        [ReadOnly(true)]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Code field
        /// </summary>
        [DisplayName("Code")]
        public string Code { get { return code; } set { code = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DefaultLocationId field
        /// </summary>
        [DisplayName("Default Location Id")]
        public int DefaultLocationId { get { return defaultLocationId; } set { defaultLocationId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UomId field
        /// </summary>
        [DisplayName("UomId")]
        public int UomId { get { return uomId; } set { uomId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Cost field
        /// </summary>
        [DisplayName("Cost")]
        public double Cost { get { return cost; } set { cost = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Cost field
        /// </summary>
        [DisplayName("TaxAmount")]
        public double TaxAmount { get { return taxAmount; } set { taxAmount = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaxPercentage field
        /// </summary>
        [DisplayName("TaxPercentage")]
        public double TaxPercentage { get { return taxPercentage; } set { taxPercentage = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Cost field
        /// </summary>
        [DisplayName("CPOSubTotal")]
        public double CPOSubTotal { get { return cpoSubtotal; } set { cpoSubtotal = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Cost field
        /// </summary>
        [DisplayName("SubTotal")]
        public double SubTotal { get { return subtotal; } set { subtotal = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastPurchasePrice field
        /// </summary>
        [DisplayName("Last Purchase Price")]
        public double LastPurchasePrice { get { return lastPurchasePrice; } set { lastPurchasePrice = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaxId field
        /// </summary>
        [DisplayName("TaxId")]
        public int TaxId { get { return taxId; } set { taxId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaxInclusiveCost field
        /// </summary>
        [DisplayName("Tax Inclusive Cost")]
        public string TaxInclusiveCost { get { return taxInclusiveCost; } set { taxInclusiveCost = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LowerLimitCost field
        /// </summary>
        [DisplayName("Lower Limit Cost")]
        public double LowerLimitCost { get { return lowerLimitCost; } set { lowerLimitCost = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UpperLimitCost field
        /// </summary>
        [DisplayName("Upper Limit Cost")]
        public double UpperLimitCost { get { return upperLimitCost; } set { upperLimitCost = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CostVariancePercentage field
        /// </summary>
        [DisplayName("Cost Variance Percentage")]
        public double CostVariancePercentage { get { return costVariancePercentage; } set { costVariancePercentage = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the lotControlled field
        /// </summary>
        [DisplayName("LotControlled")]
        public bool LotControlled { get { return lotControlled; } set { lotControlled = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MarketListItem field
        /// </summary>
        [DisplayName("MarketListItem")]
        public bool MarketListItem { get { return marketListItem; } set { marketListItem = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the expiryType field
        /// </summary>
        [DisplayName("ExpiryType")]
        public string ExpiryType { get { return expiryType; } set { expiryType = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ExpiryDays field
        /// </summary>
        [DisplayName("Expiry Days")]
        public int ExpiryDays { get { return expiryDays; } set { expiryDays = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Requisition Id fields
        /// </summary>
        [DisplayName("RequisitionId")]
        public int RequisitionId { get { return requisitionId; } set { requisitionId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Requisition Line Id fields
        /// </summary>
        [DisplayName("Requisition Line Id")]
        public int RequisitionLineId { get { return requisitionLineId; } set { requisitionLineId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FromDepartment fields
        /// </summary>
        [DisplayName("Requested Quantity")]
        public int RequestedQuantity { get { return requestedQnty; } set { requestedQnty = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RequiredByDate fields
        /// </summary>
        [DisplayName("Required By Date")]
        public DateTime RequiredByDate { get { return expectedReceiptDate; } set { expectedReceiptDate = value; IsChanged = true; } }
        /// <summary>
        /// Get method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public double Quantity { get { return quantity; } set { quantity = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the UnitPrice field
        /// </summary>
        [DisplayName("UnitPrice")]
        public double UnitPrice { get { return unitPrice; } set { unitPrice = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the PurchaseOrderLineId field
        /// </summary>
        [DisplayName("PurchaseOrderLineId")]
        public int PurchaseOrderLineId { get { return purchaseOrderLineId; } set { purchaseOrderLineId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || productId < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }
    }
}
