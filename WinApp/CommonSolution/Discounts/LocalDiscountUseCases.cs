/********************************************************************************************
 * Project Name - Discounts 
 * Description  - LocalDiscountUseCases class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      14-Apr-2021       Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// LocalDiscountUseCases
    /// </summary>
    public class LocalDiscountUseCases : LocalUseCases, IDiscountUseCases
    {
        private Semnox.Parafait.logging.Logger log;

        /// <summary>
        /// LocalDiscountUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalDiscountUseCases(ExecutionContext executionContext, string requestGuid)
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
            return await Task<DiscountContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    DiscountContainerList.Rebuild(siteId);
                }
                DiscountContainerDTOCollection result = DiscountContainerList.GetDiscountContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        /// <summary>
        /// Get Available Discount values
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="hash">hash</param>
        /// <param name="startDate">startDate</param>
        /// <param name="endDate">endDate</param>
        /// <param name="rebuildCache">rebuildCache</param>
        /// <returns></returns>
        public async Task<DiscountAvailabilityContainerDTOCollection> GetDiscountAvailabilityDTOCollection(int siteId, DateTime startDate, DateTime endDate, string hash, bool rebuildCache)
        {
            return await Task<DiscountAvailabilityContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    DiscountContainerList.Rebuild(siteId);
                }
                DiscountAvailabilityContainerDTOCollection result = DiscountContainerList.GetDiscountAvailabilityDTOCollection(siteId, startDate, endDate);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
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
            return await Task<List<DiscountsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(isActive, discountId, discountName, discountType, displayInPOS, minimumCreditsGreaterThan,
                                   minimumSaleAmountGreaterThan, automaticApply, couponMandatory, discountedGameId, discountedProductId,
                                   discountedCategoryId, siteId, pageNumber, pageSize, loadChildRecords, loadActiveChildRecords,
                                   onlyDiscountedChildRecords);
                using(UnitOfWork unitOfWork = new UnitOfWork())
                {
                    DiscountsListBL discountsListBL = new DiscountsListBL(executionContext, unitOfWork);
                    if(string.IsNullOrWhiteSpace(discountType))
                    {
                        discountType = DiscountsBL.DISCOUNT_TYPE_TRANSACTION;
                    }
                    SearchParameterList<DiscountsDTO.SearchByParameters> searchParameters = new SearchParameterList<DiscountsDTO.SearchByParameters>();
                    searchParameters.Add(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, isActive);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISCOUNT_ID, discountId);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISCOUNT_NAME, discountName);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISCOUNT_TYPE, discountType);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISPLAY_IN_POS, displayInPOS);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.MINIMUM_CREDITS_GREATER_THAN, minimumCreditsGreaterThan);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.MINIMUM_SALE_AMOUNT_GREATER_THAN, minimumSaleAmountGreaterThan);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.AUTOMATIC_APPLY, automaticApply);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.COUPON_MANDATORY, couponMandatory);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISCOUNTED_GAME_ID, discountedGameId);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISCOUNTED_PRODUCT_ID, discountedProductId);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISCOUNTED_CATEGORY_ID, discountedCategoryId);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.SITE_ID, executionContext.SiteId);
                    List<DiscountsDTO> result = discountsListBL.GetDiscountsDTOList(searchParameters, loadChildRecords, loadActiveChildRecords, onlyDiscountedChildRecords, pageNumber, pageSize);
                    return result;
                }
            });
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
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(isActive, discountId, discountName, discountType, displayInPOS, minimumCreditsGreaterThan,
                                   minimumSaleAmountGreaterThan, automaticApply, couponMandatory, discountedGameId, discountedProductId,
                                   discountedCategoryId, siteId);
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    DiscountsListBL discountsListBL = new DiscountsListBL(executionContext, unitOfWork);
                    if (string.IsNullOrWhiteSpace(discountType))
                    {
                        discountType = DiscountsBL.DISCOUNT_TYPE_TRANSACTION;
                    }
                    SearchParameterList<DiscountsDTO.SearchByParameters> searchParameters = new SearchParameterList<DiscountsDTO.SearchByParameters>();
                    searchParameters.Add(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, isActive);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISCOUNT_ID, discountId);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISCOUNT_NAME, discountName);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISCOUNT_TYPE, discountType);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISPLAY_IN_POS, displayInPOS);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.MINIMUM_CREDITS_GREATER_THAN, minimumCreditsGreaterThan);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.MINIMUM_SALE_AMOUNT_GREATER_THAN, minimumSaleAmountGreaterThan);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.AUTOMATIC_APPLY, automaticApply);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.COUPON_MANDATORY, couponMandatory);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISCOUNTED_GAME_ID, discountedGameId);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISCOUNTED_PRODUCT_ID, discountedProductId);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.DISCOUNTED_CATEGORY_ID, discountedCategoryId);
                    searchParameters.Add(DiscountsDTO.SearchByParameters.SITE_ID, executionContext.SiteId);
                    int result = discountsListBL.GetTransactionDTOListCount(searchParameters);
                    return result;
                }
            });
        }

        public async Task<List<DiscountsDTO>> SaveDiscountsDTOList(List<DiscountsDTO> discountsDTOList)
        {
            return await Task<List<DiscountsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(discountsDTOList);
                using(UnitOfWork unitOfWork = new UnitOfWork())
                {
                    if (IsDuplicateRequest())
                    {
                        DiscountsListBL discountsListBL = new DiscountsListBL();
                        List<DiscountsDTO> result = discountsListBL.GetDiscountsDTOList(GetEntityGuidList().ToList(), true, false);
                        log.LogMethodExit(result, "Duplicate request");
                        return result;
                    }
                    else
                    {
                        unitOfWork.Begin();
                        DiscountsListBL discountsListBL = new DiscountsListBL(executionContext, unitOfWork);
                        List<DiscountsDTO> result = discountsListBL.Save(discountsDTOList);
                        CreateApplicationRequestLog("Discounts", "Save Discounts", result.Select(x => x.Guid), unitOfWork.SQLTrx);
                        unitOfWork.Commit();
                        log.LogMethodExit(result);
                        return result;
                    }
                    
                }
                
            });
        }

        public async Task ClearDiscountedProducts(int discountId)
        {
            await Task.Factory.StartNew(() =>
            {
                log.LogMethodEntry(discountId);
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    if (IsDuplicateRequest())
                    {
                        log.LogMethodExit("Duplicate request");
                        return;
                    }
                    unitOfWork.Begin();
                    DiscountsBL discountsBL = new DiscountsBL(executionContext, unitOfWork, discountId, true, true);
                    discountsBL.InactivateDiscountedProducts();
                    CreateApplicationRequestLog("Discounts", "Clear Discounted Products", discountsBL.DiscountsDTO.Guid, unitOfWork.SQLTrx);
                    unitOfWork.Commit();
                    log.LogMethodExit();
                }
            });
        }

        public async Task DeselectDiscountedProducts(int discountId)
        {
            await Task.Factory.StartNew(() =>
            {
                log.LogMethodEntry(discountId);
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    if (IsDuplicateRequest())
                    {
                        log.LogMethodExit("Duplicate request");
                        return;
                    }
                    unitOfWork.Begin();
                    DiscountsBL discountsBL = new DiscountsBL(executionContext, unitOfWork, discountId, true, true);
                    discountsBL.ClearDiscountedProducts();
                    CreateApplicationRequestLog("Discounts", "Deselect Discounted Products", discountsBL.DiscountsDTO.Guid, unitOfWork.SQLTrx);
                    unitOfWork.Commit();
                    log.LogMethodExit();
                }
            });
        }

        public async Task ClearDiscountedGames(int discountId)
        {
            await Task.Factory.StartNew(() =>
            {
                log.LogMethodEntry(discountId);
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    if (IsDuplicateRequest())
                    {
                        log.LogMethodExit("Duplicate request");
                        return;
                    }
                    unitOfWork.Begin();
                    DiscountsBL discountsBL = new DiscountsBL(executionContext, unitOfWork, discountId, true, true);
                    discountsBL.InactivateDiscountedGames();
                    CreateApplicationRequestLog("Discounts", "Clear Discounted Games", discountsBL.DiscountsDTO.Guid, unitOfWork.SQLTrx);
                    unitOfWork.Commit();
                    log.LogMethodExit();
                }
            });
        }
    }
}
