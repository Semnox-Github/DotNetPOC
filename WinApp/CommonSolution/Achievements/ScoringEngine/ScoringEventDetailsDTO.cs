/********************************************************************************************
* Project Name - ScoringEventDetails DTO
* Description  - Data object of ScoringEventDetails
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
    public class ScoringEventDetailsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByScoringEventDetailsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByScoringEventDetailsParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by SCORING_EVENT_DETAIL_ID_LIST field
            /// </summary>
            SCORING_EVENT_DETAIL_ID_LIST,
            /// <summary>
            /// Search by SCORING_EVENT_ID field
            /// </summary>
            SCORING_EVENT_ID,
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
        private int scoringEventDetailId;
        private int scoringEventId;
        private int? triggerGameProfileId;
        private int? triggerGameId;
        private int? qualifyingGameplays;
        private int? qualifyingTickets;
        private int sequence;
        private double? absoluteScore;
        private int? ticketMultiplierForScore;
        private int readerThemeId;
        private bool isActive;
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
        public ScoringEventDetailsDTO()
        {
            log.LogMethodEntry();
            scoringEventDetailId = -1;
            scoringEventId = -1;
            siteId = -1;
            readerThemeId = -1;
            triggerGameId = -1;
            triggerGameProfileId = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ScoringEventDetailsDTO(int scoringEventDetailId, int scoringEventId, int? triggerGameProfileId, int? triggerGameId,
                                      int? qualifyingGameplays, int? qualifyingTickets, int sequence, double? absoluteScore, int? ticketMultiplierForScore, int readerThemeId, bool isActive)
        : this()
        {
            log.LogMethodEntry(scoringEventDetailId, scoringEventId, triggerGameProfileId, triggerGameId,
                                qualifyingGameplays, qualifyingTickets, sequence, absoluteScore, ticketMultiplierForScore, readerThemeId, isActive);
            this.scoringEventDetailId = scoringEventDetailId;
            this.scoringEventId = scoringEventId;
            this.triggerGameProfileId = triggerGameProfileId;
            this.triggerGameId = triggerGameId;
            this.qualifyingGameplays = qualifyingGameplays;
            this.qualifyingTickets = qualifyingTickets;
            this.sequence = sequence;
            this.absoluteScore = absoluteScore;
            this.ticketMultiplierForScore = ticketMultiplierForScore;
            this.readerThemeId = readerThemeId;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public ScoringEventDetailsDTO(int scoringEventDetailId, int scoringEventId, int? triggerGameProfileId, int? triggerGameId,
                                      int? qualifyingGameplays, int? qualifyingTickets, int sequence, double? absoluteScore, int? ticketMultiplierForScore, int readerThemeId,
                               string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(scoringEventDetailId, scoringEventId, triggerGameProfileId, triggerGameId,
                                qualifyingGameplays, qualifyingTickets, sequence, absoluteScore, ticketMultiplierForScore, readerThemeId, isActive)
        {
            log.LogMethodEntry(scoringEventDetailId, scoringEventId, triggerGameProfileId, triggerGameId,
                                      qualifyingGameplays, qualifyingTickets, sequence, absoluteScore, ticketMultiplierForScore, readerThemeId,
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
        public ScoringEventDetailsDTO(ScoringEventDetailsDTO scoringEventDetailsDTO)
            : this()
        {
            log.LogMethodEntry(scoringEventDetailId, scoringEventId, triggerGameProfileId, triggerGameId,
                                      qualifyingGameplays, qualifyingTickets, sequence, absoluteScore, ticketMultiplierForScore, readerThemeId,
                               createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            scoringEventDetailId = scoringEventDetailsDTO.scoringEventDetailId;
            scoringEventId = scoringEventDetailsDTO.scoringEventId;
            triggerGameProfileId = scoringEventDetailsDTO.triggerGameProfileId;
            triggerGameId = scoringEventDetailsDTO.triggerGameId;
            qualifyingGameplays = scoringEventDetailsDTO.qualifyingGameplays;
            qualifyingTickets = scoringEventDetailsDTO.qualifyingTickets;
            sequence = scoringEventDetailsDTO.sequence;
            absoluteScore = scoringEventDetailsDTO.absoluteScore;
            ticketMultiplierForScore = scoringEventDetailsDTO.ticketMultiplierForScore;
            readerThemeId = scoringEventDetailsDTO.readerThemeId;
            isActive = scoringEventDetailsDTO.isActive;
            siteId = scoringEventDetailsDTO.siteId;
            synchStatus = scoringEventDetailsDTO.synchStatus;
            guid = scoringEventDetailsDTO.guid;
            lastUpdatedBy = scoringEventDetailsDTO.lastUpdatedBy;
            lastUpdatedDate = scoringEventDetailsDTO.lastUpdatedDate;
            createdBy = scoringEventDetailsDTO.createdBy;
            creationDate = scoringEventDetailsDTO.creationDate;
            masterEntityId = scoringEventDetailsDTO.masterEntityId;
        }
        /// <summary>
        /// Get/Set method of the ScoringEventDetailsId field
        /// </summary>
        public int ScoringEventDetailId { get { return scoringEventDetailId; } set { scoringEventDetailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScoringEventId field
        /// </summary>
        public int ScoringEventId { get { return scoringEventId; } set { scoringEventId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TriggerGameProfileId field
        /// </summary>
        public int? TriggerGameProfileId { get { return triggerGameProfileId; } set { triggerGameProfileId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TriggerGameId field
        /// </summary>
        public int? TriggerGameId { get { return triggerGameId; } set { triggerGameId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the QualifyingGameplays field
        /// </summary>
        public int? QualifyingGameplays { get { return qualifyingGameplays; } set { qualifyingGameplays = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the QualifyingTickets field
        /// </summary>
        public int? QualifyingTickets { get { return qualifyingTickets; } set { qualifyingTickets = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Sequence field
        /// </summary>
        public int Sequence { get { return sequence; } set { sequence = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AbsoluteScore field
        /// </summary>
        public double? AbsoluteScore { get { return absoluteScore; } set { absoluteScore = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TicketMultiplierForScore field
        /// </summary>
        public int? TicketMultiplierForScore { get { return ticketMultiplierForScore; } set { ticketMultiplierForScore = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReaderThemeId field
        /// </summary>
        public int ReaderThemeId { get { return readerThemeId; } set { readerThemeId = value; this.IsChanged = true; } }

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
        /// Get/Set method of the LastUpdatedUser field
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
                    return notifyingObjectIsChanged || ScoringEventDetailId == -1;
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