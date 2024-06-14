/********************************************************************************************
 * Project Name - Achievement
 * Description  - Data object of AchievementClassLevel
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 ********************************************************************************************* 
  *2.70        03-07-2019    Deeksha                Modified : Added new Constructor with required fields
 *                                                            Added CreaedBy and CreationDate field.
 *                                                            changed log.debug to log.logMethodEntry
 *                                                            and log.logMethodExit
 *2.80        27-Aug-2019   Vikas Dwivedi       Added WHO columns.
 *2.80        19-Nov-2019   Vikas Dwivedi       Added Logger Method.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Achievements
{
    /// <summary>
    /// This is the AchievementClassLevelDTO data object class. This acts as data holder for the AchievementClassLevel business object
    /// </summary>
    public class AchievementClassLevelDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ACHIEVEMENT CLASS LEVEL ID field
            /// </summary>
            ACHIEVEMENT_CLASS_LEVEL_ID ,
            /// <summary>
            /// Search by ACHIEVEMENT CLASS LEVEL ID LIST field
            /// </summary>
            ACHIEVEMENT_CLASS_LEVEL_ID_LIST,
            /// <summary>
            /// Search by LEVEL NAME field
            /// </summary>
            LEVEL_NAME ,
            /// <summary>
            /// Search by ACHIEVEMENT CLASS ID field
            /// </summary>
            ACHIEVEMENT_CLASS_ID ,
            /// <summary>
            /// Search by ACHIEVEMENT CLASS ID field
            /// </summary>
            ACHIEVEMENT_CLASS_ID_LIST,
            /// <summary>
            /// Search by PARENT LEVEL ID field
            /// </summary>
            PARENT_LEVEL_ID ,
            /// <summary>
            /// Search by QUALIFYING LEVEL ID field
            /// </summary>
            QUALIFYING_LEVEL_ID ,
            /// <summary>
            /// Search by REGISTRATION REQUIRED field
            /// </summary>
            REGISTRATION_REQUIRED ,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            IS_ACTIVE ,
            /// <summary>
            /// Search by SITEID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int achievementClassLevelId;
        private string levelName;
        private int achievementClassId;
        private int parentLevelId;
        private double qualifyingScore;
        private int qualifyingLevelId;
        private bool registrationRequired;
        private string bonusEntitlement;
        private double bonusAmount;
        private bool isActive;
        private string picture;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int siteId;
        private string externalSystemReference;
        private string createdBy;
        private DateTime creationDate;
        private List<AchievementScoreConversionDTO> achievementScoreConversionDTOList;
        private List<AchievementLevelDTO> achievementLevelDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AchievementClassLevelDTO()
        {
            log.LogMethodEntry();
            achievementClassLevelId = -1;
            levelName = string.Empty;
            achievementClassId = -1;
            parentLevelId = -1;
            qualifyingScore = 0.0;
            qualifyingLevelId = -1;
            bonusAmount = 0.0;
            guid = string.Empty;
            masterEntityId = -1;
            siteId = -1;
            picture = string.Empty;
            bonusEntitlement = string.Empty;
            this.lastUpdatedUser = string.Empty;
            externalSystemReference = string.Empty;
            isActive = true;
            achievementScoreConversionDTOList = new List<AchievementScoreConversionDTO>();
            achievementLevelDTOList = new List<AchievementLevelDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public AchievementClassLevelDTO(int achievementClassLevelId, string levelName, int achievementClassId,
                            int parentLevelId, double qualifyingScore, int qualifyingLevelId, bool registrationRequired,
                            string bonusEntitlement, double bonusAmount, bool isActive, string picture)
           : this()
        {
            log.LogMethodEntry(achievementClassLevelId, levelName, achievementClassId, parentLevelId, qualifyingScore,
                                qualifyingLevelId, registrationRequired, bonusEntitlement, bonusAmount, isActive, picture);
            this.achievementClassLevelId = achievementClassLevelId;
            this.levelName = levelName;
            this.achievementClassId = achievementClassId;
            this.parentLevelId = parentLevelId;
            this.qualifyingScore = qualifyingScore;
            this.qualifyingLevelId = qualifyingLevelId;
            this.registrationRequired = registrationRequired;
            this.bonusEntitlement = bonusEntitlement;
            this.bonusAmount = bonusAmount;
            this.isActive = isActive;
            this.picture = picture;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public AchievementClassLevelDTO(int achievementClassLevelId, string levelName, int achievementClassId, int parentLevelId,
                                      double qualifyingScore, int qualifyingLevelId, bool registrationRequired, string bonusEntitlement,
                                      double bonusAmount, bool isActive, string picture, DateTime lastUpdatedDate,
                                      string lastUpdatedUser, string guid, bool synchStatus, int masterEntityId, int siteId,
                                      string externalSystemReference, string createdBy, DateTime creationDate)
            : this(achievementClassLevelId, levelName, achievementClassId, parentLevelId, qualifyingScore,
                   qualifyingLevelId, registrationRequired, bonusEntitlement, bonusAmount, isActive, picture)
        {
            log.LogMethodEntry(achievementClassLevelId, levelName, achievementClassId, parentLevelId, qualifyingScore,
                               qualifyingLevelId, registrationRequired, bonusEntitlement, bonusAmount, isActive, picture,
                               lastUpdatedDate, lastUpdatedUser, guid, synchStatus, masterEntityId, siteId,
                               externalSystemReference, createdBy, creationDate);
            this.lastUpdatedDate = lastUpdatedDate;
            this.LastUpdatedUser = LastUpdatedUser;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.siteId = siteId;
            this.externalSystemReference = externalSystemReference;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
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
        /// Get/Set method of the LevelName field
        /// </summary> 
        public string LevelName
        {
            get { return levelName; }
            set { levelName = value; this.IsChanged = true; }
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
        /// Get/Set method of the ParentLevelId field
        /// </summary>
        public int ParentLevelId
        {
            get { return parentLevelId; }
            set { parentLevelId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the QualifyingScore field
        /// </summary>
        public double QualifyingScore
        {
            get { return qualifyingScore; }
            set { qualifyingScore = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the QualifyingLevelId field
        /// </summary>
        public int QualifyingLevelId
        {
            get { return qualifyingLevelId; }
            set { qualifyingLevelId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the RegistrationRequired field
        /// </summary>
        public bool RegistrationRequired
        {
            get { return registrationRequired; }
            set { registrationRequired = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the BonusEntitlement field
        /// </summary>
        public string BonusEntitlement
        {
            get { return bonusEntitlement; }
            set { bonusEntitlement = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the BonusAmount field
        /// </summary>
        public double BonusAmount
        {
            get { return bonusAmount; }
            set { bonusAmount = value; this.IsChanged = true; }
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
        /// Get/Set method of the Picture field
        /// </summary>
        public string Picture
        {
            get { return picture; }
            set { picture = value; this.IsChanged = true; }
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
        /// Get/Set method of the AchievementScoreConversionDTOList field
        /// </summary>
        public List<AchievementScoreConversionDTO> AchievementScoreConversionDTOList
        {
            get { return achievementScoreConversionDTOList; }
            set { achievementScoreConversionDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of the AchievementLevelDTOList field
        /// </summary>
        public List<AchievementLevelDTO> AchievementLevelList
        {
            get { return achievementLevelDTOList; }
            set { achievementLevelDTOList = value; }
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
                    return notifyingObjectIsChanged || achievementClassLevelId < 0;
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
        /// Returns true or false whether the AchievementClassLevelDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (achievementScoreConversionDTOList != null &&
                   achievementScoreConversionDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (achievementLevelDTOList != null &&
                    achievementLevelDTOList.Any(x => x.IsChanged))
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
