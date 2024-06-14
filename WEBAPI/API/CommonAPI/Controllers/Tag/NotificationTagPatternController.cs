/********************************************************************************************
 * Project Name - GlowWristBand
 * Description  - Created to fetch, update and insert NotificationTagPattern
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
    public class NotificationTagPatternController : ApiController
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Ads
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Tag/NotificationTagPatterns")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int notificationTagPatternId = -1, int ledPatternNumber = -1, int buzzerPatternNumber = -1,
            string notificationTagPatternName = null)
        {
            log.LogMethodEntry(isActive, notificationTagPatternId, notificationTagPatternName);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                List<KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>(NotificationTagPatternDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>(NotificationTagPatternDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(notificationTagPatternName))
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>(NotificationTagPatternDTO.SearchByParameters.NOTIFICATIONTAGPATTERNNAME, notificationTagPatternName.ToString()));
                }
                if (notificationTagPatternId > -1)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>(NotificationTagPatternDTO.SearchByParameters.NOTIFICATIONTAGPATTERNID, notificationTagPatternId.ToString()));
                }
                if (buzzerPatternNumber > -1)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>(NotificationTagPatternDTO.SearchByParameters.BUZZERPATTERNNUMBER, buzzerPatternNumber.ToString()));
                }

                if (ledPatternNumber > -1)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>(NotificationTagPatternDTO.SearchByParameters.LEDPATTERNNUMBER, ledPatternNumber.ToString()));
                }
                INotificationTagPatternUseCases notificationTagPatternUseCases = TagUseCaseFactory.GetNotificationTagPatternUseCases(executionContext);
                List<NotificationTagPatternDTO> notificationTagPatternDTOList = await notificationTagPatternUseCases.GetNotificationTagPatterns(searchParameters);

                log.LogMethodExit(notificationTagPatternDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = notificationTagPatternDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON NotificationTagPatternDTOList
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Tag/NotificationTagPatterns")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<NotificationTagPatternDTO> notificationTagPatternDTOList)
        {
            log.LogMethodEntry(notificationTagPatternDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                if (notificationTagPatternDTOList != null && notificationTagPatternDTOList.Any())
                {
                    INotificationTagPatternUseCases notificationTagPatternUseCases = TagUseCaseFactory.GetNotificationTagPatternUseCases(executionContext);
                    await notificationTagPatternUseCases.SaveNotificationTagPatterns(notificationTagPatternDTOList);
                    log.LogMethodExit(notificationTagPatternDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = notificationTagPatternDTOList });
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
