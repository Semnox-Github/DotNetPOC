/********************************************************************************************
 * Project Name - Account
 * Description  - Account Data handler
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.130.11   03-Aug-2022     Yashodhara C H      Created
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
    /// AccountSummaryView Data Handler - Select of AccountView objects
    /// </summary>
    public class AccountSummaryViewDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"select c.*, 
	                                            isnull( case when start_time is null then time else
	                                            case when datediff(MI, getdate(), dateadd(MI,time, start_time)) < 0 then 0 
	                                            else datediff(MI, getdate(), dateadd(MI,time, start_time)) end
	                                            end,0)
	                                            BalanceTime, 
                                                isnull(cr.CreditPlusCardBalance, 0) CreditPlusCardBalance, 
                                                isnull(cr.CreditPlusCredits, 0) CreditPlusCredits, 
                                                isnull(cr.CreditPlusItemPurchase, 0) CreditPlusItemPurchase, 
                                                isnull(cr.CreditPlusBonus, 0) as CreditPlusBonus, 
                                                isnull(cr.CreditPlusTime, 0) as CreditPlusTime, 
                                                isnull(cr.CreditPlusTickets, 0) as CreditPlusTickets, 
                                                isnull(cr.CreditPlusLoyaltyPoints, 0) as CreditPlusLoyaltyPoints,
                                                ISNULL(cr.creditPlusRefundableBalance, 0) as CreditPlusRefundableBalance ,
	                                            ISNULL(cr.RedeemableCreditPlusLoyaltyPoints,0) as RedeemableCreditPlusLoyaltyPoints ,
	                                            ISNULL(cr.CreditPlusVirtualPoints,0) as CreditPlusVirtualPoints ,
	                                            mem.membershipId as MembershipId,
	                                            mem.membershipName as MembershipName,
	                                            ISNULL(cg.BalanceQuantity,0) As GamesBalance,
	                                            CASE WHEN c.CardIdentifier IS NULL THEN cn.CustomerName 
                                                                                            ELSE c.CardIdentifier 
                                                                                            END AS CustomerName
                                                from cards c 
                                                left outer join (select card_id,
					                                            isnull(sum(case CreditPlusType when 'A' then CreditPlusBalance
												                                                else 0 end), 0) CreditPlusCardBalance,
					                                            isnull(sum(case CreditPlusType when 'G' then CreditPlusBalance
												                                                else 0 end), 0) creditPlusCredits,
					                                            isnull(sum(case when CreditPlusType in ('P') then CreditPlusBalance
												                                                else 0 end), 0) CreditPlusItemPurchase,
					                                            isnull(sum(case CreditPlusType when 'B' then CreditPlusBalance
												                                                else 0 end), 0) CreditPlusBonus,
					                                            isnull(sum(case CreditPlusType when 'L' then CreditPlusBalance
												                                                else 0 end), 0) CreditPlusLoyaltyPoints,
					                                            isnull(sum(case CreditPlusType when 'T' then CreditPlusBalance
												                                                else 0 end), 0) CreditPlusTickets,
					                                            isnull(sum(case CreditPlusType when 'V' then CreditPlusBalance
												                                                else 0 end), 0) CreditPlusVirtualPoints,
					                                            isnull(sum(case CreditPlusType when 'M' then
													                                            (case when PlayStartTime is null
													                                            then CreditPlusBalance
													                                            else case when datediff(MI, getdate(), dateadd(MI, CreditPlusBalance, PlayStartTime)) < 0 then 0
															                                                else datediff(MI, getdate(), dateadd(MI, CreditPlusBalance, PlayStartTime)) end
													                                            end)
												                                                else 0 end), 0) CreditPlusTime,
					                                            isnull(sum(case when CreditPlusType in ('A', 'G', 'P') then
									                                            case Refundable when 'Y' then CreditPlusBalance
															                                            else 0 end
												                                                else 0 end), 0) creditPlusRefundableBalance,
				                                                isnull(sum(case when CreditPlusType in ('L') then
									                                            case isnull(ForMembershipOnly,'N') when 'Y' then 0
															                                            else CreditPlusBalance end
												                                                else 0 end), 0) RedeemableCreditPlusLoyaltyPoints
			                                            from CardCreditPlus cp
			                                            where (PeriodFrom is null or PeriodFrom <= GETDATE() or @IncludeFutureEntitlements = 1)
			                                                and (PeriodTo is null or PeriodTo >= GETDATE())
			                                                AND (PeriodTo is null or @EntitlementFromDate is null or PeriodTo > @EntitlementFromDate)
			                                                AND (PeriodFrom is null or @EntitlementToDate is null or PeriodFrom < @EntitlementToDate)
                                                            AND (@showExpiryEntitlements is null or @showExpiryEntitlements = 0 or ( PeriodTo is Not null AND @EntitlementFromDate <= PeriodTo AND PeriodTo <= @EntitlementToDate))
			                                                and isnull(validityStatus, 'Y') != 'H'
		                                                group by card_id) cr
                                                        on cr.Card_id = c.card_id Left outer join
		                                                (select card_id,
					                                            sum(
						                                            CASE 
							                                            WHEN ISNULL(Frequency, '') = 'N' THEN
								                                            BalanceGames
							                                            WHEN ISNULL(Frequency, '') = 'D' THEN
								                                            CASE WHEN (DATENAME(dayofyear, LastPlayedTime )) = (DATENAME(dayofyear , GetDate())) 
									                                            THEN BalanceGames
									                                            ELSE quantity 
								                                            END
							                                            WHEN ISNULL(Frequency, '') = 'W' THEN
								                                            CASE WHEN (DATENAME(week, LastPlayedTime)) = (DATEPART(week , GetDate())) 
									                                            THEN BalanceGames
									                                            ELSE quantity 
								                                            END
							                                            WHEN ISNULL(Frequency, '') = 'M' THEN
								                                            CASE WHEN (MONTH(LastPlayedTime)) = (MONTH(GetDate())) 
									                                            THEN BalanceGames
									                                            ELSE quantity 
								                                            END
							                                            WHEN ISNULL(Frequency, '') = 'Y' THEN
								                                            CASE WHEN (YEAR(LastPlayedTime)) = (YEAR(GetDate())) 
									                                            THEN BalanceGames
									                                            ELSE quantity 
								                                            END
							                                            ELSE
								                                            BalanceGames
						                                            END) BalanceQuantity	
			                                            from CardGames
			                                            where (FromDate is null or FromDate <= GETDATE() or @IncludeFutureEntitlements = 1)
				                                            and (ExpiryDate is null or ExpiryDate >= GETDATE())
				                                            AND (ExpiryDate is null or @EntitlementFromDate is null or ExpiryDate > @EntitlementFromDate)
				                                            AND (FromDate is null or @EntitlementToDate is null or FromDate < @EntitlementToDate)
                                                            AND (@showExpiryEntitlements is null or @showExpiryEntitlements = 0 or ( ExpiryDate is not null AND @EntitlementFromDate <= ExpiryDate AND ExpiryDate <= @EntitlementToDate))
				                                            and isnull(validityStatus, 'Y') != 'H'
				                                            and isnull(IsActive, 1) = 1
			                                            group by card_id) cg
			                                            on cg.Card_id = c.card_id
		                                                left outer join (SELECT cu.customer_id custId, m.membershipId, m.membershipName
		                                                                    from membership m, customers cu
							                                            where cu.membershipId = m.membershipId ) mem on  mem.custId = customer_Id

			                                            LEFT JOIN (Select Customer_Id as CustomerId, Concat(P.FirstName, ' ', P.LastName) AS CustomerName
                                                                                            FROM Customers C, Profile P where C.profileId = P.Id) cn ON c.Customer_Id = cn.CustomerId";

        /// <summary>
        /// Dictionary for searching Parameters for the Account summary object.
        /// </summary>
        private static readonly Dictionary<AccountSummaryViewDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountSummaryViewDTO.SearchByParameters, string>
        {
            {AccountSummaryViewDTO.SearchByParameters.ACCOUNT_ID,"c.card_id" },
            {AccountSummaryViewDTO.SearchByParameters.ACCOUNT_NUMBER,"c.card_number" },
            {AccountSummaryViewDTO.SearchByParameters.ACCOUNT_ID_LIST,"c.card_id" },
            {AccountSummaryViewDTO.SearchByParameters.ACCOUNT_NUMBER_LIST,"c.card_number" },
            {AccountSummaryViewDTO.SearchByParameters.VALID_FLAG,"c.valid_flag" },
            {AccountSummaryViewDTO.SearchByParameters.CUSTOMER_ID,"c.customer_id" },
            {AccountSummaryViewDTO.SearchByParameters.SITE_ID,"c.site_id" },
            {AccountSummaryViewDTO.SearchByParameters.REFUND_FLAG,"c.refund_flag" },
            {AccountSummaryViewDTO.SearchByParameters.CARD_TYPE_ID,"c.CardTypeId" },
            {AccountSummaryViewDTO.SearchByParameters.UPLOAD_SITE_ID,"c.upload_site_id" },
            {AccountSummaryViewDTO.SearchByParameters.MASTER_ENTITY_ID,"c.MasterEntityId" },
            {AccountSummaryViewDTO.SearchByParameters.TECHNICIAN_CARD,"c.technician_card" },
            {AccountSummaryViewDTO.SearchByParameters.CARD_IDENTIFIER,"c.CardIdentifier" },
            {AccountSummaryViewDTO.SearchByParameters.MEMBERSHIP_ID,"c.MembershipId" },
            {AccountSummaryViewDTO.SearchByParameters.MEMBERSHIP_NAME,"c.MembershipName" },
            {AccountSummaryViewDTO.SearchByParameters.ENTITLEMENT_FROMDATE,"c.start_time" },
            {AccountSummaryViewDTO.SearchByParameters.ENTITLEMENT_TODATE,"c.ExpiryDate" },
            {AccountSummaryViewDTO.SearchByParameters.INCLUDE_FUTURE_ENTITLEMENTS,"c.valid_flag" },
            {AccountSummaryViewDTO.SearchByParameters.SHOW_EXPIRY_ENTITLEMENTS,"c.ExpiryDate" }
        };

        /// <summary>
        /// Parameterized Constructor for AccountSummaryDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public AccountSummaryViewDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AccountSummary class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of AccountSummaryViewDTO</returns>
        public AccountSummaryViewDTO GetAccountSummaryDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountSummaryViewDTO accountSummaryViewDTO = new AccountSummaryViewDTO(dataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["card_id"]),
                                                                                    dataRow["card_number"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["card_number"]),
                                                                                    dataRow["issue_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["issue_date"]),
                                                                                    dataRow["face_value"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["face_value"]),
                                                                                    dataRow["refund_flag"] == DBNull.Value ? false : dataRow["refund_flag"].ToString() == "Y",
                                                                                    dataRow["refund_amount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["refund_amount"]),
                                                                                    dataRow["refund_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["refund_date"]),
                                                                                    dataRow["valid_flag"] == DBNull.Value ? true : dataRow["valid_flag"].ToString() == "Y",
                                                                                    dataRow["ticket_count"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ticket_count"]),
                                                                                    dataRow["notes"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["notes"]),
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
                                                                                    dataRow["technician_card"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["technician_card"]),
                                                                                    dataRow["tech_games"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["tech_games"]),
                                                                                    dataRow["timer_reset_card"] == DBNull.Value ? false : dataRow["timer_reset_card"].ToString() == "Y",
                                                                                    dataRow["loyalty_points"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["loyalty_points"]),
                                                                                    dataRow["CardTypeId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["CardTypeId"]),
                                                                                    dataRow["upload_site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["upload_site_id"]),
                                                                                    dataRow["upload_time"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["upload_time"]),
                                                                                    dataRow["ExpiryDate"] == DBNull.Value? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                                                                    dataRow["DownloadBatchId"] == DBNull.Value? -1 : Convert.ToInt32(dataRow["DownloadBatchId"]), 
                                                                                    dataRow["RefreshFromHQTime"] == DBNull.Value? (DateTime?)null : Convert.ToDateTime(dataRow["RefreshFromHQTime"]),
                                                                                    dataRow["CardIdentifier"] == DBNull.Value? string.Empty : Convert.ToString(dataRow["CardIdentifier"]),
                                                                                    dataRow["PrimaryCard"] == DBNull.Value ? false : dataRow["PrimaryCard"].ToString() == "Y",
                                                                                    dataRow["BalanceTime"] == DBNull.Value? -1 : Convert.ToDecimal(dataRow["BalanceTime"]),
                                                                                    dataRow["CreditPlusCardBalance"] == DBNull.Value? -1 : Convert.ToDecimal(dataRow["CreditPlusCardBalance"]),
                                                                                    dataRow["CreditPlusCredits"] == DBNull.Value? -1 : Convert.ToDecimal(dataRow["CreditPlusCredits"]),
                                                                                    dataRow["CreditPlusItemPurchase"] == DBNull.Value? -1 : Convert.ToDecimal(dataRow["CreditPlusItemPurchase"]),
                                                                                    dataRow["CreditPlusBonus"] == DBNull.Value? -1 : Convert.ToDecimal(dataRow["CreditPlusBonus"]),
                                                                                    dataRow["CreditplusTime"] == DBNull.Value? -1 : Convert.ToDecimal(dataRow["CreditPlusTime"]),
                                                                                    dataRow["CreditPlusTickets"] == DBNull.Value? -1 : Convert.ToDecimal(dataRow["CreditPlusTickets"]),
                                                                                    dataRow["CreditPlusLoyaltyPoints"] == DBNull.Value? -1 : Convert.ToDecimal(dataRow["CreditPlusLoyaltyPoints"]),
                                                                                    dataRow["CreditPlusRefundableBalance"] == DBNull.Value? -1 : Convert.ToDecimal(dataRow["CreditPlusRefundableBalance"]),
                                                                                    dataRow["RedeemableCreditPlusLoyaltyPoints"] == DBNull.Value? -1 : Convert.ToDecimal(dataRow["RedeemableCreditPlusLoyaltyPoints"]),
                                                                                    dataRow["CreditPlusVirtualPoints"] == DBNull.Value? -1 : Convert.ToDecimal(dataRow["CreditPlusVirtualPoints"]),
                                                                                    dataRow["MembershipId"] == DBNull.Value? -1 : Convert.ToInt32(dataRow["MembershipId"]),
                                                                                    dataRow["MembershipName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["MembershipName"]),
                                                                                    dataRow["customerName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["customerName"]),
                                                                                    dataRow["GamesBalance"] == DBNull.Value ? -1 : Convert.ToDecimal(dataRow["GamesBalance"]),
                                                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                                                    dataRow["last_update_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_update_time"]),
                                                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                                                                    dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                                                    );
            log.LogMethodExit(accountSummaryViewDTO);
            return accountSummaryViewDTO;
        }

        /// <summary>
        /// Gets the AccountSummary data of passed AccountId 
        /// </summary>
        /// <param name="accountId">accountId is passed as parameter</param>
        /// <returns>Returns AccountSummaryViewDTO</returns>
        public AccountSummaryViewDTO GetAccountSummaryViewDTO(int accountId)
        {
            log.LogMethodEntry(accountId);
            AccountSummaryViewDTO result = null;
            string query = SELECT_QUERY + @"WHERE c.card_id = @card_id ";
            SqlParameter parameter = new SqlParameter("@card_id",accountId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter },sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                result = GetAccountSummaryDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of AccountSummaryViewDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of AccountSummaryViewDTO </returns>
        public List<AccountSummaryViewDTO> GetAccountSummaryViewDTOList(List<KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>> searchParameters,
                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AccountSummaryViewDTO> accountSummaryDTOList = new List<AccountSummaryViewDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter entitlementFromDate;
            SqlParameter entitlementToDate;
            SqlParameter includeFutureEntitlements;
            SqlParameter showExpiryEntitlements;
            //Specifies if credits starting from future date must be included or not.
            if (searchParameters.Any(x => x.Key == AccountSummaryViewDTO.SearchByParameters.INCLUDE_FUTURE_ENTITLEMENTS))
            {
                includeFutureEntitlements = new SqlParameter("@IncludeFutureEntitlements", searchParameters.First(x => x.Key == AccountSummaryViewDTO.SearchByParameters.INCLUDE_FUTURE_ENTITLEMENTS).Value);
            }
            else
            {
                includeFutureEntitlements = new SqlParameter("@IncludeFutureEntitlements", DBNull.Value);
            }

            //Shows only those records which expires between the specified date range.
            if (searchParameters.Any(x => x.Key == AccountSummaryViewDTO.SearchByParameters.SHOW_EXPIRY_ENTITLEMENTS))
            {
                showExpiryEntitlements = new SqlParameter("@ShowExpiryEntitlements", searchParameters.First(x => x.Key == AccountSummaryViewDTO.SearchByParameters.SHOW_EXPIRY_ENTITLEMENTS).Value);
            }
            else
            {
                showExpiryEntitlements = new SqlParameter("@ShowExpiryEntitlements", DBNull.Value);
            }

            if (searchParameters.Any(x => x.Key == AccountSummaryViewDTO.SearchByParameters.ENTITLEMENT_FROMDATE))
            {
                entitlementFromDate = new SqlParameter("@EntitlementFromDate", DateTime.ParseExact(searchParameters.First(x => x.Key == AccountSummaryViewDTO.SearchByParameters.ENTITLEMENT_FROMDATE).Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
            }
            else
            {
                entitlementFromDate = new SqlParameter("@EntitlementFromDate", DBNull.Value);
            }

            if (searchParameters.Any(x => x.Key == AccountSummaryViewDTO.SearchByParameters.ENTITLEMENT_TODATE))
            {
                entitlementToDate = new SqlParameter("@EntitlementToDate", DateTime.ParseExact(searchParameters.First(x => x.Key == AccountSummaryViewDTO.SearchByParameters.ENTITLEMENT_TODATE).Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
            }
            else
            {
                entitlementToDate = new SqlParameter("@EntitlementToDate", DBNull.Value);
            }

            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0) )
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach(KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if(DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.ACCOUNT_ID ||
                           searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.CUSTOMER_ID ||
                           searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.MEMBERSHIP_ID ||
                           searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.MASTER_ENTITY_ID ||
                           searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.UPLOAD_SITE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.ACCOUNT_NUMBER ||
                                 searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.CARD_IDENTIFIER ||
                                 searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.MEMBERSHIP_NAME ||
                                 searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.TECHNICIAN_CARD)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if(searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.ACCOUNT_NUMBER_LIST ||
                                searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.ACCOUNT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.VALID_FLAG ||
                                 searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.REFUND_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.ENTITLEMENT_FROMDATE ||
    searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.ENTITLEMENT_TODATE)
                        {
                            //query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",getdate()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            //parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.INCLUDE_FUTURE_ENTITLEMENTS ||
                                 searchParameter.Key == AccountSummaryViewDTO.SearchByParameters.SHOW_EXPIRY_ENTITLEMENTS)
                        {

                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
            parameters.Add(entitlementFromDate);
            parameters.Add(entitlementToDate);
            parameters.Add(includeFutureEntitlements);
            parameters.Add(showExpiryEntitlements);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountSummaryViewDTO accountSummaryViewDTO = GetAccountSummaryDTO(dataRow);
                    accountSummaryDTOList.Add(accountSummaryViewDTO);
                }
            }
            log.LogMethodExit(accountSummaryDTOList);
            return accountSummaryDTOList;
        }
    }

}