/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of PostTransactionProcess
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      28-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the PostTransactionProcess data object class. This acts as data holder for the PostTransactionProcess business object
    /// </summary>
    public class PostTransactionProcessDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by  ID LIST field
            /// </summary>
            ID_LIST,
            /// <summary>
            /// Search by  TYPE field
            /// </summary>
            TYPE,
            /// <summary>
            /// Search by  ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by  IS ISOLATED field
            /// </summary>
            IS_ISOLATED,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int id;
        private string process;
        private string type;
        private bool? activeFlag;
        private int? executeOrder;
        private char? isIsolated;
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
        public PostTransactionProcessDTO()
        {
            log.LogMethodEntry();
            id = -1;
            activeFlag = true;
            isIsolated = 'Y';
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public PostTransactionProcessDTO(int id, string process, string type, bool? activeFlag, int? executeOrder,
                                         char? isIsolated)
            :this()
        {
            log.LogMethodEntry(id, process, type, activeFlag, executeOrder, isIsolated);
            this.id = id;
            this.process = process;
            this.type = type;
            this.activeFlag = activeFlag;
            this.executeOrder = executeOrder;
            this.isIsolated = isIsolated;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public PostTransactionProcessDTO(int id,string process,string type, bool? activeFlag,int? executeOrder,
                                         char? isIsolated,string guid,DateTime lastUpdatedDate,string lastUpdatedBy, int siteId,bool synchStatus,
                                         int masterEntityId, string createdBy,DateTime creationDate)
            :this(id, process, type, activeFlag, executeOrder, isIsolated)
        {
            log.LogMethodEntry(id, process, type,activeFlag,executeOrder,isIsolated, guid, lastUpdatedDate,lastUpdatedBy, 
                               siteId, synchStatus,masterEntityId, createdBy, creationDate);
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
        /// Get/Set method of the Id  field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Process  field
        /// </summary>
        public string Process
        {
            get { return process; }
            set { process = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Type  field
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ActiveFlag  field
        /// </summary>
        public bool? ActiveFlag
        {
            get { return activeFlag; }
            set { activeFlag = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ExecuteOrder field
        /// </summary>
        public int? ExecuteOrder
        {
            get { return executeOrder; }
            set { executeOrder = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the IsIsolated field
        /// </summary>
        public char? IsIsolated
        {
            get { return isIsolated; }
            set { isIsolated = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged || id < 0;
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
