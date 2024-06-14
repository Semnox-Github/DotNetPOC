/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - EnabledAttributesViewContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      20-Aug-2021      Fiona                    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.TableAttributeSetup;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// EnabledAttributesViewContainerList
    /// </summary>
    public class EnabledAttributesViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, EnabledAttributesViewContainer> enabledAttributesViewContainerCache = new Cache<int, EnabledAttributesViewContainer>();
        private static Timer refreshTimer;
        static EnabledAttributesViewContainerList()
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
            var uniqueKeyList = enabledAttributesViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                EnabledAttributesViewContainer enabledAttributesViewContainer;
                if (enabledAttributesViewContainerCache.TryGetValue(uniqueKey, out enabledAttributesViewContainer))
                {
                    enabledAttributesViewContainerCache[uniqueKey] = enabledAttributesViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static EnabledAttributesViewContainer GetEnabledAttributesViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            EnabledAttributesViewContainer result = enabledAttributesViewContainerCache.GetOrAdd(siteId, (k) => new EnabledAttributesViewContainer(siteId));
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
        public static EnabledAttributesContainerDTOCollection GetEnabledAttributesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, hash);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            EnabledAttributesViewContainer enabledAttributesViewContainer = GetEnabledAttributesViewContainer(siteId);
            EnabledAttributesContainerDTOCollection result = enabledAttributesViewContainer.GetEnabledAttributesContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetEnabledAttributesContainerDTOList
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static List<EnabledAttributesContainerDTO> GetEnabledAttributesContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            EnabledAttributesViewContainer enabledAttributesViewContainer = GetEnabledAttributesViewContainer(executionContext.SiteId);
            List<EnabledAttributesContainerDTO> result = enabledAttributesViewContainer.GetEnabledAttributesContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            EnabledAttributesViewContainer enabledAttributesViewContainer = GetEnabledAttributesViewContainer(siteId);
            enabledAttributesViewContainerCache[siteId] = enabledAttributesViewContainer.Refresh(true);
            log.LogMethodExit();
        }
    }
}
