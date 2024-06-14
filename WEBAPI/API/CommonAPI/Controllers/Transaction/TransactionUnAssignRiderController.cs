/********************************************************************************************
* Project Name - CommnonAPI - Transaction Module  
* Description  - API for the TransactionUnAssignRider Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.140.0     10-Jun-2021     Fiona               Created
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
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class TransactionUnAssignRiderController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        [HttpPost]
        [Route("api/Transaction/{transactionId}/UnAssignRider")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO, [FromUri] int transactionId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionDeliveryDetailsDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (transactionDeliveryDetailsDTO != null && transactionId > -1)
                {
                    ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                    TransactionOrderDispensingDTO result = await transactionUseCases.UnAssignRider(transactionId, transactionDeliveryDetailsDTO);
                    log.LogMethodExit(result);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
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