/********************************************************************************************
 * Project Name - SubscriptionBillingSchedule Data Handler
 * Description  - Data handler of the SubscriptionBillingSchedule 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     09-Dec-2020    Guru S A           Created for Subscription changes                                                                               
 *2.120.0     18-Mar-2021    Guru S A           For Subscription phase 2 changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// SubscriptionBillingScheduleDataHandler
    /// </summary>
    public class SubscriptionBillingScheduleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<SubscriptionBillingScheduleDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<SubscriptionBillingScheduleDTO.SearchByParameters, string>
            {
                {SubscriptionBillingScheduleDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID, "sbs.subscriptionBillingScheduleId"},
                {SubscriptionBillingScheduleDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, "sbs.SubscriptionHeaderId"},
                {SubscriptionBillingScheduleDTO.SearchByParameters.TRANSACTION_ID, "sbs.TransactionId"},
                {SubscriptionBillingScheduleDTO.SearchByParameters.TRANSACTION_LINE_ID, "sbs.TransactionLineId"}, 
                {SubscriptionBillingScheduleDTO.SearchByParameters.STATUS, "sbs.Status"},
                {SubscriptionBillingScheduleDTO.SearchByParameters.IS_ACTIVE, "sbs.IsActive"},
                {SubscriptionBillingScheduleDTO.SearchByParameters.MASTER_ENTITY_ID,"sbs.MasterEntityId"},
                {SubscriptionBillingScheduleDTO.SearchByParameters.SITE_ID, "sbs.site_id"} ,
                {SubscriptionBillingScheduleDTO.SearchByParameters.CUSTOMER_CREDIT_CARDS_ID, "sh.CustomerCreditCardsId"},
                {SubscriptionBillingScheduleDTO.SearchByParameters.UNBILLED_CYCLE, "sbs.subscriptionBillingScheduleId"},
                {SubscriptionBillingScheduleDTO.SearchByParameters.BILL_FROM_GREATER_THAN_OR_EQUAL_TO, "sbs.BillFromDate"},
                {SubscriptionBillingScheduleDTO.SearchByParameters.BILL_TO_LESS_THAN, "sbs.BillToDate"},
                {SubscriptionBillingScheduleDTO.SearchByParameters.BILL_ON_GREATER_THAN_OR_EQUAL_TO, "sbs.BillOnDate"},
                {SubscriptionBillingScheduleDTO.SearchByParameters.BILL_ON_LESS_THAN, "sbs.BillOnDate"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * from SubscriptionBillingSchedule AS sbs ";
        /// <summary>
        /// Default constructor of SubscriptionBillingScheduleDataHandler class
        /// </summary>
        public SubscriptionBillingScheduleDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        } 

        private List<SqlParameter> BuildSQLParameters(SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO, string userId, int siteId)
        {
            log.LogMethodEntry(subscriptionBillingScheduleDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionBillingScheduleId", subscriptionBillingScheduleDTO.SubscriptionBillingScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionHeaderId", subscriptionBillingScheduleDTO.SubscriptionHeaderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionId", subscriptionBillingScheduleDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionLineId", subscriptionBillingScheduleDTO.TransactionLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BillFromDate", subscriptionBillingScheduleDTO.BillFromDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BillToDate", subscriptionBillingScheduleDTO.BillToDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BillOnDate", subscriptionBillingScheduleDTO.BillOnDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BillAmount", subscriptionBillingScheduleDTO.BillAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OverridedBillAmount", subscriptionBillingScheduleDTO.OverridedBillAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OverrideReason", subscriptionBillingScheduleDTO.OverrideReason));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OverrideBy", subscriptionBillingScheduleDTO.OverrideBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OverrideApprovedBy", subscriptionBillingScheduleDTO.OverrideApprovedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CancelledBy", subscriptionBillingScheduleDTO.CancelledBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CancellationApprovedBy", subscriptionBillingScheduleDTO.CancellationApprovedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LineType", subscriptionBillingScheduleDTO.LineType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentProcessingFailureCount", subscriptionBillingScheduleDTO.PaymentProcessingFailureCount)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@Status", subscriptionBillingScheduleDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", subscriptionBillingScheduleDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", subscriptionBillingScheduleDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the SubscriptionBillingSchedule record to the database
        /// </summary>
        /// <param name="subscriptionBillingScheduleDTO">SubscriptionBillingScheduleDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted SubscriptionBillingSchedule record</returns>
        public SubscriptionBillingScheduleDTO InsertSubscriptionBillingSchedule(SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO, string userId, int siteId)
        {
            log.LogMethodEntry(subscriptionBillingScheduleDTO, userId, siteId);
            string query = @"INSERT INTO SubscriptionBillingSchedule 
                                        (  
						                    SubscriptionHeaderId,
						                    TransactionId,
						                    TransactionLineId,
						                    BillFromDate,
						                    BillToDate,
						                    BillOnDate,
						                    BillAmount,
						                    OverridedBillAmount,
						                    OverrideReason,
						                    OverrideBy,
						                    OverrideApprovedBy,
						                    PaymentProcessingFailureCount, 
						                    Status,
                                            CancelledBy, 
                                            CancellationApprovedBy,
                                            LineType,
						                    IsActive,
						                    CreatedBy,
						                    CreationDate,
						                    LastUpdatedBy,
						                    LastUpdatedDate,
						                    Guid, 
						                    site_id,     
						                    MasterEntityId 
                                        ) 
                                VALUES 
                                        (
	                                       @SubscriptionHeaderId,
                                           @TransactionId,
	                                       @TransactionLineId,
	                                       @BillFromDate,
	                                       @BillToDate,
	                                       @BillOnDate,
	                                       @BillAmount,
	                                       @OverridedBillAmount,
	                                       @OverrideReason,
						                   @OverrideBy,
						                   @OverrideApprovedBy,
						                   @PaymentProcessingFailureCount, 
						                   @Status, 
                                           @CancelledBy, 
                                           @CancellationApprovedBy,
                                           @LineType,
	                                       @IsActive,
	                                       @CreatedBy,
	                                       GETDATE(),
	                                       @LastUpdatedBy,
	                                       GETDATE(),
	                                       NEWID(),
	                                       @site_id,
	                                       @MasterEntityId
                                        )
                                        SELECT * FROM SubscriptionBillingSchedule WHERE SubscriptionBillingScheduleId = scope_identity()";


            List<SqlParameter> parameters = BuildSQLParameters(subscriptionBillingScheduleDTO, userId, siteId);
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                RefreshSubscriptionBillingScheduleDTO(subscriptionBillingScheduleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Inserting the Subscription Billing Schedule  ", ex);
                log.LogVariableState("SubscriptionBillingScheduleDTO", subscriptionBillingScheduleDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(subscriptionBillingScheduleDTO);
            return subscriptionBillingScheduleDTO;
        }

        /// <summary>
        /// Updates the SubscriptionBillingSchedule record
        /// </summary>
        /// <param name="subscriptionBillingScheduleDTO">SubscriptionBillingScheduleDTO type parameter</param>
        /// <param name="userId">User updating the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public SubscriptionBillingScheduleDTO UpdateSubscriptionBillingSchedule(SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO, string userId, int siteId)
        {
            log.LogMethodEntry(subscriptionBillingScheduleDTO, userId, siteId);
            string query = @"
                         INSERT INTO dbo.SubscriptionBillingScheduleHistory
                                    (SubscriptionBillingScheduleId, SubscriptionHeaderId, TransactionId, TransactionLineId, BillFromDate, BillToDate, BillOnDate, BillAmount,
			                        OverridedBillAmount, OverrideReason, OverrideBy, OverrideApprovedBy, PaymentProcessingFailureCount, Status,
                                    CancelledBy, CancellationApprovedBy, LineType, IsActive, site_id, 
                                    MasterEntityId, CreatedBy, CreationDate, LastUpdatedBy, LastUpdatedDate, Guid)
 	                         SELECT SubscriptionBillingScheduleId, SubscriptionHeaderId, TransactionId, TransactionLineId, BillFromDate, BillToDate, BillOnDate, BillAmount,
                                    OverridedBillAmount, OverrideReason, OverrideBy, OverrideApprovedBy, PaymentProcessingFailureCount, Status, 
                                    CancelledBy, CancellationApprovedBy,LineType, IsActive, site_id, 
                                    MasterEntityId, @CreatedBy, GETDATE(), @LastUpdatedBy, GETDATE(), NEWID()
	                           FROM dbo.SubscriptionBillingSchedule 
	                          WHERE SubscriptionBillingScheduleId = @SubscriptionBillingScheduleId;
                             UPDATE SubscriptionBillingSchedule 
                                SET SubscriptionHeaderId = @SubscriptionHeaderId,
                                    TransactionId = @TransactionId,
	                                TransactionLineId = @TransactionLineId, 
	                                BillFromDate = @BillFromDate,
	                                BillToDate = @BillToDate,
	                                BillOnDate = @BillOnDate,
	                                BillAmount = @BillAmount,
	                                OverridedBillAmount = @OverridedBillAmount,
	                                OverrideReason = @OverrideReason,
						            OverrideBy = @OverrideBy,
						            OverrideApprovedBy = @OverrideApprovedBy,
						            PaymentProcessingFailureCount = @PaymentProcessingFailureCount,  
	                                Status = @Status, 
                                    CancelledBy= @CancelledBy, 
                                    CancellationApprovedBy = @CancellationApprovedBy,
                                    LineType = @LineType,
	                                IsActive = @IsActive, 
	                                LastUpdatedBy = @LastUpdatedBy,
	                                LastUpdatedDate = GETDATE(), 
	                                site_id = @site_id,
	                                MasterEntityId = @MasterEntityId
                              WHERE SubscriptionBillingScheduleId = @SubscriptionBillingScheduleId
                             SELECT * FROM SubscriptionBillingSchedule WHERE SubscriptionBillingScheduleId = @SubscriptionBillingScheduleId";
            List<SqlParameter> parameters = BuildSQLParameters(subscriptionBillingScheduleDTO, userId, siteId);
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                RefreshSubscriptionBillingScheduleDTO(subscriptionBillingScheduleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating the Subscription Billing Schedule ", ex);
                log.LogVariableState("SubscriptionBillingScheduleDTO", subscriptionBillingScheduleDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(subscriptionBillingScheduleDTO);
            return subscriptionBillingScheduleDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="subscriptionBillingScheduleDTO">SubscriptionBillingScheduleDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshSubscriptionBillingScheduleDTO(SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO, DataTable dt)
        {
            log.LogMethodEntry(subscriptionBillingScheduleDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                subscriptionBillingScheduleDTO.SubscriptionBillingScheduleId = Convert.ToInt32(dt.Rows[0]["SubscriptionBillingScheduleId"]);
                subscriptionBillingScheduleDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                subscriptionBillingScheduleDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                subscriptionBillingScheduleDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                subscriptionBillingScheduleDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                subscriptionBillingScheduleDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                subscriptionBillingScheduleDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to SubscriptionBillingScheduleDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns SubscriptionBillingScheduleDTO</returns>
        private SubscriptionBillingScheduleDTO GetSubscriptionBillingScheduleDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO = new SubscriptionBillingScheduleDTO(Convert.ToInt32(dataRow["SubscriptionBillingScheduleId"]),
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
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(subscriptionBillingScheduleDTO);
            return subscriptionBillingScheduleDTO;
        }

        /// <summary>
        /// Gets the SubscriptionBillingSchedule data of passed SubscriptionBillingSchedule Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns SubscriptionBillingScheduleDTO</returns>
        public SubscriptionBillingScheduleDTO GetSubscriptionBillingScheduleDTO(int id)
        {
            log.LogMethodEntry(id);
            SubscriptionBillingScheduleDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE sbs.SubscriptionBillingScheduleId = @SubscriptionBillingScheduleId";
            SqlParameter parameter = new SqlParameter("@SubscriptionBillingScheduleId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetSubscriptionBillingScheduleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the SubscriptionBillingScheduleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SubscriptionBillingScheduleDTO matching the search criteria</returns>
        public List<SubscriptionBillingScheduleDTO> GetSubscriptionBillingScheduleDTOList(List<KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SubscriptionBillingScheduleDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID || 
                            searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID ||
                            searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.TRANSACTION_ID ||
                            searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.TRANSACTION_LINE_ID ||
                            searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.MASTER_ENTITY_ID )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        } 
                        else if (searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.STATUS)
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ", '')" + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.CUSTOMER_CREDIT_CARDS_ID)
                        {
                            query.Append(joiner + @" EXISTS ( SELECT 1 
                                                                FROM SubscriptionHeader sh
                                                               WHERE sh.SubscriptionHeaderId = sbs.SubscriptionHeaderId 
                                                                 AND "+ DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                                 + " ) " );
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.UNBILLED_CYCLE)
                        {
                            query.Append(joiner + @" CASE WHEN sbs.TransactionId IS NULL THEN 1 ELSE 0 END = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.BILL_FROM_GREATER_THAN_OR_EQUAL_TO ||
                                 searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.BILL_ON_GREATER_THAN_OR_EQUAL_TO)
                        {
                            query.Append(joiner + " " + DBSearchParameters[searchParameter.Key] + " >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.BILL_TO_LESS_THAN ||
                                 searchParameter.Key == SubscriptionBillingScheduleDTO.SearchByParameters.BILL_ON_LESS_THAN)
                        {
                            query.Append(joiner + " " + DBSearchParameters[searchParameter.Key] + " < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
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
                list = new List<SubscriptionBillingScheduleDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO = GetSubscriptionBillingScheduleDTO(dataRow);
                    list.Add(subscriptionBillingScheduleDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the SubscriptionBillingScheduleDTO List for header Id List
        /// </summary>
        /// <param name="subscriptionHeaderIdList">integer list parameter</param>
        /// <returns>Returns List of SubscriptionBillingScheduleDTO</returns>
        public List<SubscriptionBillingScheduleDTO> GetSubscriptionBillingScheduleDTOList(List<int> subscriptionHeaderIdList)
        {
            log.LogMethodEntry(subscriptionHeaderIdList);
            List<SubscriptionBillingScheduleDTO> list = new List<SubscriptionBillingScheduleDTO>();
            string query = @"SELECT sbs.* 
                              FROM SubscriptionBillingSchedule AS sbs
                                  inner join @SubscriptionHeaderIdList List on sbs.SubscriptionHeaderId = List.Id ";

            DataTable table = dataAccessHandler.BatchSelect(query, "@SubscriptionHeaderIdList", subscriptionHeaderIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetSubscriptionBillingScheduleDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// Gets the SubscriptionBillingScheduleDTO List for SubscriptionBillingScheduleId Id List
        /// </summary>
        /// <param name="subscriptionBillingScheduleIdList">integer list parameter</param>
        /// <returns>Returns List of SubscriptionBillingScheduleDTO</returns>
        public List<SubscriptionBillingScheduleDTO> GetSubscriptionBillingScheduleDTOListById(List<int> subscriptionBillingScheduleIdList)
        {
            log.LogMethodEntry(subscriptionBillingScheduleIdList);
            List<SubscriptionBillingScheduleDTO> list = new List<SubscriptionBillingScheduleDTO>();
            string query = @"SELECT sbs.* 
                              FROM SubscriptionBillingSchedule AS sbs
                                  inner join @SubscriptionBillingScheduleIdList List on sbs.SubscriptionBillingScheduleId = List.Id ";

            DataTable table = dataAccessHandler.BatchSelect(query, "@SubscriptionBillingScheduleIdList", subscriptionBillingScheduleIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetSubscriptionBillingScheduleDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
