/********************************************************************************************
 * Project Name - Monitor Log Data Handler
 * Description  - Data handler of the monitor log data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        06-May-2019   Jagan Mohana Rao        Created 
 *2.70.2        10-Dec-2019   Jinto Thomas         Removed siteid from update query        
 *2.90        28-May-2020    Mushahid Faizan       Modified : as per 3 tier standards.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.logger
{
    public class MonitorLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<MonitorLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MonitorLogDTO.SearchByParameters, string>
               {
                    {MonitorLogDTO.SearchByParameters.MONITOR_ID, "MonitorId"},
                    {MonitorLogDTO.SearchByParameters.MONITOR_ID_LIST, "MonitorId"},
                    {MonitorLogDTO.SearchByParameters.SITE_ID,"site_id"},
                    {MonitorLogDTO.SearchByParameters.ISACTIVE,"IsActive"}
               };
        /// <summary>
        /// Default constructor of MonitorDataHandler class
        /// </summary>
        public MonitorLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating monitors Record.
        /// </summary>
        /// <param name="monitorLogDTO">MonitorLogDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MonitorLogDTO monitorLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(monitorLogDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", monitorLogDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@monitorId", monitorLogDTO.MonitorId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@statusId", monitorLogDTO.StatusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@logText", monitorLogDTO.LogText));
            parameters.Add(dataAccessHandler.GetSQLParameter("@logKey", monitorLogDTO.LogKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@logValue", monitorLogDTO.LogValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@synchStatus", monitorLogDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", monitorLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the monitor record to the database
        /// </summary>
        /// <param name="monitorLogDTO">MonitorLogDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public MonitorLogDTO InsertMonitorLog(MonitorLogDTO monitorLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(monitorLogDTO, userId, siteId);
            string insertMonitorLogQuery = @"insert into MonitorLog 
                                                        (                                                        
                                                        MonitorId,
                                                        Timestamp,
                                                        StatusId,
                                                        LogText,
                                                        LogKey,
                                                        LogValue,
                                                        site_id,
                                                        Guid,
                                                        SynchStatus,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate
                                                        ) 
                                                values 
                                                        (
                                                        @monitorId,
                                                        getDate(),
                                                        @statusId,
                                                        @logText,
                                                        @logKey,
                                                        @logValue,
                                                        @site_id,
                                                        NewId(),
                                                        @synchStatus,                                                        
                                                        @masterEntityId,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate()
                                                        )SELECT * FROM MonitorLog WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMonitorLogQuery, GetSQLParameters(monitorLogDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorLogDTO(monitorLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(monitorLogDTO);
            return monitorLogDTO;
        }
        /// <summary>
        /// Updates the monitor record
        /// </summary>
        /// <param name="monitorLogDTO">MonitorLogDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public MonitorLogDTO UpdateMonitorLog(MonitorLogDTO monitorLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry();
            string updateMonitorLogQuery = @"update MonitorLog 
                                         set MonitorId=@monitorId,
                                             Timestamp=Getdate(),
                                             StatusId=@statusId,
                                             LogText=@logText,
                                             LogKey=@logKey,
                                             LogValue=@logValue,
                                             -- site_id=@site_id,
                                             SynchStatus = @synchStatus,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastUpdateDate = Getdate()
                                       where Id = @id
                                        select * from MonitorLog where Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMonitorLogQuery, GetSQLParameters(monitorLogDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorLogDTO(monitorLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(monitorLogDTO);
            return monitorLogDTO;
        }

        private void RefreshMonitorLogDTO(MonitorLogDTO monitorLogDTO, DataTable dt)
        {
            log.LogMethodEntry(monitorLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                monitorLogDTO.MonitorId = Convert.ToInt32(dt.Rows[0]["MonitorId"]);
                monitorLogDTO.LastupdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                monitorLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                monitorLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                monitorLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                monitorLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                monitorLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to MonitorLogDTO class type
        /// </summary>
        /// <param name="monitorLogDataRow">MonitorLogDTO DataRow</param>
        /// <returns>Returns MonitorLogDTO</returns>
        private MonitorLogDTO GetMonitorLogDTO(DataRow monitorLogDataRow)
        {
            log.LogMethodEntry(monitorLogDataRow);
            MonitorLogDTO monitorLogDataObject = new MonitorLogDTO(Convert.ToInt32(monitorLogDataRow["Id"]),
                                            monitorLogDataRow["MonitorId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorLogDataRow["MonitorId"]),
                                            monitorLogDataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(monitorLogDataRow["Timestamp"]),
                                            //monitorLogDataRow["MonitorName"].ToString(),
                                            monitorLogDataRow["StatusId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorLogDataRow["StatusId"]),
                                            monitorLogDataRow["LogText"] == DBNull.Value ? string.Empty : monitorLogDataRow["LogText"].ToString(),
                                            monitorLogDataRow["LogKey"] == DBNull.Value ? string.Empty : monitorLogDataRow["LogKey"].ToString(),
                                            monitorLogDataRow["LogValue"] == DBNull.Value ? string.Empty : monitorLogDataRow["LogValue"].ToString(),
                                            monitorLogDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(monitorLogDataRow["site_id"]),
                                            monitorLogDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(monitorLogDataRow["SynchStatus"]),
                                            monitorLogDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorLogDataRow["MasterEntityId"]),
                                            monitorLogDataRow["CreatedBy"].ToString(),
                                            monitorLogDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(monitorLogDataRow["CreationDate"]),
                                             monitorLogDataRow["LastUpdatedBy"].ToString(),
                                            monitorLogDataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(monitorLogDataRow["LastupdateDate"]),
                                            monitorLogDataRow["Guid"].ToString(),
                                            monitorLogDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(monitorLogDataRow["IsActive"])
                                            );
            log.LogMethodExit();
            return monitorLogDataObject;
        }


        /// <summary> //Used as consolidated View in the UI for MonitorDashboard UI
        /// Converts the Data row object to MonitorLogDTO class type
        /// </summary>
        /// <param name="monitorLogDataRow">MonitorLogDTO DataRow</param>
        /// <returns>Returns MonitorLogDTO</returns>
        private MonitorLogDTO GetMonitorLogViewDTO(DataRow monitorLogDataRow)
        {
            log.LogMethodEntry(monitorLogDataRow);
            MonitorLogDTO monitorLogDataObject = new MonitorLogDTO(
                          monitorLogDataRow["MonitorName"] == DBNull.Value ? string.Empty : monitorLogDataRow["MonitorName"].ToString(),
                          monitorLogDataRow["Status"] == DBNull.Value ? string.Empty : monitorLogDataRow["Status"].ToString(),
                          monitorLogDataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(monitorLogDataRow["Timestamp"]),
                          monitorLogDataRow["LogText"] == DBNull.Value ? string.Empty : monitorLogDataRow["LogText"].ToString(),
                          monitorLogDataRow["LogKey"] == DBNull.Value ? string.Empty : monitorLogDataRow["LogKey"].ToString(),
                          monitorLogDataRow["LogValue"] == DBNull.Value ? string.Empty : monitorLogDataRow["LogValue"].ToString(),
                          monitorLogDataRow["ApplicationName"] == DBNull.Value ? string.Empty : monitorLogDataRow["ApplicationName"].ToString(),
                          monitorLogDataRow["ModuleName"] == DBNull.Value ? string.Empty : monitorLogDataRow["ModuleName"].ToString(),
                          monitorLogDataRow["MonitorType"] == DBNull.Value ? string.Empty : monitorLogDataRow["MonitorType"].ToString(),
                          monitorLogDataRow["AssetName"] == DBNull.Value ? string.Empty : monitorLogDataRow["AssetName"].ToString(),
                          monitorLogDataRow["AssetHostname"] == DBNull.Value ? string.Empty : monitorLogDataRow["AssetHostname"].ToString(),
                          monitorLogDataRow["AssetType"] == DBNull.Value ? string.Empty : monitorLogDataRow["AssetType"].ToString(),
                          monitorLogDataRow["Priority"] == DBNull.Value ? string.Empty : monitorLogDataRow["Priority"].ToString(),
                          monitorLogDataRow["MonitorId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorLogDataRow["MonitorId"]),
                          monitorLogDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(monitorLogDataRow["site_id"])
                          );
            log.LogMethodExit();
            return monitorLogDataObject;
        }

        /// <summary>
        /// Gets the MonitorLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MonitorLogDTO matching the search criteria</returns>
        public List<MonitorLogDTO> GetMonitorLogList(List<KeyValuePair<MonitorLogDTO.SearchByParameters, string>> searchParameters, int currentPage, int pageSize)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<MonitorLogDTO> monitorList = new List<MonitorLogDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectMonitorQuery = @"select  * from MonitorLogView";   // Need to be check


            if (searchParameters != null)
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MonitorLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(MonitorLogDTO.SearchByParameters.MONITOR_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MonitorLogDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MonitorLogDTO.SearchByParameters.MONITOR_ID_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        //else if (searchParameter.Key.Equals(MonitorLogDTO.SearchByParameters.ISACTIVE)) // column to be added.
                        //{
                        //    query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        //    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
                        //}
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
                if (searchParameters.Count > 0)
                    selectMonitorQuery = selectMonitorQuery + query;
                if (pageSize > 0)
                {
                    selectMonitorQuery += " ORDER BY MonitorLogView.MonitorId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                    selectMonitorQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
                }
            }

            DataTable monitorData = dataAccessHandler.executeSelectQuery(selectMonitorQuery, parameters.ToArray(), sqlTransaction);
            if (monitorData.Rows.Count > 0)
            {
                foreach (DataRow monitorDataRow in monitorData.Rows)
                {
                    MonitorLogDTO monitorDataObject = GetMonitorLogViewDTO(monitorDataRow);
                    monitorList.Add(monitorDataObject);
                }

            }
            log.LogMethodExit(monitorList);
            return monitorList;
        }

        /// <summary>
        /// Gets the MonitorLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MonitorLogDTO matching the search criteria</returns>
        public List<MonitorLogDTO> GetMonitorLogDTOList(List<KeyValuePair<MonitorLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<MonitorLogDTO> monitorLogDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = "SELECT * FROM MonitorLog";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MonitorLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == MonitorLogDTO.SearchByParameters.MONITOR_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MonitorLogDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MonitorLogDTO.SearchByParameters.MONITOR_ID_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == MonitorLogDTO.SearchByParameters.ISACTIVE)
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
                monitorLogDTOList = new List<MonitorLogDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MonitorLogDTO monitorLogDTO = GetMonitorLogDTO(dataRow);
                    monitorLogDTOList.Add(monitorLogDTO);
                }
            }
            log.LogMethodExit(monitorLogDTOList);
            return monitorLogDTOList;
        }
    }
}