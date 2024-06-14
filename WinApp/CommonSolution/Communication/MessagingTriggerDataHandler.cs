/********************************************************************************************
 * Project Name - Communication
 * Description  - Data Handler object - MessagingTriggerDataHandler
 *
 **************
 ** Version Log
  **************
  * Version     Date        Modified By             Remarks
 *********************************************************************************************
 *2.70        11-Jun-2019   Girish Kundar           Created
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
    /// This is the MessagingTriggerDataHandler  object for Insert ,Update and Search for  MessagingTrigger Object
    /// </summary>
    public class MessagingTriggerDataHandler
    {
        private static readonly logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MessagingTriggers AS mt ";
        /// <summary>
        /// Dictionary for searching Parameters for the MessagingTrigger object.
        /// </summary>
        private static readonly Dictionary<MessagingTriggerDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MessagingTriggerDTO.SearchByParameters, string>
        {
            {MessagingTriggerDTO.SearchByParameters.TRIGGER_ID,"mt.TriggerId"},
            {MessagingTriggerDTO.SearchByParameters.ACTIVE_FLAG,"mt.ActiveFlag"},
            {MessagingTriggerDTO.SearchByParameters.EMAIL_SUBJECT,"mt.EmailSubject"},
            {MessagingTriggerDTO.SearchByParameters.MESSAGE_TYPE,"mt.MessageType"},
            {MessagingTriggerDTO.SearchByParameters.RECEIPT_TEMPLATE_ID,"mt.ReceiptTemplateId"},
            {MessagingTriggerDTO.SearchByParameters.SEND_RECEIPT,"mt.SendReceipt"},
            {MessagingTriggerDTO.SearchByParameters.TRIGGER_NAME,"mt.ExcludeFlag"},
            {MessagingTriggerDTO.SearchByParameters.TYPE_CODE,"mt.TypeCode"},
            {MessagingTriggerDTO.SearchByParameters.SITE_ID,"mt.site_id"},
            {MessagingTriggerDTO.SearchByParameters.MASTER_ENTITY_ID,"mt.MasterEntityId"}
        };
        /// <summary>
        /// Parameterized Constructor for MessagingTriggerDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public MessagingTriggerDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MessagingTrigger Record.
        /// </summary>
        /// <param name="messagingTriggerDTO">MessagingTriggerDTO object is passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site id </param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MessagingTriggerDTO messagingTriggerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingTriggerDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TriggerId", messagingTriggerDTO.TriggerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", messagingTriggerDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EmailIds", messagingTriggerDTO.EmailIds));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EmailSubject", messagingTriggerDTO.EmailSubject));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EmailTemplate", messagingTriggerDTO.EmailTemplate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessageCustomer", messagingTriggerDTO.MessageCustomer));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessageType", messagingTriggerDTO.MessageType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MinimumSaleAmount", messagingTriggerDTO.MinimumSaleAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MinimumTicketCount", messagingTriggerDTO.MinimumTicketCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReceiptTemplateId", messagingTriggerDTO.ReceiptTemplateId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SendReceipt", messagingTriggerDTO.SendReceipt));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SMSNumbers", messagingTriggerDTO.SMSNumbers));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SMSTemplate", messagingTriggerDTO.SMSTemplate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeStamp", messagingTriggerDTO.TimeStamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TriggerName", messagingTriggerDTO.TriggerName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TypeCode", messagingTriggerDTO.TypeCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VIPOnly", messagingTriggerDTO.VIPOnly));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", messagingTriggerDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to MessagingTriggerDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of MessagingTriggerDTO</returns>
        private MessagingTriggerDTO GetMessagingTriggerDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MessagingTriggerDTO messagingTriggerDTO = new MessagingTriggerDTO(dataRow["TriggerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TriggerId"]),
                                                         dataRow["TriggerName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TriggerName"]),
                                                         dataRow["TypeCode"] == DBNull.Value ? ' ' : Convert.ToChar(dataRow["TypeCode"]),
                                                         dataRow["ActiveFlag"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["ActiveFlag"]),
                                                         dataRow["VIPOnly"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["VIPOnly"]),
                                                         dataRow["MessageType"] == DBNull.Value ? (char?)null : Convert.ToChar(dataRow["MessageType"]),
                                                         dataRow["SMSTemplate"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SMSTemplate"]),
                                                         dataRow["EmailSubject"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EmailSubject"]),
                                                         dataRow["EmailTemplate"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EmailTemplate"]),
                                                         dataRow["MinimumSaleAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["MinimumSaleAmount"]),
                                                         dataRow["MinimumTicketCount"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["MinimumTicketCount"]),
                                                         dataRow["MessageCustomer"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["MessageCustomer"]),
                                                         dataRow["SMSNumbers"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SMSNumbers"]),
                                                         dataRow["EmailIds"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EmailIds"]),
                                                         dataRow["SendReceipt"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["SendReceipt"]),
                                                         dataRow["ReceiptTemplateId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ReceiptTemplateId"]),
                                                         dataRow["Timestamp"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Timestamp"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                        );
            log.LogMethodExit(messagingTriggerDTO);
            return messagingTriggerDTO;
        }
        /// <summary>
        /// Gets the MessagingTriggerDTO data of passed TriggerId 
        /// </summary>
        /// <param name="triggerId">triggerId  of MessagingTrigger</param>
        /// <returns>Returns MessagingTriggerDTO</returns>
        public MessagingTriggerDTO GetMessagingTriggerDTO(int triggerId)
        {
            log.LogMethodEntry(triggerId);
            MessagingTriggerDTO result = null;
            string query = SELECT_QUERY + @" WHERE mt.TriggerId = @TriggerId";
            SqlParameter parameter = new SqlParameter("@TriggerId", triggerId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMessagingTriggerDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        ///  Deletes the MessagingTrigger record
        /// </summary>
        /// <param name="messagingTriggerDTO">MessagingTriggerDTO is passed as parameter</param>
        internal void Delete(MessagingTriggerDTO messagingTriggerDTO)
        {
            log.LogMethodEntry(messagingTriggerDTO);
            string query = @"DELETE  
                             FROM MessagingTriggers
                             WHERE MessagingTriggers.TriggerId = @TriggerId";
            SqlParameter parameter = new SqlParameter("@TriggerId", messagingTriggerDTO.TriggerId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            messagingTriggerDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the MessagingTrigger Table.
        /// </summary>
        /// <param name="messagingTriggerDTO">MessagingTriggerDTO object is passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site id </param>
        /// <returns>MessagingTriggerCriteriaDTO</returns>
        public MessagingTriggerDTO Insert(MessagingTriggerDTO messagingTriggerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingTriggerDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[MessagingTriggers]
                           ([TriggerName],
                            [TypeCode],
                            [ActiveFlag],
                            [VIPOnly],
                            [MessageType],
                            [SMSTemplate],
                            [EmailSubject],
                            [EmailTemplate],
                            [MinimumSaleAmount],
                            [MinimumTicketCount],
                            [MessageCustomer],
                            [SMSNumbers],
                            [EmailIds],
                            [SendReceipt],
                            [ReceiptTemplateId],
                            [Timestamp],
                            [LastUpdatedDate],
                            [LastUpdatedBy],
                            [site_id],
                            [Guid],
                            [MasterEntityId],
                            [CreatedBy],
                            [CreationDate])
                     VALUES
                           (@TriggerName,
                            @TypeCode,
                            @ActiveFlag,
                            @VIPOnly,
                            @MessageType,
                            @SMSTemplate,
                            @EmailSubject,
                            @EmailTemplate,
                            @MinimumSaleAmount,
                            @MinimumTicketCount,
                            @MessageCustomer,
                            @SMSNumbers,
                            @EmailIds,
                            @SendReceipt,
                            @ReceiptTemplateId,
                            @Timestamp,
                            GETDATE(),
                            @LastUpdatedBy,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE())
                            SELECT * FROM MessagingTriggers WHERE TriggerId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(messagingTriggerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMessagingTriggerDTO(messagingTriggerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting MessagingTriggerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(messagingTriggerDTO);
            return messagingTriggerDTO;
        }

        /// <summary>
        ///  Updates the record to the MessagingTrigger Table.
        /// </summary>
        /// <param name="messagingTriggerDTO">MessagingTriggerDTO object is passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site id </param>
        /// <returns>MessagingTriggerDTO</returns>
        public MessagingTriggerDTO Update(MessagingTriggerDTO messagingTriggerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingTriggerDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[MessagingTriggers]
                           SET
                            [TriggerName]        = @TriggerName,
                            [TypeCode]           = @TypeCode,
                            [ActiveFlag]         = @ActiveFlag,
                            [VIPOnly]            = @VIPOnly,
                            [MessageType]        = @MessageType,
                            [SMSTemplate]        = @SMSTemplate,
                            [EmailSubject]       = @EmailSubject,
                            [EmailTemplate]      = @EmailTemplate,
                            [MinimumSaleAmount]  = @MinimumSaleAmount,
                            [MinimumTicketCount] = @MinimumTicketCount,
                            [MessageCustomer]    = @MessageCustomer,
                            [SMSNumbers]         = @SMSNumbers,
                            [EmailIds]           = @EmailIds,
                            [SendReceipt]        = @SendReceipt,
                            [ReceiptTemplateId]  = @ReceiptTemplateId,
                            [Timestamp]          = @TimeStamp,
                            [LastUpdatedDate]    = GETDATE(),
                            [LastUpdatedBy]      = @LastUpdatedBy,
                            --[site_id]            = @site_id,
                            [MasterEntityId]     = @MasterEntityId
                           WHERE TriggerId =@TriggerId
                              SELECT * FROM MessagingTriggers WHERE TriggerId = @TriggerId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(messagingTriggerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMessagingTriggerDTO(messagingTriggerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating MessagingTriggerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(messagingTriggerDTO);
            return messagingTriggerDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="messagingTriggerDTO">MessagingTriggerDTO object is passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="siteId"> site id </param>
        private void RefreshMessagingTriggerDTO(MessagingTriggerDTO messagingTriggerDTO, DataTable dt)
        {
            log.LogMethodEntry(messagingTriggerDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                messagingTriggerDTO.TriggerId = Convert.ToInt32(dt.Rows[0]["TriggerId"]);
                messagingTriggerDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                messagingTriggerDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                messagingTriggerDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                messagingTriggerDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                messagingTriggerDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                messagingTriggerDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);


            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of MessagingTriggerDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>returns the List of MssagingTriggerDTO</returns>
        public List<MessagingTriggerDTO> GetMessagingTriggerDTOList(List<KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<MessagingTriggerDTO> mssagingTriggerDTOList = new List<MessagingTriggerDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MessagingTriggerDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MessagingTriggerDTO.SearchByParameters.TRIGGER_ID
                            || searchParameter.Key == MessagingTriggerDTO.SearchByParameters.RECEIPT_TEMPLATE_ID
                            || searchParameter.Key == MessagingTriggerDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagingTriggerDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagingTriggerDTO.SearchByParameters.EMAIL_SUBJECT
                          || searchParameter.Key == MessagingTriggerDTO.SearchByParameters.TRIGGER_NAME
                          || searchParameter.Key == MessagingTriggerDTO.SearchByParameters.TYPE_CODE)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingTriggerDTO.SearchByParameters.SEND_RECEIPT) // bit

                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == MessagingTriggerDTO.SearchByParameters.ACTIVE_FLAG) // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == MessagingTriggerDTO.SearchByParameters.MESSAGE_TYPE) //char - to be checked 
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MessagingTriggerDTO mssagingTriggerDTO = GetMessagingTriggerDTO(dataRow);
                    mssagingTriggerDTOList.Add(mssagingTriggerDTO);
                }
            }
            log.LogMethodExit(mssagingTriggerDTOList);
            return mssagingTriggerDTOList;
        }
    }
}
