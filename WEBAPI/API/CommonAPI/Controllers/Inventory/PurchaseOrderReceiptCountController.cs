/********************************************************************************************
 * Project Name - ReceiptCount Controller
 * Description  - Created ReceiptCount Controller
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1   11-feb-2021   Likhitha Reddy            created
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class PurchaseOrderReceiptCountController : ApiController
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
        [Route("api/Inventory/ReceiptCounts")]
        public async Task<HttpResponseMessage> Get(int receiptId, int purchaseOrderId = -1, string vendorBillNumber = null,
                                       string vendorName = null, string grn = null, string isActive = null,  int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(receiptId, purchaseOrderId,  vendorBillNumber, vendorName, grn, isActive);
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
               
                InventoryReceiptList inventoryReceiptList = new InventoryReceiptList(executionContext);
                IReceiptUseCases receiptUseCases = InventoryUseCaseFactory.GetReceiptsUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalNoOfReceipts = await receiptUseCases.GetReceiptCount(receiptSearchParameter); 
                log.LogVariableState("totalNoOfReceipts", totalNoOfReceipts);
                totalNoOfPages = (totalNoOfReceipts / pageSize) + ((totalNoOfReceipts % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalNoOfReceipts);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalNoOfReceipts, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    data = ExceptionSerializer.Serialize(ex)
                });
            }
        }
    }
}