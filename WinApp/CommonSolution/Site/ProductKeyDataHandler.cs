/********************************************************************************************
 * Project Name - ProductKeyDataHandler
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ****************************************** ***************************************************
 *2.60        17-May-2019   Divya                Created 
 *2.100.0     31-Aug-2020   Mushahid Faizan      siteId changes in GetSQLParameters().
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.Site
{
    /// <summary>
    /// ProductKey Data Handler - Handles insert, update and select of ProductKey objects
    /// </summary>
    class ProductKeyDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ProductKey ";

        /// <summary>
        /// Dictionary for searching Parameters for the ProductKeyDTO object.
        /// </summary>
        private static readonly Dictionary<ProductKeyDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProductKeyDTO.SearchByParameters, string>
        {
            { ProductKeyDTO.SearchByParameters.SITE_KEY,"SiteKey"},
            { ProductKeyDTO.SearchByParameters.LICENSE_KEY,"LicenseKey"},
            { ProductKeyDTO.SearchByParameters.ID,"Id"},
            { ProductKeyDTO.SearchByParameters.SITE_ID,"site_id"},
            { ProductKeyDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for ProductKey.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public ProductKeyDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProductKey Record.
        /// </summary>
        /// <param name="productKeyDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(ProductKeyDTO productKeyDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productKeyDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", productKeyDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LicenseKey", productKeyDTO.LicenseKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FeatureKey", productKeyDTO.FeatureKey == null  ? new byte[] { 0 } : productKeyDTO.FeatureKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteKey", productKeyDTO.SiteKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AuthKey", productKeyDTO.AuthKey == null ?  new byte[] { 0 }: productKeyDTO.AuthKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NoOfPOSMachinesLicensed", string.IsNullOrEmpty(productKeyDTO.NoOfPOSMachinesLicensed) ? DBNull.Value:(object)productKeyDTO.NoOfPOSMachinesLicensed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId ,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", productKeyDTO.MasterEntityId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", productKeyDTO.CreatedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", productKeyDTO.LastUpdatedBy));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Builds ProductKey DTO from the passed DataRow
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private ProductKeyDTO GetProductKeyDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProductKeyDTO productKeyDTO = new ProductKeyDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                dataRow["SiteKey"] == DBNull.Value ? null : dataRow["SiteKey"] as byte[],
                                                dataRow["LicenseKey"] == DBNull.Value ? null : dataRow["LicenseKey"] as byte[],
                                                dataRow["FeatureKey"] == DBNull.Value ? null : dataRow["FeatureKey"] as byte[],
                                                dataRow["AuthKey"] == DBNull.Value ? null : dataRow["AuthKey"] as byte[],
                                                dataRow["NoOfPOSMachinesLicensed"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["NoOfPOSMachinesLicensed"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]));
            return productKeyDTO;
        }

        /// <summary>
        /// Gets the ProductKey data of passed ProductKey ID
        /// </summary>
        /// <param name="productKeyId"></param>
        /// <returns>Returns ProductKeyDTO</returns>
        public ProductKeyDTO GetProductKeyDTO(int productKeyId)
        {
            log.LogMethodEntry(productKeyId);
            ProductKeyDTO result = null;
            string query = SELECT_QUERY + @" WHERE ProductKey.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", productKeyId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetProductKeyDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        private void RefreshProductKeyDTO(ProductKeyDTO productKeyDTO, DataTable dt, string userId, int siteId)
        {
            log.LogMethodEntry(productKeyDTO, dt, userId, siteId);
            if (dt.Rows.Count > 0)
            {
                productKeyDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);                
                productKeyDTO.LastUpdatedDate = String.IsNullOrEmpty(dt.Rows[0]["LastUpdateDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
                productKeyDTO.CreationDate = String.IsNullOrEmpty(dt.Rows[0]["CreationDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                productKeyDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                productKeyDTO.LastUpdatedBy = userId;
                productKeyDTO.CreatedBy = userId;
                productKeyDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the ProductKey Table. 
        /// </summary>
        /// <param name="productKeyDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns updated ProductKeyDTO</returns>
        public ProductKeyDTO Insert(ProductKeyDTO productKeyDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productKeyDTO, userId, siteId);
            string query = @"INSERT INTO [dbo].[ProductKey]
                            (
                            SiteKey,
                            LicenseKey,
                            FeatureKey,
                            Guid,
                            site_id,
                            AuthKey,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate,
                            NoOfPOSMachinesLicensed
                            )
                            VALUES
                            (
                            @SiteKey,
                            @LicenseKey,
                            @FeatureKey,
                            NEWID(),
                            @site_id,
                            @AuthKey,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(),
                            @NoOfPOSMachinesLicensed          
                            )
                            SELECT * FROM ProductKey WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productKeyDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshProductKeyDTO(productKeyDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting ProductKeyDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productKeyDTO);
            return productKeyDTO;
        }

        /// <summary>
        /// Update the record in the ProductKey Table. 
        /// </summary>
        /// <param name="productKeyDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns updated ProductKeyDTO</returns>
        public ProductKeyDTO Update(ProductKeyDTO productKeyDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productKeyDTO, userId, siteId);
            string query = @"UPDATE [dbo].[ProductKey]
                             SET
                            SiteKey = @SiteKey,
                            LicenseKey = @LicenseKey,
                            FeatureKey = @FeatureKey,
                            --site_id = @site_id,
                            AuthKey = @AuthKey,
                            MasterEntityId = @MasterEntityId,
                            LastUpdatedBy = @LastUpdatedBy,
                            LastUpdateDate = GETDATE(),
                            NoOfPOSMachinesLicensed = @NoOfPOSMachinesLicensed 
                            WHERE Id = @Id
                            SELECT * FROM ProductKey WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productKeyDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshProductKeyDTO(productKeyDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while updating ProductKeyDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productKeyDTO);
            return productKeyDTO;
        }

        /// <summary>
        /// Returns the List of ProductKeyDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ProductKeyDTO> GetAllProductKeyDTOList(List<KeyValuePair<ProductKeyDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ProductKeyDTO> productKeyDTOList = new List<ProductKeyDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ProductKeyDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ProductKeyDTO.SearchByParameters.ID ||
                            searchParameter.Key == ProductKeyDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductKeyDTO.SearchByParameters.SITE_ID)
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ProductKeyDTO productKeyDTO = GetProductKeyDTO(dataRow);
                    productKeyDTOList.Add(productKeyDTO);
                }
            }
            log.LogMethodExit(productKeyDTOList);
            return productKeyDTOList;
        }
    }
}
