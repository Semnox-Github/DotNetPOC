/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch Inventory ReceiveLines Count.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.150.0     03-Nov-2022  Abhishek         Web Inventory UI resdesign changes with REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class PurchaseOrderReceiveLineCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the InventoryReceiveLinesCount.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/PurchaseOrderReceiveLinesCount")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int purchaseOrderReceiveLineId = -1, int purchaseOrderId = -1, int uomId=-1, int productId = -1,
                                                   int quantity = -1, int locationId = -1, int purchaseOrderLineId = -1, int receiptId=-1, bool buildChildRecords = false, bool loadActiveChild = false,
                                                   string vendorBillNumber=null, string vendorItemCode = null, string isReceived = null, int currentPage = 0, int pageSize = 10)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, purchaseOrderReceiveLineId, purchaseOrderId, currentPage, pageSize);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>> searchParameters = new List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.ISACTIVE, isActive.ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(isReceived))
                {
                    searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.IS_RECEIVED, isReceived.ToString()));
                }
                if (!string.IsNullOrEmpty(vendorItemCode))
                {
                    searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.VENDOR_ITEM_CODE, vendorItemCode.ToString()));
                }
                if (!string.IsNullOrEmpty(vendorBillNumber))
                {
                    searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.VENDOR_BILL_NUMBER, vendorBillNumber.ToString()));
                }
                if (purchaseOrderReceiveLineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_RECEIVE_LINE_ID, purchaseOrderReceiveLineId.ToString()));
                }
                if (purchaseOrderId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_ID, purchaseOrderId.ToString()));
                }
                if (productId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PRODUCT_ID, productId.ToString()));
                }
                if (quantity > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.QUANTITY, quantity.ToString()));
                }
                if (locationId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.LOCATION_ID, locationId.ToString()));
                }
                if (purchaseOrderLineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_LINE_ID, purchaseOrderLineId.ToString()));
                }
                if (receiptId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.RECEIPT_ID, receiptId.ToString()));
                }
                if (uomId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.UOM_ID, uomId.ToString()));
                }

                IInventoryReceiveLinesUseCases inventoryReceiveLinesUseCases = InventoryUseCaseFactory.GetInventoryReceiveLinesUseCases(executionContext);

                int totalNoOfPages = 0;
                int totalNoOfReceiveLines = await inventoryReceiveLinesUseCases.GetInventoryReceiveLineCounts(searchParameters);
                log.LogVariableState("totalNoOfReceiveLines", totalNoOfReceiveLines);
                totalNoOfPages = (totalNoOfReceiveLines / pageSize) + ((totalNoOfReceiveLines % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalNoOfReceiveLines);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalNoOfReceiveLines, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });
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
