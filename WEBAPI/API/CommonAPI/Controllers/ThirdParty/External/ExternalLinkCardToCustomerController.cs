/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to link card to customer.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    11-Apr-2022   Abhishek                 Created - External  REST API
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
    public class ExternalLinkCardToCustomerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Account
        /// </summary>       
        /// <param name="customerId">customerId</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/External/Account/{accountId}/LinkCustomer")]
        [Authorize]
        public HttpResponseMessage POST([FromUri] int accountId, int customerId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(accountId, customerId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (accountId < 0 || customerId < 0)
                {
                    string customException = "Invalid inputs - Account Id or Customer Id is Empty";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                ExternalCardsBL externalCardsBL = new ExternalCardsBL(executionContext, accountId);
                externalCardsBL.LinkCardToCustomer(customerId);
                string message = "Account is Linked Successfully";
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