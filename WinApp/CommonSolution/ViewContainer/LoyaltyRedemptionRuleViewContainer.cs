/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - LoyaltyRedemptionRuleContainer holds the parafait default values for a given siteId, userId and POSMachineId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    05-Mar-2021      Roshan Devadiga          Created : POS UI Redesign with REST API
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
    class LoyaltyRedemptionRuleViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly LoyaltyRedemptionRuleContainerDTOCollection loyaltyRedemptionRuleContainerDTOCollection;
        private readonly ConcurrentDictionary<int, LoyaltyRedemptionRuleContainerDTO> loyaltyRedemptionRuleContainerDTODictionary = new ConcurrentDictionary<int, LoyaltyRedemptionRuleContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="loyaltyRedemptionRuleContainerDTOCollection">loyaltyRedemptionRuleContainerDTOCollection</param>
        internal LoyaltyRedemptionRuleViewContainer(int siteId, LoyaltyRedemptionRuleContainerDTOCollection loyaltyRedemptionRuleContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, loyaltyRedemptionRuleContainerDTOCollection);
            this.siteId = siteId;
            this.loyaltyRedemptionRuleContainerDTOCollection = loyaltyRedemptionRuleContainerDTOCollection;
            if (loyaltyRedemptionRuleContainerDTOCollection != null &&
                loyaltyRedemptionRuleContainerDTOCollection.LoyaltyRedemptionRuleContainerDTOList != null &&
               loyaltyRedemptionRuleContainerDTOCollection.LoyaltyRedemptionRuleContainerDTOList.Any())
            {
                foreach (var loyaltyRedemptionRuleContainerDTO in loyaltyRedemptionRuleContainerDTOCollection.LoyaltyRedemptionRuleContainerDTOList)
                {
                    loyaltyRedemptionRuleContainerDTODictionary[loyaltyRedemptionRuleContainerDTO.LoyaltyAttributeId] = loyaltyRedemptionRuleContainerDTO;
                }
            }
            log.LogMethodExit();
        }
        internal LoyaltyRedemptionRuleViewContainer(int siteId)
              : this(siteId, GetLoyaltyRedemptionRuleContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        private static LoyaltyRedemptionRuleContainerDTOCollection GetLoyaltyRedemptionRuleContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            LoyaltyRedemptionRuleContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ILoyaltyRedemptionRuleUseCases loyaltyRedemptionRuleUseCases = PromotionUseCaseFactory.GetLoyaltyRedemptionRuleUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<LoyaltyRedemptionRuleContainerDTOCollection> task = loyaltyRedemptionRuleUseCases.GetLoyaltyRedemptionRuleContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving LoyaltyRedemptionRuleContainerDTOCollection.", ex);
                result = new LoyaltyRedemptionRuleContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns the latest in LoyaltyRedemptionRuleContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal LoyaltyRedemptionRuleContainerDTOCollection GetLoyaltyRedemptionRuleContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (loyaltyRedemptionRuleContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(loyaltyRedemptionRuleContainerDTOCollection);
            return loyaltyRedemptionRuleContainerDTOCollection;
        }

        internal List<LoyaltyRedemptionRuleContainerDTO> GetLoyaltyRedemptionRuleContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(loyaltyRedemptionRuleContainerDTOCollection.LoyaltyRedemptionRuleContainerDTOList);
            return loyaltyRedemptionRuleContainerDTOCollection.LoyaltyRedemptionRuleContainerDTOList;
        }
        internal LoyaltyRedemptionRuleViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            LoyaltyRedemptionRuleContainerDTOCollection latestLoyaltyRedemptionRuleContainerDTOCollection = GetLoyaltyRedemptionRuleContainerDTOCollection(siteId, loyaltyRedemptionRuleContainerDTOCollection.Hash, rebuildCache);
            if (latestLoyaltyRedemptionRuleContainerDTOCollection == null ||
                latestLoyaltyRedemptionRuleContainerDTOCollection.LoyaltyRedemptionRuleContainerDTOList == null ||
                latestLoyaltyRedemptionRuleContainerDTOCollection.LoyaltyRedemptionRuleContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            LoyaltyRedemptionRuleViewContainer result = new LoyaltyRedemptionRuleViewContainer(siteId, latestLoyaltyRedemptionRuleContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
