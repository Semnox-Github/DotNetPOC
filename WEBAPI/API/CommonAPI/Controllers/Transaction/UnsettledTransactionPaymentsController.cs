/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the TransactionUpdatePaymentModeDetails.
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
    public class UnsettledTransactionPaymentsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Post operation on payment modes details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Transaction/GetUnsettledTransactionPayments")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int transactionId = -1, int paymentModeId = -1, int deliveryChannelId = -1, DateTime? trxFromDate = null, DateTime? trxToDate = null)
        {

            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(transactionId, paymentModeId, deliveryChannelId, trxFromDate, trxToDate);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
               
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                var content = await transactionUseCases.GetUnsettledTransactionPayments(transactionId, paymentModeId, deliveryChannelId, trxFromDate, trxToDate);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
               
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