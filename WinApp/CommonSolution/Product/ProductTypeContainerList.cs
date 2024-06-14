/********************************************************************************************
 * Project Name - Product
 * Description  - ProductTypesContainerList class to get the List of  of values from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.120.1     24-Jun-2021       Abhishek         Created: POS Redesign
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Semnox.Parafait.Product
{
    public class ProductTypeContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, ProductTypeContainer> productTypeContainerCache = new Cache<int, ProductTypeContainer>();
        private static Timer refreshTimer;

        static ProductTypeContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = productTypeContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                ProductTypeContainer productTypeContainer;
                if (productTypeContainerCache.TryGetValue(uniqueKey, out productTypeContainer))
                {
                    productTypeContainerCache[uniqueKey] = productTypeContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static ProductTypeContainer GetProductTypeContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductTypeContainer result = productTypeContainerCache.GetOrAdd(siteId, (k) => new ProductTypeContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static ProductTypeContainerDTOCollection GetProductTypeContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductTypeContainer container = GetProductTypeContainer(siteId);
            ProductTypeContainerDTOCollection result = container.GetProductTypeContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            ProductTypeContainer productTypeContainer = GetProductTypeContainer(siteId);
            productTypeContainerCache[siteId] = productTypeContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ProductTypeContainerDTO based on the site and productTypeId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="productTypeId">option value productTypeId</param>
        /// <returns></returns>
        public static ProductTypeContainerDTO GetProductTypeContainerDTO(int siteId, int productTypeId)
        {
            log.LogMethodEntry(siteId, productTypeId);
            ProductTypeContainer productTypeContainer = GetProductTypeContainer(siteId);
            ProductTypeContainerDTO result = productTypeContainer.GetProductTypeContainerDTO(productTypeId);
            log.LogMethodExit();
            return result;
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
    }
}
