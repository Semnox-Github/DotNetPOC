/********************************************************************************************
 * Project Name - User Job Items Summary DTO
 * Description  - Data object of user job items
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        21-01-2016    Raghuveera     Created 
 *2.70        08-Mar-2019   Guru S A       Renamed MaintenanceJobSummaryDTO as UserJobItemsSummaryDTO
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
    /// This is the user job items data object class. This acts as data holder for the user job items business object
    /// </summary>
    public class UserJobItemsSummaryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByMaintenanceJobParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByUserJobItemsSummaryParameters
        {
            /// <summary>
            /// Search by MAINT_SCHEDULE_ID field
            /// </summary>
            JOB_SCHEDULE_ID = 0,
            /// <summary>
            /// Search by JOB_NAME field
            /// </summary>
            JOB_NAME = 1,
            /// <summary>
            /// Search by STATUS field
            /// </summary>
            STATUS = 2,
            /// <summary>
            /// Search by ASSIGNED_TO field
            /// </summary>
            ASSIGNED_TO = 3,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            IS_ACTIVE = 4 
        }

        int jobScheduleId;
        string maintJobName;
        DateTime chklstScheduleTime;
        int assignedTo;
        int status;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public UserJobItemsSummaryDTO()
        {
            log.LogMethodEntry();
            jobScheduleId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public UserJobItemsSummaryDTO(int jobScheduleId, string maintJobName, DateTime chklstScheduleTime, int assignedTo, int status)
        {
            log.LogMethodEntry(jobScheduleId,  maintJobName, chklstScheduleTime, assignedTo, status);
            this.jobScheduleId = jobScheduleId;
            this.maintJobName = maintJobName;
            this.chklstScheduleTime = chklstScheduleTime;
            this.assignedTo = assignedTo;
            this.status = status;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MaintScheduleId field
        /// </summary>
        [DisplayName("JOb Schedule Id")]
        [ReadOnly(true)]
        public int JobScheduleId { get { return jobScheduleId; } set { jobScheduleId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaintJobName field
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("Job Name")]
        public string MaintJobName { get { return maintJobName; } set { maintJobName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ChklstScheduleTime field
        /// </summary>
        [DisplayName("Schedule Date")]
        [ReadOnly(true)]
        public DateTime ChklstScheduleTime { get { return chklstScheduleTime; } set { chklstScheduleTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssignedTo field
        /// </summary>
        [DisplayName("Assigned To")]
        [ReadOnly(true)]
        public int AssignedTo { get { return assignedTo; } set { assignedTo = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        [DisplayName("Status")]
        [ReadOnly(true)]
        public int Status { get { return status; } set { status = value; this.IsChanged = true; } }
        
        
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
                    return notifyingObjectIsChanged|| jobScheduleId < 0;
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
