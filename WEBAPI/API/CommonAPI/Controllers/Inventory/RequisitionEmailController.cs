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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Inventory.Requisition;
using Semnox.Parafait.Reports;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class RequisitionEmailController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object MessagingRequestDTO
        /// </summary>
        /// <param name="messagingRequestDTOList">messagingRequestDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Inventory/Requisition/{requisitionId}/Email")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int requisitionId,List<MessagingRequestDTO> messagingRequestDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(requisitionId);
                //  ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (requisitionId < -1 )
                {
                    log.LogMethodExit(requisitionId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IRequisitionUseCases requisitionUseCases = InventoryUseCaseFactory.GetRequisitionUseCases(executionContext);
                MessagingRequestDTO messagingRequestDTO = await requisitionUseCases.SendRequisitionEmail(requisitionId, messagingRequestDTOList);

                log.LogMethodExit(messagingRequestDTO);
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
