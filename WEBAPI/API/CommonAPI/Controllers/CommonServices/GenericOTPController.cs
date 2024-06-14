/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the GenericOTP
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.130.11   18-Aug-2022    Yashodhara C H     Created 
 ********************************************************************************************/

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.CommonServices
{
    public class GenericOTPController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Performs a Get operation on genericOTP
        /// </summary>
        [HttpPost]
        [Route("api/CommonServices/GenericOTP/Create")]
        [Authorize]
        public async Task<HttpResponseMessage> GenerateOTP([FromBody] GenericOTPDTO genericOTPDTO)
        {
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            SecurityTokenDTO securityTokenDTO = null;

            try
            {
                log.LogMethodEntry();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (genericOTPDTO != null)
                {
                    IGenericOTPUseCases genericOTPUseCases = GenericOTPUseCaseFactory.GetGenericOTPUseCases(executionContext);
                    GenericOTPDTO result = await genericOTPUseCases.GenerateGenericOTP(genericOTPDTO);
                    log.LogMethodExit(result);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Invalid input DTO" });
                }

            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Validates the OTP
        /// </summary>
        [HttpPost]
        [Route("api/CommonServices/GenericOTP/{id}/Validate")]
        [Authorize]
        public  async Task<HttpResponseMessage> ValidateOTP([FromUri] int id, [FromBody]GenericOTPDTO genericOTPDTO)
        {
            log.LogMethodEntry(genericOTPDTO);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            SecurityTokenDTO securityTokenDTO = null;

            try
            { 
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (id > -1 && genericOTPDTO != null)
                {
                    genericOTPDTO.Id = id;
                    IGenericOTPUseCases genericOTPUseCases = GenericOTPUseCaseFactory.GetGenericOTPUseCases(executionContext);
                    await genericOTPUseCases.ValidateGenericOTP(genericOTPDTO);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "Validated Successfully" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Invalid input DTO" });
                }       
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex)});
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Get operation on genericOTP
        /// </summary>
        [HttpPost]
        [Route("api/CommonServices/GenericOTP/Resend")]
        [Authorize]
        public async Task<HttpResponseMessage>  ReSendOTP([FromBody] GenericOTPDTO genericOTPDTO)
        {
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            SecurityTokenDTO securityTokenDTO = null;

            try
            {
                log.LogMethodEntry();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (genericOTPDTO != null)
                {
                    IGenericOTPUseCases genericOTPUseCases = GenericOTPUseCaseFactory.GetGenericOTPUseCases(executionContext);
                    GenericOTPDTO genericOTPDTOValue = await genericOTPUseCases.ReSendGenericOTP(genericOTPDTO);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = genericOTPDTOValue });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Invalid input DTO" });
                }

            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
