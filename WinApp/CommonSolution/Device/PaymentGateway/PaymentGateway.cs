/*
 *  * Project Name - PaymentGateway
 * Description  - PaymentGateway
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *****************************************************************************************************************
*2.90.0       23-Jun-2020      Raghuveera       Variable refund to check variable redund is allowed or not
*2.100.0      01-Sep-2020      Guru S A         Payment link changes
 *2.110.0       30-Dec-2020      Girish Kundar       Modified : Added delete method = Payment link changes
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
*/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
//using Semnox.Parafait.TransactionPayments;
using System.Windows.Forms;
using Semnox.Parafait.logging;
using System.Drawing;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;
using System.Linq;
using System.ServiceProcess;
using System.Security.Permissions;
using System.Threading;

namespace Semnox.Parafait.Device.PaymentGateway
{

    /// <summary>
    /// Delegate that invokes to display the message.
    /// </summary>
    public delegate DialogResult ShowMessageDelegate(string Message, string Title, MessageBoxButtons msgboxButtons = MessageBoxButtons.YesNo, MessageBoxIcon msgboxIcon = MessageBoxIcon.None);

    /// <summary>
    /// Delegate that invokes to write the Log to a File
    /// </summary>
    public delegate void WriteToLogDelegate(int KioskTrxId, string Activity, int TrxId, int Value, string Message, int POSMachineId, string POSMachine);

    /// <summary>
    /// Represents a payment gateway used for accepting credit/debit card payments.
    /// </summary>
    public class PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parafait utilities.
        /// </summary>
        protected Utilities utilities;

        /// <summary>
        /// Whether the payment process is supervised by an attendant.
        /// </summary>
        protected bool isUnattended;

        /// <summary>
        /// Delegate instance to display message.
        /// </summary>
        protected ShowMessageDelegate showMessageDelegate;

        /// <summary>
        /// Delegate instance for writing the Log to File
        /// </summary>
        protected WriteToLogDelegate writeToLogDelegate;

        /// <summary>
        /// Whether the card used for the current transaction is a credit card
        /// </summary>
        public bool IsCreditCard
        {
            get; set;
        }
        private long customerId;
        /// <summary>
        /// Customer id is set before calling makepayment method for passing the customer Id to the payment gateway
        /// </summary>
        public long CustomerId { get { return customerId; } set { customerId = value; } }
        private string customerName;
        /// <summary>
        /// Customer name is set before calling makepayment method for passing the customer name to the payment gateway
        /// </summary>
        public string CustomerName { get { return customerName; } set { customerName = value; } }
        private string customerMailId;
        /// <summary>
        /// Customer mail id is set before calling makepayment method for passing the customer mail id to the payment gateway
        /// </summary>
        public string CustomerMailId { get { return customerMailId; } set { customerMailId = value; } }

        /// <summary>
        /// Whether to print receipt
        /// </summary>
        protected bool printReceipt = true;

        /// <summary>
        /// Over all transaction amount
        /// </summary>
        protected decimal overallTransactionAmount = 0;
        /// <summary>
        /// Overall tip amount entered
        /// </summary>
        protected decimal overallTipAmountEntered = 0;

        /// <summary>
        /// Get/Set Methods for printReceipt
        /// </summary>
        public bool PrintReceipt
        {
            get
            {
                return printReceipt;
            }

            set
            {
                printReceipt = value;
            }
        }

        /// <summary>
        /// Transaction of type purchase.
        /// </summary>
        protected const string CCREQUEST_TRANSACTION_TYPE_PURCHASE = "PURCHASE";

        /// <summary>
        /// Transaction of type refund. 
        /// </summary>
        protected const string CCREQUEST_TRANSACTION_TYPE_REFUND = "REFUND";

        /// <summary>
        /// Credit card transaction of type payment
        /// </summary>
        protected const string CREDIT_CARD_PAYMENT = "CREDIT CARD PAYMENT";

        /// <summary>
        /// Credit card transaction of type refund
        /// </summary>
        protected const string CREDIT_CARD_REFUND = "CREDIT CARD REFUND";
        
        /// <summary>
        /// Credit card transaction of type void
        /// </summary>
        protected const string CREDIT_CARD_VOID = "CREDIT CARD VOID";

        protected DateTime lastTransactionCompleteTime;

        // flag to indicate web hostedPayment Gateway
        protected bool isHostedPayment;

        protected bool paymentLinkEnabled = false;
        protected const string PAYMENTLINKSETUP = "PAYMENT_LINK_SETUP";
        protected const string TRANSACTIONPAYMENTLINKURL = "ONLINE_PAYMENT_LINK_URL";
        public enum TRX_STATUS_CHECK_RESPONSE
        {
            //No action required
            NO_ACTION,
            //Wait
            WAIT,
            //Send Last Trx check Request
            SEND_STATUS_CHECK
        }
        /// <summary>
        /// Constructor of payment gateway class.
        /// </summary>
        /// <param name="utilities">Parafait utilities.</param>
        /// <param name="isUnattended">Whether the payment process is supervised by an attendant.</param>
        /// <param name="showMessageDelegate"> Delegate instance to display message.</param>
        /// <param name="writeToLogDelegate">Delegate instance for writing the Log to File</param>
        public PaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            this.utilities = utilities;
            this.isUnattended = isUnattended;
            this.showMessageDelegate = showMessageDelegate;
            this.writeToLogDelegate = writeToLogDelegate;
            lastTransactionCompleteTime = ServerDateTime.Now;
            paymentLinkEnabled = IsPaymentLinkEnbled();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Initializes the payment gateway.
        /// </summary>
        public virtual void Initialize()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Returns the CCStatusPGW StatusId for the given status.
        /// </summary>
        /// <param name="status">one of the following status. (Approved, Declined, Error, NewRequest, StatusMessage, Submitted, Success, VoidSaleInitiated, WSError, Partial Approved)</param>
        /// <returns></returns>
        protected int GetCCStatusPGWStatusId(string status)
        {
            log.LogMethodEntry(status);
            int returnValue = -1;
            CCStatusPGWListBL cCStatusPGWListBL = new CCStatusPGWListBL();
            List<KeyValuePair<CCStatusPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCStatusPGWDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CCStatusPGWDTO.SearchByParameters, string>(CCStatusPGWDTO.SearchByParameters.STATUS_MESSAGE, status));
            searchParameters.Add(new KeyValuePair<CCStatusPGWDTO.SearchByParameters, string>(CCStatusPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));

            List<CCStatusPGWDTO> cCStatusPGWDTOList = cCStatusPGWListBL.GetCCStatusPGWDTOList(searchParameters);
            if (cCStatusPGWDTOList != null && cCStatusPGWDTOList.Count == 1)
            {
                returnValue = cCStatusPGWDTOList[0].StatusId;
            }
            else
            {
                log.LogMethodExit(null, "Throwing Payment Gateway Exception Status: " + status + " doesn't exist. Please check the configuration.");
                throw new PaymentGatewayException("Status: " + status + " doesn't exist. Please check the configuration.");
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Creates CCRequestPGW entry for the payment transaction.
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="transactionType"></param>
        /// <returns></returns>
        protected virtual CCRequestPGWDTO CreateCCRequestPGW(TransactionPaymentsDTO transactionPaymentsDTO, string transactionType, string PaymentProcessStatus = null)
        {
            log.LogMethodEntry(transactionPaymentsDTO, transactionType);
            CCRequestPGWDTO cCRequestPGWDTO = new CCRequestPGWDTO();
            cCRequestPGWDTO.InvoiceNo = transactionPaymentsDTO.TransactionId.ToString();
            cCRequestPGWDTO.POSAmount = transactionPaymentsDTO.Amount.ToString("#,##0.000");
            cCRequestPGWDTO.TransactionType = transactionType;
            cCRequestPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_SUCCESS);
            cCRequestPGWDTO.MerchantID = utilities.ParafaitEnv.POSMachine;
            cCRequestPGWDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_INITIATED.ToString();

            if (!String.IsNullOrWhiteSpace(transactionPaymentsDTO.Reference))
            {
                cCRequestPGWDTO.ReferenceNo = transactionPaymentsDTO.Reference;
            }
            if (!String.IsNullOrWhiteSpace(PaymentProcessStatus))
            {
                cCRequestPGWDTO.PaymentProcessStatus = PaymentProcessStatus;
            }
            CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(utilities.ExecutionContext, cCRequestPGWDTO);
            cCRequestPGWBL.Save();
            log.LogMethodExit(cCRequestPGWDTO);
            return cCRequestPGWDTO;
        }

        /// <summary>
        /// This method is to call after each transaction complete and clear all the objects which are created/set during the transaction.
        /// </summary>
        public virtual void CleanUp()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// This method sets the overall transaction amount for calculating the tip amount percentage 
        /// </summary>
        /// <param name="amount"></param>
        public void SetTransactionAmount(decimal amount)
        {
            log.LogMethodEntry(amount);
            overallTransactionAmount = amount;
            log.LogMethodExit();
        }
        /// <summary>
        /// This method sets the overall tip amount already entered to calculate the percentage including entered tip amount.
        /// </summary>
        /// <param name="tipAmount"></param>
        public void SetTotalTipAmountEntered(decimal tipAmount)
        {
            log.LogMethodEntry(tipAmount);
            overallTipAmountEntered = tipAmount;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns boolean based on whether payment requires a settlement to be done.
        /// </summary>
        /// <param name="transactionPaymentsDTO">transactionPaymentsDTO</param>
        /// <returns></returns>
        public virtual bool IsSettlementPending(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// Returns list of CCTransactionsPGWDTO's  pending for settelement. 
        /// </summary>
        /// <returns></returns>
        public virtual List<CCTransactionsPGWDTO> GetAllUnsettledCreditCardTransactions()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
            return null;
        }
        /// <summary>
        /// CheckLastTransactionStatus. 
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns></returns>
        public virtual bool CheckLastTransactionStatus(int trxId = -1)
        {
            //Any changes here make sure to review ValidateLastTransactionStatus() as well
            log.LogMethodEntry(trxId);
            try
            {
                CCRequestPGWDTO cCRequestPGWDTO;
                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                CCTransactionsPGWListBL ccTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList;
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> ccRequestSearchParams = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                if (trxId == -1)
                {
                    ccRequestSearchParams.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.MERCHANT_ID, utilities.ParafaitEnv.POSMachine));
                }
                else
                {
                    ccRequestSearchParams.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId.ToString()));
                    if (isUnattended)
                    {
                        ccRequestSearchParams.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.MERCHANT_ID, utilities.ParafaitEnv.POSMachine));
                    }
                }
                ccRequestSearchParams.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                cCRequestPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(ccRequestSearchParams);
                if (cCRequestPGWDTO == null)
                {
                    if (trxId > -1)
                    {
                        log.Debug("There is no credit card transaction done from this POS");
                    }
                    else
                    {
                        log.Debug("There is no credit card transaction done for this transaction");
                    }
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, cCRequestPGWDTO.RequestID.ToString()));
                    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                    cCTransactionsPGWDTOList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
                    if (cCTransactionsPGWDTOList == null || cCTransactionsPGWDTOList.Count == 0)
                    {
                        //ToDo: In case of capture if the response is not received then last transaction check should use the original Authorization request's request ID instead of capture request requestid for checking the last transaction status. Currently there is no link between original authorization request and capture request of that authorization.
                        //TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                        //transactionsPaymentsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, cCRequestPGWDTO.RequestID.ToString()));
                        //ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        SendLastTransactionStatusCheckRequest(cCRequestPGWDTO, null);
                    }
                    else
                    {
                        transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                        transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        List<TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);
                        if (transactionPaymentsDTOs == null || transactionPaymentsDTOs.Count == 0)
                        {
                            SendLastTransactionStatusCheckRequest(cCRequestPGWDTO, cCTransactionsPGWDTOList[0]);
                        }
                        else
                        {
                            if (trxId == -1)
                            {
                                log.Debug("credit card transaction done from this POS is up to date.");
                            }
                            else
                            {
                                log.Debug("credit card transaction done for this transaction is up to date");
                            }
                            log.LogMethodExit(true);
                            return true;
                        }
                    }
                }
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error in last transaction check", ex);
                log.LogMethodExit(false);
                return false;
            }

        }

        /// <summary>
        /// ValidateLastTransactionStatus
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns></returns>
        public virtual KeyValuePair<TRX_STATUS_CHECK_RESPONSE, List<ValidationError>> ValidateLastTransactionStatus(int trxId)
        {
            //Any changes here make sure to review CheckLastTransactionStatus() as well
            log.LogMethodEntry(trxId);
            List<ValidationError> validationErrorsList = new List<ValidationError>();
            KeyValuePair<TRX_STATUS_CHECK_RESPONSE, List<ValidationError>> keyValuePair;
            keyValuePair = new KeyValuePair<TRX_STATUS_CHECK_RESPONSE, List<ValidationError>>(TRX_STATUS_CHECK_RESPONSE.NO_ACTION, validationErrorsList);
            try
            {
                CCRequestPGWDTO cCRequestPGWDTO;
                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                CCTransactionsPGWListBL ccTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList;
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> ccRequestSearchParams = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                ccRequestSearchParams.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId.ToString()));
                ccRequestSearchParams.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                cCRequestPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(ccRequestSearchParams);
                if (cCRequestPGWDTO == null)
                {
                    keyValuePair = new KeyValuePair<TRX_STATUS_CHECK_RESPONSE, List<ValidationError>>(TRX_STATUS_CHECK_RESPONSE.NO_ACTION, validationErrorsList);
                    log.Info("There is no credit card transaction done for this transaction");
                }
                else
                {
                    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, cCRequestPGWDTO.RequestID.ToString()));
                    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                    cCTransactionsPGWDTOList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
                    if (cCTransactionsPGWDTOList == null || cCTransactionsPGWDTOList.Count == 0)
                    {
                        ////ToDo: In case of capture if the response is not received then last transaction check should use the original Authorization request's request ID instead of capture request requestid for checking the last transaction status. Currently there is no link between original authorization request and capture request of that authorization.
                        ////TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                        ////transactionsPaymentsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, cCRequestPGWDTO.RequestID.ToString()));
                        ////ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        //SendLastTransactionStatusCheckRequest(cCRequestPGWDTO, null); 

                        int waitTimeLimit = ParafaitDefaultContainerList.GetParafaitDefault<int>(utilities.ExecutionContext, "WAIT_TIME_FOR_LAST_PAYMENT_REQ_STATUS_CHECK", 15);
                        if (waitTimeLimit < 15)
                        {
                            waitTimeLimit = 15;
                            log.Info("Wait time should be minimum 15 minutes");
                        }
                        TimeSpan timeDiff = (cCRequestPGWDTO.RequestDatetime.AddMinutes(waitTimeLimit) - ServerDateTime.Now);
                        int waitMinutes = 0;
                        if (waitMinutes != null)
                        {
                            waitMinutes = (int)timeDiff.TotalMinutes;
                        }
                        if (ServerDateTime.Now > cCRequestPGWDTO.RequestDatetime.AddMinutes(waitTimeLimit))
                        {
                            string errorMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2765, waitMinutes);
                            //Payment request is pending for more that &1 minutes. Sending status check request...
                            validationErrorsList.Add(new ValidationError("cCRequestPGWDTO", "RequestDatetime", errorMsg));
                            keyValuePair = new KeyValuePair<TRX_STATUS_CHECK_RESPONSE, List<ValidationError>>(TRX_STATUS_CHECK_RESPONSE.SEND_STATUS_CHECK, validationErrorsList);
                            log.Info("Wait time for last payment has crossed, need to send status check request");
                        }
                        else
                        {
                            string errorMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2766, waitMinutes);
                            //Waiting for last card transaction response. Please attempt new payment after &1 minutes
                            validationErrorsList.Add(new ValidationError("cCRequestPGWDTO", "RequestDatetime", errorMsg));
                            keyValuePair = new KeyValuePair<TRX_STATUS_CHECK_RESPONSE, List<ValidationError>>(TRX_STATUS_CHECK_RESPONSE.WAIT, validationErrorsList);
                            log.Info("Wait for last card transaction response. " + waitMinutes.ToString());
                        }
                    }
                    else
                    {
                        transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                        transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        List<TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);
                        if (transactionPaymentsDTOs == null || transactionPaymentsDTOs.Count == 0)
                        {
                            // SendLastTransactionStatusCheckRequest(cCRequestPGWDTO, cCTransactionsPGWDTOList[0]);
                            keyValuePair = new KeyValuePair<TRX_STATUS_CHECK_RESPONSE, List<ValidationError>>(TRX_STATUS_CHECK_RESPONSE.SEND_STATUS_CHECK, validationErrorsList);
                            log.Info("Havent rececived payment, need to send status check request");
                        }
                        else
                        {
                            keyValuePair = new KeyValuePair<TRX_STATUS_CHECK_RESPONSE, List<ValidationError>>(TRX_STATUS_CHECK_RESPONSE.NO_ACTION, validationErrorsList);
                            log.Info("credit card transaction done from this POS is up to date.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(keyValuePair);
            return keyValuePair;

        }
        public virtual void SendLastTransactionStatusCheckRequest(CCRequestPGWDTO cCRequestPGWDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            log.LogMethodEntry(cCRequestPGWDTO, cCTransactionsPGWDTO);
            log.LogMethodExit();
        }
        /// <summary>
        /// -Ve ammount payment or refund without payment reference is not allowed
        /// </summary>
        internal void StandaloneRefundNotAllowed(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            if (transactionPaymentsDTO.Amount < 0)
            {
                log.LogMethodExit("Variable refund is not allowed.");
                throw new Exception(utilities.MessageUtils.getMessage("Variable refund is not allowed."));
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Print credit card receipt
        /// </summary>
        /// <param name="transactionPaymentDTOList"> Transaction payments dto list for priting the receipt</param>
        public virtual void PrintCCReceipt(List<TransactionPaymentsDTO> transactionPaymentDTOList)
        {
            log.LogMethodEntry(transactionPaymentDTOList);
            CCTransactionsPGWBL cCTransactionsPGWBL = null;
            foreach (TransactionPaymentsDTO transactionPaymentsDTO in transactionPaymentDTOList)
            {
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.CCResponseId != -1)
                {
                    cCTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    {
                        if (cCTransactionsPGWBL.CCTransactionsPGWDTO != null)
                        {
                            log.LogVariableState("CCTransactionsPGWDTO", cCTransactionsPGWBL.CCTransactionsPGWDTO);
                            try
                            {
                                if (!string.IsNullOrEmpty(cCTransactionsPGWBL.CCTransactionsPGWDTO.MerchantCopy) && ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PRINT_MERCHANT_RECEIPT").Equals("Y") && !isUnattended)
                                {
                                    log.Debug("Printing merchant copy");
                                    cCTransactionsPGWBL.CCTransactionsPGWDTO.MerchantCopy = cCTransactionsPGWBL.CCTransactionsPGWDTO.MerchantCopy.Replace("@invoiceNo", transactionPaymentsDTO.TransactionId.ToString());
                                    if (Print(cCTransactionsPGWBL.CCTransactionsPGWDTO.MerchantCopy))
                                    {
                                        log.Debug("Printing merchant copy is completed");
                                        cCTransactionsPGWBL.CCTransactionsPGWDTO.MerchantCopy = null;
                                    }
                                }
                                if (!string.IsNullOrEmpty(cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy) && ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PRINT_CUSTOMER_RECEIPT").Equals("Y"))
                                {
                                    log.Debug("Printing customer copy");
                                    cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy = cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy.Replace("@invoiceNo", transactionPaymentsDTO.TransactionId.ToString());
                                    if (Print(cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy))
                                    {
                                        log.Debug("Printing customer copy is completed");
                                        cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy = null;
                                    }
                                }
                                log.LogVariableState("Saving CCTransactionsPGWDTO", cCTransactionsPGWBL.CCTransactionsPGWDTO);
                                cCTransactionsPGWBL.Save();
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error in printing.", ex);
                            }

                        }
                    }
                    log.LogMethodExit();
                }
            }
        }




        /// <summary>
        /// Returns whether tip is allowed for the payment.
        /// </summary>
        /// <param name="transactionPaymentsDTO">transactionPaymentsDTO</param>
        /// <returns></returns>
        public virtual bool IsTipAllowed(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// Makes payment.
        /// </summary>
        /// <param name="transactionPaymentsDTO">TransactionPayments DTO</param>
        /// <returns></returns>
        public virtual TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            log.LogMethodExit(null);
            return null;
        }

        /// <summary>
        /// Reverts the payment.
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public virtual TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            log.LogMethodExit(null);
            return null;
        }

        /// <summary>
        /// Pays tip.
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public virtual TransactionPaymentsDTO PayTip(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            log.LogMethodExit(null);
            return null;
        }

        /// <summary>
        /// Performs settlement.
        /// </summary>
        /// <param name="transactionPaymentsDTO">transactionPaymentsDTO to be settled.</param>
        /// <param name="IsForcedSettlement"></param>
        /// <returns></returns>
        public virtual TransactionPaymentsDTO PerformSettlement(TransactionPaymentsDTO transactionPaymentsDTO, bool IsForcedSettlement = false)
        {
            log.LogMethodEntry(transactionPaymentsDTO, IsForcedSettlement);
            log.LogMethodExit(null);
            return null;
        }

        /// <summary>
        /// Checks if printing of last Transaction is supported
        /// This is used by PCEFTPOS Payment Gateway
        /// </summary>
        public virtual bool IsPrintLastTransactionSupported
        {
            get
            {
                return false;
            }

        }

        /// <summary>
        /// Checks whether the printer is required.
        /// This is used by QuestPaymentGateway in Kiosk
        /// </summary>
        public virtual bool IsPrinterRequired
        {
            get
            {
                return false;
            }

        }

        /// <summary>
        /// Checks whether Partial Payment is Approved .
        /// This is used by QuestPaymentGateway in Kiosk
        /// </summary>
        public virtual bool IsPartiallyApproved
        {
            get
            {
                return false;
            }

        }
        /// <summary>
        /// This indicates that the tip amount overriding is possible in the payment gateway or not.
        /// This is set to true in specific paymentgateway in which modification of tip is allowed
        /// </summary>
        public virtual bool IsTipAdjustmentAllowed
        {
            get
            {
                return false;
            }

        }

        /// <summary>
        /// Prints the last Transaction
        /// This been used by PCEFTPOS
        /// </summary> 
        public virtual void PrintLastTransaction()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// It checks for the begin Sale
        /// This been used by NCR PaymentGateway in Kiosk
        /// </summary> 
        public virtual void BeginOrder()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// It checks for the End Sale
        /// This been used by NCR PaymentGateway in Kiosk
        /// </summary> 
        public virtual void EndOrder()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
        }
        protected virtual void PrintCreditCardReceipt(TransactionPaymentsDTO transactionPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO)
        {
            log.LogMethodEntry();
            string customerCopy;
            string merchantCopy;
            customerCopy = GetReceiptText(transactionPaymentsDTO, ccTransactionsPGWDTO, false);
            merchantCopy = GetReceiptText(transactionPaymentsDTO, ccTransactionsPGWDTO, true);
            transactionPaymentsDTO.Memo = customerCopy;
            transactionPaymentsDTO.Memo += merchantCopy;

            if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
            {
                Print(customerCopy);
            }
            if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y" && !isUnattended)
            {
                Print(merchantCopy);
            }
            log.LogMethodExit();
        }
        protected virtual string GetReceiptText(TransactionPaymentsDTO trxPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO, bool IsMerchantCopy)
        {
            log.LogMethodEntry();
            try
            {
                string[] addressArray = utilities.ParafaitEnv.SiteAddress.Split(',');
                string receiptText = "";
                receiptText += CCGatewayUtils.AllignText(utilities.ParafaitEnv.SiteName, Alignment.Center);
                if (addressArray != null && addressArray.Length > 0)
                {
                    for (int i = 0; i < addressArray.Length; i++)
                    {
                        receiptText += Environment.NewLine + CCGatewayUtils.AllignText(addressArray[i] + ((i != addressArray.Length - 1) ? "," : "."), Alignment.Center);
                    }
                }
                receiptText += Environment.NewLine;
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Merchant ID") + ": ".PadLeft(6) + utilities.getParafaitDefaults("CREDIT_CARD_STORE_ID"), Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Transaction Date") + ": ".PadLeft(1) + ccTransactionsPGWDTO.TransactionDatetime.ToString("MMM dd yyyy HH:mm"), Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Transaction Type") + ": ".PadLeft(1) + ccTransactionsPGWDTO.TranCode, Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Invoice Number") + ": ".PadLeft(3) + trxPaymentsDTO.TransactionId, Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Transaction Amount") + " : " + Convert.ToDouble(trxPaymentsDTO.Amount).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Tip Amount") + " : ".PadLeft(7) + Convert.ToDouble(trxPaymentsDTO.TipAmount).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Total") + " : ".PadLeft(12) + ((Convert.ToDouble(ccTransactionsPGWDTO.Authorize) == 0) ? (trxPaymentsDTO.Amount + trxPaymentsDTO.TipAmount) : Convert.ToDouble(ccTransactionsPGWDTO.Authorize)).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                receiptText += Environment.NewLine;
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage((ccTransactionsPGWDTO.RecordNo.Equals("A")) ? "APPROVED" : (ccTransactionsPGWDTO.RecordNo.Equals("B")) ? "RETRY" : "DECLINED") + "-" + ccTransactionsPGWDTO.DSIXReturnCode, Alignment.Center);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage(ccTransactionsPGWDTO.TextResponse), Alignment.Center);

                receiptText += Environment.NewLine;
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.AuthCode))
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Authorization") + ": ".PadLeft(4) + ccTransactionsPGWDTO.AuthCode, Alignment.Left);
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.UserTraceData))
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Card Type") + ": ".PadLeft(10) + ccTransactionsPGWDTO.UserTraceData, Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Cardholder Name") + ": ".PadLeft(4) + trxPaymentsDTO.NameOnCreditCard, Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("PAN") + ": ".PadLeft(14) + trxPaymentsDTO.CreditCardNumber, Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Entry Mode") + ": ".PadLeft(7) + ccTransactionsPGWDTO.CaptureStatus, Alignment.Left);
                receiptText += Environment.NewLine;
                if (IsMerchantCopy)
                {
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Cardholder Signature"), Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText("_______________________", Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage(1180), Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText("**" + utilities.MessageUtils.getMessage("Merchant Copy") + "**", Alignment.Center);

                }
                else
                {
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("IMPORTANT— retain this copy for your records"), Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText("**" + utilities.MessageUtils.getMessage("Cardholder Copy") + " **", Alignment.Center);
                }

                receiptText += Environment.NewLine;
                receiptText += CCGatewayUtils.AllignText(" " + utilities.MessageUtils.getMessage("Thank You"), Alignment.Center);
                log.LogMethodExit(receiptText);
                return receiptText;
            }
            catch (Exception ex)
            {
                log.Fatal("GetReceiptText() failed to print receipt exception:" + ex.ToString());
                return null;
            }
        }
        public void Print(string printText, bool isMerchantCopy)
        {
            if (isMerchantCopy)
            {
                if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y" && !isUnattended)
                {
                    Print(printText);
                }
            }
            else
            {
                if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y" && PrintReceipt)
                {
                    Print(printText);
                }
            }
        }

        public bool Print(string printText)
        {
            log.LogMethodEntry(printText);
            bool status = false;
            try
            {
                using (System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument())
                {
                    pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize();//("Custom", 300, 700);
                    pd.PrintPage += (sender, e) =>
                    {
                        Font f = new Font("Arial", 9);
                        e.Graphics.DrawString(printText, f, new SolidBrush(Color.Black), new RectangleF(0, 0, pd.DefaultPageSettings.PrintableArea.Width, pd.DefaultPageSettings.PrintableArea.Height));
                    };
                    pd.Print();
                }
                status = true;
            }
            catch (Exception ex)
            {
                utilities.EventLog.logEvent("PaymentGateway", 'I', "Receipt print failed.", printText, this.GetType().Name, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                log.Error("Print receipt failed." + Environment.NewLine + printText, ex);
                status = false;
            }
            log.LogMethodExit(status);
            return status;
        }
        protected LookupValuesDTO GetLookupValues(string lookupName, string lookupValueName)
        {
            log.LogMethodEntry(lookupName, lookupValueName);
            LookupValuesDTO lookupValuesDTO = null;
            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, lookupValueName));
            lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (lookupValuesDTOList == null || (lookupValuesDTOList != null && lookupValuesDTOList.Count == 0))
            {
                lookupValuesDTO = new LookupValuesDTO();
            }
            else
            {
                lookupValuesDTO = lookupValuesDTOList[0];
            }
            log.LogMethodExit(lookupValuesDTO);
            return lookupValuesDTO;
        }

        protected void VerifyPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            if (paymentLinkEnabled && transactionPaymentsDTO != null && transactionPaymentsDTO.TransactionId > -1)
            {
                KeyValuePair<PaymentGateway.TRX_STATUS_CHECK_RESPONSE, List<ValidationError>> ValidationResponse = ValidateLastTransactionStatus(transactionPaymentsDTO.TransactionId);
                log.LogVariableState("ValidationResponse", ValidationResponse);
                if (ValidationResponse.Key == PaymentGateway.TRX_STATUS_CHECK_RESPONSE.WAIT)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, "Vaidation Error"), ValidationResponse.Value);
                }
                //else if (ValidationResponse.Key == PaymentGateway.TRX_STATUS_CHECK_RESPONSE.SEND_STATUS_CHECK)
                //{
                //    if (CheckLastTransactionStatus(transactionPaymentsDTO.TransactionId) == false)
                //    {
                //        string message = MessageContainerList.GetMessage(utilities.ExecutionContext, 2767);
                //        throw new Exception(message);
                //    }
                //}
            }
            log.LogMethodExit();
        }

        protected bool IsPaymentLinkEnbled()
        {
            log.LogMethodEntry();
            bool linkEnabled = false;
            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParam = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, PAYMENTLINKSETUP));
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, TRANSACTIONPAYMENTLINKURL));
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParam, null);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any() && string.IsNullOrWhiteSpace(lookupValuesDTOList[0].Description) == false)
            {
                linkEnabled = true;
            }
            log.LogMethodExit(linkEnabled);
            return linkEnabled;
        }
        /// <summary>
        /// Makes payment.
        /// </summary>
        /// <param name="transactionPaymentsDTO">TransactionPayments DTO</param>
        /// <returns></returns>
        public virtual TransactionPaymentsDTO MakePaymentForRecurringBilling(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            throw new NotImplementedException();
            log.LogMethodExit(null);
            return null;
        }

        /// <summary>
        /// GetCreditCardExpiryMonth
        /// </summary>
        /// <param name="cardExpiryData"></param>
        public virtual int GetCreditCardExpiryMonth(string cardExpiryData)
        {
            log.LogMethodEntry(cardExpiryData);
            throw new NotImplementedException();
            log.LogMethodExit(0);
            return 0;
        }
        /// <summary>
        /// GetCreditCardExpiryYear
        /// </summary>
        /// <param name="cardExpiryData"></param>
        public virtual int GetCreditCardExpiryYear(string cardExpiryData)
        {
            log.LogMethodEntry(cardExpiryData);
            throw new NotImplementedException();
            log.LogMethodExit(0);
            return 0;
        }
        /// <summary>
        /// GatewayComponentNeedsRestart
        /// </summary>
        /// <returns></returns>
        public virtual bool GatewayComponentNeedsRestart()
        {
            log.LogMethodEntry();
            bool needsRestart = false;
            log.LogMethodExit(needsRestart);
            return needsRestart;
        }
        /// <summary>
        /// RestartPaymentGatewayService
        /// </summary>
        /// <param name="forceRestart"></param>
        public virtual void RestartPaymentGatewayComponent(bool forceRestart = false)
        {
            log.LogMethodEntry(forceRestart);
            throw new NotImplementedException();
            log.LogMethodExit();
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        protected void RestartService(string serviceName, bool forceRestart = false)
        {
            log.LogMethodEntry(serviceName, forceRestart);
            using (ServiceController serviceController = new ServiceController(serviceName))
            {
                log.LogVariableState("serviceController-BeforeRestart", serviceController);
                if (forceRestart)
                {
                    try
                    {
                        if (serviceController.Status == ServiceControllerStatus.Stopped)
                        {
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4521, serviceName));
                            //Skipping Stop action for &1 as service is already stopped
                        }
                        else
                        {
                            serviceController.Stop();
                            serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    Thread.Sleep(10);
                    try
                    {
                        serviceController.Start();
                        serviceController.WaitForStatus(ServiceControllerStatus.Running);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4523, serviceName, ex.Message));
                        //Unable to restart service &1. Error: &2
                    }
                }
                else //restart only if service is not running
                {
                    if (serviceController.Status != ServiceControllerStatus.Running)
                    {
                        try
                        {
                            if (serviceController.Status == ServiceControllerStatus.Stopped)
                            {
                                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4521, serviceName));
                                //Skipping Stop action for &1 as service is already stopped
                            }
                            else
                            {
                                serviceController.Stop();
                                serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                        Thread.Sleep(10);
                        try
                        {
                            serviceController.Start();
                            serviceController.WaitForStatus(ServiceControllerStatus.Running);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4523, serviceName, ex.Message));
                            //Unable to restart service &1. Error: &2
                        }
                    }
                }
                serviceController.Refresh();
                log.LogVariableState("serviceController-AfterRestart", serviceController);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Settle Transaction Payment
        /// </summary>
        public virtual TransactionPaymentsDTO SettleTransactionPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            log.LogMethodExit(null);
            return null;
        }

        /// <summary>
        /// Can Adjust Transaction Payment
        /// </summary>
        public virtual void CanAdjustTransactionPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            log.LogMethodExit();
        }
    }
    
    /// <summary>
    /// Represents payment gateway error that occur during application execution. 
    /// </summary>
    public class PaymentGatewayException : Exception
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor of PaymentGatewayException.
        /// </summary>
        public PaymentGatewayException()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Initializes a new instance of PaymentGatewayException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public PaymentGatewayException(string message)
        : base(message)
        {
            log.LogMethodEntry(message);
            log.LogMethodExit(null);
        }
        /// <summary>
        /// Initializes a new instance of PaymentGatewayException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public PaymentGatewayException(string message, Exception inner)
        : base(message, inner)
        {
            log.LogMethodEntry(message, inner);
            log.LogMethodExit(null);
        }
    }

    /// <summary>
    /// Represents payment gateway error that occur during application execution. 
    /// </summary>
    public class PGDuplicatePaymentException : Exception
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor of PaymentGatewayException.
        /// </summary>
        public PGDuplicatePaymentException()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Initializes a new instance of PaymentGatewayException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public PGDuplicatePaymentException(string message)
        : base(message)
        {
            log.LogMethodEntry(message);
            log.LogMethodExit(null);
        }
        /// <summary>
        /// Initializes a new instance of PaymentGatewayException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public PGDuplicatePaymentException(string message, Exception inner)
        : base(message, inner)
        {
            log.LogMethodEntry(message, inner);
            log.LogMethodExit(null);
        }
    }
}
