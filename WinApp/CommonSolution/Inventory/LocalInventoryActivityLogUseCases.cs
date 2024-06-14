/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalInventoryActivityLogUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0      01-Jan-2021       Abhishek                 Created 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class LocalInventoryActivityLogUseCases : LocalUseCases, IInventoryActivityLogUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public LocalInventoryActivityLogUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<InventoryActivityLogDTO>> GetInventoryAcitvityLogs(List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>> parameters,
                                                                                 int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<InventoryActivityLogDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                InventoryActivityLogBLList InventoryActivityLogBLList = new InventoryActivityLogBLList(executionContext);
                int siteId = GetSiteId();
                List<InventoryActivityLogDTO> inventoryActivityLogBLList = InventoryActivityLogBLList.GetInventoryActivityLogDTOList(parameters, currentPage, pageSize, sqlTransaction);
                log.LogMethodExit(inventoryActivityLogBLList);
                return inventoryActivityLogBLList;
            });
        }

        public async Task<int> GetInventoryAcitvityCount(List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>> parameters,
                                                                                 int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                InventoryActivityLogBLList InventoryActivityLogBLList = new InventoryActivityLogBLList(executionContext);
                int siteId = GetSiteId();
                int inventoryActivityCount = InventoryActivityLogBLList.GetInventoryActivityLogCount(parameters);
                log.LogMethodExit(inventoryActivityCount);
                return inventoryActivityCount;
            });
        }

        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }
    }
}
