/********************************************************************************************
 * Project Name - Site Setup
 * Description  - API for the Ticket Station Setup in site Setup module.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70        31-Oct-2019   Rakesh Kumar          Created 
 *2.90        11-May-2020   Girish Kundar         Modified : Moved to Configuration and Changes as part of the REST API  
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;
namespace Semnox.CommonAPI.Controllers.Device
{
    public class TicketStationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of Ticket Station Details
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Device/TicketStations")]
        public HttpResponseMessage Get(string isActive = null, string ticketStationId = null, int stationId = -1)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive, ticketStationId, stationId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>> searchParameters = new List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>>()
                {
                     new KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>(TicketStationDTO.SearchByTicketStationParameters.SITE_ID, executionContext.GetSiteId().ToString())
                };
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>(TicketStationDTO.SearchByTicketStationParameters.IS_ACTIVE, isActive));
                    }
                }
                if (string.IsNullOrEmpty(ticketStationId) == false)
                {
                    searchParameters.Add(new KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>(TicketStationDTO.SearchByTicketStationParameters.TICKET_STATION_ID, ticketStationId));
                }
                if (stationId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>(TicketStationDTO.SearchByTicketStationParameters.ID, stationId.ToString()));
                }
                var content = new TicketStationListBL(executionContext).GetTicketStationDTOList(searchParameters);
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
        /// Performs a Post operation on Ticket Station Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Device/TicketStations")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<TicketStationDTO> ticketStationDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(ticketStationDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (ticketStationDTOList != null && ticketStationDTOList.Any())
                {
                    TicketStationListBL ticketStationBL = new TicketStationListBL(executionContext, ticketStationDTOList);
                    ticketStationBL.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException vexp)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(vexp, executionContext);
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
