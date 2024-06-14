/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalInventoryLotUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      22-Jun-2022      Abhishek                 Created : Web Inventory UI resdesign
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class LocalInventoryLotUseCases : IInventoryLotUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalInventoryLotUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<InventoryLotDTO>> GetInventoryLots(List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>>
                          searchParameters, int currentPage = 0, int pageSize = 0)
        {

            return await Task<List<InventoryLotDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                InventoryLotList inventoryLotListBL = new InventoryLotList(executionContext);
                List<InventoryLotDTO> inventoryLotDTOList = inventoryLotListBL.GetAllInventoryLot(searchParameters);
                log.LogMethodExit(inventoryLotDTOList);
                return inventoryLotDTOList;
            });
        }
    }
}
