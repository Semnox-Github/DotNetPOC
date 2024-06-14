/********************************************************************************************
 * Project Name - Product
 * Description  - ProductDisplayGroupFormat class to get the List of  of values from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0     25-Jun-2021      Abhishek                  Created: POS Redesign
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
    public class ProductDisplayGroupFormatContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, ProductDisplayGroupFormatContainer> productDisplayGroupFormatContainerCache = new Cache<int, ProductDisplayGroupFormatContainer>();
        private static Timer refreshTimer;

        static ProductDisplayGroupFormatContainerList()
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
            var uniqueKeyList = productDisplayGroupFormatContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                ProductDisplayGroupFormatContainer productDisplayGroupFormatContainer;
                if (productDisplayGroupFormatContainerCache.TryGetValue(uniqueKey, out productDisplayGroupFormatContainer))
                {
                    productDisplayGroupFormatContainerCache[uniqueKey] = productDisplayGroupFormatContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static ProductDisplayGroupFormatContainer GetProductDisplayGroupFormatContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductDisplayGroupFormatContainer result = productDisplayGroupFormatContainerCache.GetOrAdd(siteId, (k) => new ProductDisplayGroupFormatContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static ProductDisplayGroupFormatContainerDTOCollection GetProductDisplayGroupFormatContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductDisplayGroupFormatContainer container = GetProductDisplayGroupFormatContainer(siteId);
            ProductDisplayGroupFormatContainerDTOCollection result = container.GetProductDisplayGroupFormatContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            ProductDisplayGroupFormatContainer productDisplayGroupFormatContainer = GetProductDisplayGroupFormatContainer(siteId);
            productDisplayGroupFormatContainerCache[siteId] = productDisplayGroupFormatContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ProductDisplayGroupFormatContainerDTO based on the site and Id
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static ProductDisplayGroupFormatContainerDTO GetProductDisplayGroupFormatContainerDTO(int siteId, int id)
        {
            log.LogMethodEntry(siteId, id);
            ProductDisplayGroupFormatContainer productDisplayGroupFormatContainer = GetProductDisplayGroupFormatContainer(siteId);
            ProductDisplayGroupFormatContainerDTO result = productDisplayGroupFormatContainer.GetProductDisplayGroupFormatContainerDTO(id);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the ProductDisplayGroupFormatContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static ProductDisplayGroupFormatContainerDTO GetProductDisplayGroupFormatContainerDTO(ExecutionContext executionContext, int id)
        {
            log.LogMethodEntry(executionContext, id);
            ProductDisplayGroupFormatContainerDTO productDisplayGroupFormatContainerDTO = GetProductDisplayGroupFormatContainerDTO(executionContext.GetSiteId(), id);
            log.LogMethodExit(productDisplayGroupFormatContainerDTO);
            return productDisplayGroupFormatContainerDTO;
        }
    }
}
