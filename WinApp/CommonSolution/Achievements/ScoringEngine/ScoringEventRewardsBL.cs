/********************************************************************************************
 * Project Name - ScoringEngine
 * Description  - Bussiness logic of ScoringEventRewards
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
    /// Represents a ScoringEventRewards.
    /// </summary>
    internal class ScoringEventRewardsBL
    {
        private ScoringEventRewardsDTO scoringEventRewardsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        private ScoringEventRewardsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="scoringEventRewardsDTO">ScoringEventRewardsDTO</param>
        internal ScoringEventRewardsBL(ExecutionContext executionContext, ScoringEventRewardsDTO scoringEventRewardsDTO)
             : this(executionContext)
        {
            log.LogMethodEntry(executionContext, scoringEventRewardsDTO);
            if (scoringEventRewardsDTO.ScoringRewardId < 0)
            {
                //validate();
            }
            this.scoringEventRewardsDTO = scoringEventRewardsDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ScoringEventRewardsBL(ExecutionContext executionContext, int id, bool loadChildrecords, bool activeChildRecords, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ScoringEventRewardsDataHandler scoringEventRewardsDataHandler = new ScoringEventRewardsDataHandler(sqlTransaction);
            scoringEventRewardsDTO = scoringEventRewardsDataHandler.GetScoringEventRewards(id);
            if (scoringEventRewardsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ScoringEventRewards", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if(loadChildrecords)
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
            ScoringEventRewardTimeSlabsListBL scoringEventRewardTimeSlabsList = new ScoringEventRewardTimeSlabsListBL();
            List<KeyValuePair<ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters, string>> searchByParams = new List<KeyValuePair<ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters, string>>();
            searchByParams.Add(new KeyValuePair<ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters, string>(ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.SCORING_REWARD_ID, scoringEventRewardsDTO.ScoringRewardId.ToString()));
            if (activeChildRecords)
            {
                searchByParams.Add(new KeyValuePair<ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters, string>(ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.IS_ACTIVE, "1"));
            }
            scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList = scoringEventRewardTimeSlabsList.GetScoringEventRewardTimeSlabsDTOList(searchByParams, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// get ScoringEventRewardsDTO Object
        /// </summary>
        internal ScoringEventRewardsDTO ScoringEventRewardsDTO
        {
            get { return scoringEventRewardsDTO; }
        }


        /// <summary>
        /// Saves the ScoringEventRewardss
        /// Checks if the id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ScoringEventRewardsDataHandler scoringEventRewardsDataHandler = new ScoringEventRewardsDataHandler(sqlTransaction);

            if (scoringEventRewardsDTO.ScoringRewardId < 0)
            {
                scoringEventRewardsDTO = scoringEventRewardsDataHandler.Insert(scoringEventRewardsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                scoringEventRewardsDTO.AcceptChanges();
            }
            else
            {
                if (scoringEventRewardsDTO.IsChanged)
                {
                    scoringEventRewardsDTO = scoringEventRewardsDataHandler.Update(scoringEventRewardsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    scoringEventRewardsDTO.AcceptChanges();
                }
            }

            // Will Save the Child ScoringEventDetailsDTO
            log.Debug(" scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTO Value :" +  scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList);
            if ( scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList != null &&  scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList.Any())
            {
                List<ScoringEventRewardTimeSlabsDTO> updatedScoringEventRewardTimeSlabsDTO = new List<ScoringEventRewardTimeSlabsDTO>();
                foreach (ScoringEventRewardTimeSlabsDTO scoringEventRewardTimeSlabsDTO in  scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList)
                {
                    if (scoringEventRewardTimeSlabsDTO.ScoringRewardId !=  scoringEventRewardsDTO.ScoringRewardId)
                    {
                        scoringEventRewardTimeSlabsDTO.ScoringRewardId =  scoringEventRewardsDTO.ScoringRewardId;
                    }
                    log.Debug("ScoringEventRewardTimeSlabsDTO.IsChanged Value :" + scoringEventRewardTimeSlabsDTO.IsChanged);
                    if (scoringEventRewardTimeSlabsDTO.IsChanged)
                    {
                        updatedScoringEventRewardTimeSlabsDTO.Add(scoringEventRewardTimeSlabsDTO);
                    }
                }
                log.Debug("updatedScoringEventRewardTimeSlabsDTO Value :" + updatedScoringEventRewardTimeSlabsDTO);
                if (updatedScoringEventRewardTimeSlabsDTO.Any())
                {
                    ScoringEventRewardTimeSlabsListBL scoringEventRewardTimeSlabsListBL = new ScoringEventRewardTimeSlabsListBL(executionContext, updatedScoringEventRewardTimeSlabsDTO);
                    scoringEventRewardTimeSlabsListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        internal void Update(ScoringEventRewardsDTO parameterScoringEventRewardsDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterScoringEventRewardsDTO);
            ChangeScoringEventId(parameterScoringEventRewardsDTO.ScoringEventId);
            ChangeActiveFlag(parameterScoringEventRewardsDTO.ActiveFlag);
            ChangePatternBreachPenalty(parameterScoringEventRewardsDTO.PatternBreachPenalty);
            ChangeRewardName(parameterScoringEventRewardsDTO.RewardName);
            ChangeAllowProgressiveScoring(parameterScoringEventRewardsDTO.AllowProgressiveScoring);
            ChangeIsCumulativeScore(parameterScoringEventRewardsDTO.IsCumulativeScore);
            ChangeAbsoluteScore(parameterScoringEventRewardsDTO.AbsoluteScore);

              Dictionary<int, ScoringEventRewardTimeSlabsDTO> scoringEventRewardTimeSlabsDTODictionary = new Dictionary<int, ScoringEventRewardTimeSlabsDTO>();
            if (scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList != null &&
                scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList.Any())
            {
                foreach (var scoringEventRewardTimeSlabsDTO in scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList)
                {
                    scoringEventRewardTimeSlabsDTODictionary.Add(scoringEventRewardTimeSlabsDTO.Id, scoringEventRewardTimeSlabsDTO);
                }
            }
            if (parameterScoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList != null &&
                parameterScoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList.Any())
            {
                foreach (var parameterScoringEventRewardTimeSlabsDTO in parameterScoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList)
                {
                    if (scoringEventRewardTimeSlabsDTODictionary.ContainsKey(parameterScoringEventRewardTimeSlabsDTO.Id))
                    {
                        ScoringEventRewardTimeSlabsBL scoringEventRewardTimeSlabs = new ScoringEventRewardTimeSlabsBL(executionContext, scoringEventRewardTimeSlabsDTODictionary[parameterScoringEventRewardTimeSlabsDTO.Id]);
                        scoringEventRewardTimeSlabs.Update(parameterScoringEventRewardTimeSlabsDTO, sqlTransaction);
                    }
                    else if (parameterScoringEventRewardTimeSlabsDTO.Id > -1)
                    {
                        ScoringEventRewardTimeSlabsBL scoringEventRewardTimeSlabs = new ScoringEventRewardTimeSlabsBL(executionContext, parameterScoringEventRewardTimeSlabsDTO.Id, sqlTransaction);
                        if (scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList == null)
                        {
                            scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList = new List<ScoringEventRewardTimeSlabsDTO>();
                        }
                        scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList.Add(scoringEventRewardTimeSlabs.ScoringEventRewardTimeSlabsDTO);
                        scoringEventRewardTimeSlabs.Update(parameterScoringEventRewardTimeSlabsDTO, sqlTransaction);
                    }
                    else
                    {
                        ScoringEventRewardTimeSlabsBL scoringEventRewardTimeSlabs = new ScoringEventRewardTimeSlabsBL(executionContext, parameterScoringEventRewardTimeSlabsDTO);
                        if (scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList == null)
                        {
                            scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList = new List<ScoringEventRewardTimeSlabsDTO>();
                        }
                        scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList.Add(scoringEventRewardTimeSlabs.ScoringEventRewardTimeSlabsDTO);
                    }
                }
            }

            log.LogMethodExit();
        }


        private void ChangeAbsoluteScore(double? absoluteScore)
        {
            log.LogMethodEntry(absoluteScore);
            if (scoringEventRewardsDTO.AbsoluteScore == absoluteScore)
            {
                log.LogMethodExit(null, "No changes to AbsoluteScore");
                return;
            }
            scoringEventRewardsDTO.AbsoluteScore = absoluteScore;
            log.LogMethodExit();
        }

        private void ChangeIsCumulativeScore(bool isCumulativeScore)
        {
            log.LogMethodEntry(isCumulativeScore);
            if (scoringEventRewardsDTO.IsCumulativeScore == isCumulativeScore)
            {
                log.LogMethodExit(null, "No changes to IsCumulativeScore");
                return;
            }
            scoringEventRewardsDTO.IsCumulativeScore = isCumulativeScore;
            log.LogMethodExit();
        }

        private void ChangeAllowProgressiveScoring(bool allowProgressiveScoring)
        {
            log.LogMethodEntry(allowProgressiveScoring);
            if (scoringEventRewardsDTO.AllowProgressiveScoring == allowProgressiveScoring)
            {
                log.LogMethodExit(null, "No changes to AllowProgressiveScoring");
                return;
            }
            scoringEventRewardsDTO.AllowProgressiveScoring = allowProgressiveScoring;
            log.LogMethodExit();
        }

        private void ChangePatternBreachPenalty(int? patternBreachPenalty)
        {
            log.LogMethodEntry(patternBreachPenalty);
            if (scoringEventRewardsDTO.PatternBreachPenalty == patternBreachPenalty)
            {
                log.LogMethodExit(null, "No changes to PatternBreachPenalty");
                return;
            }
            scoringEventRewardsDTO.PatternBreachPenalty = patternBreachPenalty;
            log.LogMethodExit();
        }

        private void ChangeRewardName(string rewardName)
        {
            log.LogMethodEntry(rewardName);
            if (scoringEventRewardsDTO.RewardName == rewardName)
            {
                log.LogMethodExit(null, "No changes to RewardName");
                return;
            }
            scoringEventRewardsDTO.RewardName = rewardName;
            log.LogMethodExit();
        }

        private void ChangeActiveFlag(bool activeFlag)
        {
            log.LogMethodEntry(activeFlag);
            if (scoringEventRewardsDTO.ActiveFlag == activeFlag)
            {
                log.LogMethodExit(null, "No changes to ScoringEventRewards activeFlag");
                return;
            }
            scoringEventRewardsDTO.ActiveFlag = activeFlag;
            log.LogMethodExit();
        }

        private void ChangeScoringEventId(int scoringEventId)
        {
            log.LogMethodEntry(scoringEventId);
            // ValidateScoringEventId(scoringEventId);
            if (scoringEventRewardsDTO.ScoringEventId == scoringEventId)
            {
                log.LogMethodExit(null, "No changes to ScoringEventId");
                return;
            }
            scoringEventRewardsDTO.ScoringEventId = scoringEventId;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of ScoringEventRewards
    /// </summary>
    public class ScoringEventRewardsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ScoringEventRewardsDTO> scoringEventRewardsList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScoringEventRewardsListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="ScoringEventRewardsList"></param>
        public ScoringEventRewardsListBL(ExecutionContext executionContext, List<ScoringEventRewardsDTO> ScoringEventRewardsList)
             : this()
        {
            log.LogMethodEntry(executionContext, ScoringEventRewardsList);
            this.executionContext = executionContext;
            this.scoringEventRewardsList = ScoringEventRewardsList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ScoringEventRewardsDTO List for scoringEventId Id List
        /// </summary>
        /// <param name="scoringEventIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<ScoringEventRewardsDTO> GetScoringEventRewardsDTOListOfEvents(List<int> scoringEventIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scoringEventIdList, activeRecords, sqlTransaction);
            ScoringEventRewardsDataHandler scoringEventRewardsDataHandler = new ScoringEventRewardsDataHandler(sqlTransaction);
            List<ScoringEventRewardsDTO> scoringEventRewardsDTOList = scoringEventRewardsDataHandler.GetScoringEventRewardsDTOList(scoringEventIdList, activeRecords);

            Build(scoringEventRewardsDTOList, activeRecords, sqlTransaction);

            log.LogMethodExit(scoringEventRewardsDTOList);
            return scoringEventRewardsDTOList;
        }

        /// <summary>
        /// Returns the ScoringEventRewards list
        /// </summary>
        public List<ScoringEventRewardsDTO> GetScoringEventRewardsDTOList(List<KeyValuePair<ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters, string>> searchParameters, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ScoringEventRewardsDataHandler scoringEventRewardsDataHandler = new ScoringEventRewardsDataHandler(sqlTransaction);
            List<ScoringEventRewardsDTO> scoringEventRewardsDTOList = scoringEventRewardsDataHandler.GetScoringEventRewardsList(searchParameters);
            if(loadChildRecords)
            {
                Build(scoringEventRewardsDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(scoringEventRewardsDTOList);
            return scoringEventRewardsDTOList;
        }

        /// <summary>
        /// Builds the List of ScoringEventRewards object based on the list of ScoringEventRewards id.
        /// </summary>
        /// <param name="scoringEventRewardsDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<ScoringEventRewardsDTO> scoringEventRewardsDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scoringEventRewardsDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ScoringEventRewardsDTO> scoringEventRewardsDTOIdMap = new Dictionary<int, ScoringEventRewardsDTO>();
            List<int> scoringEventRewardsIdList = new List<int>();
            for (int i = 0; i < scoringEventRewardsDTOList.Count; i++)
            {
                if (scoringEventRewardsDTOIdMap.ContainsKey(scoringEventRewardsDTOList[i].ScoringRewardId))
                {
                    continue;
                }
                scoringEventRewardsDTOIdMap.Add(scoringEventRewardsDTOList[i].ScoringRewardId, scoringEventRewardsDTOList[i]);
                scoringEventRewardsIdList.Add(scoringEventRewardsDTOList[i].ScoringRewardId);
            }

            ScoringEventRewardTimeSlabsListBL scoringEventRewardTimeSlabsListBL = new ScoringEventRewardTimeSlabsListBL();
            List<ScoringEventRewardTimeSlabsDTO> scoringEventRewardTimeSlabsDTOList = scoringEventRewardTimeSlabsListBL.GetScoringEventRewardTimeSlabsListOfRewards(scoringEventRewardsIdList, activeChildRecords, sqlTransaction);
            if (scoringEventRewardTimeSlabsDTOList != null && scoringEventRewardTimeSlabsDTOList.Any())
            {
                for (int i = 0; i < scoringEventRewardTimeSlabsDTOList.Count; i++)
                {
                    if (scoringEventRewardsDTOIdMap.ContainsKey(scoringEventRewardTimeSlabsDTOList[i].ScoringRewardId) == false)
                    {
                        continue;
                    }
                    ScoringEventRewardsDTO scoringEventRewardsDTO = scoringEventRewardsDTOIdMap[scoringEventRewardTimeSlabsDTOList[i].ScoringRewardId];
                    if (scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList == null)
                    {
                        scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList = new List<ScoringEventRewardTimeSlabsDTO>();
                    }
                    scoringEventRewardsDTO.ScoringEventRewardTimeSlabsDTOList.Add(scoringEventRewardTimeSlabsDTOList[i]);
                }
            }
        }

            /// <summary>
            /// This method should be used to Save ScoringEventRewards
            /// </summary>
            internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (scoringEventRewardsList != null && scoringEventRewardsList.Count != 0)
                {
                    foreach (ScoringEventRewardsDTO scoringEventRewardsDTO in scoringEventRewardsList)
                    {
                        ScoringEventRewardsBL scoringEventRewardsBL = new ScoringEventRewardsBL(executionContext, scoringEventRewardsDTO);
                        scoringEventRewardsBL.Save(sqlTransaction);
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
