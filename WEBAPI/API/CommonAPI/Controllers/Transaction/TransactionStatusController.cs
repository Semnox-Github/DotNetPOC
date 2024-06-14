/********************************************************************************************
* Project Name - CommnonAPI - Transaction Module 
* Description  - API for the TransactionStatus Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.140.0     09-Jun-2021     Fiona               Created
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
    public class TransactionStatusController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        [HttpPost]
        [Route("api/Transaction/Transactions/Status")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<TransactionDTO> inputList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inputList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if(inputList != null && inputList.Any())
                {
                    ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                    List<KeyValuePair<TransactionDTO, string>> resultList = await transactionUseCases.UpdateTransactionStatus(inputList);
                    log.LogMethodExit(resultList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = resultList });
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