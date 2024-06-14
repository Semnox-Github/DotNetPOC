/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalInventoryPhysicalCount class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      30-Dec-2020       Abhishek                 Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory.PhysicalCount
{
    public class LocalInventoryPhysicalCountUseCases : LocalUseCases, IInventoryPhysicalCountUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public LocalInventoryPhysicalCountUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<InventoryPhysicalCountDTO>> GetInventoryPhysicalCounts(List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> parameters,
                                                                                      int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<InventoryPhysicalCountDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                InventoryPhysicalCountList inventoryPhysicalCountsList = new InventoryPhysicalCountList(executionContext);
                int siteId = GetSiteId();
                List<InventoryPhysicalCountDTO> inventoryPhysicalCountsDTOList = inventoryPhysicalCountsList.GetAllInventoryPhysicalCount(parameters, currentPage, pageSize, sqlTransaction);
                log.LogMethodExit(inventoryPhysicalCountsDTOList);
                return inventoryPhysicalCountsDTOList;
            });
        }

        public async Task<int> GetInventoryPhysicalCount(List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> parameters,
                                                                                      SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                InventoryPhysicalCountList inventoryPhysicalCountsList = new InventoryPhysicalCountList(executionContext);
                int siteId = GetSiteId();
                int inventoryPhysicalCounts = inventoryPhysicalCountsList.GetInventoryPhysicalCountsCount(parameters);
                log.LogMethodExit(inventoryPhysicalCounts);
                return inventoryPhysicalCounts;
            });
        }

        public async Task<List<InventoryPhysicalCountDTO>> SaveInventoryPhysicalCounts(List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList)
        {
            return await Task<List<InventoryPhysicalCountDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(inventoryPhysicalCountDTOList);
                List<InventoryPhysicalCountDTO> result = new List<InventoryPhysicalCountDTO>();
                if (inventoryPhysicalCountDTOList == null)
                {
                    throw new ValidationException("inventoryPhysicalCountDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                {
                    parafaitDBTransaction.BeginTransaction();
                    InventoryPhysicalCountList inventoryPhysicalCountListBL = new InventoryPhysicalCountList(executionContext, inventoryPhysicalCountDTOList);
                    result = inventoryPhysicalCountListBL.Save(parafaitDBTransaction.SQLTrx);
                    parafaitDBTransaction.EndTransaction();
                    log.LogMethodExit(result);
                    return result;
                }
            });
        }

        public async Task<List<InventoryPhysicalCountDTO>> UpdatePhysicalCountStatus(List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList)
        {
            return await Task<List<InventoryPhysicalCountDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(inventoryPhysicalCountDTOList);
                List<InventoryPhysicalCountDTO> result = new List<InventoryPhysicalCountDTO>();
                if (inventoryPhysicalCountDTOList == null)
                {
                    throw new ValidationException("inventoryPhysicalCountDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (InventoryPhysicalCountDTO inventoryPhysicalCountDTO in inventoryPhysicalCountDTOList)
                    {
                        parafaitDBTrx.BeginTransaction();
                        InventoryPhysicalCount inventoryPhysicalCountsBL = new InventoryPhysicalCount(executionContext, inventoryPhysicalCountDTO);
                        inventoryPhysicalCountsBL.UpdatePhysicalCountStatus(parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                        result.Add(inventoryPhysicalCountsBL.GetInventoryPhysicalCountDTO);
                    }
                    log.LogMethodExit(result);
                    return result;
                }
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
