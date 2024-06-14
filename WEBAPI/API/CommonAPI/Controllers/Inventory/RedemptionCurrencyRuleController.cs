/********************************************************************************************
 * Project Name - Currency
 * Description  - API for the Redemption Currency Details
 * 
 **************
 **Version Log
 **************
 *Version     Date         Modified By           Remarks          
 *********************************************************************************************
 *2.100.0    05-Oct-2020   Mushahid Faizan      Created 
 *2.110.0    23-Nov-2020   Mushahid Faizan         Web Inventory UI resdesign changes with REST API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Redemption;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class RedemptionCurrencyRuleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object RedemptionCurrencyRules List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/RedemptionCurrencyRules")]
        public async Task<HttpResponseMessage> Get(string isActive = null, bool loadActiveChild = false, bool buildChildRecords = false, int currencyRuleId = -1,
                                                    string currencyRuleName = null, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL(executionContext);
                List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParameters = new List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>>();

                searchParameters.Add(new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.IS_ACTIVE, isActive));
                    }
                }

                if (!string.IsNullOrEmpty(currencyRuleName))
                {
                    searchParameters.Add(new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_NAME, currencyRuleName));
                }
                if (currencyRuleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID, currencyRuleId.ToString()));
                }

                int totalNoOfPages = 0;
                int totalNoOfCurrencyRules = await Task<int>.Factory.StartNew(() => { return redemptionCurrencyRuleListBL.GetCurrencyRulesCount(searchParameters, null); });
                log.LogVariableState("totalNoOfCurrencyRules", totalNoOfCurrencyRules);
                totalNoOfPages = (totalNoOfCurrencyRules / pageSize) + ((totalNoOfCurrencyRules % pageSize) > 0 ? 1 : 0);

                IRedemptionCurrencyRuleUseCases redemptionCurrencyRuleUseCases = RedemptionUseCaseFactory.GetRedemptionCurrencyRuleUseCases(executionContext);
                List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList = await redemptionCurrencyRuleUseCases.GetRedemptionCurrencyRules(searchParameters, buildChildRecords, loadActiveChild, currentPage, pageSize, null);

              
                log.LogMethodExit(redemptionCurrencyRuleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = redemptionCurrencyRuleDTOList, currentPageNo = currentPage, TotalCount = totalNoOfCurrencyRules });
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
        /// Performs a Post operation on RedemptionCurrencyRuleDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Inventory/RedemptionCurrencyRules")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionCurrencyRuleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (redemptionCurrencyRuleDTOList == null || redemptionCurrencyRuleDTOList.Any(a => a.RedemptionCurrencyRuleId > 0))
                {
                    log.LogMethodExit(redemptionCurrencyRuleDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });

                }
                IRedemptionCurrencyRuleUseCases redemptionCurrencyRuleUseCases = RedemptionUseCaseFactory.GetRedemptionCurrencyRuleUseCases(executionContext);
                await redemptionCurrencyRuleUseCases.SaveRedemptionCurrencyRules(redemptionCurrencyRuleDTOList);

                RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL(executionContext, redemptionCurrencyRuleDTOList);
                redemptionCurrencyRuleListBL.Save();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = redemptionCurrencyRuleDTOList });
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
        /// Performs a Post operation on RedemptionCurrencyRuleDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Route("api/Inventory/RedemptionCurrencyRules")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionCurrencyRuleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (redemptionCurrencyRuleDTOList == null || redemptionCurrencyRuleDTOList.Any(a => a.RedemptionCurrencyRuleId < 0))
                {
                    log.LogMethodExit(redemptionCurrencyRuleDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });

                }
                IRedemptionCurrencyRuleUseCases redemptionCurrencyRuleUseCases = RedemptionUseCaseFactory.GetRedemptionCurrencyRuleUseCases(executionContext);
                await redemptionCurrencyRuleUseCases.SaveRedemptionCurrencyRules(redemptionCurrencyRuleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = redemptionCurrencyRuleDTOList });
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
