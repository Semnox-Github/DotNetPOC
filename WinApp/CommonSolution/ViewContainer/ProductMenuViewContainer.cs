
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// ProductMenuViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
    /// </summary>
    public class ProductMenuViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ProductMenuContainerSnapshotDTOCollection productMenuContainerSnapshotDTOCollection;
        private readonly int siteId;
        private readonly int userRoleId;
        private readonly int posMachineId;
        private readonly int languageId;
        private readonly string menuType;
        private readonly DateTimeRange dateTimeRange;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="userRoleId">user role primary key</param>
        /// <param name="posMachineId">POS machine id</param>
        /// <param name="languageId">languageId</param>
        /// <param name="menuType">menuType</param>
        /// <param name="dateTimeRange">specified date range</param>
        /// <param name="productMenuContainerSnapshotDTOCollection">productMenuContainerSnapshotDTOCollection</param>
        internal ProductMenuViewContainer(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, DateTimeRange dateTimeRange, ProductMenuContainerSnapshotDTOCollection productMenuContainerSnapshotDTOCollection)
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, dateTimeRange, productMenuContainerSnapshotDTOCollection);
            this.siteId = siteId;
            this.userRoleId = userRoleId;
            this.posMachineId = posMachineId;
            this.languageId = languageId;
            this.menuType = menuType;
            this.dateTimeRange = dateTimeRange;
            this.productMenuContainerSnapshotDTOCollection = productMenuContainerSnapshotDTOCollection;
            log.LogMethodExit();
        }

        internal ProductMenuViewContainer(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, DateTimeRange dateTimeRange)
            :this(siteId, userRoleId, posMachineId, languageId, menuType, dateTimeRange, GetProductMenuContainerSnapshotDTOCollection(siteId, userRoleId, posMachineId, languageId, menuType, dateTimeRange, null, false))
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, dateTimeRange);
            log.LogMethodExit();
        }

        private static ProductMenuContainerSnapshotDTOCollection GetProductMenuContainerSnapshotDTOCollection(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, DateTimeRange dateTimeRange, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, dateTimeRange);
            ProductMenuContainerSnapshotDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<ProductMenuContainerSnapshotDTOCollection> task = productMenuUseCases.GetProductMenuContainerSnapshotDTOCollection(siteId, userRoleId, posMachineId, languageId, menuType, dateTimeRange.StartDateTime, dateTimeRange.EndDateTime, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving ProductMenuContainerDTOCollection.", ex);
                result = new ProductMenuContainerSnapshotDTOCollection();
            }
            
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the product menu panels at any given point in time
        /// </summary>
        /// <param name="dateTime">date time value</param>
        /// <returns></returns>
        public List<ProductMenuPanelContainerDTO> GetProductMenuPanelContainerDTOList(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            List<ProductMenuPanelContainerDTO> result = new List<ProductMenuPanelContainerDTO>();
            if(productMenuContainerSnapshotDTOCollection == null || 
                productMenuContainerSnapshotDTOCollection.ProductMenuContainerSnapshotDTOList == null ||
                productMenuContainerSnapshotDTOCollection.ProductMenuContainerSnapshotDTOList.Any() == false)
            {
                log.LogMethodExit(result, "productMenuContainerSnapshotDTOCollection is empty");
                return result;
            }
            foreach (var productMenuContainerSnapshotDTO in productMenuContainerSnapshotDTOCollection.ProductMenuContainerSnapshotDTOList)
            {
                if(dateTime >= productMenuContainerSnapshotDTO.StartDateTime && dateTime < productMenuContainerSnapshotDTO.EndDateTime)
                {
                    result = productMenuContainerSnapshotDTO.ProductMenuPanelContainerDTOList;
                    break;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the latest in ProductMenuContainerSnapshotDTOCollection
        /// </summary>
        /// <returns></returns>
        internal ProductMenuContainerSnapshotDTOCollection GetProductMenuContainerSnapshotDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (productMenuContainerSnapshotDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(productMenuContainerSnapshotDTOCollection);
            return productMenuContainerSnapshotDTOCollection;
        }

        internal ProductMenuViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if(LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            ProductMenuContainerSnapshotDTOCollection latestProductMenuContainerSnapshotDTOCollection = GetProductMenuContainerSnapshotDTOCollection(siteId, userRoleId, posMachineId, languageId, menuType, dateTimeRange, productMenuContainerSnapshotDTOCollection.Hash, rebuildCache);
            if (latestProductMenuContainerSnapshotDTOCollection == null || 
                latestProductMenuContainerSnapshotDTOCollection.ProductMenuContainerSnapshotDTOList== null)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            ProductMenuViewContainer result = new ProductMenuViewContainer(siteId, userRoleId, posMachineId, languageId, menuType, dateTimeRange, latestProductMenuContainerSnapshotDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
