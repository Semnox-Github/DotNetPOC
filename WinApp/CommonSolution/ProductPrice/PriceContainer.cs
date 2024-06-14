/********************************************************************************************
 * Project Name - Product Price
 * Description  - Container class to hold the product prices
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      18-Aug-2021      Lakshminarayana           Created : Price container enhancement
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Customer.Membership.Sample;
using Semnox.Parafait.PriceList;
using Semnox.Parafait.Product;
using Semnox.Parafait.Promotions;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Container class to hold the product prices
    /// </summary>
    public class PriceContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int siteId;
        private readonly Cache<PriceContainerKey, PriceContainerDTOCollection> priceContainerDTOCollectionCache = new Cache<PriceContainerKey, PriceContainerDTOCollection>();
        private readonly DateTime? maxLastUpdateTime;
        private readonly List<PriceContainerDTO> basePriceContainerDTOList = new List<PriceContainerDTO>();
        private readonly PriceSelectionMode priceSelectionMode;
        private readonly PriceContainerDTOConsolidator priceContainerDTOConsolidator = new PriceContainerDTOConsolidator();
        private readonly Dictionary<int, ProductsContainerDTO> productIdProductsContainerDTODictionary = new Dictionary<int, ProductsContainerDTO>();
        private readonly Dictionary<int, Dictionary<int, List<PriceListProductsDTO>>> priceListIdProductIdPriceListProductsDTOListDictionary = new Dictionary<int, Dictionary<int, List<PriceListProductsDTO>>>();
        private readonly Dictionary<int, PriceListDTO> priceListIdPriceListDTODictionary = new Dictionary<int, PriceListDTO>();
        private readonly List<ProductsContainerDTO> productsContainerDTOList;
        private readonly List<PromotionDTO> promotionDTOList;
        private readonly List<CategoryContainerDTO> categoryContainerDTOList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="siteId">site identifier</param>
        public PriceContainer(int siteId)
            : this(siteId,
                  GetProductsContainerDTOList(siteId),
                  GetPriceListDTOList(siteId),
                  GetPromotionDTOList(siteId),
                  GetCategoryContainerDTOList(siteId),
                  GetMaxLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>

        public PriceContainer(int siteId,
                              List<ProductsContainerDTO> productsContainerDTOList,
                              List<PriceListDTO> priceListDTOList,
                              List<PromotionDTO> promotionDTOList,
                              List<CategoryContainerDTO> categoryContainerDTOList,
                              DateTime? maxLastUpdateTime)
        {
            log.LogMethodEntry(siteId, productsContainerDTOList, priceListDTOList, promotionDTOList, maxLastUpdateTime);
            this.siteId = siteId;
            this.maxLastUpdateTime = maxLastUpdateTime;
            this.productsContainerDTOList = productsContainerDTOList;
            this.promotionDTOList = promotionDTOList;
            this.categoryContainerDTOList = categoryContainerDTOList;
            priceSelectionMode = PriceSelectionMode.HIERARCHICAL_PRICE_SELECTION_MODE;
            
            if (priceListDTOList != null && priceListDTOList.Any())
            {
                foreach (var priceListDTO in priceListDTOList)
                {
                    if(priceListIdProductIdPriceListProductsDTOListDictionary.ContainsKey(priceListDTO.PriceListId) == false)
                    {
                        priceListIdProductIdPriceListProductsDTOListDictionary.Add(priceListDTO.PriceListId, new Dictionary<int, List<PriceListProductsDTO>>());
                    }
                    if(priceListIdPriceListDTODictionary.ContainsKey(priceListDTO.PriceListId) == false)
                    {
                        priceListIdPriceListDTODictionary.Add(priceListDTO.PriceListId, priceListDTO);
                    }
                    if(priceListDTO.PriceListProductsList == null ||
                        priceListDTO.PriceListProductsList.Any() == false)
                    {
                        continue;
                    }
                    foreach (var priceListProductsDTO in priceListDTO.PriceListProductsList)
                    {
                        if(priceListIdProductIdPriceListProductsDTOListDictionary[priceListDTO.PriceListId].ContainsKey(priceListProductsDTO.ProductId) == false)
                        {
                            priceListIdProductIdPriceListProductsDTOListDictionary[priceListDTO.PriceListId].Add(priceListProductsDTO.ProductId, new List<PriceListProductsDTO>());
                        }
                        priceListIdProductIdPriceListProductsDTOListDictionary[priceListDTO.PriceListId][priceListProductsDTO.ProductId].Add(priceListProductsDTO);
                    }
                }
            }
            

            if (productsContainerDTOList != null)
            {
                foreach (var productsContainerDTO in productsContainerDTOList)
                {
                    if (productIdProductsContainerDTODictionary.ContainsKey(productsContainerDTO.ProductId))
                    {
                        continue;
                    }
                    productIdProductsContainerDTODictionary.Add(productsContainerDTO.ProductId, productsContainerDTO);
                    PriceContainerDTO priceContainerDTO = new PriceContainerDTO(productsContainerDTO.ProductId);
                    basePriceContainerDTOList.Add(priceContainerDTO);
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the matching PriceContainerDetailDTO
        /// </summary>
        public PriceContainerDetailDTO GetPriceContainerDetailDTO(int productId, int membershipId, int userRoleId, int transactionProfileId, DateTimeRange dateTimeRange, DateTime dateTime)
        {
            log.LogMethodEntry(productId, membershipId, userRoleId, transactionProfileId, dateTimeRange, dateTime);
            PriceContainerDTOCollection priceContainerDTOCollection = GetPriceContainerDTOCollection(membershipId, userRoleId, transactionProfileId, dateTimeRange);
            PriceContainerDetailDTO result = priceContainerDTOCollection.GetPriceContainerDetailDTO(productId, dateTime);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the matching PriceContainerDTO
        /// </summary>
        public PriceContainerDTO GetPriceContainerDTO(int productId, int membershipId, int userRoleId, int transactionProfileId, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(productId, membershipId, userRoleId, transactionProfileId, dateTimeRange);
            PriceContainerDTOCollection priceContainerDTOCollection = GetPriceContainerDTOCollection(membershipId, userRoleId, transactionProfileId, dateTimeRange);
            PriceContainerDTO result = priceContainerDTOCollection.GetPriceContainerDTO(productId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the data structure holding the price of the products for a given date range
        /// </summary>
        /// <param name="membershipId"></param>
        /// <param name="userRoleId"></param>
        /// <param name="transactionProfileId"></param>
        /// <param name="dateTimeRange"></param>
        /// <returns></returns>
        public PriceContainerDTOCollection GetPriceContainerDTOCollection(int membershipId, int userRoleId, int transactionProfileId, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(membershipId, userRoleId, transactionProfileId, dateTimeRange);
            PriceContainerKey priceContainerKey = new PriceContainerKey(siteId, membershipId, userRoleId, transactionProfileId, dateTimeRange);
            var result = priceContainerDTOCollectionCache.GetOrAdd(priceContainerKey, (k) => CreatePriceContainerDTOCollection(priceContainerKey));
            log.LogMethodExit(result);
            return result;
        }

        private PriceContainerDTOCollection CreatePriceContainerDTOCollection(PriceContainerKey priceContainerKey)
        {
            log.LogMethodEntry(priceContainerKey);
            PriceContainerDTOCollection result;
            try
            {
                List<PriceInfluencer> priceInfluencerList = new List<PriceInfluencer>();
                if(priceContainerKey.UserRoleId > -1)
                {
                    UserRoleContainerDTO userRoleContainerDTO = GetUserRoleContainerDTO(priceContainerKey.UserRoleId);
                    priceInfluencerList.Add(new UserRolePriceInfluencer(userRoleContainerDTO, priceListIdPriceListDTODictionary, priceListIdProductIdPriceListProductsDTOListDictionary));
                }
                if(priceContainerKey.MembershipId > -1)
                {
                    MembershipContainerDTO membershipContainerDTO = GetMembershipContainerDTO(priceContainerKey.MembershipId);
                    priceInfluencerList.Add(new MembershipPriceInfluencer(membershipContainerDTO, priceListIdPriceListDTODictionary, priceListIdProductIdPriceListProductsDTOListDictionary));
                }
                if(priceContainerKey.TransactionProfileId > -1)
                {
                    TransactionProfileContainerDTO transactionProfileContainerDTO = GetTransactionProfileContainerDTO(priceContainerKey.TransactionProfileId);
                    priceInfluencerList.Add(new TransactionProfilePriceInfluencer(transactionProfileContainerDTO, priceListIdPriceListDTODictionary, priceListIdProductIdPriceListProductsDTOListDictionary));
                }
                priceInfluencerList.Add(new PromotionPriceInfluencer(promotionDTOList, productsContainerDTOList, categoryContainerDTOList));

                ContinuousDateTimeRanges continuousDateTimeRanges = priceContainerKey.DateTimeRange;
                foreach (var priceInfluencer in priceInfluencerList)
                {
                    continuousDateTimeRanges = priceInfluencer.SplitDateTimeRange(continuousDateTimeRanges, TimeSpan.FromMinutes(5));
                }
                List<PriceContainerDTO> priceContainerDTOList = GetBasePriceContainerDTOList(continuousDateTimeRanges);
                foreach (var priceInfluencer in priceInfluencerList)
                {
                    priceContainerDTOList = priceInfluencer.Influence(priceContainerDTOList);
                }
                priceContainerDTOList = priceSelectionMode.SelectFinalPrice(priceContainerDTOList);
                priceContainerDTOList = priceContainerDTOConsolidator.Consolidate(priceContainerDTOList);
                result = new PriceContainerDTOCollection(priceContainerDTOList);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while creating the price container", ex);
                result = new PriceContainerDTOCollection(new List<PriceContainerDTO>());
            }
            
            log.LogMethodExit(result);
            return result;
        }

        private List<PriceContainerDTO> GetBasePriceContainerDTOList(ContinuousDateTimeRanges continuousDateTimeRanges)
        {
            log.LogMethodEntry(continuousDateTimeRanges);
            List<PriceContainerDTO> priceContainerDTOList = GetCopy(basePriceContainerDTOList);
            foreach (var priceContainerDTO in priceContainerDTOList)
            {
                List<PriceContainerDetailDTO> priceContainerDetailDTOList = new List<PriceContainerDetailDTO>();
                foreach (var dateTimeRange in continuousDateTimeRanges.DateTimeRanges)
                {
                    ProductsContainerDTO productsContainerDTO = productIdProductsContainerDTODictionary[priceContainerDTO.ProductId];
                    decimal price = productsContainerDTO.Price;
                    if ("Y".Equals(productsContainerDTO.TaxInclusivePrice, StringComparison.InvariantCultureIgnoreCase) &&
                        productsContainerDTO.TaxId > -1)
                    {
                        TaxContainerDTO taxContainerDTO = TaxContainerList.GetTaxContainerDTO(siteId, productsContainerDTO.TaxId);
                        price = price / (1 + (decimal)taxContainerDTO.TaxPercentage / 100m);
                        price = Math.Round(price, 4);
                    }
                    PriceContainerDetailDTO priceContainerDetailDTO = new PriceContainerDetailDTO(dateTimeRange.StartDateTime, dateTimeRange.EndDateTime, price);
                    priceContainerDetailDTOList.Add(priceContainerDetailDTO);
                }
                priceContainerDTO.PriceContainerDetailDTOList = priceContainerDetailDTOList;
            }
            
            log.LogMethodExit(priceContainerDTOList);
            return priceContainerDTOList;
        }

        private List<PriceContainerDTO> GetCopy(List<PriceContainerDTO> priceContainerDTOList)
        {
            log.LogMethodEntry(priceContainerDTOList);
            List < PriceContainerDTO > result = new List<PriceContainerDTO>();
            if (priceContainerDTOList == null || priceContainerDTOList.Any() == false)
            {
                log.LogMethodExit(result);
                return result;
            }
            foreach (var priceContainerDTO in priceContainerDTOList)
            {
                PriceContainerDTO copy = new PriceContainerDTO(priceContainerDTO);
                result.Add(copy);
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<ProductsContainerDTO> GetProductsContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = new List<ProductsContainerDTO>();
            var activeProductsContainerDTOList = ProductsContainerList.GetActiveProductsContainerDTOList(siteId, ManualProductType.SELLABLE.ToString());
            result.AddRange(activeProductsContainerDTOList);
            var systemProductsContainerDTOList = ProductsContainerList.GetSystemProductsContainerDTOList(siteId);
            result.AddRange(systemProductsContainerDTOList);
            log.LogMethodExit(result);
            return result;
        }

        private static List<MembershipContainerDTO> GetMembershipContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = MembershipContainerList.GetMembershipContainerDTOList(siteId);
            log.LogMethodExit(result);
            return result;
        }

        private static List<PromotionDTO> GetPromotionDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<PromotionDTO> result = null;
            try
            {
                PromotionListBL promotionListBL = new PromotionListBL();
                List<KeyValuePair<PromotionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PromotionDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<PromotionDTO.SearchByParameters, string>(PromotionDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<PromotionDTO.SearchByParameters, string>(PromotionDTO.SearchByParameters.PROMOTION_TYPE, "P"));
                searchParameters.Add(new KeyValuePair<PromotionDTO.SearchByParameters, string>(PromotionDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                result = promotionListBL.GetPromotionDTOList(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving promotion details", ex);
            }
            
            if(result == null)
            {
                result = new List<PromotionDTO>();
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<TransactionProfileContainerDTO> GetTransactionProfileContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = TransactionProfileContainerList.GetTransactionProfileContainerDTOList(siteId);
            log.LogMethodExit(result);
            return result;
        }

        private static List<CategoryContainerDTO> GetCategoryContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = CategoryContainerList.GetCategoryContainerDTOList(siteId);
            log.LogMethodExit(result);
            return result;
        }

        private static List<UserRoleContainerDTO> GetUserRoleContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = UserRoleContainerList.GetUserRoleContainerDTOList(siteId);
            log.LogMethodExit(result);
            return result;
        }

        private static List<PriceListDTO> GetPriceListDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<PriceListDTO> result = null;
            try
            {
                PriceListList priceListListBL = new PriceListList();
                List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>> searchParameters = new List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>>();
                searchParameters.Add(new KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>(PriceListDTO.SearchByPriceListParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>(PriceListDTO.SearchByPriceListParameters.IS_ACTIVE, "1"));
                result = priceListListBL.GetAllPriceListProducts(searchParameters, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving transaction profile details", ex);
            }

            if (result == null)
            {
                result = new List<PriceListDTO>();
            }
            log.LogMethodExit(result);
            return result;
        }

        private static DateTime? GetMaxLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                MembershipsList membershipsList = new MembershipsList();
                result = membershipsList.GetMembershipLastUpdateTime(siteId);
                ProductsList productsList = new ProductsList();
                DateTime? productModuleLastUpdateTime = productsList.GetProductsLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (productModuleLastUpdateTime.HasValue && result.Value < productModuleLastUpdateTime.Value))
                {
                    result = productModuleLastUpdateTime;
                }
                UserRolesList userRolesList = new UserRolesList();
                DateTime? userRoleModuleLastUpdateTime = userRolesList.GetUserRoleModuleLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (userRoleModuleLastUpdateTime.HasValue && result.Value < userRoleModuleLastUpdateTime.Value))
                {
                    result = userRoleModuleLastUpdateTime;
                }
                PriceListList priceListList = new PriceListList();
                DateTime? priceListModuleLastUpdateTime = priceListList.GetPriceListModuleLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (priceListModuleLastUpdateTime.HasValue && result.Value < priceListModuleLastUpdateTime.Value))
                {
                    result = priceListModuleLastUpdateTime;
                }

                TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL();
                DateTime? transactionProfileModuleLastUpdateTime = transactionProfileListBL.GetTransactionProfileModuleLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (transactionProfileModuleLastUpdateTime.HasValue && result.Value < transactionProfileModuleLastUpdateTime.Value))
                {
                    result = transactionProfileModuleLastUpdateTime;
                }

                CategoryList categoryList = new CategoryList();
                DateTime? categoryModuleLastUpdateTime = categoryList.GetCategoryLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (categoryModuleLastUpdateTime.HasValue && result.Value < categoryModuleLastUpdateTime.Value))
                {
                    result = categoryModuleLastUpdateTime;
                }

                PromotionListBL promotionListBL = new PromotionListBL();
                DateTime? promotionModuleModuleLastUpdateTime = promotionListBL.GetPromotionModuleLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (promotionModuleModuleLastUpdateTime.HasValue && result.Value < promotionModuleModuleLastUpdateTime.Value))
                {
                    result = promotionModuleModuleLastUpdateTime;
                }

                TaxList taxList = new TaxList();
                DateTime? taxModuleModuleLastUpdateTime = taxList.GetTaxModuleLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (taxModuleModuleLastUpdateTime.HasValue && result.Value < taxModuleModuleLastUpdateTime.Value))
                {
                    result = taxModuleModuleLastUpdateTime;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the price container max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the refreshed price container
        /// </summary>
        /// <returns></returns>
        public PriceContainer Refresh()
        {
            log.LogMethodEntry();
            RemoveOldPriceContainerDTOColletions();
            DateTime? updateTime = GetMaxLastUpdateTime(siteId);
            if (maxLastUpdateTime.HasValue
                && maxLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in price container since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            MembershipContainerList.Rebuild(siteId);
            ProductsContainerList.Rebuild(siteId);
            UserRoleContainerList.Rebuild(siteId);
            CategoryContainerList.Rebuild(siteId);
            TransactionProfileContainerList.Rebuild(siteId);
            TaxContainerList.Rebuild(siteId);
            PriceContainer result = new PriceContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }

        private void RemoveOldPriceContainerDTOColletions()
        {
            var keys = priceContainerDTOCollectionCache.Keys;
            foreach (var key in keys)
            {
                if (key.DateTimeRange.EndDateTime < ServerDateTime.Now)
                {
                    PriceContainerDTOCollection value;
                    if (priceContainerDTOCollectionCache.TryRemove(key, out value))
                    {
                        log.Debug("Removing priceContainerDTOCollectionCache of key " + key);
                    }
                    else
                    {
                        log.Debug("Unable to remove priceContainerDTOCollectionCache of key " + key);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the user Role
        /// </summary>
        protected virtual UserRoleContainerDTO GetUserRoleContainerDTO(int userRoleId)
        {
            UserRoleContainerDTO result = null;
            try
            {
                result = UserRoleContainerList.GetUserRoleContainerDTO(siteId, userRoleId);
            }
            catch (Exception ex)
            {
                log.Error("Unable to find the UserRoleContainerDTO", ex);
            }  
            return result;
        }

        /// <summary>
        /// Returns the MembershipContainerDTO
        /// </summary>
        protected virtual MembershipContainerDTO GetMembershipContainerDTO(int membershipId)
        {
            MembershipContainerDTO result = null;
            try
            {
                result = MembershipContainerList.GetMembershipContainerDTO(siteId, membershipId);
            }
            catch (Exception ex)
            {
                log.Error("Unable to find the MembershipContainerDTO", ex);
            }
            return result;
        }

        /// <summary>
        /// Returns the TransactionProfileContainerDTO
        /// </summary>
        protected virtual TransactionProfileContainerDTO GetTransactionProfileContainerDTO(int transactionProfileId)
        {
            TransactionProfileContainerDTO result = null;
            try
            {
                result = TransactionProfileContainerList.GetTransactionProfileContainerDTO(siteId, transactionProfileId);
            }
            catch (Exception ex)
            {
                log.Error("Unable to find the TransactionProfileContainerDTO", ex);
            }
            return result;
        }
    }
}
