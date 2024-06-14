/********************************************************************************************
* Project Name - CommonAPI
* Description  - RequisitionTemplateController - Created to get the requisition template
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.110.0   07-Dec-2020     Mushahid Faizan            Created : As part of Inventory UI Redesign      
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using Semnox.Parafait.Inventory.Requisition;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class RequisitionTemplateController : ApiController
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
        [Route("api/Inventory/RequisitionTemplates")]
        public async Task<HttpResponseMessage> Get(int templateId = -1, string templateName = null, int requisitionType = -1, string status = null, 
                                                   bool buildChildRecords = false, string isActive = null, bool loadActiveChild = false, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(templateId, templateName, requisitionType, status, buildChildRecords, isActive, loadActiveChild);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                RequisitionTemplateList requisitionTemplateListBL = new RequisitionTemplateList(executionContext);
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
                        loadActiveChild = true;
                        requisitionTemplatesSearchParameter.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.ACTIVE_FLAG, isActive));
                    }
                }

                int totalNoOfPages = 0;
                int totalNoOfRequisitionTemplates = await Task<int>.Factory.StartNew(() => { return requisitionTemplateListBL.GetRequisitionTemplatesCount(requisitionTemplatesSearchParameter, null); });
                log.LogVariableState("totalNoOfRequisitionTemplates", totalNoOfRequisitionTemplates);
                totalNoOfPages = (totalNoOfRequisitionTemplates / pageSize) + ((totalNoOfRequisitionTemplates % pageSize) > 0 ? 1 : 0);

                IRequisitionTemplatesUseCases requisitionUseTemplatesCases = InventoryUseCaseFactory.GetRequisitionTemplatesUseCases(executionContext);
                List<RequisitionTemplatesDTO> requisitionTemplatesDTOList = await requisitionUseTemplatesCases.GetRequisitionTemplates(requisitionTemplatesSearchParameter, buildChildRecords, loadActiveChild, currentPage, pageSize, null);
                log.LogMethodExit(requisitionTemplatesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = requisitionTemplatesDTOList, currentPageNo = currentPage, TotalCount = totalNoOfRequisitionTemplates });
                
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object of RequisitionTemplatesDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/RequisitionTemplates")]
        public async Task<HttpResponseMessage> Post([FromBody] List<RequisitionTemplatesDTO> requisitionTemplatesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(requisitionTemplatesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (requisitionTemplatesDTOList == null)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (requisitionTemplatesDTOList != null && requisitionTemplatesDTOList.Any())
                {
                    IRequisitionTemplatesUseCases requisitionUseTemplatesCases = InventoryUseCaseFactory.GetRequisitionTemplatesUseCases(executionContext);
                    await requisitionUseTemplatesCases.SaveRequisitionTemplates(requisitionTemplatesDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = requisitionTemplatesDTOList });
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
        /// Put the JSON Object of RequisitionTemplatesDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Authorize]
        [Route("api/Inventory/RequisitionTemplates")]
        public async Task<HttpResponseMessage> Put([FromBody] List<RequisitionTemplatesDTO> requisitionTemplatesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(requisitionTemplatesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (requisitionTemplatesDTOList == null || requisitionTemplatesDTOList.Any(x => x.TemplateId < -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (requisitionTemplatesDTOList != null && requisitionTemplatesDTOList.Any())
                {
                    IRequisitionTemplatesUseCases requisitionUseTemplatesCases = InventoryUseCaseFactory.GetRequisitionTemplatesUseCases(executionContext);
                    await requisitionUseTemplatesCases.SaveRequisitionTemplates(requisitionTemplatesDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = requisitionTemplatesDTOList });
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