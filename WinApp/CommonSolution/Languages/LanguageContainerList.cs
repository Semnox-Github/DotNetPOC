/********************************************************************************************
 * Project Name - Utilities
 * Description  - LanguageMasterList class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     27-NOV-2020   Lakshminarayana     Created: POS Redesign
 ********************************************************************************************/

using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Languages
{
    public static class LanguageContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, LanguageContainer> languageContainerCache = new Cache<int, LanguageContainer>();
        private static Timer refreshTimer;

        static LanguageContainerList()
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
            var uniqueKeyList = languageContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                LanguageContainer languageContainer;
                if (languageContainerCache.TryGetValue(uniqueKey, out languageContainer))
                {
                    languageContainerCache[uniqueKey] = languageContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static LanguageContainer GetLanguageContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            LanguageContainer result = languageContainerCache.GetOrAdd(siteId, (k)=> new LanguageContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static LanguageContainerDTOCollection GetLanguageContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            LanguageContainer container = GetLanguageContainer(siteId);
            LanguageContainerDTOCollection result = container.GetLanguageContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            LanguageContainer languageContainer = GetLanguageContainer(siteId);
            languageContainerCache[siteId] = languageContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the LanguageContainerDTO based on the site and languageId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="optionName">option value name</param>
        /// <returns></returns>
        public static LanguageContainerDTO GetLanguageContainerDTO(int siteId, int lanuageId)
        {
            log.LogMethodEntry(siteId, lanuageId);
            LanguageContainer languageContainer = GetLanguageContainer(siteId);
            LanguageContainerDTO result = languageContainer.GetLanguageContainerDTO(lanuageId);  
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the LanguageContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static LanguageContainerDTO GetLanguageContainerDTO(ExecutionContext executionContext)
        {
            return GetLanguageContainerDTO(executionContext.SiteId, executionContext.LanguageId);
        }
    }
}
