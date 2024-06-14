/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteInventoryPhysicalCountUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      30-Dec-2020       Abhishek                 Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory.PhysicalCount
{
    public class RemoteInventoryPhysicalCountUseCases : RemoteUseCases, IInventoryPhysicalCountUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string INVENTORY_PHYSICAL_COUNTS_URL = "api/Inventory/PhysicalCounts";
        private const string INVENTORY_PHYSICAL_COUNT_STATUS_URL = "api/Inventory/PhysicalCount/Status";

        public RemoteInventoryPhysicalCountUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<InventoryPhysicalCountDTO>> GetInventoryPhysicalCounts(List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> parameters,
                                                                                      int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage", currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize", pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList = await Get<List<InventoryPhysicalCountDTO>>(INVENTORY_PHYSICAL_COUNTS_URL, searchParameterList);
            log.LogMethodExit(inventoryPhysicalCountDTOList);
            return inventoryPhysicalCountDTOList;
        }

        public async Task<int> GetInventoryPhysicalCount(List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> parameters,
                                                                                       SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            int result = await Get<int>(INVENTORY_PHYSICAL_COUNTS_URL, searchParameterList);
            log.LogMethodExit(result);
            return result;
        }


        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> inventoryPhysicalCountsSearchParams)
        {
            log.LogMethodEntry(inventoryPhysicalCountsSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string> searchParameter in inventoryPhysicalCountsSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.PHYSICAL_COUNT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Physical Count Id".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.LOCATIONID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Location Id".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Name".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.STATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Status".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<InventoryPhysicalCountDTO>> SaveInventoryPhysicalCounts(List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList)
        {
            log.LogMethodEntry(inventoryPhysicalCountDTOList);
            List<InventoryPhysicalCountDTO> responseString = await Post<List<InventoryPhysicalCountDTO>>(INVENTORY_PHYSICAL_COUNTS_URL, inventoryPhysicalCountDTOList);
            log.LogMethodExit(responseString);
            return responseString;
        }

        public async Task<List<InventoryPhysicalCountDTO>> UpdatePhysicalCountStatus(List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList)
        {
            log.LogMethodEntry(inventoryPhysicalCountDTOList);
            List<InventoryPhysicalCountDTO> responseString = await Post<List<InventoryPhysicalCountDTO>>(INVENTORY_PHYSICAL_COUNT_STATUS_URL, inventoryPhysicalCountDTOList);
            log.LogMethodExit(responseString);
            return responseString;
        }
    }
}
