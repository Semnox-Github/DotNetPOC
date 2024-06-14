/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the Purchase Orders .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1    26-Feb-2021  Mushahid Faizan         Created.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryReceiveLinesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object inventoryReceiptDTO
        /// </summary>
        /// <param name="inventoryReceiveLinesDTOList">inventoryReceiveLinesDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Inventory/Receive/{receiptId}/ReceiveLines")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList, [FromUri] int receiptId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryReceiveLinesDTOList);
                //  ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
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
                InventoryReceiptDTO inventoryReceiptDTO = await inventoryReceiveLinesUseCases.AddInventoryReceiveLines(receiptId, inventoryReceiveLinesDTOList);

                log.LogMethodExit(inventoryReceiveLinesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryReceiptDTO });
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
        /// <summary>
        /// Post the JSON Object inventoryReceiptDTO
        /// </summary>
        /// <param name="inventoryReceiveLinesDTOList">inventoryReceiveLinesDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/Receive/{receiptId}/ReceiveLines")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList, [FromUri] int receiptId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryReceiveLinesDTOList);
                //  ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
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
                InventoryReceiptDTO inventoryReceiptDTO = await inventoryReceiveLinesUseCases.UpdateInventoryReceiveLines(receiptId, inventoryReceiveLinesDTOList);

                log.LogMethodExit(inventoryReceiveLinesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryReceiptDTO });
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
