/********************************************************************************************
 * Project Name - Inventory
 * Description  - IInventoryWastageUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0    28-Dec-2020         Abhishek                 Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IInventoryWastageUseCases
    {
        Task<List<InventoryWastageSummaryDTO>> GetInventoryWastages(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters,
                                                                    int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null);
        Task<string> SaveInventoryWastages(List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList);
        Task<int> GetInventoryWastageCount(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters,
                                                                     SqlTransaction sqlTransaction = null);
    }
}
