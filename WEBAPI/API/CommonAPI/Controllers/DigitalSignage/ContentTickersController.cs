/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Tickers List
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50        28-Sept-2018   Jagan Mohana Rao          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.DigitalSignage;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;

namespace Semnox.CommonAPI.DigitalSignage
{
    [Route("api/[controller]")]
    public class ContentTickersController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Tickers List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/ContentTickers/")]
        [Authorize]
        public HttpResponseMessage Get(string isActive)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<TickerDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TickerDTO.SearchByParameters, string>>();
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.IS_ACTIVE, isActive));
                }
                searchParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));

                TickerListBL tickerListBL = new TickerListBL(executionContext);
                var content = tickerListBL.GetTickerDTOList(searchParameters);
                log.LogMethodEntry(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Post operation on ticker details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/DigitalSignage/ContentTickers/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<TickerDTO> tickerDTOs)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(tickerDTOs);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (tickerDTOs != null)
                {
                    log.Debug("ContentTickersController-Post() Method.");
                    // if tickerDTO.TickerId is less than zero then insert or else update
                    TickerListBL tickerListBL = new TickerListBL(tickerDTOs, executionContext);
                    tickerListBL.SaveUpdateTickersList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.Debug("ContentTickersController-Post() Method.");
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Delete operation on ticker details.
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        //DELETE: api/Subscriber/
        [HttpDelete]
        [Route("api/DigitalSignage/ContentTickers/")]
        [AllowAnonymous]
        public HttpResponseMessage Delete([FromBody] List<TickerDTO> tickerDTOs)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(tickerDTOs);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (tickerDTOs.Count != 0)
                {
                    TickerListBL tickerListBL = new TickerListBL(tickerDTOs, executionContext);
                    tickerListBL.SaveUpdateTickersList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.Debug("ContentTickersController-Delete() Method. Else condition");
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
    }
}