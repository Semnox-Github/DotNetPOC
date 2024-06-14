/********************************************************************************************
 * Project Name - Utilities
 * Description  - DTO of Task Types
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        11-Mar-2019   Jagan Mohana          Created 
              08-Apr-2019   Mushahid Faizan      Modified- Added log Method Entry & Exit
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.Utilities
{
    public class TaskTypesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by TASK_TYPE_ID field
            /// </summary>
            TASK_TYPE_ID,
            /// <summary>
            /// Search by TASK_TYPE field
            /// </summary>
            TASK_TYPE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID
        }

        int taskTypeId;
        string taskType;
        string requiresManagerApproval;
        string displayInPos;
        string taskTypeName;
        int siteId;
        bool synchStatus;
        int masterEntityId;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdateDate;

        /// <summary>
        /// Default constructor
        /// Added siteId = -1 on 08-Apr-2019 by Mushahid Faizan
        /// </summary>
        public TaskTypesDTO()
        {
            log.LogMethodEntry();
            this.taskTypeId = -1;
            this.masterEntityId = -1;
            this.siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TaskTypesDTO(int taskTypeId, string taskType, string requiresManagerApproval, string displayInPos, string taskTypeName, int siteId, bool synchStatus,
                            int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
        {
            log.LogMethodEntry(taskTypeId, taskType, requiresManagerApproval, displayInPos, taskTypeName, siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.taskTypeId = taskTypeId;
            this.taskType = taskType;
            this.requiresManagerApproval = requiresManagerApproval;
            this.displayInPos = displayInPos;
            this.taskTypeName = taskTypeName;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the TaskTypeId field
        /// </summary>
        [DisplayName("Task Type Id")]
        [ReadOnly(true)]
        public int TaskTypeId { get { return taskTypeId; } set { taskTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaskType field
        /// </summary>
        [DisplayName("Task Type")]
        public string TaskType { get { return taskType; } set { taskType = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RequiresManagerApproval field
        /// </summary>
        [DisplayName("Requires Manager Approval")]
        public string RequiresManagerApproval { get { return requiresManagerApproval; } set { requiresManagerApproval = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DisplayInPOS field
        /// </summary>
        [DisplayName("Display In Pos")]
        public string DisplayInPOS { get { return displayInPos; } set { displayInPos = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaskTypeName field
        /// </summary>
        [DisplayName("Task Type Name")]
        public string TaskTypeName { get { return taskTypeName; } set { taskTypeName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SyncStatus field
        /// </summary>
        [DisplayName("SyncStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || taskTypeId < 0;
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

