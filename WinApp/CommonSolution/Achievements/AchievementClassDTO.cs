/********************************************************************************************
 * Project Name - Achievements
 * Description  - Data object of AchievementClass
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 ********************************************************************************************* 
 *2.70        02-Jul-2019    Deeksha                 Modified : Added new Constructor with required fields
 *                                                             Added CreatedBy and CreationDate field.
 *                                                             changed log.debug to log.logMethodEntry
 *                                                             and log.logMethodExit
 *2.80        27-Aug-2019   Vikas Dwivedi       Added WHO columns
 *2.80        19-Nov-2019   Vikas Dwivedi       Added Logger Method
 *2.80        04-Mar-2020   Vikas Dwivedi       Modified as per the Standards for Phase 1 Changes.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Achievements
{
    public class AchievementClassDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ACHIEVEMENT CLASS ID field
            /// </summary>
            ACHIEVEMENT_CLASS_ID,
            /// <summary>
            /// Search by ACHIEVEMENT CLASS ID LIST field
            /// </summary>
            ACHIEVEMENT_CLASS_ID_LIST,
            /// <summary>
            /// Search by CLASS NAME field
            /// </summary>
            CLASS_NAME,
            /// <summary>
            /// Search by ACHIEVEMENT PROJECT ID field
            /// </summary>
            ACHIEVEMENT_PROJECT_ID,
            /// <summary>
            /// Search by ACHIEVEMENT PROJECT ID LIST field
            /// </summary>
            ACHIEVEMENT_PROJECT_ID_LIST,
            /// <summary>
            /// Search by GAME ID field
            /// </summary>
            GAME_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITEID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by EXTERNAL SYSTEM REFERENCE field
            /// </summary>
            EXTERNAL_SYSTEM_REFERENCE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int achievementClassId;
        private string className;
        private int achievementProjectId;
        private int gameId;
        private int productId;
        private string externalSystemReference;
        private bool isActive;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int siteId;
        private string createdBy;
        private DateTime creationDate;
        private List<AchievementClassLevelDTO> achievementClassLevelDTOList;
        private List<AchievementScoreLogDTO> achievementScoreLogDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AchievementClassDTO()
        {
            log.LogMethodEntry();
            achievementClassId = -1;
            className = string.Empty;
            achievementProjectId = -1;
            gameId = -1;
            isActive = true;
            guid = string.Empty;
            masterEntityId = -1;
            siteId = -1;
            externalSystemReference = string.Empty;
            productId = -1;
            achievementClassLevelDTOList = new List<AchievementClassLevelDTO> ();
            achievementScoreLogDTOList = new List<AchievementScoreLogDTO> ();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public AchievementClassDTO(int achievementClassId, string className, int achievementProjectId, int gameId,
                                   int productId, string externalSystemReference, bool isActive)
            : this()
        {
            log.LogMethodEntry(achievementClassId, className, achievementProjectId, gameId, isActive, externalSystemReference, productId);
            this.achievementClassId = achievementClassId;
            this.className = className;
            this.achievementProjectId = achievementProjectId;
            this.gameId = gameId;
            this.productId = productId;
            this.externalSystemReference = externalSystemReference;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public AchievementClassDTO(int achievementClassId, string className, int achievementProjectId, int gameId, int productId,
                                    string externalSystemReference, bool isActive, DateTime lastUpdatedDate, string lastUpdatedUser, string guid,
                                    bool synchStatus, int masterEntityId, int siteId, string createdBy, DateTime creationDate)
           : this(achievementClassId, className, achievementProjectId, gameId, productId, externalSystemReference, isActive)
        {
            log.LogMethodEntry(achievementClassId, className, achievementProjectId, gameId, productId, externalSystemReference, isActive,
                                lastUpdatedDate, lastUpdatedUser, guid, synchStatus, masterEntityId, siteId, createdBy, creationDate);
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
        /// Get/Set method of the AchievementClassId field
        /// </summary>
        public int AchievementClassId
        {
            get { return achievementClassId; }
            set { achievementClassId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ClassName field
        /// </summary>
        public string ClassName
        {
            get { return className; }
            set { className = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the AchievementProjectId field
        /// </summary>
        public int AchievementProjectId
        {
            get { return achievementProjectId; }
            set { achievementProjectId = value; this.IsChanged = true; }
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
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
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
        /// Get/Set method of the ExternalSystemReference field
        /// </summary>
        public string ExternalSystemReference
        {
            get { return externalSystemReference; }
            set { externalSystemReference = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId
        {
            get { return productId; }
            set { productId = value; this.IsChanged = true; }
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
            set { creationDate = value;  }
        }

        /// <summary>
        /// Get/Set method of the achievementClassLevelDTOList field
        /// </summary>
        public List<AchievementClassLevelDTO> AchievementClassLevelDTOList
        {
            get { return achievementClassLevelDTOList; }
            set { achievementClassLevelDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of the achievementScoreLogDTOList field
        /// </summary>
        public List<AchievementScoreLogDTO> AchievementScoreLogDTOList
        {
            get { return achievementScoreLogDTOList; }
            set { achievementScoreLogDTOList = value; }
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
                    return notifyingObjectIsChanged || achievementClassId < 0;
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
        /// Returns true or false whether the AchievementClassDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (achievementClassLevelDTOList != null &&
                   achievementClassLevelDTOList.Any(x => x.IsChangedRecursive))
                {
                    return true;
                }
                if (achievementScoreLogDTOList != null &&
                   achievementScoreLogDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
