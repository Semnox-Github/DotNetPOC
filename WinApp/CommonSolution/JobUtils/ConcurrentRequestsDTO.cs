/********************************************************************************************
 * Project Name - Concurrent Request DTO
* Description  - Data object of the Concurrent Request 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        26-Feb-2016   Jeevan              Created 
 *2.70.2      24-Jul-2019   Dakshakh raj        Modified : Added Parameterized constructor,
 *                                                         CreationDate and CreatedBy fields
 *2.90        26-May-2020   Mushahid Faizan     Modified : 3 tier changes for Rest API., Added IsActive Column.
 *2.120.1     26-Apr-2021   Deeksha             Modified as part of AWS Concurrent Programs enhancements
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the Concurrent Request data object class. This acts as data holder for the  Concurrent Request business object
    /// </summary>

    public class ConcurrentRequestsDTO : IChangeTracking
    {
        /// <summary>
        /// This is the Concurrent Request data object class. This acts as data holder for the  Concurrent Request business object
        /// </summary>

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByConcurrentRequestParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByRequestParameters
        {
            /// <summary>
            /// Search by RequestId field
            /// </summary>
            REQUEST_ID,
           
            /// <summary>
            /// Search by Phase field
            /// </summary>
            PHASE,
            
            /// <summary>
            /// Search by Status field
            /// </summary>
            STATUS,
            
            /// <summary>
            /// Search by start time field
            /// </summary>
            START_TIME,
            
            /// <summary>
            /// Search by programId field
            /// </summary>
            PROGRAM_ID,

            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by programId field
            /// </summary>
            PROGRAM_ID_LIST,

            /// <summary>
            /// Search by processId field
            /// </summary>
            PROCESS_ID,
            
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by FROM DATE field
            /// </summary>
            START_FROM_DATE,

            /// <summary>
            /// Search by Program Name field
            /// </summary>
            PROGRAM_NAME,
            /// <summary>
            /// Search by PROGRAM_EXECUTABLE_NAME field
            /// </summary>
            PROGRAM_EXECUTABLE_NAME,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            REQUEST_GUID,

        } 

        private int requestId;
        private int programId;
        private int programScheduleId;
        private string requestedTime;
        private string requestedBy;
        private string startTime;
        private string actualStartTime;
        private string endTime;
        private string phase;
        private string status;
        private bool relaunchOnExit;
        private string argument1;
        private string argument2;
        private string argument3;
        private string argument4;
        private string argument5;
        private string argument6;
        private string argument7;
        private string argument8;
        private string argument9;
        private string argument10;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int processId;
        private int errorCount;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool isActive;
        //private int concurrentRequestSetId;
        //private int concurrentRequestSetStageId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConcurrentRequestsDTO()
        {
            log.LogMethodEntry();
            siteId = -1;
            requestId = -1;
            this.argument1 = null;
            this.argument2 = null;
            this.argument3 = null;
            this.argument4 = null;
            this.argument5 = null;
            this.argument6 = null;
            this.argument7 = null;
            this.argument8 = null;
            this.argument9 = null;
            this.argument10 = null;
            errorCount = -1;
            programId = -1;
            processId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public ConcurrentRequestsDTO(int requestId, int programId, int programScheduleId, string requestedTime, string requestedBy,
                                    string startTime, string actualStartTime, string endTime,
                                    string phase, string status, bool relaunchOnExit,
                                    string argument1, string argument2, string argument3, string argument4,
                                    string argument5, string argument6, string argument7, string argument8,
                                    string argument9, string argument10, int processId, int errorCount,
                                    bool isActive)//, int concurrentRequestSetId)
            : this()
        {
            log.LogMethodEntry(requestId, programId, programScheduleId, requestedTime, requestedBy, startTime, actualStartTime, endTime, phase, status, relaunchOnExit,
                                argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, processId, errorCount, isActive);//, concurrentRequestSetId);
            this.requestId = requestId;
            this.programId = programId;
            this.programScheduleId = programScheduleId;
            this.requestedTime = requestedTime;
            this.requestedBy = requestedBy;
            this.startTime = startTime;
            this.actualStartTime = actualStartTime;
            this.endTime = endTime;
            this.phase = phase;
            this.status = status;
            this.relaunchOnExit = relaunchOnExit;
            this.argument1 = argument1;
            this.argument2 = argument2;
            this.argument3 = argument3;
            this.argument4 = argument4;
            this.argument5 = argument5;
            this.argument6 = argument6;
            this.argument7 = argument7;
            this.argument8 = argument8;
            this.argument9 = argument9;
            this.argument10 = argument10;
            this.processId = processId;
            this.errorCount = errorCount;
            this.isActive = isActive;
            //this.concurrentRequestSetId = concurrentRequestSetId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ConcurrentRequestsDTO(int requestId, int programId, int programScheduleId, string requestedTime, string requestedBy, string startTime, string actualStartTime, string endTime,
                                   string phase, string status, bool relaunchOnExit, string argument1, string argument2, string argument3, string argument4, string argument5, string argument6, string argument7, string argument8,
                                   string argument9, string argument10, int processId, int errorCount, int siteId, string guid, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, 
                                   DateTime lastUpdateDate, bool isActive)//, int concurrentRequestSetId)
           : this(requestId, programId, programScheduleId, requestedTime, requestedBy, startTime, actualStartTime, endTime, phase, status, relaunchOnExit,
                 argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8,
                 argument9, argument10, processId, errorCount, isActive)//, concurrentRequestSetId)
        {
            log.LogMethodEntry(lastUpdatedBy, lastUpdateDate, siteId, guid, synchStatus, masterEntityId,
                createdBy, creationDate);//, concurrentRequestSetId);

            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
        }

        /// <summary>
        /// Get/Set method of the RequestId field
        /// </summary>
        [DisplayName("RequestId")]
        [ReadOnly(true)]
        public int RequestId { get { return requestId; } set { requestId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProgramId field
        /// </summary>
        public int ProgramId { get { return programId; } set { programId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProgramScheduleId field
        /// </summary>
        public int ProgramScheduleId { get { return programScheduleId; } set { programScheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RequestedTime field
        /// </summary>
        [DisplayName("RequestedTime")]
        public string RequestedTime { get { return requestedTime; } set { requestedTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RequestedBy field
        /// </summary>
        [DisplayName("RequestedBy")]
        public string RequestedBy { get { return requestedBy; } set { requestedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the StartTime field
        /// </summary>
        [DisplayName("StartTime")]
        public string StartTime { get { return startTime; } set { startTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActualStartTime field
        /// </summary>
        [DisplayName("Actual Start Time")]
        public string ActualStartTime { get { return actualStartTime; } set { actualStartTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EndTime field
        /// </summary>
        [DisplayName("EndTime")]
        public string EndTime { get { return endTime; } set { endTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Phase field
        /// </summary>
        [DisplayName("Phase")]
        public string Phase { get { return phase; } set { phase = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        [DisplayName("Status")]
        public string Status { get { return status; } set { status = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RelaunchOnExit field
        /// </summary>
        [DisplayName("RelaunchOnExit")]
        public bool RelaunchOnExit { get { return relaunchOnExit; } set { relaunchOnExit = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Argument1 field
        /// </summary>
        [DisplayName("Argument1")]
        public string Argument1 { get { return argument1; } set { argument1 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Argument2 field
        /// </summary>
        [DisplayName("Argument2")]
        public string Argument2 { get { return argument2; } set { argument2 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Argument3 field
        /// </summary>
        [DisplayName("Argument3")]
        public string Argument3 { get { return argument3; } set { argument3 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Argument4 field
        /// </summary>
        [DisplayName("Argument4")]
        public string Argument4 { get { return argument4; } set { argument4 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Argument5 field
        /// </summary>
        [DisplayName("Argument5")]
        public string Argument5 { get { return argument5; } set { argument5 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Argument6 field
        /// </summary>
        [DisplayName("Argument6")]
        public string Argument6 { get { return argument6; } set { argument6 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Argument7 field
        /// </summary>
        [DisplayName("Argument7")]
        public string Argument7 { get { return argument7; } set { argument7 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Argument8 field
        /// </summary>
        [DisplayName("Argument8")]
        public string Argument8 { get { return argument8; } set { argument8 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Argument9 field
        /// </summary>
        [DisplayName("Argument9")]
        public string Argument9 { get { return argument9; } set { argument9 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Argument10 field
        /// </summary>
        [DisplayName("Argument10")]
        public string Argument10 { get { return argument10; } set { argument10 = value; this.IsChanged = true; } }

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
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProcessId field
        /// </summary>
        [DisplayName("ProcessId")]
        public int ProcessId { get { return processId; } set { processId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ErrorCount field
        /// </summary>
        [DisplayName("ErrorCount")]
        public int ErrorCount { get { return errorCount; } set { errorCount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUserId field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        [Browsable(false)]
        public string LastUpdatedUserId { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        [Browsable(false)]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

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
        /// Get method of the isActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the concurrentRequestSetId field
        /// </summary>
        //public int ConcurrentRequestSetId { get { return concurrentRequestSetId; } set { concurrentRequestSetId = value; this.IsChanged = true; } }

        
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
                    return notifyingObjectIsChanged || requestId < 0;
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
