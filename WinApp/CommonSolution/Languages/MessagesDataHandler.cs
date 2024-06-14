/********************************************************************************************
 * Project Name - Messages Data Handler
 * Description  - Messages handler of the Messages class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        04-Jan-2017   Vinayaka V          Created 
 *2.60        06-May-2019   Mushahid Faizan     Added Insert/update and GetSQLParameters() method.
                                                Added GetAllMessages(),GetMessagesDTO and GetAllMessages() method.
 *2.70.2       19-Jul -2019  Girish Kundar      Modified : Added Logger methods and Passed SqlTransaction object for Query execution
 *             29-Jul-2019   Mushahid Faizan    Added IsActive Column.
 *             16-Aug-2019   Mushahid Faizan    Added GetMaxMessageNo(),GetMessagesCount() method.
 *2.70.2       05-Dec-2019   Jinto Thomas        Removed siteid from update query
 *2.80         01-Jan-2020   Mushahid Faizan    Modified GetLabelMessageDictionary() for Customer Registration Changes.
 *2.70.3       02-Apr-2020   Girish Kundar       Modified : WMS issue fix for search message by number
 *2.80         20-Apr-2020   Mushahid Faizan    Modified : Added Literal Search Param in GetAllMessages()
                                                          Enhancement of localization to get all localization at one time.
 *********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Languages
{
    /// <summary>
    /// Messages Data Handler - Handles insert, update and select of Messages Data
    /// </summary>
    public class MessagesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private static readonly Dictionary<MessagesDTO.SearchByMessageParameters, string> DBSearchParameters = new Dictionary<MessagesDTO.SearchByMessageParameters, string>
        {
                {MessagesDTO.SearchByMessageParameters.MESSAGE_ID, "mes.MessageId"},
                {MessagesDTO.SearchByMessageParameters.MESSAGE_NO, "mes.MessageNo"},
                {MessagesDTO.SearchByMessageParameters.MESSAGE, "mes.Message"},
                {MessagesDTO.SearchByMessageParameters.SITE_ID, "mes.Site_Id"},
                {MessagesDTO.SearchByMessageParameters.MASTER_ENTITY_ID, "mes.MasterEntityId"},
                {MessagesDTO.SearchByMessageParameters.LITERALS_ONLY, "mes.MessageNo"},
                {MessagesDTO.SearchByMessageParameters.MESSAGES_ONLY, "mes.MessageNo"},
                {MessagesDTO.SearchByMessageParameters.LANGUAGE_ID, "trnmes.LanguageId"},
                
        };


        string MESSEGES_SELECT_QUERY = @"select * from Messages mes ";

        /// <summary>
        /// Default constructor of MessagesDataHandler class
        /// </summary>
        public MessagesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// GetTranslatedMessageList
        /// </summary>
        /// <param name="messageParams">messageParams</param>
        /// <returns>returns List of TranslatedMessage type message as result</returns>
        public List<TranslatedMessage> GetTranslatedMessageList(MessageParams messageParams)
        {
            log.LogMethodEntry(messageParams);

            List<TranslatedMessage> translatedMessageList = new List<TranslatedMessage>();
            string queryToFindTranslatedMessage;
            queryToFindTranslatedMessage = @"select isnull(trnmes.message, mes.message) translated_message, mes.message original_message, mes.messageId message_id 
                                                   from messages as mes inner join MessagesTranslated as trnmes on trnmes.messageId = mes.messageId 
                                                   where mes.messageId = @messageId 
                                                   and trnmes.LanguageId = @language
                                                   and (mes.site_id is null or mes.site_id = @siteId)";

            foreach (int messageId in messageParams.MessageKeysList)
            {
                List<SqlParameter> getMessageParameters = new List<SqlParameter>();
                getMessageParameters.Add(new SqlParameter("@language", messageParams.LanguageId));
                getMessageParameters.Add(new SqlParameter("@messageId", messageId));
                getMessageParameters.Add(new SqlParameter("@siteId", messageParams.Site_Id));

                DataTable dtTranslatedMessage = dataAccessHandler.executeSelectQuery(queryToFindTranslatedMessage, getMessageParameters.ToArray(), sqlTransaction);

                if (dtTranslatedMessage.Rows.Count > 0)
                    translatedMessageList.Add(new TranslatedMessage(Convert.ToInt32(dtTranslatedMessage.Rows[0]["message_id"]), dtTranslatedMessage.Rows[0]["original_message"].ToString(), dtTranslatedMessage.Rows[0]["translated_message"].ToString()));
                else
                    translatedMessageList.Add(new TranslatedMessage(messageId, "", ""));
            }

            log.LogMethodExit(translatedMessageList);
            return translatedMessageList;
        }


        /// <summary>
        /// GetMessageFromMessageKeyList
        /// </summary>
        /// <param name="messageParams">messageParams</param>
        /// <returns>returns array of TranslatedMessage type message as result</returns>
        public TranslatedMessage[] GetMessageFromMessageKeyList(MessageParams messageParams)
        {
            log.LogMethodEntry(messageParams);

            List<TranslatedMessage> translatedMessageList = new List<TranslatedMessage>();

            string queryToFindTranslatedMessage;
            queryToFindTranslatedMessage = @"select isnull(trnmes.message, mes.message) translated_message, mes.message original_message, mes.messageId message_id 
                                               from messages as mes inner join MessagesTranslated as trnmes on trnmes.MessageId = mes.MessageId 
                                               where mes.message = @messageId 
                                               and trnmes.LanguageId = @language
                                               and (mes.site_id is null or mes.site_id = @siteId)";

            foreach (string messageKey in messageParams.MessageKeyList)
            {
                List<SqlParameter> getMessageParameters = new List<SqlParameter>();
                getMessageParameters.Add(new SqlParameter("@language", messageParams.LanguageId));
                getMessageParameters.Add(new SqlParameter("@siteId", messageParams.Site_Id));
                getMessageParameters.Add(new SqlParameter("@messageId", messageKey));

                DataTable dtTranslatedMessage = dataAccessHandler.executeSelectQuery(queryToFindTranslatedMessage, getMessageParameters.ToArray(), sqlTransaction);

                if (dtTranslatedMessage.Rows.Count > 0)
                    translatedMessageList.Add(new TranslatedMessage(Convert.ToInt32(dtTranslatedMessage.Rows[0]["message_id"]), dtTranslatedMessage.Rows[0]["original_message"].ToString(), dtTranslatedMessage.Rows[0]["translated_message"].ToString()));
                else
                    translatedMessageList.Add(new TranslatedMessage(-1, messageKey, ""));
            }

            log.LogMethodExit(translatedMessageList);
            return translatedMessageList.ToArray();
        }

        /// <summary>
        /// returns the dictionary of label message where key is the message and value is the translated message
        /// </summary>
        /// <param name="languageId">language identifier</param>
        /// <returns></returns>
        public ConcurrentDictionary<string, string> GetLabelMessageDictionary(int languageId, int siteId)
        {
            log.LogMethodEntry(languageId);
            ConcurrentDictionary<string, string> labelMessageDictionary = new ConcurrentDictionary<string, string>();
            string query = @"select m.Message as Message, isnull(tl.Message, m.Message) as TranslatedMessage
                            from Messages m left outer join MessagesTranslated tl
                                on m.MessageId = tl.MessageId
                                and tl.LanguageId = @langId
                            where m.MessageNo >= 10000 and (m.site_id = @siteId or @siteId = -1)";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@langId", languageId));
            parameters.Add(new SqlParameter("@siteId", siteId));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string message = Convert.ToString(dataTable.Rows[i]["Message"]);
                    string translatedMessage = dataTable.Rows[i]["TranslatedMessage"] == DBNull.Value ? "" : Convert.ToString(dataTable.Rows[i]["TranslatedMessage"]);
                    labelMessageDictionary[message] = translatedMessage;
                }
            }
            log.LogMethodExit(labelMessageDictionary);
            return labelMessageDictionary;
        }

        /// <summary>
        /// returns the message dictionary where key is the message no and value is translated message
        /// </summary>
        /// <param name="languageId">language identifier</param>
        /// <param name="messageNo">message identifier</param>
        /// <returns></returns>
        public string GetTranslatedMessage(int languageId, int messageNo)
        {
            log.LogMethodEntry(languageId);
            string translatedMessage = string.Empty;
            string query = @"select isnull(tl.Message, m.Message)
                            from Messages m left outer join MessagesTranslated tl
                                on m.MessageId = tl.MessageId
                                and tl.LanguageId = @langId
                            where m.MessageNo = @messageNo";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@langId", languageId));
            parameters.Add(new SqlParameter("@messageNo", messageNo));
            Object translatedMessageObject = dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            if (translatedMessageObject != null && translatedMessageObject != DBNull.Value)
            {
                translatedMessage = Convert.ToString(translatedMessageObject);
            }
            log.LogMethodExit(translatedMessage);
            return translatedMessage;
        }
        /// <summary>
        /// Converts the Data row object to MessagesDTO class type
        /// </summary>
        /// <param name="MessagesDTO">MessagesDTO </param>
        /// <returns>Returns MessagesDTO</returns>
        public MessagesDTO GetMessagesDTO(DataRow messagesDataRow)
        {
            log.LogMethodEntry(messagesDataRow);
            MessagesDTO messagesDTO = new MessagesDTO(
                                                    messagesDataRow["MessageId"] == DBNull.Value ? -1 : Convert.ToInt32(messagesDataRow["MessageId"]),
                                                    messagesDataRow["MessageNo"] == DBNull.Value ? -1 : Convert.ToInt32(messagesDataRow["MessageNo"]),
                                                    messagesDataRow.Table.Columns.Contains("trnmes.Message") ? messagesDataRow["trnmes.Message"].ToString() : messagesDataRow["Message"].ToString(),
                                                    messagesDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(messagesDataRow["site_id"]),
                                                    messagesDataRow["Guid"].ToString(),
                                                    messagesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(messagesDataRow["SynchStatus"]),
                                                    messagesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(messagesDataRow["MasterEntityId"]),
                                                    messagesDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(messagesDataRow["IsActive"]),
                                                    messagesDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(messagesDataRow["CreatedBy"]),
                                                    messagesDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(messagesDataRow["CreationDate"]),
                                                    messagesDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(messagesDataRow["LastUpdatedBy"]),
                                                    messagesDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(messagesDataRow["LastUpdateDate"])
                                                     );
            log.LogMethodExit(messagesDataRow);
            return messagesDTO;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Messages Record.
        /// </summary>
        /// <param name="messagesDTO">messagesDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MessagesDTO messagesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessageId", messagesDTO.MessageId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessageNo", messagesDTO.MessageNo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Message", messagesDTO.Message));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", messagesDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", messagesDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", messagesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", messagesDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the MessagesDTO record to the database
        /// </summary>
        /// <param name="MessagesDTO">MessagesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns MessagesDTO</returns>
        public MessagesDTO InsertMessages(MessagesDTO messagesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagesDTO, loginId, siteId);
            string query = @"INSERT INTO Messages 
                                        ( 
                                            MessageNo,
                                            Message,
                                            site_id,
                                            Guid,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            IsActive
                                        ) 
                                VALUES 
                                        (
                                            @MessageNo,
                                            @Message,
                                            @site_id,
                                            NEWID(),
                                            @MasterEntityId,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @isActive
                                        ) SELECT * FROM Messages WHERE MessageId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(messagesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMessagesDTO(messagesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting messagesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(messagesDTO);
            return messagesDTO;
        }

        internal DateTime? GetMessageModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate
                            FROM (
                            select max(LastUpdateDate) LastUpdatedDate from Languages WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from Messages WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from MessagesTranslated WHERE (site_id = @siteId or @siteId = -1)) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Updates the messagesDTO record
        /// </summary>
        /// <param name="messagesDTO">messagesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the MessagesDTO</returns>
        public MessagesDTO UpdateMessages(MessagesDTO messagesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagesDTO, loginId, siteId);
            string query = @"UPDATE [Messages] SET 
                            [MessageNo] = @MessageNo, 
                            [Message] = @Message,
                           -- [site_id] = @site_id,
                            [Guid] = @Guid,
                            [MasterEntityId] = @MasterEntityId,
                            LastUpdatedBy= @LastUpdatedBy,
                            LastUpdateDate = GetDate(),
                            [IsActive] = @isActive
                            WHERE (MessageId = @MessageId)  
                            SELECT * FROM Messages WHERE MessageId = @MessageId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(messagesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMessagesDTO(messagesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating messagesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(messagesDTO);
            return messagesDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="messagesDTO">MessagesDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshMessagesDTO(MessagesDTO messagesDTO, DataTable dt)
        {
            log.LogMethodEntry(messagesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                messagesDTO.MessageId = Convert.ToInt32(dt.Rows[0]["MessageId"]);
                messagesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                messagesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                messagesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                messagesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                messagesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                messagesDTO.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the MessagesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MessagesDTO matching the search criteria</returns>
        public List<MessagesDTO> GetAllMessages(List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectLanguagesQuery = MESSEGES_SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MessagesDTO.SearchByMessageParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperator = (count == 0) ? " " : " and ";
                        if (searchParameter.Key.Equals(MessagesDTO.SearchByMessageParameters.MESSAGE_ID)
                            || searchParameter.Key.Equals(MessagesDTO.SearchByMessageParameters.MESSAGE_NO)
                            || searchParameter.Key.Equals(MessagesDTO.SearchByMessageParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagesDTO.SearchByMessageParameters.SITE_ID)
                        {
                            query.Append(joinOperator + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagesDTO.SearchByMessageParameters.LITERALS_ONLY)
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + ">= 10000 ");
                        }
                        else if (searchParameter.Key == MessagesDTO.SearchByMessageParameters.MESSAGES_ONLY)
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + "< 10000 ");
                        }
                        else if(searchParameter.Key == MessagesDTO.SearchByMessageParameters.LANGUAGE_ID)
                        {
                            selectLanguagesQuery += " left outer join MessagesTranslated as trnmes on trnmes.messageId = mes.messageId ";
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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

                if (searchParameters.Count > 0)
                    selectLanguagesQuery = selectLanguagesQuery + query;
            }

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectLanguagesQuery, parameters.ToArray(), sqlTransaction);
            List<MessagesDTO> messageDTOList = new List<MessagesDTO>();
            if (dataTable.Rows.Count > 0)
            {

                foreach (DataRow messagesDataRow in dataTable.Rows)
                {
                    MessagesDTO messagesObject = GetMessagesDTO(messagesDataRow);
                    messageDTOList.Add(messagesObject);
                }
                log.LogMethodExit(messageDTOList);
                return messageDTOList;
            }
            else
            {
                log.LogMethodExit(messageDTOList);
                return messageDTOList;
            }
        }
        /// This method is used to filter the messages based on search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public string GetMessagesFilterQuery(List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                foreach (KeyValuePair<MessagesDTO.SearchByMessageParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? " " : " and ";
                        if (searchParameter.Key.Equals(MessagesDTO.SearchByMessageParameters.MESSAGE_ID) || searchParameter.Key.Equals(MessagesDTO.SearchByMessageParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value);
                        }
                        else if (searchParameter.Key == MessagesDTO.SearchByMessageParameters.MESSAGE_NO)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "<" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == MessagesDTO.SearchByMessageParameters.LITERALS_ONLY)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + ">" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == MessagesDTO.SearchByMessageParameters.SITE_ID)
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " = -1)");
                        }
                        else if (searchParameter.Key == MessagesDTO.SearchByMessageParameters.MESSAGE)
                        {
                            double msgno;
                            if (double.TryParse(searchParameter.Value, out msgno))
                                query.Append(joinOperartor + "convert(varchar, MessageNo) like '" + searchParameter.Value + "%'");
                            else
                                query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " like '%" + searchParameter.Value + "%'");
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit();
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
            }
            log.LogMethodExit(query);
            return query.ToString();
        }

        /// <summary>
        /// Gets the MessagesDTO list matching the search Parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="currentPage">current page number</param>
        /// <param name="pageSize">page size</param>
        /// <returns>Returns the list of MessagesDTO matching the searchParameters</returns>
        public List<MessagesDTO> GetMessagesDTOList(List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>> searchParameters, int currentPage, int pageSize)
        {
            log.LogMethodEntry(searchParameters, currentPage, pageSize);
            List<MessagesDTO> messagesList = null;

            string selectQuery = MESSEGES_SELECT_QUERY;
            selectQuery += GetMessagesFilterQuery(searchParameters);
            selectQuery += " ORDER BY mes.MessageNo OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
            selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                messagesList = new List<MessagesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MessagesDTO messagesDTO = GetMessagesDTO(dataRow);
                    messagesList.Add(messagesDTO);
                }
            }
            log.LogMethodExit(messagesList);
            return messagesList;
        }
        /// <summary>
        /// Returns the no of messsagesCount matching the search Parameters
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>no of messsagesCount matching the searchParameters</returns>
        public int GetMessagesCount(List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int messsagesCount = 0;
            string selectQuery = MESSEGES_SELECT_QUERY;
            selectQuery += GetMessagesFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                messsagesCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(messsagesCount);
            return messsagesCount;
        }
        /// <summary>
        ///  Returns the Max Message no matching the searchParameters
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public int GetMaxMessageNo(List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int maxMessagesNo = 0;

            string selectQuery = @"select Max(MessageNo) as MaxMessageNo from Messages mes";
            selectQuery += GetMessagesFilterQuery(searchParameters);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                maxMessagesNo = Convert.ToInt32(dataTable.Rows[0]["MaxMessageNo"]);
            }
            log.LogMethodExit(maxMessagesNo);
            return maxMessagesNo;
        }

        /// <summary>
        ///  Deletes the Messages record based on MessageId 
        /// </summary>
        /// <param name="messageId">id is passed as parameter</param>
        internal void Delete(int messageId)
        {
            log.LogMethodEntry(messageId);
            string query = @"DELETE  
                             FROM Messages
                             WHERE Messages.MessageId = @messageId";
            SqlParameter parameter = new SqlParameter("@messageId", messageId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
    }
}