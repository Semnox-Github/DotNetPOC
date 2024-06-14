/********************************************************************************************
 * Project Name - Semnox.Parafait.Customer.Accounts -AccountCreditPlusConsumptionDataHandler
 * Description  - AccountCreditPlusConsumptionDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        21-Feb-2019   Mushahid Faizan         Added isActive Parameter & added log Method Entry & Exit
 *2.70.2      23-Jul-2019   Girish Kundar           Modified : Added RefreshDTO() method and Fix for SQL Injection Issue 
 *2.110.0     10-Dec-2020   Guru S A                For Subscription changes                   
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
    ///  AccountCreditPlusConsumption Data Handler - Handles insert, update and select of  AccountCreditPlusConsumption objects
    /// </summary>
    public class AccountCreditPlusConsumptionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<AccountCreditPlusConsumptionDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountCreditPlusConsumptionDTO.SearchByParameters, string>
            {
                {AccountCreditPlusConsumptionDTO.SearchByParameters.ACCOUNT_CREDITPLUS_CONSUMPTION_ID, "CardCreditPlusConsumption.PKId"},
                {AccountCreditPlusConsumptionDTO.SearchByParameters.ACCOUNT_CREDITPLUS_ID, "CardCreditPlusConsumption.CardCreditPlusId"},
                {AccountCreditPlusConsumptionDTO.SearchByParameters.ACCOUNT_ID, "CardCreditPlus.Card_id"},
                {AccountCreditPlusConsumptionDTO.SearchByParameters.ACCOUNT_ID_LIST, "CardCreditPlus.Card_id"},
                {AccountCreditPlusConsumptionDTO.SearchByParameters.SITE_ID, "CardCreditPlusConsumption.site_id"},
                {AccountCreditPlusConsumptionDTO.SearchByParameters.MASTER_ENTITY_ID, "CardCreditPlusConsumption.MasterEntityId"},
                {AccountCreditPlusConsumptionDTO.SearchByParameters.ISACTIVE, "CardCreditPlusConsumption.IsActive"}
            };
        private readonly DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of AccountCreditPlusConsumptionDataHandler class
        /// </summary>
        public AccountCreditPlusConsumptionDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AccountCreditPlusConsumption Record.
        /// </summary>
        /// <param name="accountCreditPlusConsumptionDTO">AccountCreditPlusConsumptionDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO, string userId, int siteId)
        {
            log.LogMethodEntry(accountCreditPlusConsumptionDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PKId", accountCreditPlusConsumptionDTO.AccountCreditPlusConsumptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardCreditPlusId", accountCreditPlusConsumptionDTO.AccountCreditPlusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSTypeId", accountCreditPlusConsumptionDTO.POSTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", accountCreditPlusConsumptionDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", accountCreditPlusConsumptionDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameProfileId", accountCreditPlusConsumptionDTO.GameProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameId", accountCreditPlusConsumptionDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountPercentage", accountCreditPlusConsumptionDTO.DiscountPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountedPrice", accountCreditPlusConsumptionDTO.DiscountedPrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ConsumptionBalance", accountCreditPlusConsumptionDTO.ConsumptionBalance));
            parameters.Add(dataAccessHandler.GetSQLParameter("@QuantityLimit", accountCreditPlusConsumptionDTO.QuantityLimit));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CategoryId", accountCreditPlusConsumptionDTO.CategoryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderTypeId", accountCreditPlusConsumptionDTO.OrderTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountAmount", accountCreditPlusConsumptionDTO.DiscountAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", accountCreditPlusConsumptionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", accountCreditPlusConsumptionDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ConsumptionQty", accountCreditPlusConsumptionDTO.ConsumptionQty));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AccountCreditPlusConsumption record to the database
        /// </summary>
        /// <param name="accountCreditPlusConsumptionDTO">AccountCreditPlusConsumptionDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted AccountCreditPlusConsumption record</returns>
        public AccountCreditPlusConsumptionDTO InsertAccountCreditPlusConsumption(AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO, string userId, int siteId)
        {
            log.LogMethodEntry(accountCreditPlusConsumptionDTO, userId, siteId);
            string query = @"INSERT INTO CardCreditPlusConsumption 
                                        ( 
                                            CardCreditPlusId,
                                            POSTypeId,
                                            ExpiryDate,
                                            ProductId,
                                            GameId,
                                            DiscountPercentage,
                                            DiscountedPrice,
                                            LastupdatedDate,
                                            LastUpdatedBy,
                                            ConsumptionBalance,
                                            QuantityLimit,
                                            CategoryId,
                                            DiscountAmount,
                                            OrderTypeId,
                                            site_id,
                                            MasterEntityId,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            ConsumptionQty
                                        ) 
                                VALUES 
                                        (
                                            @CardCreditPlusId,
                                            @POSTypeId,
                                            @ExpiryDate,
                                            @ProductId,
                                            @GameId,
                                            @DiscountPercentage,
                                            @DiscountedPrice,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            @ConsumptionBalance,
                                            @QuantityLimit,
                                            @CategoryId,
                                            @DiscountAmount,
                                            @OrderTypeId,
                                            @site_id,
                                            @MasterEntityId,
                                            @IsActive,
                                            @CreatedBy,
                                            GetDate(),
                                            @ConsumptionQty
                                        )
                                        SELECT * FROM CardCreditPlusConsumption WHERE PKId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountCreditPlusConsumptionDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAccountCreditPlusConsumptionDTO(accountCreditPlusConsumptionDTO ,dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit(accountCreditPlusConsumptionDTO);
            return accountCreditPlusConsumptionDTO;
        }

        /// <summary>
        /// Updates the AccountCreditPlusConsumption record
        /// </summary>
        /// <param name="accountCreditPlusConsumptionDTO">AccountCreditPlusConsumptionDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns updated AccountCreditPlusConsumption record</returns>
        public AccountCreditPlusConsumptionDTO UpdateAccountCreditPlusConsumption(AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO, string userId, int siteId)
        {
            log.LogMethodEntry(accountCreditPlusConsumptionDTO, userId, siteId);
            string query = @"UPDATE CardCreditPlusConsumption 
                             SET CardCreditPlusId = @CardCreditPlusId,
                                 POSTypeId = @POSTypeId,
                                 ExpiryDate = @ExpiryDate,
                                 ProductId = @ProductId,
                                 GameProfileId = @GameProfileId,
                                 GameId = @GameId,
                                 DiscountPercentage = @DiscountPercentage,
                                 DiscountedPrice = @DiscountedPrice,
                                 LastupdatedDate = GETDATE(),
                                 LastUpdatedBy = @LastUpdatedBy,
                                 ConsumptionBalance = @ConsumptionBalance,
                                 QuantityLimit = @QuantityLimit,
                                 CategoryId = @CategoryId,
                                 DiscountAmount = @DiscountAmount,
                                 OrderTypeId = @OrderTypeId,
                                 MasterEntityId = @MasterEntityId,
                                 IsActive=@IsActive,
                                 ConsumptionQty = @ConsumptionQty
                             WHERE PKId = @PKId 
                             SELECT * FROM CardCreditPlusConsumption WHERE PKId = @PKId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountCreditPlusConsumptionDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAccountCreditPlusConsumptionDTO(accountCreditPlusConsumptionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(accountCreditPlusConsumptionDTO);
            return accountCreditPlusConsumptionDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="accountCreditPlusConsumptionDTO">AccountCreditPlusConsumptionDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAccountCreditPlusConsumptionDTO(AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO, DataTable dt)
        {
            log.LogMethodEntry(accountCreditPlusConsumptionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                accountCreditPlusConsumptionDTO.AccountCreditPlusConsumptionId = Convert.ToInt32(dt.Rows[0]["PKId"]);
                accountCreditPlusConsumptionDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                accountCreditPlusConsumptionDTO.LastUpdateDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                accountCreditPlusConsumptionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                accountCreditPlusConsumptionDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                accountCreditPlusConsumptionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                accountCreditPlusConsumptionDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Deletes the AccountCreditPlusConsumption record of passed AccountCreditPlusConsumption Id
        /// </summary>
        /// <param name="accountCreditPlusConsumptionId">integer type parameter</param>
        public void DeleteAccountCreditPlusConsumption(int accountCreditPlusConsumptionId)
        {
            log.LogMethodEntry(accountCreditPlusConsumptionId);
            string query = @"DELETE  
                             FROM CardCreditPlusConsumption
                             WHERE CardCreditPlusConsumption.PkId = @PkId";
            SqlParameter parameter = new SqlParameter("@PkId", accountCreditPlusConsumptionId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AccountCreditPlusConsumptionDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AccountCreditPlusConsumptionDTO</returns>
        private AccountCreditPlusConsumptionDTO GetAccountCreditPlusConsumptionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO = new AccountCreditPlusConsumptionDTO(Convert.ToInt32(dataRow["PKId"]),
                                            dataRow["CardCreditPlusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardCreditPlusId"]),
                                            dataRow["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSTypeId"]),
                                            dataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                            dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                            dataRow["GameProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameProfileId"]),
                                            dataRow["GameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameId"]),
                                            dataRow["DiscountPercentage"] == DBNull.Value ? (decimal?) null : Convert.ToDecimal(dataRow["DiscountPercentage"]),
                                            dataRow["DiscountedPrice"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["DiscountedPrice"]),
                                            dataRow["ConsumptionBalance"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ConsumptionBalance"]),
                                            dataRow["QuantityLimit"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["QuantityLimit"]),
                                            dataRow["CategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CategoryId"]),
                                            dataRow["DiscountAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["DiscountAmount"]),
                                            dataRow["OrderTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderTypeId"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["ConsumptionQty"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ConsumptionQty"])
                                            );
            log.LogMethodExit(accountCreditPlusConsumptionDTO);
            return accountCreditPlusConsumptionDTO;
        }

        /// <summary>
        /// Gets the AccountCreditPlusConsumption data of passed AccountCreditPlusConsumption Id
        /// </summary>
        /// <param name="accountCreditPlusConsumptionId">integer type parameter</param>
        /// <returns>Returns AccountCreditPlusConsumptionDTO</returns>
        public AccountCreditPlusConsumptionDTO GetAccountCreditPlusConsumptionDTO(int accountCreditPlusConsumptionId)
        {
            log.LogMethodEntry(accountCreditPlusConsumptionId);
            AccountCreditPlusConsumptionDTO returnValue = null;
            string query = @"SELECT * 
                             FROM CardCreditPlusConsumption
                             WHERE CardCreditPlusConsumption.PKId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", accountCreditPlusConsumptionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetAccountCreditPlusConsumptionDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the AccountCreditPlusConsumptionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AccountCreditPlusConsumptionDTO matching the search criteria</returns>
        public List<AccountCreditPlusConsumptionDTO> GetAccountCreditPlusConsumptionDTOList(List<KeyValuePair<AccountCreditPlusConsumptionDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AccountCreditPlusConsumptionDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @"SELECT CardCreditPlusConsumption.* 
                                  FROM CardCreditPlusConsumption 
                                  LEFT OUTER JOIN CardCreditPlus ON CardCreditPlus.CardCreditPlusId =  CardCreditPlusConsumption.CardCreditPlusId ";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AccountCreditPlusConsumptionDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AccountCreditPlusConsumptionDTO.SearchByParameters.ACCOUNT_CREDITPLUS_CONSUMPTION_ID ||
                            searchParameter.Key == AccountCreditPlusConsumptionDTO.SearchByParameters.ACCOUNT_CREDITPLUS_ID||
                            searchParameter.Key == AccountCreditPlusConsumptionDTO.SearchByParameters.MASTER_ENTITY_ID||
                            searchParameter.Key == AccountCreditPlusConsumptionDTO.SearchByParameters.ACCOUNT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountCreditPlusConsumptionDTO.SearchByParameters.ACCOUNT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountCreditPlusConsumptionDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountCreditPlusConsumptionDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "Throwing exception - The query parameter does not exist");
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
                list = new List<AccountCreditPlusConsumptionDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO = GetAccountCreditPlusConsumptionDTO(dataRow);
                    list.Add(accountCreditPlusConsumptionDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// GetAccountCreditPlusConsumptionDTOListByAccountIdList
        /// </summary>
        /// <param name="accountIdList"></param>
        /// <returns></returns>
        internal List<AccountCreditPlusConsumptionDTO> GetAccountCreditPlusConsumptionDTOListByAccountIdList(List<int> accountIdList)
        {
            log.LogMethodEntry(accountIdList);
            List<AccountCreditPlusConsumptionDTO> list = new List<AccountCreditPlusConsumptionDTO>();
            string query = @"SELECT CardCreditPlusConsumption.* 
                               FROM CardCreditPlusConsumption, CardCreditPlus, @accountIdList List 
                              WHERE CardCreditPlus.CardCreditPlusId =  CardCreditPlusConsumption.CardCreditPlusId
                                AND CardCreditPlus.Card_id = List.Id "; 
            DataTable table = dataAccessHandler.BatchSelect(query, "@accountIdList", accountIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetAccountCreditPlusConsumptionDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
