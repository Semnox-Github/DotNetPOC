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
*2.80         06-Apr-2020    Girish Kundar       Modified: Moved from Site setup to common, renamed,sends token as part of Header  
*2.120.00     11-May-2021    Roshan Devadiga     Modified
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
using System.Linq;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.CommonAPI.Common
{
    public class CountryController : ApiController
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
        [Route("api/Common/Countries")]
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                CountryParams countryParams = new CountryParams();
                countryParams.SiteId = securityTokenDTO.SiteId;
                countryParams.ShowState = true;
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STATE_LOOKUP_FOR_COUNTRY") != "-1")
                {
                    countryParams.ShowLookupCountry = true;
                }
                ICountryUseCases countryUseCases = GenericUtilitiesUseCaseFactory.GetCountries(executionContext);
                List<CountryDTO> countryDTOList = await countryUseCases.GetCountries(countryParams);
                log.LogMethodExit(countryDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = countryDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Post operation on countryDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Common/Countries")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<CountryDTO> countryDTOList)
        {
            try
            {
                log.LogMethodEntry(countryDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (countryDTOList != null && countryDTOList.Any(a => a.CountryId > -1))
                {
                    log.LogMethodExit(countryDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICountryUseCases countryUseCases = GenericUtilitiesUseCaseFactory.GetCountries(executionContext);
                await countryUseCases.SaveCountries(countryDTOList);
                log.LogMethodExit(countryDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Delete operation on countryDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Common/Countries")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<CountryDTO> countryDTOList)
        {
            try
            {
                log.LogMethodEntry(countryDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (countryDTOList != null && countryDTOList.Any())
                {
                    ICountryUseCases countryUseCases = GenericUtilitiesUseCaseFactory.GetCountries(executionContext);
                    countryUseCases.Delete(countryDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the JSON Object of countryDTO List
        /// </summary>
        /// <param name="countryDTOList"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Common/Countries")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<CountryDTO> countryDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(countryDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (countryDTOList == null || countryDTOList.Any(a => a.CountryId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICountryUseCases countryUseCases = GenericUtilitiesUseCaseFactory.GetCountries(executionContext);
                await countryUseCases.SaveCountries(countryDTOList);
                log.LogMethodExit(countryDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
