
/********************************************************************************************
 * Project Name - View Container
 * Description  - ProductTypeViewContainer values for a given siteId, userId and POSMachineId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.1      24-Jun-2021       Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// ProductTypeViewContainer holds values for a given siteId, userId and POSMachineId
    /// </summary>
    public class ProductTypeViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ProductTypeContainerDTOCollection productTypeContainerDTOCollection;
        private readonly ConcurrentDictionary<int, ProductTypeContainerDTO> productTypeContainerDTODictionary = new ConcurrentDictionary<int, ProductTypeContainerDTO>();
        private readonly int siteId;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="productTypeContainerDTOCollection">productTypeContainerDTOCollection</param>
        internal ProductTypeViewContainer(int siteId, ProductTypeContainerDTOCollection productTypeContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, productTypeContainerDTOCollection);
            this.siteId = siteId;
            this.productTypeContainerDTOCollection = productTypeContainerDTOCollection;
            if (productTypeContainerDTOCollection != null &&
                productTypeContainerDTOCollection.ProductTypeContainerDTOList != null &&
                productTypeContainerDTOCollection.ProductTypeContainerDTOList.Any())
            {
                foreach (var productTypeContainerDTO in productTypeContainerDTOCollection.ProductTypeContainerDTOList)
                {
                    productTypeContainerDTODictionary[productTypeContainerDTO.ProductTypeId] = productTypeContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal ProductTypeViewContainer(int siteId)
            :this(siteId, GetProductTypeContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static ProductTypeContainerDTOCollection GetProductTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            ProductTypeContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IProductTypeUseCases productTypeUseCases = ProductTypeUseCaseFactory.GetProductTypeUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<ProductTypeContainerDTOCollection> task = productTypeUseCases.GetProductTypeContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving ProductTypeContainerDTOCollection.", ex);
                result = new ProductTypeContainerDTOCollection();
            }
            
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the ProductTypeContainerDTO for the productTypeId
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
        public ProductTypeContainerDTO GetProductTypeContainerDTO(int productTypeId)
        {
            log.LogMethodEntry(productTypeId);
            if (productTypeContainerDTODictionary.ContainsKey(productTypeId) == false)
            {
                string errorMessage = "Product Type with product type Id :" + productTypeId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            ProductTypeContainerDTO result = productTypeContainerDTODictionary[productTypeId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the ProductTypeContainerDTOList
        /// </summary>
        /// <returns></returns>
        public List<ProductTypeContainerDTO> GetProductTypeContainerDTOList()
        {
            log.LogMethodEntry();
            var result = productTypeContainerDTOCollection.ProductTypeContainerDTOList;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the latest in ProductTypeContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal ProductTypeContainerDTOCollection GetProductTypeContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (productTypeContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(productTypeContainerDTOCollection);
            return productTypeContainerDTOCollection;
        }

        internal ProductTypeViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if(LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            ProductTypeContainerDTOCollection latestProductTypeContainerDTOCollection = GetProductTypeContainerDTOCollection(siteId, productTypeContainerDTOCollection.Hash, rebuildCache);
            if (latestProductTypeContainerDTOCollection == null ||
                latestProductTypeContainerDTOCollection.ProductTypeContainerDTOList == null ||
                latestProductTypeContainerDTOCollection.ProductTypeContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            ProductTypeViewContainer result = new ProductTypeViewContainer(siteId, productTypeContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
