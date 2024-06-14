/********************************************************************************************
 * Project Name - JOb Task Group DTO
 * Description  - Data object of maintenance task group
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        23-Dec-2015   Raghuveera     Created 
 *2.70        08-Mar-2019   Guru S A       Rename MaintenanceTaskGroupDTO as JobTaskGroupDTO
 *2.70.2        13-Nov-2019   Guru S A       Waiver phase 2 changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// This is the Job task group data object class. This acts as data holder for the JOb task group business object
    /// </summary>
    public class JobTaskGroupDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByMaintenanceTaskGroupParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByJobTaskGroupParameters
        {
            /// <summary>
            /// Search by MAINT_TASK_GROUP_ID field
            /// </summary>
            JOB_TASK_GROUP_ID,
            /// <summary>
            /// Search by TASK_GROUP_NAME field
            /// </summary>
            TASK_GROUP_NAME,
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
            SITE_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            HAS_ACTIVE_TASKS
        }

        int jobTaskGroupId;
        string taskGroupName;
        int masterEntityId;
        bool isActive;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdatedDate;
        string guid;
        int siteId;
        bool synchStatus;

        /// <summary>
        /// Default constructor
        /// </summary>
        public JobTaskGroupDTO()
        {
            log.LogMethodEntry();
            jobTaskGroupId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public JobTaskGroupDTO(int jobTaskGroupId, string taskGroupName, int masterEntityId, bool isActive, string createdBy, DateTime creationDate,
                                        string lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus)
        {
            log.LogMethodEntry(jobTaskGroupId, taskGroupName, masterEntityId, isActive, createdBy, creationDate,
                                         lastUpdatedBy, lastUpdatedDate,  guid, siteId, synchStatus);
            this.jobTaskGroupId = jobTaskGroupId;
            this.taskGroupName = taskGroupName;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the JObTaskGroupId field
        /// </summary>
        [DisplayName("Group Id")]
        [ReadOnly(true)]
        public int JobTaskGroupId { get { return jobTaskGroupId; } set { jobTaskGroupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaskGroupName field
        /// </summary>
        [DisplayName("Group Name")]
        public string TaskGroupName { get { return taskGroupName; } set { taskGroupName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
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
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedUser { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date ")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

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
                    return notifyingObjectIsChanged || jobTaskGroupId < 0;
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
