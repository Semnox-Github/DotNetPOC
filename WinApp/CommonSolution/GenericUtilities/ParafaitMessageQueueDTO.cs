/********************************************************************************************
 * Project Name - GenericUtilities                                                                       
 * Description  - ParafaitMessageQueueDTO DTO holds the order message information
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
  *2.140.0     25-Nov-2021   Fiona Lishal      Modified : Added Searcch Parameter MESSAGE
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.GenericUtilities
{
    /// <summary>
    /// ParafaitMessageQueueDTO
    /// </summary>
    public class ParafaitMessageQueueDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                
        private int messageQueueId;
        private string entityGuid; // holds the guid of the entity
        private string entityName; // Table Name
        private string entityMessage;
        private MessageQueueStatus status;
        private bool isActive;
        private string remarks;
        private int attempts;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string actionType;

        /// <summary>
        /// 
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search By orderMessageId
            /// </summary>
            MESSAGE_QUEUE_ID,
            /// <summary>
            /// Search By entityName 
            /// </summary>
            ENTITY_NAME,
            /// <summary>
            /// Search By status 
            /// </summary>
            STATUS,
            /// <summary>
            /// Search By ENTITY_GUID 
            /// </summary>
            ENTITY_GUID,
            /// <summary>
            /// Search By ENTITY_GUID_LIST 
            /// </summary>
            ENTITY_GUID_LIST,
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
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by ACTION TYPE
            /// </summary>
            ACTION_TYPE,
            /// <summary>
            /// Search by STATUS LIST
            /// </summary>
            STATUS_LIST,
            /// <summary>
            /// Search by MESSAGE
            /// </summary>
            MESSAGE,
            /// <summary>
            /// Search by FROM_DATE
            /// </summary>
            FROM_DATE,
            /// <summary>
            /// ATTEMPTS_GREATER_THAN
            /// </summary>
            ATTEMPTS_LESS_THAN
        }

        public enum EntityNames
        {
            /// <summary>
            /// Search By Customer
            /// </summary>
            Customer,
            /// <summary>
            /// Search By Transaction
            /// </summary>
            Transaction
        }

        public enum ActionTypes
        {
            /// <summary>
            /// Search By LoadBonusDuringCustRegistration
            /// </summary>
            LoadBonusDuringCustRegistration,
            /// <summary>
            /// Search By DeleteCustomerFromBooking
            /// </summary>
            DeleteCustomerFromBooking
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public ParafaitMessageQueueDTO()
        {
            log.LogMethodEntry();
            messageQueueId = -1;
            entityGuid = string.Empty;
            entityMessage = string.Empty;
            entityName = string.Empty;
            remarks = string.Empty;
            siteId = -1;
            attempts = 0;
            masterEntityId = -1;
            isActive = true;
            actionType = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public ParafaitMessageQueueDTO(int messageQueueId, string entityGuid, string entityName,
                                               string orderMessage, MessageQueueStatus status, bool isActive, string actionType, string remarks = null, int attempts = 0)
    : this()
        {
            log.LogMethodEntry(messageQueueId, entityGuid, entityName, orderMessage, isActive, actionType, remarks, attempts);
            this.messageQueueId = messageQueueId;
            this.entityGuid = entityGuid;
            this.entityMessage = orderMessage;
            this.entityName = entityName;
            this.status = status;
            this.isActive = isActive;
            this.actionType = actionType;
            this.remarks = remarks;
            this.attempts = attempts;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public ParafaitMessageQueueDTO(ParafaitMessageQueueDTO parafaitMessageQueueDTO)
        {
            log.LogMethodEntry(parafaitMessageQueueDTO);
            this.messageQueueId = parafaitMessageQueueDTO.MessageQueueId;
            this.entityGuid = parafaitMessageQueueDTO.EntityGuid;
            this.entityMessage = parafaitMessageQueueDTO.Message;
            this.entityName = parafaitMessageQueueDTO.EntityName;
            this.status = parafaitMessageQueueDTO.Status;
            this.remarks = parafaitMessageQueueDTO.Remarks;
            this.isActive = parafaitMessageQueueDTO.IsActive;
            this.createdBy = parafaitMessageQueueDTO.CreatedBy;
            this.creationDate = parafaitMessageQueueDTO.CreationDate;
            this.lastUpdatedBy = parafaitMessageQueueDTO.LastUpdatedBy;
            this.lastUpdateDate = parafaitMessageQueueDTO.LastUpdateDate;
            this.siteId = parafaitMessageQueueDTO.SiteId;
            this.masterEntityId = parafaitMessageQueueDTO.MasterEntityId;
            this.synchStatus = parafaitMessageQueueDTO.SynchStatus;
            this.guid = parafaitMessageQueueDTO.Guid;
            this.actionType = parafaitMessageQueueDTO.ActionType;
            this.attempts = parafaitMessageQueueDTO.attempts;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with All data fields.
        /// </summary>
        public ParafaitMessageQueueDTO(int messageQueueId, string entityId, string entityName,
                                               string orderMessage, MessageQueueStatus status, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                      DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId, string actionType, string remarks, int attempts=0)
    : this(messageQueueId, entityId, entityName, orderMessage, status, isActive, actionType, remarks, attempts)
        {
            log.LogMethodEntry(messageQueueId, entityId,entityName, orderMessage, status, isActive, createdBy,
                                creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId, actionType, remarks, attempts);
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
        /// Get/Set method of the messageQueueId field
        /// </summary>
        public int MessageQueueId { get { return messageQueueId; } set { this.IsChanged = true; messageQueueId = value; } }
        /// <summary>
        /// Get/Set method of the entityId field
        /// </summary>
        public string EntityGuid { get { return entityGuid; } set { this.IsChanged = true; entityGuid = value; } }
        /// <summary>
        /// Get/Set method of the entityName field
        /// </summary>
        public string EntityName { get { return entityName; } set { this.IsChanged = true; entityName = value; } }

        /// <summary>
        /// Get/Set method of the orderMessage field
        /// </summary>
        public string Message { get { return entityMessage; } set { this.IsChanged = true; entityMessage = value; } }

        /// <summary>
        /// Get/Set method of the orderMessage field
        /// </summary>
        public MessageQueueStatus Status { get { return status; } set { this.IsChanged = true; status = value; } }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        public string Remarks { get { return remarks; } set { this.IsChanged = true; remarks = value; } }

        public int Attempts
        {
            get { return attempts; }
            set { this.IsChanged = true; attempts = value; }
        }
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
        /// Get/Set method of the ActionType field
        /// </summary>
        public string ActionType { get { return actionType; } set { this.IsChanged = true; actionType = value; } }




        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || messageQueueId < 0;
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
