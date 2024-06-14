/********************************************************************************************
 * Project Name - Semnox.Parafait.Customer.Accounts -Accoun tCreditPlus Purchase Criteria 
 * Description  - AccountCreditPlusPurchaseCriteriaDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        23-Jul-2019   Girish Kundar            Modified : Added RefreshDTO() method and Fix for SQL Injection Issue
 *2.140         14-Sep-2021      Fiona                 Modified: Issue fixes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    ///  AccountCreditPlusPurchaseCriteria Data Handler - Handles insert, update and select of  AccountCreditPlusPurchaseCriteria objects
    /// </summary>
    public class AccountCreditPlusPurchaseCriteriaDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters, string>
            {
                {AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.ACCOUNT_CREDITPLUS_PURCHASE_CRITERIA_ID, "CardCreditPlusPurchaseCriteria.Id"},
                {AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.ACCOUNT_CREDITPLUS_ID, "CardCreditPlusPurchaseCriteria.CardCreditPlusId"},
                {AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.ACCOUNT_ID, "CardCreditPlus.Card_id"},
                {AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.ACCOUNT_ID_LIST, "CardCreditPlus.Card_id"},
                {AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.POSTYPE_ID, "CardCreditPlusPurchaseCriteria.POSTypeId"},
                {AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.PRODUCT_ID,"CardCreditPlusPurchaseCriteria.ProductId"},
                {AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.MASTER_ENTITY_ID,"CardCreditPlusPurchaseCriteria.MasterEntityId"},
                {AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.SITE_ID, "CardCreditPlusPurchaseCriteria.site_id"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CardCreditPlusPurchaseCriteria AS CardCreditPlusPurchaseCriteria ";
        /// <summary>
        /// Default constructor of AccountCreditPlusPurchaseCriteriaDataHandler class
        /// </summary>
        public AccountCreditPlusPurchaseCriteriaDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AccountCreditPlusPurchaseCriteria Record.
        /// </summary>
        /// <param name="accountCreditPlusPurchaseCriteriaDTO">AccountCreditPlusPurchaseCriteriaDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AccountCreditPlusPurchaseCriteriaDTO accountCreditPlusPurchaseCriteriaDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountCreditPlusPurchaseCriteriaDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", accountCreditPlusPurchaseCriteriaDTO.AccountCreditPlusPurchaseCriteriaId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardCreditPlusId", accountCreditPlusPurchaseCriteriaDTO.AccountCreditPlusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSTypeId", accountCreditPlusPurchaseCriteriaDTO.POSTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", accountCreditPlusPurchaseCriteriaDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", accountCreditPlusPurchaseCriteriaDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AccountCreditPlusPurchaseCriteria record to the database
        /// </summary>
        /// <param name="accountCreditPlusPurchaseCriteriaDTO">AccountCreditPlusPurchaseCriteriaDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted AccountCreditPlusPurchaseCriteria record</returns>
        public AccountCreditPlusPurchaseCriteriaDTO InsertAccountCreditPlusPurchaseCriteria(AccountCreditPlusPurchaseCriteriaDTO accountCreditPlusPurchaseCriteriaDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountCreditPlusPurchaseCriteriaDTO, loginId, siteId);
            string query = @"INSERT INTO CardCreditPlusPurchaseCriteria 
                                        ( 
                                            CardCreditPlusId,
                                            POSTypeId,
                                            ProductId,
                                            LastUpdatedBy,
                                            LastupdatedDate,
                                            site_id,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate

                                        ) 
                                VALUES 
                                        (
                                            @CardCreditPlusId,
                                            @POSTypeId,
                                            @ProductId,
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId,
                                            @CreatedBy,
                                            GetDate()
                                        )
                                SELECT * FROM CardCreditPlusPurchaseCriteria WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountCreditPlusPurchaseCriteriaDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountCreditPlusPurchaseCriteriaDTO(accountCreditPlusPurchaseCriteriaDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Inserting accountCreditPlusPurchaseCriteriaDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(accountCreditPlusPurchaseCriteriaDTO);
            return accountCreditPlusPurchaseCriteriaDTO;
        }

        /// <summary>
        /// Updates the AccountCreditPlusPurchaseCriteria record
        /// </summary>
        /// <param name="accountCreditPlusPurchaseCriteriaDTO">AccountCreditPlusPurchaseCriteriaDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns updated AccountCreditPlusPurchaseCriteria record</returns>
        public AccountCreditPlusPurchaseCriteriaDTO UpdateAccountCreditPlusPurchaseCriteria(AccountCreditPlusPurchaseCriteriaDTO accountCreditPlusPurchaseCriteriaDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountCreditPlusPurchaseCriteriaDTO, loginId, siteId);
            string query = @"UPDATE CardCreditPlusPurchaseCriteria 
                             SET CardCreditPlusId = @CardCreditPlusId,
                                 POSTypeId = @POSTypeId,
                                 ProductId = @ProductId,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastupdatedDate = GETDATE(),
                                 MasterEntityId = @MasterEntityId
                             WHERE Id = @Id
                             SELECT * FROM CardCreditPlusPurchaseCriteria WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountCreditPlusPurchaseCriteriaDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountCreditPlusPurchaseCriteriaDTO(accountCreditPlusPurchaseCriteriaDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating accountCreditPlusPurchaseCriteriaDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(accountCreditPlusPurchaseCriteriaDTO);
            return accountCreditPlusPurchaseCriteriaDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="accountCreditPlusPurchaseCriteriaDTO">AccountCreditPlusPurchaseCriteriaDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAccountCreditPlusPurchaseCriteriaDTO(AccountCreditPlusPurchaseCriteriaDTO accountCreditPlusPurchaseCriteriaDTO, DataTable dt)
        {
            log.LogMethodEntry(accountCreditPlusPurchaseCriteriaDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                accountCreditPlusPurchaseCriteriaDTO.AccountCreditPlusPurchaseCriteriaId= Convert.ToInt32(dt.Rows[0]["Id"]);
                accountCreditPlusPurchaseCriteriaDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                accountCreditPlusPurchaseCriteriaDTO.LastUpdateDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                accountCreditPlusPurchaseCriteriaDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                accountCreditPlusPurchaseCriteriaDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                accountCreditPlusPurchaseCriteriaDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                accountCreditPlusPurchaseCriteriaDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to AccountCreditPlusPurchaseCriteriaDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AccountCreditPlusPurchaseCriteriaDTO</returns>
        private AccountCreditPlusPurchaseCriteriaDTO GetAccountCreditPlusPurchaseCriteriaDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountCreditPlusPurchaseCriteriaDTO accountCreditPlusPurchaseCriteriaDTO = new AccountCreditPlusPurchaseCriteriaDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["CardCreditPlusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardCreditPlusId"]),
                                            dataRow["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSTypeId"]),
                                            dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                            );
            log.LogMethodExit(accountCreditPlusPurchaseCriteriaDTO);
            return accountCreditPlusPurchaseCriteriaDTO;
        }

        /// <summary>
        /// Gets the AccountCreditPlusPurchaseCriteria data of passed AccountCreditPlusPurchaseCriteria Id
        /// </summary>
        /// <param name="accountCreditPlusPurchaseCriteriaId">integer type parameter</param>
        /// <returns>Returns AccountCreditPlusPurchaseCriteriaDTO</returns>
        public AccountCreditPlusPurchaseCriteriaDTO GetAccountCreditPlusPurchaseCriteriaDTO(int accountCreditPlusPurchaseCriteriaId)
        {
            log.LogMethodEntry(accountCreditPlusPurchaseCriteriaId);
            AccountCreditPlusPurchaseCriteriaDTO returnValue = null;
            string query =SELECT_QUERY + "   WHERE CardCreditPlusPurchaseCriteria.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", accountCreditPlusPurchaseCriteriaId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetAccountCreditPlusPurchaseCriteriaDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the AccountCreditPlusPurchaseCriteriaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AccountCreditPlusPurchaseCriteriaDTO matching the search criteria</returns>
        public List<AccountCreditPlusPurchaseCriteriaDTO> GetAccountCreditPlusPurchaseCriteriaDTOList(List<KeyValuePair<AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AccountCreditPlusPurchaseCriteriaDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @"SELECT CardCreditPlusPurchaseCriteria.* 
                                  FROM CardCreditPlusPurchaseCriteria 
                                  LEFT OUTER JOIN CardCreditPlus ON CardCreditPlus.CardCreditPlusId = CardCreditPlusPurchaseCriteria.CardCreditPlusId ";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.ACCOUNT_CREDITPLUS_PURCHASE_CRITERIA_ID ||
                            searchParameter.Key == AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.ACCOUNT_CREDITPLUS_ID ||
                            searchParameter.Key == AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.POSTYPE_ID ||
                            searchParameter.Key == AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.PRODUCT_ID ||
                            searchParameter.Key == AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.ACCOUNT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.ACCOUNT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<AccountCreditPlusPurchaseCriteriaDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountCreditPlusPurchaseCriteriaDTO accountCreditPlusPurchaseCriteriaDTO = GetAccountCreditPlusPurchaseCriteriaDTO(dataRow);
                    list.Add(accountCreditPlusPurchaseCriteriaDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// GetAccountCreditPlusPurchaseCriteriaDTOListByAccountIdList
        /// </summary>
        /// <param name="accountIdList"></param>
        /// <returns></returns>
        internal List<AccountCreditPlusPurchaseCriteriaDTO> GetAccountCreditPlusPurchaseCriteriaDTOListByAccountIdList(List<int> accountIdList)
        {
            log.LogMethodEntry(accountIdList);
            List<AccountCreditPlusPurchaseCriteriaDTO> list = new List<AccountCreditPlusPurchaseCriteriaDTO>();
            string query = @"SELECT CardCreditPlusPurchaseCriteria.* 
                                  FROM CardCreditPlusPurchaseCriteria, CardCreditPlus, @accountIdList List 
                              WHERE CardCreditPlus.CardCreditPlusId = CardCreditPlusPurchaseCriteria.CardCreditPlusId
                                AND CardCreditPlus.Card_id = List.Id ";
            DataTable table = dataAccessHandler.BatchSelect(query, "@accountIdList", accountIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetAccountCreditPlusPurchaseCriteriaDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
