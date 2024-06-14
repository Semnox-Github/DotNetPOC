/********************************************************************************************
 * Project Name - Schedule DTO
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
    /// This is the schedule data object class. This acts as data holder for the schedule business object
    /// </summary>
    public class ScheduleDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByScheduleParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByScheduleParameters
        {
            /// <summary>
            /// Search by SCHEDULE_ID field
            /// </summary>
            SCHEDULE_ID = 0,
            /// <summary>
            /// Search by ASSET_GROUP_ID field
            /// </summary>
            SCHEDULE_NAME = 1,
            /// <summary>
            /// Search by SCHEDULE_TIME field
            /// </summary>
            SCHEDULE_TIME = 2,
            /// <summary>
            /// Search by RECUR_END_DATE field
            /// </summary>
            RECUR_END_DATE = 3,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            IS_ACTIVE = 4,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 5,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 6
        }

        int scheduleId;
        string scheduleName;
        DateTime scheduleTime;
        DateTime scheduleEndDate;
        string recurFlag;
        string recurFrequency;
        DateTime recurEndDate;
        string recurType;
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
        public ScheduleDTO()
        {
            log.LogMethodEntry();
            scheduleId = -1;
            scheduleEndDate = DateTime.MinValue;
            isActive = true;
            recurFlag = "N";
            masterEntityId = -1;//Modification on 18-Jul-2016 for publish feature
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ScheduleDTO(int scheduleId, string scheduleName, DateTime scheduleTime, DateTime scheduleEndDate, string recurFlag,
                           string recurFrequency, DateTime recurEndDate, string recurType, bool isActive, string createdBy,
                           DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid,
                           int siteId, bool synchStatus, int masterEntityId)//Modification on 18-Jul-2016 for publish feature
        {
            log.LogMethodEntry(scheduleId, scheduleName, scheduleTime, scheduleEndDate, recurFlag,
                            recurFrequency, recurEndDate, recurType,  isActive,  createdBy,
                            creationDate,  lastUpdatedBy,  lastUpdateDate,  guid,
                            siteId,  synchStatus,  masterEntityId);
            this.scheduleId = scheduleId;
            this.scheduleName = scheduleName;
            this.scheduleTime = scheduleTime;
            this.scheduleEndDate = scheduleEndDate;
            this.recurFlag = recurFlag;
            this.recurFrequency = recurFrequency;
            this.recurEndDate = recurEndDate;
            this.recurType = recurType;
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
        public string RecurFlag { get { return recurFlag; } set { recurFlag =value; this.IsChanged = true; } }
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
