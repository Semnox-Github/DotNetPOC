/********************************************************************************************
 * Project Name - Job Schedule DTO
 * Description  - Data object of job schedule DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        5-Feb-2016   Raghuveera          Created 
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
    /// This is the job schedule data object class. This acts as data holder for the job schedule business object
    /// </summary>
    public class JobScheduleDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int jobId;
        string jobName;
        string isActive;
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
        public JobScheduleDTO()
        {
            log.Debug("Starts-JobScheduleDTO() default constructor.");
            jobId = -1;
            siteId = -1;
            log.Debug("Ends-JobScheduleDTO() default constructor.");
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public JobScheduleDTO(int jobId, string jobName, DateTime lastSuccessfulRunTime, string isActive, string createdBy,
                                DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus)
        {
            log.Debug("Starts-ScheduledJobDTO(with all the data fields) Parameterized constructor.");
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
            log.Debug("Ends-ScheduledJobDTO(with all the data fields) Parameterized constructor.");
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
        public string IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
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
