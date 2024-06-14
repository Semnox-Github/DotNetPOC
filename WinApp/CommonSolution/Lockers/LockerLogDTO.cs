using Semnox.Parafait.logging;
/********************************************************************************************
 * Project Name - Locker Log DTO
 * Description  - Data object of Locker Log DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        14-Jul-2017   Raghuveera          Created
 *2.70.2        19-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor,
 *                                                         LastUpdatedBy, LastUpdatedDate fields
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Device.Lockers
{
    public class LockerLogDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int logId;
        private DateTime timeStamp;
        private int lockerId;
        private string source;
        private string logType;
        private string description;
        private string status;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;

        /// <summary>
        /// Default constructor
        /// </summary>
        public LockerLogDTO()
        {
            log.LogMethodEntry();
            logId = -1;
            lockerId = -1;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public LockerLogDTO(int logId, DateTime timeStamp, int lockerId, string source, string logType,
                            string description, string status, bool isActive)
            :this()
        {
            log.LogMethodEntry(logId, timeStamp, lockerId, source, logType, description, status, isActive);
            this.logId = logId;
            this.timeStamp = timeStamp;
            this.lockerId = lockerId;
            this.source = source;
            this.logType = logType;
            this.description = description;
            this.status = status;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public LockerLogDTO(int logId, DateTime timeStamp, int lockerId, string source, string logType,
                            string description, string status, bool isActive, string createdBy, DateTime creationDate,
                            int siteId,string guid, bool synchStatus, int masterEntityId,  DateTime lastUpdateDate, string lastUpdatedBy )
            :this(logId, timeStamp, lockerId, source, logType, description, status, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, siteId, guid, synchStatus, masterEntityId, lastUpdateDate, lastUpdatedBy);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LogId field
        /// </summary>
        [DisplayName("LogId")]
        [ReadOnly(true)]
        public int LogId { get { return logId; } set { logId = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("TimeStamp")]
        public DateTime TimeStamp { get { return timeStamp; } set { timeStamp = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the LockerId field
        /// </summary>
        [DisplayName("LockerId")]
        public int LockerId { get { return lockerId; } set { lockerId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Source field
        /// </summary>
        [DisplayName("Source")]
        public string Source { get { return source; } set { source = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the LogType field
        /// </summary>
        [DisplayName("LogType")]
        public string LogType { get { return logType; } set { logType = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        [DisplayName("Status")]
        public string Status { get { return status; } set { status = value; this.IsChanged = true; } }
       
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
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("Site id")]
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; } }
        
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        [Browsable(false)]
        public DateTime LastupdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

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
