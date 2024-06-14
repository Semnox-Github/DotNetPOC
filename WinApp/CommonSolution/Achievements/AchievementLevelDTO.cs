/********************************************************************************************
 * Project Name - Achievements
 * Description  - Data Handler -AchievementLevel
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        03-JUl-2019   Deeksha                 Modified : Added new Constructor with required fields
 *                                                             Added CreaedBy and CreationDate field.
 *                                                             changed log.debug to log.logMethodEntry
 *                                                             and log.logMethodExit
 *2.80        04-Mar-2020   Vikas Dwivedi           Modified as per the Standards for Phase 1 changes.
 ********************************************************************************************/
using System;
namespace Semnox.Parafait.Achievements

{
    public class AchievementLevelDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by CARD ID field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by ACHIEVEMENT CLASS LEVEL ID field
            /// </summary>
            ACHIEVEMENT_CLASS_LEVEL_ID,
            /// <summary>
            /// Search by ACHIEVEMENT CLASS LEVEL ID LIST field
            /// </summary>
            ACHIEVEMENT_CLASS_LEVEL_ID_LIST,
            /// <summary>
            /// Search by ISVALID field
            /// </summary>
            ISVALID,
            /// <summary>
            /// Search by EFFECTIVE DATE field
            /// </summary>
            EFFECTIVE_DATE,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITEID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int id;
        private int cardId;
        private int achievementClassLevelId;
        private bool isValid;
        private bool isActive;
        private DateTime effectiveDate;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int siteId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AchievementLevelDTO()
        {
            log.LogMethodEntry();
            id = -1;
            cardId = -1;
            achievementClassLevelId = -1;
            isActive = true;
            guid = string.Empty;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public AchievementLevelDTO(int id, int cardId, int achievementClassLevelId, bool isValid,
            bool isActive, DateTime effectiveDate)
            : this()
        {
            log.LogMethodEntry(id, cardId, achievementClassLevelId, isValid, effectiveDate);
            this.id = id;
            this.cardId = cardId;
            this.achievementClassLevelId = achievementClassLevelId;
            this.effectiveDate = effectiveDate;
            this.isValid = isValid;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public AchievementLevelDTO(int id, int cardId, int achievementClassLevelId, bool isValid, bool isActive, DateTime effectiveDate,
                                    DateTime lastUpdatedDate, string lastUpdatedUser, string guid, bool synchStatus,
                                    int masterEntityId, int siteId, string createdBy, DateTime creationDate)
         : this(id, cardId, achievementClassLevelId, isValid, isActive, effectiveDate)
        {
            log.LogMethodEntry(id, cardId, achievementClassLevelId, isValid, isActive, effectiveDate, lastUpdatedDate, lastUpdatedUser,
                                 guid, synchStatus, masterEntityId, siteId, createdBy, creationDate);
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.siteId = siteId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>

        public int CardId
        {
            get { return cardId; }
            set { cardId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the AchievementClassLevelId field
        /// </summary>

        public int AchievementClassLevelId
        {
            get { return achievementClassLevelId; }
            set { achievementClassLevelId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the IsValid field
        /// </summary>

        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; this.IsChanged = true; }
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
        /// Get/Set method of the EffectiveDate field
        /// </summary>

        public DateTime EffectiveDate
        {
            get { return effectiveDate; }
            set { effectiveDate = value; }
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
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>

        public string LastUpdatedUser
        {
            get { return lastUpdatedUser; }
            set { lastUpdatedUser = value; }
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
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// Get/Set method of the createdBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        /// <summary>
        /// Get/Set method of the createdDate field
        /// </summary>
        public DateTime CreatedDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
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
            IsChanged = false;
            log.LogMethodExit();
        }

    }
}
