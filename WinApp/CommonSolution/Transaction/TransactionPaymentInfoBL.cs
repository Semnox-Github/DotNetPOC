/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  TrxPaymentsInfo
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
    /// Business logic for TrxPaymentsInfo class.
    /// </summary>
    public class TransactionPaymentInfoBL
    {
        private TransactionPaymentInfoDTO trxPaymentInfoDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of TransactionPaymentInfoBL class
        /// </summary>
        private TransactionPaymentInfoBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates TransactionPaymentInfoBL object using the TransactionPaymentInfoDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="trxPaymentInfoDTO">TransactionPaymentInfoDTO object</param>
        public TransactionPaymentInfoBL(ExecutionContext executionContext, TransactionPaymentInfoDTO trxPaymentInfoDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxPaymentInfoDTO);
            this.trxPaymentInfoDTO = trxPaymentInfoDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the TrxPaymentsInfo id as the parameter
        /// Would fetch the TrxPaymentsInfo object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of TrxPaymentsInfo passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public TransactionPaymentInfoBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TransactionPaymentInfoDataHandler trxPaymentsInfoDataHandler = new TransactionPaymentInfoDataHandler(sqlTransaction);
            trxPaymentInfoDTO = trxPaymentsInfoDataHandler.GetTrxPaymentsInfoDTO(id);
            if (trxPaymentInfoDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TrxPaymentInfo", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TransactionPaymentInfoDTO
        /// Checks if the  id is not less than  0
        /// If it is less than 0, then inserts else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object to be passed</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (trxPaymentInfoDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            TransactionPaymentInfoDataHandler trxPaymentsInfoDataHandler = new TransactionPaymentInfoDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (trxPaymentInfoDTO.Id < 0)
            {
                log.LogVariableState("trxPaymentInfoDTO", trxPaymentInfoDTO);
                trxPaymentInfoDTO = trxPaymentsInfoDataHandler.Insert(trxPaymentInfoDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                trxPaymentInfoDTO.AcceptChanges();
            }
            else if (trxPaymentInfoDTO.IsChanged)
            {
                log.LogVariableState("trxPaymentInfoDTO", trxPaymentInfoDTO);
                trxPaymentInfoDTO = trxPaymentsInfoDataHandler.Update(trxPaymentInfoDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                trxPaymentInfoDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the TransactionPaymentInfoDTO  values 
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
        public TransactionPaymentInfoDTO TrxPaymentInfoDTO
        {
            get
            {
                return trxPaymentInfoDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of TransactionPaymentInfoDTO
    /// </summary>
    public class TransactionPaymentInfoListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<TransactionPaymentInfoDTO> trxPaymentInfoDTOList = new List<TransactionPaymentInfoDTO>();
        /// <summary>
        /// Parameterized constructor for TransactionPaymentInfoListBL
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public TransactionPaymentInfoListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for TransactionPaymentInfoListBL with trxPaymentInfoDTOList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="trxPaymentInfoDTOList">TransactionPaymentInfoDTO object is passed as parameter</param>
        public TransactionPaymentInfoListBL(ExecutionContext executionContext,
                                                List<TransactionPaymentInfoDTO> trxPaymentInfoDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxPaymentInfoDTOList);
            this.trxPaymentInfoDTOList = trxPaymentInfoDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the TransactionPaymentInfoDTO List
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>Returns the List of TransactionPaymentInfoDTO List</returns>
        public List<TransactionPaymentInfoDTO> GetTrxPaymentInfoDTOList(List<KeyValuePair<TransactionPaymentInfoDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TransactionPaymentInfoDataHandler trxPaymentInfoDataHandler = new TransactionPaymentInfoDataHandler(sqlTransaction);
            List<TransactionPaymentInfoDTO> trxPaymentInfoDTOList = trxPaymentInfoDataHandler.GetTrxPaymentsInfoDTOList(searchParameters);
            log.LogMethodExit(trxPaymentInfoDTOList);
            return trxPaymentInfoDTOList;
        }

        /// <summary>
        /// Saves the  List of TransactionPaymentInfoDTO objects
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (trxPaymentInfoDTOList == null ||
                trxPaymentInfoDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < trxPaymentInfoDTOList.Count; i++)
            {
                var trxPaymentInfoDTO = trxPaymentInfoDTOList[i];
                if (trxPaymentInfoDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    TransactionPaymentInfoBL trxPaymentInfoBL = new TransactionPaymentInfoBL(executionContext, trxPaymentInfoDTO);
                    trxPaymentInfoBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving TransactionPaymentInfoDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("TransactionPaymentInfoDTO", trxPaymentInfoDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
