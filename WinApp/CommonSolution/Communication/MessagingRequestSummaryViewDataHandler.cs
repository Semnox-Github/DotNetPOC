/********************************************************************************************
 * Project Name - Communication
 * Description  - Data Handler object MessagingRequestSummaryView
 *
 **************
 ** Version Log
  **************
  * Version     Date        Modified By            Remarks
 *********************************************************************************************
 2.150.01     03-Feb-2023    Yashodhara C H         Created
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// MessagingRequestSummaryViewDataHandler object for Insert ,Update and Search for MessagingRequest Object
    /// </summary>
    public class MessagingRequestSummaryViewDataHandler
    {
        private static readonly logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MessagingRequests AS mr 
                                                LEFT OUTER JOIN 
                                                    (SELECT customer_id, Concat(cv.customer_name, ' ', cv.last_name) AS CustomerName FROM CustomerView(@PassPhrase) cv) c 
                                                        ON c.customer_id = mr.CustomerId
                                                LEFT OUTER JOIN 
                                                    (SELECT ClientId, ClientName FROM MessagingClient ) mc 
                                                        ON mc.ClientId = mr.MessagingClientId
                                                LEFT OUTER JOIN
                                                    (SELECT pfe.ParafaitFunctionEventName,pfe.ParafaitFunctionEventId FROM ParafaitFunctionEvents pfe ) pw 
                                                        ON pw.ParafaitFunctionEventId = mr.ParafaitFunctionEventId
                                                LEFT OUTER JOIN
                                                    (SELECT count(Id) resendCount, OriginalMessageId AS origMsgId FROM MessagingRequests m 
	                                                    GROUP BY OriginalMessageId ) rs 
                                                            ON mr.Id = rs.origMsgId
                                                LEFT OUTER JOIN 
                                                    (SELECT trx_no, TransactionOTP, TrxId  FROM trx_header) th 
                                                          ON th.TrxId = mr.TrxId 
                                                    ";
        /// <summary>
        /// Dictionary for searching Parameters for the MessagingRequest object.
        /// </summary>
        private static readonly Dictionary<MessagingRequestSummaryViewDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MessagingRequestSummaryViewDTO.SearchByParameters, string>
        {
            {MessagingRequestSummaryViewDTO.SearchByParameters.MESSAGE_ID,"mr.Id"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.CARD_ID,"mr.card_id"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.CUSTOMER_ID,"mr.CustomerId"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.SITE_ID,"mr.site_id"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.MASTER_ENTITY_ID,"mr.MasterEntityId"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.MESSAGING_CLIENT_ID,"mr.MessagingClientId"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.MESSAGE_TYPE,"mr.MessageType"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.FROM_DATE,"mr.CreationDate"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.TO_DATE,"mr.CreationDate"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.SIGNED_IN_CUSTOMERS_ONLY,"mr.SignedInCustomersOnly"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID,"mr.ParafaitFunctionEventId"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.MESSAGE_ID_LIST,"mr.Id"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID_LIST,"mr.ParafaitFunctionEventId"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.ORIGINAL_MESSAGE_ID,"mr.OriginalMessageId"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.TRX_NUMBER,"mr.TrxNumber"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.PARENT_AND_CHILD_MESSAGES_BY_ID,"mr.Id"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.TRX_OTP,"th.TransactionOTP"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.TO_MOBILE_LIST,"mr.ToMobile"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.TO_EMAIL_LIST,"mr.ToEmails"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.TRX_NUMBER_LIST,"mr.TrxNumber"},
            {MessagingRequestSummaryViewDTO.SearchByParameters.TRX_OTP_LIST,"th.TransactionOTP"}

        };

        /// <summary>
        /// Parameterized Constructor for MessagingRequestSummaryViewDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public MessagingRequestSummaryViewDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to MessagingRequestSummaryViewDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>MessagingRequestSummaryViewDTO</returns>
        private MessagingRequestSummaryViewDTO GetMessagingRequestSummaryViewDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MessagingRequestSummaryViewDTO messagingRequestSummaryViewDTO = new MessagingRequestSummaryViewDTO(
                                                         dataRow["Id"] == DBNull.Value ? -1 :  Convert.ToInt32(dataRow["Id"]) ,
                                                         dataRow["BatchId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["BatchId"]),
                                                         dataRow["Reference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Reference"]),
                                                         dataRow["MessageType"] == DBNull.Value ? string.Empty : MessagingRequestSummaryViewDTO.ToMessageType(Convert.ToString(dataRow["MessageType"])), 
                                                         dataRow["ToEmails"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ToEmails"]),
                                                         dataRow["ToMobile"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ToMobile"]),
                                                         dataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Status"]),
                                                         dataRow["StatusMessage"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["StatusMessage"]),
                                                         dataRow["SendDate"] == DBNull.Value ? (DateTime? ) null : Convert.ToDateTime(dataRow["SendDate"]),
                                                         dataRow["SendAttemptDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["SendAttemptDate"]),
                                                         dataRow["Attempts"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Attempts"]),
                                                         dataRow["Subject"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Subject"]),
                                                         dataRow["Body"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Body"]),
                                                         dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["card_Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["card_Id"]),
                                                         dataRow["AttachFile"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["AttachFile"]),
                                                         dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["Cc"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Cc"]),
                                                         dataRow["Bcc"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Bcc"]),
                                                         dataRow["MessagingClientId"] == DBNull.Value ?-1 : Convert.ToInt32(dataRow["MessagingClientId"]),
                                                         dataRow["MessageRead"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["MessageRead"]),
                                                         dataRow["ToDevice"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ToDevice"]),
                                                         dataRow["SignedInCustomersOnly"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["SignedInCustomersOnly"]),
                                                         dataRow["CountryCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CountryCode"]),
                                                         dataRow["TrxNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TrxNumber"]),
                                                         dataRow["ResendCount"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ResendCount"]),
                                                         dataRow["ParafaitFunctionEventId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ParafaitFunctionEventId"]),
                                                         dataRow["OriginalMessageId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["OriginalMessageId"]),
                                                          dataRow["TrxId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["TrxId"]),
                                                         dataRow["ParafaitFunctionEventName"] == DBNull.Value ? null : Convert.ToString(dataRow["ParafaitFunctionEventName"]),
                                                         dataRow["CustomerName"] == DBNull.Value ? null : Convert.ToString(dataRow["CustomerName"]),
                                                         dataRow["ClientName"] == DBNull.Value ? null : Convert.ToString(dataRow["ClientName"]),
                                                         dataRow["TransactionOTP"] == DBNull.Value ? null : Convert.ToString(dataRow["TransactionOTP"])
                                                        );
            log.LogMethodExit(messagingRequestSummaryViewDTO);
            return messagingRequestSummaryViewDTO;
        }

        /// <summary>
        /// Gets the MessagingRequestSummaryViewDTO data of passed id 
        /// </summary>
        /// <param name="id">id -MessagingRequestId </param>
        /// <returns>Returns MessagingRequestSummaryViewDTO</returns>
        public MessagingRequestSummaryViewDTO GetMessagingRequestSummaryView(int id)
        {
            log.LogMethodEntry(id);
            MessagingRequestSummaryViewDTO result = null;
            string query = SELECT_QUERY + @" WHERE mr.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMessagingRequestSummaryViewDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }       

        /// <summary>
        /// Returns the List of MessagingRequestSummaryViewDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">Search Parameters</param>
        /// <returns>Returns the List of MessagingRequestSummaryViewDTO</returns>
        public List<MessagingRequestSummaryViewDTO> GetMessagingRequestSummaryViewDTOList(List<KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>> searchParameters, ExecutionContext executionContext, int pageNumber = 0, int numberOfRecords = -1)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction, pageNumber, numberOfRecords);
            List<MessagingRequestSummaryViewDTO> messagingRequestSummaryViewDTOList = new List<MessagingRequestSummaryViewDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                String offsetQuery = "";
                if (numberOfRecords > -1 && (pageNumber * numberOfRecords) >= 0)
                {
                    offsetQuery = " OFFSET " + pageNumber * numberOfRecords + " ROWS FETCH NEXT " + numberOfRecords.ToString() + " ROWS ONLY";
                }
                foreach (KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.MESSAGE_ID
                            || searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.CUSTOMER_ID
                            || searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.CARD_ID
                            || searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.MESSAGING_CLIENT_ID
                            || searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID
                            || searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.ORIGINAL_MESSAGE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.MESSAGE_ID_LIST 
                             || searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID_LIST 
                             || searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.TRX_NUMBER_LIST)
                        {
                            query.Append(joiner + @"("+ DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")) ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.TO_MOBILE_LIST
                             || searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.TO_EMAIL_LIST)
                        {
                            query.Append(joiner +  DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.PARENT_AND_CHILD_MESSAGES_BY_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[MessagingRequestSummaryViewDTO.SearchByParameters.MESSAGE_ID] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + DBSearchParameters[MessagingRequestSummaryViewDTO.SearchByParameters.ORIGINAL_MESSAGE_ID] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.TRX_NUMBER )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.TRX_OTP)
                        {
                            query.Append(joiner + @"EXISTS " +
                                                        "(SELECT TransactionOTP FROM trx_header " +
                                                            "WHERE trx_no = mr.TrxNumber AND " + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.TRX_OTP_LIST)
                        {
                            query.Append(joiner + @"EXISTS " +
                                                        "(SELECT TransactionOTP FROM trx_header " +
                                                            "WHERE trx_no = mr.TrxNumber AND " + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") )");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.MESSAGE_TYPE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.FROM_DATE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",getdate()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingRequestSummaryViewDTO.SearchByParameters.TO_DATE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",getdate()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query + " order by CreationDate desc " + offsetQuery.ToString(); 
            }

            parameters.Add(new SqlParameter("@PassPhrase", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE")));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MessagingRequestSummaryViewDTO messagingRequestSummaryViewDTO = GetMessagingRequestSummaryViewDTO(dataRow);
                    messagingRequestSummaryViewDTOList.Add(messagingRequestSummaryViewDTO);
                }
            }
            log.LogMethodExit(messagingRequestSummaryViewDTOList);
            return messagingRequestSummaryViewDTOList;
        }
    }
}
