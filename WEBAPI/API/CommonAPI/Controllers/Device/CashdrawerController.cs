/********************************************************************************************
 * Project Name -Device
 * Description  - API for the Cashdrawers
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.140.0    11-Aug-2021   Girish Kundar         Created : Multi Cashdrawer to POS 
 *2.140.0   09-Mar-2022  Abhishek                WMS Fix : Display all cashdrawers if IsActive is false.
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer.Cashdrawers;
using Semnox.Parafait.Redemption;
namespace Semnox.CommonAPI.Controllers.Device
{
    public class CashdrawerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of Ticket Station Details
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Device/Cashdrawers")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int cashdrawerId = -1, string cashdrawerName = null)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive, cashdrawerId, cashdrawerName);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<CashdrawerDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CashdrawerDTO.SearchByParameters, string>>()
                {
                     new KeyValuePair<CashdrawerDTO.SearchByParameters, string>(CashdrawerDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString())
                };
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<CashdrawerDTO.SearchByParameters, string>(CashdrawerDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
               
                if (string.IsNullOrEmpty(cashdrawerName) == false)
                {
                    searchParameters.Add(new KeyValuePair<CashdrawerDTO.SearchByParameters, string>(CashdrawerDTO.SearchByParameters.CASHDRAWER_NAME, cashdrawerName.ToString()));
                }
                if (cashdrawerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<CashdrawerDTO.SearchByParameters, string>(CashdrawerDTO.SearchByParameters.CAHSDRAWER_ID, cashdrawerId.ToString()));
                }
                ICashdrawerUseCases cashdrawerUseCases = CashdrawerUseCaseFactory.GetCashdrawerUseCases(executionContext);
                var content = await cashdrawerUseCases.GetCashdrawers(searchParameters);
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
        [Route("api/Device/Cashdrawers")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<CashdrawerDTO> cashdrawerDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(cashdrawerDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (cashdrawerDTOList != null && cashdrawerDTOList.Any())
                {
                    ICashdrawerUseCases cashdrawerUseCases = CashdrawerUseCaseFactory.GetCashdrawerUseCases(executionContext);
                    var content = await cashdrawerUseCases.SaveCashdrawers(cashdrawerDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
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
