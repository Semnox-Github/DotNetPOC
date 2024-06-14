/********************************************************************************************
 * Project Name - Transaction Data Handler                                                                    
 * Description  - Data Handler for Transction 
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.150.01    16-Feb-2023       Yashodhara C H     Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;
using System.Globalization;

namespace Semnox.Parafait.Transaction
{
    class TransactionSummaryViewDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @" SELECT * FROM TransactionSummaryView(@PassPhrase) AS tsv";

        private static readonly Dictionary<TransactionSummaryViewDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionSummaryViewDTO.SearchByParameters, string>
        {
                {TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_ID,"tsv.TrxId"},
                {TransactionSummaryViewDTO.SearchByParameters.SITE_ID, "tsv.site_id"},
                {TransactionSummaryViewDTO.SearchByParameters.ORDER_ID, "tsv.OrderId"},
                {TransactionSummaryViewDTO.SearchByParameters.POS_MACHINE_ID, "tsv.POSMachineId"}, 
                {TransactionSummaryViewDTO.SearchByParameters.POS_TYPE_ID, "tsv.POSTypeId"},
                {TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_OTP, "tsv.TransactionOTP"},
                {TransactionSummaryViewDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, "tsv.external_system_reference"},
                {TransactionSummaryViewDTO.SearchByParameters.ORIGINAL_SYSTEM_REFERENCE, "tsv.Original_system_reference"},
                {TransactionSummaryViewDTO.SearchByParameters.CUSTOMER_ID, "tsv.customerId"},
                {TransactionSummaryViewDTO.SearchByParameters.FROM_DATE, "tsv.TrxDate"},
                {TransactionSummaryViewDTO.SearchByParameters.TO_DATE, "tsv.TrxDate"},
                {TransactionSummaryViewDTO.SearchByParameters.CREATED_FROM_DATE, "tsv.CreationDate"},
                {TransactionSummaryViewDTO.SearchByParameters.CREATED_TO_DATE, "tsv.CreationDate"},
                {TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_NUMBER, "tsv.trx_no"},
                {TransactionSummaryViewDTO.SearchByParameters.REMARKS, "tsv.Remarks"},
                {TransactionSummaryViewDTO.SearchByParameters.USER_ID, "tsv.user_id"},
                {TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_ID_LIST, "tsv.TrxId"},
                {TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_NUMBER_LIST, "tsv.trx_no"},
                {TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_OTP_LIST, "tsv.TransactionOTP"},
                {TransactionSummaryViewDTO.SearchByParameters.ORIGINAL_TRX_ID, "tsv.OriginalTrxid"},
                {TransactionSummaryViewDTO.SearchByParameters.EMAIL_ID, "tsv.EmailId"},
                {TransactionSummaryViewDTO.SearchByParameters.PHONE_NUMBER, "tsv.PhoneNumber"}
            };

        /// <summary>
        /// Default constructor of TransactionSummaryViewDataHandler class
        /// </summary>
        public TransactionSummaryViewDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();

        }

        private TransactionSummaryViewDTO GetTransactionSummaryViewDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionSummaryViewDTO transactionSummaryViewDTO = new TransactionSummaryViewDTO(dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                                                                    dataRow["TrxDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["TrxDate"]),
                                                                                    dataRow["TrxAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["TrxAmount"]),
                                                                                    dataRow["TrxDiscountPercentage"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["TrxDiscountPercentage"]),
                                                                                    dataRow["TaxAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["TaxAmount"]),
                                                                                    dataRow["TrxNetAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["TrxNetAmount"]),
                                                                                    dataRow["pos_machine"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["pos_machine"]),
                                                                                    dataRow["user_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["user_id"]),
                                                                                    dataRow["payment_mode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["payment_mode"]),
                                                                                    dataRow["CashAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["CashAmount"]),
                                                                                    dataRow["CreditCardAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["CreditCardAmount"]),
                                                                                    dataRow["GameCardAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["GameCardAmount"]),
                                                                                    dataRow["PaymentReference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PaymentReference"]),
                                                                                    dataRow["PrimaryCardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PrimaryCardId"]),
                                                                                    dataRow["OrderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderId"]),
                                                                                    dataRow["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSTypeId"]),
                                                                                    dataRow["trx_no"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["trx_no"]),
                                                                                    dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                                                                    dataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSMachineId"]),
                                                                                    dataRow["OtherPaymentModeAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["OtherPaymentModeAmount"]),
                                                                                    dataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Status"]),
                                                                                    dataRow["TrxProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxProfileId"]),
                                                                                    dataRow["TokenNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TokenNumber"]),
                                                                                    dataRow["Original_System_Reference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Original_System_Reference"]),
                                                                                    dataRow["customerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["customerId"]),
                                                                                    dataRow["External_System_Reference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["External_System_Reference"]),
                                                                                    dataRow["ReprintCount"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ReprintCount"]),
                                                                                    dataRow["OriginalTrxID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OriginalTrxID"]),
                                                                                    dataRow["OrderTypeGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderTypeGroupId"]),
                                                                                    dataRow["TransactionOTP"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TransactionOTP"]),
                                                                                    dataRow["CustomerIdentifier"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CustomerIdentifier"]),
                                                                                    dataRow["PrintCount"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PrintCount"]),
                                                                                    dataRow["SaveStartTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["SaveStartTime"]),
                                                                                    dataRow["SaveEndTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["SaveEndTime"]),
                                                                                    dataRow["PrintStartTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["PrintStartTime"]),
                                                                                    dataRow["PrintEndTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["PrintEndTime"]),
                                                                                    dataRow["TrxInitiatedTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["TrxInitiatedTime"]),
                                                                                    dataRow["TransactionIdentifier"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TransactionIdentifier"]),
                                                                                    dataRow["GuestName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GuestName"]),
                                                                                    dataRow["TentNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TentNumber"]),
                                                                                    dataRow["Channel"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Channel"]),
                                                                                    dataRow["TransactionDiscountTotal"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["TransactionDiscountTotal"]),
                                                                                    dataRow["TransactionPaymentTotal"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["TransactionPaymentTotal"]),
                                                                                    dataRow["GuestCount"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GuestCount"]),
                                                                                    dataRow["IsNonChargeable"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsNonChargeable"]),
                                                                                    dataRow["TransactionPaymentStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TransactionPaymentStatus"]),
                                                                                    dataRow["TransactionPaymentStatusChangeDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["TransactionPaymentStatusChangeDate"]),
                                                                                    dataRow["TransactionStatusChangeDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["TransactionStatusChangeDate"]),
                                                                                    dataRow["SessionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SessionId"]),
                                                                                    dataRow["TransactionReopenedCount"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionReopenedCount"]),
                                                                                    dataRow["TransactionClosedTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["TransactionClosedTime"]),
                                                                                    dataRow["TransactionCancelledTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["TransactionCancelledTime"]),
                                                                                    dataRow["TransactionReopenedTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["TransactionReopenedTime"]),
                                                                                    dataRow["ApprovedBy"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ApprovedBy"]),
                                                                                    dataRow["LockedTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LockedTime"]),
                                                                                    dataRow["TransactionStatusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionStatusId"]),
                                                                                    dataRow["TransactionPaymentStatusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionPaymentStatusId"]),
                                                                                    dataRow["LockedByPOSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LockedByPOSMachineId"]),
                                                                                    dataRow["LockStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LockStatus"]),
                                                                                    dataRow["LockedBySiteId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LockedBySiteId"]),
                                                                                    dataRow["LockedByUserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LockedByUserId"]),
                                                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                                                    dataRow["ProcessedForLoyalty"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["ProcessedForLoyalty"]),
                                                                                    dataRow["TransactionTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionTypeId"]),
                                                                                    dataRow["TransactionStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TransactionStatus"]),
                                                                                    dataRow["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EmailId"]),
                                                                                    dataRow["PhoneNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PhoneNumber"]),
                                                                                    dataRow["card_number"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["card_number"]),
                                                                                    dataRow["POS_Counter"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["POS_Counter"]),
                                                                                    dataRow["PaymentMode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PaymentMode"]),
                                                                                    dataRow["ProfileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProfileName"]),
                                                                                    dataRow["customer_name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["customer_name"]),
                                                                                    dataRow["CashRatio"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["CashRatio"]),
                                                                                    dataRow["CreditCardRatio"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["CreditCardRatio"]),
                                                                                    dataRow["GameCardRatio"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["GameCardRatio"]),
                                                                                    dataRow["OtherModeRatio"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["OtherModeRatio"]),
                                                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty: Convert.ToString(dataRow["CreatedBy"]),
                                                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                                                    dataRow["LastUpdateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateTime"]),
                                                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                                                    dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]));

            log.LogMethodExit(transactionSummaryViewDTO);
            return transactionSummaryViewDTO;
        }


        /// <summary>
        /// Gets the Transaction data of passed transaction Id
        /// </summary>
        /// <param name="transactionId">integer type parameter</param>
        /// <returns>Returns TransactionSummaryViewDTO</returns>
        public TransactionSummaryViewDTO GetTransactionSummaryViewDTO(int transactionId)
        {
            log.LogMethodEntry(transactionId);
            TransactionSummaryViewDTO result = null;
            string selectQuery = SELECT_QUERY + " WHERE tsv.TrxId = @TrxId";
            SqlParameter parameter = new SqlParameter("@TrxId", transactionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTransactionSummaryViewDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets TransactionSummaryViewDTO List
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<TransactionSummaryViewDTO> GetTransactionSummaryViewDTOList(List<KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>> searchParameters, ExecutionContext executionContext, int pageNumber = 0, int numberOfRecords = 10)
        {
            log.LogMethodEntry(searchParameters);
            List<TransactionSummaryViewDTO> transactionSummaryViewDTOList = new List<TransactionSummaryViewDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" where ");
                String offsetQuery = "";
                if (numberOfRecords > -1 && (pageNumber * numberOfRecords) >= 0)
                {
                    offsetQuery = " OFFSET " + pageNumber * numberOfRecords + " ROWS FETCH NEXT " + numberOfRecords.ToString() + " ROWS ONLY";
                }
                foreach (KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.ORDER_ID ||
                            searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_ID ||
                            searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.POS_MACHINE_ID ||
                            searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.CUSTOMER_ID ||
                            searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.ORIGINAL_TRX_ID ||
                            searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.POS_TYPE_ID ||
                                 searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.USER_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE ||
                                searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.ORIGINAL_SYSTEM_REFERENCE ||
                                searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.REMARKS ||
                                searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_OTP ||
                                searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_NUMBER )
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.EMAIL_ID ||
                                 searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.PHONE_NUMBER)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.FROM_DATE ||
                            searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.CREATED_FROM_DATE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",getdate()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.TO_DATE ||
                            searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.CREATED_TO_DATE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",getdate()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_ID_LIST ||
                                 searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_NUMBER_LIST ||
                                 searchParameter.Key == TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_OTP_LIST) 
                        {
                            query.Append(joiner + @"(" + DBSearchParameters[searchParameter.Key] + " IN ( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + "))");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                selectQuery = selectQuery + query + " ORDER BY tsv.TrxId desc " + offsetQuery.ToString();
            }

            parameters.Add(new SqlParameter("@PassPhrase", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE")));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TransactionSummaryViewDTO accountSummaryViewDTO = GetTransactionSummaryViewDTO(dataRow);
                    transactionSummaryViewDTOList.Add(accountSummaryViewDTO);
                }
            }
            log.LogMethodExit(transactionSummaryViewDTOList);
            return transactionSummaryViewDTOList;
        }
    }
}
