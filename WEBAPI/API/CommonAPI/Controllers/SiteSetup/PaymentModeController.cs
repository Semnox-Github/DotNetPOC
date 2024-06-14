/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the payment modes Controller class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         13-Mar-2019   Jagan Mohana         Created 
 **********************************************************************************************
 *2.60         25-Mar-2019   Akshay Gulaganji     passed executionContext in get() and modified response in Catch block 
               10-Apr-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                  Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
                                                  Changed Get method from GetPaymentModeList to GetAllPaymentModeList[To Access Child records.]
 ********************************************************************************************/
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using System;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device.PaymentGateway;

namespace Semnox.CommonAPI.SiteSetup
{
    public class PaymentModeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object payment modes Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/PaymentMode/")]
        public HttpResponseMessage Get()
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));

                PaymentModeList paymentModeList = new PaymentModeList(executionContext);
                var content = paymentModeList.GetAllPaymentModeList(searchParameters, true, null);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Post operation on payment modes details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/PaymentMode/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<PaymentModeDTO> paymentModeDTOList)
        {
            try
            {
                log.LogMethodEntry(paymentModeDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (paymentModeDTOList != null && paymentModeDTOList.Count > 0)
                {
                    // if paymentModeDTOs.paymentModeId is less than zero then insert or else update
                    PaymentModeList paymentModeList = new PaymentModeList(executionContext, paymentModeDTOList);
                    paymentModeList.SaveUpdatePaymentModesList();
                    log.LogMethodExit();
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
        /// Performs a Delete operation on payment modes details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/PaymentMode/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<PaymentModeDTO> paymentModeDTOList)
        {
            try
            {
                log.LogMethodEntry(paymentModeDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (paymentModeDTOList != null && paymentModeDTOList.Count > 0)
                {
                    PaymentModeList paymentModeList = new PaymentModeList(executionContext, paymentModeDTOList);
                    paymentModeList.SaveUpdatePaymentModesList();
                    log.LogMethodExit();
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
