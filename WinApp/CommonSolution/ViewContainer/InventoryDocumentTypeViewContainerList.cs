/********************************************************************************************
 * Project Name - Inventory
 * Description  - InventoryDocumentTypeViewContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      18-Aug-2022      Abhishek           Created : Web Inventory Redesign 
 ********************************************************************************************/
using System;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// InventoryDocumentTypeViewContainerList
    /// </summary>
    public class InventoryDocumentTypeViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, InventoryDocumentTypeViewContainer> inventoryDocumentTypeViewContainerCache = new Cache<int, InventoryDocumentTypeViewContainer>();
        private static Timer refreshTimer;

        static InventoryDocumentTypeViewContainerList()
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
            var uniqueKeyList = inventoryDocumentTypeViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                InventoryDocumentTypeViewContainer inventoryDocumentTypeViewContainer;
                if (inventoryDocumentTypeViewContainerCache.TryGetValue(uniqueKey, out inventoryDocumentTypeViewContainer))
                {
                    inventoryDocumentTypeViewContainerCache[uniqueKey] = inventoryDocumentTypeViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static InventoryDocumentTypeViewContainer GetInventoryDocumentTypeViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = inventoryDocumentTypeViewContainerCache.GetOrAdd(siteId, (k) => new InventoryDocumentTypeViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the InventoryDocumentTypeContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// <param name="rebuildCache">hash</param>
        /// <returns></returns>
        public static InventoryDocumentTypeContainerDTOCollection GetInventoryDocumentTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, hash, rebuildCache);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            InventoryDocumentTypeViewContainer languageViewContainer = GetInventoryDocumentTypeViewContainer(siteId);
            InventoryDocumentTypeContainerDTOCollection result = languageViewContainer.GetInventoryDocumentTypeContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            InventoryDocumentTypeViewContainer languageViewContainer = GetInventoryDocumentTypeViewContainer(siteId);
            inventoryDocumentTypeViewContainerCache[siteId] = languageViewContainer.Refresh(true);
            log.LogMethodExit();
        }
    }
}


