/********************************************************************************************
 * Project Name - CCRequestPGW Data Handler
 * Description  - Data handler of the CCRequestPGW class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        20-Jun-2017   Lakshminarayana     Created 
 *2.70.2        01-Jul-2019   Girish Kundar       Modified : For SQL Injection Issue.  
 *                                                         Added missed Columns to Insert/Update
 *2.70.2       06-Dec-2019   Jinto Thomas            Removed siteid from update query                                                         
*2.110.0       30-Dec-2020      Girish Kundar       Modified : Added delete method = Payment link changes  
*2.150.2     08-Mar-2023   Muaaz Musthafa           Modified: Added new search param to filter based TRANSACTION_TYPE
*2.150.2       31-Jan-2023      Nitin Pai           Modified - Added a new method to change the status of the CC Request. This checks the current status.
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
    ///  CCRequestPGW Data Handler - Handles insert, update and select of  CCRequestPGW objects
    /// </summary>
    public class CCRequestPGWDataHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<CCRequestPGWDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CCRequestPGWDTO.SearchByParameters, string>
            {
                {CCRequestPGWDTO.SearchByParameters.REQUEST_ID, "ccr.RequestID"},
                {CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, "ccr.InvoiceNo"},
                {CCRequestPGWDTO.SearchByParameters.SITE_ID, "ccr.site_id"},
                {CCRequestPGWDTO.SearchByParameters.MASTER_ENTITY_ID, "ccr.MasterEntityId"},
                {CCRequestPGWDTO.SearchByParameters.MERCHANT_ID, "ccr.MerchantID"},
                {CCRequestPGWDTO.SearchByParameters.PAYMENT_PROCESS_STATUS, "ccr.PaymentProcessStatus"},
                {CCRequestPGWDTO.SearchByParameters.TRANSACTION_TYPE, "ccr.TransactionType"}
            };
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * FROM CCRequestPGW AS ccr ";
        private DataAccessHandler dataAccessHandler;
        private List<SqlParameter> parameters = new List<SqlParameter>();

        /// <summary>
        /// Parameterized constructor of CCRequestPGWDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CCRequestPGWDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
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
        private void ParameterHelper(List<SqlParameter> parameters, string parameterName, object value, bool negetiveValueNull = false)
        {
            log.LogMethodEntry(parameters, parameterName, value, negetiveValueNull);

            if(parameters != null && !string.IsNullOrEmpty(parameterName))
            {
                if(value is int)
                {
                    if(negetiveValueNull && ((int)value) < 0)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else if(value is string)
                {
                    if(string.IsNullOrEmpty(value as string))
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else
                {
                    if(value == null)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
            }

            log.LogMethodExit();
        }


        /// <summary>
        /// GetSQLParameters method to build SQL parameters for Insert/ Update
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="cCRequestPGWDTO">cCRequestPGWDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(CCRequestPGWDTO cCRequestPGWDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cCRequestPGWDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParameterHelper(parameters, "@RequestID", cCRequestPGWDTO.RequestID);
            ParameterHelper(parameters, "@InvoiceNo", cCRequestPGWDTO.InvoiceNo);
            ParameterHelper(parameters, "@POSAmount", cCRequestPGWDTO.POSAmount);
            ParameterHelper(parameters, "@TransactionType", cCRequestPGWDTO.TransactionType);
            ParameterHelper(parameters, "@ReferenceNo", cCRequestPGWDTO.ReferenceNo);
            ParameterHelper(parameters, "@StatusID", cCRequestPGWDTO.StatusID, true);
            ParameterHelper(parameters, "@CardLastFourDigits", cCRequestPGWDTO.CardLastFourDigits);
            ParameterHelper(parameters, "@EDSettlement", cCRequestPGWDTO.EDSettlement);
            ParameterHelper(parameters, "@MerchantID", cCRequestPGWDTO.MerchantID);
            ParameterHelper(parameters, "@MasterEntityId", cCRequestPGWDTO.MasterEntityId, true);
            ParameterHelper(parameters, "@CreatedBy", loginId);
            ParameterHelper(parameters, "@LastUpdatedBy", loginId);
            ParameterHelper(parameters, "@site_id", siteId,true);
            ParameterHelper(parameters, "@PaymentProcessStatus", cCRequestPGWDTO.PaymentProcessStatus);
            log.LogMethodExit(parameters);
            return parameters;

        }

        /// <summary>
        /// Inserts the CCRequestPGW record to the database
        /// </summary>
        /// <param name="cCRequestPGWDTO">CCRequestPGWDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CCRequestPGWDTO</returns>
        public CCRequestPGWDTO InsertCCRequestPGW(CCRequestPGWDTO cCRequestPGWDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cCRequestPGWDTO, loginId, siteId);
            string query = @"INSERT INTO CCRequestPGW 
                                        ( 
                                            InvoiceNo,
                                            RequestDatetime,
                                            POSAmount,
                                            TransactionType,
                                            ReferenceNo,
                                            StatusID,
                                            CardLastFourDigits,
                                            EDSettlement,
                                            site_id,
                                            MerchantID,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate, 
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            PaymentProcessStatus
                                        ) 
                                VALUES 
                                        (
                                            @InvoiceNo,
                                            GETDATE(),
                                            @POSAmount,
                                            @TransactionType,
                                            @ReferenceNo,
                                            @StatusID,
                                            @CardLastFourDigits,
                                            @EDSettlement,
                                            @site_id,
                                            @MerchantID,
                                            @MasterEntityId,
                                            @CreatedBy,
                                            GETDATE(), 
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @PaymentProcessStatus
                                        )   SELECT * FROM CCRequestPGW WHERE RequestID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(cCRequestPGWDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCCRequestPGWDTO(cCRequestPGWDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cCRequestPGWDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cCRequestPGWDTO);
            return cCRequestPGWDTO;
        }

        /// <summary>
        /// Updates the CCRequestPGW record
        /// </summary>
        /// <param name="cCRequestPGWDTO">CCRequestPGWDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the CCRequestPGWDTO</returns>
        public CCRequestPGWDTO UpdateCCRequestPGW(CCRequestPGWDTO cCRequestPGWDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cCRequestPGWDTO, loginId, siteId);
            string query = @"UPDATE CCRequestPGW 
                             SET InvoiceNo=@InvoiceNo,
                                 POSAmount=@POSAmount,
                                 TransactionType=@TransactionType,
                                 ReferenceNo=@ReferenceNo,
                                 StatusID=@StatusID,  
                                 CardLastFourDigits=@CardLastFourDigits,
                                 EDSettlement=@EDSettlement,   
                                 MerchantID=@MerchantID,
                                 --site_id=@site_id,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate  = GetDate(),
                                 PaymentProcessStatus = @PaymentProcessStatus
                             WHERE RequestID = @RequestID
                            SELECT * FROM CCRequestPGW WHERE RequestID = @RequestID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(cCRequestPGWDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCCRequestPGWDTO(cCRequestPGWDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating cCRequestPGWDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cCRequestPGWDTO);
            return cCRequestPGWDTO;
        }

        /// <summary>
        /// Updates the CCRequestPGW record
        /// </summary>
        /// <param name="cCRequestPGWDTO">CCRequestPGWDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the CCRequestPGWDTO</returns>
        public int ChangePaymentProcessingStatus(CCRequestPGWDTO cCRequestPGWDTO, string currentPaymentProcessStatus, string loginId, int siteId)
        {
            log.LogMethodEntry(cCRequestPGWDTO, loginId, siteId);
            int rowsUpdated = 0;

            string query = @"UPDATE CCRequestPGW 
                             SET LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate  = GetDate(),
                                 PaymentProcessStatus = @PaymentProcessStatus
                             WHERE RequestID = @RequestID and PaymentProcessStatus = @CurrentPaymentProcessStatus
                            SELECT * FROM CCRequestPGW WHERE RequestID = @RequestID";
            try
            {
                List<SqlParameter> parametersList = GetSQLParameters(cCRequestPGWDTO, loginId, siteId);
                ParameterHelper(parametersList, "@CurrentPaymentProcessStatus", currentPaymentProcessStatus);
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, parametersList.ToArray(), sqlTransaction);
                if (rowsUpdated > 0)
                    GetCCRequestPGWDTO(cCRequestPGWDTO.RequestID);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating cCRequestPGWDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }

            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="CCRequestPGWDTO">CCRequestPGWDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCCRequestPGWDTO(CCRequestPGWDTO ccRequestPGWDTO, DataTable dt)
        {
            log.LogMethodEntry(ccRequestPGWDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                ccRequestPGWDTO.RequestID = Convert.ToInt32(dt.Rows[0]["RequestID"]);
                ccRequestPGWDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                ccRequestPGWDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                ccRequestPGWDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                ccRequestPGWDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                ccRequestPGWDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                ccRequestPGWDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to CCRequestPGWDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CCRequestPGWDTO</returns>
        private CCRequestPGWDTO GetCCRequestPGWDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CCRequestPGWDTO cCRequestPGWDTO = new CCRequestPGWDTO(Convert.ToInt32(dataRow["RequestID"]),
                                            dataRow["InvoiceNo"] == DBNull.Value ? string.Empty : dataRow["InvoiceNo"].ToString(),
                                            dataRow["RequestDatetime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["RequestDatetime"]),
                                            dataRow["POSAmount"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["POSAmount"]),
                                            dataRow["TransactionType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TransactionType"]),
                                            dataRow["ReferenceNo"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ReferenceNo"]),
                                            dataRow["StatusID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["StatusID"]),
                                            dataRow["CardLastFourDigits"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CardLastFourDigits"]),
                                            dataRow["EDSettlement"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EDSettlement"]),
                                            dataRow["MerchantID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["MerchantID"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["PaymentProcessStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PaymentProcessStatus"])
                                            );
            log.LogMethodExit(cCRequestPGWDTO);
            return cCRequestPGWDTO;
        }

        /// <summary>
        /// Gets the CCRequestPGW data of passed requestId
        /// </summary>
        /// <param name="requestId">integer type parameter</param>
        /// <returns>Returns CCRequestPGWDTO</returns>
        public CCRequestPGWDTO GetCCRequestPGWDTO(int requestId)
        {
            log.LogMethodEntry(requestId);
            CCRequestPGWDTO returnValue = null;
            string query = SELECT_QUERY + " WHERE ccr.RequestID = @RequestID";
            SqlParameter parameter = new SqlParameter("@RequestID", requestId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                returnValue = GetCCRequestPGWDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        public void Delete(int requestId)
        {
            log.LogMethodEntry(requestId);
            try
            {
                string query = "DELETE FROM CCRequestPGW WHERE RequestID = @RequestID";
                SqlParameter parameter = new SqlParameter("@RequestID", requestId);
                dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            }
            catch (Exception expn)
            {
                log.Error("Error while executing CCRequestPGW()" + expn.Message);
                log.LogMethodExit("Throwing EXception" + expn.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// ParameterBuilder method
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>StringBuilder</returns>
        private StringBuilder ParameterBuilder(List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string joiner = string.Empty;
            parameters = new List<SqlParameter>();
            StringBuilder query = new StringBuilder(" where ");
            foreach (KeyValuePair<CCRequestPGWDTO.SearchByParameters, string> searchParameter in searchParameters)
            {
                if (DBSearchParameters.ContainsKey(searchParameter.Key))
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (searchParameter.Key == CCRequestPGWDTO.SearchByParameters.REQUEST_ID
                        || searchParameter.Key == CCRequestPGWDTO.SearchByParameters.MASTER_ENTITY_ID)
                    {
                        query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                    }
                    else if (searchParameter.Key == CCRequestPGWDTO.SearchByParameters.SITE_ID)
                    {
                        query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                    }
                    else if (searchParameter.Key == CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER
                        || searchParameter.Key == CCRequestPGWDTO.SearchByParameters.MERCHANT_ID
                        || searchParameter.Key == CCRequestPGWDTO.SearchByParameters.PAYMENT_PROCESS_STATUS
                        || searchParameter.Key == CCRequestPGWDTO.SearchByParameters.TRANSACTION_TYPE)
                    {
                        query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key),searchParameter.Value));
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
                    string message = "The query parameter does not exist " + searchParameter.Key;
                    log.LogVariableState("searchParameter.Key", searchParameter.Key);
                    log.LogMethodExit(null, "Throwing exception -" + message);
                    throw new Exception(message);
                }
            }
            log.LogMethodExit(query);
            return query;
        }

        /// <summary>
        /// Gets the MediaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CCRequestPGWDTO matching the search criteria</returns>
        public List<CCRequestPGWDTO> GetCCRequestPGWDTOList(List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);

            List<CCRequestPGWDTO> list = null;
            StringBuilder query = null;
           
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                query = ParameterBuilder(searchParameters);
                selectQuery = selectQuery + query;
            }
            log.Debug("Search query: " + selectQuery);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<CCRequestPGWDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CCRequestPGWDTO cCRequestPGWDTO = GetCCRequestPGWDTO(dataRow);
                    list.Add(cCRequestPGWDTO);
                }
            }
           
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Returns the last transaction made
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>CCRequestPGWDTO object </returns>
        public CCRequestPGWDTO GetLatestCCRequestPGWDTO(List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            StringBuilder query = null;
            CCRequestPGWDTO cCRequestPGWDTO = null;
            string selectQuery = @"SELECT TOP 1 * FROM CCRequestPGW  AS ccr";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                query = ParameterBuilder(searchParameters);
                selectQuery = selectQuery + query;
            }
            selectQuery = selectQuery + " ORDER BY RequestDatetime DESC";

            log.Debug("Search query: " + selectQuery);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {               
                 cCRequestPGWDTO = GetCCRequestPGWDTO(dataTable.Rows[0]);                              
            }
            
            log.LogMethodExit(cCRequestPGWDTO);
            return cCRequestPGWDTO;
        }      
    }
}
