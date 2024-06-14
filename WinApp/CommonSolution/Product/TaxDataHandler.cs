/********************************************************************************************
 * Project Name - Tax Data Handler
 * Description  - Data handler of the asset tax class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        07-Jan-2016   Raghuveera      Created 
 *2.70        28-Jan-2019   Faizan          modified Insert and Update Methods
 *2.70        07-Jul-2019   Mehraj          Added DeleteTax() method
 *2.70.2        10-Dec-2019   Jinto Thomas  Removed siteid from update query
 *2.110.0     08-Oct-2020   Mushahid Faizan Modified as per standards, Added methods for Pagination.
 *2.130.0     21-May-2021   Girish Kundar   Modified: Added Attribue1  to 5 columns to the table as part of Xero integration
 * *2.140.0    25-Nov-2021   Fiona Lishal      Modified : Added Searcch Parameter LAST_UPDATED_DATE
 ********************************************************************************************/

using System; 
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    ///  Tax Data Handler - Handles insert, update and select of asset tax objects
    /// </summary>
    public class TaxDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM tax AS tx ";

        private static readonly Dictionary<TaxDTO.SearchByTaxParameters, string> DBSearchParameters = new Dictionary<TaxDTO.SearchByTaxParameters, string>
            {
                {TaxDTO.SearchByTaxParameters.TAX_ID, "tx.tax_id"},
                {TaxDTO.SearchByTaxParameters.ACTIVE_FLAG, "tx.active_flag"},
                {TaxDTO.SearchByTaxParameters.MASTER_ENTITY_ID,"tx.MasterEntityId"},
                {TaxDTO.SearchByTaxParameters.SITE_ID, "tx.site_id"},
                {TaxDTO.SearchByTaxParameters.TAX_NAME, "tx.tax_name"},
                {TaxDTO.SearchByTaxParameters.TAX_NAME_EXACT, "tx.tax_name"},
                {TaxDTO.SearchByTaxParameters.TAX_PERCENTAGE, "tx.tax_percentage"},
                {TaxDTO.SearchByTaxParameters.LAST_UPDATED_DATE, "tx.LastUpdateDate"}

            };
        private List<SqlParameter> parameters = new List<SqlParameter>();

        /// <summary>
        /// Default constructor of TaxDataHandler class
        /// </summary>
        public TaxDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to TaxDTO class type
        /// </summary>
        /// <param name="taxDataRow"> DataRow</param>
        /// <returns>Returns </returns>
        private TaxDTO GetTaxDTO(DataRow taxDataRow)
        {
            log.LogMethodEntry(taxDataRow);
            TaxDTO assetDataObject = new TaxDTO(Convert.ToInt32(taxDataRow["tax_id"]),
                                            taxDataRow["tax_name"].ToString(),
                                            Convert.ToDouble(taxDataRow["tax_percentage"].ToString()),
                                            taxDataRow["active_flag"].ToString() == "Y" ? true : false,
                                            taxDataRow["Guid"].ToString(),
                                            taxDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(taxDataRow["site_id"]),
                                            taxDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(taxDataRow["SynchStatus"]),
                                            taxDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(taxDataRow["MasterEntityId"]),
                                                 taxDataRow["Attribute1"] == DBNull.Value ? string.Empty : Convert.ToString(taxDataRow["Attribute1"]),
                                                     taxDataRow["Attribute2"] == DBNull.Value ? string.Empty : Convert.ToString(taxDataRow["Attribute2"]),
                                                     taxDataRow["Attribute3"] == DBNull.Value ? string.Empty : Convert.ToString(taxDataRow["Attribute3"]),
                                                     taxDataRow["Attribute4"] == DBNull.Value ? string.Empty : Convert.ToString(taxDataRow["Attribute4"]),
                                                     taxDataRow["Attribute5"] == DBNull.Value ? string.Empty : Convert.ToString(taxDataRow["Attribute5"])

                                            );
            log.LogMethodExit(assetDataObject);
            return assetDataObject;
        }

        /// <summary>
        /// Returns the no of RedemptionCurrencyRule matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetTaxCount(List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int currencyRuleDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                currencyRuleDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(currencyRuleDTOCount);
            return currencyRuleDTOCount;
        }

        /// <summary>
        /// Build the List of query based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of query</returns>
        public string GetFilterQuery(List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<TaxDTO.SearchByTaxParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TaxDTO.SearchByTaxParameters.TAX_ID
                            || searchParameter.Key == TaxDTO.SearchByTaxParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == TaxDTO.SearchByTaxParameters.TAX_PERCENTAGE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == TaxDTO.SearchByTaxParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == TaxDTO.SearchByTaxParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + (searchParameter.Value == "1" ? "'Y'" : "'N'") + " ");
                        }
                        else if (searchParameter.Key == TaxDTO.SearchByTaxParameters.TAX_NAME_EXACT)
                        {
                            query.Append(joiner + " " + DBSearchParameters[searchParameter.Key] + "  = '" + searchParameter.Value + "' ");
                        }
                        else if (searchParameter.Key == TaxDTO.SearchByTaxParameters.LAST_UPDATED_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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

        /// <summary>
        /// Gets the TaxDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns the list of RedemptionCurrencyRuleDTO matching the search criteria</returns>
        public List<TaxDTO> GetTaxDTOList(List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParameters, int currentPage, int pageSize, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TaxDTO> taxDTOList = new List<TaxDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            if (currentPage >= 0 && pageSize > 0)
                {
                selectQuery += " ORDER BY tx.Tax_Id OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                taxDTOList = new List<TaxDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TaxDTO taxDTO = GetTaxDTO(dataRow);
                    taxDTOList.Add(taxDTO);
                }
            }
            log.LogMethodExit(taxDTOList);
            return taxDTOList;
        }
        /// <summary>
        /// Gets the TaxDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TaxDTO matching the search criteria</returns>
        public List<TaxDTO> GetTaxList(List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<TaxDTO> taxDTOList = new List<TaxDTO>();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                taxDTOList = new List<TaxDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TaxDTO taxDTO = GetTaxDTO(dataRow);
                    taxDTOList.Add(taxDTO);
                }
            }
            log.LogMethodExit(taxDTOList);
            return taxDTOList;
        }
        /// <summary>
        /// Modify this function
        /// </summary>
        /// <param name="taxDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(TaxDTO taxDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(taxDTO, loginId, siteId);

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxId", taxDTO.TaxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxName", taxDTO.TaxName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxPercentage", taxDTO.TaxPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@activeFlag", taxDTO.ActiveFlag ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", taxDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute1", taxDTO.Attribute1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute2", taxDTO.Attribute2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute3", taxDTO.Attribute3));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute4", taxDTO.Attribute4));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute5", taxDTO.Attribute5));
            log.LogMethodExit(parameters);
            return parameters;
        }

        private void RefreshTaxDTO(TaxDTO taxDTO, DataTable dt)
        {
            log.LogMethodEntry(taxDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                taxDTO.TaxId = Convert.ToInt32(dt.Rows[0]["tax_id"]);
                taxDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                taxDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the TaxDTO record to the database
        /// </summary>
        /// <param name="taxDTO">TaxDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public TaxDTO InsertTax(TaxDTO taxDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(taxDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[tax]
                                     ( 
                                            tax_name,
                                            tax_percentage,
                                            active_flag,
                                            Guid,
                                            site_id,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                             Attribute1,                                        
                                             Attribute2,                                        
                                             Attribute3,                                        
                                             Attribute4,                                        
                                             Attribute5 
                                        ) 
                                VALUES 
                                        (
                                            @taxName,
                                            @taxPercentage,
                                            @activeFlag,
                                            newid(),
                                            @site_id,
                                            @masterEntityId,
                                            @createdBy,
                                            GETDATE(),
                                            @lastUpdatedBy,
                                            GETDATE(),
                                             @Attribute1,                                        
                                             @Attribute2,                                        
                                             @Attribute3,                                        
                                             @Attribute4,                                        
                                             @Attribute5 
                                         ) SELECT * FROM tax WHERE tax_id = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(taxDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTaxDTO(taxDTO, dt);
                SaveInventoryActivityLog(taxDTO, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(taxDTO);
            return taxDTO;
        }

        /// <summary>
        /// Gets the Tax data of passed TaxId
        /// </summary>
        /// <param name="TaxId">integer type parameter</param>
        /// <returns>Returns TaxDTO</returns>
        public TaxDTO GetTaxDTO(int TaxId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(TaxId);
            TaxDTO taxDTO = null;
            string query = SELECT_QUERY + @" WHERE tx.tax_id = @taxId";
            SqlParameter parameter = new SqlParameter("@taxId", TaxId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                taxDTO = GetTaxDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(taxDTO);
            return taxDTO;
        }


        /// <summary>
        /// Updates the TaxDTO record to the database
        /// </summary>
        /// <param name="taxDTO">TaxDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public TaxDTO UpdateTax(TaxDTO taxDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(taxDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[tax] set   
                                            tax_name = @taxName,
                                            tax_percentage= @taxPercentage,
                                            active_flag =@activeFlag,
                                            -- site_id =@site_id,
                                            MasterEntityId =@masterEntityId,
                                            LastUpdatedBy =@lastUpdatedBy,
                                            LastUpdateDate = GETDATE(),
                                             Attribute1 = @Attribute1 ,
                                             Attribute2 = @Attribute2 ,
                                             Attribute3 = @Attribute3 ,
                                             Attribute4 = @Attribute4 ,
                                             Attribute5 = @Attribute5 
                            where tax_id = @taxId
                             SELECT * FROM tax WHERE tax_id = @taxId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(taxDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTaxDTO(taxDTO, dt);
                SaveInventoryActivityLog(taxDTO, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(taxDTO);
            return taxDTO;
        }

        /// <summary>
        /// Delete a Tax record from DB
        /// </summary>
        /// <param name="taxId"></param>
        /// <returns></returns>
        public int DeleteTax(int taxId)
        {
            log.LogMethodEntry(taxId);
            try
            {
                string deleteQuery = @"delete from tax where tax_id = @taxId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@taxId", taxId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        ///  Inserts the record to the InventoryActivityLogDTO Table.
        /// </summary>
        /// <param name="taxInventoryActivityLogDTO">inventoryActivityLogDTO object passed as the Parameter</param>
        /// <param name="loginId">login id of the user </param>
        /// <param name="siteId">site id of the user</param>
        public void SaveInventoryActivityLog(TaxDTO taxInventoryActivityLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(taxInventoryActivityLogDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[InventoryActivityLog]
                           (TimeStamp,
                            Message,
                            Guid,
                            site_id,
                            SourceTableName,
                            InvTableKey,
                            SourceSystemId,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (@TimeStamp,
                            @Message,
                            @Guid,
                            @site_id,
                            @SourceTableName,
                            @InvTableKey,
                            @SourceSystemId,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE()
                            )SELECT CAST(scope_identity() AS int)";

            try
            {
                List<SqlParameter> taxInventoryActivityLogParameters = new List<SqlParameter>();
                taxInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@InvTableKey", DBNull.Value));
                taxInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@Message", "Tax Inserted"));
                taxInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@SourceSystemId", taxInventoryActivityLogDTO.TaxId.ToString() + ":" + taxInventoryActivityLogDTO.TaxName));
                taxInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@SourceTableName", "Tax"));
                taxInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@TimeStamp", ServerDateTime.Now));
                taxInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
                taxInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
                taxInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", taxInventoryActivityLogDTO.MasterEntityId, true));
                taxInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
                taxInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@Guid", taxInventoryActivityLogDTO.Guid));
                log.Debug(taxInventoryActivityLogParameters);

                object rowInserted = dataAccessHandler.executeScalar(query, taxInventoryActivityLogParameters.ToArray(), sqlTransaction);
                log.LogMethodExit(rowInserted);

            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting InventoryActivityLog ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        internal DateTime? GetTaxModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdateDate) LastUpdateDate 
                            FROM (
                            select max(LastUpdateDate) LastUpdateDate from tax WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdateDate from TaxStructure WHERE (site_id = @siteId or @siteId = -1)
                            ) a";
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
    }
}

