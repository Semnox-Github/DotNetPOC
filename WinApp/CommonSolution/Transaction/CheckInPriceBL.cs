/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  CheckInPrice
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
    /// Business logic for CheckInPrice class.
    /// </summary>
    public class CheckInPriceBL
    {
        private CheckInPriceDTO checkInPriceDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of CheckInPriceBL class
        /// </summary>
        private CheckInPriceBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates CheckInPriceBL object using the CheckInPriceDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="checkInPriceDTO">CheckInPriceDTO object</param>
        public CheckInPriceBL(ExecutionContext executionContext, CheckInPriceDTO checkInPriceDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, checkInPriceDTO);
            this.checkInPriceDTO = checkInPriceDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CheckInPrice id as the parameter
        /// Would fetch the CheckInPrice object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of CheckInPrice passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public CheckInPriceBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CheckInPriceDataHandler checkInPriceDataHandler = new CheckInPriceDataHandler(sqlTransaction);
            checkInPriceDTO = checkInPriceDataHandler.GetCheckInPriceDTO(id);
            if (checkInPriceDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CheckInPrice", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the CheckInPriceDTO
        /// Checks if the  id is not less than  0
        /// If it is less than 0, then inserts else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object to be passed</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (checkInPriceDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CheckInPriceDataHandler checkInPriceDataHandler = new CheckInPriceDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (checkInPriceDTO.Id < 0)
            {
                log.LogVariableState("CheckInPriceDTO", checkInPriceDTO);
                checkInPriceDTO = checkInPriceDataHandler.Insert(checkInPriceDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                checkInPriceDTO.AcceptChanges();
            }
            else if (checkInPriceDTO.IsChanged)
            {
                log.LogVariableState("CheckInPriceDTO", checkInPriceDTO);
                checkInPriceDTO = checkInPriceDataHandler.Update(checkInPriceDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                checkInPriceDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the CheckInPrice DTO values 
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
        public CheckInPriceDTO CheckInPriceDTO
        {
            get
            {
                return checkInPriceDTO;
            }
        }
    }
    /// <summary>
    /// Manages the list of CheckInPrice
    /// </summary>
    public class CheckInPriceListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CheckInPriceDTO> checkInPriceDTOList = new List<CheckInPriceDTO>();
        /// <summary>
        /// Parameterized constructor for CheckInPriceListBL
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public CheckInPriceListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for CheckInPriceListBL with CheckInDTOList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="checkInPriceDTOList">checkInPriceDTOList object is passed as parameter</param>
        public CheckInPriceListBL(ExecutionContext executionContext,
                                                List<CheckInPriceDTO> checkInPriceDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, checkInPriceDTOList);
            this.checkInPriceDTOList = checkInPriceDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the CheckInPriceDTO List
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>Returns the List of CheckInPriceDTO</returns>
        public List<CheckInPriceDTO> GetCheckInPriceDTOList(List<KeyValuePair<CheckInPriceDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CheckInPriceDataHandler checkInDataPriceHandler = new CheckInPriceDataHandler(sqlTransaction);
            List<CheckInPriceDTO> checkInPriceDTOList = checkInDataPriceHandler.GetCheckInPriceDTOList(searchParameters);
            log.LogMethodExit(checkInPriceDTOList);
            return checkInPriceDTOList;
        }

        /// <summary>
        /// Saves the  List of CheckInPriceDTO objects
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (checkInPriceDTOList == null ||
                checkInPriceDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < checkInPriceDTOList.Count; i++)
            {
                var checkInPriceDTO = checkInPriceDTOList[i];
                if (checkInPriceDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    CheckInPriceBL checkInPriceBL = new CheckInPriceBL(executionContext, checkInPriceDTO);
                    checkInPriceBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving CheckInPriceDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("CheckInPriceDTO", checkInPriceDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
