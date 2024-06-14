/********************************************************************************************
 * Project Name - Promotions
 * Description  - Created to insert, update Loyalty Redemption Rule in Promotions
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.70        17-Jul-2019   Mushahid Faizan    Created 
 *2.80        31-Mar-2020   Mushahid Faizan    Modified as per Rest API Phase 1 changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class LoyaltyRedemptionRuleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of LoyaltyRedemptionRules List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Promotion/LoyaltyRedemptionRules")]
        public HttpResponseMessage Get(string isActive = null, int redemptionRuleId = -1, int loyaltyAttributeId = -1,
                                       DateTime? expiryDate = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, redemptionRuleId, loyaltyAttributeId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>(LoyaltyRedemptionRuleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (!string.IsNullOrEmpty(isActive))
                {
                    searchParameters.Add(new KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>(LoyaltyRedemptionRuleDTO.SearchByParameters.ACTIVE_FLAG, isActive.ToString()));
                }
                if (redemptionRuleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>(LoyaltyRedemptionRuleDTO.SearchByParameters.REDEMPTION_RULE_ID, redemptionRuleId.ToString()));
                }
                if (loyaltyAttributeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>(LoyaltyRedemptionRuleDTO.SearchByParameters.LOYALTY_ATTR_ID, loyaltyAttributeId.ToString()));
                }
                if (expiryDate != null)
                {
                    DateTime value = Convert.ToDateTime(expiryDate);
                    if (value == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                    searchParameters.Add(new KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>(LoyaltyRedemptionRuleDTO.SearchByParameters.EXPIRY_DATE, value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                LoyaltyRedemptionRuleListBL loyaltyRedemptionRuleListBL = new LoyaltyRedemptionRuleListBL(executionContext);
                var content = loyaltyRedemptionRuleListBL.GetLoyaltyRedemptionRuleList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Performs a Post operation on LoyaltyRedemptionRuleDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Promotion/LoyaltyRedemptionRules")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(loyaltyRedemptionRuleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (loyaltyRedemptionRuleDTOList != null && loyaltyRedemptionRuleDTOList.Any())
                {
                    LoyaltyRedemptionRuleListBL loyaltyRedemptionRuleListBL = new LoyaltyRedemptionRuleListBL(executionContext, loyaltyRedemptionRuleDTOList);
                    loyaltyRedemptionRuleListBL.Save();
                    log.LogMethodExit(loyaltyRedemptionRuleDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = loyaltyRedemptionRuleDTOList });
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
        /// Performs a Delete operation on LoyaltyRedemptionRuleDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Promotion/LoyaltyRedemptionRules")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(loyaltyRedemptionRuleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (loyaltyRedemptionRuleDTOList != null && loyaltyRedemptionRuleDTOList.Any())
                {
                    LoyaltyRedemptionRuleListBL loyaltyRedemptionRuleListBL = new LoyaltyRedemptionRuleListBL(executionContext, loyaltyRedemptionRuleDTOList);
                    loyaltyRedemptionRuleListBL.Delete();
                    log.LogMethodExit();
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
