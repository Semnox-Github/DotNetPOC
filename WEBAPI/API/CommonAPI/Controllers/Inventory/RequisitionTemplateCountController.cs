/********************************************************************************************
 * Project Name - RequisitionTemplateCount Controller
 * Description  - Created RequisitionTemplateCount Controller
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
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.Inventory.Requisition;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class RequisitionTemplateCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of RequisitionTemplatesDTO
        /// </summary>
        /// <param name="templateId">templateId</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="activeRecordsOnly">activeRecordsOnly</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/RequisitionTemplateCounts")]
        public async Task<HttpResponseMessage> Get(int templateId = -1, string templateName = null, int requisitionType = -1, string status = null,
                                                   bool buildChildRecords = false, string isActive = null, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(templateId, templateName, requisitionType, status, buildChildRecords, isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> requisitionTemplatesSearchParameter = new List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>>();
                requisitionTemplatesSearchParameter.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.SITEID, Convert.ToString(executionContext.GetSiteId())));
                if (templateId > 0)
                {
                    requisitionTemplatesSearchParameter.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.TEMPLATE_ID, Convert.ToString(templateId)));
                }
                if (!string.IsNullOrEmpty(templateName))
                {
                    requisitionTemplatesSearchParameter.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.TEMPLATE_NAME, templateName));
                }
                if (requisitionType > 0)
                {
                    requisitionTemplatesSearchParameter.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.REQUISITION_TYPE, Convert.ToString(requisitionType)));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    requisitionTemplatesSearchParameter.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.STATUS, status));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        requisitionTemplatesSearchParameter.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.ACTIVE_FLAG, isActive));
                    }
                }
                RequisitionTemplateList requisitionTemplateList = new RequisitionTemplateList(executionContext);
                IRequisitionTemplatesUseCases requisitionUseTemplatesCases = InventoryUseCaseFactory.GetRequisitionTemplatesUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalNoOfRequisitions = await requisitionUseTemplatesCases.GetRequisitionTemplateCount(requisitionTemplatesSearchParameter);
                log.LogVariableState("totalNoOfRequisitionTemplates", totalNoOfRequisitions);
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