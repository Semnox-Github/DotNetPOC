/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the UserRoles product group inclusion exclusions details Controller class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         11-Mar-2019   Jagan Mohana         Created 
 *             25-Jun-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                  Added HttpDelete Method.
                                                  Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.User;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.SiteSetup
{
    public class UserRoleDisplayGroupExclusionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object UserRoleDisplayGroupExclusions Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/UserRoleDisplayGroupExclusion/")]
        public HttpResponseMessage Get(string userRoleId)
        {
            try
            {
                log.LogMethodEntry(userRoleId);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters, string>> searchParameters = new List<KeyValuePair<UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters, string>>();
                searchParameters.Add(new KeyValuePair<UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters, string>(UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.ROLE_ID, userRoleId.ToString()));

                UserRoleDisplayGroupExclusionsList userRoleDisplayGroupExclusionsList = new UserRoleDisplayGroupExclusionsList(executionContext);
                var content = userRoleDisplayGroupExclusionsList.GetAllUserRoleDisplayGroupExclusionsList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Performs a Post operation on userRoleDisplayGroupExclusionsDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/UserRoleDisplayGroupExclusion/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<UserRoleDisplayGroupExclusionsDTO> userRoleDisplayGroupExclusionsDTOList)
        {
            try
            {
                log.LogMethodEntry(userRoleDisplayGroupExclusionsDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (userRoleDisplayGroupExclusionsDTOList != null)
                {
                    // if userRoleDisplayGroupExclusionsDTOs.roleDisplayGroupId is less than zero then insert or else update
                    UserRoleDisplayGroupExclusionsList userRoleDisplayGroupExclusionsList = new UserRoleDisplayGroupExclusionsList(executionContext, userRoleDisplayGroupExclusionsDTOList);
                    userRoleDisplayGroupExclusionsList.SaveUpdateUserRoleDisplayGroupExclusionsList();
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
                return Request.CreateResponse(HttpStatusCode.Conflict, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Delete operation on userRoleDisplayGroupExclusionsDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/UserRoleDisplayGroupExclusion/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<UserRoleDisplayGroupExclusionsDTO> userRoleDisplayGroupExclusionsDTOList)
        {
            try
            {
                log.LogMethodEntry(userRoleDisplayGroupExclusionsDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (userRoleDisplayGroupExclusionsDTOList != null)
                {
                    // if userRoleDisplayGroupExclusionsDTOs.roleDisplayGroupId is less than zero then insert or else update
                    UserRoleDisplayGroupExclusionsList userRoleDisplayGroupExclusionsList = new UserRoleDisplayGroupExclusionsList(executionContext, userRoleDisplayGroupExclusionsDTOList);
                    userRoleDisplayGroupExclusionsList.SaveUpdateUserRoleDisplayGroupExclusionsList();
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
                return Request.CreateResponse(HttpStatusCode.Conflict, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
