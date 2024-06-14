/********************************************************************************************
 * Project Name -Communication
 * Description  -RemoteMessagingRequestUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By            Remarks          
 *********************************************************************************************
 2.150.01     24-Jan-2023      Yashodhara C H          Create
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Communication
{
    class RemoteMessagingRequestsUseCases:RemoteUseCases,IMessagingRequestsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MESSAGING_REQUEST_URL = "api/Communication/MessagingRequests/Resend";
        private const string MESSAGING_REQUEST_VIEW_URL = "api/Communication/MessagingRequestSummaryView";
        public RemoteMessagingRequestsUseCases(ExecutionContext executionContext)
          : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to resend email.
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task<string> SaveMessagingRequestDTO(string idList)
        {
            log.LogMethodEntry(idList);
            try
            {
                string responseString = await Post<string>(MESSAGING_REQUEST_URL, idList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<MessagingRequestSummaryViewDTO>> GetMessagingRequestSummaryViewDTOList(int messageId = -1, int parentAndChildMessagesById = -1, string messageIdList = null, int customerId = -1, int cardId = -1,
                                                                    string messageType = null, DateTime? fromDate = null, DateTime? toDate = null,  
                                                                    string parafaitFunctionName = null, int originalMessageId = -1, string toMobileList = null, string toEmailList = null, 
                                                                    string trxNumber = null, string trxOTP = null, string parafaitFunctionEventName = null, int pageNumber = 0, int numberofRecords = -1,
                                                                    string trxNumberList = null, string trxOTPList = null)
        {
            log.LogMethodEntry(messageId, parentAndChildMessagesById, messageIdList, customerId, cardId, messageType, fromDate, toDate, parafaitFunctionName, originalMessageId, toMobileList, toEmailList, trxOTP, parafaitFunctionEventName, pageNumber, numberofRecords, trxNumberList, trxOTPList);


            List<KeyValuePair<string, string>> searchParameters = new List<KeyValuePair<string, string>>();
            searchParameters.Add(new KeyValuePair<string, string>("messageId", messageId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("parentAndChildMessagesById", parentAndChildMessagesById.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("messageIdList", messageIdList));
            searchParameters.Add(new KeyValuePair<string, string>("customerId", customerId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("cardId", cardId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("messageType", messageType));
            searchParameters.Add(new KeyValuePair<string, string>("fromDate", fromDate.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("toDate", toDate.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("parafaitFunctionName", parafaitFunctionName));
            searchParameters.Add(new KeyValuePair<string, string>("originalMessageId", originalMessageId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("toMobile", toMobileList));
            searchParameters.Add(new KeyValuePair<string, string>("toEmails", toEmailList));
            searchParameters.Add(new KeyValuePair<string, string>("trxNumber", trxNumber));
            searchParameters.Add(new KeyValuePair<string, string>("trxOTP", trxOTP));
            searchParameters.Add(new KeyValuePair<string, string>("parafaitFunctionEventName", parafaitFunctionEventName));
            searchParameters.Add(new KeyValuePair<string, string>("pageNumber", pageNumber.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("numberofRecords", numberofRecords.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("trxNumberList", trxNumberList));
            searchParameters.Add(new KeyValuePair<string, string>("trxOTPList", trxOTPList));
            try
            {
                List<MessagingRequestSummaryViewDTO> result = await Get<List<MessagingRequestSummaryViewDTO>>(MESSAGING_REQUEST_VIEW_URL, searchParameters);
                log.LogMethodExit(result);
                return result;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

        }
    }
}
