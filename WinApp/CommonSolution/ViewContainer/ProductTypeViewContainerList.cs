/********************************************************************************************
 * Project Name - View Container
 * Description  - ProductTypeViewContainerList holds values based on siteId, userId and POSMachineId
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.1      24-Jun-2021      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// ProductTypeViewContainerList holds multiple  LanguageView containers based on siteId, userId and POSMachineId
    /// </summary>
    public class ProductTypeViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, ProductTypeViewContainer> productTypeViewContainerCache = new Cache<int, ProductTypeViewContainer>();
        private static Timer refreshTimer;

        static ProductTypeViewContainerList()
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
            var uniqueKeyList = productTypeViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                ProductTypeViewContainer productTypeViewContainer;
                if (productTypeViewContainerCache.TryGetValue(uniqueKey, out productTypeViewContainer))
                {
                    productTypeViewContainerCache[uniqueKey] = productTypeViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        

        private static ProductTypeViewContainer GetProductTypeViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = productTypeViewContainerCache.GetOrAdd(siteId, (k)=> new ProductTypeViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the ProductTypeContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// <returns></returns>
        public static ProductTypeContainerDTOCollection GetProductTypeContainerDTOCollection(int siteId, string hash)
        {
            log.LogMethodEntry(siteId, hash);
            ProductTypeViewContainer productTypeViewContainer = GetProductTypeViewContainer(siteId);
            ProductTypeContainerDTOCollection result = productTypeViewContainer.GetProductTypeContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductTypeViewContainer productTypeViewContainer = GetProductTypeViewContainer(siteId);
            productTypeViewContainerCache[siteId] = productTypeViewContainer.Refresh(true);
            log.LogMethodExit();
        }

        

        /// <summary>
        /// Returns the ProductTypeContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="productTypeId">productTypeId</param>
        /// <returns></returns>
        public static ProductTypeContainerDTO GetProductTypeContainerDTO(ExecutionContext executionContext, int productTypeId)
        {
            log.LogMethodEntry(executionContext, productTypeId);
            ProductTypeContainerDTO productTypeContainerDTO = GetProductTypeContainerDTO(executionContext.GetSiteId(), productTypeId);
            log.LogMethodExit(productTypeContainerDTO);
            return productTypeContainerDTO;
        }

        /// <summary>
        /// Returns the ProductTypeContainerDTO based on the siteId and productTypeId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="productTypeId">productTypeId</param>
        /// <returns></returns>
        public static ProductTypeContainerDTO GetProductTypeContainerDTO(int siteId, int productTypeId)
        {
            log.LogMethodEntry(siteId, productTypeId);
            ProductTypeViewContainer productTypeViewContainer = GetProductTypeViewContainer(siteId);
            ProductTypeContainerDTO result = productTypeViewContainer.GetProductTypeContainerDTO(productTypeId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the all the ProductTypeContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static List<ProductTypeContainerDTO> GetProductTypeContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            List<ProductTypeContainerDTO> productTypeContainerDTOList = GetProductTypeContainerDTOList(executionContext.SiteId);
            log.LogMethodExit(productTypeContainerDTOList);
            return productTypeContainerDTOList;
        }

        /// <summary>
        /// Returns the ProductTypeContainerDTOList based on the siteId 
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <returns></returns>
        public static List<ProductTypeContainerDTO> GetProductTypeContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductTypeViewContainer productTypeViewContainer = GetProductTypeViewContainer(siteId);
            List<ProductTypeContainerDTO> result = productTypeViewContainer.GetProductTypeContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
    }
}
