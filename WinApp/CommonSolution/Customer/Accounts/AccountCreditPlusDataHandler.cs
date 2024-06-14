/********************************************************************************************
 * Project Name - Semnox.Parafait.Customer.Accounts -AccountCreditPlusDataHandler
 * Description  - AccountCreditPlusDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.4.0       28-Sep-2018   Guru S A                 Modified for Pause allowed changes 
 *2.60        21-Feb-2019   Mushahid Faizan          Added isActive Parameter.
 *2.70.2      23-Jul-2019   Girish Kundar            Modified : Added RefreshDTO() method and Fix for SQL Injection Issue
 *2.70.2      05-Dec-2019   Jinto Thomas             Removed siteid from update query
 *2.80.0      19-Mar-2020   Jinto Thomas             chnages for newly added column SourceCreditPlusId
 *2.80.0      19-Mar-2020   Mathew NInan             Added new field ValidityStatus to track status of entitlements        
 *2.110.0     08-Dec-2020   Guru S A                 Subscription changes      
 *2.110.0     05-Feb-2021   Akshay G                 Added CREDITPLUS_TYPE_LIST searchParameter as part of License enhancement.
 *2.140       14-Sep-2021   Fiona                    Modified: Issue fixes
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
    ///  AccountCreditPlus Data Handler - Handles insert, update and select of  AccountCreditPlus objects
    /// </summary>
    public class AccountCreditPlusDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<AccountCreditPlusDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountCreditPlusDTO.SearchByParameters, string>
            {
                {AccountCreditPlusDTO.SearchByParameters.ACCOUNT_CREDITPLUS_ID, "ccp.CardCreditPlusId"},
                {AccountCreditPlusDTO.SearchByParameters.ACCOUNT_ID, "ccp.card_id"},
                {AccountCreditPlusDTO.SearchByParameters.CREDITPLUS_TYPE, "ccp.creditPlusType"} ,
                {AccountCreditPlusDTO.SearchByParameters.EXPIRE_WITH_MEMBERSHIP, "ccp.ExpireWithMembership"},
                {AccountCreditPlusDTO.SearchByParameters.FORMEMBERSHIP_ONLY, "ccp.ForMembershipOnly"},
                {AccountCreditPlusDTO.SearchByParameters.MEMBERSHIPS_ID, "ccp.MembershipId"},
                {AccountCreditPlusDTO.SearchByParameters.MEMBERSHIP_REWARDS_ID, "ccp.MembershipRewardsId" },
                {AccountCreditPlusDTO.SearchByParameters.SITE_ID, "ccp.site_id"},
                {AccountCreditPlusDTO.SearchByParameters.TRANSACTION_ID, "ccp.TrxId"},
                {AccountCreditPlusDTO.SearchByParameters.ACCOUNT_ID_LIST, "ccp.card_id"},
                {AccountCreditPlusDTO.SearchByParameters.PAUSE_ALLOWED, "ccp.PauseAllowed"},
                {AccountCreditPlusDTO.SearchByParameters.MASTER_ENTITY_ID, "ccp.MasterEntityId"},
                {AccountCreditPlusDTO.SearchByParameters.ISACTIVE, "ccp.IsActive"},
                {AccountCreditPlusDTO.SearchByParameters.SOURCE_CREDITPLUS_ID, "ccp.SourceCreditPlusId"},
                {AccountCreditPlusDTO.SearchByParameters.VALIDITYSTATUS, "ccp.ValidityStatus"},
                {AccountCreditPlusDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID, "ccp.SubscriptionBillingScheduleId"},
                {AccountCreditPlusDTO.SearchByParameters.CREDITPLUS_TYPE_LIST, "ccp.creditPlusType"},
                {AccountCreditPlusDTO.SearchByParameters.SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED, "ccp.SubscriptionBillingScheduleId"},
                {AccountCreditPlusDTO.SearchByParameters.HAS_CONSUMPTION_RULE, "ccp.CardCreditPlusId"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CardCreditPlus AS ccp ";
        /// <summary>
        /// Default constructor of AccountCreditPlusDataHandler class
        /// </summary>
        public AccountCreditPlusDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AccountCreditPlus Record.
        /// </summary>
        /// <param name="accountCreditPlusDTO">AccountCreditPlusDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AccountCreditPlusDTO accountCreditPlusDTO, string userId, int siteId)
        {
            log.LogMethodEntry(accountCreditPlusDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardCreditPlusId", accountCreditPlusDTO.AccountCreditPlusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreditPlus", accountCreditPlusDTO.CreditPlus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreditPlusType", CreditPlusTypeConverter.ToString(accountCreditPlusDTO.CreditPlusType)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Refundable", accountCreditPlusDTO.Refundable? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", accountCreditPlusDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Card_id", accountCreditPlusDTO.AccountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", accountCreditPlusDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LineId", accountCreditPlusDTO.TransactionLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreditPlusBalance", accountCreditPlusDTO.CreditPlusBalance));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PeriodFrom", accountCreditPlusDTO.PeriodFrom));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PeriodTo", accountCreditPlusDTO.PeriodTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeFrom", accountCreditPlusDTO.TimeFrom));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeTo", accountCreditPlusDTO.TimeTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NumberOfDays", accountCreditPlusDTO.NumberOfDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Monday", accountCreditPlusDTO.Monday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Tuesday", accountCreditPlusDTO.Tuesday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Wednesday", accountCreditPlusDTO.Wednesday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Thursday", accountCreditPlusDTO.Thursday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Friday", accountCreditPlusDTO.Friday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Saturday", accountCreditPlusDTO.Saturday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Sunday", accountCreditPlusDTO.Sunday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MinimumSaleAmount", accountCreditPlusDTO.MinimumSaleAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyRuleId", accountCreditPlusDTO.LoyaltyRuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExtendOnReload", accountCreditPlusDTO.ExtendOnReload ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PlayStartTime", accountCreditPlusDTO.PlayStartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketAllowed", accountCreditPlusDTO.TicketAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", accountCreditPlusDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ForMembershipOnly", accountCreditPlusDTO.ForMembershipOnly ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpireWithMembership", accountCreditPlusDTO.ExpireWithMembership ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipId", accountCreditPlusDTO.MembershipId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipRewardsId", accountCreditPlusDTO.MembershipRewardsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PauseAllowed", accountCreditPlusDTO.PauseAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", accountCreditPlusDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SourceCreditPlusId ", accountCreditPlusDTO.SourceCreditPlusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValidityStatus", (accountCreditPlusDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Valid ? "Y" : "H")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionBillingScheduleId ", accountCreditPlusDTO.SubscriptionBillingScheduleId, true));            
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AccountCreditPlus record to the database
        /// </summary>
        /// <param name="accountCreditPlusDTO">AccountCreditPlusDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted AccountCreditPlus record</returns>
        public AccountCreditPlusDTO InsertAccountCreditPlus(AccountCreditPlusDTO accountCreditPlusDTO, string userId, int siteId)
        {
            log.LogMethodEntry(accountCreditPlusDTO, userId, siteId);
            string query = @"insert into CardCreditPlus 
                                                         (
                                                            CreditPlus 
                                                           , CreditPlusType 
                                                           , Refundable 
                                                           , Remarks 
                                                           , Card_id 
                                                           , TrxId 
                                                           , LineId 
                                                           , CreditPlusBalance 
                                                           , PeriodFrom 
                                                           , PeriodTo 
                                                           , TimeFrom 
                                                           , TimeTo 
                                                           , NumberOfDays 
                                                           , Monday 
                                                           , Tuesday 
                                                           , Wednesday 
                                                           , Thursday 
                                                           , Friday 
                                                           , Saturday 
                                                           , Sunday 
                                                           , MinimumSaleAmount 
                                                           , LoyaltyRuleId 
                                                           , CreationDate 
                                                           , LastupdatedDate 
                                                           , LastUpdatedBy 
                                                           , site_id 
                                                           , ExtendOnReload 
                                                           , PlayStartTime 
                                                           , TicketAllowed 
                                                           , MasterEntityId 
                                                           , ForMembershipOnly
                                                           , ExpireWithMembership
                                                           , MembershipRewardsId
                                                           , MembershipId
                                                           , PauseAllowed
                                                           ,IsActive
                                                           ,CreatedBy
                                                           ,SourceCreditPlusId 
                                                           ,ValidityStatus
                                                           ,SubscriptionBillingScheduleId
                                                         )
                                                       values
                                                         ( 
                                                             @CreditPlus 
                                                           , @CreditPlusType 
                                                           , @Refundable 
                                                           , @Remarks 
                                                           , @Card_id 
                                                           , @TrxId 
                                                           , @LineId 
                                                           , @CreditPlusBalance 
                                                           , @PeriodFrom 
                                                           , @PeriodTo 
                                                           , @TimeFrom 
                                                           , @TimeTo 
                                                           , @NumberOfDays 
                                                           , @Monday 
                                                           , @Tuesday 
                                                           , @Wednesday 
                                                           , @Thursday 
                                                           , @Friday 
                                                           , @Saturday 
                                                           , @Sunday 
                                                           , @MinimumSaleAmount 
                                                           , @LoyaltyRuleId 
                                                           , getdate() 
                                                           , getdate() 
                                                           , @LastUpdatedBy 
                                                           , @site_id 
                                                           , @ExtendOnReload 
                                                           , @PlayStartTime 
                                                           , @TicketAllowed 
                                                           , @MasterEntityId 
                                                           , @ForMembershipOnly 
                                                           , @ExpireWithMembership 
                                                           , @MembershipRewardsId 
                                                           , @MembershipId 
                                                           , @PauseAllowed
                                                           , @IsActive
                                                           , @CreatedBy
                                                           , @SourceCreditPlusId
                                                           , @ValidityStatus
                                                           , @SubscriptionBillingScheduleId
                                                        )
                                                        SELECT * FROM CardCreditPlus WHERE CardCreditPlusId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountCreditPlusDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAccountCreditPlusDTO(accountCreditPlusDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Inserting accountCreditPlusDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(accountCreditPlusDTO);
            return accountCreditPlusDTO;
        }

        /// <summary>
        /// Updates the AccountCreditPlus record
        /// </summary>
        /// <param name="accountCreditPlusDTO">AccountCreditPlusDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns updated AccountCreditPlus record</returns>
        public AccountCreditPlusDTO UpdateAccountCreditPlus(AccountCreditPlusDTO accountCreditPlusDTO, string userId, int siteId)
        {
            log.LogMethodEntry(accountCreditPlusDTO, userId, siteId);
            string query = @"update   CardcreditPlus set
                                        CreditPlus  =  @CreditPlus 
                                    , CreditPlusType  =  @CreditPlusType 
                                    , Refundable  =  @Refundable 
                                    , Remarks  =  @Remarks 
                                    , Card_id  =  @Card_id 
                                    , TrxId  =  @TrxId 
                                    , LineId  =  @LineId 
                                    , CreditPlusBalance  =  @CreditPlusBalance  
                                    , PeriodFrom  =  @PeriodFrom 
                                    , PeriodTo  =  @PeriodTo 
                                    , TimeFrom  =  @TimeFrom  
                                    , TimeTo  =  @TimeTo  
                                    , NumberOfDays  =  @NumberOfDays  
                                    , Monday  =  @Monday  
                                    , Tuesday  =  @Tuesday  
                                    , Wednesday  =  @Wednesday  
                                    , Thursday  =  @Thursday  
                                    , Friday  =  @Friday  
                                    , Saturday  =  @Saturday  
                                    , Sunday  =  @Sunday  
                                    , MinimumSaleAmount  =  @MinimumSaleAmount  
                                    , LoyaltyRuleId  =  @LoyaltyRuleId  
                                    , LastupdatedDate  =  GETDATE()  
                                    , LastUpdatedBy  =  @LastUpdatedBy  
                                    --, site_id  =  @site_id  
                                    , ExtendOnReload  =  @ExtendOnReload  
                                    , PlayStartTime  =  @PlayStartTime  
                                    , TicketAllowed  =  @TicketAllowed  
                                    , MasterEntityId  =  @MasterEntityId  
                                    , ForMembershipOnly = @ForMembershipOnly
                                    , ExpireWithMembership = @ExpireWithMembership
                                    , MembershipRewardsId = @MembershipRewardsId
                                    , MembershipId = @MembershipId
                                    , PauseAllowed = @PauseAllowed
                                    ,IsActive = @IsActive
                                    ,SourceCreditPlusId = @SourceCreditPlusId
                                    ,ValidityStatus = @ValidityStatus
                                    ,SubscriptionBillingScheduleId = @SubscriptionBillingScheduleId
                                where CardCreditPlusId = @CardCreditPlusId
                                SELECT * FROM CardcreditPlus WHERE CardCreditPlusId = @CardCreditPlusId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountCreditPlusDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshAccountCreditPlusDTO(accountCreditPlusDTO ,dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating accountCreditPlusDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(accountCreditPlusDTO);
            return accountCreditPlusDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="accountCreditPlusDTO">AccountCreditPlusDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAccountCreditPlusDTO(AccountCreditPlusDTO accountCreditPlusDTO, DataTable dt)
        {
            log.LogMethodEntry(accountCreditPlusDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                accountCreditPlusDTO.AccountCreditPlusId = Convert.ToInt32(dt.Rows[0]["CardCreditPlusId"]);
                accountCreditPlusDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                accountCreditPlusDTO.LastUpdateDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                accountCreditPlusDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                accountCreditPlusDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                accountCreditPlusDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                accountCreditPlusDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                accountCreditPlusDTO.ValidityStatus = dataRow["ValidityStatus"] == DBNull.Value ? AccountDTO.AccountValidityStatus.Valid : (dataRow["ValidityStatus"].ToString() == "Y" ? AccountDTO.AccountValidityStatus.Valid : AccountDTO.AccountValidityStatus.Hold);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Deletes the AccountCreditPlus record of passed AccountCreditPlus Id
        /// </summary>
        /// <param name="accountCreditPlusId">integer type parameter</param>
        public void DeleteAccountCreditPlus(int accountCreditPlusId)
        {
            log.LogMethodEntry(accountCreditPlusId);
            string query = @"DELETE  
                             FROM CardCreditPlus
                             WHERE CardCreditPlus.CardCreditPlusId = @CardCreditPlusId";
            SqlParameter parameter = new SqlParameter("@CardCreditPlusId", accountCreditPlusId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AccountCreditPlusDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AccountCreditPlusDTO</returns>
        private AccountCreditPlusDTO GetAccountCreditPlusDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountCreditPlusDTO accountCreditPlusDTO = new AccountCreditPlusDTO(Convert.ToInt32(dataRow["CardCreditPlusId"]),
                                                         dataRow["CreditPlus"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["CreditPlus"]),
                                                         CreditPlusTypeConverter.FromString(dataRow["CreditPlusType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreditPlusType"])),
                                                         dataRow["Refundable"] == DBNull.Value ? false : Convert.ToString(dataRow["Refundable"]) == "Y",
                                                         dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                                         dataRow["Card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Card_id"]),
                                                         dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                                         dataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LineId"]),
                                                         dataRow["CreditPlusBalance"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["CreditPlusBalance"]),
                                                         dataRow["PeriodFrom"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["PeriodFrom"]),
                                                         dataRow["PeriodTo"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["PeriodTo"]),
                                                         dataRow["TimeFrom"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["TimeFrom"]),
                                                         dataRow["TimeTo"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["TimeTo"]),
                                                         dataRow["NumberOfDays"] == DBNull.Value ? (int?) null : Convert.ToInt32(dataRow["NumberOfDays"]),
                                                         dataRow["Monday"] == DBNull.Value ? true : Convert.ToString(dataRow["Monday"]) == "Y",
                                                         dataRow["Tuesday"] ==  DBNull.Value ? true : Convert.ToString(dataRow["Tuesday"]) == "Y",
                                                         dataRow["Wednesday"] == DBNull.Value ? true : Convert.ToString(dataRow["Wednesday"]) == "Y",
                                                         dataRow["Thursday"] == DBNull.Value ? true : Convert.ToString(dataRow["Thursday"]) == "Y",
                                                         dataRow["Friday"] == DBNull.Value ? true : Convert.ToString(dataRow["Friday"]) == "Y",
                                                         dataRow["Saturday"] == DBNull.Value ? true : Convert.ToString(dataRow["Saturday"]) == "Y",
                                                         dataRow["Sunday"] == DBNull.Value ? true : Convert.ToString(dataRow["Sunday"]) == "Y",
                                                         dataRow["MinimumSaleAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["MinimumSaleAmount"]),
                                                         dataRow["LoyaltyRuleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LoyaltyRuleId"]),
                                                         dataRow["ExtendOnReload"] == DBNull.Value ? false : Convert.ToString(dataRow["ExtendOnReload"]) == "Y",
                                                         dataRow["PlayStartTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["PlayStartTime"]),
                                                         dataRow["TicketAllowed"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["TicketAllowed"]),
                                                         dataRow["ForMembershipOnly"] == DBNull.Value ? false : Convert.ToString(dataRow["ForMembershipOnly"]) == "Y",
                                                         dataRow["ExpireWithMembership"] == DBNull.Value ? false : Convert.ToString(dataRow["ExpireWithMembership"]) == "Y",
                                                         dataRow["MembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipId"]),
                                                         dataRow["MembershipRewardsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipRewardsId"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["PauseAllowed"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["PauseAllowed"]),
                                                         dataRow["IsActive"] == DBNull.Value ? true : (Convert.ToBoolean(dataRow["IsActive"])),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["SourceCreditPlusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SourceCreditPlusId"]),
                                                         dataRow["ValidityStatus"] == DBNull.Value ? AccountDTO.AccountValidityStatus.Valid : (Convert.ToString(dataRow["ValidityStatus"]) == "Y" ? AccountDTO.AccountValidityStatus.Valid : AccountDTO.AccountValidityStatus.Hold),
                                                         dataRow["SubscriptionBillingScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SubscriptionBillingScheduleId"])
                                            );
            log.LogMethodExit(accountCreditPlusDTO);
            return accountCreditPlusDTO;
        }

        /// <summary>
        /// Gets the AccountCreditPlus data of passed AccountCreditPlus Id
        /// </summary>
        /// <param name="accountCreditPlusId">integer type parameter</param>
        /// <returns>Returns AccountCreditPlusDTO</returns>
        public AccountCreditPlusDTO GetAccountCreditPlusDTO(int accountCreditPlusId)
        {
            log.LogMethodEntry(accountCreditPlusId);
            AccountCreditPlusDTO returnValue = null;
            string query = SELECT_QUERY + "   WHERE ccp.CardCreditPlusId = @Id";
            //"   WHERE ccp.Card_Game_Id = @Card_Game_Id";
            SqlParameter parameter = new SqlParameter("@Id", accountCreditPlusId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetAccountCreditPlusDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the AccountCreditPlusDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AccountCreditPlusDTO matching the search criteria</returns>
        public List<AccountCreditPlusDTO> GetAccountCreditPlusDTOList(List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AccountCreditPlusDTO> list = null;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.ACCOUNT_ID ||
                            searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.MEMBERSHIPS_ID ||
                            searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.MEMBERSHIP_REWARDS_ID ||
                            searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.TRANSACTION_ID ||
                            searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.SOURCE_CREDITPLUS_ID ||
                            searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.ACCOUNT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.CREDITPLUS_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.EXPIRE_WITH_MEMBERSHIP 
                            || searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.FORMEMBERSHIP_ONLY)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.VALIDITYSTATUS)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.PAUSE_ALLOWED)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.CREDITPLUS_TYPE_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED)
                        {
                            query.Append(joiner + " ISNULL((SELECT top 1 CASE WHEN ISNULL(sbs.TransactionId,-1) = -1 THEN 0 ELSE 1 END" +
                                                     " FROM SubscriptionBillingSchedule sbs " +
                                                     "WHERE sbs.SubscriptionBillingScheduleId = " + DBSearchParameters[searchParameter.Key] + "),0) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1": "0")));
                        }
                        else if (searchParameter.Key == AccountCreditPlusDTO.SearchByParameters.HAS_CONSUMPTION_RULE)
                        {
                            query.Append(joiner + " EXISTS (select 'x' from cardCreditPlusConsumption ccpc where ccpc.cardCreditPlusId = ccp.cardCreditPlusId)");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception in GetAllCardCreditPlus");
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
                list = new List<AccountCreditPlusDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountCreditPlusDTO accountCreditPlusDTO = GetAccountCreditPlusDTO(dataRow);
                    list.Add(accountCreditPlusDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// GetAccountCreditPlusDTOListByAccountIdList
        /// </summary>
        /// <param name="accountIdList"></param>
        /// <returns></returns>
        public List<AccountCreditPlusDTO> GetAccountCreditPlusDTOListByAccountIdList(List<int> accountIdList)
        {
            log.LogMethodEntry(accountIdList);
            List<AccountCreditPlusDTO> list = new List<AccountCreditPlusDTO>();
            string query = SELECT_QUERY + @" , @accountIdList List WHERE ccp.card_Id = List.Id "; 
            DataTable table = dataAccessHandler.BatchSelect(query, "@accountIdList", accountIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetAccountCreditPlusDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
