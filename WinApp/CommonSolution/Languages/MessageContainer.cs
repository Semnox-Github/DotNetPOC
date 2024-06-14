/********************************************************************************************
 * Project Name - Utilities
 * Description  - MessageContainer class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Languages
{
    /// <summary>
    /// Class holds the message values for a given site.
    /// </summary>
    public class MessageContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<int, MessageContainerDTOCollection> languageIdMessageContainerDTOCollectionDictionary = new Dictionary<int, MessageContainerDTOCollection>();
        private readonly HashSet<int> languageIdHashSet = new HashSet<int>() { -1 };
        private readonly Dictionary<int, Dictionary<int, string>> languageIdMessageNumberTranslatedMessageDictionary = new Dictionary<int, Dictionary<int, string>>();
        private readonly Dictionary<int, Dictionary<string, string>> languageIdLiteralTranslatedLiteralDictionary = new Dictionary<int, Dictionary<string, string>>();
        private readonly DateTime? messageModuleLastUpdateTime;
        private readonly int siteId;
        private readonly List<MessagesDTO> messageDTOList;

        public MessageContainer(int siteId) : this(siteId, GetMessageDTOList(siteId), GetMessagesLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        public MessageContainer(int siteId, List<MessagesDTO> messageDTOList, DateTime? messageModuleLastUpdateTime)//changed from internal to public
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.messageDTOList = messageDTOList;
            this.messageModuleLastUpdateTime = messageModuleLastUpdateTime;

            Dictionary<int, List<MessageContainerDTO>> languageIdMessageContainerDTOListDictionary = new Dictionary<int, List<MessageContainerDTO>>();
            Dictionary<int, Dictionary<int, TranslatedMessageDTO>> messageIdLanguageIdTranslatedMessageDTODictionary = new Dictionary<int, Dictionary<int, TranslatedMessageDTO>>();


            foreach (MessagesDTO messagesDTO in messageDTOList)
            {
                if (messageIdLanguageIdTranslatedMessageDTODictionary.ContainsKey(messagesDTO.MessageId))
                {
                    continue;
                }
                Dictionary<int, TranslatedMessageDTO> languageIdTranslatedMessageDTODictionary = new Dictionary<int, TranslatedMessageDTO>();
                if (messagesDTO.TranslatedMessageList != null && messagesDTO.TranslatedMessageList.Any())
                {
                    foreach (var translatedMessageDTO in messagesDTO.TranslatedMessageList)
                    {
                        if (languageIdTranslatedMessageDTODictionary.ContainsKey(translatedMessageDTO.LanguageId))
                        {
                            continue;
                        }
                        languageIdTranslatedMessageDTODictionary.Add(translatedMessageDTO.LanguageId, translatedMessageDTO);
                        languageIdHashSet.Add(translatedMessageDTO.LanguageId);
                    }
                }
                messageIdLanguageIdTranslatedMessageDTODictionary.Add(messagesDTO.MessageId, languageIdTranslatedMessageDTODictionary);
            }
            foreach (var languageId in languageIdHashSet)
            {
                List<MessageContainerDTO> messageContainerDTOList = new List<MessageContainerDTO>();
                languageIdMessageContainerDTOListDictionary.Add(languageId, messageContainerDTOList);
                Dictionary<int, string> messageNumberTranslatedMessageDictionary = new Dictionary<int, string>();
                languageIdMessageNumberTranslatedMessageDictionary.Add(languageId, messageNumberTranslatedMessageDictionary);
                Dictionary<string, string> literalTranslatedLiteralDictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                languageIdLiteralTranslatedLiteralDictionary.Add(languageId, literalTranslatedLiteralDictionary);
                foreach (MessagesDTO messagesDTO in messageDTOList)
                {
                    int messageId = messagesDTO.MessageId;
                    int messageNumber = messagesDTO.MessageNo;
                    string message = messagesDTO.Message;
                    string translatedMessage = messagesDTO.Message;
                    if (messageIdLanguageIdTranslatedMessageDTODictionary.ContainsKey(messageId) &&
                        messageIdLanguageIdTranslatedMessageDTODictionary[messageId].ContainsKey(languageId))
                    {
                        translatedMessage = messageIdLanguageIdTranslatedMessageDTODictionary[messageId][languageId].Message;
                    }
                    else if (messageNumber >= 10000)
                    {
                        continue;
                    }
                    MessageContainerDTO messageContainerDTO = new MessageContainerDTO(messageId, messageNumber, message, translatedMessage);
                    messageContainerDTOList.Add(messageContainerDTO);
                    if (messageNumber >= 10000)
                    {
                        if(literalTranslatedLiteralDictionary.ContainsKey(message) == false)
                        {
                            literalTranslatedLiteralDictionary.Add(message, translatedMessage);
                        }
                    }
                    else
                    {
                        if(messageNumberTranslatedMessageDictionary.ContainsKey(messageNumber) == false)
                        {
                            messageNumberTranslatedMessageDictionary.Add(messageNumber, translatedMessage);
                        }
                    }
                }
                languageIdMessageContainerDTOCollectionDictionary.Add(languageId, new MessageContainerDTOCollection(messageContainerDTOList));
            }
        }



        private static DateTime? GetMessagesLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                MessageListBL messageListBL = new MessageListBL();
                result = messageListBL.GetMessageModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the system option max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<MessagesDTO> GetMessageDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<MessagesDTO> messagesDTOList = null;
            try
            {
                MessageListBL messageListBL = new MessageListBL();
                List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>> searchParameters = new List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>>();
                searchParameters.Add(new KeyValuePair<MessagesDTO.SearchByMessageParameters, string>(MessagesDTO.SearchByMessageParameters.SITE_ID, siteId.ToString()));
                messagesDTOList = messageListBL.GetAllMessagesList(searchParameters, true, true);
                //if (messagesDTOList == null || messagesDTOList.Any() == false)
                //{
                //    log.LogMethodExit(null, "No messages defined in the system");
                //    return null;
                //}
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the system options.", ex);
            }

            if (messagesDTOList == null)
            {
                messagesDTOList = new List<MessagesDTO>();
            }
            log.LogMethodExit(messagesDTOList);
            return messagesDTOList;
        }

        public MessageContainerDTOCollection GetMessageContainerDTOCollection(int languageId)
        {
            log.LogMethodEntry(languageId);
            MessageContainerDTOCollection result;
            if(languageIdMessageContainerDTOCollectionDictionary.ContainsKey(languageId))
            {
                result = languageIdMessageContainerDTOCollectionDictionary[languageId];
            }
            else if(languageIdMessageContainerDTOCollectionDictionary.ContainsKey(-1))
            {
                result = languageIdMessageContainerDTOCollectionDictionary[-1];
            }
            else
            {
                result = new MessageContainerDTOCollection();
            }
            log.LogMethodExit(result);
            return result;
        }

        public string GetMessage(int languageId, string literal, params object[] parameters)
        {
            log.LogMethodEntry(literal, parameters);
            string result = literal;
            if (languageIdLiteralTranslatedLiteralDictionary.ContainsKey(languageId) &&
                languageIdLiteralTranslatedLiteralDictionary[languageId].ContainsKey(literal))
            {
                result = languageIdLiteralTranslatedLiteralDictionary[languageId][literal];
            }
            result = ReplaceMessagePlaceHolders(result, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public string GetMessage(int parameterLanguageId, int messageNo, params object[] parameters)
        {
            log.LogMethodEntry();
            string result;
            int languageId = parameterLanguageId;
            if (languageIdHashSet.Contains(languageId) == false)
            {
                languageId = -1;
            }
            if(languageIdMessageNumberTranslatedMessageDictionary.ContainsKey(languageId) == false ||
                languageIdMessageNumberTranslatedMessageDictionary[languageId].ContainsKey(messageNo) == false)
            {
                result = "Message not defined for Message No: " + messageNo.ToString();
            }
            else
            {
                result = languageIdMessageNumberTranslatedMessageDictionary[languageId][messageNo];
            }
            result = ReplaceMessagePlaceHolders(result, parameters);
            log.LogMethodExit(result);
            return result;
        }

        private string ReplaceMessagePlaceHolders(string message, params object[] parameters)
        {
            log.LogMethodEntry(message, parameters);
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    message = message.Replace("&" + (i + 1).ToString(), parameters[i].ToString());
                }
            }
            log.LogMethodExit(message);
            return message;
        }

        public MessageContainer Refresh()
        {
            log.LogMethodEntry();
            MessageListBL messageListBL = new MessageListBL();
            DateTime? updateTime = messageListBL.GetMessageModuleLastUpdateTime(siteId);
            if (messageModuleLastUpdateTime.HasValue
                && messageModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in messages since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            MessageContainer result = new MessageContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
