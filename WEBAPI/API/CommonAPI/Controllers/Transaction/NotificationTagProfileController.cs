/********************************************************************************************
 * Project Name - Tools Controller
 * Description  - Created to fetch, update and insert Ads
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.90        23-Jul-2020   Mushahid Faizan           Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;


namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class NotificationTagProfileController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Ads
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/NotificationTagProfiles")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int notificationTagProfileId = -1, string notificationTagProfileName = null,
                                        bool loadActiveChild = false, bool buildChildRecords = false)
        {
            log.LogMethodEntry(isActive, notificationTagProfileId, notificationTagProfileName, loadActiveChild, buildChildRecords);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                List<KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>(NotificationTagProfileDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>(NotificationTagProfileDTO.SearchByParameters.IS_ACTIVE, "1"));
                    }
                }
                if (!string.IsNullOrEmpty(notificationTagProfileName))
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>(NotificationTagProfileDTO.SearchByParameters.NOTIFICATIONTAGPROFILENAME, notificationTagProfileName.ToString()));
                }
                if (notificationTagProfileId > -1)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>(NotificationTagProfileDTO.SearchByParameters.NOTIFICATIONTAGPROFILEID, notificationTagProfileId.ToString()));
                }
                INotificationTagProfileUseCases notificationTagProfileUseCases = TransactionUseCaseFactory.GetNotificationTagProfileUseCases(executionContext);
                List<NotificationTagProfileDTO> notificationTagProfileDTOList = await notificationTagProfileUseCases.GetNotificationTagProfiles(searchParameters);
                log.LogMethodExit(notificationTagProfileDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = notificationTagProfileDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON NotificationTagProfileDTOList
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Transaction/NotificationTagProfiles")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<NotificationTagProfileDTO> notificationTagProfileDTOList)
        {
            log.LogMethodEntry(notificationTagProfileDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                if (notificationTagProfileDTOList != null && notificationTagProfileDTOList.Any())
                {
                    INotificationTagProfileUseCases notificationTagProfileUseCases = TransactionUseCaseFactory.GetNotificationTagProfileUseCases(executionContext);
                    await notificationTagProfileUseCases.SaveNotificationTagProfiles(notificationTagProfileDTOList);
                    log.LogMethodExit(notificationTagProfileDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = notificationTagProfileDTOList });
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
