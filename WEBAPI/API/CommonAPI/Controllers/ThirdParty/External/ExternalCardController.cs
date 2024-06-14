/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to fetch card details.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    11-Apr-2022   Abhishek                 Created - External  REST API
 ***************************************************************************************************/
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
using Semnox.Parafait.ThirdParty.External;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalCardController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the JSON Object Account
        /// </summary>
        /// <param name="accountId">accountId</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/External/Cards")]
        public async Task<HttpResponseMessage> Get(string isActive = "Y", bool chkVipCustomer = false, bool chkRoamingCards = true, bool chkTechCards = false, DateTime? fromDate = null, DateTime? toDate = null,
                                                   int accountId = -1, int customerId = -1, string cardNumber = null, int? tagSiteId = null, int pageNumber = 0, int pageSize = 0, bool buildChildRecords = false, bool activeRecordsOnly = false,
                                                   bool buildActivityHistory = false, bool buildGamePlayHistory = false, int lastActivityHistoryId = -1, int lastGamePlayHistoryId = -1, string entitlementType = null,
                                                   int activityHistoryPageNumber = 0, int gamePlayHistoryPageNumber = 0, bool buildBarCode = false, bool includeFutureEntitlements = false, bool closedTransactionsOnly = true,
                                                   DateTime? activityFromDate = null, DateTime? activityToDate = null, string customerName = null)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, chkVipCustomer, chkRoamingCards, chkTechCards, fromDate, toDate, accountId, customerId, cardNumber,
                                   pageNumber, pageSize, customerName);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(executionContext);
                AccountDTOCollection accountDTOCollection = await accountUseCases.GetAccounts(isActive, chkVipCustomer, chkRoamingCards, chkTechCards, fromDate, toDate,
                                accountId, customerId, cardNumber, tagSiteId, pageNumber, pageSize, buildChildRecords, activeRecordsOnly,
                                buildActivityHistory, buildGamePlayHistory, lastActivityHistoryId, lastGamePlayHistoryId, entitlementType,
                                activityHistoryPageNumber, gamePlayHistoryPageNumber, buildBarCode, includeFutureEntitlements, closedTransactionsOnly,
                                activityFromDate, activityToDate, customerName);
                ExternalCardsBL externalCardsBL = new ExternalCardsBL(executionContext, accountDTOCollection);
                List<ExternalCardsDTO> externalCardsDTO = externalCardsBL.GetAllExternalCardsDTOList();
                return Request.CreateResponse(HttpStatusCode.OK, externalCardsDTO);
            }
            catch (ValidationException valex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(valex) });
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