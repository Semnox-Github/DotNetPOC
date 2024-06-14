/********************************************************************************************
* Project Name - CommonAPI
* Description  - ReceiptController - Created to get the receipt
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.110.00   08-Dec-2020   Mushahid Faizan              Created : As part of Inventory UI Redesign   
*2.150.0    22-Sep-2022   Abhishek                     Modified : Added search parameter orderNumber
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Net;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class PurchaseOrderReceiptController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of InventoryReceiptDTO
        /// </summary>
        /// <param name="receiptId">ReceiptId</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="activeRecordsOnly">activeRecordsOnly</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Receipts")]
        public async Task<HttpResponseMessage> Get(int receiptId = -1, int purchaseOrderId = -1, bool buildChildRecords = false, string vendorBillNumber = null,
                                               string vendorName = null,string vendorNameList = null, string grn = null, string isActive = null, bool loadActiveChild = false,
                                               bool loadReturnQuantity = false, int documentTypeId=-1, string documentTypeIdList = null, int productId=-1, string orderNumber = null, int currentPage = 0, int pageSize = 10)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(receiptId,purchaseOrderId, buildChildRecords, vendorBillNumber, grn, isActive, loadActiveChild, loadReturnQuantity,
                                    documentTypeId, documentTypeIdList, productId, orderNumber, currentPage, pageSize);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                InventoryReceiptList inventoryReceiptListBL = new InventoryReceiptList(executionContext);
                List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> receiptSearchParameter = new List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>>();
                receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (receiptId > -1)
                {
                    receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIPT_ID, Convert.ToString(receiptId)));
                }
                if (purchaseOrderId > -1)
                {
                    receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.PURCHASE_ORDER_ID, Convert.ToString(purchaseOrderId)));
                }
                if (productId > -1)
                {
                    receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.HAS_PRODUCT_ID, Convert.ToString(productId)));
                }
                if (!string.IsNullOrEmpty(vendorBillNumber))
                {
                    receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDOR_BILL_NUMBER, vendorBillNumber));
                }
                if (!string.IsNullOrEmpty(vendorName))
                {
                    receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDORNAME, vendorName));
                }
                if (!string.IsNullOrEmpty(grn))
                {
                    receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.GRN, grn));
                }
                if (documentTypeId > -1)
                {
                    receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.DOCUMENT_TYPE_ID, documentTypeId.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(documentTypeIdList))
                {
                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<int> documentList = new List<int>();

                    documentList = documentTypeIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    String docymentTypeIdListString = String.Join(",", documentList.ToArray());
                    receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.DOCUMENT_TYPE_ID_LIST, docymentTypeIdListString));
                }

                if (!string.IsNullOrEmpty(vendorNameList))
                {

                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<string> vendorList = new List<string>();

                    vendorList = vendorNameList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).ToList();

                    String vendorNameListString = String.Join(",", vendorList.ToArray());

                    receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDORNAME_LIST, vendorNameListString));
                }

                if (!string.IsNullOrEmpty(orderNumber))
                {
                    receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.ORDERNUMBER, orderNumber));
                }

                int totalNoOfPages = 0;
                int totalNoOfReceipts = await Task<int>.Factory.StartNew(() => { return inventoryReceiptListBL.GetInventoryReceiptsCount(receiptSearchParameter, null); });
                log.LogVariableState("totalNoOfReceipts", totalNoOfReceipts);
                totalNoOfPages = (totalNoOfReceipts / pageSize) + ((totalNoOfReceipts % pageSize) > 0 ? 1 : 0);

                IReceiptUseCases receiptsUseCases = InventoryUseCaseFactory.GetReceiptsUseCases(executionContext);
                List<InventoryReceiptDTO> receiptsDTOList = await receiptsUseCases.GetReceipts(receiptSearchParameter, buildChildRecords, loadActiveChild, loadReturnQuantity, currentPage, pageSize, null);
                log.LogMethodExit(receiptsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = receiptsDTOList, currentPageNo = currentPage, TotalCount = totalNoOfReceipts });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object of InventoryReceiptDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/Receipts")]
        public async Task<HttpResponseMessage> Post([FromBody] List<InventoryReceiptDTO> inventoryReceiptDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryReceiptDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (inventoryReceiptDTOList == null)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (inventoryReceiptDTOList != null && inventoryReceiptDTOList.Any())
                {
                    IReceiptUseCases receiptsUseCases = InventoryUseCaseFactory.GetReceiptsUseCases(executionContext);
                    await receiptsUseCases.SaveReceipts(inventoryReceiptDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryReceiptDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Put the JSON Object of InventoryReceiptDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Authorize]
        [Route("api/Inventory/Receipts")]
        public async Task<HttpResponseMessage> Put([FromBody] List<InventoryReceiptDTO> inventoryReceiptDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryReceiptDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (inventoryReceiptDTOList == null || inventoryReceiptDTOList.Any(x => x.ReceiptId < -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (inventoryReceiptDTOList != null && inventoryReceiptDTOList.Any())
                {
                    IReceiptUseCases receiptsUseCases = InventoryUseCaseFactory.GetReceiptsUseCases(executionContext);
                    await receiptsUseCases.SaveReceipts(inventoryReceiptDTOList);
                    log.LogMethodExit(inventoryReceiptDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryReceiptDTOList });
                }
                else
                {
                    log.LogMethodExit(inventoryReceiptDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = inventoryReceiptDTOList });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}