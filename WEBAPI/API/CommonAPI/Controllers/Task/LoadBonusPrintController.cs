/********************************************************************************************
* Project Name - CommnonAPI - POS Task Module 
* Description  - API for the LoadBonusPrint Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.120.0     24-May-2021     Fiona               Created
********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Controllers.Task
{
    public class LoadBonusPrintController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/Bonus/{TransactionId}/Receipt")]
        public async Task<HttpResponseMessage> Get([FromUri] int transactionId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITaskUseCases taskUseCases = TaskUseCaseFactory.GetTaskUseCases(executionContext);
                ReceiptClass receiptClass = await taskUseCases.GetBonusReceipt(transactionId);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = receiptClass });

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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}