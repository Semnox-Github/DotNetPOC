/********************************************************************************************
* Project Name - CommnonAPI - Transaction Module 
* Description  - API for the TransactionOrderDispensing Controller.
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
    public class TransactionOrderDispensingController: ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        [HttpPost]
        [Route("api/Transaction/TransactionOrderDispensing")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<TransactionOrderDispensingDTO> transactionOrderDispensingDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionOrderDispensingDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (transactionOrderDispensingDTOList != null)
                {
                    ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                    List<TransactionOrderDispensingDTO> result = await transactionUseCases.CreateTransactionOrderDispensingDTO(transactionOrderDispensingDTOList);
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