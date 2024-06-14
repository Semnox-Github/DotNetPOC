/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - PaymentModesViewContainer
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      18-Aug-2021      Fiona                     Created 
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentModesViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly PaymentModesContainerDTOCollection paymentModesContainerDTOCollection;
        private readonly ConcurrentDictionary<int, PaymentModesContainerDTO> PaymentModesContainerDTODictionary = new ConcurrentDictionary<int, PaymentModesContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public PaymentModesViewContainer(int siteId)//changed from internal to public
           : this(siteId, GetPaymentModesContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="paymentModesContainerDTOCollection"></param>
        public PaymentModesViewContainer(int siteId, PaymentModesContainerDTOCollection paymentModesContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, paymentModesContainerDTOCollection);
            this.siteId = siteId;
            this.paymentModesContainerDTOCollection = paymentModesContainerDTOCollection;
            if (paymentModesContainerDTOCollection != null &&
                paymentModesContainerDTOCollection.PaymentModesContainerDTOList != null &&
                paymentModesContainerDTOCollection.PaymentModesContainerDTOList.Any())
            {
                foreach (var paymentModesContainerDTO in paymentModesContainerDTOCollection.PaymentModesContainerDTOList)
                {
                    AddToPaymentModesDictionary(paymentModesContainerDTO);
                }
            }
            log.LogMethodExit();
        }

        private void AddToPaymentModesDictionary(PaymentModesContainerDTO paymentModesContainerDTO)
        {
            log.LogMethodEntry();
            PaymentModesContainerDTODictionary[paymentModesContainerDTO.PaymentModeId] = paymentModesContainerDTO;
            log.LogMethodExit();
        }
        private static PaymentModesContainerDTOCollection GetPaymentModesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            PaymentModesContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IPaymentModesUseCases paymentModeUseCases = PaymentModesUseCaseFactory.GetPaymentModesUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<PaymentModesContainerDTOCollection> task = paymentModeUseCases.GetPaymentModesContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving PaymentModesContainerDTOCollection.", ex);
                result = new PaymentModesContainerDTOCollection();
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<PaymentModesContainerDTO> GetPaymentModesContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(paymentModesContainerDTOCollection.PaymentModesContainerDTOList);
            return paymentModesContainerDTOCollection.PaymentModesContainerDTOList;
        }

        internal PaymentModesContainerDTO GetPaymentModesContainerDTO(int paymentModeId)
        {
            log.LogMethodEntry();
            if(PaymentModesContainerDTODictionary.ContainsKey(paymentModeId)==false)
            {
                string errorMessage = "PaymentMode with id :" + paymentModeId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            PaymentModesContainerDTO result = PaymentModesContainerDTODictionary[paymentModeId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetPaymentModesContainerDTOCollection
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public PaymentModesContainerDTOCollection GetPaymentModesContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (paymentModesContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(paymentModesContainerDTOCollection);
            return paymentModesContainerDTOCollection;
        }
        internal PaymentModesViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            PaymentModesContainerDTOCollection latestPaymentModesContainerDTOCollection = GetPaymentModesContainerDTOCollection(siteId, paymentModesContainerDTOCollection.Hash, rebuildCache);
            if (latestPaymentModesContainerDTOCollection == null ||
                latestPaymentModesContainerDTOCollection.PaymentModesContainerDTOList == null ||
                latestPaymentModesContainerDTOCollection.PaymentModesContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            PaymentModesViewContainer result = new PaymentModesViewContainer(siteId, latestPaymentModesContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }

    }
}
