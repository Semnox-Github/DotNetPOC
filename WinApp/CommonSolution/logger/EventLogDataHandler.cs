/********************************************************************************************
 * Project Name - EventLog Data Handler                                                                          
 * Description  - Data handler of the Eventlog class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        20-Dec-2015   Mathew              Created
 *2.70        16-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *2.80.0      20-Mar-2020   Akshay Gulaganji    Added DBSearchParameters, GetEventLogDTOList(), GetEventLogDTO()
 *2.90        26-May-2020   Vikas Dwivedi       Modified as per the Standard CheckList
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// Event Log data handler - Handles insert of eventLog data object
    /// </summary>
    public class EventLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;

        private const string SELECT_QUERY = @"SELECT * FROM EventLog AS el ";

        /// <summary>
        /// Dictionary for searching Parameters for the EventLog object.
        /// </summary>
        private static readonly Dictionary<EventLogDTO.SearchByEventLogParameters, string> DBSearchParameters = new Dictionary<EventLogDTO. SearchByEventLogParameters, string>
            {
                {EventLogDTO. SearchByEventLogParameters.EVENT_LOG_ID, "el.EventLogId"},
                {EventLogDTO. SearchByEventLogParameters.SOURCE, "el.Source"},
                {EventLogDTO. SearchByEventLogParameters.TYPE, "el.Type"},
                {EventLogDTO. SearchByEventLogParameters.USER_NAME, "el.Username"},
                {EventLogDTO. SearchByEventLogParameters.COMPUTER, "el.Computer"},
                {EventLogDTO. SearchByEventLogParameters.CATEGORY, "el.Category"},
                {EventLogDTO. SearchByEventLogParameters.TIMESTAMP, "el.Timestamp"},
                {EventLogDTO. SearchByEventLogParameters.ORDER_BY_TIMESTAMP, "el.Timestamp"},
                {EventLogDTO. SearchByEventLogParameters.SITE_ID, "el.site_id"},
                {EventLogDTO. SearchByEventLogParameters.MASTER_ENTITY_ID, "el.MasterentityId"}
         };

        /// <summary>
        /// Default constructor of MachineDataHandler class
        /// </summary>
        public EventLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        public EventLogDTO GetEventLogDTO(int id)
        {
            log.LogMethodEntry(id);
            string logquery = SELECT_QUERY + "WHERE eventLogId = @eventLogId";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@eventLogId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(logquery, parameters, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                EventLogDTO eventLogDTO = GetEventLogDTO(row);
                log.LogMethodExit(eventLogDTO);
                return eventLogDTO;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating eventLog Record.
        /// </summary>
        /// <param name="eventLogDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(EventLogDTO eventLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(eventLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@eventLogId", eventLogDTO.EventLogId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@source", eventLogDTO.Source));
            parameters.Add(dataAccessHandler.GetSQLParameter("@type", eventLogDTO.Type));
            parameters.Add(new SqlParameter("@username", string.IsNullOrEmpty(loginId) ? string.Empty : (object)loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@computer", eventLogDTO.Computer));
            parameters.Add(dataAccessHandler.GetSQLParameter("@data", eventLogDTO.Data));
            parameters.Add(new SqlParameter("@description", string.IsNullOrEmpty(eventLogDTO.Description) ? String.Empty : (object)eventLogDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@category", eventLogDTO.Category));
            parameters.Add(dataAccessHandler.GetSQLParameter("@severity", eventLogDTO.Severity, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", eventLogDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@value", eventLogDTO.EventValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", eventLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the eventLog data
        /// </summary>
        /// <returns>Returns inserted record id</returns>
        public EventLogDTO InsertEventLog(string source, string type, string username, string computer,
                                  string data, string description, string category, int severity, string name,
                                  string eventValue, bool synchStatus, string userId, int siteId)
        {
            log.LogMethodEntry();
            EventLogDTO eventLogDTO = new EventLogDTO(-1, source, DateTime.Now, type, username, computer,
                                   data, description, category, severity, name,
                                  eventValue, "", siteId, false);

            eventLogDTO = InsertEventLog(eventLogDTO, userId, siteId);
            log.LogMethodExit(eventLogDTO);
            return eventLogDTO;
        }

        /// <summary>
        /// Gets the EventLogDTO List based on searchByEventLogParameters
        /// </summary>
        /// <param name="searchByEventLogParameters"></param>
        /// <returns></returns>
        public List<EventLogDTO> GetEventLogDTOList(List<KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>> searchByEventLogParameters)
        {
            log.LogMethodEntry(searchByEventLogParameters);
            List<EventLogDTO> eventLogDTOList = new List<EventLogDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchByEventLogParameters != null) && (searchByEventLogParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<EventLogDTO.SearchByEventLogParameters, string> searchParameter in searchByEventLogParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == EventLogDTO.SearchByEventLogParameters.EVENT_LOG_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == EventLogDTO.SearchByEventLogParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == EventLogDTO.SearchByEventLogParameters.SOURCE ||
                                 searchParameter.Key == EventLogDTO.SearchByEventLogParameters.TYPE ||
                                 searchParameter.Key == EventLogDTO.SearchByEventLogParameters.USER_NAME ||
                                 searchParameter.Key == EventLogDTO.SearchByEventLogParameters.CATEGORY ||
                                 searchParameter.Key == EventLogDTO.SearchByEventLogParameters.COMPUTER)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query + " ORDER BY TimeStamp DESC";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    EventLogDTO eventLogDTO = GetEventLogDTO(dataRow);
                    eventLogDTOList.Add(eventLogDTO);
                }
            }
            log.LogMethodExit(eventLogDTOList);
            return eventLogDTOList;
        }

        /// <summary>
        /// Converts the Data row object to EventLogDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>EventLogDTO</returns>
        private EventLogDTO GetEventLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            EventLogDTO eventLogDTO = new EventLogDTO(dataRow["EventLogId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["EventLogId"]),
                                                         dataRow["Source"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Source"]),
                                                         dataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Timestamp"]),
                                                         dataRow["Type"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Type"]),
                                                         dataRow["Username"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Username"]),
                                                         dataRow["Computer"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Computer"]),
                                                         dataRow["Data"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Data"]),
                                                         dataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Description"]),
                                                         dataRow["Category"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Category"]),
                                                         dataRow["Severity"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Severity"]),
                                                         dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                                         dataRow["Value"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Value"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                                        );
            log.LogMethodExit(eventLogDTO);
            return eventLogDTO;
        }

        /// <summary>
        /// Inserts the eventLog data
        /// </summary>
        /// <param name="eventLogDTO">EventLogDTO object</param>        
        /// <param name="loginId">UserId in application</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public EventLogDTO InsertEventLog(EventLogDTO eventLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(eventLogDTO, loginId, siteId);
            string insertEventLogQuery = @"INSERT INTO[dbo].[EventLog]  
                                                               (Source,
                                                                Timestamp,
                                                                Type,
                                                                Username, 
                                                                Computer, 
                                                                Data,
                                                                Description, 
                                                                Category,
                                                                Severity, 
                                                                Name, 
                                                                Value, 
                                                                site_id,
                                                                Guid,
                                                                LastUpdatedBy,
                                                                LastUpdateDate,
                                                                CreatedBy,
                                                                CreationDate,
                                                                MasterEntityId) 
                                                        values (@source, 
                                                                getdate(), 
                                                                @type, 
                                                                @username,
                                                                @computer,
                                                                @data,
                                                                @description, 
                                                                @category,
                                                                @severity,
                                                                @name, 
                                                                @value,
                                                                @siteId,
                                                                newid(),
                                                                @lastUpdatedBy,
                                                                getdate(),
                                                                @createdBy,
                                                                getdate(),
                                                                @masterEntityId
                                                                ) 
                                                                SELECT * FROM EventLog WHERE EventLogId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertEventLogQuery, GetSQLParameters(eventLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorAssetType(eventLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting EventLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(eventLogDTO);
            return eventLogDTO;
        }

        ///// <summary>
        ///// Updates the EventLog
        ///// </summary>
        ///// <param name="eventLogDTO"></param>
        ///// <param name="loginId"></param>
        ///// <param name="siteId"></param>
        ///// <returns></returns>
        //public EventLogDTO UpdateEventLog(EventLogDTO eventLogDTO, string loginId, int siteId)
        //{
        //    log.LogMethodEntry(eventLogDTO, loginId, siteId);
        //    string updateEventLogQuery = @"UPDATE EventLog
        //                                                SET [Source] = @source
        //                                                    ,[Timestamp] = getdate()
        //                                                    ,[Type] = @type
        //                                                    ,[Username] = @username
        //                                                    ,[Computer] = @computer
        //                                                    ,[Data] = @data
        //                                                    ,[Description] = @description
        //                                                    ,[Category] = @category
        //                                                    ,[Severity] = @severity
        //                                                    ,[Name] = @name
        //                                                    ,[Value] = @value
        //                                                    ,[site_id] = @siteid
        //                                                    ,[guid] = newid()
        //                                                    ,[MasterEntityId] = @masterEntityId
        //                                                    ,[CreatedBy] = @createdBy
        //                                                    ,[CreationDate] = getdate()
        //                                                    ,[LastUpdatedBy] = @lastUpdatedBy
        //                                                    ,[LastUpdateDate] = getdate()
        //                                                WHERE EventLogId=@eventLogId                                                                   
        //                                                SELECT * FROM EventLog WHERE EventLogId  = @eventLogId";
        //    try
        //    {
        //        DataTable dt = dataAccessHandler.executeSelectQuery(updateEventLogQuery, GetSQLParameters(eventLogDTO, loginId, siteId).ToArray(), sqlTransaction);
        //        RefreshMonitorAssetType(eventLogDTO, dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occurred while inserting EventLogDTO", ex);
        //        log.LogMethodExit(null, "Throwing exception - " + ex.Message);
        //        throw;
        //    }
        //    log.LogMethodExit(eventLogDTO);
        //    return eventLogDTO;
        //}
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="monitorAssetTypeDTO"></param>
        /// <param name="dt">dt</param>
        private void RefreshMonitorAssetType(EventLogDTO eventLogDTO, DataTable dt)
        {
            log.LogMethodEntry(eventLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                eventLogDTO.EventLogId = Convert.ToInt32(dt.Rows[0]["EventLogId"]);
                eventLogDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                eventLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                eventLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                eventLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                eventLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                eventLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        ///<summary>
        /// This method is used to filter the eventLogs based on search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public string GetEventLogFilterQuery(List<KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                foreach (KeyValuePair<EventLogDTO.SearchByEventLogParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? " " : " and ";
                        if (searchParameter.Key.Equals(EventLogDTO.SearchByEventLogParameters.EVENT_LOG_ID) || searchParameter.Key.Equals(EventLogDTO.SearchByEventLogParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value);
                        }
                        else if (searchParameter.Key == EventLogDTO.SearchByEventLogParameters.SOURCE || searchParameter.Key == EventLogDTO.SearchByEventLogParameters.TYPE ||
                                 searchParameter.Key == EventLogDTO.SearchByEventLogParameters.USER_NAME || searchParameter.Key == EventLogDTO.SearchByEventLogParameters.CATEGORY ||
                                 searchParameter.Key == EventLogDTO.SearchByEventLogParameters.COMPUTER)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        }
                        else if (searchParameter.Key == EventLogDTO.SearchByEventLogParameters.SITE_ID)
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " = -1)");
                        }
                        else if (searchParameter.Key == EventLogDTO.SearchByEventLogParameters.TIMESTAMP)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " > " + dataAccessHandler.GetParameterName(searchParameter.Key));
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit();
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
            }
            log.LogMethodExit(query);
            return query.ToString();
        }

        /// <summary>
        /// Gets the EventLogDTO list matching the search Parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="currentPage">current page number</param>
        /// <param name="pageSize">page size</param>
        /// <returns>Returns the list of EventLogDTO matching the searchParameters</returns>
        public List<EventLogDTO> GetEventLogDTOList(List<KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>> searchParameters, int currentPage, int pageSize)
        {
            log.LogMethodEntry(searchParameters, currentPage, pageSize);
            List<EventLogDTO> eventLogDTOList = null;

            string selectQuery = SELECT_QUERY;
            selectQuery += GetEventLogFilterQuery(searchParameters);
            selectQuery += " ORDER BY el.Timestamp desc OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
            selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                eventLogDTOList = new List<EventLogDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    EventLogDTO eventLogDTO = GetEventLogDTO(dataRow);
                    eventLogDTOList.Add(eventLogDTO);
                }
            }
            log.LogMethodExit(eventLogDTOList);
            return eventLogDTOList;
        }

        /// <summary>
        /// Returns the no of eventLogsCount matching the search Parameters
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>no of eventLogsCount matching the searchParameters</returns>
        public int GetEventLogsCount(List<KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            int eventLogsCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetEventLogFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                eventLogsCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(eventLogsCount);
            return eventLogsCount;
        }
    }
}
