/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - AccountRelationshipUseCaseController to get Account Relationship
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.150.2     28-Nov-2022   Abhishek              Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.ServerCore;

namespace Semnox.CommonAPI.Controllers.Customer.Accounts
{
    public class AccountRelationshipUseCaseController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Account Relationship.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Customer/Account/{accountId}/AccountRelationshipUseCase")]
        public async Task<HttpResponseMessage> Get([FromUri]int accountId, int machineId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(accountId, machineId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (accountId < 0)
                {
                    log.LogMethodExit(accountId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IGameTransactionUseCases gameTransactionUseCases = GameTransactionUseCaseFactory.GetGameTransactionUseCases(executionContext);
                string cardNumber = await gameTransactionUseCases.GetAccountRelationshipUseCases(accountId, machineId);
                log.LogMethodExit(cardNumber);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = cardNumber });
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