/********************************************************************************************
 * Project Name - Schedule Exclusion DTO
 * Description  - Data object of Schedule Exclusion
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        30-Dec-2015   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the schedule exclusion data object class. This acts as data holder for the schedule exclusion business object
    /// </summary>
    public class ScheduleExclusionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByScheduleExclusionParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByScheduleExclusionParameters
        {
            /// <summary>
            /// Search by SCHEDULE_EXCLUSION_ID field
            /// </summary>
            SCHEDULE_EXCLUSION_ID = 0,
            /// <summary>
            /// Search by SCHEDULE_ID field
            /// </summary>
            SCHEDULE_ID = 1,
            /// <summary>
            /// Search by EXCLUSION_DATE field
            /// </summary>
            EXCLUSION_DATE = 2,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            IS_ACTIVE = 3,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 4,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 5
        }

        int scheduleExclusionId;
        int scheduleId;
        string exclusionDate;
        string includeDate;
        int day;
        bool isActive;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdateDate;
        string guid;
        int siteId;
        bool synchStatus;
        int masterEntityId;//Modification on 18-Jul-2016 for publish feature

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScheduleExclusionDTO()
        {
            log.LogMethodEntry();
            scheduleExclusionId = -1;
            scheduleId = -1;
            day = -1;
            isActive = true;
            IncludeDate = "N";
            masterEntityId = -1;//Modification on 18-Jul-2016 for publish feature
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ScheduleExclusionDTO(int scheduleExclusionId, int scheduleId, string exclusionDate,
                                    string includeDate, int day, bool isActive, string createdBy,
                                    DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid,
                                    int siteId, bool synchStatus, int masterEntityId)//Modification on 18-Jul-2016 for publish feature
        {
            log.LogMethodEntry(scheduleExclusionId,  scheduleId,  exclusionDate,
                                     includeDate,  day,  isActive,  createdBy,
                                     creationDate,  lastUpdatedBy,  lastUpdateDate,  guid,
                                     siteId,  synchStatus,  masterEntityId);
            this.scheduleExclusionId = scheduleExclusionId;
            this.scheduleId = scheduleId;
            this.exclusionDate = exclusionDate;
            this.includeDate = includeDate;
            this.day = day;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;//Modification on 18-Jul-2016 for publish feature
            log.LogMethodExit();
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
        public string IncludeDate { get { return includeDate; } set { includeDate = value; this.IsChanged = true; } }
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
        public string CreatedBy { get { return createdBy; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdateDate { get { return lastUpdateDate; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
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
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }//Ends:Modification on 18-Jul-2016 for publish feature


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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }
    }
}
