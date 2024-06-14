/********************************************************************************************
 * Project Name - Products
 * Description  - ProductMasterList holds the product container
 *
 **************
 ** Version Log
  **************
  * Version     Date            Modified By         Remarks
 *********************************************************************************************
  2.110.0       01-Dec-2020     Deeksha             Created : Web Inventory UI design with REST API
 ********************************************************************************************/
using System.Timers;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Holds the product container object
    /// </summary>
    public class ProductsContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Timer refreshTimer;
        private static readonly Cache<int, ProductsContainer> productsContainerCache = new Cache<int, ProductsContainer>();


        static ProductsContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = productsContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                ProductsContainer productsContainer;
                if (productsContainerCache.TryGetValue(uniqueKey, out productsContainer))
                {
                    productsContainerCache[uniqueKey] = productsContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static ProductsContainer GetProductsContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductsContainer result = productsContainerCache.GetOrAdd(siteId, (k)=> new ProductsContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static ProductsContainerDTOCollection GetProductsContainerDTOCollection(int siteId, string manualProductType)
        {
            log.LogMethodEntry(siteId);
            ProductsContainer container = GetProductsContainer(siteId);
            ProductsContainerDTOCollection result = container.GetProductsContainerDTOCollection(manualProductType);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the ProductCalendarContainerDTOCollection
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="startDateTime">startDateTime</param>
        /// <param name="endDateTime">endDateTime</param>
        /// <returns></returns>
        public static ProductCalendarContainerDTOCollection GetProductCalendarContainerDTOCollection(int siteId, string manualProductType, DateTime startDateTime, DateTime endDateTime)
        {
            log.LogMethodEntry(siteId);
            ProductsContainer container = GetProductsContainer(siteId);
            var result = container.GetProductCalendarContainerDTOCollection(manualProductType, new DateTimeRange(startDateTime, endDateTime));
            log.LogMethodExit(result);
            return result;
        }

        public static List<ProductsContainerDTO> GetActiveProductsContainerDTOList(int siteId, string manualProductType)
        {
            log.LogMethodEntry(siteId);
            ProductsContainer container = GetProductsContainer(siteId);
            var result = container.GetActiveProductsContainerDTOList(manualProductType);
            log.LogMethodExit(result);
            return result;
        }

        public static List<ProductsContainerDTO> GetActiveProductsContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductsContainer container = GetProductsContainer(siteId);
            var result = container.GetActiveProductsContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }

        public static List<ProductsContainerDTO> GetInActiveProductsContainerDTOList(int siteId, string manualProductType)
        {
            log.LogMethodEntry(siteId);
            ProductsContainer container = GetProductsContainer(siteId);
            var result = container.GetInActiveProductsContainerDTOList(manualProductType);
            log.LogMethodExit(result);
            return result;
        }
        
        public static List<ProductsContainerDTO> GetSystemProductsContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductsContainer container = GetProductsContainer(siteId);
            var result = container.GetSystemProductsContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        
        public static IEnumerable<ProductsContainerDTO> GetActiveProductsContainerDTOList(ExecutionContext executionContext, string manualProductType, Func<ProductsContainerDTO, bool> predicate)
        {
            log.LogMethodEntry(executionContext, manualProductType, predicate);
            var result = GetActiveProductsContainerDTOList(executionContext.SiteId, manualProductType, predicate);
            log.LogMethodExit(result);
            return result;
        }
        public static IEnumerable<ProductsContainerDTO> GetActiveProductsContainerDTOList(int siteId, string manualProductType, Func<ProductsContainerDTO, bool> predicate)
        {
            log.LogMethodEntry(siteId);
            ProductsContainer container = GetProductsContainer(siteId);
            var result = container.GetActiveProductsContainerDTOList(manualProductType, predicate);
            log.LogMethodExit(result);
            return result;
        }
        public static ProductsContainerDTO GetProductsContainerDTO(ExecutionContext executionContext, int productId)
        {
            log.LogMethodEntry(executionContext, productId);
            log.LogMethodExit();
            return GetProductsContainerDTO(executionContext.SiteId, productId);
        }

        public static ProductsContainerDTO GetProductsContainerDTOOrDefault(ExecutionContext executionContext, int productId)
        {
            log.LogMethodEntry(executionContext, productId);
            log.LogMethodExit();
            return GetProductsContainerDTOOrDefault(executionContext.SiteId, productId);
        }

        public static ProductsContainerDTO GetProductsContainerDTO(int siteId, int productId)
        {
            log.LogMethodEntry(siteId, productId);
            ProductsContainer container = GetProductsContainer(siteId);
            var result = container.GetProductsContainerDTO(productId);
            log.LogMethodExit(result);
            return result;
        }

        public static ProductsContainerDTO GetProductsContainerDTOOrDefault(int siteId, int productId)
        {
            log.LogMethodEntry(siteId, productId);
            ProductsContainer container = GetProductsContainer(siteId);
            var result = container.GetProductsContainerDTOOrDefault(productId);
            log.LogMethodExit(result);
            return result;
        }

        public static ProductsContainerDTO GetProductsContainerDTO(ExecutionContext executionContext, string productGuid)
        {
            log.LogMethodEntry(executionContext, productGuid);
            log.LogMethodExit();
            return GetProductsContainerDTO(executionContext.SiteId, productGuid);
        }

        public static ProductsContainerDTO GetProductsContainerDTOOrDefault(ExecutionContext executionContext, string productGuid)
        {
            log.LogMethodEntry(executionContext, productGuid);
            log.LogMethodExit();
            return GetProductsContainerDTOOrDefault(executionContext.SiteId, productGuid);
        }

        public static ProductsContainerDTO GetProductsContainerDTO(int siteId, string productGuid)
        {
            log.LogMethodEntry(siteId, productGuid);
            ProductsContainer container = GetProductsContainer(siteId);
            var result = container.GetProductsContainerDTO(productGuid);
            log.LogMethodExit(result);
            return result;
        }

        public static ProductsContainerDTO GetProductsContainerDTOOrDefault(int siteId, string productGuid)
        {
            log.LogMethodEntry(siteId, productGuid);
            ProductsContainer container = GetProductsContainer(siteId);
            var result = container.GetProductsContainerDTOOrDefault(productGuid);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Check whether product is available on date time
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="productId">product id</param>
        /// <param name="dateTime">dateTime</param>
        /// <returns></returns>
        public static bool IsProductAvailable(int siteId, int productId, DateTime dateTime)
        {
            log.LogMethodEntry(siteId, productId, dateTime);
            ProductsContainer productsContainer = GetProductsContainer(siteId);
            bool returnValue = productsContainer.IsProductAvailable(productId, dateTime);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Get the schedule of the product for a give dateTime range
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="productId">product id</param>
        /// <param name="dateTimeRange">dateTimeRange</param>
        /// <returns>productCalendarContainerDTO</returns>
        public static ProductCalendarContainerDTO GetProductCalendarContainerDTO(int siteId, int productId, DateTimeRange dateTimeRange)
        {

            log.LogMethodEntry(siteId, productId, dateTimeRange);
            ProductsContainer productsContainer = GetProductsContainer(siteId);
            var result = productsContainer.GetProductCalendarContainerDTO(productId, dateTimeRange);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Check whether product is available on date time
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="dateTime">dateTime</param>
        /// <returns></returns>
        public static bool IsProductAvailable(ExecutionContext executionContext, int discountId, DateTime dateTime)
        {
            log.LogMethodEntry(executionContext, discountId, dateTime);
            bool returnValue = IsProductAvailable(executionContext.GetSiteId(), discountId, dateTime);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            ProductsContainer productsContainer = GetProductsContainer(siteId);
            productsContainerCache[siteId] = productsContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the specified system product container DTO
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="manualProductType">manual product type</param>
        /// <param name="systemProductType">system product type</param>
        /// <param name="productName">specify if there are multiple system products</param>
        /// <returns></returns>
        public static ProductsContainerDTO GetSystemProductContainerDTO(int siteId, string systemProductType, string productName = null)
        {
            log.LogMethodEntry(siteId, systemProductType, productName);
            ProductsContainer productViewContainer = GetProductsContainer(siteId);
            var result = productViewContainer.GetSystemProductContainerDTO(systemProductType, productName);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the specified system product container DTO
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="manualProductType">manual product type</param>
        /// <param name="systemProductType">system product type</param>
        /// <param name="productName">specify if there are multiple system products</param>
        /// <returns></returns>
        public static ProductsContainerDTO GetSystemProductContainerDTO(int siteId, int productId)
        {
            log.LogMethodEntry(siteId, productId);
            ProductsContainer productViewContainer = GetProductsContainer(siteId);
            var result = productViewContainer.GetSystemProductContainerDTO(productId);
            log.LogMethodExit(result);
            return result;
        }
        // <summary>
        /// Returns the list of product id belonging to the display group id list
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="displayGroupIdList"></param>
        /// <returns></returns>
        public static List<ProductsContainerDTO> GetProductContainerDTOListOfDisplayGroups(int siteId, List<int> displayGroupIdList)
        {
            log.LogMethodEntry(siteId, displayGroupIdList);
            ProductsContainer container = GetProductsContainer(siteId);
            var result = container.GetProductContainerDTOListOfDisplayGroups(displayGroupIdList);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Return the list of products referred by the products
        /// </summary>
        public static List<int> GetReferencedProductIdList(int siteId, List<int> productIdList)
        {
            log.LogMethodEntry(siteId, productIdList);
            ProductsContainer container = GetProductsContainer(siteId);
            var result = container.GetReferencedProductIdList(productIdList);
            log.LogMethodExit(result);
            return result;
        }
        
        public static List<ProductsContainerDTO> GetProductsContainerDTOListOfCategory(int siteId, int categoryId)
        {
            log.LogMethodEntry(siteId, categoryId);
            ProductsContainer container = GetProductsContainer(siteId);
            var result = container.GetProductsContainerDTOListOfCategory(categoryId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
