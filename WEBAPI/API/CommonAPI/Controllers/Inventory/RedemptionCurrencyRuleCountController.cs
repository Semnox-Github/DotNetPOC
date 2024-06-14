/********************************************************************************************
 * Project Name - RedemptionCurrencyRuleCount Controller
 * Description  - Created RedemptionCurrencyRuleCount Controller
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
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class RedemptionCurrencyRuleCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object RedemptionCurrencyRules List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/RedemptionCurrencyRuleCounts")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int currencyRuleId = -1,
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

                IRedemptionCurrencyRuleUseCases redemptionCurrencyRuleUseCases = RedemptionUseCaseFactory.GetRedemptionCurrencyRuleUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalNoOfCurrencyRules = await Task<int>.Factory.StartNew(() => { return redemptionCurrencyRuleListBL.GetCurrencyRulesCount(searchParameters, null); });
                log.LogVariableState("totalNoOfCurrencyRules", totalNoOfCurrencyRules);
                totalNoOfPages = (totalNoOfCurrencyRules / pageSize) + ((totalNoOfCurrencyRules % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalNoOfCurrencyRules);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalNoOfCurrencyRules, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });
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