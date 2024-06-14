/********************************************************************************************
 * Project Name - Monitor Asset Type Data Handler
 * Description  - Data handler of the monitor asset type data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        23-Feb-2016   Raghuveera          Created 
 *2.70        16-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2        10-Dec-2019   Jinto Thomas       Removed siteid from update query                                                                                                                   
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
    /// Monitor Asset Type Data Handler Data Handler - Handles insert, update and select of monitor asset type data objects
    /// </summary>
    public class MonitorAssetTypeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM MonitorAssetType AS mat";

        /// <summary>
        /// Dictionary for searching Parameters for the MonitorAssetType object.
        /// </summary>
        private static readonly Dictionary<MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters, string> DBSearchParameters = new Dictionary<MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters, string>
               {
                    {MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters.ASSET_TYPE_ID, "mat.AssetTypeId"},
                    {MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters.ASSET_TYPE_NAME, "mat.AssetType"},
                    {MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters.SITE_ID, "mat.site_id"},
                    {MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters.MASTERENTITYID, "mat.MasterEntityId"}
               };

        /// <summary>
        /// Default constructor of MonitorAssetTypeDataHandler class
        /// </summary>
        public MonitorAssetTypeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating monitorAssetType parameters Record.
        /// </summary>
        /// <param name="monitorAssetTypeDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(MonitorAssetTypeDTO monitorAssetTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorAssetTypeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(dataAccessHandler.GetSQLParameter("@assetTypeId", monitorAssetTypeDTO.AssetTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetType", string.IsNullOrEmpty(monitorAssetTypeDTO.AssetType) ? DBNull.Value : (object)monitorAssetTypeDTO.AssetType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", monitorAssetTypeDTO.Siteid, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", monitorAssetTypeDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the monitor asset type record to the database
        /// </summary>
        /// <param name="monitorAssetType">MonitorAssetTypeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns Dto</returns>
        public MonitorAssetTypeDTO InsertMonitorAssetType(MonitorAssetTypeDTO monitorAssetTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorAssetTypeDTO, loginId, siteId);
            string insertMonitorAssetTypeQuery = @"INSERT INTO[dbo].[MonitorAssetType]  
                                                        (
                                                        AssetType,
                                                        site_id,
                                                        Guid,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @assetType,
                                                        @siteid,
                                                        NewId(),
                                                        @masterEntityId,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate() 
                                                        )SELECT * FROM MonitorAssetType WHERE AssetTypeId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMonitorAssetTypeQuery, GetSQLParameters(monitorAssetTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorAssetType(monitorAssetTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting MonitorAssetTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(monitorAssetTypeDTO);
            return monitorAssetTypeDTO;
        }

        /// <summary>
        /// Updates the monitor asset type record
        /// </summary>
        /// <param name="monitorAssetType">MonitorAssetTypeDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns dto</returns>
        public MonitorAssetTypeDTO UpdateMonitorAssetType(MonitorAssetTypeDTO monitorAssetTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorAssetTypeDTO, loginId, siteId);
            string updateMonitorAssetTypeQuery = @"update MonitorAssetType 
                                         set AssetType = @assetType
                                             --site_id = @siteid
                                       where AssetTypeId = @assetTypeId
                                       SELECT * FROM MonitorAssetType WHERE AssetTypeId = @assetTypeId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMonitorAssetTypeQuery, GetSQLParameters(monitorAssetTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorAssetType(monitorAssetTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating MonitorAssetType", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(monitorAssetTypeDTO);
            return monitorAssetTypeDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="monitorAssetTypeDTO"></param>
        /// <param name="dt">dt</param>
        private void RefreshMonitorAssetType(MonitorAssetTypeDTO monitorAssetTypeDTO, DataTable dt)
        {
            log.LogMethodEntry(monitorAssetTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                monitorAssetTypeDTO.AssetTypeId = Convert.ToInt32(dt.Rows[0]["AssetTypeId"]);
                monitorAssetTypeDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                monitorAssetTypeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                monitorAssetTypeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                monitorAssetTypeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                monitorAssetTypeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                monitorAssetTypeDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to MonitorAssetTypeDTO class type
        /// </summary>
        /// <param name="monitorAssetTypeDataRow">MonitorAssetTypeDTO DataRow</param>
        /// <returns>Returns MonitorAssetTypeDTO</returns>
        private MonitorAssetTypeDTO GetMonitorAssetTypeDTO(DataRow monitorAssetTypeDataRow)
        {
            log.Debug("Starts-GetMonitorAssetTypeDTO(monitorAssetTypeDataRow) Method.");
            MonitorAssetTypeDTO monitorAssetTypeDataObject = new MonitorAssetTypeDTO(Convert.ToInt32(monitorAssetTypeDataRow["AssetTypeId"]),
                                            monitorAssetTypeDataRow["AssetType"].ToString(),
                                            monitorAssetTypeDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(monitorAssetTypeDataRow["site_id"]),
                                            monitorAssetTypeDataRow["Guid"].ToString(),
                                            monitorAssetTypeDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(monitorAssetTypeDataRow["SynchStatus"]),
                                            monitorAssetTypeDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(monitorAssetTypeDataRow["MasterEntityId"]),
                                            monitorAssetTypeDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(monitorAssetTypeDataRow["CreatedBy"]),
                                            monitorAssetTypeDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(monitorAssetTypeDataRow["CreationDate"]),
                                            monitorAssetTypeDataRow["LastUpdatedBy"].ToString(),
                                            monitorAssetTypeDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(monitorAssetTypeDataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(monitorAssetTypeDataObject);
            return monitorAssetTypeDataObject;
        }

        /// <summary>
        /// Gets the monitor asset type data of passed asset type id
        /// </summary>
        /// <param name="monitorAssetTypeId">integer type parameter</param>
        /// <returns>Returns MonitorAssetTypeDTO</returns>
        public MonitorAssetTypeDTO GetMonitorAssetType(int monitorAssetTypeId)
        {
            log.LogMethodEntry(monitorAssetTypeId);
            MonitorAssetTypeDTO result = null;
            string selectMonitorAssetTypeQuery = SELECT_QUERY + @" WHERE mat.AssetTypeId = @assetTypeId";
            SqlParameter parameter = new SqlParameter("@assetTypeId", monitorAssetTypeId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectMonitorAssetTypeQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetMonitorAssetTypeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the MonitorAssetTypeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MonitorAssetTypeDTO matching the search criteria</returns>
        public List<MonitorAssetTypeDTO> GetMonitorAssetTypeList(List<KeyValuePair<MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<MonitorAssetTypeDTO> monitorAssetTypeDTOList = null; 
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters.MASTERENTITYID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                monitorAssetTypeDTOList = new List<MonitorAssetTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MonitorAssetTypeDTO monitorAssetTypeDTO  = GetMonitorAssetTypeDTO(dataRow);
                    monitorAssetTypeDTOList.Add(monitorAssetTypeDTO);
                }
            }
            log.LogMethodExit(monitorAssetTypeDTOList);
            return monitorAssetTypeDTOList;
        }
    }
}
