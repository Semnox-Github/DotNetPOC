/********************************************************************************************
 * Project Name - POS
 * Description  - Container class to hold the product menu panels
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      10-Jun-2021      Lakshminarayana           Created : Static menu enhancement
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Customer.Membership.Sample;
using Semnox.Parafait.POS;
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
    public class ProductPriceContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Cache<ProductPriceContainerCacheKey, ProductPriceContainerSnapshotDTOCollection> productPriceContainerSnapshotDTOCollectionCache = new Cache<ProductPriceContainerCacheKey, ProductPriceContainerSnapshotDTOCollection>();
        private readonly int siteId;
        private readonly DateTime? maxLastUpdateTime;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ProductPriceContainer(int siteId)
            : this(siteId,
                   GetMaxLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ProductPriceContainer(int siteId,
                                    DateTime? maxLastUpdateTime)
        {
            log.LogMethodEntry(siteId, maxLastUpdateTime);
            this.siteId = siteId;
            this.maxLastUpdateTime = maxLastUpdateTime;
            log.LogMethodExit();
        }


        private static DateTime? GetMaxLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                ProductMenuListBL productPriceListBL = new ProductMenuListBL();
                result = productPriceListBL.GetProductMenuModuleLastUpdateTime(siteId);
                ProductsList productsList = new ProductsList();
                DateTime? productModuleLastUpdateTime = productsList.GetProductsLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (productModuleLastUpdateTime.HasValue && result.Value < productModuleLastUpdateTime.Value))
                {
                    result = productModuleLastUpdateTime;
                }
                POSMachineList pOSMachineList = new POSMachineList();
                DateTime? posMachineModuleLastUpdateTime = pOSMachineList.GetPOSModuleLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (posMachineModuleLastUpdateTime.HasValue && result.Value < posMachineModuleLastUpdateTime.Value))
                {
                    result = posMachineModuleLastUpdateTime;
                }
                UserRolesList userRolesList = new UserRolesList();
                DateTime? userRoleModuleLastUpdateTime = userRolesList.GetUserRoleModuleLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (userRoleModuleLastUpdateTime.HasValue && result.Value < userRoleModuleLastUpdateTime.Value))
                {
                    result = userRoleModuleLastUpdateTime;
                }

                MembershipsList membershipsList = new MembershipsList();
                DateTime? membershipModuleLastUpdateTime = membershipsList.GetMembershipLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (membershipModuleLastUpdateTime.HasValue && result.Value < membershipModuleLastUpdateTime.Value))
                {
                    result = membershipModuleLastUpdateTime;
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
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the product menu max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }


        public ProductPriceContainer Refresh()
        {
            log.LogMethodEntry();
            var keys = productPriceContainerSnapshotDTOCollectionCache.Keys;
            foreach (var key in keys)
            {
                if (key.DateTimeRange.EndDateTime < ServerDateTime.Now)
                {
                    ProductPriceContainerSnapshotDTOCollection value;
                    if (productPriceContainerSnapshotDTOCollectionCache.TryRemove(key, out value))
                    {
                        log.Debug("Removing productPriceContainerSnapshotDTOCollection of key " + key);
                    }
                    else
                    {
                        log.Debug("Unable to remove productPriceContainerSnapshotDTOCollection of key " + key);
                    }
                }
            }

            DateTime? updateTime = GetMaxLastUpdateTime(siteId);
            if (maxLastUpdateTime.HasValue
                && maxLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in product menu since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            ProductsContainerList.Rebuild(siteId);
            ProductMenuContainerList.Rebuild(siteId);
            PriceContainerList.Rebuild(siteId);
            ProductPriceContainer result = new ProductPriceContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }


        public ProductPriceContainerSnapshotDTOCollection GetProductPriceContainerSnapshotDTOCollection(int posMachineId, int userRoleId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(posMachineId, userRoleId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange);
            ProductPriceContainerCacheKey key = new ProductPriceContainerCacheKey(siteId, posMachineId, userRoleId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange);
            ProductPriceContainerSnapshotDTOCollection result = productPriceContainerSnapshotDTOCollectionCache.GetOrAdd(key, (k) => CreateProductPriceContainerSnapshotDTOCollection(key));
            log.LogMethodExit(result);
            return result;
        }

        private ProductPriceContainerSnapshotDTOCollection CreateProductPriceContainerSnapshotDTOCollection(ProductPriceContainerCacheKey key)
        {
            log.LogMethodEntry(key);
            List<ProductPriceContainerSnapshotDTO> productPriceContainerSnapshotDTOList = GetProductPriceContainerSnapshotDTOList(key);
            ProductPriceContainerSnapshotDTOCollection result = new ProductPriceContainerSnapshotDTOCollection(productPriceContainerSnapshotDTOList);
            log.LogMethodExit(result);
            return result;
        }

        private List<ProductPriceContainerSnapshotDTO> GetProductPriceContainerSnapshotDTOList(ProductPriceContainerCacheKey key)
        {
            log.LogMethodEntry(key);
            ContinuousDateTimeRanges continuousDateTimeRanges = key.DateTimeRange;
            ProductMenuContainerSnapshotDTOCollection productMenuContainerSnapshotDTOCollection = ProductMenuContainerList.GetProductMenuContainerSnapshotDTOCollection(siteId, key.PosMachineId, key.UserRoleId, key.LanguageId, key.MenuType, key.DateTimeRange.StartDateTime, key.DateTimeRange.EndDateTime);
            bool useProductCalendar = productMenuContainerSnapshotDTOCollection.ProductMenuContainerSnapshotDTOList.SelectMany(x => x.ProductMenuPanelContainerDTOList).Any() == false;

            foreach (var productMenuContainerSnapshotDTO in productMenuContainerSnapshotDTOCollection.ProductMenuContainerSnapshotDTOList)
            {
                continuousDateTimeRanges = continuousDateTimeRanges.Split(productMenuContainerSnapshotDTO.StartDateTime, TimeSpan.FromMinutes(5));
                continuousDateTimeRanges = continuousDateTimeRanges.Split(productMenuContainerSnapshotDTO.EndDateTime, TimeSpan.FromMinutes(5));
            }
            List<DateTimeRange> dateTimeRanges = continuousDateTimeRanges.DateTimeRanges.ToList();
            foreach (var dateTimeRange in dateTimeRanges)
            {
                List<int> referencedProductIdList = GetReferencedProductIdList(key, dateTimeRange);
                foreach (var referencedProductId in referencedProductIdList)
                {
                    try
                    {
                        PriceContainerDTO priceContainerDTO = PriceContainerList.GetPriceContainerDTO(key.SiteId, referencedProductId, key.MembershipId, key.UserRoleId, key.TransactionProfileId, dateTimeRange.StartDateTime);
                        foreach (var priceContainerDetailDTO in priceContainerDTO.PriceContainerDetailDTOList)
                        {
                            continuousDateTimeRanges = continuousDateTimeRanges.Split(priceContainerDetailDTO.StartDateTime);
                            continuousDateTimeRanges = continuousDateTimeRanges.Split(priceContainerDetailDTO.EndDateTime);
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    
                    if (useProductCalendar)
                    {
                        try
                        {
                            ProductCalendarContainerDTO productCalendarContainerDTO = ProductsContainerList.GetProductCalendarContainerDTO(siteId, referencedProductId, key.DateTimeRange);
                            foreach (var productCalendarDetailContainerDTO in productCalendarContainerDTO.ProductCalendarDetailContainerDTOList)
                            {
                                continuousDateTimeRanges = continuousDateTimeRanges.Split(productCalendarDetailContainerDTO.StartDateTime);
                                continuousDateTimeRanges = continuousDateTimeRanges.Split(productCalendarDetailContainerDTO.EndDateTime);
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                       
                    }
                }
            }

            List<ProductPriceContainerSnapshotDTO> result = new List<ProductPriceContainerSnapshotDTO>();
            foreach (var range in continuousDateTimeRanges.DateTimeRanges)
            {
                ProductPriceContainerSnapshotDTO productPriceContainerSnapshotDTO = GetProductPriceContainerSnapshotDTO(key, range, useProductCalendar);
                result.Add(productPriceContainerSnapshotDTO);
            }
            log.LogMethodExit(result);
            return result;
        }

        private ProductPriceContainerSnapshotDTO GetProductPriceContainerSnapshotDTO(ProductPriceContainerCacheKey key, DateTimeRange dateTimeRange, bool useProductCalendar)
        {
            log.LogMethodEntry(key, dateTimeRange);
            List<int> referencedProductIdList = GetReferencedProductIdList(key, dateTimeRange);
            List<ProductsPriceContainerDTO> productsPriceContainerDTOList = new List<ProductsPriceContainerDTO>();
            foreach (var referencedProductId in referencedProductIdList)
            {
                    if (useProductCalendar && ProductsContainerList.IsProductAvailable(siteId, referencedProductId, dateTimeRange.StartDateTime) == false)
                    {
                        continue;
                    }
                try
                {
                    ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(siteId, referencedProductId);
                    PriceContainerDetailDTO priceContainerDetailDTO = PriceContainerList.GetPriceContainerDetailDTO(key.SiteId, referencedProductId, key.MembershipId, key.UserRoleId, key.TransactionProfileId, dateTimeRange.StartDateTime);
                    TaxContainerDTO taxContainerDTO = null;
                    decimal finalPriceWithTax = priceContainerDetailDTO.FinalPrice.Value;
                    if (productsContainerDTO.TaxId > -1)
                    {
                        taxContainerDTO = TaxContainerList.GetTaxContainerDTO(siteId, productsContainerDTO.TaxId);
                        finalPriceWithTax = finalPriceWithTax * (1 + (decimal)taxContainerDTO.TaxPercentage / 100);
                    }
                    finalPriceWithTax = Math.Round(finalPriceWithTax, 4);
                    ProductsPriceContainerDTO productsPriceContainerDTO = new ProductsPriceContainerDTO(productsContainerDTO, priceContainerDetailDTO, finalPriceWithTax);
                    productsPriceContainerDTO.WebDescription = ObjectTranslationContainerList.GetObjectTranslation(siteId, key.LanguageId, "PRODUCTS", "WEBDESCRIPTION", productsContainerDTO.Guid, productsPriceContainerDTO.WebDescription);
                    productsPriceContainerDTO.TranslatedProductDescription = ObjectTranslationContainerList.GetObjectTranslation(siteId, key.LanguageId, "PRODUCTS", "DESCRIPTION", productsContainerDTO.Guid, productsPriceContainerDTO.TranslatedProductDescription);
                    productsPriceContainerDTO.TranslatedProductName = ObjectTranslationContainerList.GetObjectTranslation(siteId, key.LanguageId, "PRODUCTS", "PRODUCT_NAME", productsContainerDTO.Guid, productsPriceContainerDTO.TranslatedProductName);
                    if(productsPriceContainerDTO.ComboProductContainerDTOList != null && productsPriceContainerDTO.ComboProductContainerDTOList.Any())
                    {
                        foreach(ComboProductContainerDTO comboProductContainerDTO in productsPriceContainerDTO.ComboProductContainerDTOList)
                        {
                            ProductsContainerDTO childProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(siteId, comboProductContainerDTO.ChildProductId);
                            PriceContainerDetailDTO childProductPriceContainerDetailDTO = PriceContainerList.GetPriceContainerDetailDTO(key.SiteId, comboProductContainerDTO.ChildProductId, key.MembershipId, key.UserRoleId, key.TransactionProfileId, dateTimeRange.StartDateTime);
                            if (priceContainerDetailDTO.FinalPrice == null || priceContainerDetailDTO.FinalPrice < 0)
                            {
                                if (comboProductContainerDTO.Price == null || comboProductContainerDTO.Price <= 0)
                                {
                                    comboProductContainerDTO.FinalPrice = childProductPriceContainerDetailDTO.FinalPrice.Value;
                                    if (childProductsContainerDTO.TaxId > -1)
                                    {
                                        taxContainerDTO = TaxContainerList.GetTaxContainerDTO(siteId, childProductsContainerDTO.TaxId);
                                        comboProductContainerDTO.FinalPriceWithTax = childProductPriceContainerDetailDTO.FinalPrice.Value * (1 + (decimal)taxContainerDTO.TaxPercentage / 100);
                                    }
                                    else
                                    {
                                        comboProductContainerDTO.FinalPriceWithTax = childProductPriceContainerDetailDTO.FinalPrice.Value;
                                    }
                                }
                                else
                                {
                                    if (childProductsContainerDTO.TaxInclusivePrice.Equals("Y", StringComparison.InvariantCultureIgnoreCase) && childProductsContainerDTO.TaxId > -1)
                                    {
                                        taxContainerDTO = TaxContainerList.GetTaxContainerDTO(siteId, childProductsContainerDTO.TaxId);
                                        comboProductContainerDTO.FinalPrice = (decimal)(comboProductContainerDTO.Price.Value / (1 + taxContainerDTO.TaxPercentage / 100));
                                        comboProductContainerDTO.FinalPriceWithTax = (decimal)comboProductContainerDTO.Price;
                                    }
                                    else
                                    {
                                        comboProductContainerDTO.FinalPrice = (decimal)comboProductContainerDTO.Price.Value;
                                        if (childProductsContainerDTO.TaxId > -1)
                                        {
                                            taxContainerDTO = TaxContainerList.GetTaxContainerDTO(siteId, childProductsContainerDTO.TaxId);
                                            comboProductContainerDTO.FinalPriceWithTax = (decimal)(comboProductContainerDTO.Price.Value * (1 + taxContainerDTO.TaxPercentage / 100));
                                        }
                                        else
                                        {
                                            comboProductContainerDTO.FinalPriceWithTax = (decimal)comboProductContainerDTO.Price.Value;
                                        }
                                    }
                                }
                            }
                            comboProductContainerDTO.FinalPriceWithTax = Math.Round(comboProductContainerDTO.FinalPriceWithTax, 4);
                            comboProductContainerDTO.FinalPrice = Math.Round(comboProductContainerDTO.FinalPrice, 4);

                            if(comboProductContainerDTO.DisplayGroupId > -1)
                            {
                                ProductDisplayGroupFormatContainerDTO productDisplayGroupFormatContainerDTO = ProductDisplayGroupFormatContainerList.GetProductDisplayGroupFormatContainerDTO(siteId, comboProductContainerDTO.DisplayGroupId);
                                comboProductContainerDTO.DisplayGroup = productDisplayGroupFormatContainerDTO.DisplayGroup;
                            }
                            if(comboProductContainerDTO.CategoryId > -1)
                            {
                                CategoryContainerDTO categoryContainerDTO = CategoryContainerList.GetCategoryContainerDTO(siteId, comboProductContainerDTO.CategoryId);
                                comboProductContainerDTO.CategoryName = categoryContainerDTO.Name;
                            }
                        }
                    }
                    productsPriceContainerDTOList.Add(productsPriceContainerDTO);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while creating the product price container DTO", ex);
                }              
                
            }
            ProductPriceContainerSnapshotDTO productPriceContainerSnapshotDTO = new ProductPriceContainerSnapshotDTO(dateTimeRange.StartDateTime, dateTimeRange.EndDateTime);
            productPriceContainerSnapshotDTO.ProductsPriceContainerDTOList = productsPriceContainerDTOList;
            log.LogMethodExit(productPriceContainerSnapshotDTO);
            return productPriceContainerSnapshotDTO;
        }

        private List<int> GetReferencedProductIdList(ProductPriceContainerCacheKey key, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(key, dateTimeRange);
            List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList = GetProductMenuPanelContainerDTOList(key, dateTimeRange.StartDateTime);
            List<int> productIdList = new List<int>();
            productIdList.AddRange(productMenuPanelContainerDTOList
                                   .SelectMany(x => x.ProductMenuPanelContentContainerDTOList)
                                   .Where(x => x.ProductId > -1)
                                   .Select(x => x.ProductId));
            if (productIdList.Count == 0)
            {
                POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(key.SiteId, key.PosMachineId);
                if (pOSMachineContainerDTO != null)
                {
                    productIdList = pOSMachineContainerDTO.IncludedProductIdList;
                }
                UserRoleContainerDTO userRoleContainerDTO = UserRoleContainerList.GetUserRoleContainerDTOOrDefault(siteId, key.UserRoleId);
                if(userRoleContainerDTO != null)
                {
                    productIdList = productIdList.Except(userRoleContainerDTO.ExcludedProductIdList).ToList();
                }
            }
            List<int> result = GetReferencedProductIdList(productIdList);
            log.LogMethodExit(result);
            return result;
        }

        private List<int> GetReferencedProductIdList(List<int> productIdList)
        {
            log.LogMethodEntry(productIdList);
            var result = ProductsContainerList.GetReferencedProductIdList(siteId, productIdList);
            log.LogMethodExit(result);
            return result;
        }

        private List<ProductMenuPanelContainerDTO> GetProductMenuPanelContainerDTOList(ProductPriceContainerCacheKey key, DateTime dateTime)
        {
            log.LogMethodEntry(key, dateTime);
            var result = ProductMenuContainerList.GetProductMenuPanelContainerDTOList(siteId, key.UserRoleId, key.PosMachineId, key.LanguageId, key.MenuType, key.DateTimeRange, dateTime);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns ProductsPriceContainerDTO List
        /// </summary>
        public List<ProductsPriceContainerDTO> GetProductsPriceContainerDTOList(int posMachineId, int userRoleId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTimeRange dateTimeRange, DateTime dateTime)
        {
            log.LogMethodEntry(posMachineId, userRoleId, languageId, menuType, dateTimeRange, dateTime);
            ProductPriceContainerSnapshotDTOCollection productPriceContainerSnapshotDTOCollection = GetProductPriceContainerSnapshotDTOCollection(posMachineId, userRoleId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange);
            List<ProductsPriceContainerDTO> result = new List<ProductsPriceContainerDTO>();
            if (productPriceContainerSnapshotDTOCollection == null ||
                productPriceContainerSnapshotDTOCollection.ProductPriceContainerSnapshotDTOList == null ||
                productPriceContainerSnapshotDTOCollection.ProductPriceContainerSnapshotDTOList.Any() == false)
            {
                log.LogMethodExit(result, "productPriceContainerSnapshotDTOCollection is empty");
                return result;
            }
            foreach (var productPriceContainerSnapshotDTO in productPriceContainerSnapshotDTOCollection.ProductPriceContainerSnapshotDTOList)
            {
                if (dateTime >= productPriceContainerSnapshotDTO.StartDateTime && dateTime < productPriceContainerSnapshotDTO.EndDateTime)
                {
                    result = productPriceContainerSnapshotDTO.ProductsPriceContainerDTOList;
                    break;
                }
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
