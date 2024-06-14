/********************************************************************************************
 * Project Name - Communications
 * Description  - Controller for MessagingRequestSummaryView class.
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 2.150.01    02-Feb-2023     Yashodhara C H      Created
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;

namespace Semnox.CommonAPI.Controllers.Communication
{
    public class MessagingRequestSummaryViewController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation on messagingViewDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Communication/MessagingRequestSummaryView")]
        public async Task<HttpResponseMessage> Get(int messageId = -1, int parentAndChildMessagesById = -1, string messageIdList = null, int customerId = -1, int cardId = -1, string messageType = null, DateTime? fromDate = null,
                                                    DateTime? toDate = null, string parafaitFunctionName = null, int originalMessageId = -1, 
                                                    string toMobileList = null, string toEmailList = null, string trxNumber = null, string trxOTP = null, 
                                                    string parafaitFunctionEventName = null, int pageNumber = 0, int numberofRecords = 100, string trxNumberList = null, string trxOTPList = null)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(messageId, parentAndChildMessagesById, messageIdList, customerId, cardId, messageType, fromDate, toDate, parafaitFunctionName, originalMessageId, toMobileList, toEmailList, trxNumber, trxOTP, parafaitFunctionEventName, pageNumber, numberofRecords, trxNumberList, trxOTPList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IMessagingRequestsUseCases messagingRequestsUseCases = CommunicationUseCaseFactory.GetMessagingRequestsUseCases(executionContext);
                List<MessagingRequestSummaryViewDTO> result = await messagingRequestsUseCases.GetMessagingRequestSummaryViewDTOList(messageId, parentAndChildMessagesById, messageIdList, customerId, cardId, messageType, fromDate, toDate, parafaitFunctionName,
                                                                                                                                    originalMessageId, toMobileList, toEmailList , trxNumber, trxOTP, parafaitFunctionEventName, pageNumber, numberofRecords, trxNumberList, trxOTPList);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
