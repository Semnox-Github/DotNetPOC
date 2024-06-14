/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - Holds the Online Order Delivery Integration data
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      13-Jul-2022      Guru S A                   Created
 ********************************************************************************************/ 
using Semnox.Parafait.DeliveryIntegration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// OnlineOrderDeliveryIntegrationViewContainer
    /// </summary>
    public class OnlineOrderDeliveryIntegrationViewContainer: AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly OnlineOrderDeliveryIntegrationContainerDTOCollection deliveryIntegrationDTOCollection;
        private readonly ConcurrentDictionary<int, OnlineOrderDeliveryIntegrationContainerDTO> deliveryIntegrationIdContainerDTODictionary
            = new ConcurrentDictionary<int, OnlineOrderDeliveryIntegrationContainerDTO>();
        private readonly ConcurrentDictionary<string, OnlineOrderDeliveryIntegrationContainerDTO> deliveryIntegrationGuidContainerDTODictionary 
            = new ConcurrentDictionary<string, OnlineOrderDeliveryIntegrationContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// view Container Constructor.
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="deliveryIntegrationDTOCollection"></param>
        internal OnlineOrderDeliveryIntegrationViewContainer(int siteId, OnlineOrderDeliveryIntegrationContainerDTOCollection deliveryIntegrationDTOCollection)
        {
            log.LogMethodEntry(siteId, deliveryIntegrationDTOCollection);
            this.siteId = siteId;
            this.deliveryIntegrationDTOCollection = deliveryIntegrationDTOCollection;
            if (deliveryIntegrationDTOCollection != null &&
               deliveryIntegrationDTOCollection.OnlineOrderDeliveryIntegrationContainerDTOList != null &&
               deliveryIntegrationDTOCollection.OnlineOrderDeliveryIntegrationContainerDTOList.Any())
            {
                foreach (OnlineOrderDeliveryIntegrationContainerDTO containerDTO in deliveryIntegrationDTOCollection.OnlineOrderDeliveryIntegrationContainerDTOList)
                {
                    deliveryIntegrationIdContainerDTODictionary[containerDTO.DeliveryIntegrationId] = containerDTO;
                    deliveryIntegrationGuidContainerDTODictionary[containerDTO.Guid.ToUpper()] = containerDTO;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// View Containe Constructor with SiteId parameter.
        /// </summary>
        /// <param name="siteId"></param>
        internal OnlineOrderDeliveryIntegrationViewContainer(int siteId) :
            this(siteId, GetOnlineOrderDeliveryIntegrationContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the Container DTO collection for the Site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        private static OnlineOrderDeliveryIntegrationContainerDTOCollection GetOnlineOrderDeliveryIntegrationContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, hash, rebuildCache);
            OnlineOrderDeliveryIntegrationContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IOnlineOrderDeliveryIntegrationUseCases deliveryIntegrationUseCases = OnlineOrderDeliveryIntegrationUseCaseFactory.GetOnlineOrderDeliveryIntegrationUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<OnlineOrderDeliveryIntegrationContainerDTOCollection> viewDTOCollectionTask = deliveryIntegrationUseCases.GetOnlineOrderDeliveryIntegrationContainerDTOCollection(siteId, hash, rebuildCache);
                    viewDTOCollectionTask.Wait();
                    result = viewDTOCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving OnlineOrderDeliveryIntegrationContainerDTOCollection.", ex);
                result = new OnlineOrderDeliveryIntegrationContainerDTOCollection();
            } 
            return result;
        }
    }
}
