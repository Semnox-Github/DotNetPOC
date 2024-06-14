/********************************************************************************************
 * Project Name - Product Price
 * Description  - Represents promotion based price influencer
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      1-Sep-2021      Lakshminarayana           Created : Price container enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Product;
using Semnox.Parafait.Promotions;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Represents promotion based price influencer
    /// </summary>
    public class PromotionPriceInfluencer : PriceInfluencer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<PromotionDTO> promotionDTOList;
        private readonly Dictionary<int, List<int>> productIdCategoryIdListDictionary = new Dictionary<int, List<int>>();
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public PromotionPriceInfluencer(List<PromotionDTO> promotionDTOList, 
                                        List<ProductsContainerDTO> productsContainerDTOList,
                                        List<CategoryContainerDTO> categoryContainerDTOList)
        {
            log.LogMethodEntry(promotionDTOList, productsContainerDTOList, categoryContainerDTOList);
            this.promotionDTOList = promotionDTOList;
            Dictionary<int, CategoryContainerDTO> categoryIdCategoryContainerDTODictionary = new Dictionary<int, CategoryContainerDTO>();
            if(categoryContainerDTOList != null)
            {
                foreach (var categoryContainerDTO in categoryContainerDTOList)
                {
                    if(categoryIdCategoryContainerDTODictionary.ContainsKey(categoryContainerDTO.CategoryId))
                    {
                        continue;
                    }
                    categoryIdCategoryContainerDTODictionary.Add(categoryContainerDTO.CategoryId, categoryContainerDTO);
                }
            }
            if(productsContainerDTOList != null)
            {
                foreach (var productsContainerDTO in productsContainerDTOList)
                {
                    if(productIdCategoryIdListDictionary.ContainsKey(productsContainerDTO.ProductId))
                    {
                        continue;
                    }
                    List<int> categoryIdList = GetCategoryIdList(productsContainerDTO.CategoryId, categoryIdCategoryContainerDTODictionary);
                    productIdCategoryIdListDictionary.Add(productsContainerDTO.ProductId, categoryIdList);
                }
            }
            log.LogMethodExit();
        }

        private List<int> GetCategoryIdList(int categoryId, Dictionary<int, CategoryContainerDTO> categoryIdCategoryContainerDTODictionary)
        {
            log.LogMethodEntry(categoryId, categoryIdCategoryContainerDTODictionary);
            List<int> result = new List<int>();
            if(categoryId == -1)
            {
                log.LogMethodExit(result, "categoryId == -1");
                return result;
            }
            result.Add(categoryId);
            if(categoryIdCategoryContainerDTODictionary.ContainsKey(categoryId))
            {
                result.AddRange(categoryIdCategoryContainerDTODictionary[categoryId].ParentCategoryIdList);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Influence the price container dto
        /// </summary>
        public override List<PriceContainerDTO> Influence(List<PriceContainerDTO> priceContainerDTOList)
        {
            log.LogMethodEntry(priceContainerDTOList);
            if (priceContainerDTOList == null || 
                priceContainerDTOList.Any() == false)
            {
                List<PriceContainerDTO> result = new List<PriceContainerDTO>();
                log.LogMethodExit(result);
                return result;
            }
            if (promotionDTOList == null ||
                promotionDTOList.Any() == false)
            {
                log.LogMethodExit(priceContainerDTOList, "promotionDTOList == null");
                return priceContainerDTOList;
            }
            foreach (var priceContainerDTO in priceContainerDTOList)
            {
                UpdatePromotionPrice(priceContainerDTO);
            }
            log.LogMethodExit(priceContainerDTOList);
            return priceContainerDTOList;
        }

        private void UpdatePromotionPrice(PriceContainerDTO priceContainerDTO)
        {
            log.LogMethodEntry(priceContainerDTO);
            foreach (var priceContainerDetailDTO in priceContainerDTO.PriceContainerDetailDTOList)
            {
                List<PromotionDTO> applicablePromotionDTOList = promotionDTOList.Where(x => IsPromotionAvailable(x, priceContainerDTO.ProductId, priceContainerDTO.MembershipId, priceContainerDetailDTO.StartDateTime)).ToList();
                if (priceContainerDTO.MembershipId > -1)
                {
                    List<PromotionDTO> membershipPromotionDTOList = applicablePromotionDTOList.Where(x => x.PromotionRuleDTOList != null && x.PromotionRuleDTOList.Any(y => y.MembershipId == priceContainerDTO.MembershipId)).ToList();
                    if (membershipPromotionDTOList.Any())
                    {
                        applicablePromotionDTOList = membershipPromotionDTOList;
                    }
                    else
                    {
                        applicablePromotionDTOList = applicablePromotionDTOList.Where(x => x.PromotionRuleDTOList == null || x.PromotionRuleDTOList.Any() == false).ToList();
                    }
                }
                else
                {
                    applicablePromotionDTOList = applicablePromotionDTOList.Where(x => x.PromotionRuleDTOList == null || x.PromotionRuleDTOList.Any() == false).ToList();
                }
                if (applicablePromotionDTOList.Any() == false)
                {
                    continue;
                }
                PromotionDTO applicablePromotionDTO = applicablePromotionDTOList.OrderBy(x => GetPromotionSortOrder(x, priceContainerDTO.ProductId)).First();
                PromotionDetailDTO promotionDetailDTO = applicablePromotionDTO.PromotionDetailDTOList.FirstOrDefault(x => x.ProductId == priceContainerDTO.ProductId);
                if (promotionDetailDTO == null)
                {
                    List<int> categoryIdList = GetCategoryIdList(priceContainerDTO.ProductId);
                    promotionDetailDTO = applicablePromotionDTO.PromotionDetailDTOList.FirstOrDefault(x => categoryIdList.Contains(x.CategoryId));
                    if (promotionDetailDTO == null)
                    {
                        promotionDetailDTO = applicablePromotionDTO.PromotionDetailDTOList.FirstOrDefault(x => x.ProductId == -1 && x.CategoryId == -1);
                    }
                }
                if (promotionDetailDTO == null)
                {
                    continue;
                }
                priceContainerDetailDTO.PromotionId = applicablePromotionDTO.PromotionId;
                if (promotionDetailDTO.AbsoluteCredits.HasValue)
                {
                    priceContainerDetailDTO.PromotionPrice = promotionDetailDTO.AbsoluteCredits.Value;
                }
                else if (promotionDetailDTO.DiscountAmount.HasValue)
                {
                    priceContainerDetailDTO.PromotionPrice = priceContainerDetailDTO.BasePrice - promotionDetailDTO.DiscountAmount.Value;
                }
                else if (promotionDetailDTO.DiscountOnCredits.HasValue)
                {
                    priceContainerDetailDTO.PromotionPrice = priceContainerDetailDTO.BasePrice - priceContainerDetailDTO.BasePrice * promotionDetailDTO.DiscountOnCredits.Value / 100m;
                }
                if (priceContainerDetailDTO.PromotionPrice.HasValue && priceContainerDetailDTO.PromotionPrice.Value < 0)
                {
                    priceContainerDetailDTO.PromotionPrice = 0;
                }
            }

            log.LogMethodExit();
        }

        private int GetPromotionSortOrder(PromotionDTO promotionDTO, int productId)
        {
            log.LogMethodEntry(promotionDTO, productId);
            int result = 0;
            List<int> categoryIdList = GetCategoryIdList(productId);
            if (promotionDTO.PromotionDetailDTOList.Any(x => x.ProductId == productId))
            {
                result = 0;
            }
            else if (promotionDTO.PromotionDetailDTOList.Any(x => x.CategoryId != -1 && categoryIdList.Contains(x.CategoryId)))
            {
                result = 1;
            }
            else
            {
                result = 2;
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<int> GetCategoryIdList(int productId)
        {
            log.LogMethodEntry(productId);
            List<int> result;
            if (productIdCategoryIdListDictionary.ContainsKey(productId) == false)
            {
                result = new List<int>();
                log.LogMethodExit(result, "invalid product");
                return result;
            }
            result = productIdCategoryIdListDictionary[productId];
            log.LogMethodExit(result);
            return result;
        }

        private bool IsPromotionAvailable(PromotionDTO promotionDTO, int productId, int membershipId, DateTime startDateTime)
        {
            log.LogMethodEntry(promotionDTO, productId, membershipId, startDateTime);
            bool result = false;
            PromotionCalendarCalculator promotionCalendarCalculator = new PromotionCalendarCalculator(promotionDTO);
            if (promotionDTO.PromotionRuleDTOList != null &&
               promotionDTO.PromotionRuleDTOList.Any() &&
               promotionDTO.PromotionRuleDTOList.Any(x => x.MembershipId == membershipId) == false)
            {
                log.LogMethodExit(result, "promotion is not for the membership");
                return result;
            }
            if (promotionCalendarCalculator.IsPromotionExistOn(startDateTime) == false)
            {
                log.LogMethodExit(result, "promotion is not started");
                return result;
            }
            List<int> categoryIdList = GetCategoryIdList(productId);
            if (promotionDTO.PromotionDetailDTOList.Any(x => x.ProductId == productId || categoryIdList.Contains(x.CategoryId) || (x.ProductId == -1 && x.CategoryId == -1)) == false)
            {
                log.LogMethodExit(result, "given product is not promoted");
                return result;
            }
            result = true;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Splits the continuousDateTimeRanges
        /// </summary>
        public override ContinuousDateTimeRanges SplitDateTimeRange(ContinuousDateTimeRanges continuousDateTimeRanges, TimeSpan margin)
        {
            log.LogMethodEntry(continuousDateTimeRanges, margin);
            ContinuousDateTimeRanges result = continuousDateTimeRanges;
            if (promotionDTOList == null || promotionDTOList.Any() == false)
            {
                log.LogMethodExit(result);
                return result;
            }
            foreach (var promotionDTO in promotionDTOList)
            {
                PromotionCalendarCalculator promotionCalendarCalculator = new PromotionCalendarCalculator(promotionDTO);
                List<DateTime> significantDateTimes = promotionCalendarCalculator.GetSignificantDateTimesInRange(continuousDateTimeRanges, margin);
                foreach (var significantDateTime in significantDateTimes)
                {
                    result = result.Split(significantDateTime, margin);
                }
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
