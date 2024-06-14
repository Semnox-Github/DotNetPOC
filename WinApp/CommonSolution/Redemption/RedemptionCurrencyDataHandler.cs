/********************************************************************************************
* Project Name - RedemptionCurrency Data Handler
* Description  - Data handler of the RedemptionCurrency class
* 
**************
**Version Log
**************
*Version     Date          Modified By      Remarks          
*********************************************************************************************
*1.00        28-Dec-2016   Amaresh          Created 
*2.3.0       25-Jun-2018   Guru S A         MOdiffied for Redemption Currency short cut keys
*2.7.0       08-Jul-2019   Archana          Redemption Receipt changes to show ticket allocation details
*2.70.2        20-Jul-2019   Deeksha          Modifications as per three tier standard.
*2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query
*2.110.0     14-Oct-2020   Mushahid Faizan   Added methods for Pagination and modified search filters method .
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Redemption
{
    ///<summary>
    ///RedemptionCurrency Data Handler - Handles insert, update and select of RedemptionCurrency Data objects
    ///</summary>
    public class RedemptionCurrencyDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM RedemptionCurrency AS rc ";
        private List<SqlParameter> parameters = new List<SqlParameter>();

        /// <summary>
        /// Dictionary for searching Parameters for the Redemption Currency  object.
        /// </summary>
        private static readonly Dictionary<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string> DBSearchParameters = new Dictionary<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>
            {
                {RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_ID, "rc.CurrencyId"},
                {RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_NAME, "rc.CurrencyName"},
                {RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.PRODUCT_ID, "rc.ProductId"},
                {RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.BARCODE, "rc.BarCode"},
                {RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SHORTCUT_KEYS, "rc.ShortcutKeys"},
                {RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.ISACTIVE, "rc.IsActive"},
                {RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SITE_ID, "rc.site_id"},
                {RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_ID_LIST, "rc.CurrencyId"},
                {RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.MASTER_ENTITY_ID, "rc.MasteEntityId"}

            };

        /// <summary>
        /// Default constructor of RedemptionCurrencyDataHandler class
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public RedemptionCurrencyDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating RedemptionCurrencyDataHandler Record.
        /// </summary>
        /// <param name="redemptionCurrencyDTO">RedemptionCurrencyDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(RedemptionCurrencyDTO redemptionCurrencyDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionCurrencyDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@currencyId", redemptionCurrencyDTO.CurrencyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", redemptionCurrencyDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@valueInTickets", redemptionCurrencyDTO.ValueInTickets == 0 ? DBNull.Value : (object)redemptionCurrencyDTO.ValueInTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@barCode", string.IsNullOrEmpty(redemptionCurrencyDTO.BarCode) ? DBNull.Value : (object)redemptionCurrencyDTO.BarCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@managerApproval", redemptionCurrencyDTO.ManagerApproval));
            parameters.Add(dataAccessHandler.GetSQLParameter("@showQtyPrompt", redemptionCurrencyDTO.ShowQtyPrompt));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shortCutKeys", string.IsNullOrEmpty(redemptionCurrencyDTO.ShortCutKeys) ? DBNull.Value : (object)redemptionCurrencyDTO.ShortCutKeys));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", redemptionCurrencyDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", redemptionCurrencyDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@currencyName", string.IsNullOrEmpty(redemptionCurrencyDTO.CurrencyName) ? DBNull.Value : (object)redemptionCurrencyDTO.CurrencyName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastModifiedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the RedemptionCurrency record to the database
        /// </summary>
        /// <param name="redemptionCurrencyDTO">RedemptionCurrencyDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public RedemptionCurrencyDTO InsertRedemptionCurrency(RedemptionCurrencyDTO redemptionCurrencyDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionCurrencyDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[RedemptionCurrency]
                                                        (                                                         
                                                         CurrencyName,
                                                         ValueInTickets,
                                                         LastModifiedDate,
                                                         LastModifiedBy,
                                                         ProductId,
                                                         site_id,
                                                         Guid,
                                                         BarCode,
                                                         MasterEntityId,
                                                         IsActive,
                                                         ShowQtyPrompt,
                                                         ManagerApproval,
														 ShortcutKeys,
                                                         CreatedBy,
                                                         CreationDate
                                                       ) 
                                                values 
                                                       (                                                        
                                                         @currencyName,
                                                         @valueInTickets,
                                                         Getdate(),
                                                         @lastModifiedBy,
                                                         @productId,
                                                         @siteId,
                                                         NewId(),
                                                         @barCode,
                                                         @masterEntityId,
                                                         @isActive,
                                                         @showQtyPrompt,
                                                         @managerApproval,
														 @shortCutKeys,
                                                         @createdBy,
                                                         GETDATE()
                                                        ) SELECT * FROM RedemptionCurrency WHERE CurrencyId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(redemptionCurrencyDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionCurrencyDTO(redemptionCurrencyDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting redemptionCurrencyDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(redemptionCurrencyDTO);
            return redemptionCurrencyDTO;
        }

        /// <summary>
        /// Updates the RedemptionCurrency record
        /// </summary>
        /// <param name="redemptionCurrencyDTO">RedemptionCurrencyDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public RedemptionCurrencyDTO UpdateRedemptionCurrency(RedemptionCurrencyDTO redemptionCurrencyDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionCurrencyDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[RedemptionCurrency]
                                            SET         CurrencyName = @currencyName,
                                                        ValueInTickets = @valueInTickets,
                                                        LastModifiedDate = Getdate(),
                                                        LastModifiedBy = @lastModifiedBy,
                                                        ProductId = @productId,
                                                        -- site_id = @siteId,
                                                        BarCode = @barCode,
                                                        MasterEntityId =@masterEntityId,
                                                        IsActive = @isActive,
                                                        ShowQtyPrompt = @showQtyPrompt,
                                                        ManagerApproval = @managerApproval,
														ShortCutKeys = @shortCutKeys
                                                   where CurrencyId = @currencyId
                                SELECT * FROM RedemptionCurrency WHERE CurrencyId = @currencyId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(redemptionCurrencyDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionCurrencyDTO(redemptionCurrencyDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating redemptionCurrencyDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(redemptionCurrencyDTO);
            return redemptionCurrencyDTO;
        }
        /// <summary>
        /// Converts the Data row object to RedemptionCurrencyDTO class type
        /// </summary>
        /// <param name="redemptionCurrencyDataRow">RedemptionCurrencyDTO DataRow</param>
        /// <returns>Returns RedemptionCurrencyDTO</returns>
        private RedemptionCurrencyDTO GetRedemptionCurrencyDTO(DataRow redemptionCurrencyDataRow)
        {
            log.LogMethodEntry(redemptionCurrencyDataRow);
            RedemptionCurrencyDTO redemptionCurrencyDataObject = new RedemptionCurrencyDTO(Convert.ToInt32(redemptionCurrencyDataRow["CurrencyId"]),
                                                    redemptionCurrencyDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(redemptionCurrencyDataRow["ProductId"]),
                                                    redemptionCurrencyDataRow["CurrencyName"] == DBNull.Value ? string.Empty : Convert.ToString(redemptionCurrencyDataRow["CurrencyName"]),
                                                    redemptionCurrencyDataRow["ValueInTickets"] == DBNull.Value ? 0 : Convert.ToDouble(redemptionCurrencyDataRow["ValueInTickets"]),
                                                    redemptionCurrencyDataRow["BarCode"] == DBNull.Value ? string.Empty : Convert.ToString(redemptionCurrencyDataRow["BarCode"]),
                                                    redemptionCurrencyDataRow["LastModifiedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(redemptionCurrencyDataRow["LastModifiedDate"]),
                                                    redemptionCurrencyDataRow["LastModifiedBy"] == DBNull.Value ? string.Empty : Convert.ToString(redemptionCurrencyDataRow["LastModifiedBy"]),
                                                    redemptionCurrencyDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(redemptionCurrencyDataRow["site_id"]),
                                                    redemptionCurrencyDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(redemptionCurrencyDataRow["Guid"]),
                                                    redemptionCurrencyDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(redemptionCurrencyDataRow["SynchStatus"]),
                                                    redemptionCurrencyDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(redemptionCurrencyDataRow["MasterEntityId"]),
                                                    redemptionCurrencyDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(redemptionCurrencyDataRow["IsActive"]),
                                                    redemptionCurrencyDataRow["ShowQtyPrompt"] == DBNull.Value ? false : Convert.ToBoolean(redemptionCurrencyDataRow["ShowQtyPrompt"]),
                                                    redemptionCurrencyDataRow["ManagerApproval"] == DBNull.Value ? false : Convert.ToBoolean(redemptionCurrencyDataRow["ManagerApproval"]),
                                                    redemptionCurrencyDataRow["ShortCutKeys"] == DBNull.Value ? string.Empty : Convert.ToString(redemptionCurrencyDataRow["ShortCutKeys"]),
                                                    redemptionCurrencyDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(redemptionCurrencyDataRow["CreatedBy"]),
                                                    redemptionCurrencyDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(redemptionCurrencyDataRow["CreationDate"])

                                                    );
            log.LogMethodExit();
            return redemptionCurrencyDataObject;
        }

        /// <summary>
        /// Delete the record from the RedemptionCurrency database based on currencyId
        /// </summary>
        /// <param name="currencyId">currencyId </param>
        /// <returns>return the int </returns>
        internal int Delete(int currencyId)
        {
            log.LogMethodEntry(currencyId);
            string query = @"DELETE  
                             FROM RedemptionCurrency
                             WHERE RedemptionCurrency.CurrencyId = @currencyId";
            SqlParameter parameter = new SqlParameter("@currencyId", currencyId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="redemptionCurrencyDTO">RedemptionCurrencyDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshRedemptionCurrencyDTO(RedemptionCurrencyDTO redemptionCurrencyDTO, DataTable dt)
        {
            log.LogMethodEntry(redemptionCurrencyDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                redemptionCurrencyDTO.CurrencyId = Convert.ToInt32(dt.Rows[0]["CurrencyId"]);
                redemptionCurrencyDTO.LastUpdatedDate = dataRow["LastModifiedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastModifiedDate"]);
                redemptionCurrencyDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                redemptionCurrencyDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                redemptionCurrencyDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                redemptionCurrencyDTO.LastModifiedBy = dataRow["LastModifiedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastModifiedBy"]);
                redemptionCurrencyDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the RedemptionCurrency data of passed currencyId
        /// </summary>
        /// <param name="currencyId">integer type parameter</param>
        /// <returns>Returns RedemptionCurrencyDTO</returns>
        public RedemptionCurrencyDTO GetRedemptionCurrency(int currencyId)
        {
            log.LogMethodEntry(currencyId);
            RedemptionCurrencyDTO result = null;
            string query = SELECT_QUERY + @" WHERE rc.CurrencyId= @currencyId";
            SqlParameter parameter = new SqlParameter("@currencyId", currencyId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetRedemptionCurrencyDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the no of RedemptionCurrencies matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetRedemptionCurrenciesCount(List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int currencyDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                currencyDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(currencyDTOCount);
            return currencyDTOCount;
        }

        /// <summary>
        /// Returns the List of RedemptionCurrencyDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of CategoryDTO</returns>
        public string GetFilterQuery(List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_ID)
                            || searchParameter.Key.Equals(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.MASTER_ENTITY_ID)
                            || searchParameter.Key.Equals(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.PRODUCT_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_ID_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_NAME) ||
                                  searchParameter.Key.Equals(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.BARCODE) ||
                                  searchParameter.Key.Equals(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SHORTCUT_KEYS))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.ISACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));

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
        /// Gets the RedemptionCurrencyDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of RedemptionCurrencyDTO matching the search criteria</returns>
        public List<RedemptionCurrencyDTO> GetRedemptionCurrencyList(List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParameters, int currentPage, int pageSize)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = new List<RedemptionCurrencyDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            if (currentPage > 0 && pageSize > 0)
            {
                selectQuery += " ORDER BY rc.CurrencyName OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }

            DataTable redemptionCurrencyData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (redemptionCurrencyData.Rows.Count > 0)
            {
                foreach (DataRow RedemptionCurrencyDataRow in redemptionCurrencyData.Rows)
                {
                    RedemptionCurrencyDTO redemptionCurrencyDataObject = GetRedemptionCurrencyDTO(RedemptionCurrencyDataRow);
                    redemptionCurrencyDTOList.Add(redemptionCurrencyDataObject);
                }

            }
            log.LogMethodExit(redemptionCurrencyDTOList);
            return redemptionCurrencyDTOList;
        }

        /// <summary>
        /// Gets the RedemptionCurrencyDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of RedemptionCurrencyDTO matching the search criteria</returns>
        public List<RedemptionCurrencyDTO> GetRedemptionCurrencyList(List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = new List<RedemptionCurrencyDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);

            DataTable redemptionCurrencyData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (redemptionCurrencyData.Rows.Count > 0)
            {
                foreach (DataRow RedemptionCurrencyDataRow in redemptionCurrencyData.Rows)
                {
                    RedemptionCurrencyDTO redemptionCurrencyDataObject = GetRedemptionCurrencyDTO(RedemptionCurrencyDataRow);
                    redemptionCurrencyDTOList.Add(redemptionCurrencyDataObject);
                }

            }
            log.LogMethodExit(redemptionCurrencyDTOList);
            return redemptionCurrencyDTOList;
        }
        internal DateTime? GetRedemptionCurrencyModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(last_updated_date) LastUpdatedDate from games WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from GameProfileAttributes WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from GameProfileAttributeValues WHERE (site_id = @siteId or @siteId = -1) and machine_id is null
                            ) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Inserts the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="functionalGuid">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void AddManagementFormAccess(string formName, string functionGuid, int siteId,bool isActive)
        {
            log.LogMethodEntry(formName, functionGuid, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'Redemption Currency',@formName,'Data Access',@siteId,@functionGuid,@isActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", isActive));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Rename the managementFormAccess record to the database
        /// </summary>
        /// <param name="newFormName">string type object</param>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void RenameManagementFormAccess(string newFormName, string formName, int siteId, string functionGuid)
        {
            log.LogMethodEntry(newFormName, formName, siteId);
            string query = @"exec RenameManagementFormAccess @newFormName,'Redemption Currency',@formName,'Data Access',@siteId,@functionGuid";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@newFormName", newFormName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Update the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="updatedIsActive">Site to which the record belongs</param>
        /// <param name="functionGuid">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void UpdateManagementFormAccess(string formName, int siteId, bool updatedIsActive, string functionGuid)
        {
            log.LogMethodEntry(formName, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'Redemption Currency',@formName,'Data Access',@siteId,@functionGuid,@updatedIsActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@updatedIsActive", updatedIsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
    }
}
