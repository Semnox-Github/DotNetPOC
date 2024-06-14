/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the Requisition .
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
using Semnox.Parafait.Inventory.Requisition;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class RequisitionLinesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object RequisitionDTO
        /// </summary>
        /// <param name="requisitionLineDTOList">requisitionLineDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Inventory/Requisition/{requisitionId}/RequisitionLines")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<RequisitionLinesDTO> requisitionLineDTOList, [FromUri] int requisitionId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(requisitionLineDTOList);
                //  ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (requisitionLineDTOList == null || requisitionLineDTOList.Any(a => a.RequisitionLineId > 0))
                {
                    log.LogMethodExit(requisitionLineDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IRequisitionUseCases requisitionUseCases = InventoryUseCaseFactory.GetRequisitionUseCases(executionContext);
                RequisitionDTO requisitionDTO = await requisitionUseCases.AddRequisitionLines(requisitionId, requisitionLineDTOList);

                log.LogMethodExit(requisitionLineDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = requisitionDTO });
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
        /// Post the JSON Object RequisitionDTO
        /// </summary>
        /// <param name="requisitionLineDTOList">requisitionLineDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/Requisition/{requisitionId}/RequisitionLines")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<RequisitionLinesDTO> requisitionLineDTOList, [FromUri] int requisitionId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(requisitionLineDTOList);
                //  ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (requisitionLineDTOList == null || requisitionLineDTOList.Any(a => a.RequisitionLineId < 0))
                {
                    log.LogMethodExit(requisitionLineDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IRequisitionUseCases requisitionUseCases = InventoryUseCaseFactory.GetRequisitionUseCases(executionContext);
                RequisitionDTO requisitionDTO = await requisitionUseCases.UpdateRequisitionLines(requisitionId, requisitionLineDTOList);

                log.LogMethodExit(requisitionLineDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = requisitionDTO });
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
