/********************************************************************************************
 * Project Name - AccountSummeryGamesView
 * Description  - AccountSummeryGamesView Data handler
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.130.11    07-Sep-2022     Yashodhara C H      Created
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// AccountGamesSummaryView Data Handler - select of  AccountGamesSummaryView objects
    /// </summary>
    public class AccountGamesSummaryViewDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CardGamesView AS cgv ";

        private static readonly Dictionary<AccountGamesSummaryViewDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountGamesSummaryViewDTO.SearchByParameters, string>
        {
            {AccountGamesSummaryViewDTO.SearchByParameters.ACCOUNT_ID, "cgv.card_id"},
            {AccountGamesSummaryViewDTO.SearchByParameters.SITE_ID, "cgv.site_id"}
        };

        /// <summary>
        /// Default constructor of AccountGamesSummaryViewDataHandler class
        /// </summary>
        public AccountGamesSummaryViewDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AccountGameDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AccountGamesSummaryViewDTO</returns>
        private AccountGamesSummaryViewDTO GetAccountGamesSummaryViewDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountGamesSummaryViewDTO accountGamesSummaryViewDTO = new AccountGamesSummaryViewDTO(
                                                         dataRow["Card_Game_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Card_Game_Id"]),
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
                                                         dataRow["SubscriptionBillingScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SubscriptionBillingScheduleId"]),
                                                         dataRow["GameName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GameName"]),
                                                         dataRow["ProfileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProfileName"]),
                                                         dataRow["MembershipName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["MembershipName"]),
                                                         dataRow["BalanceQuantity"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BalanceQuantity"])
                                            );
            log.LogMethodExit(accountGamesSummaryViewDTO);
            return accountGamesSummaryViewDTO;
        }

        /// <summary>
        /// Gets the AccountGamesSummaryView data of passed AccountGame Id
        /// </summary>
        /// <param name="accountGameId">integer type parameter</param>
        /// <returns>ReturnsAccountGamesSummaryViewDTO</returns>
        public AccountGamesSummaryViewDTO GetAccountGamesSummaryViewDTO(int accountGameId)
        {
            log.LogMethodEntry(accountGameId);
            AccountGamesSummaryViewDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE cgv.Card_Game_Id = @Card_Game_Id";
            SqlParameter parameter = new SqlParameter("@Card_Game_Id", accountGameId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetAccountGamesSummaryViewDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the AccountGamesSummaryViewDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the AccountGamesSummaryViewDTO matching the search criteria</returns>
        public List<AccountGamesSummaryViewDTO> GetAccountGamesSummaryViewDTOs(List<KeyValuePair<AccountGamesSummaryViewDTO.SearchByParameters, string>> searchParameters)
        {

            log.LogMethodEntry(searchParameters);
            List<AccountGamesSummaryViewDTO> accountGamesSumaryViewDTOList = new List<AccountGamesSummaryViewDTO>();
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AccountGamesSummaryViewDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AccountGamesSummaryViewDTO.SearchByParameters.ACCOUNT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountGamesSummaryViewDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                   AccountGamesSummaryViewDTO accountGamesSumaryViewDTO = GetAccountGamesSummaryViewDTO(dataRow);
                   accountGamesSumaryViewDTOList.Add(accountGamesSumaryViewDTO);
                }
            }
            log.LogMethodExit(accountGamesSumaryViewDTOList);
            return accountGamesSumaryViewDTOList;
        }

     }
}
