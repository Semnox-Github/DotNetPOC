/********************************************************************************************
 * Project Name - Concurrent ProgramsDTO
 * Description  - Data object of the Concurrent Programs
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By         Remarks          
 *********************************************************************************************
 *1.00        16-Feb-2016    Amaresh             Created 
 *2.70.2      24-Jul-2019    Dakshakh raj        Modified : Added Parameterized costrustor,
 *                                                          CreationDate,CreatedBy and 
 *                                                          MasterEntityField fields
 *2.80        26-May-2020    Mushahid Faizan     Modified : 3 tier changes for Rest API. Added Child DTO's.                                                         
 *2.120.1     09-Jun-2021    Deeksha             Modified: As part of AWS concurrent program enhancements
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the Concurrent Programs data object class. This acts as data holder for the  Concurrent Programs business object
    /// </summary>

    public class ConcurrentProgramsDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByProgramsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByProgramsParameters
        {
            /// <summary>
            /// Search by ProgramId field
            /// </summary>
            PROGRAM_ID,
            /// <summary>
            /// Search by ProgramId List field
            /// </summary>
            PROGRAM_ID_LIST,
            /// <summary>
            /// Search by ProgramName field
            /// </summary>
            PROGRAM_NAME,

            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ACTIVE_FLAG,

            /// <summary>
            /// Search by IsActive field
            /// </summary>
            SYSTEM_PROGRAM,

            /// <summary>
            /// Search by IsActive field
            /// </summary>
            KEEP_RUNNING,

            /// <summary>
            /// Search by SUCCESS_NOTIFICATION_MAIL field
            /// </summary>
            SUCCESS_NOTIFICATION_MAIL,

            /// <summary>
            /// Search by ERROR_NOTIFICATION_MAIL field
            /// </summary>
            ERROR_NOTIFICATION_MAIL,

            /// <summary>
            /// Search by EXECUTABLENAME field
            /// </summary>
            EXECUTABLE_NAME,
            /// <summary>
            /// Search by MULTIPLE_INSTANCE_RUN_ALLOWED field
            /// </summary>
            MULTIPLE_INSTANCE_RUN_ALLOWED,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID
        }

        private int programId;
        private string programName;
        private string executionMethod;
        private string executableName;
        private bool systemProgram;
        private bool keepRunning;
        private bool isActive;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private string lastExecutedOn;
        private string successNotificationMailId;
        private string errorNotificationMailId;
        private int argumentCount;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool mutlipleInstanceRunAllowed;

        private List<ConcurrentProgramSchedulesDTO> concurrentProgramSchedulesDTOList;
        private List<ConcurrentProgramParametersDTO> concurrentProgramParametersDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConcurrentProgramsDTO()
        {
            log.LogMethodEntry();
            siteId = -1;
            programId = -1;
            argumentCount = -1;
            isActive = true;
            keepRunning = false;
            executionMethod = "P";
            masterEntityId = -1;
            concurrentProgramSchedulesDTOList = new List<ConcurrentProgramSchedulesDTO>();
            concurrentProgramParametersDTOList = new List<ConcurrentProgramParametersDTO>();
            mutlipleInstanceRunAllowed = false;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public ConcurrentProgramsDTO(int programId, string programName, string executionMethod, string executableName, bool systemProgram, bool keepRunning, bool isActive,
                                     string lastExecutedOn, string successNotificationMailId, int argumentCount, string errorNotificationMailId,bool mutlipleInstanceRunAllowed)
            : this()
        {
            log.LogMethodEntry(programId, programName, executionMethod, executableName, systemProgram, keepRunning, isActive, lastExecutedOn, successNotificationMailId, argumentCount, errorNotificationMailId);
            this.programId = programId;
            this.programName = programName;
            this.executionMethod = executionMethod;
            this.executableName = executableName;
            this.systemProgram = systemProgram;
            this.keepRunning = keepRunning;
            this.isActive = isActive;
            this.lastExecutedOn = lastExecutedOn;
            this.successNotificationMailId = successNotificationMailId;
            this.argumentCount = argumentCount;
            this.errorNotificationMailId = errorNotificationMailId;
            this.mutlipleInstanceRunAllowed = mutlipleInstanceRunAllowed;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ConcurrentProgramsDTO(int programId, string programName, string executionMethod, string executableName, bool systemProgram, bool keepRunning, bool isActive, int siteId,
                                     string guid, bool synchStatus, DateTime lastUpdatedDate, string lastUpdatedUser, string lastExecutedOn,
                                     string successNotificationMailId, int argumentCount, string errorNotificationMailId, bool mutlipleInstanceRunAllowed, int masterEntityId, string createdBy, DateTime creationDate)
            : this(programId, programName, executionMethod, executableName, systemProgram, keepRunning, isActive, lastExecutedOn, successNotificationMailId, argumentCount, errorNotificationMailId, mutlipleInstanceRunAllowed)
        {
            log.LogMethodEntry(siteId, guid, synchStatus, lastUpdatedDate, lastUpdatedUser, masterEntityId, createdBy, creationDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.argumentCount = argumentCount;
            this.errorNotificationMailId = errorNotificationMailId;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ProgramId field
        /// </summary>
        [DisplayName("Program Id")]
        public int ProgramId { get { return programId; } set { programId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProgramId field
        /// </summary>
        [DisplayName("Program Name")]
        public string ProgramName { get { return programName; } set { programName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ExecutionMethod field
        /// </summary>
        [DisplayName("Execution Method")]
        public string ExecutionMethod { get { return executionMethod; } set { executionMethod = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ExecutableName field
        /// </summary>
        [DisplayName("Executable Name")]
        public string ExecutableName { get { return executableName; } set { executableName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SystemProgram field
        /// </summary>
        [Browsable(false)]
        [DisplayName("System Program")]
        public bool SystemProgram { get { return systemProgram; } set { systemProgram = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the KeepRunning field
        /// </summary>
        [DisplayName("Keep Running")]
        public bool KeepRunning { get { return keepRunning; } set { keepRunning = value; this.IsChanged = true; } }

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
        [ReadOnly(true)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("Last Updated User")]
        [ReadOnly(true)]
        public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value; } }

        /// <summary>
        /// Get/Set method of the LastExecutedOn field
        /// </summary>
        [Browsable(false)]
        [DisplayName("Last Executed On")]
        public string LastExecutedOn { get { return lastExecutedOn; } set { lastExecutedOn = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SuccessNotificationMailId field
        /// </summary>
        [DisplayName("Success Notification Email Id")]
        public string SuccessNotificationMailId { get { return successNotificationMailId; } set { successNotificationMailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ArgumentCount field
        /// </summary>
        [Browsable(false)]
        [DisplayName("ArgumentCount")]
        public int ArgumentCount { get { return argumentCount; } set { argumentCount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ErrorNotificationMailId field
        /// </summary>
        [DisplayName("Error Notification Email Id")]
        public string ErrorNotificationMailId { get { return errorNotificationMailId; } set { errorNotificationMailId = value; this.IsChanged = true; } }

        [DisplayName("Mutliple Instance RunAllowed")]
        [ReadOnly(true)]
        public bool MutlipleInstanceRunAllowed { get { return mutlipleInstanceRunAllowed; } set { mutlipleInstanceRunAllowed = value; this.IsChanged = true; } }
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

        public List<ConcurrentProgramSchedulesDTO> ConcurrentProgramSchedulesDTOList { get { return concurrentProgramSchedulesDTOList; } set { concurrentProgramSchedulesDTOList = value; } }
        public List<ConcurrentProgramParametersDTO> ConcurrentProgramParametersDTOList { get { return concurrentProgramParametersDTOList; } set { concurrentProgramParametersDTOList = value; } }

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
                if (concurrentProgramSchedulesDTOList != null &&
                  concurrentProgramSchedulesDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (concurrentProgramParametersDTOList != null &&
                 concurrentProgramParametersDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

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
                    return notifyingObjectIsChanged || programId < 0;
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
