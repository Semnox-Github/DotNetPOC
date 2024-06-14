/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - ProductViewContainer class
 *
 **************
 ** Version Log
  **************
  * Version      Date               Modified By             Remarks
 *********************************************************************************************
 2.110.0         01-Dec-2020        Deeksha                 Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.ViewContainer
{
    // <summary>
    // Holds the list of products
    // </summary>
    public class ProductViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ProductsContainerDTOCollection productContainerDTOCollection;
        private readonly List<ProductsContainerDTO> activeProductsContainerDTOList = new List<ProductsContainerDTO>();
        private readonly List<ProductsContainerDTO> inactiveProductsContainerDTOList = new List<ProductsContainerDTO>();
        private readonly ConcurrentDictionary<int, ProductsContainerDTO> productIdProductsContainerDTODictionary = new ConcurrentDictionary<int, ProductsContainerDTO>();
        private readonly ConcurrentDictionary<string, ProductsContainerDTO> barCodeProductsContainerDTODictionary = new ConcurrentDictionary<string, ProductsContainerDTO>(StringComparer.InvariantCultureIgnoreCase);
        private readonly ConcurrentDictionary<string, ProductsContainerDTO> productGuidProductsContainerDTODictionary = new ConcurrentDictionary<string, ProductsContainerDTO>();
        private readonly int siteId;
        private readonly string manualProductType;
        private readonly Dictionary<string, List<ProductsContainerDTO>> systemProductsDTOListDictionary = new Dictionary<string, List<ProductsContainerDTO>>();
        private Cache<DateTimeRange, ProductCalendarViewContainer> productCalendarViewContainerCache = new Cache<DateTimeRange, ProductCalendarViewContainer>();

        public ProductViewContainer(int siteId,  string manualProductType, 
                                ProductsContainerDTOCollection productContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, productContainerDTOCollection);
            this.siteId = siteId;
            this.manualProductType = manualProductType;
            this.productContainerDTOCollection = productContainerDTOCollection;
            if (productContainerDTOCollection != null &&
                productContainerDTOCollection.ProductContainerDTOList != null &&
                productContainerDTOCollection.ProductContainerDTOList.Any())
            {
                foreach (var productContainerDTO in productContainerDTOCollection.ProductContainerDTOList)
                {
                    if(productContainerDTO.IsSystemProduct == false)
                    {
                        productIdProductsContainerDTODictionary[productContainerDTO.ProductId] = productContainerDTO;
                        productGuidProductsContainerDTODictionary[productContainerDTO.Guid] = productContainerDTO;
                        if (productContainerDTO.IsActive)
                        {
                            activeProductsContainerDTOList.Add(productContainerDTO);
                            if (productContainerDTO!=null && productContainerDTO.InventoryItemContainerDTO !=null && productContainerDTO.InventoryItemContainerDTO.ProductBarcodeContainerDTOList != null && productContainerDTO.InventoryItemContainerDTO.ProductBarcodeContainerDTOList.Any())
                            {
                                foreach (ProductBarcodeContainerDTO productBarcodeContainerDTO in productContainerDTO.InventoryItemContainerDTO.ProductBarcodeContainerDTOList)
                                {
                                    if (barCodeProductsContainerDTODictionary.ContainsKey(productBarcodeContainerDTO.BarCode) == false)
                                    {
                                        barCodeProductsContainerDTODictionary[productBarcodeContainerDTO.BarCode] = productContainerDTO;
                                    }
                                }
                            }
                        }
                        else
                        {
                            inactiveProductsContainerDTOList.Add(productContainerDTO);
                        }
                    }
                    else
                    {
                        if(systemProductsDTOListDictionary.ContainsKey(productContainerDTO.ProductType) == false)
                        {
                            systemProductsDTOListDictionary.Add(productContainerDTO.ProductType, new List<ProductsContainerDTO>());
                        }
                        systemProductsDTOListDictionary[productContainerDTO.ProductType].Add(productContainerDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        public ProductViewContainer(int siteId,  string manualProductType)
            : this(siteId,  manualProductType, GetProductContainerDTOCollection(siteId,  manualProductType, null, false))
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private static ProductsContainerDTOCollection GetProductContainerDTOCollection(int siteId, string manualProductType, 
            string hash, bool rebuildCache)
        {
            log.LogMethodEntry();
            ProductsContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IProductsUseCases productUseCases = ProductsUseCaseFactory.GetProductUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<ProductsContainerDTOCollection> task = productUseCases.GetProductsContainerDTOCollection(siteId,  manualProductType, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving ProductsContainerDTOCollection.", ex);
                result = new ProductsContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }

        public List<ProductsContainerDTO> GetActiveProductContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(activeProductsContainerDTOList);
            return activeProductsContainerDTOList;
        }

        public List<ProductsContainerDTO> GetInActiveProductContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(inactiveProductsContainerDTOList);
            return inactiveProductsContainerDTOList;
        }

        /// <summary>
        /// returns the latest in ProductViewDTOCollection
        /// </summary>
        /// <returns></returns>
        public ProductsContainerDTOCollection GetProductContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (productContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(productContainerDTOCollection);
            return productContainerDTOCollection;
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
            if(string.IsNullOrWhiteSpace(productName))
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

        internal ProductsContainerDTO GetProductsContainerDTOOrDefault(int productId)
        {
            log.LogMethodEntry(productId);
            if (productIdProductsContainerDTODictionary.ContainsKey(productId) == false)
            {
                log.LogMethodExit(null, "Products with productId : " + productId + " doesn't exist.");
                return null;
            }
            var result = productIdProductsContainerDTODictionary[productId];
            return result;
        }

        internal ProductsContainerDTO GetProductsContainerDTOOrDefault(string productGuid)
        {
            log.LogMethodEntry(productGuid);
            if (productGuidProductsContainerDTODictionary.ContainsKey(productGuid) == false)
            {
                log.LogMethodExit(null, "Products with productGuid : " + productGuid + " doesn't exist.");
                return null;
            }
            var result = productGuidProductsContainerDTODictionary[productGuid];
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
            log.LogMethodExit(result);
            return result;
        }
        public ProductsContainerDTO GetProductsContainerDTObyBarCode(string barCode)
        {
            log.LogMethodEntry(barCode);
            if (barCodeProductsContainerDTODictionary.ContainsKey(barCode) == false)
            {
                string errorMessage = "Products with barcode : " + barCode + " doesn't exist.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = barCodeProductsContainerDTODictionary[barCode];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Whether the product is available on specific datetime
        /// </summary>
        /// <returns></returns>
        public bool IsProductAvailable(int productId, DateTime dateTime)
        {
            log.LogMethodEntry(productId, dateTime);
            DateTimeRange dateTimeRange = GetDateTimeRange(dateTime);
            ProductCalendarViewContainer productCalendarViewContainer = GetProductCalendarViewContainer(dateTimeRange);
            bool returnValue = productCalendarViewContainer.IsProductAvailable(productId, dateTime);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private ProductCalendarViewContainer GetProductCalendarViewContainer(DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(dateTimeRange);
            ProductCalendarViewContainer result = productCalendarViewContainerCache.GetOrAdd(dateTimeRange, (k) => new ProductCalendarViewContainer(siteId, manualProductType, dateTimeRange));
            log.LogMethodExit(result);
            return result;
        }

        private DateTimeRange GetDateTimeRange(DateTime dateTime)
        {
            return new DateTimeRange(dateTime.Date, dateTime.Date.AddDays(1));
        }

        public ProductCalendarContainerDTOCollection GetProductCalendarContainerDTOCollection(DateTimeRange dateTimeRange, string hash)
        {
            log.LogMethodEntry(dateTimeRange, hash);
            ProductCalendarViewContainer productCalendarViewContainer = GetProductCalendarViewContainer(dateTimeRange);
            ProductCalendarContainerDTOCollection returnValue = productCalendarViewContainer.GetProductCalendarContainerDTOCollection(hash);
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        public ProductViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            var dateTimeRangeList = productCalendarViewContainerCache.Keys;
            foreach (var dateTimeRange in dateTimeRangeList)
            {
                ProductCalendarViewContainer productCalendarViewContainer;
                if (dateTimeRange.EndDateTime < DateTime.Now)
                {
                    if (productCalendarViewContainerCache.TryRemove(dateTimeRange, out productCalendarViewContainer))
                    {
                        log.Debug("Removing ProductCalendarViewContainer of date range" + dateTimeRange);
                    }
                    else
                    {
                        log.Debug("Unable to remove ProductCalendarViewContainer of date range" + dateTimeRange);
                    }
                }
                else
                {
                    if (productCalendarViewContainerCache.TryGetValue(dateTimeRange, out productCalendarViewContainer))
                    {
                        productCalendarViewContainerCache[dateTimeRange] = productCalendarViewContainer.Refresh(rebuildCache);
                    }
                }
            }
            LastRefreshTime = DateTime.Now;
            ProductsContainerDTOCollection latestProductViewDTOCollection = GetProductContainerDTOCollection(siteId,  manualProductType, productContainerDTOCollection.Hash, rebuildCache);
            if (latestProductViewDTOCollection == null ||
                latestProductViewDTOCollection.ProductContainerDTOList == null ||
                latestProductViewDTOCollection.ProductContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            ProductViewContainer result = new ProductViewContainer(siteId,  manualProductType, latestProductViewDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
