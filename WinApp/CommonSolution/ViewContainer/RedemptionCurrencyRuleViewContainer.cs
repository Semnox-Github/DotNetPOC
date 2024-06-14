/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - RedemptionCurrencyRuleViewContainer class to get the List of 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyRuleViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RedemptionCurrencyRuleContainerDTOCollection redemptionCurrencyRuleDTOCollection;
        private readonly ConcurrentDictionary<int, RedemptionCurrencyRuleContainerDTO> redemptionCurrencyRuleDictionary;
        private readonly int siteId;
        //private DateTime lastRefreshTime;
        //private readonly object locker = new object();

        internal RedemptionCurrencyRuleViewContainer(int siteId, RedemptionCurrencyRuleContainerDTOCollection redemptionCurrencyRuleDTOCollection)
        {
            log.LogMethodEntry(siteId, redemptionCurrencyRuleDTOCollection);
            this.siteId = siteId;
            this.redemptionCurrencyRuleDTOCollection = redemptionCurrencyRuleDTOCollection;
            this.redemptionCurrencyRuleDictionary = new ConcurrentDictionary<int, RedemptionCurrencyRuleContainerDTO>();
            //lastRefreshTime = DateTime.Now;
            if (redemptionCurrencyRuleDTOCollection != null &&
               redemptionCurrencyRuleDTOCollection.RedemptionCurrencyRuleContainerDTOList != null &&
               redemptionCurrencyRuleDTOCollection.RedemptionCurrencyRuleContainerDTOList.Any())
            {
                foreach (var redemptionCurrencyRuleContainerDTO in redemptionCurrencyRuleDTOCollection.RedemptionCurrencyRuleContainerDTOList)
                {
                    redemptionCurrencyRuleDictionary[redemptionCurrencyRuleContainerDTO.RedemptionCurrencyRuleId] = redemptionCurrencyRuleContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal RedemptionCurrencyRuleViewContainer(int siteId) :
            this(siteId, GetRedemptionCurrencyRuleContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static RedemptionCurrencyRuleContainerDTOCollection GetRedemptionCurrencyRuleContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, hash, rebuildCache);
            RedemptionCurrencyRuleContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IRedemptionCurrencyRuleUseCases redemptionCurrencyRuleUseCases = RedemptionUseCaseFactory.GetRedemptionCurrencyRuleUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<RedemptionCurrencyRuleContainerDTOCollection> redemptionCurrencyRuleViewDTOCollectionTask = redemptionCurrencyRuleUseCases.GetRedemptionCurrencyRuleContainerDTOCollection(siteId, hash, rebuildCache);
                    redemptionCurrencyRuleViewDTOCollectionTask.Wait();
                    result = redemptionCurrencyRuleViewDTOCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving RedemptionCurrencyRuleContainerDTOCollection.", ex);
                result = new RedemptionCurrencyRuleContainerDTOCollection();
            }
            return result;
        }

        /// <summary>
        /// returns the latest in RedemptionCurrencyDTOCollection
        /// </summary>
        /// <returns></returns>
        internal RedemptionCurrencyRuleContainerDTOCollection GetRedemptionCurrencyRuleDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (redemptionCurrencyRuleDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(redemptionCurrencyRuleDTOCollection);
            return redemptionCurrencyRuleDTOCollection;
        }
        internal List<RedemptionCurrencyRuleContainerDTO> GetRedemptionCurrencyRuleContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(redemptionCurrencyRuleDTOCollection.RedemptionCurrencyRuleContainerDTOList);
            return redemptionCurrencyRuleDTOCollection.RedemptionCurrencyRuleContainerDTOList;
        }
        internal RedemptionCurrencyRuleViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            RedemptionCurrencyRuleContainerDTOCollection latestRedemptionCurrencyRuleDTOCollection = GetRedemptionCurrencyRuleContainerDTOCollection(siteId, redemptionCurrencyRuleDTOCollection.Hash, rebuildCache);
            if (latestRedemptionCurrencyRuleDTOCollection == null ||
                latestRedemptionCurrencyRuleDTOCollection.RedemptionCurrencyRuleContainerDTOList == null ||
                latestRedemptionCurrencyRuleDTOCollection.RedemptionCurrencyRuleContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            RedemptionCurrencyRuleViewContainer result = new RedemptionCurrencyRuleViewContainer(siteId, latestRedemptionCurrencyRuleDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
        //private DateTime LastRefreshTime
        //{
        //    get
        //    {
        //        lock (locker)
        //        {
        //            return lastRefreshTime;
        //        }
        //    }

        //    set
        //    {
        //        lock (locker)
        //        {
        //            lastRefreshTime = value;
        //        }
        //    }
        //}
    }
}
