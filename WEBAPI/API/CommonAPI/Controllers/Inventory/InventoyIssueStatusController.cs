/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the Issue Headers and Lines .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1    01-Mar-2021  Mushahid Faizan         Created.
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
    public class InventoryIssueStatusController : ApiController
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object InventoryIssueHeaderDTO
        /// </summary>
        /// <param name="inventoryIssueHeaderStatus">inventoryIssueHeaderDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/Issue/{issueId}/Status")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromUri] int issueId, [FromUri] string issueStatus)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(issueStatus);
                //  ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (issueId < 0 || string.IsNullOrWhiteSpace(issueStatus))
                {
                    log.LogMethodExit(issueId, issueStatus);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IInventoryIssueHeaderUseCases inventoryIssueHeaderUseCases = InventoryUseCaseFactory.GetInventoryIssueHeadersUseCases(executionContext);
                InventoryIssueHeaderDTO inventoryIssueHeaderDTO = await inventoryIssueHeaderUseCases.UpdateIssueStatus(issueId, issueStatus);

                log.LogMethodExit(issueStatus);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryIssueHeaderDTO });
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
