/********************************************************************************************
 * Project Name - Inventory
 * Description  - InventoryDocumentTypeViewContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      18-Aug-2022      Abhishek           Created : Web Inventory Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.Parafait.ViewContainer
{
    public class InventoryDocumentTypeViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly InventoryDocumentTypeContainerDTOCollection inventoryDocumentTypeContainerDTOCollection;
        private readonly ConcurrentDictionary<int, InventoryDocumentTypeContainerDTO> inventoryDocumentTypeDictionary;
        private readonly int siteId;

        internal InventoryDocumentTypeViewContainer(int siteId, InventoryDocumentTypeContainerDTOCollection inventoryDocumentTypeContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, inventoryDocumentTypeContainerDTOCollection);
            this.siteId = siteId;
            this.inventoryDocumentTypeContainerDTOCollection = inventoryDocumentTypeContainerDTOCollection;
            this.inventoryDocumentTypeDictionary = new ConcurrentDictionary<int, InventoryDocumentTypeContainerDTO>();
            if (inventoryDocumentTypeContainerDTOCollection != null &&
               inventoryDocumentTypeContainerDTOCollection.InventoryDocumentTypeContainerDTOList != null &&
               inventoryDocumentTypeContainerDTOCollection.InventoryDocumentTypeContainerDTOList.Any())
            {
                foreach (var inventoryDocumentTypeContainerDTO in inventoryDocumentTypeContainerDTOCollection.InventoryDocumentTypeContainerDTOList)
                {
                    inventoryDocumentTypeDictionary[inventoryDocumentTypeContainerDTO.DocumentTypeId] = inventoryDocumentTypeContainerDTO;
                }
            }
            log.Info("Number of items loaded by InventoryDocumentTypeViewContainer " + siteId + ":" + inventoryDocumentTypeContainerDTOCollection.InventoryDocumentTypeContainerDTOList.Count);
            log.LogMethodExit();
        }

        internal InventoryDocumentTypeViewContainer(int siteId) :
            this(siteId, GetInventoryDocumentTypeContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        internal List<InventoryDocumentTypeContainerDTO> GetInventoryDocumentTypeContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(inventoryDocumentTypeContainerDTOCollection.InventoryDocumentTypeContainerDTOList);
            return inventoryDocumentTypeContainerDTOCollection.InventoryDocumentTypeContainerDTOList;
        }

        private static InventoryDocumentTypeContainerDTOCollection GetInventoryDocumentTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {

            log.LogMethodEntry(siteId, hash, rebuildCache);
            InventoryDocumentTypeContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IApprovalRuleUseCases approvalRuleUseCases = InventoryUseCaseFactory.GetApprovalRuleUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<InventoryDocumentTypeContainerDTOCollection> inventoryDocumentTypeViewCollectionTask = approvalRuleUseCases.GetInventoryDocumentTypeContainerDTOCollection(siteId, hash, rebuildCache);
                    inventoryDocumentTypeViewCollectionTask.Wait();
                    result = inventoryDocumentTypeViewCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving InventoryDocumentTypeContainerDTOCollection.", ex);
                result = new InventoryDocumentTypeContainerDTOCollection();
            }

            return result;
        }

        /// <summary>
        /// returns the latest in InventoryDocumentTypeContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal InventoryDocumentTypeContainerDTOCollection GetInventoryDocumentTypeContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (inventoryDocumentTypeContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(inventoryDocumentTypeContainerDTOCollection);
            return inventoryDocumentTypeContainerDTOCollection;
        }

        internal InventoryDocumentTypeViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            InventoryDocumentTypeContainerDTOCollection latestInventoryDocumentTypeDTOCollection = GetInventoryDocumentTypeContainerDTOCollection(siteId, inventoryDocumentTypeContainerDTOCollection.Hash, rebuildCache);
            if (latestInventoryDocumentTypeDTOCollection == null ||
                latestInventoryDocumentTypeDTOCollection.InventoryDocumentTypeContainerDTOList == null ||
                latestInventoryDocumentTypeDTOCollection.InventoryDocumentTypeContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            InventoryDocumentTypeViewContainer result = new InventoryDocumentTypeViewContainer(siteId, latestInventoryDocumentTypeDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
