/********************************************************************************************
 * Project Name - Promotions
 * Description  - Controller for the Loyalty class.
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.70.0      31-Mar-2020     Mushahid Faizan    Created 
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Promotions;

namespace Semnox.CommonAPI.Controllers.Promotion
{
    public class LoyaltyController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of Promotions Calendar Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Promotion/Loyalties")]
        public HttpResponseMessage Get(string isActive = null, bool loadActiveChild = false, bool buildChildRecords = false,
                                        string vipOnly = null, int loyaltyRuleId = -1, string purchaseOrConsumptionApplicability = null,
                                        bool isExpired = false, int membershipId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, vipOnly);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>(LoyaltyRuleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>(LoyaltyRuleDTO.SearchByParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(vipOnly))
                {
                    searchParameters.Add(new KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>(LoyaltyRuleDTO.SearchByParameters.VIP_ONLY, vipOnly.ToString()));
                }
                if (!string.IsNullOrEmpty(purchaseOrConsumptionApplicability))
                {
                    searchParameters.Add(new KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>(LoyaltyRuleDTO.SearchByParameters.PURCHASE_OR_CONSUMPTION_APPLICABILITY, purchaseOrConsumptionApplicability.ToString()));
                }
                if (isExpired)
                {
                    searchParameters.Add(new KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>(LoyaltyRuleDTO.SearchByParameters.EXPIRY_DATE, "GetDate()"));
                }
                if (loyaltyRuleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>(LoyaltyRuleDTO.SearchByParameters.LOYALTY_RULE_ID, loyaltyRuleId.ToString()));
                }
                if (membershipId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>(LoyaltyRuleDTO.SearchByParameters.MEMBERSHIP_ID, membershipId.ToString()));
                }
                LoyaltyRuleListBL loyaltyRuleListBL = new LoyaltyRuleListBL(executionContext);
                List<LoyaltyRuleDTO> loyaltyRuleList = loyaltyRuleListBL.GetAllLoyaltyRuleDTOList(searchParameters, buildChildRecords, loadActiveChild, null);
                log.LogMethodExit(loyaltyRuleList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = loyaltyRuleList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Performs a Post operation on LoyaltyRuleDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Promotion/Loyalties")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<LoyaltyRuleDTO> loyaltyRuleList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(loyaltyRuleList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (loyaltyRuleList != null && loyaltyRuleList.Any())
                {
                    LoyaltyRuleListBL loyaltyRuleListBL = new LoyaltyRuleListBL(executionContext, loyaltyRuleList);
                    loyaltyRuleListBL.Save();
                    log.LogMethodExit(loyaltyRuleList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = loyaltyRuleList });
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
        /// Performs a Post operation on LoyaltyRuleDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Promotion/Loyalties")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<LoyaltyRuleDTO> loyaltyRuleList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(loyaltyRuleList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (loyaltyRuleList != null && loyaltyRuleList.Any())
                {
                    LoyaltyRuleListBL loyaltyRuleListBL = new LoyaltyRuleListBL(executionContext, loyaltyRuleList);
                    loyaltyRuleListBL.Delete();
                    log.LogMethodExit(loyaltyRuleList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
