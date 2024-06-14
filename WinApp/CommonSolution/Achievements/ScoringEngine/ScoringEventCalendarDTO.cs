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
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Achievements.ScoringEngine
{
    public class ScoringEventCalendarDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByScoringEventParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByScoringEventCalendarParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by SCORING_EVENT_ID field
            /// </summary>
            SCORING_EVENT_ID,
            /// <summary>
            /// Search by SCORING_EVENT_CALENDAR_ID_LIST field
            /// </summary>
            SCORING_EVENT_CALENDAR_ID_LIST,
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
        private int id;
        private int? day;
        private DateTime? date;
        private string fromTime;
        private string toTime;
        private DateTime? fromDate;
        private DateTime? endDate;
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
        public ScoringEventCalendarDTO()
        {
            log.LogMethodEntry();
            scoringEventId = -1;
            id = -1;
            siteId = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ScoringEventCalendarDTO(int id, int scoringEventId, int? day, DateTime? date, string fromTime, string toTime, DateTime? fromDate, DateTime? endDate, bool isActive)
        : this()
        {
            log.LogMethodEntry(id, scoringEventId, day, date, fromTime, toTime, fromDate, endDate, isActive);
            this.scoringEventId = scoringEventId;
            this.id = id;
            this.day = day;
            this.date = date;
            this.fromTime = fromTime;
            this.toTime = toTime;
            this.fromDate = fromDate;
            this.endDate = endDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public ScoringEventCalendarDTO(int id, int scoringEventId, int? day, DateTime? date, string fromTime, string toTime, DateTime? fromDate, DateTime? endDate,
                               string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(id, scoringEventId, day, date, fromTime, toTime, fromDate, endDate, isActive)
        {
            log.LogMethodEntry(scoringEventId, id, day, date, fromTime, toTime, fromDate, endDate,
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
        public ScoringEventCalendarDTO(ScoringEventCalendarDTO scoringEventCalendarDTO)
            : this()
        {
            log.LogMethodEntry(scoringEventId, id, day, date, fromTime, toTime,
                               createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            scoringEventId = scoringEventCalendarDTO.scoringEventId;
            id = scoringEventCalendarDTO.id;
            day = scoringEventCalendarDTO.day;
            date = scoringEventCalendarDTO.date;
            fromTime = scoringEventCalendarDTO.fromTime;
            toTime = scoringEventCalendarDTO.toTime;
            fromDate = scoringEventCalendarDTO.fromDate;
            endDate = scoringEventCalendarDTO.endDate;
            isActive = scoringEventCalendarDTO.isActive;
            siteId = scoringEventCalendarDTO.siteId;
            synchStatus = scoringEventCalendarDTO.synchStatus;
            guid = scoringEventCalendarDTO.guid;
            lastUpdatedBy = scoringEventCalendarDTO.lastUpdatedBy;
            lastUpdatedDate = scoringEventCalendarDTO.lastUpdatedDate;
            createdBy = scoringEventCalendarDTO.createdBy;
            creationDate = scoringEventCalendarDTO.creationDate;
            masterEntityId = scoringEventCalendarDTO.masterEntityId;
        }
        /// <summary>
        /// Get/Set method of the scoringEventId field
        /// </summary>
        public int ScoringEventId { get { return scoringEventId; } set { scoringEventId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Day field
        /// </summary>
        public int? Day { get { return day; } set { day = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Date field
        /// </summary>
        public DateTime? Date { get { return date; } set { date = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FromTime field
        /// </summary>
        public string FromTime { get { return fromTime; } set { fromTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ToTime field
        /// </summary>
        public string ToTime { get { return toTime; } set { toTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FromTime field
        /// </summary>
        public DateTime? FromDate { get { return fromDate; } set { fromDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EndDate field
        /// </summary>
        public DateTime? EndDate { get { return endDate; } set { endDate = value; this.IsChanged = true; } }

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