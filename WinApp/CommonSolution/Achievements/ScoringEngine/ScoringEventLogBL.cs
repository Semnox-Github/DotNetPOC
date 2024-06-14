/********************************************************************************************
 * Project Name - ScoringEngine
 * Description  - Bussiness logic of ScoringEventLog
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
    /// Represents a ScoringEventLog.
    /// </summary>
    public class ScoringEventLogBL
    {
        private ScoringEventLogDTO scoringEventLogDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        private ScoringEventLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="scoringEventLogDTO">ScoringEventLogDTO</param>
        public ScoringEventLogBL(ExecutionContext executionContext, ScoringEventLogDTO scoringEventLogDTO)
             : this(executionContext)
        {
            log.LogMethodEntry(executionContext, scoringEventLogDTO);
            if (scoringEventLogDTO.ScoringEventLogId < 0)
            {
                //validate();
            }
            this.scoringEventLogDTO = scoringEventLogDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ScoringEventLogBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ScoringEventLogDataHandler scoringEventLogDataHandler = new ScoringEventLogDataHandler(sqlTransaction);
            scoringEventLogDTO = scoringEventLogDataHandler.GetScoringEventLog(id);
            if (scoringEventLogDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ScoringEventLog", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get ScoringEventLogDTO Object
        /// </summary>
        public ScoringEventLogDTO ScoringEventLogDTO
        {
            get { return scoringEventLogDTO; }
        }


        /// <summary>
        /// Saves the ScoringEventLogs
        /// Checks if the id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ScoringEventLogDataHandler scoringEventLogDataHandler = new ScoringEventLogDataHandler(sqlTransaction);

            if (scoringEventLogDTO.ScoringEventLogId < 0)
            {
                scoringEventLogDTO = scoringEventLogDataHandler.Insert(scoringEventLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                scoringEventLogDTO.AcceptChanges();
            }
            else
            {
                if (scoringEventLogDTO.IsChanged)
                {
                    scoringEventLogDTO = scoringEventLogDataHandler.Update(scoringEventLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    scoringEventLogDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        internal void Update(ScoringEventLogDTO parameterScoringEventLogDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterScoringEventLogDTO);
            ChangeScoringEventId(parameterScoringEventLogDTO.ScoringEventId);
            ChangeActiveFlag(parameterScoringEventLogDTO.ActiveFlag);
            ChangeBreachCount(parameterScoringEventLogDTO.BreachCount);
            ChangeCardId(parameterScoringEventLogDTO.CardId);
            ChangeEventState(parameterScoringEventLogDTO.EventState);
            ChangeEventDate(parameterScoringEventLogDTO.EventDate);
            ChangeIsFinal(parameterScoringEventLogDTO.IsFinal);
            ChangeIsPatternbreach(parameterScoringEventLogDTO.IsPatternbreach);
            ChangeIsTimeout(parameterScoringEventLogDTO.IsTimeout);
            ChangeScore(parameterScoringEventLogDTO.Score);
            log.LogMethodExit();
        }

        private void ChangeIsPatternbreach(bool isPatternbreach)
        {
            log.LogMethodEntry(isPatternbreach);
            if (scoringEventLogDTO.IsPatternbreach == isPatternbreach)
            {
                log.LogMethodExit(null, "No changes to IsPatternbreach");
                return;
            }
            scoringEventLogDTO.IsPatternbreach = isPatternbreach;
            log.LogMethodExit();
        }

        private void ChangeScore(double? score)
        {
            log.LogMethodEntry(score);
            if (scoringEventLogDTO.Score == score)
            {
                log.LogMethodExit(null, "No changes to Score");
                return;
            }
            scoringEventLogDTO.Score = score;
            log.LogMethodExit();
        }

        private void ChangeIsTimeout(bool isTimeout)
        {
            log.LogMethodEntry(isTimeout);
            if (scoringEventLogDTO.IsTimeout == isTimeout)
            {
                log.LogMethodExit(null, "No changes to ReaderThemeId");
                return;
            }
            scoringEventLogDTO.IsTimeout = isTimeout;
            log.LogMethodExit();
        }
        private void ChangeIsFinal(bool isFinal)
        {
            log.LogMethodEntry(isFinal);
            if (scoringEventLogDTO.IsFinal == isFinal)
            {
                log.LogMethodExit(null, "No changes to IsFinal");
                return;
            }
            scoringEventLogDTO.IsFinal = isFinal;
            log.LogMethodExit();
        }
        private void ChangeEventDate(DateTime? eventDate)
        {
            log.LogMethodEntry(eventDate);
            if (scoringEventLogDTO.EventDate == eventDate)
            {
                log.LogMethodExit(null, "No changes to EventDate");
                return;
            }
            scoringEventLogDTO.EventDate = eventDate;
            log.LogMethodExit();
        }

        private void ChangeEventState(int? eventState)
        {
            log.LogMethodEntry(eventState);
            if (scoringEventLogDTO.EventState == eventState)
            {
                log.LogMethodExit(null, "No changes to EventState");
                return;
            }
            scoringEventLogDTO.EventState = eventState;
            log.LogMethodExit();
        }

        private void ChangeBreachCount(int? breachCount)
        {
            log.LogMethodEntry(breachCount);
            if (scoringEventLogDTO.BreachCount == breachCount)
            {
                log.LogMethodExit(null, "No changes to BreachCount");
                return;
            }
            scoringEventLogDTO.BreachCount = breachCount;
            log.LogMethodExit();
        }

        private void ChangeCardId(int? cardId)
        {
            log.LogMethodEntry(cardId);
            if (scoringEventLogDTO.CardId == cardId)
            {
                log.LogMethodExit(null, "No changes to CardId");
                return;
            }
            scoringEventLogDTO.CardId = cardId;
            log.LogMethodExit();
        }

        private void ChangeActiveFlag(bool activeFlag)
        {
            log.LogMethodEntry(activeFlag);
            if (scoringEventLogDTO.ActiveFlag == activeFlag)
            {
                log.LogMethodExit(null, "No changes to ScoringEventLog activeFlag");
                return;
            }
            scoringEventLogDTO.ActiveFlag = activeFlag;
            log.LogMethodExit();
        }

        private void ChangeScoringEventId(int scoringEventId)
        {
            log.LogMethodEntry(scoringEventId);
            // ValidateScoringEventId(scoringEventId);
            if (scoringEventLogDTO.ScoringEventId == scoringEventId)
            {
                log.LogMethodExit(null, "No changes to ScoringEventId");
                return;
            }
            scoringEventLogDTO.ScoringEventId = scoringEventId;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of ScoringEventLog
    /// </summary>
    public class ScoringEventLogListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ScoringEventLogDTO> scoringEventLogList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScoringEventLogListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="ScoringEventLogList"></param>
        public ScoringEventLogListBL(ExecutionContext executionContext, List<ScoringEventLogDTO> ScoringEventLogList)
             : this(executionContext)
        {
            log.LogMethodEntry(executionContext, ScoringEventLogList);
            this.executionContext = executionContext;
            this.scoringEventLogList = ScoringEventLogList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ScoringEventLogDTO List for scoringEventId Id List
        /// </summary>
        /// <param name="scoringEventIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<ScoringEventLogDTO> GetScoringEventLogDTOListOfEvents(List<int> scoringEventIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scoringEventIdList, activeRecords, sqlTransaction);
            ScoringEventLogDataHandler scoringEventLogDataHandler = new ScoringEventLogDataHandler(sqlTransaction);
            List<ScoringEventLogDTO> scoringEventLogDTOList = scoringEventLogDataHandler.GetScoringEventLogDTOList(scoringEventIdList, activeRecords);
            log.LogMethodExit(scoringEventLogDTOList);
            return scoringEventLogDTOList;
        }

        /// <summary>
        /// Returns the ScoringEventLog list
        /// </summary>
        public List<ScoringEventLogDTO> GetScoringEventLogDTOList(List<KeyValuePair<ScoringEventLogDTO.SearchByScoringEventLogParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, searchParameters, sqlTransaction);
            ScoringEventLogDataHandler scoringEventLogDataHandler = new ScoringEventLogDataHandler(sqlTransaction);
            List<ScoringEventLogDTO> scoringEventLogDTOList = scoringEventLogDataHandler.GetScoringEventLogList(searchParameters);
            log.LogMethodExit(scoringEventLogDTOList);
            return scoringEventLogDTOList;
        }

        public DateTime MaxScoringEventDate(int scoringEventId, int cardId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scoringEventId, cardId, sqlTransaction);
            ScoringEventLogDataHandler scoringEventLogDataHandler = new ScoringEventLogDataHandler(sqlTransaction);
            object maxEventLogDate = scoringEventLogDataHandler.GetMaxScoringEventDate(scoringEventId, cardId);
            DateTime maxScoringEventDate;
            if (maxEventLogDate != null && maxEventLogDate != DBNull.Value)
            {
                maxScoringEventDate = Convert.ToDateTime(maxEventLogDate);
            }
            else
            {
                maxScoringEventDate = DateTime.MinValue;
            }
            log.LogMethodExit(maxScoringEventDate);
            return maxScoringEventDate;
        }

        /// <summary>
        /// This method should be used to Save ScoringEventLog
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (scoringEventLogList != null && scoringEventLogList.Count != 0)
                {
                    foreach (ScoringEventLogDTO scoringEventLogDTO in scoringEventLogList)
                    {
                        ScoringEventLogBL scoringEventLogBL = new ScoringEventLogBL(executionContext, scoringEventLogDTO);
                        scoringEventLogBL.Save(sqlTransaction);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 547)
                {
                    throw new ValidationException(sqlEx.Message);
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
