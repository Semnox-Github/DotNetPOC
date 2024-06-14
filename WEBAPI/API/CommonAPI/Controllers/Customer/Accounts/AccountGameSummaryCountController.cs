/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - AccountActivity API
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
**2.150.02   27-Mar-2023     Yashodhara C H        Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Customer.Accounts
{
    public class AccountGameSummaryCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ExecutionContext executionContext;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object Cards Collections.
        /// </summary>       
        [HttpGet]
        [Route("api/Customer/Account/AccountGameSummaryCount")]
        public async Task<HttpResponseMessage> Get(int accountId = -1, string accountNumber = null, DateTime? fromDate = null, DateTime? toDate = null, string creditPlusType = null, bool showExpiryEntitlements = false)
        {
            try
            {
                log.LogMethodEntry(accountId, accountNumber, fromDate, toDate, showExpiryEntitlements);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(executionContext);
                int count = await accountUseCases.GetAccountGameSummaryDTOListCount(accountId, accountNumber, fromDate, toDate, showExpiryEntitlements);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = count });
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
