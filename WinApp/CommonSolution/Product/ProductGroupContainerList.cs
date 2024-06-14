/********************************************************************************************
 * Project Name - Product
 * Description  - ProductGroupContainerList class to get the List of  of values from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.170.0     07-Jul-2023      Lakshminarayana           Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Timers;

namespace Semnox.Parafait.Product
{
    public class ProductGroupContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, ProductGroupContainer> productGroupContainerCache = new Cache<int, ProductGroupContainer>();
        private static Timer refreshTimer;

        static ProductGroupContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = productGroupContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                ProductGroupContainer productGroupContainer;
                if (productGroupContainerCache.TryGetValue(uniqueKey, out productGroupContainer))
                {
                    productGroupContainerCache[uniqueKey] = productGroupContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static ProductGroupContainer GetProductGroupContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductGroupContainer result = productGroupContainerCache.GetOrAdd(siteId, (k) => new ProductGroupContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static ProductGroupContainerDTOCollection GetProductGroupContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductGroupContainer container = GetProductGroupContainer(siteId);
            ProductGroupContainerDTOCollection result = container.GetProductGroupContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            ProductGroupContainer productGroupContainer = GetProductGroupContainer(siteId);
            productGroupContainerCache[siteId] = productGroupContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ProductGroupContainerDTOList based on the site 
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="productGroupId">option value productGroupId</param>
        /// <returns></returns>
        public static List<ProductGroupContainerDTO> GetProductGroupContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductGroupContainer productGroupContainer = GetProductGroupContainer(siteId);
            var result = productGroupContainer.GetProductGroupContainerDTOList();
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the ProductGroupContainerDTO based on the site and productGroupId
        /// </summary>  
        /// <param name="siteId">site Id</param>
        /// <param name="productGroupId">option value productGroupId</param>
        /// <returns></returns>
        public static ProductGroupContainerDTO GetProductGroupContainerDTO(int siteId, int productGroupId)
        {
            log.LogMethodEntry(siteId, productGroupId);
            ProductGroupContainer productGroupContainer = GetProductGroupContainer(siteId);
            ProductGroupContainerDTO result = productGroupContainer.GetProductGroupContainerDTO(productGroupId);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the ProductGroupContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="productGroupId">productGroupId</param>
        /// <returns></returns>
        public static ProductGroupContainerDTO GetProductGroupContainerDTO(ExecutionContext executionContext, int productGroupId)
        {
            log.LogMethodEntry(executionContext, productGroupId);
            ProductGroupContainerDTO productGroupContainerDTO = GetProductGroupContainerDTO(executionContext.GetSiteId(), productGroupId);
            log.LogMethodExit(productGroupContainerDTO);
            return productGroupContainerDTO;
        }

        /// <summary>
        /// Returns the ProductGroupContainerDTO based on the site and productGroupId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="productGroupId">option value productGroupId</param>
        /// <returns></returns>
        public static ProductGroupContainerDTO GetProductGroupContainerDTOOrDefault(ExecutionContext executionContext, int productGroupId)
        {
            log.LogMethodEntry(executionContext, productGroupId);
            ProductGroupContainer productGroupContainer = GetProductGroupContainer(executionContext.SiteId);
            ProductGroupContainerDTO result = productGroupContainer.GetProductGroupContainerDTOOrDefault(productGroupId);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the ProductGroupContainerDTO based on the site and productGroupGuid
        /// </summary>  
        /// <param name="siteId">site Id</param>
        /// <param name="productGroupGuid">option value productGroupGuid</param>
        /// <returns></returns>
        public static ProductGroupContainerDTO GetProductGroupContainerDTO(int siteId, string productGroupGuid)
        {
            log.LogMethodEntry(siteId, productGroupGuid);
            ProductGroupContainer productGroupContainer = GetProductGroupContainer(siteId);
            ProductGroupContainerDTO result = productGroupContainer.GetProductGroupContainerDTO(productGroupGuid);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the ProductGroupContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="productGroupGuid">productGroupGuid</param>
        /// <returns></returns>
        public static ProductGroupContainerDTO GetProductGroupContainerDTO(ExecutionContext executionContext, string productGroupGuid)
        {
            log.LogMethodEntry(executionContext, productGroupGuid);
            ProductGroupContainerDTO productGroupContainerDTO = GetProductGroupContainerDTO(executionContext.GetSiteId(), productGroupGuid);
            log.LogMethodExit(productGroupContainerDTO);
            return productGroupContainerDTO;
        }

        /// <summary>
        /// Returns the ProductGroupContainerDTO based on the site and productGroupGuid
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="productGroupGuid">option value productGroupGuid</param>
        /// <returns></returns>
        public static ProductGroupContainerDTO GetProductGroupContainerDTOOrDefault(ExecutionContext executionContext, string productGroupGuid)
        {
            log.LogMethodEntry(executionContext, productGroupGuid);
            ProductGroupContainer productGroupContainer = GetProductGroupContainer(executionContext.SiteId);
            ProductGroupContainerDTO result = productGroupContainer.GetProductGroupContainerDTOOrDefault(productGroupGuid);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the referred productId hash set based on the site and productGroupId
        /// </summary>  
        /// <param name="siteId">site Id</param>
        /// <param name="productGroupId">option value productGroupId</param>
        /// <returns></returns>
        public static HashSet<int> GetRefferedProductIdHashSet(int siteId, int productGroupId)
        {
            log.LogMethodEntry(siteId, productGroupId);
            ProductGroupContainer productGroupContainer = GetProductGroupContainer(siteId);
            var result = productGroupContainer.GetRefferedProductIdHashSet(productGroupId);
            log.LogMethodExit();
            return result;
        }

    }
}
