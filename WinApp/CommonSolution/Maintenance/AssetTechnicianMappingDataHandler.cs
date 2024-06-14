/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Data Handler of the AssetTechnicianMapping class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.100.0    22-Sept-2020   Mushahid Faizan         Created.
 *2.110.0    13-March-2020   Gururaja Kanjan        Changed to site_id.
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Maintenance
{
    public class AssetTechnicianMappingDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<AssetTechnicianMappingDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AssetTechnicianMappingDTO.SearchByParameters, string>
               {
                    {AssetTechnicianMappingDTO.SearchByParameters.ASSET_ID, "AssetId"},
                    {AssetTechnicianMappingDTO.SearchByParameters.MAP_ID, "MapId"},
                    {AssetTechnicianMappingDTO.SearchByParameters.ASSET_TYPE_ID, "AssetTypeId"},
                    {AssetTechnicianMappingDTO.SearchByParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                    {AssetTechnicianMappingDTO.SearchByParameters.USER_ID, "user_id"},
                    {AssetTechnicianMappingDTO.SearchByParameters.SITE_ID,"Site_Id"},
                    {AssetTechnicianMappingDTO.SearchByParameters.IS_ACTIVE,"IsActive"},
                    {AssetTechnicianMappingDTO.SearchByParameters.IS_PRIMARY,"IsPrimary"},
               };

        /// <summary>
        /// Default constructor of AssetTechnicianMappingDataHandler class
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public AssetTechnicianMappingDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AssetTechnicianMappingDTO class type
        /// </summary>
        /// <param name="AssetTechnicianMappingDataRow">AssetTechnicianMappingDTO DataRow</param>
        /// <returns>Returns AssetTechnicianMappingDTO</returns>
        private AssetTechnicianMappingDTO GetAssetTechnicianMappingDTO(DataRow AssetTechnicianMappingDataRow)
        {
            log.LogMethodEntry(AssetTechnicianMappingDataRow);
            AssetTechnicianMappingDTO AssetTechnicianMappingDataObject = new AssetTechnicianMappingDTO(Convert.ToInt32(AssetTechnicianMappingDataRow["MapId"]),
                                            AssetTechnicianMappingDataRow["AssetId"] == DBNull.Value ? -1 : Convert.ToInt32(AssetTechnicianMappingDataRow["AssetId"]),
                                            AssetTechnicianMappingDataRow["AssetTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(AssetTechnicianMappingDataRow["AssetTypeId"]),
                                            AssetTechnicianMappingDataRow["user_id"] == DBNull.Value ? -1 : Convert.ToInt32(AssetTechnicianMappingDataRow["user_id"]),
                                            AssetTechnicianMappingDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(AssetTechnicianMappingDataRow["IsActive"]),
                                            AssetTechnicianMappingDataRow["IsPrimary"] == DBNull.Value ? false : Convert.ToBoolean(AssetTechnicianMappingDataRow["IsPrimary"]),
                                            AssetTechnicianMappingDataRow["CreatedBy"].ToString(),
                                            AssetTechnicianMappingDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(AssetTechnicianMappingDataRow["CreationDate"]),
                                            AssetTechnicianMappingDataRow["LastUpdatedBy"].ToString(),
                                            AssetTechnicianMappingDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(AssetTechnicianMappingDataRow["LastUpdateDate"]),
                                            AssetTechnicianMappingDataRow["Site_Id"] == DBNull.Value ? -1 : Convert.ToInt32(AssetTechnicianMappingDataRow["Site_Id"]),
                                            AssetTechnicianMappingDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(AssetTechnicianMappingDataRow["MasterEntityId"]),
                                            AssetTechnicianMappingDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(AssetTechnicianMappingDataRow["SynchStatus"]),
                                            AssetTechnicianMappingDataRow["Guid"].ToString()
                                            );
            log.LogMethodExit();
            return AssetTechnicianMappingDataObject;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AssetTechnicianMappingDTO Record.
        /// </summary>
        /// <param name="AssetTechnicianMappingDTO">AssetTechnicianMappingDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AssetTechnicianMappingDTO AssetTechnicianMappingDTO, string userId, int siteId)
        {
            log.LogMethodEntry(AssetTechnicianMappingDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@mapId", AssetTechnicianMappingDTO.MapId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetId", AssetTechnicianMappingDTO.AssetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetTypeId", AssetTechnicianMappingDTO.AssetTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userId", AssetTechnicianMappingDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", AssetTechnicianMappingDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isPrimary", AssetTechnicianMappingDTO.IsPrimary));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", AssetTechnicianMappingDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }
        private void RefreshAssetTechnicianMappingDTO(AssetTechnicianMappingDTO AssetTechnicianMappingDTO, DataTable dt)
        {
            log.LogMethodEntry(AssetTechnicianMappingDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                AssetTechnicianMappingDTO.MapId = Convert.ToInt32(dt.Rows[0]["MapId"]);
                AssetTechnicianMappingDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                AssetTechnicianMappingDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                AssetTechnicianMappingDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                AssetTechnicianMappingDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                AssetTechnicianMappingDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                AssetTechnicianMappingDTO.SiteId = dataRow["Site_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_Id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the AssetTechnicianMapping record to the database
        /// </summary>
        /// <param name="AssetTechnicianMappingDTO">AssetTechnicianMappingDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public AssetTechnicianMappingDTO InsertAssetTechnicianMapping(AssetTechnicianMappingDTO AssetTechnicianMappingDTO, string userId, int siteId)
        {
            log.LogMethodEntry(AssetTechnicianMappingDTO, userId, siteId);
            string insertQuery = @"insert into AssetTechnicianMapping 
                                                        (
                                                           AssetId,
                                                           AssetTypeId,
                                                           user_id,
                                                           IsActive,
                                                           IsPrimary,
                                                           CreatedBy,
                                                           CreationDate,
                                                           LastUpdatedBy,
                                                           LastUpdateDate,
                                                           Site_Id,
                                                           Guid,
                                                           MasterEntityId
                                                        ) 
                                                values 
                                                        ( 
                                                           @assetId,
                                                           @assetTypeId,
                                                           @userId,
                                                           @isActive,
                                                           @isPrimary,
                                                           @createdBy,
                                                           GetDate(),
                                                           @lastUpdatedBy,
                                                           GetDate(),
                                                           @siteid,
                                                           NewId(),
                                                           @masterEntityId
                                                        )SELECT * FROM AssetTechnicianMapping WHERE MapId = scope_identity() ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, GetSQLParameters(AssetTechnicianMappingDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAssetTechnicianMappingDTO(AssetTechnicianMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(AssetTechnicianMappingDTO);
            return AssetTechnicianMappingDTO;
        }

        /// <summary>
        /// Updates the AssetTechnicianMappingDTO record
        /// </summary>
        /// <param name="AssetTechnicianMappingDTO">AssetTechnicianMappingDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public AssetTechnicianMappingDTO UpdateAssetTechnicianMapping(AssetTechnicianMappingDTO AssetTechnicianMappingDTO, string userId, int siteId)
        {
            log.LogMethodEntry(AssetTechnicianMappingDTO, userId, siteId);
            string updateQuery = @"update AssetTechnicianMapping  set 
                                              AssetId= @assetId,
                                              AssetTypeId  = @assetTypeId,
                                              user_id  = @userId,
                                              IsActive                  = @isActive,
                                              IsPrimary                 = @isPrimary,
                                              LastUpdatedBy             = @lastUpdatedBy,
                                              LastUpdateDate            = GetDate(),
                                              MasterEntityId            = @masterEntityId
                                       where MapId = @mapId
                                       select * from AssetTechnicianMapping WHERE MapId = @mapId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, GetSQLParameters(AssetTechnicianMappingDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAssetTechnicianMappingDTO(AssetTechnicianMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(AssetTechnicianMappingDTO);
            return AssetTechnicianMappingDTO;
        }

        /// <summary>
        /// Deletes the AssetTechnicianMapping record of passed  mapId
        /// </summary>
        /// <param name="mapId">integer type parameter</param>
        public void Delete(int mapId)
        {
            log.LogMethodEntry(mapId);
            string query = @"DELETE  
                             FROM AssetTechnicianMapping
                             WHERE MapId = @MapId";
            SqlParameter parameter = new SqlParameter("@MapId", mapId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter });
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the AssetTechnicianMapping data of passed mapId
        /// </summary>
        /// <param name="mapId">integer type parameter</param>
        /// <returns>Returns AssetTechnicianMappingDTO</returns>
        public AssetTechnicianMappingDTO GetAssetTechnicianMappingDTO(int mapId)
        {
            log.LogMethodEntry(mapId);
            string selectQuery = @"select * from AssetTechnicianMapping where MapId = @mapId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@mapId", mapId);
            DataTable AssetTechnicianMapping = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters);
            if (AssetTechnicianMapping.Rows.Count > 0)
            {
                DataRow dataRow = AssetTechnicianMapping.Rows[0];
                AssetTechnicianMappingDTO AssetTechnicianMappingDTO = GetAssetTechnicianMappingDTO(dataRow);
                log.LogMethodExit(AssetTechnicianMappingDTO);
                return AssetTechnicianMappingDTO;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// returns the dictionary 
        /// </summary>
        /// <param name="assetId">language identifier</param>
        /// <returns></returns>
        public ConcurrentDictionary<int, int> GetTechnicianMappingDictionary(int assetId =-1, int siteId =-1)
        {
            log.LogMethodEntry(assetId);
            ConcurrentDictionary<int, int> mappedIdDictionary = new ConcurrentDictionary<int, int>();
            string query = @"Select * from AssetTechnicianMapping where AssetId = @assetId and (m.site_id = @siteId or @siteId = -1)";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@assetId", assetId));
            parameters.Add(new SqlParameter("@siteId", siteId));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int mappedAssetId =  dataTable.Rows[i]["AssetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataTable.Rows[i]["AssetId"]);
                    int mappedUserId =  dataTable.Rows[i]["user_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataTable.Rows[i]["user_id"]);
                    mappedIdDictionary[mappedAssetId] = mappedUserId; // Need to check the values

                    
                    mappedIdDictionary[mappedAssetId] = mappedAssetId;
                    mappedIdDictionary[mappedUserId] = mappedUserId;
                }
            }
            log.LogMethodExit(mappedIdDictionary);
            return mappedIdDictionary;
        }


        internal DateTime? GetAssetTechnicianMappingTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdateDate) LastUpdateDate 
                            FROM AssetTechnicianMapping WHERE (site_Id = @siteId or @siteId = -1) 
                            ";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdateDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdateDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets All TechnicianMappingDTO list
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ALL AssetTechnicianMappingDTO</returns>
        public List<AssetTechnicianMappingDTO> GetAllTechnicianMappingDTOList(List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AssetTechnicianMappingDTO> AssetTechnicianMappingDTOList = new List<AssetTechnicianMappingDTO>();

            string selectQuery = GetTechnicianMappingQuery(searchParameters);
            List<SqlParameter> parameters = GetTechnicianMappingParameters(searchParameters);

            DataTable AssetTechnicianMappingDataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (AssetTechnicianMappingDataTable.Rows.Count > 0)
            {
                foreach (DataRow AssetTechnicianMappingDataRow in AssetTechnicianMappingDataTable.Rows)
                {
                    AssetTechnicianMappingDTO AssetTechnicianMappingDataObject = GetAssetTechnicianMappingDTO(AssetTechnicianMappingDataRow);
                    AssetTechnicianMappingDTOList.Add(AssetTechnicianMappingDataObject);
                }
            }
            log.LogMethodExit(AssetTechnicianMappingDTOList);
            return AssetTechnicianMappingDTOList;
        }

        /// <summary>
        /// Gets the AssetTechnicianMappingDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AssetTechnicianMappingDTO matching the search criteria</returns>
        public List<AssetTechnicianMappingDTO> GetAssetTechnicianMappingDTOList(List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AssetTechnicianMappingDTO> AssetTechnicianMappingDTOList = new List<AssetTechnicianMappingDTO>();

            string joiner = " where ";
            string selectQuery = GetTechnicianMappingQuery(searchParameters);
            List<SqlParameter> parameters = GetTechnicianMappingParameters(searchParameters);

            
            if (searchParameters.Count > 0)
            {
                joiner = " and ";
            }

            selectQuery = selectQuery + joiner + DBSearchParameters[AssetTechnicianMappingDTO.SearchByParameters.ASSET_ID] + " is NOT NULL";
            selectQuery = selectQuery + " and " + DBSearchParameters[AssetTechnicianMappingDTO.SearchByParameters.ASSET_TYPE_ID] + " is NULL";

            DataTable AssetTechnicianMappingDataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (AssetTechnicianMappingDataTable.Rows.Count > 0)
            {
                foreach (DataRow AssetTechnicianMappingDataRow in AssetTechnicianMappingDataTable.Rows)
                {
                    AssetTechnicianMappingDTO AssetTechnicianMappingDataObject = GetAssetTechnicianMappingDTO(AssetTechnicianMappingDataRow);
                    AssetTechnicianMappingDTOList.Add(AssetTechnicianMappingDataObject);
                }
            }
            log.LogMethodExit(AssetTechnicianMappingDTOList);
            return AssetTechnicianMappingDTOList;
        }

        /// <summary>
        /// Gets the AssetTechnicianMappingDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AssetTechnicianMappingDTO matching the search criteria</returns>
        public List<AssetTechnicianMappingDTO> GetAssetTypeTechnicianMappingDTOList(List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AssetTechnicianMappingDTO> AssetTechnicianMappingDTOList = new List<AssetTechnicianMappingDTO>();

            string joiner = " where ";
            string selectQuery = GetTechnicianMappingQuery(searchParameters);
            List<SqlParameter> parameters = GetTechnicianMappingParameters(searchParameters);


            if (searchParameters.Count > 0)
            {
                joiner = " and ";
            }

            selectQuery = selectQuery + joiner + DBSearchParameters[AssetTechnicianMappingDTO.SearchByParameters.ASSET_TYPE_ID] + " is NOT NULL";
            selectQuery = selectQuery + " and " + DBSearchParameters[AssetTechnicianMappingDTO.SearchByParameters.ASSET_ID] + " is NULL";


            DataTable AssetTechnicianMappingDataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (AssetTechnicianMappingDataTable.Rows.Count > 0)
            {
                foreach (DataRow AssetTechnicianMappingDataRow in AssetTechnicianMappingDataTable.Rows)
                {
                    AssetTechnicianMappingDTO AssetTechnicianMappingDataObject = GetAssetTechnicianMappingDTO(AssetTechnicianMappingDataRow);
                    AssetTechnicianMappingDTOList.Add(AssetTechnicianMappingDataObject);
                }
            }
            log.LogMethodExit(AssetTechnicianMappingDTOList);
            return AssetTechnicianMappingDTOList;
        }

        /// <summary>
        /// Gets the AssetTechnicianMappingDTO list for site matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AssetTechnicianMappingDTO matching the search criteria</returns>
        public List<AssetTechnicianMappingDTO> GetSiteTechnicianMappingDTOList(List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AssetTechnicianMappingDTO> AssetTechnicianMappingDTOList = new List<AssetTechnicianMappingDTO>();

            string joiner = " where ";
            string selectQuery = GetTechnicianMappingQuery(searchParameters);
            List<SqlParameter> parameters = GetTechnicianMappingParameters(searchParameters);


            if (searchParameters.Count > 0)
            {
                joiner = " and ";
            }

            selectQuery = selectQuery + joiner + DBSearchParameters[AssetTechnicianMappingDTO.SearchByParameters.ASSET_ID] + " is NULL";
            selectQuery = selectQuery + " and " + DBSearchParameters[AssetTechnicianMappingDTO.SearchByParameters.ASSET_TYPE_ID] + " is NULL";


            DataTable AssetTechnicianMappingDataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (AssetTechnicianMappingDataTable.Rows.Count > 0)
            {
                foreach (DataRow AssetTechnicianMappingDataRow in AssetTechnicianMappingDataTable.Rows)
                {
                    AssetTechnicianMappingDTO AssetTechnicianMappingDataObject = GetAssetTechnicianMappingDTO(AssetTechnicianMappingDataRow);
                    AssetTechnicianMappingDTOList.Add(AssetTechnicianMappingDataObject);
                }
            }
            log.LogMethodExit(AssetTechnicianMappingDTOList);
            return AssetTechnicianMappingDTOList;
        }


        /// <summary>
        /// Gets the AssetTechnicianMappingDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AssetTechnicianMappingDTO matching the search criteria</returns>
        public string GetTechnicianMappingQuery(List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;

            List<SqlParameter> parameters = new List<SqlParameter>();

            string selectQuery = @"select * from AssetTechnicianMapping";

            
            if (searchParameters != null)
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ((searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.MAP_ID) ||
                            searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.ASSET_ID) ||
                            searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.ASSET_TYPE_ID) ||
                            searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.USER_ID) ||
                            searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.MASTER_ENTITY_ID)))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            
                        }
                        else if (searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.IS_ACTIVE))
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

                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }

            log.LogMethodExit(selectQuery);
            return selectQuery;
        }

        public List<SqlParameter> GetTechnicianMappingParameters(List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (searchParameters != null)
            {
               foreach (KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ((searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.MAP_ID) ||
                            searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.ASSET_ID) ||
                            searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.ASSET_TYPE_ID) ||
                            searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.USER_ID) ||
                            searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.MASTER_ENTITY_ID)))

                        {
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));

                        }
                        else if (searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.SITE_ID))
                        {
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(AssetTechnicianMappingDTO.SearchByParameters.IS_ACTIVE))
                        {
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
                        }
                        else
                        {
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
                }
            }

            log.LogMethodExit(parameters);
            return parameters;
        }

    }
}
