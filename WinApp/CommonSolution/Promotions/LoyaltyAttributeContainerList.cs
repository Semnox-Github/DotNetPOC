/********************************************************************************************
 * Project Name - Achievements
 * Description  - LoyaltyAttributeMasterList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00     04-Mar-2021      Roshan Devadiga         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Semnox.Parafait.Promotions
{
    public class LoyaltyAttributeContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, LoyaltyAttributeContainer> loyaltyAttributeContainerDictionary = new Cache<int, LoyaltyAttributeContainer>();
        private static Timer refreshTimer;

        static LoyaltyAttributeContainerList()
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
            var uniqueKeyList = loyaltyAttributeContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                LoyaltyAttributeContainer loyaltyAttributeContainer;
                if (loyaltyAttributeContainerDictionary.TryGetValue(uniqueKey, out loyaltyAttributeContainer))
                {
                    loyaltyAttributeContainerDictionary[uniqueKey] = loyaltyAttributeContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static LoyaltyAttributeContainer GetLoyaltyAttributeContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            LoyaltyAttributeContainer result = loyaltyAttributeContainerDictionary.GetOrAdd(siteId, (k) => new LoyaltyAttributeContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        public static List<LoyaltyAttributeContainerDTO> GetLoyaltyAttributeContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            LoyaltyAttributeContainer container = GetLoyaltyAttributeContainer(siteId);
            List<LoyaltyAttributeContainerDTO> loyaltyAttributeContainerDTOList = container.GetLoyaltyAttributeContainerDTOList();
            log.LogMethodExit(loyaltyAttributeContainerDTOList);
            return loyaltyAttributeContainerDTOList;
        }

        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            LoyaltyAttributeContainer redemptionCurrencyContainer = GetLoyaltyAttributeContainer(siteId);
            loyaltyAttributeContainerDictionary[siteId] = redemptionCurrencyContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
