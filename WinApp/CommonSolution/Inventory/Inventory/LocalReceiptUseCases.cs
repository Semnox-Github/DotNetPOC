/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalReceiptUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0      15-Dec-2020       Abhishek                 Created 
 *2.130        04-Jun-2021     Girish Kundar              Modified - POS stock changes
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    public class LocalReceiptUseCases : LocalUseCases, IReceiptUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public LocalReceiptUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<InventoryReceiptDTO>> GetReceipts(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> parameters,
                                                                 bool loadChildRecords = false, bool activeChildRecords = true, bool loadReturnQuantity = false, int currentPage = 0,
                                                                 int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<InventoryReceiptDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);

                InventoryReceiptList inventoryReceiptListBL = new InventoryReceiptList(executionContext);
                List<InventoryReceiptDTO> inventoryReceiptDTOList = inventoryReceiptListBL.GetAllInventoryReceipt(parameters, loadChildRecords, activeChildRecords, loadReturnQuantity, currentPage, pageSize, sqlTransaction);
                log.LogMethodExit(inventoryReceiptDTOList);
                return inventoryReceiptDTOList;
            });
        }

        public async Task<int> GetReceiptCount(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> parameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);

                InventoryReceiptList inventoryReceiptList = new InventoryReceiptList(executionContext);
                int inventoryReceiptCount = inventoryReceiptList.GetInventoryReceiptsCount(parameters);
                log.LogMethodExit(inventoryReceiptCount);
                return inventoryReceiptCount;
            });
        }

        public async Task<string> SaveReceipts(List<InventoryReceiptDTO> inventoryReceiptDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(inventoryReceiptDTOList);
                string result = string.Empty;
                if (inventoryReceiptDTOList == null)
                {
                    throw new ValidationException("inventoryReceiptDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (InventoryReceiptDTO inventoryReceiptDTO in inventoryReceiptDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            InventoryReceiptsBL inventoryReceiptsBL = new InventoryReceiptsBL(inventoryReceiptDTO, executionContext);//note
                            inventoryReceiptsBL.Save(parafaitDBTrx.SQLTrx);
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

        public async Task<PurchaseOrderDTO> ReceivePurchaseOrder(int purchaseOrderId, List<InventoryReceiptDTO> inventoryReceiptDTOList)
        {
            return await Task<PurchaseOrderDTO>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(purchaseOrderId ,inventoryReceiptDTOList);
                PurchaseOrderDTO result = null;
                if (purchaseOrderId < 0 || inventoryReceiptDTOList == null)
                {
                    throw new ValidationException("inventoryReceiptDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            PurchaseOrder purchaseOrderBL = new PurchaseOrder(purchaseOrderId,executionContext, parafaitDBTrx.SQLTrx,true,true);
                            result = purchaseOrderBL.ReceiveLines(inventoryReceiptDTOList, parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw ;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ;
                        }
                }
                log.LogMethodExit(result);
                return result;
            });
        }
   
    }
}
