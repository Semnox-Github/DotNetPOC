/********************************************************************************************
 * Project Name - Asset Type Data Handler
 * Description  - Data handler -Asset Type Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        23-Dec-2015   Raghuveera     Created 
 *2.70        04-Jul-2019   Dakshakh raj   Modified 
 *2.70.2      10-Dec-2019   Jinto Thomas   Removed siteid from update query  
 *2.80        10-May-2020   Girish Kundar  Modified: REST API Changes merge from WMS  
 ********************************************************************************************/


using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Globalization;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Asset Type Data Handler - Handles insert, update and select of asset type objects
    /// </summary>
    public class AssetTypeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Maint_Asset_Types AS mat ";

        /// <summary>
        /// Dictionary for searching Parameters for the CheckInPrice object.
        /// </summary>

        private static readonly Dictionary<AssetTypeDTO.SearchByAssetTypeParameters, string> DBSearchParameters = new Dictionary<AssetTypeDTO.SearchByAssetTypeParameters, string>
        {
                {AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_NAME, "mat.Name"},
                {AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_ID, "mat.AssetTypeId"},
                {AssetTypeDTO.SearchByAssetTypeParameters.ACTIVE_FLAG, "mat.IsActive"},
                {AssetTypeDTO.SearchByAssetTypeParameters.LASTUPDATEDDATE, "mat.LastUpdatedDate"},
                {AssetTypeDTO.SearchByAssetTypeParameters.MASTER_ENTITY_ID,"mat.MasterEntityId"},
                {AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, "mat.site_id"}
        };

        /// <summary>
        /// Parameterized Constructor for AssetTypeDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public AssetTypeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }





        /// <summary>
        /// Inserts the asset type record to the database
        /// </summary>
        /// <param name="assetTypeDTO">AssetTypeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted AssetTypeDTO</returns>
        public AssetTypeDTO InsertAssetType(AssetTypeDTO assetTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(assetTypeDTO, loginId, siteId);
            string insertAssetTypeQuery = @"insert into Maint_Asset_Types 
                                                        ( 
                                                        Name,
                                                        MasterEntityId,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        Guid,
                                                        site_id
                                                        
                                                        ) 
                                                values 
                                                        (
                                                         @name,
                                                         @masterEntityId,
                                                         @isActive,
                                                         @CreatedBy,
                                                         Getdate(),                                                         
                                                         @CreatedBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @site_id
                                                         )SELECT * FROM Maint_Asset_Types WHERE AssetTypeId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertAssetTypeQuery, GetSQLParameters(assetTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAssetTypeDTO(assetTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AssetTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(assetTypeDTO);
            return assetTypeDTO;
        }


        /// <summary>
        /// Updates the Asset type record
        /// </summary>
        /// <param name="assetTypeDTO">AssetTypeDTO type parameter</param>
        /// <param name="loginId">Login Id</param>
        /// <param name="siteId">Site Id</param>
        /// <returns>Returns the AssetTypeDTO</returns>
        public AssetTypeDTO UpdateAssetType(AssetTypeDTO assetTypeDTO, string loginId, int siteId)
        {


            log.LogMethodEntry(assetTypeDTO, loginId, siteId);
            string updateAssetTypeQuery = @"update Maint_Asset_Types 
                                          set Name = @name,
                                              MasterEntityId = @masterEntityId,
                                              IsActive = @isActive, 
                                              LastUpdatedBy = @lastUpdatedBy, 
                                              LastupdatedDate = Getdate()
                                              --site_id = @site_id                                           
                                              where  AssetTypeId = @assetTypeId
                                              SELECT* FROM Maint_Asset_Types WHERE AssetTypeId = @assetTypeId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateAssetTypeQuery, GetSQLParameters(assetTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAssetTypeDTO(assetTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating AssetTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(assetTypeDTO);
            return assetTypeDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="assetTypeDTO">AssetTypeDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        private void RefreshAssetTypeDTO(AssetTypeDTO assetTypeDTO, DataTable dt)
        {
            log.LogMethodEntry(assetTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                assetTypeDTO.AssetTypeId = Convert.ToInt32(dt.Rows[0]["AssetTypeId"]);
                assetTypeDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                assetTypeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                assetTypeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                assetTypeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                assetTypeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                assetTypeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Maint_Asset_Types Record.
        /// </summary>
        /// <param name="assetTypeDTO">AssetTypeDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(AssetTypeDTO assetTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(assetTypeDTO, loginId, siteId);

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetTypeId", assetTypeDTO.AssetTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", assetTypeDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", assetTypeDTO.IsActive ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", assetTypeDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Converts the Data row object to AssetTypeDTO class type
        /// </summary>
        /// <param name="assetTypeDataRow">AssetType DataRow</param>
        /// <returns>Returns AssetType</returns>
        private AssetTypeDTO GetAssetTypeDTO(DataRow assetTypeDataRow)
        {
            log.LogMethodEntry(assetTypeDataRow);
            AssetTypeDTO assetTypeDTO = new AssetTypeDTO(assetTypeDataRow["AssetTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(assetTypeDataRow["AssetTypeId"]),
                                            assetTypeDataRow["Name"].ToString(),
                                            assetTypeDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(assetTypeDataRow["MasterEntityId"]),
                                            assetTypeDataRow["IsActive"] == DBNull.Value ? true : assetTypeDataRow["IsActive"].ToString() == "Y" ? true : false,
                                            assetTypeDataRow["CreatedBy"].ToString(),
                                            assetTypeDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(assetTypeDataRow["CreationDate"]),
                                            assetTypeDataRow["LastUpdatedBy"].ToString(),
                                            assetTypeDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(assetTypeDataRow["LastupdatedDate"]),
                                            assetTypeDataRow["Guid"].ToString(),
                                            assetTypeDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(assetTypeDataRow["site_id"]),
                                            assetTypeDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(assetTypeDataRow["SynchStatus"])
                                            );
            log.LogMethodExit(assetTypeDTO);
            return assetTypeDTO;
        }

        /// <summary>
        /// Gets the Asset type data of passed asset asset Group Id
        /// </summary>
        /// <param name="assetTypeId">integer type parameter</param>
        /// <returns>Returns AssetTypeDTO</returns>
        public AssetTypeDTO GetAssetType(int assetTypeId)
        {
            log.LogMethodEntry(assetTypeId);
            AssetTypeDTO result = null;
            string query = SELECT_QUERY + " WHERE mat.AssetTypeId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", assetTypeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAssetTypeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of AssetTypeDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of AssetTypeDTO</returns>
        public List<AssetTypeDTO> GetAssetTypeList(List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AssetTypeDTO> GetAssetTypeList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_ID
                            || searchParameter.Key == AssetTypeDTO.SearchByAssetTypeParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AssetTypeDTO.SearchByAssetTypeParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "Y" || searchParameter.Value == "1") ? "Y" : "N"));
                        }
                        else if (searchParameter.Key == AssetTypeDTO.SearchByAssetTypeParameters.LASTUPDATEDDATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID)
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
                GetAssetTypeList = new List<AssetTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AssetTypeDTO assetTypeDTO = GetAssetTypeDTO(dataRow);
                    GetAssetTypeList.Add(assetTypeDTO);
                }
            }
            log.LogMethodExit(GetAssetTypeList);
            return GetAssetTypeList;
        }

        /// <summary>
        /// Gets the AssetTypeDTO list matching the search key in a batch
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="maxRows">maximum rows which need to be synched</param>
        /// <returns>Returns the list of AssetTypeDTO matching the search criteria</returns>
        public List<AssetTypeDTO> GetAssetTypeListBatch(List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> searchParameters, int maxRows)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AssetTypeDTO> GetAssetTypeList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int counter = 0;
            string selectAssetTypeQuery = @"SELECT TOP (@maxRows)
                                                   *
                                              FROM Maint_Asset_Types as mat";
            SqlParameter[] selectAssetTypeParameters = new SqlParameter[1];
            parameters.Add(new SqlParameter("@maxRows", maxRows));
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AssetTypeDTO.SearchByAssetTypeParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AssetTypeDTO.SearchByAssetTypeParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "Y" || searchParameter.Value == "1") ? "Y" : "N"));
                        }
                        else if (searchParameter.Key == AssetTypeDTO.SearchByAssetTypeParameters.LASTUPDATEDDATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID)
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
                selectAssetTypeQuery = selectAssetTypeQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectAssetTypeQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                GetAssetTypeList = new List<AssetTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AssetTypeDTO assetTypeDTO = GetAssetTypeDTO(dataRow);
                    GetAssetTypeList.Add(assetTypeDTO);
                }
            }
            log.LogMethodExit(GetAssetTypeList);
            return GetAssetTypeList;
        }
    }
}
