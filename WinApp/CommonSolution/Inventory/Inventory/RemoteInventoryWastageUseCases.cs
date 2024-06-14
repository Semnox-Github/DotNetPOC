/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteInventoryWastageUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     28-Dec-2020       Abhishek                  Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public class RemoteInventoryWastageUseCases : RemoteUseCases, IInventoryWastageUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string INVENTORY_WASTAGES_URL = "api/Inventory/Wastages";
        private const string INVENTORY_WASTAGE_COUNT_URL = "api/Inventory/WastageCounts";

        public RemoteInventoryWastageUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<InventoryWastageSummaryDTO>> GetInventoryWastages(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> parameters,
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
                List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = await Get<List<InventoryWastageSummaryDTO>>(INVENTORY_WASTAGES_URL, searchParameterList);
                log.LogMethodExit(inventoryWastageSummaryDTOList);
                return inventoryWastageSummaryDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetInventoryWastageCount(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> parameters,
                                                                                SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
    
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                int inventoryWastageCount = await Get<int>(INVENTORY_WASTAGE_COUNT_URL, searchParameterList);
                log.LogMethodExit(inventoryWastageCount);
                return inventoryWastageCount;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> inventoryReceiptsSearchParams)
        {
            log.LogMethodEntry(inventoryReceiptsSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string> searchParameter in inventoryReceiptsSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_FROM_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Wastage From Date".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_TO_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Wastage To Date".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.CATEGORY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Category".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.PRODUCT_DESCRIPTION:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Product Description".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveInventoryWastages(List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList)
        {
            log.LogMethodEntry(inventoryWastageSummaryDTOList);
            try
            {
                string responseString = await Post<string>(INVENTORY_WASTAGES_URL, inventoryWastageSummaryDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
