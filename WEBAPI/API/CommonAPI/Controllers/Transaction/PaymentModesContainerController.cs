/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the PaymentModesContainer.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.140.0    19-Aug-2021     Fiona               Created 
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http; 
using Semnox.Core.Utilities;  
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class PaymentModesContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/PaymentModesContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);

                PaymentModesContainerDTOCollection paymentModesContainerDTOCollection = await
                          Task<PaymentModesContainerDTOCollection>.Factory.StartNew(() =>
                          {
                              return PaymentModesViewContainerList.GetPaymentModesContainerDTOCollection(siteId, hash, rebuildCache);
                          });

                log.LogMethodExit(paymentModesContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = paymentModesContainerDTOCollection });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
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