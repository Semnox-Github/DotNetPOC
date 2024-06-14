/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - API for the Account Entitlements
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.2     03-Feb-2023   Abhishek       Created 
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.ServerCore;

namespace Semnox.CommonAPI.Controllers.Customer.Accounts
{
    public class AccountEntitlementController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object GameServerPlayDTO.
        /// </summary>       
        [HttpGet]
        [Route("api/Customer/Account/{accountId}/Entitlement")]
        [Authorize]
        public async Task<HttpResponseMessage> Get([FromUri]int accountId, int machineId = -1, string machineReference = null, int productId = -1)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(machineId, accountId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IGameTransactionUseCases gameTransactionUseCases = GameTransactionUseCaseFactory.GetGameTransactionUseCases(executionContext);
                var content = await gameTransactionUseCases.AccountEntitlement(machineId, accountId);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
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
