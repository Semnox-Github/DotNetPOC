/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalInventoryWastageUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      28-Dec-2020       Abhishek                 Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.Inventory
{
    public class LocalInventoryWastageUseCases : LocalUseCases, IInventoryWastageUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public LocalInventoryWastageUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<InventoryWastageSummaryDTO>> GetInventoryWastages(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> parameters,
                                                                                 int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)    
        {
            return await Task<List<InventoryWastageSummaryDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
                int siteId = GetSiteId();
                List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = inventoryAdjustmentsList.GetInventoryWastageSummaryList(parameters, currentPage, pageSize );
                log.LogMethodExit(inventoryWastageSummaryDTOList);
                return inventoryWastageSummaryDTOList;
            });
        }

        public async Task<int> GetInventoryWastageCount(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> parameters,
                                                                                  SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
                int siteId = GetSiteId();
                int inventoryWastageCount = inventoryAdjustmentsList.GetInventoryWastagesCount(parameters, sqlTransaction);
                log.LogMethodExit(inventoryWastageCount);
                return inventoryWastageCount;
            });
        }

        public async Task<string> SaveInventoryWastages(List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(inventoryWastageSummaryDTOList);
                string result = string.Empty;
                if (inventoryWastageSummaryDTOList == null)
                {
                    throw new ValidationException("inventoryWastageSummaryDTOList is Empty");
                }              
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        InventoryWastageSummaryListBL inventoryWastageSummaryListBL = new InventoryWastageSummaryListBL(executionContext, inventoryWastageSummaryDTOList);//note
                        inventoryWastageSummaryListBL.Save(parafaitDBTrx.SQLTrx);
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
    }
}
