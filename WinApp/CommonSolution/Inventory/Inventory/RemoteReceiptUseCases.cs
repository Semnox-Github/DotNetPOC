/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteReceiptUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     15-Dec-2020       Abhishek                  Created 
 *2.130        04-Jun-2021     Girish Kundar              Modified - POS stock changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class RemoteReceiptUseCases : RemoteUseCases, IReceiptUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string RECEIPTS_URL = "api/Inventory/Receipts";
        private const string RECEIPT_COUNTS_URL = "api/Inventory/ReceiptCounts";
        private string RECEIVE_PO_URL = "api/Inventory/PurchaseOrder/{purchaseOrderId}/Receive";

        public RemoteReceiptUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<InventoryReceiptDTO>> GetReceipts(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> parameters,
                                                                bool loadChildRecords = false, bool activeChildRecords = true, bool loadReturnQuantity = false, int currentPage = 0,
                                                                int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords", loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords", activeChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadReturnQuantity", loadReturnQuantity.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage", currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize", pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<InventoryReceiptDTO> inventoryReceiptDTOList = await Get<List<InventoryReceiptDTO>>(RECEIPTS_URL, searchParameterList);
                log.LogMethodExit(inventoryReceiptDTOList);
                return inventoryReceiptDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetReceiptCount(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> parameters,
                                                                SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                int results = await Get<int>(RECEIPT_COUNTS_URL, searchParameterList);
                log.LogMethodExit(results);
                return results;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> inventoryReceiptsSearchParams)
        {
            log.LogMethodEntry(inventoryReceiptsSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string> searchParameter in inventoryReceiptsSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case InventoryReceiptDTO.SearchByInventoryReceiptParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIPT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("receiptId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiptDTO.SearchByInventoryReceiptParameters.PURCHASE_ORDER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("purchaseOrderId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDOR_BILL_NUMBER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("vendorBillNumber".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiptDTO.SearchByInventoryReceiptParameters.GRN:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("GRN".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDORNAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("vendorName".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveReceipts(List<InventoryReceiptDTO> inventoryReceiptDTOList)
        {
            log.LogMethodEntry(inventoryReceiptDTOList);
            try
            {
                string responseString = await Post<string>(RECEIPTS_URL, inventoryReceiptDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<PurchaseOrderDTO> ReceivePurchaseOrder( int purchaseOrderId,List<InventoryReceiptDTO> inventoryReceiptDTOList)
        {
            log.LogMethodEntry(purchaseOrderId, inventoryReceiptDTOList);
            try
            {
                RECEIVE_PO_URL = "api/Inventory/PurchaseOrder/"+ purchaseOrderId + "/Receive";
                PurchaseOrderDTO PurchaseOrderDTO = await Post<PurchaseOrderDTO>(RECEIVE_PO_URL, inventoryReceiptDTOList);
                log.LogMethodExit(PurchaseOrderDTO);
                return PurchaseOrderDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
