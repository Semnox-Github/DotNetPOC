/********************************************************************************************
 * Project Name - Fiscalization
 * Description  - Class for FiscalizationDataHandler 
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 ********************************************************************************************* 
 *2.155.1     13-Aug-2023       Guru S A           Created for Chile fiscalization       
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Fiscalization
{
    /// <summary>
    /// FiscalizationDataHandler
    /// </summary>
    internal class FiscalizationDataHandler
    {
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected DataAccessHandler dataAccessHandler;
        protected SqlTransaction sqlTransaction;
        protected string selectQry;
        private static readonly Dictionary<FiscalizationPendingTransactionDTO.SearchParameters, string> DBSearchParameters = new Dictionary<FiscalizationPendingTransactionDTO.SearchParameters, string>
            {
                {FiscalizationPendingTransactionDTO.SearchParameters.TRX_ID, "th.trxId"},
                {FiscalizationPendingTransactionDTO.SearchParameters.TRX_FROM_DATE, "th.trxDate"},
                {FiscalizationPendingTransactionDTO.SearchParameters.TRX_TO_DATE, "th.trxDate"},
                {FiscalizationPendingTransactionDTO.SearchParameters.IGNORE_WIP_TRX, ""}
            };

        /// <summary>
        /// Default constructor of FiscalizationDataHandler class
        /// </summary>
        internal FiscalizationDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        protected List<FiscalizationPendingTransactionDTO> CreateFiscalizationPendingTransactionDTOList(SqlDataReader reader)
        {
            log.LogMethodEntry(reader);
            List<FiscalizationPendingTransactionDTO> dTOList = new List<FiscalizationPendingTransactionDTO>();
            int fiscalization = reader.GetOrdinal("Fiscalization");
            int transactionId = reader.GetOrdinal("TransactionId");
            int transactionNumber = reader.GetOrdinal("TransactionNumber");
            int transactionDate = reader.GetOrdinal("transactionDate");
            int transactionCustomerName = reader.GetOrdinal("transactionCustomerName");
            int transactionPOSMachine = reader.GetOrdinal("transactionPOSMachine");
            int transactionJsonBuildError = reader.GetOrdinal("JsonErrorInfo");
            int transactionPostError = reader.GetOrdinal("PostErrorInfo");
            int transactionCreationDate = reader.GetOrdinal("transactionCreationDate");
            int transactionCreatedBy = reader.GetOrdinal("transactionCreatedBy");
            int transactionLastUpdateDate = reader.GetOrdinal("transactionLastUpdateDate");
            int transactionLastUpdatedBy = reader.GetOrdinal("transactionLastUpdatedBy");
            int latestRequestId = reader.GetOrdinal("latestRequestId");
            int latestRequestStatus = reader.GetOrdinal("latestRequestStatus");
            int latestRequestPhase = reader.GetOrdinal("latestRequestPhase");
            int invoiceOptionName = reader.GetOrdinal("invoiceOptionName");
            int taxCode = reader.GetOrdinal("taxCode");
            int uniqueId = reader.GetOrdinal("uniqueId");
            int site_id = reader.GetOrdinal("site_id");

            while (reader.Read())
            {
                FiscalizationPendingTransactionDTO transactionDTO = new FiscalizationPendingTransactionDTO(
                                        reader.IsDBNull(fiscalization) ? string.Empty : reader.GetString(fiscalization),
                                        reader.IsDBNull(transactionId) ? -1 : reader.GetInt32(transactionId),
                                        reader.IsDBNull(transactionNumber) ? string.Empty : reader.GetString(transactionNumber),
                                        reader.IsDBNull(transactionDate) ? DateTime.MinValue : reader.GetDateTime(transactionDate),
                                        reader.IsDBNull(transactionCustomerName) ? string.Empty : reader.GetString(transactionCustomerName),
                                        reader.IsDBNull(transactionPOSMachine) ? string.Empty : reader.GetString(transactionPOSMachine),
                                        reader.IsDBNull(transactionJsonBuildError) ? string.Empty : reader.GetString(transactionJsonBuildError),
                                        reader.IsDBNull(transactionPostError) ? string.Empty : reader.GetString(transactionPostError),
                                        reader.IsDBNull(transactionCreationDate) ? DateTime.MinValue : reader.GetDateTime(transactionCreationDate),
                                        reader.IsDBNull(transactionCreatedBy) ? string.Empty : Convert.ToString(reader.GetInt32(transactionCreatedBy)),
                                        reader.IsDBNull(transactionLastUpdateDate) ? DateTime.MinValue : reader.GetDateTime(transactionLastUpdateDate),
                                        reader.IsDBNull(transactionLastUpdatedBy) ? string.Empty : reader.GetString(transactionLastUpdatedBy),
                                        reader.IsDBNull(latestRequestId) ? -1 : reader.GetInt32(latestRequestId),
                                        reader.IsDBNull(latestRequestStatus) ? string.Empty : reader.GetString(latestRequestStatus),
                                        reader.IsDBNull(latestRequestPhase) ? string.Empty : reader.GetString(latestRequestPhase),
                                        reader.IsDBNull(invoiceOptionName) ? string.Empty : reader.GetString(invoiceOptionName),
                                        reader.IsDBNull(taxCode) ? string.Empty : reader.GetString(taxCode),
                                        reader.IsDBNull(uniqueId) ? string.Empty : reader.GetString(uniqueId),
                                        reader.IsDBNull(site_id) ? -1 : reader.GetInt32(site_id)
                                        );
                dTOList.Add(transactionDTO);
            }
            log.LogMethodExit(dTOList);
            return dTOList;
        }
        protected FiscalizationPendingTransactionDTO GetFiscalizationPendingTransactionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            FiscalizationPendingTransactionDTO dtoData = new FiscalizationPendingTransactionDTO(
                                            dataRow["fiscalization"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["fiscalization"]),
                                            dataRow["transactionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["transactionId"]),
                                            dataRow["transactionNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["transactionNumber"]),
                                            dataRow["transactionDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["transactionDate"]),
                                            dataRow["transactionCustomerName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["transactionCustomerName"]),
                                            dataRow["transactionPOSMachine"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["transactionPOSMachine"]),
                                            dataRow["transactionJsonBuildError"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["transactionJsonBuildError"]),
                                            dataRow["transactionPostError"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["transactionPostError"]),
                                            dataRow["transactionCreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["transactionCreationDate"]),
                                            dataRow["transactionCreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["transactionCreatedBy"]),
                                            dataRow["transactionLastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["transactionLastUpdateDate"]),
                                            dataRow["transactionLastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["transactionLastUpdatedBy"]),
                                            dataRow["latestRequestId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["latestRequestId"]),
                                            dataRow["latestRequestStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["latestRequestStatus"]),
                                            dataRow["latestRequestPhase"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["latestRequestPhase"]),
                                            dataRow["invoiceOptionName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["invoiceOptionName"]),
                                            dataRow["taxCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["taxCode"]),
                                            dataRow["uniqueId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["uniqueId"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"])
                                            );
            log.LogMethodExit(dtoData);
            return dtoData;
        }
        protected ConcurrentRequestsDTO GetConcurrentRequestsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ConcurrentRequestsDTO dtoData = new ConcurrentRequestsDTO(
                                            dataRow["requestId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["requestId"]),
                                            dataRow["programId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["programId"]),
                                            dataRow["programScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["programScheduleId"]),
                                            dataRow["requestedTime"] == DBNull.Value ? DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : Convert.ToDateTime(dataRow["requestedTime"]).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                            dataRow["RequestedBy"] == DBNull.Value ? null : dataRow["RequestedBy"].ToString(),
                                            dataRow["StartTime"] == DBNull.Value ? DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                                                                     : Convert.ToDateTime(dataRow["StartTime"]).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                            dataRow["ActualStartTime"] == DBNull.Value ? DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                                                                     : Convert.ToDateTime(dataRow["ActualStartTime"]).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                            dataRow["EndTime"] == DBNull.Value ? DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                                                                    : Convert.ToDateTime(dataRow["EndTime"]).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                            dataRow["Phase"] == DBNull.Value ? null : dataRow["Phase"].ToString(),
                                            dataRow["Status"] == DBNull.Value ? null : dataRow["Status"].ToString(),
                                            dataRow["RelaunchOnExit"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["RelaunchOnExit"]),
                                            dataRow["SplitValue"] == DBNull.Value ? null : dataRow["SplitValue"].ToString(), //Argument1 will have trxId
                                            dataRow["Argument2"] == DBNull.Value ? null : dataRow["Argument2"].ToString(),
                                            dataRow["Argument3"] == DBNull.Value ? null : dataRow["Argument3"].ToString(),
                                            dataRow["Argument4"] == DBNull.Value ? null : dataRow["Argument4"].ToString(),
                                            dataRow["Argument5"] == DBNull.Value ? null : dataRow["Argument5"].ToString(),
                                            dataRow["Argument6"] == DBNull.Value ? null : dataRow["Argument6"].ToString(),
                                            dataRow["Argument7"] == DBNull.Value ? null : dataRow["Argument7"].ToString(),
                                            dataRow["Argument8"] == DBNull.Value ? null : dataRow["Argument8"].ToString(),
                                            dataRow["Argument9"] == DBNull.Value ? null : dataRow["Argument9"].ToString(),
                                            dataRow["Argument10"] == DBNull.Value ? null : dataRow["Argument10"].ToString(),
                                            dataRow["ProcessId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProcessId"]),
                                            dataRow["ErrorCount"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ErrorCount"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                            );
            log.LogMethodExit(dtoData);
            return dtoData;
        }
        internal List<FiscalizationPendingTransactionDTO> GetPendingTransactions(List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>> searchParams,
            string passPhrase, int pageNumber = 0, int pageSize = 50)
        {
            log.LogMethodEntry(searchParams, pageNumber, pageSize);
            List<FiscalizationPendingTransactionDTO> list = new List<FiscalizationPendingTransactionDTO>();
            List<FiscalizationPendingTransactionDTO> finalList = new List<FiscalizationPendingTransactionDTO>();
            if (string.IsNullOrWhiteSpace(selectQry) == false)
            {
                string selectQuery = GetPendingTransactionQuery();
                List<SqlParameter> parameters = new List<SqlParameter>();
                selectQuery = selectQuery + SetWhereClause(searchParams, out parameters);
                parameters.Add(new SqlParameter("@PassPhrase", passPhrase));
                selectQuery += " ORDER BY th.TrxId desc OFFSET " + (pageNumber * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
                list = dataAccessHandler.GetDataFromReader(selectQuery, parameters.ToArray(), sqlTransaction, CreateFiscalizationPendingTransactionDTOList);
                if (list != null && list.Any())
                {
                    finalList = new List<FiscalizationPendingTransactionDTO>(list);
                }
            }
            log.LogMethodExit(finalList);
            return finalList;
        }
        internal int GetPendingTransactionCount(List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>> searchParams)
        {
            log.LogMethodEntry(searchParams);
            int trxCount = 0;
            string selectQuery = GetPendingTransactionCountQuery();
            List<SqlParameter> parameters = new List<SqlParameter>();
            selectQuery = selectQuery + SetWhereClause(searchParams, out parameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                trxCount = dataTable.Rows.Count;
            }
            log.LogMethodExit(trxCount);
            return trxCount;
        }
        internal List<ConcurrentRequestsDTO> GetLatestSubmittedRequestsByTrxId(List<string> trxIDList)
        {
            log.LogMethodEntry(trxIDList);
            List<ConcurrentRequestsDTO> list = new List<ConcurrentRequestsDTO>();
            string query = GetLatestSubmittedRequestqry();
            DataTable table = dataAccessHandler.BatchSelect(query, "@trxIdList", trxIDList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetConcurrentRequestsDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        protected virtual string GetPendingTransactionQuery()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            throw new NotImplementedException();
        }
        private string SetWhereClause(List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>> searchParams, out List<SqlParameter> parameters)
        {
            log.LogMethodEntry(searchParams);
            parameters = new List<SqlParameter>();
            int count = 0;
            string whereClauseString = string.Empty;
            if (searchParams == null || searchParams.Count == 0)
            {
                log.LogMethodExit(string.Empty, "search parameters is empty");
                return whereClauseString;
            }
            string joiner = string.Empty;
            StringBuilder query = new StringBuilder(" where ");
            foreach (KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string> searchParameter in searchParams)
            {
                if (DBSearchParameters.ContainsKey(searchParameter.Key))
                {
                    joiner = (count == 0) ? " " : " and ";
                    {
                        if (searchParameter.Key.Equals(FiscalizationPendingTransactionDTO.SearchParameters.TRX_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(FiscalizationPendingTransactionDTO.SearchParameters.TRX_FROM_DATE))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(FiscalizationPendingTransactionDTO.SearchParameters.TRX_TO_DATE))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(FiscalizationPendingTransactionDTO.SearchParameters.IGNORE_WIP_TRX))
                        {
                            query.Append(joiner + GetIgnoreWIPTrxCondition());
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    count++;
                }
                else
                {
                    log.Error("Ends-SetWhereClause(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                    log.LogMethodExit("Throwing exception- The query parameter does not exist ");
                    throw new Exception("The query parameter does not exist");
                }
            }
            whereClauseString = query.ToString();
            log.LogMethodExit(whereClauseString);
            return whereClauseString;
        }
        protected virtual string GetIgnoreWIPTrxCondition()
        {
            log.LogMethodEntry();
            string cmdQqry = @" NOt EXISTS (
                                            SELECT 1
                                            FROM ( 
                                                SELECT
                                                    CAST('<value>' + REPLACE(ParameterValue, ',', '</value><value>') + '</value>' AS XML) AS xmlvalues 
                                                    --,RANK() OVER (ORDER BY ISNULL(cr.actualStarttime, ISNULL(cr.startTime, cr.RequestedTime)) DESC, cr.requestId DESC) AS crrank
                                                FROM
                                                    ConcurrentPrograms cp, 
                                                    ConcurrentProgramParameters cpp, 
                                                    ConcurrentProgramSchedules cps,  
                                                    ConcurrentRequests cr,  
                                                    ProgramParameterValue pp
                                                WHERE
                                                    cp.ExecutableName = 'InvoiceJsonReprocessingProgram.Exe'
                                                    AND cpp.ParameterName = 'TransactionIdList'
				                                    and cp.ProgramId = cpp.ProgramId
				                                    and cp.ProgramId = cpp.ProgramId
				                                    and cps.ProgramId = cp.ProgramId
				                                    and cr.ProgramScheduleId = cps.ProgramScheduleId
				                                    and pp.ProgramId = cpp.ProgramId
                                                    AND pp.ParameterId = cpp.ConcurrentProgramParameterId
                                                    AND pp.ConcurrentProgramScheduleId = cps.ProgramScheduleId
				                                    and cr.Phase in ( 'Pending', 'Running')   
                                            ) AS aaa
                                            WHERE aaa.xmlvalues.value('(//value)[1]', 'VARCHAR(255)') = CAST(th.TrxId AS VARCHAR(255))
                                        ) ";
            log.LogMethodExit(cmdQqry);
            return cmdQqry;
        }
        protected virtual string GetPendingTransactionCountQuery()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            throw new NotImplementedException();
        }
        protected virtual string GetLatestSubmittedRequestqry()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            throw new NotImplementedException();
        }
    }
}
