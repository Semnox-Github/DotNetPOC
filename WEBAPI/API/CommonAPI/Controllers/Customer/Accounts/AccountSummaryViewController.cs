/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - API for the Cards "ViewCards" entity. Created to fetch Cards.
 * 
 **************
 **Version Log
 **************
 *Version      Date         Modified By       Remarks          
 *********************************************************************************************
 *2.130.11   10-Aug-2022   Yashodhara C H      Created 
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
    public class AccountSummaryViewController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object Cards Collections.
        /// </summary>
        [HttpGet]
        [Route("api/Customer/Account/AccountSummaryView")]
        public async Task<HttpResponseMessage> Get(string isActive = null, string accountIdList = null, string accountNumberList = null, int accountId = -1,
                                                   string accountNumber = null, bool validFlag = false, int customerId = -1, int? cardTypeId = -1,
                                                   int uploadSiteId = -1, int masterEntityId = -1, string cardIdentifier = null, int membershipId = -1,string membershipName = null, DateTime? entitlementFromDate = null, DateTime? entitlementToDate = null, bool includeFutureEntitlements = false, bool showExpiryEntitlements = false)
        {
            try
            {
                log.LogMethodEntry(isActive, accountId, accountIdList, accountNumberList, accountNumber, validFlag, customerId, cardTypeId, uploadSiteId, masterEntityId, cardIdentifier,
                                   membershipId, membershipName, entitlementFromDate, entitlementToDate, includeFutureEntitlements, showExpiryEntitlements);

                //Prevent a blank search of accounts
                if (customerId == -1 && accountId == -1 && accountIdList == null && String.IsNullOrWhiteSpace(accountNumber))
                {
                    log.Fatal("Blank search encountered. Forcing an error. " + this.Request.Content);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = "Invalid Search Parameters" });
                }

                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(executionContext);
                List<AccountSummaryViewDTO> accountSummaryViewDTO = await accountUseCases.GetAccountSummaryViewDTOList(isActive, accountIdList, accountNumberList, accountId, accountNumber, validFlag, customerId,
                                                                                                           cardTypeId, uploadSiteId, masterEntityId, cardIdentifier,
                                                                                                           membershipId, membershipName, entitlementFromDate, entitlementToDate, includeFutureEntitlements, showExpiryEntitlements);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = accountSummaryViewDTO });
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

