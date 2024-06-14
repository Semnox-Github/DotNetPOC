/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  TrxTaxLine
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
    /// Business logic for TrxTaxLine class.
    /// </summary>
    public class TransactionTaxLineBL
    {
        private TransactionTaxLineDTO trxTaxLineDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of TransactionTaxLineBL class
        /// </summary>
        private TransactionTaxLineBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates TransactionTaxLineBL object using the TransactionTaxLineDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="trxTaxLineDTO">TransactionTaxLineDTO object</param>
        public TransactionTaxLineBL(ExecutionContext executionContext, TransactionTaxLineDTO trxTaxLineDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxTaxLineDTO);
            this.trxTaxLineDTO = trxTaxLineDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the TrxTaxLine id as the parameter
        /// Would fetch the TrxTaxLine object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of TrxTaxLine passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public TransactionTaxLineBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TransactionTaxLineDataHandler trxTaxLineDataHandler = new TransactionTaxLineDataHandler(sqlTransaction);
            trxTaxLineDTO = trxTaxLineDataHandler.GetTrxTaxLineDTO(id);
            if (trxTaxLineDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TransactionTaxLineDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TransactionTaxLineDTO
        /// Checks if the  id is not less than  0
        /// If it is less than 0, then inserts else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object to be passed</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (trxTaxLineDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            TransactionTaxLineDataHandler trxTaxLineDataHandler = new TransactionTaxLineDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (trxTaxLineDTO.TrxTaxLineId < 0)
            {
                log.LogVariableState("TransactionTaxLineDTO", trxTaxLineDTO);
                trxTaxLineDTO = trxTaxLineDataHandler.Insert(trxTaxLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                trxTaxLineDTO.AcceptChanges();
            }
            else if (trxTaxLineDTO.IsChanged)
            {
                log.LogVariableState("TransactionTaxLineDTO", trxTaxLineDTO);
                trxTaxLineDTO = trxTaxLineDataHandler.Update(trxTaxLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                trxTaxLineDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the TransactionTaxLineDTO  values 
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
        public TransactionTaxLineDTO TrxTaxLineDTO
        {
            get
            {
                return trxTaxLineDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of TransactionTaxLineDTO
    /// </summary>
    public class TransactionTaxLineListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<TransactionTaxLineDTO> trxTaxLineDTOList = new List<TransactionTaxLineDTO>();
        /// <summary>
        /// Parameterized constructor for TransactionTaxLineListBL
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public TransactionTaxLineListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for TransactionTaxLineListBL with trxTaxLineDTOList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="trxTaxLineDTOList">TransactionTaxLineDTO List object is passed as parameter</param>
        public TransactionTaxLineListBL(ExecutionContext executionContext,
                                                List<TransactionTaxLineDTO> trxTaxLineDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxTaxLineDTOList);
            this.trxTaxLineDTOList = trxTaxLineDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the TransactionTaxLineDTO List
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>Returns the List of TransactionTaxLineDTO List</returns>
        public List<TransactionTaxLineDTO> GetTrxTaxLineDTOList(List<KeyValuePair<TransactionTaxLineDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TransactionTaxLineDataHandler trxTaxLineDataHandler = new TransactionTaxLineDataHandler(sqlTransaction);
            List<TransactionTaxLineDTO> trxTaxLineDTOList = trxTaxLineDataHandler.GetTrxTaxLineDTOList(searchParameters);
            log.LogMethodExit(trxTaxLineDTOList);
            return trxTaxLineDTOList;
        }

        /// <summary>
        /// Saves the  List of TransactionTaxLineDTO objects
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (trxTaxLineDTOList == null ||
                trxTaxLineDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < trxTaxLineDTOList.Count; i++)
            {
                var trxTaxLineDTO = trxTaxLineDTOList[i];
                if (trxTaxLineDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    TransactionTaxLineBL trxTaxLineBL = new TransactionTaxLineBL(executionContext, trxTaxLineDTO);
                    trxTaxLineBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving TransactionTaxLineDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("TransactionTaxLineDTO", trxTaxLineDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
