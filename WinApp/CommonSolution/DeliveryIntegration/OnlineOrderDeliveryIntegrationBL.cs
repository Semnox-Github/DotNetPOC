/********************************************************************************************
 * Project Name - DeliveryIntegration
 * Description  - BL class for OnlineOrderDeliveryIntegration  
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 2.140.3      11-Jul-2022   Guru S A       Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// OnlineOrderDeliveryIntegrationBL
    /// </summary>
    public class OnlineOrderDeliveryIntegrationBL
    {
        private OnlineOrderDeliveryIntegrationDTO deliveryIntegrationDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private const int HUNDREDCHARS = 100;
        private const int FOURHUNDREDCHARS = 400;
        private const int THOUSANDCHARS = 1000;
        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private OnlineOrderDeliveryIntegrationBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the Id parameter
        /// </summary>
        /// <param name="deliveryIntegrationId">deliveryIntegrationId</param>
        /// <param name="loadChildRecords">To load the child DTO Records</param>
        public OnlineOrderDeliveryIntegrationBL(ExecutionContext executionContext, int deliveryIntegrationId, bool loadChildRecords = false, bool activeChildRecords = false,
            SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(deliveryIntegrationId, loadChildRecords, activeChildRecords);
            LoadDeliveryIntegration(deliveryIntegrationId, loadChildRecords, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// LoadOnlineOrderDeliveryIntegration
        /// Would fetch the DTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        private void LoadDeliveryIntegration(int id, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, executionContext, sqlTransaction);
            OnlineOrderDeliveryIntegrationDataHandler dataHandler = new OnlineOrderDeliveryIntegrationDataHandler(sqlTransaction);
            deliveryIntegrationDTO = dataHandler.GetOnlineOrderDeliveryIntegration(id);
            ThrowIfDTOIsNull(id);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(deliveryIntegrationDTO);
        }
        private void ThrowIfDTOIsNull(int id)
        {
            log.LogMethodEntry(id);
            if (deliveryIntegrationDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "OnlineOrderDeliveryIntegration", id);
                log.LogMethodExit(null, "Throwing Exception - "); //+ //message);
                throw new EntityNotFoundException("invalid Id");//message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterDTO">OnlineOrderDeliveryIntegrationDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public OnlineOrderDeliveryIntegrationBL(ExecutionContext executionContext, OnlineOrderDeliveryIntegrationDTO parameterDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterDTO, sqlTransaction);
            if (parameterDTO.DeliveryIntegrationId > -1)
            {
                LoadDeliveryIntegration(parameterDTO.DeliveryIntegrationId, true, false, sqlTransaction);
                ThrowIfDTOIsNull(parameterDTO.DeliveryIntegrationId);
                Update(parameterDTO, sqlTransaction);
            }
            else
            {
                ValidateNewDTOElementsAndAddToDTO(parameterDTO, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the child records for delivery integration object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)    //added build
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            DeliveryChannelListBL deliveryChannelListBL = new DeliveryChannelListBL(executionContext);
            List<DeliveryChannelDTO> deliveryChannelDTOList = deliveryChannelListBL.GetDeliveryChannelDTOList(new List<int>() { deliveryIntegrationDTO.DeliveryIntegrationId }, activeChildRecords, sqlTransaction);
            if (deliveryChannelDTOList != null && deliveryChannelDTOList.Any())
            {
                deliveryIntegrationDTO.DeliveryChannelDTOList = deliveryChannelDTOList;
            }
            log.LogMethodExit();
        }

        private void Update(OnlineOrderDeliveryIntegrationDTO parameterDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterDTO, sqlTransaction);
            ChangeIntegrationName(parameterDTO.IntegrationName);
            ChangeSemnoxWebhookAPIAuthorizationKey(parameterDTO.SemnoxWebhookAPIAuthorizationKey);
            ChangeIntegratorAPIAuthorizationKey(parameterDTO.IntegratorAPIAuthorizationKey);   
            ChangeIntegratorAPIBaseURL(parameterDTO.IntegratorAPIBaseURL);
            ChangeStoreCreationCallBackURL(parameterDTO.StoreCreationCallBackURL);
            ChangeStoreActionsCallBackURL(parameterDTO.StoreActionsCallBackURL);
            ChangeCatalogueIngestionCallBackURL(parameterDTO.CatalogueIngestionCallBackURL);
            ChangeItemActionsCallBackURL(parameterDTO.ItemActionsCallBackURL);
            ChangeItemOptionActionsCallBackURL(parameterDTO.ItemOptionActionsCallBackURL);        
            ChangeOrderRelayCallBackURL(parameterDTO.OrderRelayCallBackURL);
            ChangeOrderStatusChangeCallBackURL(parameterDTO.OrderStatusChangeCallBackURL);
            ChangeRiderStatusChangeCallBackURL(parameterDTO.RiderStatusChangeCallBackURL);
            ChangeFullfillmentModes(parameterDTO.FullfillmentModes);
            ChangePackagingChargeProductName(parameterDTO.PackagingChargeProductName);
            ChangePackagingChargePercentage(parameterDTO.PackagingChargePercentage);
            ChangePaymentModeName(parameterDTO.PaymentModeName);
            ChangeAggregatorDiscountName(parameterDTO.AggregatorDiscountName);
            ChangeProductTaxNameList(parameterDTO.ProductTaxNameList);
            ChangeMinimumInventoryQtyForItems(parameterDTO.MinimumInventoryQtyForItems);
            ChangeFoodTypesSegmentName(parameterDTO.FoodTypesSegmentName);
            ChangeGoodsTypeSegmentName(parameterDTO.GoodsTypeSegmentName);
            ChangeTooManyRequestErrorCode(parameterDTO.TooManyRequestErrorCode);
            ChangePackageChargeCode(parameterDTO.PackageChargeCode);
            ChangePackageChargeApplicableOn(parameterDTO.PackageChargeApplicableOn);
            ChangeIsActive(parameterDTO.IsActive, sqlTransaction);
            ChangeChildDTOEntries(parameterDTO, sqlTransaction);
        }

        private void ChangeIntegrationName(string integrationName)
        {
            log.LogMethodEntry(integrationName);
            if (deliveryIntegrationDTO.IntegrationName == integrationName)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration integrationName");
                return;
            }
            ValidateIntegrationName(integrationName);
            deliveryIntegrationDTO.IntegrationName = integrationName;
            log.LogMethodExit();
        }
        private void ValidateIntegrationName(string integrationName)
        {
            log.LogMethodEntry(integrationName);
            if (string.IsNullOrWhiteSpace(integrationName))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "IntegrationName"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("IntegrationName is empty.", "OnlineOrderDeliveryIntegration", "IntegrationName", errorMessage);
            }
            if (integrationName.Length > FOURHUNDREDCHARS)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "IntegrationName"), FOURHUNDREDCHARS);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("IntegrationName greater than " + FOURHUNDREDCHARS + " characters.", "OnlineOrderDeliveryIntegration", "IntegrationName", errorMessage);
            }
            log.LogMethodExit();
        }
        private void ChangeSemnoxWebhookAPIAuthorizationKey(string semnoxWebhookAPIAuthorizationKey)
        {
            log.LogMethodEntry(semnoxWebhookAPIAuthorizationKey);
            if (deliveryIntegrationDTO.SemnoxWebhookAPIAuthorizationKey == semnoxWebhookAPIAuthorizationKey)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration semnoxWebhookAPIAuthorizationKey");
                return;
            }
            ValidateString(semnoxWebhookAPIAuthorizationKey, "SemnoxWebhookAPIAuthorizationKey", THOUSANDCHARS);
            deliveryIntegrationDTO.SemnoxWebhookAPIAuthorizationKey = semnoxWebhookAPIAuthorizationKey;
            log.LogMethodExit();
        }
        private void ValidateString(string stringValue, string stringName, int allowedLength)
        {
            log.LogMethodEntry(stringValue, stringName, allowedLength);
            //if (string.IsNullOrWhiteSpace(stringValue))
            //{
            //    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, stringName));
            //    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
            //    throw new ValidationException("Name is empty.", "OnlineOrderDeliveryIntegration", stringName, errorMessage);
            //}
            if (string.IsNullOrWhiteSpace(stringValue) == false && stringValue.Length > allowedLength)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, stringName), allowedLength);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(stringName + " greater than " + allowedLength + " characters.", "OnlineOrderDeliveryIntegration", stringName, errorMessage);
            }
            log.LogMethodExit();
        }

        private void ChangeIntegratorAPIAuthorizationKey(string integratorAPIAuthorizationKey)
        {
            log.LogMethodEntry(integratorAPIAuthorizationKey);
            if (deliveryIntegrationDTO.IntegratorAPIAuthorizationKey == integratorAPIAuthorizationKey)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration IntegratorAPIAuthorizationKey");
                return;
            }
            ValidateString(integratorAPIAuthorizationKey, "IntegratorAPIAuthorizationKey", THOUSANDCHARS);
            deliveryIntegrationDTO.IntegratorAPIAuthorizationKey = integratorAPIAuthorizationKey;
            log.LogMethodExit();
        }
        private void ChangeIntegratorAPIBaseURL(string integratorAPIBaseURL)
        {
            log.LogMethodEntry(integratorAPIBaseURL);
            if (deliveryIntegrationDTO.IntegratorAPIBaseURL == integratorAPIBaseURL)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration IntegratorAPIBaseURL");
                return;
            }
            ValidateString(integratorAPIBaseURL, "IntegratorAPIBaseURL", THOUSANDCHARS);
            deliveryIntegrationDTO.IntegratorAPIBaseURL = integratorAPIBaseURL;
            log.LogMethodExit();
        }

        private void ChangeStoreCreationCallBackURL(string storeCreationCallBackURL)
        {
            log.LogMethodEntry(storeCreationCallBackURL);
            if (deliveryIntegrationDTO.StoreCreationCallBackURL == storeCreationCallBackURL)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration StoreCreationCallBackURL");
                return;
            }
            ValidateString(storeCreationCallBackURL, "StoreCreationCallBackURL", THOUSANDCHARS);
            deliveryIntegrationDTO.StoreCreationCallBackURL = storeCreationCallBackURL;
            log.LogMethodExit();
        }
        private void ChangeStoreActionsCallBackURL(string storeActionsCallBackURL)
        {
            log.LogMethodEntry(storeActionsCallBackURL);
            if (deliveryIntegrationDTO.StoreActionsCallBackURL == storeActionsCallBackURL)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration StoreActionsCallBackURL");
                return;
            }
            ValidateString(storeActionsCallBackURL, "StoreActionsCallBackURL", THOUSANDCHARS);
            deliveryIntegrationDTO.StoreActionsCallBackURL = storeActionsCallBackURL;
            log.LogMethodExit();
        }
        
        private void ChangeCatalogueIngestionCallBackURL(string catalogueIngestionCallBackURL)
        {
            log.LogMethodEntry(catalogueIngestionCallBackURL);
            if (deliveryIntegrationDTO.CatalogueIngestionCallBackURL == catalogueIngestionCallBackURL)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration CatalogueIngestionCallBackURL");
                return;
            }
            ValidateString(catalogueIngestionCallBackURL, "CatalogueIngestionCallBackURL", THOUSANDCHARS);
            deliveryIntegrationDTO.CatalogueIngestionCallBackURL = catalogueIngestionCallBackURL;
            log.LogMethodExit();
        }
        private void ChangeItemActionsCallBackURL(string itemActionsCallBackURL)
        {
            log.LogMethodEntry(itemActionsCallBackURL);
            if (deliveryIntegrationDTO.ItemActionsCallBackURL == itemActionsCallBackURL)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration ItemActionsCallBackURL");
                return;
            }
            ValidateString(itemActionsCallBackURL, "ItemActionsCallBackURL", THOUSANDCHARS);
            deliveryIntegrationDTO.ItemActionsCallBackURL = itemActionsCallBackURL;
            log.LogMethodExit();
        }

        private void ChangeItemOptionActionsCallBackURL(string itemOptionActionsCallBackURL)
        {
            log.LogMethodEntry(itemOptionActionsCallBackURL);
            if (deliveryIntegrationDTO.ItemOptionActionsCallBackURL == itemOptionActionsCallBackURL)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration ItemOptionActionsCallBackURL");
                return;
            }
            ValidateString(itemOptionActionsCallBackURL, "ItemOptionActionsCallBackURL", THOUSANDCHARS);
            deliveryIntegrationDTO.ItemOptionActionsCallBackURL = itemOptionActionsCallBackURL;
            log.LogMethodExit();
        }

        private void ChangeOrderRelayCallBackURL(string orderRelayCallBackURL)
        {
            log.LogMethodEntry(orderRelayCallBackURL);
            if (deliveryIntegrationDTO.OrderRelayCallBackURL == orderRelayCallBackURL)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration OrderRelayCallBackURL");
                return;
            }
            ValidateString(orderRelayCallBackURL, "OrderRelayCallBackURL", THOUSANDCHARS);
            deliveryIntegrationDTO.OrderRelayCallBackURL = orderRelayCallBackURL;
            log.LogMethodExit();
        }

        private void ChangeOrderStatusChangeCallBackURL(string orderStatusChangeCallBackURL)
        {
            log.LogMethodEntry(orderStatusChangeCallBackURL);
            if (deliveryIntegrationDTO.OrderStatusChangeCallBackURL == orderStatusChangeCallBackURL)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration OrderStatusChangeCallBackURL");
                return;
            }
            ValidateString(orderStatusChangeCallBackURL, "OrderStatusChangeCallBackURL", THOUSANDCHARS);
            deliveryIntegrationDTO.OrderStatusChangeCallBackURL = orderStatusChangeCallBackURL;
            log.LogMethodExit();
        }

        private void ChangeRiderStatusChangeCallBackURL(string riderStatusChangeCallBackURL)
        {
            log.LogMethodEntry(riderStatusChangeCallBackURL);
            if (deliveryIntegrationDTO.RiderStatusChangeCallBackURL == riderStatusChangeCallBackURL)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration RiderStatusChangeCallBackURL");
                return;
            }
            ValidateString(riderStatusChangeCallBackURL, "RiderStatusChangeCallBackURL", THOUSANDCHARS);
            deliveryIntegrationDTO.RiderStatusChangeCallBackURL = riderStatusChangeCallBackURL;
            log.LogMethodExit();
        }

        private void ChangeFullfillmentModes(string fullfillmentModes)
        {
            log.LogMethodEntry(fullfillmentModes);
            if (deliveryIntegrationDTO.FullfillmentModes == fullfillmentModes)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration FullfillmentModes");
                return;
            }
            ValidateString(fullfillmentModes, "FullfillmentModes", FOURHUNDREDCHARS);
            deliveryIntegrationDTO.FullfillmentModes = fullfillmentModes;
            log.LogMethodExit();
        }

        private void ChangePackagingChargeProductName(string packagingChargeProductName)
        {
            log.LogMethodEntry(packagingChargeProductName);
            if (deliveryIntegrationDTO.PackagingChargeProductName == packagingChargeProductName)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration PackagingChargeProductName");
                return;
            }
            deliveryIntegrationDTO.PackagingChargeProductName = packagingChargeProductName;
            log.LogMethodExit();
        }

        private void ChangePackagingChargePercentage(decimal packagingChargePercentage)
        {
            log.LogMethodEntry(packagingChargePercentage);
            if (deliveryIntegrationDTO.PackagingChargePercentage == packagingChargePercentage)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration PackagingChargePercentage");
                return;
            }
            deliveryIntegrationDTO.PackagingChargePercentage = packagingChargePercentage;
            log.LogMethodExit();
        }

        private void ChangePaymentModeName(string paymentModeName)
        {
            log.LogMethodEntry(paymentModeName);
            if (deliveryIntegrationDTO.PaymentModeName == paymentModeName)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration PaymentMode");
                return;
            }
            deliveryIntegrationDTO.PaymentModeName = paymentModeName;
            log.LogMethodExit();
        }

        private void ChangeAggregatorDiscountName(string aggregatorDiscountName)
        {
            log.LogMethodEntry(aggregatorDiscountName);
            if (deliveryIntegrationDTO.AggregatorDiscountName == aggregatorDiscountName)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration AggregatorDiscount");
                return;
            }
            deliveryIntegrationDTO.AggregatorDiscountName = aggregatorDiscountName;
            log.LogMethodExit();
        }

        private void ChangeProductTaxNameList(string productTaxNameList)
        {
            log.LogMethodEntry(productTaxNameList);
            if (deliveryIntegrationDTO.ProductTaxNameList == productTaxNameList)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration ProductTaxNameList");
                return;
            }
            ValidateString(productTaxNameList, "ProductTaxNameList", THOUSANDCHARS);
            deliveryIntegrationDTO.ProductTaxNameList = productTaxNameList;
            log.LogMethodExit();
        }

        private void ChangeMinimumInventoryQtyForItems(decimal minimumInventoryQtyForItems)
        {
            log.LogMethodEntry(minimumInventoryQtyForItems);
            if (deliveryIntegrationDTO.MinimumInventoryQtyForItems == minimumInventoryQtyForItems)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration MinimumInventoryQtyForItems");
                return;
            }
            deliveryIntegrationDTO.MinimumInventoryQtyForItems = minimumInventoryQtyForItems;
            log.LogMethodExit();
        }

        private void ChangeGoodsTypeSegmentName(string goodsTypeSegmentName)
        {
            log.LogMethodEntry(goodsTypeSegmentName);
            if (deliveryIntegrationDTO.GoodsTypeSegmentName == goodsTypeSegmentName)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration GoodsTypeSegment");
                return;
            }
            deliveryIntegrationDTO.GoodsTypeSegmentName = goodsTypeSegmentName;
            log.LogMethodExit();
        }

        private void ChangeTooManyRequestErrorCode(string tooManyRequestErrorCode)
        {
            log.LogMethodEntry(tooManyRequestErrorCode);
            if (deliveryIntegrationDTO.TooManyRequestErrorCode == tooManyRequestErrorCode)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration TooManyRequestErrorCode");
                return;
            }
            ValidateString(tooManyRequestErrorCode, "TooManyRequestErrorCode", HUNDREDCHARS);
            deliveryIntegrationDTO.TooManyRequestErrorCode = tooManyRequestErrorCode;
            log.LogMethodExit();
        }
        private void ChangePackageChargeCode(string packageChargeCode)
        {
            log.LogMethodEntry(packageChargeCode);
            if (deliveryIntegrationDTO.PackageChargeCode == packageChargeCode)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration PackageChargeCode");
                return;
            }
            ValidateString(packageChargeCode, "PackageChargeCode", HUNDREDCHARS);
            deliveryIntegrationDTO.PackageChargeCode = packageChargeCode;
            log.LogMethodExit();
        }

        private void ChangePackageChargeApplicableOn(string packageChargeApplicableOn)
        {
            log.LogMethodEntry(packageChargeApplicableOn);
            if (deliveryIntegrationDTO.PackageChargeApplicableOn == packageChargeApplicableOn)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration PackageChargeApplicableOn");
                return;
            }
            ValidateString(packageChargeApplicableOn, "PackageChargeApplicableOn", HUNDREDCHARS);
            deliveryIntegrationDTO.PackageChargeApplicableOn = packageChargeApplicableOn;
            log.LogMethodExit();
        }

        private void ChangeFoodTypesSegmentName(string foodTypesSegmentName)
        {
            log.LogMethodEntry(foodTypesSegmentName);
            if (deliveryIntegrationDTO.FoodTypesSegmentName == foodTypesSegmentName)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration FoodTypesSegment");
                return;
            }
            deliveryIntegrationDTO.FoodTypesSegmentName = foodTypesSegmentName;
            log.LogMethodExit();
        }

        private void ChangeIsActive(bool isActive, SqlTransaction sqlTransaction)
        {

            log.LogMethodEntry(isActive);
            if (deliveryIntegrationDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to OnlineOrderDeliveryIntegration IsActive");
                return;
            }
            ValidateIsActive(isActive, sqlTransaction);
            deliveryIntegrationDTO.IsActive = isActive;
            log.LogMethodExit();
        }
        private void ValidateIsActive(bool isActive, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(isActive);
            if ((deliveryIntegrationDTO != null && deliveryIntegrationDTO.DeliveryIntegrationId > -1) && isActive == false)
            {
                OnlineOrderDeliveryIntegrationDataHandler deliveryIntegrationDataHandler = new OnlineOrderDeliveryIntegrationDataHandler(sqlTransaction);
                bool isRecordReferenced = deliveryIntegrationDataHandler.GetIsRecordReferenced(deliveryIntegrationDTO.DeliveryIntegrationId);
                if (isRecordReferenced)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1869);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Unable to delete this record. Please check the reference record first.", "OnlineOrderDeliveryIntegration", "IsActive", errorMessage);
                }
            }
            log.LogMethodExit();
        }

        private void ChangeChildDTOEntries(OnlineOrderDeliveryIntegrationDTO parameterDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(parameterDTO, sqlTransaction);
            Dictionary<int, DeliveryChannelDTO> deliveryChannelDTODictionary = new Dictionary<int, DeliveryChannelDTO>();
            if (deliveryIntegrationDTO.DeliveryChannelDTOList != null &&
                deliveryIntegrationDTO.DeliveryChannelDTOList.Any())
            {
                foreach (DeliveryChannelDTO deliveryChannelDTO in deliveryIntegrationDTO.DeliveryChannelDTOList)
                {
                    deliveryChannelDTODictionary.Add(deliveryChannelDTO.DeliveryChannelId, deliveryChannelDTO);
                }
            }
            if (parameterDTO.DeliveryChannelDTOList != null &&
                parameterDTO.DeliveryChannelDTOList.Any())
            {
                if (deliveryIntegrationDTO.DeliveryChannelDTOList == null)
                {
                    deliveryIntegrationDTO.DeliveryChannelDTOList = new List<DeliveryChannelDTO>();
                }
                foreach (DeliveryChannelDTO deliveryChannelDTO in parameterDTO.DeliveryChannelDTOList)
                {
                    DeliveryChannelBL deliveryChannelBL = null;
                    deliveryChannelBL = new DeliveryChannelBL(executionContext, deliveryChannelDTO, sqlTransaction);
                    if (deliveryChannelDTODictionary.ContainsKey(deliveryChannelDTO.DeliveryChannelId))
                    {
                        DeliveryChannelDTO deliveryChannelDTOObject = deliveryIntegrationDTO.DeliveryChannelDTOList.Find(dc => dc.DeliveryChannelId == deliveryChannelDTO.DeliveryChannelId);
                        int indexOfDTO = deliveryIntegrationDTO.DeliveryChannelDTOList.IndexOf(deliveryChannelDTOObject);
                        if (indexOfDTO > -1)
                        {
                            deliveryIntegrationDTO.DeliveryChannelDTOList[indexOfDTO] = deliveryChannelBL.DeliveryChannelDTO;
                        }
                    }
                    else
                    {
                        deliveryChannelBL = new DeliveryChannelBL(executionContext, deliveryChannelDTO, sqlTransaction);
                        deliveryIntegrationDTO.DeliveryChannelDTOList.Add(deliveryChannelBL.DeliveryChannelDTO);
                    }
                }
            }
            log.LogMethodExit();
        }
        private void ValidateNewDTOElementsAndAddToDTO(OnlineOrderDeliveryIntegrationDTO parameterDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(parameterDTO, sqlTransaction);
            ValidateIntegrationName(parameterDTO.IntegrationName);
            ValidateString(parameterDTO.SemnoxWebhookAPIAuthorizationKey, "SemnoxWebhookAPIAuthorizationKey", THOUSANDCHARS);
            ValidateString(parameterDTO.IntegratorAPIAuthorizationKey, "IntegratorAPIAuthorizationKey", THOUSANDCHARS);
            ValidateString(parameterDTO.IntegratorAPIBaseURL, "IntegratorAPIBaseURL", THOUSANDCHARS);
            ValidateString(parameterDTO.StoreCreationCallBackURL, "StoreCreationCallBackURL", THOUSANDCHARS);
            ValidateString(parameterDTO.StoreActionsCallBackURL, "StoreActionsCallBackURL", THOUSANDCHARS);
            ValidateString(parameterDTO.CatalogueIngestionCallBackURL, "CatalogueIngestionCallBackURL", THOUSANDCHARS);
            ValidateString(parameterDTO.ItemActionsCallBackURL, "ItemActionsCallBackURL", THOUSANDCHARS);
            ValidateString(parameterDTO.ItemOptionActionsCallBackURL, "ItemOptionActionsCallBackURL", THOUSANDCHARS);
            ValidateString(parameterDTO.OrderRelayCallBackURL, "OrderRelayCallBackURL", THOUSANDCHARS);
            ValidateString(parameterDTO.OrderStatusChangeCallBackURL, "OrderStatusChangeCallBackURL", THOUSANDCHARS);
            ValidateString(parameterDTO.RiderStatusChangeCallBackURL, "RiderStatusChangeCallBackURL", THOUSANDCHARS);
            ValidateString(parameterDTO.FullfillmentModes, "FullfillmentModes", FOURHUNDREDCHARS);
            ValidateString(parameterDTO.ProductTaxNameList, "ProductTaxNameList", THOUSANDCHARS);
            ValidateString(parameterDTO.TooManyRequestErrorCode, "TooManyRequestErrorCode", HUNDREDCHARS);
            ValidateString(parameterDTO.PackageChargeCode, "PackageChargeCode", HUNDREDCHARS);
            ValidateString(parameterDTO.PackageChargeApplicableOn, "PackageChargeApplicableOn", HUNDREDCHARS);
            ValidateIsActive(parameterDTO.IsActive, sqlTransaction);
            deliveryIntegrationDTO = new OnlineOrderDeliveryIntegrationDTO(parameterDTO);
            if (parameterDTO.DeliveryChannelDTOList != null && parameterDTO.DeliveryChannelDTOList.Any())
            {
                deliveryIntegrationDTO.DeliveryChannelDTOList = new List<DeliveryChannelDTO>();
                foreach (DeliveryChannelDTO paramDeliveryChannelDTO in parameterDTO.DeliveryChannelDTOList)
                {
                    if (paramDeliveryChannelDTO.DeliveryChannelId > -1)
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 2196, "DeliveryChannelDTO", paramDeliveryChannelDTO.DeliveryChannelId);
                        log.LogMethodExit(null, "Throwing Exception - " + message);
                        throw new EntityNotFoundException(message);
                    }
                    paramDeliveryChannelDTO.DeliveryIntegrationId = deliveryIntegrationDTO.DeliveryIntegrationId;
                    DeliveryChannelDTO deliveryChannelDTO = new DeliveryChannelDTO(paramDeliveryChannelDTO);
                    DeliveryChannelBL deliveryChannelBL = new DeliveryChannelBL(executionContext, deliveryChannelDTO, sqlTransaction);
                    deliveryIntegrationDTO.DeliveryChannelDTOList.Add(deliveryChannelBL.DeliveryChannelDTO);
                }
            }
            log.LogMethodExit();
        }




        /// <summary>
        /// Saves the DTO
        /// Checks if the User id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            OnlineOrderDeliveryIntegrationDataHandler deliveryIntegrationDataHandler = new OnlineOrderDeliveryIntegrationDataHandler(sqlTransaction);
            if (deliveryIntegrationDTO.DeliveryIntegrationId < 0)
            {
                deliveryIntegrationDTO = deliveryIntegrationDataHandler.Insert(deliveryIntegrationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(deliveryIntegrationDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("onlineOrderDeliveryIntegration", deliveryIntegrationDTO.Guid, sqlTransaction);
                }
                deliveryIntegrationDTO.AcceptChanges();
            }
            else
            {
                if (deliveryIntegrationDTO.IsChanged)
                {
                    deliveryIntegrationDTO = deliveryIntegrationDataHandler.Update(deliveryIntegrationDTO, executionContext.GetUserId(), executionContext.GetSiteId());

                    if (!string.IsNullOrEmpty(deliveryIntegrationDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("onlineOrderDeliveryIntegration", deliveryIntegrationDTO.Guid, sqlTransaction);
                    }
                    deliveryIntegrationDTO.AcceptChanges();
                }
            }

            // Will Save the Child DeliveryChannelDTO List
            log.Debug("deliveryIntegrationDTO.DeliveryChannelDTOList Value :" + deliveryIntegrationDTO.DeliveryChannelDTOList);
            if (deliveryIntegrationDTO.DeliveryChannelDTOList != null && deliveryIntegrationDTO.DeliveryChannelDTOList.Any())
            {
                List<DeliveryChannelDTO> updatedDeliveryChannelDTOList = new List<DeliveryChannelDTO>();
                foreach (DeliveryChannelDTO deliveryChannelMappingDTO in deliveryIntegrationDTO.DeliveryChannelDTOList)
                {
                    if (deliveryChannelMappingDTO.DeliveryIntegrationId != deliveryIntegrationDTO.DeliveryIntegrationId)
                    {
                        deliveryChannelMappingDTO.DeliveryIntegrationId = deliveryIntegrationDTO.DeliveryIntegrationId;
                    }
                    log.Debug("deliveryChannelMappingDTO.IsChanged Value :" + deliveryChannelMappingDTO.IsChanged);
                    if (deliveryChannelMappingDTO.IsChanged)
                    {
                        updatedDeliveryChannelDTOList.Add(deliveryChannelMappingDTO);
                    }
                }
                log.Debug("updatedDeliveryChannelDTOList Value :" + updatedDeliveryChannelDTOList);
                if (updatedDeliveryChannelDTOList.Any())
                {
                    DeliveryChannelListBL deliveryChannelListBL = new DeliveryChannelListBL(executionContext);
                    deliveryChannelListBL.Save(updatedDeliveryChannelDTOList, sqlTransaction);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public OnlineOrderDeliveryIntegrationDTO OnlineOrderDeliveryIntegrationDTO
        {
            get
            {
                OnlineOrderDeliveryIntegrationDTO result = new OnlineOrderDeliveryIntegrationDTO(deliveryIntegrationDTO);
                return result;
            }
        }
    }

    /// <summary>
    /// Manages the list of OnlineOrderDeliveryIntegration
    /// </summary>
    /// 

    public class OnlineOrderDeliveryIntegrationListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public OnlineOrderDeliveryIntegrationListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// OnlineOrderDeliveryIntegrationListBL
        /// </summary>
        /// <param name="executionContext"></param>
        public OnlineOrderDeliveryIntegrationListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the OnlineOrderDeliveryIntegration list
        /// </summary>
        public List<OnlineOrderDeliveryIntegrationDTO> GetOnlineOrderDeliveryIntegrationDTOList(List<KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords);
            OnlineOrderDeliveryIntegrationDataHandler deliveryIntegrationDataHandler = new OnlineOrderDeliveryIntegrationDataHandler(sqlTransaction);
            List<OnlineOrderDeliveryIntegrationDTO> onlineOrderDeliveryIntegrationList = deliveryIntegrationDataHandler.GetOnlineOrderDeliveryIntegrationDTOList(searchParameters,
                                                                                          sqlTransaction);
            if (onlineOrderDeliveryIntegrationList != null && onlineOrderDeliveryIntegrationList.Any() && loadChildRecords)
            {
                Build(onlineOrderDeliveryIntegrationList, loadActiveChildRecords, sqlTransaction);
            }
            log.LogMethodExit(onlineOrderDeliveryIntegrationList);
            return onlineOrderDeliveryIntegrationList;
        }

        public List<OnlineOrderDeliveryIntegrationDTO> GetOnlineOrderDeliveryIntegrationDTOList(List<string> onlineOrderDeliveryIntegrationGuidList, bool loadChildRecords = false,
            bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(onlineOrderDeliveryIntegrationGuidList, loadChildRecords, loadActiveChildRecords);
            OnlineOrderDeliveryIntegrationDataHandler dataHandlerObject = new OnlineOrderDeliveryIntegrationDataHandler(sqlTransaction);
            List<OnlineOrderDeliveryIntegrationDTO> deliveryIntegrationDTOList = dataHandlerObject.GetOnlineOrderDeliveryIntegrationDTOList(onlineOrderDeliveryIntegrationGuidList, sqlTransaction);
            if (deliveryIntegrationDTOList != null && deliveryIntegrationDTOList.Any() && loadChildRecords)
            {
                Build(deliveryIntegrationDTOList, loadActiveChildRecords, sqlTransaction);
            }
            log.LogMethodExit(deliveryIntegrationDTOList);
            return deliveryIntegrationDTOList;
        }
        private void Build(List<OnlineOrderDeliveryIntegrationDTO> onlineOrderDeliveryIntegrationDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(onlineOrderDeliveryIntegrationDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, OnlineOrderDeliveryIntegrationDTO> deliveryIntegrationDTOIdMap = new Dictionary<int, OnlineOrderDeliveryIntegrationDTO>();
            List<int> deliveryIntegrationIdList = new List<int>();
            Dictionary<int, OnlineOrderDeliveryIntegrationDTO> deliveryIntegrationDTODictionary = new Dictionary<int, OnlineOrderDeliveryIntegrationDTO>();
            for (int i = 0; i < onlineOrderDeliveryIntegrationDTOList.Count; i++)
            {
                if (deliveryIntegrationDTOIdMap.ContainsKey(onlineOrderDeliveryIntegrationDTOList[i].DeliveryIntegrationId))
                {
                    continue;
                }
                deliveryIntegrationDTOIdMap.Add(onlineOrderDeliveryIntegrationDTOList[i].DeliveryIntegrationId, onlineOrderDeliveryIntegrationDTOList[i]);
                deliveryIntegrationIdList.Add(onlineOrderDeliveryIntegrationDTOList[i].DeliveryIntegrationId);
            }
            DeliveryChannelListBL deliveryChannelListBL = new DeliveryChannelListBL(executionContext);
            List<DeliveryChannelDTO> deliveryChannelDTOList = deliveryChannelListBL.GetDeliveryChannelDTOList(deliveryIntegrationIdList, activeChildRecords, sqlTransaction);
            if (deliveryChannelDTOList != null && deliveryChannelDTOList.Any())
            {
                for (int i = 0; i < deliveryChannelDTOList.Count; i++)
                {
                    if (deliveryIntegrationDTOIdMap.ContainsKey(deliveryChannelDTOList[i].DeliveryIntegrationId) == false)
                    {
                        continue;
                    }
                    OnlineOrderDeliveryIntegrationDTO deliveryIntegrationDTO = deliveryIntegrationDTOIdMap[deliveryChannelDTOList[i].DeliveryIntegrationId];
                    if (deliveryIntegrationDTO.DeliveryChannelDTOList == null)
                    {
                        deliveryIntegrationDTO.DeliveryChannelDTOList = new List<DeliveryChannelDTO>();
                    }
                    deliveryIntegrationDTO.DeliveryChannelDTOList.Add(deliveryChannelDTOList[i]);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// This method should be used to Save and Update the OnlineOrderDeliveryIntegration.
        /// </summary>
        public List<OnlineOrderDeliveryIntegrationDTO> Save(List<OnlineOrderDeliveryIntegrationDTO> onlineOrderDeliveryIntegrationDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<OnlineOrderDeliveryIntegrationDTO> savedDeliveryIntegrationDTOList = new List<OnlineOrderDeliveryIntegrationDTO>();
            if (onlineOrderDeliveryIntegrationDTOList == null || onlineOrderDeliveryIntegrationDTOList.Any() == false)
            {
                log.LogMethodExit(savedDeliveryIntegrationDTOList);
                return savedDeliveryIntegrationDTOList;
            }
            foreach (OnlineOrderDeliveryIntegrationDTO deliveryIntegrationDTO in onlineOrderDeliveryIntegrationDTOList)
            {
                OnlineOrderDeliveryIntegrationBL deliveryIntegrationBL = new OnlineOrderDeliveryIntegrationBL(executionContext, deliveryIntegrationDTO, sqlTransaction);
                deliveryIntegrationBL.Save(sqlTransaction);
                savedDeliveryIntegrationDTOList.Add(deliveryIntegrationBL.OnlineOrderDeliveryIntegrationDTO);
            }
            log.LogMethodExit(savedDeliveryIntegrationDTOList);
            return savedDeliveryIntegrationDTOList;
        }

        /// <summary>
        /// DeliveryIntegration Module Last Update Date
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public DateTime? GetDeliveryIntegrationModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            OnlineOrderDeliveryIntegrationDataHandler dataHandlerObject = new OnlineOrderDeliveryIntegrationDataHandler();
            DateTime? result = dataHandlerObject.GetDeliveryIntegrationModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

    }
}
