/**************************************************************************************************
 * Project Name - Reports 
 * Description  - Controller for UserTarget
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.90        27-May-2020       Vikas Dwivedi             Created to Get Methods.
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Reports
{
    public class UserTargetController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of UserPeriodDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Report/UserTargets")]
        public HttpResponseMessage Get(string isActive = null, int userTargetId = -1, int gameId = -1, int periodId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, userTargetId, gameId, periodId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<UserTargetDTO.SearchByUserTargetSearchParameters, string>> userTargetSearchParameter = new List<KeyValuePair<UserTargetDTO.SearchByUserTargetSearchParameters, string>>();
                userTargetSearchParameter.Add(new KeyValuePair<UserTargetDTO.SearchByUserTargetSearchParameters, string>(UserTargetDTO.SearchByUserTargetSearchParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        userTargetSearchParameter.Add(new KeyValuePair<UserTargetDTO.SearchByUserTargetSearchParameters, string>(UserTargetDTO.SearchByUserTargetSearchParameters.IS_ACTIVE, isActive));
                    }
                }
                if (userTargetId > -1)
                {
                    userTargetSearchParameter.Add(new KeyValuePair<UserTargetDTO.SearchByUserTargetSearchParameters, string>(UserTargetDTO.SearchByUserTargetSearchParameters.USER_TARGET_ID, userTargetId.ToString()));
                }
                if (gameId > -1)
                {
                    userTargetSearchParameter.Add(new KeyValuePair<UserTargetDTO.SearchByUserTargetSearchParameters, string>(UserTargetDTO.SearchByUserTargetSearchParameters.GAME_ID, gameId.ToString()));
                }
                if (periodId > -1)
                {
                    userTargetSearchParameter.Add(new KeyValuePair<UserTargetDTO.SearchByUserTargetSearchParameters, string>(UserTargetDTO.SearchByUserTargetSearchParameters.PERIOD_ID, periodId.ToString()));
                }
                UserTargetListBL userTargetListBL = new UserTargetListBL(executionContext);
                List<UserTargetDTO> userTargetDTOList = userTargetListBL.GetUserTargetDTOList(userTargetSearchParameter);
                log.LogMethodExit(userTargetDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = userTargetDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object of UserTargetDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Report/UserTargets")]
        public HttpResponseMessage Post([FromBody] List<UserTargetDTO> userTargetDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(userTargetDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (userTargetDTOList != null && userTargetDTOList.Any())
                {
                    UserTargetListBL userTargetListBL = new UserTargetListBL(executionContext, userTargetDTOList);
                    userTargetListBL.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = userTargetDTOList });
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
