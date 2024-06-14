/********************************************************************************************
 * Project Name - Achievements
 * Description  - Data object of AchievementProject
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        03-JUl-2019   Deeksha                 Modified : Added new Constructor with required fields
 *                                                             Added CreatedBy and CreationDate field.
 *                                                             changed log.debug to log.logMethodEntry
 *                                                             and log.logMethodExit
 *2.80        04-Mar-2020   Vikas Dwivedi           Modified as per the Standards for Phase 1 changes.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Achievements
{
    public class AchievementProjectDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ACHIEVEMENTPROJECT ID field
            /// </summary>
            ACHIEVEMENTPROJECT_ID,
            /// <summary>
            /// Search by PROJECT NAME field
            /// </summary>
            PROJECT_NAME,
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

        private int achievementProjectId;
        private string projectName;
        private bool isActive;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int siteId;
        private string externalSystemReference;
        private string createdBy;
        private DateTime creationDate;
        private List<AchievementClassDTO> achievementClassDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AchievementProjectDTO()
        {
            log.LogMethodEntry();
            achievementProjectId = -1;
            projectName = string.Empty;
            isActive = true;
            lastUpdatedUser = string.Empty;
            guid = string.Empty;
            masterEntityId = -1;
            siteId = -1;
            externalSystemReference = string.Empty;
            achievementClassDTOList = new List<AchievementClassDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required parameters
        /// </summary>
        public AchievementProjectDTO(int achievementProjectId, string projectName, bool isActive)
             : this()
        {
            log.LogMethodEntry(achievementProjectId, projectName, isActive);
            this.achievementProjectId = achievementProjectId;
            this.projectName = projectName;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the parameters
        /// </summary>
        public AchievementProjectDTO(int achievementProjectId, string projectName, bool isActive, DateTime lastUpdatedDate,
                                    string lastUpdatedUser, string guid, bool synchStatus, int masterEntityId, int siteId,
                                    string externalSystemReference, string createdBy, DateTime creationDate)
        : this(achievementProjectId, projectName, isActive)
        {
            log.LogMethodEntry(achievementProjectId, projectName, isActive, lastUpdatedDate, lastUpdatedUser, guid, synchStatus,
                                masterEntityId, siteId, externalSystemReference, createdBy, creationDate);
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
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
        /// Get/Set method of the AchievementProjectId field
        /// </summary>
        public int AchievementProjectId
        {
            get { return achievementProjectId; }
            set { achievementProjectId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ProjectName field
        /// </summary>
        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; this.IsChanged = true; }
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
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        /// <summary>
        /// Get/Set method of the CreatedDate field
        /// </summary>
        public DateTime CreatedDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get/Set method of the achievementClassDTOList field
        /// </summary>
        public List<AchievementClassDTO> AchievementClassDTOList
        {
            get { return achievementClassDTOList; }
            set { achievementClassDTOList = value; }
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
                    return notifyingObjectIsChanged || achievementProjectId < 0;
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
        /// Returns true or false whether the AchievementProjectDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (achievementClassDTOList != null &&
                   achievementClassDTOList.Any(x => x.IsChangedRecursive))
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
