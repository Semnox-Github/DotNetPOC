/********************************************************************************************
 * Project Name - Scheduled Job DTO
 * Description  - Data object of scheduled job DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By         Remarks          
 *********************************************************************************************
 *1.00        4-Feb-2016     Raghuveera          Created 
 *2.70.2        19-Sep-2019    Dakshakh            Modified : Added logs
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the scheduled job data object class. This acts as data holder for the scheduled job business object
    /// </summary>
    public class ScheduledJobDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int scheduleId;
        int maintScheduleId;
        string maintJobType;
        string maintJobName;
        string assignedTo;
        int assignedUserId;
        int departmentId;
        int durationToComplete;
        DateTime chklstScheduleTime;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdatedDate;
        /// <summary>
        /// Default constructor
        /// </summary>
        public ScheduledJobDTO()
        {
            log.LogMethodEntry();
            scheduleId = -1;
            maintScheduleId = -1;
            assignedUserId = -1;
            departmentId = -1;
            durationToComplete = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ScheduledJobDTO(int scheduleId, int maintScheduleId, string maintJobType, string maintJobName,
                                string assignedTo, int assignedUserId, int departmentId, int durationToComplete,
                                DateTime chklstScheduleTime, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate)
        {
            log.LogMethodEntry(scheduleId, maintScheduleId, maintJobType, maintJobName, assignedTo, assignedUserId, departmentId, durationToComplete,
                               chklstScheduleTime, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate);
            this.scheduleId = scheduleId;
            this.maintScheduleId = maintScheduleId;
            this.maintJobName = maintJobName;
            this.maintJobType = maintJobType;
            this.chklstScheduleTime = chklstScheduleTime;
            this.assignedTo = assignedTo;
            this.assignedUserId = assignedUserId;
            this.departmentId = departmentId;
            this.durationToComplete = durationToComplete;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("Schedule Id")]
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaintScheduleId field
        /// </summary>
        [DisplayName("Maint Schedule Id")]
        public int MaintScheduleId { get { return maintScheduleId; } set { maintScheduleId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaintJobType field
        /// </summary>
        [DisplayName("Maint Job Type")]
        public string MaintJobType { get { return maintJobType; } set { maintJobType = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaintJobName field
        /// </summary>
        [DisplayName("Maint Job Name")]
        public string MaintJobName { get { return maintJobName; } set { maintJobName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssignedTo field
        /// </summary>
        [DisplayName("Assigned To")]
        public string AssignedTo { get { return assignedTo; } set { assignedTo = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssignedUserId field
        /// </summary>
        [DisplayName("Assigned UserId")]
        public int AssignedUserId { get { return assignedUserId; } set { assignedUserId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DepartmentId field
        /// </summary>
        [DisplayName("Department Id")]
        public int DepartmentId { get { return departmentId; } set { departmentId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DurationToComplete field
        /// </summary>
        [DisplayName("Duration To Complete")]
        public int DurationToComplete { get { return durationToComplete; } set { durationToComplete = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ChklstScheduleTime field
        /// </summary>
        [DisplayName("Chklst Schedule Time")]
        public DateTime ChklstScheduleTime { get { return chklstScheduleTime; } set { chklstScheduleTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("creation Date")]
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || scheduleId < 0;
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
