/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to save Card Identifier.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.140.5    19-Apr-2023   Abhishek                 Created - External  REST API
 ***************************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty.External;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalCardIdentifierController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object Cards
        /// </summary>
        /// <param name="accountIdentifier">accountNumber</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/External/Account/{accountId}/AccountIdentifier")]
        [Authorize]
        public HttpResponseMessage Post([FromUri]int accountId, [FromBody] string accountIdentifier)
        {
            log.LogMethodEntry(accountIdentifier);
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            try
            {
                ExternalTransactionBL externalTransactionBL = new ExternalTransactionBL(executionContext, accountId);
                externalTransactionBL.SaveAccountIdentifier(accountIdentifier);
                string result = "Account Identifier Saved Successfully";
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }
    }
}