/********************************************************************************************
 * Project Name - Inventory
 * Description  - IInventoryPhysicalCount class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      30-Dec-2020       Abhishek                 Created 
 *2.110.1     01-Mar-2021      Mushahid Faizan          Modified : Web Inventory UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory.PhysicalCount
{
    public interface IInventoryPhysicalCountUseCases
    {
        Task<List<InventoryPhysicalCountDTO>> GetInventoryPhysicalCounts(List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> searchParameters,
                                                                         int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null);
        Task<int> GetInventoryPhysicalCount(List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> searchParameters,
                                                                          SqlTransaction sqlTransaction = null);
        Task<List<InventoryPhysicalCountDTO>> SaveInventoryPhysicalCounts(List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList);
        /// <summary>
        /// UpdatePhysicalCountStatus
        /// </summary>
        /// <param name="inventoryPhysicalCountDTOList">inventoryPhysicalCountDTOList</param>
        /// <returns>HttpMessage</returns>
        Task<List<InventoryPhysicalCountDTO>> UpdatePhysicalCountStatus(List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList);
    }
}
