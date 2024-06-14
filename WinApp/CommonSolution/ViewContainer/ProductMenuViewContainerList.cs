/********************************************************************************************
 * Project Name - View Container
 * Description  - ProductMenuViewContainerList holds view container values based on siteId, userRoleId, POSMachineId, languageId and dateTimeRange
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
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// holds view container values based on siteId, userRoleId, POSMachineId, languageId and dateTimeRange
    /// </summary>
    public class ProductMenuViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<ProductMenuContainerCacheKey, ProductMenuViewContainer> productMenuViewContainerCache = new Cache<ProductMenuContainerCacheKey, ProductMenuViewContainer>();
        private static Timer refreshTimer;

        static ProductMenuViewContainerList()
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
            var uniqueKeyList = productMenuViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                ProductMenuViewContainer productMenuViewContainer;
                if (productMenuViewContainerCache.TryGetValue(uniqueKey, out productMenuViewContainer))
                {
                    productMenuViewContainerCache[uniqueKey] = productMenuViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static ProductMenuViewContainer GetProductMenuViewContainer(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(siteId);
            ProductMenuContainerCacheKey key = new ProductMenuContainerCacheKey(siteId, posMachineId,userRoleId, languageId, menuType, dateTimeRange);
            var result = productMenuViewContainerCache.GetOrAdd(key, (k)=> new ProductMenuViewContainer(siteId, userRoleId, posMachineId, languageId, menuType, dateTimeRange));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the ProductMenuContainerSnapshotDTOCollection for a given keys and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="userRoleId">user role Id</param>
        /// <param name="posMachineId">pos machine id</param>
        /// <param name="languageId">language id</param>
        /// <param name="menuType">menu type</param>
        /// <param name="startDateTime">start date time</param>
        /// <param name="endDateTime">end date time</param>
        /// <param name="hash">hash</param>
        /// <returns></returns>
        public static ProductMenuContainerSnapshotDTOCollection GetProductMenuContainerSnapshotDTOCollection(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, DateTime startDateTime, DateTime endDateTime, string hash)
        {
            log.LogMethodEntry(siteId, hash);
            ProductMenuViewContainer productMenuViewContainer = GetProductMenuViewContainer(siteId, userRoleId, posMachineId, languageId, menuType, new DateTimeRange(startDateTime, endDateTime));
            ProductMenuContainerSnapshotDTOCollection result = productMenuViewContainer.GetProductMenuContainerSnapshotDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(ExecutionContext executionContext, string menuType)
        {
            log.LogMethodEntry(executionContext, menuType);
            Rebuild(executionContext, menuType, DateTime.Now);
            log.LogMethodExit();
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(ExecutionContext executionContext, string menuType, DateTime dateTime)
        {
            log.LogMethodEntry(executionContext, menuType, dateTime);
            int userRoleId = UserViewContainerList.GetUserContainerDTO(executionContext.SiteId, executionContext.UserId).RoleId;
            Rebuild(executionContext.SiteId, userRoleId, executionContext.MachineId, executionContext.LanguageId, menuType, GetDateTimeRange(dateTime));
            log.LogMethodExit();
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, dateTimeRange);
            ProductMenuContainerCacheKey key = new ProductMenuContainerCacheKey(siteId, posMachineId, userRoleId, languageId, menuType, dateTimeRange);
            ProductMenuViewContainer productMenuViewContainer = GetProductMenuViewContainer(siteId, userRoleId, posMachineId, languageId, menuType, dateTimeRange);
            productMenuViewContainerCache[key] = productMenuViewContainer.Refresh(true);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns ProductMenuPanelContainerDTOList based on the execution context, menuType and a current date time
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="menuType">menu type</param>
        /// <returns></returns>
        public static List<ProductMenuPanelContainerDTO> GetProductMenuPanelContainerDTOList(ExecutionContext executionContext, string menuType)
        {
            log.LogMethodEntry(executionContext, menuType);
            var productMenuPanelContainerDTOList = GetProductMenuPanelContainerDTOList(executionContext, menuType, DateTime.Now);
            log.LogMethodExit(productMenuPanelContainerDTOList);
            return productMenuPanelContainerDTOList;
        }

        /// <summary>
        /// Returns ProductMenuPanelContainerDTOList based on the execution context, menuType and a specified date time
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="menuType">menu type</param>
        /// <param name="dateTime">date time</param>
        /// <returns></returns>
        public static List<ProductMenuPanelContainerDTO> GetProductMenuPanelContainerDTOList(ExecutionContext executionContext, string menuType, DateTime dateTime)
        {
            log.LogMethodEntry(executionContext, menuType, dateTime);
            int userRoleId = UserViewContainerList.GetUserContainerDTO(executionContext.SiteId, executionContext.UserId).RoleId;
            var productMenuPanelContainerDTOList = GetProductMenuPanelContainerDTOList(executionContext.SiteId, userRoleId, executionContext.MachineId, executionContext.LanguageId, menuType, dateTime);
            log.LogMethodExit(productMenuPanelContainerDTOList);
            return productMenuPanelContainerDTOList;
        }

        /// <summary>
        /// Returns ProductMenuPanelContainerDTOList based on the execution context, menuType and a specified date time
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="userRoleId">user role id</param>
        /// <param name="posMachineId">pos machine id</param>
        /// <param name="languageId">language id</param>
        /// <param name="menuType">menu type</param>
        /// <param name="dateTime">date time</param>
        /// <returns></returns>
        public static List<ProductMenuPanelContainerDTO> GetProductMenuPanelContainerDTOList(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, DateTime dateTime)
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, dateTime);
            ProductMenuViewContainer productMenuViewContainer = GetProductMenuViewContainer(siteId, userRoleId, posMachineId, languageId, menuType, GetDateTimeRange(dateTime));
            var productMenuPanelContainerDTOList = productMenuViewContainer.GetProductMenuPanelContainerDTOList(dateTime);
            log.LogMethodExit(productMenuPanelContainerDTOList);
            return productMenuPanelContainerDTOList;
        }

        private static DateTimeRange GetDateTimeRange(DateTime dateTime)
        {
            return new DateTimeRange(dateTime.Date, dateTime.Date.AddDays(1));
        }
        
    }
}
