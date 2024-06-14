/********************************************************************************************
 * Project Name - Transaction
 * Description  - Created to fetch, update and insert NotificationTagManualEvent
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.120.00    12-Mar-2021   Roshan Devadiga          Created
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class NotificationTagManualEventController : ApiController
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/NotificationTagManualEvents")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int notificationTagId = -1, int notificationTagEventId = -1, string guid=null,
            string processingStatus = null, DateTime? timestamp= null)
        {
            log.LogMethodEntry(isActive,notificationTagId,notificationTagEventId, guid,processingStatus,timestamp);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                NotificationTagManualEventsListBL notificationTagManualEventsListBL = new NotificationTagManualEventsListBL(executionContext);
                List<KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>(NotificationTagManualEventsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>(NotificationTagManualEventsDTO.SearchByParameters.IS_ACTIVE, "1"));
                    }
                }
                if (!string.IsNullOrEmpty(processingStatus))
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>(NotificationTagManualEventsDTO.SearchByParameters.PROCESSING_STATUS, processingStatus.ToString()));
                }
                if (!string.IsNullOrEmpty(guid))
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>(NotificationTagManualEventsDTO.SearchByParameters.GUID, guid.ToString()));
                }
                if (timestamp != null )
                {
                    DateTime dateTime = Convert.ToDateTime(timestamp);
                    searchParameters.Add(new KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>(NotificationTagManualEventsDTO.SearchByParameters.TIMESTAMP, dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (notificationTagId > -1)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>(NotificationTagManualEventsDTO.SearchByParameters.NOTIFICATION_TAG_ID, notificationTagId.ToString()));
                }
                if (notificationTagEventId > -1)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>(NotificationTagManualEventsDTO.SearchByParameters.NOTIFICATION_TAG_EVENT_ID, notificationTagEventId.ToString()));
                }
                INotificationTagManualEventUseCases notificationTagManualEventUseCases = TransactionUseCaseFactory.GetNotificationTagManualEventsUseCases(executionContext);
                List<NotificationTagManualEventsDTO> notificationTagManualEventsDTOList = await notificationTagManualEventUseCases.GetNotificationTagManualEvents(searchParameters);
                log.LogMethodExit(notificationTagManualEventsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = notificationTagManualEventsDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// api/Tag/NotificationTagManualEvents
        /// </summary>
        /// <param name="NotificationTagManualEventsDTOList"></param>
        /// <returns></returns>      
        [HttpPost]
        [Route("api/Transaction/NotificationTagManualEvents")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<NotificationTagManualEventsDTO> notificationTagManualEventsDTOList)
        {
            log.LogMethodEntry(notificationTagManualEventsDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                if (notificationTagManualEventsDTOList != null && notificationTagManualEventsDTOList.Any()  && notificationTagManualEventsDTOList.All(x=>x.NotificationTagMEventId < 0))
                {
                    INotificationTagManualEventUseCases notificationTagManualEventUseCases =TransactionUseCaseFactory.GetNotificationTagManualEventsUseCases(executionContext);
                    await notificationTagManualEventUseCases.SaveNotificationTagManualEvents(notificationTagManualEventsDTOList);
                    log.LogMethodExit(notificationTagManualEventsDTOList);
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

        /// <summary>
        /// api/Tag/NotificationTagManualEvents
        /// </summary>
        /// <param name="NotificationTagManualEventList"></param>
        /// <returns></returns>      
        [HttpPut]
        [Route("api/Transaction/NotificationTagManualEvents")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<NotificationTagManualEventsDTO> notificationTagManualEventsDTOList)
        {
            log.LogMethodEntry(notificationTagManualEventsDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                if (notificationTagManualEventsDTOList != null && notificationTagManualEventsDTOList.Any() && notificationTagManualEventsDTOList.All(x => x.NotificationTagMEventId > -1))
                {
                    INotificationTagManualEventUseCases notificationTagManualEventUseCases = TransactionUseCaseFactory.GetNotificationTagManualEventsUseCases(executionContext);
                    await notificationTagManualEventUseCases.SaveNotificationTagManualEvents(notificationTagManualEventsDTOList);
                    log.LogMethodExit(notificationTagManualEventsDTOList);
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
