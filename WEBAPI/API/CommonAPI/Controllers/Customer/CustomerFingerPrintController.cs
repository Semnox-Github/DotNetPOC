/********************************************************************************************
 * Project Name - Customer
 * Description  - Created to Add CustomerFingerPrints
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.150.2     29-Nov-2022   Abhishek              Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ServerCore;

namespace Semnox.CommonAPI.Controllers.Customer
{
    public class CustomerFingerPrintController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Save CustomerFingerPrint
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/{accountId}/{machineId}/Fingerprint")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int accountId, [FromUri] int machineId, byte[] fpTemplate)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(accountId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (machineId < 0 || fpTemplate == null)
                {
                    log.LogMethodExit(accountId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IGameTransactionUseCases gameTransactionUseCases = GameTransactionUseCaseFactory.GetGameTransactionUseCases(executionContext);
                bool result = await gameTransactionUseCases.SaveCustomerFingerPrint(accountId, machineId, fpTemplate);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
