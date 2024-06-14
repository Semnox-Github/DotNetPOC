/********************************************************************************************
 * Project Name - Schedule Exclusion DTO
 * Description  - Data object of Schedule Exclusion
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        30-Dec-2015   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 *2.70        08-Mar-2019   Guru S A            Renamed ScheduleExclusionDTO as ScheduleCalendarExclusionDTO
 *2.70.2        25-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the schedule exclusion data object class. This acts as data holder for the schedule exclusion business object
    /// </summary>
    public class ScheduleCalendarExclusionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByScheduleExclusionParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByScheduleCalendarExclusionParameters
        {
            /// <summary>
            /// Search by SCHEDULE EXCLUSIONID field
            /// </summary>
            SCHEDULE_EXCLUSION_ID,
            
            /// <summary>
            /// Search by SCHEDULE ID field
            /// </summary>
            SCHEDULE_ID,
            
            /// <summary>
            /// Search by EXCLUSION DATE field
            /// </summary>
            EXCLUSION_DATE,
           
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
            SITE_ID 
        }

        private int scheduleExclusionId;
        private int scheduleId;
        private string exclusionDate;
        private bool includeDate;
        private int day;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScheduleCalendarExclusionDTO()
        {
            log.LogMethodEntry();
            scheduleExclusionId = -1;
            scheduleId = -1;
            day = -1;
            isActive = true;
            includeDate = false;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ScheduleCalendarExclusionDTO(int scheduleExclusionId, int scheduleId, string exclusionDate, bool includeDate, int day, bool isActive)
            :this()
        {
            log.LogMethodEntry(scheduleExclusionId, scheduleId, exclusionDate, includeDate, day, isActive);
            this.scheduleExclusionId = scheduleExclusionId;
            this.scheduleId = scheduleId;
            this.exclusionDate = exclusionDate;
            this.includeDate = includeDate;
            this.day = day;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ScheduleCalendarExclusionDTO(int scheduleExclusionId, int scheduleId, string exclusionDate,
                                    bool includeDate, int day, bool isActive, string createdBy,
                                    DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid,
                                    int siteId, bool synchStatus, int masterEntityId)
            :this(scheduleExclusionId, scheduleId, exclusionDate, includeDate, day, isActive)
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

        public ScheduleCalendarExclusionDTO(ScheduleCalendarExclusionDTO scheduleCalendarExclusionDTO)
           : this()
        {
            log.LogMethodEntry(scheduleExclusionId, scheduleId, exclusionDate,
                                    includeDate, day, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, guid,
                                    siteId, synchStatus, masterEntityId);
            scheduleExclusionId = scheduleCalendarExclusionDTO.scheduleExclusionId;
            scheduleId = scheduleCalendarExclusionDTO.scheduleId;
            exclusionDate = scheduleCalendarExclusionDTO.exclusionDate;
            includeDate = scheduleCalendarExclusionDTO.includeDate;
            day = scheduleCalendarExclusionDTO.day;
            isActive = scheduleCalendarExclusionDTO.isActive;
            createdBy = scheduleCalendarExclusionDTO.createdBy;
            creationDate = scheduleCalendarExclusionDTO.creationDate;
            lastUpdatedBy = scheduleCalendarExclusionDTO.lastUpdatedBy;
            lastUpdateDate = scheduleCalendarExclusionDTO.lastUpdateDate;
            guid = scheduleCalendarExclusionDTO.guid;
            siteId = scheduleCalendarExclusionDTO.siteId;
            synchStatus = scheduleCalendarExclusionDTO.synchStatus;
            masterEntityId = scheduleCalendarExclusionDTO.masterEntityId;
        }

        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("Exclusion Id")]
        [ReadOnly(true)]
        public int ScheduleExclusionId { get { return scheduleExclusionId; } set { scheduleExclusionId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("Schedule Id")]
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the ExclusionDate field
        /// </summary>
        [DisplayName("Exclusion Date")]
        public string ExclusionDate { get { return exclusionDate; } set { exclusionDate = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the Day field
        /// </summary>
        [DisplayName("Day")]
        public int Day { get { return day; } set { day = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the IncludeDate field
        /// </summary>
        [DisplayName("Include Date?")]
        public bool IncludeDate { get { return includeDate; } set { includeDate = value; this.IsChanged = true; } }
        
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
        [DisplayName("SiteId")]
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || scheduleExclusionId < 0;
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
