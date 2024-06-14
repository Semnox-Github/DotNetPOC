/********************************************************************************************
 * Project Name - Asset Group Asset Data Handler
 * Description  - Data handler of the asset group asset class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        24-Dec-2015   Raghuveera          Created 
 *1.00        18-Jul-2016   Raghuveera          Modified 
 *2.70        05-Jul-2019   Dakshakh raj        Modified (Added SELECT_QUERY,GetSQLParameters)
 *2.70.2        10-Dec-2019 Jinto Thomas        Removed siteid from update query
  *2.80        10-May-2020  Girish Kundar       Modified: REST API Changes    
 ********************************************************************************************/

using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Asset Group Asset Data Handler - Handles insert, update and select of Asset Group Asset Data objects
    /// </summary>

    public class AssetGroupAssetDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Maint_AssetGroup_Assets AS mg ";

        /// <summary>
        /// Dictionary for searching Parameters for the AssetGroupAssetDataHandler object.
        /// </summary>

        private static readonly Dictionary<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string> DBSearchParameters = new Dictionary<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>
            {
                {AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_GROUP_ASSET_ID, "mg.AssetGroupAssetId"},
                {AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_GROUP_ID, "mg.AssetGroupId"},
                {AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_ID, "mg.AssetId"},
                {AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ACTIVE_FLAG, "mg.IsActive"},
                {AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.MASTER_ENTITY_ID,"mg.MasterEntityId"},//starts:Modification on 18-Jul-2016 for publish feature
                {AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.SITE_ID, "mg.site_id"}//Ends:Modification on 18-Jul-2016 for publish feature
            };

        /// <summary>
        /// Parameterized Constructor for AssetGroupAssetDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>

        public AssetGroupAssetDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the asset group asset record to the database
        /// </summary>
        /// <param name="assetGroupAssetDTO">AssetGroupAssetDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>

        public AssetGroupAssetDTO InsertAssetGroupAsset(AssetGroupAssetDTO assetGroupAssetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(assetGroupAssetDTO, loginId, siteId);   
            //Modification on 18-Jul-2016 for publish feature
            string insertAssetGroupAssetQuery = @"INSERT INTO[dbo].[Maint_AssetGroup_Assets]
                                                       (                                              
                                                        AssetGroupId,
                                                        AssetId,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdatedDate,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @assetGroupId,
                                                        @assetId,
                                                        @isActive,
                                                        @createdBy,
                                                        Getdate(),
                                                        @LastUpdatedBy,
                                                        Getdate(),                                                        
                                                        Newid(),
                                                        @siteid,
                                                        @masterEntityId
                                                        )SELECT * FROM Maint_AssetGroup_Assets WHERE AssetGroupAssetId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertAssetGroupAssetQuery, GetSQLParameters(assetGroupAssetDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAssetGroupAssetDTO(assetGroupAssetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AssetGroupAssetDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(assetGroupAssetDTO);
            return assetGroupAssetDTO;
        }

        /// <summary>
        /// Updates the asset group asset record
        /// </summary>
        /// <param name="assetGroupAssetDTO">AssetGroupAssetDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>

        public AssetGroupAssetDTO UpdateAssetGroupAsset(AssetGroupAssetDTO assetGroupAssetDTO, string loginId, int siteId)
        {
            
            log.LogMethodEntry(assetGroupAssetDTO, loginId, siteId);
            string updateAssetGroupAssetQuery = @"update Maint_AssetGroup_Assets 
                                                     set AssetGroupId = @assetGroupId,
                                                         AssetId = @assetId,
                                                         IsActive = @isActive,
                                                         LastUpdatedBy = @LastUpdatedBy, 
                                                         LastupdatedDate = Getdate(),
                                                         -- site_id=@siteid,
                                                         MasterEntityId=@masterEntityId
                                                         where AssetGroupAssetId = @AssetGroupAssetId
                                   SELECT* FROM Maint_AssetGroup_Assets WHERE AssetGroupAssetId = @AssetGroupAssetId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateAssetGroupAssetQuery, GetSQLParameters(assetGroupAssetDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAssetGroupAssetDTO(assetGroupAssetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating AssetGroupAssetDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(assetGroupAssetDTO);
            return assetGroupAssetDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="assetGroupAssetDTO">AssetGroupAssetDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>

        private void RefreshAssetGroupAssetDTO(AssetGroupAssetDTO assetGroupAssetDTO, DataTable dt)
        {
            log.LogMethodEntry(assetGroupAssetDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                assetGroupAssetDTO.AssetGroupAssetId = Convert.ToInt32(dt.Rows[0]["AssetGroupAssetId"]);
                assetGroupAssetDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                assetGroupAssetDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                assetGroupAssetDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                assetGroupAssetDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                assetGroupAssetDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                assetGroupAssetDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating assetGroupAsset Record.
        /// </summary>
        /// <param name="AssetGroupAssetDTO">assetGroupAssetDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>

        private List<SqlParameter> GetSQLParameters(AssetGroupAssetDTO assetGroupAssetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(assetGroupAssetDTO, loginId, siteId);
           
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AssetGroupAssetId", assetGroupAssetDTO.AssetGroupAssetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetGroupId", assetGroupAssetDTO.AssetGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetId", assetGroupAssetDTO.AssetId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", assetGroupAssetDTO.IsActive == true ? 'Y' :'N'));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", assetGroupAssetDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }



        /// <summary>
        /// Converts the Data row object to AssetGroupAssetDTO class type
        /// </summary>
        /// <param name="assetGroupAssetDataRow">AssetGroupAssetDTO DataRow</param>
        /// <returns>Returns AssetGroupAssetDTO</returns>

        private AssetGroupAssetDTO GetAssetGroupAssetDTO(DataRow assetGroupAssetDataRow)
        {
            log.LogMethodEntry(assetGroupAssetDataRow);
            AssetGroupAssetDTO assetGroupAssetDataObject = new AssetGroupAssetDTO(Convert.ToInt32(assetGroupAssetDataRow["AssetGroupAssetId"]),
                                            assetGroupAssetDataRow["AssetGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(assetGroupAssetDataRow["AssetGroupId"]),
                                            assetGroupAssetDataRow["AssetId"] == DBNull.Value ? -1 : Convert.ToInt32(assetGroupAssetDataRow["AssetId"]),
                                            //assetGroupAssetDataRow["IsActive"].ToString(),
                                            assetGroupAssetDataRow["IsActive"] == DBNull.Value ? true : assetGroupAssetDataRow["IsActive"].ToString() == "Y" ? true : false,
                                            assetGroupAssetDataRow["CreatedBy"].ToString(),
                                            assetGroupAssetDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(assetGroupAssetDataRow["CreationDate"]),
                                            assetGroupAssetDataRow["LastUpdatedBy"].ToString(),
                                            assetGroupAssetDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(assetGroupAssetDataRow["LastupdatedDate"]),
                                            assetGroupAssetDataRow["Guid"].ToString(),
                                            assetGroupAssetDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(assetGroupAssetDataRow["site_id"]),
                                            assetGroupAssetDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(assetGroupAssetDataRow["SynchStatus"]),
                                            assetGroupAssetDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(assetGroupAssetDataRow["MasterEntityId"])//Modification on 18-Jul-2016 for publish feature
                                            );
            log.LogMethodExit(assetGroupAssetDataObject);
            return assetGroupAssetDataObject;
        }

        /// <summary>
        /// Gets the asset group asset data of passed asset group asset Id
        /// </summary>
        /// <param name="assetGroupAssetId">Asset group asset id</param>
        /// <returns>Returns AssetGroupAssetDTO</returns>

        public AssetGroupAssetDTO GetAssetGroupAsset(int assetGroupAssetId)
        {
            log.LogMethodEntry(assetGroupAssetId);
            AssetGroupAssetDTO result = null;
            string selectAssetGroupAssetQuery = SELECT_QUERY + @" WHERE AssetGroupAssetId = @AssetGroupAssetId";
            SqlParameter parameter = new SqlParameter("@AssetGroupAssetId", assetGroupAssetId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectAssetGroupAssetQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetAssetGroupAssetDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the List of AssetGroupAssetDTO based on the search parameters.
        /// </summary>
        /// <param name="SearchByAssetGroupAssetParameters">search Parameters</param>
        /// <returns>Returns the List of AssetGroupAssetDTO</returns>
        public List<AssetGroupAssetDTO> GetAssetGroupAssetDTOList(List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AssetGroupAssetDTO> assetGroupAssetDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_GROUP_ASSET_ID
                            || searchParameter.Key == AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_GROUP_ID
                            || searchParameter.Key == AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_ID
                            || searchParameter.Key == AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        
                        else if (searchParameter.Key == AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? 'Y' : 'N')));
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
                assetGroupAssetDTOList = new List<AssetGroupAssetDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AssetGroupAssetDTO assetGroupAssetDTO = GetAssetGroupAssetDTO(dataRow);
                    assetGroupAssetDTOList.Add(assetGroupAssetDTO);
                }
            }
            log.LogMethodExit(assetGroupAssetDTOList);
            return assetGroupAssetDTOList;
        }

    }
}
