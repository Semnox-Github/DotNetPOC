/**********************************************************************************************************************************
 * Project Name - TransactionPaymentLink
 * Description  - Business Logic to send payment link
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 **********************************************************************************************************************************                                        
 *2.100.0     13-Jul-2020      Guru S A        Created for payment link changes
 *2.100.0     02-Nov-2020      Girish Kundar   Modified : Added validation for creating payment link to check reversed transaction
 *2.110.0     08-Dec-2020      Guru S A        Subscription changes
 *2.130.7     13-Apr-2022      Guru S A        Payment mode OTP validation changes
 **********************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TransactionPaymentLink class is used to generate and save the payment link for sale process
    /// </summary>
    public class TransactionPaymentLink
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private Transaction transaction;
        private Utilities utilities;
        private const string TRANSACTIONPAYMENTLINKTEMPLATE = "TRANSACTION_PAYMENT_LINK_TEMPLATE";
        private const string PAYMENTLINKSETUP = "PAYMENT_LINK_SETUP";
        private const string TRANSACTIONPAYMENTLINKURL = "ONLINE_PAYMENT_LINK_URL";
        private const string RECEIPTPRINTREMARKS = "PAYMENT_LINK_RECEIPT_PRINT_REMARKS";
        private TransactionEventContactsDTO transactionEventContactsDTO;
       
        /// <summary>
        /// MessageChannel - EMAIL or SMS
        /// </summary>
        public enum MessageChannel
        {
            /// <summary>
            /// Send via Email
            /// </summary>
            EMAIL,
            /// <summary>
            /// Send via SMS
            /// </summary>
            SMS
        }
        private MessageChannel messageChannel;

        /// <summary>
        /// Constructor with transactionPaymentLinkDTO 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="transactionEventContactsDTO"></param>
        /// <param name="utilities"></param>
        public TransactionPaymentLink(ExecutionContext executionContext, Utilities utilities, TransactionEventContactsDTO transactionEventContactsDTO)
        {
            log.LogMethodEntry(executionContext, transactionEventContactsDTO, utilities);
            this.executionContext = executionContext;
            this.transactionEventContactsDTO = transactionEventContactsDTO;
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            this.messageChannel = transactionEventContactsDTO.MessageChannel;
            this.transaction = transactionUtils.CreateTransactionFromDB(transactionEventContactsDTO.TransactionId, utilities);
            this.utilities = utilities;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parmaeterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utilities"></param>
        /// <param name="transaction"></param>
        public TransactionPaymentLink(ExecutionContext executionContext, Utilities utilities, Transaction transaction)
        {
            log.LogMethodEntry(executionContext, transaction, utilities);
            this.executionContext = executionContext;
            this.transaction = transaction;
            this.utilities = utilities;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parmaeterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utilities"></param>
        /// <param name="transactionId"></param>
        public TransactionPaymentLink(ExecutionContext executionContext, Utilities utilities, int transactionId)
        {
            log.LogMethodEntry(executionContext, transactionId, utilities);
            this.executionContext = executionContext;
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            this.transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
            this.utilities = utilities;
            log.LogMethodExit();
        }

        /// <summary>
        /// Send Payment Link
        /// </summary>
        /// <param name="messageChannel"></param>
        /// <param name="sqlTransaction"></param>
        public void SendPaymentLink(MessageChannel messageChannel, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(messageChannel, sqlTransaction);
            this.messageChannel = messageChannel;
            switch (messageChannel)
            {
                case MessageChannel.EMAIL:
                    SendPaymentLinkEmail(sqlTransaction);
                    break;
                default:
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 2763)); //"Invalid message channel"
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// GeneratePaymentLink 
        /// </summary> 
        public string GeneratePaymentLink()
        {
            log.LogMethodEntry();
            string urlLink = string.Empty;
            if (transaction != null)
            {
                string subURLLink = GetPaymentLinkURL();
                List<ValidationError> validationErrorList = CanSendPaymentLinkEmail();
                if (validationErrorList != null && validationErrorList.Any())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
                }
                try
                {
                    string securityToken = transaction.GenerateTransactionBasedToken();
                    if (string.IsNullOrEmpty(securityToken) || string.IsNullOrEmpty(securityToken.Trim()))
                    {
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 2746));// "Sorry unable to generate token for payment link URL"
                    }
                    urlLink = subURLLink + securityToken;

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw;
                }
            }
            else
            {
                log.Info("Transaction object is not available");
                throw new Exception(MessageContainerList.GetMessage(executionContext, 2154, MessageContainerList.GetMessage(executionContext, "Transaction")));
            }
            log.LogMethodExit(urlLink);
            return urlLink;
        }

        /// <summary>
        /// ISPaymentLinkEnabled - Checks for enabling and disabling the payment link in UI
        /// </summary>
        /// <param name="machineContext"></param>
        /// <returns></returns>
        public static bool ISPaymentLinkEnbled(ExecutionContext machineContext)
        {
            log.LogMethodEntry(machineContext);
            bool linkEnabled = false;
            string subURLLinkValue = string.Empty;
            LookupValuesList lookupValuesList = new LookupValuesList(machineContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParam = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, PAYMENTLINKSETUP));
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, TRANSACTIONPAYMENTLINKURL));
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineContext.GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParam, null);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any() && string.IsNullOrWhiteSpace(lookupValuesDTOList[0].Description) == false)
            {
                linkEnabled = true;
            }
            log.LogMethodExit(linkEnabled);
            return linkEnabled;
        }

        /// <summary>
        /// Get Payment link Email Template DTO
        /// </summary> 
        /// <returns></returns>
        public static EmailTemplateDTO GetPaymentLinkEmailTemplateDTO(ExecutionContext machineContext)
        {
            log.LogMethodEntry();
            EmailTemplateDTO emailTemplateDTO = null;
            string emailTemplate = string.Empty;
            emailTemplate = ParafaitDefaultContainerList.GetParafaitDefault(machineContext, TRANSACTIONPAYMENTLINKTEMPLATE);
            if (String.IsNullOrWhiteSpace(emailTemplate) == false)
            {
                emailTemplateDTO = new EmailTemplate(machineContext).GetEmailTemplate(emailTemplate, machineContext.GetSiteId());
            }
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;
        }

        private string GetPaymentLinkURL()
        {
            log.LogMethodEntry();
            string subURLLinkValue = string.Empty;
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParam = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, PAYMENTLINKSETUP));
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, TRANSACTIONPAYMENTLINKURL));
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParam, null);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
            {
                subURLLinkValue = lookupValuesDTOList[0].Description;
            }
            if (string.IsNullOrWhiteSpace(subURLLinkValue))
            {
                string missingParamName = MessageContainerList.GetMessage(executionContext, TRANSACTIONPAYMENTLINKURL) + " " + MessageContainerList.GetMessage(executionContext, "parameter");
                throw new Exception(MessageContainerList.GetMessage(executionContext, 2753, missingParamName));//&1 details is missing in Payment line URL setup
            }
            log.LogMethodExit(subURLLinkValue);
            return subURLLinkValue;
        }


        private List<ValidationError> CanSendPaymentLinkEmail()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (transaction != null)
            {
                if (transaction.Trx_id <= 0)
                {
                    validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Payment"), MessageContainerList.GetMessage(executionContext, "Link"),
                                                                MessageContainerList.GetMessage(executionContext, 2656)));//"Please save the Transaction first"
                }

                if (transaction.Status == Transaction.TrxStatus.CANCELLED
                    || transaction.Status == Transaction.TrxStatus.SYSTEMABANDONED)
                {
                    validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Payment"), MessageContainerList.GetMessage(executionContext, "Link"),
                                                                MessageContainerList.GetMessage(executionContext, 2658, MessageContainerList.GetMessage(executionContext, "Transaction"),
                                                               transaction.Status)));//"Sorry unable to proceed. &1 is in &2 status"
                }
                else if (transaction.IsReversedTransaction())
                {
                    validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Payment"), MessageContainerList.GetMessage(executionContext, "Transaction"),
                                                     MessageContainerList.GetMessage(executionContext, 2858)));

                }
                EmailTemplateDTO emailTemplateDTO = GetPaymentLinkEmailTemplateDTO(executionContext);
                if (emailTemplateDTO == null || emailTemplateDTO.EmailTemplateId == -1)
                {
                    validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Payment"), MessageContainerList.GetMessage(executionContext, "Email"),
                                                     MessageContainerList.GetMessage(executionContext, 2744)));//'Email template to send payment link is not defined'
                }
            }
            else
            {
                log.Info("Transaction object is not available");
                throw new Exception(MessageContainerList.GetMessage(executionContext, 2154, MessageContainerList.GetMessage(executionContext, "Transaction")));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }


        /// <summary>
        /// Checks whether the transaction has the customer details
        /// </summary>
        /// <returns></returns>
        private bool IsTransactionCustomerHasContactInfo(MessageChannel messageChannel)
        {
            log.LogMethodEntry(messageChannel);
            bool trxHasCustomer = false;
            try
            {
                switch (messageChannel)
                {
                    case MessageChannel.EMAIL:
                        {
                            string email = TransactionEventsBL.GetCustomerEmailId(executionContext, transaction, transactionEventContactsDTO);
                            if (string.IsNullOrWhiteSpace(email) == false)
                            {
                                trxHasCustomer = true;
                            }
                        }
                        break;
                    default:
                        {
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 2763));
                        }
                }
                log.LogMethodExit(trxHasCustomer);
                return trxHasCustomer;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        //Email /SMS  separate method 

        private void SendPaymentLinkEmail(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (transaction != null)
            {
                List<ValidationError> validationErrorList = CanSendPaymentLinkEmail();
                if (validationErrorList != null && validationErrorList.Any())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
                }
                if (IsTransactionCustomerHasContactInfo(MessageChannel.EMAIL))
                {
                    try
                    {
                        bool reservationTransaction = transaction.IsReservationTransaction(sqlTransaction);
                        TransactionEventsBL transactionEventsBL = new TransactionEventsBL(executionContext, utilities, ParafaitFunctionEvents.PAYMENT_LINK_EVENT, transaction, transactionEventContactsDTO, null, sqlTransaction);
                        transactionEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.NONE, null, sqlTransaction);

                        //EmailTemplateDTO emailTemplateDTO = GetPaymentLinkEmailTemplateDTO(executionContext);
                        //string emailContent = string.Empty;
                        //if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > 0)
                        //{
                        //    emailContent = emailTemplateDTO.EmailTemplate;
                        //    TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(executionContext, utilities, emailTemplateDTO.EmailTemplateId, transaction);
                        //    emailContent = transactionEmailTemplatePrint.GenerateEmailTemplateContent();
                        //}
                        //else
                        //{
                        //    throw new Exception(MessageContainerList.GetMessage(executionContext, 2744));//'Email template to send payment link is not defined'
                        //}
                        //int messagingClientId = -1;
                        //MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(executionContext);
                        //List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", "PURCHASE", "E");
                        //if (messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
                        //{
                        //    messagingClientId = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;
                        //}
                        //// Get all the email addresses - 
                        //string emailAddress = GetPaymentLinkCustomerEmailId(executionContext, transaction, transactionPaymentLinkDTO);
                        //string[] emailAddressList = emailAddress.Split(',');
                        //foreach (string emailId in emailAddressList)
                        //{
                        //    MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, "Send Payment Link Email", "E", emailId, "", "", null, null, null, null,
                        //          emailTemplateDTO.Description, emailContent, -1, null, "", true, "", "", messagingClientId, false, "");
                        //    MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                        //    messagingRequestBL.Save(sqlTransaction);
                        //}

                        if (reservationTransaction)
                        {
                            // Get all the email addresses - 
                            string emailAddress = TransactionEventsBL.GetCustomerEmailId(executionContext, transaction, transactionEventContactsDTO);
                            object bookingGuId = null;
                            ReservationListBL reservationListBL = new ReservationListBL(executionContext);
                            List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ReservationDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.TRX_ID, transaction.Trx_id.ToString()));
                            searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            List<ReservationDTO> reservationDTOList = reservationListBL.GetReservationDTOList(searchParameters, sqlTransaction);
                            if (reservationDTOList != null && reservationDTOList.Any())
                            {
                                bookingGuId = reservationDTOList[0].Guid;
                            }
                            if (bookingGuId == null)
                            {
                                throw new Exception(MessageContainerList.GetMessage(executionContext, 2650));//Unable to fetch booking details for the transaction
                            }
                            Core.GenericUtilities.EventLog audit = new Core.GenericUtilities.EventLog(utilities.ExecutionContext);
                            audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"),
                                                                      'D', executionContext.GetUserId(),
                                                                      MessageContainerList.GetMessage(executionContext, 2745, emailAddress),//Payment link email is sent to &1
                                                                      MessageContainerList.GetMessage(executionContext, "Reservation"),
                                                                      0, "", bookingGuId.ToString(), sqlTransaction);

                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        throw;
                    }
                }
                else
                {
                    log.LogMethodExit("No valid email id found to send payment link mail");
                    return;
                }
            }
            else
            {
                log.Info("Transaction object is not available");
                throw new Exception(MessageContainerList.GetMessage(executionContext, 2154, MessageContainerList.GetMessage(executionContext, "Transaction")));
            }
            log.LogMethodExit();
        }

        
        public static string GetPaymentLinkPrintRemarks(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            string receiptPrintRemarks = string.Empty;
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParam = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, PAYMENTLINKSETUP));
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, RECEIPTPRINTREMARKS));
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParam, null);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
            {
                receiptPrintRemarks = lookupValuesDTOList[0].Description;
            } 
            log.LogMethodExit(receiptPrintRemarks);
            return receiptPrintRemarks;
        }
    }
}
