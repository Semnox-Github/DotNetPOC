/********************************************************************************************
* Project Name - CommnonAPI - POS Task Module 
* Description  - API for the RedeemEntitlements Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.120.0     27-May-2021     Fiona               Created
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
    public class RedeemEntitlementsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        [HttpPost]
        [Route("api/Task/RedeemEntitlements")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] RedeemEntitlementDTO redeemEntitlementDTO)
        {
            log.LogMethodEntry(redeemEntitlementDTO);
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITaskUseCases taskUseCases = TaskUseCaseFactory.GetTaskUseCases(executionContext);
                RedeemEntitlementDTO result = await taskUseCases.RedeemEntitlements(redeemEntitlementDTO);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = result,
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