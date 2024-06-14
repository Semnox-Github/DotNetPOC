/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the kiosk setup details list
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         18-Mar-2019   Jagan Mohana         Created 
 *2.60         23-Apr-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                  Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
                                                  Added isActive SearchParameter in HttpGet Method.
                                                  Added HttpDeleteMethod, Modified HttpPost Method.
*2.90         14-07-2020     Girish Kundar        Modified : Moved to Kiosk resource folder and REST API staandard changes
*2.120.00     27-Apr-2021    Roshan Devadiga      Modified 
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.CommonAPI.Kiosk
{
    public class KioskSetupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object KioskSetup List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Kiosk/KioskCurrencies")]
        public async Task<HttpResponseMessage> Get(string isActive)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<KioskSetupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<KioskSetupDTO.SearchByParameters, string>>();

                searchParameters.Add(new KeyValuePair<KioskSetupDTO.SearchByParameters, string>(KioskSetupDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<KioskSetupDTO.SearchByParameters, string>(KioskSetupDTO.SearchByParameters.ISACTIVE, isActive));
                }
                IKioskSetupUseCases kioskSetupUseCases = GenericUtilitiesUseCaseFactory.GetKioskSetupUseCases(executionContext);
                List<KioskSetupDTO> kioskSetupDTOList = await kioskSetupUseCases.GetKioskSetups(searchParameters);
                log.LogMethodExit(kioskSetupDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = kioskSetupDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Post operation on KioskSetupDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Kiosk/KioskCurrencies")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<KioskSetupDTO> kioskSetupDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(kioskSetupDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (kioskSetupDTOList == null)
                {
                    log.LogMethodExit(kioskSetupDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IKioskSetupUseCases kioskSetupUseCases = GenericUtilitiesUseCaseFactory.GetKioskSetupUseCases(executionContext);
                await kioskSetupUseCases.SaveKioskSetups(kioskSetupDTOList);
                log.LogMethodExit(kioskSetupDTOList);
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
        /// Performs a Delete operation on KioskSetupDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Kiosk/KioskCurrencies")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<KioskSetupDTO> kioskSetupDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(kioskSetupDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (kioskSetupDTOList != null && kioskSetupDTOList.Any())
                {
                    IKioskSetupUseCases kioskSetupUseCases = GenericUtilitiesUseCaseFactory.GetKioskSetupUseCases(executionContext);
                    kioskSetupUseCases.Delete(kioskSetupDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
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
        /// Put
        /// </summary>
        /// <param name="kioskSetupDTOList"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Kiosk/KioskCurrencies")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<KioskSetupDTO> kioskSetupDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(kioskSetupDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (kioskSetupDTOList == null || kioskSetupDTOList.Any(a => a.Id < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IKioskSetupUseCases kioskSetupUseCases = GenericUtilitiesUseCaseFactory.GetKioskSetupUseCases(executionContext);
                await kioskSetupUseCases.SaveKioskSetups(kioskSetupDTOList);
                log.LogMethodExit(kioskSetupDTOList);
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




