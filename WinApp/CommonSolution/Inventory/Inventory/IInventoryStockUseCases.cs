/********************************************************************************************
 * Project Name - Inventory
 * Description  - IInventoryStockUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         30-Nov-2020       Girish         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IInventoryStockUseCases
    {
        Task<List<InventoryDTO>> GetInventoryDTOList(List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams);
    }
}
