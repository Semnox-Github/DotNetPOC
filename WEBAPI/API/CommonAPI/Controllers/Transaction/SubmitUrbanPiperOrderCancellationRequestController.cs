/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the SubmitUrbanPiperOrderCancellationRequest.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.140.0    08-Oct-2021     Fiona               Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Device.PaymentGateway;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class SubmitUrbanPiperOrderCancellationRequestController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Performs a Post operation on payment modes details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Transaction/SubmitUrbanPiperOrderCancellationRequest")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] Dictionary<TransactionDTO, string> transactionDictionary)
        {
            log.LogMethodEntry(transactionDictionary);
            ExecutionContext executionContext = null;

            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (transactionDictionary != null && transactionDictionary.Count > 0)
                {
                    ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                    var content = await transactionUseCases.SubmitUrbanPiperOrderCancellationRequest(transactionDictionary);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.Debug("Bad Request exiting...");
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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