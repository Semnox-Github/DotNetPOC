/********************************************************************************************
 * Project Name - Messages
 * Description  - Business logic of Messages
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        04-Jan-2017   Vinayaka V       Created 
 *2.60        06-May-2019   Mushahid Faizan  Added new Class MessageListBL, Save() method and Constructor.
              16-Aug-2019   Mushahid Faizan  Added GetMaxMessageNo(),GetMessagesCount() method.
 *2.70.2        19-Jul -2019  Girish Kundar    Modified :  Save() method and Added Logger methods and Passed SqlTransaction object for Query execution
 *            16-Aug-2019   Mushahid Faizan  Added GetMaxMessageNo(),GetMessagesCount() method.
 *2.80        01-Jan-2020   Mushahid Faizan    Modified GetLabelMessageDictionary() for Customer Registration Changes.
 *2.80        13-May-2020   Mushahid Faizan    Enhancement of localization to get all localization at one time.
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Languages
{
    /// <summary>
    /// Messages allows to access the Message details based on the business logic.
    /// </summary>
    public class Messages
    {

        private MessagesDTO messagesDTO = null;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public Messages(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.messagesDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Messages object using the messagesDTO
        /// </summary>
        public Messages(ExecutionContext executionContext, MessagesDTO messagesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, messagesDTO);
            this.executionContext = executionContext;
            this.messagesDTO = messagesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Messages
        /// Checks if the  MessageId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            MessagesDataHandler messagesDataHandler = new MessagesDataHandler(sqlTransaction);
            if (messagesDTO.IsActive)
            {
                if (messagesDTO.MessageId < 0)
                {
                    messagesDTO = messagesDataHandler.InsertMessages(messagesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    messagesDTO.AcceptChanges();
                }
                else
                {
                    if (messagesDTO.IsChanged)
                    {
                        messagesDTO = messagesDataHandler.UpdateMessages(messagesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        messagesDTO.AcceptChanges();
                    }
                }
                if (messagesDTO.TranslatedMessageList != null && messagesDTO.TranslatedMessageList.Count != 0)
                {
                    foreach (TranslatedMessageDTO translatedMessageDTO in messagesDTO.TranslatedMessageList)
                    {
                        if (translatedMessageDTO.IsChanged)
                        {
                            translatedMessageDTO.MessageId = messagesDTO.MessageId;
                            TranslatedMessageBL translatedMessage = new TranslatedMessageBL(executionContext, translatedMessageDTO);
                            translatedMessage.Save(sqlTransaction);
                        }
                    }
                }
            }
            else
            {
                if (messagesDTO.MessageId >= 0)
                {
                    messagesDataHandler.Delete(messagesDTO.MessageId);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// GetTranslatedMessageList
        /// </summary>
        /// <param name="messageParams">messageParams</param>
        /// <returns>returns List of TranslatedMessage type message as result</returns>
        public List<TranslatedMessage> GetTranslatedMessageList(MessageParams messageParams, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(messageParams);

            List<TranslatedMessage> translatedMessageList = new List<TranslatedMessage>();
            MessagesDataHandler messageDatahandler = new MessagesDataHandler(sqlTransaction);
            if (messageParams.MessageKeyList != null)
            {
                var translatedMessageArray = messageDatahandler.GetMessageFromMessageKeyList(messageParams);
                if (translatedMessageArray != null)
                {
                    translatedMessageList = new List<TranslatedMessage>(translatedMessageArray);
                }
            }
            else if (messageParams.MessageKeysList != null)
            {
                translatedMessageList = messageDatahandler.GetTranslatedMessageList(messageParams);
            }
            log.LogMethodExit(translatedMessageList);

            return translatedMessageList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public MessagesDTO MessagesDTO
        {
            get
            {
                return messagesDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of messages
    /// </summary>
    public class MessageListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<MessagesDTO> messagesList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MessageListBL()
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public MessageListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.messagesList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="messagesList"></param>
        public MessageListBL(ExecutionContext executionContext, List<MessagesDTO> messagesList)
        {
            log.LogMethodEntry(executionContext, messagesList);
            this.executionContext = executionContext;
            this.messagesList = messagesList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the messages dictionary where key is language id and value is the message.
        /// </summary>
        /// <returns></returns>
        public string GetTranslatedMessage(int languageId, int messageNo, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(languageId, messageNo, sqlTransaction);
            MessagesDataHandler messagesDataHandler = new MessagesDataHandler(sqlTransaction);
            string translatedMessage = messagesDataHandler.GetTranslatedMessage(languageId, messageNo);
            log.LogMethodExit(translatedMessage);
            return translatedMessage;
        }

        /// <summary>
        /// Returns the messages dictionary where key is language id and value is the message.
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<string, string> GetLabelMessageDictionary(int languageId, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(languageId, sqlTransaction);
            MessagesDataHandler messagesDataHandler = new MessagesDataHandler(sqlTransaction);
            ConcurrentDictionary<string, string> labelMessageDictionary = messagesDataHandler.GetLabelMessageDictionary(languageId, siteId);
            log.LogMethodExit(labelMessageDictionary);
            return labelMessageDictionary;
        }

        /// <summary>
        ///  Returns the Max Message no matching the searchParameters
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public int GetMaxMessageNo(List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MessagesDataHandler messagesDataHandler = new MessagesDataHandler();
            int messagesCount = messagesDataHandler.GetMaxMessageNo(searchParameters);
            log.LogMethodExit(messagesCount);
            return messagesCount;
        }

        /// <summary>
        /// Gets the MessagesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MessagesDTO matching the search criteria</returns>
        public List<MessagesDTO> GetAllMessagesList(List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>> searchParameters, bool loadChild = false, bool activeRecordsOnly = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChild);
            MessagesDataHandler messagesDataHandler = new MessagesDataHandler(sqlTransaction);
            messagesList = messagesDataHandler.GetAllMessages(searchParameters);

            if (messagesList != null && messagesList.Any() && loadChild)
            {
                Build(messagesList, activeRecordsOnly, sqlTransaction);
            }
            log.LogMethodExit(messagesList);
            return messagesList;
        }

        private void Build(List<MessagesDTO> messagesDTOList, bool activeRecordsOnly = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(messagesDTOList, sqlTransaction);
            Dictionary<int, MessagesDTO> messageIdDictionary = new Dictionary<int, MessagesDTO>();
            List<int> messageIdList = new List<int>();
            int length = messagesDTOList.Count;
            for (int i = 0; i < length; i++)
            {
                if (messageIdDictionary.ContainsKey(messagesDTOList[i].MessageId) == false)
                {
                    messageIdDictionary.Add(messagesDTOList[i].MessageId, messagesDTOList[i]);
                    messageIdList.Add(messagesDTOList[i].MessageId);
                }
            }

            TranslatedMessageListBL translatedMessageListBL = new TranslatedMessageListBL();
            List<TranslatedMessageDTO> translatedMessageDTOList = translatedMessageListBL.GetTranslatedMessageDTOListForMessages(messageIdList, activeRecordsOnly, sqlTransaction);
            if (translatedMessageDTOList != null && translatedMessageDTOList.Any())
            {
                log.LogVariableState("translatedMessageDTOList", translatedMessageDTOList);
                foreach (var translatedMessageDTO in translatedMessageDTOList)
                {
                    if (messageIdDictionary.ContainsKey(translatedMessageDTO.MessageId))
                    {
                        if (messageIdDictionary[translatedMessageDTO.MessageId].TranslatedMessageList == null)
                        {
                            messageIdDictionary[translatedMessageDTO.MessageId].TranslatedMessageList = new List<TranslatedMessageDTO>();
                        }
                        messageIdDictionary[translatedMessageDTO.MessageId].TranslatedMessageList.Add(translatedMessageDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MessagesDTO matching the search Parameters</returns>
        public List<MessagesDTO> GetMessagesDTOList(List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>> searchParameters,
            int currentPage, int pageSize, bool loadChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, currentPage, pageSize, loadChildRecords, sqlTransaction);
            MessagesDataHandler messagesDataHandler = new MessagesDataHandler(sqlTransaction);
            messagesList = messagesDataHandler.GetMessagesDTOList(searchParameters, currentPage, pageSize);
            if (loadChildRecords)
            {
                if (messagesList != null && messagesList.Count > 0)
                {
                    foreach (MessagesDTO messagesDTO in messagesList)
                    {
                        List<KeyValuePair<TranslatedMessageDTO.SearchByParameters, string>> searchOrderTypeParameters = new List<KeyValuePair<TranslatedMessageDTO.SearchByParameters, string>>();
                        searchOrderTypeParameters.Add(new KeyValuePair<TranslatedMessageDTO.SearchByParameters, string>(TranslatedMessageDTO.SearchByParameters.MESSAGE_ID, messagesDTO.MessageId.ToString()));
                        TranslatedMessageListBL translatedMessageListBL = new TranslatedMessageListBL();
                        messagesDTO.TranslatedMessageList = translatedMessageListBL.GetAllTranslatedMessagesList(searchOrderTypeParameters);
                    }
                }
            }
            log.LogMethodExit(messagesList);
            return messagesList;
        }

        /// <summary>
        /// Returns the no of messagesCount matching the searchParameters
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns></returns>
        public int GetMessagesCount(List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            MessagesDataHandler messagesDataHandler = new MessagesDataHandler();
            int messagesCount = messagesDataHandler.GetMessagesCount(searchParameters);
            return messagesCount;
        }
        /// <summary>
        /// This method should be used to Save and Update the Messages details for Web Management Studio.
        /// </summary>
        public void SaveUpdateMessages()
        {
            log.LogMethodEntry();

            if (messagesList != null && messagesList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (MessagesDTO messageDTO in messagesList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            Messages messages = new Messages(executionContext, messageDTO);
                            messages.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
                log.LogMethodExit();
            }
        }

        internal DateTime? GetMessageModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            MessagesDataHandler messagesDataHandler = new MessagesDataHandler();
            DateTime? result = messagesDataHandler.GetMessageModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
