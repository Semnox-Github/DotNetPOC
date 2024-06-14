/********************************************************************************************
 * Project Name - Inventory
 * Description  - IInventoryAdjustmentsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0    29-Dec-2020         Abhishek                 Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IInventoryAdjustmentsUseCases
    {
        Task<List<InventoryAdjustmentsSummaryDTO>> GetInventoryAdjustmentsSummary(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> searchParameters,
                                                                                  string advancedSearch = null, string pivotColumns = null, bool ignoreWastage = true, 
                                                                                  int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null);
        Task<string> SaveInventoryAdjustments(List<InventoryAdjustmentsDTO> inventoryAdjustmentsDTOList);
        Task<int> GetInventoryAdjustmentCount(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> searchParameters,
                                                                                  string advancedSearch = null, string pivotColumns = null, bool ignoreWastage = true, int currentPage = 0,
                                                                                  int pageSize = 10, SqlTransaction sqlTransaction = null);
        /// <summary>
        /// Returns the Inventory Total Cost
        /// </summary>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        Task<string> GetInventoryTotalCost(SqlTransaction sqlTransaction = null);
    }
}
