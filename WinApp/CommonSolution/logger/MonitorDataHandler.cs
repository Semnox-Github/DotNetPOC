/********************************************************************************************
 * Project Name - Monitor Data Handler
 * Description  - Data handler of the monitor data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        06-May-2019   Jagan Mohana Rao        Created 
 *2.70.2        10-Dec-2019   Jinto Thomas            Removed siteid from update query     
 *2.90        28-May-2020    Mushahid Faizan        Modified : 3 tier changes for Rest API, Added GetMonitorFilterQuery().
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// Monitor Data Handler Data Handler - Handles insert, update and select of monitor data objects
    /// </summary>
    public class MonitorDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private List<SqlParameter> parameters = new List<SqlParameter>();

        private static readonly Dictionary<MonitorDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MonitorDTO.SearchByParameters, string>
               {
                    {MonitorDTO.SearchByParameters.MONITOR_ID, "MonitorId"},
                    {MonitorDTO.SearchByParameters.MONITOR_NAME, "Name"},
                    {MonitorDTO.SearchByParameters.ASSET_ID, "AssetId"},
                    {MonitorDTO.SearchByParameters.PRIORITY_ID, "PriorityId"},
                    {MonitorDTO.SearchByParameters.MONITOR_TYPE_ID, "MonitorTypeId"},
                    {MonitorDTO.SearchByParameters.APPLICATION_ID, "ApplicationId"},
                    {MonitorDTO.SearchByParameters.APPMODULE_ID, "AppModuleId"},
                    {MonitorDTO.SearchByParameters.SITE_ID,"site_id"},
                    {MonitorDTO.SearchByParameters.ISACTIVE,"Active"}
               };
        /// <summary>
        /// Default constructor of MonitorDataHandler class
        /// </summary>
        public MonitorDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        string MONITOR_SELECT_QUERY = @"select * from Monitor ";

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating monitors Record.
        /// </summary>
        /// <param name="monitorDTO">monitorDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MonitorDTO monitorDTO, string userId, int siteId)
        {
            log.LogMethodEntry(monitorDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@monitorId", monitorDTO.MonitorId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", monitorDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetId", monitorDTO.AssetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@monitorTypeId", monitorDTO.MonitorTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@applicationId", monitorDTO.ApplicationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@appModuleId", monitorDTO.AppModuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@interval", monitorDTO.Interval, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", monitorDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", monitorDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@priorityId", monitorDTO.PriorityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", monitorDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the monitor record to the database
        /// </summary>
        /// <param name="monitorDTO">MonitorDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public MonitorDTO InsertMonitor(MonitorDTO monitorDTO, string userId, int siteId)
        {
            log.LogMethodEntry(monitorDTO, userId, siteId);
            string insertMonitorQuery = @"insert into Monitor 
                                                        (
                                                        Name,
                                                        AssetId,
                                                        MonitorTypeId,
                                                        ApplicationId,
                                                        AppModuleId,
                                                        Interval,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        site_id,
                                                        Guid,
                                                       -- SynchStatus,
                                                        Active,
                                                        PriorityId,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @name,
                                                        @assetId,
                                                        @monitorTypeId,
                                                        @applicationId,
                                                        @appModuleId,
                                                        @interval,
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        @site_id,
                                                        NewId(),
                                                        --@synchStatus,
                                                        @active,
                                                        @priorityId,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        getDate()
                                                        )SELECT * FROM Monitor WHERE MonitorId = scope_identity() ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMonitorQuery, GetSQLParameters(monitorDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorDTO(monitorDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(monitorDTO);
            return monitorDTO;
        }

        /// <summary>
        /// Updates the monitor record
        /// </summary>
        /// <param name="monitorDTO">MonitorDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public MonitorDTO UpdateMonitor(MonitorDTO monitorDTO, string userId, int siteId)
        {
            log.LogMethodEntry(monitorDTO, userId, siteId);
            string updateMonitorQuery = @"update Monitor 
                                         set Name=@name,
                                             AssetId=@assetId,
                                             MonitorTypeId=@monitorTypeId,
                                             ApplicationId=@applicationId,
                                             AppModuleId=@appModuleId,
                                             Interval=@interval,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastUpdatedDate = Getdate(),
                                             -- site_id=@site_id,
                                             --SynchStatus = @synchStatus,
                                             Active = @active,
                                             PriorityId = @priorityId
                                       where MonitorId = @monitorId
                                       select * from Monitor where MonitorId = @monitorId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMonitorQuery, GetSQLParameters(monitorDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorDTO(monitorDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(monitorDTO);
            return monitorDTO;
        }
        private void RefreshMonitorDTO(MonitorDTO monitorDTO, DataTable dt)
        {
            log.LogMethodEntry(monitorDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                monitorDTO.MonitorId = Convert.ToInt32(dt.Rows[0]["MonitorId"]);
                monitorDTO.LastupdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                monitorDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                monitorDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                monitorDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                monitorDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                monitorDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the no of messsagesCount matching the search Parameters
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>no of messsagesCount matching the searchParameters</returns>
        public int GetMonitorDTOCount(List<KeyValuePair<MonitorDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int monitorDTOCount = 0;
            string selectQuery = MONITOR_SELECT_QUERY;
            selectQuery += GetMonitorFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                monitorDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(monitorDTOCount);
            return monitorDTOCount;
        }

        /// <summary>
        /// Deletes the Monitor record of passed Monitor monitorId
        /// </summary>
        /// <param name="monitorId">integer type parameter</param>
        public void DeleteMonitor(int monitorId)
        {
            log.LogMethodEntry(monitorId);
            string query = @"DELETE  
                             FROM Monitor
                             WHERE MonitorId = @monitorId";
            SqlParameter parameter = new SqlParameter("@monitorId", monitorId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter });
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to MonitorDTO class type
        /// </summary>
        /// <param name="monitorDataRow">MonitorDTO DataRow</param>
        /// <returns>Returns MonitorDTO</returns>
        private MonitorDTO GetMonitorDTO(DataRow monitorDataRow)
        {
            log.LogMethodEntry(monitorDataRow);
            MonitorDTO monitorDataObject = new MonitorDTO(Convert.ToInt32(monitorDataRow["MonitorId"]),
                                            monitorDataRow["Name"].ToString(),
                                            monitorDataRow["AssetId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorDataRow["AssetId"]),
                                            monitorDataRow["MonitorTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorDataRow["MonitorTypeId"]),
                                            monitorDataRow["ApplicationId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorDataRow["ApplicationId"]),
                                            monitorDataRow["AppModuleId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorDataRow["AppModuleId"]),
                                            monitorDataRow["Interval"] == DBNull.Value ? -1 : Convert.ToInt32(monitorDataRow["Interval"]),
                                            monitorDataRow["LastUpdatedBy"].ToString(),
                                            monitorDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(monitorDataRow["LastupdatedDate"]),
                                            monitorDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(monitorDataRow["site_id"]),
                                            monitorDataRow["Guid"].ToString(),
                                            monitorDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(monitorDataRow["SynchStatus"]),
                                            monitorDataRow["Active"] == DBNull.Value ? true : Convert.ToBoolean(monitorDataRow["Active"]),
                                            monitorDataRow["PriorityId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorDataRow["PriorityId"]),
                                            monitorDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorDataRow["MasterEntityId"]),
                                            monitorDataRow["CreatedBy"].ToString(),
                                            monitorDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(monitorDataRow["CreationDate"])
                                            );
            log.LogMethodExit(monitorDataObject);
            return monitorDataObject;
        }

        /// <summary>
        /// Gets the monitor data of passed monitor id
        /// </summary>
        /// <param name="monitorAssetId">integer type parameter</param>
        /// <returns>Returns MonitorDTO</returns>
        public MonitorDTO GetMonitor(int monitorId)
        {
            log.LogMethodEntry(monitorId);
            string selectMonitorAssetQuery = @"select * from Monitor where MonitorId = @monitorId";
            SqlParameter[] selectMonitorAssetParameters = new SqlParameter[1];
            selectMonitorAssetParameters[0] = new SqlParameter("@monitorId", monitorId);
            DataTable monitor = dataAccessHandler.executeSelectQuery(selectMonitorAssetQuery, selectMonitorAssetParameters);
            if (monitor.Rows.Count > 0)
            {
                DataRow monitorAssetRow = monitor.Rows[0];
                MonitorDTO monitorDataObject = GetMonitorDTO(monitorAssetRow);
                log.LogMethodExit(monitorDataObject);
                return monitorDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Gets the MonitorDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MonitorDTO matching the search criteria</returns>
        public List<MonitorDTO> GetMonitorList(List<KeyValuePair<MonitorDTO.SearchByParameters, string>> searchParameters, int currentPage, int pageSize)
        {
            log.LogMethodEntry(searchParameters);
            List<MonitorDTO> monitorList = new List<MonitorDTO>();
            parameters.Clear();
            string selectMonitorQuery = @"select * from Monitor";
            selectMonitorQuery += GetMonitorFilterQuery(searchParameters);
            selectMonitorQuery += " ORDER BY Monitor.MonitorId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
            selectMonitorQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            DataTable monitorData = dataAccessHandler.executeSelectQuery(selectMonitorQuery, parameters.ToArray(), sqlTransaction);
            if (monitorData.Rows.Count > 0)
            {
                foreach (DataRow monitorDataRow in monitorData.Rows)
                {
                    MonitorDTO monitorDataObject = GetMonitorDTO(monitorDataRow);
                    monitorList.Add(monitorDataObject);
                }

            }
            log.LogMethodExit(monitorList);
            return monitorList;
        }
        /// <summary>
        /// Gets the MonitorDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MonitorDTO matching the search criteria</returns>
        public string GetMonitorFilterQuery(List<KeyValuePair<MonitorDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<MonitorDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(MonitorDTO.SearchByParameters.MONITOR_ID) ||
                            searchParameter.Key.Equals(MonitorDTO.SearchByParameters.ASSET_ID) ||
                            searchParameter.Key.Equals(MonitorDTO.SearchByParameters.MONITOR_TYPE_ID) ||
                            searchParameter.Key.Equals(MonitorDTO.SearchByParameters.PRIORITY_ID) ||
                            searchParameter.Key.Equals(MonitorDTO.SearchByParameters.APPLICATION_ID) ||
                            searchParameter.Key.Equals(MonitorDTO.SearchByParameters.APPMODULE_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MonitorDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MonitorDTO.SearchByParameters.MONITOR_NAME))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(MonitorDTO.SearchByParameters.ISACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
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
            }
            log.LogMethodExit(query);
            return query.ToString();
        }
    }
}