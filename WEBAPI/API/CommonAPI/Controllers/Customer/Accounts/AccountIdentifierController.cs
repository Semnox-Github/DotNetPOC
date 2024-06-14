/********************************************************************************************
 * Project Name - Accounts
 * Description  - AccountIdentifierController class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.8      23-May-2022      Nitin Pai                 Created :
 *******************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.CommonAPI.Controllers.Customer.Accounts
{
    public class AccountIdentifierController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Generate the base64String for Account Activity.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Customer/Account/{accountId}/AccountIdentifier")]
        public async Task<HttpResponseMessage> Post([FromUri]int accountId, [FromBody]AccountDTO accountDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(executionContext);
                var content = await accountUseCases.SaveAccountIdentifier(accountId, accountDTO);
                return Request.CreateResponse(HttpStatusCode.OK, content);
            }
            catch (ValidationException ex)
            {                log.Error(ex);
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
