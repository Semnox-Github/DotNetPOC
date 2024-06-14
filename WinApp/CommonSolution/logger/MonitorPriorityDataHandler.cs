/********************************************************************************************
 * Project Name - MonitorPriority Priority(Master Data) Data Handler
 * Description  - Data handler of the MonitorPriority master data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        06-May-2019   Jagan Mohana Rao        Created
 *2.70.2        10-Dec-2019   Jinto Thomas          Removed siteid from update query                                                          
 *2.90.0       10-Jun-2020     Faizan               Modified : REST API related changes                                                      
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
    /// MonitorPriority Priority Handler Data Handler - Handles insert, update and select of MonitorPriority priority(Master Data) data objects
    /// </summary>
    public class MonitorPriorityDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<MonitorPriorityDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MonitorPriorityDTO.SearchByParameters, string>
               {
                    {MonitorPriorityDTO.SearchByParameters.PRIORITY_ID, "PriorityId"},
                    {MonitorPriorityDTO.SearchByParameters.PRIORITY_ID_LIST, "PriorityId"},
                    { MonitorPriorityDTO.SearchByParameters.PRIORITY_NAME, "Name"},
                    {MonitorPriorityDTO.SearchByParameters.SITE_ID,"site_id"},
                    {MonitorPriorityDTO.SearchByParameters.ISACTIVE,"IsActive"}
               };
        /// <summary>
        /// Default constructor of MonitorPriorityDataHandler class
        /// </summary>
        public MonitorPriorityDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MonitorPrioritys Record.
        /// </summary>
        /// <param name="monitorPriorityDTO">monitorPriorityDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MonitorPriorityDTO monitorPriorityDTO, string userId, int siteId)
        {
            log.LogMethodEntry(monitorPriorityDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@priorityId", monitorPriorityDTO.PriorityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", monitorPriorityDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", monitorPriorityDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", monitorPriorityDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", monitorPriorityDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the MonitorPriority record to the database
        /// </summary>
        /// <param name="monitorPriorityDTO">MonitorPriorityDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public MonitorPriorityDTO InsertMonitorPriority(MonitorPriorityDTO monitorPriorityDTO, string userId, int siteId)
        {
            log.LogMethodEntry(monitorPriorityDTO, userId, siteId);
            string insertMonitorPriorityQuery = @"insert into MonitorPriority 
                                                        (
                                                        Name,
                                                        Description,
                                                        site_id,
                                                        Guid,
                                                       -- SynchStatus,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        IsActive
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @name,
                                                        @description,
                                                        @site_id,
                                                        NewId(),
                                                       -- @synchStatus,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        @isActive
                                                        )SELECT * FROM MonitorPriority WHERE PriorityId = scope_identity() ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMonitorPriorityQuery, GetSQLParameters(monitorPriorityDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorPriorityDTO(monitorPriorityDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(monitorPriorityDTO);
            return monitorPriorityDTO;
        }
        /// <summary>
        /// Updates the MonitorPriority record
        /// </summary>
        /// <param name="monitorPriorityDTO">MonitorPriorityDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public MonitorPriorityDTO UpdateMonitorPriority(MonitorPriorityDTO monitorPriorityDTO, string userId, int siteId)
        {
            log.LogMethodEntry(monitorPriorityDTO, userId, siteId);
            string updateMonitorPriorityQuery = @"update MonitorPriority 
                                         set Name=@name,
                                             Description=@description,
                                             -- site_id=@site_id,
                                            -- SynchStatus = @synchStatus,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastUpdateDate = Getdate(),
                                             IsActive = @isActive
                                       where PriorityId = @priorityId
                                       select * from MonitorPriority where  PriorityId = @priorityId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMonitorPriorityQuery, GetSQLParameters(monitorPriorityDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorPriorityDTO(monitorPriorityDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(monitorPriorityDTO);
            return monitorPriorityDTO;
        }

        private void RefreshMonitorPriorityDTO(MonitorPriorityDTO monitorPriorityDTO, DataTable dt)
        {
            log.LogMethodEntry(monitorPriorityDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                monitorPriorityDTO.PriorityId = Convert.ToInt32(dt.Rows[0]["PriorityId"]);
                monitorPriorityDTO.LastupdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                monitorPriorityDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                monitorPriorityDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                monitorPriorityDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                monitorPriorityDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                monitorPriorityDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Deletes the MonitorPriority record of passed MonitorPriority monitorPriorityId
        /// </summary>
        /// <param name="priorityId">integer type parameter</param>
        public void DeleteMonitorPriority(int priorityId)
        {
            log.LogMethodEntry(priorityId);
            string query = @"DELETE  
                             FROM MonitorPriority
                             WHERE PriorityId = @priorityId";
            SqlParameter parameter = new SqlParameter("@priorityId", priorityId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter });
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to MonitorPriorityDTO class type
        /// </summary>
        /// <param name="monitorPriorityDataRow">MonitorPriorityDTO DataRow</param>
        /// <returns>Returns MonitorPriorityDTO</returns>
        private MonitorPriorityDTO GetMonitorPriorityDTO(DataRow monitorPriorityDataRow)
        {
            log.LogMethodEntry(monitorPriorityDataRow);
            MonitorPriorityDTO monitorPriorityDataObject = new MonitorPriorityDTO(Convert.ToInt32(monitorPriorityDataRow["PriorityId"]),
                                            monitorPriorityDataRow["Name"].ToString(),
                                            monitorPriorityDataRow["Description"].ToString(),
                                            monitorPriorityDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(monitorPriorityDataRow["site_id"]),
                                            monitorPriorityDataRow["Guid"].ToString(),
                                            monitorPriorityDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(monitorPriorityDataRow["SynchStatus"]),
                                            monitorPriorityDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorPriorityDataRow["MasterEntityId"]),
                                            monitorPriorityDataRow["CreatedBy"].ToString(),
                                            monitorPriorityDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(monitorPriorityDataRow["CreationDate"]),
                                            monitorPriorityDataRow["LastUpdatedBy"].ToString(),
                                            monitorPriorityDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(monitorPriorityDataRow["LastUpdateDate"]),
                                            monitorPriorityDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(monitorPriorityDataRow["IsActive"])
                                            );
            log.LogMethodExit();
            return monitorPriorityDataObject;
        }

        /// <summary>
        /// Gets the MonitorPriority data of passed MonitorPriority id
        /// </summary>
        /// <param name="MonitorPriorityAssetId">integer type parameter</param>
        /// <returns>Returns MonitorPriorityDTO</returns>
        public MonitorPriorityDTO GetMonitorPriority(int priorityId)
        {
            log.LogMethodEntry(priorityId);
            string selectMonitorPriorityAssetQuery = @"select * from MonitorPriority where PriorityId = @priorityId";
            SqlParameter[] selectMonitorPriorityAssetParameters = new SqlParameter[1];
            selectMonitorPriorityAssetParameters[0] = new SqlParameter("@priorityId", priorityId);
            DataTable monitorPriority = dataAccessHandler.executeSelectQuery(selectMonitorPriorityAssetQuery, selectMonitorPriorityAssetParameters);
            if (monitorPriority.Rows.Count > 0)
            {
                DataRow monitorPriorityAssetRow = monitorPriority.Rows[0];
                MonitorPriorityDTO monitorPriorityDataObject = GetMonitorPriorityDTO(monitorPriorityAssetRow);
                log.LogMethodExit(monitorPriorityDataObject);
                return monitorPriorityDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Gets the MonitorPriorityDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MonitorPriorityDTO matching the search criteria</returns>
        public List<MonitorPriorityDTO> GetMonitorPriorityList(List<KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<MonitorPriorityDTO> monitorPriorityList = new List<MonitorPriorityDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            string selectMonitorPriorityQuery = @"select * from MonitorPriority";
            if (searchParameters != null)
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MonitorPriorityDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(MonitorPriorityDTO.SearchByParameters.PRIORITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MonitorPriorityDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MonitorPriorityDTO.SearchByParameters.ISACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
                        }
                        else if (searchParameter.Key.Equals(MonitorPriorityDTO.SearchByParameters.PRIORITY_ID_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(MonitorPriorityDTO.SearchByParameters.PRIORITY_NAME))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                if (searchParameters.Count > 0)
                    selectMonitorPriorityQuery = selectMonitorPriorityQuery + query;
            }

            DataTable monitorPriorityData = dataAccessHandler.executeSelectQuery(selectMonitorPriorityQuery, parameters.ToArray(), sqlTransaction);
            if (monitorPriorityData.Rows.Count > 0)
            {
                foreach (DataRow monitorPriorityDataRow in monitorPriorityData.Rows)
                {
                    MonitorPriorityDTO monitorPriorityDataObject = GetMonitorPriorityDTO(monitorPriorityDataRow);
                    monitorPriorityList.Add(monitorPriorityDataObject);
                }
            }
            log.LogMethodExit(monitorPriorityList);
            return monitorPriorityList;
        }
    }
}