/********************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - Controller for receiving response from payment gateways 
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.150.2      08-Jan-2023   Nitin Pai            Base version
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Site;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.ViewContainer;
using static Semnox.Parafait.Transaction.Transaction;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class HostedPaymentCallbackController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // payment was handled but it was not successful, return ok
        private String failureMessage = "Your payment for order (reference @TrxId) could not be processed. Reason: @ReturnCode @Reason";
        private String successMessage = "Your payment for order (reference @TrxId) was processed successfully. <br>Card Type: @CardType <br>Card Number: @CardNumber <br>Transaction Time: @PaymentTime";
        private String defaultRedirectURL = "";

        private async Task<string> GetResponseBody(HttpRequestMessage request)
        {
            log.LogMethodEntry(request);
            string requestBody = "";
            try
            {
                log.Debug("Request URL " + request.RequestUri.ToString());
                if (request.Method == HttpMethod.Post)
                {
                    log.Debug("Request.Content " + request.Content);
                    log.Debug("Request.Content String " + request.Content.ReadAsStringAsync().Result);
                    Stream receiveStream = await request.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader(HttpContext.Current.Request.InputStream);
                    readStream.BaseStream.Position = 0;
                    requestBody = readStream.ReadToEnd();
                    log.Debug("Request Body " + requestBody);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(requestBody);
            return requestBody;
        }

        private void SetExecutionContext(ExecutionContext executionContext, Utilities utility, int siteId, bool forceCorporate = false)
        {
            log.LogMethodEntry(executionContext, utility, siteId, forceCorporate);

            executionContext.SetSiteId(-1);
            executionContext.SetUserId("");
            executionContext.SetToken("");
            utility.ParafaitEnv.User_Id = -1;
            utility.ParafaitEnv.SiteId = -1;
            if (SiteContainerList.IsCorporate())
            {
                utility.ExecutionContext.IsCorporate = true;
                utility.ExecutionContext.SiteId = siteId;
                utility.ParafaitEnv.IsCorporate = true;
                utility.ParafaitEnv.SiteId = siteId;
            }

            utility.ParafaitEnv.Initialize();
            Semnox.Parafait.User.Users users = new Semnox.Parafait.User.Users(executionContext, "External POS", utility.ParafaitEnv.SiteId);
            utility.ParafaitEnv.User_Id = users.UserDTO.UserId;
            utility.ParafaitEnv.ExternalPOSUserId = utility.ParafaitEnv.User_Id;
            utility.ParafaitEnv.LoginID = users.UserDTO.LoginId;
            utility.ExecutionContext.UserId = users.UserDTO.LoginId;
            utility.ExecutionContext.UserPKId = users.UserDTO.UserId;

            List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.COMPUTER_NAME, Environment.MachineName));
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, utility.ExecutionContext.GetSiteId().ToString()));

            POSMachineList pOSMachinesList = new POSMachineList(executionContext);
            List<POSMachineDTO> pOSMachineDTOList = pOSMachinesList.GetAllPOSMachines(searchParameters);
            if (pOSMachineDTOList != null && pOSMachineDTOList.Any())
            {
                pOSMachineDTOList = pOSMachineDTOList.Where(x => x.ComputerName == Environment.MachineName).ToList();
            }
            if (pOSMachineDTOList != null && pOSMachineDTOList.Any())
            {
                utility.ParafaitEnv.POSMachine = pOSMachineDTOList[0].ComputerName;
                utility.ParafaitEnv.POSMachineId = pOSMachineDTOList[0].POSMachineId;
                utility.ParafaitEnv.POSTypeId = pOSMachineDTOList[0].POSTypeId;
                utility.ExecutionContext.POSMachineName = pOSMachineDTOList[0].ComputerName;
            }
            else
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1743, Environment.MachineName));//use 1743 messsage
            }

            executionContext.SetSiteId(utility.ExecutionContext.GetSiteId());
            executionContext.SetUserId(users.UserDTO.LoginId);
            if (utility.ExecutionContext.GetIsCorporate())
            {
                executionContext.SetIsCorporate(true);
            }
            else
            {
                utility.ParafaitEnv.SiteId = -1;
            }

            if (forceCorporate)
            {
                utility.ParafaitEnv.IsCorporate = true;
            }

            log.Debug("Execution Context " + executionContext);
            log.Debug("Utility Execution Context " + utility.ExecutionContext);

            String temp = MessageContainerList.GetMessage(executionContext, 5112);
            if (!string.IsNullOrWhiteSpace(temp))
            {
                successMessage = temp;
                log.Debug("Success Message " + successMessage);
            }
            temp = MessageContainerList.GetMessage(executionContext, 5113);
            if (!string.IsNullOrWhiteSpace(temp))
            {
                failureMessage = temp;
                log.Debug("Failure Message " + failureMessage);
            }

            LookupValuesList lookupValuesList = new LookupValuesList(utility.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lksearchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lksearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
            lksearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utility.ExecutionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(lksearchParameters);
            if(lookupValuesDTOlist != null && lookupValuesDTOlist.Any())
            {
                if (lookupValuesDTOlist.Where(x => x.LookupValue == "DEFAULT_REDIRECT_URL").Count() == 1)
                {
                    defaultRedirectURL = lookupValuesDTOlist.Where(x => x.LookupValue == "DEFAULT_REDIRECT_URL").First().Description;
                    log.Debug("defaultRedirectURL " + defaultRedirectURL);
                }
            }

            log.LogMethodExit();

        }

        private string BuildMessage(String template, HostedGatewayDTO hostedGatewayDTO)
        {
            log.LogMethodEntry(template, hostedGatewayDTO);
            log.Debug(hostedGatewayDTO.SiteId + ":" + hostedGatewayDTO.TrxId + ":" + hostedGatewayDTO.PaymentStatusMessage + ":" + hostedGatewayDTO.CCTransactionsPGWDTO.TransactionDatetime + ":" + hostedGatewayDTO.CCTransactionsPGWDTO.DSIXReturnCode + ":" + hostedGatewayDTO.CCTransactionsPGWDTO.TextResponse);

            String message = template;
            message = message.Replace("@TrxId", hostedGatewayDTO.TrxId.ToString());
            message = message.Replace("@TransactionId", hostedGatewayDTO.TrxId.ToString());
            message = message.Replace("@TrxNo", hostedGatewayDTO.TrxId.ToString());

            if(!String.IsNullOrEmpty(hostedGatewayDTO.PaymentStatusMessage))
            {
                log.Debug("hostedGatewayDTO.PaymentStatusMessage is not empty");
                message = message.Replace("@Reason", hostedGatewayDTO.PaymentStatusMessage);
            }

            if(hostedGatewayDTO.CCTransactionsPGWDTO != null)
            {
                log.Debug("Building message from CCTransaction");
                if (message.Contains("@PaymentTime"))
                {
                    TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                    int offSetDuration = 0;
                    DateTime paymentDate = hostedGatewayDTO.CCTransactionsPGWDTO.TransactionDatetime;
                    log.Debug("Before offset calculation " + paymentDate);
                    offSetDuration = timeZoneUtil.GetOffSetDuration(hostedGatewayDTO.SiteId, paymentDate);
                    paymentDate = paymentDate.AddSeconds(-1*offSetDuration);
                    log.Debug("After offset calculation " + paymentDate);
                    String dateTimeFormat = "dd-MMM-yyyy h:mm tt";
                    String tempFormat = ParafaitDefaultContainerList.GetParafaitDefault(new ExecutionContext("External POS", SiteContainerList.GetMasterSiteId(), -1, -1, SiteContainerList.IsCorporate(), -1), "DATETIME_FORMAT");
                    if (!String.IsNullOrEmpty(tempFormat))
                    {
                        dateTimeFormat = tempFormat;
                    }
                    message = message.Replace("@PaymentTime", paymentDate.ToString(dateTimeFormat));
                }

                message = message.Replace("@CardType", hostedGatewayDTO.CCTransactionsPGWDTO.CardType);
                message = message.Replace("@CardNumber", hostedGatewayDTO.CCTransactionsPGWDTO.AcctNo);
                message = message.Replace("@ReturnCode", hostedGatewayDTO.CCTransactionsPGWDTO.DSIXReturnCode);
                message = message.Replace("@Reason", hostedGatewayDTO.CCTransactionsPGWDTO.TextResponse);
            }
            else if (hostedGatewayDTO.TransactionPaymentsDTO != null &&
                !string.IsNullOrWhiteSpace(hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber))
            {
                log.Debug("Building message from TrxPayments");
                if (message.Contains("@PaymentTime"))
                {
                    TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                    int offSetDuration = 0;
                    DateTime paymentDate = hostedGatewayDTO.TransactionPaymentsDTO.PaymentDate;
                    offSetDuration = timeZoneUtil.GetOffSetDuration(-1*hostedGatewayDTO.SiteId, paymentDate);
                    paymentDate = paymentDate.AddSeconds(offSetDuration);
                    String dateTimeFormat = "dd-MMM-yyyy h:mm tt";
                    String tempFormat = ParafaitDefaultContainerList.GetParafaitDefault(new ExecutionContext("External POS", SiteContainerList.GetMasterSiteId(), -1, -1, SiteContainerList.IsCorporate(), -1), "DATETIME_FORMAT");
                    if (!String.IsNullOrEmpty(tempFormat))
                    {
                        dateTimeFormat = tempFormat;
                    }
                    message = message.Replace("@PaymentTime", paymentDate.ToString(dateTimeFormat));
                }

                message = message.Replace("@CardType", hostedGatewayDTO.TransactionPaymentsDTO.CreditCardName);
                message = message.Replace("@CardNumber", hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber);
                message = message.Replace("@ReturnCode", "");
                message = message.Replace("@Reason", "");
            }
            else
            {
                log.Debug("Going into default state");
                message = message.Replace("@PaymentTime", "");
                message = message.Replace("@CardType", "");
                message = message.Replace("@CardNumber", "");
                message = message.Replace("@ReturnCode", "");
                message = message.Replace("@Reason", "");
            }
            log.LogMethodExit(message);
            return message;
        }

        public virtual HostedGatewayDTO InitiatePaymentProcessing(ExecutionContext executionContext, HostedPaymentGateway paymentGatewayObj, HostedGatewayDTO hostedGatewayDTO)
        {
            log.LogMethodEntry(hostedGatewayDTO);
            log.Debug("Initiating payment gateway response processing");

            CCRequestPGWDTO cCRequestPGWDTO = null;
            int trxId = hostedGatewayDTO.TrxId;
            if (trxId == -1)
            {
                log.Error("Transaction Id not found ");
                throw new Exception("Transaction Id not found.");
            }

            // Step 1 - Get the CC request
            log.Debug("Getting the latest CC request");
            // First check if the payment has been completed
            CCRequestPGWListBL cCRequestPGWListBLTemp = new CCRequestPGWListBL();
            List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGWTemp = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
            searchParametersPGWTemp.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId.ToString()));
            cCRequestPGWDTO = cCRequestPGWListBLTemp.GetLastestCCRequestPGWDTO(searchParametersPGWTemp);
            if (cCRequestPGWDTO == null)
            {
                log.Error("No entry found in CCRequest ");
                throw new Exception("Payment has been rejected.");
            }

            log.Debug(cCRequestPGWDTO.RequestID + ":" + cCRequestPGWDTO.PaymentProcessStatus);
            // Start assigning values to hosted payment DTO
            hostedGatewayDTO.SiteId = cCRequestPGWDTO.SiteId;
            hostedGatewayDTO.TrxId = trxId;

            bool checkForParallelThreads = false;
            //bool paymentProcessingCompleted = false;
            // Step 2 - If CC Request is in Payment_Initiated Status, it indicates first response (between response and call back, update to payment processing
            if (cCRequestPGWDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_INITIATED.ToString())
            {
                try
                {
                    log.Debug("Trying to update the CC request to payment processing status");
                    hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_PROCESSING;
                    //CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(executionContext, cCRequestPGWDTO);
                    //int rowsUpdated = cCRequestPGWBL.ChangePaymentProcessingStatus(PaymentProcessStatusType.PAYMENT_INITIATED.ToString(), PaymentProcessStatusType.PAYMENT_PROCESSING.ToString());
                    if (!paymentGatewayObj.UpdatePaymentProcessingStatus(hostedGatewayDTO))
                    {
                        log.Debug("CC request could not be updated, indicates that a parallel thread might be processing this");
                        checkForParallelThreads = true;
                    }
                    else
                    {
                        log.Debug("CC request updated to payment processing status");
                    }
                }
                catch (Exception ex)
                {
                    log.Debug("Got an error while updated the CC request status. Assume that a parallel thread is in progress and continue " + ex);
                    checkForParallelThreads = true;
                }
            }
            else if (cCRequestPGWDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_PROCESSING.ToString())
            {
                log.Debug("Payment is already under processing, check for parallel threads");
                checkForParallelThreads = true;
            }

            // Step 3 - If a parallel thread is in progress, poll CCRequest for status
            if (checkForParallelThreads)
            {
                log.Debug("A parallel thread is in progress. Wait for some time to validate the status");
                int numberOfAttempts = 10;
                do
                {
                    if(numberOfAttempts < 9)
                        System.Threading.Thread.Sleep(1000); // Make this as a configuration
                    else
                        System.Threading.Thread.Sleep(5000); // Make this as a configuration

                    log.Debug("Checking CCRequest Status");
                    CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(executionContext, cCRequestPGWDTO.RequestID);
                    cCRequestPGWDTO = cCRequestPGWBL.CCRequestPGWDTO;
                    if (cCRequestPGWDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_CANCELLED.ToString() ||
                        cCRequestPGWDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_FAILED.ToString() ||
                        cCRequestPGWDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_COMPLETED.ToString())
                    {
                        log.Debug("payment processing has completed" + cCRequestPGWDTO.PaymentProcessStatus);
                        numberOfAttempts = 10;
                    }
                    else
                    {
                        numberOfAttempts++;
                    }
                }
                while (numberOfAttempts < 10);
            }

            // Step 4 - Check if the payment processing is completed.
            if (cCRequestPGWDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_CANCELLED.ToString())
            {
                log.Debug("Payment is in cancelled state");
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.CANCELLED;
                hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_CANCELLED;
            }
            else if (cCRequestPGWDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_FAILED.ToString())
            {
                log.Debug("Payment is in failed state");
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
            }
            else if (cCRequestPGWDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_COMPLETED.ToString())
            {
                log.Debug("Payment is in completed state");
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_COMPLETED;
            }
            else
            {
                log.Debug("Payment needs to be processed.");
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.NONE;
                hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_PROCESSING;
            }

            if (hostedGatewayDTO.TransactionPaymentsDTO == null)
                hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            hostedGatewayDTO.TransactionPaymentsDTO.Reference = trxId.ToString();

            // If the payments has been processed, build the CCTransactionList
            if(hostedGatewayDTO.PaymentStatus == PaymentStatusType.CANCELLED
                || hostedGatewayDTO.PaymentStatus == PaymentStatusType.FAILED
                || hostedGatewayDTO.PaymentStatus == PaymentStatusType.SUCCESS)
            {
                log.Debug("Building CC Transaction List");
                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, cCRequestPGWDTO.RequestID.ToString()));
                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                {
                    log.Debug("Got CC Transaction List. Count " + cCTransactionsPGWDTOList.Count);
                    hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTOList.OrderByDescending(x => x.CreationDate).First();
                    hostedGatewayDTO.PaymentStatusMessage = String.IsNullOrWhiteSpace(hostedGatewayDTO.CCTransactionsPGWDTO.TextResponse)? "": hostedGatewayDTO.CCTransactionsPGWDTO.TextResponse;
                }
            }

            log.LogMethodExit(hostedGatewayDTO);
            return hostedGatewayDTO;
        }

        private HostedGatewayDTO ProcessPayment(String paymentGateway, string gatewayResponse, String method)
        {
            log.LogMethodEntry(paymentGateway, gatewayResponse, method);

            HostedGatewayDTO hostedGatewayDTO = null;
            HostedPaymentGateway paymentGatewayObj = null;
            ExecutionContext executionContext = new ExecutionContext("External POS", SiteContainerList.GetMasterSiteId(), -1, -1, SiteContainerList.IsCorporate(), -1);
            int siteId = SiteContainerList.GetMasterSiteId();
            executionContext.SetIsCorporate(SiteContainerList.IsCorporate());
            executionContext.SetSiteId(siteId);
            String message = "";
            PaymentProcessStatusType initialPaymentProcessType = PaymentProcessStatusType.PAYMENT_PROCESSING;
            using (Utilities utilities = new Utilities(new ParafaitDBTransaction().GetConnection()))
            {
                using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                {
                    try
                    {
                        log.Debug("Initiating payment gateway response processing");
                        parafaitDBTransaction.BeginTransaction();

                        SetExecutionContext(executionContext, utilities, siteId);
                        log.Debug("Initiate payment with " + utilities.ExecutionContext.ToString());

                        PaymentGatewayFactory.GetInstance().Initialize(utilities, true, null);
                        paymentGatewayObj = (HostedPaymentGateway)PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentGateway);
                        if (paymentGatewayObj != null)
                        {
                            log.Debug("Payment gateway identified. Calling function to check payment status and process gateway response. TrxId ");
                            hostedGatewayDTO = paymentGatewayObj.InitiatePaymentProcessing(gatewayResponse);
                            String tempMessage = method + " Initiate processing for " + hostedGatewayDTO.TrxId + " and PG Reference " + hostedGatewayDTO.GatewayReferenceNumber;
                            log.Debug("temp message");
                            utilities.EventLog.logEvent(paymentGateway, 'I', tempMessage, "Web Response", "Web Payments", 1, "", "Transaction: " + hostedGatewayDTO.TrxId, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

                            hostedGatewayDTO = InitiatePaymentProcessing(utilities.ExecutionContext, paymentGatewayObj, hostedGatewayDTO);
                            log.Debug("Completed initial processing for " + hostedGatewayDTO.TrxId + " and PG Reference " + hostedGatewayDTO.GatewayReferenceNumber);

                            // If the check process failed or if the CC request is in payment processing status, process the response
                            if (hostedGatewayDTO == null || hostedGatewayDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_PROCESSING)
                            {
                                log.Debug("The payment needs to be processed, it has not been picked by a different thread");
                                initialPaymentProcessType = hostedGatewayDTO.PaymentProcessStatus;
                                // check if the context has to be changed
                                if (hostedGatewayDTO != null && hostedGatewayDTO.SiteId != siteId)
                                {
                                    log.Debug("Reset the context to" + hostedGatewayDTO.SiteId);
                                    SetExecutionContext(utilities.ExecutionContext, utilities, hostedGatewayDTO.SiteId);
                                }
                                // Do this as the execution context is reset
                                log.Debug("Calling payment gateway process response");
                                PaymentGatewayFactory.GetInstance().Initialize(utilities, true, null);
                                paymentGatewayObj = (HostedPaymentGateway)PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentGateway);
                                hostedGatewayDTO = paymentGatewayObj.ProcessGatewayResponse(gatewayResponse);
                                log.Debug("Payment gateway response has been processed");
                            }
                            else if(hostedGatewayDTO != null && hostedGatewayDTO.SiteId != siteId)
                            {
                                log.Debug("Reset the context to" + hostedGatewayDTO.SiteId);
                                initialPaymentProcessType = hostedGatewayDTO.PaymentProcessStatus;
                                SetExecutionContext(utilities.ExecutionContext, utilities, hostedGatewayDTO.SiteId);
                            }

                            log.Debug("initialPaymentProcessType " + initialPaymentProcessType.ToString());

                            // The gateway response has been processed.
                            if (hostedGatewayDTO == null)
                            {
                                message = "Payment gateway response processing failed.";
                                log.Debug(message);
                                utilities.EventLog.logEvent(paymentGateway, 'E', message, "Processing failed", "Web Payments", 1, "", "Transaction: ", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, message));
                            }

                            if(hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeDTO == null)
                            {
                                log.Debug("Get the payment mode dto");
                                int gatewayLookUpId = -1;
                                LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                                searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                                searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, paymentGateway));
                                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParameters);
                                if (lookupValuesDTOList != null &&
                                    lookupValuesDTOList.Any())
                                {
                                    gatewayLookUpId = lookupValuesDTOList[0].LookupValueId;
                                    log.Debug("gatewayLookUpId" + gatewayLookUpId);
                                }
                                else
                                {
                                    log.Debug("gatewayLookUpId not found " + searchParameters.ToString());
                                    log.Debug("gatewayLookUpId not found");
                                }

                                PaymentModeList paymentModesListBL = new PaymentModeList(utilities.ExecutionContext);
                                List<PaymentModeDTO> paymentModesDTOList = paymentModesListBL.GetPaymentModesWithPaymentGateway(false);
                                PaymentModeDTO paymentModeDTO = paymentModesDTOList.FirstOrDefault(x => x.Gateway.Equals(gatewayLookUpId));

                                if (paymentModeDTO == null)
                                {
                                    throw new ValidationException("Payment Gateway Not found");
                                }

                                log.Debug("Assigning the payment mode dto " + paymentModeDTO);
                                hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeDTO = paymentModeDTO;
                            }

                            log.Debug("Payment gateway response. Post processing begins. Payment status is " + hostedGatewayDTO.PaymentStatus);
                            if (hostedGatewayDTO.CCTransactionsPGWDTO != null && hostedGatewayDTO.CCTransactionsPGWDTO.IsChanged)
                            {
                                log.Debug("Saving CCTransaction");
                                CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(hostedGatewayDTO.CCTransactionsPGWDTO, utilities.ExecutionContext);
                                cCTransactionsPGWBL.Save(parafaitDBTransaction.SQLTrx);
                                hostedGatewayDTO.TransactionPaymentsDTO.CCResponseId = cCTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                log.Debug("Created CCTransaction " + cCTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID);
                            }

                            if (hostedGatewayDTO.TrxId <= 0)
                                hostedGatewayDTO.TrxId = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId;

                            log.Debug("Build Transaction Object " + hostedGatewayDTO.TrxId);
                            TransactionUtils trxUtils = new TransactionUtils(utilities);
                            Semnox.Parafait.Transaction.Transaction newTrx = trxUtils.CreateTransactionFromDB(hostedGatewayDTO.TrxId, utilities, sqlTrx: parafaitDBTransaction.SQLTrx);
                            if (newTrx == null)
                            {
                                throw new ValidationException("Transaction Not Found");
                            }

                            // assign the trxGuid
                            hostedGatewayDTO.TrxGuid = newTrx.TrxGuid;
                            hostedGatewayDTO.SiteId = utilities.ExecutionContext.GetSiteId();
                            log.Debug("Trx after build " + newTrx.TrxGuid + ":" + newTrx.Status);

                            if (String.IsNullOrEmpty(newTrx.TrxGuid))
                            {
                                log.Debug("GUID will be empty for cancelled transactions. Need to get this separately");
                                try
                                {
                                    DataTable dt = utilities.executeDataTable(@"select status, trxid, guid from trx_header where TrxId = @trx_id ", new SqlParameter("@trx_id", hostedGatewayDTO.TrxId));
                                    if (dt != null)
                                    {
                                        DataRow dr = dt.Rows[0];
                                        string status = dr["status"].ToString().ToUpper();
                                        newTrx.Status = (Semnox.Parafait.Transaction.Transaction.TrxStatus)Enum.Parse(typeof(Semnox.Parafait.Transaction.Transaction.TrxStatus), dr["status"].ToString().ToUpper(), true);
                                        hostedGatewayDTO.TrxGuid = newTrx.TrxGuid = dr["guid"].ToString().ToUpper();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Debug("Got an error while getting trx guid from db " + ex.Message);
                                }
                                log.Debug("Trx after building again from DB " + newTrx.TrxGuid + ":" + newTrx.Status);
                            }

                            bool exceptionFlagDuplicatePayment = false;
                            bool exceptionFlagParallelProcess = false;
                            // Check for exception scenarios here
                            // 1. Check if transaction is already in a closed status. If yes, determine what type of payment this is
                            if (newTrx.Status == TrxStatus.CLOSED || newTrx.Status == TrxStatus.CANCELLED)
                            {
                                log.Debug("Transaction is in closed or cancelled status. Checking if this is a duplicate or new payment");
                                if (newTrx.TransactionPaymentsDTOList != null && newTrx.TransactionPaymentsDTOList.Any())
                                {
                                    log.Debug("Payment found for transaction. Check status.");
                                    if (newTrx.TransactionPaymentsDTOList.FirstOrDefault(x => x.Reference ==
                                        hostedGatewayDTO.GatewayReferenceNumber &&
                                        !String.IsNullOrWhiteSpace(hostedGatewayDTO.GatewayReferenceNumber)) != null)
                                    {
                                        log.Debug("Payment with same reference number is found. This is a duplication callback or response call");
                                        log.Debug("Mark Parallel process as true.");
                                        exceptionFlagParallelProcess = true;
                                    }
                                    else if (newTrx.TransactionPaymentsDTOList.FirstOrDefault(x => x.Reference !=
                                        hostedGatewayDTO.GatewayReferenceNumber &&
                                        !String.IsNullOrWhiteSpace(hostedGatewayDTO.GatewayReferenceNumber)) != null)
                                    {
                                        log.Debug("A duplicate payment is found for this transaction. This needs to be checked and refunded");
                                        log.Debug("Mark Processing is completed.");
                                        exceptionFlagDuplicatePayment = true;
                                    }
                                }
                                else
                                {
                                    // A transaction in a closed state with no payments is an exception. The payment cannot be handeled.
                                    log.Debug("This payment cannot be accepted as the transaction is already in a closed state");
                                    log.Debug("Set refund flag");
                                    exceptionFlagDuplicatePayment = true;
                                }
                            }
                            else
                            {
                                if (initialPaymentProcessType == PaymentProcessStatusType.PAYMENT_COMPLETED)
                                {
                                    log.Debug("The payment is marked as completed but the trx is still open. This is a race condition.");
                                    exceptionFlagParallelProcess = true;
                                }
                            }

                            log.Debug("Duplicate and Parallel Check Flag Status " + exceptionFlagDuplicatePayment + ":" + exceptionFlagDuplicatePayment);

                            if (hostedGatewayDTO.PaymentStatus == PaymentStatusType.SUCCESS)
                            {
                                String tempMessage1 = method + " Successfully processed Payment " + hostedGatewayDTO.TrxId + " and PG Reference " + hostedGatewayDTO.GatewayReferenceNumber;
                                log.Debug(tempMessage1);
                                utilities.EventLog.logEvent(paymentGateway, 'I', tempMessage1, "Web Response", "Web Payments", 1, "", "Transaction: " + hostedGatewayDTO.TrxId, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

                                log.Debug("Process further for successful payment Duplicate Flag: " +
                                    exceptionFlagDuplicatePayment + " Parallel Process flag:" + exceptionFlagParallelProcess);

                                if (!exceptionFlagDuplicatePayment)
                                {
                                    log.Debug("This is not a duplicate payment. Continue processing.");
                                    if (exceptionFlagParallelProcess)
                                    {
                                        log.Debug("Parallel check has been flagged. Check if transaction is already processed.");
                                        if (newTrx.Status != TrxStatus.CLOSED && newTrx.Status != TrxStatus.CANCELLED)
                                        {
                                            log.Debug("A parallel thread may be processeing the transaction. Poll the transaction to check the status");
                                            int numberOfAttempts = 0;
                                            do
                                            {
                                                log.Debug("Wait for trx processing attempt " + numberOfAttempts);
                                                if(numberOfAttempts < 9)
                                                    System.Threading.Thread.Sleep(2000);
                                                else
                                                    System.Threading.Thread.Sleep(10000);

                                                string status = "";
                                                try
                                                {
                                                    DataTable dt = utilities.executeDataTable(@"select status, trxid, guid from trx_header where TrxId = @trx_id ", new SqlParameter("@trx_id", hostedGatewayDTO.TrxId));
                                                    if (dt != null)
                                                    {
                                                        DataRow dr = dt.Rows[0];
                                                        status = dr["status"].ToString().ToUpper();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    log.Debug("Got an error while getting trx guid from db " + ex.Message);
                                                }

                                                log.Debug("Trx Status in loop " + newTrx.Status);
                                                if (status == TrxStatus.CLOSED.ToString() || status == TrxStatus.CANCELLED.ToString())
                                                {
                                                    log.Debug("Transaction is in closed or cancelled state");
                                                    numberOfAttempts = 10;
                                                }
                                                else
                                                {
                                                    log.Debug("Incrementing the count");
                                                    numberOfAttempts++;
                                                }
                                            }
                                            while (numberOfAttempts < 10);
                                        }

                                        newTrx = trxUtils.CreateTransactionFromDB(hostedGatewayDTO.TrxId, utilities, sqlTrx: parafaitDBTransaction.SQLTrx);
                                    }

                                    // Exception scenario, CCTransaction seems to be missing
                                    if (hostedGatewayDTO.CCTransactionsPGWDTO == null)
                                    {
                                        CCRequestPGWDTO cCRequestPGWDTO = null;
                                        // Step 1 - Get the CC request
                                        log.Debug("Getting the latest CC request");
                                        // First check if the payment has been completed
                                        CCRequestPGWListBL cCRequestPGWListBLTemp = new CCRequestPGWListBL();
                                        List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGWTemp = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                                        searchParametersPGWTemp.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TrxId.ToString()));
                                        cCRequestPGWDTO = cCRequestPGWListBLTemp.GetLastestCCRequestPGWDTO(searchParametersPGWTemp);
                                        if (cCRequestPGWDTO == null)
                                        {
                                            log.Error("No entry found in CCRequest ");
                                            throw new Exception("Payment has been rejected.");
                                        }

                                        CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                                        List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                                        searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, cCRequestPGWDTO.RequestID.ToString()));
                                        List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                                        if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                                        {
                                            log.Debug("Got CC Transaction List. Count " + cCTransactionsPGWDTOList.Count);
                                            hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTOList.OrderByDescending(x => x.CreationDate).First();
                                            hostedGatewayDTO.PaymentStatusMessage = String.IsNullOrWhiteSpace(hostedGatewayDTO.CCTransactionsPGWDTO.TextResponse) ? "" : hostedGatewayDTO.CCTransactionsPGWDTO.TextResponse;
                                        }
                                    }

                                    if (newTrx.Status != TrxStatus.CLOSED && newTrx.Status != TrxStatus.CANCELLED)
                                    {
                                        log.Debug("Transaction is not in closed or cancelled status. Try to close the transaction " + newTrx.Status.ToString());
                                        if (newTrx.TransactionPaymentsDTOList == null)
                                        {
                                            log.Debug("No Payments exist for this transaction. Create new list");
                                            newTrx.TransactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
                                        }
                                        newTrx.TransactionPaymentsDTOList.Add(hostedGatewayDTO.TransactionPaymentsDTO);

                                        //if (newTrx.TransactionPaymentsDTOList != null &&
                                        //    newTrx.TransactionPaymentsDTOList.FirstOrDefault(x => x.Reference == hostedGatewayDTO.CCTransactionsPGWDTO.RefNo) != null)
                                        //{
                                        //    log.Debug("Payment is already applied to transaction. Do not add again.");
                                        //    checkForParallelTrxProcessing = true;
                                        //}
                                        //else
                                        //{
                                        //    log.Debug("Payment is not applied to transaction. Add.");
                                        //    newTrx.TransactionPaymentsDTOList.Add(hostedGatewayDTO.TransactionPaymentsDTO);
                                        //}


                                        log.Debug("Trx Status before closing " + newTrx.Status);
                                        //if (newTrx.Status != TrxStatus.CLOSED && newTrx.Status != TrxStatus.CANCELLED)
                                        //{
                                        log.Debug("Closing the transaction");
                                        int retcode = 0;
                                        retcode = newTrx.SaveTransacation(parafaitDBTransaction.SQLTrx, ref message);

                                        log.Debug("Trx Return code " + retcode);
                                        log.Debug("Trx Message " + message);

                                        if (retcode == 0)
                                        {
                                            log.Debug("Transaction closed successfully. Commiting");
                                            newTrx.Status = TrxStatus.CLOSED;
                                            parafaitDBTransaction.EndTransaction();
                                            try
                                            {
                                                tempMessage1 = method + " Successfully applied Payment " + hostedGatewayDTO.TrxId + " and PG Reference " + hostedGatewayDTO.GatewayReferenceNumber;
                                                log.Debug(tempMessage1);
                                                utilities.EventLog.logEvent(paymentGateway, 'I', tempMessage1, "Web Response", "Web Payments", 1, "", "Transaction: " + hostedGatewayDTO.TrxId, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

                                                log.Debug("Sending Transaction email");
                                                newTrx.SendTransactionPurchaseMessage(MessagingClientDTO.MessagingChanelType.NONE, null);
                                                log.Debug("Creating DB Sync Entries for card");
                                                foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine currTrxLine in newTrx.TrxLines)
                                                {
                                                    if (currTrxLine.card != null)
                                                    {
                                                        AccountBL accountBL = new AccountBL(newTrx.Utilities.ExecutionContext, currTrxLine.card.card_id, true, true);
                                                        accountBL.CreateAccountRoamingDataForTransaction(newTrx.Trx_id);
                                                    }
                                                }
                                                log.Debug("Completed Creating DB Sync Entries for card");
                                            }
                                            catch (Exception ex)
                                            {
                                                log.Debug("Error encountered while closing the transaction");
                                            }
                                            log.Debug("Closed the transaction");
                                        }
                                        else
                                        {
                                            message = utilities.MessageUtils.getMessage(240, message);
                                            message = MessageContainerList.GetMessage(newTrx.Utilities.ExecutionContext, message);
                                            //parafaitDBTransaction.RollBack();
                                            log.Info("saveTrx() in SaveTransacation as retcode != 0 error: " + message + newTrx);
                                            exceptionFlagDuplicatePayment = true;
                                        }
                                    }
                                    else
                                    {
                                        if (hostedGatewayDTO.TransactionPaymentsDTO == null &&  newTrx.TransactionPaymentsDTOList != null && newTrx.TransactionPaymentsDTOList.Any())
                                        {
                                            log.Debug("hostedGatewayDTO.TransactionPaymentsDTO is empty, assign it ");
                                            hostedGatewayDTO.TransactionPaymentsDTO = newTrx.TransactionPaymentsDTOList.Last();
                                        }
                                        log.Debug("Transaction is in closed or cancelled status. Do nothing " + newTrx.Status.ToString());
                                    }
                                }

                                if(exceptionFlagDuplicatePayment)
                                {
                                    message = "This payment could not be processed. A refund has been initiated. ";
                                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                                    hostedGatewayDTO.PaymentStatusMessage = message;
                                    log.Debug("The transaction is already in closed or cancelled states. The payment could not be processed. Initiating the refund for " +
                                        hostedGatewayDTO.TrxId + " PG reference: " + hostedGatewayDTO.GatewayReferenceNumber + " Status:" + newTrx.Status);
                                    log.Debug("Launching Refund");
                                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                                        Refund(utilities, hostedGatewayDTO, newTrx, paymentGateway, paymentGatewayObj));
                                    log.Debug("Launched Refund");
                                }
                            }
                            else if (hostedGatewayDTO.PaymentStatus == PaymentStatusType.FAILED ||
                                        hostedGatewayDTO.PaymentStatus == PaymentStatusType.CANCELLED ||
                                        hostedGatewayDTO.PaymentStatus == PaymentStatusType.ERROR)
                            {
                                log.Debug(" In Payment Failed, Cancelled or Error block. Record the attempt in CC Transaction ");
                                message = "Payment not processed. Returning null.";
                                log.Debug(message);
                                if (hostedGatewayDTO != null)
                                {
                                    if (String.IsNullOrEmpty(hostedGatewayDTO.PaymentStatusMessage))
                                    {
                                        log.Debug("Empty status message");
                                        hostedGatewayDTO.PaymentStatusMessage = "";
                                    }
                                    message += hostedGatewayDTO.TrxId > -1 ? " Trx Id:" + hostedGatewayDTO.TrxId : "";
                                    message += "Payment Status Type:" + hostedGatewayDTO.PaymentStatus.ToString();
                                    message += "Payment Process Status Type:" + hostedGatewayDTO.PaymentProcessStatus.ToString();
                                    message += "PaymentStatusMessage:" + hostedGatewayDTO.PaymentStatusMessage.ToString();
                                    log.Debug("Extended message " + message);
                                }
                                utilities.EventLog.logEvent(paymentGateway, 'E', message, "Error", "Web Payments", 1, "", "Transaction: " + hostedGatewayDTO.TrxId, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                                parafaitDBTransaction.EndTransaction();

                                try
                                {
                                    log.Debug("Launching Cancel Transaction");
                                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                                        CancelTransaction(utilities, newTrx));
                                    log.Debug("Finished cancelling transaction");
                                }
                                catch(Exception ex)
                                {
                                    log.Debug("Got error while trying to cancel transaction " + ex.Message);
                                }
                            }
                            else
                            {
                                log.Debug(" Payment is neither successful nor failed, return an error. Status is " + hostedGatewayDTO.PaymentStatus);
                                message = "Payment not processed. Returning null.";
                                log.Debug(message);
                                if(hostedGatewayDTO != null)
                                {
                                    if (String.IsNullOrEmpty(hostedGatewayDTO.PaymentStatusMessage))
                                    {
                                        log.Debug("Empty status message");
                                        hostedGatewayDTO.PaymentStatusMessage = "";
                                    }
                                    message += hostedGatewayDTO.TrxId > -1 ? " Trx Id:" + hostedGatewayDTO.TrxId : "";
                                    message += "Payment Status Type:" + hostedGatewayDTO.PaymentStatus.ToString();
                                    message += "Payment Process Status Type:" + hostedGatewayDTO.PaymentProcessStatus.ToString();
                                    message += "PaymentStatusMessage:" + hostedGatewayDTO.PaymentStatusMessage.ToString();
                                    log.Debug("Extended message 1 " + message);
                                }
                                utilities.EventLog.logEvent(paymentGateway, 'E', message, "Error", "Web Payments", 1, "", "Transaction: " + hostedGatewayDTO.TrxId, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                                parafaitDBTransaction.EndTransaction();
                                //throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, message));
                            }
                        }
                        else
                        {
                            message = "payment gateway is not identified " + paymentGateway + ":" + gatewayResponse + ":" + (hostedGatewayDTO != null ? hostedGatewayDTO.TrxId : -1);
                            log.Debug(message);
                            throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, message));
                        }
                    }
                    catch (Exception ex)
                    {
                        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                        log.Fatal("Error encountered while processing payment " + paymentGateway + ":" + gatewayResponse);
                        log.Fatal(customException);
                        parafaitDBTransaction.RollBack();
                    }
                    finally
                    {
                        log.Debug("In Finally bock");
                        // Create the return object. In error scenarios, this will be null
                        if (hostedGatewayDTO != null && hostedGatewayDTO.PaymentStatus == PaymentStatusType.NONE)
                        {
                            log.Debug("The payment status is still in null, check if the trx is processed");
                            try
                            {
                                TransactionBL transactionBL = new TransactionBL(utilities.ExecutionContext, hostedGatewayDTO.TrxId);
                                if (transactionBL.TransactionDTO.Status == "CLOSED")
                                {
                                    log.Debug("Transaction is in closed status. Return as success");
                                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                                    hostedGatewayDTO.TrxGuid = transactionBL.TransactionDTO.Guid;
                                    if (transactionBL.TransactionDTO.TrxPaymentsDTOList != null && transactionBL.TransactionDTO.TrxPaymentsDTOList.Any())
                                    {
                                        hostedGatewayDTO.TransactionPaymentsDTO = transactionBL.TransactionDTO.TrxPaymentsDTOList.OrderByDescending(x => x.PaymentId).ToList()[0];
                                    }
                                }
                            }
                            catch(Exception ex)
                            {
                                log.Debug("Caught error while creating the default transaction in finally block");
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(hostedGatewayDTO);
            return hostedGatewayDTO;
        }

        private static async void Refund(Utilities utilities, HostedGatewayDTO hostedGatewayDTO, Semnox.Parafait.Transaction.Transaction newTrx, String paymentGateway, PaymentGateway paymentGatewayObj)
        {
#pragma warning disable 4014
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                log.LogMethodEntry();
                log.Debug("Running refund as a parallel task. Sleep");
                System.Threading.Thread.Sleep(60000);
                log.Debug("Running refund as a parallel task. Awake");
                String errorMessage = "The transaction is already in closed or cancelled states. The payment could not be processed. Initiating the refund for " +
                                        hostedGatewayDTO.TrxId + " PG reference: " + hostedGatewayDTO.GatewayReferenceNumber + " Status:" + newTrx.Status;
                log.Debug(errorMessage);
                try
                {
                    // Call the refund here
                    log.Debug("Initiating refund");
                    TransactionPaymentsDTO refundPaymentDTO = new TransactionPaymentsDTO();
                    refundPaymentDTO.TransactionId = hostedGatewayDTO.TrxId;
                    refundPaymentDTO.Reference = hostedGatewayDTO.GatewayReferenceNumber;

                    double refundAmount = 0;
                    if (hostedGatewayDTO.TransactionPaymentsDTO != null)
                        refundAmount = hostedGatewayDTO.TransactionPaymentsDTO.Amount;
                    if (refundAmount == 0)
                        refundAmount = newTrx.Transaction_Amount;

                    log.Debug("Refund Amount " + refundAmount);
                    utilities.EventLog.logEvent(paymentGateway, 'I', errorMessage + " Refund Amount: " + refundAmount, "Refund", "Web Payments", 1, "", "Transaction: " + newTrx.Trx_id, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    refundPaymentDTO.Amount = refundAmount;
                    if (refundAmount > 0)
                    {
                        paymentGatewayObj.RefundAmount(refundPaymentDTO);
                        log.Debug("Completed refund");
                    }
                    else
                    {
                        log.Debug("Refund amount is 0. Did not initiate refund");
                    }
                }
                catch (Exception ex)
                {
                    utilities.EventLog.logEvent(paymentGateway, 'E', ex.Message, "PAYMENT REFUND FAILED", "Web Payments", 1, "", "Transaction: " + newTrx.Trx_id, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    log.Debug("Got an error during refund " + ex.Message);
                }
                finally
                {
                    log.LogMethodExit();
                }
            }).ConfigureAwait(false);
#pragma warning restore 4014
        }

        private static async void CancelTransaction(Utilities utilities, Semnox.Parafait.Transaction.Transaction newTrx)
        {
#pragma warning disable 4014
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                log.LogMethodEntry();
                log.Debug("Running cancel transaction as a parallel task.");
                String errorMessage = "Payment has failed, cancelling the transaction. " +
                                        newTrx.Trx_id + " Status:" + newTrx.Status;
                log.Debug(errorMessage);
                try
                {
                    // Nitin: this needs to be checked
                    foreach(Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine in newTrx.TrxLines)
                    {
                        trxLine.AllowEdit = true;
                    }

                    if (newTrx.cancelTransaction(ref errorMessage))
                    {
                        log.Debug("Cancel transaction is successful ");
                    }
                    else
                    {
                        log.Debug("Cancel transaction failed " + errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    log.Debug("Got an error during cancel transaction " + ex.Message);
                }
                finally
                {
                    log.LogMethodExit();
                }
            }).ConfigureAwait(false);
#pragma warning restore 4014
        }

        private HostedGatewayDTO ProcessFailedOrCancelledPayment(String paymentGateway, String gatewayResponse)
        {
            log.LogMethodEntry(paymentGateway, gatewayResponse);

            HostedGatewayDTO hostedGatewayDTO = null;
            ExecutionContext executionContext = new ExecutionContext("External POS", SiteContainerList.GetMasterSiteId(), -1, -1, SiteContainerList.IsCorporate(), -1); 
            int siteId = SiteContainerList.GetMasterSiteId();
            int trxId = -1;
            try
            {
                log.Debug("Initiating payment gateway response processing - failed/cancelled responses");
                using (Utilities utilities = new Utilities(new ParafaitDBTransaction().GetConnection()))
                {
                    PaymentGatewayFactory.GetInstance().Initialize(utilities, true, null);
                    HostedPaymentGateway paymentGatewayObj = (HostedPaymentGateway)PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentGateway);
                    if (paymentGatewayObj != null)
                    {
                        log.Debug("Payment gateway identified. Calling function to check payment status and process gateway response.");
                        hostedGatewayDTO = paymentGatewayObj.InitiatePaymentProcessing(gatewayResponse);
                        hostedGatewayDTO = InitiatePaymentProcessing(utilities.ExecutionContext, paymentGatewayObj, hostedGatewayDTO);

                        // If the check process failed or if the CC request is in payment processing status, process the response
                        if (hostedGatewayDTO == null || hostedGatewayDTO.PaymentProcessStatus == PaymentProcessStatusType.PAYMENT_PROCESSING)
                        {
                            // check if the context has to be changed
                            if (hostedGatewayDTO != null && hostedGatewayDTO.SiteId != siteId)
                            {
                                SetExecutionContext(utilities.ExecutionContext, utilities, hostedGatewayDTO.SiteId);
                            }
                            // Do this as the execution context is reset
                            PaymentGatewayFactory.GetInstance().Initialize(utilities, true, null);
                            paymentGatewayObj = (HostedPaymentGateway)PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentGateway);
                            hostedGatewayDTO = paymentGatewayObj.ProcessGatewayResponse(gatewayResponse);
                            log.Debug("Payment gateway response has been processed");
                        }

                        // The gateway response has been processed.
                        if (hostedGatewayDTO != null)
                        {
                            log.Debug("Payment gateway response. Post processing begins");
                            if (hostedGatewayDTO.CCTransactionsPGWDTO != null && hostedGatewayDTO.CCTransactionsPGWDTO.IsChanged)
                            {
                                log.Debug("Saving CCTransaction");
                                CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(hostedGatewayDTO.CCTransactionsPGWDTO);
                                cCTransactionsPGWBL.Save();
                                hostedGatewayDTO.TransactionPaymentsDTO.CCResponseId = cCTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                log.Debug("Created CCTransaction " + cCTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID);
                            }
                        }
                        else
                        {
                            log.Debug("Payment gateway response processing failed.");
                        }
                    }
                    else
                    {
                        log.Fatal("payment gateway is not identified " + paymentGateway + ":" + gatewayResponse + ":" + trxId);

                    }
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Fatal("Error encountered while processing payment " + paymentGateway + ":" + gatewayResponse);
                log.Error(customException);
            }
            finally
            {
                // Create the return object. In error scenarios, this will be null
                if (hostedGatewayDTO == null)
                {
                    hostedGatewayDTO = new HostedGatewayDTO();
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                    if (hostedGatewayDTO.TransactionPaymentsDTO == null)
                        hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();

                    hostedGatewayDTO.TransactionPaymentsDTO.Reference = trxId.ToString();
                }
            }
            log.LogMethodExit(hostedGatewayDTO);
            return hostedGatewayDTO;
        }

        /// <summary>
        /// Transfer entitlements from parent card to child card
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("api/Transaction/Payments/HostedGateways/Callback/{paymentGateway}")]
        public async Task<HttpResponseMessage> Post([FromUri] String paymentGateway, [FromBody] dynamic gatewayResponse)
        {
            log.LogMethodEntry(paymentGateway, gatewayResponse);
            try
            {
                log.Debug("Calling Process Payment");
                // Get the response from the response body
                var gatewayResponseString = await GetResponseBody(this.Request);

                if (gatewayResponseString == null || String.IsNullOrEmpty(gatewayResponseString.ToString()))
                    gatewayResponseString = Convert.ToString(gatewayResponse);

                HostedGatewayDTO hostedGatewayDTO = ProcessPayment(paymentGateway, gatewayResponseString, "CALLBACK");
                log.Debug("Completed Process Payment " + hostedGatewayDTO);
                if (hostedGatewayDTO == null)
                {
                    // payment could not be processed, return a non ok status code
                    log.Debug("Hoasted DTO is null. Returning error.");
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
                else if (hostedGatewayDTO.PaymentStatus != PaymentStatusType.SUCCESS)
                {
                    log.Debug("Payment was not successful " + hostedGatewayDTO.PGFailedResponseMessage);
                    // payment was handled but it was not successful, return ok
                    return Request.CreateResponse(HttpStatusCode.OK, hostedGatewayDTO.PGFailedResponseMessage);
                }
                else
                {
                    log.Debug("Payment was successful " + hostedGatewayDTO.PGSuccessResponseMessage);
                    // payment was handled and it was successful, return ok
                    return Request.CreateResponse(HttpStatusCode.OK, hostedGatewayDTO.PGSuccessResponseMessage);
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, new ExecutionContext("Semnox", SiteContainerList.GetMasterSiteId(), -1, -1, true, -1));
                log.Fatal("Error encountered while processing payment " + paymentGateway + ":" + gatewayResponse);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Handling the response from payment gateway
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("api/Transaction/Payments/HostedGateways/Success/{paymentGateway}")]
        public async Task<HttpResponseMessage> PostResponse([FromUri] String paymentGateway, [FromBody] dynamic gatewayResponse)
        {
            log.LogMethodEntry(paymentGateway, gatewayResponse);
            HttpResponseMessage response = null;
            String redirectPath = "";
            bool isHostedGateway = true;

            try
            {
                // Get the response from the response body
                var gatewayResponseString = await GetResponseBody(this.Request);

                if (gatewayResponseString == null || String.IsNullOrEmpty(gatewayResponseString.ToString()))
                    gatewayResponseString = Convert.ToString(gatewayResponse);

                log.Debug("Calling Process Payment");
                HostedGatewayDTO hostedGatewayDTO = ProcessPayment(paymentGateway, gatewayResponseString, "RESPONSE");
                log.Debug("Completed Process Payment " + hostedGatewayDTO);
                if (hostedGatewayDTO == null)
                {
                    log.Debug("Hosted DTO is null, redirect to default error path");
                    redirectPath = defaultRedirectURL;
                }
                else if (hostedGatewayDTO.PaymentStatus != PaymentStatusType.SUCCESS)
                {
                    isHostedGateway = hostedGatewayDTO.IsHostedGateway;
                    String replacedfailureMessage = BuildMessage(failureMessage, hostedGatewayDTO);
                    log.Debug("Payment was not successful " + hostedGatewayDTO.FailureURL + " : message :" + replacedfailureMessage);
                    if (hostedGatewayDTO.TrxGuid != null)
                    {
                        redirectPath = hostedGatewayDTO.FailureURL.Replace("@trxGuid", hostedGatewayDTO.TrxGuid.ToString()).Replace("@message", replacedfailureMessage);
                    }
                    else
                    {
                        redirectPath = defaultRedirectURL;
                    }
                }
                else
                {
                    // payment was handled but it was not successful, return ok
                    isHostedGateway = hostedGatewayDTO.IsHostedGateway;
                    String replacedsuccessMessage = BuildMessage(successMessage, hostedGatewayDTO);
                    log.Debug("Payment was successful " + hostedGatewayDTO.SuccessURL + " :message: " + replacedsuccessMessage);
                    redirectPath = hostedGatewayDTO.SuccessURL.Replace("@trxGuid", hostedGatewayDTO.TrxGuid.ToString()).Replace("@message", replacedsuccessMessage);
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, new ExecutionContext("Semnox", SiteContainerList.GetMasterSiteId(), -1, -1, true, -1));
                log.Fatal("Error encountered while processing payment " + paymentGateway + ":" + gatewayResponse);
                log.Error(customException);
                redirectPath = defaultRedirectURL;
            }
            log.Debug("Redirecting to " + redirectPath);

            if (isHostedGateway)
            {
                log.Debug("This is a hosted gateway, creating a redirect response");
                response = Request.CreateResponse(HttpStatusCode.Redirect);
                response.Headers.Add("Location", redirectPath);
            }
            else
            {
                log.Debug("This is not a hosted gateway. Send the response back as 200 status code");
                response = Request.CreateResponse(HttpStatusCode.OK, new { data = redirectPath });
            }
            return response;
        }

        /// <summary>
        /// Handling the failed payments
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("api/Transaction/Payments/HostedGateways/Failed/{paymentGateway}")]
        public async Task<HttpResponseMessage> PostFailureResponse([FromUri] String paymentGateway, [FromBody] dynamic gatewayResponse)
        {
            log.LogMethodEntry(paymentGateway, gatewayResponse);
            HttpResponseMessage response = null;
            String redirectPath = "";
            bool isHostedGateway = true;

            try
            {
                log.Debug("Calling Process Payment - Failure URL");
                HostedGatewayDTO hostedGatewayDTO = null;

                if(gatewayResponse != null)
                {
                    // Get the response from the response body
                    var gatewayResponseString = await GetResponseBody(this.Request);

                    if (gatewayResponseString == null || String.IsNullOrEmpty(gatewayResponseString.ToString()))
                        gatewayResponseString = Convert.ToString(gatewayResponse);

                    log.Debug("Got gateway response string" + gatewayResponseString);

                    //hostedGatewayDTO = ProcessFailedOrCancelledPayment(paymentGateway, gatewayResponseString);
                    hostedGatewayDTO = ProcessPayment(paymentGateway, gatewayResponseString, "FAILED");
                    log.Debug("Completed Process Payment " + hostedGatewayDTO);
                }

                if (hostedGatewayDTO == null || string.IsNullOrWhiteSpace(hostedGatewayDTO.TrxGuid))
                {
                    log.Debug("Unable to processed the response. Redirect to the default link");
                    redirectPath = defaultRedirectURL;
                }
                else
                {
                    // payment was handled but it was not successful, return ok
                    isHostedGateway = hostedGatewayDTO.IsHostedGateway;
                    String replacedfailureMessage = BuildMessage(failureMessage, hostedGatewayDTO);
                    log.Debug("Payment was not successful " + hostedGatewayDTO.FailureURL + " : message :" + replacedfailureMessage);
                    redirectPath = hostedGatewayDTO.FailureURL.Replace("@trxGuid", hostedGatewayDTO.TrxGuid.ToString()).Replace("@message", replacedfailureMessage);
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, new ExecutionContext("Semnox", SiteContainerList.GetMasterSiteId(), -1, -1, true, -1));
                log.Fatal("Error encountered while processing payment " + paymentGateway + ":" + gatewayResponse);
                log.Error(customException);
                redirectPath = defaultRedirectURL;
            }
            log.Debug("Redirecting to " + redirectPath);
            if (isHostedGateway)
            {
                log.Debug("This is a hosted gateway, creating a redirect response");
                response = Request.CreateResponse(HttpStatusCode.Redirect);
                response.Headers.Add("Location", redirectPath);
            }
            else
            {
                log.Debug("This is not a hosted gateway. Send the response back as 200 status code");
                response = Request.CreateResponse(HttpStatusCode.OK, new { data = redirectPath });
            }
            return response;
        }


        /// <summary>
        /// Handling the response from payment gateway
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Route("api/Transaction/Payments/HostedGateways/Cancel/{paymentGateway}/{transactionId}")]
        public async Task<HttpResponseMessage> CancelGet([FromUri] String paymentGateway, String transactionId)
        {
            log.LogMethodEntry(paymentGateway, transactionId);
            HttpResponseMessage response = null;
            String redirectPath = "";
            bool isHostedGateway = true;
            int trxId = -1;
            using (Utilities utilities = new Utilities(new ParafaitDBTransaction().GetConnection()))
            {
                try
                {
                    int.TryParse(transactionId, out trxId);
                    utilities.EventLog.logEvent(paymentGateway, 'I', "Cancelled Payment", "Web Response", "Web Payments", 1, "", paymentGateway + " Transaction: " + trxId, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    ExecutionContext executionContext = new ExecutionContext("External POS", SiteContainerList.GetMasterSiteId(), -1, -1, SiteContainerList.IsCorporate(), -1);
                    HostedGatewayDTO hostedGatewayDTO = new HostedGatewayDTO();
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                    if (hostedGatewayDTO.TransactionPaymentsDTO == null)
                        hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();

                    TransactionBL transactionBL = new TransactionBL(executionContext, trxId);
                    log.Debug("Got transaction dto " + transactionBL.TransactionDTO.TransactionId + ":" + transactionBL.TransactionDTO.Guid);
                    hostedGatewayDTO.TrxGuid = transactionBL.TransactionDTO.Guid;
                    hostedGatewayDTO.TrxId = trxId;
                    hostedGatewayDTO.PaymentStatusMessage = MessageViewContainerList.GetMessage(executionContext, "Payment has been cancelled by the customer");

                    LookupsContainerDTO lookupValuesList = LookupsViewContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), "WEB_SITE_CONFIGURATION");
                    if (lookupValuesList == null)
                    {
                        String errMsg = MessageViewContainerList.GetMessage(executionContext, "Please enter &1 lookup. Site : &2", new string[] { "WEB_SITE_CONFIGURATION", executionContext.GetSiteId().ToString() });
                        log.Error(errMsg.ToString());
                        throw new PaymentGatewayConfigurationException(MessageViewContainerList.GetMessage(executionContext, errMsg.ToString()));
                    }
                    List<LookupValuesContainerDTO> lookupValuesDTOlist = lookupValuesList.LookupValuesContainerDTOList;
                    if (lookupValuesDTOlist == null || !lookupValuesDTOlist.Any())
                    {
                        log.Error("WEB_SITE_CONFIGURATION lookup not found.");
                        throw new PaymentGatewayConfigurationException(MessageViewContainerList.GetMessage(executionContext, "WEB_SITE_CONFIGURATION lookup not found."));
                    }
                    log.Debug("Built lookups");

                    if (lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_REDIRECT_URL").Count() > 0)
                    {
                        hostedGatewayDTO.CancelURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_REDIRECT_URL").First().Description;
                    }

                    isHostedGateway = hostedGatewayDTO.IsHostedGateway;
                    String replacedfailureMessage = BuildMessage(failureMessage, hostedGatewayDTO);
                    log.Debug("Payment was cancelled " + hostedGatewayDTO.CancelURL + " : message :" + replacedfailureMessage);
                    if (hostedGatewayDTO.TrxGuid != null)
                    {
                        redirectPath = hostedGatewayDTO.CancelURL.Replace("@trxGuid", hostedGatewayDTO.TrxGuid.ToString()).Replace("@message", replacedfailureMessage);
                    }
                    else
                    {
                        redirectPath = defaultRedirectURL;
                    }

                    utilities.EventLog.logEvent(paymentGateway, 'I', "Cancelled Payment", "Web Response", "Web Payments", 1, "", " Redirecting to: " + redirectPath, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

                    log.Debug("Trying to reverse transaction now");
                    transactionBL.TransactionDTO.ReverseTransaction = true;
                    transactionBL.Save();
                    log.Debug("Reversed transaction");
                    utilities.EventLog.logEvent(paymentGateway, 'I', "Cancelled Payment", "Web Response", "Web Payments", 1, "", " Canelled transaction: " + trxId, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                }
                catch (Exception ex)
                {
                    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, new ExecutionContext("Semnox", SiteContainerList.GetMasterSiteId(), -1, -1, true, -1));
                    log.Fatal("Error encountered while processing payment " + paymentGateway + ":" + transactionId);
                    log.Error(customException);
                    redirectPath = defaultRedirectURL;
                    utilities.EventLog.logEvent(paymentGateway, 'E', "Cancelled Payment", "Web Response", "Web Payments", 1, "", ex.Message, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                }
            }
            log.Debug("Redirecting to " + redirectPath);

            log.Debug("This is a hosted gateway, creating a redirect response");
            response = Request.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Add("Location", redirectPath);
            return response;
        }

    }
}
