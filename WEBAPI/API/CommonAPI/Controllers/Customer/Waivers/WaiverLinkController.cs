/********************************************************************************************
 * Project Name - Transaction
 * Description  - API for the WaiverLink 
 * 
 **************
 **Version Log
 **************
 *Version     Date         Modified By           Remarks          
 *********************************************************************************************
 *2.110       15-Feb-2021  Girish Kundar        Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Customer.Waiver
{
    public class WaiverLinkController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object PaymentLinks 
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Customer/Waiver/Links")]
        public async Task<HttpResponseMessage> Get(int transactionId = -1)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (transactionId < 0)
                {
                    log.Error("Invalid transaction id");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Invalid transaction id"));
                }
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                var waiverLink = await transactionUseCases.GetWaiverLinks(transactionId);
                log.LogMethodExit(waiverLink);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = waiverLink });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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