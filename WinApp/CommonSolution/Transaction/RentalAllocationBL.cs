/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  RentalAllocation
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
    /// Business logic for RentalAllocation class.
    /// </summary>
    public class RentalAllocationBL
    {
        private RentalAllocationDTO rentalAllocationDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of RentalAllocationBL class
        /// </summary>
        private RentalAllocationBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates RentalAllocationBL object using the RentalAllocationDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="rentalAllocationDTO">RentalAllocationDTO object</param>
        public RentalAllocationBL(ExecutionContext executionContext, RentalAllocationDTO rentalAllocationDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, rentalAllocationDTO);
            this.rentalAllocationDTO = rentalAllocationDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the RentalAllocation id as the parameter
        /// Would fetch the RentalAllocation object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of RentalAllocation passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public RentalAllocationBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            RentalAllocationDataHandler rentalAllocationDataHandler = new RentalAllocationDataHandler(sqlTransaction);
            rentalAllocationDTO = rentalAllocationDataHandler.GetRentalAllocationDTO(id);
            if (rentalAllocationDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "RentalAllocation", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the RentalAllocationDTO
        /// Checks if the  id is not less than  0
        /// If it is less than 0, then inserts else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object to be passed</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (rentalAllocationDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            RentalAllocationDataHandler rentalAllocationDataHandler = new RentalAllocationDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (rentalAllocationDTO.Id < 0)
            {
                log.LogVariableState("RentalAllocationDTO", rentalAllocationDTO);
                rentalAllocationDTO = rentalAllocationDataHandler.Insert(rentalAllocationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                rentalAllocationDTO.AcceptChanges();
            }
            else if (rentalAllocationDTO.IsChanged)
            {
                log.LogVariableState("RentalAllocationDTO", rentalAllocationDTO);
                rentalAllocationDTO = rentalAllocationDataHandler.Update(rentalAllocationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                rentalAllocationDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the RentalAllocationDTO  values 
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
        public RentalAllocationDTO RentalAllocationDTO
        {
            get
            {
                return rentalAllocationDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of RentalAllocationDTO
    /// </summary>
    public class RentalAllocationListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<RentalAllocationDTO> rentalAllocationDTOList = new List<RentalAllocationDTO>();
        /// <summary>
        /// Parameterized constructor for RentalAllocationListBL
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public RentalAllocationListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for RentalAllocationListBL with rentalAllocationDTOList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="rentalAllocationDTOList">RentalAllocationDTO List object is passed as parameter</param>
        public RentalAllocationListBL(ExecutionContext executionContext,
                                                List<RentalAllocationDTO> rentalAllocationDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, rentalAllocationDTOList);
            this.rentalAllocationDTOList = rentalAllocationDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the RentalAllocationDTO List
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>Returns the List of RentalAllocationDTO List</returns>
        public List<RentalAllocationDTO> GetRentalAllocationDTOList(List<KeyValuePair<RentalAllocationDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RentalAllocationDataHandler rentalAllocationDataHandler = new RentalAllocationDataHandler(sqlTransaction);
            List<RentalAllocationDTO> rentalAllocationDTOList = rentalAllocationDataHandler.GetRentalAllocationDTOList(searchParameters);
            log.LogMethodExit(rentalAllocationDTOList);
            return rentalAllocationDTOList;
        }

        /// <summary>
        /// Saves the  List of RentalAllocationDTO objects
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (rentalAllocationDTOList == null ||
                rentalAllocationDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < rentalAllocationDTOList.Count; i++)
            {
                var rentalAllocationDTO = rentalAllocationDTOList[i];
                if (rentalAllocationDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    RentalAllocationBL rentalAllocationBL = new RentalAllocationBL(executionContext, rentalAllocationDTO);
                    rentalAllocationBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving RentalAllocationDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("RentalAllocationDTO", rentalAllocationDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
