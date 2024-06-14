/********************************************************************************************
* Project Name - Communication
* Description  - LocalMessagingRequestsUseCases class 
*  
**************
**Version Log
**************
*Version       Date             Modified By               Remarks          
*********************************************************************************************
 2.150.01     24-Jan-2023      Yashodhara C H          Create
********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Semnox.Parafait.Communication
{
    public class LocalMessagingRequestsUseCases: IMessagingRequestsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalMessagingRequestsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to resend email.
        /// </summary>
        /// <param name="messageIdList"></param>
        /// <returns></returns>
        public async Task<string> SaveMessagingRequestDTO(string messageIdList)
        {
            return await Task<string>.Factory.StartNew(() => {
                log.LogMethodEntry(messageIdList);
                if (messageIdList == null)
                {
                    string errorMessage = "messageIdList is empty";
                    log.LogMethodExit("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                string result = string.Empty;
                List<KeyValuePair<MessagingRequestDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingRequestDTO.SearchByParameters, string>>();
                messageIdList = Regex.Replace(messageIdList, @"\s", "");
                List<string> uniqueIds = messageIdList.Split(',').Distinct().ToList();
                string idList = string.Join(",", uniqueIds);
                searchParameters.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.ID_LIST, idList));
                MessagingRequestListBL messagingRequestListBL = new MessagingRequestListBL(executionContext);
                List<MessagingRequestDTO> messagingRequestDTOList = messagingRequestListBL.GetAllMessagingRequestList(searchParameters);
      
                if (uniqueIds.Count() == messagingRequestDTOList.Count())
                {
                    if (messagingRequestDTOList != null && messagingRequestDTOList.Any())
                    {
                        foreach (MessagingRequestDTO messagingRequest in messagingRequestDTOList)
                        {
                            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                            {
                                try
                                {
                                    parafaitDBTrx.BeginTransaction();
                                    MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, messagingRequest.BatchId, messagingRequest.Reference, messagingRequest.MessageType,
                                                                                                          messagingRequest.ToEmail, messagingRequest.ToMobile, null, null,
                                                                                                          null, null, null, messagingRequest.Subject,
                                                                                                          messagingRequest.Body, messagingRequest.CustomerId, messagingRequest.CardId, messagingRequest.AttachFile,
                                                                                                          messagingRequest.ActiveFlag, messagingRequest.Cc, messagingRequest.Bcc, messagingRequest.MessagingClientId,
                                                                                                          false, messagingRequest.ToDevice, messagingRequest.SignedInCustomersOnly, messagingRequest.CountryCode,
                                                                                                          messagingRequest.TrxNumber, messagingRequest.ParafaitFunctionEventId, messagingRequest.Id, messagingRequest.TrxId);
                                    MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                                    messagingRequestBL.Save(parafaitDBTrx.SQLTrx);
                                    parafaitDBTrx.EndTransaction();
                                }
                                catch (ValidationException ex)
                                {
                                    log.Error(ex);
                                    parafaitDBTrx.EndTransaction();
                                    throw;
                                }
                                catch (Exception ex)
                                {
                                    parafaitDBTrx.RollBack();
                                    log.Error(ex);
                                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                    throw;
                                }
                            }                
                        }   
                    }
                    else
                    {
                        string errorMessage = "messagingRequestDTOList is empty.";
                        log.LogMethodExit("Throwing Exception- " + errorMessage);
                        throw new ValidationException(errorMessage);
                    }
                }
                else
                {
                    string errorMessage = "Message not found.";
                    log.LogMethodExit("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        
}

        /// <summary>
        /// Returns the List of MessagingRequestSummaryViewDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of MessagingRequestSummaryViewDTO </returns>
        public async Task<List<MessagingRequestSummaryViewDTO>> GetMessagingRequestSummaryViewDTOList(int messageId = -1, int parentAndChildMessagesById = -1, string messageIdList = null, int customerId = -1, int cardId = -1,
                                                                    string messageType = null, DateTime? fromDate = null, DateTime? toDate = null, string parafaitFunctionName = null,
                                                                    int originalMessageId = -1, string toMobileList = null, string toEmailList = null, string trxNumber = null, string trxOTP = null, 
                                                                    string parafaitFunctionEventName = null, int pageNumber = 0, int numberOfRecords = -1,
                                                                    string trxNumberList = null, string trxOTPList = null)
        {
            return await Task<List<MessagingRequestSummaryViewDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(messageId, parentAndChildMessagesById, messageIdList, customerId, cardId, messageType, fromDate, toDate, parafaitFunctionName, originalMessageId, toMobileList, toEmailList, trxNumber, trxOTP, parafaitFunctionEventName, pageNumber, numberOfRecords, trxNumberList, trxOTPList);
                double businessDayStartTime = !String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BUSINESS_DAY_START_TIME")) ? ParafaitDefaultContainerList.GetParafaitDefault<double>(executionContext, "BUSINESS_DAY_START_TIME") : 6;
                MessagingRequestSummaryViewListBL messagingRequestViewListBL = new MessagingRequestSummaryViewListBL(executionContext);
                DateTime startDate = ServerDateTime.Now;
                DateTime endDate = ServerDateTime.Now.AddDays(1);
                List<KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>>();
                if (messageId != -1)
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.MESSAGE_ID, messageId.ToString()));
                }
                if (parentAndChildMessagesById != -1)
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.PARENT_AND_CHILD_MESSAGES_BY_ID, parentAndChildMessagesById.ToString()));
                }
                if (!string.IsNullOrWhiteSpace(messageIdList))
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.MESSAGE_ID_LIST, messageIdList));
                }
                if (customerId != -1)
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }
                if (cardId != -1)
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.CARD_ID, cardId.ToString()));
                }
                if (!String.IsNullOrEmpty(messageType))
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.MESSAGE_TYPE, MessagingRequestSummaryViewDTO.GetMessageType(messageType)));
                }

                if (fromDate != null)
                {
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = MessageContainerList.GetMessage(executionContext, 15);
                        log.Error(customException);
                        throw new ValidationException(customException);
                    }

                    if (toDate == null)
                        toDate = startDate.AddDays(1);
                }

                if (toDate != null)
                {
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = MessageContainerList.GetMessage(executionContext, 15);
                        log.Error(customException);
                        throw new ValidationException(customException);
                    }
                }

                if(startDate > endDate)
                {
                    string customException = MessageContainerList.GetMessage(executionContext, 2642);
                    log.LogMethodExit("Throwing Exception - " + customException);
                    throw new ValidationException(customException);
                }
                if (fromDate != null || toDate != null)
                {
                    startDate = SiteContainerList.ToSiteDateTime(executionContext.GetSiteId(), startDate);
                    endDate = SiteContainerList.ToSiteDateTime(executionContext.GetSiteId(), endDate);

                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.FROM_DATE, startDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.TO_DATE, endDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if(!string.IsNullOrWhiteSpace(parafaitFunctionName))
                {
                    List<KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>> parafaitFunctionsSearchParameters = new List<KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>>();
                    parafaitFunctionsSearchParameters.Add(new KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>(ParafaitFunctionsDTO.SearchByParameters.PARAFAIT_FUNCTION_NAME, parafaitFunctionName.ToUpper()));
                    ParafaitFunctionsListBL parafaitFunctionsListBL = new ParafaitFunctionsListBL(executionContext);
                    List<ParafaitFunctionsDTO> parafaitFunctionsDTOList = parafaitFunctionsListBL.GetAllParafaitFunctionsDTOList(parafaitFunctionsSearchParameters,true);
                    if(parafaitFunctionsDTOList != null && parafaitFunctionsDTOList.Any())
                    {
                        foreach (ParafaitFunctionsDTO parafaitFunctionsDTO in parafaitFunctionsDTOList)
                        {
                            if(parafaitFunctionsDTO.ParafaitFunctionEventDTOList != null && parafaitFunctionsDTO.ParafaitFunctionEventDTOList.Any())
                            {
                                string parafaitFunctionEventIdList = string.Join(",", parafaitFunctionsDTO.ParafaitFunctionEventDTOList.Select(x => x.ParafaitFunctionEventId).ToList());
                                searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID_LIST, parafaitFunctionEventIdList));
                            }
                            else
                            {
                                string errorMessage = "ParafaitFunctionEvent is null";
                                log.Error(errorMessage);
                                throw new ValidationException(errorMessage);
                            }
                        }
                    }
                    else
                    {
                        string errorMessage = "ParafaitFunctions is null";
                        log.Error(errorMessage);
                        throw new ValidationException(errorMessage);
                    }   
                }
                if (originalMessageId != -1)
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.ORIGINAL_MESSAGE_ID, originalMessageId.ToString()));
                }
                if(!string.IsNullOrWhiteSpace(toMobileList))
                {
                    toMobileList = Regex.Replace(toMobileList, @"\s", "");
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.TO_MOBILE_LIST, toMobileList));
                }
                if(!string.IsNullOrWhiteSpace(toEmailList))
                {
                    toEmailList = Regex.Replace(toEmailList, @"\s", "");
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.TO_EMAIL_LIST, toEmailList));
                }
                if (!string.IsNullOrWhiteSpace(trxNumber))
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.TRX_NUMBER, trxNumber));
                }
                if (!string.IsNullOrWhiteSpace(trxOTP))
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.TRX_OTP, trxOTP));
                }
                if(!string.IsNullOrWhiteSpace(parafaitFunctionEventName))
                {
                    List<KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>> parafaitFunctionEventSearchParameters = new List<KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>>();
                    parafaitFunctionEventSearchParameters.Add(new KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>(ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_NAME, parafaitFunctionEventName.ToUpper()));
                    ParafaitFunctionEventListBL parafaitFunctionEventListBL = new ParafaitFunctionEventListBL(executionContext);
                    List<ParafaitFunctionEventDTO> parafaitFunctionEventDTOList = parafaitFunctionEventListBL.GetAllParafaitFunctionEventDTOList(parafaitFunctionEventSearchParameters);
                    if (parafaitFunctionEventDTOList != null && parafaitFunctionEventDTOList.Any())
                    {
                        string parafaitFunctionEventIdList = string.Join(",", parafaitFunctionEventDTOList.Select(x => x.ParafaitFunctionEventId).ToList());
                        searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID_LIST, parafaitFunctionEventIdList));
                    }
                    else
                    {
                        string errorMessage = "ParafaitFunctionEvent is null";
                        log.Error(errorMessage);
                        throw new ValidationException(errorMessage);
                    }
                }
                if (!string.IsNullOrWhiteSpace(trxOTPList))
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.TRX_OTP_LIST, trxOTPList));
                }
                if (!string.IsNullOrWhiteSpace(trxNumberList))
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>(MessagingRequestSummaryViewDTO.SearchByParameters.TRX_NUMBER_LIST, trxNumberList));
                }
                List<MessagingRequestSummaryViewDTO> messagingRequestSummaryViewDTOList = messagingRequestViewListBL.GetAllMessagingRequestViewList(searchParameters, pageNumber, numberOfRecords);
                foreach(MessagingRequestSummaryViewDTO messagingRequestSummaryViewDTO in messagingRequestSummaryViewDTOList)
                {
                    if(messagingRequestSummaryViewDTO.SendDate != null && messagingRequestSummaryViewDTO.SendDate != DateTime.MinValue)
                    {
                        DateTime sentDate = Convert.ToDateTime(messagingRequestSummaryViewDTO.SendDate);
                        messagingRequestSummaryViewDTO.SendDate = startDate = SiteContainerList.FromSiteDateTime(executionContext.GetSiteId(), sentDate);
                    }
                    if (messagingRequestSummaryViewDTO.SendAttemptDate != null && messagingRequestSummaryViewDTO.SendAttemptDate != DateTime.MinValue)
                    {
                        DateTime sendAttemptDate = Convert.ToDateTime(messagingRequestSummaryViewDTO.SendAttemptDate);
                        messagingRequestSummaryViewDTO.SendAttemptDate = SiteContainerList.FromSiteDateTime(executionContext.GetSiteId(), sendAttemptDate);
                    }
                }
                log.LogMethodExit(messagingRequestSummaryViewDTOList);
                return messagingRequestSummaryViewDTOList;
            });
        }
    }
}
