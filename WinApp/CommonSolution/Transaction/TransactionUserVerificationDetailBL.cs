/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  TrxUserVerificationDetail
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
    /// Business logic for TransactionUserVerificationDetailBL class.
    /// </summary>
    public class TransactionUserVerificationDetailBL
    {
        private TransactionUserVerificationDetailDTO trxUserVerificationDetailDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of TransactionUserVerificationDetailBL class
        /// </summary>
        private TransactionUserVerificationDetailBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates TransactionUserVerificationDetailBL object using the TransactionUserVerificationDetailDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="trxUserVerificationDetailDTO">TransactionUserVerificationDetailDTO object</param>
        public TransactionUserVerificationDetailBL(ExecutionContext executionContext, TransactionUserVerificationDetailDTO trxUserVerificationDetailDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxUserVerificationDetailDTO);
            this.trxUserVerificationDetailDTO = trxUserVerificationDetailDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the TrxUserVerificationDetail id as the parameter
        /// Would fetch the TrxUserVerificationDetail object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of TrxUserVerificationDetail passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public TransactionUserVerificationDetailBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TransactionUserVerificationDetailDataHandler trxUserVerificationDetailDataHandler = new TransactionUserVerificationDetailDataHandler(sqlTransaction);
            trxUserVerificationDetailDTO = trxUserVerificationDetailDataHandler.GetTrxUserVerificationDetailDTO(id);
            if (trxUserVerificationDetailDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TrxUserVerificationDetail", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TransactionUserVerificationDetailDTO
        /// Checks if the  id is not less than  0
        /// If it is less than 0, then inserts else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object to be passed</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (trxUserVerificationDetailDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            TransactionUserVerificationDetailDataHandler trxUserVerificationDetailDataHandler = new TransactionUserVerificationDetailDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (trxUserVerificationDetailDTO.TrxUserVerificationDetId < 0)
            {
                log.LogVariableState("TransactionUserVerificationDetailDTO", trxUserVerificationDetailDTO);
                trxUserVerificationDetailDTO = trxUserVerificationDetailDataHandler.Insert(trxUserVerificationDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                trxUserVerificationDetailDTO.AcceptChanges();
            }
            else if (trxUserVerificationDetailDTO.IsChanged)
            {
                log.LogVariableState("TransactionUserVerificationDetailDTO", trxUserVerificationDetailDTO);
                trxUserVerificationDetailDTO = trxUserVerificationDetailDataHandler.Update(trxUserVerificationDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                trxUserVerificationDetailDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the TransactionUserVerificationDetailDTO  values 
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
        public TransactionUserVerificationDetailDTO TrxUserVerificationDetailDTO
        {
            get
            {
                return trxUserVerificationDetailDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of TransactionUserVerificationDetailDTO
    /// </summary>
    public class TransactionUserVerificationDetailListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<TransactionUserVerificationDetailDTO> trxUserVerificationDetailDTOList = new List<TransactionUserVerificationDetailDTO>();
        /// <summary>
        /// Parameterized constructor for TransactionUserVerificationDetailListBL
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public TransactionUserVerificationDetailListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for TransactionUserVerificationDetailListBL with trxUserVerificationDetailDTOList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="trxUserVerificationDetailDTOList">TransactionUserVerificationDetailDTO List object is passed as parameter</param>
        public TransactionUserVerificationDetailListBL(ExecutionContext executionContext,
                                                List<TransactionUserVerificationDetailDTO> trxUserVerificationDetailDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxUserVerificationDetailDTOList);
            this.trxUserVerificationDetailDTOList = trxUserVerificationDetailDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the TransactionUserVerificationDetailDTO List
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>Returns the List of TransactionUserVerificationDetailDTO List</returns>
        public List<TransactionUserVerificationDetailDTO> GetTrxUserVerificationDetailDTOList(List<KeyValuePair<TransactionUserVerificationDetailDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TransactionUserVerificationDetailDataHandler trxUserVerificationDetailDataHandler = new TransactionUserVerificationDetailDataHandler(sqlTransaction);
            List<TransactionUserVerificationDetailDTO> trxUserVerificationDetailDTOList = trxUserVerificationDetailDataHandler.GetTrxUserVerificationDetailDTOList(searchParameters);
            log.LogMethodExit(trxUserVerificationDetailDTOList);
            return trxUserVerificationDetailDTOList;
        }

        /// <summary>
        /// Saves the  List of TransactionUserVerificationDetailDTO objects
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (trxUserVerificationDetailDTOList == null ||
                trxUserVerificationDetailDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < trxUserVerificationDetailDTOList.Count; i++)
            {
                var trxUserVerificationDetailDTO = trxUserVerificationDetailDTOList[i];
                if (trxUserVerificationDetailDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    TransactionUserVerificationDetailBL trxUserVerificationDetailBL = new TransactionUserVerificationDetailBL(executionContext, trxUserVerificationDetailDTO);
                    trxUserVerificationDetailBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving TransactionUserVerificationDetailDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("TransactionUserVerificationDetailDTO", trxUserVerificationDetailDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
