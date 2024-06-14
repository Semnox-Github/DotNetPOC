/********************************************************************************************
 * Project Name - Event Log
 * Description  - Logical grouping of event log 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        08-Feb-2016   Raghuveera          Created 
 *2.70        16-Jul-2019   Dakshakh raj        Modified :
 *2.80.0      20-Mar-2020   Akshay Gulaganji    Added GetEventLogDTOList() Modified : constructor, Save()
 *2.90        08-Jul-2020   Vikas Dwivedi       Modified as per Review Comments
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// Bussiness logic for event log
    /// </summary>
    public class EventLog
    {
        private EventLogDTO eventLogDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        private EventLog(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates EventLog object using the eventLogDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="eventLogDTO">eventLogDTO object is passed as parameter</param>
        public EventLog(ExecutionContext executionContext, EventLogDTO eventLogDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, eventLogDTO);
            this.eventLogDTO = eventLogDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the EventLog id as the parameter
        /// Would fetch the EventLog object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="eventLogId">id of EventLog Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public EventLog(ExecutionContext executionContext, int eventLogId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, eventLogId, sqlTransaction);
            EventLogDataHandler eventLogDataHandler = new EventLogDataHandler(sqlTransaction);
            this.eventLogDTO = eventLogDataHandler.GetEventLogDTO(eventLogId);
            if (eventLogDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "EventLogDTO", eventLogId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the EventLogDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            return validationErrorList;
            // Validation Logic here 
        }

        /// <summary>
        /// Saves the event log
        /// Checks if the eventLogId id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            EventLogDataHandler eventLogDataHandler = new EventLogDataHandler(sqlTransaction);
            if (eventLogDTO.EventLogId <= 0)
            {
                eventLogDTO = eventLogDataHandler.InsertEventLog(eventLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                eventLogDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Create Logs
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="source">source</param>
        /// <param name="type">type</param>
        /// <param name="computer">computer</param>
        /// <param name="data">data</param>
        /// <param name="description">description</param>
        /// <param name="category">category</param>
        /// <param name="severity">severity</param>
        /// <param name="name">name</param>
        /// <param name="eventValue">eventValue</param>
        public static void Log(ExecutionContext executionContext, string source, string type, string computer,
                               string data, string description, string category, int severity, string name, string eventValue)
        {
            log.LogMethodEntry(executionContext, source,  type, computer,
                                data, description, category, severity, name, eventValue);
            try
            {
                EventLogDTO eventLogDTO = new EventLogDTO(-1, source, DateTime.Now, type, executionContext.GetUserId(), computer, data, description, category, severity, name, eventValue, string.Empty, -1, false);
                EventLog eventLog = new EventLog(executionContext, eventLogDTO);
                eventLog.Save();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Logging", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get EventLog DTO
        /// </summary>
        public EventLogDTO GetEventLogDTO
        {
            get { return eventLogDTO; }
        }
    }

    public class EventLogList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<EventLogDTO> eventLogDTOList = new List<EventLogDTO>();

        /// <summary>
        /// Parameterized Constructor with ExecutionContext Parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public EventLogList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="eventLogDTOList">eventLogDTOList</param>
        public EventLogList(ExecutionContext executionContext, List<EventLogDTO> eventLogDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, eventLogDTOList);
            this.eventLogDTOList = eventLogDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of EventLog
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of EventLog List</returns>
        public List<EventLogDTO> GetEventLogDTOList(List<KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>> searchParameters, int currentPage, int pageSize, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            EventLogDataHandler eventLogDataHandler = new EventLogDataHandler(sqlTransaction);
            List<EventLogDTO> eventLogDTOList = eventLogDataHandler.GetEventLogDTOList(searchParameters, currentPage, pageSize);
            log.LogMethodExit(eventLogDTOList);
            return eventLogDTOList;
        }

        /// <summary>
        /// Returns the no of eventLogsCount matching the searchParameters
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns></returns>
        public int GetEventLogsCount(List<KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            EventLogDataHandler eventLogDataHandler = new EventLogDataHandler(sqlTransaction);
            int eventLogsCount = eventLogDataHandler.GetEventLogsCount(searchParameters, sqlTransaction);
            return eventLogsCount;
        }

        internal List<EventLogDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (eventLogDTOList == null || eventLogDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return null;
            }
            List<EventLogDTO> result = new List<EventLogDTO>();
            for (int i = 0; i < eventLogDTOList.Count; i++)
            {
                var eventLogDTO = eventLogDTOList[i];
                if (eventLogDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    EventLog eventLog = new EventLog(executionContext, eventLogDTO);
                    eventLog.Save(sqlTransaction);
                    result.Add(eventLog.GetEventLogDTO);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving eventLogDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("eventLogDTO", eventLogDTO);
                    throw;
                }
            }
            log.LogMethodExit();
            return result;
        }

    }
}
