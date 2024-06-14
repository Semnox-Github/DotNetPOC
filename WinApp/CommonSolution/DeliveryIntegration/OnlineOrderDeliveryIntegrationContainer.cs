/********************************************************************************************
 * Project Name - DeliveryIntegration                                                                        
 * Description  - Container class for OnlineOrderDeliveryIntegrationContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************  
  *2.150.0     13-Jul-2022   Guru S A       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// OnlineOrderDeliveryIntegrationContainer
    /// </summary>
    public class OnlineOrderDeliveryIntegrationContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int siteId;
        private readonly DateTime? maxLastUpdateTime;
        private readonly List<OnlineOrderDeliveryIntegrationDTO> onlineOrderDeliveryIntegrationDTOList;
        //private readonly List<DeliveryChannelDTO> deliveryChannelDTOList;
        private readonly Dictionary<int, OnlineOrderDeliveryIntegrationDTO> deliveryIntegrationIdAndDTODictionary = new Dictionary<int, OnlineOrderDeliveryIntegrationDTO>();
        //private readonly Dictionary<int, DeliveryChannelDTO> deliveryChannelIdAndDTODictionary = new Dictionary<int, DeliveryChannelDTO>();
        //private readonly Dictionary<string, DeliveryChannelDTO> deliveryChannelGuidAndDTODictionary = new Dictionary<string, DeliveryChannelDTO>();
        private readonly Dictionary<int, OnlineOrderDeliveryIntegrationContainerDTO> deliveryIntegrationContainerDTODictionary = new Dictionary<int, OnlineOrderDeliveryIntegrationContainerDTO>();
        private readonly Dictionary<string, OnlineOrderDeliveryIntegrationContainerDTO> deliveryIntegrationGuidAndContainerDTODictionary = new Dictionary<string, OnlineOrderDeliveryIntegrationContainerDTO>(); 
        private OnlineOrderDeliveryIntegrationContainerDTOCollection deliveryIntegrationContainerDTOCollection; 


        /// <summary>
        /// OnlineOrderDeliveryIntegrationContainer
        /// </summary>
        /// <param name="siteId"></param> 
        public OnlineOrderDeliveryIntegrationContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.maxLastUpdateTime = GetMaxLastUpdateTime(siteId);
            this.onlineOrderDeliveryIntegrationDTOList = GetOnlineOrderDeliveryIntegrationDTOList(siteId);
            BuildContainerDTOList(); 
            log.LogMethodExit();
        } 

        private static DateTime? GetMaxLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                OnlineOrderDeliveryIntegrationListBL onlineOrderDeliveryIntegrationListBL = new OnlineOrderDeliveryIntegrationListBL();
                result = onlineOrderDeliveryIntegrationListBL.GetDeliveryIntegrationModuleLastUpdateTime(siteId); 
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the online Order Delivery Integration max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<OnlineOrderDeliveryIntegrationDTO> GetOnlineOrderDeliveryIntegrationDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<OnlineOrderDeliveryIntegrationDTO> result = null;
            try
            {
                OnlineOrderDeliveryIntegrationListBL onlineOrderDeliveryIntegrationListBL = new OnlineOrderDeliveryIntegrationListBL();
                List<KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>> searchParameters = 
                                                                             new List<KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, 
                                                                             string>(OnlineOrderDeliveryIntegrationDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, 
                                                                             string>(OnlineOrderDeliveryIntegrationDTO.SearchByParameters.IS_ACTIVE, "1"));
                result = onlineOrderDeliveryIntegrationListBL.GetOnlineOrderDeliveryIntegrationDTOList(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the Online Order Delivery Integration.", ex);
            }

            if (result == null)
            {
                result = new List<OnlineOrderDeliveryIntegrationDTO>();
            }
            log.LogMethodExit(result);
            return result;
        }
        protected void BuildContainerDTOList()
        {
            log.LogMethodEntry();
            deliveryIntegrationContainerDTODictionary.Clear();
            deliveryIntegrationGuidAndContainerDTODictionary.Clear();
            List<OnlineOrderDeliveryIntegrationContainerDTO> containerDTOList = new List<OnlineOrderDeliveryIntegrationContainerDTO>();
            foreach (OnlineOrderDeliveryIntegrationDTO integrationDTO in onlineOrderDeliveryIntegrationDTOList)
            {

                OnlineOrderDeliveryIntegrationContainerDTO integrationContainerDTO = GetOnlineOrderDeliveryIntegrationContainerDTO(integrationDTO);
                deliveryIntegrationContainerDTODictionary.Add(integrationDTO.DeliveryIntegrationId, integrationContainerDTO);
                deliveryIntegrationGuidAndContainerDTODictionary.Add(integrationDTO.Guid, integrationContainerDTO);
                containerDTOList.Add(integrationContainerDTO);
            }
            deliveryIntegrationContainerDTOCollection = new OnlineOrderDeliveryIntegrationContainerDTOCollection(containerDTOList);
            log.LogMethodExit();
        }
        private OnlineOrderDeliveryIntegrationContainerDTO GetOnlineOrderDeliveryIntegrationContainerDTO(OnlineOrderDeliveryIntegrationDTO integrationlDTO)
        {
            log.LogMethodEntry(integrationlDTO);
            OnlineOrderDeliveryIntegrationContainerDTO result = new OnlineOrderDeliveryIntegrationContainerDTO(integrationlDTO.DeliveryIntegrationId, 
                integrationlDTO.IntegrationName, integrationlDTO.SemnoxWebhookAPIAuthorizationKey, integrationlDTO.IntegratorAPIAuthorizationKey, 
                integrationlDTO.IntegratorAPIBaseURL, integrationlDTO.StoreCreationCallBackURL, integrationlDTO.StoreActionsCallBackURL,
            integrationlDTO.CatalogueIngestionCallBackURL, integrationlDTO.ItemActionsCallBackURL, integrationlDTO.ItemOptionActionsCallBackURL, integrationlDTO.OrderRelayCallBackURL, integrationlDTO.OrderStatusChangeCallBackURL, integrationlDTO.RiderStatusChangeCallBackURL,
            integrationlDTO.FullfillmentModes, integrationlDTO.PackagingChargeProductName, integrationlDTO.PackagingChargePercentage,
            integrationlDTO.PaymentModeName, integrationlDTO.AggregatorDiscountName, integrationlDTO.ProductTaxNameList, integrationlDTO.MinimumInventoryQtyForItems,
            integrationlDTO.FoodTypesSegmentName, integrationlDTO.GoodsTypeSegmentName, integrationlDTO.TooManyRequestErrorCode, integrationlDTO.PackageChargeCode,
            integrationlDTO.PackageChargeApplicableOn, integrationlDTO.IsActive, integrationlDTO.Guid);
            result.DeliveryChannelContainerDTOList = new List<DeliveryChannelContainerDTO>();
            if (integrationlDTO.DeliveryChannelDTOList != null && integrationlDTO.DeliveryChannelDTOList.Any())
            {
                foreach (DeliveryChannelDTO deliveryChannelDTO in integrationlDTO.DeliveryChannelDTOList)
                {
                    DeliveryChannelContainerDTO deliveryChannelContainerDTO =
                        new DeliveryChannelContainerDTO(deliveryChannelDTO.DeliveryChannelId, deliveryChannelDTO.DeliveryIntegrationId, deliveryChannelDTO.ChannelName,
                        deliveryChannelDTO.ChannelAPIUrl, deliveryChannelDTO.ChannelAPIKey, deliveryChannelDTO.AutoAcceptOrders,
                        deliveryChannelDTO.ManualRiderAssignmentAllowed, deliveryChannelDTO.ReConfirmOrder, deliveryChannelDTO.ReConfirmPreparation,
                        deliveryChannelDTO.DefaultRiderId, deliveryChannelDTO.ExternalSourceReference, deliveryChannelDTO.IsActive, deliveryChannelDTO.Guid);
                    result.DeliveryChannelContainerDTOList.Add(deliveryChannelContainerDTO);

                }
            }
            log.LogMethodExit(result);
            return result;
        }

        public OnlineOrderDeliveryIntegrationContainerDTO GetOnlineOrderDeliveryIntegrationContainerDTOOrDefault(int deliveryIntegrationId)
        {
            log.LogMethodEntry(deliveryIntegrationId); 
            if (deliveryIntegrationContainerDTODictionary.ContainsKey(deliveryIntegrationId) == false)
            {
                string message = "OnlineOrderDeliveryIntegration with id : " + deliveryIntegrationId + " doesn't exist.";
                log.LogMethodExit(null, message);
                return null;
            }
            OnlineOrderDeliveryIntegrationContainerDTO result = deliveryIntegrationContainerDTODictionary[deliveryIntegrationId];
            return result; 
        }

        public OnlineOrderDeliveryIntegrationContainerDTO GetOnlineOrderDeliveryIntegrationContainerDTOOrDefault(string deliveryIntegrationguid)
        {
            log.LogMethodEntry(deliveryIntegrationguid);
            if (deliveryIntegrationGuidAndContainerDTODictionary.ContainsKey(deliveryIntegrationguid) == false)
            {
                string message = "OnlineOrderDeliveryIntegration with Guid : " + deliveryIntegrationguid + " doesn't exist.";
                log.LogMethodExit(null, message);
                return null;
            }
            OnlineOrderDeliveryIntegrationContainerDTO result = deliveryIntegrationGuidAndContainerDTODictionary[deliveryIntegrationguid];
            return result;
        }


        /// <summary>
        /// Returns paymentModeContainerDTOCollection.
        /// </summary>
        /// <returns></returns>
        public OnlineOrderDeliveryIntegrationContainerDTOCollection GetOnlineOrderDeliveryIntegrationContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(deliveryIntegrationContainerDTOCollection);
            return deliveryIntegrationContainerDTOCollection;
        }

        //private static List<OnlineOrderDeliveryIntegrationContainerDTO> GetOnlineOrderDeliveryIntegrationContainerDTOList(int siteId)
        //{
        //    log.LogMethodEntry(siteId);
        //    List<OnlineOrderDeliveryIntegrationContainerDTO> result = OnlineOrderDeliveryIntegrationContainerDTOList.GetOnlineOrderDeliveryIntegrationContainerDTOList(siteId);
        //    log.LogMethodExit(result);
        //    return result;
        //}
        /// <summary>
        /// Refresh OnlineOrderDeliveryIntegrationContainer()
        /// </summary>
        /// <returns></returns>
        public OnlineOrderDeliveryIntegrationContainer Refresh()
        {
            log.LogMethodEntry(); 
            DateTime? updateTime = GetMaxLastUpdateTime(siteId);
            if (maxLastUpdateTime.HasValue
                && maxLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Online Order Delivery Integration since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            OnlineOrderDeliveryIntegrationContainerList.Rebuild(siteId);
            OnlineOrderDeliveryIntegrationContainer result = new OnlineOrderDeliveryIntegrationContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }

    }
}
