/********************************************************************************************
 * Project Name - POS
 * Description  - Holds a list of product menu containers each for a site
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.130.0     10-Aug-2021   Lakshminarayana              Created: Static menu enhancement
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.ProductPrice;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Semnox.Parafait.ProductPrice
{
    public static class ProductPriceContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, ProductPriceContainer> productMenuContainerCache = new Cache<int, ProductPriceContainer>();
        private static Timer refreshTimer;

        static ProductPriceContainerList()
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
            var uniqueKeyList = productMenuContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                ProductPriceContainer productMenuContainer;
                if (productMenuContainerCache.TryGetValue(uniqueKey, out productMenuContainer))
                {
                    productMenuContainerCache[uniqueKey] = productMenuContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static ProductPriceContainer GetProductPriceContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductPriceContainer result = productMenuContainerCache.GetOrAdd(siteId, (k) => new ProductPriceContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static ProductPriceContainerSnapshotDTOCollection GetProductPriceContainerSnapshotDTOCollection(int siteId, int posMachineId, int userRoleId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTime startDateTime, DateTime endDateTime, string hash)
        {
            log.LogMethodEntry(siteId);
            ProductPriceContainer container = GetProductPriceContainer(siteId);
            ProductPriceContainerSnapshotDTOCollection result = container.GetProductPriceContainerSnapshotDTOCollection(posMachineId, userRoleId, languageId, menuType, membershipId, transactionProfileId, new DateTimeRange(startDateTime, endDateTime));
            if (hash == result.Hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(result);
            return result;
        }

        public static List<ProductsPriceContainerDTO> GetProductsPriceContainerDTOList(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTime dateTime)
        {
            return GetProductsPriceContainerDTOList(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, GetDateTimeRange(dateTime), dateTime);
        }

        public static List<ProductsPriceContainerDTO> GetProductsPriceContainerDTOList(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTimeRange dateTimeRange, DateTime dateTime)
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, dateTime);
            ProductPriceContainer productPriceContainer = GetProductPriceContainer(siteId);
            var productMenuPanelContainerDTOList = productPriceContainer.GetProductsPriceContainerDTOList(userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange, dateTime);
            log.LogMethodExit(productMenuPanelContainerDTOList);
            return productMenuPanelContainerDTOList;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            ProductPriceContainer productMenuContainer = GetProductPriceContainer(siteId);
            productMenuContainerCache[siteId] = productMenuContainer.Refresh();
            log.LogMethodExit();
        }

        private static DateTimeRange GetDateTimeRange(DateTime dateTime)
        {
            return new DateTimeRange(dateTime.Date, dateTime.Date.AddDays(1));
        }

    }
}
