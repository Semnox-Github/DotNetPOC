/********************************************************************************************
 * Project Name - Inventory
 * Description  - ReceivePurchaseOrderController used to receive PO .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.0    15-Jun-2021   Girish Kundar           Created.
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
    public class ReceivePurchaseOrderController : ApiController
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object InventoryReceiveLinesDTO
        /// </summary>
        /// <param name="inventoryReceiveLinesDTOList">inventoryReceiveLinesDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Inventory/PurchaseOrder/{purchaseOrderId}/Receive")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int purchaseOrderId, [FromBody]List<InventoryReceiptDTO> inventoryReceiptDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(purchaseOrderId, inventoryReceiptDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (inventoryReceiptDTOList == null || inventoryReceiptDTOList.Any() == false)
                {
                    log.LogMethodExit(inventoryReceiptDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (purchaseOrderId < 0)
                {
                    log.LogMethodExit(purchaseOrderId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IReceiptUseCases receiptUseCases = InventoryUseCaseFactory.GetReceiptsUseCases(executionContext);
                PurchaseOrderDTO purchaseOrderDTO = await receiptUseCases.ReceivePurchaseOrder(purchaseOrderId, inventoryReceiptDTOList);
                log.LogVariableState("purchaseOrderDTO : ", purchaseOrderDTO);
                log.LogMethodExit(purchaseOrderDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = purchaseOrderDTO });
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
