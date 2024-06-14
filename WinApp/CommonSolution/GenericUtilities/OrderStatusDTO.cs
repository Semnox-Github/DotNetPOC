/********************************************************************************************
 * Project Name - Transaction                                                                       
 * Description  - OrderStatusDTO holds the status details 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.GenericUtilities
{
    /// <summary>
    /// This class is used to hold the status constants for Applicability to  Transaction/Inventory/Reservation/Orders 
    /// </summary>
    public  class OrderStatusDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int orderStatusId;
        private string orderStatus;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        /// <summary>
        /// Search paramters 
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search By ORDER_STATUS_ID
            /// </summary>
            ORDER_STATUS_ID,
            /// <summary>
            /// Search By ORDER_STATUS
            /// </summary>
            ORDER_STATUS,
            /// <summary>
            /// Search By ACTIVE FLAG
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search By SITE ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search By MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID
        }
        /// <summary>
        /// default constructor
        /// </summary>
        public OrderStatusDTO()
        {
            log.LogMethodEntry();
            orderStatusId = -1;
            orderStatus = string.Empty;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public OrderStatusDTO(int orderyStatusId, string orderStatus,bool isActive)
    : this()
        {
            log.LogMethodEntry(orderyStatusId, orderStatus, isActive);
            this.orderStatusId = orderyStatusId;
            this.orderStatus = orderStatus;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public OrderStatusDTO(OrderStatusDTO orderStatusDTO)
        {
            log.LogMethodEntry(orderStatusDTO);
            this.orderStatusId = orderStatusDTO.orderStatusId;
            this.orderStatus = orderStatusDTO.OrderStatus;
            this.isActive = orderStatusDTO.IsActive;
            this.createdBy = orderStatusDTO.CreatedBy;
            this.creationDate = orderStatusDTO.CreationDate;
            this.lastUpdatedBy = orderStatusDTO.LastUpdatedBy;
            this.lastUpdateDate = orderStatusDTO.LastUpdateDate;
            this.siteId = orderStatusDTO.SiteId;
            this.masterEntityId = orderStatusDTO.MasterEntityId;
            this.synchStatus = orderStatusDTO.SynchStatus;
            this.guid = orderStatusDTO.Guid;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with All data fields.
        /// </summary>
        public OrderStatusDTO(int orderyStatusId, string orderStatus, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                              DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId)
    : this(orderyStatusId, orderStatus, isActive)
        {
            log.LogMethodEntry(orderyStatusId, orderStatus, isActive, createdBy,
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
        /// Get/Set method of the orderyStatusId field
        /// </summary>
        public int OrderStatusId { get { return orderStatusId; } set { this.IsChanged = true; orderStatusId = value; } }
        /// <summary>
        /// Get/Set method of the orderStatus field
        /// </summary>
        public string OrderStatus { get { return orderStatus; } set { this.IsChanged = true; orderStatus = value; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

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
                    return notifyingObjectIsChanged || orderStatusId < 0;
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
