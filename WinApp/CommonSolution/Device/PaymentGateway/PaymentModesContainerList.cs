/********************************************************************************************
 * Project Name - Device  
 * Description  - PaymentModesContainerList class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      19-Aug-2021      Fiona           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class PaymentModesContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, PaymentModesContainer> paymentModesContainerCache = new Cache<int, PaymentModesContainer>();
        private static Timer refreshTimer;
        static PaymentModesContainerList()
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
            var uniqueKeyList = paymentModesContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                PaymentModesContainer paymentModesContainer;
                if (paymentModesContainerCache.TryGetValue(uniqueKey, out paymentModesContainer))
                {
                    paymentModesContainerCache[uniqueKey] = paymentModesContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static PaymentModesContainer GetPaymentModesContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            PaymentModesContainer result = paymentModesContainerCache.GetOrAdd(siteId, (k) => new PaymentModesContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        public static PaymentModesContainerDTOCollection GetPaymentModeContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            PaymentModesContainer container = GetPaymentModesContainer(siteId);
            PaymentModesContainerDTOCollection result = container.GetPaymentModeContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }
        public static PaymentModesContainerDTO GetPaymentModesContainerDTO(ExecutionContext executionContext, int Id)
        {
            log.LogMethodEntry(executionContext, Id);
            log.LogMethodExit();
            return GetPaymentModesContainerDTO(executionContext.SiteId, Id);
        }
        public static PaymentModesContainerDTO GetPaymentModesContainerDTO(int siteId, int Id)
        {
            log.LogMethodEntry(siteId, Id);
            PaymentModesContainer container = GetPaymentModesContainer(siteId);
            var result = container.GetPaymentModesContainerDTO(Id);
            log.LogMethodExit(result);
            return result;
        }
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            PaymentModesContainer paymentModesContainer = GetPaymentModesContainer(siteId);
            paymentModesContainerCache[siteId] = paymentModesContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
