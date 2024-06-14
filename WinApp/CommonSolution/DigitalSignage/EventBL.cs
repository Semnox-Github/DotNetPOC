/********************************************************************************************
 * Project Name - Event BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017      Lakshminarayana     Created
 *2.40        28-Sep-2018      Jagan Mohan         Added new constructor EventBL, EventListBL and 
 *                                                 methods SaveUpdateEventList and getEventType
 *2.70.2        31-Jul-2019      Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
  *2.90        28-Jul-2020      Mushahid Faizan     Modified : 3 tier changes for Rest API.
 *2.110.0     27-Nov-2020       Prajwal S          Modified : Constructor with Id parameter
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Business logic for Event class.
    /// </summary>
    public class EventBL
    {
        private EventDTO eventDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of EventBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private EventBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.eventDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the event id as the parameter
        /// Would fetch the event object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="executionContext">executionContext</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public EventBL(ExecutionContext executionContext, int id, SqlTransaction sqltransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqltransaction);
            EventDataHandler eventDataHandler = new EventDataHandler(sqltransaction);
            this.eventDTO = eventDataHandler.GetEventDTO(id);
            if (eventDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "event", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(eventDTO);
        }

        /// <summary>
        /// Creates EventBL object using the EventDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="eventDTO">eventDTO</param>
        public EventBL(ExecutionContext executionContext, EventDTO eventDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(eventDTO, eventDTO);
            this.eventDTO = eventDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Event
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqltransaction">sqltransaction</param>
        public void Save(SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(sqltransaction);
            EventDataHandler eventDataHandler = new EventDataHandler(sqltransaction);
            if (eventDTO.IsChanged == false && eventDTO.Id >-1)
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
            if (eventDTO.Id < 0)
            {
                eventDTO = eventDataHandler.InsertEvent(eventDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                eventDTO.AcceptChanges();
            }
            else
            {
                if (eventDTO.IsChanged)
                {
                    eventDTO = eventDataHandler.UpdateEvent(eventDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    eventDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (eventDTO == null)
            {
                //Validation to be implemented.
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Checks whether the query is valid. 
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public bool CheckQuery(string query, SqlTransaction sqlTransaction =null)
        {
            log.LogMethodEntry(query, sqlTransaction);
            EventDataHandler eventDataHandler = new EventDataHandler(sqlTransaction);
            log.LogMethodExit(eventDataHandler.CheckQuery(query));
            return eventDataHandler.CheckQuery(query);
        }

        /// <summary>
        /// Returns the whether dynamic event ocuured
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public bool GetEventData(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            bool returnValue = false;
            string lookupValue = string.Empty;
            if (eventDTO != null)
            {
                ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "EVENT_TYPE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> eventTypeLookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (eventTypeLookupValuesDTOList != null && eventTypeLookupValuesDTOList.Count > 0)
                {
                    foreach (LookupValuesDTO lookupValuesDTO in eventTypeLookupValuesDTOList)
                    {
                        if (lookupValuesDTO.LookupValueId == eventDTO.TypeId)
                        {
                            lookupValue = lookupValuesDTO.LookupValue;
                        }
                    }
                }

                if (string.Equals(lookupValue, "Query"))
                {
                    if (!string.IsNullOrEmpty(eventDTO.Parameter))
                    {
                        EventDataHandler eventDataHandler = new EventDataHandler(sqlTransaction);
                        returnValue = eventDataHandler.GetEventData(eventDTO.Parameter);
                    }
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public EventDTO EventDTO
        {
            get
            {
                return eventDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of Event
    /// </summary>
    public class EventListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<EventDTO> eventDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of EventListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public EventListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///  create the Event list object
        /// </summary>
        /// <param name="eventToList">eventToList</param>
        /// <param name="executionContext">executionContext</param>
        public EventListBL(ExecutionContext executionContext, List<EventDTO> eventDTOList) : this(executionContext)
        {
            log.LogMethodEntry(eventDTOList, executionContext);
            this.eventDTOList = eventDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Event list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>event List</returns>
        public List<EventDTO> GetEventDTOList(List<KeyValuePair<EventDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            EventDataHandler eventDataHandler = new EventDataHandler(sqlTransaction);
            this.eventDTOList = eventDataHandler.GetEventDTOList(searchParameters);
            log.LogMethodExit(eventDTOList);
            return eventDTOList;
        }

        /// <summary>
        /// Save and Update the event details
        /// </summary>
        public void Save()
        {
            try
            {
                log.LogMethodEntry();
                bool queryValid = true;
                if (eventDTOList != null && eventDTOList.Any())
                {
                    foreach (EventDTO eventDTO in eventDTOList)
                    {
                        EventBL eventBL = new EventBL(executionContext, eventDTO);
                        if (string.Equals(getEventType(eventDTO.TypeId), "Query"))
                        {
                            if (!string.IsNullOrEmpty(eventDTO.Parameter))
                            {
                                queryValid = eventBL.CheckQuery(eventDTO.Parameter);
                            }
                            else
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1831));
                            }
                        }
                        else if (string.Equals(getEventType(eventDTO.TypeId), "Timer"))
                        {
                            if (string.IsNullOrEmpty(eventDTO.Parameter) || string.IsNullOrWhiteSpace(eventDTO.Parameter))
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144));
                            }
                            else
                            {
                                int i;
                                if (!int.TryParse(eventDTO.Parameter, out i) || Convert.ToInt32(eventDTO.Parameter) <= 0)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1133));
                                }
                            }
                        }
                        if (queryValid == false)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1776));
                        }
                        eventBL.Save();
                    }
                }
                log.LogMethodExit();
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validate the event parameter based on the event type
        /// </summary>
        /// <param name="typeId">typeId</param>
        /// <returns>event type</returns>
        private string getEventType(int typeId)
        {
            log.LogMethodEntry(typeId);
            string type = string.Empty;

            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "EVENT_TYPE"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> eventTypeLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);

            if (typeId >= 0 && eventTypeLookUpValueList != null && eventTypeLookUpValueList.Count > 0)
            {
                foreach (LookupValuesDTO lookupValuesDTO in eventTypeLookUpValueList)
                {
                    if (lookupValuesDTO.LookupValueId == typeId)
                    {
                        type = lookupValuesDTO.LookupValue;
                        break;
                    }
                }
            }
            log.LogMethodExit(type);
            return type;
        }
    }
}
