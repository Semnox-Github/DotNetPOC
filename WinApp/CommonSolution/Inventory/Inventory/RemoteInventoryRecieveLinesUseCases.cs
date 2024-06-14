/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteInventoryReceiveLinesUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     24-Dec-2020      Prajwal S                Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class RemoteInventoryReceiveLinesUseCases : RemoteUseCases, IInventoryReceiveLinesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string InventoryReceiveLines_URL = "api/Inventory/ReceiveLines";
        private const string PurchaseOrderReceiveLines_URL = "api/Inventory/PurchaseOrderReceiveLines";
        private const string InventoryReceiveLineCounts_URL = "api/Inventory/PurchaseOrderReceiveLineCounts";
        private string INVENTORY_RECEIVE_LINES_URL = "api/Inventory/Receive/{receiptId}/ReceiveLines";

        public RemoteInventoryReceiveLinesUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<InventoryReceiveLinesDTO>> GetInventoryReceiveLines(List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>>
                          parameters, int currentPage = 0, int pageSize = 0, bool loadChildRecords = false, bool loadActiveChild = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChild".ToString(), loadActiveChild.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                
                List<InventoryReceiveLinesDTO> result = await Get<List<InventoryReceiveLinesDTO>>(InventoryReceiveLines_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetInventoryReceiveLineCounts(List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>>parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {

                int result = await Get<int>(InventoryReceiveLineCounts_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.LOCATION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("locationId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("purchaseOrderId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.IS_RECEIVED:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isReceived".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.VENDOR_ITEM_CODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("vendorItemCode".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.VENDOR_BILL_NUMBER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("vendorBillNumber".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_RECEIVE_LINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("purchaseOrderreceiveLineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_LINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("purchaseOrderLineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.UOM_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("uomId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.QUANTITY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("quantity".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.RECEIPT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("receiptId".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<InventoryReceiptDTO> AddInventoryReceiveLines(int receiptid, List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList)
        {
            log.LogMethodEntry(receiptid, inventoryReceiveLinesDTOList);
            try
            {
                INVENTORY_RECEIVE_LINES_URL = "api/Inventory/Receive/" + receiptid + "/ReceiveLines";
                InventoryReceiptDTO responseString = await Post<InventoryReceiptDTO>(INVENTORY_RECEIVE_LINES_URL, inventoryReceiveLinesDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<InventoryReceiptDTO> UpdateInventoryReceiveLines(int receiptid, List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList)
        {
            log.LogMethodEntry(receiptid, inventoryReceiveLinesDTOList);
            try
            {
                INVENTORY_RECEIVE_LINES_URL = "api/Inventory/Receive/" + receiptid + "/ReceiveLines";
                InventoryReceiptDTO responseString = await Post<InventoryReceiptDTO>(INVENTORY_RECEIVE_LINES_URL, inventoryReceiveLinesDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> SaveInventoryReceiveLines(List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList)
        {
            log.LogMethodEntry(inventoryReceiveLinesDTOList);
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(inventoryReceiveLinesDTOList);
                string responseString = await Post<string>(PurchaseOrderReceiveLines_URL, content);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
