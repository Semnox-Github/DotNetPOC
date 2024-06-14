/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - ExternalPaymentController  API -  add and delete payment data in Parafait
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By                Remarks          
 *********************************************************************************************
 *2.130.7     07-Apr-2022     Vignesh Bhat              Created - External  REST API           
 **********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.ThirdParty.External;
using Semnox.Parafait.ThirdParty.CenterEdge;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalTransactionPaymentController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Payment
        /// </summary>       
        /// <param name="externalAddPaymentDTO">externalAddPaymentDTO</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/External/Transaction/{transactionId}/Payment")]
        [Authorize]
        public HttpResponseMessage Post([FromUri] int transactionId, [FromBody] ExternalAddPaymentDTO externalAddPaymentDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId, externalAddPaymentDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (externalAddPaymentDTO == null)
                {
                    string customException = "Payment data cannot be null.Please enter the Payment Details";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                ExternalTransactionListBL externalTransactionBL = new ExternalTransactionListBL(executionContext);
                ExternalTransactionDTO externalTransactionDTO = externalTransactionBL.AddPayment(transactionId, externalAddPaymentDTO);
                //string message = "Payment Added Successfully";
                log.LogMethodExit(externalTransactionDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = externalTransactionDTO });
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = customException });
            }
        }

        /// <summary>
        /// Delete the JSON Payment
        /// </summary>     
        /// <param name="externalRemovePaymentDTO">externalRemovePaymentDTO</param>
        /// <returns>HttpResponseMessage</returns>    
        [HttpDelete]
        [Route("api/External/Transaction/{transactionId}/Payment")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete([FromUri] int transactionId, [FromBody]ExternalRemovePaymentDTO externalRemovePaymentDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId, externalRemovePaymentDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (externalRemovePaymentDTO == null)
                {
                    string customException = "Payment data cannot be null.Please enter the Payment Details";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                ExternalTransactionListBL externalTransactionBL = new ExternalTransactionListBL(executionContext);
                externalTransactionBL.RemovePayment(transactionId, externalRemovePaymentDTO);
                string message = "Payment Removed Successfully";
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK,  new { data = message } );
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
