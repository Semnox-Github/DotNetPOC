/********************************************************************************************
 * Project Name - Logger
 * Description  - Data object of DBAuditLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3     29-May-2019   Girish Kundar           Created
 *2.140.0    25-Nov-2021   Fiona Lishal      Modified : Added Searcch Parameter DATE_OF_LOG
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// This is the DBAuditLog data object class. This acts as data holder for the DBAuditLog business object
    /// </summary>
    public class DBAuditLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by TABLE NAME field
            /// </summary>
            TABLE_NAME,
            /// <summary>
            /// Search by RECORD ID field
            /// </summary>
            RECORD_ID,
            /// <summary>
            /// Search by  FIELD NAME field
            /// </summary>
            FIELD_NAME,
            /// <summary>
            /// Search by  TYPE field
            /// </summary>
            TYPE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            DATE_OF_LOG
        }
        private DateTime dateOfLog;
        private string tableName;
        private string recordId;
        private string fieldName;
        private char type;
        private string oldValue;
        private string newValue;
        private string userName;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
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
        public DBAuditLogDTO()
        {
            log.LogMethodEntry();
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields
        /// </summary>
        public DBAuditLogDTO(DateTime dateOfLog, string tableName, string recordId, string fieldName, char type, string oldValue, string newValue, string userName)
            : this()
        {

            log.LogMethodEntry(dateOfLog, tableName, recordId, fieldName, type, oldValue, newValue, userName);
            this.dateOfLog = dateOfLog;
            this.tableName = tableName;
            this.recordId = recordId;
            this.fieldName = fieldName;
            this.type = type;
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.userName = userName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public DBAuditLogDTO(DateTime dateOfLog, string tableName, string recordId, string fieldName, char type, string oldValue, string newValue, string userName,
                             DateTime lastUpdatedDate, string lastUpdatedBy, string guid, int siteId, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate)
            : this(dateOfLog, tableName, recordId, fieldName, type, oldValue, newValue, userName)
        {

            log.LogMethodEntry(dateOfLog, tableName, recordId, fieldName, type, oldValue, newValue, userName, lastUpdatedDate, lastUpdatedBy,
                               guid, siteId, synchStatus, masterEntityId, createdBy, creationDate);
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
        /// Get/Set method of the DateOfLog  field
        /// </summary>
        public DateTime DateOfLog
        {
            get { return dateOfLog; }
            set { dateOfLog = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the TableName  field
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the FieldName  field
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Record Id  field
        /// </summary>
        public string RecordId
        {
            get { return recordId; }
            set { recordId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Type  field
        /// </summary>
        public char Type
        {
            get { return type; }
            set { type = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Old Value  field
        /// </summary>
        public string OldValue
        {
            get { return oldValue; }
            set { oldValue = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the NewValue  field
        /// </summary>
        public string NewValue
        {
            get { return newValue; }
            set { newValue = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the UserName  field
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
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
            set { createdBy = value; }
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
