/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteInventoryActivityLogUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     01-Jan-2021       Abhishek                  Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class RemoteInventoryActivityLogUseCases : RemoteUseCases, IInventoryActivityLogUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string INVENTORY_ACTIVITY_LOG_URL = "api/Inventory/InventoryActivities";
        private const string INVENTORY_ACTIVITY_COUNT_URL = "api/Inventory/InventoryActivityCounts";

        public RemoteInventoryActivityLogUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<InventoryActivityLogDTO>> GetInventoryAcitvityLogs(List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>> parameters,
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
            try
            {
                List<InventoryActivityLogDTO> inventoryActivityLogDTOList = await Get<List<InventoryActivityLogDTO>>(INVENTORY_ACTIVITY_LOG_URL, searchParameterList);
                log.LogMethodExit(inventoryActivityLogDTOList);
                return inventoryActivityLogDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetInventoryAcitvityCount(List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>> parameters,
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
            try
            {
                int inventoryActivityCount = await Get<int>(INVENTORY_ACTIVITY_COUNT_URL, searchParameterList);
                log.LogMethodExit(inventoryActivityCount);
                return inventoryActivityCount;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>> inventoryActivityLogSearchParams)
        {
            log.LogMethodEntry(inventoryActivityLogSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string> searchParameter in inventoryActivityLogSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case InventoryActivityLogDTO.SearchByParameters.INV_TABLE_KEY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Inventory Table Key".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryActivityLogDTO.SearchByParameters.SOURCE_TABLE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Source Table Name".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryActivityLogDTO.SearchByParameters.MESSAGE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Message".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

    }
}
