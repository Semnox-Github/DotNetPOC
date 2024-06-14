/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to Activate/Issue card.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.11   28-Oct-2022   Abhishek                 Created - External  REST API
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
    public class ExternalCardActivateController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object Account
        /// </summary>
        /// <param name="accountNumber">accountNumber</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/External/Account/Activate")]
        [Authorize]
        public HttpResponseMessage Post(string accountNumber)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(accountNumber);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                ExternalTransactionListBL externalTransactionBL = new ExternalTransactionListBL(executionContext);
                externalTransactionBL.Activate(accountNumber);
                string message = "Account Issued Successfully";
                log.LogMethodExit(message);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = message });
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