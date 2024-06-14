
/********************************************************************************************
 * Project Name - Utilities
 * Description  - MessageViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
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
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// MessageViewContainer holds the messages values for a given siteId, languageId
    /// </summary>
    public class MessageViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly MessageContainerDTOCollection messageContainerDTOCollection;
        private readonly Dictionary<int, string> messageNumberTranslatedMessageDictionary = new Dictionary<int, string>();
        private readonly Dictionary<string, string> literalTranslatedLiteralDictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        private readonly int siteId;
        private readonly int languageId;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="languageId">user primary key</param>
        /// <param name="messageContainerDTOCollection">messageContainerDTOCollection</param>
        public MessageViewContainer(int siteId, int languageId, MessageContainerDTOCollection messageContainerDTOCollection)//changes from internal to public
        {
            log.LogMethodEntry(siteId, languageId, messageContainerDTOCollection);
            this.siteId = siteId;
            this.languageId = languageId;
            this.messageContainerDTOCollection = messageContainerDTOCollection;
            if (messageContainerDTOCollection != null &&
                messageContainerDTOCollection.MessageContainerDTOList != null &&
                messageContainerDTOCollection.MessageContainerDTOList.Any())
            {
                foreach (var messageContainerDTO in messageContainerDTOCollection.MessageContainerDTOList)
                {
                    if (messageContainerDTO.MessageNumber < 10000)
                    {
                        if (messageNumberTranslatedMessageDictionary.ContainsKey(messageContainerDTO.MessageNumber) == false)
                        {
                            messageNumberTranslatedMessageDictionary.Add(messageContainerDTO.MessageNumber, messageContainerDTO.TranslatedMessage);
                        }
                    }
                    else
                    {
                        if (literalTranslatedLiteralDictionary.ContainsKey(messageContainerDTO.Message) == false)
                        {
                            literalTranslatedLiteralDictionary.Add(messageContainerDTO.Message, messageContainerDTO.TranslatedMessage);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        public MessageViewContainer(int siteId, int languageId)//Changed from internal to public
            :this(siteId, languageId, GetMessageContainerDTOCollection(siteId, languageId, null, false))
        {
            log.LogMethodEntry(siteId, languageId);
            log.LogMethodExit();
        }

        private static MessageContainerDTOCollection GetMessageContainerDTOCollection(int siteId, int languageId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, languageId);
            MessageContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IMessageUseCases messageUseCases = CommunicationUseCaseFactory.GetMessageUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<MessageContainerDTOCollection> task = messageUseCases.GetMessageContainerDTOCollection(siteId, languageId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving MessageContainerDTOCollection.", ex);
                result = new MessageContainerDTOCollection();
            }
            
            log.LogMethodExit(result);
            return result;
        }

        public string GetMessage(string literal, params object[] parameters)//changed from internal to public
        {
            log.LogMethodEntry(literal, parameters);
            string result = literal;
            if (literalTranslatedLiteralDictionary.ContainsKey(literal))
            {
                result = literalTranslatedLiteralDictionary[literal];
            }
            result = ReplaceMessagePlaceHolders(result, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public string GetMessage(int messageNo, params object[] parameters)//Changed from internal to public
        {
            log.LogMethodEntry();
            string result;
            if(messageNumberTranslatedMessageDictionary.ContainsKey(messageNo) == false)
            {
                result = "Message not defined for Message No: " + messageNo.ToString();
            }
            else
            {
                result = messageNumberTranslatedMessageDictionary[messageNo];
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

        /// <summary>
        /// returns the latest in MessageContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        public MessageContainerDTOCollection GetMessageContainerDTOCollection(string hash)//changed from public to internal
        {
            log.LogMethodEntry(hash);
            if (messageContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(messageContainerDTOCollection);
            return messageContainerDTOCollection;
        }

        internal MessageViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if(LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            MessageContainerDTOCollection latestMessageContainerDTOCollection = GetMessageContainerDTOCollection(siteId, languageId, messageContainerDTOCollection.Hash, rebuildCache);
            if (latestMessageContainerDTOCollection == null || 
                latestMessageContainerDTOCollection.MessageContainerDTOList == null ||
                latestMessageContainerDTOCollection.MessageContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            MessageViewContainer result = new MessageViewContainer(siteId, languageId, latestMessageContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
