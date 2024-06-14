/********************************************************************************************
* Project Name - Account Data Handler
* Description  - DataHandler
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*1.0.0       12-May-2017      Lakshminarayana     Created 
*2.4.0       14-Sep-2018      Archana             Modified to a add refund flag chaeck in GetAccountDTO()
*2.60        19-Feb-2019      Mushahid Faizan     Modified to continue loop in GetAccountFilterQuery(), Added count++ .
*2.70.2      23-Jul-2019      Girish Kundar       Modified :  Added RefreshDTO() methods and CreatedBy and CreationDate fields.
*2.70.2      15-Oct-2019      Nitin Pai           Modified :  Gateway cleanup - added new methods to be called from REST.
*2.70.3      04-Feb-2020      Nitin Pai           Guest App phase 2 changes.
*2.80.0      29-Apr-2020      Akshay G            Added searchParameter ACCOUNT_ID_LIST
*2.80.0      23-Jun-2020      Deeksha             Issue Fix : Miami time play can not cancel line
*2.110.0     10-Dec-2020      Guru S A            For Subscription changes                   
*2.140.0     12-Dec-2021      Guru S A            Booking execute process performance fixes
*2.130.7     23-Apr-2022      Nitin Pai           Get linked cards and child's cards for a customer in website
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
    ///  Account Data Handler - Handles insert, update and select of  Account objects
    /// </summary>
    public class AccountDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<AccountDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountDTO.SearchByParameters, string>
            {
                {AccountDTO.SearchByParameters.ACCOUNT_ID, "Cards.card_id"},
                {AccountDTO.SearchByParameters.TAG_NUMBER, "Cards.card_number"},
                {AccountDTO.SearchByParameters.ISSUE_DATE, "Cards.issue_date"},
                {AccountDTO.SearchByParameters.FACE_VALUE, "Cards.face_value"},
                {AccountDTO.SearchByParameters.REFUND_FLAG, "Cards.refund_flag"},
                {AccountDTO.SearchByParameters.REFUND_AMOUNT, "Cards.refund_amount"},
                {AccountDTO.SearchByParameters.REFUND_DATE, "Cards.refund_date"},
                {AccountDTO.SearchByParameters.TICKET_COUNT, "Cards.ticket_count"},
                {AccountDTO.SearchByParameters.NOTES, "Cards.notes"},
                {AccountDTO.SearchByParameters.LAST_UPDATE_TIME, "Cards.last_update_time"},
                {AccountDTO.SearchByParameters.CREDITS, "Cards.credits"},
                {AccountDTO.SearchByParameters.COURTESY, "Cards.courtesy"},
                {AccountDTO.SearchByParameters.BONUS, "Cards.bonus"},
                {AccountDTO.SearchByParameters.TIME, "Cards.time"},
                {AccountDTO.SearchByParameters.CREDITS_PLAYED, "Cards.credits_played"},
                {AccountDTO.SearchByParameters.TICKET_ALLOWED, "Cards.ticket_allowed"},
                {AccountDTO.SearchByParameters.REAL_TICKET_MODE, "Cards.real_ticket_mode"},
                {AccountDTO.SearchByParameters.VIP_CUSTOMER, "Cards.vip_customer"},
                {AccountDTO.SearchByParameters.START_TIME, "Cards.start_time"},
                {AccountDTO.SearchByParameters.LAST_PLAYED_TIME, "Cards.last_played_time"},
                {AccountDTO.SearchByParameters.TECHNICIAN_CARD, "Cards.technician_card"},
                {AccountDTO.SearchByParameters.TECH_GAMES, "Cards.tech_games"},
                {AccountDTO.SearchByParameters.TIMER_RESET_CARD, "Cards.timer_reset_card"},
                {AccountDTO.SearchByParameters.LOYALTY_POINTS, "Cards.loyalty_points"},
                {AccountDTO.SearchByParameters.LAST_UPDATED_BY, "Cards.LastUpdatedBy"},
                {AccountDTO.SearchByParameters.UPLOAD_SITE_ID, "Cards.upload_site_id"},
                {AccountDTO.SearchByParameters.UPLOAD_TIME, "Cards.upload_time"},
                {AccountDTO.SearchByParameters.EXPIRY_DATE, "Cards.ExpiryDate"},
                {AccountDTO.SearchByParameters.ACCOUNT_IDENTIFIER, "Cards.CardIdentifier"},
                {AccountDTO.SearchByParameters.PRIMARY_ACCOUNT, "Cards.PrimaryCard"},
                {AccountDTO.SearchByParameters.CUSTOMER_ID, "Cards.customer_id"},
                {AccountDTO.SearchByParameters.VALID_FLAG, "Cards.valid_flag"},
                {AccountDTO.SearchByParameters.MEMBERSHIP_ID, "Membership.MembershipID"},
                {AccountDTO.SearchByParameters.MEMBERSHIP_NAME, "Membership.MembershipName"},
                {AccountDTO.SearchByParameters.SITE_ID, "Cards.site_id"},
                {AccountDTO.SearchByParameters.MASTER_ENTITY_ID, "Cards.MasterEntityId"},
                {AccountDTO.SearchByParameters.CUSTOMER_NAME, "(ISNULL(Profile.FirstName,'') + ' ' + ISNULL(Profile.LastName,''))"},
                {AccountDTO.SearchByParameters.ISSUE_DATE_FROM, "Cards.issue_date"},
                {AccountDTO.SearchByParameters.ISSUE_DATE_TO, "Cards.issue_date"},
                {AccountDTO.SearchByParameters.ACCOUNT_ID_LIST, "Cards.card_id"},
                {AccountDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, "sbs.SubscriptionHeaderId"},
                {AccountDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID, "sbs.SubscriptionBillingScheduleId"},
                {AccountDTO.SearchByParameters.CUSTOMER_ID_LIST, "Cards.customer_id"},
            };
        private readonly DataAccessHandler dataAccessHandler;
        List<SqlParameter> parameters = new List<SqlParameter>();
        private static readonly string ACCOUNT_SELECT_QUERY = @"SELECT Cards.*,  ISNULL(Profile.FirstName,'') + ' ' + ISNULL(Profile.LastName,'') AS CustomerName, 
                                                                Membership.MembershipID, Membership.MembershipName
                                                                FROM Cards 
                                                                LEFT OUTER JOIN Customers ON Cards.customer_id = Customers.customer_id
                                                                LEFT OUTER JOIN Profile ON Profile.Id = Customers.ProfileId 
                                                                LEFT OUTER JOIN Membership ON Customers.MembershipId = Membership.MembershipID ";

        /// <summary>
        /// Parameterized constructor of AccountDataHandler class
        /// </summary>
        public AccountDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Account Record.
        /// </summary>
        /// <param name="accountDTO">AccountDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AccountDTO accountDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountDTO, loginId, siteId);
            parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AccountId", accountDTO.AccountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TagNumber", accountDTO.TagNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IssueDate", accountDTO.IssueDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FaceValue", accountDTO.FaceValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RefundFlag", accountDTO.RefundFlag ? "Y" : "N"));
            if (accountDTO.RefundAmount != null && accountDTO.RefundAmount == -1)
            {
                accountDTO.RefundAmount = null;
            }
            parameters.Add(dataAccessHandler.GetSQLParameter("@RefundAmount", accountDTO.RefundAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RefundDate", accountDTO.RefundDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValidFlag", accountDTO.ValidFlag ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketCount", accountDTO.TicketCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Notes", accountDTO.Notes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Credits", accountDTO.Credits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Courtesy", accountDTO.Courtesy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Bonus", accountDTO.Bonus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Time", accountDTO.Time));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", accountDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreditsPlayed", accountDTO.CreditsPlayed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketAllowed", accountDTO.TicketAllowed ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RealTicketMode", accountDTO.RealTicketMode ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VipCustomer", accountDTO.VipCustomer ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StartTime", accountDTO.StartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastPlayedTime", accountDTO.LastPlayedTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TechnicianCard", string.IsNullOrWhiteSpace(accountDTO.TechnicianCard)? "N" : accountDTO.TechnicianCard));
            if (accountDTO.TechGames == -1)
            {
                parameters.Add(new SqlParameter("@TechGames", DBNull.Value));
            }
            else
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@TechGames", accountDTO.TechGames));
            }
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimerResetCard", accountDTO.TimerResetCard ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyPoints", accountDTO.LoyaltyPoints));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UploadSiteId", accountDTO.UploadSiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UploadTime", accountDTO.UploadTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", accountDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DownloadBatchId", accountDTO.DownloadBatchId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RefreshFromHQTime", accountDTO.RefreshFromHQTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardIdentifier", accountDTO.AccountIdentifier));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PrimaryCard", accountDTO.PrimaryAccount ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", accountDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Account record to the database
        /// </summary>
        /// <param name="accountDTO">AccountDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted account record</returns>
        public AccountDTO InsertAccount(AccountDTO accountDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountDTO, loginId, siteId);
            string query = @"INSERT INTO Cards 
                                        ( 
                                            card_number,
                                            issue_date,
                                            face_value,
                                            refund_flag,
                                            refund_amount,
                                            refund_date, 
                                            valid_flag, 
                                            ticket_count, 
                                            notes, 
                                            last_update_time, 
                                            credits, 
                                            courtesy, 
                                            bonus, 
                                            time, 
                                            customer_id, 
                                            credits_played, 
                                            ticket_allowed, 
                                            real_ticket_mode, 
                                            vip_customer, 
                                            site_id, 
                                            start_time, 
                                            last_played_time, 
                                            technician_card, 
                                            tech_games, 
                                            timer_reset_card, 
                                            loyalty_points, 
                                            LastUpdatedBy, 
                                            upload_site_id, 
                                            upload_time, 
                                            ExpiryDate, 
                                            DownloadBatchId, 
                                            RefreshFromHQTime, 
                                            MasterEntityId, 
                                            CardIdentifier, 
                                            PrimaryCard,
                                            CreatedBy ,
                                            CreationDate
                                        ) 
                                VALUES 
                                        (
                                            @TagNumber,
                                            @IssueDate,
                                            @FaceValue,
                                            @RefundFlag,
                                            @RefundAmount,
                                            @RefundDate, 
                                            @ValidFlag, 
                                            @TicketCount, 
                                            @Notes, 
                                            GETDATE(), 
                                            @Credits, 
                                            @Courtesy, 
                                            @Bonus, 
                                            @Time, 
                                            @CustomerId, 
                                            @CreditsPlayed, 
                                            @TicketAllowed, 
                                            @RealTicketMode, 
                                            @VipCustomer, 
                                            @site_id, 
                                            @StartTime, 
                                            @LastPlayedTime, 
                                            @TechnicianCard, 
                                            @TechGames, 
                                            @TimerResetCard, 
                                            @LoyaltyPoints, 
                                            @LastUpdatedBy, 
                                            @UploadSiteId, 
                                            @UploadTime, 
                                            @ExpiryDate, 
                                            @DownloadBatchId, 
                                            @RefreshFromHQTime, 
                                            @MasterEntityId, 
                                            @CardIdentifier, 
                                            @PrimaryCard,
                                            @CreatedBy ,
                                            GetDate()
                                        )
                                        SELECT * FROM Cards WHERE Card_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountDTO(accountDTO, dt);
            }
            catch (Exception ex)
            {
                log.LogVariableState("AccountDTO", accountDTO);
                log.Error("Error occurred while inserting the account record", ex);
                log.LogMethodExit(null, "throwing exception" + ex.Message);
                throw;
            }
            log.LogMethodExit(accountDTO);
            return accountDTO;
        }

        /// <summary>
        /// Updates the Account record
        /// </summary>
        /// <param name="accountDTO">AccountDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns updated account record</returns>
        public AccountDTO UpdateAccount(AccountDTO accountDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountDTO, loginId, siteId);
            string query = @"UPDATE Cards
                            SET card_number = @TagNumber,
	                            issue_date = @IssueDate,
	                            face_value = @FaceValue,
	                            refund_flag = @RefundFlag,
	                            refund_amount = @RefundAmount,
	                            refund_date = @RefundDate,
	                            valid_flag = @ValidFlag,  
	                            ticket_count = @TicketCount, 
	                            notes = @Notes, 
	                            last_update_time = GETDATE(),
	                            credits = @Credits, 
	                            courtesy = @Courtesy, 
	                            bonus = @Bonus,
	                            time =  @Time, 
	                            customer_id = @CustomerId, 
	                            credits_played = @CreditsPlayed, 
	                            ticket_allowed = @TicketAllowed, 
	                            real_ticket_mode =  @RealTicketMode, 
	                            vip_customer = @VipCustomer, 
	                            start_time = @StartTime,
	                            last_played_time = @LastPlayedTime,  
	                            technician_card =  @TechnicianCard, 
	                            tech_games = @TechGames, 
	                            timer_reset_card = @TimerResetCard, 
	                            loyalty_points = @LoyaltyPoints, 
	                            LastUpdatedBy = @LastUpdatedBy,  
	                            upload_site_id = @UploadSiteId, 
	                            upload_time = @UploadTime, 
	                            ExpiryDate = @ExpiryDate, 
	                            DownloadBatchId = @DownloadBatchId, 
	                            RefreshFromHQTime = @RefreshFromHQTime, 
	                            MasterEntityId = @MasterEntityId, 
	                            CardIdentifier = @CardIdentifier, 
	                            PrimaryCard = @PrimaryCard
                            WHERE card_id = @AccountId
                            SELECT * FROM Cards WHERE card_id = @AccountId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountDTO(accountDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating the account record", ex);
                log.LogVariableState("AccountDTO", accountDTO);
                log.LogMethodExit(null, "throwing exception" + ex.Message);
                throw;
            }
            log.LogMethodExit(accountDTO);
            return accountDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="accountDTO">AccountDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAccountDTO(AccountDTO accountDTO, DataTable dt)
        {
            log.LogMethodEntry(accountDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                accountDTO.AccountId = Convert.ToInt32(dt.Rows[0]["Card_id"]);
                accountDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                accountDTO.LastPlayedTime = dataRow["last_played_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_played_time"]);
                accountDTO.LastUpdateDate = dataRow["last_update_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_update_time"]);
                accountDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                accountDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                accountDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                accountDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AccountDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AccountDTO</returns>
        private AccountDTO GetAccountDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountDTO accountDTO = new AccountDTO(Convert.ToInt32(dataRow["card_id"]),
                                            dataRow["card_number"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["card_number"]),
                                            dataRow["CustomerName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CustomerName"]),
                                            dataRow["issue_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["issue_date"]),
                                            dataRow["face_value"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["face_value"]),
                                            dataRow["refund_flag"] == DBNull.Value ? false : dataRow["refund_flag"].ToString() == "Y",
                                            dataRow["refund_amount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["refund_amount"]),
                                            dataRow["refund_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["refund_date"]),
                                            dataRow["valid_flag"] == DBNull.Value ? true : dataRow["valid_flag"].ToString() == "Y",
                                            dataRow["ticket_count"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ticket_count"]),
                                            dataRow["notes"] == DBNull.Value ? string.Empty : dataRow["notes"].ToString(),
                                            dataRow["credits"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["credits"]),
                                            dataRow["courtesy"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["courtesy"]),
                                            dataRow["bonus"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["bonus"]),
                                            dataRow["time"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["time"]),
                                            dataRow["customer_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["customer_id"]),
                                            dataRow["credits_played"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["credits_played"]),
                                            dataRow["ticket_allowed"] == DBNull.Value ? true : dataRow["ticket_allowed"].ToString() == "Y",
                                            dataRow["real_ticket_mode"] == DBNull.Value ? false : dataRow["real_ticket_mode"].ToString() == "Y",
                                            dataRow["vip_customer"] == DBNull.Value ? false : dataRow["vip_customer"].ToString() == "Y",
                                            dataRow["start_time"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["start_time"]),
                                            dataRow["last_played_time"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["last_played_time"]),
                                            dataRow["technician_card"] == DBNull.Value ? "N" : dataRow["technician_card"].ToString(),
                                            dataRow["tech_games"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["tech_games"]),
                                            dataRow["timer_reset_card"] == DBNull.Value ? false : dataRow["timer_reset_card"].ToString() == "Y",
                                            dataRow["loyalty_points"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["loyalty_points"]),
                                            dataRow["upload_site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["upload_site_id"]),
                                            dataRow["upload_time"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["upload_time"]),
                                            dataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                            dataRow["DownloadBatchId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DownloadBatchId"]),
                                            dataRow["RefreshFromHQTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["RefreshFromHQTime"]),
                                            dataRow["CardIdentifier"] == DBNull.Value ? string.Empty : dataRow["CardIdentifier"].ToString(),
                                            dataRow["PrimaryCard"] == DBNull.Value ? false : dataRow["PrimaryCard"].ToString() == "Y",
                                            dataRow["MembershipID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipID"]),
                                            dataRow["MembershipName"] == DBNull.Value ? string.Empty : dataRow["MembershipName"].ToString(),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["last_update_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_update_time"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                            );
            log.LogMethodExit(accountDTO);
            return accountDTO;
        }

        /// <summary>
        /// Gets the Account data of passed Account Id
        /// </summary>
        /// <param name="accountId">integer type parameter</param>
        /// <returns>Returns AccountDTO</returns>
        public AccountDTO GetAccountDTO(int accountId)
        {
            log.LogMethodEntry(accountId);
            AccountDTO returnValue = null;
            string query = ACCOUNT_SELECT_QUERY + @" WHERE Cards.card_id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", accountId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetAccountDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;

        }

        /// <summary>
        /// Gets the Account data of passed Account Number
        /// </summary>
        /// <param name="accountNumber">string type parameter</param>
        /// <returns>Returns AccountDTO</returns>
        public AccountDTO GetAccountDTO(string accountNumber)
        {
            log.LogMethodEntry(accountNumber);
            AccountDTO returnValue = null;
            string query = ACCOUNT_SELECT_QUERY + @" WHERE Cards.card_number = @accountNumber AND refund_flag = 'N' AND Valid_flag = 'Y' ";
            SqlParameter parameter = new SqlParameter("@accountNumber", accountNumber);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetAccountDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;

        }

        public bool DeactivateFingerprint(int cardId, string userId)
        {
            bool status = false;
            try
            {
                string cardFingerPrintUpdateQuery = @"update CustomerFingerPrint 
                                                     set ActiveFlag = 'False', 
                                                     LastUpdatedDate = getdate(), 
                                                     LastUpdatedBy = @userId 
                                            where CardId = @card_id ";

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@userId", userId);
                parameters[1] = new SqlParameter("@card_id", cardId);

                int rowsEffected = dataAccessHandler.executeUpdateQuery(cardFingerPrintUpdateQuery, parameters);
                if (rowsEffected != 0)
                    status = true;
            }
            catch
            {
                throw;
            }
            return status;
        }

        /// <summary>
        /// Gets Last update time for the account
        /// </summary>
        /// <param name="accountId">integer type parameter</param>
        /// <returns>Returns Last Account update date time</returns>
        public DateTime GetLastAccountUpdateDateTime(int accountId)
        {
            log.LogMethodEntry(accountId);
            DateTime lastUpdateTime = DateTime.MinValue;
            string query = "select last_update_time from cards WHERE card_id = @accountId";
            SqlParameter parameter = new SqlParameter("@accountId", accountId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                lastUpdateTime = dataTable.Rows[0]["last_update_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataTable.Rows[0]["last_update_time"]);
            }
            log.LogMethodExit(lastUpdateTime);
            return lastUpdateTime;

        }

        /// <summary>
        /// Is Account Updated By Others
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="lastActivityDate"></param>
        /// <returns></returns>
        public bool IsUpdatedByOthers(int accountId, DateTime lastActivityDate)
        {
            log.LogMethodEntry(accountId, lastActivityDate);
            bool isUpdatedByOthers = false;
            //string query = "select last_update_time from cards WHERE card_id = @accountId";
            string query = @"select top 1 1 
                                from cards c
                                where c.card_id = @accountId
                                   and (c.refund_flag = 'Y'
                                    or exists (select 1
                                                from gameplay gp
                                                where gp.card_id = c.card_id
                                                  and gp.play_date >  getdate()-1
                                                  and gp.play_date > DATEADD(ss,10,@lastActivityDate) 
                                                  and not exists (select 1 
	                                                                from trx_header th, trx_lines tlin, 
						                                                products pp, TransactionLineGamePlayMapping tlg
					                                                where th.TrxId = tlin.TrxId
					                                                and th.Status not in ('CANCELLED','SYSTEMABANDONED')
					                                                and tlin.product_id = pp.product_id
					                                                and pp.product_name = 'Load Bonus Task'
					                                                and tlin.CancelledTime is null
						                                            and tlin.TrxId = tlg.TrxId
						                                            and tlin.LineId = tlg.LineId
						                                            and tlg.IsActive = 1
						                                            and tlg.GamePlayId = gp.gameplay_id
						                                                    )
                                               )
								    or exists (select 1
									             from redemption R, Redemption_cards rc
												WHERE r.redemption_id = rc.redemption_id
												  and rc.card_id = c.card_id
                                                  and r.redemptionStatus <> 'NEW'
                                                  and r.redemptionstatus <> 'ABANDONED'
                                                  and r.redemptionstatus <> 'SUSPENDED'
                                                  and ISNULL(rc.ticket_count,0) != 0
                                                  and r.redeemed_date >  getdate()-1
												  and r.redeemed_date > @lastActivityDate)
                                    or exists ( select 1
									             from trxPayments tp
												WHERE tp.CardId = c.card_id 
                                                  and tp.PaymentDate > getdate()-1
												  and tp.PaymentDate > @lastActivityDate )
                                    or exists (select 1
                                                from tasks t, task_type tp
                                                where t.task_type_id = tp.task_type_id
												and (t.card_id = c.card_id
														OR (T.transfer_to_card_id IS NOT NULL 
														    AND T.transfer_to_card_id = C.card_id)
														OR (T.consolidate_card1 IS NOT NULL 
														    AND T.consolidate_card1 = C.card_id)
														OR (T.consolidate_card2 IS NOT NULL 
														    AND T.consolidate_card2 = C.card_id)
														OR (T.consolidate_card3 IS NOT NULL 
														   AND T.consolidate_card3 = C.card_id)
														OR (T.consolidate_card4 IS NOT NULL 
														    AND T.consolidate_card4 = C.card_id)
														OR (T.consolidate_card5 IS NOT NULL 
														    AND T.consolidate_card5 = C.card_id)
													)
                                                and not exists (select 1 
                                                            FROM transactionlinegameplaymapping tlg
                                                            WHERE tlg.trxid = t.trxid
                                                              AND tp.task_type = 'LOADBONUS')
                                                and t.task_date > getdate()-1
                                                and t.task_date > @lastActivityDate))";
            SqlParameter parameter1 = new SqlParameter("@accountId", accountId);
            SqlParameter parameter2 = new SqlParameter("@lastActivityDate", lastActivityDate);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter1, parameter2 }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                isUpdatedByOthers = true;
            }
            log.LogMethodExit(isUpdatedByOthers);
            return isUpdatedByOthers;

        }

        /// <summary>
        /// Returns the no of accounts matching the search criteria
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetAccountCount(List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int accountCount = 0;
            string selectQuery = @" SELECT COUNT(1) AS TotalCount 
                                    FROM Cards  
                                    LEFT OUTER JOIN Customers ON Cards.customer_id = Customers.customer_id
                                    LEFT OUTER JOIN Profile ON Profile.Id = Customers.ProfileId 
                                    LEFT OUTER JOIN Membership ON Customers.MembershipId = Membership.MembershipID ";
            selectQuery += GetAccountFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                accountCount = Convert.ToInt32(dataTable.Rows[0]["TotalCount"]);
            }
            log.LogMethodExit(accountCount);
            return accountCount;
        }

        private String GetAccountFilterQuery(List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            string joiner = "";
            int count = 0;
            parameters = new List<SqlParameter>();
            StringBuilder query = new StringBuilder(" ");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                query.Append(" WHERE ");
                foreach (KeyValuePair<AccountDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? " " : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AccountDTO.SearchByParameters.ACCOUNT_ID ||
                            searchParameter.Key == AccountDTO.SearchByParameters.CUSTOMER_ID ||
                            searchParameter.Key == AccountDTO.SearchByParameters.MEMBERSHIP_ID ||
                            searchParameter.Key == AccountDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountDTO.SearchByParameters.TAG_NUMBER)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountDTO.SearchByParameters.VALID_FLAG
                            || searchParameter.Key == AccountDTO.SearchByParameters.TICKET_ALLOWED
                            || searchParameter.Key == AccountDTO.SearchByParameters.PRIMARY_ACCOUNT
                            || searchParameter.Key == AccountDTO.SearchByParameters.REFUND_FLAG
                            || searchParameter.Key == AccountDTO.SearchByParameters.TECHNICIAN_CARD
                            || searchParameter.Key == AccountDTO.SearchByParameters.REAL_TICKET_MODE)

                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountDTO.SearchByParameters.ISSUE_DATE_FROM)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToDateTime(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountDTO.SearchByParameters.ISSUE_DATE_TO)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToDateTime(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountDTO.SearchByParameters.ACCOUNT_ID_LIST
                            || searchParameter.Key == AccountDTO.SearchByParameters.CUSTOMER_ID_LIST)
                        {
                            query.Append(joiner + @"(" + DBSearchParameters[searchParameter.Key] + " IN ( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + "))");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID ||
                                 searchParameter.Key == AccountDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID)
                        {
                            query.Append(joiner + @" EXISTS (SELECT top 1 1 
                                                               FROM subscriptionBillingSchedule sbs
                                                              where " + DBSearchParameters[searchParameter.Key] + "  = " + dataAccessHandler.GetParameterName(searchParameter.Key) +
                                                   @"           and exists (SELECT top 1 1 
                                                                              from cardcreditplus cp 
                                                                             where cp.subscriptionBillingScheduleId = sbs.subscriptionBillingScheduleId
																		       and cp.Card_id = Cards.card_id
                                                                            UNION ALL      
                                                                            SELECT top 1 1 
                                                                              from cardgames cg 
                                                                             where cg.subscriptionBillingScheduleId = sbs.subscriptionBillingScheduleId
																			   and cg.Card_id = Cards.card_id
                                                                            UNION ALL      
                                                                            SELECT top 1 1 
                                                                              from carddiscounts cd 
                                                                             where cd.subscriptionBillingScheduleId = sbs.subscriptionBillingScheduleId
																			   and cd.Card_id = Cards.card_id
                                                                           ))
                                                   ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        } 
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
            }
            log.LogMethodExit(query);
            return query.ToString();
        }

        /// <summary>
        /// Gets the AccountDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AccountDTO matching the search criteria</returns>
        public List<AccountDTO> GetAccountDTOList(List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AccountDTO> list = null;

            string selectQuery = ACCOUNT_SELECT_QUERY;
            selectQuery += GetAccountFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<AccountDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountDTO accountDTO = GetAccountDTO(dataRow);
                    list.Add(accountDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the AccountDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="pageNumber">current page number</param>
        /// <param name="pageSize">page size</param>
        /// <returns>Returns the list of AccountDTO matching the search criteria</returns>
        public List<AccountDTO> GetAccountDTOList(List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters, int pageNumber, int pageSize)
        {
            log.LogMethodEntry(searchParameters);
            List<AccountDTO> list = null;

            string selectQuery = ACCOUNT_SELECT_QUERY;
            selectQuery += GetAccountFilterQuery(searchParameters);
            selectQuery += " ORDER BY Cards.card_id OFFSET " + (pageNumber * pageSize).ToString() + " ROWS";
            selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<AccountDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountDTO accountDTO = GetAccountDTO(dataRow);
                    list.Add(accountDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Returns the no of accounts matching the search criteria
        /// </summary>
        /// <param name="searchCriteria">account search criteria</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetAccountCount(AccountSearchCriteria searchCriteria)
        {
            log.LogMethodEntry(searchCriteria);
            int accountCount = 0;
            string selectQuery = @" SELECT COUNT(1) AS TotalCount 
                                    FROM Cards  
                                    LEFT OUTER JOIN Customers ON Cards.customer_id = Customers.customer_id
                                    LEFT OUTER JOIN Profile ON Profile.Id = Customers.ProfileId 
                                    LEFT OUTER JOIN Membership ON Customers.MembershipId = Membership.MembershipID ";
            if (searchCriteria.ContainsCondition)
            {
                selectQuery += (" WHERE " + searchCriteria.GetWhereClause());
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, searchCriteria.GetSqlParameters().ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                accountCount = Convert.ToInt32(dataTable.Rows[0]["TotalCount"]);
            }
            log.LogMethodExit(accountCount);
            return accountCount;
        }

        /// <summary>
        /// Gets the AccountDTO list matching the search key
        /// </summary>
        /// <param name="searchCriteria">account search criteria</param>
        /// <returns>Returns the list of AccountDTO matching the search criteria</returns>
        public List<AccountDTO> GetAccountDTOList(AccountSearchCriteria searchCriteria)
        {
            log.LogMethodEntry(searchCriteria);
            List<AccountDTO> list = null;

            string selectQuery = ACCOUNT_SELECT_QUERY;
            selectQuery += searchCriteria.GetQuery();
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, searchCriteria.GetSqlParameters().ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<AccountDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountDTO accountDTO = GetAccountDTO(dataRow);
                    list.Add(accountDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        internal List<AccountDTO> CardNumberInfoForExecuteProcess(List<string> accountNumberList, bool reactivateExpired, int expireAfterMonths, int bonusdays)
        {
            log.LogMethodEntry(accountNumberList, reactivateExpired, expireAfterMonths, bonusdays);
            List<AccountDTO> list = new List<AccountDTO>();
            string selectQuery = null;

            selectQuery = @"SELECT c.* , ' ' CustomerName, -1 as MembershipID, null MembershipName
                              FROM Cards c, @accountNumberList list
					         WHERE c.card_number = list.Value 
                               AND c.Refund_flag = 'N'
                               AND ((valid_flag = 'Y' and ISNULL(ExpiryDate ,getdate()+1) > getdate())
                                     or (@reactivateExpired = 1 
                                         and expiryDate is not null
                                         and case when  @expireAfterMonths = -1  then DATEADD(day,@bonusdays*-1,ExpiryDate)  else ExpiryDate  end < getdate()
                                         and card_id = (SELECT max(card_id) 
                                                          FROM cards ce 
                                                         WHERE ce.card_number = c.card_number))) ";

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@reactivateExpired", reactivateExpired));
            sqlParameters.Add(new SqlParameter("@expireAfterMonths", expireAfterMonths));
            sqlParameters.Add(new SqlParameter("@bonusdays", bonusdays));

            DataTable table = dataAccessHandler.BatchSelect(selectQuery, "@accountNumberList", accountNumberList, sqlParameters.ToArray(), sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetAccountDTO(x)).ToList();
            }
            for (int i = 0; i < accountNumberList.Count; i++)
            {
                if (list.Exists(act => act.TagNumber == accountNumberList[i]) == false)
                {
                    AccountDTO accountDTO = new AccountDTO();
                    accountDTO.TagNumber = accountNumberList[i];
                    accountDTO.IssueDate = DateTime.Now;
                    accountDTO.AcceptChanges();
                    list.Add(accountDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// GetAccountDTOList
        /// </summary>
        /// <param name="accountIdList"></param>
        /// <returns></returns>
        public List<AccountDTO> GetAccountDTOList(List<int> accountIdList, bool activeRecords)
        {
            log.LogMethodEntry(accountIdList);
            List<AccountDTO> list = new List<AccountDTO>();
            string query = ACCOUNT_SELECT_QUERY + @" , @accountIdList List WHERE Cards.card_id = List.Id ";
            if (activeRecords)
            {
                query = query + @" AND Cards.Valid_Flag = 'Y' 
                                   AND refund_flag = 'N'
                                   AND ISNULL(Cards.ExpiryDate, getdate()+1) >= getdate() ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@accountIdList", accountIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetAccountDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// GetAccountDTOListByCustomerIds
        /// </summary>
        /// <param name="custIdList"></param>
        /// <returns></returns>
        public List<AccountDTO> GetAccountDTOListByCustomerIds(List<int> custIdList, bool activeRecords)
        {
            log.LogMethodEntry(custIdList);
            List<AccountDTO> list = new List<AccountDTO>();
            string query = ACCOUNT_SELECT_QUERY + @" , @custIdList List WHERE Cards.Customer_id = List.Id ";
            if (activeRecords)
            {
                query = query + @" AND Cards.Valid_Flag = 'Y' 
                                   AND refund_flag = 'N'
                                   AND ISNULL(Cards.ExpiryDate, getdate()+1) >= getdate() ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@custIdList", custIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetAccountDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
