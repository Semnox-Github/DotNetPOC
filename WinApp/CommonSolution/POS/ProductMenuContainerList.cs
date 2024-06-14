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
using System;
using System.Collections.Generic;
using System.Timers;

namespace Semnox.Parafait.POS
{
    public static class ProductMenuContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, ProductMenuContainer> productMenuContainerCache = new Cache<int, ProductMenuContainer>();
        private static Timer refreshTimer;

        static ProductMenuContainerList()
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
                ProductMenuContainer productMenuContainer;
                if (productMenuContainerCache.TryGetValue(uniqueKey, out productMenuContainer))
                {
                    productMenuContainerCache[uniqueKey] = productMenuContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static ProductMenuContainer GetProductMenuContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductMenuContainer result = productMenuContainerCache.GetOrAdd(siteId, (k) => new ProductMenuContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static ProductMenuContainerSnapshotDTOCollection GetProductMenuContainerSnapshotDTOCollection(int siteId, int posMachineId, int userRoleId, int languageId, string menuType, DateTime startDateTime, DateTime endDateTime)
        {
            log.LogMethodEntry(siteId);
            ProductMenuContainer container = GetProductMenuContainer(siteId);
            ProductMenuContainerSnapshotDTOCollection result = container.GetProductMenuContainerSnapshotDTOCollection(posMachineId, userRoleId, languageId, menuType, new DateTimeRange(startDateTime, endDateTime));
            log.LogMethodExit(result);
            return result;
        }

        public static List<ProductMenuPanelContainerDTO> GetProductMenuPanelContainerDTOList(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, DateTime dateTime)
        {
            return GetProductMenuPanelContainerDTOList(siteId, userRoleId, posMachineId, languageId, menuType, GetDateTimeRange(dateTime), dateTime);
        }

        public static List<ProductMenuPanelContainerDTO> GetProductMenuPanelContainerDTOList(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, DateTimeRange dateTimeRange, DateTime dateTime)
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, dateTime);
            ProductMenuContainer productMenuViewContainer = GetProductMenuContainer(siteId);
            var productMenuPanelContainerDTOList = productMenuViewContainer.GetProductMenuPanelContainerDTOList(posMachineId, userRoleId, languageId, menuType, dateTimeRange, dateTime);
            log.LogMethodExit(productMenuPanelContainerDTOList);
            return productMenuPanelContainerDTOList;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            ProductMenuContainer productMenuContainer = GetProductMenuContainer(siteId);
            productMenuContainerCache[siteId] = productMenuContainer.Refresh();
            log.LogMethodExit();
        }

        private static DateTimeRange GetDateTimeRange(DateTime dateTime)
        {
            return new DateTimeRange(dateTime.Date, dateTime.Date.AddDays(1));
        }

    }
}
