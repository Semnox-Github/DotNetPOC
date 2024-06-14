/********************************************************************************************
 * Project Name - Inventory
 * Description  - InventoryDocumentTypeContainerList class to get the List of Document Type from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.150.0     18-Aug-2022      Abhishek           Created : Web Inventory Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class InventoryDocumentTypeContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, InventoryDocumentTypeContainer> inventoryDocumentTypeContainerCache = new Cache<int, InventoryDocumentTypeContainer>();
        private static Timer refreshTimer;

        static InventoryDocumentTypeContainerList()
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
            var uniqueKeyList = inventoryDocumentTypeContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                InventoryDocumentTypeContainer inventoryDocumentTypeContainer;
                if (inventoryDocumentTypeContainerCache.TryGetValue(uniqueKey, out inventoryDocumentTypeContainer))
                {
                    inventoryDocumentTypeContainerCache[uniqueKey] = inventoryDocumentTypeContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static InventoryDocumentTypeContainer GetInventoryDocumentTypeContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            InventoryDocumentTypeContainer result = inventoryDocumentTypeContainerCache.GetOrAdd(siteId, (k) => new InventoryDocumentTypeContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static InventoryDocumentTypeContainerDTOCollection GetInventoryDocumentTypeContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            InventoryDocumentTypeContainer container = GetInventoryDocumentTypeContainer(siteId);
            InventoryDocumentTypeContainerDTOCollection result = container.GetInventoryDocumentTypeContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            InventoryDocumentTypeContainer inventoryDocumentTypeContainer = GetInventoryDocumentTypeContainer(siteId);
            inventoryDocumentTypeContainerCache[siteId] = inventoryDocumentTypeContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the InventoryDocumentTypeContainerDTO based on the site and documentTypeId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="documentTypeId">documentTypeId</param>
        /// <returns>InventoryDocumentTypeContainerDTO</returns>
        public static InventoryDocumentTypeContainerDTO GetInventoryDocumentTypeContainerDTO(int siteId, int documentTypeId)
        {
            log.LogMethodEntry(siteId, documentTypeId);
            InventoryDocumentTypeContainer inventoryDocumentTypeContainer = GetInventoryDocumentTypeContainer(siteId);
            InventoryDocumentTypeContainerDTO result = inventoryDocumentTypeContainer.GetInventoryDocumentTypeContainerDTO(documentTypeId);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the InventoryDocumentTypeContainerDTO based on the execution context and documentTypeId
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="documentTypeId">documentTypeId</param>
        /// <returns>InventoryDocumentTypeContainerDTO</returns>
        public static InventoryDocumentTypeContainerDTO GetInventoryDocumentTypeContainerDTO(ExecutionContext executionContext, int documentTypeId)
        {
            log.LogMethodEntry(executionContext, documentTypeId);
            InventoryDocumentTypeContainerDTO inventoryDocumentTypeContainerDTO = GetInventoryDocumentTypeContainerDTO(executionContext.GetSiteId(), documentTypeId);
            log.LogMethodExit(inventoryDocumentTypeContainerDTO);
            return inventoryDocumentTypeContainerDTO;
        }
    }
}
