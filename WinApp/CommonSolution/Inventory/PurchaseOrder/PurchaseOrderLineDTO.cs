/********************************************************************************************
 * Project Name -PurchaseOrderLineDTO
 * Description  -Data object of asset PurchaseOrderLine
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 **********************************************************************************************
 *2.60        11-Apr-2019       Girish Kundar      Modified : Added PurchaseTaxId field and Get/Set
 *2.70.2      16-Jul-2019       Deeksha            Modified required field constructor
 *                                                 Added recursive function for List DTO and making all data field as private.
*2.100.0      27-Jul-2020       Deeksha            Modified : Added UOMId field.
  ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.Inventory
{
    public class PurchaseOrderLineDTO
    {
        private static readonly  Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByVendorParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByPurchaseOrderLineParameters
        {
            /// <summary>
            /// Search by PURCHASEORDERLINEID field
            /// </summary>
            PURCHASE_ORDER_LINE_ID,
            /// <summary>
            /// Search by PURCHASEORDERID field
            /// </summary>
            PURCHASE_ORDER_ID,
            /// <summary>
            /// Search by PRODUCTID field
            /// </summary>
            PRODUCT_ID ,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by PURCHASEORDERIDS field
            /// </summary>
            PURCHASE_ORDER_IDS,
            // <summary>
            /// Search by ORIGINAL REFERENCE GUID field
            /// </summary>
            ORIGINAL_REFERENCE_GUID,
            // <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            // <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by UOM ID TO field
            /// </summary>
            UOM_ID
        }

        private int purchaseOrderLineId;
        private int purchaseOrderId;
        private string itemCode;
        private string description;
        private double quantity;
        private double unitPrice;
        private double subTotal;
        private DateTime timestamp;
        private double taxAmount;
        private double discountPercentage;
        private DateTime requiredByDate;
        private int siteid;
        private int productId;
        private string guid;
        private bool synchStatus;
        private string IsActive;
        private DateTime cancelledDate;
        private int masterEntityId;
        private int requisitionId;
        private int requisitionLineId;
        private double unitLogisticsCost;
        private double priceInTickets;
        private string originalReferenceGUID;
        private string externalSystemReference;
        private int purchaseTaxId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int uomId;
        private List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PurchaseOrderLineDTO()
        {
            log.LogMethodEntry();
            purchaseOrderLineId = -1;
            purchaseOrderId = -1;
            siteid = -1;
            masterEntityId = -1;
            itemCode = string.Empty;
            description =string.Empty;
            quantity = 0;
            unitPrice = 0;
            subTotal = 0;
            requisitionId = -1;
            requisitionLineId = -1;
            unitLogisticsCost = -1;
            purchaseTaxId = -1;
            externalSystemReference = string.Empty;
            purchaseOrderTaxLineDTOList = new List<PurchaseOrderTaxLineDTO>();
            uomId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public PurchaseOrderLineDTO(int purchaseOrderLineId, int purchaseOrderId, string itemCode, string description, double quantity,
                                    double unitPrice, double subTotal, DateTime timestamp, double taxAmount, double discountPercentage,
                                    DateTime requiredByDate, int siteid, int productId, string guid, bool synchStatus, string IsActive,
                                    DateTime cancelledDate, int masterEntityId, int requisitionId, int requisitionLineId, double unitLogisticsCost,
                                    double priceInTickets, string originalReferenceGUID, string externalSystemReference, int purchaseTaxId, string createdBy,
                                    DateTime creationDate,string lastUpdatedBy, DateTime lastUpdateDate, int uomId)
            :this(purchaseOrderLineId,purchaseOrderId ,itemCode, description, quantity, unitPrice, subTotal, taxAmount, discountPercentage, requiredByDate,
                                     productId, cancelledDate, requisitionId, requisitionLineId, unitLogisticsCost, priceInTickets,
                                     IsActive, originalReferenceGUID, externalSystemReference, purchaseTaxId, uomId)
        {
            log.LogMethodEntry(purchaseOrderLineId, purchaseOrderId, itemCode, description, quantity, unitPrice, subTotal, timestamp,
                                taxAmount, discountPercentage, requiredByDate, siteid, productId, guid, synchStatus, IsActive,
                                cancelledDate, masterEntityId, requisitionId, requisitionLineId, unitLogisticsCost, priceInTickets,
                                originalReferenceGUID, externalSystemReference, purchaseTaxId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, uomId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.masterEntityId = masterEntityId;
            this.siteid = siteid;
            this.synchStatus = synchStatus;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields with PurchaseOrderId and Line id
        /// </summary>
        public PurchaseOrderLineDTO(int purchaseOrderLineId, int purchaseOrderId ,string itemCode, string description, double quantity, double unitPrice, double subTotal,
                                     double taxAmount, double discountPercentage, DateTime requiredByDate, int productId,
                                     DateTime cancelledDate, int requisitionId, int requisitionLineId, double unitLogisticsCost,
                                     double priceInTickets, string IsActive, string originalReferenceGUID, string externalSystemReference, int purchaseTaxId, int uomId)
            : this(itemCode, description, quantity, unitPrice, subTotal, taxAmount, discountPercentage, requiredByDate,
                                     productId, cancelledDate, requisitionId, requisitionLineId, unitLogisticsCost, priceInTickets,
                                     IsActive, originalReferenceGUID, externalSystemReference, purchaseTaxId, uomId)
        {
            log.LogMethodEntry(itemCode, description, quantity, unitPrice, subTotal, taxAmount, discountPercentage, requiredByDate,
                                     productId, cancelledDate, requisitionId, requisitionLineId, unitLogisticsCost, priceInTickets,
                                     IsActive, originalReferenceGUID, externalSystemReference, purchaseTaxId);

            this.purchaseOrderId = purchaseOrderId;
            this.purchaseOrderLineId = purchaseOrderLineId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public PurchaseOrderLineDTO(string itemCode, string description, double quantity, double unitPrice, double subTotal,
                                     double taxAmount, double discountPercentage, DateTime requiredByDate, int productId,
                                     DateTime cancelledDate, int requisitionId, int requisitionLineId, double unitLogisticsCost,
                                     double priceInTickets, string IsActive, string originalReferenceGUID, string externalSystemReference, int purchaseTaxId,int uomId)
            : this()
        {
            log.LogMethodEntry(itemCode, description, quantity, unitPrice, subTotal, taxAmount, discountPercentage, requiredByDate,
                                     productId, cancelledDate, requisitionId, requisitionLineId, unitLogisticsCost, priceInTickets, IsActive, originalReferenceGUID, externalSystemReference, uomId);

            this.itemCode = itemCode;
            this.description = description;
            this.quantity = quantity;
            this.unitPrice = unitPrice;
            this.subTotal = subTotal;
            this.taxAmount = taxAmount;
            this.discountPercentage = discountPercentage;
            this.requiredByDate = requiredByDate;
            this.productId = productId;
            this.cancelledDate = cancelledDate;
            this.requisitionId = requisitionId;
            this.requisitionLineId = requisitionLineId;
            this.unitLogisticsCost = unitLogisticsCost;
            this.priceInTickets = priceInTickets;
            this.IsActive = IsActive;
            this.originalReferenceGUID = originalReferenceGUID;
            this.externalSystemReference = externalSystemReference;
            this.purchaseTaxId = purchaseTaxId;
            this.uomId = uomId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get method of the PurchaseOrderLineId field
        /// </summary>
        [DisplayName("PurchaseOrderLineId")]
        [ReadOnly(true)]
        public int PurchaseOrderLineId { get { return purchaseOrderLineId; } set { purchaseOrderLineId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the PurchaseOrderId field
        /// </summary>
        [DisplayName("PurchaseOrderId")]
        [ReadOnly(true)]
        public int PurchaseOrderId { get { return purchaseOrderId; } set { purchaseOrderId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the RequisitionId field
        /// </summary>
        [DisplayName("RequisitionId")]
        [ReadOnly(true)]
        public int RequisitionId { get { return requisitionId; } set { requisitionId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the RequisitionLineId field
        /// </summary>
        [DisplayName("RequisitionLineId")]
        [ReadOnly(true)]
        public int RequisitionLineId { get { return requisitionLineId; } set { requisitionLineId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the ItemCode field
        /// </summary>
        [DisplayName("ItemCode")]
        public string ItemCode { get { return itemCode; } set { itemCode = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
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
        /// Get method of the SubTotal field
        /// </summary>
        [DisplayName("SubTotal")]
        public double SubTotal { get { return subTotal; } set { subTotal = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the Timestamp field
        /// </summary>
        [DisplayName("Timestamp")]
        public DateTime Timestamp { get { return timestamp; } set { timestamp = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the TaxAmount field
        /// </summary>
        [DisplayName("TaxAmount")]
        public double TaxAmount { get { return taxAmount; } set { taxAmount = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the DiscountPercentage field
        /// </summary>
        [DisplayName("DiscountPercentage")]
        public double DiscountPercentage { get { return discountPercentage; } set { discountPercentage = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the RequiredByDate field
        /// </summary>
        [DisplayName("RequiredByDate")]
        public DateTime RequiredByDate { get { return requiredByDate; } set { requiredByDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the OriginalReferenceGUID field
        /// </summary>
        [DisplayName("OriginalReferenceGUID")]
        public string OriginalReferenceGUID { get { return originalReferenceGUID; } set { originalReferenceGUID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ExternalSystemReference field
        /// </summary>
        [DisplayName("ExternalSystemReference")]
        public string ExternalSystemReference { get { return externalSystemReference; } set { externalSystemReference = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the site_id field
        /// </summary>
        [DisplayName("site_id")]
        public int site_id { get { return siteid; } set { siteid = value;  } }
        /// <summary>
        /// Get method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }
        /// <summary>
        /// Get method of the isActive field
        /// </summary>
        [DisplayName("isActive")]
        public string isActive { get { return IsActive; } set { IsActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the CancelledDate field
        /// </summary>
        [DisplayName("CancelledDate")]
        public DateTime CancelledDate { get { return cancelledDate; } set { cancelledDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the UnitLogisticsCost field
        /// </summary>
        [DisplayName("UnitLogisticsCost")]
        public double UnitLogisticsCost { get { return unitLogisticsCost; } set { unitLogisticsCost = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the PriceInTickets field
        /// </summary>
        [DisplayName("PriceInTickets")]
        public double PriceInTickets { get { return priceInTickets; } set { priceInTickets = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the PurchaseTaxId field
        /// </summary>
        [DisplayName("PurchaseTaxId")]
        public int PurchaseTaxId { get { return purchaseTaxId; } set { purchaseTaxId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the UOMId field
        /// </summary>
        [DisplayName("UOMId")]
        [ReadOnly(true)]
        public int UOMId { get { return uomId; } set { uomId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PurchaseOrderTaxLineDTOList field
        /// </summary>
        public List<PurchaseOrderTaxLineDTO> PurchaseOrderTaxLineDTOList
        {
            get { return purchaseOrderTaxLineDTOList; }
            set { purchaseOrderTaxLineDTOList = value; }
        }


        /// <summary>
        /// Get/Set method of the CreationDate fields
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy fields
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy fields
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the LastupdatedDate fields
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Returns whether the purchaseOrderLineDTO changed or any of its Lists  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (purchaseOrderTaxLineDTOList != null &&
                   purchaseOrderTaxLineDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

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
                    return notifyingObjectIsChanged || purchaseOrderLineId < 0;
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
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
