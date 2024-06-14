/********************************************************************************************
 * Project Name - Maintenance Schedule DTO
 * Description  - Data object of Maintenance Schedule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Dec-2015   Raghuveera          Created 
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
    /// This is the Maintenance schedule data object class. This acts as data holder for the Maintenance schedule business object
    /// </summary>
    public class MaintenanceScheduleDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
         Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByMaintenanceScheduleParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByMaintenanceScheduleParameters
        {
            /// <summary>
            /// Search by MAINT_SCHEDULE_ID field
            /// </summary>
            MAINT_SCHEDULE_ID = 0,
            /// <summary>
            /// Search by SCHEDULE_ID field
            /// </summary>
            SCHEDULE_ID = 1,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG = 2,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 3,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 4
        }

        int maintScheduleId;
        int scheduleId;
        int userId;
        int departmentId;
        int durationToComplete;
        DateTime maxValueJobCreated;
        string isActive;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdatedDate;
        string guid;
        int siteId;
        bool synchStatus;
        int masterEntityId;//Modification on 18-Jul-2016 for publish feature

        /// <summary>
        /// Default constructor
        /// </summary>
        public MaintenanceScheduleDTO()
        {
            log.Debug("Starts-MaintenanceScheduleDTO() default constructor.");
            maintScheduleId = -1;
            scheduleId = -1;
            departmentId = -1;
            userId = -1;
            isActive = "Y";
            masterEntityId = -1;//Modification on 18-Jul-2016 for publish feature
            log.Debug("Ends-MaintenanceScheduleDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MaintenanceScheduleDTO(int maintScheduleId, int scheduleId, int userId, int departmentId, int durationToComplete, DateTime maxValueJobCreated,
                                       string isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                       DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId)//Modification on 18-Jul-2016 for publish feature
        {
            log.Debug("Starts-MaintenanceScheduleDTO(with all the data fields) Parameterized constructor.");
            this.maintScheduleId = maintScheduleId;
            this.scheduleId = scheduleId;
            this.userId = userId;
            this.departmentId = departmentId;
            this.durationToComplete = durationToComplete;
            this.maxValueJobCreated = maxValueJobCreated;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;//Modification on 18-Jul-2016 for publish feature
            log.Debug("Ends-MaintenanceScheduleDTO(with all the data fields) Parameterized constructor.");
        }


        /// <summary>
        /// Get/Set method of the MaintScheduleId field
        /// </summary>
        [DisplayName("MaintSchedule Id")]
        [ReadOnly(true)]
        public int MaintScheduleId { get { return maintScheduleId; } set { maintScheduleId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("Schedule Id")]
        [Browsable(false)]
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("Assigned To")]
        public int UserId { get { return userId; } set { userId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DepartmentId field
        /// </summary>
        [DisplayName("Department")]
        public int DepartmentId { get { return departmentId; } set { departmentId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DurationToComplete field
        /// </summary>
        [DisplayName("Duration To Complete")]
        public int DurationToComplete { get { return durationToComplete; } set { durationToComplete = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaxValueJobCreated field
        /// </summary>
        [DisplayName("Max Value Job Created")]
        [Browsable(false)]
        public DateTime MaxValueJobCreated { get { return maxValueJobCreated; } set { maxValueJobCreated = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public string IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
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
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } }
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
