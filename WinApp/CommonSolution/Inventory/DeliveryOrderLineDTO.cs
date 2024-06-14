/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of DeliveryOrderLine
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        28-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the DeliveryOrderLine data object class. This acts as data holder for the DeliveryOrderLine business object
    /// </summary>
    public class DeliveryOrderLineDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  DELIVERY ORDER LINE ID field
            /// </summary>
            DELIVERY_ORDER_LINE_ID,
            /// <summary>
            /// Search by   DELIVERY ORDER ID field
            /// </summary>
            DELIVERY_ORDER_ID,
            /// <summary>
            /// Search by   DELIVERY ORDER ID LIST field
            /// </summary>
            DELIVERY_ORDER_ID_LIST,
            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by  PURCHASE ORDER ID field
            /// </summary>
            PURCHASE_ORDER_LINE_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE
        }
        private int deliveryOrderLineId;
        private int deliveryOrderId;
        private int productId;
        private decimal deliveryQuantity;
        private decimal receivedQuantity;
        private string remarks;
        private int purchaseOrderLineId;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool isActive;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DeliveryOrderLineDTO()
        {
            log.LogMethodEntry();
            siteId = -1;
            masterEntityId = -1;
            deliveryOrderId = -1;
            purchaseOrderLineId = -1;
            productId = -1;
            deliveryOrderLineId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public DeliveryOrderLineDTO(int deliveryOrderLineId, int deliveryOrderId, int productId, decimal deliveryQuantity,
                                    decimal receivedQuantity, string remarks, int purchaseOrderLineId,bool isActive)
            :this()
        {
            log.LogMethodEntry(deliveryOrderLineId, deliveryOrderId, productId, deliveryQuantity, receivedQuantity, remarks,
                               purchaseOrderLineId, isActive);
            this.deliveryOrderLineId = deliveryOrderLineId;
            this.deliveryOrderId = deliveryOrderId;
            this.productId = productId;
            this.deliveryQuantity = deliveryQuantity;
            this.receivedQuantity = receivedQuantity;
            this.remarks = remarks;
            this.purchaseOrderLineId = purchaseOrderLineId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public DeliveryOrderLineDTO(int deliveryOrderLineId,int deliveryOrderId, int productId, decimal deliveryQuantity,
                                    decimal receivedQuantity, string remarks, int purchaseOrderLineId,bool isActive, DateTime lastUpdatedDate,
                                    string lastUpdatedBy, int siteId, string guid, bool synchStatus, int masterEntityId, 
                                    string createdBy, DateTime creationDate)
            :this(deliveryOrderLineId, deliveryOrderId, productId, deliveryQuantity, receivedQuantity, remarks, purchaseOrderLineId, isActive)
        {
            log.LogMethodEntry(deliveryOrderLineId, deliveryOrderId, productId, deliveryQuantity,receivedQuantity, remarks,
                               purchaseOrderLineId, isActive, lastUpdatedDate, lastUpdatedBy, siteId, guid,synchStatus, masterEntityId,
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
        /// Get/Set method of the DeliveryOrderLineId  field
        /// </summary>
        public int DeliveryOrderLineId
        {
            get { return deliveryOrderLineId; }
            set { deliveryOrderLineId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the DeliveryOrderId  field
        /// </summary>
        public int DeliveryOrderId
        {
            get { return deliveryOrderId; }
            set { deliveryOrderId = value; this.IsChanged = true; }
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
        /// Get/Set method of the DeliveryQuantity field
        /// </summary>
        public decimal DeliveryQuantity
        {
            get { return deliveryQuantity; }
            set { deliveryQuantity = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ReceivedQuantity field
        /// </summary>
        public decimal ReceivedQuantity
        {
            get { return receivedQuantity; }
            set { receivedQuantity = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PurchaseOrderLineId field
        /// </summary>
        public int PurchaseOrderLineId
        {
            get { return purchaseOrderLineId; }
            set { purchaseOrderLineId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Is active field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
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
            set { creationDate = value; }
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
                    return notifyingObjectIsChanged || deliveryOrderLineId < 0;
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
