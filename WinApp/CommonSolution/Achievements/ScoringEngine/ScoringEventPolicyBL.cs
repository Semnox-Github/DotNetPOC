/********************************************************************************************
 * Project Name - ScoringEngine
 * Description  - Bussiness logic of ScoringEventPolicy
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.120.0        02-02-2021   Prajwal S      Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Achievements.ScoringEngine
{
    /// <summary>
    /// Represents a ScoringEventPolicy.
    /// </summary>
    public class ScoringEventPolicyBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ScoringEventPolicyDTO scoringEventPolicyDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private ScoringEventPolicyBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterScoringEventPolicyDTO">scoringEventPolicyDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ScoringEventPolicyBL(ExecutionContext executionContext, ScoringEventPolicyDTO parameterScoringEventPolicyDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterScoringEventPolicyDTO, sqlTransaction);

            if (parameterScoringEventPolicyDTO.ScoringEventPolicyId > -1)
            {
                LoadScoringEventPolicyDTO(parameterScoringEventPolicyDTO.ScoringEventPolicyId, true, true, sqlTransaction);//added sql
                ThrowIfScoringEventPolicyDTOIsNull(parameterScoringEventPolicyDTO.ScoringEventPolicyId);
                Update(parameterScoringEventPolicyDTO, sqlTransaction);
            }
            else
            {
                //Validate();
                scoringEventPolicyDTO = new ScoringEventPolicyDTO(-1, parameterScoringEventPolicyDTO.ScoringPolicyName, parameterScoringEventPolicyDTO.StartDate, parameterScoringEventPolicyDTO.EndDate, parameterScoringEventPolicyDTO.AchievementClassId, true);
                if (parameterScoringEventPolicyDTO.ScoringEventDTOList != null && parameterScoringEventPolicyDTO.ScoringEventDTOList.Any())
                {
                    scoringEventPolicyDTO.ScoringEventDTOList = new List<ScoringEventDTO>();
                    foreach (ScoringEventDTO parameterScoringEventDTO in parameterScoringEventPolicyDTO.ScoringEventDTOList)
                    {
                        if (parameterScoringEventDTO.ScoringEventId > -1)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2196, "ScoringEvent", parameterScoringEventDTO.ScoringEventId);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new EntityNotFoundException(message);
                        }
                        var scoringEventDTO = new ScoringEventDTO(-1, -1, parameterScoringEventDTO.TimeLimitInDays, parameterScoringEventDTO.TimeLimitInMinutes, parameterScoringEventDTO.TeamEvent,
                                                                   parameterScoringEventDTO.EventType, parameterScoringEventDTO.EventName, parameterScoringEventDTO.EnforcePattern, parameterScoringEventDTO.PatternBreachMaxAllowed,
                                                                   parameterScoringEventDTO.ResetBreachCountOnProgress, parameterScoringEventDTO.OnDifferentDays, parameterScoringEventDTO.AchievementClassId, true);
                        scoringEventDTO.ScoringEventCalendarDTOList = parameterScoringEventDTO.ScoringEventCalendarDTOList;
                        scoringEventDTO.ScoringEventDetailsDTOList = parameterScoringEventDTO.ScoringEventDetailsDTOList;
                       // scoringEventDTO.ScoringEventLogDTOList = parameterScoringEventDTO.ScoringEventLogDTOList;
                        scoringEventDTO.ScoringEventRewardsDTOList = parameterScoringEventDTO.ScoringEventRewardsDTOList;
                        ScoringEventBL scoringEventBL = new ScoringEventBL(executionContext, scoringEventDTO);
                        scoringEventPolicyDTO.ScoringEventDTOList.Add(scoringEventBL.ScoringEventDTO);
                    }
                }

            }
            log.LogMethodExit();
        }

        private void LoadScoringEventPolicyDTO(int scoringEventPolicyId, bool loadChildRecords, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scoringEventPolicyId, loadChildRecords, activeChildRecords);
            ScoringEventPolicyDataHandler scoringEventPolicyDataHandler = new ScoringEventPolicyDataHandler(sqlTransaction);
            scoringEventPolicyDTO = scoringEventPolicyDataHandler.GetScoringEventPolicy(scoringEventPolicyId);
            ThrowIfScoringEventPolicyDTOIsNull(scoringEventPolicyId);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void ThrowIfScoringEventPolicyDTOIsNull(int scoringEventPolicyId)
        {
            log.LogMethodEntry();
            if (scoringEventPolicyDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "scoringEventPolicy", scoringEventPolicyId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(activeChildRecords);
            ScoringEventListBL scoringEventListBL = new ScoringEventListBL();
            List<ScoringEventDTO> scoringEventDTOList = scoringEventListBL.GetScoringEventDTOListOfPolicy(new List<int>() { scoringEventPolicyDTO.ScoringEventPolicyId }, activeChildRecords, sqlTransaction);
            scoringEventPolicyDTO.ScoringEventDTOList = scoringEventDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the scoringEventPolicyId parameter
        /// </summary>
        /// <param name="scoringEventPolicyId">scoringEventPolicyId</param>
        /// <param name="loadChildRecords">To load the child DTO Records</param>
        public ScoringEventPolicyBL(ExecutionContext executionContext, int scoringEventPolicyId, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(scoringEventPolicyId, loadChildRecords, activeChildRecords);
            LoadScoringEventPolicyDTO(scoringEventPolicyId, loadChildRecords, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// get ScoringEventPolicyDTO Object
        /// </summary>
        public ScoringEventPolicyDTO ScoringEventPolicyDTO
        {
            get
            {
                ScoringEventPolicyDTO result = new ScoringEventPolicyDTO(scoringEventPolicyDTO);
                result.AcceptChanges();
                return result;
            }
        }

        /// <summary>
        /// Saves the ScoringEventPolicy
        /// Checks if the scoringEventPolicy id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {

            log.LogMethodEntry(sqlTransaction);

            ScoringEventPolicyDataHandler scoringEventPolicyDataHandler = new ScoringEventPolicyDataHandler(sqlTransaction);
            if (scoringEventPolicyDTO.ScoringEventPolicyId < 0)
            {
                scoringEventPolicyDTO = scoringEventPolicyDataHandler.Insert(scoringEventPolicyDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                scoringEventPolicyDTO.AcceptChanges();
            }
            else
            {
                if (scoringEventPolicyDTO.IsChanged)
                {
                    scoringEventPolicyDTO = scoringEventPolicyDataHandler.Update(scoringEventPolicyDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    scoringEventPolicyDTO.AcceptChanges();
                }
                log.LogMethodExit();
            }
            // Will Save the Child ScoringEventDTO
            log.Debug("scoringEventPolicyDTO.ScoringEventDTOList Value :" + scoringEventPolicyDTO.ScoringEventDTOList);
            if (scoringEventPolicyDTO.ScoringEventDTOList != null && scoringEventPolicyDTO.ScoringEventDTOList.Any())
            {
                List<ScoringEventDTO> updatedScoringEventDTO = new List<ScoringEventDTO>();
                foreach (ScoringEventDTO scoringEventDTO in scoringEventPolicyDTO.ScoringEventDTOList)
                {
                    if (scoringEventDTO.ScoringEventPolicyId != scoringEventPolicyDTO.ScoringEventPolicyId)
                    {
                        scoringEventDTO.ScoringEventPolicyId = scoringEventPolicyDTO.ScoringEventPolicyId;
                    }
                    log.Debug("ScoringEventDTO.IsChanged Value :" + scoringEventDTO.IsChanged);
                    if (scoringEventDTO.IsChanged)
                    {
                        updatedScoringEventDTO.Add(scoringEventDTO);
                    }
                }
                log.Debug("updatedScoringEventDTO Value :" + updatedScoringEventDTO);
                if (updatedScoringEventDTO.Any())
                {
                    ScoringEventListBL scoringEventListBL = new ScoringEventListBL(executionContext, updatedScoringEventDTO);
                    scoringEventListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        private void Update(ScoringEventPolicyDTO parameterScoringEventPolicyDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterScoringEventPolicyDTO, sqlTransaction);
            ChangeScoringPolicyName(parameterScoringEventPolicyDTO.ScoringPolicyName);
            ChangeStartDate(parameterScoringEventPolicyDTO.StartDate);
            ChangeEndDate(parameterScoringEventPolicyDTO.EndDate);
            ChangeAchievementClassId(parameterScoringEventPolicyDTO.AchievementClassId);
            ChangeIsActive(parameterScoringEventPolicyDTO.ActiveFlag);
            Dictionary<int, ScoringEventDTO> scoringEventDTODictionary = new Dictionary<int, ScoringEventDTO>();
            if (scoringEventPolicyDTO.ScoringEventDTOList != null &&
                scoringEventPolicyDTO.ScoringEventDTOList.Any())
            {
                foreach (var scoringEventDTO in scoringEventPolicyDTO.ScoringEventDTOList)
                {
                    scoringEventDTODictionary.Add(scoringEventDTO.ScoringEventId, scoringEventDTO);
                }
            }
            if (parameterScoringEventPolicyDTO.ScoringEventDTOList != null &&
                parameterScoringEventPolicyDTO.ScoringEventDTOList.Any())
            {
                foreach (var parameterScoringEventDTO in parameterScoringEventPolicyDTO.ScoringEventDTOList)
                {
                    if (scoringEventDTODictionary.ContainsKey(parameterScoringEventDTO.ScoringEventId))
                    {
                        ScoringEventBL scoringEventBL = new ScoringEventBL(executionContext, scoringEventDTODictionary[parameterScoringEventDTO.ScoringEventId]);
                        scoringEventBL.Update(parameterScoringEventDTO, sqlTransaction);
                    }
                    else if (parameterScoringEventDTO.ScoringEventId > -1)
                    {
                        ScoringEventBL scoringEventBL = new ScoringEventBL(executionContext, parameterScoringEventDTO.ScoringEventId, true, true, sqlTransaction);
                        if (scoringEventPolicyDTO.ScoringEventDTOList == null)
                        {
                            scoringEventPolicyDTO.ScoringEventDTOList = new List<ScoringEventDTO>();
                        }
                        scoringEventPolicyDTO.ScoringEventDTOList.Add(scoringEventBL.ScoringEventDTO);
                        scoringEventBL.Update(parameterScoringEventDTO);
                    }
                    else
                    {
                        ScoringEventBL scoringEventBL = new ScoringEventBL(executionContext, parameterScoringEventDTO);
                        if (scoringEventPolicyDTO.ScoringEventDTOList == null)
                        {
                            scoringEventPolicyDTO.ScoringEventDTOList = new List<ScoringEventDTO>();
                        }
                        scoringEventPolicyDTO.ScoringEventDTOList.Add(scoringEventBL.ScoringEventDTO);
                    }
                }
            }

            log.LogMethodExit();
        }

        public void ChangeEndDate(DateTime? endDate)
        {
            log.LogMethodEntry(endDate);
            if (scoringEventPolicyDTO.EndDate == endDate)
            {
                log.LogMethodExit(null, "No changes to scoringEventPolicy EndDate");
                return;
            }
            scoringEventPolicyDTO.EndDate = endDate;
            log.LogMethodExit();
        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (scoringEventPolicyDTO.ActiveFlag == isActive)
            {
                log.LogMethodExit(null, "No changes to scoringEventPolicy isActive");
                return;
            }
            scoringEventPolicyDTO.ActiveFlag = isActive;
            log.LogMethodExit();
        }

        private void ChangeAchievementClassId(int achievementClassId)
        {
            log.LogMethodEntry(achievementClassId);

            if (scoringEventPolicyDTO.AchievementClassId == achievementClassId)
            {
                log.LogMethodExit(null, "No changes to scoringEventPolicy AchievementClassId");
                return;
            }
            //ValidateAchievementClassId(achievementClassId);
            scoringEventPolicyDTO.AchievementClassId = achievementClassId;
            log.LogMethodExit();
        }



        public void ChangeStartDate(DateTime? startDate)
        {
            log.LogMethodEntry(startDate);
            if (scoringEventPolicyDTO.StartDate == startDate)
            {
                log.LogMethodExit(null, "No changes to scoringEventPolicy role");
                return;
            }
            //ValidateStartDate(startDate);
            scoringEventPolicyDTO.StartDate = startDate;
            log.LogMethodExit();
        }


        public void ChangeScoringPolicyName(string scoringPolicyName)
        {
            log.LogMethodEntry(scoringPolicyName);
            if (scoringEventPolicyDTO.ScoringPolicyName == scoringPolicyName)
            {
                log.LogMethodExit(null, "No changes to ScoringPolicyName");
                return;
            }
            //ValidateScoringPolicyName(scoringPolicyName);
            scoringEventPolicyDTO.ScoringPolicyName = scoringPolicyName;
            log.LogMethodExit();
        }
    }
    /// <summary>
    /// Manages the list of ScoringEventPolicy
    /// </summary>
    public class ScoringEventPolicyListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ScoringEventPolicyDTO> scoringEventPolicyList;

        /// <summary>
        /// default constructor
        /// </summary>
        public ScoringEventPolicyListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public ScoringEventPolicyListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.scoringEventPolicyList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="scoringEventPolicyList"></param>
        public ScoringEventPolicyListBL(ExecutionContext executionContext, List<ScoringEventPolicyDTO> scoringEventPolicyList)
        {
            log.LogMethodEntry(executionContext, scoringEventPolicyList);
            this.scoringEventPolicyList = scoringEventPolicyList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the scoringEventPolicy list
        /// </summary>
        public List<ScoringEventPolicyDTO> GetAllScoringEventPolicy(List<KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string>> searchParameters,
                                         bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ScoringEventPolicyDataHandler scoringEventPolicyDataHandler = new ScoringEventPolicyDataHandler(sqlTransaction);
            List<ScoringEventPolicyDTO> scoringEventPolicyDTOList = scoringEventPolicyDataHandler.GetScoringEventPolicyList(searchParameters, sqlTransaction);
            if (loadChildRecords)
            {
                Build(scoringEventPolicyDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(scoringEventPolicyDTOList);
            return scoringEventPolicyDTOList;
        }

        private void Build(List<ScoringEventPolicyDTO> scoringEventPolicyDTOList, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            Dictionary<int, ScoringEventPolicyDTO> scoringEventPolicyIdDTODictionary = new Dictionary<int, ScoringEventPolicyDTO>();
            List<int> scoringEventPolicyIdList = new List<int>();
            if (scoringEventPolicyDTOList != null && scoringEventPolicyDTOList.Any())
            {
                for (int i = 0; i < scoringEventPolicyDTOList.Count; i++)
                {
                    if (scoringEventPolicyDTOList[i].ScoringEventPolicyId == -1 ||
                        scoringEventPolicyIdDTODictionary.ContainsKey(scoringEventPolicyDTOList[i].ScoringEventPolicyId))
                    {
                        continue;
                    }

                    scoringEventPolicyIdList.Add(scoringEventPolicyDTOList[i].ScoringEventPolicyId);
                    scoringEventPolicyIdDTODictionary.Add(scoringEventPolicyDTOList[i].ScoringEventPolicyId, scoringEventPolicyDTOList[i]);
                }
                ScoringEventListBL scoringEventListBL = new ScoringEventListBL();
                List<ScoringEventDTO> scoringEventDTOList = scoringEventListBL.GetScoringEventDTOListOfPolicy(scoringEventPolicyIdList, activeChildRecords, sqlTransaction);
                if (scoringEventDTOList != null && scoringEventDTOList.Any())
                {
                    log.LogVariableState("scoringEventDTOList", scoringEventDTOList);
                    foreach (ScoringEventDTO scoringEventDTO in scoringEventDTOList)
                    {
                        if (scoringEventPolicyIdDTODictionary.ContainsKey(scoringEventDTO.ScoringEventPolicyId))
                        {
                            if (scoringEventPolicyIdDTODictionary[scoringEventDTO.ScoringEventPolicyId].ScoringEventDTOList == null)
                            {
                                scoringEventPolicyIdDTODictionary[scoringEventDTO.ScoringEventPolicyId].ScoringEventDTOList = new List<ScoringEventDTO>();
                            }
                            scoringEventPolicyIdDTODictionary[scoringEventDTO.ScoringEventPolicyId].ScoringEventDTOList.Add(scoringEventDTO);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method should be used to Save and Update the ScoringEventPolicy details for Web Management Studio.
        /// </summary>
        public List<ScoringEventPolicyDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ScoringEventPolicyDTO> savedScoringEventPolicyDTOList = new List<ScoringEventPolicyDTO>();
            try
            {
                if (scoringEventPolicyList != null && scoringEventPolicyList.Any())
                {
                    foreach (ScoringEventPolicyDTO scoringEventPolicyDTO in scoringEventPolicyList)
                    {
                        ScoringEventPolicyBL scoringEventPolicyBL = new ScoringEventPolicyBL(executionContext, scoringEventPolicyDTO, sqlTransaction);
                        scoringEventPolicyBL.Save(sqlTransaction);
                        savedScoringEventPolicyDTOList.Add(scoringEventPolicyBL.ScoringEventPolicyDTO);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                sqlTransaction.Rollback();
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                if (sqlEx.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                }
                else
                {
                    throw;
                }
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                sqlTransaction.Rollback();
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sqlTransaction.Rollback();
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }

            log.LogMethodExit(savedScoringEventPolicyDTOList);
            return savedScoringEventPolicyDTOList;
        }
    }
}
