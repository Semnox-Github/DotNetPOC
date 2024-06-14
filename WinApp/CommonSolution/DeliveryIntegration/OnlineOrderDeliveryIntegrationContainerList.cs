/********************************************************************************************
 * Project Name - DeliveryIntegration                                                                        
 * Description  - Container list class for OnlineOrderDeliveryIntegrationContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************  
  *2.150.0     13-Jul-2022   Guru S A       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// OnlineOrderDeliveryIntegrationContainerList
    /// </summary>
    public class OnlineOrderDeliveryIntegrationContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, OnlineOrderDeliveryIntegrationContainer> deliveryIntegrationContainerCache = new Cache<int, OnlineOrderDeliveryIntegrationContainer>();
        private static Timer refreshTimer;

        static OnlineOrderDeliveryIntegrationContainerList()
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
            var uniqueKeyList = deliveryIntegrationContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                OnlineOrderDeliveryIntegrationContainer deliveryIntegrationContainer;
                if (deliveryIntegrationContainerCache.TryGetValue(uniqueKey, out deliveryIntegrationContainer))
                {
                    deliveryIntegrationContainerCache[uniqueKey] = deliveryIntegrationContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static OnlineOrderDeliveryIntegrationContainer GetOnlineOrderDeliveryIntegrationContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            OnlineOrderDeliveryIntegrationContainer result = deliveryIntegrationContainerCache.GetOrAdd(siteId, (k) => new OnlineOrderDeliveryIntegrationContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static OnlineOrderDeliveryIntegrationContainerDTOCollection GetOnlineOrderDeliveryIntegrationContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            OnlineOrderDeliveryIntegrationContainer container = new OnlineOrderDeliveryIntegrationContainer(siteId);
            OnlineOrderDeliveryIntegrationContainerDTOCollection result = container.GetOnlineOrderDeliveryIntegrationContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            OnlineOrderDeliveryIntegrationContainer deliveryIntegrationContainer = GetOnlineOrderDeliveryIntegrationContainer(siteId);
            deliveryIntegrationContainerCache[siteId] = deliveryIntegrationContainer.Refresh();
            log.LogMethodExit();
        } 
        public static OnlineOrderDeliveryIntegrationContainerDTO GetOnlineOrderDeliveryIntegrationContainerDTOOrDefault(int siteId, int deliveryIntegrationId)
        {
            log.LogMethodEntry(siteId, deliveryIntegrationId);
            OnlineOrderDeliveryIntegrationContainer container = GetOnlineOrderDeliveryIntegrationContainer(siteId);
            var result = container.GetOnlineOrderDeliveryIntegrationContainerDTOOrDefault(deliveryIntegrationId);
            log.LogMethodExit(result);
            return result;
        } 
        public static OnlineOrderDeliveryIntegrationContainerDTO GetOnlineOrderDeliveryIntegrationContainerDTOOrDefault(int siteId, string deliveryIntergrationGuid)
        {
            log.LogMethodEntry(siteId, deliveryIntergrationGuid);
            OnlineOrderDeliveryIntegrationContainer container = GetOnlineOrderDeliveryIntegrationContainer(siteId);
            var result = container.GetOnlineOrderDeliveryIntegrationContainerDTOOrDefault(deliveryIntergrationGuid);
            log.LogMethodExit(result);
            return result;
        }
    }
}
