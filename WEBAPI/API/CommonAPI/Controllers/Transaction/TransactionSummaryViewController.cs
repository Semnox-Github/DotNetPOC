/**************************************************************************************************
 * Project Name - Transaction 
 * Description  - TransactionSummaryViewController
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By         Remarks          
 **************************************************************************************************
  2.150.01    17-Feb-2023       Yashodhara C H      Created
 **************************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Transaction
{
    public class TransactionSummaryViewController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get Transaction Object.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/TransactionSummaryView")]
        public async Task<HttpResponseMessage> Get(int transactionId = -1, int orderId = -1, int posMachineId = -1, string transactionOTP = null,
                                                   string externalSystemReference = null, int customerId = -1, int poseTypeId = -1, DateTime? fromDate = null, DateTime? toDate = null,
                                                   string originalSystemReference = null, int user_id = -1, string transactionNumber = null, string remarks = null, string transactionIdList = null,
                                                   string transactionNumberList = null, string transactionOTPList = null, int originalTransactionId = -1,
                                                   string emailIdList = null, string phoneNumberList = null, int pageNumber = 0, int numberOdRecords = 100)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId, orderId, posMachineId, transactionOTP, externalSystemReference, customerId, poseTypeId, fromDate, toDate, originalSystemReference, user_id, transactionNumber,
                                    remarks, transactionIdList, transactionNumberList, transactionOTPList, originalTransactionId, emailIdList, phoneNumberList, pageNumber, numberOdRecords);             
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                ITransactionSummaryViewUseCases transactionSummaryViewUseCases = TransactionUseCaseFactory.GetTransactionSummaryViewUseCases(executionContext);
                List<TransactionSummaryViewDTO> transactionSummaryViewDTOList = await transactionSummaryViewUseCases.GetTransactionSummaryViewDTOList(transactionId, orderId, posMachineId, transactionOTP, externalSystemReference, customerId, poseTypeId, fromDate, toDate, originalSystemReference, user_id, transactionNumber,
                                    remarks, transactionIdList, transactionNumberList, transactionOTPList, originalTransactionId, emailIdList, phoneNumberList, pageNumber, numberOdRecords);
                log.LogMethodExit(transactionSummaryViewDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = transactionSummaryViewDTOList });

            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

    }
}


