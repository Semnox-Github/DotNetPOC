/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Products "MembershipExclusionRule". Created to fetch, update and insert MembershipExclusionRule.
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.60        08-Feb-2019     Mushahid Faizan     Created
 *********************************************************************************************
 *2.60        13-May-2019     Akshay Gulaganji    Added ExecutionContext and modified Get() by adding productId param
 *2.70        10-Sept-2019    Jagan Mohana        Used for the both Cards and product module membership entity
  *2.100.0     10-Sep-2020     Vikas Dwivedi       Modified as per the REST API Standards.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Product
{
    public class MembershipExclusionRuleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object MembershipExclusionRule Collection
        /// </summary>       
        [HttpGet]
        [Route("api/Product/MembershipExclusionRules")]
        public HttpResponseMessage Get(bool accountMembership = false,bool productMembership = false, string isActive =null, string membershipId = null, string productId = null, bool isPopulate = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(accountMembership, productMembership, isActive, membershipId, productId, isPopulate);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>(MembershipExclusionRuleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>(MembershipExclusionRuleDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (accountMembership && !string.IsNullOrEmpty(membershipId))
                {
                    searchParameters.Add(new KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>(MembershipExclusionRuleDTO.SearchByParameters.MEMBERSHIP_ID, membershipId.ToString()));
                }
                else if (productMembership && !string.IsNullOrEmpty(productId))
                {
                    searchParameters.Add(new KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>(MembershipExclusionRuleDTO.SearchByParameters.PRODUCT_ID, productId));
                }
                MembershipExclusionRuleListBL membershipList = new MembershipExclusionRuleListBL(executionContext);
                List<MembershipExclusionRuleDTO> membershipExclusionRuleDTOList = new List<MembershipExclusionRuleDTO>();
                if (isPopulate)
                {
                    List<MembershipExclusionRuleDTO> membershipExclusionRuleList = membershipList.PopulateMembershipExclusion(searchParameters);
                    if (membershipExclusionRuleList != null && membershipExclusionRuleList.Count > 0)
                    {
                        if (Convert.ToInt32(productId) >= 0)
                        {
                            if (membershipExclusionRuleList.FindAll(m => m.MembershipId == -1 && m.Id > -1 && m.ProductId > -1).Count > 0)
                            {
                                membershipExclusionRuleDTOList = membershipExclusionRuleList;
                            }
                            else
                            {
                                membershipExclusionRuleList.Find(m => m.Id == -1 && m.ProductId == -1 && m.MembershipId == -1).ProductId = Convert.ToInt32(productId);
                                membershipExclusionRuleDTOList = membershipExclusionRuleList;
                            }
                        }
                    }
                }
                else
                {
                    membershipExclusionRuleDTOList = membershipList.GetMembershipExclusionRuleDTOList(searchParameters);
                }
                log.LogMethodExit(membershipExclusionRuleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = membershipExclusionRuleDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Post the JSON Object MembershipExclusionRule Collections.
        /// </summary>
        /// <param name="membershipExclusionList"></param>
        [HttpPost]
        [Route("api/Product/MembershipExclusionRules")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<MembershipExclusionRuleDTO> membershipExclusionList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(membershipExclusionList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (membershipExclusionList != null && membershipExclusionList.Any())
                {
                    MembershipExclusionRuleListBL membershipExclusionRuleList = new MembershipExclusionRuleListBL(executionContext, membershipExclusionList);
                    membershipExclusionRuleList.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "" });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Delete the JSON Object MembershipExclusionRule Collections.
        /// </summary>
        /// <param name="membershipExclusionList"></param>
        [HttpDelete]
        [Route("api/Product/MembershipExclusionRules")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<MembershipExclusionRuleDTO> membershipExclusionList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(membershipExclusionList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (membershipExclusionList != null && membershipExclusionList.Any())
                {
                    MembershipExclusionRuleListBL membershipExclusionRuleList = new MembershipExclusionRuleListBL(executionContext, membershipExclusionList);
                    membershipExclusionRuleList.Delete();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "" });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }
    }
}
