/********************************************************************************************
 * Project Name - Printer
 * Description  - Business logic file for  TicketTemplateElement
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      24-Jun-2019     Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// Business logic for TicketTemplateElement class.
    /// </summary>
    public class TicketTemplateElementBL
    {
        private TicketTemplateElementDTO ticketTemplateElementDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of TicketTemplateElementBL class
        /// </summary>
        private TicketTemplateElementBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates TicketTemplateElementBL object using the TicketTemplateElementDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="ticketTemplateElementDTO">TicketTemplateElement DTO object</param>
        public TicketTemplateElementBL(ExecutionContext executionContext, TicketTemplateElementDTO ticketTemplateElementDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, ticketTemplateElementDTO);
            this.ticketTemplateElementDTO = ticketTemplateElementDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the TicketTemplateElement  id as the parameter
        /// Would fetch the TicketTemplateElement object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id -TicketTemplateElement </param>
        /// <param name="loadChildRecords">loadChildRecords either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public TicketTemplateElementBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TicketTemplateElementDataHandler ticketTemplateElementDataHandler = new TicketTemplateElementDataHandler(sqlTransaction);
            ticketTemplateElementDTO = ticketTemplateElementDataHandler.GetTicketTemplateElementDTO(id);
            if (ticketTemplateElementDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TicketTemplateElementDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TicketTemplateElement DTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (ticketTemplateElementDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            TicketTemplateElementDataHandler ticketTemplateElementDataHandler = new TicketTemplateElementDataHandler(sqlTransaction);
            if (ticketTemplateElementDTO.ActiveFlag == true)
            {
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (ticketTemplateElementDTO.TicketTemplateElementId < 0)
                {
                    log.LogVariableState("TicketTemplateElementDTO", ticketTemplateElementDTO);
                    ticketTemplateElementDTO = ticketTemplateElementDataHandler.Insert(ticketTemplateElementDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    ticketTemplateElementDTO.AcceptChanges();
                }
                else if (ticketTemplateElementDTO.IsChanged)
                {
                    log.LogVariableState("TicketTemplateElementDTO", ticketTemplateElementDTO);
                    ticketTemplateElementDTO = ticketTemplateElementDataHandler.Update(ticketTemplateElementDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    ticketTemplateElementDTO.AcceptChanges();
                }

            }

            else  // For Hard delete : Only for the existing BL , Not for New BL  
            {

                if (ticketTemplateElementDTO.TicketTemplateElementId >= 0)
                {
                    ticketTemplateElementDataHandler.Delete(ticketTemplateElementDTO);
                }
                ticketTemplateElementDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the TicketTemplateElementDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.
            // Validation do here
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TicketTemplateElementDTO TicketTemplateElementDTO
        {
            get
            {
                return ticketTemplateElementDTO;
            }
        }
    }
    /// <summary>
    /// Manages the list of TicketTemplateElement
    /// </summary>
    public class TicketTemplateElementListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<TicketTemplateElementDTO> ticketTemplateElementDTOList = new List<TicketTemplateElementDTO>(); // To be initialized
        /// <summary>
        /// Parameterized constructor of TicketTemplateElementListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public TicketTemplateElementListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor of TicketTemplateElementListBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="ticketTemplateElementDTOList">TicketTemplateElement DTO List as parameter </param>
        public TicketTemplateElementListBL(ExecutionContext executionContext,
                                              List<TicketTemplateElementDTO> ticketTemplateElementDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, ticketTemplateElementDTOList);
            this.ticketTemplateElementDTOList = ticketTemplateElementDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the TicketTemplateElement DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of TicketTemplateElementDTO </returns>
        public List<TicketTemplateElementDTO> GetTicketTemplateElementDTOList(List<KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>> searchParameters,
                                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TicketTemplateElementDataHandler ticketTemplateElementDataHandler = new TicketTemplateElementDataHandler(sqlTransaction);
            List<TicketTemplateElementDTO> ticketTemplateElementDTOList = ticketTemplateElementDataHandler.GetTicketTemplateElementDTOList(searchParameters);
            log.LogMethodExit(ticketTemplateElementDTOList);
            return ticketTemplateElementDTOList;
        }

        /// <summary>
        /// Saves the  list of TicketTemplateElement DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (ticketTemplateElementDTOList == null ||
                ticketTemplateElementDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < ticketTemplateElementDTOList.Count; i++)
            {
                var ticketTemplateElementDTO = ticketTemplateElementDTOList[i];
                if (ticketTemplateElementDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    TicketTemplateElementBL ticketTemplateElementBL = new TicketTemplateElementBL(executionContext, ticketTemplateElementDTO);
                    ticketTemplateElementBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving TicketTemplateElementDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("TicketTemplateElementDTO", ticketTemplateElementDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
