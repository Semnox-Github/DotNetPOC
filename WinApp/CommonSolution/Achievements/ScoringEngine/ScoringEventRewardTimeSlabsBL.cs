/********************************************************************************************
 * Project Name - ScoringEngine
 * Description  - Bussiness logic of ScoringEventRewardTimeSlabs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.120.0     02-02-2021   Prajwal S      Created 
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
    /// Represents a ScoringEventRewardTimeSlabs.
    /// </summary>
    internal class ScoringEventRewardTimeSlabsBL
    {
        private ScoringEventRewardTimeSlabsDTO scoringEventRewardTimeSlabsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        private ScoringEventRewardTimeSlabsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="scoringEventRewardTimeSlabsDTO">ScoringEventRewardTimeSlabsDTO</param>
        internal ScoringEventRewardTimeSlabsBL(ExecutionContext executionContext, ScoringEventRewardTimeSlabsDTO scoringEventRewardTimeSlabsDTO)
             : this(executionContext)
        {
            log.LogMethodEntry(executionContext, scoringEventRewardTimeSlabsDTO);
            if (scoringEventRewardTimeSlabsDTO.Id < 0)
            {
                //validate();
            }
            this.scoringEventRewardTimeSlabsDTO = scoringEventRewardTimeSlabsDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ScoringEventRewardTimeSlabsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ScoringEventRewardTimeSlabsDataHandler scoringEventRewardTimeSlabsDataHandler = new ScoringEventRewardTimeSlabsDataHandler(sqlTransaction);
            scoringEventRewardTimeSlabsDTO = scoringEventRewardTimeSlabsDataHandler.GetScoringEventRewardTimeSlabs(id);
            if (scoringEventRewardTimeSlabsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ScoringEventRewardTimeSlabs", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get ScoringEventRewardTimeSlabsDTO Object
        /// </summary>
        internal ScoringEventRewardTimeSlabsDTO ScoringEventRewardTimeSlabsDTO
        {
            get { return scoringEventRewardTimeSlabsDTO; }
        }


        /// <summary>
        /// Saves the ScoringEventRewardTimeSlabs
        /// Checks if the id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ScoringEventRewardTimeSlabsDataHandler scoringEventRewardTimeSlabsDataHandler = new ScoringEventRewardTimeSlabsDataHandler(sqlTransaction);

            if (scoringEventRewardTimeSlabsDTO.Id < 0)
            {
                scoringEventRewardTimeSlabsDTO = scoringEventRewardTimeSlabsDataHandler.Insert(scoringEventRewardTimeSlabsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                scoringEventRewardTimeSlabsDTO.AcceptChanges();
            }
            else
            {
                if (scoringEventRewardTimeSlabsDTO.IsChanged)
                {
                    scoringEventRewardTimeSlabsDTO = scoringEventRewardTimeSlabsDataHandler.Update(scoringEventRewardTimeSlabsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    scoringEventRewardTimeSlabsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        internal void Update(ScoringEventRewardTimeSlabsDTO parameterScoringEventRewardTimeSlabsDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterScoringEventRewardTimeSlabsDTO);
            ChangeScoringRewardId(parameterScoringEventRewardTimeSlabsDTO.ScoringRewardId);
            ChangeActiveFlag(parameterScoringEventRewardTimeSlabsDTO.ActiveFlag);
            ChangeFinishTimeSlabDays(parameterScoringEventRewardTimeSlabsDTO.FinishTimeSlabDays);
            ChangeFinishTimeSlabMinutes(parameterScoringEventRewardTimeSlabsDTO.FinishTimeSlabMinutes);
            ChangeIsTimeSlabScoreAbsoluteOrIncremental(parameterScoringEventRewardTimeSlabsDTO.IsTimeSlabScoreAbsoluteOrIncremental);
            ChangeTimeSlabScore(parameterScoringEventRewardTimeSlabsDTO.TimeSlabScore);
            log.LogMethodExit();
        }

        private void ChangeTimeSlabScore(double? timeSlabScore)
        {
            log.LogMethodEntry(timeSlabScore);
            if (scoringEventRewardTimeSlabsDTO.TimeSlabScore == timeSlabScore)
            {
                log.LogMethodExit(null, "No changes to Score");
                return;
            }
            scoringEventRewardTimeSlabsDTO.TimeSlabScore = timeSlabScore;
            log.LogMethodExit();
        }

        private void ChangeIsTimeSlabScoreAbsoluteOrIncremental(bool isTimeSlabScoreAbsoluteOrIncremental)
        {
            log.LogMethodEntry(isTimeSlabScoreAbsoluteOrIncremental);
            if (scoringEventRewardTimeSlabsDTO.IsTimeSlabScoreAbsoluteOrIncremental == isTimeSlabScoreAbsoluteOrIncremental)
            {
                log.LogMethodExit(null, "No changes to IsTimeSlabScoreAbsoluteOrIncremental");
                return;
            }
            scoringEventRewardTimeSlabsDTO.IsTimeSlabScoreAbsoluteOrIncremental = isTimeSlabScoreAbsoluteOrIncremental;
            log.LogMethodExit();
        }

        private void ChangeFinishTimeSlabDays(int? finishTimeSlabDays)
        {
            log.LogMethodEntry(finishTimeSlabDays);
            if (scoringEventRewardTimeSlabsDTO.FinishTimeSlabDays == finishTimeSlabDays)
            {
                log.LogMethodExit(null, "No changes to FinishTimeSlabDays");
                return;
            }
            scoringEventRewardTimeSlabsDTO.FinishTimeSlabDays = finishTimeSlabDays;
            log.LogMethodExit();
        }

        private void ChangeFinishTimeSlabMinutes(int? finishTimeSlabMinutes)
        {
            log.LogMethodEntry(finishTimeSlabMinutes);
            if (scoringEventRewardTimeSlabsDTO.FinishTimeSlabMinutes == finishTimeSlabMinutes)
            {
                log.LogMethodExit(null, "No changes to FinishTimeSlabMinutes");
                return;
            }
            scoringEventRewardTimeSlabsDTO.FinishTimeSlabMinutes = finishTimeSlabMinutes;
            log.LogMethodExit();
        }

        private void ChangeActiveFlag(bool activeFlag)
        {
            log.LogMethodEntry(activeFlag);
            if (scoringEventRewardTimeSlabsDTO.ActiveFlag == activeFlag)
            {
                log.LogMethodExit(null, "No changes to ScoringEventRewardTimeSlabs activeFlag");
                return;
            }
            scoringEventRewardTimeSlabsDTO.ActiveFlag = activeFlag;
            log.LogMethodExit();
        }

        private void ChangeScoringRewardId(int scoringRewardId)
        {
            log.LogMethodEntry(scoringRewardId);
            // ValidateScoringEventId(scoringEventId);
            if (scoringEventRewardTimeSlabsDTO.ScoringRewardId == scoringRewardId)
            {
                log.LogMethodExit(null, "No changes to ScoringRewardId");
                return;
            }
            scoringEventRewardTimeSlabsDTO.ScoringRewardId = scoringRewardId;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of ScoringEventRewardTimeSlabs
    /// </summary>
    public class ScoringEventRewardTimeSlabsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ScoringEventRewardTimeSlabsDTO> scoringEventRewardTimeSlabsList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScoringEventRewardTimeSlabsListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="ScoringEventRewardTimeSlabsList"></param>
        public ScoringEventRewardTimeSlabsListBL(ExecutionContext executionContext, List<ScoringEventRewardTimeSlabsDTO> ScoringEventRewardTimeSlabsList)
             : this()
        {
            log.LogMethodEntry(executionContext, ScoringEventRewardTimeSlabsList);
            this.executionContext = executionContext;
            this.scoringEventRewardTimeSlabsList = ScoringEventRewardTimeSlabsList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ScoringEventRewardTimeSlabsDTO List for scoringEventId Id List
        /// </summary>
        /// <param name="scoringEventIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<ScoringEventRewardTimeSlabsDTO> GetScoringEventRewardTimeSlabsListOfRewards(List<int> scoringEventIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scoringEventIdList, activeRecords, sqlTransaction);
            ScoringEventRewardTimeSlabsDataHandler scoringEventRewardTimeSlabsDataHandler = new ScoringEventRewardTimeSlabsDataHandler(sqlTransaction);
            List<ScoringEventRewardTimeSlabsDTO> scoringEventRewardTimeSlabsDTOList = scoringEventRewardTimeSlabsDataHandler.GetScoringEventRewardTimeSlabsDTOList(scoringEventIdList, activeRecords);
            log.LogMethodExit(scoringEventRewardTimeSlabsDTOList);
            return scoringEventRewardTimeSlabsDTOList;
        }

        /// <summary>
        /// Returns the ScoringEventRewardTimeSlabs list
        /// </summary>
        public List<ScoringEventRewardTimeSlabsDTO> GetScoringEventRewardTimeSlabsDTOList(List<KeyValuePair<ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ScoringEventRewardTimeSlabsDataHandler scoringEventRewardTimeSlabsDataHandler = new ScoringEventRewardTimeSlabsDataHandler(sqlTransaction);
            List<ScoringEventRewardTimeSlabsDTO> scoringEventRewardTimeSlabsDTOList = scoringEventRewardTimeSlabsDataHandler.GetScoringEventRewardTimeSlabsList(searchParameters);
            log.LogMethodExit(scoringEventRewardTimeSlabsDTOList);
            return scoringEventRewardTimeSlabsDTOList;
        }

        /// <summary>
        /// This method should be used to Save ScoringEventRewardTimeSlabs
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (scoringEventRewardTimeSlabsList != null && scoringEventRewardTimeSlabsList.Count != 0)
                {
                    foreach (ScoringEventRewardTimeSlabsDTO scoringEventRewardTimeSlabsDTO in scoringEventRewardTimeSlabsList)
                    {
                        ScoringEventRewardTimeSlabsBL scoringEventRewardTimeSlabsBL = new ScoringEventRewardTimeSlabsBL(executionContext, scoringEventRewardTimeSlabsDTO);
                        scoringEventRewardTimeSlabsBL.Save(sqlTransaction);
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
