/********************************************************************************************
 * Project Name - ScoringEngine
 * Description  - Bussiness logic of TransactionLineGamePlayMapping
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.0     18-Jan-2022   Prajwal S      Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Represents a TransactionLineGamePlayMappingBL.
    /// </summary>
    internal class TransactionLineGamePlayMappingBL
    {
        private TransactionLineGamePlayMappingDTO transactionLineGamePlayMappingDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        private TransactionLineGamePlayMappingBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="transactionLineGamePlayMappingDTO">TransactionLineGamePlayMappingDTO</param>
        internal TransactionLineGamePlayMappingBL(ExecutionContext executionContext, TransactionLineGamePlayMappingDTO transactionLineGamePlayMappingDTO)
             : this(executionContext)
        {
            log.LogMethodEntry(executionContext, transactionLineGamePlayMappingDTO);
            if (transactionLineGamePlayMappingDTO.TransactionLineGamePlayMappingId < 0)
            {
                //validate();
            }
            this.transactionLineGamePlayMappingDTO = transactionLineGamePlayMappingDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public TransactionLineGamePlayMappingBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            TransactionLineGamePlayMappingDataHandler transactionLineGamePlayMappingDataHandler = new TransactionLineGamePlayMappingDataHandler(sqlTransaction);
            transactionLineGamePlayMappingDTO = transactionLineGamePlayMappingDataHandler.GetTransactionLineGamePlayMapping(id);
            if (transactionLineGamePlayMappingDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TransactionLineGamePlayMapping", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get TransactionLineGamePlayMappingDTO Object
        /// </summary>
        internal TransactionLineGamePlayMappingDTO TransactionLineGamePlayMappingDTO
        {
            get { return transactionLineGamePlayMappingDTO; }
        }


        /// <summary>
        /// Saves the TransactionLineGamePlayMappings
        /// Checks if the id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            TransactionLineGamePlayMappingDataHandler transactionLineGamePlayMappingDataHandler = new TransactionLineGamePlayMappingDataHandler(sqlTransaction);

            if (transactionLineGamePlayMappingDTO.TransactionLineGamePlayMappingId < 0)
            {
                transactionLineGamePlayMappingDTO = transactionLineGamePlayMappingDataHandler.Insert(transactionLineGamePlayMappingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                transactionLineGamePlayMappingDTO.AcceptChanges();
            }
            else
            {
                if (transactionLineGamePlayMappingDTO.IsChanged)
                {
                    transactionLineGamePlayMappingDTO = transactionLineGamePlayMappingDataHandler.Update(transactionLineGamePlayMappingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    transactionLineGamePlayMappingDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        internal void Update(TransactionLineGamePlayMappingDTO parameterTransactionLineGamePlayMappingDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterTransactionLineGamePlayMappingDTO);
            ChangeGamePlayId(parameterTransactionLineGamePlayMappingDTO.GamePlayId);
            ChangeActiveFlag(parameterTransactionLineGamePlayMappingDTO.IsActive);
            ChangeTransactionId(parameterTransactionLineGamePlayMappingDTO.TransactionId);
            ChangeTransactionLineId(parameterTransactionLineGamePlayMappingDTO.TransactionLineId);
            log.LogMethodExit();
        }

        private void ChangeTransactionLineId(int transactionLineId)
        {
            log.LogMethodEntry(transactionLineId);
            if (transactionLineGamePlayMappingDTO.TransactionLineId == transactionLineId)
            {
                log.LogMethodExit(null, "No changes to transactionLineId");
                return;
            }
            transactionLineGamePlayMappingDTO.TransactionLineId = transactionLineId;
            log.LogMethodExit();
        }

        private void ChangeTransactionId(int transactionId)
        {
            log.LogMethodEntry(transactionId);
            if (transactionLineGamePlayMappingDTO.TransactionId == transactionId)
            {
                log.LogMethodExit(null, "No changes to TransactionId");
                return;
            }
            transactionLineGamePlayMappingDTO.TransactionId = transactionId;
            log.LogMethodExit();
        }

        private void ChangeActiveFlag(bool activeFlag)
        {
            log.LogMethodEntry(activeFlag);
            if (transactionLineGamePlayMappingDTO.IsActive == activeFlag)
            {
                log.LogMethodExit(null, "No changes to TransactionLineGamePlayMapping activeFlag");
                return;
            }
            transactionLineGamePlayMappingDTO.IsActive = activeFlag;
            log.LogMethodExit();
        }

        private void ChangeGamePlayId(int gamePlayId)
        {
            log.LogMethodEntry(gamePlayId);
            // ValidateScoringEventId(accountCreditPlusConsumptionId);
            if (transactionLineGamePlayMappingDTO.GamePlayId == gamePlayId)
            {
                log.LogMethodExit(null, "No changes to GamePlayId");
                return;
            }
            transactionLineGamePlayMappingDTO.GamePlayId = gamePlayId;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of TransactionLineGamePlayMapping
    /// </summary>
    public class TransactionLineGamePlayMappingListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default Constructor
        /// </summary>
        internal TransactionLineGamePlayMappingListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="TransactionLineGamePlayMappingList"></param>
        public TransactionLineGamePlayMappingListBL(ExecutionContext executionContext)
             : this()
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the TransactionLineGamePlayMappingDTO List for accountCreditPlusConsumptionId Id List
        /// </summary>
        /// <param name="accountCreditPlusConsumptionIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<TransactionLineGamePlayMappingDTO> GetTransactionLineGamePlayMappingDTOListOfConsumption(List<int> accountCreditPlusConsumptionIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountCreditPlusConsumptionIdList, activeRecords, sqlTransaction);
            TransactionLineGamePlayMappingDataHandler transactionLineGamePlayMappingDataHandler = new TransactionLineGamePlayMappingDataHandler(sqlTransaction);
            List<TransactionLineGamePlayMappingDTO> transactionLineGamePlayMappingDTOList = transactionLineGamePlayMappingDataHandler.GetTransactionLineGamePlayMappingDTOList(accountCreditPlusConsumptionIdList, activeRecords);
            log.LogMethodExit(transactionLineGamePlayMappingDTOList);
            return transactionLineGamePlayMappingDTOList;
        }

        /// <summary>
        /// Returns the TransactionLineGamePlayMapping list
        /// </summary>
        public List<TransactionLineGamePlayMappingDTO> GetTransactionLineGamePlayMappingDTOList(List<KeyValuePair<TransactionLineGamePlayMappingDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TransactionLineGamePlayMappingDataHandler transactionLineGamePlayMappingDataHandler = new TransactionLineGamePlayMappingDataHandler(sqlTransaction);
            List<TransactionLineGamePlayMappingDTO> transactionLineGamePlayMappingDTOList = transactionLineGamePlayMappingDataHandler.GetTransactionLineGamePlayMappingList(searchParameters);
            log.LogMethodExit(transactionLineGamePlayMappingDTOList);
            return transactionLineGamePlayMappingDTOList;
        }

        /// <summary>
        /// This method should be used to Save TransactionLineGamePlayMapping
        /// </summary>
        public List<TransactionLineGamePlayMappingDTO> Save(List<TransactionLineGamePlayMappingDTO> transactionLineGamePlayMappingDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<TransactionLineGamePlayMappingDTO> savedDTOs = new List<TransactionLineGamePlayMappingDTO>();
            if (transactionLineGamePlayMappingDTOList != null && transactionLineGamePlayMappingDTOList.Count != 0)
            {
                foreach (TransactionLineGamePlayMappingDTO transactionLineGamePlayMappingDTO in transactionLineGamePlayMappingDTOList)
                {
                    TransactionLineGamePlayMappingBL transactionLineGamePlayMappingBL = new TransactionLineGamePlayMappingBL(executionContext, transactionLineGamePlayMappingDTO);
                    transactionLineGamePlayMappingBL.Save(sqlTransaction);
                    savedDTOs.Add(transactionLineGamePlayMappingBL.TransactionLineGamePlayMappingDTO);
                }
            }
            log.LogMethodExit();
            return savedDTOs;
        }
    }
}
