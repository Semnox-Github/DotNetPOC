/********************************************************************************************
 * Project Name - ApprovalRuleCount Controller
 * Description  - Created ApprovalRuleCount Controller
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1   10-feb-2021   Likhitha Reddy            created
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class ApprovalRuleCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the ApprovalRules.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/ApprovalRuleCounts")]
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
                IApprovalRuleUseCases approvalRuleUseCases = InventoryUseCaseFactory.GetApprovalRuleUseCases(executionContext);

                int totalNoOfPages = 0;
                int totalCount = await approvalRuleUseCases.GetApprovalRuleCount(searchParameters);
                log.LogVariableState("totalCount", totalCount);
                totalNoOfPages = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalCount);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalCount, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });

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