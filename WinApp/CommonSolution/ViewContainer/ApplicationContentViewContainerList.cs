/********************************************************************************************
* Project Name - ViewContainer
* Description  - ApplicationContentViewContainerList class 
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
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationContentViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, ApplicationContentViewContainer> applicationContentViewContainerCache = new Cache<int, ApplicationContentViewContainer>();
        private static Timer refreshTimer;

        static ApplicationContentViewContainerList()
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
            var uniqueKeyList = applicationContentViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                ApplicationContentViewContainer applicationContentViewContainer;
                if (applicationContentViewContainerCache.TryGetValue(uniqueKey, out applicationContentViewContainer))
                {
                    applicationContentViewContainerCache[uniqueKey] = applicationContentViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }


        private static ApplicationContentViewContainer GetApplicationContentViewContainer(int siteId, int languageId)
        {
            log.LogMethodEntry(siteId, languageId);
            var result = applicationContentViewContainerCache.GetOrAdd(siteId, (k) => new ApplicationContentViewContainer(siteId, languageId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the ApplicationContentContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<ApplicationContentContainerDTO> GetApplicationContentContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ApplicationContentViewContainer applicationContentViewContainer = GetApplicationContentViewContainer(executionContext.SiteId, executionContext.LanguageId);
            List<ApplicationContentContainerDTO> result = applicationContentViewContainer.GetApplicationContentContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="languageId"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static ApplicationContentContainerDTOCollection GetApplicationContentContainerDTOCollection(int siteId, int languageId, string hash)
        {
            log.LogMethodEntry(siteId);

            ApplicationContentViewContainer container = GetApplicationContentViewContainer(siteId, languageId);
            ApplicationContentContainerDTOCollection applicationContentContainerDTOCollection = container.GetApplicationContentContainerDTOCollection(hash);
            return applicationContentContainerDTOCollection;
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
        public static void Rebuild(int siteId, int languageId)
        {
            log.LogMethodEntry();
            string uniqueKey = GetUniqueKey(siteId, languageId);
            ApplicationContentViewContainer container = GetApplicationContentViewContainer(siteId, languageId);

            var uniqueKeyList = applicationContentViewContainerCache.Keys;
            foreach (var uniqueKeys in uniqueKeyList)
            {

                if (applicationContentViewContainerCache.TryGetValue(uniqueKeys, out container))
                {
                    applicationContentViewContainerCache[uniqueKeys] = container.Refresh(true);
                }
            }
            //  applicationContentViewContainerCache[uniqueKey] = container.Refresh(true);

            //ApplicationContentViewContainer container = GetApplicationContentViewContainer(siteId);
            //applicationContentViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}
