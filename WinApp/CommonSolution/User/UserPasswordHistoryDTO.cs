/********************************************************************************************
 * Project Name - User
 * Description  - Data object of UserPasswordHistory
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        27-May-2020   Girish Kundar               Created 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the UserPasswordHistoryDTO data object class. This acts as data holder for the UserPasswordHistory business object
    /// </summary>
   public class UserPasswordHistoryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByWorkShiftScheduleParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByUserPasswordHistoryParameters
        {
            /// <summary>
            /// Search by userPasswordHistory Id field
            /// </summary>
            USER_PASSWORD_HISTORY_ID,
            /// <summary>
            /// Search by changeDate Id field
            /// </summary>
            CHANGE_DATE,
            /// <summary>
            /// Search by userId field
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int userPasswordHistoryId;
        private int userId;
        private byte[] passwordHash;
        private DateTime changeDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private string passwordSalt;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor for UserPasswordHistoryDTO
        /// </summary>
        public UserPasswordHistoryDTO()
        {
            log.LogMethodEntry();
            userPasswordHistoryId = -1;
            userId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for UserPasswordHistoryDTO with required fields
        /// </summary>
        public UserPasswordHistoryDTO(int userPasswordHistoryId, int userId, byte[] passwordHash, DateTime changeDate, string passwordSalt) 
            : this()
        {
            log.LogMethodEntry(userPasswordHistoryId, userId, passwordHash, changeDate, passwordSalt);
            this.userPasswordHistoryId = userPasswordHistoryId;
            this.userId = userId;
            this.passwordHash = passwordHash;
            this.changeDate = changeDate;
            this.passwordSalt = passwordSalt;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for UserPasswordHistoryDTO with all fields
        /// </summary>
        public UserPasswordHistoryDTO(int userPasswordHistoryId, int userId, byte[] passwordHash, DateTime changeDate, int siteId, 
            string guid, bool synchStatus, string passwordSalt, int masterEntityId, string createdBy, DateTime creationDate, 
            string lastUpdatedBy, DateTime lastUpdateDate) 
            : this(userPasswordHistoryId, userId, passwordHash, changeDate, passwordSalt)
        {
            log.LogMethodEntry();
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
        /// Get/Set method of the UserPasswordHistoryId field
        /// </summary>
        public int UserPasswordHistoryId { get { return userPasswordHistoryId; } set { userPasswordHistoryId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        public int UserId { get { return userId; } set { userId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PasswordHash field
        /// </summary>
        public byte[] PasswordHash { get { return passwordHash; } set { passwordHash = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ChangeDate field
        /// </summary>
        public DateTime ChangeDate { get { return changeDate; } set { changeDate = value; IsChanged = true; } }
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
        /// Get/Set method of the PasswordSalt field
        /// </summary>
        public string PasswordSalt { get { return passwordSalt; } set { passwordSalt = value; IsChanged = true; } }
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
                    return notifyingObjectIsChanged || userPasswordHistoryId < 0;
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
