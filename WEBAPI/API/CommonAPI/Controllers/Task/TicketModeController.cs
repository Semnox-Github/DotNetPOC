/********************************************************************************************
* Project Name - CommnonAPI - POS Task Module 
* Description  - API for the TicketMode Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.120.0     23-Mar-2021     Vikas Dwivedi       Created
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

namespace Semnox.CommonAPI.Task
{
    public class TicketModeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Post the JSON Object TicketModeDTO
        /// </summary>
        /// <param name="ticketModeDTO">ticketModeDTO</param>
        [HttpPost]
        [Route("api/Task/TicketMode")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] TicketModeDTO ticketModeDTO)
        {
            log.LogMethodEntry(ticketModeDTO);
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITaskUseCases taskUseCases = TaskUseCaseFactory.GetTaskUseCases(executionContext);
                TicketModeDTO tempTicketModeDTO = await taskUseCases.TicketModes(ticketModeDTO);
                log.LogMethodExit(tempTicketModeDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = tempTicketModeDTO,
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
