/********************************************************************************************
 * Project Name - Account Audit Data Handler
 * Description  -  Data Handler for AccountAudit class.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70.2        23-Jul-2019   Girish Kundar     Modified : Added RefreshDTO() method and Fix for SQL Injection Issue 
 *2.70.2       05-Dec-2019   Jinto Thomas            Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    ///  AccountAudit Data Handler - Handles insert, update and select of  AccountAudit objects
    /// </summary>
    public class AccountAuditDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<AccountAuditDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountAuditDTO.SearchByParameters, string>
            {
                {AccountAuditDTO.SearchByParameters.ACCOUNT_ID, "CardsAudit.card_id"},
            };
        private readonly DataAccessHandler dataAccessHandler;
        List<SqlParameter> parameters = new List<SqlParameter>();
        private static readonly string ACCOUNT_SELECT_QUERY = @"SELECT CardsAudit.*,  ISNULL(Profile.FirstName,'') + ' ' + ISNULL(Profile.LastName,'') AS CustomerName
                                                                FROM CardsAudit 
                                                                LEFT OUTER JOIN Customers ON CardsAudit.customer_id = Customers.customer_id
                                                                LEFT OUTER JOIN Profile ON Profile.Id = Customers.ProfileId ";

        /// <summary>
        /// Parameterized constructor of AccountAuditDataHandler class
        /// </summary>
        public AccountAuditDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AccountAudit Record.
        /// </summary>
        /// <param name="accountDTO">AccountAuditDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AccountAuditDTO accountDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AccountAuditId", accountDTO.AccountAuditId, true));
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
            parameters.Add(dataAccessHandler.GetSQLParameter("@TechnicianCard", accountDTO.TechnicianCard));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TechGames", accountDTO.TechGames));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimerResetCard", accountDTO.TimerResetCard ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyPoints", accountDTO.LoyaltyPoints));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UploadSiteId", accountDTO.UploadSiteId));
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
        /// Inserts the AccountAudit record to the database
        /// </summary>
        /// <param name="accountAuditDTO">AccountAuditDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted AccountAudit record </returns>
        public AccountAuditDTO InsertAccountAudit(AccountAuditDTO accountAuditDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountAuditDTO, loginId, siteId);
            string query = @"INSERT INTO CardsAudit 
                                        ( 
                                            card_id,
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
                                            GUID ,
                                            CreatedBy,
                                            CreationDate
                                        ) 
                                VALUES 
                                        (
                                            @AccountId,
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
                                            NEWID(),
                                            @CreatedBy,
                                            GetDate()
                                        )
                                        SELECT * FROM CardsAudit WHERE Card_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountAuditDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountAuditDTO(accountAuditDTO ,dt);
            }
            catch (Exception ex)
            {
                log.LogVariableState("AccountAuditDTO", accountAuditDTO);
                log.Error("Error occurred while inserting the account record", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(accountAuditDTO);
            return accountAuditDTO;
        }


        /// <summary>
        /// Updates the AccountAudit record to the database
        /// </summary>
        /// <param name="accountAuditDTO">AccountAuditDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted AccountAudit record </returns>
        public AccountAuditDTO UpdateAccountAudit(AccountAuditDTO accountAuditDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountAuditDTO, loginId, siteId);
            string query = @"UPDATE CardsAudit 
                                        SET 
                                            card_id     =  @AccountId,
                                            card_number =  @TagNumber,
                                            issue_date  =  @IssueDate,
                                            face_value  =  @FaceValue,
                                            refund_flag =  @RefundFlag,
                                            refund_amount =  @RefundAmount,
                                            refund_date  =  @RefundDate, 
                                            valid_flag =  @ValidFlag, 
                                            ticket_count =  @TicketCount, 
                                            notes =   @Notes, 
                                            last_update_time = GETDATE(), 
                                            credits =  @Credits, 
                                            courtesy =  @Courtesy, 
                                            bonus =   @Bonus, 
                                            time =   @Time, 
                                            customer_id = @CustomerId, 
                                            credits_played = @CreditsPlayed, 
                                            ticket_allowed = @TicketAllowed, 
                                            real_ticket_mode = @RealTicketMode, 
                                            vip_customer =@VipCustomer, 
                                           -- site_id =  @site_id, 
                                            start_time =  @StartTime, 
                                            last_played_time = @LastPlayedTime, 
                                            technician_card =  @TechnicianCard, 
                                            tech_games =   @TechGames, 
                                            timer_reset_card = @TimerResetCard, 
                                            loyalty_points = @LoyaltyPoints, 
                                            LastUpdatedBy =  @LastUpdatedBy, 
                                            upload_site_id =  @UploadSiteId, 
                                            upload_time =  @UploadTime, 
                                            ExpiryDate = @ExpiryDate, 
                                            DownloadBatchId =  @DownloadBatchId, 
                                            RefreshFromHQTime =@RefreshFromHQTime,
                                            MasterEntityId = @MasterEntityId, 
                                            CardIdentifier = @CardIdentifier, 
                                            PrimaryCard = @PrimaryCard
                                            Where Card_id = @@AccountAuditId
                                           SELECT * FROM CardsAudit WHERE CardsAuditId = @AccountAuditId";

             try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountAuditDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountAuditDTO(accountAuditDTO, dt);
            }
            catch (Exception ex)
            {
                log.LogVariableState("AccountAuditDTO", accountAuditDTO);
                log.Error("Error occurred while inserting the account record", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(accountAuditDTO);
            return accountAuditDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="accountAuditDTO">AccountAuditDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAccountAuditDTO(AccountAuditDTO accountAuditDTO, DataTable dt)
        {
            log.LogMethodEntry(accountAuditDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                accountAuditDTO.AccountAuditId = Convert.ToInt32(dt.Rows[0]["CardsAuditId"]);
                accountAuditDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                accountAuditDTO.LastUpdateDate = dataRow["last_update_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_update_time"]);
                accountAuditDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                accountAuditDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                accountAuditDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                accountAuditDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AccountAuditDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AccountAuditDTO</returns>
        private AccountAuditDTO GetAccountAuditDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountAuditDTO accountDTO = new AccountAuditDTO(Convert.ToInt32(dataRow["CardsAuditId"]),
                                            Convert.ToInt32(dataRow["card_id"]),
                                            dataRow["card_number"] == DBNull.Value ? "" : Convert.ToString(dataRow["card_number"]),
                                            dataRow["CustomerName"] == DBNull.Value ? "" : Convert.ToString(dataRow["CustomerName"]),
                                            dataRow["issue_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["issue_date"]),
                                            dataRow["face_value"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["face_value"]),
                                            dataRow["refund_flag"] == DBNull.Value ? false : dataRow["refund_flag"].ToString() == "Y",
                                            dataRow["refund_amount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["refund_amount"]),
                                            dataRow["refund_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["refund_date"]),
                                            dataRow["valid_flag"] == DBNull.Value ? true : dataRow["valid_flag"].ToString() == "Y",
                                            dataRow["ticket_count"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ticket_count"]),
                                            dataRow["notes"] == DBNull.Value ? "" : dataRow["notes"].ToString(),
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
                                            dataRow["CardIdentifier"] == DBNull.Value ? "" : dataRow["CardIdentifier"].ToString(),
                                            dataRow["PrimaryCard"] == DBNull.Value ? false : dataRow["PrimaryCard"].ToString() == "Y",
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["last_update_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_update_time"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                            );
            log.LogMethodExit(accountDTO);
            return accountDTO;
        }

        /// <summary>
        /// Returns the no of accounts matching the search criteria
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetAccountAuditCount(List<KeyValuePair<AccountAuditDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int accountCount = 0;
            string selectQuery = @" SELECT COUNT(1) AS TotalCount 
                                    FROM CardsAudit  
                                    LEFT OUTER JOIN Customers ON Cards.customer_id = Customers.customer_id
                                    LEFT OUTER JOIN Profile ON Profile.Id = Customers.ProfileId ";
            selectQuery += GetAccountAuditFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                accountCount = Convert.ToInt32(dataTable.Rows[0]["TotalCount"]);
            }
            log.LogMethodExit(accountCount);
            return accountCount;
        }

        private String GetAccountAuditFilterQuery(List<KeyValuePair<AccountAuditDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            string joiner = string.Empty;
            int count = 0;
            StringBuilder query = new StringBuilder(" ");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                query.Append(" WHERE ");
                foreach (KeyValuePair<AccountAuditDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? " " : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AccountAuditDTO.SearchByParameters.ACCOUNT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                }
            }
            log.LogMethodExit(query);
            return query.ToString();
        }

        /// <summary>
        /// Gets the AccountAuditDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AccountAuditDTO matching the search criteria</returns>
        public List<AccountAuditDTO> GetAccountAuditDTOList(List<KeyValuePair<AccountAuditDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AccountAuditDTO> list = null;

            string selectQuery = ACCOUNT_SELECT_QUERY;
            selectQuery += GetAccountAuditFilterQuery(searchParameters);
            selectQuery += " order by last_update_time ";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery,parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<AccountAuditDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountAuditDTO accountDTO = GetAccountAuditDTO(dataRow);
                    list.Add(accountDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the AccountAuditDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="pageNumber">current page number</param>
        /// <param name="pageSize">page size</param>
        /// <returns>Returns the list of AccountAuditDTO matching the search criteria</returns>
        public List<AccountAuditDTO> GetAccountAuditDTOList(List<KeyValuePair<AccountAuditDTO.SearchByParameters, string>> searchParameters, int pageNumber, int pageSize)
        {
            log.LogMethodEntry(searchParameters);
            List<AccountAuditDTO> list = null;

            string selectQuery = ACCOUNT_SELECT_QUERY;
            selectQuery += GetAccountAuditFilterQuery(searchParameters);
            selectQuery += " ORDER BY CardsAudit.CardsAuditId OFFSET " + (pageNumber * pageSize).ToString() + " ROWS";
            selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<AccountAuditDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountAuditDTO accountDTO = GetAccountAuditDTO(dataRow);
                    list.Add(accountDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
