/********************************************************************************************
 * Project Name - Account
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      17-June-2019     Girish Kundar           Modified: Fix for the SQL Injection Issue 
 *2.70.2      05-Dec-2019      Jinto Thomas            Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.CardCore
{
    public class DailyCardBalanceDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM DailyCardBalance AS dcb ";
        private static readonly Dictionary<DailyCardBalanceDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DailyCardBalanceDTO.SearchByParameters, string>
            {
                {DailyCardBalanceDTO.SearchByParameters.DAILY_CARD_BALANCE_ID, "dcb.DailyCardBalanceId"},
                {DailyCardBalanceDTO.SearchByParameters.CUSTOMER_ID, "dcb.CustomerId"},
                {DailyCardBalanceDTO.SearchByParameters.CARD_ID, "dcb.CardId"},
                {DailyCardBalanceDTO.SearchByParameters.CARD_BALANCE_DATE, "dcb.CardBalanceDate"},
                {DailyCardBalanceDTO.SearchByParameters.CARD_BALANCE_DATE_FROM, "dcb.CardBalanceDate"},
                {DailyCardBalanceDTO.SearchByParameters.CARD_BALANCE_DATE_TO, "dcb.CardBalanceDate"},
                {DailyCardBalanceDTO.SearchByParameters.CREDIT_PLUS_ATTRIBUTE, "dcb.CreditPlusAttribute"},
                {DailyCardBalanceDTO.SearchByParameters.MASTER_ENTITY_ID,"dcb.MasterEntityId"},
                {DailyCardBalanceDTO.SearchByParameters.SITE_ID, "dcb.Site_id"}
            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of DailyCardBalanceDataHandler class
        /// </summary>
        public DailyCardBalanceDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DailyCardBalance Record.
        /// </summary>
        /// <param name="dailyCardBalanceDTO">DailyCardBalanceDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(DailyCardBalanceDTO dailyCardBalanceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dailyCardBalanceDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@dailyCardBalanceId", dailyCardBalanceDTO.DailyCardBalanceId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@customerId", dailyCardBalanceDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardId", dailyCardBalanceDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardBalanceDate", dailyCardBalanceDTO.CardBalanceDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@totalCreditPlusBalance", dailyCardBalanceDTO.TotalCreditPlusBalance));
            parameters.Add(dataAccessHandler.GetSQLParameter("@earnedCreditPlusBalance", dailyCardBalanceDTO.EarnedCreditPlusBalance));
            parameters.Add(dataAccessHandler.GetSQLParameter("@creditPlusAttribute", dailyCardBalanceDTO.CreditPlusAttribute));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", dailyCardBalanceDTO.MasterEntityId, true));
            log.LogMethodExit(parameters); 
            return parameters;
        }

        /// <summary>
        /// Inserts the DailyCardBalance record to the database
        /// </summary>
        /// <param name="dailyCardBalanceDTO">DailyCardBalanceDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public DailyCardBalanceDTO InsertDailyCardBalance(DailyCardBalanceDTO dailyCardBalanceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dailyCardBalanceDTO, loginId, siteId);
            string query = @"INSERT INTO DailyCardBalance 
                                        ( 
                                             [CustomerId],
                                             [CardId],
                                             [CardBalanceDate],
                                             [CreditPlusAttribute],
                                             [TotalCreditPlusBalance],
                                             [EarnedCreditPlusBalance],
                                             [CreatedBy],
                                             [CreationDate],
                                             [LastUpdatedBy],
                                             [LastupdateDate],
                                             [Guid],
                                             [site_id],
                                             [MasterEntityId]
                                        ) 
                                VALUES 
                                        (
                                            @customerId ,
                                            @cardId ,
                                            @cardBalanceDate,
                                            @creditPlusAttribute,
                                            @totalCreditPlusBalance,
                                            @earnedCreditPlusBalance, 
                                            @createdBy,
                                            GETDATE(),
                                            @lastUpdatedBy,
                                            GETDATE(),
                                            NEWID(),
                                            @siteId, 
                                            @masterEntityId 
                                        ) SELECT * FROM DailyCardBalance WHERE DailyCardBalanceId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(dailyCardBalanceDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDailyCardBalanceDTO(dailyCardBalanceDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting DailyCardBalance", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dailyCardBalanceDTO);
            return dailyCardBalanceDTO;
        }

        /// <summary>
        /// updates the DailyCardBalanceDTO with Id ,who columns values for further process.
        /// </summary>
        /// <param name="dailyCardBalanceDTO"></param>
        /// <param name="dt"></param>
        private void RefreshDailyCardBalanceDTO(DailyCardBalanceDTO dailyCardBalanceDTO, DataTable dt)
        {
            log.LogMethodEntry(dailyCardBalanceDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                dailyCardBalanceDTO.DailyCardBalanceId = Convert.ToInt32(dt.Rows[0]["DailyCardBalanceId"]);
                dailyCardBalanceDTO.LastUpdateDate = dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"]);
                dailyCardBalanceDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                dailyCardBalanceDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                dailyCardBalanceDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                dailyCardBalanceDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                dailyCardBalanceDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the DailyCardBalance record
        /// </summary>
        /// <param name="dailyCardBalanceDTO">DailyCardBalanceDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public DailyCardBalanceDTO UpdateDailyCardBalance(DailyCardBalanceDTO dailyCardBalanceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dailyCardBalanceDTO, loginId, siteId);
            string query = @"UPDATE DailyCardBalance 
                                SET [CustomerId] = @customerId,
                                    [CardId] = @cardId,
                                    [CardBalanceDate] = @cardBalanceDate,
                                    [CreditPlusAttribute] = @creditPlusAttribute,
                                    [TotalCreditPlusBalance] = @totalCreditPlusBalance,
                                    [EarnedCreditPlusBalance] = @earnedCreditPlusBalance ,
                                    [LastUpdatedBy] = @lastUpdatedBy,
                                    [LastupdateDate] = getdate(),
                                    --[site_id] = @siteId,
                                    [MasterEntityId] = @masterEntityId
                             WHERE DailyCardBalanceId = @dailyCardBalanceId
                             SELECT * FROM DailyCardBalance WHERE DailyCardBalanceId = @dailyCardBalanceId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(dailyCardBalanceDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDailyCardBalanceDTO(dailyCardBalanceDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating DailyCardBalance", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dailyCardBalanceDTO);
            return dailyCardBalanceDTO;
        }

        /// <summary>
        /// Converts the Data row object to DailyCardBalanceDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DailyCardBalanceDTO</returns>
        private DailyCardBalanceDTO GetDailyCardBalanceDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DailyCardBalanceDTO dailyCardBalanceDTO = new DailyCardBalanceDTO(Convert.ToInt32(dataRow["DailyCardBalanceId"]),
                                            dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                            dataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardId"]),
                                            dataRow["CardBalanceDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CardBalanceDate"]),
                                            dataRow["TotalCreditPlusBalance"] == DBNull.Value ? 0: Convert.ToDouble(dataRow["TotalCreditPlusBalance"]),
                                            dataRow["EarnedCreditPlusBalance"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["EarnedCreditPlusBalance"]),
                                            dataRow["CreditPlusAttribute"] == DBNull.Value ? string.Empty : dataRow["CreditPlusAttribute"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty: dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"])
                                            );
            log.LogMethodExit(dailyCardBalanceDTO);
            return dailyCardBalanceDTO;
        } 

        /// <summary>
        /// Gets the DailyCardBalance data of passed DailyCardBalance Id
        /// </summary>
        /// <param name="dailyCardBalanceId">integer type parameter</param>
        /// <returns>Returns DailyCardBalanceDTO</returns>
        public DailyCardBalanceDTO GetDailyCardBalanceDTO(int dailyCardBalanceId)
        {
            log.LogMethodEntry(dailyCardBalanceId);
            DailyCardBalanceDTO returnValue = null;
            string query = SELECT_QUERY + " WHERE dcb.DailyCardBalanceId = @dailyCardBalanceId";
            SqlParameter parameter = new SqlParameter("@dailyCardBalanceId", dailyCardBalanceId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetDailyCardBalanceDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the DailyCardBalanceDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DailyCardBalanceDTO matching the search criteria</returns>
        public List<DailyCardBalanceDTO> GetDailyCardBalanceDTOList(List<KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<DailyCardBalanceDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == DailyCardBalanceDTO.SearchByParameters.DAILY_CARD_BALANCE_ID ||
                            searchParameter.Key == DailyCardBalanceDTO.SearchByParameters.CARD_ID ||
                            searchParameter.Key == DailyCardBalanceDTO.SearchByParameters.CUSTOMER_ID  ||
                            searchParameter.Key == DailyCardBalanceDTO.SearchByParameters.MASTER_ENTITY_ID
                           )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DailyCardBalanceDTO.SearchByParameters.SITE_ID )
                               
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        } 
                        else if (searchParameter.Key == DailyCardBalanceDTO.SearchByParameters.CARD_BALANCE_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == DailyCardBalanceDTO.SearchByParameters.CARD_BALANCE_DATE_FROM)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == DailyCardBalanceDTO.SearchByParameters.CARD_BALANCE_DATE_TO)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));   //"yyyy-MM-dd HH:mm:ss.fff" 
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == DailyCardBalanceDTO.SearchByParameters.CREDIT_PLUS_ATTRIBUTE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<DailyCardBalanceDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DailyCardBalanceDTO dailyCardBalanceDTO = GetDailyCardBalanceDTO(dataRow);
                    list.Add(dailyCardBalanceDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
