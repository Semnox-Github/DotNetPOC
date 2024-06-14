/********************************************************************************************
 * Project Name - Utilities  Class
 * Description  - LanguageViewContainerList holds multiple  LanguageView containers based on siteId, userId and POSMachineId
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// LanguageViewContainerList holds multiple  LanguageView containers based on siteId, userId and POSMachineId
    /// </summary>
    public class LanguageViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, LanguageViewContainer> languageViewContainerCache = new Cache<int, LanguageViewContainer>();
        private static Timer refreshTimer;

        static LanguageViewContainerList()
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
            var uniqueKeyList = languageViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                LanguageViewContainer languageViewContainer;
                if (languageViewContainerCache.TryGetValue(uniqueKey, out languageViewContainer))
                {
                    languageViewContainerCache[uniqueKey] = languageViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        

        private static LanguageViewContainer GetLanguageViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = languageViewContainerCache.GetOrAdd(siteId, (k)=> new LanguageViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the LanguageContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// <returns></returns>
        public static LanguageContainerDTOCollection GetLanguageContainerDTOCollection(int siteId, string hash)
        {
            log.LogMethodEntry(siteId, hash);
            LanguageViewContainer languageViewContainer = GetLanguageViewContainer(siteId); 
            LanguageContainerDTOCollection result = languageViewContainer.GetLanguageContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            LanguageViewContainer languageViewContainer = GetLanguageViewContainer(siteId);
            languageViewContainerCache[siteId] = languageViewContainer.Refresh(true);
            log.LogMethodExit();
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

        /// <summary>
        /// Returns the LanguageContainerDTO based on the siteId and languageId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="languageId">language Id</param>
        /// <returns></returns>
        public static LanguageContainerDTO GetLanguageContainerDTO(int siteId, int languageId)
        {
            log.LogMethodEntry(siteId, languageId);
            LanguageViewContainer languageViewContainer = GetLanguageViewContainer(siteId);
            LanguageContainerDTO result = languageViewContainer.GetLanguageContainerDTO(languageId);
            log.LogMethodExit();
            return result;
        }
    }
}
