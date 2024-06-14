/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteInventoryAdjustmentsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     29-Dec-2020       Abhishek                  Created 
 ********************************************************************************************/
using System;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public class RemoteInventoryAdjustmentsUseCases : RemoteUseCases, IInventoryAdjustmentsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string INVENTORY_ADJUSTMENTS_URL = "api/Inventory/Adjustments";
        private const string INVENTORY_ADJUSTMENT_COUNT_URL = "api/Inventory/AdjustmentCounts";
        private const string INVENTORY_TOTAL_COST_URL = "api/Inventory/TotalCost";

        public RemoteInventoryAdjustmentsUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<InventoryAdjustmentsSummaryDTO>> GetInventoryAdjustmentsSummary(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> parameters,
                                                                                               string advancedSearch = null, string pivotColumns = null, bool ignoreWastage = true, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        { 
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("advancedSearch", advancedSearch));
            searchParameterList.Add(new KeyValuePair<string, string>("pivotColumns", pivotColumns));
            searchParameterList.Add(new KeyValuePair<string, string>("ignoreWastage", ignoreWastage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage", currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize", pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<InventoryAdjustmentsSummaryDTO> inventoryAdjustmentsSummaryDTOList = await Get<List<InventoryAdjustmentsSummaryDTO>>(INVENTORY_ADJUSTMENTS_URL, searchParameterList);
                log.LogMethodExit(inventoryAdjustmentsSummaryDTOList);
                return inventoryAdjustmentsSummaryDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetInventoryAdjustmentCount(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> parameters,
                                                                                               string advancedSearch = null, string pivotColumns = null, bool ignoreWastage = true, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("advancedSearch", advancedSearch));
            searchParameterList.Add(new KeyValuePair<string, string>("pivotColumns", pivotColumns));
            searchParameterList.Add(new KeyValuePair<string, string>("ignoreWastage", ignoreWastage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage", currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize", pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                int result = await Get<int>(INVENTORY_ADJUSTMENT_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> inventoryAdjustmentsSearchParams)
        {
            log.LogMethodEntry(inventoryAdjustmentsSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string> searchParameter in inventoryAdjustmentsSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.PRODUCT_CODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Product Code".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.DESCRIPTION:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Description".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.BARCODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Barcode".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.LOCATION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Location Id".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.PURCHASEABLE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Purchaseable".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveInventoryAdjustments(List<InventoryAdjustmentsDTO> inventoryAdjustmentsDTOList)
        {
            log.LogMethodEntry(inventoryAdjustmentsDTOList);
            try
            {
                string responseString = await Post<string>(INVENTORY_ADJUSTMENTS_URL, inventoryAdjustmentsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Returns the Inventory Total Cost
        /// </summary>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public async Task<string> GetInventoryTotalCost(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                string responseData = await Get<string>(INVENTORY_TOTAL_COST_URL, new List<KeyValuePair<string, string>>());
                log.LogMethodExit(responseData);
                return responseData;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
