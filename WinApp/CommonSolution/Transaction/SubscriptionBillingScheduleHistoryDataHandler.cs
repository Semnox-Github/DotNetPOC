/********************************************************************************************
 * Project Name - SubscriptionBillingScheduleHistoryDataHandler
 * Description  - Data handler of the SubscriptionBillingScheduleHistory 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     14-Dec-2020    Fiona              Created for Subscription changes                                                                               
 *2.120.0     18-Mar-2021    Guru S A           For Subscription phase 2 changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// SubscriptionBillingScheduleHistoryDataHandler
    /// </summary>
    public class SubscriptionBillingScheduleHistoryDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM SubscriptionBillingScheduleHistory AS shh ";
        private static readonly Dictionary<SubscriptionBillingScheduleHistoryDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<SubscriptionBillingScheduleHistoryDTO.SearchByParameters, string>
            {
             {SubscriptionBillingScheduleHistoryDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_HISTORY_ID, "shh.SubscriptionBillingScheduleHistoryId"},
             {SubscriptionBillingScheduleHistoryDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID, "shh.SubscriptionBillingScheduleId"},
             {SubscriptionBillingScheduleHistoryDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, "shh.SubscriptionHeaderId"},
             {SubscriptionBillingScheduleHistoryDTO.SearchByParameters.IS_ACTIVE, "shh.IsActive"},
             {SubscriptionBillingScheduleHistoryDTO.SearchByParameters.MASTER_ENTITY_ID, "shh.MasterEntityId"},
             {SubscriptionBillingScheduleHistoryDTO.SearchByParameters.SITE_ID, "shh.site_id"}
            };
        /// <summary>
        /// Parameterized Constructor for SubscriptionBillingScheduleHistoryDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public SubscriptionBillingScheduleHistoryDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        private List<SqlParameter> GetSQLParameters(SubscriptionBillingScheduleHistoryDTO subscriptionBillingScheduleHistoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(subscriptionBillingScheduleHistoryDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionBillingScheduleHistoryId", subscriptionBillingScheduleHistoryDTO.SubscriptionBillingScheduleHistoryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionBillingScheduleId", subscriptionBillingScheduleHistoryDTO.SubscriptionBillingScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionHeaderId", subscriptionBillingScheduleHistoryDTO.SubscriptionHeaderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionId", subscriptionBillingScheduleHistoryDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionLineId", subscriptionBillingScheduleHistoryDTO.TransactionLineId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BillFromDate", subscriptionBillingScheduleHistoryDTO.BillFromDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BillToDate", subscriptionBillingScheduleHistoryDTO.BillToDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BillOnDate", subscriptionBillingScheduleHistoryDTO.BillOnDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BillAmount", subscriptionBillingScheduleHistoryDTO.BillAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OverridedBillAmount", subscriptionBillingScheduleHistoryDTO.OverridedBillAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OverrideReason", subscriptionBillingScheduleHistoryDTO.OverrideReason));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OverrideBy", subscriptionBillingScheduleHistoryDTO.OverrideBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OverrideApprovedBy", subscriptionBillingScheduleHistoryDTO.OverrideApprovedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentProcessingFailureCount", subscriptionBillingScheduleHistoryDTO.PaymentProcessingFailureCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Status", subscriptionBillingScheduleHistoryDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CancelledBy", subscriptionBillingScheduleHistoryDTO.CancelledBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CancellationApprovedBy", subscriptionBillingScheduleHistoryDTO.CancellationApprovedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LineType", subscriptionBillingScheduleHistoryDTO.LineType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", subscriptionBillingScheduleHistoryDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", subscriptionBillingScheduleHistoryDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", subscriptionBillingScheduleHistoryDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        internal SubscriptionBillingScheduleHistoryDTO GetSubscriptionBillingScheduleHistoryDTO(int subscriptionBillingScheduleId)
        {
            log.LogMethodEntry(subscriptionBillingScheduleId);
            SubscriptionBillingScheduleHistoryDTO result = null;
            string query = SELECT_QUERY + @" WHERE pfe.SubscriptionJobProcessId = @SubscriptionJobProcessId";
            SqlParameter parameter = new SqlParameter("@SubscriptionJobProcessId", subscriptionBillingScheduleId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetSubscriptionBillingScheduleHistoryDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        private SubscriptionBillingScheduleHistoryDTO GetSubscriptionBillingScheduleHistoryDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            SubscriptionBillingScheduleHistoryDTO subscriptionBillingScheduleHistoryDTO = new SubscriptionBillingScheduleHistoryDTO(
                dataRow["SubscriptionBillingScheduleHistoryId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SubscriptionBillingScheduleHistoryId"]),
                dataRow["SubscriptionBillingScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SubscriptionBillingScheduleId"]),
                dataRow["SubscriptionHeaderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SubscriptionHeaderId"]),
                dataRow["TransactionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionId"]),
                dataRow["TransactionLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionLineId"]),
                dataRow["BillFromDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["BillFromDate"]),
                dataRow["BillToDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["BillToDate"]),
                dataRow["BillOnDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["BillOnDate"]),
                dataRow["BillAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["BillAmount"]),
                dataRow["OverridedBillAmount"] == DBNull.Value ? (decimal?) null : Convert.ToDecimal(dataRow["OverridedBillAmount"]),
                dataRow["OverrideReason"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["OverrideReason"]),
                dataRow["OverrideBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["OverrideBy"]),
                dataRow["OverrideApprovedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["OverrideApprovedBy"]),
                dataRow["PaymentProcessingFailureCount"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PaymentProcessingFailureCount"]),
                dataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Status"]),
                dataRow["CancelledBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CancelledBy"]),
                dataRow["cancellationApprovedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["cancellationApprovedBy"]),
                dataRow["LineType"] == DBNull.Value ? SubscriptionLineType.BILLING_LINE : Convert.ToString(dataRow["LineType"]),
                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"])
                );
            log.LogMethodExit(subscriptionBillingScheduleHistoryDTO);
            return subscriptionBillingScheduleHistoryDTO;
        }

        internal List<SubscriptionBillingScheduleHistoryDTO> GetSubscriptionBillingScheduleHistoryDTOList(List<int> idList, bool loadChild)
        {
            log.LogMethodEntry(idList, loadChild);
            List<SubscriptionBillingScheduleHistoryDTO> list = new List<SubscriptionBillingScheduleHistoryDTO>();
            string query = @"SELECT pfe.* 
                              FROM SubscriptionBillingScheduleHistory AS sbsh
                                  inner join @subscriptionJobProcessIdList List on sbsh.SubscriptionHeaderId = List.Id
                              where isnull(sbsh.isactive,0) = @LoadActiveChildren";

            DataTable table = dataAccessHandler.BatchSelect(query, "@subscriptionJobProcessIdList",
                                                                  idList, new SqlParameter[] { new SqlParameter("@LoadActiveChildren", loadChild) },
                                                                  sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetSubscriptionBillingScheduleHistoryDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        internal List<SubscriptionBillingScheduleHistoryDTO> GetSubscriptionBillingScheduleHistoryDTOList(List<KeyValuePair<SubscriptionBillingScheduleHistoryDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<SubscriptionBillingScheduleHistoryDTO> subscriptionBillingScheduleHistoryDTOList = new List<SubscriptionBillingScheduleHistoryDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Any()))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<SubscriptionBillingScheduleHistoryDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == SubscriptionBillingScheduleHistoryDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_HISTORY_ID ||
                            searchParameter.Key == SubscriptionBillingScheduleHistoryDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == SubscriptionBillingScheduleHistoryDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID ||
                            searchParameter.Key == SubscriptionBillingScheduleHistoryDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }


                        else if (searchParameter.Key == SubscriptionBillingScheduleHistoryDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == SubscriptionBillingScheduleHistoryDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
            if (dataTable != null && dataTable.Rows.Cast<DataRow>().Any())
            {
                subscriptionBillingScheduleHistoryDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetSubscriptionBillingScheduleHistoryDTO(x)).ToList();
            }
            log.LogMethodExit(subscriptionBillingScheduleHistoryDTOList);
            return subscriptionBillingScheduleHistoryDTOList;
        }
    }
}

