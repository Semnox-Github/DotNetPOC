/********************************************************************************************
 * Project Name - User
 * Description  - Data object of WorkShiftSchedule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        27-May-2019   Divya A                 Created 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the WorkShiftSchedule data object class. This acts as data holder for the WorkShiftSchedule business object
    /// </summary>
    public class WorkShiftScheduleDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByWorkShiftScheduleParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByWorkShiftScheduleParameters
        {
            /// <summary>
            /// Search by Work Shift Schedule Id field
            /// </summary>
            WORK_SHIFT_SCHEDULE_ID,
            /// <summary>
            /// Search by Work Shift Id field
            /// </summary>
            WORK_SHIFT_ID,
            /// <summary>
            /// Search by Work Shift Id field
            /// </summary>
            WORK_SHIFT_ID_LIST,
            /// <summary>
            /// Search by Sequence field
            /// </summary>
            SEQUENCE,
            /// <summary>
            /// Search by Start Time field
            /// </summary>
            START_TIME,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by master entity Id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by is active field
            /// </summary>
            IS_ACTIVE
        }

        private int workShiftScheduleId;
        private int workShiftId;
        private int sequence;
        private DateTime startTime;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor of WorkShiftScheduleDTO with required fields
        /// </summary>
        public WorkShiftScheduleDTO()
        {
            log.LogMethodEntry();
            workShiftScheduleId = -1;
            workShiftId = -1;
            sequence = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of WorkShiftScheduleDTO with required fields
        /// </summary>
        public WorkShiftScheduleDTO(int workShiftScheduleId, int workShiftId, int sequence, DateTime startTime,bool isActive) 
            : this()
        {
            log.LogMethodEntry(workShiftScheduleId, workShiftId, sequence, startTime, isActive);
            this.workShiftScheduleId = workShiftScheduleId;
            this.workShiftId = workShiftId;
            this.sequence = sequence;
            this.startTime = startTime;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of WorkShiftScheduleDTO with all the fields.
        /// </summary>
        public WorkShiftScheduleDTO(int workShiftScheduleId, int workShiftId, int sequence, DateTime startTime, 
                            int siteId, string guid, bool synchStatus, int masterEntityId,  string createdBy, 
                            DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,bool isActive)
            : this(workShiftScheduleId, workShiftId, sequence, startTime, isActive)
        {
            log.LogMethodEntry(workShiftScheduleId, workShiftId, sequence, startTime ,guid, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, isActive);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the WorkShiftScheduleId field
        /// </summary>
        public int WorkShiftScheduleId { get { return workShiftScheduleId; } set { workShiftScheduleId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the WorkShiftId field
        /// </summary>
        public int WorkShiftId { get { return workShiftId; } set { workShiftId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Sequence field
        /// </summary>
        public int Sequence { get { return sequence; } set { sequence = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the StartTime field
        /// </summary>
        public DateTime StartTime { get { return startTime; } set { startTime = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }
        public bool IsActive { get { return isActive; } set { isActive = value; IsChanged = true; } }
        
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
                    return notifyingObjectIsChanged || workShiftScheduleId < 0;
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
