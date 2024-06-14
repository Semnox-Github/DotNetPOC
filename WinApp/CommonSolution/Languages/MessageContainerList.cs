/********************************************************************************************
 * Project Name - Utilities
 * Description  - MessageMasterList class to holds the list of message containers 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     27-NOV-2020      Lakshminarayana     Created: POS Redesign
 ********************************************************************************************/

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Languages
{
    public static class MessageContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, MessageContainer> messageContainerDictionary = new Cache<int, MessageContainer>();
        private static Timer refreshTimer;

        static MessageContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = messageContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                MessageContainer messageContainer;
                if (messageContainerDictionary.TryGetValue(uniqueKey, out messageContainer))
                {
                    messageContainerDictionary[uniqueKey] = messageContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static MessageContainer GetMessageContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            MessageContainer result = messageContainerDictionary.GetOrAdd(siteId, (k) => new MessageContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the MessageContainerDTOCollection data structure used to build the view container
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userPkId"></param>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public static MessageContainerDTOCollection GetMessageContainerDTOCollection(int siteId, int languageId)
        {
            MessageContainer container = GetMessageContainer(siteId);
            return container.GetMessageContainerDTOCollection(languageId);
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            MessageContainer messageContainer = GetMessageContainer(siteId);
            messageContainerDictionary[siteId] = messageContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the translated literal based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="literal">literal</param>
        /// <param name="parameters">replace values</param>
        /// <returns></returns>
        public static string GetMessage(ExecutionContext executionContext, string literal, params object[] parameters)
        {
            log.LogMethodEntry(executionContext, literal, parameters);
            string result = GetMessage(executionContext.SiteId, executionContext.LanguageId, literal, parameters);
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Returns the translated literal based on the siteId and default language Id
        /// </summary>
        /// <param name="siteId">current siteId</param>
        /// <param name="literal">literal</param>
        /// <param name="parameters">replace values</param>
        /// <returns></returns>
        public static string GetMessage(int siteId, string literal, params object[] parameters)
        {
            log.LogMethodEntry(siteId, literal, parameters);
            string result = GetMessage(siteId, -1, literal, parameters);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the translated literal based on the siteId, language Id
        /// </summary>
        /// <param name="siteId">current siteId</param>
        /// <param name="languageId">language Id</param>
        /// <param name="literal">literal</param>
        /// <param name="parameters">replace values</param>
        /// <returns></returns>
        public static string GetMessage(int siteId, int languageId, string literal, params object[] parameters)
        {
            log.LogMethodEntry(siteId, languageId, literal, parameters);
            MessageContainer messageContainer = GetMessageContainer(siteId);
            string result = messageContainer.GetMessage(languageId, literal, parameters);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the translated message based on the language of the execution context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="messageNumber">message number</param>
        /// <param name="parameters">place holder values</param>
        /// <returns></returns>
        public static string GetMessage(ExecutionContext executionContext, int messageNumber, params object[] parameters)
        {
            log.LogMethodEntry(executionContext, messageNumber, parameters);
            string result = GetMessage(executionContext.SiteId, executionContext.LanguageId, messageNumber, parameters);
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Returns the translated message based on the siteId and language Id
        /// </summary>
        /// <param name="siteId">current siteId</param>
        /// <param name="languageId">language Id</param>
        /// <param name="messageNumber">message number</param>
        /// <param name="parameters">place holder values</param>
        /// <returns></returns>
        public static string GetMessage(int siteId, int languageId, int messageNumber, params object[] parameters)
        {
            log.LogMethodEntry(siteId, languageId, messageNumber, parameters);
            MessageContainer messageContainer = GetMessageContainer(siteId);
            string result = messageContainer.GetMessage(languageId, messageNumber, parameters);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the translated message based on the siteId and default language Id
        /// </summary>
        /// <param name="siteId">current siteId</param>
        /// <param name="messageNumber">message number</param>
        /// <param name="parameters">place holder values</param>
        /// <returns></returns>
        public static string GetMessage(int siteId, int messageNumber, params object[] parameters)
        {
            log.LogMethodEntry(siteId, messageNumber, parameters);
            string result = GetMessage(siteId, -1, messageNumber, parameters);
            log.LogMethodExit(result);
            return result;
        }

    }
}
