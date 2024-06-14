/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalInventoryStockUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         30-Nov-2020       Girish         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public class LocalInventoryStockUseCases : IInventoryStockUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalInventoryStockUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<InventoryDTO>> GetInventoryDTOList(List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams)
        {
            return await Task<List<InventoryDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(inventorySearchParams);
            InventoryList inventoryList = new InventoryList(executionContext);
            List<InventoryDTO> inventoryDTOList = inventoryList.GetAllInventory(inventorySearchParams,true);
            log.LogMethodExit(inventoryDTOList);
            return inventoryDTOList;
            });
        }
    }
}
