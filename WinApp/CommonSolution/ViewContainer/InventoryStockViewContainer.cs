/********************************************************************************************
 * Project Name - View Container
 * Description  - Stock View container to maintain in UI
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      11-Dec-2020      Amitha           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ViewContainer
{

    /// <summary>
     /// Stock View container class, retrieves and holds stock for the pos machine
     /// </summary>
    public class InventoryStockViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ConcurrentDictionary<int, List<InventoryDTO>> productIdInventoryDTODictionary = new ConcurrentDictionary<int, List<InventoryDTO>>();
        private readonly int siteId;
        private readonly int machineId;
        private readonly ManualProductType manualProductType;
        private readonly List<InventoryDTO> inventoryDTOList;
        private readonly DateTime? maxLastUpdateDate;
        //private readonly object locker = new object();

        internal InventoryStockViewContainer(int siteId, int machineId, ManualProductType manualProductType, List<InventoryDTO> inventoryDTOList, List<InventoryDTO> deltainventoryDTOList)
        {
            log.LogMethodEntry(siteId, inventoryDTOList);
            this.siteId = siteId;
            this.machineId = machineId;
            this.manualProductType = manualProductType;
            if (deltainventoryDTOList != null && deltainventoryDTOList.Count() > 0)
            {
                foreach (InventoryDTO i in deltainventoryDTOList)
                {
                    int index = inventoryDTOList.FindIndex(x=>x.InventoryId==i.InventoryId);
                    if (index > -1)
                    {
                        inventoryDTOList.RemoveAt(index);
                        inventoryDTOList.Insert(index, i);
                    }
                    else
                    {
                        inventoryDTOList.Add(i);
                    }
                }
            }
            this.inventoryDTOList = inventoryDTOList;
            
            if (inventoryDTOList != null && inventoryDTOList.Count() >0 )
            {
                foreach (var inventoryDTO in inventoryDTOList)
                {
                    if (productIdInventoryDTODictionary.ContainsKey(inventoryDTO.ProductId) == false)
                    {
                        productIdInventoryDTODictionary[inventoryDTO.ProductId] = new List<InventoryDTO>();
                    }
                    productIdInventoryDTODictionary[inventoryDTO.ProductId].Add(inventoryDTO);
                }
                this.maxLastUpdateDate = GetLastUpdatedDate().Max();
            }            
            log.LogMethodExit();

        }
        /// <summary>
        /// to build stock view container
        /// </summary>
        public InventoryStockViewContainer(int siteId,int machineId, ManualProductType manualProductType)  
            : this(siteId, machineId, manualProductType, GetInventoryList(siteId, machineId, manualProductType),null)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// to get inventory list for all locations
        /// </summary>
        private static List<InventoryDTO> GetInventoryList(int siteid, int machineid, ManualProductType manualProductType,bool deltarefresh=false,DateTime? lastUpdateDate=null)
        {
            log.LogMethodEntry();
            List<InventoryDTO> result = new List<InventoryDTO>();
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.SITE_ID, Convert.ToString(siteid)));
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.POS_MACHINE_ID, Convert.ToString(machineid)));
                //if (!deltarefresh)
                //{
                //    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.GREATER_THAN_ZERO_STOCK, "Y"));
                //}
                if (manualProductType == ManualProductType.REDEEMABLE)
                {
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.IS_REDEEMABLE, "Y"));
                }
                else if (manualProductType == ManualProductType.SELLABLE)
                {
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.IS_SELLABLE, "Y"));
                }
                if (deltarefresh)
                {
                    if (lastUpdateDate != null)
                    {
                        inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.UPDATED_AFTER_DATE, ((DateTime)lastUpdateDate).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }

                }
                IInventoryStockUseCases iInventoryStockUseCases = InventoryUseCaseFactory.GetInventoryStockUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<InventoryDTO>> inventorytask = iInventoryStockUseCases.GetInventoryDTOList(inventorySearchParams);
                    inventorytask.Wait();
                    result = inventorytask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving inventory list.", ex);
            }
            log.LogMethodExit();

            return result;
        }
        
        internal InventoryStockViewContainer Refresh()
        {
            log.LogMethodEntry();
            //if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            //{
            //    log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
            //    return this;
            //}
            LastRefreshTime = DateTime.Now;
            List<InventoryDTO> deltainventoryListDTO = GetInventoryList(siteId, machineId, manualProductType, true, maxLastUpdateDate);
            if (deltainventoryListDTO == null )
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            InventoryStockViewContainer result = new InventoryStockViewContainer(siteId, machineId, manualProductType, inventoryDTOList,deltainventoryListDTO);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// to get stock for  product and a location
        /// </summary>
        //
        internal double GetStock(int productId, int locationId)
        {
            double result = 0;
            log.LogMethodEntry(productId, locationId);
            if (productIdInventoryDTODictionary.ContainsKey(productId) == true)
            {
                foreach (InventoryDTO i in productIdInventoryDTODictionary[productId])
                {
                    if (i.LocationId == locationId)
                    {
                        result += i.Quantity;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// to get stock with lot for  product and a location
        /// </summary>
        //
        internal Dictionary<string, double> GetStockWithLot(int productId, int locationId)
        {
            log.LogMethodEntry(productId, locationId);
            Dictionary<string, double> result = new Dictionary<string, double>();
            if (productIdInventoryDTODictionary.ContainsKey(productId) == true)
            {
                foreach (InventoryDTO i in productIdInventoryDTODictionary[productId])
                {
                    if (i.LocationId == locationId)
                    {
                        if (result.ContainsKey(((i.InventoryLotDTO == null || i.InventoryLotDTO.LotNumber == null) ? "-1" : i.InventoryLotDTO.LotNumber)) == true)
                            continue;
                        result.Add(((i.InventoryLotDTO == null || i.InventoryLotDTO.LotNumber == null) ? "-1" : i.InventoryLotDTO.LotNumber),i.Quantity);
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// to get inventory list for all locations for a product
        /// </summary>
        internal List<int> GetStockLocationList( int productid)
        {
            List<int> result = new List<int>();
            log.LogMethodEntry(productid);
            if (productIdInventoryDTODictionary.ContainsKey(productid) == true)
            {
                result = productIdInventoryDTODictionary[productid].Select(x => x.LocationId).Distinct().ToList();
            }
            log.LogMethodExit(result);
            return result;
        }
        
        private IEnumerable<DateTime> GetLastUpdatedDate()
        {
            foreach (int p in productIdInventoryDTODictionary.Keys)
            {
                foreach (var timestamp in productIdInventoryDTODictionary[p].Select(x => x.Timestamp))
                {
                    yield return timestamp;
                }
            }
        }
        
    }
}
