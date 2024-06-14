/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to send email for the Purchase Orders .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.120.0    04-Mar-2021  Mushahid Faizan         Created.
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
using Semnox.Parafait.Communication;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class PurchaseOrderEmailController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Post the JSON Object MessagingRequestDTO
        /// </summary>
        /// <param name="messagingRequestDTOList">messagingRequestDTOList</param>
        [HttpPost]
        [Route("api/Inventory/PurchaseOrder/{purchaseOrderId}/Email")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int purchaseOrderId, List<MessagingRequestDTO> messagingRequestDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(purchaseOrderId);
                //  ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (purchaseOrderId < -1)
                {
                    log.LogMethodExit(purchaseOrderId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IPurchaseOrderUseCases poUseCases = InventoryUseCaseFactory.GetPurchaseOrderUseCases(executionContext);
                MessagingRequestDTO messagingRequestDTO = await poUseCases.SendPurchaseOrderEmail(purchaseOrderId, messagingRequestDTOList);

                log.LogMethodExit(purchaseOrderId);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = messagingRequestDTO });
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
