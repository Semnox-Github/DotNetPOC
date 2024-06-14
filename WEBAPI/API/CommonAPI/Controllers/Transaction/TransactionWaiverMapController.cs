/********************************************************************************************
* Project Name - CommnonAPI - Transaction Module 
* Description  - API for the TransactionWaiverMap Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.150.0     12-Dec-2022     Abhishek            Created
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
using Semnox.Parafait.Waiver;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class TransactionWaiverMapController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        [HttpPost]
        [Route("api/Transaction/{transactionId}/MapCustomerSignedWaiverToTransaction")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri]int transactionId, [FromBody]List<WaiverSignatureDTO> waiverSignatureDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(waiverSignatureDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if(waiverSignatureDTOList != null && waiverSignatureDTOList.Any() && transactionId > -1)
                {
                    ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                    List<WaiverSignatureDTO> waiverSignatureList = await transactionUseCases.SaveWaiverSignatures(transactionId, waiverSignatureDTOList);
                    log.LogMethodExit(waiverSignatureList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = waiverSignatureList });
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