/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to pause and resume time.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.150.0    18-Sep-2023   Abhishek                 Created - External  REST API
 ***************************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalPauseTimeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object AccountTimeStatus
        /// </summary>
        /// <param name="accountPauseDTO">accountTimeStatusDTO</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/External/Time/Status")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] AccountTimeStatusDTO accountPauseDTO)
        {
            log.LogMethodEntry(accountPauseDTO);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITaskUseCases taskUseCases = TaskUseCaseFactory.GetTaskUseCases(executionContext);
                AccountTimeStatusDTO accountTimeStatusDTO = await taskUseCases.UpdateTimeStatus(accountPauseDTO);
                log.LogMethodExit(accountTimeStatusDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = accountTimeStatusDTO,
                });
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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