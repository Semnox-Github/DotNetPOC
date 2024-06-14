/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of ReorderStockLine
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      28-May-2019    Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the ReorderStockLineDTO data object class. This acts as data holder for the ReorderStockLine business object
    /// </summary>
    public class ReorderStockLineDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by  REORDER STOCK LINE ID field
            /// </summary>
            REORDER_STOCK_LINE_ID,
            /// <summary>
            /// Search by  REORDER STOCK ID field
            /// </summary>
            REORDER_STOCK_ID,
            /// <summary>
            /// Search by  REORDER STOCK ID LIST field
            /// </summary>
            REORDER_STOCK_ID_LIST,
            /// <summary>
            /// Search by  REORDER STOCK LINE ID field
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
        private int reorderStockLineId;
        private int reorderStockId;
        private int productId;
        private decimal quantityOnHand;
        private decimal reorderPoint;
        private decimal reorderQuantity;
        private int vendorId;
        private string guid;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ReorderStockLineDTO()
        {
            log.LogMethodEntry();
            reorderStockLineId = -1;
            reorderStockId = -1;
            productId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public ReorderStockLineDTO(int reorderStockLineId, int reorderStockId, int productId, decimal quantityOnHand,
                                   decimal reorderPoint, decimal reorderQuantity, int vendorId)
            :this()
        {
            log.LogMethodEntry(reorderStockLineId, reorderStockId, productId, quantityOnHand, reorderPoint, reorderQuantity,
                              vendorId);
            this.reorderStockLineId = reorderStockLineId;
            this.reorderStockId = reorderStockId;
            this.productId = productId;
            this.quantityOnHand = quantityOnHand;
            this.reorderPoint = reorderPoint;
            this.reorderQuantity = reorderQuantity;
            this.vendorId = vendorId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public ReorderStockLineDTO(int reorderStockLineId,int reorderStockId,int productId,decimal quantityOnHand,
                                   decimal reorderPoint,decimal reorderQuantity,int vendorId,String guid, int siteId, bool synchStatus,int masterEntityId,
                                   DateTime lastUpdatedDate,string lastUpdatedBy, string createdBy,DateTime creationDate)
            :this(reorderStockLineId, reorderStockId, productId, quantityOnHand, reorderPoint, reorderQuantity,vendorId)
        {
            log.LogMethodEntry(reorderStockLineId,reorderStockId, productId,quantityOnHand,reorderPoint, reorderQuantity,
                              vendorId, guid, siteId, synchStatus, masterEntityId, lastUpdatedDate, lastUpdatedBy,
                              createdBy, creationDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the ReorderStockLineId  field
        /// </summary>
        public int ReorderStockLineId
        {
            get { return reorderStockLineId; }
            set { reorderStockLineId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ReorderStockId  field
        /// </summary>
        public int ReorderStockId
        {
            get { return reorderStockId; }
            set { reorderStockId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ProductId  field
        /// </summary>
        public int ProductId
        {
            get { return productId; }
            set { productId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the QuantityOnHand  field
        /// </summary>
        public decimal QuantityOnHand
        {
            get { return quantityOnHand; }
            set { quantityOnHand = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ReorderPoint  field
        /// </summary>
        public decimal ReorderPoint
        {
            get { return reorderPoint; }
            set { reorderPoint = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ReorderQuantity  field
        /// </summary>
        public decimal ReorderQuantity
        {
            get { return reorderQuantity; }
            set { reorderQuantity = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the VendorId  field
        /// </summary>
        public int VendorId
        {
            get { return vendorId; }
            set { vendorId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
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
            set { siteId = value;  }
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
                    return notifyingObjectIsChanged || reorderStockLineId < 0;
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
