/********************************************************************************************
 * Project Name - Contact Data Handler
 * Description  - Data handler of the Contact class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Lakshminarayana     Created 
 *2.60        08-May-2019   Nitin Pai           Added UUID parameter for Guest App
 *2.70.2      23-Jul-2019   Girish Kundar       Modified : Added RefreshDTO() method and Fix for SQL Injection Issue
 *2.70.2      15-Oct-2019   Nitin Pai           Gateway Cleanup
 *2.70.2      04-Feb-2020   Nitin Pai           Guest App phase 2 changes
 *2.70.2      03-Mar-2020   Jeevan              added filter option excludedProductList to exclude based on product name
 *                                              modifed issues search based on from and to date
 *2.130.0     19-July-2021     Girish Kundar       Modified : Virtual point column added part of Arcade changes                                              
 *2.130.2     13-Dec-2021   Deeksha             Modified : Handled CounterItems and PlayCredits columns                                   
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
    ///  AccountActivityView Data Handler - Handles insert, update and select of  AccountActivityView objects
    /// </summary>
    public class AccountActivityDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<AccountActivityDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountActivityDTO.SearchByParameters, string>
            {
                {AccountActivityDTO.SearchByParameters.ACCOUNT_ID, "cav.card_id"},
                {AccountActivityDTO.SearchByParameters.ACCOUNT_ID_LIST, "cav.card_id"},
                {AccountActivityDTO.SearchByParameters.FROM_DATE, "cav.date"},
                {AccountActivityDTO.SearchByParameters.TO_DATE, "cav.date"},
                {AccountActivityDTO.SearchByParameters.TRANSACTION_STATUS, "cav.Status"},
                {AccountActivityDTO.SearchByParameters.EXCLUDED_PRODUCT_LIST, "cav.Product"},
                {AccountActivityDTO.SearchByParameters.SITE, "cav.Site"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT ROW_NUMBER() OVER(ORDER BY date desc, product desc) AS RowNumber, cav.* FROM CardActivityView as cav ";
        /// <summary>
        /// Default constructor of AccountActivityViewDataHandler class
        /// </summary>
        public AccountActivityDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AccountActivityDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AccountActivityDTO</returns>
        public AccountActivityDTO GetAccountActivityDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountActivityDTO accountActivityDTO = new AccountActivityDTO(Convert.ToInt32(dataRow["card_id"]),
                                            dataRow["Date"] == DBNull.Value ? (DateTime?) null: Convert.ToDateTime(dataRow["Date"]),
                                            dataRow["Product"] == DBNull.Value ? "" : Convert.ToString(dataRow["Product"]),
                                            dataRow["Amount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Amount"]),
                                            dataRow["Credits"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Credits"]),
                                            dataRow["Courtesy"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Courtesy"]),
                                            dataRow["Bonus"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Bonus"]),
                                            dataRow["Time"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Time"]),
                                            dataRow["Tokens"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Tokens"]),
                                            dataRow["Tickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Tickets"]),
                                            dataRow["Loyalty Points"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Loyalty Points"]),
                                            dataRow["Site"] == DBNull.Value ? "" : Convert.ToString(dataRow["Site"]),
                                            dataRow["POS"] == DBNull.Value ? "" : Convert.ToString(dataRow["POS"]),
                                            dataRow["Username"] == DBNull.Value ? "" : Convert.ToString(dataRow["Username"]),
                                            dataRow["Quantity"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Quantity"]),
                                            dataRow["Price"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Price"]),
                                            dataRow["RefId"] == DBNull.Value ? (int?) null: Convert.ToInt32(dataRow["RefId"]),
                                            dataRow["ActivityType"] == DBNull.Value ? "" : Convert.ToString(dataRow["ActivityType"]),
                                            dataRow["RowNumber"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["RowNumber"]),
                                            dataRow["Virtual Points"] == DBNull.Value ? (decimal?)null : Convert.ToInt32(dataRow["Virtual Points"]),
                                           dataRow["CounterItems"] == DBNull.Value ? (decimal?)null : Convert.ToInt32(dataRow["CounterItems"]),
                                            dataRow["PlayCredits"] == DBNull.Value ? (decimal?)null : Convert.ToInt32(dataRow["PlayCredits"])
                                         );
            log.LogMethodExit(accountActivityDTO);
            return accountActivityDTO;
        }

        /// <summary>
        /// Gets the AccountActivityDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AccountActivityDTO matching the search criteria</returns>
        public List<AccountActivityDTO> GetAccountActivityDTOList(List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> searchParameters, int numberOfRecords = -1, int pageNumber = 0, int lastRowNumberId = -1)
        {
            log.LogMethodEntry(searchParameters);
            List<AccountActivityDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;

            String offsetQuery = "";
            if (numberOfRecords > -1 && (pageNumber * numberOfRecords) >= 0)
            {
                offsetQuery = " OFFSET " + pageNumber * numberOfRecords + " ROWS FETCH NEXT " + numberOfRecords.ToString() + " ROWS ONLY";
            }

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;

                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AccountActivityDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AccountActivityDTO.SearchByParameters.ACCOUNT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountActivityDTO.SearchByParameters.ACCOUNT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountActivityDTO.SearchByParameters.TRANSACTION_STATUS)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " is null or " + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")) ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountActivityDTO.SearchByParameters.FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == AccountActivityDTO.SearchByParameters.TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == AccountActivityDTO.SearchByParameters.EXCLUDED_PRODUCT_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " NOT IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountActivityDTO.SearchByParameters.SITE)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + DBSearchParameters[searchParameter.Key] + " IS NULL)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                selectQuery = "SELECT * FROM (" + selectQuery + query + ") AS Activities WHERE RowNumber > " + (lastRowNumberId > -1 ? lastRowNumberId.ToString() : "0") + " order by date desc, product desc " + offsetQuery;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<AccountActivityDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountActivityDTO accountActivityDTO = GetAccountActivityDTO(dataRow);
                    list.Add(accountActivityDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the Login Key matching the current user
        /// </summary>
        /// <param name="loginId">loginId</param>
        /// <returns>Returns CompanyKey</returns>
        public string GetCompanyKey()
        {
            try
            {
                log.LogMethodEntry();
                string selectAttributeQuery = @"select login_key from company";
                DataTable companyDetails = dataAccessHandler.executeSelectQuery(selectAttributeQuery, null, sqlTransaction);

                //string selectAttributeQuery = @"select login_key from company where login_key = @loginKey ";
                //SqlParameter[] getAttributeIdParameters = new SqlParameter[1];
                //getAttributeIdParameters[0] = new SqlParameter("@loginKey", loginId);
                //DataTable companyDetails = dataAccessHandler.executeSelectQuery(selectAttributeQuery, getAttributeIdParameters);                
                DataRow companyDetailsDataRow = companyDetails.Rows[0];
                string companyKey = companyDetailsDataRow["login_key"].ToString();
                log.LogMethodExit(companyKey);
                return companyKey;
            }
            catch (Exception ex)
            {
                log.LogMethodExit(null, "throwing exception in GetCompanyKey() method" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Get Account Activity DTO List By Account Id List
        /// </summary> 
        /// <returns></returns>
        public List<AccountActivityDTO> GetAccountActivityDTOListByAccountIdList(List<int> accountIdList, int numberOfRecords = -1, int pageNumber = 0, int lastRowNumberId = -1)
        {
            log.LogMethodEntry(accountIdList, numberOfRecords, pageNumber, lastRowNumberId);
            List<AccountActivityDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if (accountIdList != null && accountIdList.Any())
            {
                selectQuery = selectQuery + ", @accountIdList List WHERE cav.Card_Id = List.Id ";
            }
            else
            {
                log.LogMethodExit(null, "throwing exception");
                log.LogVariableState("accountIdList", accountIdList);
                throw new Exception("The input parameter (accountIdList) is not provided");
            }

            String offsetQuery = "";
            if (numberOfRecords > -1 && (pageNumber * numberOfRecords) >= 0)
            {
                offsetQuery = " OFFSET " + pageNumber * numberOfRecords + " ROWS FETCH NEXT " + numberOfRecords.ToString() + " ROWS ONLY";
            }


            selectQuery = "SELECT * FROM (" + selectQuery + ") AS Activities WHERE RowNumber > " + (lastRowNumberId > -1 ? lastRowNumberId.ToString() : "0") + " order by date desc, product desc " + offsetQuery;

            DataTable table = dataAccessHandler.BatchSelect(selectQuery, "@accountIdList", accountIdList, parameters.ToArray(), sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetAccountActivityDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
