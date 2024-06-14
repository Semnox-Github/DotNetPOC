/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  TrxParentModifierDetail
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
    /// Business logic for TrxParentModifierDetail class.
    /// </summary>
    public class TransactionParentModifierDetailBL
    {
        private TransactionParentModifierDetailDTO trxParentModifierDetailDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of TransactionParentModifierDetailBL class
        /// </summary>
        private TransactionParentModifierDetailBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates TransactionParentModifierDetailBL object using the TransactionParentModifierDetailDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="trxParentModifierDetailDTO">TransactionParentModifierDetailDTO object</param>
        public TransactionParentModifierDetailBL(ExecutionContext executionContext, TransactionParentModifierDetailDTO trxParentModifierDetailDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxParentModifierDetailDTO);
            this.trxParentModifierDetailDTO = trxParentModifierDetailDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the TrxParentModifierDetail id as the parameter
        /// Would fetch the TrxParentModifierDetail object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of TrxParentModifierDetail passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public TransactionParentModifierDetailBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TransactionParentModifierDetailDataHandler trxParentModifierDetailDataHandler = new TransactionParentModifierDetailDataHandler(sqlTransaction);
            trxParentModifierDetailDTO = trxParentModifierDetailDataHandler.GetTrxParentModifierDetailDTO(id);
            if (trxParentModifierDetailDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TrxParentModifierDetail", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TrxParentModifierDetail DTO
        /// Checks if the  id is not less than  0
        /// If it is less than 0, then inserts else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object to be passed</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (trxParentModifierDetailDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            TransactionParentModifierDetailDataHandler trxParentModifierDetailDataHandler = new TransactionParentModifierDetailDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (trxParentModifierDetailDTO.Id < 0)
            {
                log.LogVariableState("TrxParentModifierDetailDTO", trxParentModifierDetailDTO);
                trxParentModifierDetailDTO = trxParentModifierDetailDataHandler.Insert(trxParentModifierDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                trxParentModifierDetailDTO.AcceptChanges();
            }
            else if (trxParentModifierDetailDTO.IsChanged)
            {
                log.LogVariableState("TrxParentModifierDetailDTO", trxParentModifierDetailDTO);
                trxParentModifierDetailDTO = trxParentModifierDetailDataHandler.Update(trxParentModifierDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                trxParentModifierDetailDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the TransactionParentModifierDetailDTO  values 
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
        public TransactionParentModifierDetailDTO TrxParentModifierDetailDTO
        {
            get
            {
                return trxParentModifierDetailDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of TransactionParentModifierDetailDTO
    /// </summary>
    public class TransactionParentModifierDetailListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<TransactionParentModifierDetailDTO> trxParentModifierDetailDTOList = new List<TransactionParentModifierDetailDTO>();
        /// <summary>
        /// Parameterized constructor for TransactionParentModifierDetailListBL
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public TransactionParentModifierDetailListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for TransactionParentModifierDetailListBL with trxParentModifierDetailDTOList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="trxParentModifierDetailDTOList">TrxParent ModifierDetail DTO object is passed as parameter</param>
        public TransactionParentModifierDetailListBL(ExecutionContext executionContext,
                                                List<TransactionParentModifierDetailDTO> trxParentModifierDetailDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxParentModifierDetailDTOList);
            this.trxParentModifierDetailDTOList = trxParentModifierDetailDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the TransactionParentModifierDetailDTO List
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>Returns the List of trxParentModifierDetailDTOList</returns>
        public List<TransactionParentModifierDetailDTO> GetTrxParentModifierDetailDTOList(List<KeyValuePair<TransactionParentModifierDetailDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TransactionParentModifierDetailDataHandler trxParentModifierDetailDataHandler = new TransactionParentModifierDetailDataHandler(sqlTransaction);
            List<TransactionParentModifierDetailDTO> trxParentModifierDetailDTOList = trxParentModifierDetailDataHandler.GetTrxParentModifierDetailDTOList(searchParameters);
            log.LogMethodExit(trxParentModifierDetailDTOList);
            return trxParentModifierDetailDTOList;
        }

        /// <summary>
        /// Saves the  List of TransactionParentModifierDetailDTO objects
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (trxParentModifierDetailDTOList == null ||
                trxParentModifierDetailDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < trxParentModifierDetailDTOList.Count; i++)
            {
                var trxParentModifierDetailDTO = trxParentModifierDetailDTOList[i];
                if (trxParentModifierDetailDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    TransactionParentModifierDetailBL trxParentModifierDetailBL = new TransactionParentModifierDetailBL(executionContext, trxParentModifierDetailDTO);
                    trxParentModifierDetailBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving TrxParentModifierDetailDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("TrxParentModifierDetailDTO", trxParentModifierDetailDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
