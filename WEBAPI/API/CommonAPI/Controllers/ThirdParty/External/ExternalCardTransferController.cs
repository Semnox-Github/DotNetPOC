/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to transfer card.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.140.5    20-Jan-2022   Abhishek                 Created - External  REST API
 ***************************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty.External;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalCardTransferController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object Account Transfer
        /// </summary>
        /// <param name="sourceAccountNumber">sourceAccountNumber</param>
        /// <param name="destinationAccountNumber">destinationAccountNumber</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/External/Account/{sourceAccountNumber}/Transfer")]
        [Authorize]
        public HttpResponseMessage Post([FromUri]string sourceAccountNumber, [FromBody]string destinationAccountNumber)
        {
            log.LogMethodEntry(sourceAccountNumber, destinationAccountNumber);
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            try
            {
                ExternalCardTasksDTO externalCardTasksDTO = new ExternalCardTasksDTO(AccountTaskType.TRANSFER_CARD,-1,-1,-1,-1,null,DateTime.Now,string.Empty,
                    null,-1,-1,-1,string.Empty,false, sourceAccountNumber, destinationAccountNumber,string.Empty);
                ExternalCardTasksBL externalCardTasksBL = new ExternalCardTasksBL(executionContext, externalCardTasksDTO);
                string result = externalCardTasksBL.PerformTask();
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }
    }
}