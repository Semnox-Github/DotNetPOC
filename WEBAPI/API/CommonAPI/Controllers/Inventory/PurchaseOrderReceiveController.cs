/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the Inventory ReceiveLines .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    15-Dec-2020  Mushahid Faizan         Created.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class PurchaseOrderReceiveController : ApiController
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the InventoryReceiveLines.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/PurchaseOrderReceiveLines")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int purchaseOrderReceiveLineId = -1, int purchaseOrderId = -1, int uomId=-1, int productId = -1,
                                                   int quantity = -1, int locationId = -1, int purchaseOrderLineId = -1, int receiptId=-1, bool buildChildRecords = false, bool loadActiveChild = false,
                                                   string vendorBillNumber=null, string vendorItemCode = null, string isReceived = null, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, purchaseOrderReceiveLineId, purchaseOrderId, currentPage, pageSize);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

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

                InventoryReceiptLineList inventoryReceiveLinesList = new InventoryReceiptLineList(executionContext);

                int totalNoOfPages = 0;
                int totalCount = await Task<int>.Factory.StartNew(() => { return inventoryReceiveLinesList.GetInventoryReceiveLinesCount(searchParameters, null); });
                log.LogVariableState("totalCount", totalCount);
                totalNoOfPages = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

                IInventoryReceiveLinesUseCases inventoryReceiveLinesUseCases = InventoryUseCaseFactory.GetInventoryReceiveLinesUseCases(executionContext);
                List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList = await inventoryReceiveLinesUseCases.GetInventoryReceiveLines(searchParameters, currentPage, pageSize);

                log.LogMethodExit(inventoryReceiveLinesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryReceiveLinesDTOList, currentPageNo = currentPage, TotalCount = totalCount });

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
        /// Post the JSON Object InventoryReceiveLinesDTO
        /// </summary>
        /// <param name="inventoryReceiveLinesDTOList">inventoryReceiveLinesDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Inventory/PurchaseOrderReceiveLines")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryReceiveLinesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (inventoryReceiveLinesDTOList == null || inventoryReceiveLinesDTOList.Any(a => a.PurchaseOrderReceiveLineId > 0))
                {
                    log.LogMethodExit(inventoryReceiveLinesDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IInventoryReceiveLinesUseCases inventoryReceiveLinesUseCases = InventoryUseCaseFactory.GetInventoryReceiveLinesUseCases(executionContext);
                inventoryReceiveLinesUseCases.SaveInventoryReceiveLines(inventoryReceiveLinesDTOList);

                log.LogMethodExit(inventoryReceiveLinesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
        /// Update the JSON Object InventoryReceiveLinesDTO
        /// </summary>
        /// <param name="inventoryReceiveLinesDTOList">inventoryReceiveLinesDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/PurchaseOrderReceiveLines")]
        [Authorize]
        public HttpResponseMessage Put([FromBody] List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryReceiveLinesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (inventoryReceiveLinesDTOList == null || inventoryReceiveLinesDTOList.Any(a => a.PurchaseOrderReceiveLineId < 0))
                {
                    log.LogMethodExit(inventoryReceiveLinesDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IInventoryReceiveLinesUseCases inventoryReceiveLinesUseCases = InventoryUseCaseFactory.GetInventoryReceiveLinesUseCases(executionContext);
                inventoryReceiveLinesUseCases.SaveInventoryReceiveLines(inventoryReceiveLinesDTOList);

                log.LogMethodExit(inventoryReceiveLinesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
