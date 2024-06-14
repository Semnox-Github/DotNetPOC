/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the ApprovalRules .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.100.0    14-Oct-2020  Mushahid Faizan         Created.
 *2.110.0    23-Nov-2020  Mushahid Faizan         Web Inventory UI resdesign changes with REST API.
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
    public class ApprovalRuleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the ApprovalRules.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/ApprovalRules")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int documentTypeId = -1, int roleId = -1, int numberOfApprovalLevels = -1,
                                                    int approvalRuleId = -1, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, documentTypeId, roleId, numberOfApprovalLevels, approvalRuleId, currentPage, pageSize);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> searchParameters = new List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>>();
                searchParameters.Add(new KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>(ApprovalRuleDTO.SearchByApprovalRuleParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>(ApprovalRuleDTO.SearchByApprovalRuleParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                if (approvalRuleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>(ApprovalRuleDTO.SearchByApprovalRuleParameters.APPROVAL_RULE_ID, approvalRuleId.ToString()));
                }
                if (documentTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>(ApprovalRuleDTO.SearchByApprovalRuleParameters.DOCUMENT_TYPE_ID, documentTypeId.ToString()));
                }
                if (roleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>(ApprovalRuleDTO.SearchByApprovalRuleParameters.ROLE_ID, roleId.ToString()));
                }
                if (numberOfApprovalLevels > -1)
                {
                    searchParameters.Add(new KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>(ApprovalRuleDTO.SearchByApprovalRuleParameters.NUMBER_OF_APPROVAL_LEVELS, numberOfApprovalLevels.ToString()));
                }

                ApprovalRulesList approvalRulesList = new ApprovalRulesList(executionContext);

                int totalNoOfPages = 0;
                int totalCount = await Task<int>.Factory.StartNew(() => { return approvalRulesList.GetApprovalRuleCount(searchParameters, null); });
                log.LogVariableState("totalCount", totalCount);
                totalNoOfPages = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

                IApprovalRuleUseCases approvalRuleUseCases = InventoryUseCaseFactory.GetApprovalRuleUseCases(executionContext);
                List<ApprovalRuleDTO> approvalRuleDTOList = await approvalRuleUseCases.GetApprovalRules(searchParameters, currentPage, pageSize);

                log.LogMethodExit(approvalRuleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = approvalRuleDTOList, currentPageNo = currentPage, TotalCount = totalCount });

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
        /// Post the JSON Object Approval Rules
        /// </summary>
        /// <param name="approvalRuleDTOList">approvalRuleDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Inventory/ApprovalRules")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<ApprovalRuleDTO> approvalRuleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(approvalRuleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (approvalRuleDTOList == null || approvalRuleDTOList.Any(a => a.ApprovalRuleID > 0))
                {
                    log.LogMethodExit(approvalRuleDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IApprovalRuleUseCases approvalRuleUseCases = InventoryUseCaseFactory.GetApprovalRuleUseCases(executionContext);
                await approvalRuleUseCases.SaveApprovalRules(approvalRuleDTOList);

                
                log.LogMethodExit(approvalRuleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = approvalRuleDTOList });
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
        /// Post the JSON Object Approval Rules
        /// </summary>
        /// <param name="approvalRuleDTOList">approvalRuleDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/ApprovalRules")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<ApprovalRuleDTO> approvalRuleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(approvalRuleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (approvalRuleDTOList == null || approvalRuleDTOList.Any(a => a.ApprovalRuleID < 0))
                {
                    log.LogMethodExit(approvalRuleDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IApprovalRuleUseCases approvalRuleUseCases = InventoryUseCaseFactory.GetApprovalRuleUseCases(executionContext);
                await approvalRuleUseCases.SaveApprovalRules(approvalRuleDTOList);

               
                log.LogMethodExit(approvalRuleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = approvalRuleDTOList });
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
