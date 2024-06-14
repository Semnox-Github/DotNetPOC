/********************************************************************************************
 * Project Name - Discounts
 * Description  - RemoteDiscountUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      14-Apr-2021      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// RemoteDiscountUseCases
    /// </summary>
    public class RemoteDiscountUseCases : RemoteUseCases, IDiscountUseCases
    {
        private Semnox.Parafait.logging.Logger log;
        private const string DISCOUNT_URL = "api/Discount/Discounts";
        private const string DISCOUNT_COUNT_URL = "api/Discount/DiscountsCount";
        private const string CLEAR_DISCOUNTED_PRODUCT_URL = "api/Discount/ClearDiscountedProducts";
        private const string CLEAR_DISCOUNTED_GAME_URL = "api/Discount/ClearDiscountedGames";
        private const string DESELCT_DISCOUNTED_PRODUCT_URL = "api/Discount/DeselectDiscountedProducts";
        private const string DISCOUNT_CONTAINER_URL = "api/Discount/DiscountContainer";
        private const string DISCOUNT_AVAILABILITY_CONTAINER_URL = "api/Discount/DiscountAvailabilityContainer";

        /// <summary>
        /// RemoteProductSubscriptionUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteDiscountUseCases(ExecutionContext executionContext, string requestGuid)
           : base(executionContext, requestGuid)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, requestGuid);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Discount values
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="hash">hash</param>
        /// <param name="rebuildCache">rebuildCache</param>
        /// <returns></returns>
        public async Task<DiscountContainerDTOCollection> GetDiscountContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            DiscountContainerDTOCollection result = await Get<DiscountContainerDTOCollection>(DISCOUNT_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get Available Discount values
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="startDate">startDate</param>
        /// <param name="endDate">endDate</param>
        /// <param name="hash">hash</param>
        /// <param name="rebuildCache">rebuildCache</param>
        /// <returns></returns>
        public async Task<DiscountAvailabilityContainerDTOCollection> GetDiscountAvailabilityDTOCollection(int siteId, DateTime startDate, DateTime endDate, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new WebApiGetRequestParameterCollection("siteId", siteId, "startDate", startDate, "endDate", endDate, "hash", hash, "rebuildCache", rebuildCache);
            DiscountAvailabilityContainerDTOCollection result = await Get<DiscountAvailabilityContainerDTOCollection>(DISCOUNT_AVAILABILITY_CONTAINER_URL,  parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<DiscountsDTO>> GetDiscountsDTOList(string isActive = null, 
                                                            int discountId = -1, 
                                                            string discountName = null, 
                                                            string discountType = null, 
                                                            string displayInPOS = null, 
                                                            decimal? minimumCreditsGreaterThan = null, 
                                                            decimal? minimumSaleAmountGreaterThan = null, 
                                                            string automaticApply = null, 
                                                            string couponMandatory = null, 
                                                            int discountedGameId = -1, 
                                                            int discountedProductId = -1, 
                                                            int discountedCategoryId = -1, 
                                                            int siteId = -1, 
                                                            int pageNumber = 0, 
                                                            int pageSize = 0, 
                                                            bool loadChildRecords = false, 
                                                            bool loadActiveChildRecords = true, 
                                                            bool onlyDiscountedChildRecords = false)
        {
            log.LogMethodEntry(isActive, discountId, discountName, discountType, displayInPOS, minimumCreditsGreaterThan,
                               minimumSaleAmountGreaterThan, automaticApply, couponMandatory, discountedGameId, discountedProductId,
                               discountedCategoryId, siteId, pageNumber, pageSize, loadChildRecords, loadActiveChildRecords,
                               onlyDiscountedChildRecords);
            WebApiGetRequestParameterCollection parameters = new WebApiGetRequestParameterCollection("isActive", isActive,
                                                                                                     "discountId", discountId,
                                                                                                     "discountName", discountName,
                                                                                                     "discountType", discountType,
                                                                                                     "displayInPOS", displayInPOS,
                                                                                                     "minimumCreditsGreaterThan", minimumCreditsGreaterThan,
                                                                                                     "minimumSaleAmountGreaterThan", minimumSaleAmountGreaterThan,
                                                                                                     "automaticApply", automaticApply,
                                                                                                     "couponMandatory", couponMandatory,
                                                                                                     "discountedGameId", discountedGameId,
                                                                                                     "discountedProductId", discountedProductId,
                                                                                                     "discountedCategoryId", discountedCategoryId,
                                                                                                     "siteId", siteId,
                                                                                                     "pageNumber", pageNumber,
                                                                                                     "pageSize", pageSize,
                                                                                                     "loadChildRecords", loadChildRecords,
                                                                                                     "loadActiveChildRecords", loadActiveChildRecords,
                                                                                                     "onlyDiscountedChildRecords", onlyDiscountedChildRecords
                                                                                                     );
            List<DiscountsDTO> result = await Get<List<DiscountsDTO>>(DISCOUNT_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<int> GetDiscountsDTOListCount(string isActive = null,
                                                        int discountId = -1,
                                                        string discountName = null,
                                                        string discountType = null,
                                                        string displayInPOS = null,
                                                        decimal? minimumCreditsGreaterThan = null,
                                                        decimal? minimumSaleAmountGreaterThan = null,
                                                        string automaticApply = null,
                                                        string couponMandatory = null,
                                                        int discountedGameId = -1,
                                                        int discountedProductId = -1,
                                                        int discountedCategoryId = -1,
                                                        int siteId = -1)
        {
            log.LogMethodEntry(isActive, discountId, discountName, discountType, displayInPOS, minimumCreditsGreaterThan,
                               minimumSaleAmountGreaterThan, automaticApply, couponMandatory, discountedGameId, discountedProductId,
                               discountedCategoryId, siteId);
            WebApiGetRequestParameterCollection parameters = new WebApiGetRequestParameterCollection("isActive", isActive,
                                                                                                     "discountId", discountId,
                                                                                                     "discountName", discountName,
                                                                                                     "discountType", discountType,
                                                                                                     "displayInPOS", displayInPOS,
                                                                                                     "minimumCreditsGreaterThan", minimumCreditsGreaterThan,
                                                                                                     "minimumSaleAmountGreaterThan", minimumSaleAmountGreaterThan,
                                                                                                     "automaticApply", automaticApply,
                                                                                                     "couponMandatory", couponMandatory,
                                                                                                     "discountedGameId", discountedGameId,
                                                                                                     "discountedProductId", discountedProductId,
                                                                                                     "discountedCategoryId", discountedCategoryId,
                                                                                                     "siteId", siteId
                                                                                                     );
            int result = await Get<int>(DISCOUNT_COUNT_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<DiscountsDTO>> SaveDiscountsDTOList(List<DiscountsDTO> discountsDTOList)
        {
            log.LogMethodEntry(discountsDTOList);
            List<DiscountsDTO> result = await Post<List<DiscountsDTO>>(DISCOUNT_URL, discountsDTOList);
            log.LogMethodExit(result);
            return result;
        }

        public async Task ClearDiscountedProducts(int discountId)
        {
            log.LogMethodEntry(discountId);
            await Post<string>(CLEAR_DISCOUNTED_PRODUCT_URL, discountId);
            log.LogMethodExit();
        }

        public async Task DeselectDiscountedProducts(int discountId)
        {
            log.LogMethodEntry(discountId);
            await Post<string>(DESELCT_DISCOUNTED_PRODUCT_URL, discountId);
            log.LogMethodExit();
        }

        public async Task ClearDiscountedGames(int discountId)
        {
            log.LogMethodEntry(discountId);
            await Post<string>(CLEAR_DISCOUNTED_GAME_URL, discountId);
            log.LogMethodExit();
        }
    }
}
