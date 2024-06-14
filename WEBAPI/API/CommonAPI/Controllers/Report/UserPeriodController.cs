/**************************************************************************************************
 * Project Name - Reports 
 * Description  - Controller for UserPeriod
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
    public class UserPeriodController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of UserPeriodDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Report/UserPeriod")]
        public HttpResponseMessage Get(DateTime? fromDate = null, DateTime? toDate = null, int periodId = -1, string name = null, int parentId = -1, string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(fromDate, toDate, isActive, periodId, name, parentId, isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<UserPeriodDTO.SearchByUserPeriodSearchParameters, string>> userPeriodSearchParameter = new List<KeyValuePair<UserPeriodDTO.SearchByUserPeriodSearchParameters, string>>();
                userPeriodSearchParameter.Add(new KeyValuePair<UserPeriodDTO.SearchByUserPeriodSearchParameters, string>(UserPeriodDTO.SearchByUserPeriodSearchParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));

                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddDays(1);

                if (periodId > -1)
                {
                    userPeriodSearchParameter.Add(new KeyValuePair<UserPeriodDTO.SearchByUserPeriodSearchParameters, string>(UserPeriodDTO.SearchByUserPeriodSearchParameters.PERIOD_ID, periodId.ToString()));
                }
                if (fromDate != null)
                {
                    startDate = Convert.ToDateTime(startDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }
                if (toDate != null)
                {
                    endDate = Convert.ToDateTime(endDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }
                if (!string.IsNullOrEmpty(name))
                {
                    userPeriodSearchParameter.Add(new KeyValuePair<UserPeriodDTO.SearchByUserPeriodSearchParameters, string>(UserPeriodDTO.SearchByUserPeriodSearchParameters.NAME, name.ToString()));
                }
                if (parentId > -1)
                {
                    userPeriodSearchParameter.Add(new KeyValuePair<UserPeriodDTO.SearchByUserPeriodSearchParameters, string>(UserPeriodDTO.SearchByUserPeriodSearchParameters.PARENT_ID, parentId.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        userPeriodSearchParameter.Add(new KeyValuePair<UserPeriodDTO.SearchByUserPeriodSearchParameters, string>(UserPeriodDTO.SearchByUserPeriodSearchParameters.IS_ACTIVE, isActive));
                    }
                }
                UserPeriodListBL userPeriodListBL = new UserPeriodListBL(executionContext);
                List<UserPeriodDTO> userPeriodDTOList = userPeriodListBL.GetUserPeriodDTOList(userPeriodSearchParameter);
                log.LogMethodExit(userPeriodDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = userPeriodDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object of UserPeriodDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Report/UserPeriod")]
        public HttpResponseMessage Post([FromBody] List<UserPeriodDTO> userPeriodDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(userPeriodDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (userPeriodDTOList != null && userPeriodDTOList.Any())
                {
                    UserPeriodListBL userPeriodListBL = new UserPeriodListBL(executionContext, userPeriodDTOList);
                    userPeriodListBL.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = userPeriodDTOList });
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
