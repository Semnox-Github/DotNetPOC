/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - Holds the Product container object
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By             Remarks
 *********************************************************************************************
 2.110.0         07-Dec-2020       Deeksha                 Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Holds the Product container object
    /// </summary>
    public class ProductViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<string, ProductViewContainer> productsViewContainerCache = new Cache<string, ProductViewContainer>();
        private static Timer refreshTimer;

        static ProductViewContainerList()
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
            var uniqueKeyList = productsViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                ProductViewContainer productsViewContainer;
                if (productsViewContainerCache.TryGetValue(uniqueKey, out productsViewContainer))
                {
                    productsViewContainerCache[uniqueKey] = productsViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static ProductViewContainer GetProductsViewContainer(int siteId, string manualProductType)
        {
            log.LogMethodEntry(siteId);
            string uniqueKey = GetUniqueKey(siteId,  manualProductType);
            ProductViewContainer result = productsViewContainerCache.GetOrAdd(uniqueKey, (k)=>new ProductViewContainer(siteId,  manualProductType));
            log.LogMethodExit(result);
            return result;
        }

        private static string GetUniqueKey(int siteId,  string manualProductType)
        {
            log.LogMethodEntry(siteId,  manualProductType);
            string uniqueKey = "SiteId:" + siteId + "ManualProductType:" + manualProductType;
            log.LogMethodExit(uniqueKey);
            return uniqueKey;
        }

        /// <summary>
        /// Returns the ProductsContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="manualProductType"></param>
        /// <param name="hash">hash</param>
        /// <returns></returns>
        public static ProductsContainerDTOCollection GetProductsContainerDTOCollection(int siteId, 
                                                string manualProductType, string hash)
        {
            log.LogMethodEntry(siteId, hash);
            ProductViewContainer productsViewContainer = GetProductsViewContainer(siteId,  manualProductType);
            ProductsContainerDTOCollection result = productsViewContainer.GetProductContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the ProductCalendarContainerDTOCollection
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="manualProductType">manualProductType</param>
        /// <param name="startDateTime">startDateTime</param>
        /// <param name="endDateTime">endDateTime</param>
        /// <param name="hash">hash</param>
        /// <returns></returns>
        public static ProductCalendarContainerDTOCollection GetProductCalendarContainerDTOCollection(int siteId, string manualProductType, DateTime startDateTime, DateTime endDateTime, string hash)
        {
            log.LogMethodEntry(siteId);
            ProductViewContainer productsViewContainer = GetProductsViewContainer(siteId, manualProductType);
            var result = productsViewContainer.GetProductCalendarContainerDTOCollection(new DateTimeRange(startDateTime, endDateTime), hash);
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Returns the active ProductsContainerDTOList for a given context and product type
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="manualProductType"></param>
        /// <returns></returns>
        public static List<ProductsContainerDTO> GetActiveProductsContainerDTOList(ExecutionContext executionContext, 
                                                        string manualProductType)
        {
            log.LogMethodEntry(executionContext);
            ProductViewContainer productViewContainer = GetProductsViewContainer(executionContext.SiteId,  manualProductType);
            List<ProductsContainerDTO> result = productViewContainer.GetActiveProductContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the active ProductsContainerDTOList for a given context and product type
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="manualProductType"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static ProductsContainerDTO GetProductsContainerDTO(ExecutionContext executionContext,
                                                        string manualProductType, int productId)
        {
            log.LogMethodEntry(executionContext);
            ProductViewContainer productViewContainer = GetProductsViewContainer(executionContext.SiteId, manualProductType);
            ProductsContainerDTO result = productViewContainer.GetProductsContainerDTO(productId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the active ProductsContainerDTOList for a given context and product type
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="manualProductType"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static ProductsContainerDTO GetProductsContainerDTOOrDefault(ExecutionContext executionContext,
                                                        string manualProductType, int productId)
        {
            log.LogMethodEntry(executionContext);
            ProductViewContainer productViewContainer = GetProductsViewContainer(executionContext.SiteId, manualProductType);
            ProductsContainerDTO result = productViewContainer.GetProductsContainerDTOOrDefault(productId);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the active ProductsContainerDTOList for a given context and barcode
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="manualProductType"></param>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public static ProductsContainerDTO GetProductsContainerDTOByBarCode(ExecutionContext executionContext,
                                                        string manualProductType, string barCode)
        {
            log.LogMethodEntry(executionContext);
            ProductViewContainer productViewContainer = GetProductsViewContainer(executionContext.SiteId, manualProductType);
            ProductsContainerDTO result = productViewContainer.GetProductsContainerDTObyBarCode(barCode);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns the product container dto matching the guid if exists or null
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="manualProductType"></param>
        /// <param name="productGuid"></param>
        /// <returns></returns>
        public static ProductsContainerDTO GetProductsContainerDTOOrDefault(ExecutionContext executionContext, string manualProductType, string productGuid)
        {
            log.LogMethodEntry(executionContext);
            ProductViewContainer productViewContainer = GetProductsViewContainer(executionContext.SiteId, manualProductType);
            ProductsContainerDTO result = productViewContainer.GetProductsContainerDTOOrDefault(productGuid);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the in active ProductsContainerDTOList for a given context and product type
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="manualProductType"></param>
        /// <returns></returns>
        public static List<ProductsContainerDTO> GetInActiveProductContainerDTOList(ExecutionContext executionContext, 
                                                        string manualProductType)
        {
            log.LogMethodEntry(executionContext);
            ProductViewContainer productViewContainer = GetProductsViewContainer(executionContext.SiteId,  manualProductType);
            List<ProductsContainerDTO> result = productViewContainer.GetInActiveProductContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Returns the specified system product container DTO
        /// </summary>
        /// <param name="executionContext">current execution context</param>
        /// <param name="manualProductType">manual product type</param>
        /// <param name="systemProductType">system product type</param>
        /// <returns></returns>
        public static ProductsContainerDTO GetSystemProductContainerDTO(ExecutionContext executionContext, 
                        string manualProductType, string systemProductType)
        {
            return GetSystemProductContainerDTO(executionContext.SiteId, manualProductType, systemProductType);
        }

        /// <summary>
        /// Returns the specified system product container DTO
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="manualProductType">manual product type</param>
        /// <param name="systemProductType">system product type</param>
        /// <param name="productName">specify if there are multiple system products</param>
        /// <returns></returns>
        public static ProductsContainerDTO GetSystemProductContainerDTO(int siteId,
                        string manualProductType, string systemProductType, string productName = null)
        {
            log.LogMethodEntry(siteId, manualProductType, systemProductType, productName);
            ProductViewContainer productViewContainer = GetProductsViewContainer(siteId, manualProductType);
            var result = productViewContainer.GetSystemProductContainerDTO(systemProductType, productName);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId,  string manualProductType)
        {
            log.LogMethodEntry();

            ProductViewContainer productsViewContainer = GetProductsViewContainer(siteId,  manualProductType);
            string uniqueKey = GetUniqueKey(siteId,  manualProductType);
            productsViewContainerCache[uniqueKey] = productsViewContainer.Refresh(true);
            log.LogMethodExit();
        }


        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(ExecutionContext executionContext, string manualProductType)
        {
            log.LogMethodEntry();
            Rebuild(executionContext.GetSiteId(), manualProductType);
            log.LogMethodExit();
        }

    }
}
