/********************************************************************************************
 * Project Name - Job Schedule DTO
 * Description  - Data object of JOb Schedule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Dec-2015   Raghuveera          Created 
 *2.70        08-May-2019   Mehraj              Added ScheduleAssetTaskDTO as childList Property
 *2.70        08-Mar-2019   Guru S A            Renamed MaintenanceScheduleDTO as JobScheduleDTO
 *2.70.2        25-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the Maintenance schedule data object class. This acts as data holder for the Maintenance schedule business object
    /// </summary>
    public class JobScheduleDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByMaintenanceScheduleParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByJobScheduleDTOParameters
        {
            /// <summary>
            /// Search by MAINT_SCHEDULE_ID field
            /// </summary>
            JOB_SCHEDULE_ID,
            /// <summary>
            /// Search by SCHEDULE_ID field
            /// </summary>
            SCHEDULE_ID,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID
        }

        int jobScheduleId;
        int scheduleId;
        int userId;
        int departmentId;
        int durationToComplete;
        DateTime maxValueJobCreated;
        bool isActive;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdatedDate;
        string guid;
        int siteId;
        bool synchStatus;
        int masterEntityId;//Modification on 18-Jul-2016 for publish feature
        List<JobScheduleTasksDTO> jobScheduleTasksDTOList;
        /// <summary>
        /// Default constructor
        /// </summary>
        public JobScheduleDTO()
        {
            log.LogMethodEntry();
            jobScheduleId = -1;
            scheduleId = -1;
            departmentId = -1;
            userId = -1;
            isActive = true;
            jobScheduleTasksDTOList = new List<JobScheduleTasksDTO>();
            masterEntityId = -1;//Modification on 18-Jul-2016 for publish feature
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with required fields
        /// </summary>
        public JobScheduleDTO(int jobScheduleId, int scheduleId, int userId, int departmentId, int durationToComplete, DateTime maxValueJobCreated,
                                       bool isActive)
            :this()
        {
            log.LogMethodEntry(jobScheduleId, scheduleId, userId, departmentId, durationToComplete, maxValueJobCreated, isActive);
            this.jobScheduleId = jobScheduleId;
            this.scheduleId = scheduleId;
            this.userId = userId;
            this.departmentId = departmentId;
            this.durationToComplete = durationToComplete;
            this.maxValueJobCreated = maxValueJobCreated;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public JobScheduleDTO(int jobScheduleId, int scheduleId, int userId, int departmentId, int durationToComplete, DateTime maxValueJobCreated,
                                       bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                       DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId)
            :this(jobScheduleId, scheduleId, userId, departmentId, durationToComplete, maxValueJobCreated, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdatedDate,
                               guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;//Modification on 18-Jul-2016 for publish feature
            log.LogMethodExit();
        }

        public JobScheduleDTO(JobScheduleDTO jobScheduleDTO)
           : this()
        {
            log.LogMethodEntry(jobScheduleId, scheduleId, userId, departmentId, durationToComplete, maxValueJobCreated,
                                       isActive, createdBy, creationDate, lastUpdatedBy,
                                       lastUpdatedDate, guid, siteId, synchStatus, masterEntityId);
            jobScheduleId = jobScheduleDTO.jobScheduleId;
            scheduleId = jobScheduleDTO.scheduleId;
            userId = jobScheduleDTO.userId;
            departmentId = jobScheduleDTO.departmentId;
            durationToComplete = jobScheduleDTO.durationToComplete;
            maxValueJobCreated = jobScheduleDTO.maxValueJobCreated;
            isActive = jobScheduleDTO.isActive;
            createdBy = jobScheduleDTO.createdBy;
            creationDate = jobScheduleDTO.creationDate;
            lastUpdatedBy = jobScheduleDTO.lastUpdatedBy;
            lastUpdatedDate = jobScheduleDTO.lastUpdatedDate;
            guid = jobScheduleDTO.guid;
            siteId = jobScheduleDTO.siteId;
            synchStatus = jobScheduleDTO.synchStatus;
            masterEntityId = jobScheduleDTO.masterEntityId;
            if (jobScheduleDTO.jobScheduleTasksDTOList != null)
            {
                jobScheduleTasksDTOList = new List<JobScheduleTasksDTO>();
                foreach (var jobScheduleTasksDTO in jobScheduleDTO.jobScheduleTasksDTOList)
                {
                    jobScheduleTasksDTOList.Add(new JobScheduleTasksDTO(jobScheduleTasksDTO));
                }
            }
        }
        /// <summary>
        /// Get/Set method of the JobScheduleId field
        /// </summary>
        [DisplayName("Job Schedule Id")]
        [ReadOnly(true)]
        public int JobScheduleId { get { return jobScheduleId; } set { jobScheduleId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("Schedule Id")]
        [Browsable(false)]
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("Assigned To")]
        public int UserId { get { return userId; } set { userId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DepartmentId field
        /// </summary>
        [DisplayName("Department")]
        public int DepartmentId { get { return departmentId; } set { departmentId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DurationToComplete field
        /// </summary>
        [DisplayName("Duration To Complete")]
        public int DurationToComplete { get { return durationToComplete; } set { durationToComplete = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaxValueJobCreated field
        /// </summary>
        [DisplayName("Max Value Job Created")]
        [Browsable(false)]
        public DateTime MaxValueJobCreated { get { return maxValueJobCreated; } set { maxValueJobCreated = value; IsChanged = true; } }
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
        public string CreatedBy { get { return createdBy; } set { createdBy = value; IsChanged = true; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; IsChanged = true; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; IsChanged = true; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
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

        /// <summary>//starts:Modification on 18-Jul-2016 for publish feature
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }//Ends:Modification on 18-Jul-2016 for publish feature

        /// <summary>
        /// Get/Set method of the jobScheduleTasksDTOList field
        /// </summary>
        [DisplayName("jobScheduleTasksDTOList")]
        [Browsable(false)]
        public List<JobScheduleTasksDTO> JobScheduleTasksDTOList { get { return jobScheduleTasksDTOList; } set { jobScheduleTasksDTOList = value; } }


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
                    return notifyingObjectIsChanged || jobScheduleId < 0;
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
