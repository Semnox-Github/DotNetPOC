/********************************************************************************************
 * Project Name - Monitor Asset Data Handler
 * Description  - Data handler of the monitor asset data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Feb-2016   Raghuveera          Created
 *2.60.2      17-June-2019  Jagan Mohana        Created the DeleteMonitorAsset methods
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query                                                          
  *2.90        18-Jun-2020    Mushahid Faizan    Modified : 3 tier changes for Rest API.
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.logger
{
    /// <summary>
    /// Monitor Asset Data Handler Data Handler - Handles insert, update and select of monitor asset data objects
    /// </summary>
    public class MonitorAssetDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private static readonly Dictionary<MonitorAssetDTO.SearchByMonitorAssetParameters, string> DBSearchParameters = new Dictionary<MonitorAssetDTO.SearchByMonitorAssetParameters, string>
               {
                    {MonitorAssetDTO.SearchByMonitorAssetParameters.ASSET_ID, "AssetId"},
                    {MonitorAssetDTO.SearchByMonitorAssetParameters.HOST_NAME, "Hostname"},
                    {MonitorAssetDTO.SearchByMonitorAssetParameters.IP_ADDRESS, "IPAddress"},
                    {MonitorAssetDTO.SearchByMonitorAssetParameters.MAC_ADDRESS, "MacAddress"},
                    {MonitorAssetDTO.SearchByMonitorAssetParameters.NAME, "Name"},
                    {MonitorAssetDTO.SearchByMonitorAssetParameters.SITE_ID,"site_id"},
                    {MonitorAssetDTO.SearchByMonitorAssetParameters.ASSET_TYPE_ID,"AssetTypeId"},
                    {MonitorAssetDTO.SearchByMonitorAssetParameters.ISACTIVE,"IsActive"}
               };
        /// <summary>
        /// Default constructor of MonitorAssetDataHandler class
        /// </summary>
        public MonitorAssetDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Ads Record.
        /// </summary>
        /// <param name="monitorAssetDTO">AdsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>MonitorAssetDTO monitorAsset
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MonitorAssetDTO monitorAssetDTO, string userId, int siteId)
        {
            log.LogMethodEntry(monitorAssetDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetId", monitorAssetDTO.AssetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", monitorAssetDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetTypeId", monitorAssetDTO.AssetTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@hostname", monitorAssetDTO.Hostname));
            parameters.Add(dataAccessHandler.GetSQLParameter("@iPAddress", monitorAssetDTO.IPAddress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@macAddress", monitorAssetDTO.MacAddress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", monitorAssetDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", monitorAssetDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));

            log.LogMethodExit(parameters);
            return parameters;
        }


        private void RefreshAdsDTO(MonitorAssetDTO monitorAssetDTO, DataTable dt)
        {
            log.LogMethodEntry(monitorAssetDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                monitorAssetDTO.AssetId = Convert.ToInt32(dt.Rows[0]["AssetId"]);
                monitorAssetDTO.LastupdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                monitorAssetDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                monitorAssetDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                monitorAssetDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                monitorAssetDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                monitorAssetDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the monitor asset record to the database
        /// </summary>
        /// <param name="monitorAsset">MonitorAssetDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public MonitorAssetDTO InsertMonitorAsset(MonitorAssetDTO monitorAssetDTO, string userId, int siteId)
        {
            log.LogMethodEntry(monitorAssetDTO, userId, siteId);
            string insertMonitorAssetQuery = @"insert into MonitorAsset 
                                                        (
                                                        Name,
                                                        AssetTypeId,
                                                        Hostname,
                                                        IPAddress,
                                                        MacAddress,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        site_id,
                                                        Guid,
                                                        IsActive,CreatedBy, CreationDate
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @name,
                                                        @assetTypeId,
                                                        @hostname,
                                                        @iPAddress,
                                                        @macAddress,
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        @siteId,
                                                        NewId(),
                                                        @isActive, @createdBy, getDate()
                                                        )SELECT * FROM MonitorAsset WHERE AssetId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMonitorAssetQuery, GetSQLParameters(monitorAssetDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAdsDTO(monitorAssetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating monitorAssetDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(monitorAssetDTO);
            return monitorAssetDTO;
        }
        /// <summary>
        /// Updates the monitor asset record
        /// </summary>
        /// <param name="monitorAsset">MonitorAssetDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public MonitorAssetDTO UpdateMonitorAsset(MonitorAssetDTO monitorAssetDTO, string userId, int siteId)
        {
            log.LogMethodEntry(monitorAssetDTO, userId, siteId);
            string updateMonitorAssetQuery = @"update MonitorAsset 
                                         set Name=@name,
                                             AssetTypeId=@assetTypeId,
                                             Hostname=@hostname,
                                             IPAddress=@iPAddress,
                                             MacAddress=@macAddress,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             IsActive = @isActive
                                       where AssetId = @assetId
                                    SELECT * FROM MonitorAsset WHERE AssetId = @assetId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMonitorAssetQuery, GetSQLParameters(monitorAssetDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAdsDTO(monitorAssetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating monitorAssetDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(monitorAssetDTO);
            return monitorAssetDTO;
        }

        /// <summary>
        /// Deletes the MonitorAsset record of passed MonitorAsset assetId
        /// </summary>
        /// <param name="assetId">integer type parameter</param>
        public void DeleteMonitorAsset(int assetId)
        {
            log.LogMethodEntry(assetId);
            string query = @"DELETE  
                             FROM MonitorAsset
                             WHERE AssetId = @assetId";
            SqlParameter parameter = new SqlParameter("@assetId", assetId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter });
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to MonitorAssetDTO class type
        /// </summary>
        /// <param name="monitorAssetDataRow">MonitorAssetDTO DataRow</param>
        /// <returns>Returns MonitorAssetDTO</returns>
        private MonitorAssetDTO GetMonitorAssetDTO(DataRow monitorAssetDataRow)
        {
            log.LogMethodEntry(monitorAssetDataRow);
            MonitorAssetDTO monitorAssetDataObject = new MonitorAssetDTO(Convert.ToInt32(monitorAssetDataRow["AssetId"]),
                                            monitorAssetDataRow["Name"].ToString(),
                                            monitorAssetDataRow["AssetTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorAssetDataRow["AssetTypeId"]),
                                            monitorAssetDataRow["Hostname"].ToString(),
                                            monitorAssetDataRow["IPAddress"].ToString(),
                                            monitorAssetDataRow["MacAddress"].ToString(),
                                            monitorAssetDataRow["LastUpdatedBy"].ToString(),
                                            monitorAssetDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(monitorAssetDataRow["LastupdatedDate"]),
                                            monitorAssetDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(monitorAssetDataRow["site_id"]),
                                            monitorAssetDataRow["Guid"].ToString(),
                                            monitorAssetDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(monitorAssetDataRow["SynchStatus"]),
                                            monitorAssetDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(monitorAssetDataRow["IsActive"]),
                                            monitorAssetDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorAssetDataRow["MasterEntityId"]),
                                            monitorAssetDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(monitorAssetDataRow["CreationDate"]),
                                            monitorAssetDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(monitorAssetDataRow["CreatedBy"])
                                            );
            log.LogMethodExit(monitorAssetDataObject);
            return monitorAssetDataObject;
        }

        /// <summary>
        /// Gets the monitor asset data of passed asset  id
        /// </summary>
        /// <param name="monitorAssetId">integer type parameter</param>
        /// <returns>Returns MonitorAssetDTO</returns>
        public MonitorAssetDTO GetMonitorAsset(int monitorAssetId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(monitorAssetId);
            string selectMonitorAssetQuery = @"select *
                                         from MonitorAsset
                                       WHERE AssetId = @assetId";
            SqlParameter[] selectMonitorAssetParameters = new SqlParameter[1];
            selectMonitorAssetParameters[0] = new SqlParameter("@assetId", monitorAssetId);
            DataTable monitorAsset = dataAccessHandler.executeSelectQuery(selectMonitorAssetQuery, selectMonitorAssetParameters, sqlTransaction);
            if (monitorAsset.Rows.Count > 0)
            {
                DataRow monitorAssetRow = monitorAsset.Rows[0];
                MonitorAssetDTO monitorAssetDataObject = GetMonitorAssetDTO(monitorAssetRow);
                log.Debug("Ends-GetMonitorAsset(monitorAssetId) Method by returnting monitorAssetDataObject.");
                return monitorAssetDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Gets the MonitorAssetDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MonitorAssetDTO matching the search criteria</returns>
        public List<MonitorAssetDTO> GetMonitorAssetList(List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetMonitorAssetList(searchParameters) Method.");
            int count = 0;
            string selectMonitorAssetQuery = @"select *
                                         from MonitorAsset";
            List<MonitorAssetDTO> monitorAssetDTOList = new List<MonitorAssetDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (searchParameters != null)
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(MonitorAssetDTO.SearchByMonitorAssetParameters.ISACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key.Equals(MonitorAssetDTO.SearchByMonitorAssetParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MonitorAssetDTO.SearchByMonitorAssetParameters.ASSET_ID) || searchParameter.Key.Equals(MonitorAssetDTO.SearchByMonitorAssetParameters.ASSET_TYPE_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MonitorAssetDTO.SearchByMonitorAssetParameters.NAME) || searchParameter.Key.Equals(MonitorAssetDTO.SearchByMonitorAssetParameters.HOST_NAME)
                            || searchParameter.Key.Equals(MonitorAssetDTO.SearchByMonitorAssetParameters.IP_ADDRESS) || searchParameter.Key.Equals(MonitorAssetDTO.SearchByMonitorAssetParameters.MAC_ADDRESS))
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
                    selectMonitorAssetQuery = selectMonitorAssetQuery + query;
            }

            DataTable monitorAssetData = dataAccessHandler.executeSelectQuery(selectMonitorAssetQuery, parameters.ToArray(), sqlTransaction);
            if (monitorAssetData.Rows.Count > 0)
            {
                foreach (DataRow monitorAssetDataRow in monitorAssetData.Rows)
                {
                    MonitorAssetDTO monitorAssetDataObject = GetMonitorAssetDTO(monitorAssetDataRow);
                    monitorAssetDTOList.Add(monitorAssetDataObject);
                }
            }
            log.LogMethodExit(monitorAssetDTOList);
            return monitorAssetDTOList;
        }
    }
}
