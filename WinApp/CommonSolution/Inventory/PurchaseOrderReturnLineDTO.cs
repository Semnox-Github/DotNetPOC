/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of PurchaseOrderReturn_Line
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        23-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the PurchaseOrderReturnLineDTO data object class. This acts as data holder for the PurchaseOrderReturnLine business object
    /// </summary>
    public class PurchaseOrderReturnLineDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by   PURCHASE ORDER RETURN LINE ID field
            /// </summary>
            PURCHASE_ORD_RET_LINE_ID,
            /// <summary>
            /// Search by PURCHASE ORDER ID field
            /// </summary>
            PURCHASE_ORDER_ID,
            /// <summary>
            /// Search by  PRODUCT ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int purchaseOrderReturnLineId;
        private int purchaseOrderId;
        private int productId;
        private string vendorItemCode;
        private decimal quantity;
        private decimal unitPrice;
        private decimal subTotal;
        private DateTime timestamp;
        private string remarks;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PurchaseOrderReturnLineDTO()
        {
            log.LogMethodEntry();
            purchaseOrderReturnLineId = -1;
            purchaseOrderId = -1;
            productId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public PurchaseOrderReturnLineDTO(int purchaseOrderReturnLineId, int purchaseOrderId, int productId, string vendorItemCode,
                                          decimal quantity, decimal unitPrice, decimal subTotal, DateTime timestamp, string remarks)

            :this()
        {
            log.LogMethodEntry(purchaseOrderReturnLineId, purchaseOrderId, productId, vendorItemCode, quantity, unitPrice,
                               subTotal, timestamp, remarks);
            this.purchaseOrderReturnLineId = purchaseOrderReturnLineId;
            this.purchaseOrderId = purchaseOrderId;
            this.productId = productId;
            this.vendorItemCode = vendorItemCode;
            this.quantity = quantity;
            this.unitPrice = unitPrice;
            this.subTotal = subTotal;
            this.timestamp = timestamp;
            this.remarks = remarks;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public PurchaseOrderReturnLineDTO(int purchaseOrderReturnLineId,int purchaseOrderId,int productId,string vendorItemCode,
                                          decimal quantity,decimal unitPrice,decimal subTotal, DateTime timestamp,string remarks,
                                          string guid, int siteId, bool synchStatus, int masterEntityId,DateTime lastUpdatedDate,
                                          string lastUpdatedBy, string createdBy, DateTime creationDate)
            :this(purchaseOrderReturnLineId, purchaseOrderId, productId, vendorItemCode, quantity, unitPrice,
                               subTotal, timestamp, remarks)
        {
            log.LogMethodEntry(purchaseOrderReturnLineId, purchaseOrderId, productId, vendorItemCode,  quantity, unitPrice,
                               subTotal, timestamp, remarks, guid,siteId,synchStatus, masterEntityId,lastUpdatedDate,
                               lastUpdatedBy, createdBy, creationDate);
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PurchaseOrderReturnLineId field
        /// </summary>
        public int PurchaseOrderReturnLineId
        {
            get { return purchaseOrderReturnLineId; }
            set { purchaseOrderReturnLineId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PurchaseOrderId field
        /// </summary>
        public int PurchaseOrderId
        {
            get { return purchaseOrderId; }
            set { purchaseOrderId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId
        {
            get { return productId; }
            set { productId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the VendorItemCode field
        /// </summary>
        public string VendorItemCode
        {
            get { return vendorItemCode; }
            set { vendorItemCode = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the UnitPrice field
        /// </summary>
        public decimal UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SubTotal field
        /// </summary>
        public decimal SubTotal
        {
            get { return subTotal; }
            set { subTotal = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Timestamp field
        /// </summary>
        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the AllowedTimeInMinutes field
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value;  }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value;  }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value;  }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value;  }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value;  }
        }
        
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || purchaseOrderReturnLineId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
