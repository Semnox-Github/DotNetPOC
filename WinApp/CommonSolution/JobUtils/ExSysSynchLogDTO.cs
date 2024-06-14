/********************************************************************************************
 * Project Name - Logger
 * Description  - Data object of ExSysSynchLog
 *
 **************
 ** Version Log
  **************
  * Version     Date            Modified By            Remarks
 *********************************************************************************************
 *2.70.3        29-May-2019      Girish Kundar           Created
 *2.80.0        02-Apr-2020      Akshay Gulaganji        Added search parameters - PARAFAIT_OBJECT, PARAFAIT_OBJECT_ID, HAVING_UN_SUCCESSFUL_COUNT_LESS_THAN and PARAFAIT_OBJECT_ID_LIST
 *2.130.7    14-APR-2022    Girish Kundar           Modified : Aloha BSP integration changes
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the ExSysSynchLog data object class. This acts as data holder for the ExSysSynchLog business object
    /// </summary>
    public class ExSysSynchLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by LOG ID field
            /// </summary>
            LOG_ID,
            /// <summary>
            /// Search by IS SUCCESSFUL field
            /// </summary>
            IS_SUCCESSFUL,
            /// <summary>
            /// Search by  EX SYSTEM NAME field
            /// </summary>
            EX_SYSTEM_NAME,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by PARAFAIT OBJECT field
            /// </summary>
            PARAFAIT_OBJECT,
            /// <summary>
            /// Search by PARAFAIT OBJECT ID field
            /// </summary>
            PARAFAIT_OBJECT_ID,
            /// <summary>
            /// Search by PARAFAIT OBJECT ID LIST field
            /// </summary>
            PARAFAIT_OBJECT_ID_LIST,
            /// <summary>
            /// Search by HAVING UN SUCCESSFUL COUNT LESS THAN field
            /// </summary>
            HAVING_UN_SUCCESSFUL_COUNT_LESS_THAN,
            /// <summary>
            /// Search by DATA
            /// </summary>
            DATA,
            /// <summary>
            /// Search by REMARKS
            /// </summary>
            REMARKS,
            /// <summary>
            /// Search by HAVING UN SUCCESSFUL COUNT LESS THAN field
            /// </summary>
            STATUS,
            /// <summary>
            /// Search by STATUS
            /// </summary>
            PARAFAIT_OBJECT_GUID,
            /// <summary>
            /// Search by PARAFAIT_OBJECT_GUID
            /// </summary>
            TIMESTAMP_TO,
            /// <summary>
            /// Search by TIMESTAMP_TO
            /// </summary>
            TIMESTAMP_FROM,
            /// <summary>
            /// Search by REQUEST_ID
            /// </summary>
            REQUEST_ID,
            /// <summary>
            /// Search by STATUS_LIST
            /// </summary>
            STATUS_LIST,
            /// <summary>
            /// Search by REQUEST_ID_LIST
            /// </summary>
            REQUEST_ID_LIST,
            /// <summary>
            /// Search by LOG_ID_LIST
            /// </summary>
            LOG_ID_LIST,
            /// <summary>
            /// Search by UNIQUE_STATUS_LIST
            /// </summary>
            UNIQUE_OBJECT_STATUS_LIST,
            /// <summary>
            /// Search by BSP_ID
            /// </summary>
            BSP_ID,
            /// <summary>
            /// Search by NOT_IN_REPROCESSING_STATE
            /// </summary>
            NOT_IN_REPROCESSING_STATE,
            /// <summary>
            /// Search by STATUS_NOT_IN_SUCCESS
            /// </summary>
            STATUS_NOT_IN_SUCCESS,
            /// <summary>
            /// Search by TRX_FROM_DATE
            /// </summary>
            TRX_FROM_DATE,
            /// <summary>
            /// Search by TRX_TO_DATE
            /// </summary>
            TRX_TO_DATE
        }
        private int logId;
        private int concurrentRequestId;
        private DateTime timestamp;
        private string exSysName;
        private string parafaitObject;
        private int parafaitObjectId;
        private string parafaitObjectGuid;
        private bool isSuccessFul;
        private string status;
        private string data;
        private string remarks;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExSysSynchLogDTO()
        {
            log.LogMethodEntry();
            logId = -1;
            siteId = -1;
            concurrentRequestId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public ExSysSynchLogDTO(int logId, DateTime timestamp, string exSysName, string parafaitObject, int parafaitObjectId,
                                 string parafaitObjectGuid, bool isSuccessFul,
                                 string status, string data, string remarks, int concurrentRequestId)
            : this()
        {

            log.LogMethodEntry(logId, timestamp, exSysName, parafaitObject, parafaitObjectId, parafaitObjectGuid,
                               isSuccessFul, status, data, remarks, concurrentRequestId);
            this.logId = logId;
            this.timestamp = timestamp;
            this.exSysName = exSysName;
            this.parafaitObject = parafaitObject;
            this.parafaitObjectId = parafaitObjectId;
            this.parafaitObjectGuid = parafaitObjectGuid;
            this.isSuccessFul = isSuccessFul;
            this.status = status;
            this.data = data;
            this.remarks = remarks;
            this.concurrentRequestId = concurrentRequestId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public ExSysSynchLogDTO(int logId, DateTime timestamp, string exSysName, string parafaitObject, int parafaitObjectId, string parafaitObjectGuid, bool isSuccessFul,
                                string status, string data, string remarks, string guid, int siteId, bool synchStatus, DateTime lastUpdatedDate, string lastUpdatedBy, int masterEntityId,
                                string createdBy, DateTime creationDate, int concurrentRequestId)
           : this(logId, timestamp, exSysName, parafaitObject, parafaitObjectId, parafaitObjectGuid, isSuccessFul, status, data, remarks, concurrentRequestId)
        {

            log.LogMethodEntry(logId, timestamp, exSysName, parafaitObject, parafaitObjectId, parafaitObjectGuid, isSuccessFul, status, data, remarks, guid, siteId,
                               synchStatus, lastUpdatedDate, lastUpdatedBy, masterEntityId, createdBy, creationDate, concurrentRequestId);

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
        /// Get/Set method of the LogId  field
        /// </summary>
        public int LogId
        {
            get { return logId; }
            set { logId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the concurrentRequestId  field
        /// </summary>
        public int ConcurrentRequestId
        {
            get { return concurrentRequestId; }
            set { concurrentRequestId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Timestamp  field
        /// </summary>
        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ExSysName  field
        /// </summary>
        public string ExSysName
        {
            get { return exSysName; }
            set { exSysName = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ParafaitObject  field
        /// </summary>
        public string ParafaitObject
        {
            get { return parafaitObject; }
            set { parafaitObject = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ParafaitObjectId  field
        /// </summary>
        public int ParafaitObjectId
        {
            get { return parafaitObjectId; }
            set { parafaitObjectId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ParafaitObjectGuid  field
        /// </summary>
        public string ParafaitObjectGuid
        {
            get { return parafaitObjectGuid; }
            set { parafaitObjectGuid = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the IsSuccessFul  field
        /// </summary>
        public bool IsSuccessFul
        {
            get { return isSuccessFul; }
            set { isSuccessFul = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Status  field
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Data  field
        /// </summary>
        public string Data
        {
            get { return data; }
            set { data = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Remarks  field
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
                    return notifyingObjectIsChanged || logId < 0;
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
