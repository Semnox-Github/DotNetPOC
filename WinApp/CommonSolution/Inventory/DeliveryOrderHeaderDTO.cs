/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of DeliveryOrderHeader
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70     27-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the DeliveryOrderHeader data object class. This acts as data holder for the DeliveryOrderHeader business object
    /// </summary>
    public class DeliveryOrderHeaderDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  DELIVERY ORDER ID field
            /// </summary>
           DELIVERY_ORDER_ID,
            /// <summary>
            /// Search by   DELIVERY ORDER DATE field
            /// </summary>
            DELIVERY_ORDER_DATE,
            /// <summary>
            /// Search by DELIVERY ORDER NUMBER field
            /// </summary>
            DELIVERY_ORDER_NUMBER,
            /// <summary>
            /// Search by  PURCHASE ORDER ID field
            /// </summary>
            PURCHASE_ORDER_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int deliveryOrderId;
        private DateTime deliveryOrderDate;
        private string deliveryOrderNumber;
        private char type;
        private int purchaseOrderId;
        private string issuer;
        private string deliverTo;
        private string status;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private DateTime receivedDate;
        private string receivedBy;
        private string grn;
        private string remarks;
        private string createdBy;
        private DateTime creationDate;
        private List<DeliveryOrderLineDTO> deliveryOrderLineDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DeliveryOrderHeaderDTO()
        {
            log.LogMethodEntry();
            siteId = -1;
            masterEntityId = -1;
            deliveryOrderId = -1;
            purchaseOrderId = -1;
            deliveryOrderLineDTOList = new List<DeliveryOrderLineDTO>();
            log.LogMethodExit();

        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public DeliveryOrderHeaderDTO(int deliveryOrderId, DateTime deliveryOrderDate, string deliveryOrderNumber,
                                        char type, int purchaseOrderId, string issuer, string deliverTo, string status,
                                        DateTime receivedDate,string receivedBy, string grn, string remarks)
            :this()
        {
            log.LogMethodEntry(deliveryOrderId, deliveryOrderDate, deliveryOrderNumber, type, purchaseOrderId, issuer,
                               deliverTo, status, receivedDate, receivedBy, grn, remarks);
            this.deliveryOrderId = deliveryOrderId;
            this.deliveryOrderDate = deliveryOrderDate;
            this.deliveryOrderNumber = deliveryOrderNumber;
            this.type = type;
            this.purchaseOrderId = purchaseOrderId;
            this.issuer = issuer;
            this.deliverTo = deliverTo;
            this.status = status;
            this.receivedDate = receivedDate;
            this.receivedBy = receivedBy;
            this.grn = grn;
            this.remarks = remarks;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public DeliveryOrderHeaderDTO(  int deliveryOrderId, DateTime deliveryOrderDate, string deliveryOrderNumber,
                                        char type,int purchaseOrderId, string issuer, string deliverTo,string status,
                                        int siteId,string guid, bool synchStatus, int masterEntityId, DateTime receivedDate,
                                        string receivedBy,string grn,string remarks, string createdBy,DateTime creationDate,
                                        DateTime lastUpdatedDate, string lastUpdatedBy)
            :this(deliveryOrderId, deliveryOrderDate, deliveryOrderNumber, type, purchaseOrderId, issuer,
                               deliverTo, status, receivedDate, receivedBy, grn, remarks)
        {
            log.LogMethodEntry(deliveryOrderId, deliveryOrderDate, deliveryOrderNumber,type, purchaseOrderId, issuer,
                               deliverTo, status,  siteId, guid, synchStatus, masterEntityId,
                               receivedDate,receivedBy, grn, remarks, createdBy, creationDate, lastUpdatedDate, lastUpdatedBy);
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
        /// Get/Set method of the DeliveryOrderId  field
        /// </summary>
        public int DeliveryOrderId
        {
            get { return deliveryOrderId; }
            set { deliveryOrderId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the DeliveryOrderDate field
        /// </summary>
        public DateTime DeliveryOrderDate
        {
            get { return deliveryOrderDate; }
            set { deliveryOrderDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the DeliveryOrderNumber field
        /// </summary>
        public string DeliveryOrderNumber
        {
            get { return deliveryOrderNumber; }
            set { deliveryOrderNumber = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        public char Type
        {
            get { return type; }
            set { type = value; this.IsChanged = true; }
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
        /// Get/Set method of the Issuer field
        /// </summary>
        public string Issuer
        {
            get { return issuer; }
            set { issuer = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the DeliverTo field
        /// </summary>
        public string DeliverTo
        {
            get { return deliverTo; }
            set { deliverTo = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ReceivedDate field
        /// </summary>
        public DateTime ReceivedDate
        {
            get { return receivedDate; }
            set { receivedDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ReceivedBy field
        /// </summary>
        public string ReceivedBy
        {
            get { return receivedBy; }
            set { receivedBy = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the GRN field
        /// </summary>
        public string GRN
        {
            get { return grn; }
            set { grn = value; this.IsChanged = true; }
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
            set { synchStatus = value; }
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
        /// Get/Set method of the DeliveryOrderLineDTOList field
        /// </summary>
        public List<DeliveryOrderLineDTO> DeliveryOrderLineDTOList
        {
            get { return deliveryOrderLineDTOList; }
            set { deliveryOrderLineDTOList = value; }
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
                    return notifyingObjectIsChanged || deliveryOrderId < 0;
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
        /// Returns whether the DeliveryOrderHeaderDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (deliveryOrderLineDTOList != null &&
                  deliveryOrderLineDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
