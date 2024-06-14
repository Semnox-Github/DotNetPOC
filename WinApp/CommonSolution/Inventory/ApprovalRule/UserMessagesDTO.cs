/********************************************************************************************
 * Project Name - User Messages DTO
 * Description  - Data object of UserMessages
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-OCT-2017   Raghuveera          Created 
 *2.70.2      13-Aug-2019   Deeksha             modifications as per 3 tier standards
 *2.100.0     17-Sep-2020   Deeksha             Modified Is changed property to handle unsaved records.
 ********************************************************************************************/
using Semnox.Parafait.logging;
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the User Messages data object class. This acts as data holder for the User Messages business object
    /// </summary>
    public class UserMessagesDTO
    {
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// UserMessagesStatus enum 
        /// </summary>
        public enum UserMessagesStatus
        {
            /// <summary>
            /// Initial status
            /// </summary>
            PENDING,
            /// <summary>
            /// After approval
            /// </summary>
            APPROVED,
            /// <summary>
            /// After reject
            /// </summary>
            REJECTED,
            /// <summary>
            /// After reject remaining will be updated as canceled
            /// </summary>
            CANCELLED
        }
        /// <summary>
        /// SearchByUserMessagesParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByUserMessagesParameters
        {
            /// <summary>
            /// Search by MESSAGE ID field
            /// </summary>
            MESSAGE_ID ,
            /// <summary>
            /// Search by APPROVAL RULE ID field
            /// </summary>
            APPROVAL_RULE_ID ,
            /// <summary>
            /// Search by MESSAGE TYPE field
            /// </summary>
            MESSAGE_TYPE ,
            /// <summary>
            /// Search by ROLE ID field
            /// </summary>
            ROLE_ID ,
            /// <summary>
            /// Search by USER ID field
            /// </summary>
            USER_ID ,
            /// <summary>
            /// Search by MODULE TYPE field
            /// </summary>
            MODULE_TYPE ,
            /// <summary>
            /// Search by OBJECT TYPE field
            /// </summary>
            OBJECT_TYPE ,
            /// <summary>
            /// Search by OBJECT GUID field
            /// </summary>
            OBJECT_GUID ,
            /// <summary>
            /// Search by STATUS field
            /// </summary>
            STATUS ,
            /// <summary>
            /// Search by ACTED BY USER field
            /// </summary>
            ACTED_BY_USER ,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG ,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE_TILL ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID 
        }
        private int messageID;
        private int approvalRuleID;
        private string messageType;
        private string message;
        private int roleId;
        private int userId;
        private int level;
        private string moduleType;
        private string objectType;
        private string objectGUID;
        private string status;
        private int actedByUser;
        private int masterEntityId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserMessagesDTO()
        {
            log.LogMethodEntry();
            messageID = -1;
            approvalRuleID = -1;
            roleId = -1;
            userId = -1;
            level = 0;
            actedByUser = -1;
            masterEntityId = -1;
            status = UserMessagesStatus.PENDING.ToString();
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public UserMessagesDTO(int messageID, int approvalRuleID, string messageType, string message, int roleId,
                               int userId, int level, string moduleType, string objectType, string objectGUID, string status,
                               int actedByUser,  bool isActive)
            :this( )
        {
            log.LogMethodEntry(messageID, approvalRuleID, messageType, message, roleId, userId, level, moduleType, objectType, objectGUID, status,
                    actedByUser, isActive);
            this.messageID = messageID;
            this.approvalRuleID = approvalRuleID;
            this.messageType = messageType;
            this.message = message;
            this.roleId = roleId;
            this.userId = userId;
            this.level = level;
            this.moduleType = moduleType;
            this.objectType = objectType;
            this.objectGUID = objectGUID;
            this.status = status;
            this.actedByUser = actedByUser;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public UserMessagesDTO(int messageID, int approvalRuleID, string messageType, string message, int roleId,
                               int userId, int level, string moduleType, string objectType, string objectGUID, string status,
                               int actedByUser, int masterEntityId, bool isActive, string createdBy, DateTime creationDate,
                            string lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus)
            :this(messageID, approvalRuleID, messageType, message, roleId, userId, level, moduleType, objectType, objectGUID, status,
                    actedByUser, isActive)
        {
            log.LogMethodEntry(messageID, approvalRuleID, messageType, message, roleId, userId, level, moduleType, objectType, objectGUID, status,
                    actedByUser, masterEntityId, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

       
        /// <summary>
        /// Get/Set method of the MessageId field
        /// </summary>
        [DisplayName("Message Id")]
        [ReadOnly(true)]
        public int MessageId { get { return messageID; } set { messageID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ApprovalRuleID field
        /// </summary>
        [DisplayName("Approval Rule")]
        public int ApprovalRuleID { get { return approvalRuleID; } set { approvalRuleID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Message Type field
        /// </summary>        
        [DisplayName("Message Type")]
        public string MessageType { get { return messageType; } set { messageType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Message field
        /// </summary>        
        [DisplayName("Message")]
        public string Message { get { return message; } set { message = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Role field
        /// </summary>
        [DisplayName("Role")]
        public int RoleId { get { return roleId; } set { roleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the User field
        /// </summary>
        [DisplayName("User")]
        public int UserId { get { return userId; } set { userId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Level field
        /// </summary>
        [DisplayName("Level")]
        public int Level { get { return level; } set { level = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Module Type field
        /// </summary>        
        [DisplayName("Module Type")]
        public string ModuleType { get { return moduleType; } set { moduleType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ObjectType field
        /// </summary>        
        [DisplayName("Object Type")]
        public string ObjectType { get { return objectType; } set { objectType = value; this.IsChanged = true; } }

        
        /// <summary>
        /// Get/Set method of the ObjectGUID field
        /// </summary>        
        [DisplayName("Object GUID")]
        public string ObjectGUID { get { return objectGUID; } set { objectGUID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>        
        [DisplayName("Status")]
        public string Status { get { return status; } set { status = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ActedByUser field
        /// </summary>
        [DisplayName("Acted By User")]
        public int ActedByUser { get { return actedByUser; } set { actedByUser = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || messageID < 0;
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
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
