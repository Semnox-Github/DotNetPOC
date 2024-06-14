/********************************************************************************************
 * Project Name - Event Data Handler
 * Description  - Data handler of the Event class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017   Lakshminarayana     Created 
 *2.70.2        30-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2       06-Dec-2019   Jinto Thomas         Removed siteid from update query                                                          
 *2.90        28-Jul-2020   Mushahid Faizan     Modified : default isActive value to true.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    ///  Event Data Handler - Handles insert, update and select of  Event objects
    /// </summary>
    public class EventDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Event AS ev";

        /// <summary>
        /// Dictionary for searching Parameters for the Event object.
        /// </summary>
        private static readonly Dictionary<EventDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<EventDTO.SearchByParameters, string>
            {
                {EventDTO.SearchByParameters.ID, "ev.Id"},
                {EventDTO.SearchByParameters.TYPE_ID, "ev.TypeId"},
                {EventDTO.SearchByParameters.NAME, "ev.Name"},
                {EventDTO.SearchByParameters.IS_ACTIVE, "ev.IsActive"},
                {EventDTO.SearchByParameters.MASTER_ENTITY_ID,"ev.MasterEntityId"},
                {EventDTO.SearchByParameters.SITE_ID, "ev.site_id"}
            };

        /// <summary>
        /// Default constructor of EventDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public EventDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating EventDTO parameters Record.
        /// </summary>
        /// <param name="eventDTO">eventDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(EventDTO eventDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(eventDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", eventDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", string.IsNullOrEmpty(eventDTO.Name) ? DBNull.Value : (object)eventDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TypeId", eventDTO.TypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Parameter", string.IsNullOrEmpty(eventDTO.Parameter) ? DBNull.Value : (object)eventDTO.Parameter));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", string.IsNullOrEmpty(eventDTO.Description) ? DBNull.Value : (object)eventDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (eventDTO.IsActive == true? "Y":"N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", eventDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Event record to the database
        /// </summary>
        /// <param name="eventDTO">EventDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Event DTO</returns>
        public EventDTO InsertEvent(EventDTO eventDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(eventDTO, loginId, siteId);
            string query = @"INSERT INTO Event 
                                        ( 
                                            Name,
                                            TypeId,
                                            Parameter,
                                            Description,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            site_id,
                                            MasterEntityId,
                                            Guid
                                        ) 
                                VALUES 
                                        (
                                            @Name,
                                            @TypeId,
                                            @Parameter,
                                            @Description,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId,
                                            NEWID()
                                        )SELECT * FROM Event WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(eventDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshEventDTO(eventDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting eventDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(eventDTO);
            return eventDTO;
        }

        /// <summary>
        /// Updates the Event record
        /// </summary>
        /// <param name="eventDTO">EventDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Event DTO</returns>
        public EventDTO UpdateEvent(EventDTO eventDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(eventDTO, loginId, siteId);
            string query = @"UPDATE Event 
                             SET Name=@Name,
                                 TypeId=@TypeId,
                                 Parameter=@Parameter,
                                 Description=@Description,
                                 IsActive=@IsActive,
                                 LastUpdatedBy=@LastUpdatedBy,
                                 LastUpdatedDate = GETDATE()
                                 --site_id=@site_id
                             WHERE Id = @Id
                             SELECT* FROM Event WHERE Id = @Id";
            try
            {
                if (string.Equals(eventDTO.IsActive, "N") && GetEventReferenceCount(eventDTO.Id) > 0)
                {
                    throw new ForeignKeyException("Cannot Inactivate records for which matching detail data exists.");
                }
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(eventDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshEventDTO(eventDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating eventDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(eventDTO);
            return eventDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="eventDTO">eventDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshEventDTO(EventDTO eventDTO, DataTable dt)
        {
            log.LogMethodEntry(eventDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                eventDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                eventDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                eventDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                eventDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                eventDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                eventDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                eventDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks whether event is in use.
        /// <param name="id">Event Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        private int GetEventReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT Count(*) as ReferenceCount
                             FROM ScreenTransitions
                             WHERE EventId = @EventId AND IsActive = 'Y'";
            SqlParameter parameter = new SqlParameter("@EventId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }

        /// <summary>
        /// Converts the Data row object to EventDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns EventDTO</returns>
        private EventDTO GetEventDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            EventDTO eventDTO = new EventDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["Name"] == DBNull.Value ? "" : dataRow["Name"].ToString(),
                                            dataRow["TypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TypeId"]),
                                            dataRow["Parameter"] == DBNull.Value ? "" : dataRow["Parameter"].ToString(),
                                            dataRow["Description"] == DBNull.Value ? "" : dataRow["Description"].ToString(),
                                            dataRow["IsActive"] == DBNull.Value ? true: (dataRow["IsActive"].ToString()=="Y"? true: false),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(eventDTO);
            return eventDTO;
        }

        /// <summary>
        /// Gets the Event data of passed Event Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns EventDTO</returns>
        public EventDTO GetEventDTO(int id)
        {
            log.LogMethodEntry(id);
            EventDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE ev.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                returnValue = GetEventDTO(dataTable.Rows[0]);
            }
           
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Checks whether the query is valid. 
        /// </summary>
        /// <param name="query">query</param>
        public bool CheckQuery(string query)
        {
            log.LogMethodEntry(query);
            bool valid = false;
            if(ValidateQueryString(query))
            {
                try
                {
                    dataAccessHandler.executeSelectQuery(query, new System.Data.SqlClient.SqlParameter[] { }, sqlTransaction);
                    valid = true;
                }
                catch(Exception)
                {
                    valid = false;
                }
            }
            log.LogMethodExit(valid);
            return valid;
        }

        /// <summary>
        /// Gets Event Data
        /// </summary>
        /// <param name="query">query</param>
        public bool GetEventData(string query)
        {
            log.LogMethodEntry(query);
            bool returnValue = false;
            if(ValidateQueryString(query))
            {
                try
                {
                    DataTable dataTable = dataAccessHandler.executeSelectQuery(query, null, sqlTransaction);
                    if(dataTable.Rows.Count > 0 && dataTable.Columns.Count > 0)
                    {
                        if(dataTable.Rows[0][0] != DBNull.Value)
                        {
                            if(Convert.ToInt32(dataTable.Rows[0][0]) > 0)
                            {
                                returnValue = true;
                            }
                        }
                    }
                }
                catch(Exception)
                {
                    returnValue = false;
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Validate Query String
        /// </summary>
        /// <param name="query">query</param>
        /// <returns></returns>
        private bool ValidateQueryString(string query)
        {
            bool valid = true;
            if(!string.IsNullOrEmpty(query))
            {
                query = Regex.Replace(query, @"\s+", " ");
                CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
                if(compareInfo.IndexOf(query, "DELETE ", CompareOptions.IgnoreCase) >= 0)
                {
                    valid = false;
                }
                //if(compareInfo.IndexOf(query, "DROP ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if(compareInfo.IndexOf(query, "ALTER ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if(compareInfo.IndexOf(query, "TRUNCATE TABLE ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if(compareInfo.IndexOf(query, "CREATE ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if(compareInfo.IndexOf(query, "EXEC ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if(compareInfo.IndexOf(query, "INSERT ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if(compareInfo.IndexOf(query, "UPDATE ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if(compareInfo.IndexOf(query, "SELECT ", CompareOptions.IgnoreCase) < 0)
                //{
                //    valid = false;
                //}
            }
            else
            {
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        /// <summary>
        /// Gets the MediaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of EventDTO matching the search criteria</returns>
        public List<EventDTO> GetEventDTOList(List<KeyValuePair<EventDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<EventDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach(KeyValuePair<EventDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if(DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if(searchParameter.Key == EventDTO.SearchByParameters.ID ||
                            searchParameter.Key == EventDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == EventDTO.SearchByParameters.TYPE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key == EventDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key == EventDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<EventDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    EventDTO eventDTO = GetEventDTO(dataRow);
                    list.Add(eventDTO);
                }

            }
            log.LogMethodExit(list);
            return list;
        }
    }

    /// <summary>
    /// Represents foreign key error that occur during application execution. 
    /// </summary>
    public class ForeignKeyException : Exception
    {
        /// <summary>
        /// Default constructor of ForeignKeyException.
        /// </summary>
        public ForeignKeyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of ForeignKeyException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public ForeignKeyException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of ForeignKeyException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public ForeignKeyException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

}
