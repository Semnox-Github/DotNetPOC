/********************************************************************************************
 * Project Name - AccountDiscount Data Handler
 * Description  - AccountDiscount handler of the AccountDiscount class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.60        14-Mar-2019    Mushahid Faizan     Modified -  isActive Parameter in GetAccountDiscountDTOList().
 *2.70.2      19-Jul-2019    Girish Kundar       Modified :Fix for SQL Injection Issue  
 *2.80.0      19-Mar-2020    Mathew NInan        Added new field ValidityStatus to track status of entitlements      
 *2.110.0     08-Dec-2020    Guru S A            Subscription changes                                                                               
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    ///  AccountDiscount Data Handler - Handles insert, update and select of  AccountDiscount objects
    /// </summary>
    public class AccountDiscountDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<AccountDiscountDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountDiscountDTO.SearchByParameters, string>
            {
                {AccountDiscountDTO.SearchByParameters.ACCOUNT_DISCOUNT_ID, "cd.CardDiscountId"},
                {AccountDiscountDTO.SearchByParameters.ACCOUNT_ID, "cd.card_id"},
                {AccountDiscountDTO.SearchByParameters.ACCOUNT_ID_LIST, "cd.card_id"},
                {AccountDiscountDTO.SearchByParameters.DISCOUNT_ID, "cd.discount_id"},
                {AccountDiscountDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN, "cd.expiry_date"},
                {AccountDiscountDTO.SearchByParameters.EXPIRY_DATE_LESS_THAN, "cd.expiry_date"},
                {AccountDiscountDTO.SearchByParameters.IS_ACTIVE, "cd.IsActive"},
                {AccountDiscountDTO.SearchByParameters.MASTER_ENTITY_ID,"cd.MasterEntityId"},
                {AccountDiscountDTO.SearchByParameters.TRANSACTION_ID,"cd.TransactionId"},
                {AccountDiscountDTO.SearchByParameters.LINE_ID,"cd.LineId"},
                {AccountDiscountDTO.SearchByParameters.TASK_ID,"cd.TaskId"},
                {AccountDiscountDTO.SearchByParameters.SITE_ID, "cd.site_id"},
                {AccountDiscountDTO.SearchByParameters.EXPIREWITHMEMBERSHIP, "cd.ExpireWithMembership"},
                {AccountDiscountDTO.SearchByParameters.MEMBERSHIPREWARDSID, "cd.MembershipRewardsId"},
                {AccountDiscountDTO.SearchByParameters.MEMBERSHIPSID, "cd.MembershipId"},
                {AccountDiscountDTO.SearchByParameters.VALIDITYSTATUS, "cd.ValidityStatus"},
                {AccountDiscountDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID, "cd.SubscriptionBillingScheduleId"},
                {AccountDiscountDTO.SearchByParameters.SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED, "cd.SubscriptionBillingScheduleId"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * from CardDiscounts AS cd ";
        /// <summary>
        /// Default constructor of AccountDiscountDataHandler class
        /// </summary>
        public AccountDiscountDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AccountDiscount Record.
        /// </summary>
        /// <param name="accountDiscountDTO">AccountDiscountDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(AccountDiscountDTO accountDiscountDTO, string userId, int siteId)
        {
            log.LogMethodEntry(accountDiscountDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardDiscountId", accountDiscountDTO.AccountDiscountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@card_id", accountDiscountDTO.AccountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@discount_id", accountDiscountDTO.DiscountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionId", accountDiscountDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LineId", accountDiscountDTO.LineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaskId", accountDiscountDTO.TaskId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@expiry_date", accountDiscountDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@last_updated_user", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InternetKey", accountDiscountDTO.InternetKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", accountDiscountDTO.IsActive ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", accountDiscountDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpireWithMembership", accountDiscountDTO.ExpireWithMembership));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipId", accountDiscountDTO.MembershipId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipRewardsId", accountDiscountDTO.MembershipRewardsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValidityStatus", (accountDiscountDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Valid ? "Y" : "H")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionBillingScheduleId", accountDiscountDTO.SubscriptionBillingScheduleId, true));            
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AccountDiscount record to the database
        /// </summary>
        /// <param name="accountDiscountDTO">AccountDiscountDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted AccountDiscount record</returns>
        public AccountDiscountDTO InsertAccountDiscount(AccountDiscountDTO accountDiscountDTO, string userId, int siteId)
        {
            log.LogMethodEntry(accountDiscountDTO, userId, siteId);
            string query = @"INSERT INTO CardDiscounts 
                                        ( 
                                            card_id,
                                            discount_id,
                                            TransactionId,
                                            LineId,
                                            TaskId,
                                            expiry_date,
                                            CreatedBy,
                                            CreationDate,
                                            last_updated_user,
                                            last_updated_date,
                                            InternetKey,
                                            IsActive,
                                            site_id,
                                            MasterEntityId,
                                            ExpireWithMembership,
                                            MembershipRewardsId,
                                            MembershipId,
                                            ValidityStatus,
                                            SubscriptionBillingScheduleId
                                        ) 
                                VALUES 
                                        (
                                            @card_id,
                                            @discount_id,
                                            @TransactionId,
                                            @LineId,
                                            @TaskId,
                                            @expiry_date,
                                            @CreatedBy,
                                            GETDATE(),
                                            @last_updated_user,
                                            GetDate(),
                                            @InternetKey,
                                            @IsActive,
                                            @site_id,
                                            @MasterEntityId,
                                            @ExpireWithMembership,
                                            @MembershipRewardsId,
                                            @MembershipId,
                                            @ValidityStatus,
                                            @SubscriptionBillingScheduleId
                                        )
                                        SELECT * FROM CardDiscounts WHERE CardDiscountId = scope_identity()";


            List<SqlParameter> parameters = BuildSQLParameters(accountDiscountDTO, userId, siteId);
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                RefreshAccountDiscountDTO(accountDiscountDTO ,dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Inserting the account Discount ", ex);
                log.LogVariableState("AccountDiscountDTO", accountDiscountDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(accountDiscountDTO);
            return accountDiscountDTO;
        }

        /// <summary>
        /// Updates the AccountDiscount record
        /// </summary>
        /// <param name="accountDiscountDTO">AccountDiscountDTO type parameter</param>
        /// <param name="userId">User updating the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public AccountDiscountDTO UpdateAccountDiscount(AccountDiscountDTO accountDiscountDTO, string userId, int siteId)
        {
            log.LogMethodEntry(accountDiscountDTO, userId, siteId);
            string query = @"UPDATE CardDiscounts 
                             SET card_id=@card_id,
                                 discount_id=@discount_id,
                                 TransactionId=@TransactionId,
                                 LineId=@LineId,
                                 TaskId=@TaskId,
                                 expiry_date=@expiry_date,
                                 last_updated_user=@last_updated_user,
                                 last_updated_date=GetDate(),
                                 InternetKey=@InternetKey,
                                 IsActive=@IsActive,
                                 MasterEntityId=@MasterEntityId,
                                 ExpireWithMembership = @ExpireWithMembership,
                                 MembershipRewardsId = @MembershipRewardsId,
                                 MembershipId = @MembershipId,
                                 ValidityStatus = @ValidityStatus,
                                 SubscriptionBillingScheduleId = @SubscriptionBillingScheduleId
                             WHERE CardDiscountId = @CardDiscountId
                             SELECT * FROM CardDiscounts WHERE CardDiscountId = @CardDiscountId";
            List<SqlParameter> parameters = BuildSQLParameters(accountDiscountDTO, userId, siteId);
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                RefreshAccountDiscountDTO(accountDiscountDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating the account Discount ", ex);
                log.LogVariableState("AccountDiscountDTO", accountDiscountDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(accountDiscountDTO);
            return accountDiscountDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="accountDiscountDTO">AccountDiscountDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAccountDiscountDTO(AccountDiscountDTO accountDiscountDTO, DataTable dt)
        {
            log.LogMethodEntry(accountDiscountDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                accountDiscountDTO.AccountDiscountId = Convert.ToInt32(dt.Rows[0]["CardDiscountId"]);
                accountDiscountDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                accountDiscountDTO.LastUpdatedDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                accountDiscountDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                accountDiscountDTO.LastUpdatedUser = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : dataRow["last_updated_user"].ToString();
                accountDiscountDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                accountDiscountDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                accountDiscountDTO.ValidityStatus = dataRow["validityStatus"] == DBNull.Value ? AccountDTO.AccountValidityStatus.Valid : (Convert.ToString(dataRow["ValidityStatus"]) == "Y" ? AccountDTO.AccountValidityStatus.Valid : AccountDTO.AccountValidityStatus.Hold);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AccountDiscountDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AccountDiscountDTO</returns>
        private AccountDiscountDTO GetAccountDiscountDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountDiscountDTO accountDiscountDTO = new AccountDiscountDTO(Convert.ToInt32(dataRow["CardDiscountId"]),
                                            dataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["card_id"]),
                                            dataRow["discount_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["discount_id"]),
                                            dataRow["expiry_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["expiry_date"]),
                                            dataRow["TransactionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionId"]),
                                            dataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LineId"]),
                                            dataRow["TaskId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TaskId"]),
                                            dataRow["last_updated_user"] == DBNull.Value ? "" : Convert.ToString(dataRow["last_updated_user"]),
                                            dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]),
                                            dataRow["InternetKey"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["InternetKey"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToString(dataRow["IsActive"]) == "Y",
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["ExpireWithMembership"] == DBNull.Value ? "N" : dataRow["ExpireWithMembership"].ToString(),
                                            dataRow["MembershipRewardsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipRewardsId"]),
                                            dataRow["MembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["ValidityStatus"] == DBNull.Value ? AccountDTO.AccountValidityStatus.Valid : (dataRow["ValidityStatus"].ToString() == "Y" ? AccountDTO.AccountValidityStatus.Valid : AccountDTO.AccountValidityStatus.Hold),
                                            dataRow["SubscriptionBillingScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SubscriptionBillingScheduleId"]) 
                                            );
            log.LogMethodExit(accountDiscountDTO);
            return accountDiscountDTO;
        }

        /// <summary>
        /// Gets the AccountDiscount data of passed AccountDiscount Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns AccountDiscountDTO</returns>
        public AccountDiscountDTO GetAccountDiscountDTO(int id)
        {
            log.LogMethodEntry(id);
            AccountDiscountDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE cd.CardDiscountId = @CardDiscountId";
            SqlParameter parameter = new SqlParameter("@CardDiscountId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetAccountDiscountDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the AccountDiscountDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AccountDiscountDTO matching the search criteria</returns>
        public List<AccountDiscountDTO> GetAccountDiscountDTOList(List<KeyValuePair<AccountDiscountDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AccountDiscountDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AccountDiscountDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == AccountDiscountDTO.SearchByParameters.ACCOUNT_DISCOUNT_ID ||
                            searchParameter.Key == AccountDiscountDTO.SearchByParameters.DISCOUNT_ID ||
                            searchParameter.Key == AccountDiscountDTO.SearchByParameters.TRANSACTION_ID ||
                            searchParameter.Key == AccountDiscountDTO.SearchByParameters.LINE_ID ||
                            searchParameter.Key == AccountDiscountDTO.SearchByParameters.TASK_ID ||
                            searchParameter.Key == AccountDiscountDTO.SearchByParameters.ACCOUNT_ID ||
                            searchParameter.Key == AccountDiscountDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == AccountDiscountDTO.SearchByParameters.MEMBERSHIPSID ||
                            searchParameter.Key == AccountDiscountDTO.SearchByParameters.MEMBERSHIPREWARDSID ||
                            searchParameter.Key == AccountDiscountDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountDiscountDTO.SearchByParameters.ACCOUNT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountDiscountDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " IS NULL OR " + DBSearchParameters[searchParameter.Key]+ " > " + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == AccountDiscountDTO.SearchByParameters.EXPIRY_DATE_LESS_THAN)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + " IS NULL OR " + DBSearchParameters[searchParameter.Key] + " < " + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == AccountDiscountDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountDiscountDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == AccountDiscountDTO.SearchByParameters.EXPIREWITHMEMBERSHIP)
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountDiscountDTO.SearchByParameters.VALIDITYSTATUS)
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountDiscountDTO.SearchByParameters.SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED)
                        {
                            query.Append(joiner + " ISNULL((SELECT top 1 CASE WHEN ISNULL(sbs.TransactionId,-1) = -1 THEN 0 ELSE 1 END " +
                                                     " FROM SubscriptionBillingSchedule sbs " +
                                                     "WHERE sbs.SubscriptionBillingScheduleId = " + DBSearchParameters[searchParameter.Key] + "),0) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<AccountDiscountDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountDiscountDTO accountDiscountDTO = GetAccountDiscountDTO(dataRow);
                    list.Add(accountDiscountDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// GetAccountDiscountDTOListByAccountIdList
        /// </summary>
        /// <param name="accountIdList"></param>
        /// <returns></returns>
        public List<AccountDiscountDTO> GetAccountDiscountDTOListByAccountIdList(List<int> accountIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(accountIdList);
            List<AccountDiscountDTO> list = new List<AccountDiscountDTO>();
            string query = SELECT_QUERY + @" , @accountIdList List WHERE cd.card_Id = List.Id ";
            if (activeChildRecords)
            {
                query = query + (" AND Isnull(cd.IsActive,'Y') = 'Y' "); 
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@accountIdList", accountIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetAccountDiscountDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
