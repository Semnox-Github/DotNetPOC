/********************************************************************************************
* Project Name - CommonAPI
* Description  - ReceiptController - Created to get the receipt
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.110.00   08-Dec-2020     Abhishek              Created : As part of Inventory UI Redesign      
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Net;
using Semnox.Core.GenericUtilities;
using System.Threading.Tasks;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class ReceiptController : ApiController
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
        public async Task<HttpResponseMessage> Get(int receiptId, int purchaseOrderId = -1, bool buildChildRecords = false, string vendorBillNumber = null,
                                       string vendorName = null, string grn = null, string isActive = null, bool loadActiveChild = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(receiptId,purchaseOrderId, buildChildRecords, vendorBillNumber, vendorName, grn, isActive, loadActiveChild);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> receiptSearchParameter = new List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>>();
                receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (receiptId > 0)
                {
                    receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIPT_ID, Convert.ToString(receiptId)));
                }
                if (purchaseOrderId > 0)
                {
                    receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.PURCHASE_ORDER_ID, Convert.ToString(purchaseOrderId)));
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
                //if (string.IsNullOrEmpty(isActive) == false
                //{
                //    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                //    {
                //        loadActiveChild = true;
                //        receiptSearchParameter.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.i, isActive));
                //    }
                //}
                //int totalNoOfPages = 0;
                //int totalNoOfReceipts = await Task<int>.Factory.StartNew(() => { return inventoryReceiptBL.GetReceiptssCount(receiptSearchParameter, null); });
                //log.LogVariableState("totalNoOfReceipts", totalNoOfReceipts);
                //totalNoOfPages = (totalNoOfReceipts / pageSize) + ((totalNoOfReceipts % pageSize) > 0 ? 1 : 0);

                //IReceiptsUseCases receiptsUseCases = InventoryUseCaseFactory.GetReceiptsUseCases(executionContext);
                //List<InventoryReceiptDTO> receiptsDTOList = await requisitionUseCases.GetReceipts(receiptSearchParameter, buildChildRecords, loadActiveChild,currentPage, pageSize, null);
                //log.LogMethodExit(receiptsDTOList);
                //return Request.CreateResponse(HttpStatusCode.OK, new { data = receiptsDTOList, currentPageNo = currentPage, TotalCount = totalNoOfRequisitions });
                InventoryReceiptDTO inventoryReceiptDTO = new InventoryReceiptDTO();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryReceiptDTO });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object of RecipeEstimationHeaderDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/Receipts")]
        public HttpResponseMessage Post([FromBody] List<InventoryReceiptDTO> inventoryReceiptDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryReceiptDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (inventoryReceiptDTOList == null || inventoryReceiptDTOList.Any(x => x.ReceiptId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (inventoryReceiptDTOList != null && inventoryReceiptDTOList.Any())
                {
                    //IReceiptsUseCases receiptsUseCases = InventoryUseCaseFactory.GetReceiptsUseCases(executionContext);
                    //receiptsUseCases.SaveReceipts(inventoryReceiptDTOList);
                    //log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
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
        /// Post the JSON Object of RecipeEstimationHeaderDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Authorize]
        [Route("api/Inventory/Receipts")]
        public HttpResponseMessage Put([FromBody] List<InventoryReceiptDTO> inventoryReceiptDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryReceiptDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (inventoryReceiptDTOList == null || inventoryReceiptDTOList.Any(x => x.ReceiptId < -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (inventoryReceiptDTOList != null && inventoryReceiptDTOList.Any())
                {
                    //IReceiptsUseCases receiptsUseCases = InventoryUseCaseFactory.GetReceiptsUseCases(executionContext);
                    //receiptsUseCases.SaveReceipts(inventoryReceiptDTOList);
                    //log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
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
    }
}