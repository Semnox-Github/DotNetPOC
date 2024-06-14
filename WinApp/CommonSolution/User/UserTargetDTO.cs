/********************************************************************************************
 * Project Name - Reports
 * Description  - Data object of UserTarget
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 ********************************************************************************************* 
 *2.80        31-May-2020   Vikas Dwivedi       Created
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the User Target data object class. This acts as data holder for the User Target business object
    /// </summary>
    public class UserTargetDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByUserTargetSearchParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByUserTargetSearchParameters
        {
            /// <summary>
            /// Search by USER TARGET ID field
            /// </summary>
            USER_TARGET_ID,
            /// <summary>
            /// Search by GAME ID field
            /// </summary>
            GAME_ID,
            /// <summary>
            /// Search by PERIOD ID field
            /// </summary>
            PERIOD_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int userTargetId;
        private int gameId;
        private int periodId;
        private int target;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserTargetDTO()
        {
            log.LogMethodEntry();
            userTargetId = -1;
            gameId = -1;
            periodId = -1;
            target = -1;
            guid = string.Empty;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public UserTargetDTO(int userTargetId, int gameId, int periodId, int target, bool isActive)
            : this()
        {
            log.LogMethodEntry(userTargetId, gameId, periodId, target);
            this.userTargetId = userTargetId;
            this.gameId = gameId;
            this.periodId = periodId;
            this.target = target;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public UserTargetDTO(int userTargetId, int gameId, int periodId, int target, string lastUpdatedBy, DateTime lastUpdatedDate,
                                    string guid, int siteId, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, bool isActive)
            : this(userTargetId, gameId, periodId, target, isActive)
        {
            log.LogMethodEntry(userTargetId, gameId, periodId, target, lastUpdatedBy, lastUpdatedDate,
                                guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, isActive);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the UserTargetId field
        /// </summary>
        public int UserTargetId
        {
            get { return userTargetId; }
            set { userTargetId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the GameId field
        /// </summary>
        public int GameId
        {
            get { return gameId; }
            set { gameId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the PeriodId field
        /// </summary>
        public int PeriodId
        {
            get { return periodId; }
            set { periodId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Target field
        /// </summary>
        public int Target
        {
            get { return target; }
            set { target = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }
        
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary> 
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged || userTargetId < 0;
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
