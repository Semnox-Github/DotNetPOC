/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  TipPayment
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
    /// Business logic for TipPayment class.
    /// </summary>
    public class TipPaymentBL
    {
        private TipPaymentDTO tipPaymentDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of TipPaymentBL class
        /// </summary>
        private TipPaymentBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates TipPaymentBL object using the TipPaymentDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="tipPaymentDTO">TipPaymentDTO object</param>
        public TipPaymentBL(ExecutionContext executionContext, TipPaymentDTO tipPaymentDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, tipPaymentDTO);
            this.tipPaymentDTO = tipPaymentDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Task id as the parameter
        /// Would fetch the Task object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of Task passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public TipPaymentBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TipPaymentDataHandler tipPaymentDataHandler = new TipPaymentDataHandler(sqlTransaction);
            tipPaymentDTO = tipPaymentDataHandler.GetTipPaymentDTO(id);
            if (tipPaymentDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TipPayment", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TipPaymentDTO
        /// Checks if the  id is not less than  0
        /// If it is less than 0, then inserts else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object to be passed</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (tipPaymentDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            TipPaymentDataHandler tipPaymentDataHandler = new TipPaymentDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (tipPaymentDTO.TipId < 0)
            {
                log.LogVariableState("TipPaymentDTO", tipPaymentDTO);
                tipPaymentDTO = tipPaymentDataHandler.Insert(tipPaymentDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                tipPaymentDTO.AcceptChanges();
            }
            else if (tipPaymentDTO.IsChanged)
            {
                log.LogVariableState("TipPaymentDTO", tipPaymentDTO);
                tipPaymentDTO = tipPaymentDataHandler.Update(tipPaymentDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                tipPaymentDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the TipPaymentDTO  values 
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
        public TipPaymentDTO TipPaymentDTO
        {
            get
            {
                return tipPaymentDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of TipPaymentDTO
    /// </summary>
    public class TipPaymentListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<TipPaymentDTO> tipPaymentDTOList = new List<TipPaymentDTO>();
        /// <summary>
        /// Parameterized constructor for TipPaymentListBL
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public TipPaymentListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for TipPaymentListBL with tipPaymentDTOList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="tipPaymentDTOList">TipPayment DTO List object is passed as parameter</param>
        public TipPaymentListBL(ExecutionContext executionContext,
                                                List<TipPaymentDTO> tipPaymentDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, tipPaymentDTOList);
            this.tipPaymentDTOList = tipPaymentDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the TipPaymentDTO List
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>Returns the List of TipPaymentDTO</returns>
        public List<TipPaymentDTO> GetTipPaymentDTOList(List<KeyValuePair<TipPaymentDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TipPaymentDataHandler tipPaymentDataHandler = new TipPaymentDataHandler(sqlTransaction);
            List<TipPaymentDTO> tipPaymentDTOList = tipPaymentDataHandler.GetTipPaymentDTOList(searchParameters);
            log.LogMethodExit(tipPaymentDTOList);
            return tipPaymentDTOList;
        }

        /// <summary>
        /// Saves the  List of TipPaymentDTO objects
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (tipPaymentDTOList == null ||
                tipPaymentDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < tipPaymentDTOList.Count; i++)
            {
                var tipPaymentDTO = tipPaymentDTOList[i];
                if (tipPaymentDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    TipPaymentBL tipPaymentBL = new TipPaymentBL(executionContext, tipPaymentDTO);
                    tipPaymentBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving TipPaymentDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("TipPaymentDTO", tipPaymentDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
