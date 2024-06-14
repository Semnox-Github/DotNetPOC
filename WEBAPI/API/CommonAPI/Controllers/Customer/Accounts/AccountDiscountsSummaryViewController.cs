/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - API for the DiscountsCards "CardDiscountView" entity. Created to fetch Cards.
 * 
 **************
 **Version Log
 **************
 *Version      Date         Modified By       Remarks          
 *********************************************************************************************
 *2.130.11   10-Sep-2022   Yashodhara C H      Created 
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


namespace Semnox.CommonAPI.Controllers.Customer.Accounts
{
    public class AccountDiscountsSummaryViewController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object Cards Collections.
        /// </summary>
        [HttpGet]
        [Route("api/Customer/Account/{accountId}/AccountDiscountsSummary")]
        public async Task<HttpResponseMessage> Get([FromUri] int accountId)
        {
            try
            {
                log.LogMethodEntry(accountId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(executionContext);
                List<AccountDiscountsSummaryViewDTO> accountDiscountsSummaryViewDTOs = await accountUseCases.GetAccountDiscountsSummaryViewDTOList(accountId);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = accountDiscountsSummaryViewDTOs });
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

