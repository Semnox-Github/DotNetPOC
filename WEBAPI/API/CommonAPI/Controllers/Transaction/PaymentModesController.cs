/********************************************************************************************
 * Project Name - PaymentModesController                                                                     
 * Description  - Controller for getting the payment modes
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.70.2       08-May-2019   Nitin Pai            Initial version
 *2.80        05-Apr-2020    Girish Kundar        Modified: API end point and removed token from the body 
 *2.90        15-Jul-2020    Girish Kundar        Modified: Added  paymentModeId =-1, bool loadChildRecords = false, string isActive as additional Get paramters
 *2.140.0     20-Aug-2021    Fiona                Modified: As per new version  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class PaymentModesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get Payment Gateways permitted for the respective device.
        /// </summary>
        [HttpGet]
        [Route("api/Transaction/PaymentModes")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string paymentChannel = null, int siteId = -1,  int paymentModeId =-1, bool loadChildRecords = false, string isActive = null, bool loadActiveChild = false)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(paymentChannel, paymentModeId, loadChildRecords);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<PaymentModeDTO> paymentModeDTOList = new List<PaymentModeDTO>();
                List<int> paymentModeIdList = new List<int>();
                
                List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                if(siteId != -1)
                {
                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                }
                else
                {
                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                }
                
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISACTIVE, isActive.ToString()));
                    }
                } 

                if (string.IsNullOrWhiteSpace(paymentChannel) && paymentModeId > -1 )
                {
                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModeId.ToString()));
                }
                if (string.IsNullOrEmpty(paymentChannel) == false)
                {
                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_CHANNEL_NAME, paymentChannel)); 
                }
                IPaymentModesUseCases paymentModesUseCases = PaymentModesUseCaseFactory.GetPaymentModesUseCases(executionContext);
                paymentModeDTOList = await paymentModesUseCases.GetPaymentModes(searchPaymentModeParameters, loadChildRecords, loadActiveChild);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = paymentModeDTOList });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Post operation on payment modes details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Transaction/PaymentModes")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<PaymentModeDTO> paymentModeDTOList)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(paymentModeDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (paymentModeDTOList != null && paymentModeDTOList.Count > 0)
                {
                    IPaymentModesUseCases paymentModesUseCases = PaymentModesUseCaseFactory.GetPaymentModesUseCases(executionContext);
                    paymentModesUseCases.SavePaymentModes(paymentModeDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }

        ///// <summary>
        ///// Performs a Delete operation on payment modes details
        ///// </summary>        
        ///// <returns>HttpResponseMessage</returns>
        //[HttpDelete]
        //[Route("api/Transaction/PaymentModes")]
        //[Authorize]
        //public HttpResponseMessage Delete([FromBody] List<PaymentModeDTO> paymentModeDTOList)
        //{
        //    SecurityTokenDTO securityTokenDTO = null;
        //    ExecutionContext executionContext = null;

        //    try
        //    {
        //        log.LogMethodEntry(paymentModeDTOList);
        //        SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        //        securityTokenBL.GenerateJWTToken();
        //        securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
        //        executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
        //        if (paymentModeDTOList != null && paymentModeDTOList.Count > 0)
        //        {
        //            PaymentModeList paymentModeList = new PaymentModeList(executionContext, paymentModeDTOList);
        //            paymentModeList.SaveUpdatePaymentModesList();
        //            log.LogMethodExit();
        //            return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
        //        }
        //        else
        //        {
        //            log.LogMethodExit();
        //            return Request.CreateResponse(HttpStatusCode.NotFound, new { data = ""  });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
        //        log.Error(customException);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException  });
        //    }
        //}
    }
}
