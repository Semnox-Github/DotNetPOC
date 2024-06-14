/********************************************************************************************
 * Project Name - CCTransactionsPGW Data Handler
 * Description  - Data handler of the CCTransactionsPGW class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        20-Jun-2017   Lakshminarayana     Created 
 *2.70.2      10-Jul-2019   Girish Kundar       Modified : For SQL Injection Issue.  
 *                                                         Added missed Columns to Insert/Update
 *2.70.2      06-Dec-2019   Jinto Thomas        Removed siteid from update query          
 *2.110.0     08-Dec-2020   Guru S A            Subscription changes                                                          
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    ///  CCTransactionsPGW Data Handler - Handles insert, update and select of  CCTransactionsPGW objects
    /// </summary>
    public class CCTransactionsPGWDataHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<CCTransactionsPGWDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CCTransactionsPGWDTO.SearchByParameters, string>
            {
                {CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, "Tr.ResponseID"},
                {CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, "Tr.InvoiceNo"},
                {CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, "Tr.TranCode"},
                {CCTransactionsPGWDTO.SearchByParameters.MASTER_ENTITY_ID, "Tr.MasterEntityId"},
                {CCTransactionsPGWDTO.SearchByParameters.SITE_ID, "Tr.site_id"},
                {CCTransactionsPGWDTO.SearchByParameters.REF_NO,"Tr.RefNo" },
                {CCTransactionsPGWDTO.SearchByParameters.AUTH_CODE,"Tr.AuthCode" },
                {CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN,"Tr.ResponseOrigin" }
            };

        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * FROM CCTransactionsPGW Tr ";
        private DataAccessHandler dataAccessHandler;
        private List<SqlParameter> parameters = new List<SqlParameter>();

        /// <summary>
        /// Default constructor of CCTransactionsPGWDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CCTransactionsPGWDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new  DataAccessHandler ();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// ParameterHelper method for building SQL parameters for query
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="parameterName">parameterName</param>
        /// <param name="value">value</param>
        /// <param name="negetiveValueNull">negetiveValueNull</param>
        //private void ParameterHelper(List<SqlParameter> parameters, string parameterName, object value, bool negetiveValueNull = false)
        //{
        //    log.LogMethodEntry(parameters, parameterName, value, negetiveValueNull);
        //    if(parameters != null && !string.IsNullOrEmpty(parameterName))
        //    {
        //        if(value is int)
        //        {
        //            if(negetiveValueNull && ((int)value) < 0)
        //            {
        //                parameters.Add(new SqlParameter(parameterName, DBNull.Value));
        //            }
        //            else
        //            {
        //                parameters.Add(new SqlParameter(parameterName, value));
        //            }
        //        }
        //        else if(value is string)
        //        {
        //            if(string.IsNullOrEmpty(value as string))
        //            {
        //                parameters.Add(new SqlParameter(parameterName, DBNull.Value));
        //            }
        //            else
        //            {
        //                parameters.Add(new SqlParameter(parameterName, value));
        //            }
        //        }
        //        else
        //        {
        //            if(value == null)
        //            {
        //                parameters.Add(new SqlParameter(parameterName, DBNull.Value));
        //            }
        //            else
        //            {
        //                parameters.Add(new SqlParameter(parameterName, value));
        //            }
        //        }
        //    }
        //    log.LogMethodExit();
        //}


        /// <summary>
        /// GetSQLParameters method to build SQL parameters for Insert/ Update
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="CCStatusPGWDTO">CCStatusPGWDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters( CCTransactionsPGWDTO cCTransactionsPGWDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cCTransactionsPGWDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ResponseID", cCTransactionsPGWDTO.ResponseID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InvoiceNo", cCTransactionsPGWDTO.InvoiceNo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TokenID", cCTransactionsPGWDTO.TokenID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecordNo", cCTransactionsPGWDTO.RecordNo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DSIXReturnCode", cCTransactionsPGWDTO.DSIXReturnCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StatusID", cCTransactionsPGWDTO.StatusID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TextResponse", cCTransactionsPGWDTO.TextResponse));
            parameters.Add(dataAccessHandler.GetSecureSQLParameter("@AcctNo", cCTransactionsPGWDTO.AcctNo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardType", cCTransactionsPGWDTO.CardType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TranCode", cCTransactionsPGWDTO.TranCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RefNo", cCTransactionsPGWDTO.RefNo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Purchase", cCTransactionsPGWDTO.Purchase));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Authorize", cCTransactionsPGWDTO.Authorize));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionDatetime", cCTransactionsPGWDTO.TransactionDatetime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AuthCode", cCTransactionsPGWDTO.AuthCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProcessData", cCTransactionsPGWDTO.ProcessData));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ResponseOrigin", cCTransactionsPGWDTO.ResponseOrigin));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserTraceData", cCTransactionsPGWDTO.UserTraceData));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CaptureStatus", cCTransactionsPGWDTO.CaptureStatus));
            parameters.Add(dataAccessHandler.GetSecureSQLParameter("@AcqRefData", cCTransactionsPGWDTO.AcqRefData));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TipAmount", cCTransactionsPGWDTO.TipAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", cCTransactionsPGWDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MerchantCopy", cCTransactionsPGWDTO.MerchantCopy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerCopy", cCTransactionsPGWDTO.CustomerCopy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerCardProfileId", cCTransactionsPGWDTO.CustomerCardProfileId));

            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CCTransactionsPGW record to the database
        /// </summary>
        /// <param name="cCTransactionsPGWDTO">CCTransactionsPGWDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CCTransactionsPGWDTO</returns>
        public CCTransactionsPGWDTO InsertCCTransactionsPGW(CCTransactionsPGWDTO cCTransactionsPGWDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cCTransactionsPGWDTO, loginId, siteId);
            string query = @"INSERT INTO CCTransactionsPGW 
                                        ( 
                                            InvoiceNo,
                                            TokenID,
                                            RecordNo,
                                            DSIXReturnCode,
                                            StatusID,
                                            TextResponse,
                                            AcctNo,
                                            CardType,
                                            TranCode,
                                            RefNo,
                                            Purchase,
                                            Authorize,
                                            TransactionDatetime,
                                            AuthCode,
                                            ProcessData,
                                            ResponseOrigin,
                                            UserTraceData,
                                            CaptureStatus,
                                            AcqRefData,
                                            TipAmount,
                                            site_id,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate, 
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            Customercopy,
                                            MerchantCopy,
                                            CustomerCardProfileId
                                        ) 
                                VALUES 
                                        (
                                            @InvoiceNo,
                                            @TokenID,
                                            @RecordNo,
                                            @DSIXReturnCode,
                                            @StatusID,
                                            @TextResponse,
                                            @AcctNo,
                                            @CardType,
                                            @TranCode,
                                            @RefNo,
                                            @Purchase,
                                            @Authorize,
                                            @TransactionDatetime,
                                            @AuthCode,
                                            @ProcessData,
                                            @ResponseOrigin,
                                            @UserTraceData,
                                            @CaptureStatus,
                                            @AcqRefData,
                                            @TipAmount,
                                            @site_id,
                                            @MasterEntityId,
                                            @CreatedBy,
                                            GETDATE(), 
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @CustomerCopy,
                                            @MerchantCopy,
                                            @CustomerCardProfileId
                                        )SELECT * FROM CCTransactionsPGW WHERE ResponseID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(cCTransactionsPGWDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCCTransactionsPGWDTO(cCTransactionsPGWDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cCTransactionsPGWDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cCTransactionsPGWDTO);
            return cCTransactionsPGWDTO;
        }

        /// <summary>
        /// Updates the CCTransactionsPGW record
        /// </summary>
        /// <param name="cCTransactionsPGWDTO">CCTransactionsPGWDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the CCTransactionsPGWDTO</returns>
        public CCTransactionsPGWDTO UpdateCCTransactionsPGW(CCTransactionsPGWDTO cCTransactionsPGWDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cCTransactionsPGWDTO, loginId, siteId);
            string query = @"UPDATE CCTransactionsPGW 
                             SET InvoiceNo=@InvoiceNo,
                                 TokenID=@TokenID,
                                 RecordNo=@RecordNo,
                                 DSIXReturnCode=@DSIXReturnCode,
                                 StatusID=@StatusID,  
                                 TextResponse=@TextResponse,
                                 AcctNo=@AcctNo,   
                                 CardType=@CardType,
                                 TranCode=@TranCode,
                                 RefNo=@RefNo,
                                 Purchase=@Purchase,
                                 Authorize=@Authorize,
                                 TransactionDatetime=@TransactionDatetime,
                                 AuthCode=@AuthCode,
                                 ProcessData=@ProcessData,
                                 ResponseOrigin=@ResponseOrigin,
                                 UserTraceData=@UserTraceData,
                                 CaptureStatus=@CaptureStatus,
                                 AcqRefData=@AcqRefData,
                                 TipAmount=@TipAmount,
                                 --site_id=@site_id,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate  = GetDate(),
                                 CustomerCardProfileId = @CustomerCardProfileId
                             WHERE  ResponseID = @ResponseID
                             SELECT * FROM CCTransactionsPGW WHERE ResponseID = @ResponseID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(cCTransactionsPGWDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCCTransactionsPGWDTO(cCTransactionsPGWDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating cCTransactionsPGWDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cCTransactionsPGWDTO);
            return cCTransactionsPGWDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="ccTransactionsPGWDTO">CCTransactionsPGWDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCCTransactionsPGWDTO(CCTransactionsPGWDTO ccTransactionsPGWDTO, DataTable dt)
        {
            log.LogMethodEntry(ccTransactionsPGWDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                ccTransactionsPGWDTO.ResponseID = Convert.ToInt32(dt.Rows[0]["ResponseID"]);
                ccTransactionsPGWDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                ccTransactionsPGWDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                ccTransactionsPGWDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                ccTransactionsPGWDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                ccTransactionsPGWDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                ccTransactionsPGWDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to CCTransactionsPGWDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CCTransactionsPGWDTO</returns>
        private CCTransactionsPGWDTO GetCCTransactionsPGWDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO(Convert.ToInt32(dataRow["ResponseID"]),
                                            dataRow["InvoiceNo"] == DBNull.Value ? string.Empty : dataRow["InvoiceNo"].ToString(),
                                            dataRow["TokenID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TokenID"]),
                                            dataRow["RecordNo"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RecordNo"]),
                                            dataRow["DSIXReturnCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DSIXReturnCode"]),
                                            dataRow["StatusID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["StatusID"]),
                                            dataRow["TextResponse"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TextResponse"]),
                                            dataRow["AcctNo"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["AcctNo"]),
                                            dataRow["CardType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CardType"]),
                                            dataRow["TranCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TranCode"]),
                                            dataRow["RefNo"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RefNo"]),
                                            dataRow["Purchase"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Purchase"]),
                                            dataRow["Authorize"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Authorize"]),
                                            dataRow["TransactionDatetime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["TransactionDatetime"]),
                                            dataRow["AuthCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["AuthCode"]),
                                            dataRow["ProcessData"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProcessData"]),
                                            dataRow["ResponseOrigin"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ResponseOrigin"]),
                                            dataRow["UserTraceData"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UserTraceData"]),
                                            dataRow["CaptureStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CaptureStatus"]),
                                            dataRow["AcqRefData"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["AcqRefData"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["TipAmount"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TipAmount"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["CustomerCopy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CustomerCopy"]),
                                            dataRow["MerchantCopy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["MerchantCopy"]),
                                            dataRow["CustomerCardProfileId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CustomerCardProfileId"])
                                            );

            log.LogMethodExit(cCTransactionsPGWDTO);
            return cCTransactionsPGWDTO;
        }

        /// <summary>
        /// Gets the CCTransactionsPGW data of passed transactionsId
        /// </summary>
        /// <param name="responseId">integer type parameter</param>
        /// <returns>Returns CCTransactionsPGWDTO</returns>
        public CCTransactionsPGWDTO GetCCTransactionsPGWDTO(int responseId)
        {
            log.LogMethodEntry(responseId);
            CCTransactionsPGWDTO returnValue = null;
            string query = SELECT_QUERY +"    WHERE Tr.ResponseID = @ResponseID";
            SqlParameter parameter = new SqlParameter("@ResponseID", responseId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter },sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                returnValue = GetCCTransactionsPGWDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// To Build the Query String
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>Query String object</returns>
        private string BuildQueryString(List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string joiner = string.Empty;
            StringBuilder query = new StringBuilder(" where ");
            parameters = new List<SqlParameter>();
            foreach (KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string> searchParameter in searchParameters)
            {
                if(DBSearchParameters.ContainsKey(searchParameter.Key))
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if(searchParameter.Key == CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID ||
                       searchParameter.Key == CCTransactionsPGWDTO.SearchByParameters.MASTER_ENTITY_ID)
                    {
                        query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                    }
                    else if(searchParameter.Key == CCTransactionsPGWDTO.SearchByParameters.SITE_ID)
                    {
                        query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " =-1)");
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                    }
                    else if (searchParameter.Key == CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER ||
                            searchParameter.Key == CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE ||
                            searchParameter.Key == CCTransactionsPGWDTO.SearchByParameters.AUTH_CODE ||
                            searchParameter.Key == CCTransactionsPGWDTO.SearchByParameters.REF_NO||
                            searchParameter.Key == CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN)
                    {
                        query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key),searchParameter.Value));
                    }
                    else
                    {
                        query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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

            log.LogMethodExit(query.ToString());
            return query.ToString();
        }

        /// <summary>
        /// Gets the CCTransactionsPGWDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CCTransactionsPGWDTO matching the search criteria</returns>
        public List<CCTransactionsPGWDTO> GetCCTransactionsPGWDTOList(List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CCTransactionsPGWDTO> list = null;
            
            string selectQuery =SELECT_QUERY;
            if((searchParameters != null) && (searchParameters.Count > 0))
            {

                selectQuery = selectQuery + BuildQueryString(searchParameters);
            }
            log.Debug("Search query: " + selectQuery);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                list = new List<CCTransactionsPGWDTO>();
                foreach(DataRow dataRow in dataTable.Rows)
                {
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = GetCCTransactionsPGWDTO(dataRow);
                    list.Add(cCTransactionsPGWDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the CCTransactionsPGWDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Non Reversed CCTransactionsPGWDTO matching the search criteria</returns>
        public List<CCTransactionsPGWDTO> GetNonReversedCCTransactionsPGWDTOList(List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CCTransactionsPGWDTO> list = null;

            string selectQuery = SELECT_QUERY;
            KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>? splitIdParameter = null;
            KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>? transactionIdParameter = null;
            if((searchParameters != null) && (searchParameters.Count > 0))
            {
                foreach(var item in searchParameters)
                {
                    if(item.Key == CCTransactionsPGWDTO.SearchByParameters.SPLIT_ID)
                    {
                        splitIdParameter = item;
                    }
                    if(item.Key == CCTransactionsPGWDTO.SearchByParameters.TRANSACTION_ID)
                    {
                        transactionIdParameter = item;
                    }
                }
                if(splitIdParameter != null)
                {
                    searchParameters.Remove(splitIdParameter.Value);
                }
                if(transactionIdParameter != null)
                {
                    searchParameters.Remove(transactionIdParameter.Value);
                }
                selectQuery = selectQuery + BuildQueryString(searchParameters) + " AND ";
            }
            else
            {
                selectQuery = selectQuery + " WHERE ";
            }
            StringBuilder transactionAndSplitIdCondition = new StringBuilder();
            if(transactionIdParameter != null)
            {
                transactionAndSplitIdCondition.Append(" tp.TrxId=");
                transactionAndSplitIdCondition.Append(transactionIdParameter.Value.Value);
                transactionAndSplitIdCondition.Append(" AND ");
            }
            if(splitIdParameter != null)
            {
                transactionAndSplitIdCondition.Append(" tp.SplitId=");
                transactionAndSplitIdCondition.Append(splitIdParameter.Value.Value);
                transactionAndSplitIdCondition.Append(" AND ");
            }
            selectQuery = selectQuery + " EXISTS(SELECT * FROM TrxPayments tp WHERE " + transactionAndSplitIdCondition.ToString()+ " tp.CCResponseId = Tr.ResponseID AND tp.ParentPaymentId IS NULL AND NOT EXISTS(SELECT * FROM TrxPayments WHERE TrxPayments.ParentPaymentId = tp.PaymentId)) ";
            log.Debug("Search query: " + selectQuery);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(),sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                list = new List<CCTransactionsPGWDTO>();
                for(int i = 0; i < dataTable.Rows.Count; i++)
                {
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = GetCCTransactionsPGWDTO(dataTable.Rows[i]);
                    list.Add(cCTransactionsPGWDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
