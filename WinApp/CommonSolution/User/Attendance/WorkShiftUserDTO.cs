/********************************************************************************************
 * Project Name - User
 * Description  - Data object of WorkShiftUsers
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
    /// This is the WorkShiftUsers data object class. This acts as data holder for the WorkShiftUser business object
    /// </summary>
    public class WorkShiftUserDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByWorkShiftScheduleParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByWorkShiftUserParameters
        {
            /// <summary>
            /// Search by Work Shift Users Id field
            /// </summary>
            WORK_SHIFT_USERS_ID,
            /// <summary>
            /// Search by Work Shift Id field
            /// </summary>
            WORK_SHIFT_ID,
            /// <summary>
            /// Search by Work Shift Id field
            /// </summary>
            WORK_SHIFT_ID_LIST,
            /// <summary>
            /// Search by User Id field
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master Entity Id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by is active field
            /// </summary>
            IS_ACTIVE
        }

        private int id;
        private int workShiftId;
        private int userId;
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
        /// Default constructor of WorkShiftUsersDTO
        /// </summary>
        public WorkShiftUserDTO()
        {
            log.LogMethodEntry();
            Id = -1;
            workShiftId = -1;
            userId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of WorkShiftUsersDTO with required fields
        /// </summary>
        /// <param name="workShiftUsersId">workShiftUsersId</param>
        /// <param name="workShiftId">workShiftId</param>
        /// <param name="userId">userId</param>
        public WorkShiftUserDTO(int workShiftUsersId, int workShiftId, int userId,bool isActive)
            : this()
        {
            log.LogMethodEntry(workShiftUsersId, workShiftId, userId);
            this.Id = workShiftUsersId;
            this.workShiftId = workShiftId;
            this.userId = userId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of WorkShiftUsersDTO with all fields
        /// </summary>
        public WorkShiftUserDTO(int id, int workShiftId, int userId, int siteId, string guid, bool synchStatus, 
                     int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,bool isActive)
            : this(id, workShiftId, userId,isActive)
        {
            log.LogMethodEntry(id, workShiftId, userId, isActive,siteId, guid, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
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
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { id = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the WorkShiftId field
        /// </summary>
        public int WorkShiftId { get { return workShiftId; } set { workShiftId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        public int UserId { get { return userId; } set { userId = value; IsChanged = true; } }
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
                    return notifyingObjectIsChanged || id < 0;
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
