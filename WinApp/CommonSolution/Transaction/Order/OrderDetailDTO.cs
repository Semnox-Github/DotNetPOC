/********************************************************************************************
 * Project Name - Transactions                                                                       
 * Description  - OrderDetailsDTO holds the delivery channel like rider details 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
 ********************************************************************************************/
using Semnox.Parafait.Transaction.Order;
using System;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This class will hold the additional information about the Orders like rider info, status etc
    /// </summary>
    public class OrderDetailDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int orderDetailId;
        private int orderId;
        private int deliveryChannelId;
        private string riderName;
        private string riderPhoneNumber;
        private int riderDeliveryStatus;
        private string remarks;
        private bool isActive;
        private string deliveryType;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        public enum SearchByParameters
        {
            /// <summary>
            /// Search By ORDER_DETAIL_ID
            /// </summary>
            ORDER_DETAIL_ID,
            /// <summary>
            /// Search By ORDER_ID
            /// </summary>
            ORDER_ID,
            /// <summary>
            /// Search By ORDER_ID_LIST
            /// </summary>
            ORDER_ID_LIST,
            /// <summary>
            /// Search By deliveryChannelId 
            /// </summary>
            DELIVERY_CHANNEL_ID,
            /// <summary>
            /// Search By RIDER_NAME 
            /// </summary>
            RIDER_NAME,
            /// <summary>
            /// Search By ACTIVE FLAG
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search By DELIVERY_TYPE 
            /// </summary>
            DELIVERY_TYPE,

            /// <summary>
            /// Search By SITE ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search By MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE
        }
        /// <summary>
        /// default constructor
        /// </summary>
        public OrderDetailDTO()
        {
            log.LogMethodEntry();
            orderId = -1;
            orderDetailId = -1;
            deliveryChannelId = -1;
            riderName = null;
            riderPhoneNumber = null;
            remarks = string.Empty;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            deliveryType = DeliveryTypes.SELF.ToString();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public OrderDetailDTO(int orderDetailId, int orderId, int deliveryChannelId,string riderName, string riderPhoneNumber,int riderDeliveryStatus,
                                               string remarks, bool isActive,string deliveryType)
    : this()
        {
            log.LogMethodEntry(orderDetailId, orderId, deliveryChannelId, riderName, riderPhoneNumber, riderDeliveryStatus,
                              remarks, isActive, deliveryType);
            this.orderDetailId = orderDetailId;
            this.orderId = orderId;
            this.deliveryChannelId = deliveryChannelId;
            this.riderName = riderName;
            this.riderPhoneNumber = riderPhoneNumber;
            this.riderDeliveryStatus = riderDeliveryStatus;
            this.remarks = remarks;
            this.isActive = isActive;
            this.deliveryType = deliveryType;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public OrderDetailDTO(OrderDetailDTO orderDetailsDTO)
        {
            log.LogMethodEntry(orderDetailsDTO);
            this.orderDetailId = orderDetailsDTO.OrderDetailId;
            this.orderId = orderDetailsDTO.OrderId;
            this.deliveryChannelId = orderDetailsDTO.DeliveryChannelId;
            this.riderPhoneNumber = orderDetailsDTO.RiderPhoneNumber;
            this.riderName = orderDetailsDTO.RiderName;
            this.riderDeliveryStatus = orderDetailsDTO.RiderDeliveryStatus;
            this.remarks = orderDetailsDTO.Remarks;
            this.isActive = orderDetailsDTO.IsActive;
            this.createdBy = orderDetailsDTO.CreatedBy;
            this.creationDate = orderDetailsDTO.CreationDate;
            this.lastUpdatedBy = orderDetailsDTO.LastUpdatedBy;
            this.lastUpdateDate = orderDetailsDTO.LastUpdateDate;
            this.siteId = orderDetailsDTO.SiteId;
            this.masterEntityId = orderDetailsDTO.MasterEntityId;
            this.synchStatus = orderDetailsDTO.SynchStatus;
            this.guid = orderDetailsDTO.Guid;
            this.deliveryType = orderDetailsDTO.DeliveryType;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with All data fields.
        /// </summary>
        public OrderDetailDTO(int orderDetailId, int orderId, int deliveryChannelId, string riderName, string riderPhoneNumber, int riderDeliveryStatus,
                              string remarks, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                              DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId, string deliveryType)
    : this(orderDetailId, orderId, deliveryChannelId, riderName, riderPhoneNumber, riderDeliveryStatus, remarks, isActive, deliveryType)
        {
            log.LogMethodEntry(orderDetailId, orderId, deliveryChannelId, riderName, riderPhoneNumber, riderDeliveryStatus, remarks, isActive, createdBy,
                                creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the OrderDetailId field
        /// </summary>
        public int OrderDetailId { get { return orderDetailId; } set { this.IsChanged = true; orderDetailId = value; } }
        /// <summary>
        /// Get/Set method of the OrderId field
        /// </summary>
        public int OrderId { get { return orderId; } set { this.IsChanged = true; orderId = value; } }
        /// <summary>
        /// Get/Set method of the deliveryChannelId field
        /// </summary>
        public int DeliveryChannelId { get { return deliveryChannelId; } set { this.IsChanged = true; deliveryChannelId = value; } }

        /// <summary>
        /// Get/Set method of the RiderName field
        /// </summary>
        public string RiderName { get { return riderName; } set { this.IsChanged = true; riderName = value; } }
        /// <summary>
        /// Get/Set method of the riderPhoneNumber field
        /// </summary>
        public string RiderPhoneNumber { get { return riderPhoneNumber; } set { this.IsChanged = true; riderPhoneNumber = value; } }
        /// <summary>
        /// Get/Set method of the riderDeliveryStatus field
        /// </summary>
        public int RiderDeliveryStatus { get { return riderDeliveryStatus; } set { this.IsChanged = true; riderDeliveryStatus = value; } }
        /// <summary>
        /// Get/Set method of the remarks field
        /// </summary>
        public string Remarks { get { return remarks; } set { this.IsChanged = true; remarks = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the deliveryType field
        /// </summary>
        public string DeliveryType { get { return deliveryType; } set { deliveryType = value; this.IsChanged = true; } }
  
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }


        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || orderDetailId < 0;
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
