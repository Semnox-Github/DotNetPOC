/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Event
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 *********************************************************************************************
 *2.50        28-Sept-2018      Jagan Mohana Rao          Created 
 *2.80        05-Apr-2020       Girish Kundar             Modified: API end point in route 
 *2.90        12-Aug-2020    Mushahid Faizan        Modified : Added search parameters in get, Removed Delete() 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Linq;
using Semnox.Parafait.DigitalSignage;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.DigitalSignage
{
    public class EventController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Events List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/Events")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, int eventId = -1, int typeId = -1, string eventName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, eventId, typeId, eventName);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<EventDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EventDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (eventId > -1)
                {
                    searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.ID, eventId.ToString()));
                }
                if (typeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.TYPE_ID, typeId.ToString()));
                }
                if (!string.IsNullOrEmpty(eventName))
                {
                    searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.NAME, eventName.ToString()));
                }

                EventListBL eventListBL = new EventListBL(executionContext);
                var content = eventListBL.GetEventDTOList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Post operation on event details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/DigitalSignage/Events")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<EventDTO> eventDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(eventDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (eventDTOList != null && eventDTOList.Any())
                {
                    // if eventDTO.Id is less than zero then insert or else update
                    EventListBL eventList = new EventListBL(executionContext, eventDTOList);
                    eventList.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = eventDTOList });
                }
                else
                {
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
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }
    }
}
