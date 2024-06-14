/********************************************************************************************
* Project Name - CommnonAPI - POS Task Module 
* Description  - API for the RefundCardController Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.150.2     22-Feb-2023     Abhishek            Created - Game Server Cloud Movement.
********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Controllers.Task
{
    public class RefundCardController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        [HttpPost]
        [Route("api/Task/RefundCard")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] RefundCardDTO refundCardDTO)
        {
            log.LogMethodEntry(refundCardDTO);
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITaskUseCases taskUseCases = TaskUseCaseFactory.GetTaskUseCases(executionContext);
                RefundCardDTO result = await taskUseCases.RefundCard(refundCardDTO);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = result,
                });
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