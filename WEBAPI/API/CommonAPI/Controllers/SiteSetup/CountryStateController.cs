/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Country and States details list
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-Mar-2019   Jagan Mohana         Created 
 *             26-Mar-2019   Mushahid Faizan     Added log Method Entry & Exit &
                                                 declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
                                                 Modified Delete() method.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Site;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.SiteSetup
{
    public class CountryStateController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object Country and States List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/CountryState/")]
        public HttpResponseMessage Get()
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                CountryParams countryParams = new CountryParams();
                countryParams.SiteId = securityTokenDTO.SiteId;
                countryParams.ShowState = true;
                if (ParafaitDefaultContainer.GetParafaitDefault(executionContext, "STATE_LOOKUP_FOR_COUNTRY") != "-1")
                {
                    countryParams.ShowLookupCountry = true;
                }

                CountryList countryList = new CountryList();
                var content = countryList.GetCountryList(countryParams);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
        
        /// <summary>
        /// Performs a Post operation on countryDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/CountryState/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<CountryDTO> countryDTOList)
        {
            try
            {
                log.LogMethodEntry(countryDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (countryDTOList != null)
                {
                    // if countryDTOs.CountryId is less than zero then insert or else update
                    CountryDTOList countryList = new CountryDTOList(executionContext, countryDTOList);
                    countryList.SaveUpdateCountryList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Delete operation on countryDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/CountryState/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<CountryDTO> countryDTOList)
        {
            try
            {
                log.LogMethodEntry(countryDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (countryDTOList != null)
                {
                    CountryDTOList countryList = new CountryDTOList(executionContext, countryDTOList);
                    countryList.SaveUpdateCountryList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
