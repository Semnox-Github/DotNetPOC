/********************************************************************************************
 * Project Name - Communication
 * Description  - Data Handler object -MessagingRequestDataHandler
 *
 **************
 ** Version Log
  **************
  * Version     Date        Modified By             Remarks
 *********************************************************************************************
 *2.70        11-Jun-2019   Girish Kundar           Created
 *2.70.3      04-Feb-2020      Nitin Pai           Guest App phase 2 changes
 *2.100.0     10-Sep-2020     Jinto Thomas         Add messaging client id to table
 *2.100.0     15-Sep-2020     Nitin Pai            Push Notification: Added ToDevice (token), MessageType, MessageRead
 *2.150.0     26-Nov-2021      Deeksha                 Modified to Add country Code field.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// MessagingRequestDataHandler object for Insert ,Update and Search for MessagingRequest Object
    /// </summary>
    public class MessagingRequestDataHandler
    {
        private static readonly logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MessagingRequests AS mrq ";
        /// <summary>
        /// Dictionary for searching Parameters for the MessagingRequest object.
        /// </summary>
        private static readonly Dictionary<MessagingRequestDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MessagingRequestDTO.SearchByParameters, string>
        {
            {MessagingRequestDTO.SearchByParameters.ID,"mrq.Id"},
            {MessagingRequestDTO.SearchByParameters.CARD_ID,"mrq.card_id"},
            {MessagingRequestDTO.SearchByParameters.BATCH_ID,"mrq.BatchId"},
            {MessagingRequestDTO.SearchByParameters.CUSTOMER_ID,"mrq.CustomerId"},
            {MessagingRequestDTO.SearchByParameters.REFERENCE,"mrq.Reference"},
            {MessagingRequestDTO.SearchByParameters.STATUS,"mrq.Status"},
            {MessagingRequestDTO.SearchByParameters.SUBJECT,"mrq.Subject"},
            {MessagingRequestDTO.SearchByParameters.SEND_DATE,"mrq.SendDate"},
            {MessagingRequestDTO.SearchByParameters.SITE_ID,"mrq.site_id"},
            {MessagingRequestDTO.SearchByParameters.MASTER_ENTITY_ID,"mrq.MasterEntityId"},
            {MessagingRequestDTO.SearchByParameters.ACTIVE_FLAG,"mrq.ActiveFlag"},
            //{MessagingRequestDTO.SearchByParameters.MESSAGING_CLIENT_NAME,"mrq.MessagingClientName"},
            {MessagingRequestDTO.SearchByParameters.MESSAGING_CLIENT_ID,"mrq.MessagingClientId"},
            {MessagingRequestDTO.SearchByParameters.ATTEMPT_LESS_THAN,"mrq.Attempts"},
            {MessagingRequestDTO.SearchByParameters.STATUS_NOT_EQ,"mrq.Status"},
            {MessagingRequestDTO.SearchByParameters.MESSAGE_TYPE,"mrq.MessageType"},
            {MessagingRequestDTO.SearchByParameters.MESSAGE_READ,"mrq.MessageRead"},
            {MessagingRequestDTO.SearchByParameters.FROM_DATE,"mrq.SendDate"},
            {MessagingRequestDTO.SearchByParameters.TO_DATE,"mrq.SendDate"},
            {MessagingRequestDTO.SearchByParameters.SIGNED_IN_CUSTOMERS_ONLY,"mrq.SignedInCustomersOnly"},
            {MessagingRequestDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID,"mrq.ParafaitFunctionEventId"},
            {MessagingRequestDTO.SearchByParameters.ID_LIST,"mrq.Id"}
        };

        /// <summary>
        /// Parameterized Constructor for MessagingRequestDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public MessagingRequestDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating messagingRequest Record.
        /// </summary>
        /// <param name="messagingRequestDTO">MessagingRequestDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site Id </param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MessagingRequestDTO messagingRequestDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingRequestDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", messagingRequestDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", messagingRequestDTO.CustomerId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttachFile", messagingRequestDTO.AttachFile));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attempts", messagingRequestDTO.Attempts));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BatchId", messagingRequestDTO.BatchId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Body", messagingRequestDTO.Body));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", messagingRequestDTO.CardId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessageType", messagingRequestDTO.MessageType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Reference", messagingRequestDTO.Reference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SendAttemptDate", messagingRequestDTO.SendAttemptDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SendDate", messagingRequestDTO.SendDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Status", messagingRequestDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StatusMessage", messagingRequestDTO.StatusMessage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Subject", messagingRequestDTO.Subject));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToEmail", messagingRequestDTO.ToEmail));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToMobile", messagingRequestDTO.ToMobile));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToDevice", messagingRequestDTO.ToDevice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", messagingRequestDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", messagingRequestDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Cc", messagingRequestDTO.Cc));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Bcc", messagingRequestDTO.Bcc));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessagingClientName", messagingRequestDTO.MessagingClientName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessagingClientId", messagingRequestDTO.MessagingClientId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessageRead", messagingRequestDTO.MessageRead));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SignedInCustomersOnly", messagingRequestDTO.SignedInCustomersOnly));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CountryCode", messagingRequestDTO.CountryCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxNumber", messagingRequestDTO.TrxNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParafaitFunctionEventId", messagingRequestDTO.ParafaitFunctionEventId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OriginalMessageId", messagingRequestDTO.OriginalMessageId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", messagingRequestDTO.TrxId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to MessagingRequestDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>MessagingRequestDTO</returns>
        private MessagingRequestDTO GetMessagingRequestDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                         dataRow["BatchId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["BatchId"]),
                                                         dataRow["Reference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Reference"]),
                                                         dataRow["MessageType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["MessageType"]),  // To be checked for default message Type
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
                                                         true,
                                                         dataRow["Cc"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Cc"]),
                                                         dataRow["Bcc"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Bcc"]),
                                                         dataRow["MessagingClientId"] == DBNull.Value ?-1 : Convert.ToInt32(dataRow["MessagingClientId"]),
                                                         dataRow["MessageRead"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["MessageRead"]),
                                                         dataRow["ToDevice"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ToDevice"]),
                                                          dataRow["SignedInCustomersOnly"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["SignedInCustomersOnly"]),
                                                         dataRow["CountryCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CountryCode"]),
                                                         dataRow["TrxNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TrxNumber"]),
                                                         dataRow["ParafaitFunctionEventId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ParafaitFunctionEventId"]),
                                                         dataRow["OriginalMessageId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["OriginalMessageId"]),
                                                         dataRow["TrxId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["TrxId"])
                                                        );
            log.LogMethodExit(messagingRequestDTO);
            return messagingRequestDTO;
        }

        /// <summary>
        /// Gets the MessagingRequestDTO data of passed id 
        /// </summary>
        /// <param name="id">id -MessagingRequestId </param>
        /// <returns>Returns MessagingRequestDTO</returns>
        public MessagingRequestDTO GetMessagingRequest(int id)
        {
            log.LogMethodEntry(id);
            MessagingRequestDTO result = null;
            string query = SELECT_QUERY + @" WHERE mrq.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMessagingRequestDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        ///  Inserts the record to the MessagingRequest Table.
        /// </summary>
        /// <param name="messagingRequestDTO">MessagingRequestDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site Id </param>
        /// <returns>MessagingRequestDTO</returns>
        public MessagingRequestDTO Insert(MessagingRequestDTO messagingRequestDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingRequestDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[MessagingRequests]
                           ([BatchId],
                            [Reference],
                            [MessageType],
                            [ToEmails],
                            [ToMobile],
                            [Status],
                            [StatusMessage],
                            [SendDate],
                            [SendAttemptDate],
                            [Attempts],
                            [Subject],
                            [Body],
                            [CustomerId],
                            [Guid],
                            [site_id],
                            [AttachFile],
                            [card_id],
                            [MasterEntityId],
                            [CreatedBy],
                            [CreationDate],
                            [LastUpdatedBy],
                            [LastUpdateDate],
                            [Cc],
                            [Bcc],
                            [MessagingClientId],
                            [MessageRead],
                            [ToDevice],
                             [SignedInCustomersOnly],
                            [CountryCode],
                            [TrxNumber],
                            [ParafaitFunctionEventId],
                            [OriginalMessageId],
                            [TrxId])
                     VALUES
                           (@BatchId,
                            @Reference,
                            @MessageType,
                            @ToEmail,
                            @ToMobile,
                            @Status,
                            @StatusMessage,
                            @SendDate,
                            @SendAttemptDate,
                            @Attempts,
                            @Subject,
                            @Body,
                            @CustomerId,
                            NEWID(),
                            @siteId,
                            @AttachFile,
                            @cardId,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(),
                            @Cc,
                            @Bcc,
                            @MessagingClientId,
                            0,
                            @ToDevice,
                            @SignedInCustomersOnly,
                            @CountryCode,
                            @TrxNumber,
                            @ParafaitFunctionEventId,
                            @OriginalMessageId,
                            @TrxId)
                            SELECT * FROM MessagingRequests WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(messagingRequestDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMessagingRequestDTO(messagingRequestDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting MessagingRequestDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(messagingRequestDTO);
            return messagingRequestDTO;
        }

        /// <summary>
        ///  Updates the record to the MessagingRequest Table.
        /// </summary>
        /// <param name="messagingRequestDTO">MessagingRequestDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site Id </param>
        /// <returns>MessagingRequestDTO</returns>
        public MessagingRequestDTO Update(MessagingRequestDTO messagingRequestDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingRequestDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[MessagingRequests]
                           SET
                            [BatchId]         = @BatchId,
                            [Reference]       = @Reference,
                            [MessageType]     = @MessageType,
                            [ToEmails]        = @ToEmail,
                            [ToMobile]        = @ToMobile,
                            [Status]          = @Status,
                            [StatusMessage]   = @StatusMessage,
                            [SendDate]        = @SendDate,
                            [SendAttemptDate] = @SendAttemptDate,
                            [Attempts]        = @Attempts,
                            [Subject]         = @Subject,
                            [Body]            = @Body,
                            [CustomerId]      = @CustomerId,
                            --[site_id]         = @siteId,
                            [AttachFile]      = @AttachFile,
                            [card_id]         = @cardId,
                            [MasterEntityId]  = @MasterEntityId,
                            [LastUpdatedBy]   = @LastUpdatedBy,
                            [LastUpdateDate] =  GETDATE(),
                            [Cc]             = @Cc,
                            [Bcc]            = @Bcc,
                            [MessagingClientId] = @MessagingClientId,
                            [MessageRead]    =  @MessageRead,
                            [ToDevice] = @ToDevice,
                            [SignedInCustomersOnly] = @SignedInCustomersOnly,
                            [CountryCode]  = @CountryCode,
                            [TrxNumber]  = @TrxNumber,
                            [ParafaitFunctionEventId]  = @ParafaitFunctionEventId,
                            [OriginalMessageId] = @OriginalMessageId,
                            [TrxId] = @TrxId
                            WHERE Id = @Id;
                            SELECT * FROM MessagingRequests WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(messagingRequestDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMessagingRequestDTO(messagingRequestDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating MessagingRequestDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(messagingRequestDTO);
            return messagingRequestDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="messagingRequestDTO">MessagingRequestDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site Id </param>
        private void RefreshMessagingRequestDTO(MessagingRequestDTO messagingRequestDTO, DataTable dt)
        {
            log.LogMethodEntry(messagingRequestDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                messagingRequestDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                messagingRequestDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                messagingRequestDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                messagingRequestDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                messagingRequestDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                messagingRequestDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                messagingRequestDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of MessagingRequestDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">Search Parameters</param>
        /// <returns>Returns the List of MessagingRequestDTO</returns>
        public List<MessagingRequestDTO> GetMessagingRequestDTOList(List<KeyValuePair<MessagingRequestDTO.SearchByParameters, string>> searchParameters, int pageNumber = 0, int pageSize = 500)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<MessagingRequestDTO> messagingRequestDTOList = new List<MessagingRequestDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MessagingRequestDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MessagingRequestDTO.SearchByParameters.ID 
                            || searchParameter.Key == MessagingRequestDTO.SearchByParameters.CUSTOMER_ID
                            || searchParameter.Key == MessagingRequestDTO.SearchByParameters.BATCH_ID
                            || searchParameter.Key == MessagingRequestDTO.SearchByParameters.CARD_ID
                            || searchParameter.Key == MessagingRequestDTO.SearchByParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == MessagingRequestDTO.SearchByParameters.MESSAGING_CLIENT_ID
                            || searchParameter.Key == MessagingRequestDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key == MessagingRequestDTO.SearchByParameters.ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingRequestDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagingRequestDTO.SearchByParameters.REFERENCE
                            || searchParameter.Key == MessagingRequestDTO.SearchByParameters.STATUS
                            || searchParameter.Key == MessagingRequestDTO.SearchByParameters.SUBJECT)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingRequestDTO.SearchByParameters.ACTIVE_FLAG 
                                || searchParameter.Key == MessagingRequestDTO.SearchByParameters.SIGNED_IN_CUSTOMERS_ONLY) // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == MessagingRequestDTO.SearchByParameters.ATTEMPT_LESS_THAN)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0) <" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingRequestDTO.SearchByParameters.STATUS_NOT_EQ)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'x') not in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingRequestDTO.SearchByParameters.SEND_DATE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",getdate()) <= getdate()");
                        }
                        else if (searchParameter.Key == MessagingRequestDTO.SearchByParameters.FROM_DATE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",getdate()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingRequestDTO.SearchByParameters.TO_DATE)
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
                selectQuery = selectQuery + query;

                String offsetQuery = "";
                if (pageSize > -1 && (pageNumber * pageSize) >= 0)
                {
                    offsetQuery = " ORDER BY ID OFFSET " + pageNumber * pageSize + " ROWS FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
                }
                selectQuery = selectQuery + offsetQuery;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MessagingRequestDTO messagingRequestDTO = GetMessagingRequestDTO(dataRow);
                    messagingRequestDTOList.Add(messagingRequestDTO);
                }
            }
            log.LogMethodExit(messagingRequestDTOList);
            return messagingRequestDTOList;
        }
    }
}
