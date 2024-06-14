/********************************************************************************************
 * Project Name - Job Schedule DTO
 * Description  - Data object of job schedule DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        5-Feb-2016   Raghuveera      Created 
 *2.70        11-Mar-2019   Guru S A       Rename JobScheduleDTO as JobSchedulerDTO
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the job scheduler data object class. This acts as data holder for the job scheduler business object
    /// </summary>
    public class JobSchedulerDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int jobId;
        string jobName;
        bool isActive;
        DateTime lastSuccessfulRunTime;
        string guid;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdatedDate;
        int siteId;
        bool synchStatus;
        /// <summary>
        /// Default constructor
        /// </summary>
        public JobSchedulerDTO()
        {
            log.LogMethodEntry();
            jobId = -1;
            siteId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public JobSchedulerDTO(int jobId, string jobName, DateTime lastSuccessfulRunTime, bool isActive, string createdBy,
                                DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus)
        {
            log.LogMethodEntry(jobId, jobName, lastSuccessfulRunTime, isActive, createdBy,
                                creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus);
            this.jobId = jobId;
            this.jobName = jobName;
            this.isActive = isActive;
            this.lastSuccessfulRunTime=lastSuccessfulRunTime;
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
        /// Get/Set method of the JobId field
        /// </summary>
        [DisplayName("Job Id")]
        [ReadOnly(true)]
        public int JobId { get { return jobId; } set { jobId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the JobName field
        /// </summary>
        [DisplayName("Job Name")]
        public string JobName { get { return jobName; } set { jobName = value; this.IsChanged = true; } }        
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastSuccessfulRunTime field
        /// </summary>
        [DisplayName("Last Successful RunTime")]
        public DateTime LastSuccessfulRunTime { get { return lastSuccessfulRunTime; } set { lastSuccessfulRunTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || jobId < 0;
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
