/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Data Object of the MaintenanceJobStatus class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.100.0    22-Sept-2020   Mushahid Faizan         Created.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Maintenance
{
    public class MaintenanceJobStatusDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by JOB_STATUS_ID
            /// </summary>
            JOB_STATUS_ID,
            /// <summary>
            /// Search by MAINT_CHKLST_DETAIL_ID
            /// </summary>
            MAINT_CHKLST_DETAIL_ID,
            /// <summary>
            /// Search by JOB_STATUS
            /// </summary>
            JOB_STATUS,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
        }

        private int jobStatusId;
        private int maintChklstdetId;
        private string jobStatus;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        /// <summary>
        /// Default Constructor
        /// </summary>
        public MaintenanceJobStatusDTO()
        {
            log.LogMethodEntry();
            jobStatusId = -1;
            maintChklstdetId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        /// <param name="jobStatusId"></param>
        /// <param name="maintChklstdetId"></param>
        /// <param name="assetTypeId"></param>
        /// <param name="userId"></param>
        /// <param name="jobStatus"></param>
        /// <param name="isActive"></param>
        public MaintenanceJobStatusDTO(int jobStatusId, int maintChklstdetId, string jobStatus, bool isActive)
        {
            log.LogMethodEntry(jobStatusId, maintChklstdetId,  jobStatus, isActive);
            this.jobStatusId = jobStatusId;
            this.maintChklstdetId = maintChklstdetId;
            this.jobStatus = jobStatus;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the fields
        /// </summary>
        /// <param name="jobStatusId"></param>
        /// <param name="maintChklstdetId"></param>
        /// <param name="jobStatus"></param>
        /// <param name="isActive"></param>
        /// <param name="createdBy"></param>
        /// <param name="creationDate"></param>
        /// <param name="lastUpdatedBy"></param>
        /// <param name="lastUpdateDate"></param>
        /// <param name="siteId"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="synchStatus"></param>
        /// <param name="guid"></param>
        public MaintenanceJobStatusDTO(int jobStatusId, int maintChklstdetId,  string jobStatus, bool isActive, string createdBy, DateTime creationDate,
                                          string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid)
                                          : this(jobStatusId, maintChklstdetId, jobStatus, isActive)
        {
            log.LogMethodEntry(jobStatusId, maintChklstdetId, jobStatus, isActive,
                                createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the jobStatusId field
        /// </summary>
        public int JobStatusId { get { return jobStatusId; } set { this.IsChanged = true; jobStatusId = value; } }

        /// <summary>
        /// Get/Set method of the maintChklstdetId field
        /// </summary>
        public int MaintChklstdetailId { get { return maintChklstdetId; } set { this.IsChanged = true; maintChklstdetId = value; } }

        /// <summary>
        /// Get/Set method of the jobStatus field
        /// </summary>
        public string JobStatus { get { return jobStatus; } set { this.IsChanged = true; jobStatus = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || jobStatusId < 0;
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
