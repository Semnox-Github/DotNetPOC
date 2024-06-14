/********************************************************************************************
 * Project Name - Utilities  Class
 * Description  - MessageViewContainerList holds multiple  MessageView containers based on siteId, userId and POSMachineId
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement
 ********************************************************************************************/
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// MessageViewContainerList holds multiple  MessageView containers based on siteId, userId and POSMachineId
    /// </summary>
    public class MessageViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<string, MessageViewContainer> messageViewContainerCache = new Cache<string, MessageViewContainer>();
        private static Timer refreshTimer;

        static MessageViewContainerList()
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
            var uniqueKeyList = messageViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                MessageViewContainer messageViewContainer;
                if (messageViewContainerCache.TryGetValue(uniqueKey, out messageViewContainer))
                {
                    messageViewContainerCache[uniqueKey] = messageViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static MessageViewContainer GetMessageViewContainer(int siteId, int languageId)
        {
            log.LogMethodEntry(siteId, languageId);
            string uniqueKey = GetUniqueKey(siteId, languageId);
            MessageViewContainer result = messageViewContainerCache.GetOrAdd(uniqueKey, (k)=> new MessageViewContainer(siteId, languageId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the SystemOptionContainerDTOCollection for a given siteId, languageId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="languageId">language Id</param>
        /// <param name="hash">hash</param>
        /// <returns></returns>
        public static MessageContainerDTOCollection GetMessageContainerDTOCollection(int siteId, int languageId, string hash)
        {
            log.LogMethodEntry(siteId, languageId, hash);
            MessageViewContainer messageViewContainer = GetMessageViewContainer(siteId, languageId);
            MessageContainerDTOCollection result = messageViewContainer.GetMessageContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        private static string GetUniqueKey(int siteId, int languageId)
        {
            log.LogMethodEntry(siteId, languageId);
            string uniqueKey = "SiteId:" + siteId + "LanguageId:" + languageId;
            log.LogMethodExit(uniqueKey);
            return uniqueKey;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId, int languageId)
        {
            log.LogMethodEntry(siteId, languageId);
            string uniqueKey = GetUniqueKey(siteId, languageId);
            MessageViewContainer messageViewContainer = GetMessageViewContainer(siteId, languageId);
            messageViewContainerCache[uniqueKey] = messageViewContainer.Refresh(true);
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
            MessageViewContainer messageViewContainer = GetMessageViewContainer(executionContext.SiteId, executionContext.LanguageId);
            string result = messageViewContainer.GetMessage(literal, parameters);
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
            MessageViewContainer messageViewContainer = GetMessageViewContainer(executionContext.SiteId, executionContext.LanguageId);
            string result = messageViewContainer.GetMessage(messageNumber, parameters);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the translated message based on the language of the execution context
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="languageId"></param>
        /// <param name="messageNumber">message number</param>
        /// <param name="parameters">place holder values</param>
        /// <returns></returns>
        public static string GetMessage(int siteId, int languageId, int messageNumber, params object[] parameters)
        {
            log.LogMethodEntry(siteId, languageId, messageNumber, parameters);
            MessageViewContainer messageViewContainer = GetMessageViewContainer(siteId, languageId);
            string result = messageViewContainer.GetMessage(messageNumber, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}
