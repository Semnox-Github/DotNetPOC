/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  TrxSplitLine
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      16-June-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Business logic for TrxSplitLine class.
    /// </summary>
    public class TransactionSplitLineBL
    {
        private TransactionSplitLineDTO trxSplitLineDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of TransactionSplitLineBL class
        /// </summary>
        private TransactionSplitLineBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates TransactionSplitLineBL object using the TransactionSplitLineDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="trxSplitLineDTO">TransactionSplitLineDTO object</param>
        public TransactionSplitLineBL(ExecutionContext executionContext, TransactionSplitLineDTO trxSplitLineDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxSplitLineDTO);
            this.trxSplitLineDTO = trxSplitLineDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the TrxSplitLine id as the parameter
        /// Would fetch the TrxSplitLine object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of TrxSplitLine passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public TransactionSplitLineBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TransactionSplitLineDataHandler trxSplitLineDataHandler = new TransactionSplitLineDataHandler(sqlTransaction);
            trxSplitLineDTO = trxSplitLineDataHandler.GetTrxSplitLineDTO(id);
            if (trxSplitLineDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TrxSplitLine", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TransactionSplitLineDTO
        /// Checks if the  id is not less than  0
        /// If it is less than 0, then inserts else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object to be passed</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (trxSplitLineDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            TransactionSplitLineDataHandler trxSplitLineDataHandler = new TransactionSplitLineDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (trxSplitLineDTO.Id < 0)
            {
                log.LogVariableState("TransactionSplitLineDTO", trxSplitLineDTO);
                trxSplitLineDTO = trxSplitLineDataHandler.Insert(trxSplitLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                trxSplitLineDTO.AcceptChanges();
            }
            else if (trxSplitLineDTO.IsChanged)
            {
                log.LogVariableState("TransactionSplitLineDTO", trxSplitLineDTO);
                trxSplitLineDTO = trxSplitLineDataHandler.Update(trxSplitLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                trxSplitLineDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the TransactionSplitLineDTO  values 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (true) // Fields to be validated here.
            {
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TransactionSplitLineDTO TrxSplitLineDTO
        {
            get
            {
                return trxSplitLineDTO;
            }
        }

    }
    /// <summary>
    /// Manages the list of TransactionSplitLineDTO
    /// </summary>
    public class TransactionSplitLineListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<TransactionSplitLineDTO> trxSplitLineDTOList = new List<TransactionSplitLineDTO>();
        /// <summary>
        /// Parameterized constructor for TransactionSplitLineListBL
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public TransactionSplitLineListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for TransactionSplitLineListBL with trxSplitLineDTOList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="trxSplitLineDTOList">TransactionSplitLineDTO List object is passed as parameter</param>
        public TransactionSplitLineListBL(ExecutionContext executionContext,
                                                List<TransactionSplitLineDTO> trxSplitLineDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxSplitLineDTOList);
            this.trxSplitLineDTOList = trxSplitLineDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the TransactionSplitLineDTO List
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>Returns the List of TransactionSplitLineDTO List</returns>
        public List<TransactionSplitLineDTO> GetTrxSplitLineDTOList(List<KeyValuePair<TransactionSplitLineDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TransactionSplitLineDataHandler trxSplitLineDataHandler = new TransactionSplitLineDataHandler(sqlTransaction);
            List<TransactionSplitLineDTO> trxSplitLineDTOList = trxSplitLineDataHandler.GetTrxSplitLineDTOList(searchParameters);
            log.LogMethodExit(trxSplitLineDTOList);
            return trxSplitLineDTOList;
        }

        /// <summary>
        /// Saves the  List of TransactionSplitLineDTO objects
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (trxSplitLineDTOList == null ||
                trxSplitLineDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < trxSplitLineDTOList.Count; i++)
            {
                var trxSplitLineDTO = trxSplitLineDTOList[i];
                if (trxSplitLineDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    TransactionSplitLineBL trxSplitLineBL = new TransactionSplitLineBL(executionContext, trxSplitLineDTO);
                    trxSplitLineBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving TransactionSplitLineDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("TransactionSplitLineDTO", trxSplitLineDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
