/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - RedemptionCurrencyViewContainer class to get the List of 
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
    public class RedemptionCurrencyViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RedemptionCurrencyContainerDTOCollection redemptionCurrencyDTOCollection;
        private readonly ConcurrentDictionary<int, RedemptionCurrencyContainerDTO> redemptionCurrencyDictionary;
        private readonly int siteId;
        //private DateTime lastRefreshTime;
        //private readonly object locker = new object();

        internal RedemptionCurrencyViewContainer(int siteId, RedemptionCurrencyContainerDTOCollection redemptionCurrencyDTOCollection)
        {
            log.LogMethodEntry(siteId, redemptionCurrencyDTOCollection);
            this.siteId = siteId;
            this.redemptionCurrencyDTOCollection = redemptionCurrencyDTOCollection;
            this.redemptionCurrencyDictionary = new ConcurrentDictionary<int, RedemptionCurrencyContainerDTO>();
            //lastRefreshTime = DateTime.Now;
            if (redemptionCurrencyDTOCollection != null &&
               redemptionCurrencyDTOCollection.RedemptionCurrencyContainerDTOList != null &&
               redemptionCurrencyDTOCollection.RedemptionCurrencyContainerDTOList.Any())
            {
                foreach (var redemptionCurrencyContainerDTO in redemptionCurrencyDTOCollection.RedemptionCurrencyContainerDTOList)
                {
                    redemptionCurrencyDictionary[redemptionCurrencyContainerDTO.CurrencyId] = redemptionCurrencyContainerDTO;
                   // redemptionCurrencyDictionary = new ConcurrentDictionary<int, RedemptionCurrencyContainerDTO>();
                }
            }
            log.LogMethodExit();
        }

        internal RedemptionCurrencyViewContainer(int siteId) :
            this(siteId, GetRedemptionCurrencyContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static RedemptionCurrencyContainerDTOCollection GetRedemptionCurrencyContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {

            log.LogMethodEntry(siteId, hash, rebuildCache);
            RedemptionCurrencyContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IRedemptionCurrencyUseCases redemptionCurrencyUseCases = RedemptionUseCaseFactory.GetRedemptionCurrencyUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<RedemptionCurrencyContainerDTOCollection> redemptionCurrencyViewDTOCollectionTask = redemptionCurrencyUseCases.GetRedemptionCurrencyContainerDTOCollection(siteId, hash, rebuildCache);
                    redemptionCurrencyViewDTOCollectionTask.Wait();
                    result = redemptionCurrencyViewDTOCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving RedemptionCurrencyContainerDTOCollection.", ex);
                result = new RedemptionCurrencyContainerDTOCollection();
            }

            return result;
        }
        internal List<RedemptionCurrencyContainerDTO> GetRedemptionCurrencyContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(redemptionCurrencyDTOCollection.RedemptionCurrencyContainerDTOList);
            return redemptionCurrencyDTOCollection.RedemptionCurrencyContainerDTOList;
        }
        internal double GetValueInTickets(int currencyId)
        {
            if (redemptionCurrencyDictionary.ContainsKey(currencyId) == false)
            {
                string errorMessage = "Currency with currencyId :" + currencyId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            double result = redemptionCurrencyDictionary[currencyId].ValueInTickets;
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns the latest in RedemptionCurrencyDTOCollection
        /// </summary>
        /// <returns></returns>
        internal RedemptionCurrencyContainerDTOCollection GetRedemptionCurrencyDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (redemptionCurrencyDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(redemptionCurrencyDTOCollection);
            return redemptionCurrencyDTOCollection;
        }

        internal RedemptionCurrencyViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            RedemptionCurrencyContainerDTOCollection latestRedemptionCurrencyDTOCollection = GetRedemptionCurrencyContainerDTOCollection(siteId, redemptionCurrencyDTOCollection.Hash, rebuildCache);
            if (latestRedemptionCurrencyDTOCollection == null ||
                latestRedemptionCurrencyDTOCollection.RedemptionCurrencyContainerDTOList == null ||
                latestRedemptionCurrencyDTOCollection.RedemptionCurrencyContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            RedemptionCurrencyViewContainer result = new RedemptionCurrencyViewContainer(siteId, latestRedemptionCurrencyDTOCollection);
            log.LogMethodExit(result);
            return result;
        }

        //private static ExecutionContext GetSystemUserExecutionContext()
        //{
        //    throw new NotImplementedException();
        //}

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
