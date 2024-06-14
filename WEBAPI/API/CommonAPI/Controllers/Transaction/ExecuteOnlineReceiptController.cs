/********************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - Controller for performing the Execute Online transaction Receipt related operations
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 ********************************************************************************************* 
 *2.150.5      28-Sep-2023    Guru S A             Execute online transaction changes in Kiosk
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
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class ExecuteOnlineReceiptController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Semnox.Core.Utilities.Utilities Utilities = new Semnox.Core.Utilities.Utilities();
         
        /// <summary>
        /// 
        /// </summary>
        [HttpPost]
        [Route("api/Transaction/{TransactionId}/TransactionServices/ExecuteOnlineReceipt")]
        [Authorize]
        public async Task<HttpResponseMessage> PrintExecuteOnlinetaskReceipt([FromUri] int TransactionId, [FromBody]List<int> taskInfoList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(TransactionId, taskInfoList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                TransactionDTO result = await transactionUseCases.PrintExecuteOnlinetaskReceipt(TransactionId, taskInfoList);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
