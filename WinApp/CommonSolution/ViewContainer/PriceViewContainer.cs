/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - PriceViewContainer holds the product availability given siteId, productId and a dataRange
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      04-Aug-2021      Lakshminarayana           Created : price container enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.ProductPrice;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// PriceViewContainer holds the Products values for a given siteId
    /// </summary>
    public class PriceViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private PriceContainerDTOCollection priceContainerDTOCollection;
        private PriceContainerKey key;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="priceContainerDTOCollection">priceContainerDTOCollection</param>
        public PriceViewContainer(PriceContainerKey key, PriceContainerDTOCollection priceContainerDTOCollection)
        {
            log.LogMethodEntry(priceContainerDTOCollection);
            this.key = key;
            this.priceContainerDTOCollection = priceContainerDTOCollection;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="key">key</param>
        public PriceViewContainer(PriceContainerKey key)
            : this(key, GetPriceContainerDTOCollection(key, null, false))
        {
            log.LogMethodEntry(key);
            log.LogMethodExit();
        }

        private static PriceContainerDTOCollection GetPriceContainerDTOCollection(PriceContainerKey key, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(key, hash, rebuildCache);
            PriceContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IProductPriceUseCases productPriceUseCases = ProductPriceUseCaseFactory.GetProductPriceUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<PriceContainerDTOCollection> task = productPriceUseCases.GetPriceContainerDTOCollection(key.SiteId, key.MembershipId, key.UserRoleId, key.TransactionProfileId, key.DateTimeRange.StartDateTime, key.DateTimeRange.EndDateTime, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving PriceContainerDTOCollection.", ex);
                result = new PriceContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the latest in priceContainerDTOCollection
        /// </summary>
        ///  <param name="hash">hash</param>
        /// <returns></returns>
        public PriceContainerDTOCollection GetPriceContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (priceContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(priceContainerDTOCollection);
            return priceContainerDTOCollection;
        }

        /// <summary>
        /// Returns the matching PriceContainerDetailDTO
        /// </summary>
        public PriceContainerDetailDTO GetPriceContainerDetailDTO(int productId, DateTime dateTime)
        {
            log.LogMethodEntry(productId, dateTime);
            PriceContainerDetailDTO result = priceContainerDTOCollection.GetPriceContainerDetailDTO(productId, dateTime);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the matching PriceContainerDTO
        /// </summary>
        public PriceContainerDTO GetPriceContainerDTO(int productId)
        {
            log.LogMethodEntry(productId);
            PriceContainerDTO result = priceContainerDTOCollection.GetPriceContainerDTO(productId);
            log.LogMethodExit(result);
            return result;
        }

        internal PriceViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            PriceContainerDTOCollection latestPriceContainerDTOCollection = GetPriceContainerDTOCollection(key, priceContainerDTOCollection.Hash, rebuildCache);
            if (latestPriceContainerDTOCollection == null ||
                latestPriceContainerDTOCollection.PriceContainerDTOList == null ||
                latestPriceContainerDTOCollection.PriceContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            PriceViewContainer result = new PriceViewContainer(key, latestPriceContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
