/********************************************************************************************
 * Project Name - AccountDiscountsSummaryView
 * Description  - AccountDiscountsSummaryView Data handler
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
    /// AccountDiscountsSummaryView DataHandler class
    /// </summary>
    public class AccountDiscountsSummaryViewDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * from CardDiscountsView AS cdv ";

        private static readonly Dictionary<AccountDiscountsSummaryViewDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountDiscountsSummaryViewDTO.SearchByParameters, string>
        {
            {AccountDiscountsSummaryViewDTO.SearchByParameters.ACCOUNT_DISCOUNT_ID, "cdv.CardDiscountId"},
            {AccountDiscountsSummaryViewDTO.SearchByParameters.ACCOUNT_ID, "cdv.card_id"},
            {AccountDiscountsSummaryViewDTO.SearchByParameters.SITE_ID, "cdv.site_id"}
        };

        /// <summary>
        /// Default constructor of AccountDiscountsSummaryDataHandler class
        /// </summary>
        public AccountDiscountsSummaryViewDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AccountDiscountsSummaryViewDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AccountDiscountDTO</returns>
        private AccountDiscountsSummaryViewDTO GetAccountDiscountsSummaryViewDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountDiscountsSummaryViewDTO accountDiscountsSummaryViewDTO = new AccountDiscountsSummaryViewDTO(
                                            dataRow["CardDiscountId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardDiscountId"]),
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
                                            dataRow["SubscriptionBillingScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SubscriptionBillingScheduleId"]),
                                            dataRow["discount_name"] == DBNull.Value ? "" : Convert.ToString(dataRow["discount_name"]),
                                            dataRow["MembershipName"] == DBNull.Value ? "" : Convert.ToString(dataRow["MembershipName"])
                                            );
            log.LogMethodExit(accountDiscountsSummaryViewDTO);
            return accountDiscountsSummaryViewDTO;
        }

        /// <summary>
        /// Gets the AAccountDiscountsSummaryView data of passed AccountDiscount Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns AccountDiscountsSummaryViewDTO</returns>
        public AccountDiscountsSummaryViewDTO GetAccountDiscountsSummaryViewDTO(int id)
        {
            log.LogMethodEntry(id);
            AccountDiscountsSummaryViewDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE cdv.CardDiscountId = @CardDiscountId";
            SqlParameter parameter = new SqlParameter("@CardDiscountId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetAccountDiscountsSummaryViewDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the AccountDiscountsSummaryViewDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the AccountDiscountsSummaryViewDTO matching the search criteria</returns>
        public List<AccountDiscountsSummaryViewDTO> GetAccountDiscountSummaryViewDTOList(List<KeyValuePair<AccountDiscountsSummaryViewDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AccountDiscountsSummaryViewDTO> accountDiscountsSummaryViewDTOList = new List<AccountDiscountsSummaryViewDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AccountDiscountsSummaryViewDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == AccountDiscountsSummaryViewDTO.SearchByParameters.ACCOUNT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountDiscountsSummaryViewDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    accountDiscountsSummaryViewDTOList.Add(GetAccountDiscountsSummaryViewDTO(dataRow));
                }
            }
            log.LogMethodExit(accountDiscountsSummaryViewDTOList);
            return accountDiscountsSummaryViewDTOList;
        }
    }
}
