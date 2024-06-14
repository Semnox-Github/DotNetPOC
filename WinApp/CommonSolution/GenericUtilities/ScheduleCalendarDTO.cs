/********************************************************************************************
 * Project Name - ScheduleCalendarDTO
 * Description  - Data object of Schedule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Dec-2015   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 *********************************************************************************************
 *1.00        24-Apr-2017   Lakshminarayana     Modified 
 *2.70        05-May-2019   Mehraj              Added JobScheduleDTOList as child list property
 *2.70        08-Mar-2019   Guru S A            Renamed ScheduleDTO as ScheduleCalendarDTO
 *2.70        07-Jun-2019   Akshay Gulaganji    Code merge from Development to WebManagementStudio
 *                                              Added SCHEDULE_ID_LIST in SearchByScheduleCalendarParameters
 *2.70.2        26-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor      
  *2.80        20-May-2020   Mushahid Faizan     Modified : Added IsValidateRequired to check for Validation
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the schedule data object class. This acts as data holder for the schedule business object
    /// </summary>
    public class ScheduleCalendarDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByScheduleParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByScheduleCalendarParameters
        {
            /// <summary>
            /// Search by SCHEDULE ID field
            /// </summary>
            SCHEDULE_ID,

            /// <summary>
            /// Search by ASSET GROUP ID field
            /// </summary>
            SCHEDULE_NAME,

            /// <summary>
            /// Search by SCHEDULE TIME field
            /// </summary>
            SCHEDULE_TIME,

            /// <summary>
            /// Search by RECUR END DATE field
            /// </summary>
            RECUR_END_DATE,

            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by SCHEDULE ID LIST field
            /// </summary>
            SCHEDULE_ID_LIST, 

            /// <summary>
            /// Search by SCHEDULE_FROM_TIME field
            /// </summary>
            SCHEDULE_FROM_TIME,

            /// <summary>
            /// Search by SCHEDULE_TO_TIME field
            /// </summary>
            SCHEDULE_TO_TIME
        }

        private int scheduleId;
        private string scheduleName;
        private DateTime scheduleTime;
        private DateTime scheduleEndDate;
        private string recurFlag;
        private string recurFrequency;
        private DateTime recurEndDate;
        private string recurType;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool isValidateRequired;
        List<JobScheduleDTO> jobScheduleDTOList;
        List<ScheduleCalendarExclusionDTO> scheduleCalendarExclusionDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScheduleCalendarDTO()
        {
            log.LogMethodEntry();
            scheduleId = -1;
            scheduleEndDate = DateTime.MinValue;
            isActive = true;
            recurFlag = "N";
            masterEntityId = -1;
            scheduleName = string.Empty;
            siteId = -1;
            isValidateRequired = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ScheduleCalendarDTO(int scheduleId, string scheduleName, DateTime scheduleTime, DateTime scheduleEndDate, string recurFlag,
                           string recurFrequency, DateTime recurEndDate, string recurType, bool isActive)
            : this()
        {
            log.LogMethodEntry(scheduleId, scheduleName, scheduleTime, scheduleEndDate, recurFlag, recurFrequency, recurEndDate, recurType, isActive);
            this.scheduleId = scheduleId;
            this.scheduleName = scheduleName;
            this.scheduleTime = scheduleTime;
            this.scheduleEndDate = scheduleEndDate;
            this.recurFlag = recurFlag;
            this.recurFrequency = recurFrequency;
            this.recurEndDate = recurEndDate;
            this.recurType = recurType;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ScheduleCalendarDTO(int scheduleId, string scheduleName, DateTime scheduleTime, DateTime scheduleEndDate, string recurFlag,
                           string recurFrequency, DateTime recurEndDate, string recurType, bool isActive, string createdBy,
                           DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid,
                           int siteId, bool synchStatus, int masterEntityId)
            : this(scheduleId, scheduleName, scheduleTime, scheduleEndDate, recurFlag, recurFrequency, recurEndDate, recurType, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        public ScheduleCalendarDTO(ScheduleCalendarDTO scheduleCalendarDTO)
            :this()
        {
            log.LogMethodEntry(scheduleId, scheduleName, scheduleTime, scheduleEndDate, recurFlag, recurFrequency, recurEndDate, recurType, isActive, createdBy,
                           creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
            scheduleId = scheduleCalendarDTO.scheduleId;
            scheduleName = scheduleCalendarDTO.scheduleName;
            scheduleTime = scheduleCalendarDTO.ScheduleTime;
            scheduleEndDate = scheduleCalendarDTO.scheduleEndDate;
            recurFlag = scheduleCalendarDTO.recurFlag;
            recurFrequency = scheduleCalendarDTO.recurFrequency;
            recurEndDate = scheduleCalendarDTO.recurEndDate;
            recurType = scheduleCalendarDTO.recurType;
            isActive = scheduleCalendarDTO.isActive;
            createdBy = scheduleCalendarDTO.createdBy;
            creationDate = scheduleCalendarDTO.creationDate;
            lastUpdatedBy = scheduleCalendarDTO.lastUpdatedBy;
            lastUpdateDate = scheduleCalendarDTO.lastUpdateDate;
            guid = scheduleCalendarDTO.guid;
            siteId = scheduleCalendarDTO.siteId;
            synchStatus = scheduleCalendarDTO.synchStatus;
            masterEntityId = scheduleCalendarDTO.masterEntityId;
            if (scheduleCalendarDTO.scheduleCalendarExclusionDTOList != null)
            {
                scheduleCalendarExclusionDTOList = new List<ScheduleCalendarExclusionDTO>();
                foreach (var scheduleCalendarExclusionDTO in scheduleCalendarDTO.scheduleCalendarExclusionDTOList)
                {
                    scheduleCalendarExclusionDTOList.Add(new ScheduleCalendarExclusionDTO(scheduleCalendarExclusionDTO));
                }
            }
            if (scheduleCalendarDTO.jobScheduleDTOList != null)
            {
                jobScheduleDTOList = new List<JobScheduleDTO>();
                foreach (var campaignCustomerProfileMapDTO in scheduleCalendarDTO.jobScheduleDTOList)
                {
                    jobScheduleDTOList.Add(new JobScheduleDTO(campaignCustomerProfileMapDTO));
                }
            }
        }

        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("Schedule Id")]
        [ReadOnly(true)]
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleName field
        /// </summary>
        [DisplayName("Schedule Name")]
        public string ScheduleName { get { return scheduleName; } set { scheduleName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleTime field
        /// </summary>
        [DisplayName("Schedule Time")]
        public DateTime ScheduleTime { get { return scheduleTime; } set { scheduleTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleTime field
        /// </summary>
        [DisplayName("Schedule End Date")]
        public DateTime ScheduleEndDate { get { return scheduleEndDate; } set { scheduleEndDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RecurFlag field
        /// </summary>
        [DisplayName("Recur Flag")]
        public string RecurFlag { get { return recurFlag; } set { recurFlag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RecurFrequency field
        /// </summary>
        [DisplayName("Recur Frequency")]
        public string RecurFrequency { get { return recurFrequency; } set { recurFrequency = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RecurEndDate field
        /// </summary>
        [DisplayName("End Date")]
        public DateTime RecurEndDate { get { return recurEndDate; } set { recurEndDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RecurType field
        /// </summary>
        [DisplayName("Recur Type")]
        public string RecurType { get { return recurType; } set { recurType = value; this.IsChanged = true; } }

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
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

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

        /// <summary>//starts:Modification on 18-Jul-2016 for publish feature
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the JobScheduleDTOList field
        /// </summary>
        [DisplayName("JobScheduleDTOList")]
        [Browsable(false)]
        public List<JobScheduleDTO> JobScheduleDTOList { get { return jobScheduleDTOList; } set { jobScheduleDTOList = value; } }
        /// <summary>
        /// Get/Set method of the ScheduleCalendarExclusionDTOList field
        /// </summary>
        [DisplayName("ScheduleCalendarExclusionDTOList")]
        [Browsable(false)]
        public List<ScheduleCalendarExclusionDTO> ScheduleCalendarExclusionDTOList { get { return scheduleCalendarExclusionDTOList; } set { scheduleCalendarExclusionDTOList = value; } }

        /// <summary>
        /// Checks For the Validation 
        /// </summary>
        public bool IsValidateRequired { get { return isValidateRequired; } set { isValidateRequired = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || scheduleId < 0 ;
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
        /// Returns whether the jobScheduleDTOL changed or any of its jobScheduleDTOL childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (jobScheduleDTOList != null &&
                   jobScheduleDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (scheduleCalendarExclusionDTOList != null &&
                  scheduleCalendarExclusionDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
