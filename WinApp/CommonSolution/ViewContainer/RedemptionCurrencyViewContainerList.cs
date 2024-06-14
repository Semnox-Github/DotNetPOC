/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - RedemptionCurrencyViewContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Nov-2020       Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, RedemptionCurrencyViewContainer> redemptionCurrencyViewContainerCache = new Cache<int, RedemptionCurrencyViewContainer>();
        private static Timer refreshTimer;
        static RedemptionCurrencyViewContainerList()
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
            var uniqueKeyList = redemptionCurrencyViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                RedemptionCurrencyViewContainer redemptionCurrencyViewContainer;
                if (redemptionCurrencyViewContainerCache.TryGetValue(uniqueKey, out redemptionCurrencyViewContainer))
                {
                    redemptionCurrencyViewContainerCache[uniqueKey] = redemptionCurrencyViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// return the current RedemptionCurrency container DTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        //public static RedemptionCurrencyContainerDTO GetCurrentRedemptionCurrencyContainerDTO(ExecutionContext executionContext)
        //{
        //    log.LogMethodEntry();
        //    /// Need to change.
        //    RedemptionCurrencyContainerDTO redemptionCurrencyContainerDTO = null;
        //    //= redemptionCurrencyViewContainer.GetRedemptionCurrencyDTOCollection(executionContext.GetSiteId());
        //    log.LogMethodExit(redemptionCurrencyContainerDTO);
        //    return redemptionCurrencyContainerDTO;
        //}

        private static RedemptionCurrencyViewContainer GetRedemptionCurrencyViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            RedemptionCurrencyViewContainer result = redemptionCurrencyViewContainerCache.GetOrAdd(siteId, (k)=> new RedemptionCurrencyViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the value in tickets for the given currency
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        public static double GetValueInTickets(int siteId, int currencyId)
        {
            log.LogMethodEntry(siteId, currencyId);
            RedemptionCurrencyViewContainer container = GetRedemptionCurrencyViewContainer(siteId);
            double result = container.GetValueInTickets(currencyId);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the RedemptionCurrencyContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<RedemptionCurrencyContainerDTO> GetRedemptionCurrencyContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            RedemptionCurrencyViewContainer redemptioncurrencyViewContainer = GetRedemptionCurrencyViewContainer(executionContext.SiteId);
            List<RedemptionCurrencyContainerDTO> result = redemptioncurrencyViewContainer.GetRedemptionCurrencyContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }


        public static RedemptionCurrencyContainerDTOCollection GetRedemptionCurrencyContainerDTOCollection(int siteId, string hash/*, bool rebuildCache*/)
        {
            log.LogMethodEntry(siteId);
            //if (rebuildCache)
            //{
            //    Rebuild(siteId);
            //}
            RedemptionCurrencyViewContainer container = GetRedemptionCurrencyViewContainer(siteId);
            RedemptionCurrencyContainerDTOCollection redemptionCurrencyContainerDTOCollection = container.GetRedemptionCurrencyDTOCollection(hash);
            return redemptionCurrencyContainerDTOCollection;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            RedemptionCurrencyViewContainer container = GetRedemptionCurrencyViewContainer(siteId);
            redemptionCurrencyViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}
