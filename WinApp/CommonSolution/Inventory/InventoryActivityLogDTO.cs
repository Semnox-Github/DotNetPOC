/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of InventoryActivityLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      23-May-2019    Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the InventoryActivityLog data object class. This acts as data holder for the InventoryActivityLog business object
    /// </summary>
    public class InventoryActivityLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by MESSAGE field
            /// </summary>
            MESSAGE,
            /// <summary>
            /// Search by INVENTORY TABLE KEY field
            /// </summary>
            INV_TABLE_KEY,
            /// <summary>
            /// Search by  SOURCE TABLE NAME field
            /// </summary>
            SOURCE_TABLE_NAME,
            /// <summary>
            /// Search by SOURCE SYSTEM ID field
            /// </summary>
            SOURCE_SYSTEM_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private DateTime timeStamp;
        private String message;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private string sourceTableName;
        private int invTableKey;
        private string sourceSystemId;
        private int masterEntityId;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InventoryActivityLogDTO()
        {
            log.LogMethodEntry();
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public InventoryActivityLogDTO(DateTime timeStamp, String message, string sourceTableName, int invTableKey, string sourceSystemId)
            :this()
        {
            log.LogMethodEntry(timeStamp, message, sourceTableName, invTableKey, sourceSystemId);
            this.timeStamp = timeStamp;
            this.message = message;
            this.sourceTableName = sourceTableName;
            this.invTableKey = invTableKey;
            this.sourceSystemId = sourceSystemId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public InventoryActivityLogDTO( DateTime timeStamp,String message,string guid,bool synchStatus,int siteId,
                                        string sourceTableName, int invTableKey, string sourceSystemId,int masterEntityId,
                                        string lastUpdatedBy,DateTime lastUpdatedDate,string createdBy, DateTime creationDate )
            :this(timeStamp, message, sourceTableName, invTableKey, sourceSystemId)
        {
            log.LogMethodEntry(timeStamp,message, guid,synchStatus, siteId, sourceTableName,invTableKey,sourceSystemId, masterEntityId,
                               lastUpdatedBy,  lastUpdatedDate, createdBy,  creationDate);
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
        /// Get/Set method of the TimeStamp field
        /// </summary>
        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Message field
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SourceTableName field
        /// </summary>
        public string SourceTableName
        {
            get { return sourceTableName; }
            set { sourceTableName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the InvTableKey field
        /// </summary>
        public int InvTableKey
        {
            get { return invTableKey; }
            set { invTableKey = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SourceSystemId field
        /// </summary>
        public string SourceSystemId
        {
            get { return sourceSystemId; }
            set { sourceSystemId = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged;
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
