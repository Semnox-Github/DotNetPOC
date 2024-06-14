/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - ProductCalendarViewContainer holds the product availability given siteId, productId and a dataRange
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      04-Aug-2021      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// ProductCalendarViewContainer holds the Products values for a given siteId
    /// </summary>
    public class ProductCalendarViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<int, ProductCalendarContainerDTO> productIdProductCalendarDTODictionary = new Dictionary<int, ProductCalendarContainerDTO>();
        private ProductCalendarContainerDTOCollection productCalendarContainerDTOCollection;
        private int siteId;
        private DateTimeRange dateTimeRange;
        private string manualProductType;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="manualProductType">manualProductType</param>
        /// <param name="dateTimeRange">dateTimeRange</param>
        /// <param name="ProductCalendarDTOCollection">ProductCalendarDTOCollection</param>
        public ProductCalendarViewContainer(int siteId, string manualProductType, DateTimeRange dateTimeRange, ProductCalendarContainerDTOCollection ProductCalendarDTOCollection)
        {
            log.LogMethodEntry( ProductCalendarDTOCollection);
            this.siteId = siteId;
            this.dateTimeRange = dateTimeRange;
            this.manualProductType = manualProductType;
            this.productCalendarContainerDTOCollection = ProductCalendarDTOCollection;
            if (ProductCalendarDTOCollection != null &&
                ProductCalendarDTOCollection.ProductCalendarContainerDTOList != null &&
                ProductCalendarDTOCollection.ProductCalendarContainerDTOList.Any())
            {
                foreach (var productCalendarContainerDTO in ProductCalendarDTOCollection.ProductCalendarContainerDTOList)
                {
                    if (productIdProductCalendarDTODictionary.ContainsKey(productCalendarContainerDTO.ProductId))
                    {
                        continue;
                    }
                    productIdProductCalendarDTODictionary.Add(productCalendarContainerDTO.ProductId, productCalendarContainerDTO);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="manualProductType">manualProductType</param>
        /// <param name="dateTimeRange">dateTimeRange</param>
        public ProductCalendarViewContainer(int siteId, string manualProductType, DateTimeRange dateTimeRange)
            : this(siteId, manualProductType, dateTimeRange, GetProductCalendarDTOCollection(siteId, manualProductType, dateTimeRange, null, false))
        {
            log.LogMethodEntry(siteId, manualProductType, dateTimeRange);
            log.LogMethodExit();
        }

        private static ProductCalendarContainerDTOCollection GetProductCalendarDTOCollection(int siteId, string manualProductType, DateTimeRange dateTimeRange, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, manualProductType, dateTimeRange, hash, rebuildCache);
            ProductCalendarContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IProductsUseCases productUseCases = ProductsUseCaseFactory.GetProductUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<ProductCalendarContainerDTOCollection> task = productUseCases.GetProductCalendarContainerDTOCollection(siteId, manualProductType, dateTimeRange.StartDateTime, dateTimeRange.EndDateTime, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving ProductCalendarContainerDTOCollection.", ex);
                result = new ProductCalendarContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Check whether the product is available on specific dateTime
        /// </summary>
        /// <param name="productId">productId</param>
        /// <param name="dateTime">dateTime</param>
        /// <returns></returns>
        public bool IsProductAvailable(int productId, DateTime dateTime)
        {
            if (productIdProductCalendarDTODictionary.ContainsKey(productId) == false)
            {
                string message = MessageViewContainerList.GetMessage(siteId, -1, 2196, "Product", productId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            ProductCalendarContainerDTO ProductCalendarDTO = productIdProductCalendarDTODictionary[productId];
            foreach (var productCalendarDetailDTO in ProductCalendarDTO.ProductCalendarDetailContainerDTOList)
            {
                if (dateTime >= productCalendarDetailDTO.StartDateTime && dateTime < productCalendarDetailDTO.EndDateTime)
                {
                    log.LogMethodExit(productCalendarDetailDTO.Available);
                    return productCalendarDetailDTO.Available;
                }
            }
            log.LogMethodExit(false, "Unable to find ProductCalendarDetailDTO");
            return false;
        }

        /// <summary>
        /// returns the latest in ProductCalendarDTOCollection
        /// </summary>
        ///  <param name="hash">hash</param>
        /// <returns></returns>
        public ProductCalendarContainerDTOCollection GetProductCalendarContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (productCalendarContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(productCalendarContainerDTOCollection);
            return productCalendarContainerDTOCollection;
        }

        internal ProductCalendarViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            ProductCalendarContainerDTOCollection latestProductCalendarDTOCollection = GetProductCalendarDTOCollection(siteId, manualProductType, dateTimeRange, productCalendarContainerDTOCollection.Hash, rebuildCache);
            if (latestProductCalendarDTOCollection == null ||
                latestProductCalendarDTOCollection.ProductCalendarContainerDTOList == null ||
                latestProductCalendarDTOCollection.ProductCalendarContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            ProductCalendarViewContainer result = new ProductCalendarViewContainer(siteId, manualProductType, dateTimeRange, latestProductCalendarDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
