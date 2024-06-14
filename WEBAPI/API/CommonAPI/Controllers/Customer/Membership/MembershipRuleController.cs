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
 *2.80        06-Apr-2020   Girish Kundar             Modified: Moved from Site setup to common, renamed,sends token as part of Header             
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

namespace Semnox.CommonAPI.Controllers.Customer.Membership
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
        [Route("api/Customer/Membership/MembershipRules")]
        public HttpResponseMessage Get(string isActive = null)
        {
            try
            {
                log.LogMethodEntry(isActive);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MembershipRuleDTO.SearchByParameters, string>(MembershipRuleDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<MembershipRuleDTO.SearchByParameters, string>(MembershipRuleDTO.SearchByParameters.ISACTIVE, isActive.ToString()));
                    }
                }
                MembershipRulesList membershipRulesList = new MembershipRulesList(executionContext);
                var content = membershipRulesList.GetAllMembershipRule(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content});
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Post the JSON Object MembershipRule
        /// </summary>
        /// <param name="membershipRuleDTOList">MembershipRulesList</param>
        [HttpPost]
        [Route("api/Customer/Membership/MembershipRules")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<MembershipRuleDTO> membershipRuleDTOList)
        {
            try
            {
                log.LogMethodEntry(membershipRuleDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (membershipRuleDTOList != null && membershipRuleDTOList.Any())
                {
                    MembershipRulesList membershipRulesBL = new MembershipRulesList(membershipRuleDTOList, executionContext);
                    membershipRulesBL.SaveUpdateMembershipRule();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""});
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException});
            }
        }

        ///// <summary>
        ///// Delete the JSON Object MembershipRule
        ///// </summary>
        ///// <param name="membershipRuleDTOList">MembershipRulesList</param>
        //[HttpDelete]
        //[Route("api/Customer/Membership/MembershipRules")]
        //[Authorize]
        //public HttpResponseMessage Delete([FromBody] List<MembershipRuleDTO> membershipRuleDTOList)
        //{
        //    try
        //    {
        //        log.LogMethodEntry();
        //        securityTokenBL.GenerateJWTToken();
        //        securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

        //        executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
        //        if (membershipRuleDTOList.Count != 0)
        //        {
        //            MembershipRulesList membershipRulesBL = new MembershipRulesList(membershipRuleDTOList, executionContext);
        //            membershipRulesBL.SaveUpdateMembershipRule();
        //            log.LogMethodExit();
        //            return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
        //        }
        //        else
        //        {
        //            log.LogMethodExit();
        //            return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
        //        }
        //    }
        //    catch (Exception ex)
        //    {                
        //        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
        //        log.Error(customException);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
        //    }
        //}
    }
}
