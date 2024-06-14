
/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - Holds the product menu values for a given siteId, userRoleId, POSMachineId, menuType and languageId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      10-Aug-2020      Lakshminarayana           Created : Static menu enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.ProductPrice;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// ProductPriceViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
    /// </summary>
    public class ProductPriceViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ProductPriceContainerSnapshotDTOCollection productPriceContainerSnapshotDTOCollection;
        private readonly int siteId;
        private readonly int posMachineId;
        private readonly int userRoleId;
        private readonly int languageId;
        private readonly string menuType;
        private readonly int membershipId;
        private readonly int transactionProfileId;
        private readonly DateTimeRange dateTimeRange;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="userRoleId">user role primary key</param>
        /// <param name="posMachineId">POS machine id</param>
        /// <param name="languageId">languageId</param>
        /// <param name="menuType">menuType</param>
        /// <param name="membershipId">membershipId</param>
        /// <param name="transactionProfileId">transactionProfileId</param>
        /// <param name="dateTimeRange">specified date range</param>
        /// <param name="productPriceContainerSnapshotDTOCollection">productPriceContainerSnapshotDTOCollection</param>
        internal ProductPriceViewContainer(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTimeRange dateTimeRange, ProductPriceContainerSnapshotDTOCollection productPriceContainerSnapshotDTOCollection)
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, dateTimeRange, membershipId, transactionProfileId, productPriceContainerSnapshotDTOCollection);
            this.siteId = siteId;
            this.userRoleId = userRoleId;
            this.posMachineId = posMachineId;
            this.languageId = languageId;
            this.menuType = menuType;
            this.membershipId = membershipId;
            this.transactionProfileId = transactionProfileId;
            this.dateTimeRange = dateTimeRange;
            this.productPriceContainerSnapshotDTOCollection = productPriceContainerSnapshotDTOCollection;
            log.LogMethodExit();
        }

        internal ProductPriceViewContainer(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTimeRange dateTimeRange)
            :this(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange, GetProductPriceContainerSnapshotDTOCollection(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange, null, false))
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange);
            log.LogMethodExit();
        }

        private static ProductPriceContainerSnapshotDTOCollection GetProductPriceContainerSnapshotDTOCollection(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTimeRange dateTimeRange, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange, hash, rebuildCache);
            ProductPriceContainerSnapshotDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IProductPriceUseCases productPriceUseCases = ProductPriceUseCaseFactory.GetProductPriceUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<ProductPriceContainerSnapshotDTOCollection> task = productPriceUseCases.GetProductPriceContainerSnapshotDTOCollection(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange.StartDateTime, dateTimeRange.EndDateTime, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving ProductPriceContainerDTOCollection.", ex);
                result = new ProductPriceContainerSnapshotDTOCollection();
            }
            
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the product menu panels at any given point in time
        /// </summary>
        /// <param name="dateTime">date time value</param>
        /// <returns></returns>
        public List<ProductsPriceContainerDTO> GetProductsPriceContainerDTOList(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            List<ProductsPriceContainerDTO> result = new List<ProductsPriceContainerDTO>();
            if(productPriceContainerSnapshotDTOCollection == null || 
                productPriceContainerSnapshotDTOCollection.ProductPriceContainerSnapshotDTOList == null ||
                productPriceContainerSnapshotDTOCollection.ProductPriceContainerSnapshotDTOList.Any() == false)
            {
                log.LogMethodExit(result, "productPriceContainerSnapshotDTOCollection is empty");
                return result;
            }
            foreach (var productPriceContainerSnapshotDTO in productPriceContainerSnapshotDTOCollection.ProductPriceContainerSnapshotDTOList)
            {
                if(dateTime >= productPriceContainerSnapshotDTO.StartDateTime && dateTime < productPriceContainerSnapshotDTO.EndDateTime)
                {
                    result = productPriceContainerSnapshotDTO.ProductsPriceContainerDTOList;
                    break;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the latest in ProductPriceContainerSnapshotDTOCollection
        /// </summary>
        /// <returns></returns>
        internal ProductPriceContainerSnapshotDTOCollection GetProductPriceContainerSnapshotDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (productPriceContainerSnapshotDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(productPriceContainerSnapshotDTOCollection);
            return productPriceContainerSnapshotDTOCollection;
        }

        internal ProductPriceViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if(LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            ProductPriceContainerSnapshotDTOCollection latestProductPriceContainerSnapshotDTOCollection = GetProductPriceContainerSnapshotDTOCollection(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange, productPriceContainerSnapshotDTOCollection.Hash, rebuildCache);
            if (latestProductPriceContainerSnapshotDTOCollection == null || 
                latestProductPriceContainerSnapshotDTOCollection.ProductPriceContainerSnapshotDTOList== null)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            ProductPriceViewContainer result = new ProductPriceViewContainer(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange, latestProductPriceContainerSnapshotDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
