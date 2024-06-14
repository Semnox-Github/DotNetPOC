/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Data Handler object of Accounting Calendar Master
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       24-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Globalization;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Accounting Calendar Master DataHandler
    /// </summary>
    public class AccountingCalendarMasterDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AccountingCalendarMaster AS acm ";

        private static readonly Dictionary<AccountingCalendarMasterDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountingCalendarMasterDTO.SearchByParameters, string>
            {
                {AccountingCalendarMasterDTO.SearchByParameters.ACCOUNTING_CALENDAR_MASTER_ID, "acm.AccountingCalendarMasterId"},
                {AccountingCalendarMasterDTO.SearchByParameters.ACCOUNTING_CALENDAR_FROM_DATE, "acm.Date"},
                {AccountingCalendarMasterDTO.SearchByParameters.ACCOUNTING_CALENDAR_TO_DATE, "acm.Date"},
                {AccountingCalendarMasterDTO.SearchByParameters.IS_ACTIVE, "acm.IsActive"},
                {AccountingCalendarMasterDTO.SearchByParameters.MASTER_ENTITY_ID, "acm.MasterEntityId"},
                {AccountingCalendarMasterDTO.SearchByParameters.SITE_ID, "acm.site_id"},
            };

        /// <summary>
        /// Parameterized Constructor for AccountingCalendarMasterDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public AccountingCalendarMasterDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(AccountingCalendarMasterDTO accountingCalendarMasterDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountingCalendarMasterDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AccountingCalendarMasterId", accountingCalendarMasterDTO.AccountingCalendarMasterId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Date", accountingCalendarMasterDTO.AccountingCalenderDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MM", accountingCalendarMasterDTO.Month));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DD", accountingCalendarMasterDTO.Day));
            parameters.Add(dataAccessHandler.GetSQLParameter("@YYYY", accountingCalendarMasterDTO.Year));
            parameters.Add(dataAccessHandler.GetSQLParameter("@QTR", accountingCalendarMasterDTO.Quarter));
            parameters.Add(dataAccessHandler.GetSQLParameter("@WeekMonth", accountingCalendarMasterDTO.WeekMonth));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DayYear", accountingCalendarMasterDTO.DayYear));
            parameters.Add(dataAccessHandler.GetSQLParameter("@WeekYear", accountingCalendarMasterDTO.WeekYear));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DayWeek", accountingCalendarMasterDTO.DayWeek));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DayQtr", accountingCalendarMasterDTO.DayQtr));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MonthQtr", accountingCalendarMasterDTO.MonthQtr));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DAY", accountingCalendarMasterDTO.Day));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", accountingCalendarMasterDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", accountingCalendarMasterDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        internal AccountingCalendarMasterDTO GetccountingCalendarMasterId(int accountingCalendarMasterId)
        {
            log.LogMethodEntry(accountingCalendarMasterId);
            AccountingCalendarMasterDTO result = null;
            string query = SELECT_QUERY + @" WHERE acm.AccountingCalendarMasterId = @AccountingCalendarMasterId";
            SqlParameter parameter = new SqlParameter("@AccountingCalendarMasterId", accountingCalendarMasterId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAccountingCalendarMasterDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal void Delete(AccountingCalendarMasterDTO accountingCalendarMasterDTO)
        {
            log.LogMethodEntry(accountingCalendarMasterDTO);
            string query = @"DELETE  
                             FROM AccountingCalendarMaster
                             WHERE AccountingCalendarMaster.AccountingCalendarMasterId = @AccountingCalendarMasterId";
            SqlParameter parameter = new SqlParameter("@AccountingCalendarMasterId", accountingCalendarMasterDTO.AccountingCalendarMasterId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            accountingCalendarMasterDTO.AcceptChanges();
            log.LogMethodExit();
        }


        private void RefreshAccountingCalendarMasterDTO(AccountingCalendarMasterDTO accountingCalendarMasterDTO, DataTable dt)
        {
            log.LogMethodEntry(accountingCalendarMasterDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                accountingCalendarMasterDTO.AccountingCalendarMasterId = Convert.ToInt32(dt.Rows[0]["AccountingCalendarMasterId"]);
                accountingCalendarMasterDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                accountingCalendarMasterDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                accountingCalendarMasterDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                accountingCalendarMasterDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                accountingCalendarMasterDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                accountingCalendarMasterDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private AccountingCalendarMasterDTO GetAccountingCalendarMasterDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountingCalendarMasterDTO accountingCalendarMasterDTO = new AccountingCalendarMasterDTO(dataRow["AccountingCalendarMasterId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AccountingCalendarMasterId"]),
                                                dataRow["Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Date"]),
                                                dataRow["MM"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["MM"]),
                                                dataRow["DD"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["DD"]),
                                                dataRow["YYYY"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["YYYY"]), 
                                                dataRow["QTR"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["QTR"]),
                                                dataRow["WeekMonth"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["WeekMonth"]),
                                                dataRow["DayYear"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["DayYear"]),
                                                dataRow["WeekYear"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["WeekYear"]),
                                                dataRow["DayWeek"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["DayWeek"]),
                                                dataRow["DayQtr"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["DayQtr"]),
                                                dataRow["MonthQtr"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["MonthQtr"]),
                                                dataRow["DAY"] == DBNull.Value ? null : Convert.ToString(dataRow["DAY"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? null : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? null : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["Guid"] == DBNull.Value ? null : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]));
            log.LogMethodExit(accountingCalendarMasterDTO);
            return accountingCalendarMasterDTO;
        }

        internal AccountingCalendarMasterDTO Insert(AccountingCalendarMasterDTO accountingCalendarMasterDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountingCalendarMasterDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[AccountingCalendarMaster]
                               ([Date]
                               ,[MM]
                               ,[DD]
                               ,[YYYY]
                               ,[QTR]
                               ,[WeekMonth]
                               ,[DayYear]
                               ,[WeekYear]
                               ,[DayWeek]
                               ,[DayQtr]
                               ,[MonthQtr]
                               ,[DAY]
                               ,[IsActive]
                               ,[CreatedBy]
                               ,[CreationDate]
                               ,[LastUpdatedBy]
                               ,[LastUpdateDate]
                               ,[site_id]
                               ,[Guid]
                               ,[MasterEntityId])
                               
                         VALUES
                               (
                                @Date,
                                @MM,
                                @DD,
                                @YYYY,
                                @QTR,
                                @WeekMonth,
                                @DayYear,
                                @WeekYear,
                                @DayWeek,
                                @DayQtr,
                                @MonthQtr,
                                @DAY,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                @SiteId,
                                NEWID(), 
                                @MasterEntityId
                                 )
                                SELECT * FROM AccountingCalendarMaster WHERE AccountingCalendarMasterId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountingCalendarMasterDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountingCalendarMasterDTO(accountingCalendarMasterDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(accountingCalendarMasterDTO);
            return accountingCalendarMasterDTO;
        }

        internal AccountingCalendarMasterDTO Update(AccountingCalendarMasterDTO accountingCalendarMasterDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountingCalendarMasterDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[AccountingCalendarMaster] set
                               [Date]               = @Date,
                               [MM]                 = @MM,
                               [DD]                 = @DD,
                               [YYYY]               = @YYYY,
                               [QTR]                = @QTR,
                               [WeekMonth]          = @WeekMonth,
                               [DayYear]            = @DayYear,
                               [WeekYear]           = @WeekYear,
                               [DAY]                = @DAY,
                               [IsActive]           = @IsActive
                               [LastUpdatedBy]      = @LastUpdatedBy,
                               [MasterEntityId]     = @MasterEntityId,
                               [LastUpdateDate]     = GETDATE()
                               where AccountingCalendarMasterId = @AccountingCalendarMasterId
                             SELECT * FROM AccountingCalendarMaster WHERE AccountingCalendarMasterId = @AccountingCalendarMasterId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountingCalendarMasterDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountingCalendarMasterDTO(accountingCalendarMasterDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(accountingCalendarMasterDTO);
            return accountingCalendarMasterDTO;
        }

        internal List<AccountingCalendarMasterDTO> GetAccountingCalendarMasterDTOList(List<KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AccountingCalendarMasterDTO> accountingCalendarMasterDTOList = new List<AccountingCalendarMasterDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty ;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AccountingCalendarMasterDTO.SearchByParameters.ACCOUNTING_CALENDAR_MASTER_ID ||
                            searchParameter.Key == AccountingCalendarMasterDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountingCalendarMasterDTO.SearchByParameters.ACCOUNTING_CALENDAR_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == AccountingCalendarMasterDTO.SearchByParameters.ACCOUNTING_CALENDAR_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == AccountingCalendarMasterDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }   
                        else if (searchParameter.Key == AccountingCalendarMasterDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                accountingCalendarMasterDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetAccountingCalendarMasterDTO(x)).ToList();
            }
            log.LogMethodExit(accountingCalendarMasterDTOList);
            return accountingCalendarMasterDTOList;
        }
    }
}
