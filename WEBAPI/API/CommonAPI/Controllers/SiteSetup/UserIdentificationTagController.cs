/********************************************************************************************
 * Project Name - User
 * Description  - API for the User IdentificationTags  for Users details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        08-May-2019   Mushahid Faizan          Created 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.SiteSetup
{
    public class UserIdentificationTagController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object Users Identification Tags Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/UserIdentificationTag/")]
        public HttpResponseMessage Get(string isActive, int userId)
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<UserIdentificationTagsDTO> userIdentificationTagsDTOList = new List<UserIdentificationTagsDTO>();
                if (userId != 0 && userId > 0)
                {
                    List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>> searchParameters = new List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>>();
                    searchParameters.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                    if (isActive == "1")
                    {
                        searchParameters.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ACTIVE_FLAG, isActive));
                    }
                    searchParameters.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.USER_ID, userId.ToString()));
                    UserIdTagsList userIdTagsList = new UserIdTagsList(executionContext);
                    userIdentificationTagsDTOList = userIdTagsList.GetAllUsers(searchParameters);
                }

                log.LogMethodExit(userIdentificationTagsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = userIdentificationTagsDTOList, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Conflict, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Performs a Post operation on userIdentificationTagsDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/UserIdentificationTag/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<UserIdentificationTagsDTO> userIdentificationTagsDTO)
        {
            try
            {
                log.LogMethodEntry(userIdentificationTagsDTO);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (userIdentificationTagsDTO != null && userIdentificationTagsDTO.Count != 0)
                {
                    // if usersDTOs.UserId is less than zero then insert or else update
                    UserIdTagsList userIdTagsList = new UserIdTagsList(executionContext, userIdentificationTagsDTO);
                    userIdTagsList.SaveUpdateUserIdList();
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
        /// Performs a Delete operation on userIdentificationTagsDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/UserIdentificationTag/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<UserIdentificationTagsDTO> userIdentificationTagsDTO)
        {
            try
            {
                log.LogMethodEntry(userIdentificationTagsDTO);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (userIdentificationTagsDTO != null && userIdentificationTagsDTO.Any())
                {
                    // if usersDTOs.UserId is less than zero then insert or else update
                    UserIdTagsList userIdTagsList = new UserIdTagsList(executionContext, userIdentificationTagsDTO);
                    userIdTagsList.Delete();
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
