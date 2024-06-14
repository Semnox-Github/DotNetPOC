/********************************************************************************************
 * Project Name - SubscriptionHeaderHistoryDataHandler
 * Description  - Data handler of the SubscriptionHeaderHistory 
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
    /// SubscriptionHeaderHistoryDataHandler class
    /// </summary>
    public class SubscriptionHeaderHistoryDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM SubscriptionHeaderHistory AS shh ";
        private static readonly Dictionary<SubscriptionHeaderHistoryDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<SubscriptionHeaderHistoryDTO.SearchByParameters, string>
            {
                {SubscriptionHeaderHistoryDTO.SearchByParameters.SUBSCRIPTION_HEADER_HISTORY_ID, "shh.SubscriptionHeaderHistoryId"},
                {SubscriptionHeaderHistoryDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, "shh.SubscriptionHeaderId"},
                {SubscriptionHeaderHistoryDTO.SearchByParameters.CUSTOMER_ID, "shh.CustomerId"},
                {SubscriptionHeaderHistoryDTO.SearchByParameters.IS_ACTIVE, "shh.IsActive"},
                {SubscriptionHeaderHistoryDTO.SearchByParameters.MASTER_ENTITY_ID, "shh.MasterEntityId"},
                {SubscriptionHeaderHistoryDTO.SearchByParameters.SITE_ID, "shh.site_id"}

            };
        /// <summary>
        /// Parameterized Constructor for SubscriptionHeaderHistoryDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public SubscriptionHeaderHistoryDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        private List<SqlParameter> GetSQLParameters(SubscriptionHeaderHistoryDTO subscriptionHeaderHistoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(subscriptionHeaderHistoryDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionHeaderHistoryId", subscriptionHeaderHistoryDTO.SubscriptionHeaderHistoryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionHeaderId", subscriptionHeaderHistoryDTO.SubscriptionHeaderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionId", subscriptionHeaderHistoryDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionLineId", subscriptionHeaderHistoryDTO.TransactionLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", subscriptionHeaderHistoryDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerContactId", subscriptionHeaderHistoryDTO.CustomerContactId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerCreditCardsID", subscriptionHeaderHistoryDTO.CustomerCreditCardsID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductSubscriptionId", subscriptionHeaderHistoryDTO.ProductSubscriptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductSubscriptionName", subscriptionHeaderHistoryDTO.ProductSubscriptionName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductSubscriptionDescription", subscriptionHeaderHistoryDTO.ProductSubscriptionDescription));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionPrice", subscriptionHeaderHistoryDTO.SubscriptionPrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionCycle", subscriptionHeaderHistoryDTO.SubscriptionCycle));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UnitOfSubscriptionCycle", subscriptionHeaderHistoryDTO.UnitOfSubscriptionCycle));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionCycleValidity", subscriptionHeaderHistoryDTO.SubscriptionCycleValidity));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@SeasonalSubscription", subscriptionHeaderHistoryDTO.SeasonalSubscription));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SeasonStartDate", subscriptionHeaderHistoryDTO.SeasonStartDate));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@SeasonEndDate", subscriptionHeaderHistoryDTO.SeasonEndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FreeTrialPeriodCycle", subscriptionHeaderHistoryDTO.FreeTrialPeriodCycle));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AllowPause", subscriptionHeaderHistoryDTO.AllowPause));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BillInAdvance", subscriptionHeaderHistoryDTO.BillInAdvance));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionPaymentCollectionMode", subscriptionHeaderHistoryDTO.SubscriptionPaymentCollectionMode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SelectedPaymentCollectionMode", subscriptionHeaderHistoryDTO.SelectedPaymentCollectionMode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AutoRenew", subscriptionHeaderHistoryDTO.AutoRenew));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AutoRenewalMarkupPercent", subscriptionHeaderHistoryDTO.AutoRenewalMarkupPercent));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RenewalGracePeriodCycle", subscriptionHeaderHistoryDTO.RenewalGracePeriodCycle));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NoOfRenewalReminders", subscriptionHeaderHistoryDTO.NoOfRenewalReminders));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReminderFrequencyInDays", subscriptionHeaderHistoryDTO.ReminderFrequencyInDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SendFirstReminderBeforeXDays", subscriptionHeaderHistoryDTO.SendFirstReminderBeforeXDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastRenewalReminderSentOn", subscriptionHeaderHistoryDTO.LastRenewalReminderSentOn));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RenewalReminderCount", subscriptionHeaderHistoryDTO.RenewalReminderCount)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastPaymentRetryLimitReminderSentOn", subscriptionHeaderHistoryDTO.LastPaymentRetryLimitReminderSentOn));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentRetryLimitReminderCount", subscriptionHeaderHistoryDTO.PaymentRetryLimitReminderCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SourceSubscriptionHeaderId", subscriptionHeaderHistoryDTO.SourceSubscriptionHeaderId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionStartDate", subscriptionHeaderHistoryDTO.SubscriptionStartDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionEndDate", subscriptionHeaderHistoryDTO.SubscriptionEndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PausedBy", subscriptionHeaderHistoryDTO.PausedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UnPausedBy", subscriptionHeaderHistoryDTO.UnPausedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PauseApprovedBy", subscriptionHeaderHistoryDTO.PauseApprovedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UnPauseApprovedBy", subscriptionHeaderHistoryDTO.UnPauseApprovedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CancellationOption", subscriptionHeaderHistoryDTO.CancellationOption));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CancelledBy", subscriptionHeaderHistoryDTO.CancelledBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CancellationApprovedBy", subscriptionHeaderHistoryDTO.CancellationApprovedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Status", subscriptionHeaderHistoryDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaxInclusivePrice", subscriptionHeaderHistoryDTO.TaxInclusivePrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductsId", subscriptionHeaderHistoryDTO.ProductsId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", subscriptionHeaderHistoryDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", subscriptionHeaderHistoryDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", subscriptionHeaderHistoryDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId)); 

            log.LogMethodExit(parameters);
            return parameters;
        }
        private SubscriptionHeaderHistoryDTO GetSubscriptionHeaderHistoryDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            SubscriptionHeaderHistoryDTO subscriptionHeaderHistoryDTO = new SubscriptionHeaderHistoryDTO(
                dataRow["SubscriptionHeaderHistoryId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SubscriptionHeaderHistoryId"]),
                dataRow["SubscriptionHeaderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SubscriptionHeaderId"]),
                dataRow["TransactionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionId"]),
                dataRow["TransactionLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionLineId"]),
                dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                dataRow["CustomerContactId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerContactId"]),
                dataRow["CustomerCreditCardsID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerCreditCardsID"]),
                dataRow["ProductSubscriptionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductSubscriptionId"]),                
                dataRow["ProductSubscriptionName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProductSubscriptionName"]),
                dataRow["ProductSubscriptionDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProductSubscriptionDescription"]),
                dataRow["SubscriptionPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["SubscriptionPrice"]),
                dataRow["SubscriptionCycle"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SubscriptionCycle"]),
                dataRow["UnitOfSubscriptionCycle"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UnitOfSubscriptionCycle"]),
                dataRow["SubscriptionCycleValidity"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SubscriptionCycleValidity"]),
               // dataRow["SeasonalSubscription"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SeasonalSubscription"]),
                dataRow["SeasonStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["SeasonStartDate"]),
                //dataRow["SeasonEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["SeasonEndDate"]),
                dataRow["FreeTrialPeriodCycle"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["FreeTrialPeriodCycle"]),
                dataRow["AllowPause"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["AllowPause"]),
                dataRow["BillInAdvance"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["BillInAdvance"]),
                dataRow["SubscriptionPaymentCollectionMode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SubscriptionPaymentCollectionMode"]),
                dataRow["SelectedPaymentCollectionMode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SelectedPaymentCollectionMode"]),
                dataRow["AutoRenew"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["AutoRenew"]),
                dataRow["AutoRenewalMarkupPercent"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["AutoRenewalMarkupPercent"]),
                dataRow["RenewalGracePeriodCycle"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["RenewalGracePeriodCycle"]),
                dataRow["NoOfRenewalReminders"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["NoOfRenewalReminders"]),
                dataRow["ReminderFrequencyInDays"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ReminderFrequencyInDays"]),
                dataRow["SendFirstReminderBeforeXDays"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SendFirstReminderBeforeXDays"]),
                dataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Status"]),
                dataRow["TaxInclusivePrice"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["TaxInclusivePrice"]),
                dataRow["ProductsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductsId"]),
                dataRow["LastRenewalReminderSentOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastRenewalReminderSentOn"]),
                dataRow["RenewalReminderCount"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["RenewalReminderCount"]), 
                dataRow["LastPaymentRetryLimitReminderSentOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastPaymentRetryLimitReminderSentOn"]),
                dataRow["PaymentRetryLimitReminderCount"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["PaymentRetryLimitReminderCount"]),
                dataRow["sourceSubscriptionHeaderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["sourceSubscriptionHeaderId"]),
                dataRow["subscriptionStartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["subscriptionStartDate"]),
                dataRow["subscriptionEndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["subscriptionEndDate"]),
                dataRow["pausedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["pausedBy"]),
                dataRow["pauseApprovedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["pauseApprovedBy"]),
                dataRow["unPausedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["unPausedBy"]),
                dataRow["unPauseApprovedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["unPauseApprovedBy"]),
                dataRow["cancellationOption"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["cancellationOption"]),
                dataRow["cancelledBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["cancelledBy"]),
                dataRow["cancellationApprovedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["cancellationApprovedBy"]), 
                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                dataRow["SubscriptionNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SubscriptionNumber"])
                );
            log.LogMethodExit(subscriptionHeaderHistoryDTO);
            return subscriptionHeaderHistoryDTO;
        }
        internal SubscriptionHeaderHistoryDTO GetSubscriptionHeaderHistoryDTO(int subscriptionHeaderHistoryId)
        {
            log.LogMethodEntry(subscriptionHeaderHistoryId);
            SubscriptionHeaderHistoryDTO result = null;
            string query = SELECT_QUERY + @" WHERE pfe.SubscriptionHeaderHistoryId = @SubscriptionHeaderHistoryId";
            SqlParameter parameter = new SqlParameter("@SubscriptionHeaderHistoryId", subscriptionHeaderHistoryId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetSubscriptionHeaderHistoryDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        internal List<SubscriptionHeaderHistoryDTO> GetSubscriptionHeaderHistoryDTOList(List<KeyValuePair<SubscriptionHeaderHistoryDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<SubscriptionHeaderHistoryDTO> subscriptionHeaderHistoryDTOList = new List<SubscriptionHeaderHistoryDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Any()))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<SubscriptionHeaderHistoryDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == SubscriptionHeaderHistoryDTO.SearchByParameters.SUBSCRIPTION_HEADER_HISTORY_ID ||
                            searchParameter.Key == SubscriptionHeaderHistoryDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == SubscriptionHeaderHistoryDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID ||
                            searchParameter.Key == SubscriptionHeaderHistoryDTO.SearchByParameters.CUSTOMER_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }


                        else if (searchParameter.Key == SubscriptionHeaderHistoryDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderHistoryDTO.SearchByParameters.SITE_ID)
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
                subscriptionHeaderHistoryDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetSubscriptionHeaderHistoryDTO(x)).ToList();
            }
            log.LogMethodExit(subscriptionHeaderHistoryDTOList);
            return subscriptionHeaderHistoryDTOList;
        }
    }
}
