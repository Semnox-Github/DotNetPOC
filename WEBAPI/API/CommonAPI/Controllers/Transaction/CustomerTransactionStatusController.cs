/********************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - Controller for getting the last transaction status for a customer 
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.150.2      08-Jan-2023   Nitin Pai            Base version
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class TransactionStatusDTO
    {
        public int TransactionId { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionDate { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentGatewayReason { get; set; }
        public string Message { get; set; }
    }
    public class CustomerTransactionStatusController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/CustomerTransactionStatus")]
        public async Task<HttpResponseMessage> Get(int trxId = -1, int customerId = -1, string guestPhoneNumber = null, string guestEmail = null)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(trxId, customerId, guestPhoneNumber, guestEmail);

                if (trxId == -1 && customerId == -1 && string.IsNullOrWhiteSpace(guestPhoneNumber) && string.IsNullOrWhiteSpace(guestEmail))
                {
                    log.LogMethodExit(null, "Bad Request");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                int offSetDuration = 0;
                TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                offSetDuration = timeZoneUtil.GetOffSetDuration(executionContext.GetSiteId(), DateTime.Now);

                List<TransactionStatusDTO> transactionStatusList = null;
                List<KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>>();
                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                DateTime searchStartTime = serverTimeObject.GetServerDateTime().AddMinutes(-30);
                DateTime searchEndTime = serverTimeObject.GetServerDateTime().AddMinutes(-1);

                log.Debug("Search by searchStartTime " + searchStartTime);
                log.Debug("Search by searchEndTime " + searchEndTime);
                searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.CREATED_FROM_DATE, searchStartTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.CREATED_TO_DATE, searchEndTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));

                if (trxId > -1)
                {
                    log.Debug("Search by trxId " + trxId);
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_ID, trxId.ToString()));
                }
                else if (customerId > -1)
                {
                    log.Debug("Search by customerId " + customerId);
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }
                else
                {
                    if(!string.IsNullOrWhiteSpace(guestPhoneNumber))
                    {
                        log.Debug("Search by guestPhoneNumber " + guestPhoneNumber);
                        searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.PHONE_NUMBER, guestPhoneNumber.ToString()));
                    }

                    if (!string.IsNullOrWhiteSpace(guestEmail))
                    {
                        log.Debug("Search by guestEmail " + guestEmail);
                        searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.EMAIL_ID, guestEmail.ToString()));
                    }
                }

                TransactionSummaryViewListBL transactionSummaryViewListBL = new TransactionSummaryViewListBL(executionContext);
                IList<TransactionSummaryViewDTO> transactionDTOList = null;
                transactionDTOList = await Task<List<TransactionSummaryViewDTO>>.Factory.StartNew(() =>
                {
                    return transactionSummaryViewListBL.GetTransactionSummaryViewDTOList(searchParameters, 0, 10);
                });

                log.Debug("Finished searching transactions");
                if(transactionDTOList != null && transactionDTOList.Any())
                {
                    log.Debug("Got transactions count " + transactionDTOList.Count);
                    transactionStatusList = new List<TransactionStatusDTO>();
                    transactionDTOList = transactionDTOList.OrderByDescending(x => x.CreationDate).ToList();
                    //foreach(TransactionSummaryViewDTO trxDTO in transactionDTOList)
                    //{
                    //    log.Debug("Got transactions " + trxDTO.TransactionId + ":" + trxDTO.CreationDate);
                    //}
                    TransactionSummaryViewDTO transactionDTO = transactionDTOList[0];
                    {
                        TransactionStatusDTO transactionStatusDTO = new TransactionStatusDTO();
                        DateTime trxDate = transactionDTO.CreationDate.AddSeconds(-1*offSetDuration);
                        log.Debug("Transaction creation date " + trxDate);
                        String creationDate = trxDate.ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"));
                        String message = "";
                        transactionStatusDTO.TransactionId = transactionDTO.TransactionId;
                        transactionStatusDTO.TransactionStatus = transactionDTO.Status;
                        transactionStatusDTO.TransactionDate = creationDate;

                        log.Debug("Processing trx id " + transactionDTO.TransactionId + ":" + transactionDTO.Status + ":" + creationDate);

                        if ((transactionDTO.Status != "CLOSED" && transactionDTO.Status != "CANCELLED"))
                        {
                            log.Debug("Transaction is not in closed or cancelled state " + transactionDTO.Status);
                            CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                            List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGWTemp = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                            searchParametersPGWTemp.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, transactionDTO.TransactionId.ToString()));
                            CCRequestPGWDTO cCRequestPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGWTemp);

                            if (cCRequestPGWDTO != null)
                            {
                                log.Debug("Got CCRequestDTO " + cCRequestPGWDTO.RequestID + ":" + cCRequestPGWDTO.InvoiceNo + ":" + cCRequestPGWDTO.PaymentProcessStatus + ":" + cCRequestPGWDTO.RequestDatetime);
                            }

                            if (cCRequestPGWDTO == null)
                            {
                                message = MessageContainerList.GetMessage(executionContext, 5038);
                                transactionStatusDTO.PaymentStatus = "NONE";
                                log.Debug("No CC Request found " + message);
                            }
                            else if (cCRequestPGWDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_INITIATED.ToString())
                            {
                                message = MessageContainerList.GetMessage(executionContext, 5039);
                                transactionStatusDTO.PaymentStatus = "NOT INTIATED";
                                log.Debug("Payment in initiated state " + message);
                            }
                            else if (cCRequestPGWDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_PROCESSING.ToString())
                            {
                                message = MessageContainerList.GetMessage(executionContext, 5040);
                                transactionStatusDTO.PaymentStatus = "IN_PROGRESS";
                                log.Debug("Payment in processing state " + message);
                            }
                            else if (cCRequestPGWDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_COMPLETED.ToString())
                            {
                                message = MessageContainerList.GetMessage(executionContext, 5041);
                                transactionStatusDTO.PaymentStatus = "COMPLETED";
                                log.Debug("Payment is in completed state " + message);
                            }
                            else
                            {
                                log.Debug("cCRequestPGWDTO.PaymentProcessStatus " + cCRequestPGWDTO.PaymentProcessStatus);
                                message = MessageContainerList.GetMessage(executionContext, 5042);
                                transactionStatusDTO.PaymentStatus = "FAILED";
                                log.Debug("Payment has failed " + message);

                                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParametersTemp = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                                searchParametersTemp.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, cCRequestPGWDTO.RequestID.ToString()));
                                CCTransactionsPGWListBL cCTransactionsPGWListBLTemp = new CCTransactionsPGWListBL();
                                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBLTemp.GetCCTransactionsPGWDTOList(searchParametersTemp);
                                if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Any())
                                {
                                    List<CCTransactionsPGWDTO> rejectedList = cCTransactionsPGWDTOList.Where(x => !String.IsNullOrWhiteSpace(x.TextResponse) && x.TextResponse.ToUpper() != "APPROVAL").ToList();
                                    if (rejectedList != null && rejectedList.Any())
                                    {
                                        message = message.Replace("@reason", rejectedList[0].TextResponse);
                                        transactionStatusDTO.PaymentGatewayReason = rejectedList[0].TextResponse;
                                        log.Debug("Payment rejected " + message);
                                    }
                                }
                            }
                        }
                        else if (transactionDTO.Status == "CANCELLED")
                        {
                            log.Debug("Transaction is in cancelled state " + transactionDTO.TransactionId + ":" + transactionDTO.Status);
                            CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                            List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGWTemp = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                            searchParametersPGWTemp.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, transactionDTO.TransactionId.ToString()));
                            CCRequestPGWDTO cCRequestPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGWTemp);
                            if (cCRequestPGWDTO == null)
                            {
                                log.Debug("No CC Request DTO found ");
                                message = MessageContainerList.GetMessage(executionContext, 5038);
                                transactionStatusDTO.PaymentStatus = "NONE";
                                log.Debug("No CC Request found " + message);
                            }
                            else if (cCRequestPGWDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_COMPLETED.ToString())
                            {
                                log.Debug("Payment is in completed state " + cCRequestPGWDTO.PaymentProcessStatus);
                                message = MessageContainerList.GetMessage(executionContext, 5073);
                                transactionStatusDTO.PaymentStatus = "REFUND INITIATED";
                                log.Debug("Payment is in completed state but transaction is in cancelled state. A refund will be initiated. " + message);
                            }
                            else
                            {
                                log.Debug("CC Request status " + cCRequestPGWDTO.RequestID + ":" + cCRequestPGWDTO.PaymentProcessStatus);
                                message = MessageContainerList.GetMessage(executionContext, 5042);
                                transactionStatusDTO.PaymentStatus = "FAILED";
                                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParametersTemp = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                                searchParametersTemp.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, cCRequestPGWDTO.RequestID.ToString()));
                                CCTransactionsPGWListBL cCTransactionsPGWListBLTemp = new CCTransactionsPGWListBL();
                                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBLTemp.GetCCTransactionsPGWDTOList(searchParametersTemp);
                                if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Any())
                                {
                                    log.Debug("Got CC Trnsactions");
                                    List<CCTransactionsPGWDTO> rejectedList = cCTransactionsPGWDTOList.Where(x => !String.IsNullOrWhiteSpace(x.TextResponse) && x.TextResponse.ToUpper() != "APPROVAL").ToList();
                                    if (rejectedList != null && rejectedList.Any())
                                    {
                                        log.Debug("Got rejected cc transactions " + rejectedList[0].ResponseID + ":" + rejectedList[0]);
                                        foreach (CCTransactionsPGWDTO tempDTO in rejectedList)
                                            log.Debug(tempDTO.ResponseID +":" + tempDTO);

                                        message = message.Replace("@reason", rejectedList[0].TextResponse);
                                        transactionStatusDTO.PaymentGatewayReason = rejectedList[0].TextResponse;
                                        log.Debug("Payment rejected " + message);
                                    }
                                }
                            }
                        }
                        else if(transactionDTO.Status == "CLOSED")
                        {
                            log.Debug("Transaction is in closed state " + transactionDTO.Status);
                            message = MessageContainerList.GetMessage(executionContext, 5041);
                            log.Debug("Transaction is closed " + message);
                        }

                        message = message.Replace("@trxId", transactionDTO.TransactionId.ToString());
                        message = message.Replace("@trxDate", creationDate);
                        log.Debug("Message " + message);
                        transactionStatusDTO.Message = message;
                        transactionStatusList.Add(transactionStatusDTO);
                    }
                }
                else
                {
                    log.Debug("No transactions were found");
                }

                log.LogMethodExit(transactionStatusList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = transactionStatusList });
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