/********************************************************************************************
 * Project Name -Inventory Adjustments DTO
 * Description  -Data object of InventoryAdjustments
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
  *1.00        12-Aug-2016    Amaresh          Created 
  *1.10        16-May-2017    Lakshminarayana  Modified   Added new serach filters.
  *2.70.2      13-JUl-2019    Deeksha          Modifications as per three tier standard
  *2.100.0     27-Jul-2020    Deeksha          Modified :Added UOMId field
 ********************************************************************************************/
using System;
using Semnox.Parafait.logging;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the inventory Adjustments data object class. This acts as data holder for the inventory Adjustments object
    /// </summary>
    public class InventoryAdjustmentsDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByInventoryAdjustmentsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryAdjustmentsParameters
        {
            /// <summary>
            /// Search by ADJUSTMENT ID field
            /// </summary>
            ADJUSTMENT_ID,
            /// <summary>
            /// Search by FROM_LOCATION ID field
            /// </summary>
            FROM_LOCATION_ID ,
            /// <summary>
            /// Search by TO LOCATION ID field
            /// </summary>
            TO_LOCATION_ID ,
            /// <summary>
            /// Search by ADJUSTMENT TYPE field
            /// </summary>
            ADJUSTMENT_TYPE ,
            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID ,
            /// <summary>
            /// Search by LOT ID field
            /// </summary>
            LOT_ID ,
            /// <summary>
            /// Search by DOCUMENT TYPE ID field
            /// </summary>
            DOCUMENT_TYPE_ID ,
            /// <summary>
            /// Search by ADJUSTMENT TYPE ID field
            /// </summary>
            ADJUSTMENT_TYPE_ID ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by BULKUPDATED field
            /// </summary>
            BULK_UPDATED ,
            /// <summary>
            /// Search by FROM TIMESTAMP field
            /// </summary
            FROM_TIMESTAMP ,
            /// <summary>
            /// Search by TO TIMESTAMP field
            /// </summary>
            TO_TIMESTAMP,
             /// <summary>
             /// Search by TO MASTER ENTITY ID field
             /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by TO UOM ID field
            /// </summary>
            UOM_ID
        }

        private double adjustmentId;
        private string adjustmentType;
        private double adjustmentQuantity;
        private int fromLocationId;
        private int toLocationId;
        private string remarks;
        private int productId;
        private DateTime timestamp;
        private string userId;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private string sourceSystemID;
        private int adjustmentTypeId;
        private int masterEntityId;
        private int lotID;
        private double price;
        private int documentTypeID;
        private bool bulkUpdated;
        private int originalReferenceId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int uomId;
        private int purchaseOrderReceiveLineId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public InventoryAdjustmentsDTO()
        {
            log.LogMethodEntry();
            adjustmentId = -1;
            adjustmentQuantity = 0;
            fromLocationId = -1;
            toLocationId = -1;
            productId = -1;
            siteId = -1;
            adjustmentTypeId = -1;
            masterEntityId = -1;
            lotID = -1;
            documentTypeID = -1;
            price = 0;
            bulkUpdated = false;
            originalReferenceId = -1;
            uomId = -1;
            purchaseOrderReceiveLineId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public InventoryAdjustmentsDTO(double adjustmentId, string adjustmentType, double adjustmentQuantity,
                                        int fromLocationId, int toLocationId,string remarks, int productId,
                                        DateTime timestamp, string userId,string sourceSystemID, int adjustmentTypeId, 
                                        int lotID, double price, int documentTypeID, bool bulkUpdated,int originalReferenceId,int uomId, int purchaseOrderReceiveLineId)
            : this()
        {
            log.LogMethodEntry(adjustmentId, adjustmentType, adjustmentQuantity, fromLocationId, toLocationId, remarks,
                                productId,timestamp, userId, sourceSystemID, adjustmentTypeId, lotID, price, 
                                documentTypeID, bulkUpdated,originalReferenceId, uomId, purchaseOrderReceiveLineId);
            this.adjustmentId = adjustmentId;
            this.adjustmentType = adjustmentType;
            this.adjustmentQuantity = adjustmentQuantity;
            this.fromLocationId = fromLocationId;
            this.toLocationId = toLocationId;
            this.remarks = remarks;
            this.productId = productId;
            this.timestamp = timestamp;
            this.userId = userId;
            this.sourceSystemID = sourceSystemID;
            this.adjustmentTypeId = adjustmentTypeId;
            this.lotID = lotID;
            this.price = price;
            this.documentTypeID = documentTypeID;
            this.bulkUpdated = bulkUpdated;
            this.originalReferenceId = originalReferenceId;
            this.uomId = uomId;
            this.purchaseOrderReceiveLineId = purchaseOrderReceiveLineId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public InventoryAdjustmentsDTO(double adjustmentId, string adjustmentType, double adjustmentQuantity, int fromLocationId, int toLocationId,
                                       string remarks, int productId, DateTime timestamp, string userId, int siteId, string guid, bool synchStatus,
                                       string sourceSystemID, int adjustmentTypeId, int masterEntityId, int lotID, double price, int documentTypeID, bool bulkUpdated,
                                       int originalReferenceId,string createdBy,DateTime creationDate,string lastUpdatedBy,
                                       DateTime lastUpdateDate,int uomId, int purchaseOrderReceiveLineId)
           : this(adjustmentId, adjustmentType, adjustmentQuantity, fromLocationId, toLocationId, remarks, productId,
                 timestamp, userId, sourceSystemID, adjustmentTypeId, lotID, price, documentTypeID, bulkUpdated, 
                 originalReferenceId, uomId, purchaseOrderReceiveLineId)
        {
            log.LogMethodEntry(adjustmentId, adjustmentType, adjustmentQuantity, fromLocationId, toLocationId, remarks, 
                                productId,timestamp, userId, siteId,guid,synchStatus, sourceSystemID, adjustmentTypeId,
                                masterEntityId, lotID, price, documentTypeID,bulkUpdated, originalReferenceId,createdBy,
                                creationDate,lastUpdatedBy,lastUpdateDate, uomId, purchaseOrderReceiveLineId);
           
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.siteId = siteId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the AdjustmentId field
        /// </summary>
        [DisplayName("Adjustment Id")]
        [ReadOnly(true)]
        public double AdjustmentId
        {
            get
            {
                return adjustmentId;
            }
            set
            {
                adjustmentId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the AdjustmentType field
        /// </summary>
        [DisplayName("Adjustment Type")]
        public string AdjustmentType
        {
            get
            {
                return adjustmentType;
            }
            set
            {
                adjustmentType = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the AdjustmentQuantity field
        /// </summary>        
        [DisplayName("Adjustment Quantity")]
        public Double AdjustmentQuantity
        {
            get
            {
                return adjustmentQuantity;
            }
            set
            {
                adjustmentQuantity = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the FromLocationId field
        /// </summary>        
        [DisplayName("From Location Id")]
        public int FromLocationId
        {
            get
            {
                return fromLocationId;
            }
            set
            {
                fromLocationId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ToLocationId field
        /// </summary>        
        [DisplayName("To Location Id")]
        public int ToLocationId
        {
            get
            {
                return toLocationId;
            }
            set
            {
                toLocationId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>        
        [DisplayName("Remarks")]
        public string Remarks
        {
            get
            {
                return remarks;
            }
            set
            {
                remarks = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>        
        [DisplayName("Product Id")]
        public int ProductId
        {
            get
            {
                return productId;
            }
            set
            {
                productId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Timestamp field
        /// </summary>        
        [DisplayName("Timestamp")]
        public DateTime Timestamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                timestamp = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>        
        [DisplayName("User Id")]
        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                siteId = value;
             
            }
        }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                synchStatus = value;
             
            }
        }

        /// <summary>
        /// Get/Set method of the SourceSystemID field
        /// </summary>
        [DisplayName("Source System ID")]
        public string SourceSystemID
        {
            get
            {
                return sourceSystemID;
            }
            set
            {
                sourceSystemID = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the AdjustmentTypeId field
        /// </summary>
        [DisplayName("Adjustment Type Id")]
        public int AdjustmentTypeId
        {
            get
            {
                return adjustmentTypeId;
            }
            set
            {
                adjustmentTypeId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                masterEntityId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the LotID field
        /// </summary>        
        [DisplayName("Lot ID")]
        public int LotID
        {
            get
            {
                return lotID;
            }
            set
            {
                lotID = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>        
        [DisplayName("Price")]
        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the DocumentTypeID field
        /// </summary>        
        [DisplayName("Document Type ID")]
        public int DocumentTypeID
        {
            get
            {
                return documentTypeID;
            }
            set
            {
                documentTypeID = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the BulkUpdated field
        /// </summary>
        [DisplayName("BulkUpdated")]
        public bool BulkUpdated
        {
            get
            {
                return bulkUpdated;
            }
            set
            {
                bulkUpdated = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the OriginalReferenceId field
        /// </summary>        
        [DisplayName("OriginalReferenceId")]
        public int OriginalReferenceId
        {
            get
            {
                return originalReferenceId;
            }
            set
            {
                originalReferenceId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the UOMId field
        /// </summary>        
        [DisplayName("UOMId")]
        public int UOMId
        {
            get
            {
                return uomId;
            }
            set
            {
                uomId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the purchaseOrderReceiveLineId field
        /// </summary>        
        [DisplayName("purchaseOrderReceiveLineId")]
        public int PurchaseOrderReceiveLineId
        {
            get
            {
                return purchaseOrderReceiveLineId;
            }
            set
            {
                purchaseOrderReceiveLineId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate
        {
            get
                {
                    return lastUpdateDate;
                }
            set
                {
                    lastUpdateDate = value;
                }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
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
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || adjustmentId < 0;
                }
            }

            set
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    if(!Boolean.Equals(notifyingObjectIsChanged, value))
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
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
