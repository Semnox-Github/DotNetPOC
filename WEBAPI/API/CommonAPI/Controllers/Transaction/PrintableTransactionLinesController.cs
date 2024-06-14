
/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  -PrintableTransactionLinesController Controller
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *2.130.9    16-Jun-2022    guru s a        Created for execute online transaction changes in Kiosk
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.ViewContainer;
namespace Semnox.CommonAPI.Transaction
{
    /// <summary>
    /// PrintableTransactionLinesController
    /// </summary>
    public class PrintableTransactionLinesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/PrintableTransactionLines")]

        public async Task<HttpResponseMessage> Get(int transactionId, string printerTypeList, bool forVirtualStore)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId, printerTypeList, forVirtualStore);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                List<KeyValuePair<string,  List<TransactionLineDTO>>> printableLineDetailss = await transactionUseCases.GetPrintableTransactionLines(transactionId, printerTypeList, forVirtualStore);
                log.LogMethodExit(printableLineDetailss);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = printableLineDetailss });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
}
