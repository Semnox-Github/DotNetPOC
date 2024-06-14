/********************************************************************************************
* Project Name - Product 
* Description  - Container Class for Product to get all the Products DTO list 
* 
**************
**Version Log
**************
*Version      Date            Modified By         Remarks          
*********************************************************************************************
*2.110.0     02-Dec-2020      Deeksha              Created 
*2.130.0     02-Aug-2020      Girish Kundar        Modified:  Added CustomDataContainerDTOList to productContainerDTO
*2.140.0     23-June-2021     Prashanth V          Modified : CreateProductsContainerDTO method
*2.140.0     14-Sep-2021      Prajwal S            Modified: Child containers added under product container
*2.150.0     28-Mar-2022      Girish Kundar        Modified : Added a new column  MaximumQuantity & PauseType to Products
********************************************************************************************/
using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Product
{
    public class ProductsContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<ProductsDTO> productsDTOList;
        private readonly List<SalesOfferGroupDTO> salesOfferGroupDTOList;
        private readonly ProductsContainerDTOCollection redeemableProductsContainerDTOCollection;
        private readonly ProductsContainerDTOCollection sellableProductsContainerDTOCollection;
        private readonly ProductsContainerDTOCollection inventoryProductsContainerDTOCollection;
        private readonly List<ProductsContainerDTO> redeemableProductsContainerDTOList = new List<ProductsContainerDTO>();
        private readonly List<ProductsContainerDTO> inactiveRedeemableProductsContainerDTOList = new List<ProductsContainerDTO>();
        private readonly List<ProductsContainerDTO> sellableProductsContainerDTOList = new List<ProductsContainerDTO>();
        private readonly List<ProductsContainerDTO> inactiveSellableProductsContainerDTOList = new List<ProductsContainerDTO>();
        private readonly List<ProductsContainerDTO> inventoryProductsContainerDTOList = new List<ProductsContainerDTO>();
        private readonly List<ProductsContainerDTO> inactiveInventoryProductsContainerDTOList = new List<ProductsContainerDTO>();
        private readonly List<ProductsContainerDTO> systemProductsContainerDTOList = new List<ProductsContainerDTO>();
        private readonly DateTime? productsLastUpdateTime;
        private readonly ConcurrentDictionary<int, ProductsDTO> productsDTODictionary = new ConcurrentDictionary<int, ProductsDTO>();
        private readonly ConcurrentDictionary<int, ProductsContainerDTO> productIdProductsContainerDTODictionary = new ConcurrentDictionary<int, ProductsContainerDTO>();
        private readonly ConcurrentDictionary<int, SalesOfferGroupContainerDTO> saleGroupIdSaleGroupContainerDTODictionary = new ConcurrentDictionary<int, SalesOfferGroupContainerDTO>();
        private readonly ConcurrentDictionary<int, ProductModifierContainerDTO> productIdProductModifierContainerDTODictionary = new ConcurrentDictionary<int, ProductModifierContainerDTO>();
        private readonly ConcurrentDictionary<int, ComboProductContainerDTO> comboProductContainerDTODictionary = new ConcurrentDictionary<int, ComboProductContainerDTO>();
        private readonly ConcurrentDictionary<string, ProductsContainerDTO> productGuidProductsContainerDTODictionary = new ConcurrentDictionary<string, ProductsContainerDTO>();
        private readonly List<ProductsContainerDTO> activeProductsContainerDTOList = new List<ProductsContainerDTO>();
        private readonly List<ProductsContainerDTO> inactiveProductsContainerDTOList = new List<ProductsContainerDTO>();
        private readonly Dictionary<string, List<ProductsContainerDTO>> systemProductsDTOListDictionary = new Dictionary<string, List<ProductsContainerDTO>>();
        private readonly Dictionary<int, List<ProductsContainerDTO>> categoryIdProductsContainerDTOListDictionary = new Dictionary<int, List<ProductsContainerDTO>>();
        private readonly Dictionary<int, ProductsContainerDTO> systemProductsContainerDTODictionary = new Dictionary<int, ProductsContainerDTO>();
        private readonly Dictionary<int, List<int>> displayGroupIdProductIdListDictionary = new Dictionary<int, List<int>>();
        private Cache<DateTimeRange, ProductCalendarContainer> productAvailabilityContainerCache = new Cache<DateTimeRange, ProductCalendarContainer>();
        private readonly int siteId;
        private Cache<DateTimeRange, ProductCalendarContainer> productCalendarContainerCache = new Cache<DateTimeRange, ProductCalendarContainer>();
        private Cache<DateTimeRange, Dictionary<string, ProductCalendarContainerDTOCollection>> productCalendarContainerDTOCollectionCache = new Cache<DateTimeRange, Dictionary<string, ProductCalendarContainerDTOCollection>>();
        private Dictionary<int, HashSet<int>> productIdReferencedProductIdDictionary = new Dictionary<int, HashSet<int>>();
        private Dictionary<int, ProductDisplayGroupFormatDTO> displayGroupIdProductDisplayGroupFormatDTODictionary = new Dictionary<int, ProductDisplayGroupFormatDTO>();
        private const int AGE_UPPER_LIMIT = 999;
        private readonly Dictionary<string, List<EntityOverrideDateContainerDTO>> productCreditPlusGuidEntityOverrideDateContainerDTOListDictionary = new Dictionary<string, List<EntityOverrideDateContainerDTO>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<string, List<EntityOverrideDateContainerDTO>> productGamesGuidEntityOverrideDateContainerDTOListDictionary = new Dictionary<string, List<EntityOverrideDateContainerDTO>>(StringComparer.InvariantCultureIgnoreCase);
        public ProductsContainer(int siteId) :
             this(siteId, GetProductsDTOList(siteId), GetSystemProductDTOList(siteId), GetProductsDisplayGroupFormatDTOList(siteId), GetProductsLastUpdateTime(siteId), GetSalesOfferGroupDTOList(siteId), GetProductCreditPlusEntityOverrideDateList(siteId), GetProductGamesEntityOverrideDateList(siteId))
        {

        }
        private static List<EntityOverrideDatesDTO> GetProductCreditPlusEntityOverrideDateList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<EntityOverrideDatesDTO> result = null;
            try
            {
                EntityOverrideList entityOverrideList = new EntityOverrideList();
                List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>> searchParameter = new List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>>();
                searchParameter.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_NAME, "PRODUCTCREDITPLUS"));
                searchParameter.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.SITE_ID, siteId.ToString()));
                searchParameter.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.IS_ACTIVE, "1"));
                result = entityOverrideList.GetAllEntityOverrideList(searchParameter);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading the product creditplus entity override dateList", ex);
            }

            if (result == null)
            {
                result = new List<EntityOverrideDatesDTO>();
            }
            log.LogMethodExit(result);
            return result;
        }
        private static List<EntityOverrideDatesDTO> GetProductGamesEntityOverrideDateList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<EntityOverrideDatesDTO> result = null;
            try
            {
                EntityOverrideList entityOverrideList = new EntityOverrideList();
                List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>> searchParameter = new List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>>();
                searchParameter.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_NAME, "PRODUCTGAMES"));
                searchParameter.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.SITE_ID, siteId.ToString()));
                searchParameter.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.IS_ACTIVE, "1"));
                result = entityOverrideList.GetAllEntityOverrideList(searchParameter);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading the product creditplus entity override dateList", ex);
            }

            if (result == null)
            {
                result = new List<EntityOverrideDatesDTO>();
            }
            log.LogMethodExit(result);
            return result;
        }
        private static List<ProductDisplayGroupFormatDTO> GetProductsDisplayGroupFormatDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<ProductDisplayGroupFormatDTO> productsDisplayGroupDTOList = null;
            try
            {
                ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList();
                List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchDisplayParameters = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
                searchDisplayParameters.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, Convert.ToString(siteId)));
                productsDisplayGroupDTOList = productDisplayGroupList.GetAllProductDisplayGroup(searchDisplayParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading the products", ex);
            }
            if (productsDisplayGroupDTOList == null)
            {
                productsDisplayGroupDTOList = new List<ProductDisplayGroupFormatDTO>();
            }
            log.LogMethodExit(productsDisplayGroupDTOList);
            return productsDisplayGroupDTOList;
        }

        public ProductsContainer(int siteId, List<ProductsDTO> productsDTOList, List<ProductsDTO> systemProducts, List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList, DateTime? productsLastUpdateTime, List<SalesOfferGroupDTO> salesOfferGroupDTOList, List<EntityOverrideDatesDTO> productCreditPlusEntityOverrideDateDTOList, List<EntityOverrideDatesDTO> productGamesEntityOverrideDateDTOList)
        {
            log.LogMethodEntry(siteId);
            this.productsDTOList = productsDTOList;
            this.productsLastUpdateTime = productsLastUpdateTime;
            this.siteId = siteId;
            this.salesOfferGroupDTOList = salesOfferGroupDTOList;
            HashSet<int> activeRedeemableProductIdHashSet = new HashSet<int>();
            HashSet<int> inactiveRedeemableProductIdHashSet = new HashSet<int>();
            HashSet<int> activeSellableProductIdHashSet = new HashSet<int>();
            HashSet<int> inactiveSellableProductIdHashSet = new HashSet<int>();
            HashSet<int> activeInventoryProductIdHashSet = new HashSet<int>();
            HashSet<int> inactiveInventoryProductIdHashSet = new HashSet<int>();
            foreach (var productCreditPlusEntityOverrideDateDTO in productCreditPlusEntityOverrideDateDTOList)
            {
                if (productCreditPlusGuidEntityOverrideDateContainerDTOListDictionary.ContainsKey(productCreditPlusEntityOverrideDateDTO.EntityGuid) == false)
                {
                    productCreditPlusGuidEntityOverrideDateContainerDTOListDictionary.Add(productCreditPlusEntityOverrideDateDTO.EntityGuid, new List<EntityOverrideDateContainerDTO>());
                }
                EntityOverrideDateContainerDTO entityOverrideDateContainerDTO = new EntityOverrideDateContainerDTO(productCreditPlusEntityOverrideDateDTO.ID,
                                                                                                                   productCreditPlusEntityOverrideDateDTO.EntityName,
                                                                                                                   productCreditPlusEntityOverrideDateDTO.EntityGuid,
                                                                                                                   productCreditPlusEntityOverrideDateDTO.OverrideDate,
                                                                                                                   productCreditPlusEntityOverrideDateDTO.IncludeExcludeFlag,
                                                                                                                   productCreditPlusEntityOverrideDateDTO.Day,
                                                                                                                   productCreditPlusEntityOverrideDateDTO.Remarks);
                productCreditPlusGuidEntityOverrideDateContainerDTOListDictionary[productCreditPlusEntityOverrideDateDTO.EntityGuid].Add(entityOverrideDateContainerDTO);
            }
            foreach (var productGamesEntityOverrideDateDTO in productGamesEntityOverrideDateDTOList)
            {
                if (productGamesGuidEntityOverrideDateContainerDTOListDictionary.ContainsKey(productGamesEntityOverrideDateDTO.EntityGuid) == false)
                {
                    productGamesGuidEntityOverrideDateContainerDTOListDictionary.Add(productGamesEntityOverrideDateDTO.EntityGuid, new List<EntityOverrideDateContainerDTO>());
                }
                EntityOverrideDateContainerDTO entityOverrideDateContainerDTO = new EntityOverrideDateContainerDTO(productGamesEntityOverrideDateDTO.ID,
                                                                                                                   productGamesEntityOverrideDateDTO.EntityName,
                                                                                                                   productGamesEntityOverrideDateDTO.EntityGuid,
                                                                                                                   productGamesEntityOverrideDateDTO.OverrideDate,
                                                                                                                   productGamesEntityOverrideDateDTO.IncludeExcludeFlag,
                                                                                                                   productGamesEntityOverrideDateDTO.Day,
                                                                                                                   productGamesEntityOverrideDateDTO.Remarks);
                productGamesGuidEntityOverrideDateContainerDTOListDictionary[productGamesEntityOverrideDateDTO.EntityGuid].Add(entityOverrideDateContainerDTO);
            }
            if (productDisplayGroupFormatDTOList != null && productDisplayGroupFormatDTOList.Any())
            {
                foreach (var productDisplayGroupFormatDTO in productDisplayGroupFormatDTOList)
                {
                    if (displayGroupIdProductDisplayGroupFormatDTODictionary.ContainsKey(productDisplayGroupFormatDTO.Id))
                    {
                        continue;
                    }
                    displayGroupIdProductDisplayGroupFormatDTODictionary.Add(productDisplayGroupFormatDTO.Id, productDisplayGroupFormatDTO);
                }
            }
            if (salesOfferGroupDTOList.Any())
            {
                foreach (SalesOfferGroupDTO salesOfferGroupDTO in salesOfferGroupDTOList)
                {
                    if (saleGroupIdSaleGroupContainerDTODictionary.ContainsKey(salesOfferGroupDTO.SaleGroupId) == false)
                    {
                        SalesOfferGroupContainerDTO salesOfferGroupContainerDTO = new SalesOfferGroupContainerDTO(salesOfferGroupDTO.SaleGroupId, salesOfferGroupDTO.Name, salesOfferGroupDTO.IsUpsell, salesOfferGroupDTO.Guid);
                        if (salesOfferGroupDTO.SaleGroupProductMapDTOList != null && salesOfferGroupDTO.SaleGroupProductMapDTOList.Any())
                        {
                            salesOfferGroupContainerDTO.SaleGroupProductMapDTOContainerList = new List<SaleGroupProductMapContainerDTO>();
                            foreach (SaleGroupProductMapDTO saleGroupProductMapDTO in salesOfferGroupDTO.SaleGroupProductMapDTOList)
                            {
                                salesOfferGroupContainerDTO.SaleGroupProductMapDTOContainerList.Add(new SaleGroupProductMapContainerDTO(saleGroupProductMapDTO.TypeMapId, saleGroupProductMapDTO.SaleGroupId, saleGroupProductMapDTO.ProductId, saleGroupProductMapDTO.SequenceId, saleGroupProductMapDTO.Guid));
                            }
                        }
                        saleGroupIdSaleGroupContainerDTODictionary[salesOfferGroupContainerDTO.SaleGroupId] = salesOfferGroupContainerDTO;
                    }
                }
            }
            foreach (ProductsDTO productsDTO in productsDTOList)
            {
                productsDTODictionary[productsDTO.ProductId] = productsDTO;
                ProductsContainerDTO productsContainerDTO = CreateProductsContainerDTO(productsDTO, false);
                productIdProductsContainerDTODictionary[productsContainerDTO.ProductId] = productsContainerDTO;
                productGuidProductsContainerDTODictionary[productsContainerDTO.Guid] = productsContainerDTO;
                if (productsContainerDTO.IsActive)
                {
                    activeProductsContainerDTOList.Add(productsContainerDTO);
                    if(categoryIdProductsContainerDTOListDictionary.ContainsKey(productsContainerDTO.CategoryId) == false)
                    {
                        categoryIdProductsContainerDTOListDictionary.Add(productsContainerDTO.CategoryId, new List<ProductsContainerDTO>());
                    }
                    categoryIdProductsContainerDTOListDictionary[productsContainerDTO.CategoryId].Add(productsContainerDTO);
                }
                else
                {
                    inactiveProductsContainerDTOList.Add(productsContainerDTO);
                }
                if (productsContainerDTO.InventoryItemContainerDTO != null &&
                    productsContainerDTO.InventoryItemContainerDTO.IsRedeemable == "Y")
                {
                    if (productsContainerDTO.IsActive)
                    {
                        redeemableProductsContainerDTOList.Add(productsContainerDTO);
                        activeRedeemableProductIdHashSet.Add(productsContainerDTO.ProductId);
                    }
                    else
                    {
                        inactiveRedeemableProductsContainerDTOList.Add(productsContainerDTO);
                        inactiveRedeemableProductIdHashSet.Add(productsContainerDTO.ProductId);
                    }
                }
                if (productsContainerDTO.InventoryItemContainerDTO == null ||
                    productsContainerDTO.InventoryItemContainerDTO.IsSellable == "Y")
                {
                    if (productsContainerDTO.IsActive)
                    {
                        sellableProductsContainerDTOList.Add(productsContainerDTO);
                        activeSellableProductIdHashSet.Add(productsContainerDTO.ProductId);
                    }
                    else
                    {
                        inactiveSellableProductsContainerDTOList.Add(productsContainerDTO);
                        inactiveSellableProductIdHashSet.Add(productsContainerDTO.ProductId);
                    }
                }
                if (productsContainerDTO.InventoryItemContainerDTO != null)
                {
                    if (productsContainerDTO.IsActive)
                    {
                        inventoryProductsContainerDTOList.Add(productsContainerDTO);
                        activeInventoryProductIdHashSet.Add(productsContainerDTO.ProductId);
                    }
                    else
                    {
                        inactiveInventoryProductsContainerDTOList.Add(productsContainerDTO);
                        inactiveInventoryProductIdHashSet.Add(productsContainerDTO.ProductId);
                    }
                }
                if (productsDTO.ActiveFlag)
                {
                    if (productsDTO.ProductsDisplayGroupDTOList != null && productsDTO.ProductsDisplayGroupDTOList.Any())
                    {
                        foreach (var productsDisplayGroupDTO in productsDTO.ProductsDisplayGroupDTOList)
                        {
                            if (displayGroupIdProductIdListDictionary.ContainsKey(productsDisplayGroupDTO.DisplayGroupId) == false)
                            {
                                displayGroupIdProductIdListDictionary.Add(productsDisplayGroupDTO.DisplayGroupId, new List<int>());
                            }
                            displayGroupIdProductIdListDictionary[productsDisplayGroupDTO.DisplayGroupId].Add(productsDTO.ProductId);
                        }
                    }
                    else
                    {
                        if (displayGroupIdProductIdListDictionary.ContainsKey(-1) == false)
                        {
                            displayGroupIdProductIdListDictionary.Add(-1, new List<int>());
                        }
                        displayGroupIdProductIdListDictionary[-1].Add(productsDTO.ProductId);
                    }
                }

            }

            if (systemProducts != null && systemProducts.Any())
            {
                productsDTOList.AddRange(systemProducts);
                foreach (ProductsDTO productsDTO in systemProducts)
                {
                    ProductsContainerDTO productsContainerDTO = CreateProductsContainerDTO(productsDTO, true);
                    if (systemProductsDTOListDictionary.ContainsKey(productsDTO.ProductType) == false)
                    {
                        systemProductsDTOListDictionary.Add(productsDTO.ProductType, new List<ProductsContainerDTO>());
                    }
                    systemProductsDTOListDictionary[productsDTO.ProductType].Add(productsContainerDTO);
                    systemProductsContainerDTOList.Add(productsContainerDTO);
                    if (systemProductsContainerDTODictionary.ContainsKey(productsDTO.ProductId) == false)
                    {
                        systemProductsContainerDTODictionary.Add(productsDTO.ProductId, new ProductsContainerDTO());
                    }
                    systemProductsContainerDTODictionary[productsDTO.ProductId]= productsContainerDTO;
                }
            }
            BuildProductIdReferencedProductIdDictionary();
            HashSet<int> activeRedeemableReferencedProductIdHashSet = new HashSet<int>();
            HashSet<int> activeSellableReferencedProductIdHashSet = new HashSet<int>();
            HashSet<int> activeInventoryReferencedProductIdHashSet = new HashSet<int>();
            foreach (var productId in activeRedeemableProductIdHashSet)
            {
                if (productIdReferencedProductIdDictionary.ContainsKey(productId) == false)
                {
                    continue;
                }
                activeRedeemableReferencedProductIdHashSet.UnionWith(productIdReferencedProductIdDictionary[productId]);
            }
            foreach (var productId in activeSellableProductIdHashSet)
            {
                if (productIdReferencedProductIdDictionary.ContainsKey(productId) == false)
                {
                    continue;
                }
                activeSellableReferencedProductIdHashSet.UnionWith(productIdReferencedProductIdDictionary[productId]);
            }
            foreach (var productId in activeInventoryProductIdHashSet)
            {
                if (productIdReferencedProductIdDictionary.ContainsKey(productId) == false)
                {
                    continue;
                }
                activeInventoryReferencedProductIdHashSet.UnionWith(productIdReferencedProductIdDictionary[productId]);
            }
            // referenced Products
            if (activeRedeemableReferencedProductIdHashSet.Any())
            {
                activeRedeemableReferencedProductIdHashSet.ExceptWith(activeRedeemableProductIdHashSet);
                foreach (var item in activeRedeemableReferencedProductIdHashSet)
                {
                    redeemableProductsContainerDTOList.Add(productIdProductsContainerDTODictionary[item]);
                }
            }
            if (activeSellableReferencedProductIdHashSet.Any())
            {
                activeSellableReferencedProductIdHashSet.ExceptWith(activeSellableProductIdHashSet);
                foreach (var item in activeSellableReferencedProductIdHashSet)
                {
                    sellableProductsContainerDTOList.Add(productIdProductsContainerDTODictionary[item]);
                }
            }
            if (activeInventoryReferencedProductIdHashSet.Any())
            {
                activeInventoryReferencedProductIdHashSet.ExceptWith(activeInventoryProductIdHashSet);
                foreach (var item in activeInventoryReferencedProductIdHashSet)
                {
                    inventoryProductsContainerDTOList.Add(productIdProductsContainerDTODictionary[item]);
                }
            }
            redeemableProductsContainerDTOCollection = new ProductsContainerDTOCollection(redeemableProductsContainerDTOList.Concat(inactiveRedeemableProductsContainerDTOList).Concat(systemProductsContainerDTOList).ToList());
            sellableProductsContainerDTOCollection = new ProductsContainerDTOCollection(sellableProductsContainerDTOList.Concat(inactiveSellableProductsContainerDTOList).Concat(systemProductsContainerDTOList).ToList());
            inventoryProductsContainerDTOCollection = new ProductsContainerDTOCollection(inventoryProductsContainerDTOList.Concat(inactiveInventoryProductsContainerDTOList).Concat(systemProductsContainerDTOList).ToList());
            log.LogMethodExit();
        }

        internal List<ProductsContainerDTO> GetProductsContainerDTOListOfCategory(int categoryId)
        {
            log.LogMethodEntry(categoryId);
            List<ProductsContainerDTO> result;
            if (categoryIdProductsContainerDTOListDictionary.ContainsKey(categoryId) == false)
            {
                result = new List<ProductsContainerDTO>();
                log.LogMethodExit(result, "categoryIdProductsContainerDTOListDictionary doesn't contain categoryId : " + categoryId);
                return result;
            }
            result = categoryIdProductsContainerDTOListDictionary[categoryId];
            log.LogMethodExit(result);
            return result;
        }

        private void BuildProductIdReferencedProductIdDictionary()
        {
            log.LogMethodEntry();
            foreach (var productId in productIdProductsContainerDTODictionary.Keys)
            {
                if (productIdReferencedProductIdDictionary.ContainsKey(productId))
                {
                    continue;
                }
                HashSet<int> referencedProductIdHashSet = new HashSet<int>();
                CollectReferencedProductIds(productId, referencedProductIdHashSet);
                productIdReferencedProductIdDictionary.Add(productId, referencedProductIdHashSet);
            }
            log.LogMethodExit();
        }

        private void CollectReferencedProductIds(int productId, HashSet<int> referencedProductIdHashSet)
        {
            log.LogMethodEntry(productId, referencedProductIdHashSet);
            if (productIdProductsContainerDTODictionary.ContainsKey(productId) == false)
            {
                return;
            }
            if (referencedProductIdHashSet.Contains(productId))
            {
                return;
            }
            referencedProductIdHashSet.Add(productId);
            ProductsContainerDTO productsContainerDTO = productIdProductsContainerDTODictionary[productId];
            if (productsContainerDTO.ProductModifierContainerDTOList != null && productsContainerDTO.ProductModifierContainerDTOList.Count > 0)
            {
                for (int i = 0; i < productsContainerDTO.ProductModifierContainerDTOList.Count; i++)
                {
                    if (productsContainerDTO.ProductModifierContainerDTOList[i].ModifierSetContainerDTO != null &&
                     productsContainerDTO.ProductModifierContainerDTOList[i].ModifierSetContainerDTO.ModifierSetDetailsContainerDTOList != null &&
                     productsContainerDTO.ProductModifierContainerDTOList[i].ModifierSetContainerDTO.ModifierSetDetailsContainerDTOList.Count > 0)
                    {
                        foreach (ModifierSetDetailsContainerDTO modifierSetDetailsContainerDTO in productsContainerDTO.ProductModifierContainerDTOList[i].ModifierSetContainerDTO.ModifierSetDetailsContainerDTOList)
                        {
                            CollectReferencedProductIds(modifierSetDetailsContainerDTO.ModifierProductId, referencedProductIdHashSet);
                        }
                    }
                }
            }
            if (productsContainerDTO.ComboProductContainerDTOList != null && productsContainerDTO.ComboProductContainerDTOList.Count > 0)
            {
                for (int i = 0; i < productsContainerDTO.ComboProductContainerDTOList.Count; i++)
                {
                    if (productsContainerDTO.ComboProductContainerDTOList[i].ChildProductId > 0)
                    {
                        CollectReferencedProductIds(productsContainerDTO.ComboProductContainerDTOList[i].ChildProductId, referencedProductIdHashSet);
                    }
                }
            }
            if (productsContainerDTO.CrossSellProductsContainerDTOList != null && productsContainerDTO.CrossSellProductsContainerDTOList.Count > 0)
            {
                for (int i = 0; i < productsContainerDTO.CrossSellProductsContainerDTOList.Count; i++)
                {
                    if (productsContainerDTO.CrossSellProductsContainerDTOList[i].OfferProductId > 0)
                    {
                        CollectReferencedProductIds(productsContainerDTO.CrossSellProductsContainerDTOList[i].OfferProductId, referencedProductIdHashSet);
                        if (productsContainerDTO.CrossSellProductsContainerDTOList[i].SalesOfferGroupContainerDTO != null && productsContainerDTO.CrossSellProductsContainerDTOList[i].SalesOfferGroupContainerDTO.SaleGroupProductMapDTOContainerList != null
                          && productsContainerDTO.CrossSellProductsContainerDTOList[i].SalesOfferGroupContainerDTO.SaleGroupProductMapDTOContainerList.Any())
                        {
                            for (int j = 0; j < productsContainerDTO.CrossSellProductsContainerDTOList[i].SalesOfferGroupContainerDTO.SaleGroupProductMapDTOContainerList.Count; j++)
                            {
                                if (productsContainerDTO.CrossSellProductsContainerDTOList[i].SalesOfferGroupContainerDTO.SaleGroupProductMapDTOContainerList[j].ProductId > -1)
                                {
                                    CollectReferencedProductIds(productsContainerDTO.CrossSellProductsContainerDTOList[i].SalesOfferGroupContainerDTO.SaleGroupProductMapDTOContainerList[j].ProductId, referencedProductIdHashSet);
                                }
                            }
                        }
                    }
                }
            }
            if (productsContainerDTO.UpsellOffersContainerDTOList != null && productsContainerDTO.UpsellOffersContainerDTOList.Count > 0)
            {
                for (int i = 0; i < productsContainerDTO.UpsellOffersContainerDTOList.Count; i++)
                {
                    if (productsContainerDTO.UpsellOffersContainerDTOList[i].OfferProductId > 0)
                    {
                        CollectReferencedProductIds(productsContainerDTO.UpsellOffersContainerDTOList[i].OfferProductId, referencedProductIdHashSet);
                        if (productsContainerDTO.UpsellOffersContainerDTOList[i].SalesOfferGroupContainerDTO != null && productsContainerDTO.UpsellOffersContainerDTOList[i].SalesOfferGroupContainerDTO.SaleGroupProductMapDTOContainerList != null
                     && productsContainerDTO.UpsellOffersContainerDTOList[i].SalesOfferGroupContainerDTO.SaleGroupProductMapDTOContainerList.Any())
                        {
                            for (int j = 0; j < productsContainerDTO.UpsellOffersContainerDTOList[i].SalesOfferGroupContainerDTO.SaleGroupProductMapDTOContainerList.Count; j++)
                            {
                                if (productsContainerDTO.UpsellOffersContainerDTOList[i].SalesOfferGroupContainerDTO.SaleGroupProductMapDTOContainerList[j].ProductId > -1)
                                {
                                    CollectReferencedProductIds(productsContainerDTO.UpsellOffersContainerDTOList[i].SalesOfferGroupContainerDTO.SaleGroupProductMapDTOContainerList[j].ProductId, referencedProductIdHashSet);
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        internal List<int> GetReferencedProductIdList(List<int> productIdList)
        {
            log.LogMethodEntry(productIdList);
            HashSet<int> referencedProductIdHashSet = new HashSet<int>();
            foreach (var productId in productIdList)
            {
                if (productIdReferencedProductIdDictionary.ContainsKey(productId) == false)
                {
                    continue;
                }
                referencedProductIdHashSet.UnionWith(productIdReferencedProductIdDictionary[productId]);
            }
            List<int> result = referencedProductIdHashSet.ToList();
            log.LogMethodExit(result);
            return result;
        }

        private static List<ProductsDTO> GetSystemProductDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<ProductsDTO> productsDTOList = null;
            try
            {
                ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
                executionContext.SetSiteId(siteId);
                ProductsList productsListBL = new ProductsList(executionContext);
                productsDTOList = productsListBL.GetSystemProductsDTOList(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading the system products", ex);
            }

            if (productsDTOList == null)
            {
                productsDTOList = new List<ProductsDTO>();
            }
            log.LogMethodExit(productsDTOList);
            return productsDTOList;
        }

        private static DateTime? GetProductsLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? productsLastUpdateTime = null;
            try
            {
                ProductsList productsListBL = new ProductsList();
                productsLastUpdateTime = productsListBL.GetProductsLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                productsLastUpdateTime = null;
                log.Error("Error occurred while loading the products last update time", ex);
            }
            return productsLastUpdateTime;
        }



        private static List<ProductsDTO> GetProductsDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<ProductsDTO> productsDTOList = null;
            try
            {
                ProductsList productsListBL = new ProductsList();
                List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, siteId.ToString()));
                productsDTOList = productsListBL.GetProductsDTOList(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading the products", ex);
            }

            if (productsDTOList == null)
            {
                productsDTOList = new List<ProductsDTO>();
            }
            log.LogMethodExit(productsDTOList);
            return productsDTOList;
        }

        private static List<SalesOfferGroupDTO> GetSalesOfferGroupDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<SalesOfferGroupDTO> salesOfferGroupDTOList = null;
            try
            {
                SalesOfferGroupList salesOfferGroupList = new SalesOfferGroupList();
                List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>> searchParameters = new List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>>();
                searchParameters.Add(new KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>(SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.SITE_ID, siteId.ToString()));
                salesOfferGroupDTOList = salesOfferGroupList.GetAllSalesOfferGroups(searchParameters, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading the SalesGroup", ex);
            }
            if (salesOfferGroupDTOList == null)
            {
                salesOfferGroupDTOList = new List<SalesOfferGroupDTO>();
            }
            log.LogMethodExit(salesOfferGroupDTOList);
            return salesOfferGroupDTOList;
        }

        private ProductsContainerDTO CreateProductsContainerDTO(ProductsDTO productsDTO, bool isSystemProduct)
        {
            log.LogMethodEntry(productsDTO);
            ProductsContainerDTO productsContainerDTO = new ProductsContainerDTO(productsDTO.ProductId, productsDTO.ProductName, productsDTO.ProductType, productsDTO.Description,
                                                                                    productsDTO.ProductTypeId, productsDTO.CategoryId, productsDTO.Price, productsDTO.SortOrder, productsDTO.SiteId, productsDTO.AutoGenerateCardNumber, productsDTO.QuantityPrompt, productsDTO.OnlyForVIP, productsDTO.AllowPriceOverride,
                                                                        productsDTO.RegisteredCustomerOnly, productsDTO.ManagerApprovalRequired, productsDTO.VerifiedCustomerOnly,
                                                                        productsDTO.MinimumQuantity, productsDTO.TrxHeaderRemarksMandatory, productsDTO.ImageFileName, productsDTO.AdvancePercentage,
                                                                        productsDTO.AdvanceAmount, productsDTO.WaiverRequired, productsDTO.InvokeCustomerRegistration, productsDTO.WaiverSetId,
                                                                        productsDTO.LoadToSingleCard, productsDTO.IsGroupMeal, isSystemProduct, productsDTO.ActiveFlag, productsDTO.POSTypeId, productsDTO.IssueNotificationDevice, productsDTO.NotificationTagProfileId, productsDTO.SearchDescription, productsDTO.IsRecommended,
                                                                        productsDTO.MaxQtyPerDay, productsDTO.WebDescription, productsDTO.AvailableUnits, productsDTO.TranslatedProductName, productsDTO.TranslatedProductDescription, productsDTO.DisplayInPOS, productsDTO.Guid, productsDTO.ExternalSystemReference, 
                                                                        productsDTO.StartDate, productsDTO.ExpiryDate, productsDTO.Tax_id, (decimal)productsDTO.TaxPercentage, productsDTO.OrderTypeId.HasValue == false ? -1 : productsDTO.OrderTypeId.Value, productsDTO.CardCount, productsDTO.TrxRemarksMandatory,
                                                                        productsDTO.Tickets, productsDTO.FaceValue, productsDTO.DisplayGroup, productsDTO.TicketAllowed, productsDTO.VipCard, productsDTO.TaxInclusivePrice, productsDTO.InventoryProductCode, productsDTO.ExpiryDate,
                                                                        productsDTO.AutoCheckOut, productsDTO.CheckInFacilityId, productsDTO.MaxCheckOutAmount, productsDTO.CustomDataSetId, productsDTO.CardTypeId, productsDTO.OverridePrintTemplateId, productsDTO.StartDate, productsDTO.ButtonColor,
                                                                        productsDTO.MinimumUserPrice, productsDTO.TextColor, productsDTO.Font, productsDTO.Modifier, productsDTO.EmailTemplateId, productsDTO.MaximumTime, productsDTO.MinimumTime, productsDTO.CardValidFor, productsDTO.AdditionalTaxInclusive,
                                                                        productsDTO.AdditionalPrice, productsDTO.AdditionalTaxId, productsDTO.SegmentCategoryId, productsDTO.CardExpiryDate, productsDTO.ProductDisplayGroupFormatId, productsDTO.EnableVariableLockerHours, productsDTO.CategoryName,
                                                                        productsDTO.CardSale, productsDTO.ZoneCode, productsDTO.LockerMode, productsDTO.TaxName, productsDTO.UsedInDiscounts, productsDTO.CreditPlusConsumptionId, productsDTO.MapedDisplayGroup, productsDTO.LinkChildCard,
                                                                        productsDTO.LicenseType, productsDTO.ZoneId, productsDTO.LockerExpiryInHours, productsDTO.LockerExpiryDate, productsDTO.HsnSacCode, productsDTO.MembershipId, productsDTO.IsSellable,
                                                                        productsDTO.ServiceCharge, productsDTO.PackingCharge, productsDTO.Bonus, productsDTO.Credits, productsDTO.Courtesy, productsDTO.Time, productsDTO.ServiceChargeIsApplicable, productsDTO.ServiceChargePercentage,
                                                                        productsDTO.GratuityIsApplicable, productsDTO.GratuityPercentage, productsDTO.MaximumQuantity, productsDTO.PauseType, productsDTO.CustomerProfilingGroupId, 0, AGE_UPPER_LIMIT);
            if (productsDTO.InventoryItemDTO != null && productsDTO.InventoryItemDTO.IsActive)
            {
                InventoryItemContainerDTO inventoryItemContainerDTO = new InventoryItemContainerDTO(productsDTO.InventoryItemDTO.ProductId,
                                                                       productsDTO.InventoryItemDTO.Code, productsDTO.InventoryItemDTO.Description,
                                                                       productsDTO.InventoryItemDTO.CategoryId, productsDTO.InventoryItemDTO.DefaultLocationId,
                                                                       productsDTO.InventoryItemDTO.OutboundLocationId, productsDTO.InventoryItemDTO.IsRedeemable,
                                                                       productsDTO.InventoryItemDTO.IsSellable, Convert.ToDecimal(productsDTO.InventoryItemDTO.PriceInTickets),
                                                                       productsDTO.InventoryItemDTO.ImageFileName, productsDTO.InventoryItemDTO.TurnInPriceInTickets,
                                                                       productsDTO.InventoryItemDTO.LotControlled, Convert.ToDecimal(productsDTO.InventoryItemDTO.LastPurchasePrice),
                                                                       productsDTO.InventoryItemDTO.TaxInclusiveCost, productsDTO.InventoryItemDTO.ExpiryType,
                                                                       productsDTO.InventoryItemDTO.ExpiryDays, productsDTO.InventoryItemDTO.MarketListItem,
                                                                       productsDTO.InventoryItemDTO.UomId, productsDTO.InventoryItemDTO.InventoryUOMId, productsDTO.InventoryItemDTO.Cost,
                                                                       productsDTO.InventoryItemDTO.ReorderQuantity, productsDTO.InventoryItemDTO.PurchaseTaxId, productsDTO.InventoryItemDTO.MasterEntityId);
                if (productsDTO.InventoryItemDTO.ProductBarcodeDTOList != null && productsDTO.InventoryItemDTO.ProductBarcodeDTOList.Any())
                {
                    List<ProductBarcodeContainerDTO> productBarcodeContainerDTOList = new List<ProductBarcodeContainerDTO>();
                    foreach (ProductBarcodeDTO productBarcodeDTO in productsDTO.InventoryItemDTO.ProductBarcodeDTOList)
                    {
                        productBarcodeContainerDTOList.Add(new ProductBarcodeContainerDTO(productBarcodeDTO.Id, productBarcodeDTO.BarCode));
                    }
                    inventoryItemContainerDTO.ProductBarcodeContainerDTOList = productBarcodeContainerDTOList;
                }

                productsContainerDTO.InventoryItemContainerDTO = inventoryItemContainerDTO;
            }
            if (productsDTO.ComboProductDTOList != null && productsDTO.ComboProductDTOList.Any())
            {
                List<ComboProductContainerDTO> comboProductContainerDTOList = new List<ComboProductContainerDTO>();                
                foreach (ComboProductDTO comboProductDTO in productsDTO.ComboProductDTOList)
                {
                    comboProductContainerDTOList.Add(new ComboProductContainerDTO(comboProductDTO.ComboProductId, comboProductDTO.ProductId, comboProductDTO.ChildProductId,
                                                                            comboProductDTO.ChildProductName, comboProductDTO.ChildProductType, comboProductDTO.ChildProductAutoGenerateCardNumber,
                                                                            Convert.ToDecimal(comboProductDTO.Quantity), comboProductDTO.CategoryId, comboProductDTO.SortOrder, comboProductDTO.Price,
                                                                            comboProductDTO.PriceInclusive, comboProductDTO.DisplayGroupId, comboProductDTO.DisplayGroup,
                                                                            comboProductDTO.AdditionalProduct, comboProductDTO.MaximumQuantity, 0, 0));
                }
                productsContainerDTO.ComboProductContainerDTOList = comboProductContainerDTOList;
            }
            if (productsDTO.ProductModifierDTOList != null && productsDTO.ProductModifierDTOList.Any())
            {
                List<ProductModifierContainerDTO> productModifierContainerDTOList = new List<ProductModifierContainerDTO>();
                foreach (ProductModifiersDTO productModifiersDTO in productsDTO.ProductModifierDTOList)
                {
                    ProductModifierContainerDTO productModifierContainerDTO = new ProductModifierContainerDTO(productModifiersDTO.ProductModifierId, productModifiersDTO.CategoryId,
                                                                productModifiersDTO.ProductId, productModifiersDTO.ModifierSetId, productModifiersDTO.AutoShowinPOS, productModifiersDTO.SortOrder);
                    if (productModifiersDTO.ModifierSetDTO != null)
                    {
                        productModifierContainerDTO.ModifierSetContainerDTO = new ModifierSetContainerDTO(productModifiersDTO.ModifierSetDTO.ModifierSetId, productModifiersDTO.ModifierSetDTO.SetName, productModifiersDTO.ModifierSetDTO.MinQuantity, productModifiersDTO.ModifierSetDTO.MaxQuantity, productModifiersDTO.ModifierSetDTO.FreeQuantity);
                        if (productModifiersDTO.ModifierSetDTO.ParentModifierSetId > -1)
                        {
                            List<ProductModifiersDTO> parentModifierSetDTO = productsDTO.ProductModifierDTOList.Where(x => x.ModifierSetDTO != null && x.ModifierSetDTO.ModifierSetId == productModifiersDTO.ModifierSetDTO.ParentModifierSetId).ToList();
                            if (parentModifierSetDTO != null && parentModifierSetDTO.Any())
                            {
                                productModifierContainerDTO.ModifierSetContainerDTO.ParentModifierSetDTO = new ModifierSetContainerDTO(parentModifierSetDTO[0].ModifierSetDTO.ModifierSetId, parentModifierSetDTO[0].ModifierSetDTO.SetName, parentModifierSetDTO[0].ModifierSetDTO.MinQuantity, parentModifierSetDTO[0].ModifierSetDTO.MaxQuantity, parentModifierSetDTO[0].ModifierSetDTO.FreeQuantity);
                                productModifierContainerDTO.ModifierSetContainerDTO.ParentModifierSetDTO.ModifierSetDetailsContainerDTOList = new List<ModifierSetDetailsContainerDTO>();
                                foreach (ModifierSetDetailsDTO modifierSetDetailsDTO in parentModifierSetDTO[0].ModifierSetDTO.ModifierSetDetailsDTO)
                                {
                                    productModifierContainerDTO.ModifierSetContainerDTO.ParentModifierSetDTO.ModifierSetDetailsContainerDTOList.Add(new ModifierSetDetailsContainerDTO(modifierSetDetailsDTO.ModifierSetDetailId, modifierSetDetailsDTO.ModifierSetId, modifierSetDetailsDTO.Price, modifierSetDetailsDTO.SortOrder, modifierSetDetailsDTO.ModifierProductId));
                                }
                            }
                        }
                        if (productModifiersDTO.ModifierSetDTO.ModifierSetDetailsDTO != null && productModifiersDTO.ModifierSetDTO.ModifierSetDetailsDTO.Any())
                        {
                            productModifierContainerDTO.ModifierSetContainerDTO.ModifierSetDetailsContainerDTOList = new List<ModifierSetDetailsContainerDTO>();
                            foreach (ModifierSetDetailsDTO modifierSetDetailsDTO in productModifiersDTO.ModifierSetDTO.ModifierSetDetailsDTO)
                            {
                                productModifierContainerDTO.ModifierSetContainerDTO.ModifierSetDetailsContainerDTOList.Add(new ModifierSetDetailsContainerDTO(modifierSetDetailsDTO.ModifierSetDetailId, modifierSetDetailsDTO.ModifierSetId, modifierSetDetailsDTO.Price, modifierSetDetailsDTO.SortOrder, modifierSetDetailsDTO.ModifierProductId));
                            }
                        }
                    }
                    productModifierContainerDTOList.Add(productModifierContainerDTO);
                }
                productsContainerDTO.ProductModifierContainerDTOList = productModifierContainerDTOList;
            }
            if (productsDTO.ProductsDisplayGroupDTOList != null && productsDTO.ProductsDisplayGroupDTOList.Any())
            {
                List<ProductsDisplayGroupContainerDTO> productsDisplayGroupContainerDTOList = new List<ProductsDisplayGroupContainerDTO>();
                foreach (ProductsDisplayGroupDTO productsDisplayGroupDTO in productsDTO.ProductsDisplayGroupDTOList)
                {
                    string displayGroupName = string.Empty;
                    if (displayGroupIdProductDisplayGroupFormatDTODictionary.ContainsKey(productsDisplayGroupDTO.DisplayGroupId))
                    {
                        displayGroupName = displayGroupIdProductDisplayGroupFormatDTODictionary[productsDisplayGroupDTO.DisplayGroupId].DisplayGroup;
                    }
                    productsDisplayGroupContainerDTOList.Add(new ProductsDisplayGroupContainerDTO(productsDisplayGroupDTO.Id, productsDisplayGroupDTO.DisplayGroupId, displayGroupName/*, productDisplayGroupFormatDTOList.Any() ? productDisplayGroupFormatDTOList[0].ImageFileName : string.Empty, productDisplayGroupFormatDTOList.Any() ? productDisplayGroupFormatDTOList[0].SortOrder : -1*/));
                }
                productsContainerDTO.ProductsDisplayGroupContainerDTOList = productsDisplayGroupContainerDTOList;
            }
            if (productsDTO.ProductSubscriptionDTO != null)
            {
                ProductSubscriptionDTO tempDTO = productsDTO.ProductSubscriptionDTO;
                ProductSubscriptionContainerDTO productSubscriptionContainerDTO = new ProductSubscriptionContainerDTO(tempDTO.ProductSubscriptionId, tempDTO.ProductsId,
                    tempDTO.ProductSubscriptionName, tempDTO.ProductSubscriptionDescription, tempDTO.SubscriptionPrice, tempDTO.SubscriptionCycle, tempDTO.UnitOfSubscriptionCycle,
                    tempDTO.SubscriptionCycleValidity, tempDTO.SeasonStartDate, tempDTO.FreeTrialPeriodCycle, tempDTO.AllowPause, tempDTO.BillInAdvance, tempDTO.PaymentCollectionMode,
                    tempDTO.AutoRenew, tempDTO.AutoRenewalMarkupPercent, tempDTO.RenewalGracePeriodCycle, tempDTO.NoOfRenewalReminders, tempDTO.SendFirstReminderBeforeXDays,
                    tempDTO.ReminderFrequencyInDays, tempDTO.CancellationOption);
                productsContainerDTO.ProductSubscriptionContainerDTO = productSubscriptionContainerDTO;
            }
            if (productsDTO.UpsellOffersDTOList != null && productsDTO.UpsellOffersDTOList.Any())
            {
                List<UpsellOffersContainerDTO> upsellOffersContainerDTOList = new List<UpsellOffersContainerDTO>();
                foreach (UpsellOffersDTO upsellOffersDTO in productsDTO.UpsellOffersDTOList)
                {
                    UpsellOffersContainerDTO upsellOffersContainerDTO = new UpsellOffersContainerDTO(upsellOffersDTO.OfferId, upsellOffersDTO.ProductId, upsellOffersDTO.OfferProductId, upsellOffersDTO.OfferMessage, upsellOffersDTO.SaleGroupId, upsellOffersDTO.EffectiveDate);
                    if (saleGroupIdSaleGroupContainerDTODictionary.ContainsKey(upsellOffersContainerDTO.SaleGroupId))
                    {
                        upsellOffersContainerDTO.SalesOfferGroupContainerDTO = saleGroupIdSaleGroupContainerDTODictionary[upsellOffersContainerDTO.SaleGroupId];
                    }
                    upsellOffersContainerDTOList.Add(upsellOffersContainerDTO);
                }
                productsContainerDTO.UpsellOffersContainerDTOList = upsellOffersContainerDTOList;
            }
            if (productsDTO.CrossSellOffersDTOList != null && productsDTO.CrossSellOffersDTOList.Any())
            {
                List<UpsellOffersContainerDTO> crossSellOffersContainerDTOList = new List<UpsellOffersContainerDTO>();
                foreach (UpsellOffersDTO crossSellOfferDTO in productsDTO.CrossSellOffersDTOList)
                {
                    UpsellOffersContainerDTO crossSellOffersContainerDTO = new UpsellOffersContainerDTO(crossSellOfferDTO.OfferId, crossSellOfferDTO.ProductId, crossSellOfferDTO.OfferProductId, crossSellOfferDTO.OfferMessage, crossSellOfferDTO.SaleGroupId, crossSellOfferDTO.EffectiveDate);
                    if (saleGroupIdSaleGroupContainerDTODictionary.ContainsKey(crossSellOffersContainerDTO.SaleGroupId))
                    {
                        crossSellOffersContainerDTO.SalesOfferGroupContainerDTO = saleGroupIdSaleGroupContainerDTODictionary[crossSellOffersContainerDTO.SaleGroupId];
                    }
                    crossSellOffersContainerDTOList.Add(crossSellOffersContainerDTO);

                }
                productsContainerDTO.CrossSellProductsContainerDTOList = crossSellOffersContainerDTOList;
            }
            if (productsDTO.CustomDataSetDTO != null)
            {
                productsContainerDTO.CustomDataContainerDTOList = GetCustomDataContainerDTOList(productsDTO.CustomDataSetDTO);
            }
            if (productsDTO.ProductGamesDTOList != null && productsDTO.ProductGamesDTOList.Any())
            {
                List<ProductGamesContainerDTO> productGamesContainerDTOList = new List<ProductGamesContainerDTO>();
                foreach (ProductGamesDTO productGamesDTO in productsDTO.ProductGamesDTOList)
                {
                    ProductGamesContainerDTO productGamesContainerDTO = new ProductGamesContainerDTO(productGamesDTO.Product_game_id, productGamesDTO.Product_id, productGamesDTO.Game_id, productGamesDTO.Quantity, productGamesDTO.ValidFor, productGamesDTO.ExpiryDate,
                                                                                  productGamesDTO.ValidMinutesDays, productGamesDTO.Game_profile_id, productGamesDTO.Frequency, productGamesDTO.Guid, productGamesDTO.CardTypeId,
                                                                                  productGamesDTO.EntitlementType, productGamesDTO.OptionalAttribute, productGamesDTO.ExpiryTime, productGamesDTO.CustomDataSetId, productGamesDTO.TicketAllowed, productGamesDTO.EffectiveAfterDays,
                                                                                  productGamesDTO.FromDate, productGamesDTO.Monday, productGamesDTO.Tuesday, productGamesDTO.Wednesday, productGamesDTO.Thursday, productGamesDTO.Friday, productGamesDTO.Saturday, productGamesDTO.Sunday);
                    if (productGamesDTO.ProductGamesExtendedDTOList != null && productGamesDTO.ProductGamesExtendedDTOList.Any())
                    {
                        foreach (ProductGamesExtendedDTO productGamesExtendedDTO in productGamesDTO.ProductGamesExtendedDTOList)
                        {
                            ProductGamesExtendedContainerDTO productGamesExtendedContainerDTO = new ProductGamesExtendedContainerDTO(productGamesExtendedDTO.Id, productGamesExtendedDTO.ProductGameId, productGamesExtendedDTO.GameId, productGamesExtendedDTO.GameProfileId, productGamesExtendedDTO.Exclude, productGamesExtendedDTO.Guid, productGamesExtendedDTO.PlayLimitPerGame);
                            productGamesContainerDTO.ProductGamesExtendedContainerDTOList.Add(productGamesExtendedContainerDTO);
                        }
                    }
                    if (productGamesGuidEntityOverrideDateContainerDTOListDictionary.ContainsKey(productGamesDTO.Guid))
                    {
                        productGamesContainerDTO.EntityOverrideDateContainerDTOList = productGamesGuidEntityOverrideDateContainerDTOListDictionary[productGamesDTO.Guid];
                    }
                    productGamesContainerDTOList.Add(productGamesContainerDTO);
                }
                productsContainerDTO.ProductGamesContainerDTOList = productGamesContainerDTOList;
            }
            if (productsDTO.ProductCreditPlusDTOList != null && productsDTO.ProductCreditPlusDTOList.Any())
            {
                List<ProductCreditPlusContainerDTO> productCreditPlusContainerDTOList = new List<ProductCreditPlusContainerDTO>();
                foreach (ProductCreditPlusDTO productCreditPlusDTO in productsDTO.ProductCreditPlusDTOList)
                {
                    ProductCreditPlusContainerDTO productCreditPlusContainerDTO = new ProductCreditPlusContainerDTO(productCreditPlusDTO.ProductCreditPlusId, productCreditPlusDTO.CreditPlus, "Y".Equals(productCreditPlusDTO.Refundable, StringComparison.InvariantCultureIgnoreCase), productCreditPlusDTO.Remarks, productCreditPlusDTO.Product_id, productCreditPlusDTO.CreditPlusType,
                                                                                  productCreditPlusDTO.PeriodFrom, productCreditPlusDTO.PeriodTo, productCreditPlusDTO.ValidForDays, "Y".Equals(productCreditPlusDTO.ExtendOnReload, StringComparison.InvariantCultureIgnoreCase), productCreditPlusDTO.TimeFrom < 0 ? (decimal?)null : productCreditPlusDTO.TimeFrom,
                                                                                  productCreditPlusDTO.TimeTo < 0 ? (decimal?)null : productCreditPlusDTO.TimeTo, productCreditPlusDTO.Minutes, productCreditPlusDTO.Monday, productCreditPlusDTO.Tuesday, productCreditPlusDTO.Wednesday, productCreditPlusDTO.Thursday,
                                                                                  productCreditPlusDTO.Friday, productCreditPlusDTO.Saturday, productCreditPlusDTO.Sunday, productCreditPlusDTO.TicketAllowed, productCreditPlusDTO.Frequency, productCreditPlusDTO.PauseAllowed, productCreditPlusDTO.EffectiveAfterMinutes);
                    if (productCreditPlusDTO.CreditPlusConsumptionRulesList != null && productCreditPlusDTO.CreditPlusConsumptionRulesList.Any())
                    {
                        foreach (CreditPlusConsumptionRulesDTO creditPlusConsumptionRulesDTO in productCreditPlusDTO.CreditPlusConsumptionRulesList)
                        {
                            CreditPlusConsumptionRulesContainerDTO creditPlusConsumptionRulesContainerDTO = new CreditPlusConsumptionRulesContainerDTO(creditPlusConsumptionRulesDTO.PKId, creditPlusConsumptionRulesDTO.ProductCreditPlusId, creditPlusConsumptionRulesDTO.POSTypeId,
                                                                                                                 creditPlusConsumptionRulesDTO.ExpiryDate == DateTime.MinValue ? (DateTime?)null : creditPlusConsumptionRulesDTO.ExpiryDate, creditPlusConsumptionRulesDTO.Guid, creditPlusConsumptionRulesDTO.GameId, creditPlusConsumptionRulesDTO.GameProfileId, creditPlusConsumptionRulesDTO.ProductId,
                                                                                                                 creditPlusConsumptionRulesDTO.Quantity, creditPlusConsumptionRulesDTO.QuantityLimit, creditPlusConsumptionRulesDTO.CategoryId, creditPlusConsumptionRulesDTO.DiscountAmount, creditPlusConsumptionRulesDTO.DiscountPercentage,
                                                                                                                 creditPlusConsumptionRulesDTO.DiscountedPrice, creditPlusConsumptionRulesDTO.OrderTypeId);
                            productCreditPlusContainerDTO.CreditPlusConsumptionRulesContainerDTOList.Add(creditPlusConsumptionRulesContainerDTO);
                        }
                    }
                    if (productCreditPlusGuidEntityOverrideDateContainerDTOListDictionary.ContainsKey(productCreditPlusDTO.Guid))
                    {
                        productCreditPlusContainerDTO.EntityOverrideDateContainerDTOList = productCreditPlusGuidEntityOverrideDateContainerDTOListDictionary[productCreditPlusDTO.Guid];
                    }
                    productCreditPlusContainerDTOList.Add(productCreditPlusContainerDTO);
                }
                productsContainerDTO.ProductCreditPlusContainerDTOList = productCreditPlusContainerDTOList;
            }
            if (productsDTO.CustomerProfilingGroupId > -1 && productsDTO.CustomerProfilingGroupDTO != null)
            {
                List<CustomerProfilingContainerDTO> list = new List<CustomerProfilingContainerDTO>();
                if (productsDTO.CustomerProfilingGroupDTO.CustomerProfilingDTOList != null &&
                    productsDTO.CustomerProfilingGroupDTO.CustomerProfilingDTOList.Any())
                {
                    foreach (CustomerProfilingDTO customerProfilingDTO in productsDTO.CustomerProfilingGroupDTO.CustomerProfilingDTOList)
                    {
                        CustomerProfilingContainerDTO customerProfilingContainerDTO = new CustomerProfilingContainerDTO(
                            customerProfilingDTO.CustomerProfilingId,
                            customerProfilingDTO.CustomerProfilingGroupId,
                            customerProfilingDTO.ProfileType,
                            customerProfilingDTO.CompareOperator,
                            customerProfilingDTO.ProfileValue,
                            customerProfilingDTO.ProfileTypeName);
                        list.Add(customerProfilingContainerDTO);
                    }
                }
                productsDTO.CustomerProfilingGroupDTO.AgeUpperLimit = GetAgeUpperLimit(productsDTO.CustomerProfilingGroupDTO);
                productsDTO.CustomerProfilingGroupDTO.AgeLowerLimit = GetAgeLowerLimit(productsDTO.CustomerProfilingGroupDTO);

                CustomerProfilingGroupContainerDTO customerProfilingGroupContainerDTO = new CustomerProfilingGroupContainerDTO(
                            productsDTO.CustomerProfilingGroupDTO.CustomerProfilingGroupId,
                            productsDTO.CustomerProfilingGroupDTO.GroupName, list);
                productsContainerDTO.CustomerProfilingGroupContainerDTO = customerProfilingGroupContainerDTO;
                productsContainerDTO.AgeUpperLimit = GetAgeUpperLimit(productsDTO.CustomerProfilingGroupDTO);
                productsContainerDTO.AgeLowerLimit = GetAgeLowerLimit(productsDTO.CustomerProfilingGroupDTO);

            }
            log.LogMethodExit(productsContainerDTO);
            return productsContainerDTO;
        }


        internal int GetAgeLowerLimit(CustomerProfilingGroupDTO customerProfilingGroupDTO)
        {
            log.LogMethodEntry(customerProfilingGroupDTO);
            int ageLowerLimit = 0;
            LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(siteId, "CUSTOMER_PROFILE_TYPES");
            if (lookupsContainerDTO != null
                && lookupsContainerDTO.LookupValuesContainerDTOList != null
                && lookupsContainerDTO.LookupValuesContainerDTOList.Any())
            {
                LookupValuesContainerDTO ageLookupValues = lookupsContainerDTO.LookupValuesContainerDTOList
                                                                   .Where(x => x.LookupValue == "Age").FirstOrDefault();
                if (ageLookupValues != null)
                {
                    if (customerProfilingGroupDTO.CustomerProfilingDTOList != null &&
                              customerProfilingGroupDTO.CustomerProfilingDTOList.Any())
                    {
                        List<CustomerProfilingDTO> ageProfiling = customerProfilingGroupDTO.CustomerProfilingDTOList
                                                                    .Where(x => x.ProfileType == ageLookupValues.LookupValueId).ToList();
                        if (ageProfiling != null && ageProfiling.Any())
                        {
                            foreach (CustomerProfilingDTO customerProfilingDTO in customerProfilingGroupDTO.CustomerProfilingDTOList)
                            {
                                if (customerProfilingDTO.CompareOperator == ">" || customerProfilingDTO.CompareOperator == ">=")
                                {
                                    ageLowerLimit = Convert.ToInt32(customerProfilingDTO.ProfileValue);
                                }
                            }
                        }
                        else
                        {
                            log.Debug("Product does not have any Age profiling record");
                        }
                    }
                }
                else
                {
                    log.Debug("No Age profiling is set");
                }
            }
            else
            {
                log.Debug("No Profiling is set.");
            }
            log.LogMethodExit(ageLowerLimit);
            return ageLowerLimit;
        }
        internal int GetAgeUpperLimit(CustomerProfilingGroupDTO customerProfilingGroupDTO)
        {
            log.LogMethodEntry(customerProfilingGroupDTO);
            int ageUpperLimit = AGE_UPPER_LIMIT;
            LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(siteId, "CUSTOMER_PROFILE_TYPES");
            if (lookupsContainerDTO != null
                && lookupsContainerDTO.LookupValuesContainerDTOList != null
                && lookupsContainerDTO.LookupValuesContainerDTOList.Any())
            {
                LookupValuesContainerDTO ageLookupValues = lookupsContainerDTO.LookupValuesContainerDTOList
                                                                   .Where(x => x.LookupValue == "Age").FirstOrDefault();
                if (ageLookupValues != null)
                {
                    if (customerProfilingGroupDTO.CustomerProfilingDTOList != null &&
                              customerProfilingGroupDTO.CustomerProfilingDTOList.Any())
                    {

                        List<CustomerProfilingDTO> ageProfiling = customerProfilingGroupDTO.CustomerProfilingDTOList
                                                                    .Where(x => x.ProfileType == ageLookupValues.LookupValueId).ToList();
                        if (ageProfiling != null && ageProfiling.Any())
                        {
                            foreach (CustomerProfilingDTO customerProfilingDTO in customerProfilingGroupDTO.CustomerProfilingDTOList)
                            {
                                if (customerProfilingDTO.CompareOperator == "<" || customerProfilingDTO.CompareOperator == "<=")
                                {
                                    ageUpperLimit = Convert.ToInt32(customerProfilingDTO.ProfileValue);
                                }
                            }
                        }
                        else
                        {
                            log.Debug("Product does not have any Age profiling record");
                        }
                    }
                }
                else
                {
                    log.Debug("No Age profiling is set");
                }
            }
            else
            {
                log.Debug("No Profiling is set.");
            }
            log.LogMethodExit(ageUpperLimit);
            return ageUpperLimit;
        }

        private ProductCalendarContainer GetProductCalendarContainer(DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(dateTimeRange);
            ProductCalendarContainer result = productCalendarContainerCache.GetOrAdd(dateTimeRange, (k) => new ProductCalendarContainer(siteId, productsDTOList, dateTimeRange, TimeSpan.FromMinutes(5)));
            log.LogMethodExit(result);
            return result;
        }


        public ProductCalendarContainerDTOCollection GetProductCalendarContainerDTOCollection(string manualProductType, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(manualProductType, dateTimeRange);
            Dictionary<string, ProductCalendarContainerDTOCollection> productCalendarContainerDTOCollectionDictionary = productCalendarContainerDTOCollectionCache.GetOrAdd(dateTimeRange, (k) => GetProductCalendarContainerDTOCollectionDictinary(dateTimeRange));
            if (productCalendarContainerDTOCollectionDictionary.ContainsKey(manualProductType) == false)
            {
                string errorMessage = "ProductCalendarContainerDTOCollection for manualProductType :" + manualProductType.ToString() + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            ProductCalendarContainerDTOCollection returnValue = productCalendarContainerDTOCollectionDictionary[manualProductType];
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private Dictionary<string, ProductCalendarContainerDTOCollection> GetProductCalendarContainerDTOCollectionDictinary(DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(dateTimeRange);
            Dictionary<string, ProductCalendarContainerDTOCollection> result = new Dictionary<string, ProductCalendarContainerDTOCollection>();
            ProductCalendarContainer productCalendarContainer = GetProductCalendarContainer(dateTimeRange);
            List<ProductCalendarContainerDTO> productCalendarContainerDTOList = new List<ProductCalendarContainerDTO>();
            foreach (var redeemableProductsContainerDTO in redeemableProductsContainerDTOList)
            {
                if (redeemableProductsContainerDTO.IsActive == false)
                {
                    continue;
                }
                ProductCalendarContainerDTO productCalendarContainerDTO = productCalendarContainer.GetProductCalendarContainerDTO(redeemableProductsContainerDTO.ProductId);
                productCalendarContainerDTOList.Add(productCalendarContainerDTO);
            }
            result.Add(ManualProductType.REDEEMABLE.ToString(), new ProductCalendarContainerDTOCollection(productCalendarContainerDTOList));
            productCalendarContainerDTOList = new List<ProductCalendarContainerDTO>();
            foreach (var sellableProductsContainerDTO in sellableProductsContainerDTOList)
            {
                if (sellableProductsContainerDTO.IsActive == false)
                {
                    continue;
                }
                ProductCalendarContainerDTO productCalendarContainerDTO = productCalendarContainer.GetProductCalendarContainerDTO(sellableProductsContainerDTO.ProductId);
                productCalendarContainerDTOList.Add(productCalendarContainerDTO);
            }
            result.Add(ManualProductType.SELLABLE.ToString(), new ProductCalendarContainerDTOCollection(productCalendarContainerDTOList));
            log.LogMethodExit(result);
            return result;
        }


        private DateTimeRange GetDateTimeRange(DateTime dateTime)
        {
            return new DateTimeRange(dateTime.Date, dateTime.Date.AddDays(1));
        }

        private List<CustomDataContainerDTO> GetCustomDataContainerDTOList(CustomDataSetDTO customDataSetDTO)
        {
            log.LogMethodEntry(customDataSetDTO);
            try
            {
                List<CustomDataContainerDTO> result = new List<CustomDataContainerDTO>();
                if (customDataSetDTO.CustomDataDTOList != null && customDataSetDTO.CustomDataDTOList.Any())
                {
                    foreach (var item in customDataSetDTO.CustomDataDTOList)
                    {
                        log.Debug("item.CustomAttributeId" + item.CustomAttributeId);
                        CustomAttributesContainerDTO customAttributesContainerDTO = CustomAttributeContainerList.GetCustomAttributeContainerDTO(siteId, item.CustomAttributeId);
                        if (customAttributesContainerDTO == null)
                        {
                            log.Error("Could not find the CustomAttributesContainerDTO for attributeId :" + item.CustomAttributeId);
                            continue;
                        }
                        CustomDataContainerDTO customDataContainerDTO = new CustomDataContainerDTO();
                        customDataContainerDTO.CustomAttributeId = item.CustomAttributeId;
                        customDataContainerDTO.Name = customAttributesContainerDTO.Name;
                        customDataContainerDTO.Type = customAttributesContainerDTO.Type;
                        customDataContainerDTO.CustomDataSetId = customDataSetDTO.CustomDataSetId;
                        customDataContainerDTO.CustomDataId = item.CustomDataId;
                        customDataContainerDTO.CustomDataText = item.CustomDataText;
                        customDataContainerDTO.CustomDataNumber = item.CustomDataNumber;
                        customDataContainerDTO.CustomDataDate = item.CustomDataDate;
                        if (item.ValueId > -1 && customAttributesContainerDTO.CustomAttributeValueListContainerDTOList != null
                            && customAttributesContainerDTO.CustomAttributeValueListContainerDTOList.Any())
                        {
                            var dto = customAttributesContainerDTO.CustomAttributeValueListContainerDTOList.Where(x => x.ValueId == item.ValueId).FirstOrDefault();
                            if (dto != null)
                            {
                                customDataContainerDTO.Value = dto.Value;
                                customDataContainerDTO.ValueId = dto.ValueId;
                            }
                            else
                            {
                                customDataContainerDTO.Value = string.Empty;
                                customDataContainerDTO.ValueId = -1;
                            }
                        }
                        result.Add(customDataContainerDTO);
                    }
                }
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public ProductsContainerDTO GetSystemProductContainerDTO(string productType, string productName = null)
        {
            log.LogMethodEntry(productType);
            if (string.IsNullOrWhiteSpace(productType))
            {
                string errorMessage = "Products with productType is empty.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            if (systemProductsDTOListDictionary.ContainsKey(productType) == false)
            {
                string errorMessage = "Products with productType :" + productType + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            ProductsContainerDTO result;
            if (string.IsNullOrWhiteSpace(productName))
            {
                result = systemProductsDTOListDictionary[productType][0];
            }
            else
            {
                result = systemProductsDTOListDictionary[productType].First(x => x.ProductName == productName);
            }
            log.LogMethodExit(result);
            return result;
        }
        public ProductsContainerDTO GetSystemProductContainerDTO(int productId)
        {
            log.LogMethodEntry(productId);
            if (systemProductsContainerDTODictionary.ContainsKey(productId) == false)
            {
                string errorMessage = "Products with productId : " + productId + " doesn't exist.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = systemProductsContainerDTODictionary[productId];
            return result;
        }

        public ProductsContainerDTO GetProductsContainerDTO(int productId)
        {
            log.LogMethodEntry(productId);
            if (productIdProductsContainerDTODictionary.ContainsKey(productId) == false)
            {
                string errorMessage = "Products with productId : " + productId + " doesn't exist.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = productIdProductsContainerDTODictionary[productId];
            return result;
        }

        public ProductsContainerDTO GetProductsContainerDTOOrDefault(int productId)
        {
            log.LogMethodEntry(productId);
            if (productIdProductsContainerDTODictionary.ContainsKey(productId) == false)
            {
                string message = "Products with productId : " + productId + " doesn't exist.";
                log.LogMethodExit(null, message);
                return null;
            }
            var result = productIdProductsContainerDTODictionary[productId];
            return result;
        }

        public ProductsContainerDTO GetProductsContainerDTO(string productGuid)
        {
            log.LogMethodEntry(productGuid);
            if (productGuidProductsContainerDTODictionary.ContainsKey(productGuid) == false)
            {
                string errorMessage = "Products with productGuid : " + productGuid + " doesn't exist.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = productGuidProductsContainerDTODictionary[productGuid];
            return result;
        }

        public ProductsContainerDTO GetProductsContainerDTOOrDefault(string productGuid)
        {
            log.LogMethodEntry(productGuid);
            if (productGuidProductsContainerDTODictionary.ContainsKey(productGuid) == false)
            {
                string message = "Products with productGuid : " + productGuid + " doesn't exist.";
                log.LogMethodExit(null, message);
                return null;
            }
            var result = productGuidProductsContainerDTODictionary[productGuid];
            return result;
        }

        public ProductsContainerDTOCollection GetProductsContainerDTOCollection(string manualProductType)
        {
            log.LogMethodEntry(manualProductType);
            ProductsContainerDTOCollection result;
            if (manualProductType == ManualProductType.SELLABLE.ToString())
            {
                result = sellableProductsContainerDTOCollection;
            }
            else if (manualProductType == ManualProductType.REDEEMABLE.ToString())
            {
                result = redeemableProductsContainerDTOCollection;
            }
            else if (manualProductType == ManualProductType.INVENTORY.ToString())
            {
                result = inventoryProductsContainerDTOCollection;
            }
            else
            {
                string errorMessage = "ProductsContainerDTOCollection for manualProductType :" + manualProductType + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            log.LogMethodExit(result);
            return result;
        }

        public List<ProductsContainerDTO> GetActiveProductsContainerDTOList(string manualProductType)
        {
            log.LogMethodEntry(manualProductType);
            List<ProductsContainerDTO> result;
            if (manualProductType == ManualProductType.SELLABLE.ToString())
            {
                result = sellableProductsContainerDTOList;
            }
            else if (manualProductType == ManualProductType.REDEEMABLE.ToString())
            {
                result = redeemableProductsContainerDTOList;
            }
            else
            {
                string errorMessage = "ProductsContainerDTOList for manualProductType :" + manualProductType.ToString() + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal IEnumerable<ProductsContainerDTO> GetActiveProductsContainerDTOList(string manualProductType, Func<ProductsContainerDTO, bool> predicate)
        {
            log.LogMethodEntry(manualProductType, "predicate");
            IEnumerable<ProductsContainerDTO> result;
            if (manualProductType == ManualProductType.SELLABLE.ToString())
            {
                result = sellableProductsContainerDTOList.Where(predicate);
            }
            else if (manualProductType == ManualProductType.REDEEMABLE.ToString())
            {
                result = redeemableProductsContainerDTOList.Where(predicate);
            }
            else
            {
                string errorMessage = "ProductsContainerDTOList for manualProductType :" + manualProductType.ToString() + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            log.LogMethodExit(result);
            return result;
        }

        public List<ProductsContainerDTO> GetActiveProductsContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(activeProductsContainerDTOList);
            return activeProductsContainerDTOList;
        }
        
        public List<ProductsContainerDTO> GetSystemProductsContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(systemProductsContainerDTOList);
            return systemProductsContainerDTOList;
        }

        public List<ProductsContainerDTO> GetInActiveProductsContainerDTOList(string manualProductType)
        {
            log.LogMethodEntry(manualProductType);
            List<ProductsContainerDTO> result;
            if (manualProductType == ManualProductType.SELLABLE.ToString())
            {
                result = inactiveSellableProductsContainerDTOList;
            }
            else if (manualProductType == ManualProductType.REDEEMABLE.ToString())
            {
                result = inactiveRedeemableProductsContainerDTOList;
            }
            else
            {
                string errorMessage = "ProductsContainerDTOList for manualProductType :" + manualProductType.ToString() + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Check whether product is available
        /// <param name="productId">productId</param>
        /// <param name="dateTime">dateTime</param>
        /// </summary>
        /// <returns></returns>
        public bool IsProductAvailable(int productId, DateTime dateTime)
        {
            log.LogMethodEntry(productId, dateTime);
            DateTimeRange dateTimeRange = GetDateTimeRange(dateTime);
            ProductCalendarContainer productCalendarContainer = GetProductCalendarContainer(dateTimeRange);
            bool returnValue = productCalendarContainer.IsProductAvailable(productId, dateTime);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Get the schedule of the product for a give dateTime range
        /// <param name="productId">productId</param>
        /// <param name="dateTime">dateTime</param>
        /// </summary>
        /// <returns></returns>
        public ProductCalendarContainerDTO GetProductCalendarContainerDTO(int productId, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(productId, dateTimeRange);
            DateTimeRange productDateTimeRange = GetDateTimeRange(dateTimeRange.StartDateTime);
            ProductCalendarContainer productCalendarContainer;
            if (productDateTimeRange.Contains(dateTimeRange))
            {
                productCalendarContainer = GetProductCalendarContainer(productDateTimeRange);
            }
            else
            {
                productCalendarContainer = GetProductCalendarContainer(dateTimeRange);
            }
            var returnValue = productCalendarContainer.GetProductCalendarContainerDTO(productId);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public ProductsContainer Refresh()
        {
            log.LogMethodEntry();
            var dateTimeRangeList = productCalendarContainerCache.Keys;
            foreach (var dateTimeRange in dateTimeRangeList)
            {
                if (dateTimeRange.EndDateTime < ServerDateTime.Now)
                {
                    ProductCalendarContainer value;
                    if (productCalendarContainerCache.TryRemove(dateTimeRange, out value))
                    {
                        log.Debug("Removing ProductCalendarContainer of date range" + dateTimeRange);
                    }
                    else
                    {
                        log.Debug("Unable to remove ProductCalendarContainer of date range" + dateTimeRange);
                    }
                }
            }
            dateTimeRangeList = productCalendarContainerDTOCollectionCache.Keys;
            foreach (var dateTimeRange in dateTimeRangeList)
            {
                if (dateTimeRange.EndDateTime < ServerDateTime.Now)
                {
                    Dictionary<string, ProductCalendarContainerDTOCollection> value;
                    if (productCalendarContainerDTOCollectionCache.TryRemove(dateTimeRange, out value))
                    {
                        log.Debug("Removing ProductCalendarContainerDTOCollection of date range" + dateTimeRange);
                    }
                    else
                    {
                        log.Debug("Unable to remove ProductCalendarContainerDTOCollection of date range" + dateTimeRange);
                    }
                }
            }


            ProductsList ProductsListBL = new ProductsList();
            DateTime? updateTime = ProductsListBL.GetProductsLastUpdateTime(siteId);
            if (productsLastUpdateTime.HasValue
                && productsLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Products since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            ProductsContainer result = new ProductsContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }


        public List<ProductsContainerDTO> GetProductContainerDTOListOfDisplayGroups(List<int> displayGroupIdList)
        {
            log.LogMethodEntry(displayGroupIdList);
            HashSet<int> productIdList = new HashSet<int>();
            List<ProductsContainerDTO> result = new List<ProductsContainerDTO>();
            foreach (var displayGroupId in displayGroupIdList)
            {
                if (displayGroupIdProductIdListDictionary.ContainsKey(displayGroupId) == false)
                {
                    continue;
                }
                productIdList.UnionWith(displayGroupIdProductIdListDictionary[displayGroupId]);
            }
            foreach (var productId in productIdList)
            {
                result.Add(productIdProductsContainerDTODictionary[productId]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
