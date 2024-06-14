/********************************************************************************************
* Project Name - ScoringEventPolicy DTO
* Description  - Data object of ScoringEventPolicy
* 
**************
**Version Log
**************
*Version     Date              Modified By         Remarks          
*********************************************************************************************
*2.120.00     01-03-2021       Prajwal             Created 
* *******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Achievements.ScoringEngine
{
    public class ScoringEventPolicyDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByScoringEventPolicyParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByScoringEventPolicyParameters
        {
            /// <summary>
            /// Search by SCORING_EVENT_POLICY_ID field
            /// </summary>
            SCORING_EVENT_POLICY_ID,
            /// <summary>
            /// Search by SCORING_EVENT_POLICY_ID_LIST field
            /// </summary>
            SCORING_EVENT_POLICY_ID_LIST,
            /// <summary>
            /// Search by ACHIEVEMENT_CLASS_ID field
            /// </summary>
            ACHIEVEMENT_CLASS_ID,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by masterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID
        }
        private int scoringEventPolicyId;
        private  string scoringPolicyName;
        private DateTime? startDate;
        private DateTime? endDate;
        private int achievementClassId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private String lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        List<ScoringEventDTO> scoringEventDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScoringEventPolicyDTO()
        {
            log.LogMethodEntry();
            scoringEventPolicyId = -1;
            achievementClassId = -1;
            siteId = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ScoringEventPolicyDTO(int scoringEventPolicyId, string scoringPolicyName, DateTime? startDate, DateTime? endDate, int achievementClassId, bool isActive)
        : this()
        {
            log.LogMethodEntry(scoringEventPolicyId, scoringPolicyName, startDate, endDate, achievementClassId, isActive);
            this.startDate = startDate;
            this.endDate = endDate;
            this.scoringEventPolicyId = scoringEventPolicyId;
            this.scoringPolicyName = scoringPolicyName;
            this.achievementClassId = achievementClassId;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public ScoringEventPolicyDTO(int scoringEventPolicyId, string scoringPolicyName, DateTime? startDate, DateTime? endDate,
                                      int achievementClassId, string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                      int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(scoringEventPolicyId, scoringPolicyName, startDate, endDate, achievementClassId, isActive)
        {
            log.LogMethodEntry(scoringEventPolicyId, scoringPolicyName, startDate, endDate, achievementClassId,
                               createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public ScoringEventPolicyDTO(ScoringEventPolicyDTO scoringEventPolicyDTO)
            : this()
        {
            log.LogMethodEntry(achievementClassId, scoringEventPolicyId, scoringPolicyName, startDate, endDate, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            startDate = scoringEventPolicyDTO.startDate;
            endDate = scoringEventPolicyDTO.endDate;
            scoringEventPolicyId = scoringEventPolicyDTO.scoringEventPolicyId;
            scoringPolicyName = scoringEventPolicyDTO.scoringPolicyName;
            achievementClassId = scoringEventPolicyDTO.achievementClassId;
            isActive = scoringEventPolicyDTO.isActive;
            siteId = scoringEventPolicyDTO.siteId;
            synchStatus = scoringEventPolicyDTO.synchStatus;
            guid = scoringEventPolicyDTO.guid;
            lastUpdatedBy = scoringEventPolicyDTO.lastUpdatedBy;
            lastUpdatedDate = scoringEventPolicyDTO.lastUpdatedDate;
            createdBy = scoringEventPolicyDTO.createdBy;
            creationDate = scoringEventPolicyDTO.creationDate;
            masterEntityId = scoringEventPolicyDTO.masterEntityId;

            if (scoringEventPolicyDTO.scoringEventDTOList != null)
            {
                scoringEventDTOList = new List<ScoringEventDTO>();
                foreach (var scoringEventDTO in scoringEventPolicyDTO.scoringEventDTOList)
                {
                    scoringEventDTOList.Add(new ScoringEventDTO(scoringEventDTO));
                }
            }
        }
        /// <summary>
        /// Get/Set method of the scoringEventPolicyId field
        /// </summary>
        public int ScoringEventPolicyId { get { return scoringEventPolicyId; } set { scoringEventPolicyId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScoringPolicyName field
        /// </summary>
        public string ScoringPolicyName { get { return scoringPolicyName; } set { scoringPolicyName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the StartDate field
        /// </summary>
        public DateTime? StartDate { get { return startDate; } set { startDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the EndDate field
        /// </summary>
        public DateTime? EndDate { get { return endDate; } set { endDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AchievementClassId field
        /// </summary>
        public int AchievementClassId { get { return achievementClassId; } set { achievementClassId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedUser { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }


        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool ActiveFlag { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScoringEventDTOList field
        /// </summary>
        public List<ScoringEventDTO> ScoringEventDTOList { get { return scoringEventDTOList; } set { scoringEventDTOList = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || scoringEventPolicyId == -1;
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
        /// Returns whether the ScoringEventPolicyDTO changed or any of its scoringEventDTO children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (scoringEventDTOList != null &&
                   scoringEventDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}