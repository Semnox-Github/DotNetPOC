/********************************************************************************************
 * Project Name - Concurrent Program Schedule DTO
 * Description  - Data object of the concurrent Programs Schedule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        17-Feb-2015    Amaresh              Created 
 *2.70.2      24-Jul-2019    Dakshakh raj        Modified : Added Parameterized costrustor,
 *                                                         CreationDate, CreatedBy  and
 *                                                         MasterEntityId fields
 *2.120.1     11-May-2021    Deeksha             Modified for AWS JobScheduler enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the Concurrent Programs Schedules data object class. This acts as data holder for the  Concurrent Programs Schedules business object
    /// </summary>

    public class ConcurrentProgramSchedulesDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByProgramSceduleParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByProgramSceduleParameters
        {
            /// <summary>
            /// Search by ProgramScheduleId field
            /// </summary>
            PROGRAM_SCHEDULE_ID,
           
            /// <summary>
            /// Search by ProgramId field
            /// </summary>
            PROGRAM_ID,
            /// <summary>
            /// Search by ProgramId field
            /// </summary>
            PROGRAM_ID_LIST,

            /// <summary>
            /// Search by Active Flag field
            /// </summary>
            ACTIVE,

            /// <summary>
            /// Search by RunAt field
            /// </summary>
            RUNAT,

            /// <summary>
            /// Search by Start date field
            /// </summary>
            START_DATE,

            /// <summary>
            /// Search by Keep Running field
            /// </summary>
            KEEP_RUNNING,

            /// <summary>
            /// Search by frequency field
            /// </summary>
            FREQUENCY,

            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID

            ///// <summary>
            ///// Search by CONCURRENT REQUEST ID field
            ///// </summary>
            //CONCURRENT_REQUEST_ID

            ///// <summary>
            ///// Search by CONCURRENT SET PROGRAM ID field
            ///// </summary>
            //CONCURRENT_SET_PROGRAM_ID
        }

        private int programScheduleId;
        private int programId;
        private DateTime startDate;
        private string runAt;
        private int frequency;
        private DateTime endDate;
        private bool isActive;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private string lastExecutedOn;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private List<ProgramParameterValueDTO> programParameterValueDTOList = new List<ProgramParameterValueDTO>();
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

         /// <summary>
        /// Default constructor
        /// </summary>
        public ConcurrentProgramSchedulesDTO()
        {
            log.LogMethodEntry();
            siteId = -1;
            programScheduleId = -1;
            isActive = true;
            frequency = -1;
            programId = -1;
            runAt = "0:00";
            masterEntityId = -1;
            log.LogMethodExit();              
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public ConcurrentProgramSchedulesDTO(int programScheduleId, int programId, DateTime startDate, string runAt, int frequency,
                                            DateTime endDate, bool isActive, string lastExecutedOn)
            :this()
        {
            log.LogMethodEntry(programScheduleId, programId, startDate, runAt, frequency, endDate, isActive, lastExecutedOn);
            this.programScheduleId = programScheduleId;
            this.programId = programId;
            this.startDate = startDate;
            this.runAt = runAt;
            this.frequency = frequency;
            this.endDate = endDate;
            this.isActive = isActive;
            this.lastExecutedOn = lastExecutedOn;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>

        public ConcurrentProgramSchedulesDTO(int programScheduleId, int programId, DateTime startDate, string runAt, int frequency, DateTime endDate, bool isActive,
                                              int siteId, string guid, bool synchStatus, DateTime lastUpdatedDate,  string lastUpdatedUser, string lastExecutedOn,
                                              int masterEntityId, string createdBy, DateTime creationDate)
            :this(programScheduleId, programId, startDate, runAt, frequency, endDate, isActive, lastExecutedOn)
        {
            log.LogMethodEntry(siteId, guid, synchStatus, lastUpdatedDate, lastUpdatedUser, masterEntityId, createdBy, creationDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ProgramScheduleId field
        /// </summary>
        [DisplayName("Schedule Id")]
        [ReadOnly(true)]
        public int ProgramScheduleId { get { return programScheduleId; } set { programScheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProgramId field
        /// </summary>
        [DisplayName("Program Id")]
        public int ProgramId { get { return programId; } set { programId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TimeFrom field
        /// </summary>
        [DisplayName("Start Date")]
        public DateTime StartDate { get { return startDate; } set { startDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RunAt field
        /// </summary>
        [DisplayName("RunAt")]
        public string RunAt { get { return runAt; } set { runAt = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Frequency field
        /// </summary>
        [DisplayName("Frequency")]
        public int Frequency { get { return frequency; } set { frequency = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EndTime field
        /// </summary>
        [DisplayName("End Date")]
        public DateTime EndDate { get { return endDate; } set { endDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
       [DisplayName("Site Id")]
       [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("Last Updated User")]
        [Browsable(false)]
        public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastExecutedOn field
        /// </summary>
        [DisplayName("Last Executed On")]
        public string LastExecutedOn { get { return lastExecutedOn; } set { lastExecutedOn = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; IsChanged = true; } }

        public List<ProgramParameterValueDTO> ProgramParameterValueDTOList { get { return programParameterValueDTOList; } set { programParameterValueDTOList = value; } }


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
                    return notifyingObjectIsChanged || programScheduleId < 0;
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
        /// Returns whether the AdsDTO changed or any of its AdBroadcastDTO DTO  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (programParameterValueDTOList != null &&
                  programParameterValueDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
