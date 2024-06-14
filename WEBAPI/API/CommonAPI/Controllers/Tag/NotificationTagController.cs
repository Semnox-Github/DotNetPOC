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
using Semnox.Parafait.Tags;

namespace Semnox.CommonAPI.Controllers.Tags
{
    public class NotificationTagsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Ads
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Tag/NotificationTags")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int notificationTagId = -1,
                                                bool loadActiveChild = false, bool buildChildRecords = false,
                                                string tagNumber =null,string defaultChannel = null,string tagStatus =null,
                                                bool isInStorage = false)
        {
            log.LogMethodEntry(isActive, notificationTagId, loadActiveChild, buildChildRecords);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (notificationTagId > -1)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.NOTIFICATIONTAGID, notificationTagId.ToString()));
                }
                if (string.IsNullOrWhiteSpace(tagNumber) == false)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.TAGNUMBER, tagNumber.ToString()));
                }
                if (string.IsNullOrWhiteSpace(defaultChannel) == false)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.DEFAULT_CHANNEL, defaultChannel.ToString()));
                }
                if (string.IsNullOrWhiteSpace(tagStatus) == false)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.TAGNOTIFICATIONSTATUS, tagStatus.ToString()));
                }
                if (isInStorage)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.ISINSTORAGE, "1"));
                }
                INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(executionContext);
                List<NotificationTagsDTO> notificationTagsDTOList = await notificationTagUseCases.GetNotificationTags(searchParameters, buildChildRecords, loadActiveChild);
                log.LogMethodExit(notificationTagsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = notificationTagsDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON NotificationTagsDTOList
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Tag/NotificationTags")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<NotificationTagsDTO> notificationTagsDTOList)
        {
            log.LogMethodEntry(notificationTagsDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                if (notificationTagsDTOList != null && notificationTagsDTOList.Any())
                {
                    INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(executionContext);
                    await notificationTagUseCases.SaveNotificationTags(notificationTagsDTOList);
                    log.LogMethodExit(notificationTagsDTOList);
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
