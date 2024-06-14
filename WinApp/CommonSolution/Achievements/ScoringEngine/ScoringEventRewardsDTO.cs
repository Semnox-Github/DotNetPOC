/********************************************************************************************
* Project Name - ScoringEvent DTO
* Description  - Data object of ScoringEvent
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
    public class ScoringEventRewardsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByScoringEventParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByScoringEventRewardsParameters
        {
            /// <summary>
            /// Search by SCORING_EVENT_ID field
            /// </summary>
            SCORING_EVENT_ID,
            /// <summary>
            /// Search by SCORING_EVENT_REWARD_ID_LIST field
            /// </summary>
            SCORING_EVENT_REWARD_ID_LIST,
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
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
        private int scoringEventId;
        private int scoringRewardId;
        private double? absoluteScore;
        private int? patternBreachPenalty;
        private string rewardName;
        private bool isCumulativeScore;
        private bool allowProgressiveScoring;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private String lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        List<ScoringEventRewardTimeSlabsDTO> scoringEventRewardTimeSlabsDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScoringEventRewardsDTO()
        {
            log.LogMethodEntry();
            scoringEventId = -1;
            scoringRewardId = -1;
            siteId = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ScoringEventRewardsDTO(int scoringRewardId, int scoringEventId, double? absoluteScore, int? patternBreachPenalty,
                               string rewardName, bool isCumulativeScore, bool allowProgressiveScoring, bool isActive)
        : this()
        {
            log.LogMethodEntry(scoringRewardId, scoringEventId, absoluteScore, patternBreachPenalty, rewardName, isCumulativeScore,
                                allowProgressiveScoring,  isActive);
            this.scoringEventId = scoringEventId;
            this.scoringRewardId = scoringRewardId;
            this.absoluteScore = absoluteScore;
            this.patternBreachPenalty = patternBreachPenalty;
            this.rewardName = rewardName;
            this.isCumulativeScore = isCumulativeScore;
            this.allowProgressiveScoring = allowProgressiveScoring;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public ScoringEventRewardsDTO(int scoringRewardId,  int scoringEventId, double? absoluteScore, int? patternBreachPenalty,
                                     string rewardName, bool isCumulativeScore, bool allowProgressiveScoring, string createdBy, DateTime creationDate, String lastUpdatedBy, 
                                     DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(scoringRewardId, scoringEventId, absoluteScore, patternBreachPenalty, rewardName, isCumulativeScore,
                                allowProgressiveScoring, isActive)
        {
            log.LogMethodEntry(scoringRewardId, scoringEventId, absoluteScore, patternBreachPenalty,
                                     rewardName, isCumulativeScore, allowProgressiveScoring, createdBy, creationDate, lastUpdatedBy,
                                     lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
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
        public ScoringEventRewardsDTO(ScoringEventRewardsDTO scoringEventRewardsDTO)
            : this()
        {
            log.LogMethodEntry(scoringRewardId, scoringEventId, absoluteScore, patternBreachPenalty, rewardName, isCumulativeScore, allowProgressiveScoring, createdBy, creationDate, lastUpdatedBy,
                               lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            scoringEventId = scoringEventRewardsDTO.scoringEventId;
            scoringRewardId = scoringEventRewardsDTO.scoringRewardId;
            absoluteScore = scoringEventRewardsDTO.absoluteScore;
            patternBreachPenalty = scoringEventRewardsDTO.patternBreachPenalty;
            rewardName = scoringEventRewardsDTO.rewardName;
            isCumulativeScore = scoringEventRewardsDTO.isCumulativeScore;
            allowProgressiveScoring = scoringEventRewardsDTO.allowProgressiveScoring;
            isActive = scoringEventRewardsDTO.isActive;
            siteId = scoringEventRewardsDTO.siteId;
            synchStatus = scoringEventRewardsDTO.synchStatus;
            guid = scoringEventRewardsDTO.guid;
            lastUpdatedBy = scoringEventRewardsDTO.lastUpdatedBy;
            lastUpdatedDate = scoringEventRewardsDTO.lastUpdatedDate;
            createdBy = scoringEventRewardsDTO.createdBy;
            creationDate = scoringEventRewardsDTO.creationDate;
            masterEntityId = scoringEventRewardsDTO.masterEntityId;

            if (scoringEventRewardsDTO.scoringEventRewardTimeSlabsDTOList != null)
            {
                scoringEventRewardTimeSlabsDTOList = new List<ScoringEventRewardTimeSlabsDTO>();
                foreach (var scoringEventRewardTimeSlabsDTO in scoringEventRewardsDTO.scoringEventRewardTimeSlabsDTOList)
                {
                    scoringEventRewardTimeSlabsDTOList.Add(new ScoringEventRewardTimeSlabsDTO(scoringEventRewardTimeSlabsDTO));
                }
            }
        }
        /// <summary>
        /// Get/Set method of the scoringEventId field
        /// </summary>
        public int ScoringEventId { get { return scoringEventId; } set { scoringEventId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScoringRewardId field
        /// </summary>
        public int ScoringRewardId { get { return scoringRewardId; } set { scoringRewardId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AbsoluteScore field
        /// </summary>
        public double? AbsoluteScore { get { return absoluteScore; } set { absoluteScore = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PatternBreachPenalty field
        /// </summary>
        public int? PatternBreachPenalty { get { return patternBreachPenalty; } set { patternBreachPenalty = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RewardName field
        /// </summary>
        public string RewardName { get { return rewardName; } set { rewardName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsCumulativeScore field
        /// </summary>
        public bool IsCumulativeScore { get { return isCumulativeScore; } set { isCumulativeScore = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AllowProgressiveScoring field
        /// </summary>
        public bool AllowProgressiveScoring { get { return allowProgressiveScoring; } set { allowProgressiveScoring = value; this.IsChanged = true; } }

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
        /// Get/Set method of the ScoringEventRewardTimeSlabsDTOList field
        /// </summary>
        public List<ScoringEventRewardTimeSlabsDTO> ScoringEventRewardTimeSlabsDTOList { get { return scoringEventRewardTimeSlabsDTOList; } set { scoringEventRewardTimeSlabsDTOList = value; } }


        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || scoringEventId == -1;
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
        /// Returns whether the ScoringEventRewardsDTO changed or any of its scoringEventRewardTimeSlabs children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (scoringEventRewardTimeSlabsDTOList != null &&
                   scoringEventRewardTimeSlabsDTOList.Any(x => x.IsChanged))
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