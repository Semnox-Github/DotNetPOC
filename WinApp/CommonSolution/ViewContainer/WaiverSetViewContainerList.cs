/********************************************************************************************
* Project Name - ViewContainer
* Description  - WaiverSetViewContainerList class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    20-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Waiver;

namespace Semnox.Parafait.ViewContainer
{
    public class WaiverSetViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, WaiverSetViewContainer> waiverSetViewContainerCache = new Cache<int, WaiverSetViewContainer>();
        private static Timer refreshTimer;

        static WaiverSetViewContainerList()
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
            var uniqueKeyList = waiverSetViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                WaiverSetViewContainer waiverSetViewContainer;
                if (waiverSetViewContainerCache.TryGetValue(uniqueKey, out waiverSetViewContainer))
                {
                    waiverSetViewContainerCache[uniqueKey] = waiverSetViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static WaiverSetViewContainer GetWaiverSetViewContainer(int siteId, int languageId)
        {
            log.LogMethodEntry(siteId, languageId);
            var result = waiverSetViewContainerCache.GetOrAdd(siteId, (k) => new WaiverSetViewContainer(siteId, languageId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the WaiverSetContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<WaiverSetContainerDTO> GetWaiverSetContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            WaiverSetViewContainer waiverSetViewContainer = GetWaiverSetViewContainer(executionContext.SiteId, executionContext.LanguageId);
            List<WaiverSetContainerDTO> result = waiverSetViewContainer.GetWaiverSetContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="languageId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public static WaiverSetContainerDTOCollection GetWaiverSetContainerDTOCollection(int siteId, int languageId,string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId, languageId);
            }
            WaiverSetViewContainer container = GetWaiverSetViewContainer(siteId, languageId);
            WaiverSetContainerDTOCollection waiverSetContainerDTOCollection = container.GetWaiverSetContainerDTOCollection(hash);
            return waiverSetContainerDTOCollection;
        }

        private static string GetUniqueKey(int siteId, int languageId)
        {
            log.LogMethodEntry(siteId, languageId);
            string uniqueKey = "SiteId:" + siteId + "LanguageId:" + languageId;
            log.LogMethodExit(uniqueKey);
            return uniqueKey;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="languageId"></param>
        public static void Rebuild(int siteId,int languageId)
        {
            log.LogMethodEntry();
            string uniqueKey = GetUniqueKey(siteId, languageId);
            WaiverSetViewContainer container = GetWaiverSetViewContainer(siteId, languageId);
            var uniqueKeyList = waiverSetViewContainerCache.Keys;
            foreach (var uniqueKeys in uniqueKeyList)
            {

                if (waiverSetViewContainerCache.TryGetValue(uniqueKeys, out container))
                {
                    waiverSetViewContainerCache[uniqueKeys] = container.Refresh(true);
                }
            }
            log.LogMethodExit();
        }
    }
}
