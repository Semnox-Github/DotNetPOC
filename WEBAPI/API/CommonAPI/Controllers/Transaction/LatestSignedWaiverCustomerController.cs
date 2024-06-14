/********************************************************************************************
* Project Name - CommnonAPI - Transaction Module 
* Description  - API for the LatestSignedWaiverCustomer Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.150.0     22-Dec-2022     Abhishek            Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class LatestSignedWaiverCustomerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        [HttpGet]
        [Route("api/Transaction/{transactionId}/LatestSignedCustomers")]
        [Authorize]
        public async Task<HttpResponseMessage> Get([FromUri]int transactionId, int totalCount = 10)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if(transactionId > -1)
                {
                    ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                    List<CustomerDTO> customerDTOList = await transactionUseCases.GetLatestSignedCustomers(transactionId, totalCount);
                    log.LogMethodExit(customerDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = customerDTOList });
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