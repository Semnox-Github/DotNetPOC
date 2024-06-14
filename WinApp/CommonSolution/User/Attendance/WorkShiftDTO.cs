/********************************************************************************************
 * Project Name - User
 * Description  - Data object of WorkShift
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        27-May-2019   Divya A                 Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the WorkShift data object class. This acts as data holder for the WorkShift business object
    /// </summary>
    public class WorkShiftDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByWorkShiftParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByWorkShiftParameters
        {
            /// <summary>
            /// Search by WorkShift Id field
            /// </summary>
            WORK_SHIFT_ID,
            /// <summary>
            /// Search by Name field
            /// </summary>
            NAME,
            /// <summary>
            /// Search by StartDate field
            /// </summary>
            STARTDATE,
            /// <summary>
            /// Search by end tDate field
            /// </summary>
            ENDDATE,
            /// <summary>
            /// Search by Frequency field
            /// </summary>
            FREQUENCY,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Status field
            /// </summary>
            STATUS,
            /// <summary>
            /// Search by master entity id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by is active field
            /// </summary>
            IS_ACTIVE
        }

        private int workShiftId;
        private string name;
        private DateTime startDate;
        private string frequency;
        private string status;
        private DateTime endDate;
        private string weekSchedule;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private List<WorkShiftScheduleDTO> workShiftScheduleDTOList;
        private List<WorkShiftUserDTO> workShiftUserDTOList;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor of WorkShiftDTO
        /// </summary>
        public WorkShiftDTO()
        {
            log.LogMethodEntry();
            workShiftId = -1;
            siteId = -1;
            masterEntityId = -1;
            workShiftScheduleDTOList = new List<WorkShiftScheduleDTO>();
            workShiftUserDTOList = new List<WorkShiftUserDTO>();
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of WorkShiftDTO with required data fields
        /// </summary>
        public WorkShiftDTO(int workShiftId, string name, DateTime startDate, string frequency, string status, 
                         DateTime endDate, string weekSchedule,bool isActive)
            : this()
        {
            log.LogMethodEntry(workShiftId, name, startDate, frequency, status, endDate, weekSchedule,
                 isActive);
            this.workShiftId = workShiftId;
            this.name = name;
            this.startDate = startDate;
            this.frequency = frequency;
            this.status = status;
            this.endDate = endDate;
            this.weekSchedule = weekSchedule;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of WorkShiftDTO with all the data fields
        /// </summary>
        public WorkShiftDTO(int workShiftId, string name, DateTime startDate, string frequency, string status,
                         DateTime endDate, string weekSchedule, int siteId, string guid, bool synchStatus, int masterEntityId,
                         string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,bool isActive)
            : this(workShiftId, name, startDate, frequency, status, endDate, weekSchedule, isActive)
        {
            log.LogMethodEntry(siteId, guid, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
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
        /// Get/Set method of the WorkShiftId field
        /// </summary>
        public int WorkShiftId { get { return workShiftId; } set { workShiftId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { name = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the StartDate field
        /// </summary>
        public DateTime StartDate { get { return startDate; } set { startDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Frequency field
        /// </summary>
        public string Frequency { get { return frequency; } set { frequency = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        public string Status { get { return status; } set { status = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the EndDate field
        /// </summary>
        public DateTime EndDate { get { return endDate; } set { endDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the WeekSchedule field
        /// </summary>
        public string WeekSchedule { get { return weekSchedule; } set { weekSchedule = value; IsChanged = true; } }
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
        /// Get/Set method of the workShiftScheduleDTOList List
        /// </summary>
        public List<WorkShiftScheduleDTO> WorkShiftScheduleDTOList { get { return workShiftScheduleDTOList; } set { workShiftScheduleDTOList = value; } }
        /// <summary>
        /// Get/Set method of the WorkShiftUsersDTOList List
        /// </summary>
        public List<WorkShiftUserDTO> WorkShiftUsersDTOList { get { return workShiftUserDTOList; } set { workShiftUserDTOList = value; } }

        /// <summary>
        /// Returns whether the workShiftDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (workShiftScheduleDTOList != null &&
                    workShiftScheduleDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (workShiftUserDTOList != null &&
                    workShiftUserDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

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
                    return notifyingObjectIsChanged || workShiftId < 0;
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
