/********************************************************************************************
 * Project Name - Semnox.Parafait.DeliveryIntegration 
 * Description  - Data object of OnlineOrderDeliveryIntegration  
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
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// OnlineOrderDeliveryIntegrationDTO
    /// </summary>
    public class OnlineOrderDeliveryIntegrationDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by DELIVERY_INTEGRATION_ID
            /// </summary>
            DELIVERY_INTEGRATION_ID,
            /// <summary>
            /// Search by INTEGRATION_NAME
            /// </summary>
            INTEGRATION_NAME,
            /// <summary>
            /// Search by IS_ACTIVE
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID
            /// </summary>
            SITE_ID,
        }

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
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private bool synchStatus;
        private int site_id;
        private int masterEntityId;
        private List<DeliveryChannelDTO> deliveryChannelDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public OnlineOrderDeliveryIntegrationDTO()
        {
            log.LogMethodEntry();
            deliveryIntegrationId = -1;
            integrationName = string.Empty; 
            packagingChargePercentage = 0;  
            minimumInventoryQtyForItems = 1; 
            isActive = true;
            synchStatus = false;
            site_id = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameter constructor
        /// </summary>
        public OnlineOrderDeliveryIntegrationDTO(int deliveryIntegrationId, string integrationName, string semnoxWebhookAPIAuthorizationKey, 
            string integratorAPIAuthorizationKey, string integratorAPIBaseURL, string storeCreationCallBackURL, string storeActionsCallBackURL, 
            string catalogueIngestionCallBackURL, string itemActionsCallBackURL, string itemOptionActionsCallBackURL, string orderRelayCallBackURL,
            string orderStatusChangeCallBackURL, string riderStatusChangeCallBackURL, string fullfillmentModes, string packagingChargeProductName,
            decimal packagingChargePercentage, string paymentModeName, string aggregatorDiscountName, string productTaxNameList,
            decimal minimumInventoryQtyForItems, string foodTypesSegmentName, string goodsTypeSegmentName, string tooManyRequestErrorCode, 
            string packageChargeCode, string packageChargeApplicableOn) : this()
        {
            log.LogMethodEntry(deliveryIntegrationId, integrationName, semnoxWebhookAPIAuthorizationKey, integratorAPIAuthorizationKey,
                integratorAPIBaseURL, storeCreationCallBackURL, storeActionsCallBackURL, catalogueIngestionCallBackURL, itemActionsCallBackURL, 
                itemOptionActionsCallBackURL, orderRelayCallBackURL, orderStatusChangeCallBackURL, riderStatusChangeCallBackURL, fullfillmentModes,
                packagingChargeProductName, packagingChargePercentage, paymentModeName, aggregatorDiscountName, productTaxNameList, 
                minimumInventoryQtyForItems, foodTypesSegmentName, goodsTypeSegmentName, tooManyRequestErrorCode, packageChargeCode,
                packageChargeApplicableOn);
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
            log.LogMethodExit();
        }
        /// <summary>
        /// All field parameter constructor
        /// </summary>
        public OnlineOrderDeliveryIntegrationDTO(int deliveryIntegrationId, string integrationName, string semnoxWebhookAPIAuthorizationKey,
            string integratorAPIAuthorizationKey, string integratorAPIBaseURL, string storeCreationCallBackURL, string storeActionsCallBackURL,
            string catalogueIngestionCallBackURL, string itemActionsCallBackURL, string itemOptionActionsCallBackURL, string orderRelayCallBackURL,
            string orderStatusChangeCallBackURL, string riderStatusChangeCallBackURL, string fullfillmentModes, string packagingChargeProductName,
            decimal packagingChargePercentage, string paymentModeName, string aggregatorDiscountName, string productTaxNameList, 
            decimal minimumInventoryQtyForItems, string foodTypesSegmentName, string goodsTypeSegmentName, string tooManyRequestErrorCode, 
            string packageChargeCode,  string packageChargeApplicableOn, bool isActive, string createdBy, DateTime creationDate,
            string lastUpdatedBy, DateTime lastUpdatedDate, string guid, bool synchStatus, int site_id, int masterEntityId) 
            :this(deliveryIntegrationId, integrationName, semnoxWebhookAPIAuthorizationKey, integratorAPIAuthorizationKey, integratorAPIBaseURL,
                storeCreationCallBackURL, storeActionsCallBackURL, catalogueIngestionCallBackURL, itemActionsCallBackURL, 
                itemOptionActionsCallBackURL, orderRelayCallBackURL,orderStatusChangeCallBackURL, riderStatusChangeCallBackURL, fullfillmentModes,
                packagingChargeProductName, packagingChargePercentage, paymentModeName, aggregatorDiscountName, productTaxNameList, 
                minimumInventoryQtyForItems, foodTypesSegmentName, goodsTypeSegmentName, tooManyRequestErrorCode, packageChargeCode,
                packageChargeApplicableOn)
        {
            log.LogMethodEntry(deliveryIntegrationId, integrationName, semnoxWebhookAPIAuthorizationKey, integratorAPIAuthorizationKey,
                integratorAPIBaseURL, storeCreationCallBackURL, storeActionsCallBackURL, catalogueIngestionCallBackURL, itemActionsCallBackURL, 
                itemOptionActionsCallBackURL, orderRelayCallBackURL, orderStatusChangeCallBackURL, riderStatusChangeCallBackURL, fullfillmentModes,
                packagingChargeProductName, packagingChargePercentage, paymentModeName, aggregatorDiscountName, productTaxNameList, 
                minimumInventoryQtyForItems, foodTypesSegmentName, goodsTypeSegmentName, tooManyRequestErrorCode, packageChargeCode,
                packageChargeApplicableOn, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, synchStatus, site_id, 
                masterEntityId); 
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.site_id = site_id;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="onlineOrderDeliveryIntegrationDTO"></param>
        public OnlineOrderDeliveryIntegrationDTO(OnlineOrderDeliveryIntegrationDTO onlineOrderDeliveryIntegrationDTO)
            :this(onlineOrderDeliveryIntegrationDTO.deliveryIntegrationId, onlineOrderDeliveryIntegrationDTO.integrationName, 
                onlineOrderDeliveryIntegrationDTO.semnoxWebhookAPIAuthorizationKey, onlineOrderDeliveryIntegrationDTO.integratorAPIAuthorizationKey,
                onlineOrderDeliveryIntegrationDTO.integratorAPIBaseURL, onlineOrderDeliveryIntegrationDTO.storeCreationCallBackURL,
                onlineOrderDeliveryIntegrationDTO.storeActionsCallBackURL, onlineOrderDeliveryIntegrationDTO.catalogueIngestionCallBackURL, 
                onlineOrderDeliveryIntegrationDTO.itemActionsCallBackURL, onlineOrderDeliveryIntegrationDTO.itemOptionActionsCallBackURL, 
                onlineOrderDeliveryIntegrationDTO.orderRelayCallBackURL, onlineOrderDeliveryIntegrationDTO.orderStatusChangeCallBackURL, 
                onlineOrderDeliveryIntegrationDTO.riderStatusChangeCallBackURL, onlineOrderDeliveryIntegrationDTO.fullfillmentModes, 
                onlineOrderDeliveryIntegrationDTO.packagingChargeProductName, onlineOrderDeliveryIntegrationDTO.packagingChargePercentage, 
                onlineOrderDeliveryIntegrationDTO.paymentModeName, onlineOrderDeliveryIntegrationDTO.aggregatorDiscountName, 
                onlineOrderDeliveryIntegrationDTO.productTaxNameList, onlineOrderDeliveryIntegrationDTO.minimumInventoryQtyForItems, 
                onlineOrderDeliveryIntegrationDTO.foodTypesSegmentName, onlineOrderDeliveryIntegrationDTO.goodsTypeSegmentName, 
                onlineOrderDeliveryIntegrationDTO.tooManyRequestErrorCode, onlineOrderDeliveryIntegrationDTO.packageChargeCode,
                onlineOrderDeliveryIntegrationDTO.packageChargeApplicableOn, onlineOrderDeliveryIntegrationDTO.isActive, onlineOrderDeliveryIntegrationDTO.createdBy, 
                onlineOrderDeliveryIntegrationDTO.creationDate, onlineOrderDeliveryIntegrationDTO.lastUpdatedBy, onlineOrderDeliveryIntegrationDTO.lastUpdatedDate, 
                onlineOrderDeliveryIntegrationDTO.guid, onlineOrderDeliveryIntegrationDTO.synchStatus, onlineOrderDeliveryIntegrationDTO.site_id, 
                onlineOrderDeliveryIntegrationDTO.masterEntityId)
        {
            log.LogMethodEntry(onlineOrderDeliveryIntegrationDTO);

            log.LogMethodExit();
        }

        /// <summary>
        /// Get and Set for DeliveryIntegrationId
        /// </summary>
        public int DeliveryIntegrationId {get { return deliveryIntegrationId; } set { deliveryIntegrationId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for IntegrationName
        /// </summary>
        public string IntegrationName {get { return integrationName; } set { integrationName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for SemnoxWebhookAPIAuthorizationKey
        /// </summary>
        public string SemnoxWebhookAPIAuthorizationKey {get { return semnoxWebhookAPIAuthorizationKey; } set { semnoxWebhookAPIAuthorizationKey =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for IntegratorAPIAuthorizationKey
        /// </summary>
        public string IntegratorAPIAuthorizationKey {get { return integratorAPIAuthorizationKey; } set { integratorAPIAuthorizationKey =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for IntegratorAPIBaseURL
        /// </summary>
        public string IntegratorAPIBaseURL {get { return integratorAPIBaseURL; } set { integratorAPIBaseURL =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for StoreCreationCallBackURL
        /// </summary>
        public string StoreCreationCallBackURL {get { return storeCreationCallBackURL; } set { storeCreationCallBackURL =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for StoreActionsCallBackURL
        /// </summary>
        public string StoreActionsCallBackURL {get { return storeActionsCallBackURL; } set { storeActionsCallBackURL =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for CatalogueIngestionCallBackURL
        /// </summary>
        public string CatalogueIngestionCallBackURL {get { return catalogueIngestionCallBackURL; } set { catalogueIngestionCallBackURL =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for ItemActionsCallBackURL
        /// </summary>
        public string ItemActionsCallBackURL {get { return itemActionsCallBackURL; } set { itemActionsCallBackURL =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for ItemOptionActionsCallBackURL
        /// </summary>
        public string ItemOptionActionsCallBackURL {get { return itemOptionActionsCallBackURL; } set { itemOptionActionsCallBackURL =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for OrderRelayCallBackURL
        /// </summary>
        public string OrderRelayCallBackURL {get { return orderRelayCallBackURL; } set { orderRelayCallBackURL =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for DeliveryIntegrationId
        /// </summary>
        public string OrderStatusChangeCallBackURL {get { return orderStatusChangeCallBackURL; } set { orderStatusChangeCallBackURL =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for DeliveryIntegrationId
        /// </summary>
        public string RiderStatusChangeCallBackURL {get { return riderStatusChangeCallBackURL; } set { riderStatusChangeCallBackURL =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for FullfillmentModes
        /// </summary>
        public string FullfillmentModes {get { return fullfillmentModes; } set { fullfillmentModes =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for packagingChargeProductName
        /// </summary>
        public string PackagingChargeProductName {get { return packagingChargeProductName; } set { packagingChargeProductName =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for PackagingChargePercentage
        /// </summary>
        public decimal PackagingChargePercentage {get { return packagingChargePercentage; } set { packagingChargePercentage =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for PaymentModeName
        /// </summary>
        public string PaymentModeName {get { return paymentModeName; } set { paymentModeName =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for aggregatorDiscountName
        /// </summary>
        public string AggregatorDiscountName {get { return aggregatorDiscountName; } set { aggregatorDiscountName =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for ProductTaxNameList
        /// </summary>
        public string ProductTaxNameList {get { return productTaxNameList; } set { productTaxNameList =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for MinimumInventoryQtyForItems
        /// </summary>
        public decimal MinimumInventoryQtyForItems {get { return minimumInventoryQtyForItems; } set { minimumInventoryQtyForItems =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for FoodTypesSegmentName
        /// </summary>
        public string FoodTypesSegmentName {get { return foodTypesSegmentName; } set { foodTypesSegmentName =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for GoodsTypeSegmentName
        /// </summary>
        public string GoodsTypeSegmentName {get { return goodsTypeSegmentName; } set { goodsTypeSegmentName =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for TooManyRequestErrorCode
        /// </summary>
        public string TooManyRequestErrorCode {get { return tooManyRequestErrorCode; } set { tooManyRequestErrorCode =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for PackageChargeCode
        /// </summary>
        public string PackageChargeCode {get { return packageChargeCode; } set { packageChargeCode =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for PackageChargeApplicableOn
        /// </summary>
        public string PackageChargeApplicableOn {get { return packageChargeApplicableOn; } set { packageChargeApplicableOn =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for IsActive
        /// </summary>
        public bool IsActive {get { return isActive; } set { isActive =  value; this.IsChanged = true; } }
        /// <summary>
        /// Get and Set for CreatedBy
        /// </summary>
        public string CreatedBy {get { return createdBy; } set { createdBy =  value;  } }
        /// <summary>
        /// Get and Set for CreationDate
        /// </summary>
        public DateTime CreationDate {get { return creationDate; } set { creationDate =  value;  } }
        /// <summary>
        /// Get and Set for LastUpdatedBy
        /// </summary>
        public string LastUpdatedBy {get { return lastUpdatedBy; } set { lastUpdatedBy =  value;  } }
        /// <summary>
        /// Get and Set for LastUpdatedDate
        /// </summary>
        public DateTime LastUpdatedDate {get { return lastUpdatedDate; } set { lastUpdatedDate =  value;  } }
        /// <summary>
        /// Get and Set for Guid
        /// </summary>
        public string Guid {get { return guid; } set { guid =  value;  } }
        /// <summary>
        /// Get and Set for SynchStatus
        /// </summary>
        public bool SynchStatus {get { return synchStatus; } set { synchStatus =  value;  } }
        /// <summary>
        /// Get and Set for Site_id
        /// </summary>
        public int Site_id {get { return site_id; } set { site_id =  value;  } }
        /// <summary>
        /// Get and Set for MasterEntityId
        /// </summary>
        public int MasterEntityId {get { return masterEntityId; } set { masterEntityId =  value; this.IsChanged = true; } }


        public List<DeliveryChannelDTO> DeliveryChannelDTOList { get { return deliveryChannelDTOList; } set { deliveryChannelDTOList = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || deliveryIntegrationId < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }
        /// <summary>
        /// Returns whether the DTO changed or any of its deliveryChannelDTOList List items  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (deliveryChannelDTOList != null &&
                    deliveryChannelDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
