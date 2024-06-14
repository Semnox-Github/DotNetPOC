/********************************************************************************************
 * Project Name - Inventory
 * Description  - IInventoryNotesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     11-Dec-2020       Mushahid Faizan         Created 
 ********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IInventoryNotesUseCases
    {
        Task<List<InventoryNotesDTO>> GetInventoryNotes(List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>>
          searchParameters, int currentPage = 0, int pageSize = 0);
        Task<string> SaveInventoryNotes(List<InventoryNotesDTO> purchaseOrderDTOList);
    }
}
