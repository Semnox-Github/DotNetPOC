/********************************************************************************************
* Project Name - CommnonAPI - POS Task Module 
* Description  - API for the TicketMode Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.120.0     07-Apr-2021     Fiona               Created
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
    public class BalanceTransferController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Post the JSON Object BalanceTransferDTO
        /// </summary>
        /// <param name="balanceTransferDTO">balanceTransferDTO</param>
        [HttpPost]
        [Route("api/Task/BalanceTransfer")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] BalanceTransferDTO balanceTransferDTO)
        {
            log.LogMethodEntry(balanceTransferDTO);
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITaskUseCases taskUseCases = TaskUseCaseFactory.GetTaskUseCases(executionContext);
                BalanceTransferDTO tempBalanceTransferDTO = await taskUseCases.BalanceTransfer(balanceTransferDTO);
                log.LogMethodExit(tempBalanceTransferDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = tempBalanceTransferDTO,
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