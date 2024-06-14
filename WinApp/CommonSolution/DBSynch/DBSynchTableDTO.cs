/********************************************************************************************
 * Project Name - Utilities
 * Description  - DTO of 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        24-Mar-2019   Jagan Mohana          Created 
 *2.60        09-Apr-2019   Mushahid Faizan       Modified : Added LogMethodEntry & Exit.
 *            29-Jul-2019   Mushahid Faizan       Added IsActive
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.DBSynch
{
    public class DBSynchTableDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by DBSYNCH_ID field
            /// </summary>
            DBSYNCH_ID,
            /// <summary>
            /// Search by TABLE_NAME field
            /// </summary>
            TABLE_NAME,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID
        }

        private int dbSynchId;
        private string tableName;
        private string uploadOnly;
        private string synchronize;
        private string insertOnly;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private string initialLoadDone;
        private string ignoreColumnsOnRoaming;
        private string ignoreOnError;
        private string synchDeletes;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string dbSynchLogName;
        private int batchNumber;
        private int uploadFrequency;
        private decimal batchStartTime;
        private decimal batchEndTime;
        private decimal uploadBatchMaxHours;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DBSynchTableDTO()
        {
            log.LogMethodEntry();
            this.dbSynchId = -1;
            this.masterEntityId = -1;
            this.siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DBSynchTableDTO(int dbSynchId, string tableName, string uploadOnly, string synchronize, string insertOnly, int siteId, string guid, bool synchStatus, string initialLoadDone, string ignoreColumnsOnRoaming,
                               string ignoreOnError, string synchDeletes, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                               string dbSynchLogName, int batchNumber, int uploadFrequency, decimal batchStartTime, decimal batchEndTime, decimal uploadBatchMaxHours)
        {
            log.LogMethodEntry(dbSynchId, tableName, uploadOnly, synchronize, insertOnly, siteId, guid, synchStatus, initialLoadDone, ignoreColumnsOnRoaming, ignoreOnError, synchDeletes, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                               dbSynchLogName, batchNumber, uploadFrequency, batchStartTime, batchEndTime, uploadBatchMaxHours);
            this.dbSynchId = dbSynchId;
            this.tableName = tableName;  
            this.uploadOnly = uploadOnly;
            this.synchronize = synchronize;
            this.insertOnly = insertOnly;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.initialLoadDone = initialLoadDone;
            this.ignoreColumnsOnRoaming = ignoreColumnsOnRoaming;
            this.ignoreOnError = ignoreOnError;
            this.synchDeletes = synchDeletes;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.masterEntityId = masterEntityId;
            this.dbSynchLogName = dbSynchLogName;
            this.batchNumber = batchNumber;
            this.uploadFrequency = uploadFrequency;
            this.batchStartTime = batchStartTime;
            this.batchEndTime = batchEndTime;
            this.uploadBatchMaxHours = uploadBatchMaxHours;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int DbSynchId { get { return dbSynchId; } set { dbSynchId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TableName field
        /// </summary>
        [DisplayName("TableName")]
        public string TableName { get { return tableName; } set { tableName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UploadOnly field
        /// </summary>
        [DisplayName("UploadOnly")]
        public string UploadOnly { get { return uploadOnly; } set { uploadOnly = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Synchronize field
        /// </summary>
        [DisplayName("Synchronize")]
        public string Synchronize { get { return synchronize; } set { synchronize = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the InsertOnly field
        /// </summary>
        [DisplayName("InsertOnly")]
        public string InsertOnly { get { return insertOnly; } set { insertOnly = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SyncStatus field
        /// </summary>
        [DisplayName("SyncStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the InitialLoadDone field
        /// </summary>
        [DisplayName("InitialLoadDone")]
        public string InitialLoadDone { get { return initialLoadDone; } set { initialLoadDone = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IgnoreColumnsOnRoaming field
        /// </summary>
        [DisplayName("IgnoreColumnsOnRoaming")]
        public string IgnoreColumnsOnRoaming { get { return ignoreColumnsOnRoaming; } set { ignoreColumnsOnRoaming = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IgnoreOnError field
        /// </summary>
        [DisplayName("IgnoreOnError")]
        public string IgnoreOnError { get { return ignoreOnError; } set { ignoreOnError = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchDeletes field
        /// </summary>
        [DisplayName("SynchDeletes")]
        public string SynchDeletes { get { return synchDeletes; } set { synchDeletes = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DbSynchLogName field
        /// </summary>
        public string DbSynchLogName { get { return dbSynchLogName; } set { dbSynchLogName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BatchNumber field
        /// </summary>
        public int BatchNumber { get { return batchNumber; } set { batchNumber = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UploadFrequency field
        /// </summary>
        public int UploadFrequency { get { return uploadFrequency; } set { uploadFrequency = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BatchStartTime field
        /// </summary>
        public decimal BatchStartTime { get { return batchStartTime; } set { batchStartTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BatchEndTime field
        /// </summary>
        public decimal BatchEndTime { get { return batchEndTime; } set { batchEndTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UploadBatchMaxHours field
        /// </summary>
        public decimal UploadBatchMaxHours { get { return uploadBatchMaxHours; } set { uploadBatchMaxHours = value; this.IsChanged = true; } }
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
                    return notifyingObjectIsChanged || dbSynchId < 0;
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