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
    public class TransactionUpdateRiderAssignmentRemarksController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        [HttpPost]
        [Route("api/Transaction/{transactionId}/UpdateRiderAssignmentRemarks")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList, [FromUri] int transactionId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionDeliveryDetailsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (transactionDeliveryDetailsDTOList != null && transactionId > -1)
                {
                    ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                    TransactionOrderDispensingDTO result = await transactionUseCases.SaveRiderAssignmentRemarks(transactionId, transactionDeliveryDetailsDTOList);
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