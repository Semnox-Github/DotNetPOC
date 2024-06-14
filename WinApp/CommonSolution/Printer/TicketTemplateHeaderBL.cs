/********************************************************************************************
 * Project Name - Printer
 * Description  - Business logic file for  TicketTemplateHeader
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      24-Jun-2019     Girish Kundar           Created 
 *2.70.3     2-Apr-2019     Girish Kundar           Modified : Build() method 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// Business logic for TicketTemplateHeader class.
    /// </summary>
    public class TicketTemplateHeaderBL
    {
        private TicketTemplateHeaderDTO ticketTemplateHeaderDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of TicketTemplateHeaderBL class
        /// </summary>
        private TicketTemplateHeaderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates TicketTemplateHeaderBL object using the TicketTemplateHeaderDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="ticketTemplateHeaderDTO">TicketTemplateHeaderDTO object</param>
        public TicketTemplateHeaderBL(ExecutionContext executionContext, TicketTemplateHeaderDTO ticketTemplateHeaderDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, ticketTemplateHeaderDTO);
            this.ticketTemplateHeaderDTO = ticketTemplateHeaderDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the TicketTemplateHeader id as the parameter
        /// Would fetch the TicketTemplateHeader object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id - TicketTemplateHeader </param>
        /// <param name="loadChildRecords">loadChildRecords either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public TicketTemplateHeaderBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TicketTemplateHeaderDataHandler ticketTemplateHeaderDataHandler = new TicketTemplateHeaderDataHandler(sqlTransaction);
            ticketTemplateHeaderDTO = ticketTemplateHeaderDataHandler.GetTicketTemplateHeaderDTO(id);
            if (ticketTemplateHeaderDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TicketTemplateHeaderDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the child records for TicketTemplateHeaderDTO object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            TicketTemplateElementListBL ticketTemplateElementListBL = new TicketTemplateElementListBL(executionContext);
            List<KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>> searchParameters = new List<KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>>();
            searchParameters.Add(new KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>(TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.TICKET_TEMPLATE_ID, ticketTemplateHeaderDTO.TicketTemplateId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>(TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.ACTIVE_FLAG, "1"));
            }
            ticketTemplateHeaderDTO.TicketTemplateElementDTOList = ticketTemplateElementListBL.GetTicketTemplateElementDTOList(searchParameters, sqlTransaction);
        }


        /// <summary>
        /// Saves the TicketTemplateHeaderDTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            if (ticketTemplateHeaderDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            TicketTemplateHeaderDataHandler ticketTemplateHeaderDataHandler = new TicketTemplateHeaderDataHandler(sqlTransaction);
            if (ticketTemplateHeaderDTO.ActiveFlag == true)
            {
                if (ticketTemplateHeaderDTO.TicketTemplateId < 0)
                {
                    log.LogVariableState("TicketTemplateHeaderDTO", ticketTemplateHeaderDTO);
                    ticketTemplateHeaderDTO = ticketTemplateHeaderDataHandler.Insert(ticketTemplateHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    ticketTemplateHeaderDTO.AcceptChanges();
                }
                else if (ticketTemplateHeaderDTO.IsChanged)
                {
                    log.LogVariableState("TicketTemplateHeaderDTO", ticketTemplateHeaderDTO);
                    ticketTemplateHeaderDTO = ticketTemplateHeaderDataHandler.Update(ticketTemplateHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    ticketTemplateHeaderDTO.AcceptChanges();
                }
                SaveTicketTemplateElements(sqlTransaction);
            }
            else  // Hard Delete only for the existing Files . For new file use Soft Delete method
            {
                //if (ticketTemplateHeaderDTO.TicketTemplateElementDTOList.Any(x => x.ActiveFlag == true))
                //{
                //    string message = MessageContainerList.GetMessage(executionContext, 1143);
                //    log.LogMethodExit(null, "Throwing Exception - " + message);
                //    throw new ForeignKeyException(message);
                //}
                log.LogVariableState("TicketTemplateHeaderDTO", ticketTemplateHeaderDTO);
                DeleteTicketTemplateElements(sqlTransaction);
                if (ticketTemplateHeaderDTO.TicketTemplateId >= 0)
                {
                    ticketTemplateHeaderDataHandler.Delete(ticketTemplateHeaderDTO);
                }
                ticketTemplateHeaderDTO.AcceptChanges();
            }
        }

        private void DeleteTicketTemplateElements(SqlTransaction sqlTransaction)
        {

            // For child records :TicketTemplateElementDTO
            if (ticketTemplateHeaderDTO.TicketTemplateElementDTOList != null &&
                ticketTemplateHeaderDTO.TicketTemplateElementDTOList.Any())
            {
                List<TicketTemplateElementDTO> updatedTicketTemplateElementDTOList = new List<TicketTemplateElementDTO>();
                foreach (var ticketTemplateElementDTO in ticketTemplateHeaderDTO.TicketTemplateElementDTOList)
                {
                    if (ticketTemplateElementDTO.TicketTemplateId == ticketTemplateHeaderDTO.TicketTemplateId)
                    {
                        ticketTemplateElementDTO.ActiveFlag = false;
                        updatedTicketTemplateElementDTOList.Add(ticketTemplateElementDTO);
                    }
                }
                if (updatedTicketTemplateElementDTOList.Any())
                {
                    log.LogVariableState("UpdatedTicketTemplateElementDTOList", updatedTicketTemplateElementDTOList);
                    TicketTemplateElementListBL ticketTemplateElementListBL = new TicketTemplateElementListBL(executionContext, updatedTicketTemplateElementDTOList);
                    ticketTemplateElementListBL.Save(sqlTransaction);
                }
            }
            else
            {
                log.Debug(" ticketTemplateHeaderDTO.TicketTemplateElementDTOList");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : TicketTemplateELements List 
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        private void SaveTicketTemplateElements(SqlTransaction sqlTransaction)
        {

            // For child records :TicketTemplateElementDTO
            if (ticketTemplateHeaderDTO.TicketTemplateElementDTOList != null &&
                ticketTemplateHeaderDTO.TicketTemplateElementDTOList.Any())
            {
                List<TicketTemplateElementDTO> updatedTicketTemplateElementDTOList = new List<TicketTemplateElementDTO>();
                foreach (var ticketTemplateElementDTO in ticketTemplateHeaderDTO.TicketTemplateElementDTOList)
                {
                    if (ticketTemplateElementDTO.TicketTemplateId != ticketTemplateHeaderDTO.TicketTemplateId)
                    {
                        ticketTemplateElementDTO.TicketTemplateId = ticketTemplateHeaderDTO.TicketTemplateId;
                    }
                    if (ticketTemplateElementDTO.IsChanged)
                    {
                        updatedTicketTemplateElementDTOList.Add(ticketTemplateElementDTO);
                    }
                }
                if (updatedTicketTemplateElementDTOList.Any())
                {
                    log.LogVariableState("UpdatedTicketTemplateElementDTOList", updatedTicketTemplateElementDTOList);
                    TicketTemplateElementListBL ticketTemplateElementListBL = new TicketTemplateElementListBL(executionContext, updatedTicketTemplateElementDTOList);
                    ticketTemplateElementListBL.Save(sqlTransaction);
                }
            }
            else
            {
                log.Debug(" ticketTemplateHeaderDTO.TicketTemplateElementDTOList");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the TicketTemplateHeaderDTO and TicketTemplateElementDTOList 
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.

            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation if required for TicketTemplateHeader do here.

            //calling validation for child : TicketTemplateElementDTOList
            if (ticketTemplateHeaderDTO.TicketTemplateElementDTOList != null)
            {
                foreach (var ticketTemplateElementDTO in ticketTemplateHeaderDTO.TicketTemplateElementDTOList)
                {
                    if (ticketTemplateElementDTO.IsChanged)
                    {
                        log.LogVariableState("TicketTemplateElementDTO", ticketTemplateElementDTO);
                        TicketTemplateElementBL ticketTemplateElementBL = new TicketTemplateElementBL(executionContext, ticketTemplateElementDTO);
                        validationErrorList.AddRange(ticketTemplateElementBL.Validate(sqlTransaction));
                    }
                }
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TicketTemplateHeaderDTO TicketTemplateHeaderDTO
        {
            get
            {
                return ticketTemplateHeaderDTO;
            }
        }


    }
    /// <summary>
    /// Manages the list of TicketTemplateHeader
    /// </summary>
    public class TicketTemplateHeaderListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<TicketTemplateHeaderDTO> ticketTemplateHeaderDTOList = new List<TicketTemplateHeaderDTO>();

        /// <summary>
        /// Parameterized constructor for TicketTemplateHeaderListBL
        /// </summary>
        /// <param name="executionContext">ExecutionContext object as parameter</param>
        public TicketTemplateHeaderListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor for TicketTemplateHeaderListBL
        /// </summary>
        /// <param name="executionContext">ExecutionContext object as parameter</param>
        /// <param name="ticketTemplateHeaderDTOList">TicketTemplateHeaderDTO List object as parameter</param>
        public TicketTemplateHeaderListBL(ExecutionContext executionContext,
                                             List<TicketTemplateHeaderDTO> ticketTemplateHeaderDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, ticketTemplateHeaderDTOList);
            this.ticketTemplateHeaderDTOList = ticketTemplateHeaderDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the TicketTemplateHeaderDTO List based on the search Parameters
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <returns>returns the TicketTemplateHeaderDTO List</returns>
        public List<TicketTemplateHeaderDTO> GetTicketTemplateHeaderDTOList(List<KeyValuePair<TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters, string>> searchParameters,
                                                                       bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TicketTemplateHeaderDataHandler ticketTemplateHeaderDataHandler = new TicketTemplateHeaderDataHandler(sqlTransaction);
            List<TicketTemplateHeaderDTO> ticketTemplateHeaderDTOList = ticketTemplateHeaderDataHandler.GetTicketTemplateHeaderDTOList(searchParameters);
            if (loadChildRecords && ticketTemplateHeaderDTOList.Any())
            {
                Build(ticketTemplateHeaderDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(ticketTemplateHeaderDTOList);
            return ticketTemplateHeaderDTOList;
        }

        /// <summary>
        /// Builds the List of TicketTemplateHeaderDTO objects based on the list of TicketTemplateHeader id.
        /// </summary>
        /// <param name="ticketTemplateHeaderDTOList">TicketTemplateHeaderDTO List</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        private void Build(List<TicketTemplateHeaderDTO> ticketTemplateHeaderDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ticketTemplateHeaderDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, TicketTemplateHeaderDTO> ticketTemplateHeaderIdElementIdDictionary = new Dictionary<int, TicketTemplateHeaderDTO>();
            string ticketTemplateHeaderIdSet=string.Empty;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < ticketTemplateHeaderDTOList.Count; i++)
            {
                if (ticketTemplateHeaderDTOList[i].TicketTemplateId == -1 || ticketTemplateHeaderIdElementIdDictionary.ContainsKey(ticketTemplateHeaderDTOList[i].TicketTemplateId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(ticketTemplateHeaderDTOList[i].TicketTemplateId);
                ticketTemplateHeaderIdElementIdDictionary.Add(ticketTemplateHeaderDTOList[i].TicketTemplateId, ticketTemplateHeaderDTOList[i]);
            }
            ticketTemplateHeaderIdSet = sb.ToString();
            TicketTemplateElementListBL ticketTemplateElementListBL = new TicketTemplateElementListBL(executionContext);
            List<KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>> searchParam = new List<KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>>();
            searchParam.Add(new KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>(TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParam.Add(new KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>(TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.TICKET_TEMPLATE_ID_LIST, ticketTemplateHeaderIdSet.ToString()));
            if(activeChildRecords)
            {
                searchParam.Add(new KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>(TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.ACTIVE_FLAG, "1"));
            }
            List<TicketTemplateElementDTO> ticketTemplateElementDTOList = ticketTemplateElementListBL.GetTicketTemplateElementDTOList(searchParam,sqlTransaction);
            if (ticketTemplateElementDTOList.Any())
            {
                log.LogVariableState("TicketTemplateElementDTOList", ticketTemplateElementDTOList);
                foreach (var ticketTemplateElementDTO in ticketTemplateElementDTOList)
                {
                    if (ticketTemplateHeaderIdElementIdDictionary.ContainsKey(ticketTemplateElementDTO.TicketTemplateId))
                    {
                        if (ticketTemplateHeaderIdElementIdDictionary[ticketTemplateElementDTO.TicketTemplateId].TicketTemplateElementDTOList == null)
                        {
                            ticketTemplateHeaderIdElementIdDictionary[ticketTemplateElementDTO.TicketTemplateId].TicketTemplateElementDTOList = new List<TicketTemplateElementDTO>();
                        }
                        ticketTemplateHeaderIdElementIdDictionary[ticketTemplateElementDTO.TicketTemplateId].TicketTemplateElementDTOList.Add(ticketTemplateElementDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TicketTemplateHeader DTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (ticketTemplateHeaderDTOList == null ||
                ticketTemplateHeaderDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < ticketTemplateHeaderDTOList.Count; i++)
            {
                var ticketTemplateHeaderDTO = ticketTemplateHeaderDTOList[i];
                if (ticketTemplateHeaderDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    TicketTemplateHeaderBL ticketTemplateHeaderBL = new TicketTemplateHeaderBL(executionContext, ticketTemplateHeaderDTO);
                    ticketTemplateHeaderBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving TicketTemplateHeaderDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("TicketTemplateHeaderDTO", ticketTemplateHeaderDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
