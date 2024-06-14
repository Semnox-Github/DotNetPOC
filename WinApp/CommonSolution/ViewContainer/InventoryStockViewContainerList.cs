using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Stock View master list class
    /// </summary>
    public class InventoryStockViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<string, InventoryStockViewContainer> stockViewContainerCache = new Cache<string, InventoryStockViewContainer>();
        private static Timer refreshTimer;

        static InventoryStockViewContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(300000); // 5 mts
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }
        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = stockViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                InventoryStockViewContainer stockViewContainer;
                if (stockViewContainerCache.TryGetValue(uniqueKey, out stockViewContainer))
                {
                    stockViewContainerCache[uniqueKey] = stockViewContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static InventoryStockViewContainer GetStockViewContainer(int siteId, int machineId, ManualProductType manualProductType)
        {
            log.LogMethodEntry(siteId);
            string uniqueKey = GetUniqueKey(siteId, machineId, manualProductType);
            InventoryStockViewContainer result = stockViewContainerCache.GetOrAdd(uniqueKey, (k)=> new InventoryStockViewContainer(siteId, machineId, manualProductType));
            log.LogMethodExit(result);
            return result;
        }
        private static string GetUniqueKey(int siteId, int machineId, ManualProductType manualProductType)
        {
            log.LogMethodEntry(siteId, machineId, manualProductType);
            string uniqueKey = "SiteId:" + siteId + "MachineId:" + machineId + "ManualProductType:" + manualProductType;
            log.LogMethodExit(uniqueKey);
            return uniqueKey;
        }
        /// <summary>
        /// get inventory list for a product for all locations
        /// </summary>
        public static List<int> GetStockLocationList(int siteid, int machineid,ManualProductType manualProductType, int productid)
        {
            log.LogMethodEntry(productid);
            InventoryStockViewContainer stockViewContainer = GetStockViewContainer(siteid, machineid, manualProductType);
            List<int> result = stockViewContainer.GetStockLocationList(productid);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// get total stock for a product  and location
        /// </summary>
        public static double GetStock(int siteid, int machineid, ManualProductType manualProductType, int productid, int locationid)
        {
            log.LogMethodEntry(productid);
            InventoryStockViewContainer stockViewContainer = GetStockViewContainer(siteid, machineid, manualProductType);
            double result = stockViewContainer.GetStock(productid, locationid);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// get stock with lot info for a product  and location
        /// </summary>
        public static Dictionary<string, double> GetStockWithLot(int siteid, int machineid, ManualProductType manualProductType, int productid, int locationid)
        {
            log.LogMethodEntry(productid);
            Dictionary<string, double> result = new Dictionary<string, double>();
            InventoryStockViewContainer stockViewContainer = GetStockViewContainer(siteid, machineid, manualProductType);
            result = stockViewContainer.GetStockWithLot(productid, locationid);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId, int machineId, ManualProductType manualProductType)
        {
            log.LogMethodEntry();
            InventoryStockViewContainer stockViewContainer = GetStockViewContainer(siteId, machineId, manualProductType);
            string uniqueKey = GetUniqueKey(siteId, machineId, manualProductType);
            stockViewContainerCache[uniqueKey] = stockViewContainer.Refresh();
            log.LogMethodExit();
        }
        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(ExecutionContext executionContext, ManualProductType manualProductType)
        {
            log.LogMethodEntry();
            Rebuild(executionContext.GetSiteId(), executionContext.GetMachineId(), manualProductType);
            log.LogMethodExit();
        }
    }
}
