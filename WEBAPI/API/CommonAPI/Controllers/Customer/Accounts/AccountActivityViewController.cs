/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - AccountActivity API
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.130.11   14-Oct-2022     Yashodhara C H       Created 
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
    public class AccountActivityViewController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ExecutionContext executionContext;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object Cards Collections.
        /// </summary>       
        [HttpGet]
        [Route("api/Customer/Account/{accountId}/AccountActivity")]
        public async Task<HttpResponseMessage> Get(int accountId, String startDate = "", String endDate = "", int numberOfDays = 180, int pageNumber = 0, bool addSummaryRow = false, int lastRowNumberId = -1)
        {
            try
            {
                log.LogMethodEntry(accountId, startDate, endDate, numberOfDays, pageNumber, addSummaryRow, lastRowNumberId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(executionContext);
                List<AccountActivityDTO> accountActivityDTOs = await accountUseCases.GetAccountActivityDTOList(accountId, startDate, endDate, numberOfDays, pageNumber, addSummaryRow, lastRowNumberId);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = accountActivityDTOs });
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
