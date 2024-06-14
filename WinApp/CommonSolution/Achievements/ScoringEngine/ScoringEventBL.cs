/********************************************************************************************
 * Project Name - ScoringEngine
 * Description  - Bussiness logic of ScoringEvent
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.120.0        02-02-2021   Prajwal S      Created 
 ********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.GenericUtilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Achievements.ScoringEngine
{
    /// <summary>
    /// Represents a ScoringEvent.
    /// </summary>
    internal class ScoringEventBL
    {
        private ScoringEventDTO scoringEventDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        private ScoringEventBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="scoringEventDTO">ScoringEventDTO</param>
        internal ScoringEventBL(ExecutionContext executionContext, ScoringEventDTO scoringEventDTO)
             : this(executionContext)
        {
            log.LogMethodEntry(executionContext, scoringEventDTO);
            if (scoringEventDTO.ScoringEventId < 0)
            {
                //validate();
            }
            this.scoringEventDTO = scoringEventDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ScoringEventBL(ExecutionContext executionContext, int id, bool loadChildrecords, bool activeChildRecords, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ScoringEventDataHandler scoringEventDataHandler = new ScoringEventDataHandler(sqlTransaction);
            scoringEventDTO = scoringEventDataHandler.GetScoringEvent(id);
            if (scoringEventDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ScoringEvent", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildrecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the child records for MachineGroups object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            //ScoringEventLogListBL scoringEventLogList = new ScoringEventLogListBL();
            //List<KeyValuePair<ScoringEventLogDTO.SearchByScoringEventLogParameters, string>> searchByParams = new List<KeyValuePair<ScoringEventLogDTO.SearchByScoringEventLogParameters, string>>();
            //searchByParams.Add(new KeyValuePair<ScoringEventLogDTO.SearchByScoringEventLogParameters, string>(ScoringEventLogDTO.SearchByScoringEventLogParameters.SCORING_EVENT_ID, scoringEventDTO.ScoringEventId.ToString()));
            //if (activeChildRecords)
            //{
            //    searchByParams.Add(new KeyValuePair<ScoringEventLogDTO.SearchByScoringEventLogParameters, string>(ScoringEventLogDTO.SearchByScoringEventLogParameters.IS_ACTIVE, "1"));
            //}
            //scoringEventDTO.ScoringEventLogDTOList = scoringEventLogList.GetScoringEventLogDTOList(searchByParams, sqlTransaction);

            ScoringEventRewardsListBL ScoringEventRewardsList = new ScoringEventRewardsListBL();
            List<KeyValuePair<ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters, string>> searchByRewardsParams = new List<KeyValuePair<ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters, string>>();
            searchByRewardsParams.Add(new KeyValuePair<ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters, string>(ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.SCORING_EVENT_ID, scoringEventDTO.ScoringEventId.ToString()));
            if (activeChildRecords)
            {
                searchByRewardsParams.Add(new KeyValuePair<ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters, string>(ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.IS_ACTIVE, "1"));
            }
            scoringEventDTO.ScoringEventRewardsDTOList = ScoringEventRewardsList.GetScoringEventRewardsDTOList(searchByRewardsParams, true, true, sqlTransaction);

            ScoringEventDetailsListBL scoringEventDetailsList = new ScoringEventDetailsListBL();
            List<KeyValuePair<ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters, string>> searchByDetailsParams = new List<KeyValuePair<ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters, string>>();
            searchByDetailsParams.Add(new KeyValuePair<ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters, string>(ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.SCORING_EVENT_ID, scoringEventDTO.ScoringEventId.ToString()));
            if (activeChildRecords)
            {
                searchByDetailsParams.Add(new KeyValuePair<ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters, string>(ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.IS_ACTIVE, "1"));
            }
            scoringEventDTO.ScoringEventDetailsDTOList = scoringEventDetailsList.GetScoringEventDetailsDTOList(searchByDetailsParams, sqlTransaction);

            ScoringEventCalendarListBL ScoringEventCalendarList = new ScoringEventCalendarListBL();
            List<KeyValuePair<ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters, string>> searchByCalendarParams = new List<KeyValuePair<ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters, string>>();
            searchByCalendarParams.Add(new KeyValuePair<ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters, string>(ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.SCORING_EVENT_ID, scoringEventDTO.ScoringEventId.ToString()));
            if (activeChildRecords)
            {
                searchByCalendarParams.Add(new KeyValuePair<ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters, string>(ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.IS_ACTIVE, "1"));
            }
            scoringEventDTO.ScoringEventCalendarDTOList = ScoringEventCalendarList.GetScoringEventCalendarDTOList(searchByCalendarParams, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// get ScoringEventDTO Object
        /// </summary>
        internal ScoringEventDTO ScoringEventDTO
        {
            get { return scoringEventDTO; }
        }


        /// <summary>
        /// Saves the ScoringEvents
        /// Checks if the id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ScoringEventDataHandler scoringEventDataHandler = new ScoringEventDataHandler(sqlTransaction);

            if (scoringEventDTO.ScoringEventId < 0)
            {
                scoringEventDTO = scoringEventDataHandler.Insert(scoringEventDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                scoringEventDTO.AcceptChanges();
            }
            else
            {
                if (scoringEventDTO.IsChanged)
                {
                    scoringEventDTO = scoringEventDataHandler.Update(scoringEventDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    scoringEventDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();

            // Will Save the Child ScoringEventLogDTO
            //log.Debug("scoringEventDTO.ScoringEventLogDTO Value :" + scoringEventDTO.ScoringEventLogDTOList);
            //if (scoringEventDTO.ScoringEventLogDTOList != null && scoringEventDTO.ScoringEventLogDTOList.Any())
            //{
            //    List<ScoringEventLogDTO> updatedScoringEventLogDTO = new List<ScoringEventLogDTO>();
            //    foreach (ScoringEventLogDTO scoringEventLogDTO in scoringEventDTO.ScoringEventLogDTOList)
            //    {
            //        if (scoringEventLogDTO.ScoringEventId != scoringEventDTO.ScoringEventId)
            //        {
            //            scoringEventLogDTO.ScoringEventId = scoringEventDTO.ScoringEventId;
            //        }
            //        log.Debug("ScoringEventLogDTO.IsChanged Value :" + scoringEventLogDTO.IsChanged);
            //        if (scoringEventLogDTO.IsChanged)
            //        {
            //            updatedScoringEventLogDTO.Add(scoringEventLogDTO);
            //        }
            //    }
            //    log.Debug("updatedScoringEventLogDTO Value :" + updatedScoringEventLogDTO);
            //    if (updatedScoringEventLogDTO.Any())
            //    {
            //        ScoringEventLogListBL scoringEventLogListBL = new ScoringEventLogListBL(executionContext, updatedScoringEventLogDTO);
            //        scoringEventLogListBL.Save(sqlTransaction);
            //    }
            //}

            // Will Save the Child ScoringEventDetailsDTO
            log.Debug("scoringEventDTO.ScoringEventDetailsDTO Value :" + scoringEventDTO.ScoringEventDetailsDTOList);
            if (scoringEventDTO.ScoringEventDetailsDTOList != null && scoringEventDTO.ScoringEventDetailsDTOList.Any())
            {
                List<ScoringEventDetailsDTO> updatedScoringEventDetailsDTO = new List<ScoringEventDetailsDTO>();
                foreach (ScoringEventDetailsDTO scoringEventDetailsDTO in scoringEventDTO.ScoringEventDetailsDTOList)
                {
                    if (scoringEventDetailsDTO.ScoringEventId != scoringEventDTO.ScoringEventId)
                    {
                        scoringEventDetailsDTO.ScoringEventId = scoringEventDTO.ScoringEventId;
                    }
                    log.Debug("ScoringEventDetailsDTO.IsChanged Value :" + scoringEventDetailsDTO.IsChanged);
                    if (scoringEventDetailsDTO.IsChanged)
                    {
                        updatedScoringEventDetailsDTO.Add(scoringEventDetailsDTO);
                    }
                }
                log.Debug("updatedScoringEventDetailsDTO Value :" + updatedScoringEventDetailsDTO);
                if (updatedScoringEventDetailsDTO.Any())
                {
                    ScoringEventDetailsListBL scoringEventDetailsListBL = new ScoringEventDetailsListBL(executionContext, updatedScoringEventDetailsDTO);
                    scoringEventDetailsListBL.Save(sqlTransaction);
                }
            }

              // Will Save the Child ScoringEventRewardsDTO
            log.Debug("scoringEventDTO.ScoringEventRewardsDTO Value :" + scoringEventDTO.ScoringEventRewardsDTOList);
            if (scoringEventDTO.ScoringEventRewardsDTOList != null && scoringEventDTO.ScoringEventRewardsDTOList.Any())
            {
                List<ScoringEventRewardsDTO> updatedScoringEventRewardsDTO = new List<ScoringEventRewardsDTO>();
                foreach (ScoringEventRewardsDTO scoringEventRewardsDTO in scoringEventDTO.ScoringEventRewardsDTOList)
                {
                    if (scoringEventRewardsDTO.ScoringEventId != scoringEventDTO.ScoringEventId)
                    {
                        scoringEventRewardsDTO.ScoringEventId = scoringEventDTO.ScoringEventId;
                    }
                    log.Debug("ScoringEventRewardsDTO.IsChanged Value :" + scoringEventRewardsDTO.IsChanged);
                    if (scoringEventRewardsDTO.IsChanged)
                    {
                        updatedScoringEventRewardsDTO.Add(scoringEventRewardsDTO);
                    }
                }
                log.Debug("updatedScoringEventRewardsDTO Value :" + updatedScoringEventRewardsDTO);
                if (updatedScoringEventRewardsDTO.Any())
                {
                    ScoringEventRewardsListBL scoringEventRewardsListBL = new ScoringEventRewardsListBL(executionContext, updatedScoringEventRewardsDTO);
                    scoringEventRewardsListBL.Save(sqlTransaction);
                }
            }

            // Will Save the Child ScoringEventCalendarDTO
            log.Debug("scoringEventDTO.ScoringEventCalendarDTO Value :" + scoringEventDTO.ScoringEventCalendarDTOList);
            if (scoringEventDTO.ScoringEventCalendarDTOList != null && scoringEventDTO.ScoringEventCalendarDTOList.Any())
            {
                List<ScoringEventCalendarDTO> updatedScoringEventCalendarDTO = new List<ScoringEventCalendarDTO>();
                foreach (ScoringEventCalendarDTO scoringEventCalendarDTO in scoringEventDTO.ScoringEventCalendarDTOList)
                {
                    if (scoringEventCalendarDTO.ScoringEventId != scoringEventDTO.ScoringEventId)
                    {
                        scoringEventCalendarDTO.ScoringEventId = scoringEventDTO.ScoringEventId;
                    }
                    log.Debug("ScoringEventCalendarDTO.IsChanged Value :" + scoringEventCalendarDTO.IsChanged);
                    if (scoringEventCalendarDTO.IsChanged)
                    {
                        updatedScoringEventCalendarDTO.Add(scoringEventCalendarDTO);
                    }
                }
                log.Debug("updatedScoringEventCalendarDTO Value :" + updatedScoringEventCalendarDTO);
                if (updatedScoringEventCalendarDTO.Any())
                {
                    ScoringEventCalendarListBL scoringEventCalendarListBL = new ScoringEventCalendarListBL(executionContext, updatedScoringEventCalendarDTO);
                    scoringEventCalendarListBL.Save(sqlTransaction);
                }
            }
        }

        internal void Update(ScoringEventDTO parameterScoringEventDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterScoringEventDTO);
            ChangeScoringEventPolicyId(parameterScoringEventDTO.ScoringEventPolicyId);
            ChangeActiveFlag(parameterScoringEventDTO.ActiveFlag);
            ChangeAchievementClassId(parameterScoringEventDTO.AchievementClassId);
            ChangeEventName(parameterScoringEventDTO.EventName);
            ChangeEnforcePattern(parameterScoringEventDTO.EnforcePattern);
            ChangeEventType(parameterScoringEventDTO.EventType);
            ChangeOnDifferentDays(parameterScoringEventDTO.OnDifferentDays);
            ChangePatternBreachMaxAllowed(parameterScoringEventDTO.PatternBreachMaxAllowed);
            ChangeOnResetBreachCountOnProgress(parameterScoringEventDTO.ResetBreachCountOnProgress);
            ChangeTeamEvent(parameterScoringEventDTO.TeamEvent);
            ChangeTimeLimitInDays(parameterScoringEventDTO.TimeLimitInDays);
            ChangeTimeLimitInMinutes(parameterScoringEventDTO.TimeLimitInMinutes);

            //Dictionary<int, ScoringEventLogDTO> scoringEventLogDTODictionary = new Dictionary<int, ScoringEventLogDTO>();
            //if (scoringEventDTO.ScoringEventLogDTOList != null &&
            //    scoringEventDTO.ScoringEventLogDTOList.Any())
            //{
            //    foreach (var scoringEventLogDTO in scoringEventDTO.ScoringEventLogDTOList)
            //    {
            //        scoringEventLogDTODictionary.Add(scoringEventLogDTO.ScoringEventLogId, scoringEventLogDTO);
            //    }
            //}
            //if (parameterScoringEventDTO.ScoringEventLogDTOList != null &&
            //    parameterScoringEventDTO.ScoringEventLogDTOList.Any())
            //{
            //    foreach (var parameterScoringEventLogDTO in parameterScoringEventDTO.ScoringEventLogDTOList)
            //    {
            //        if (scoringEventLogDTODictionary.ContainsKey(parameterScoringEventLogDTO.ScoringEventLogId))
            //        {
            //            ScoringEventLogBL scoringEventLog = new ScoringEventLogBL(executionContext, scoringEventLogDTODictionary[parameterScoringEventLogDTO.ScoringEventLogId]);
            //            scoringEventLog.Update(parameterScoringEventLogDTO, sqlTransaction );
            //        }
            //        else if (parameterScoringEventLogDTO.ScoringEventLogId > -1)
            //        {
            //            ScoringEventLogBL scoringEventLog = new ScoringEventLogBL(executionContext, parameterScoringEventLogDTO.ScoringEventLogId, sqlTransaction);
            //            if (scoringEventDTO.ScoringEventLogDTOList == null)
            //            {
            //                scoringEventDTO.ScoringEventLogDTOList = new List<ScoringEventLogDTO>();
            //            }
            //            scoringEventDTO.ScoringEventLogDTOList.Add(scoringEventLog.ScoringEventLogDTO);
            //            scoringEventLog.Update(parameterScoringEventLogDTO, sqlTransaction);
            //        }
            //        else
            //        {
            //            ScoringEventLogBL scoringEventLog = new ScoringEventLogBL(executionContext, parameterScoringEventLogDTO);
            //            if (scoringEventDTO.ScoringEventLogDTOList == null)
            //            {
            //                scoringEventDTO.ScoringEventLogDTOList = new List<ScoringEventLogDTO>();
            //            }
            //            scoringEventDTO.ScoringEventLogDTOList.Add(scoringEventLog.ScoringEventLogDTO);
            //        }
            //    }
            //}


            Dictionary<int, ScoringEventDetailsDTO> scoringEventDetailsDTODictionary = new Dictionary<int, ScoringEventDetailsDTO>();
            if (scoringEventDTO.ScoringEventDetailsDTOList != null &&
                scoringEventDTO.ScoringEventDetailsDTOList.Any())
            {
                foreach (var scoringEventDetailsDTO in scoringEventDTO.ScoringEventDetailsDTOList)
                {
                    scoringEventDetailsDTODictionary.Add(scoringEventDetailsDTO.ScoringEventDetailId, scoringEventDetailsDTO);
                }
            }
            if (parameterScoringEventDTO.ScoringEventDetailsDTOList != null &&
                parameterScoringEventDTO.ScoringEventDetailsDTOList.Any())
            {
                foreach (var parameterScoringEventDetailsDTO in parameterScoringEventDTO.ScoringEventDetailsDTOList)
                {
                    if (scoringEventDetailsDTODictionary.ContainsKey(parameterScoringEventDetailsDTO.ScoringEventDetailId))
                    {
                        ScoringEventDetailsBL scoringEventDetails = new ScoringEventDetailsBL(executionContext, scoringEventDetailsDTODictionary[parameterScoringEventDetailsDTO.ScoringEventDetailId]);
                        scoringEventDetails.Update(parameterScoringEventDetailsDTO, sqlTransaction);
                    }
                    else if (parameterScoringEventDetailsDTO.ScoringEventDetailId > -1)
                    {
                        ScoringEventDetailsBL scoringEventDetails = new ScoringEventDetailsBL(executionContext, parameterScoringEventDetailsDTO.ScoringEventDetailId, sqlTransaction);
                        if (scoringEventDTO.ScoringEventDetailsDTOList == null)
                        {
                            scoringEventDTO.ScoringEventDetailsDTOList = new List<ScoringEventDetailsDTO>();
                        }
                        scoringEventDTO.ScoringEventDetailsDTOList.Add(scoringEventDetails.ScoringEventDetailsDTO);
                        scoringEventDetails.Update(parameterScoringEventDetailsDTO, sqlTransaction);
                    }
                    else
                    {
                        ScoringEventDetailsBL scoringEventDetails = new ScoringEventDetailsBL(executionContext, parameterScoringEventDetailsDTO);
                        if (scoringEventDTO.ScoringEventDetailsDTOList == null)
                        {
                            scoringEventDTO.ScoringEventDetailsDTOList = new List<ScoringEventDetailsDTO>();
                        }
                        scoringEventDTO.ScoringEventDetailsDTOList.Add(scoringEventDetails.ScoringEventDetailsDTO);
                    }
                }
            }

            Dictionary<int, ScoringEventRewardsDTO> scoringEventRewardsDTODictionary = new Dictionary<int, ScoringEventRewardsDTO>();
            if (scoringEventDTO.ScoringEventRewardsDTOList != null &&
                scoringEventDTO.ScoringEventRewardsDTOList.Any())
            {
                foreach (var scoringEventRewardsDTO in scoringEventDTO.ScoringEventRewardsDTOList)
                {
                    scoringEventRewardsDTODictionary.Add(scoringEventRewardsDTO.ScoringRewardId, scoringEventRewardsDTO);
                }
            }
            if (parameterScoringEventDTO.ScoringEventRewardsDTOList != null &&
                parameterScoringEventDTO.ScoringEventRewardsDTOList.Any())
            {
                foreach (var parameterScoringEventRewardsDTO in parameterScoringEventDTO.ScoringEventRewardsDTOList)
                {
                    if (scoringEventRewardsDTODictionary.ContainsKey(parameterScoringEventRewardsDTO.ScoringRewardId))
                    {
                        ScoringEventRewardsBL scoringEventRewards = new ScoringEventRewardsBL(executionContext, scoringEventRewardsDTODictionary[parameterScoringEventRewardsDTO.ScoringRewardId]);
                        scoringEventRewards.Update(parameterScoringEventRewardsDTO, sqlTransaction);
                    }
                    else if (parameterScoringEventRewardsDTO.ScoringRewardId > -1)
                    {
                        ScoringEventRewardsBL scoringEventRewards = new ScoringEventRewardsBL(executionContext, parameterScoringEventRewardsDTO.ScoringRewardId, true, true, sqlTransaction);
                        if (scoringEventDTO.ScoringEventRewardsDTOList == null)
                        {
                            scoringEventDTO.ScoringEventRewardsDTOList = new List<ScoringEventRewardsDTO>();
                        }
                        scoringEventDTO.ScoringEventRewardsDTOList.Add(scoringEventRewards.ScoringEventRewardsDTO);
                        scoringEventRewards.Update(parameterScoringEventRewardsDTO, sqlTransaction);
                    }
                    else
                    {
                        ScoringEventRewardsBL scoringEventRewards = new ScoringEventRewardsBL(executionContext, parameterScoringEventRewardsDTO);
                        if (scoringEventDTO.ScoringEventRewardsDTOList == null)
                        {
                            scoringEventDTO.ScoringEventRewardsDTOList = new List<ScoringEventRewardsDTO>();
                        }
                        scoringEventDTO.ScoringEventRewardsDTOList.Add(scoringEventRewards.ScoringEventRewardsDTO);
                    }
                }
            }

            Dictionary<int, ScoringEventCalendarDTO> scoringEventCalendarDTODictionary = new Dictionary<int, ScoringEventCalendarDTO>();
            if (scoringEventDTO.ScoringEventCalendarDTOList != null &&
                scoringEventDTO.ScoringEventCalendarDTOList.Any())
            {
                foreach (var scoringEventCalendarDTO in scoringEventDTO.ScoringEventCalendarDTOList)
                {
                    scoringEventCalendarDTODictionary.Add(scoringEventCalendarDTO.Id, scoringEventCalendarDTO);
                }
            }
            if (parameterScoringEventDTO.ScoringEventCalendarDTOList != null &&
                parameterScoringEventDTO.ScoringEventCalendarDTOList.Any())
            {
                foreach (var parameterScoringEventCalendarDTO in parameterScoringEventDTO.ScoringEventCalendarDTOList)
                {
                    if (scoringEventCalendarDTODictionary.ContainsKey(parameterScoringEventCalendarDTO.Id))
                    {
                        ScoringEventCalendarBL scoringEventCalendar = new ScoringEventCalendarBL(executionContext, scoringEventCalendarDTODictionary[parameterScoringEventCalendarDTO.Id]);
                        scoringEventCalendar.Update(parameterScoringEventCalendarDTO, sqlTransaction);
                    }
                    else if (parameterScoringEventCalendarDTO.Id > -1)
                    {
                        ScoringEventCalendarBL scoringEventCalendar = new ScoringEventCalendarBL(executionContext, parameterScoringEventCalendarDTO.Id, sqlTransaction);
                        if (scoringEventDTO.ScoringEventCalendarDTOList == null)
                        {
                            scoringEventDTO.ScoringEventCalendarDTOList = new List<ScoringEventCalendarDTO>();
                        }
                        scoringEventDTO.ScoringEventCalendarDTOList.Add(scoringEventCalendar.ScoringEventCalendarDTO);
                        scoringEventCalendar.Update(parameterScoringEventCalendarDTO, sqlTransaction);
                    }
                    else
                    {
                        ScoringEventCalendarBL scoringEventCalendar = new ScoringEventCalendarBL(executionContext, parameterScoringEventCalendarDTO);
                        if (scoringEventDTO.ScoringEventCalendarDTOList == null)
                        {
                            scoringEventDTO.ScoringEventCalendarDTOList = new List<ScoringEventCalendarDTO>();
                        }
                        scoringEventDTO.ScoringEventCalendarDTOList.Add(scoringEventCalendar.ScoringEventCalendarDTO);
                    }
                }
            }

            log.LogMethodExit();
        }


        private void ChangeTimeLimitInDays(int? timeLimitInDays)
        {
            log.LogMethodEntry(timeLimitInDays);
            if (scoringEventDTO.TimeLimitInDays == timeLimitInDays)
            {
                log.LogMethodExit(null, "No changes to TimeLimitInDays");
                return;
            }
            scoringEventDTO.TimeLimitInDays = timeLimitInDays;
            log.LogMethodExit();
        }

        private void ChangeTimeLimitInMinutes(int? timeLimitInMinutes)
        {
            log.LogMethodEntry(timeLimitInMinutes);
            if (scoringEventDTO.TimeLimitInMinutes == timeLimitInMinutes)
            {
                log.LogMethodExit(null, "No changes to TimeLimitInMinutes");
                return;
            }
            scoringEventDTO.TimeLimitInMinutes = timeLimitInMinutes;
            log.LogMethodExit();
        }

        private void ChangePatternBreachMaxAllowed(int? patternBreachMaxAllowed)
        {
            log.LogMethodEntry(patternBreachMaxAllowed);
            if (scoringEventDTO.PatternBreachMaxAllowed == patternBreachMaxAllowed)
            {
                log.LogMethodExit(null, "No changes to PatternBreachMaxAllowed");
                return;
            }
            scoringEventDTO.PatternBreachMaxAllowed = patternBreachMaxAllowed;
            log.LogMethodExit();
        }

        private void ChangeOnResetBreachCountOnProgress(bool resetBreachCountOnProgress)
        {
            log.LogMethodEntry(resetBreachCountOnProgress);
            if (scoringEventDTO.ResetBreachCountOnProgress == resetBreachCountOnProgress)
            {
                log.LogMethodExit(null, "No changes to ResetBreachCountOnProgress");
                return;
            }
            scoringEventDTO.ResetBreachCountOnProgress = resetBreachCountOnProgress;
            log.LogMethodExit();
        }
        private void ChangeTeamEvent(bool teamEvent)
        {
            log.LogMethodEntry(teamEvent);
            if (scoringEventDTO.TeamEvent == teamEvent)
            {
                log.LogMethodExit(null, "No changes to TeamEvent");
                return;
            }
            scoringEventDTO.TeamEvent = teamEvent;
            log.LogMethodExit();
        }
        private void ChangeOnDifferentDays(bool onDifferentDays)
        {
            log.LogMethodEntry(onDifferentDays);
            if (scoringEventDTO.OnDifferentDays == onDifferentDays)
            {
                log.LogMethodExit(null, "No changes to OnDifferentDays");
                return;
            }
            scoringEventDTO.OnDifferentDays = onDifferentDays;
            log.LogMethodExit();
        }
        private void ChangeEventType(EventPatternTypes eventType)
        {
            log.LogMethodEntry(eventType);
            if (EventPatternTypesConverter.ToString(scoringEventDTO.EventType) == EventPatternTypesConverter.ToString(eventType))
            {
                log.LogMethodExit(null, "No changes to EventType");
                return;
            }
            scoringEventDTO.EventType = eventType;
            log.LogMethodExit();
        }

        private void ChangeEnforcePattern(bool enforcePattern)
        {
            log.LogMethodEntry(enforcePattern);
            if (scoringEventDTO.EnforcePattern == enforcePattern)
            {
                log.LogMethodExit(null, "No changes to EnforcePattern");
                return;
            }
            scoringEventDTO.EnforcePattern = enforcePattern;
            log.LogMethodExit();
        }

        private void ChangeAchievementClassId(int achievementClassId)
        {
            log.LogMethodEntry(achievementClassId);
            if (scoringEventDTO.AchievementClassId == achievementClassId)
            {
                log.LogMethodExit(null, "No changes to AchievementClassId");
                return;
            }
            scoringEventDTO.AchievementClassId = achievementClassId;
            log.LogMethodExit();
        }

        private void ChangeEventName(string eventName)
        {
            log.LogMethodEntry(eventName);
            if (scoringEventDTO.EventName == eventName)
            {
                log.LogMethodExit(null, "No changes to EventName");
                return;
            }
            scoringEventDTO.EventName = eventName;
            log.LogMethodExit();
        }

        private void ChangeActiveFlag(bool activeFlag)
        {
            log.LogMethodEntry(activeFlag);
            if (scoringEventDTO.ActiveFlag == activeFlag)
            {
                log.LogMethodExit(null, "No changes to ScoringEvent activeFlag");
                return;
            }
            scoringEventDTO.ActiveFlag = activeFlag;
            log.LogMethodExit();
        }

        private void ChangeScoringEventPolicyId(int scoringEventPolicyId)
        {
            log.LogMethodEntry(scoringEventPolicyId);
            // ValidateScoringEventId(scoringEventId);
            if (scoringEventDTO.ScoringEventPolicyId == scoringEventPolicyId)
            {
                log.LogMethodExit(null, "No changes to ScoringEventPolicyId");
                return;
            }
            scoringEventDTO.ScoringEventPolicyId = scoringEventPolicyId;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of ScoringEvent
    /// </summary>
    public class ScoringEventListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ScoringEventDTO> scoringEventList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScoringEventListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="ScoringEventList"></param>
        public ScoringEventListBL(ExecutionContext executionContext, List<ScoringEventDTO> ScoringEventList)
             : this()
        {
            log.LogMethodEntry(executionContext, ScoringEventList);
            this.executionContext = executionContext;
            this.scoringEventList = ScoringEventList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ScoringEventDTO List for scoringEventId Id List
        /// </summary>
        /// <param name="scoringEventIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<ScoringEventDTO> GetScoringEventDTOListOfPolicy(List<int> scoringEventIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scoringEventIdList, activeRecords, sqlTransaction);
            ScoringEventDataHandler scoringEventDataHandler = new ScoringEventDataHandler(sqlTransaction);
            List<ScoringEventDTO> scoringEventDTOList = scoringEventDataHandler.GetScoringEventDTOList(scoringEventIdList, activeRecords);

            Build(scoringEventDTOList, activeRecords, sqlTransaction);

            log.LogMethodExit(scoringEventDTOList);
            return scoringEventDTOList;
        }

        /// <summary>
        /// Returns the ScoringEvent list
        /// </summary>
        public List<ScoringEventDTO> GetScoringEventDTOList(ExecutionContext executionContext, List<KeyValuePair<ScoringEventDTO.SearchByScoringEventParameters, string>> searchParameters, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, searchParameters, sqlTransaction);
            this.executionContext = executionContext;
            ScoringEventDataHandler scoringEventDataHandler = new ScoringEventDataHandler(sqlTransaction);
            List<ScoringEventDTO> scoringEventDTOList = scoringEventDataHandler.GetScoringEventList(searchParameters);
            if (loadChildRecords)
            {
                Build(scoringEventDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(scoringEventDTOList);
            return scoringEventDTOList;
        }

        /// <summary>
        /// Builds the List of ScoringEvent object based on the list of ScoringEvent id.
        /// </summary>
        /// <param name="scoringEventDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<ScoringEventDTO> scoringEventDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scoringEventDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ScoringEventDTO> scoringEventDTOIdMap = new Dictionary<int, ScoringEventDTO>();
            List<int> scoringEventIdList = new List<int>();
            for (int i = 0; i < scoringEventDTOList.Count; i++)
            {
                if (scoringEventDTOIdMap.ContainsKey(scoringEventDTOList[i].ScoringEventId))
                {
                    continue;
                }
                scoringEventDTOIdMap.Add(scoringEventDTOList[i].ScoringEventId, scoringEventDTOList[i]);
                scoringEventIdList.Add(scoringEventDTOList[i].ScoringEventId);
            }

            //ScoringEventLogListBL scoringEventLogListBL = new ScoringEventLogListBL();
            //List<ScoringEventLogDTO> scoringEventLogDTOList = scoringEventLogListBL.GetScoringEventLogDTOListOfEvents(scoringEventIdList, activeChildRecords, sqlTransaction);
            //if (scoringEventLogDTOList != null && scoringEventLogDTOList.Any())
            //{
            //    for (int i = 0; i < scoringEventLogDTOList.Count; i++)
            //    {
            //        if (scoringEventDTOIdMap.ContainsKey(scoringEventLogDTOList[i].ScoringEventId) == false)
            //        {
            //            continue;
            //        }
            //        ScoringEventDTO scoringEventDTO = scoringEventDTOIdMap[scoringEventLogDTOList[i].ScoringEventId];
            //        if (scoringEventDTO.ScoringEventLogDTOList == null)
            //        {
            //            scoringEventDTO.ScoringEventLogDTOList = new List<ScoringEventLogDTO>();
            //        }
            //        scoringEventDTO.ScoringEventLogDTOList.Add(scoringEventLogDTOList[i]);
            //    }
            //}

            ScoringEventDetailsListBL scoringEventDetailsListBL = new ScoringEventDetailsListBL();
            List<ScoringEventDetailsDTO> scoringEventDetailsDTOList = scoringEventDetailsListBL.GetScoringEventDetailsDTOListOfEvents(scoringEventIdList, activeChildRecords, sqlTransaction);
            if (scoringEventDetailsDTOList != null && scoringEventDetailsDTOList.Any())
            {
                for (int i = 0; i < scoringEventDetailsDTOList.Count; i++)
                {
                    if (scoringEventDTOIdMap.ContainsKey(scoringEventDetailsDTOList[i].ScoringEventId) == false)
                    {
                        continue;
                    }
                    ScoringEventDTO scoringEventDTO = scoringEventDTOIdMap[scoringEventDetailsDTOList[i].ScoringEventId];
                    if (scoringEventDTO.ScoringEventDetailsDTOList == null)
                    {
                        scoringEventDTO.ScoringEventDetailsDTOList = new List<ScoringEventDetailsDTO>();
                    }
                    scoringEventDTO.ScoringEventDetailsDTOList.Add(scoringEventDetailsDTOList[i]);
                }
            }

            ScoringEventRewardsListBL scoringEventRewardsListBL = new ScoringEventRewardsListBL();
            List<ScoringEventRewardsDTO> scoringEventRewardsDTOList = scoringEventRewardsListBL.GetScoringEventRewardsDTOListOfEvents(scoringEventIdList, activeChildRecords, sqlTransaction);
            if (scoringEventRewardsDTOList != null && scoringEventRewardsDTOList.Any())
            {
                for (int i = 0; i < scoringEventRewardsDTOList.Count; i++)
                {
                    if (scoringEventDTOIdMap.ContainsKey(scoringEventRewardsDTOList[i].ScoringEventId) == false)
                    {
                        continue;
                    }
                    ScoringEventDTO scoringEventDTO = scoringEventDTOIdMap[scoringEventRewardsDTOList[i].ScoringEventId];
                    if (scoringEventDTO.ScoringEventRewardsDTOList == null)
                    {
                        scoringEventDTO.ScoringEventRewardsDTOList = new List<ScoringEventRewardsDTO>();
                    }
                    scoringEventDTO.ScoringEventRewardsDTOList.Add(scoringEventRewardsDTOList[i]);
                }
            }

            ScoringEventCalendarListBL scoringEventCalendarListBL = new ScoringEventCalendarListBL();
            List<ScoringEventCalendarDTO> scoringEventCalendarDTOList = scoringEventCalendarListBL.GetScoringEventCalendarDTOListOfEvents(scoringEventIdList, activeChildRecords, sqlTransaction);
            if (scoringEventCalendarDTOList != null && scoringEventCalendarDTOList.Any())
            {
                for (int i = 0; i < scoringEventCalendarDTOList.Count; i++)
                {
                    if (scoringEventDTOIdMap.ContainsKey(scoringEventCalendarDTOList[i].ScoringEventId) == false)
                    {
                        continue;
                    }
                    ScoringEventDTO scoringEventDTO = scoringEventDTOIdMap[scoringEventCalendarDTOList[i].ScoringEventId];
                    if (scoringEventDTO.ScoringEventCalendarDTOList == null)
                    {
                        scoringEventDTO.ScoringEventCalendarDTOList = new List<ScoringEventCalendarDTO>();
                    }
                    scoringEventDTO.ScoringEventCalendarDTOList.Add(scoringEventCalendarDTOList[i]);
                }
            }
        }

        /// <summary>
        /// This method should be used to Save ScoringEvent
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (scoringEventList != null && scoringEventList.Count != 0)
                {
                    foreach (ScoringEventDTO scoringEventDTO in scoringEventList)
                    {
                        ScoringEventBL scoringEventBL = new ScoringEventBL(executionContext, scoringEventDTO);
                        scoringEventBL.Save(sqlTransaction);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }
}
