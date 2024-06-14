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
     public class ScoringEventDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByScoringEventParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByScoringEventParameters
        {
            /// <summary>
            /// Search by SCORING_EVENT_ID field
            /// </summary>
            SCORING_EVENT_ID,
            /// <summary>
            /// Search by SCORING_EVENT_ID_LIST field
            /// </summary>
            SCORING_EVENT_ID_LIST,
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
        private int scoringEventId;
        private int scoringEventPolicyId;
        private int? timeLimitInDays;
        private int? timeLimitInMinutes;
        private bool teamEvent;
        private EventPatternTypes eventType;
        private string eventName;
        private bool enforcePattern;
        private int? patternBreachMaxAllowed;
        private bool resetBreachCountOnProgress;
        private bool onDifferentDays;
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

        List<ScoringEventDetailsDTO> scoringEventDetailsDTOList;
        //List<ScoringEventLogDTO> scoringEventLogDTOList;
        List<ScoringEventRewardsDTO> scoringEventRewardsDTOList;
        List<ScoringEventCalendarDTO> scoringEventCalendarDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScoringEventDTO()
        {
            log.LogMethodEntry();
            scoringEventId = -1;
            achievementClassId = -1;
            siteId = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ScoringEventDTO(int scoringEventId,int scoringEventPolicyId, int? timeLimitInDays, int? timeLimitInMinutes, bool teamEvent, EventPatternTypes eventType,
                               string eventName, bool enforcePattern, int? patternBreachMaxAllowed, bool resetBreachCountOnProgress, bool onDifferentDays, int achievementClassId, bool isActive)
        : this()
        {
            log.LogMethodEntry(scoringEventId, scoringEventPolicyId, timeLimitInDays, timeLimitInMinutes, teamEvent,  eventType,
                                eventName, enforcePattern, patternBreachMaxAllowed, resetBreachCountOnProgress, onDifferentDays, achievementClassId, isActive);
            this.scoringEventId = scoringEventId;
            this.scoringEventPolicyId = scoringEventPolicyId;
            this.timeLimitInDays = timeLimitInDays;
            this.timeLimitInMinutes = timeLimitInMinutes;
            this.teamEvent = teamEvent;
            this.eventType = eventType;
            this.eventName = eventName;
            this.enforcePattern = enforcePattern;
            this.patternBreachMaxAllowed = patternBreachMaxAllowed;
            this.resetBreachCountOnProgress = resetBreachCountOnProgress;
            this.onDifferentDays = onDifferentDays;
            this.achievementClassId = achievementClassId;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public ScoringEventDTO(int scoringEventId, int scoringEventPolicyId, int timeLimitInDays, int timeLimitInMinutes, bool teamEvent, EventPatternTypes eventType,
                               string eventName, bool enforcePattern, int patternBreachMaxAllowed, bool resetBreachCountOnProgress, bool onDifferentDays, int achievementClassId,
                               string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(scoringEventId, scoringEventPolicyId, timeLimitInDays, timeLimitInMinutes, teamEvent, eventType,
                                eventName, enforcePattern, patternBreachMaxAllowed, resetBreachCountOnProgress, onDifferentDays, achievementClassId, isActive)
        {
            log.LogMethodEntry(scoringEventId, scoringEventPolicyId, timeLimitInDays, timeLimitInMinutes, teamEvent, eventType,
                               eventName, enforcePattern, patternBreachMaxAllowed, resetBreachCountOnProgress, onDifferentDays, achievementClassId,
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
        public ScoringEventDTO(ScoringEventDTO scoringEventDTO)
            : this()
        {
            log.LogMethodEntry(scoringEventId, scoringEventPolicyId, timeLimitInDays, timeLimitInMinutes, teamEvent, eventType,
                               eventName, enforcePattern, patternBreachMaxAllowed, resetBreachCountOnProgress, onDifferentDays, achievementClassId,
                               createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            scoringEventId = scoringEventDTO.scoringEventId;
            scoringEventPolicyId = scoringEventDTO.scoringEventPolicyId;
            timeLimitInDays = scoringEventDTO.timeLimitInDays;
            timeLimitInMinutes = scoringEventDTO.timeLimitInMinutes;
            teamEvent = scoringEventDTO.teamEvent;
            eventType = scoringEventDTO.eventType;
            eventName = scoringEventDTO.eventName;
            enforcePattern = scoringEventDTO.enforcePattern;
            patternBreachMaxAllowed = scoringEventDTO.patternBreachMaxAllowed;
            resetBreachCountOnProgress = scoringEventDTO.resetBreachCountOnProgress;
            onDifferentDays = scoringEventDTO.onDifferentDays;
            achievementClassId = scoringEventDTO.achievementClassId;
            isActive = scoringEventDTO.isActive;
            siteId = scoringEventDTO.siteId;
            synchStatus = scoringEventDTO.synchStatus;
            guid = scoringEventDTO.guid;
            lastUpdatedBy = scoringEventDTO.lastUpdatedBy;
            lastUpdatedDate = scoringEventDTO.lastUpdatedDate;
            createdBy = scoringEventDTO.createdBy;
            creationDate = scoringEventDTO.creationDate;
            masterEntityId = scoringEventDTO.masterEntityId;
            //if (scoringEventDTO.scoringEventLogDTOList != null)
            //{
            //    scoringEventLogDTOList = new List<ScoringEventLogDTO>();
            //    foreach (var scoringEventLogDTO in scoringEventDTO.scoringEventLogDTOList)
            //    {
            //        scoringEventLogDTOList.Add(new ScoringEventLogDTO(scoringEventLogDTO));
            //    }
            //}

            if (scoringEventDTO.scoringEventDetailsDTOList != null)
            {
                scoringEventDetailsDTOList = new List<ScoringEventDetailsDTO>();
                foreach (var scoringEventDetailsDTO in scoringEventDTO.scoringEventDetailsDTOList)
                {
                    scoringEventDetailsDTOList.Add(new ScoringEventDetailsDTO(scoringEventDetailsDTO));
                }
            }

            if (scoringEventDTO.scoringEventRewardsDTOList != null)
            {
                scoringEventRewardsDTOList = new List<ScoringEventRewardsDTO>();
                foreach (var scoringEventRewardsDTO in scoringEventDTO.scoringEventRewardsDTOList)
                {
                    scoringEventRewardsDTOList.Add(new ScoringEventRewardsDTO(scoringEventRewardsDTO));
                }
            }

            if (scoringEventDTO.scoringEventCalendarDTOList != null)
            {
                scoringEventCalendarDTOList = new List<ScoringEventCalendarDTO>();
                foreach (var scoringEventCalendarDTO in scoringEventDTO.scoringEventCalendarDTOList)
                {
                    scoringEventCalendarDTOList.Add(new ScoringEventCalendarDTO(scoringEventCalendarDTO));
                }
            }
        }
        /// <summary>
        /// Get/Set method of the scoringEventId field
        /// </summary>
        public int ScoringEventId { get { return scoringEventId; } set { scoringEventId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScoringEventPolicyId field
        /// </summary>
        public int ScoringEventPolicyId { get { return scoringEventPolicyId; } set { scoringEventPolicyId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TimeLimitInDays field
        /// </summary>
        public int? TimeLimitInDays { get { return timeLimitInDays; } set { timeLimitInDays = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TimeLimitInMinutes field
        /// </summary>
        public int? TimeLimitInMinutes { get { return timeLimitInMinutes; } set { timeLimitInMinutes = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PatternBreachMaxAllowed field
        /// </summary>
        public int? PatternBreachMaxAllowed { get { return patternBreachMaxAllowed; } set { patternBreachMaxAllowed = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EventType field
        /// </summary>
        public EventPatternTypes EventType { get { return eventType; } set { eventType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string EventName { get { return eventName; } set { eventName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TeamEvent field
        /// </summary>
        public bool TeamEvent { get { return teamEvent; } set { teamEvent = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EnforcePattern field
        /// </summary>
        public bool EnforcePattern { get { return enforcePattern; } set { enforcePattern = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ResetBreachCountOnProgress field
        /// </summary>
        public bool ResetBreachCountOnProgress { get { return resetBreachCountOnProgress; } set { resetBreachCountOnProgress = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the OnDifferentDays field
        /// </summary>
        public bool OnDifferentDays { get { return onDifferentDays; } set { onDifferentDays = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleId field
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
        /// Get/Set method to track changes to the object
        /// </summary>

        /// <summary>
        /// Get/Set method of the ScoringEventLogDTOList field
        /// </summary>
        //public List<ScoringEventLogDTO> ScoringEventLogDTOList { get { return scoringEventLogDTOList; } set { scoringEventLogDTOList = value; } }

        /// <summary>
        /// Get/Set method of the ScoringEventCalendarDTOList field
        /// </summary>
        public List<ScoringEventCalendarDTO> ScoringEventCalendarDTOList { get { return scoringEventCalendarDTOList; } set { scoringEventCalendarDTOList = value; } }

        /// <summary>
        /// Get/Set method of the ScoringEventRewardsDTOList field
        /// </summary>
        public List<ScoringEventRewardsDTO> ScoringEventRewardsDTOList { get { return scoringEventRewardsDTOList; } set { scoringEventRewardsDTOList = value; } }

        /// <summary>
        /// Get/Set method of the ScoringEventDetailsDTOList field
        /// </summary>
        public List<ScoringEventDetailsDTO> ScoringEventDetailsDTOList { get { return scoringEventDetailsDTOList; } set { scoringEventDetailsDTOList = value; } }
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
        /// Returns whether the ScoringEventDTO changed or any of its  children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                //if (scoringEventLogDTOList != null &&
                //   scoringEventLogDTOList.Any(x => x.IsChanged))
                //{
                //    return true;
                //}
                if (scoringEventCalendarDTOList != null &&
                 scoringEventCalendarDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (scoringEventRewardsDTOList != null &&
                 scoringEventRewardsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (scoringEventDetailsDTOList != null &&
                 scoringEventDetailsDTOList.Any(x => x.IsChanged))
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

    public enum EventPatternTypes
    {
        /// <summary>
        /// Ordered
        /// </summary>
        Ordered,
        /// <summary>
        /// All
        /// </summary>
        All,
        /// <summary>
        /// Any
        /// </summary>
        Any
    }
    /// <summary>
    /// Converts TagNotificationStatus from/to string
    /// </summary>
    public class EventPatternTypesConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Converts EventPatternTypes from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EventPatternTypes FromString(string value)
        {
            log.LogMethodEntry("value");
            switch (value.ToUpper())
            {
                case "ORDERED":
                    {
                        return EventPatternTypes.Ordered;
                    }
                case "ANY":
                    {
                        return EventPatternTypes.Any;
                    }
                case "ALL":
                    {
                        return EventPatternTypes.All;
                    }
                default:
                    {
                        log.Error("Error :Not a valid string to convert to Event Pattern Type: " + value.ToUpper());
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Event Pattern Type");
                    }
            }
        }
        /// <summary>
        /// Converts EventPatternTypes to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(EventPatternTypes value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case EventPatternTypes.Ordered:
                    {
                        return "Ordered";
                    }
                case EventPatternTypes.Any:
                    {
                        return "Any";
                    }
                case EventPatternTypes.All:
                    {
                        return "All";
                    }
                default:
                    {
                        log.Error("Error :Not a valid Event pattern type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Event pattern type");
                    }
            }
        }
    }
}
