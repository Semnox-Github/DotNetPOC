/********************************************************************************************
* Project Name - CommnonAPI - POS Task Module 
* Description  - API for the Exchange Tokens Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.120.0     07-Apr-2021     Abhishek            Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Controllers.Task
{
    public class ExchangeTokenController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Post the JSON Object ExchangeTokenDTO
        /// </summary>
        /// <param name="exchangeTokenDTO">exchangeTokenDTO</param>
        [HttpPost]
        [Route("api/Task/ExchangeTokens")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] ExchangeTokenDTO exchangeTokenDTO)
        {
            log.LogMethodEntry(exchangeTokenDTO);
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITaskUseCases taskUseCases = TaskUseCaseFactory.GetTaskUseCases(executionContext);
                ExchangeTokenDTO exchangeTokensDTO = await taskUseCases.ExchangeTokens(exchangeTokenDTO);
                log.LogMethodExit(exchangeTokensDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = exchangeTokensDTO,
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