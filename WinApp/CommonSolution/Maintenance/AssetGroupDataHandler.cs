/********************************************************************************************
 * Project Name - Asset Group Data Handler
 * Description  - Data handler of the asset group class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        23-Dec-2015   Raghuveera      Created 
 *2.70        07-Jul-2019   Dakshakh raj    Modified (Added SELECT_QUERY,GetSQLParameters) 
 *2.70.2      10-Dec-2019   Jinto Thomas  Removed siteid from update query        
 *2.80        10-May-2020   Girish Kundar  Modified: REST API Changes    
 *********************************************************************************************/


using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Asset Group Data Handler - Handles insert, update and select of asset group objects
    /// </summary>
    public class AssetGroupDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Maint_AssetGroups AS mag ";

        /// <summary>
        /// Dictionary for searching Parameters for the AssetGroup object.
        /// </summary>

        private static readonly Dictionary<AssetGroupDTO.SearchByAssetGroupParameters, string> DBSearchParameters = new Dictionary<AssetGroupDTO.SearchByAssetGroupParameters, string>
            {
                {AssetGroupDTO.SearchByAssetGroupParameters.ASSETGROUP_NAME, "AssetGroupName"},
                {AssetGroupDTO.SearchByAssetGroupParameters.ASSETGROUP_ID, "AssetGroupId"},
                {AssetGroupDTO.SearchByAssetGroupParameters.ACTIVE_FLAG, "IsActive"},
                {AssetGroupDTO.SearchByAssetGroupParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                {AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID, "site_id"}
            };


        /// <summary>
        /// Parameterized Constructor for AssetGroupDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>

        public AssetGroupDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AssetGroup Record.
        /// </summary>
        /// <param name="assetGroupDTO">AssetGroupDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>

        private List<SqlParameter> GetSQLParameters(AssetGroupDTO assetGroupDTO, string loginId, int siteId)
        {
            
            log.LogMethodEntry(assetGroupDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AssetGroupId", assetGroupDTO.AssetGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetGroupName", assetGroupDTO.AssetGroupName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", assetGroupDTO.IsActive == true ? 'Y' : 'N'));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", assetGroupDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Asset Group record
        /// </summary>
        /// <param name="assetGroup">AssetGroupDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public AssetGroupDTO InsertAssetGroup(AssetGroupDTO assetGroupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(assetGroupDTO, loginId, siteId);
            string insertAssetGroupQuery = @"insert into [dbo].[Maint_AssetGroups] 
                                                        (                                                          
                                                        AssetGroupName,
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
                                                         @assetGroupName,
                                                         @masterEntityId,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),     
                                                         @createdBy,
                                                         Getdate(),     
                                                         NEWID(),
                                                         @siteid
                                                        )SELECT* FROM Maint_AssetGroups WHERE AssetGroupId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertAssetGroupQuery, GetSQLParameters(assetGroupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshassetGroupDTO(assetGroupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AssetGroupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(assetGroupDTO);
            return assetGroupDTO;
        }



        /// <summary>
        /// Updates the Asset Group record
        /// </summary>
        /// <param name="assetGroupDTO">AssetGroupDTO</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public AssetGroupDTO UpdateAssetGroup(AssetGroupDTO assetGroupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(assetGroupDTO, loginId, siteId);
            string updateAssetGroupQuery = @"update [dbo].[Maint_AssetGroups] 
                                                set AssetGroupName = @assetGroupName,
                                                    MasterEntityId=@masterEntityId,
                                                    IsActive = @isActive, 
                                                    LastUpdatedBy = @lastUpdatedBy, 
                                                    LastupdatedDate = Getdate()
                                                    --site_id = @siteid                                             
                                              where AssetGroupId = @AssetGroupId SELECT * FROM Maint_AssetGroups WHERE AssetGroupId = @AssetGroupId"; 
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateAssetGroupQuery, GetSQLParameters(assetGroupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshassetGroupDTO(assetGroupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating AssetGroupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(assetGroupDTO);
            return assetGroupDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="">AssetGroupDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        private void RefreshassetGroupDTO(AssetGroupDTO assetGroupDTO, DataTable dt)
        {
            log.LogMethodEntry(assetGroupDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                assetGroupDTO.AssetGroupId = Convert.ToInt32(dt.Rows[0]["AssetGroupID"]);
                assetGroupDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                assetGroupDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                assetGroupDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                assetGroupDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                assetGroupDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                assetGroupDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to AssetGroupDTO class type
        /// </summary>
        /// <param name="assetGroupDataRow">AssetGroup DataRow</param>
        /// <returns>Returns AssetGroup</returns>
        private AssetGroupDTO GetAssetGroupDTO(DataRow assetGroupDataRow)
        {
            log.LogMethodEntry(assetGroupDataRow);
            AssetGroupDTO assetGroupDTO = new AssetGroupDTO(Convert.ToInt32(assetGroupDataRow["AssetGroupId"]),
                                            assetGroupDataRow["AssetGroupName"].ToString(),
                                            assetGroupDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(assetGroupDataRow["MasterEntityId"]),
                                            assetGroupDataRow["IsActive"] == DBNull.Value ? true : assetGroupDataRow["IsActive"].ToString() == "Y" ? true : false,
                                            assetGroupDataRow["CreatedBy"].ToString(),
                                            assetGroupDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(assetGroupDataRow["CreationDate"]),
                                            assetGroupDataRow["LastUpdatedBy"].ToString(),
                                            assetGroupDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(assetGroupDataRow["LastupdatedDate"]),
                                            assetGroupDataRow["Guid"].ToString(),
                                            assetGroupDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(assetGroupDataRow["site_id"]),
                                            assetGroupDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(assetGroupDataRow["SynchStatus"])
                                            );
            log.LogMethodEntry(assetGroupDTO);
            return assetGroupDTO;
        }

        /// <summary>
        /// Gets the Asset Group data of passed asset asset Group Id
        /// </summary>
        /// <param name="assetGroupId">integer type parameter</param>
        /// <returns>Returns AssetGroupDTO</returns>
        public AssetGroupDTO GetAssetGroup(int assetGroupId)
        {
            log.LogMethodEntry(assetGroupId);
            AssetGroupDTO result = null;
            string selectAssetGroupQuery =SELECT_QUERY + @" WHERE mag.AssetGroupId = @AssetGroupId";
            SqlParameter parameter = new SqlParameter("@AssetGroupId", assetGroupId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectAssetGroupQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetAssetGroupDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of AssetGroupDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of AssetGroupDTO</returns>
        public List<AssetGroupDTO> GetAssetGroupList(List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AssetGroupDTO> assetGroupDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AssetGroupDTO.SearchByAssetGroupParameters.ASSETGROUP_ID
                            || searchParameter.Key == AssetGroupDTO.SearchByAssetGroupParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AssetGroupDTO.SearchByAssetGroupParameters.ASSETGROUP_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == AssetGroupDTO.SearchByAssetGroupParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
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
                assetGroupDTOList = new List<AssetGroupDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AssetGroupDTO assetGroupDTO = GetAssetGroupDTO(dataRow);
                    assetGroupDTOList.Add(assetGroupDTO);
                }
            }
            log.LogMethodExit(assetGroupDTOList);
            return assetGroupDTOList;
        }

    }
}
