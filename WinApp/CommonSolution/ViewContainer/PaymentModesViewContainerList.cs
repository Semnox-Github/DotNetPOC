/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - PaymentModesViewContainerList
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      18-Aug-2021      Fiona                    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// PaymentModesViewContainerList
    /// </summary>
    public class PaymentModesViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, PaymentModesViewContainer> PaymentModesViewContainerCache = new Cache<int, PaymentModesViewContainer>();
        private static Timer refreshTimer;
        static PaymentModesViewContainerList()
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
            var uniqueKeyList = PaymentModesViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                PaymentModesViewContainer PaymentModesViewContainer;
                if (PaymentModesViewContainerCache.TryGetValue(uniqueKey, out PaymentModesViewContainer))
                {
                    PaymentModesViewContainerCache[uniqueKey] = PaymentModesViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static PaymentModesViewContainer GetPaymentModesViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            PaymentModesViewContainer result = PaymentModesViewContainerCache.GetOrAdd(siteId, (k) => new PaymentModesViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetPaymentModesContainerDTOCollection
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public static PaymentModesContainerDTOCollection GetPaymentModesContainerDTOCollection(int siteId, string hash,bool rebuildCache)
        {
            log.LogMethodEntry(siteId, hash);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            PaymentModesViewContainer paymentModesViewContainer = GetPaymentModesViewContainer(siteId);
            PaymentModesContainerDTOCollection result = paymentModesViewContainer.GetPaymentModesContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetPaymentModesContainerDTOList
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static List<PaymentModesContainerDTO> GetPaymentModesContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            PaymentModesViewContainer PaymentModesViewContainer = GetPaymentModesViewContainer(executionContext.SiteId);
            List<PaymentModesContainerDTO> result = PaymentModesViewContainer.GetPaymentModesContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="paymentModeId"></param>
        /// <returns></returns>
        public static PaymentModesContainerDTO GetPaymentModesContainerDTO(ExecutionContext executionContext, int paymentModeId)
        {
            log.LogMethodEntry(executionContext);
            PaymentModesViewContainer PaymentModesViewContainer = GetPaymentModesViewContainer(executionContext.SiteId);
            PaymentModesContainerDTO result = PaymentModesViewContainer.GetPaymentModesContainerDTO(paymentModeId);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            PaymentModesViewContainer paymentModesViewContainer = GetPaymentModesViewContainer(siteId);
            PaymentModesViewContainerCache[siteId] = paymentModesViewContainer.Refresh(true);
            log.LogMethodExit();
        }
    }
}
