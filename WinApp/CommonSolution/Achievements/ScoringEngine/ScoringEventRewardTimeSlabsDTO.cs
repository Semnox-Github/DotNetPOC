/********************************************************************************************
* Project Name - ScoringEventRewardTimeSlabs DTO
* Description  - Data object of ScoringEventRewardTimeSlabs
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
    public class ScoringEventRewardTimeSlabsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByScoringEventRewardTimeSlabsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByScoringEventRewardTimeSlabsParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by SCORING_EVENT_REWARD_TIMESLABS_ID_LIST field
            /// </summary>
            SCORING_EVENT_REWARD_TIMESLABS_ID_LIST,
            /// <summary>
            /// Search by SCORING_REWARD_ID field
            /// </summary>
            SCORING_REWARD_ID,
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
        private int id;
        private int scoringRewardId;
        private int? finishTimeSlabDays;
        private int? finishTimeSlabMinutes;
        private double? timeSlabScore;
        private bool isActive;
        private bool isTimeSlabScoreAbsoluteOrIncremental;
        private string createdBy;
        private DateTime creationDate;
        private String lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScoringEventRewardTimeSlabsDTO()
        {
            log.LogMethodEntry();
            scoringRewardId = -1;
            id = -1;
            siteId = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ScoringEventRewardTimeSlabsDTO(int id, int scoringRewardId, int? finishTimeSlabDays, int? finishTimeSlabMinutes, double? timeSlabScore, bool isTimeSlabScoreAbsoluteOrIncremental, bool isActive)
        : this()
        {
            log.LogMethodEntry(id, scoringRewardId, finishTimeSlabDays, finishTimeSlabMinutes, timeSlabScore, isTimeSlabScoreAbsoluteOrIncremental, isActive);
            this.id = id;
            this.scoringRewardId = scoringRewardId;
            this.finishTimeSlabDays = finishTimeSlabDays;
            this.finishTimeSlabMinutes = finishTimeSlabMinutes;
            this.timeSlabScore = timeSlabScore;
            this.isTimeSlabScoreAbsoluteOrIncremental = isTimeSlabScoreAbsoluteOrIncremental;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public ScoringEventRewardTimeSlabsDTO(int id, int scoringRewardId, int? finishTimeSlabDays, int? finishTimeSlabMinutes, double? timeSlabScore, bool isTimeSlabScoreAbsoluteOrIncremental,
                               string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(id, scoringRewardId, finishTimeSlabDays, finishTimeSlabMinutes, timeSlabScore, isTimeSlabScoreAbsoluteOrIncremental, isActive)
        {
            log.LogMethodEntry(id, scoringRewardId, finishTimeSlabDays, finishTimeSlabMinutes, timeSlabScore, isTimeSlabScoreAbsoluteOrIncremental,
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
        public ScoringEventRewardTimeSlabsDTO(ScoringEventRewardTimeSlabsDTO scoringEventRewardTimeSlabsDTO)
            : this()
        {
            log.LogMethodEntry(id, scoringRewardId, finishTimeSlabDays, finishTimeSlabMinutes, timeSlabScore, isTimeSlabScoreAbsoluteOrIncremental,
                               createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            id = scoringEventRewardTimeSlabsDTO.id;
            scoringRewardId = scoringEventRewardTimeSlabsDTO.scoringRewardId;
            finishTimeSlabDays = scoringEventRewardTimeSlabsDTO.finishTimeSlabDays;
            finishTimeSlabMinutes = scoringEventRewardTimeSlabsDTO.finishTimeSlabMinutes;
            timeSlabScore = scoringEventRewardTimeSlabsDTO.timeSlabScore;
            isTimeSlabScoreAbsoluteOrIncremental = scoringEventRewardTimeSlabsDTO.isTimeSlabScoreAbsoluteOrIncremental;
            isActive = scoringEventRewardTimeSlabsDTO.isActive;
            siteId = scoringEventRewardTimeSlabsDTO.siteId;
            synchStatus = scoringEventRewardTimeSlabsDTO.synchStatus;
            guid = scoringEventRewardTimeSlabsDTO.guid;
            lastUpdatedBy = scoringEventRewardTimeSlabsDTO.lastUpdatedBy;
            lastUpdatedDate = scoringEventRewardTimeSlabsDTO.lastUpdatedDate;
            createdBy = scoringEventRewardTimeSlabsDTO.createdBy;
            creationDate = scoringEventRewardTimeSlabsDTO.creationDate;
            masterEntityId = scoringEventRewardTimeSlabsDTO.masterEntityId;
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScoringRewardId field
        /// </summary>
        public int ScoringRewardId { get { return scoringRewardId; } set { scoringRewardId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FinishTimeSlabDays field
        /// </summary>
        public int? FinishTimeSlabDays { get { return finishTimeSlabDays; } set { finishTimeSlabDays = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FinishTimeSlabMinutes field
        /// </summary>
        public int? FinishTimeSlabMinutes { get { return finishTimeSlabMinutes; } set { finishTimeSlabMinutes = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TimeSlabScore field
        /// </summary>
        public double? TimeSlabScore { get { return timeSlabScore; } set { timeSlabScore = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsTimeSlabScoreAbsoluteOrIncremental field
        /// </summary>
        public bool IsTimeSlabScoreAbsoluteOrIncremental { get { return isTimeSlabScoreAbsoluteOrIncremental; } set { isTimeSlabScoreAbsoluteOrIncremental = value; this.IsChanged = true; } }

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
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id == -1;
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