/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalInventoryAdjustmentsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      29-Dec-2020       Abhishek                 Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public class LocalInventoryAdjustmentsUseCases : LocalUseCases, IInventoryAdjustmentsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public LocalInventoryAdjustmentsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<InventoryAdjustmentsSummaryDTO>> GetInventoryAdjustmentsSummary(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> parameters,
                                                                                               string advancedSearch = null, string pivotColumns = null, bool ignoreWastage = true, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)    
        {
            return await Task<List<InventoryAdjustmentsSummaryDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
                int siteId = GetSiteId();
                List<InventoryAdjustmentsSummaryDTO> inventoryAdjustmentsDTOList = inventoryAdjustmentsList.GetAllInventoryAdjustmentsSummaryDTO(parameters, advancedSearch, pivotColumns, sqlTransaction, ignoreWastage, currentPage, pageSize);
                log.LogMethodExit(inventoryAdjustmentsDTOList);
                return inventoryAdjustmentsDTOList;
            });
        }

        public async Task<int> GetInventoryAdjustmentCount(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> parameters,
                                                                                               string advancedSearch = null, string pivotColumns = null, 
                                                                                               bool ignoreWastage = true, int currentPage = 0, int pageSize = 10,
                                                                                               SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
                int siteId = GetSiteId();
                int inventoryAdjustmentCount = inventoryAdjustmentsList.GetInventoryAdjustmentsSummaryCount(parameters, advancedSearch, pivotColumns, sqlTransaction);
                log.LogMethodExit(inventoryAdjustmentCount);
                return inventoryAdjustmentCount;
            });
        }

        public async Task<string> SaveInventoryAdjustments(List<InventoryAdjustmentsDTO> inventoryAdjustmentsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(inventoryAdjustmentsDTOList);
                string result = string.Empty;
                if (inventoryAdjustmentsDTOList == null)
                {
                    throw new ValidationException("inventoryAdjustmentsDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (InventoryAdjustmentsDTO inventoryAdjustmentsDTO in inventoryAdjustmentsDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            InventoryAdjustmentsBL inventoryAdjustmentsBL = new InventoryAdjustmentsBL(inventoryAdjustmentsDTO, executionContext);
                            inventoryAdjustmentsBL.SaveInventory(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
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

        /// <summary>
        /// Returns the Inventory Total Cost
        /// </summary>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public async Task<string> GetInventoryTotalCost(SqlTransaction sqlTransaction = null)
        {
            {
                return await Task<string>.Factory.StartNew(() =>
                {
                    log.LogMethodEntry();
                    InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
                    string inventoryAdjustmentCount = inventoryAdjustmentsList.GetInventoryTotalCost(sqlTransaction);
                    log.LogMethodExit(inventoryAdjustmentCount);
                    return inventoryAdjustmentCount;
                });
            }
        }
    }
}
