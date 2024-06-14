/********************************************************************************************
 * Project Name - Inventory
 * Description  - IInvenoryActivityLogUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     01-Jan-2021        Abhishek                Created 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IInventoryActivityLogUseCases
    {
        Task<List<InventoryActivityLogDTO>> GetInventoryAcitvityLogs(List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null);
        Task<int> GetInventoryAcitvityCount(List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null);
    }
}
