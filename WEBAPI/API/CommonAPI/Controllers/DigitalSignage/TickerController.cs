/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Tickers List
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By            Remarks          
 *********************************************************************************************
 *2.50        28-Sept-2018   Jagan Mohana Rao       Created 
 *2.90        29-Jul-2020    Mushahid Faizan        Modified : Renamed Controller from ContentTickersController to TickerController
 *                                                  Added search parameters in get, Removed Delete() and removed token from response body.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using Semnox.Parafait.DigitalSignage;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.DigitalSignage
{
    public class TickerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Tickers List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/Tickers")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, int tickerId = -1, string tickerName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<TickerDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TickerDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(tickerName))
                {
                    searchParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.NAME, tickerName));
                }
                if (tickerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.TICKER_ID, tickerId.ToString()));
                }

                TickerListBL tickerListBL = new TickerListBL(executionContext);
                var content = tickerListBL.GetTickerDTOList(searchParameters);
                log.LogMethodEntry(content);
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
        /// Performs a Post operation on ticker details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/DigitalSignage/Tickers")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<TickerDTO> tickerDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(tickerDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (tickerDTOList != null && tickerDTOList.Any())
                {
                    // if tickerDTO.TickerId is less than zero then insert or else update
                    TickerListBL tickerListBL = new TickerListBL(executionContext, tickerDTOList);
                    tickerListBL.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = tickerDTOList });
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