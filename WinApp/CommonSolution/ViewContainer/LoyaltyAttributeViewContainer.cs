/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - LoyaltyAttributeViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    04-Mar-2021      Roshan Devadiga          Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Promotions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// LoyaltyAttributeViewContainer
    /// </summary>
    public class LoyaltyAttributeViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly LoyaltyAttributeContainerDTOCollection loyaltyAttributeContainerDTOCollection;
        private readonly ConcurrentDictionary<int, LoyaltyAttributeContainerDTO> loyaltyAttributeContainerDTODictionary = new ConcurrentDictionary<int, LoyaltyAttributeContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="loyaltyAttributeContainerDTOCollection">loyaltyAttributeContainerDTOCollection</param>
        internal LoyaltyAttributeViewContainer(int siteId, LoyaltyAttributeContainerDTOCollection loyaltyAttributeContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, loyaltyAttributeContainerDTOCollection);
            this.siteId = siteId;
            this.loyaltyAttributeContainerDTOCollection = loyaltyAttributeContainerDTOCollection;
            if (loyaltyAttributeContainerDTOCollection != null &&
                loyaltyAttributeContainerDTOCollection.LoyaltyAttributeContainerDTOList != null &&
                loyaltyAttributeContainerDTOCollection.LoyaltyAttributeContainerDTOList.Any())
            {
                foreach (var loyaltyAttributeContainerDTO in loyaltyAttributeContainerDTOCollection.LoyaltyAttributeContainerDTOList)
                {
                    loyaltyAttributeContainerDTODictionary[loyaltyAttributeContainerDTO.LoyaltyAttributeId] = loyaltyAttributeContainerDTO;
                }
            }
            log.LogMethodExit();
        }
        internal LoyaltyAttributeViewContainer(int siteId)
              : this(siteId, GetLoyaltyAttributeContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        private static LoyaltyAttributeContainerDTOCollection GetLoyaltyAttributeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            LoyaltyAttributeContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ILoyaltyAttributeUseCases loyaltyAttributeUseCases =PromotionUseCaseFactory.GetLoyaltyAttributeUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<LoyaltyAttributeContainerDTOCollection> task = loyaltyAttributeUseCases.GetLoyaltyAttributeContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving LoyaltyAttributeContainerDTOCollection.", ex);
                result = new LoyaltyAttributeContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns the latest in LoyaltyAttributeContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal LoyaltyAttributeContainerDTOCollection GetLoyaltyAttributeContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (loyaltyAttributeContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(loyaltyAttributeContainerDTOCollection);
            return loyaltyAttributeContainerDTOCollection;
        }
        internal List<LoyaltyAttributeContainerDTO> GetLoyaltyAttributeContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(loyaltyAttributeContainerDTOCollection.LoyaltyAttributeContainerDTOList);
            return loyaltyAttributeContainerDTOCollection.LoyaltyAttributeContainerDTOList;
        }
        internal LoyaltyAttributeViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            LoyaltyAttributeContainerDTOCollection latestLoyaltyAttributeContainerDTOCollection = GetLoyaltyAttributeContainerDTOCollection(siteId, loyaltyAttributeContainerDTOCollection.Hash, rebuildCache);
            if (latestLoyaltyAttributeContainerDTOCollection == null ||
                latestLoyaltyAttributeContainerDTOCollection.LoyaltyAttributeContainerDTOList == null ||
                latestLoyaltyAttributeContainerDTOCollection.LoyaltyAttributeContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            LoyaltyAttributeViewContainer result = new LoyaltyAttributeViewContainer(siteId, latestLoyaltyAttributeContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }

    }
}
