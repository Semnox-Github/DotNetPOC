/********************************************************************************************
* Project Name - ViewContainer
* Description  - CustomAttributeViewContainerList class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    27-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System.Timers;
using System.Collections.Generic;

namespace Semnox.Parafait.ViewContainer
{
    public class CustomAttributeViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, CustomAttributeViewContainer> CustomAttributeViewContainerCache = new Cache<int, CustomAttributeViewContainer>();
        private static Timer refreshTimer;

        static CustomAttributeViewContainerList()
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
            var uniqueKeyList = CustomAttributeViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                CustomAttributeViewContainer CustomAttributeViewContainer;
                if (CustomAttributeViewContainerCache.TryGetValue(uniqueKey, out CustomAttributeViewContainer))
                {
                    CustomAttributeViewContainerCache[uniqueKey] = CustomAttributeViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static CustomAttributeViewContainer GetCustomAttributeViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = CustomAttributeViewContainerCache.GetOrAdd(siteId, (k) => new CustomAttributeViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the CustomAttributesContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<CustomAttributesContainerDTO> GetCustomAttributesContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            CustomAttributeViewContainer CustomAttributeViewContainer = GetCustomAttributeViewContainer(executionContext.SiteId);
            List<CustomAttributesContainerDTO> result = CustomAttributeViewContainer.GetCustomAttributesContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public static CustomAttributeContainerDTOCollection GetCustomAttributesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            CustomAttributeViewContainer container = GetCustomAttributeViewContainer(siteId);
            CustomAttributeContainerDTOCollection customAttributesContainerDTOCollection = container.GetCustomAttributeContainerDTOCollection(hash);
            return customAttributesContainerDTOCollection;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            CustomAttributeViewContainer container = GetCustomAttributeViewContainer(siteId);
            CustomAttributeViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}
