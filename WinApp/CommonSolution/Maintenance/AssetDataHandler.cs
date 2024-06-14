/********************************************************************************************
 * Project Name - Asset Data Handler
 * Description  - Data handler of the asset class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        23-Dec-2015   Raghuveera     Created 
 *2.70        07-Jul-2019   Dakshakh raj   Modified (Added SELECT_QUERY,GetSQLParameters)
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query                                                          
 *2.80        10-May-2020    Girish Kundar  Modified: REST API Changes                                                           
 ********************************************************************************************/

using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Globalization;
using Semnox.Core.Utilities;

//using Semnox.Core.DataAccess;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Asset Data Handler - Handles insert, update and select of asset objects
    /// </summary>
    public class AssetDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Maint_Assets AS ma ";

        /// <summary>
        /// Dictionary for searching Parameters for the Category object.
        /// </summary>
        
        private static readonly Dictionary<GenericAssetDTO.SearchByAssetParameters, string> DBSearchParameters = new Dictionary<GenericAssetDTO.SearchByAssetParameters, string>
            {
                {GenericAssetDTO.SearchByAssetParameters.ASSET_NAME, "ma.Name"},
                {GenericAssetDTO.SearchByAssetParameters.ASSET_ID, "ma.AssetId"},
                {GenericAssetDTO.SearchByAssetParameters.ASSET_TYPE_ID, "ma.AssetTypeId"},
                {GenericAssetDTO.SearchByAssetParameters.URN, "ma.URN"},
                {GenericAssetDTO.SearchByAssetParameters.ASSET_STATUS, "ma.AssetStatus"},
                {GenericAssetDTO.SearchByAssetParameters.LOCATION, "ma.Location"},
                {GenericAssetDTO.SearchByAssetParameters.ACTIVE_FLAG, "ma.IsActive"},
                {GenericAssetDTO.SearchByAssetParameters.LASTUPDATEDDATE, "ma.LastUpdatedDate"},
                {GenericAssetDTO.SearchByAssetParameters.MACHINEID, "ma.Machineid"},
                {GenericAssetDTO.SearchByAssetParameters.MASTER_ENTITY_ID,"ma.MasterEntityId"},
                {GenericAssetDTO.SearchByAssetParameters.SITE_ID, "ma.site_id"}
            };

        /// <summary>
        /// Parameterized Constructor for AssetDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>

        public AssetDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Category Record.
        /// </summary>
        /// <param name="GenericAssetDTO">GenericAssetDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>

        private List<SqlParameter> GetSQLParameters(GenericAssetDTO genericAssetDTO, string loginId, int siteId)
        {
           
            log.LogMethodEntry(genericAssetDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetId", genericAssetDTO.AssetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", genericAssetDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", genericAssetDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineid", genericAssetDTO.Machineid, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetTypeId", genericAssetDTO.AssetTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@location", genericAssetDTO.Location));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetStatus", genericAssetDTO.AssetStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@urn", genericAssetDTO.URN));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseDate", genericAssetDTO.PurchaseDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@saleDate", genericAssetDTO.SaleDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scrapDate", genericAssetDTO.ScrapDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetTaxTypeId", genericAssetDTO.AssetTaxTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseValue", genericAssetDTO.PurchaseValue, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@saleValue", genericAssetDTO.SaleValue, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scrapValue", genericAssetDTO.ScrapValue, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", genericAssetDTO.IsActive == true ? 'Y' :'N'));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", genericAssetDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }



        /// <summary>
        /// Inserts the asset record to the database
        /// </summary>
        /// <param name="assetDTO">GenericAssetDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public GenericAssetDTO InsertAsset(GenericAssetDTO assetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(assetDTO, loginId, siteId);
            string insertAssetQuery = @"INSERT INTO[dbo].[Maint_Assets]
                                                        ( 
                                                         Name,
                                                         Description,
                                                         Machineid,
                                                         AssetTypeId,
                                                         Location,
                                                         AssetStatus,
                                                         URN,
                                                         PurchaseDate,
                                                         SaleDate,
                                                         ScrapDate,
                                                         AssetTaxTypeId,
                                                         PurchaseValue,
                                                         SaleValue,
                                                         ScrapValue,
                                                         MasterEntityId,
                                                         IsActive,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdatedBy,
                                                         LastupdatedDate,
                                                         Guid,
                                                         site_id
                                                        ) 
                                                values 
                                                        (
                                                         @name,
                                                         @description,
                                                         @machineid,
                                                         @assetTypeId,
                                                         @location,
                                                         @assetStatus,
                                                         @urn,
                                                         @purchaseDate,
                                                         @saleDate,
                                                         @scrapDate,
                                                         @assetTaxTypeId,
                                                         @purchaseValue,
                                                         @saleValue,
                                                         @scrapValue,
                                                         @masterEntityId,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(), 
                                                         @lastUpdatedBy,
                                                         GetDate(),                                                     
                                                         Newid(),
                                                         @siteId
                                                        ) SELECT * FROM Maint_Assets WHERE AssetId =scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertAssetQuery, GetSQLParameters(assetDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAssetDTO(assetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting GenericAssetDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(assetDTO);
            return assetDTO;
        }

        /// <summary>
        /// Updates the Asset record
        /// </summary>
        /// <param name="assetDTO">GenericAssetDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public GenericAssetDTO UpdateAsset(GenericAssetDTO assetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(assetDTO, loginId, siteId);
            string updateAssetQuery = @"update Maint_Assets 
                                         set Name = @name,
                                             Description=@description,
                                             Machineid=@machineid,
                                             AssetTypeId=@assetTypeId,
                                             Location=@location,
                                             AssetStatus=@assetStatus,
                                             URN=@urn,
                                             PurchaseDate=@purchaseDate,
                                             SaleDate=@saleDate,
                                             ScrapDate=@scrapDate,
                                             AssetTaxTypeId=@assetTaxTypeId,
                                             PurchaseValue=@purchaseValue,
                                             SaleValue=@saleValue,
                                             ScrapValue=@scrapValue,
                                             MasterEntityId=@masterEntityId,
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate()
                                             -- site_id=@siteid
                                        where AssetId = @assetId
                                        SELECT* FROM Maint_Assets WHERE AssetId = @assetId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateAssetQuery, GetSQLParameters(assetDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAssetDTO(assetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating GenericAssetDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(assetDTO);
            return assetDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="assetDTO">GenericAssetDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        private void RefreshAssetDTO(GenericAssetDTO assetDTO, DataTable dt)
        {
            log.LogMethodEntry(assetDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                assetDTO.AssetId = Convert.ToInt32(dt.Rows[0]["AssetId"]);
                assetDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                assetDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                assetDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                assetDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                assetDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                assetDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to GenericAssetDTO class type
        /// </summary>
        /// <param name="assetDataRow">Asset DataRow</param>
        /// <returns>Returns Asset</returns>
        private GenericAssetDTO GetGenericAssetDTO(DataRow assetDataRow)
        {
            log.LogMethodEntry(assetDataRow);
            GenericAssetDTO genericAssetDTO = new GenericAssetDTO(Convert.ToInt32(assetDataRow["AssetId"]),
                                            assetDataRow["Name"].ToString(),
                                            assetDataRow["Description"].ToString(),
                                            assetDataRow["Machineid"] == DBNull.Value ? -1 : Convert.ToInt32(assetDataRow["Machineid"]),
                                            Convert.ToInt32(assetDataRow["AssetTypeId"]),
                                            assetDataRow["Location"].ToString(),
                                            assetDataRow["AssetStatus"].ToString(),
                                            assetDataRow["URN"].ToString(),
                                            assetDataRow["PurchaseDate"] == DBNull.Value ? "" : Convert.ToDateTime(assetDataRow["PurchaseDate"]).ToString(),
                                            assetDataRow["SaleDate"] == DBNull.Value ? "" : Convert.ToDateTime(assetDataRow["SaleDate"]).ToString(),
                                            assetDataRow["ScrapDate"] == DBNull.Value ? "" : Convert.ToDateTime(assetDataRow["ScrapDate"]).ToString(),
                                            assetDataRow["AssetTaxTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(assetDataRow["AssetTaxTypeId"]),
                                            assetDataRow["PurchaseValue"] == DBNull.Value ? 0 : Convert.ToDouble(assetDataRow["PurchaseValue"]),
                                            assetDataRow["SaleValue"] == DBNull.Value ? 0 : Convert.ToDouble(assetDataRow["SaleValue"]),
                                            assetDataRow["ScrapValue"] == DBNull.Value ? 0 : Convert.ToDouble(assetDataRow["ScrapValue"]),
                                            assetDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(assetDataRow["MasterEntityId"]),
                                           // assetDataRow["IsActive"].ToString(),
                                            assetDataRow["IsActive"] == DBNull.Value ? true : assetDataRow["IsActive"].ToString() == "Y" ? true : false,
                                            assetDataRow["CreatedBy"].ToString(),
                                            assetDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(assetDataRow["CreationDate"]),
                                            assetDataRow["LastUpdatedBy"].ToString(),
                                            assetDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(assetDataRow["LastupdatedDate"]),
                                            assetDataRow["Guid"].ToString(),
                                            assetDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(assetDataRow["site_id"]),
                                            assetDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(assetDataRow["SynchStatus"])
                                            );
            log.LogMethodExit(genericAssetDTO);
            return genericAssetDTO;
        }

        /// <summary>
        /// Gets the Asset data of passed asset asset Group Id
        /// </summary>
        /// <param name="assetId">integer type parameter</param>
        /// <returns>Returns GenericAssetDTO</returns>
        public GenericAssetDTO GetAsset(int assetId)
        {
            log.LogMethodEntry(assetId);
            GenericAssetDTO result = null;
            string selectAssetQuery = SELECT_QUERY + @" WHERE ma.AssetId = @assetId";
            SqlParameter parameter = new SqlParameter("@assetId", assetId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectAssetQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetGenericAssetDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of GenericAssetDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of GenericAssetDTO</returns>
        public List<GenericAssetDTO> GetGenericAssetsList(List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<GenericAssetDTO> genericAssetDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == GenericAssetDTO.SearchByAssetParameters.ASSET_ID
                            || searchParameter.Key == GenericAssetDTO.SearchByAssetParameters.ASSET_TYPE_ID
                            || searchParameter.Key == GenericAssetDTO.SearchByAssetParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == GenericAssetDTO.SearchByAssetParameters.MACHINEID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GenericAssetDTO.SearchByAssetParameters.ASSET_NAME
                           || searchParameter.Key == GenericAssetDTO.SearchByAssetParameters.URN
                            || searchParameter.Key == GenericAssetDTO.SearchByAssetParameters.LOCATION
                            || searchParameter.Key == GenericAssetDTO.SearchByAssetParameters.ASSET_STATUS)
                            
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == GenericAssetDTO.SearchByAssetParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == GenericAssetDTO.SearchByAssetParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value =="Y" ? 'Y' : 'N' )));
                        }
                        else if (searchParameter.Key == GenericAssetDTO.SearchByAssetParameters.LASTUPDATEDDATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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
                genericAssetDTOList = new List<GenericAssetDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GenericAssetDTO genericAssetDTO = GetGenericAssetDTO(dataRow);
                    genericAssetDTOList.Add(genericAssetDTO);
                }
            }
            log.LogMethodExit(genericAssetDTOList);
            return genericAssetDTOList;
        }

        /// <summary>
        /// Gets the GenericAssetDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="maxRows">Maximum row to be fetched based on input from external call</param>
        /// <returns>Returns the list of GenericAssetDTO matching the search criteria</returns>
        public List<GenericAssetDTO> GetGenericAssetListBatch(List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> searchParameters, int maxRows)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string joiner;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectGenericAssetQuery = @"SELECT TOP (@maxRows)
                                                      *
                                                 FROM Maint_Assets as ma";
            //SqlParameter[] selectAssetParameters = new SqlParameter[1];
            parameters.Add(new SqlParameter("@maxRows", maxRows));

            //selectAssetParameters[0] = new SqlParameter("@maxRows", maxRows);
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and  ";
                        {
                            if (searchParameter.Key == GenericAssetDTO.SearchByAssetParameters.LASTUPDATEDDATE)
                            {
                                query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) > " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            }

                            else if (searchParameter.Key == GenericAssetDTO.SearchByAssetParameters.ASSET_ID)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
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
                query.Append(" ORDER BY " + DBSearchParameters[GenericAssetDTO.SearchByAssetParameters.ASSET_ID]);
                selectGenericAssetQuery = selectGenericAssetQuery + query;
            }
            DataTable assetData = dataAccessHandler.executeSelectQuery(selectGenericAssetQuery,parameters.ToArray(), sqlTransaction);
            if (assetData.Rows.Count > 0)
            {
                List<GenericAssetDTO> assetList = new List<GenericAssetDTO>();
                foreach (DataRow assetDataRow in assetData.Rows)
                {
                    GenericAssetDTO assetDataObject = GetGenericAssetDTO(assetDataRow);
                    assetList.Add(assetDataObject);
                }
                log.LogMethodExit(assetList);
                return assetList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
    }
}
