/********************************************************************************************
 * Project Name - RequisitionCount Controller
 * Description  - Created RequisitionCount Controller
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1   11-feb-2021   Likhitha Reddy            created
 ***************************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Parafait.Inventory;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.Inventory.Requisition;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class RequisitionCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of RequisitionDTO
        /// </summary>
        /// <param name="requisitionId">requisitionId</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/RequisitionCounts")]
        public async Task<HttpResponseMessage> Get(int requisitionId = -1, string requisitionNumber = null, int requisitionType = -1, string status = null,
                                                  string isActive = null, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(requisitionId, requisitionNumber, requisitionType, status, isActive, currentPage, pageSize);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                RequisitionList requisitionListBL = new RequisitionList(executionContext);
                List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> requisitionSearchParameter = new List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>>();
                requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (requisitionId > 0)
                {
                    requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_ID, Convert.ToString(requisitionId)));
                }
                if (!string.IsNullOrEmpty(requisitionNumber))
                {
                    requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_NUMBER, requisitionNumber));
                }
                if (requisitionType > 0)
                {
                    requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_TYPE, Convert.ToString(requisitionType)));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.STATUS, status));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.ACTIVE_FLAG, isActive));
                    }
                }
                RequisitionDTO requisitionDTO = new RequisitionDTO();
                IRequisitionUseCases requisitionUseCases = InventoryUseCaseFactory.GetRequisitionUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalNoOfRequisitions = await requisitionUseCases.GetRequisitionCount(requisitionSearchParameter);
                log.LogVariableState("totalNoOfRequisitions", totalNoOfRequisitions);
                totalNoOfPages = (totalNoOfRequisitions / pageSize) + ((totalNoOfRequisitions % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalNoOfRequisitions);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalNoOfRequisitions, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });
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