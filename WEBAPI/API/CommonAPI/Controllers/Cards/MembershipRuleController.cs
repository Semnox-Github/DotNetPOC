/********************************************************************************************
 * Project Name - Cards MembershipRuleController
 * Description  - Created to fetch, update and insert in the MembershipRule   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60        21-Feb-2019   Nagesh Badiger            Created.
 ***************************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Cards
{
    public class MembershipRuleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object MembershipRule Collections. 
        /// </summary>       
        [HttpGet]
        [Authorize]
        [Route("api/Cards/MembershipRule/")]
        public HttpResponseMessage Get(string isActive)
        {
            try
            {
                log.LogMethodEntry(isActive);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MembershipRuleDTO.SearchByParameters, string>(MembershipRuleDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));

                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<MembershipRuleDTO.SearchByParameters, string>(MembershipRuleDTO.SearchByParameters.ISACTIVE, isActive.ToString()));
                }
                MembershipRulesList membershipRulesList = new MembershipRulesList(executionContext);
                var content = membershipRulesList.GetAllMembershipRule(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Post the JSON Object MembershipRule
        /// </summary>
        /// <param name="membershipRuleDTOList">MembershipRulesList</param>
        [HttpPost]
        [Route("api/Cards/MembershipRule/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<MembershipRuleDTO> membershipRuleDTOList)
        {
            try
            {
                log.LogMethodEntry(membershipRuleDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (membershipRuleDTOList.Count != 0)
                {
                    MembershipRulesList membershipRulesBL = new MembershipRulesList(membershipRuleDTOList, executionContext);
                    membershipRulesBL.SaveUpdateMembershipRule();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {                
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Delete the JSON Object MembershipRule
        /// </summary>
        /// <param name="membershipRuleDTOList">MembershipRulesList</param>
        [HttpDelete]
        [Route("api/Cards/MembershipRule/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<MembershipRuleDTO> membershipRuleDTOList)
        {
            try
            {
                log.LogMethodEntry();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (membershipRuleDTOList.Count != 0)
                {
                    MembershipRulesList membershipRulesBL = new MembershipRulesList(membershipRuleDTOList, executionContext);
                    membershipRulesBL.SaveUpdateMembershipRule();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {                
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
