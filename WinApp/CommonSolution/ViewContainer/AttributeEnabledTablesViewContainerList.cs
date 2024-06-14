/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - AttributeEnabledTablesViewContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.0      24-Aug-2021      Fiona                    Created 
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
    /// 
    /// </summary>
    public class AttributeEnabledTablesViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, AttributeEnabledTablesViewContainer> attributeEnabledTablesViewContainerCache = new Cache<int, AttributeEnabledTablesViewContainer>();
        private static Timer refreshTimer;
        static AttributeEnabledTablesViewContainerList()
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
            var uniqueKeyList = attributeEnabledTablesViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                AttributeEnabledTablesViewContainer AttributeEnabledTablesViewContainer;
                if (attributeEnabledTablesViewContainerCache.TryGetValue(uniqueKey, out AttributeEnabledTablesViewContainer))
                {
                    attributeEnabledTablesViewContainerCache[uniqueKey] = AttributeEnabledTablesViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static AttributeEnabledTablesViewContainer GetAttributeEnabledTablesViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            AttributeEnabledTablesViewContainer result = attributeEnabledTablesViewContainerCache.GetOrAdd(siteId, (k) => new AttributeEnabledTablesViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetAttributeEnabledTablesContainerDTOCollection
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public static AttributeEnabledTablesContainerDTOCollection GetAttributeEnabledTablesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, hash);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            AttributeEnabledTablesViewContainer attributeEnabledTablesViewContainer = GetAttributeEnabledTablesViewContainer(siteId);
            AttributeEnabledTablesContainerDTOCollection result = attributeEnabledTablesViewContainer.GetAttributeEnabledTablesContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetAttributeEnabledTablesContainerDTOList
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static List<AttributeEnabledTablesContainerDTO> GetAttributeEnabledTablesContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            AttributeEnabledTablesViewContainer attributeEnabledTablesViewContainer = GetAttributeEnabledTablesViewContainer(executionContext.SiteId);
            List<AttributeEnabledTablesContainerDTO> result = attributeEnabledTablesViewContainer.GetAttributeEnabledTablesContainerDTOList();
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
            AttributeEnabledTablesViewContainer attributeEnabledTablesViewContainer = GetAttributeEnabledTablesViewContainer(siteId);
            attributeEnabledTablesViewContainerCache[siteId] = attributeEnabledTablesViewContainer.Refresh(true);
            log.LogMethodExit();
        }
    }
}
