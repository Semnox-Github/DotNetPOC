  /********************************************************************************************
 * Project Name - Inventory
 * Description  - ILocationUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     24-Dec-2020      Prajwal S                Created : POS UI Redesign with REST API
 ********************************************************************************************/
 using System.Collections.Generic;
 using System.Data.SqlClient;
 using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IInventoryReceiveLinesUseCases
    {
        Task<List<InventoryReceiveLinesDTO>> GetInventoryReceiveLines(List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>>
                          searchParameters, int currentPage = 0, int pageSize = 0, bool loadChildRecords = false, bool loadActiveChild = false, SqlTransaction sqlTransaction = null);
        Task<string> SaveInventoryReceiveLines(List<InventoryReceiveLinesDTO> InventoryReceiveLinesDTOList);
        Task<int> GetInventoryReceiveLineCounts(List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>> searchParameters);
        Task<InventoryReceiptDTO> AddInventoryReceiveLines(int receiptId, List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList);
        Task<InventoryReceiptDTO> UpdateInventoryReceiveLines(int receiptId, List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList);
    }
}