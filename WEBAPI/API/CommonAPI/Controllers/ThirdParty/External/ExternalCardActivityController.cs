/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to fetch card activity details.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    29-Jul-2022   Abhishek                 Created - External  REST API
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalCardActivityController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the JSON Object Account
        /// </summary>
        /// <param name="accountId">accountId</param>
        /// <param name="addSummaryRow">addSummaryRow</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/External/Account/{accountId}/Activities")]
        [Authorize]
        public HttpResponseMessage Get([FromUri] int accountId, bool addSummaryRow = false, int pageSize = -1, int currentPage = -1, int lastRowNumberId = -1)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(accountId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>>();
                if (accountId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.ACCOUNT_ID, accountId.ToString()));
                }
                else
                {
                    if (accountId < 0)
                    {
                        string customException = "Invalid inputs - Account Id  is empty";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }
                AccountActivityViewListBL accountActivityViewListBL = new AccountActivityViewListBL(executionContext);
                List<AccountActivityDTO> accountActivityDTOList = accountActivityViewListBL.GetAccountActivityDTOList(searchParameters, addSummaryRow, null, pageSize, currentPage, lastRowNumberId);
                if (accountActivityDTOList == null/* && !accountActivityDTOList.Any()*/)
                {
                    string messsage = "Card Activity Details not found";
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = messsage });
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { data = accountActivityDTOList });
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