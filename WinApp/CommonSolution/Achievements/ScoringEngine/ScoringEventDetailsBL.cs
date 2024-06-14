/********************************************************************************************
 * Project Name - ScoringEngine
 * Description  - Bussiness logic of ScoringEventDetails
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
    /// Represents ScoringEventDetails.
    /// </summary>
    internal class ScoringEventDetailsBL
    {
        private ScoringEventDetailsDTO scoringEventDetailsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        private ScoringEventDetailsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="scoringEventDetailsDTO">ScoringEventDetailsDTO</param>
        internal ScoringEventDetailsBL(ExecutionContext executionContext, ScoringEventDetailsDTO scoringEventDetailsDTO)
             : this(executionContext)
        {
            log.LogMethodEntry(executionContext, scoringEventDetailsDTO);
            if (scoringEventDetailsDTO.ScoringEventDetailId < 0)
            {
               //validate();
            }
            this.scoringEventDetailsDTO = scoringEventDetailsDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the Id parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ScoringEventDetailsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ScoringEventDetailsDataHandler scoringEventDetailsDataHandler = new ScoringEventDetailsDataHandler(sqlTransaction);
            scoringEventDetailsDTO = scoringEventDetailsDataHandler.GetScoringEventDetails(id);
            if (scoringEventDetailsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ScoringEventDetails", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get ScoringEventDetailsDTO Object
        /// </summary>
        internal ScoringEventDetailsDTO ScoringEventDetailsDTO
        {
            get { return scoringEventDetailsDTO; }
        }


        /// <summary>
        /// Saves the ScoringEventDetails
        /// Checks if the ScoringEventDetails id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ScoringEventDetailsDataHandler scoringEventDetailsDataHandler = new ScoringEventDetailsDataHandler(sqlTransaction);

            if (scoringEventDetailsDTO.ScoringEventDetailId < 0)
            {
                scoringEventDetailsDTO = scoringEventDetailsDataHandler.Insert(scoringEventDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                scoringEventDetailsDTO.AcceptChanges();
            }
            else
            {
                if (scoringEventDetailsDTO.IsChanged)
                {
                    scoringEventDetailsDTO = scoringEventDetailsDataHandler.Update(scoringEventDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    scoringEventDetailsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        internal void Update(ScoringEventDetailsDTO parameterScoringEventDetailsDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterScoringEventDetailsDTO);
            ChangeScoringEventId(parameterScoringEventDetailsDTO.ScoringEventId);
            ChangeActiveFlag(parameterScoringEventDetailsDTO.ActiveFlag);
            ChangeTriggerGameId(parameterScoringEventDetailsDTO.TriggerGameId);
            ChangeTriggerGameProfileId(parameterScoringEventDetailsDTO.TriggerGameProfileId);
            ChangeTicketMultiplierForScore(parameterScoringEventDetailsDTO.TicketMultiplierForScore);
            ChangeSequence(parameterScoringEventDetailsDTO.Sequence);
            ChangeQualifyingGameplays(parameterScoringEventDetailsDTO.QualifyingGameplays);
            ChangeQualifyingTickets(parameterScoringEventDetailsDTO.QualifyingTickets);
            ChangeReaderThemeId(parameterScoringEventDetailsDTO.ReaderThemeId);
            ChangeAbsoluteScore(parameterScoringEventDetailsDTO.AbsoluteScore);
            log.LogMethodExit();
        }

        private void ChangeQualifyingTickets(int? qualifyingTickets)
        {
            log.LogMethodEntry(qualifyingTickets);
            if (scoringEventDetailsDTO.QualifyingTickets == qualifyingTickets)
            {
                log.LogMethodExit(null, "No changes to QualifyingTickets");
                return;
            }
            scoringEventDetailsDTO.QualifyingTickets = qualifyingTickets;
            log.LogMethodExit();
        }

        private void ChangeAbsoluteScore(double? absoluteScore)
        {
            log.LogMethodEntry(absoluteScore);
            if (scoringEventDetailsDTO.AbsoluteScore == absoluteScore)
            {
                log.LogMethodExit(null, "No changes to AbsoluteScore");
                return;
            }
            scoringEventDetailsDTO.AbsoluteScore = absoluteScore;
            log.LogMethodExit();
        }

        private void ChangeReaderThemeId(int readerThemeId)
        {
            log.LogMethodEntry(readerThemeId);
            if (scoringEventDetailsDTO.ReaderThemeId == readerThemeId)
            {
                log.LogMethodExit(null, "No changes to ReaderThemeId");
                return;
            }
            scoringEventDetailsDTO.ReaderThemeId = readerThemeId;
            log.LogMethodExit();
        }
        private void ChangeQualifyingGameplays(int? qualifyingGameplays)
        {
            log.LogMethodEntry(qualifyingGameplays);
            if (scoringEventDetailsDTO.QualifyingGameplays == qualifyingGameplays)
            {
                log.LogMethodExit(null, "No changes to qualifyingGameplays");
                return;
            }
            scoringEventDetailsDTO.QualifyingGameplays = qualifyingGameplays;
            log.LogMethodExit();
        }
        private void ChangeSequence(int sequence)
        {
            log.LogMethodEntry(sequence);
            if (scoringEventDetailsDTO.Sequence == sequence)
            {
                log.LogMethodExit(null, "No changes to Sequence");
                return;
            }
            scoringEventDetailsDTO.Sequence = sequence;
            log.LogMethodExit();
        }

        private void ChangeTicketMultiplierForScore(int? ticketMultiplierForScore)
        {
            log.LogMethodEntry(ticketMultiplierForScore);
            if (scoringEventDetailsDTO.TicketMultiplierForScore == ticketMultiplierForScore)
            {
                log.LogMethodExit(null, "No changes to TicketMultiplierForScore");
                return;
            }
            scoringEventDetailsDTO.TicketMultiplierForScore = ticketMultiplierForScore;
            log.LogMethodExit();
        }

        private void ChangeTriggerGameId(int? triggerGameId)
        {
            log.LogMethodEntry(triggerGameId);
            if (scoringEventDetailsDTO.TriggerGameId == triggerGameId)
            {
                log.LogMethodExit(null, "No changes to TriggerGameId");
                return;
            }
            scoringEventDetailsDTO.TriggerGameId = triggerGameId;
            log.LogMethodExit();
        }

        private void ChangeTriggerGameProfileId(int? triggerGameProfileId)
        {
            log.LogMethodEntry(triggerGameProfileId);
            if (scoringEventDetailsDTO.TriggerGameProfileId == triggerGameProfileId)
            {
                log.LogMethodExit(null, "No changes to TriggerGameProfileId");
                return;
            }
            scoringEventDetailsDTO.TriggerGameProfileId = triggerGameProfileId;
            log.LogMethodExit();
        }

        private void ChangeActiveFlag(bool activeFlag)
        {
            log.LogMethodEntry(activeFlag);
            if (scoringEventDetailsDTO.ActiveFlag == activeFlag)
            {
                log.LogMethodExit(null, "No changes to ScoringEventDetailsDTO activeFlag");
                return;
            }
            scoringEventDetailsDTO.ActiveFlag = activeFlag;
            log.LogMethodExit();
        }

        private void ChangeScoringEventId(int scoringEventId)
        {
            log.LogMethodEntry(scoringEventId);
           // ValidateScoringEventId(scoringEventId);
            if (scoringEventDetailsDTO.ScoringEventId == scoringEventId)
            {
                log.LogMethodExit(null, "No changes to ScoringEventId");
                return;
            }
            scoringEventDetailsDTO.ScoringEventId = scoringEventId;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of ScoringEventDetails
    /// </summary>
    public class ScoringEventDetailsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ScoringEventDetailsDTO> scoringEventDetailsList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScoringEventDetailsListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="ScoringEventDetailsList"></param>
        public ScoringEventDetailsListBL(ExecutionContext executionContext, List<ScoringEventDetailsDTO> ScoringEventDetailsList)
             : this()
        {
            log.LogMethodEntry(executionContext, ScoringEventDetailsList);
            this.executionContext = executionContext;
            this.scoringEventDetailsList = ScoringEventDetailsList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ScoringEventDetailsDTO List for scoringEventId Id List
        /// </summary>
        /// <param name="scoringEventIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<ScoringEventDetailsDTO> GetScoringEventDetailsDTOListOfEvents(List<int> scoringEventIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scoringEventIdList, activeRecords, sqlTransaction);
            ScoringEventDetailsDataHandler scoringEventDetailsDataHandler = new ScoringEventDetailsDataHandler(sqlTransaction);
            List<ScoringEventDetailsDTO> scoringEventDetailsDTOList = scoringEventDetailsDataHandler.GetScoringEventDetailsDTOList(scoringEventIdList, activeRecords);
            log.LogMethodExit(scoringEventDetailsDTOList);
            return scoringEventDetailsDTOList;
        }

        /// <summary>
        /// Returns the ScoringEventDetails list
        /// </summary>
        public List<ScoringEventDetailsDTO> GetScoringEventDetailsDTOList(List<KeyValuePair<ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ScoringEventDetailsDataHandler scoringEventDetailsDataHandler = new ScoringEventDetailsDataHandler(sqlTransaction);
            List<ScoringEventDetailsDTO> scoringEventDetailsDTOList = scoringEventDetailsDataHandler.GetScoringEventDetailsList(searchParameters);
            log.LogMethodExit(scoringEventDetailsDTOList);
            return scoringEventDetailsDTOList;
        }

        /// <summary>
        /// This method should be used to Save ScoringEventDetails
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (scoringEventDetailsList != null && scoringEventDetailsList.Count != 0)
                {
                    foreach (ScoringEventDetailsDTO scoringEventDetailsDTO in scoringEventDetailsList)
                    {
                        ScoringEventDetailsBL scoringEventDetailsBL = new ScoringEventDetailsBL(executionContext, scoringEventDetailsDTO);
                        scoringEventDetailsBL.Save(sqlTransaction);
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
