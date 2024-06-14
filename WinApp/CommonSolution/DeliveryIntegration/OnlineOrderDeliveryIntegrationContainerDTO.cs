/********************************************************************************************
 * Project Name - Semnox.Parafait.DeliveryIntegration 
 * Description  - Container object of OnlineOrderDeliveryIntegration  
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 2.150.0      11-Jul-2022   Guru S A       Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// OnlineOrderDeliveryIntegrationContainerDTO
    /// </summary>
    public class OnlineOrderDeliveryIntegrationContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                
        private int deliveryIntegrationId;
        private string integrationName;
        private string semnoxWebhookAPIAuthorizationKey;
        private string integratorAPIAuthorizationKey;
        private string integratorAPIBaseURL;
        private string storeCreationCallBackURL;
        private string storeActionsCallBackURL;
        private string catalogueIngestionCallBackURL;
        private string itemActionsCallBackURL;
        private string itemOptionActionsCallBackURL;
        private string orderRelayCallBackURL;
        private string orderStatusChangeCallBackURL;
        private string riderStatusChangeCallBackURL;
        private string fullfillmentModes;
        private string packagingChargeProductName;
        private decimal packagingChargePercentage;
        private string paymentModeName;
        private string aggregatorDiscountName;
        private string productTaxNameList;
        private decimal minimumInventoryQtyForItems;
        private string foodTypesSegmentName;
        private string goodsTypeSegmentName;
        private string tooManyRequestErrorCode;
        private string packageChargeCode;
        private string packageChargeApplicableOn;
        private bool isActive;
        private string guid;
        private List<DeliveryChannelContainerDTO> deliveryChannelContainerDTOList;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public OnlineOrderDeliveryIntegrationContainerDTO()
        {
            log.LogMethodEntry();
            deliveryIntegrationId = -1;  
            deliveryChannelContainerDTOList = new List<DeliveryChannelContainerDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Paramter constructor
        /// </summary>
        public OnlineOrderDeliveryIntegrationContainerDTO(int deliveryIntegrationId, string integrationName, string semnoxWebhookAPIAuthorizationKey,
            string integratorAPIAuthorizationKey, string integratorAPIBaseURL, string storeCreationCallBackURL, string storeActionsCallBackURL,
            string catalogueIngestionCallBackURL, string itemActionsCallBackURL, string itemOptionActionsCallBackURL, string orderRelayCallBackURL,
            string orderStatusChangeCallBackURL, string riderStatusChangeCallBackURL, string fullfillmentModes, string packagingChargeProductName, 
            decimal packagingChargePercentage, string paymentModeName, string aggregatorDiscountName, string productTaxNameList, 
            decimal minimumInventoryQtyForItems, string foodTypesSegmentName, string goodsTypeSegmentName, string tooManyRequestErrorCode, 
            string packageChargeCode, string packageChargeApplicableOn, bool isActive, string guid) : this()
        {
            log.LogMethodEntry(deliveryIntegrationId, integrationName, semnoxWebhookAPIAuthorizationKey, integratorAPIAuthorizationKey, integratorAPIBaseURL, storeCreationCallBackURL, storeActionsCallBackURL, catalogueIngestionCallBackURL, itemActionsCallBackURL, itemOptionActionsCallBackURL, orderRelayCallBackURL, orderStatusChangeCallBackURL, riderStatusChangeCallBackURL, fullfillmentModes, 
                packagingChargeProductName, packagingChargePercentage, paymentModeName, aggregatorDiscountName, productTaxNameList, 
                minimumInventoryQtyForItems, foodTypesSegmentName, goodsTypeSegmentName, tooManyRequestErrorCode, packageChargeCode,
                packageChargeApplicableOn, isActive, guid);
            this.deliveryIntegrationId = deliveryIntegrationId;
            this.integrationName = integrationName;
            this.semnoxWebhookAPIAuthorizationKey = semnoxWebhookAPIAuthorizationKey;
            this.integratorAPIAuthorizationKey = integratorAPIAuthorizationKey;
            this.integratorAPIBaseURL = integratorAPIBaseURL;
            this.storeCreationCallBackURL = storeCreationCallBackURL;
            this.storeActionsCallBackURL = storeActionsCallBackURL;
            this.catalogueIngestionCallBackURL = catalogueIngestionCallBackURL;
            this.itemActionsCallBackURL = itemActionsCallBackURL;
            this.itemOptionActionsCallBackURL = itemOptionActionsCallBackURL;
            this.orderRelayCallBackURL = orderRelayCallBackURL;
            this.orderStatusChangeCallBackURL = orderStatusChangeCallBackURL;
            this.riderStatusChangeCallBackURL = riderStatusChangeCallBackURL;
            this.fullfillmentModes = fullfillmentModes;
            this.packagingChargeProductName = packagingChargeProductName;
            this.packagingChargePercentage = packagingChargePercentage;
            this.paymentModeName = paymentModeName;
            this.aggregatorDiscountName = aggregatorDiscountName;
            this.productTaxNameList = productTaxNameList;
            this.minimumInventoryQtyForItems = minimumInventoryQtyForItems;
            this.foodTypesSegmentName = foodTypesSegmentName;
            this.goodsTypeSegmentName = goodsTypeSegmentName;
            this.tooManyRequestErrorCode = tooManyRequestErrorCode;
            this.packageChargeCode = packageChargeCode;
            this.packageChargeApplicableOn = packageChargeApplicableOn;
            this.isActive = isActive;
            this.guid = guid;
            log.LogMethodExit();
        }
        /// <summary>
        /// Copy constructor
        /// </summary>
        public OnlineOrderDeliveryIntegrationContainerDTO(OnlineOrderDeliveryIntegrationContainerDTO onlineDelIntContDTO) 
            : this(onlineDelIntContDTO.DeliveryIntegrationId, onlineDelIntContDTO.IntegrationName, onlineDelIntContDTO.SemnoxWebhookAPIAuthorizationKey,
                  onlineDelIntContDTO.IntegratorAPIAuthorizationKey, onlineDelIntContDTO.IntegratorAPIBaseURL, onlineDelIntContDTO.StoreCreationCallBackURL,
                  onlineDelIntContDTO.StoreActionsCallBackURL, onlineDelIntContDTO.CatalogueIngestionCallBackURL, onlineDelIntContDTO.ItemActionsCallBackURL,
                  onlineDelIntContDTO.ItemOptionActionsCallBackURL, onlineDelIntContDTO.OrderRelayCallBackURL, onlineDelIntContDTO.OrderStatusChangeCallBackURL,
                  onlineDelIntContDTO.RiderStatusChangeCallBackURL, onlineDelIntContDTO.FullfillmentModes, onlineDelIntContDTO.packagingChargeProductName,
                  onlineDelIntContDTO.PackagingChargePercentage, onlineDelIntContDTO.paymentModeName, onlineDelIntContDTO.aggregatorDiscountName,
                  onlineDelIntContDTO.ProductTaxNameList, onlineDelIntContDTO.MinimumInventoryQtyForItems, onlineDelIntContDTO.FoodTypesSegmentName,
                  onlineDelIntContDTO.GoodsTypeSegmentName, onlineDelIntContDTO.TooManyRequestErrorCode, onlineDelIntContDTO.PackageChargeCode,
                  onlineDelIntContDTO.PackageChargeApplicableOn, onlineDelIntContDTO.IsActive, onlineDelIntContDTO.Guid)
        {
            log.LogMethodEntry(onlineDelIntContDTO); 
            if (onlineDelIntContDTO.deliveryChannelContainerDTOList != null  && onlineDelIntContDTO.deliveryChannelContainerDTOList.Any())
            {
                this.deliveryChannelContainerDTOList = new List<DeliveryChannelContainerDTO>();
                for (int i = 0; i < onlineDelIntContDTO.deliveryChannelContainerDTOList.Count; i++)
                {
                    DeliveryChannelContainerDTO containerDTO = new DeliveryChannelContainerDTO(onlineDelIntContDTO.deliveryChannelContainerDTOList[i]);
                    this.deliveryChannelContainerDTOList.Add(containerDTO);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Set and get for DeliveryIntegrationId
        /// </summary>
        public int DeliveryIntegrationId { get { return deliveryIntegrationId; } set { deliveryIntegrationId = value; } }
        /// <summary>
        /// Set and get for IntegrationName
        /// </summary>
        public string IntegrationName {  get { return  integrationName; } set {  integrationName = value; } }
        /// <summary>
        /// Set and get for SemnoxWebhookAPIAuthorizationKey
        /// </summary>
        public string SemnoxWebhookAPIAuthorizationKey { get { return semnoxWebhookAPIAuthorizationKey; } set { semnoxWebhookAPIAuthorizationKey = value; } }
        /// <summary>
        /// Set and get for IntegratorAPIAuthorizationKey
        /// </summary>
        public string IntegratorAPIAuthorizationKey { get { return integratorAPIAuthorizationKey; } set { integratorAPIAuthorizationKey = value; } }
        /// <summary>
        /// Set and get for IntegratorAPIBaseURL
        /// </summary>
        public string IntegratorAPIBaseURL { get { return integratorAPIBaseURL; } set { integratorAPIBaseURL = value; } }
        /// <summary>
        /// Set and get for StoreCreationCallBackURL
        /// </summary>
        public string StoreCreationCallBackURL { get { return storeCreationCallBackURL; } set { storeCreationCallBackURL = value; } }
        /// <summary>
        /// Set and get for StoreActionsCallBackURL
        /// </summary>
        public string StoreActionsCallBackURL { get { return storeActionsCallBackURL; } set { storeActionsCallBackURL = value; } }
        /// <summary>
        /// Set and get for CatalogueIngestionCallBackURL
        /// </summary>
        public string CatalogueIngestionCallBackURL { get { return catalogueIngestionCallBackURL; } set { catalogueIngestionCallBackURL = value; } }
        /// <summary>
        /// Set and get for ItemActionsCallBackURL
        /// </summary>
        public string ItemActionsCallBackURL { get { return itemActionsCallBackURL; } set { itemActionsCallBackURL = value; } }
        /// <summary>
        /// Set and get for ItemOptionActionsCallBackURL
        /// </summary>
        public string ItemOptionActionsCallBackURL { get { return itemOptionActionsCallBackURL; } set { itemOptionActionsCallBackURL = value; } }
        /// <summary>
        /// Set and get for OrderRelayCallBackURL
        /// </summary>
        public string OrderRelayCallBackURL { get { return orderRelayCallBackURL; } set { orderRelayCallBackURL = value; } }
        /// <summary>
        /// Set and get for OrderStatusChangeCallBackURL
        /// </summary>
        public string OrderStatusChangeCallBackURL { get { return orderStatusChangeCallBackURL; } set { orderStatusChangeCallBackURL = value; } }
        /// <summary>
        /// Set and get for RiderStatusChangeCallBackURL
        /// </summary>
        public string RiderStatusChangeCallBackURL { get { return riderStatusChangeCallBackURL; } set { riderStatusChangeCallBackURL = value; } }
        /// <summary>
        /// Set and get for FullfillmentModes
        /// </summary>
        public string FullfillmentModes { get { return fullfillmentModes; } set { fullfillmentModes = value; } }
        /// <summary>
        /// Set and get for packagingChargeProductName
        /// </summary>
        public string PackagingChargeProductName { get { return packagingChargeProductName; } set { packagingChargeProductName = value; } }
        /// <summary>
        /// Set and get for PackagingChargePercentage
        /// </summary>
        public decimal PackagingChargePercentage { get { return packagingChargePercentage; } set { packagingChargePercentage = value; } }
        /// <summary>
        /// Set and get for PaymentModeId
        /// </summary>
        public string PaymentModeName { get { return paymentModeName; } set { paymentModeName = value; } }
        /// <summary>
        /// Set and get for AggregatorDiscountId
        /// </summary>
        public string AggregatorDiscountName { get { return aggregatorDiscountName; } set { aggregatorDiscountName = value; } }
        /// <summary>
        /// Set and get for ProductTaxNameList
        /// </summary>
        public string ProductTaxNameList { get { return productTaxNameList; } set { productTaxNameList = value; } }
        /// <summary>
        /// Set and get for MinimumInventoryQtyForItems
        /// </summary>
        public decimal MinimumInventoryQtyForItems { get { return minimumInventoryQtyForItems; } set { minimumInventoryQtyForItems = value; } }
        /// <summary>
        /// Set and get for FoodTypesSegmentName
        /// </summary>
        public string FoodTypesSegmentName { get { return foodTypesSegmentName; } set { foodTypesSegmentName = value; } }
        /// <summary>
        /// Set and get for GoodsTypeSegmentName
        /// </summary>
        public string GoodsTypeSegmentName { get { return goodsTypeSegmentName; } set { goodsTypeSegmentName = value; } }
        /// <summary>
        /// Set and get for TooManyRequestErrorCode
        /// </summary>
        public string TooManyRequestErrorCode { get { return tooManyRequestErrorCode; } set { tooManyRequestErrorCode = value; } }
        /// <summary>
        /// Set and get for PackageChargeCode
        /// </summary>
        public string PackageChargeCode { get { return packageChargeCode; } set { packageChargeCode = value; } }
        /// <summary>
        /// Set and get for PackageChargeApplicableOn
        /// </summary>
        public string PackageChargeApplicableOn { get { return packageChargeApplicableOn; } set { packageChargeApplicableOn = value; } }
        /// <summary>
        /// Set and get for IsActive
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        /// <summary>
        /// Set and get for Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Set and get for DeliveryChannelContainerDTO
        /// </summary>
        public List<DeliveryChannelContainerDTO> DeliveryChannelContainerDTOList
        {
            get { return deliveryChannelContainerDTOList; }
            set { deliveryChannelContainerDTOList = value; }
        } 
    }

    /// <summary>
    /// DeliveryIntegrations
    /// </summary>
    public enum DeliveryIntegrations
    {
        /// <summary>
        /// UrbanPiper
        /// </summary>
        UrbanPiper
    }
}
