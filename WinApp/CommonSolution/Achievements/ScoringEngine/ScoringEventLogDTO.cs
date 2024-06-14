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
    public class ScoringEventLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByScoringEventParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByScoringEventLogParameters
        {
            /// <summary>
            /// Search by SCORING_EVENT_ID field
            /// </summary>
            SCORING_EVENT_ID,
            /// <summary>
            /// Search by SCORING_EVENT_LOG_ID field
            /// </summary>
            SCORING_EVENT_LOG_ID,
            /// <summary>
            /// Search by SCORING_EVENT_LOG_ID_LIST field
            /// </summary>
            SCORING_EVENT_LOG_ID_LIST,
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
        private int scoringEventLogId;
        private int? cardId;
        private DateTime? eventDate;
        private bool isFinal;
        private bool isPatternbreach;
        private bool isActive;
        private bool isTimeout;
        private double? score;
        private int? breachCount;
        private int? eventState;
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
        public ScoringEventLogDTO()
        {
            log.LogMethodEntry();
            scoringEventId = -1;
            scoringEventLogId = -1;
            siteId = -1;
            cardId = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ScoringEventLogDTO(int scoringEventLogId, int scoringEventId, int? cardId, DateTime? eventDate, bool isFinal, bool isPatternbreach, bool isTimeout, double? score, int? breachCount, int? eventState, bool isActive)
        : this()
        {
            log.LogMethodEntry(scoringEventLogId, scoringEventId, cardId, eventDate, isFinal, isPatternbreach, isTimeout, score, breachCount, eventState, isActive);
            this.scoringEventId = scoringEventId;
            this.scoringEventLogId = scoringEventLogId;
            this.cardId = cardId;
            this.eventDate = eventDate;
            this.isFinal = isFinal;
            this.isPatternbreach = isPatternbreach;
            this.isTimeout = isTimeout;
            this.score = score;
            this.breachCount = breachCount;
            this.eventState = eventState;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public ScoringEventLogDTO(int scoringEventLogId, int scoringEventId, int? cardId, DateTime? eventDate, bool isFinal, bool isPatternbreach, bool isTimeout, double? score, int? breachCount, int? eventState,
                               string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(scoringEventLogId, scoringEventId, cardId, eventDate, isFinal, isPatternbreach, isTimeout, score, breachCount, eventState, isActive)
        {
            log.LogMethodEntry(scoringEventLogId, scoringEventId, cardId, eventDate, isFinal, isPatternbreach, isTimeout, score, breachCount, eventState, isActive,
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
        public ScoringEventLogDTO(ScoringEventLogDTO scoringEventLogDTO)
            : this()
        {
            log.LogMethodEntry(scoringEventId, scoringEventLogId, cardId, eventDate, isFinal, isPatternbreach, isTimeout, score, breachCount, eventState, isActive,
                               createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            scoringEventId = scoringEventLogDTO.scoringEventId;
            scoringEventLogId = scoringEventLogDTO.scoringEventLogId;
            cardId = scoringEventLogDTO.cardId;
            eventDate = scoringEventLogDTO.eventDate;
            isFinal = scoringEventLogDTO.isFinal;
            isPatternbreach = scoringEventLogDTO.isPatternbreach;
            isActive = scoringEventLogDTO.isActive;
            siteId = scoringEventLogDTO.siteId;
            synchStatus = scoringEventLogDTO.synchStatus;
            guid = scoringEventLogDTO.guid;
            lastUpdatedBy = scoringEventLogDTO.lastUpdatedBy;
            lastUpdatedDate = scoringEventLogDTO.lastUpdatedDate;
            createdBy = scoringEventLogDTO.createdBy;
            creationDate = scoringEventLogDTO.creationDate;
            masterEntityId = scoringEventLogDTO.masterEntityId;
        }
        /// <summary>
        /// Get/Set method of the scoringEventId field
        /// </summary>
        public int ScoringEventId { get { return scoringEventId; } set { scoringEventId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScoringEventLogId field
        /// </summary>
        public int ScoringEventLogId { get { return scoringEventLogId; } set { scoringEventLogId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public int? CardId { get { return cardId; } set { cardId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BreachCount field
        /// </summary>
        public int? BreachCount { get { return breachCount; } set { breachCount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EventState field
        /// </summary>
        public int? EventState { get { return eventState; } set { eventState = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Score field
        /// </summary>
        public double? Score { get { return score; } set { score = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the EventDate field
        /// </summary>
        public DateTime? EventDate { get { return eventDate; } set { eventDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsFinal field
        /// </summary>
        public bool IsFinal { get { return isFinal; } set { isFinal = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsTimeout field
        /// </summary>
        public bool IsTimeout { get { return isTimeout; } set { isTimeout = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the IsPatternbreach field
        /// </summary>
        public bool IsPatternbreach { get { return isPatternbreach; } set { isPatternbreach = value; this.IsChanged = true; } }

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