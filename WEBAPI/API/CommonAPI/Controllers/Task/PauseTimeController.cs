/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Pause Time Controller
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     18-May-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Task
{
    public class PauseTimeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        [HttpPost]
        [Route("api/Task/Time/Status")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] AccountTimeStatusDTO accountPauseDTO)
        {
            log.LogMethodEntry(accountPauseDTO);
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITaskUseCases taskUseCases = TaskUseCaseFactory.GetTaskUseCases(executionContext);
                AccountTimeStatusDTO accountTimeStatusDTO =  await taskUseCases.UpdateTimeStatus(accountPauseDTO);
                log.LogMethodExit(accountTimeStatusDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = accountTimeStatusDTO,
                });
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}