/********************************************************************************************
 * Project Name - AccountGame Data Handler
 * Description  - AccountGame handler of the AccountGame class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.60        21-Feb-2019   Mushahid Faizan   Added isActive Parameter.
 *2.70.2      23-Jul-2019   Girish Kundar     Modified : Added RefreshDTO() method and Fix for SQL Injection Issue
 *2.70.2      05-Dec-2019   Jinto Thomas      Removed siteid from update query
 *2.80.0      19-Mar-2020   Mathew NInan      Added new field ValidityStatus to track status of entitlements
 *2.90.0      09-Sep-2020   Girish Kundar     Modified : Issue fix membershipId/rewardsId/expity with fields are missing in DBSearch dictionary       
 *2.110.0     08-Dec-2020   Guru S A          Subscription changes                                                                                                                
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
    ///  AccountGame Data Handler - Handles insert, update and select of  AccountGame objects
    /// </summary>
    public class AccountGameDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<AccountGameDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountGameDTO.SearchByParameters, string>
            {
                {AccountGameDTO.SearchByParameters.ACCOUNT_GAME_ID, "cg.card_game_id"},
                {AccountGameDTO.SearchByParameters.ACCOUNT_ID, "cg.card_id"},
                {AccountGameDTO.SearchByParameters.ACCOUNT_ID_LIST, "cg.card_id"},
                {AccountGameDTO.SearchByParameters.GAME_ID, "cg.game_id"},
                {AccountGameDTO.SearchByParameters.GAME_PROFILE_ID, "cg.game_profile_id"},
                {AccountGameDTO.SearchByParameters.SITE_ID, "cg.site_id"},
                {AccountGameDTO.SearchByParameters.MASTER_ENTITY_ID, "cg.MasterEntityId"},
                {AccountGameDTO.SearchByParameters.ISACTIVE, "cg.IsActive"},
                {AccountGameDTO.SearchByParameters.VALIDITYSTATUS, "cg.ValidityStatus"},
                {AccountGameDTO.SearchByParameters.TRANSACTION_ID, "cg.TrxId"},
                {AccountGameDTO.SearchByParameters.MEMBERSHIP_ID, "cg.MembershipId"},
                {AccountGameDTO.SearchByParameters.MEMBERSHIP_REWARDS_ID, "cg.MembershipRewardsId"},
                {AccountGameDTO.SearchByParameters.EXPIRE_WITH_MEMBERSHIP, "cg.ExpireWithMembership"},
                {AccountGameDTO.SearchByParameters.TRANSACTION_ID_LIST, "cg.TrxId"},
                {AccountGameDTO.SearchByParameters.FROM_DATE, "cg.FromDate"},
                {AccountGameDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID, "cg.SubscriptionBillingScheduleId"},
                {AccountGameDTO.SearchByParameters.SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED, "cg.SubscriptionBillingScheduleId"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CardGames AS cg ";
        /// <summary>
        /// Default constructor of AccountGameDataHandler class
        /// </summary>
        public AccountGameDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AccountGame Record.
        /// </summary>
        /// <param name="accountGameDTO">AccountGameDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AccountGameDTO accountGameDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountGameDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Card_Game_Id", accountGameDTO.AccountGameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Card_Id", accountGameDTO.AccountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Game_Id", accountGameDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Quantity", accountGameDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", accountGameDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Game_Profile_Id", accountGameDTO.GameProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Frequency", accountGameDTO.Frequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastPlayedTime", accountGameDTO.LastPlayedTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BalanceGames", accountGameDTO.BalanceGames));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", accountGameDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Site_Id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", accountGameDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxLineId", accountGameDTO.TransactionLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EntitlementType", accountGameDTO.EntitlementType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OptionalAttribute", accountGameDTO.OptionalAttribute));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomDataSetId", accountGameDTO.CustomDataSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketAllowed", accountGameDTO.TicketAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromDate", accountGameDTO.FromDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", accountGameDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Monday", accountGameDTO.Monday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Tuesday", accountGameDTO.Tuesday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Wednesday", accountGameDTO.Wednesday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Thursday", accountGameDTO.Thursday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Friday", accountGameDTO.Friday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Saturday", accountGameDTO.Saturday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Sunday", accountGameDTO.Sunday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpireWithMembership", accountGameDTO.ExpireWithMembership));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipId", accountGameDTO.MembershipId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipRewardsId", accountGameDTO.MembershipRewardsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", accountGameDTO.IsActive ));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValidityStatus", (accountGameDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Valid ? "Y" : "H")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionBillingScheduleId", accountGameDTO.SubscriptionBillingScheduleId, true));            
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AccountGame record to the database
        /// </summary>
        /// <param name="accountGameDTO">AccountGameDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted AccountGame record</returns>
        public AccountGameDTO InsertAccountGame(AccountGameDTO accountGameDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountGameDTO, loginId, siteId);
            string query = @"INSERT INTO CardGames 
                                        ( 
                                            card_id
                                            ,game_id
                                            ,quantity
                                            ,ExpiryDate
                                            ,game_profile_id
                                            ,Frequency
                                            ,LastPlayedTime
                                            ,BalanceGames
                                            ,Guid
                                            ,site_id
                                            ,CreatedBy
                                            ,CreationDate
                                            ,LastUpdatedBy
                                            ,last_update_date
                                            ,TrxId
                                            ,TrxLineId
                                            ,EntitlementType
                                            ,OptionalAttribute
                                            ,CustomDataSetId
                                            ,TicketAllowed
                                            ,FromDate
                                            ,MasterEntityId
                                            ,Monday
                                            ,Tuesday
                                            ,Wednesday
                                            ,Thursday
                                            ,Friday
                                            ,Saturday
                                            ,Sunday
                                            ,ExpireWithMembership
                                            ,MembershipId
                                            ,MembershipRewardsId
                                            ,IsActive
                                            ,ValidityStatus
                                            ,SubscriptionBillingScheduleId
                                        ) 
                                VALUES 
                                        (
                                             @Card_Id
                                            ,@Game_Id
                                            ,@Quantity
                                            ,@ExpiryDate
                                            ,@Game_Profile_Id
                                            ,@Frequency
                                            ,@LastPlayedTime
                                            ,@BalanceGames
                                            ,NEWID()
                                            ,@Site_Id
                                            ,@CreatedBy
                                            ,GETDATE()
                                            ,@LastUpdatedBy
                                            ,GETDATE()
                                            ,@TrxId
                                            ,@TrxLineId
                                            ,@EntitlementType
                                            ,@OptionalAttribute
                                            ,@CustomDataSetId
                                            ,@TicketAllowed
                                            ,@FromDate
                                            ,@MasterEntityId
                                            ,@Monday
                                            ,@Tuesday
                                            ,@Wednesday
                                            ,@Thursday
                                            ,@Friday
                                            ,@Saturday
                                            ,@Sunday
                                            ,@ExpireWithMembership
                                            ,@MembershipId
                                            ,@MembershipRewardsId
                                            ,@IsActive
                                            ,@ValidityStatus
                                            ,@SubscriptionBillingScheduleId
                                        )
                                        SELECT * FROM CardGames WHERE Card_Game_Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountGameDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountGameDTO(accountGameDTO , dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Inserting accountGameDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(accountGameDTO);
            return accountGameDTO;
        }

        /// <summary>
        /// Updates the AccountGame record
        /// </summary>
        /// <param name="accountGameDTO">AccountGameDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted AccountGame record</returns>
        public AccountGameDTO UpdateAccountGame(AccountGameDTO accountGameDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountGameDTO, loginId, siteId);
            string query = @"UPDATE CardGames 
                                SET card_id = @Card_Id
                                   ,game_id = @Game_Id 
                                   ,quantity = @Quantity
                                   ,ExpiryDate = @ExpiryDate
                                   ,game_profile_id = @Game_Profile_Id
                                   ,Frequency = @Frequency
                                   ,LastPlayedTime = @LastPlayedTime
                                   ,BalanceGames = @BalanceGames 
                                   --,site_id = @Site_Id
                                   ,LastUpdatedBy = @LastUpdatedBy
                                   ,last_update_date = GETDATE()
                                   ,TrxId = @TrxId
                                   ,TrxLineId = @TrxLineId
                                   ,EntitlementType = @EntitlementType
                                   ,OptionalAttribute = @OptionalAttribute
                                   ,CustomDataSetId = @CustomDataSetId
                                   ,TicketAllowed = @TicketAllowed
                                   ,FromDate = @FromDate
                                   ,MasterEntityId = @MasterEntityId
                                   ,Monday = @Monday
                                   ,Tuesday = @Tuesday
                                   ,Wednesday = @Wednesday
                                   ,Thursday = @Thursday
                                   ,Friday = @Friday
                                   ,Saturday = @Saturday
                                   ,Sunday = @Sunday
                                   ,ExpireWithMembership = @ExpireWithMembership
                                   ,MembershipId = @MembershipId
                                   ,MembershipRewardsId = @MembershipRewardsId
                                   ,IsActive = @IsActive
                                   ,ValidityStatus = @ValidityStatus
                                   ,SubscriptionBillingScheduleId = @SubscriptionBillingScheduleId
                             WHERE Card_Game_Id = @Card_Game_Id
                             SELECT * FROM CardGames WHERE Card_Game_Id = @Card_Game_Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountGameDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountGameDTO(accountGameDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating accountGameDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(accountGameDTO);
            return accountGameDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="accountGameDTO">AccountGameDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAccountGameDTO(AccountGameDTO accountGameDTO, DataTable dt)
        {
            log.LogMethodEntry(accountGameDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                accountGameDTO.AccountGameId = Convert.ToInt32(dt.Rows[0]["Card_Game_Id"]);
                accountGameDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                accountGameDTO.LastUpdateDate = dataRow["last_update_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_update_date"]);
                accountGameDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                accountGameDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                accountGameDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                accountGameDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                accountGameDTO.ValidityStatus = dataRow["ValidityStatus"] == DBNull.Value ? AccountDTO.AccountValidityStatus.Valid : (Convert.ToString(dataRow["ValidityStatus"]) == "Y" ? AccountDTO.AccountValidityStatus.Valid : AccountDTO.AccountValidityStatus.Hold);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Deletes the AccountGame record of passed AccountGame Id
        /// </summary>
        /// <param name="accountGameId">integer type parameter</param>
        public void DeleteAccountGame(int accountGameId)
        {
            log.LogMethodEntry(accountGameId);
            string query = @"DELETE  
                             FROM CardGames
                             WHERE CardGames.Card_Game_Id = @Card_Game_Id";
            SqlParameter parameter = new SqlParameter("@Card_Game_Id", accountGameId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AccountGameDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AccountGameDTO</returns>
        private AccountGameDTO GetAccountGameDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountGameDTO accountGameDTO = new AccountGameDTO(Convert.ToInt32(dataRow["Card_Game_Id"]),
                                                         dataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["card_id"]),
                                                         dataRow["game_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["game_id"]),
                                                         dataRow["quantity"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["quantity"]),
                                                         dataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                                         dataRow["game_profile_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["game_profile_id"]),
                                                         dataRow["Frequency"] == DBNull.Value ? "N" : Convert.ToString(dataRow["Frequency"]),
                                                         dataRow["LastPlayedTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastPlayedTime"]),
                                                         dataRow["BalanceGames"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BalanceGames"]),
                                                         dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                                         dataRow["TrxLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxLineId"]),
                                                         dataRow["EntitlementType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EntitlementType"]),
                                                         dataRow["OptionalAttribute"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["OptionalAttribute"]),
                                                         dataRow["CustomDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomDataSetId"]),
                                                         dataRow["TicketAllowed"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["TicketAllowed"]),
                                                         dataRow["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["FromDate"]),
                                                         dataRow["Monday"] == DBNull.Value ? false : Convert.ToString(dataRow["Monday"]) == "Y",
                                                         dataRow["Tuesday"] == DBNull.Value ? false : Convert.ToString(dataRow["Tuesday"]) == "Y",
                                                         dataRow["Wednesday"] == DBNull.Value ? false : Convert.ToString(dataRow["Wednesday"]) == "Y",
                                                         dataRow["Thursday"] == DBNull.Value ? false : Convert.ToString(dataRow["Thursday"]) == "Y",
                                                         dataRow["Friday"] == DBNull.Value ? false : Convert.ToString(dataRow["Friday"]) == "Y",
                                                         dataRow["Saturday"] == DBNull.Value ? false : Convert.ToString(dataRow["Saturday"]) == "Y",
                                                         dataRow["Sunday"] == DBNull.Value ? false : Convert.ToString(dataRow["Sunday"]) == "Y",
                                                         dataRow["ExpireWithMembership"] == DBNull.Value ? false : dataRow["ExpireWithMembership"].ToString() == "Y",
                                                         dataRow["MembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipId"]),
                                                         dataRow["MembershipRewardsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipRewardsId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["last_update_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_update_date"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["IsActive"] == DBNull.Value ? true : (Convert.ToBoolean(dataRow["IsActive"])),
                                                         dataRow["ValidityStatus"] == DBNull.Value ? AccountDTO.AccountValidityStatus.Valid : (Convert.ToString(dataRow["ValidityStatus"]) == "Y" ? AccountDTO.AccountValidityStatus.Valid : AccountDTO.AccountValidityStatus.Hold),
                                                         dataRow["SubscriptionBillingScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SubscriptionBillingScheduleId"])
                                            );
            log.LogMethodExit(accountGameDTO);
            return accountGameDTO;
        }

        /// <summary>
        /// Gets the AccountGame data of passed AccountGame Id
        /// </summary>
        /// <param name="accountGameId">integer type parameter</param>
        /// <returns>Returns AccountGameDTO</returns>
        public AccountGameDTO GetAccountGameDTO(int accountGameId)
        {
            log.LogMethodEntry(accountGameId);
            AccountGameDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE cg.Card_Game_Id = @Card_Game_Id";
            SqlParameter parameter = new SqlParameter("@Card_Game_Id", accountGameId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetAccountGameDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the AccountGameDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AccountGameDTO matching the search criteria</returns>
        public List<AccountGameDTO> GetAccountGameDTOList(List<KeyValuePair<AccountGameDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AccountGameDTO> list = null;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AccountGameDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ((searchParameter.Key == AccountGameDTO.SearchByParameters.ACCOUNT_GAME_ID) ||
                             (searchParameter.Key == AccountGameDTO.SearchByParameters.ACCOUNT_ID) ||
                             (searchParameter.Key == AccountGameDTO.SearchByParameters.GAME_ID) ||
                             (searchParameter.Key == AccountGameDTO.SearchByParameters.GAME_PROFILE_ID) ||
                             (searchParameter.Key == AccountGameDTO.SearchByParameters.MEMBERSHIP_ID) ||
                             (searchParameter.Key == AccountGameDTO.SearchByParameters.MEMBERSHIP_REWARDS_ID) ||
                             (searchParameter.Key == AccountGameDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                             (searchParameter.Key == AccountGameDTO.SearchByParameters.TRANSACTION_ID) ||
                             (searchParameter.Key == AccountGameDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if ((searchParameter.Key == AccountGameDTO.SearchByParameters.ACCOUNT_ID_LIST) ||
                            (searchParameter.Key == AccountGameDTO.SearchByParameters.TRANSACTION_ID_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountGameDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountGameDTO.SearchByParameters.FROM_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == AccountGameDTO.SearchByParameters.EXPIRE_WITH_MEMBERSHIP) // char
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountGameDTO.SearchByParameters.VALIDITYSTATUS) // char
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountGameDTO.SearchByParameters.ISACTIVE) //bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == AccountGameDTO.SearchByParameters.SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED)
                        {
                            query.Append(joiner + " ISNULL((SELECT top 1 CASE WHEN ISNULL(sbs.TransactionId,-1) = -1 THEN 0 ELSE 1 END " +
                                                     " FROM SubscriptionBillingSchedule sbs " +
                                                     "WHERE sbs.SubscriptionBillingScheduleId = " + DBSearchParameters[searchParameter.Key] + "),0) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
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
                list = new List<AccountGameDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountGameDTO accountGameDTO = GetAccountGameDTO(dataRow);
                    list.Add(accountGameDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// GetAccountGameDTOListByAccountIdList
        /// </summary>
        /// <param name="accountIdList"></param>
        /// <returns></returns>
        public List<AccountGameDTO> GetAccountGameDTOListByAccountIdList(List<int> accountIdList)
        {
            log.LogMethodEntry(accountIdList);
            List<AccountGameDTO> list = new List<AccountGameDTO>();
            string query = SELECT_QUERY + @" , @accountIdList List WHERE cg.card_Id = List.Id ";
            DataTable table = dataAccessHandler.BatchSelect(query, "@accountIdList", accountIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetAccountGameDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
