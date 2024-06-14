/********************************************************************************************
 * Project Name - Maintenance Job DTO
 * Description  - Data object of maintenance job
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        21-01-2016   Raghuveera          Created 
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
    /// This is the maintenance job data object class. This acts as data holder for the maintenance job business object
    /// </summary>
    public class MaintenanceJobSummaryDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByMaintenanceJobParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByMaintenanceJobSummaryParameters
        {
            /// <summary>
            /// Search by MAINT_SCHEDULE_ID field
            /// </summary>
            MAINT_SCHEDULE_ID = 0,
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
            ACTIVE_FLAG = 4
            
        }

        int maintScheduleId;
        string maintJobName;
        DateTime chklstScheduleTime;
        int assignedTo;
        int status;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public MaintenanceJobSummaryDTO()
        {
            log.Debug("Starts-MaintenanceJobSummaryDTO() default constructor.");
            
            log.Debug("Ends-MaintenanceJobSummaryDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MaintenanceJobSummaryDTO(int maintScheduleId, string maintJobName, DateTime chklstScheduleTime, int assignedTo, int status)
        {
            log.Debug("Starts-MaintenanceJobSummaryDTO(with all the data fields) Parameterized constructor.");

            this.maintScheduleId = maintScheduleId;
            this.maintJobName = maintJobName;
            this.chklstScheduleTime = chklstScheduleTime;
            this.assignedTo = assignedTo;
            this.status = status;
            log.Debug("Ends-MaintenanceJobSummaryDTO(with all the data fields) Parameterized constructor.");
        }

        /// <summary>
        /// Get/Set method of the MaintScheduleId field
        /// </summary>
        [DisplayName("Maint Schedule Id")]
        [ReadOnly(true)]
        public int MaintScheduleId { get { return maintScheduleId; } set { maintScheduleId = value; this.IsChanged = true; } }
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
