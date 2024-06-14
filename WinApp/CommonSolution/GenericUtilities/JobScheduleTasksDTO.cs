/********************************************************************************************
 * Project Name - Schedule Asset Task DTO
 * Description  - Data object of Schedule Asset Task
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Dec-2015   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 *2.70        11-Mar-2019   Guru S A            Rename MaintenanceScheduleDTO as JobScheduleDTO
 *2.70.2        25-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor
 *2.70.2        25-Jul-2019   Guru S A            Waiver phase 2 changes
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    public class JobScheduleTasksDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByJobScheduleTaskParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByJobScheduleTaskParameters
        {
            /// <summary>
            /// Search by JOB_SCHEDULE_TASK_ID field
            /// </summary>
            JOB_SCHEDULE_TASK_ID,
            /// <summary>
            /// Search by JOB_SCHEDULE_ID field
            /// </summary>
            JOB_SCHEDULE_ID,
            /// <summary>
            /// Search by ASSET_GROUP_ID field
            /// </summary>
            ASSET_GROUP_ID,
            /// <summary>
            /// Search by ASSET_TYPE_ID field
            /// </summary>
            ASSET_TYPE_ID,
            /// <summary>
            /// Search by ASSET_ID field
            /// </summary>
            ASSET_ID,
            /// <summary>
            /// Search by JOB_TASK_GROUP_ID field
            /// </summary>
            JOB_TASK_GROUP_ID,
            /// <summary>
            /// Search by JOB_TASK_ID field
            /// </summary>
            JOB_TASK_ID,
            /// <summary>
            /// Search by IS_ACTIVE field
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
            /// Search by Booking_ID field
            /// </summary>
            BOOKING_ID,
            /// <summary>
            /// Search by BOOKING_CHECK_LIST_ID field
            /// </summary>
            BOOKING_CHECK_LIST_ID
        }

        int jobScheduleTaskId;
        int jobScheduleId;
        int assetGroupId;
        int assetTypeId;
        int assetID;
        int jobTaskGroupId;
        int jobTaskId;
        bool isActive;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdatedDate;
        string guid;
        int siteId;
        bool synchStatus;
        int masterEntityId;
        int bookingId;
        int bookingCheckListId;
        /// <summary>
        /// Default constructor
        /// </summary>
        public JobScheduleTasksDTO()
        {
            log.LogMethodEntry();
            jobScheduleTaskId = -1;
            jobScheduleId = -1;
            assetGroupId = -1;
            assetTypeId = -1;
            assetID = -1;
            jobTaskGroupId = -1;
            jobTaskId = -1;
            isActive = true;
            masterEntityId = -1;
            bookingId = -1;
            bookingCheckListId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with required fields
        /// </summary>
        public JobScheduleTasksDTO(int jobScheduleTaskId, int jobScheduleId, int assetGroupId, int assetTypeId,
                                    int assetID, int jobTaskGroupId, int jobTaskId, bool isActive)
            :this()
        {
            log.LogMethodEntry(jobScheduleTaskId, jobScheduleId, assetGroupId, assetTypeId,
                                    assetID, jobTaskGroupId, jobTaskId, isActive);
            this.jobScheduleTaskId = jobScheduleTaskId;
            this.jobScheduleId = jobScheduleId;
            this.assetGroupId = assetGroupId;
            this.assetTypeId = assetTypeId;
            this.assetID = assetID;
            this.jobTaskGroupId = jobTaskGroupId;
            this.jobTaskId = jobTaskId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public JobScheduleTasksDTO(int jobScheduleTaskId, int jobScheduleId, int assetGroupId, int assetTypeId,
                                    int assetID, int jobTaskGroupId, int jobTaskId, bool isActive, string createdBy,
                                    DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                    int siteId, bool synchStatus, int masterEntityId, int bookingId, int bookingCheckListId)
            :this(jobScheduleTaskId, jobScheduleId, assetGroupId, assetTypeId,
                                    assetID, jobTaskGroupId, jobTaskId, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid,
                               siteId, synchStatus, masterEntityId, bookingId, bookingCheckListId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.bookingId = bookingId;
            this.bookingCheckListId = bookingCheckListId;
            log.LogMethodExit();
        }

        public JobScheduleTasksDTO(JobScheduleTasksDTO jobScheduleTasksDTO)
         : this()
        {
            log.LogMethodEntry(jobScheduleTaskId, jobScheduleId, assetGroupId, assetTypeId,
                                    assetID, jobTaskGroupId, jobTaskId, isActive, createdBy,
                                    creationDate, lastUpdatedBy, lastUpdatedDate, guid,
                                    siteId, synchStatus, masterEntityId, bookingId, bookingCheckListId);
            jobScheduleTaskId = jobScheduleTasksDTO.jobScheduleTaskId;
            jobScheduleId = jobScheduleTasksDTO.jobScheduleId;
            assetGroupId = jobScheduleTasksDTO.assetGroupId;
            assetTypeId = jobScheduleTasksDTO.assetTypeId;
            assetID = jobScheduleTasksDTO.assetID;
            jobTaskGroupId = jobScheduleTasksDTO.jobTaskGroupId;
            jobTaskId = jobScheduleTasksDTO.jobTaskId;
            bookingId = jobScheduleTasksDTO.bookingId;
            bookingCheckListId = jobScheduleTasksDTO.bookingCheckListId;
            isActive = jobScheduleTasksDTO.isActive;
            createdBy = jobScheduleTasksDTO.createdBy;
            creationDate = jobScheduleTasksDTO.creationDate;
            lastUpdatedBy = jobScheduleTasksDTO.lastUpdatedBy;
            lastUpdatedDate = jobScheduleTasksDTO.lastUpdatedDate;
            guid = jobScheduleTasksDTO.guid;
            siteId = jobScheduleTasksDTO.siteId;
            synchStatus = jobScheduleTasksDTO.synchStatus;
            masterEntityId = jobScheduleTasksDTO.masterEntityId;
        }

        /// <summary>
        /// Get/Set method of the JobScheduleTaskId field
        /// </summary>
        [DisplayName("JOb Schedule Task Id")]
        [ReadOnly(true)]
        public int JobScheduleTaskId { get { return jobScheduleTaskId; } set { jobScheduleTaskId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the JobScheduleId field
        /// </summary>
        [DisplayName("JOb Schedule")]
        public int JobScheduleId { get { return jobScheduleId; } set { jobScheduleId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the assetGroupId field
        /// </summary>
        [DisplayName("Asset Group")]
        public int AssetGroupId { get { return assetGroupId; } set { assetGroupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssetTypeId field
        /// </summary>
        [DisplayName("Asset Type")]
        public int AssetTypeId { get { return assetTypeId; } set { assetTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the assetID field
        /// </summary>
        [DisplayName("Asset")]
        public int AssetID { get { return assetID; } set { assetID = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the JObTaskGroupId field
        /// </summary>
        [DisplayName("JOb Task Group")]
        public int JObTaskGroupId { get { return jobTaskGroupId; } set { jobTaskGroupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaintTaskId field
        /// </summary>
        [DisplayName("Job Task")]
        public int JobTaskId { get { return jobTaskId; } set { jobTaskId = value; this.IsChanged = true; } }
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
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }
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
        /// Get/Set method of the BookingId field
        /// </summary>
        [DisplayName("Booking Id")]
        public int BookingId { get { return bookingId; } set { bookingId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the BookingCheckListId field
        /// </summary>
        [DisplayName("Booking CheckList Id")]
        public int BookingCheckListId { get { return bookingCheckListId; } set { bookingCheckListId = value; this.IsChanged = true; } }
        /// <summary>
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
                    return notifyingObjectIsChanged || jobScheduleTaskId < 0;
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
