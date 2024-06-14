/********************************************************************************************
 * Project Name - View Container
 * Description  - ProductPriceViewContainerList holds view container values based on siteId, userRoleId, POSMachineId, menuType, membershipId, transactionProfileId, languageId and dateTimeRange
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.1      10-Aug-2021      Lakshminarayana           Created : Static menu enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ProductPrice;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// holds view container values based on siteId, userRoleId, POSMachineId, languageId and dateTimeRange
    /// </summary>
    public class ProductPriceViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<ProductPriceContainerCacheKey, ProductPriceViewContainer> productPriceViewContainerCache = new Cache<ProductPriceContainerCacheKey, ProductPriceViewContainer>();
        private static Timer refreshTimer;

        static ProductPriceViewContainerList()
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
            var uniqueKeyList = productPriceViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                ProductPriceViewContainer productPriceViewContainer;
                if (productPriceViewContainerCache.TryGetValue(uniqueKey, out productPriceViewContainer))
                {
                    productPriceViewContainerCache[uniqueKey] = productPriceViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static ProductPriceViewContainer GetProductPriceViewContainer(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(siteId);
            ProductPriceContainerCacheKey key = new ProductPriceContainerCacheKey(siteId, posMachineId,userRoleId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange);
            var result = productPriceViewContainerCache.GetOrAdd(key, (k)=> new ProductPriceViewContainer(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the ProductPriceContainerSnapshotDTOCollection for a given keys and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        public static ProductPriceContainerSnapshotDTOCollection GetProductPriceContainerSnapshotDTOCollection(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTime startDateTime, DateTime endDateTime, string hash)
        {
            log.LogMethodEntry(siteId, hash);
            ProductPriceViewContainer productPriceViewContainer = GetProductPriceViewContainer(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, new DateTimeRange(startDateTime, endDateTime));
            ProductPriceContainerSnapshotDTOCollection result = productPriceViewContainer.GetProductPriceContainerSnapshotDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(ExecutionContext executionContext, string menuType, int membershipId, int transactionProfileId)
        {
            log.LogMethodEntry(executionContext, menuType);
            Rebuild(executionContext, menuType, membershipId, transactionProfileId, DateTime.Now);
            log.LogMethodExit();
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(ExecutionContext executionContext, string menuType, int membershipId, int transactionProfileId, DateTime dateTime)
        {
            log.LogMethodEntry(executionContext, menuType, dateTime);
            int userRoleId = UserViewContainerList.GetUserContainerDTO(executionContext.SiteId, executionContext.UserId).RoleId;
            Rebuild(executionContext.SiteId, userRoleId, executionContext.MachineId, executionContext.LanguageId, menuType, membershipId, transactionProfileId, GetDateTimeRange(dateTime));
            log.LogMethodExit();
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, dateTimeRange);
            ProductPriceContainerCacheKey key = new ProductPriceContainerCacheKey(siteId, posMachineId, userRoleId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange);
            ProductPriceViewContainer productPriceViewContainer = GetProductPriceViewContainer(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange);
            productPriceViewContainerCache[key] = productPriceViewContainer.Refresh(true);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns ProductsPriceContainerDTOList based on the execution context, menuType and a current date time
        /// </summary>
        public static List<ProductsPriceContainerDTO> GetProductsPriceContainerDTOList(ExecutionContext executionContext, string menuType, int membershipId, int transactionProfileId)
        {
            log.LogMethodEntry(executionContext, menuType);
            var productsPriceContainerDTOList = GetProductsPriceContainerDTOList(executionContext, menuType, membershipId, transactionProfileId, DateTime.Now);
            log.LogMethodExit(productsPriceContainerDTOList);
            return productsPriceContainerDTOList;
        }

        /// <summary>
        /// Returns ProductsPriceContainerDTOList based on the execution context, menuType and a specified date time
        /// </summary>
        public static List<ProductsPriceContainerDTO> GetProductsPriceContainerDTOList(ExecutionContext executionContext, string menuType, int membershipId, int transactionProfileId, DateTime dateTime)
        {
            log.LogMethodEntry(executionContext, menuType, dateTime);
            int userRoleId = UserViewContainerList.GetUserContainerDTO(executionContext.SiteId, executionContext.UserId).RoleId;
            var productsPriceContainerDTOList = GetProductsPriceContainerDTOList(executionContext.SiteId, userRoleId, executionContext.MachineId, executionContext.LanguageId, menuType, membershipId, transactionProfileId, dateTime);
            log.LogMethodExit(productsPriceContainerDTOList);
            return productsPriceContainerDTOList;
        }

        /// <summary>
        /// Returns ProductsPriceContainerDTOList based on the execution context, menuType and a specified date time
        /// </summary>
        public static List<ProductsPriceContainerDTO> GetProductsPriceContainerDTOList(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTime dateTime)
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, dateTime);
            ProductPriceViewContainer productPriceViewContainer = GetProductPriceViewContainer(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, GetDateTimeRange(dateTime));
            var productsPriceContainerDTOList = productPriceViewContainer.GetProductsPriceContainerDTOList(dateTime);
            log.LogMethodExit(productsPriceContainerDTOList);
            return productsPriceContainerDTOList;
        }

        private static DateTimeRange GetDateTimeRange(DateTime dateTime)
        {
            return new DateTimeRange(dateTime.Date, dateTime.Date.AddDays(1));
        }
        
    }
}
