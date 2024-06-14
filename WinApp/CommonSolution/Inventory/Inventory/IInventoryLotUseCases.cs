/********************************************************************************************
 * Project Name - Inventory
 * Description  - IInventoryLotUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      22-Jun-2022      Abhishek                 Created : Web Inventory UI resdesign
 ********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IInventoryLotUseCases
    {
        Task<List<InventoryLotDTO>> GetInventoryLots(List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>>
          searchParameters, int currentPage = 0, int pageSize = 0);
        //Task<string> SaveInventoryLots(List<InventoryNotesDTO> purchaseOrderDTOList);
    }
}
