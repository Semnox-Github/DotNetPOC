/********************************************************************************************
 * Project Name - TransactionEventsBL 
 * Description  -BL class of the Transaction function Events
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     12-Dec-2020      Guru S A          Created for Subscription changes                                                                               
 *2.110.0     24-Dec-2021      Girish            Barcode image issue in email template
 *2.130.5     14-Mar-2022      Girish            Modified : Added new column WBPrinterModel to printer table
 *2.130.7     13-Apr-2022      Guru S A          Payment mode OTP validation changes
 *2.140.2     18-Apr-2022      Girish Kundar     Modified:  BOCA changes - Added new column WBModel to printer class
 *2.150.1     22-Feb-2023      Guru S A          Kiosk Cart Enhancements
 *2.150.3.0   28-Apr-2023      Vignesh Bhat      Modified for TableTop Kiosk Changes
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Printer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TransactionEventsBL
    /// </summary>
    public class TransactionEventsBL : ParafaitFunctionEventBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private readonly ExecutionContext executionContext;
        /// <summary>
        /// transaction
        /// </summary>
        protected Transaction transaction;
        /// <summary>
        /// transactionPaymentLinkDTO
        /// </summary>
        protected TransactionEventContactsDTO transactionEventContactsDTO;
        private int? executedCardCount;
        /// <summary>
        /// utilities
        /// </summary>
        protected Utilities utilities;
        private readonly List<ParafaitFunctionEvents> transactionFunctionEventList = new ParafaitFunctionEvents[]{
            ParafaitFunctionEvents.PURCHASE_EVENT,
            ParafaitFunctionEvents.PAYMENT_LINK_EVENT,
            ParafaitFunctionEvents.EXECUTE_ONLINE_TRANSACTION_EVENT,
            ParafaitFunctionEvents.PURCHASE_MESSAGE_TRIGGER_EVENT,
            ParafaitFunctionEvents.PAYMENT_MODE_OTP_EVENT,
            ParafaitFunctionEvents.PAYMENT_REFUND_EVENT,
            ParafaitFunctionEvents.RESERVATION_PURCHASE_EVENT,
            ParafaitFunctionEvents.REDEEM_TOKEN_TRANSACTION_EVENT,
            ParafaitFunctionEvents.ABORT_REDEEM_TOKEN_TRANSACTION_EVENT,
            ParafaitFunctionEvents.ABORT_TRANSACTION_EVENT,
            ParafaitFunctionEvents.KIOSK_CARD_DISPENSER_ERROR_EVENT,
            ParafaitFunctionEvents.KiOSK_WRISTBAND_PRINT_ERROR
        }.ToList();
        private const string PURCHASE_MSG_REF = "Purchase";
        private const string PAYMENT_LINK_MSG_REF = "Send Payment Link";
        private const string EXECUTE_ONLINE_TRANSACTION_MSG_REF = "Execute Online Transaction";
        private const string PURCHASE_MESSAGE_TRIGGERD_MSG_REF = "Purchase Trigger";
        private const string PAYMENT_MODE_OTP_EVENT_MSG_REF = "Payment Mode OTP Validation";
        private const string REDEEM_TOKEN_TRANSACTION_REF = "Redeem Token";
        private const string ABORT_REDEEM_TOKEN_TRANSACTION_REF = "Abort Redeem Token";
        private const string ABORT_TRANSACTION_REF = "Abort Transaction";
        private const string KIOSK_CARD_DISPENSER_ERROR_REF = "Card Dispenser Error In Kiosk";
        private const string KiOSK_WRISTBAND_PRINT_ERROR_REF = "Wristband Print Error in Kiosk";

        private const string TRANSACTIONPAYMENTLINKTEMPLATE = "TRANSACTION_PAYMENT_LINK_TEMPLATE";
        private const string ONLINE_RECEIPT_EMAIL_TEMPLATE = "ONLINE_RECEIPT_EMAIL_TEMPLATE";
        private const string ONLINE_TICKETS_SMS_TEXT_TEMPLATE = "ONLINE_TICKETS_SMS_TEXT_TEMPLATE";
        private const string CARD_PRINT_ERROR_RECEIPT_TEMPLATE = "CARD_PRINT_ERROR_RECEIPT_TEMPLATE";
        private const string CARD_DISPENSER_ERROR_RECEIPT_TEMPLATE = "CARD_DISPENSER_ERROR_RECEIPT_TEMPLATE";
        private string NUMBER_FORMAT = string.Empty;
        private string AMOUNT_WITH_CURRENCY_SYMBOL = string.Empty;
        private const string IMAGE_DIRECTORY = "IMAGE_DIRECTORY";
        private const string ONLINE_RESERVATION_EMAIL_TEMPLATE = "ONLINE_RESERVATION_EMAIL_TEMPLATE";
        protected List<KeyValuePair<string, string>> customerContacts = null;
        /// <summary>
        /// TransactionEventsBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utilities"></param>
        /// <param name="parafaitFunctionEvents"></param>
        /// <param name="transaction"></param>
        /// <param name="transactionEventContactsDTO"></param>
        /// <param name="executedCardCount"></param>
        /// <param name="sqlTransaction"></param>
        public TransactionEventsBL(ExecutionContext executionContext, Utilities utilities, ParafaitFunctionEvents parafaitFunctionEvents, Transaction transaction,
            TransactionEventContactsDTO transactionEventContactsDTO = null, int? executedCardCount = null, SqlTransaction sqlTransaction = null)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext, utilities, parafaitFunctionEvents, transaction, transactionEventContactsDTO, executedCardCount, sqlTransaction);
            this.transaction = transaction;
            this.utilities = utilities;
            this.transactionEventContactsDTO = transactionEventContactsDTO;
            this.executedCardCount = executedCardCount;
            this.NUMBER_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");
            this.AMOUNT_WITH_CURRENCY_SYMBOL = utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL;
            ValidTransactionFunctionEvent(parafaitFunctionEvents);
            LoadParafaitFunctionEventDTO(parafaitFunctionEvents, sqlTransaction);

            log.LogMethodExit();
        }

        private void ValidTransactionFunctionEvent(ParafaitFunctionEvents parafaitFunctionEvents)
        {
            log.LogMethodEntry(parafaitFunctionEvents);
            if (transactionFunctionEventList.Contains(parafaitFunctionEvents) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2883, parafaitFunctionEvents));//"&1 is not a valid transaction event
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// MessageSubjectFormatter
        /// </summary>
        /// <param name="messageTemplateSubject"></param>
        /// <returns></returns>
        public override string MessageSubjectFormatter(string messageTemplateSubject)
        {
            log.LogMethodEntry(messageTemplateSubject);
            string messageSubjectContent = string.Empty;
            if (string.IsNullOrWhiteSpace(messageTemplateSubject) == false && messageTemplateSubject.Contains("@"))
            {
                TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(executionContext, utilities, -1, transaction, null, true);
                messageSubjectContent = transactionEmailTemplatePrint.BuildContent(messageTemplateSubject, true);
            }
            else
            {
                messageSubjectContent = messageTemplateSubject;
            }
            log.LogMethodExit(messageSubjectContent);
            return messageSubjectContent;
        }
        /// <summary>
        /// MessageBodyFormatter
        /// </summary>
        /// <param name="messageTemplateContent"></param>
        /// <returns></returns>
        public override string MessageBodyFormatter(string messageTemplateContent)
        {
            log.LogMethodEntry(messageTemplateContent);
            string messageBodyContent = string.Empty;
            TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(executionContext, utilities, -1, transaction, null);
            messageBodyContent = transactionEmailTemplatePrint.BuildContent(messageTemplateContent,true);
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.EXECUTE_ONLINE_TRANSACTION_EVENT)
            {
                messageBodyContent = messageBodyContent.Replace("@TodaysIssuedCards", (executedCardCount == null
                                                                                         ? String.Empty : ((int)executedCardCount).ToString(NUMBER_FORMAT)));
            }
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.PAYMENT_MODE_OTP_EVENT)
            {
                messageBodyContent = messageBodyContent.Replace("@PaymentModeOTP", (transactionEventContactsDTO == null ? String.Empty : transactionEventContactsDTO.OTPValue));
                messageBodyContent = messageBodyContent.Replace("@OTPGameCard", (transactionEventContactsDTO == null ? String.Empty : transactionEventContactsDTO.OTPGameCard));
            }
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.REDEEM_TOKEN_TRANSACTION_EVENT
                || this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.ABORT_REDEEM_TOKEN_TRANSACTION_EVENT)
            {
                 
                int count = (transaction != null && transaction.TransactionPaymentsDTOList != null ? transaction.TransactionPaymentsDTOList.Count : 0);
                messageBodyContent = messageBodyContent.Replace("@TotalTokenInserted", (count == 0
                                                                                         ? "0" : count.ToString(NUMBER_FORMAT)));
                double totalPaymentReceived = 0;
                double cashPaymentReceived = 0;
                double creditCardPaymentReceived = 0;
                if (transaction != null && transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                {
                    for (int i = 0; i < transaction.TransactionPaymentsDTOList.Count; i++)
                    {
                        TransactionPaymentsDTO trxPaymentDTO = transaction.TransactionPaymentsDTOList[i];
                        totalPaymentReceived = totalPaymentReceived + trxPaymentDTO.Amount
                                               + (trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard
                                                   ? trxPaymentDTO.PaymentUsedCreditPlus : 0);
                        if (trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsCash)
                        {
                            cashPaymentReceived = cashPaymentReceived + trxPaymentDTO.Amount;
                        }
                        if (trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsCreditCard)
                        {
                            creditCardPaymentReceived = creditCardPaymentReceived + trxPaymentDTO.Amount;
                        }
                    }
                }
                messageBodyContent = messageBodyContent.Replace("@TotalPointsLoaded", (totalPaymentReceived == 0
                                                                                        ? "0" : totalPaymentReceived.ToString(NUMBER_FORMAT)));
                messageBodyContent = messageBodyContent.Replace("@TotalAmount",
                    (totalPaymentReceived == 0 ? "0" : totalPaymentReceived.ToString(AMOUNT_WITH_CURRENCY_SYMBOL)));
                messageBodyContent = messageBodyContent.Replace("@CashAmount",
                    (cashPaymentReceived == 0 ? "0" : cashPaymentReceived.ToString(AMOUNT_WITH_CURRENCY_SYMBOL)));
                messageBodyContent = messageBodyContent.Replace("@CreditCardAmount",
                    (creditCardPaymentReceived == 0 ? "0" : creditCardPaymentReceived.ToString(AMOUNT_WITH_CURRENCY_SYMBOL)));
            }
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.ABORT_TRANSACTION_EVENT)
            {                
                double totalPaymentReceived = 0;
                double cashPaymentReceived = 0;
                double creditCardPaymentReceived = 0;
                if (transaction != null && transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                {
                    for (int i = 0; i < transaction.TransactionPaymentsDTOList.Count; i++)
                    {
                        TransactionPaymentsDTO trxPaymentDTO = transaction.TransactionPaymentsDTOList[i];
                        totalPaymentReceived = totalPaymentReceived + trxPaymentDTO.Amount
                                               + (trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsDebitCard
                                                   ? trxPaymentDTO.PaymentUsedCreditPlus : 0);
                        if (trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsCash)
                        {
                            cashPaymentReceived = cashPaymentReceived + trxPaymentDTO.Amount;
                        }
                        if (trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsCreditCard)
                        {
                            creditCardPaymentReceived = creditCardPaymentReceived + trxPaymentDTO.Amount;
                        }
                    }
                }
                messageBodyContent = messageBodyContent.Replace("@TotalAmount", 
                    (totalPaymentReceived == 0 ? "0" : totalPaymentReceived.ToString(AMOUNT_WITH_CURRENCY_SYMBOL))); 
                messageBodyContent = messageBodyContent.Replace("@CashAmount", 
                    (cashPaymentReceived == 0 ? "0" : cashPaymentReceived.ToString(AMOUNT_WITH_CURRENCY_SYMBOL)));
                messageBodyContent = messageBodyContent.Replace("@CreditCardAmount",
                    (creditCardPaymentReceived == 0 ? "0" : creditCardPaymentReceived.ToString(AMOUNT_WITH_CURRENCY_SYMBOL)));
            }

            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.KIOSK_CARD_DISPENSER_ERROR_EVENT)
            { 

            }
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.KiOSK_WRISTBAND_PRINT_ERROR)
            {
                 
            }
            log.LogMethodExit(messageBodyContent);
            return messageBodyContent;
        }

        /// <summary>
        /// AppNotificationMessageBodyFormatter
        /// </summary>
        /// <param name="messagingChanelType"></param>
        /// <param name="messageSubject"></param>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        protected override string AppNotificationMessageBodyFormatter(MessagingClientDTO.MessagingChanelType messagingChanelType, string messageSubject, string messageBody)
        {
            log.LogMethodEntry(messagingChanelType, messageSubject, messageBody);
            if (messagingChanelType == MessagingClientDTO.MessagingChanelType.APP_NOTIFICATION)
            {
                AppNotificationContentBuilder messagingTriggerAppNotificationContentBuilder = new AppNotificationContentBuilder(executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName);
                AppNotificationContentBuilder.AppNotificationType appNotificationType = AppNotificationContentBuilder.AppNotificationType.NONE;
                string transactionId = (this.transaction.Trx_id > 0? this.transaction.Trx_id.ToString(): "");
                messageBody = messagingTriggerAppNotificationContentBuilder.FormatAppNotificationContent(messageSubject, messageBody, appNotificationType, null, transactionId, null);
            }
            log.LogMethodExit(messageBody);
            return messageBody;
        }
        /// <summary>
        /// SendMessage
        /// </summary>
        /// <param name="messagingChanelType"></param>
        /// <param name="sqlTrx"></param>
        public virtual void SendMessage(MessagingClientDTO.MessagingChanelType messagingChanelType, List<KeyValuePair<string, string>> customerContacts = null, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(messagingChanelType, sqlTrx);
            this.customerContacts = customerContacts;
            if (parafaitFunctionEventDTO != null && parafaitFunctionEventDTO.MessagingClientFunctionLookUpDTOList != null
                && parafaitFunctionEventDTO.MessagingClientFunctionLookUpDTOList.Any()
                && (messagingChanelType == MessagingClientDTO.MessagingChanelType.NONE
                    || (messagingChanelType != MessagingClientDTO.MessagingChanelType.NONE
                         && parafaitFunctionEventDTO.MessagingClientFunctionLookUpDTOList.Exists(mcfl => messagingChanelType == MessagingClientDTO.SourceEnumFromString(mcfl.MessageType)))))
            {
                foreach (MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO in parafaitFunctionEventDTO.MessagingClientFunctionLookUpDTOList)
                {
                    BuildAndSendMessage(messagingChanelType, messagingClientFunctionLookUpDTO, sqlTrx);
                }
            }
            else
            {
                //When MessagingClientFunctionLookUp is not defined  
                if (parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.PURCHASE_EVENT || parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.RESERVATION_PURCHASE_EVENT)
                {
                    SendEMail(sqlTrx);
                    SendSMS(sqlTrx);
                }
                if (parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.PAYMENT_LINK_EVENT ||
                    parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.EXECUTE_ONLINE_TRANSACTION_EVENT ||
                    parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.REDEEM_TOKEN_TRANSACTION_EVENT ||
                    parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.ABORT_REDEEM_TOKEN_TRANSACTION_EVENT ||
                    parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.ABORT_TRANSACTION_EVENT ||
                    parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.KIOSK_CARD_DISPENSER_ERROR_EVENT ||
                    parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.KiOSK_WRISTBAND_PRINT_ERROR)
                {
                    SendEMail(sqlTrx);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// SaveMessagingRequestDTO
        /// </summary>
        /// <param name="messageSubject"></param>
        /// <param name="messageBody"></param>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <param name="sqlTrx"></param>
        protected override void SaveMessagingRequestDTO(string messageSubject, string messageBody, MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messageSubject, messageBody, messagingClientFunctionLookUpDTO, sqlTrx);
            string messageReference = GetMessageReference();
            int messagingClientId = messagingClientFunctionLookUpDTO.MessageClientId > -1 && messagingClientFunctionLookUpDTO.MessagingClientDTO != null
                                         ? messagingClientFunctionLookUpDTO.MessagingClientDTO.ClientId : messagingClientFunctionLookUpDTO.MessageClientId;
            string messageType = messagingClientFunctionLookUpDTO.MessageType;
            string toEmails = GetToEmails(messagingClientFunctionLookUpDTO);
            string toMobile = GetToMobileNumber(messagingClientFunctionLookUpDTO);
            int? cardId = GetCardId();
            string attachFile = GetAttachFile(messagingClientFunctionLookUpDTO);
            string toDevice = GetToDevice(messagingClientFunctionLookUpDTO);
            int customerId = GetCustomerId(messagingClientFunctionLookUpDTO);
            CreateMessagingRequest(messageSubject, messageBody, messagingClientFunctionLookUpDTO, messageReference, messagingClientId, messageType, toEmails, toMobile, cardId, customerId, attachFile, toDevice, sqlTrx, null, transaction.Trx_id, transaction.Trx_No);
            log.LogMethodExit();
        }
        private string GetMessageReference()
        {
            log.LogMethodEntry();
            string messageReference = string.Empty;
            switch (this.parafaitFunctionEventDTO.ParafaitFunctionEventName)
            {
                case ParafaitFunctionEvents.PURCHASE_EVENT:
                case ParafaitFunctionEvents.RESERVATION_PURCHASE_EVENT:
                    messageReference = PURCHASE_MSG_REF;
                    break;
                case ParafaitFunctionEvents.PAYMENT_LINK_EVENT:
                    messageReference = PAYMENT_LINK_MSG_REF;
                    break;
                case ParafaitFunctionEvents.EXECUTE_ONLINE_TRANSACTION_EVENT:
                    messageReference = EXECUTE_ONLINE_TRANSACTION_MSG_REF;
                    break;
                case ParafaitFunctionEvents.PURCHASE_MESSAGE_TRIGGER_EVENT:
                    messageReference = PURCHASE_MESSAGE_TRIGGERD_MSG_REF;
                    break;
                case ParafaitFunctionEvents.PAYMENT_MODE_OTP_EVENT:
                    messageReference = PAYMENT_MODE_OTP_EVENT_MSG_REF;
                    break;
                case ParafaitFunctionEvents.REDEEM_TOKEN_TRANSACTION_EVENT:
                    messageReference = REDEEM_TOKEN_TRANSACTION_REF;
                    break;
                case ParafaitFunctionEvents.ABORT_REDEEM_TOKEN_TRANSACTION_EVENT:
                    messageReference = ABORT_REDEEM_TOKEN_TRANSACTION_REF;
                    break;
                case ParafaitFunctionEvents.ABORT_TRANSACTION_EVENT:
                    messageReference = ABORT_TRANSACTION_REF;
                    break;
                case ParafaitFunctionEvents.KIOSK_CARD_DISPENSER_ERROR_EVENT:
                    messageReference = KIOSK_CARD_DISPENSER_ERROR_REF;
                    break;
                case ParafaitFunctionEvents.KiOSK_WRISTBAND_PRINT_ERROR:
                    messageReference = KiOSK_WRISTBAND_PRINT_ERROR_REF;
                    break;
                default:
                    break;
            }
            log.LogMethodExit(messageReference);
            return messageReference;
        }
        /// <summary>
        /// GetToEmails
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <returns></returns>
        protected override string GetToEmails(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO);
            string toEmails = string.Empty;
            if (MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.EMAIL)
            {
                switch (this.parafaitFunctionEventDTO.ParafaitFunctionEventName)
                {
                    case ParafaitFunctionEvents.PURCHASE_EVENT:
                    case ParafaitFunctionEvents.PAYMENT_REFUND_EVENT:
                    case ParafaitFunctionEvents.RESERVATION_PURCHASE_EVENT:
                    case ParafaitFunctionEvents.REDEEM_TOKEN_TRANSACTION_EVENT:
                    case ParafaitFunctionEvents.ABORT_REDEEM_TOKEN_TRANSACTION_EVENT:
                    case ParafaitFunctionEvents.ABORT_TRANSACTION_EVENT:
                    case ParafaitFunctionEvents.KIOSK_CARD_DISPENSER_ERROR_EVENT:
                    case ParafaitFunctionEvents.KiOSK_WRISTBAND_PRINT_ERROR:
                    case ParafaitFunctionEvents.EXECUTE_ONLINE_TRANSACTION_EVENT:
                        toEmails = GetEmailAddress();
                        break;
                    case ParafaitFunctionEvents.PAYMENT_LINK_EVENT:
                    case ParafaitFunctionEvents.PAYMENT_MODE_OTP_EVENT:
                        toEmails = GetCustomerEmailId(executionContext, transaction, transactionEventContactsDTO, customerContacts);
                        break;
                    case ParafaitFunctionEvents.PURCHASE_MESSAGE_TRIGGER_EVENT:
                        toEmails = GetTrxCustomerEmailId();
                        break;
                    default:
                        break;
                }
            }
            log.LogMethodExit(toEmails);
            return toEmails;
        }

        private string GetEmailAddress()
        {
            log.LogMethodEntry();
            string emailAddress = string.Empty;
            List<string> emailIdList = new List<string>();
            if (this.customerContacts != null)
            {
                foreach (KeyValuePair<string, string> customerContact in customerContacts)
                {
                    if (customerContact.Key.Equals("EMAILID", StringComparison.InvariantCultureIgnoreCase))
                    {
                        emailIdList.Add(customerContact.Value);
                    }
                }
                emailAddress = string.Join(",", emailIdList);
            }
            else
            {
                if (!string.IsNullOrEmpty(transaction.customerIdentifier))
                {
                    string decryptedCustomerReference = Encryption.Decrypt(transaction.customerIdentifier);
                    string[] customerIdentifierStringArray = decryptedCustomerReference.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < customerIdentifierStringArray.Length; i++)
                    {
                        if (Regex.IsMatch(customerIdentifierStringArray[i], @"^((([\w]+\.[\w]+)+)|([\w]+))@(([\w]+\.)+)([A-Za-z]{1,9})$"))
                        {
                            emailAddress = customerIdentifierStringArray[i];
                        }
                    }
                }
                if (string.IsNullOrWhiteSpace(emailAddress))//no guest email id, check whether trx has customer dto
                {
                    if (transaction.customerDTO != null)
                    {
                        if (string.IsNullOrWhiteSpace(transaction.customerDTO.UserName) == false)
                        {
                            emailAddress = transaction.customerDTO.UserName;
                        }
                        if (string.IsNullOrWhiteSpace(emailAddress) && transaction.customerDTO.ContactDTOList != null &&
                            transaction.customerDTO.ContactDTOList.Any(x => x.ContactType == ContactType.EMAIL))
                        {
                            ContactDTO emailContactDTO = transaction.customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL).OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
                            if (emailContactDTO != null)
                            {
                                emailAddress = emailContactDTO.Attribute1;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(emailAddress);
            return emailAddress;
        }
        private string GetTrxCustomerEmailId()
        {
            log.LogMethodEntry();
            string emailId = string.Empty;
            List<string> emailIdList = new List<string>();
            if (customerContacts != null)
            {
                foreach (KeyValuePair<string, string> customerContact in customerContacts)
                {
                    if (customerContact.Key.Equals("EMAILID", StringComparison.InvariantCultureIgnoreCase))
                    {
                        emailIdList.Add(customerContact.Value);
                    }
                }
                emailId = string.Join(",", emailIdList);
            }
            else
            {
                emailId = (this.transaction != null && this.transaction.customerDTO != null
                                  ? this.transaction.customerDTO.Email
                                  : this.transaction != null && this.transaction.PrimaryCard != null && this.transaction.PrimaryCard.customerDTO != null
                                                       ? this.transaction.PrimaryCard.customerDTO.Email : string.Empty);
            }
            log.LogMethodExit(emailId);
            return emailId;
        }
        /// <summary>
        /// GetToMobileNumber
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <returns></returns>
        protected override string GetToMobileNumber(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO);
            string toPhoneNumber = string.Empty;
            if (MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.SMS)
            {
                switch (this.parafaitFunctionEventDTO.ParafaitFunctionEventName)
                {
                    case ParafaitFunctionEvents.PURCHASE_EVENT:
                    case ParafaitFunctionEvents.RESERVATION_PURCHASE_EVENT:
                    case ParafaitFunctionEvents.REDEEM_TOKEN_TRANSACTION_EVENT:
                    case ParafaitFunctionEvents.ABORT_REDEEM_TOKEN_TRANSACTION_EVENT:
                    case ParafaitFunctionEvents.ABORT_TRANSACTION_EVENT:
                    case ParafaitFunctionEvents.KIOSK_CARD_DISPENSER_ERROR_EVENT:
                    case ParafaitFunctionEvents.KiOSK_WRISTBAND_PRINT_ERROR:
                        toPhoneNumber = GetPhoneNumber(this.transaction, this.customerContacts);
                        break;
                    case ParafaitFunctionEvents.PAYMENT_LINK_EVENT:
                        toPhoneNumber = GetTrxCustomerPhoneNumber();
                        break;
                    case ParafaitFunctionEvents.PAYMENT_MODE_OTP_EVENT:
                        toPhoneNumber = GetCustomerPhoneNumber(executionContext, this.transaction, transactionEventContactsDTO, this.customerContacts);
                        break;
                    case ParafaitFunctionEvents.EXECUTE_ONLINE_TRANSACTION_EVENT:
                    case ParafaitFunctionEvents.PURCHASE_MESSAGE_TRIGGER_EVENT:
                        toPhoneNumber = GetTrxCustomerPhoneNumber();
                        break;
                    default:
                        break;
                }
            }
            else if (MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.WHATSAPP_MESSAGE)
            {
                switch (this.parafaitFunctionEventDTO.ParafaitFunctionEventName)
                {
                    case ParafaitFunctionEvents.PURCHASE_EVENT:
                    case ParafaitFunctionEvents.RESERVATION_PURCHASE_EVENT:
                    case ParafaitFunctionEvents.REDEEM_TOKEN_TRANSACTION_EVENT:
                    case ParafaitFunctionEvents.ABORT_REDEEM_TOKEN_TRANSACTION_EVENT:
                    case ParafaitFunctionEvents.ABORT_TRANSACTION_EVENT:
                    case ParafaitFunctionEvents.KIOSK_CARD_DISPENSER_ERROR_EVENT:
                    case ParafaitFunctionEvents.KiOSK_WRISTBAND_PRINT_ERROR:
                        toPhoneNumber = GetWhatsAppNumber();
                        break;
                    case ParafaitFunctionEvents.PAYMENT_LINK_EVENT:
                        toPhoneNumber = GetTrxCustomerWhatsAppsNumber();
                        break;
                    case ParafaitFunctionEvents.EXECUTE_ONLINE_TRANSACTION_EVENT:
                    case ParafaitFunctionEvents.PURCHASE_MESSAGE_TRIGGER_EVENT:
                        toPhoneNumber = GetTrxCustomerWhatsAppsNumber();
                        break;
                    default:
                        break;
                }
            }
            log.LogMethodExit(toPhoneNumber);
            return toPhoneNumber;
        }
        private static string GetPhoneNumber(Transaction transaction, List<KeyValuePair<string, string>> customerContacts = null)
        {
            log.LogMethodEntry("transaction");
            string phoneNumber = string.Empty;
            List<string> phoneNumberList = new List<string>();
            if (customerContacts != null)
            {
                foreach (KeyValuePair<string, string> customerContact in customerContacts)
                {
                    if (customerContact.Key.Equals("PHONENUMBER", StringComparison.InvariantCultureIgnoreCase))
                    {
                        phoneNumberList.Add(customerContact.Value);
                    }
                }
                phoneNumber = string.Join(",", phoneNumberList);
            }
            else
            {
                
                if (transaction != null && !string.IsNullOrEmpty(transaction.customerIdentifier))
                {
                    string decryptedCustomerReference = Encryption.Decrypt(transaction.customerIdentifier);
                    string[] customerIdentifierStringArray = decryptedCustomerReference.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < customerIdentifierStringArray.Length; i++)
                    {
                        if (Regex.IsMatch(customerIdentifierStringArray[i], @"^\d+$"))
                        {
                            phoneNumber = customerIdentifierStringArray[i];
                        }
                    }
                }
                else if (transaction != null && transaction.customerDTO != null)
                {
                    if (!string.IsNullOrWhiteSpace(transaction.customerDTO.PhoneNumber))
                    {
                        phoneNumber = transaction.customerDTO.PhoneNumber;
                    }

                    if (string.IsNullOrWhiteSpace(phoneNumber) &&
                        transaction.customerDTO.ContactDTOList != null &&
                        transaction.customerDTO.ContactDTOList.Any(x => x.ContactType == ContactType.PHONE))
                    {
                        ContactDTO phoneContactDTO = transaction.customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.PHONE).OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
                        if (phoneContactDTO != null)
                        {
                            phoneNumber = phoneContactDTO.Attribute1;
                        }
                    }
                }
            }
            log.LogMethodExit(phoneNumber);
            return phoneNumber;
        }
        private string GetWhatsAppNumber()
        {
            log.LogMethodEntry();
            string phoneNumber = string.Empty;

            if (transaction.customerDTO != null && transaction.customerDTO.ProfileDTO != null && transaction.customerDTO.ProfileDTO.OptOutWhatsApp == false)
            {
                if (string.IsNullOrWhiteSpace(transaction.customerDTO.PhoneNumber) == false)
                {
                    phoneNumber = transaction.customerDTO.PhoneNumber;
                }

                if (string.IsNullOrWhiteSpace(phoneNumber) &&
                    transaction.customerDTO.ContactDTOList != null &&
                    transaction.customerDTO.ContactDTOList.Any(x => x.ContactType == ContactType.PHONE && x.WhatsAppEnabled))
                {
                    ContactDTO phoneContactDTO = transaction.customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.PHONE && x.WhatsAppEnabled).OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
                    if (phoneContactDTO != null)
                    {
                        phoneNumber = phoneContactDTO.Attribute1;
                    }
                }
            }
            log.LogMethodExit(phoneNumber);
            return phoneNumber;
        }
        private string GetTrxCustomerPhoneNumber()
        {
            log.LogMethodEntry();
            string phNumber = string.Empty;
            List<string> phoneNumberList = new List<string>();
            if (customerContacts != null)
            {
                foreach (KeyValuePair<string, string> customerContact in customerContacts)
                {
                    if (customerContact.Key.Equals("PHONENUMBER", StringComparison.InvariantCultureIgnoreCase))
                    {
                        phoneNumberList.Add(customerContact.Value);
                    }
                }
                phNumber = string.Join(",", phoneNumberList);
            }
            else
            {
                phNumber = (this.transaction != null && this.transaction.customerDTO != null
                                      ? this.transaction.customerDTO.PhoneNumber
                                      : this.transaction != null && this.transaction.PrimaryCard != null && this.transaction.PrimaryCard.customerDTO != null
                                                           ? this.transaction.PrimaryCard.customerDTO.PhoneNumber : string.Empty);
            }
            log.LogMethodExit(phNumber);
            return phNumber;
        }
        private string GetTrxCustomerWhatsAppsNumber()
        {
            log.LogMethodEntry();
            CustomerDTO customerDTO = (this.transaction != null && transaction.customerDTO != null
                                               ? transaction.customerDTO
                                               : transaction != null && transaction.PrimaryCard != null && transaction.PrimaryCard.customerDTO != null
                                                   ? transaction.PrimaryCard.customerDTO : null);
            string toPhoneNumber = CustomerEventsBL.GetCustomerLevelWhatsAppsNumber(customerDTO);
            log.LogMethodExit(toPhoneNumber);
            return toPhoneNumber;

        }
        /// <summary>
        /// GetCardId
        /// </summary>
        /// <returns></returns>
        protected override int? GetCardId()
        {
            log.LogMethodEntry();
            int? cardId = null;
            log.LogMethodExit(cardId);
            return cardId;
        }
        /// <summary>
        /// GetToDevice
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <returns></returns>
        protected override string GetToDevice(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry();
            CustomerDTO customerDTO = ((this.transaction != null && this.transaction.customerDTO != null)
                                          ? this.transaction.customerDTO
                                          : ((this.transaction != null && this.transaction.PrimaryCard != null && this.transaction.PrimaryCard.customerDTO != null)
                                                      ? this.transaction.PrimaryCard.customerDTO : null));
            int customerId = (customerDTO != null ? customerDTO.Id : -1);
            string toDeviceName = GetToDeviceName(messagingClientFunctionLookUpDTO, customerId);
            log.LogMethodExit(toDeviceName);
            return toDeviceName;
        }
        /// <summary>
        /// GetAttachFile
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <returns></returns>
        protected override string GetAttachFile(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry();
            string attachFile = string.Empty;
            string finalFile = string.Empty;
            string contentID = "";
            log.Info(messagingClientFunctionLookUpDTO.MessageType);
            if (MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.EMAIL)
            {
                if (messagingClientFunctionLookUpDTO.ReceiptPrintTemplateId > -1 && this.transaction != null
                    && this.transaction.Trx_id > 0)
                {
                    this.transaction.TransactionInfo.createTransactionInfo(this.transaction.Trx_id);

                    PrinterDTO printerDTO = new PrinterDTO(-1, "Default", "Default", 0, true, DateTime.Now, "", DateTime.Now, "", "", "", -1, PrinterDTO.PrinterTypes.ReceiptPrinter, -1, "", false, -1,-1, 0);
                    int printTemplateId = messagingClientFunctionLookUpDTO.ReceiptPrintTemplateId;
                    Printer.ReceiptPrintTemplateHeaderDTO receiptPrintTemplateDTO = new ReceiptPrintTemplateHeaderBL(executionContext, printTemplateId, true).ReceiptPrintTemplateHeaderDTO;
                    POS.POSPrinterDTO posPrinterDTO = new POS.POSPrinterDTO(-1, utilities.ParafaitEnv.POSMachineId, -1, -1, -1, -1, printTemplateId, printerDTO, null, receiptPrintTemplateDTO, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1, -1);
                    List<POS.POSPrinterDTO> posPrintersDTOList = new List<POS.POSPrinterDTO>();
                    posPrintersDTOList.Add(posPrinterDTO);
                    PrintTransaction printTransaction = new PrintTransaction(posPrintersDTOList);
                    string base64ImageCopy = printTransaction.printPosReceipt(this.transaction, posPrinterDTO, -1, 303, -1, false);
                    if (string.IsNullOrEmpty(base64ImageCopy) == false)
                    {
                        //ReceiptClass Content = POSPrint.PrintReceipt(this.transaction, posPrinterDTO, false);
                        //string imageFolder = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, IMAGE_DIRECTORY);
                        //if (string.IsNullOrWhiteSpace(imageFolder))
                        //{
                        //    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2902, IMAGE_DIRECTORY));//&1 is not set
                        //}
                        //imageFolder = imageFolder + "\\";
                        //Bitmap receipt = new Bitmap(300, 2000);
                        //receipt = ConcLib.getTrxReceipt(receipt, Content, Graphics.FromImage(receipt));
                        contentID = Guid.NewGuid().ToString() + ".gif";
                        attachFile = System.IO.Path.GetTempPath() + contentID;
                        Image attachmentCopy = GenericUtils.ConvertBase64StringToImage(base64ImageCopy);
                        attachmentCopy.Save(attachFile, System.Drawing.Imaging.ImageFormat.Gif);
                        //body += "<img src=\"cid:" + contentID + "\">";
                    }
                }
                else
                {
                    log.Info("Before logo build");
                    if (utilities.ParafaitEnv.CompanyLogo != null)
                    {
                        contentID = "ParafaitLogo" + Guid.NewGuid().ToString() + ".jpg";//Content Id is the identifier for the image
                        attachFile = System.IO.Path.GetTempPath() + contentID;

                        log.Info("contentID:" + contentID);
                        log.Info("attachFile:" + attachFile);
                        try
                        {
                            utilities.ParafaitEnv.CompanyLogo.Save(attachFile, System.Drawing.Imaging.ImageFormat.Jpeg);// Save the logo to the folder as a jpeg file
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                    log.Info("After logo build");
                }
                log.Info("attachFile:" + attachFile);
                if (string.IsNullOrWhiteSpace(attachFile) == false)
                {
                    finalFile = SaveFileToServer(attachFile);
                }
            }
            log.Info("finalFile:" + finalFile);
            log.LogMethodExit(finalFile);
            return finalFile;
        }

        private string SaveFileToServer(string fileNameWithPath)
        {
            log.LogMethodEntry(fileNameWithPath);
            string filename = Path.GetFileName(fileNameWithPath);
            RandomString randomStringObj = new RandomString(3); 
            string outputFile = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY") + "\\" + randomStringObj.Value + filename; ;
            log.LogVariableState("OutputFile", outputFile);
            using (System.IO.FileStream fs = new System.IO.FileStream(fileNameWithPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                fs.CopyTo(ms);
                byte[] bytes = ms.GetBuffer();
                GenericUtils genericUtils = new GenericUtils();
                genericUtils.WriteFileToServer(bytes, outputFile);

                try
                {
                    bytes = null;
                    ms.Dispose();
                }
                catch { }
            } 

            try
            {
                File.Delete(filename);
            }
            catch (Exception ex) { log.Error(ex.Message); }
            log.LogMethodExit(outputFile);
            return outputFile;
        }
        /// <summary>
        /// GetCustomerId
        /// </summary>
        /// <returns></returns>
        protected override int GetCustomerId(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO);
            CustomerDTO customerDTO = ((this.transaction != null && this.transaction.customerDTO != null)
                                          ? this.transaction.customerDTO
                                          : ((this.transaction != null && this.transaction.PrimaryCard != null && this.transaction.PrimaryCard.customerDTO != null)
                                                      ? this.transaction.PrimaryCard.customerDTO : null));
            int customerId = (customerDTO != null ? customerDTO.Id : -1);
            log.LogMethodExit(customerId);
            return customerId;
        }
        /// <summary>
        /// SendEMail
        /// </summary>
        /// <param name="sqlTrx"></param>
        protected virtual void SendEMail(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            SendMsg(MessagingClientDTO.MessagingChanelType.EMAIL, sqlTrx);
            log.LogMethodExit();
        }
        /// <summary>
        /// SendSMS
        /// </summary>
        /// <param name="sqlTrx"></param>
        protected virtual void SendSMS(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            SendMsg(MessagingClientDTO.MessagingChanelType.SMS, sqlTrx);
            log.LogMethodExit();
        }
        /// <summary>
        /// SendAppNotification
        /// </summary>
        /// <param name="sqlTrx"></param>
        protected virtual void SendAppNotification(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            SendMsg(MessagingClientDTO.MessagingChanelType.APP_NOTIFICATION, sqlTrx);
            log.LogMethodExit();
        }
        /// <summary>
        /// SendWhatsApps
        /// </summary>
        /// <param name="sqlTrx"></param>
        protected virtual void SendWhatsApps(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            SendMsg(MessagingClientDTO.MessagingChanelType.WHATSAPP_MESSAGE, sqlTrx);
            log.LogMethodExit();
        }
        /// <summary>
        /// SendMsg
        /// </summary>
        /// <param name="messagingChanelType"></param>
        /// <param name="sqlTrx"></param>
        protected virtual void SendMsg(MessagingClientDTO.MessagingChanelType messagingChanelType,  SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            EmailTemplateDTO emailTemplateDTO = GetEmailTemplateDTO(messagingChanelType, sqlTrx);

            string messageSubject = MessageSubjectFormatter(emailTemplateDTO.Description);
            string messageBody = MessageBodyFormatter(emailTemplateDTO.EmailTemplate);
            messageBody = AppNotificationMessageBodyFormatter(messagingChanelType, messageSubject, messageBody);
            int receiptTemplateId = GetReceiptTemplateId();
            MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO = new MessagingClientFunctionLookUpDTO(-1, -1,
                MessagingClientDTO.SourceEnumToString(messagingChanelType), -1, -1, receiptTemplateId, null, null);
            messagingClientFunctionLookUpDTO.MessagingClientDTO = new MessagingClientDTO();
            SaveMessagingRequestDTO(messageSubject, messageBody, messagingClientFunctionLookUpDTO, sqlTrx);
            log.LogMethodExit();
        }

        private int GetReceiptTemplateId()
        {
            log.LogMethodEntry();
            int receiptTemplateId = -1;
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.KIOSK_CARD_DISPENSER_ERROR_EVENT)
            {
                receiptTemplateId = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, CARD_DISPENSER_ERROR_RECEIPT_TEMPLATE, -1);
            }
            else if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.KiOSK_WRISTBAND_PRINT_ERROR)
            {
                receiptTemplateId = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, CARD_PRINT_ERROR_RECEIPT_TEMPLATE, -1);
            }
            log.LogMethodExit(receiptTemplateId);
            return receiptTemplateId;
        }

        /// <summary>
        /// GetEmailTemplateDTO
        /// </summary>
        /// <param name="messagingChanelType"></param>
        /// <param name="sqlTrx"></param>
        /// <returns></returns>
        protected virtual EmailTemplateDTO GetEmailTemplateDTO(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messagingChanelType, sqlTrx);
            EmailTemplateDTO emailTemplateDTO = null;
            string templateName = GetEventBasedTemplateNames(messagingChanelType);
            try
            {
                if (string.IsNullOrWhiteSpace(templateName) == false)
                {
                    emailTemplateDTO = new EmailTemplate(executionContext).GetEmailTemplate(templateName, executionContext.GetSiteId(), sqlTrx);
                }
                else
                {
                    if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.EXECUTE_ONLINE_TRANSACTION_EVENT)
                    {
                        emailTemplateDTO = CreateExecuteOnlineTransactionEmailTemplateDTO();
                    }
                    else if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.REDEEM_TOKEN_TRANSACTION_EVENT)
                    {
                        emailTemplateDTO = CreateRedeemTokenEmailTemplateDTO();
                    }
                    else if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.ABORT_REDEEM_TOKEN_TRANSACTION_EVENT)
                    {
                        emailTemplateDTO = CreateAbortRedeemTokenEmailTemplateDTO();
                    }
                    else if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.ABORT_TRANSACTION_EVENT)
                    {
                        emailTemplateDTO = CreateAbortTransactionEmailTemplateDTO();
                    }
                    else if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.KIOSK_CARD_DISPENSER_ERROR_EVENT)
                    {
                        emailTemplateDTO = CreateKioskCardDispenserErrorEmailTemplateDTO();
                    }
                    else if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.KiOSK_WRISTBAND_PRINT_ERROR)
                    {
                        emailTemplateDTO = CreateKioskWristbandPrintErrorEmailTemplateDTO();
                    } 
                }
                if ((this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.PURCHASE_EVENT || this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.RESERVATION_PURCHASE_EVENT)
                    && string.IsNullOrWhiteSpace(templateName) && messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS)
                {
                    string templateContent = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, ONLINE_TICKETS_SMS_TEXT_TEMPLATE);
                    emailTemplateDTO = new EmailTemplateDTO();
                    emailTemplateDTO.Description = "Purchase";
                    emailTemplateDTO.EmailTemplate = templateContent;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while retrieving the email template for " + templateName, ex);
            }
            if (emailTemplateDTO == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2881, messagingChanelType.ToString(), this.ParafaitFunctionEventDTO.ParafaitFunctionEventName.ToString());
                //'Unable to fetch &1 template for &2'
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;

        }
        private string GetEventBasedTemplateNames(MessagingClientDTO.MessagingChanelType messagingChanelType)
        {
            log.LogMethodEntry(messagingChanelType);
            string templateName = string.Empty;
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.PAYMENT_LINK_EVENT)
            {
                if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
                {
                    templateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, TRANSACTIONPAYMENTLINKTEMPLATE);
                }
            }
            else if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.PURCHASE_EVENT)
            {
                if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
                {
                    templateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, ONLINE_RECEIPT_EMAIL_TEMPLATE);
                }
                //if (messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS)
                //{
                //    templateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, ONLINE_TICKETS_SMS_TEXT_TEMPLATE);
                //}
            }
            else if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.RESERVATION_PURCHASE_EVENT)
            {
                if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
                {
                    templateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, ONLINE_RESERVATION_EMAIL_TEMPLATE);
                }
            }
            log.LogMethodExit(templateName);
            return templateName;
        }
        private EmailTemplateDTO CreateExecuteOnlineTransactionEmailTemplateDTO()
        {
            log.LogMethodEntry();
            EmailTemplateDTO emailTemplateDTO = new EmailTemplateDTO();
            emailTemplateDTO.Description = "@SiteName" + MessageContainerList.GetMessage(executionContext, " - Issuing Cards");
            emailTemplateDTO.EmailTemplate = MessageContainerList.GetMessage(executionContext, 1689, "@FirstName", Environment.NewLine, "@transactionOTP",
                                                              "@TodaysIssuedCards", "@SiteName");
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;
        }
        private EmailTemplateDTO CreateRedeemTokenEmailTemplateDTO()
        {
            log.LogMethodEntry();
            EmailTemplateDTO emailTemplateDTO = new EmailTemplateDTO();
            emailTemplateDTO.Description = MessageContainerList.GetMessage(executionContext, "Redeem Token Receipt");
            string msgBody = MessageContainerList.GetMessage(executionContext, "Date") + ": @TrxDate" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, "Bill No") + ": @TrxNo " + MessageContainerList.GetMessage(executionContext, "Transaction Id") + ": @TransactionId" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, "Kiosk") + ": @POSName" + Environment.NewLine
                           + Environment.NewLine
                           + "*******************" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, 797) + Environment.NewLine //"Redeem Successful"; 
                           + "*******************" + Environment.NewLine
                           + "*******************" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, "Total Tokens inserted") + ": @TotalTokenInserted" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, "Points Loaded") + ": @TotalPointsLoaded" + Environment.NewLine
                           + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, 499)
                           + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, ", ") + " @SiteName" + Environment.NewLine;

            emailTemplateDTO.EmailTemplate = msgBody;
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;
        }
        private EmailTemplateDTO CreateAbortRedeemTokenEmailTemplateDTO()
        {
            log.LogMethodEntry();
            EmailTemplateDTO emailTemplateDTO = new EmailTemplateDTO();
            emailTemplateDTO.Description = MessageContainerList.GetMessage(executionContext, "Redeem Token Abort Receipt");
            string msgBody = MessageContainerList.GetMessage(executionContext, "Date") + ": @TrxDate" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, "Bill No") + ": @TrxNo " + MessageContainerList.GetMessage(executionContext, "Transaction Id") + ": @TransactionId" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, "Kiosk") + ": @POSName" + Environment.NewLine
                           + Environment.NewLine
                           + "*******************" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, 439) + Environment.NewLine //"TRANSACTION ABORTED";
                           + "*******************" + Environment.NewLine
                           + "*******************" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, 440, ": @TotalAmount") + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, "Cash") + ": @CashAmount" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, "Credit Card") + ": @CreditCardAmount" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, 441)
                           + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, ", ") + " @SiteName" + Environment.NewLine;

            emailTemplateDTO.EmailTemplate = msgBody;
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;
        }
        private EmailTemplateDTO CreateAbortTransactionEmailTemplateDTO()
        {
            log.LogMethodEntry();
            EmailTemplateDTO emailTemplateDTO = new EmailTemplateDTO();
            emailTemplateDTO.Description = MessageContainerList.GetMessage(executionContext, "Transaction Abort Receipt");
            string msgBody = MessageContainerList.GetMessage(executionContext, "Date") + ": @TrxDate" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, "Bill No") + ": @TrxNo " + MessageContainerList.GetMessage(executionContext, "Transaction Id") + ": @TransactionId" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, "Kiosk") + ": @POSName" + Environment.NewLine
                           + Environment.NewLine
                           + "*******************" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, 439) + Environment.NewLine //"TRANSACTION ABORTED";
                           + "*******************" + Environment.NewLine
                           + "*******************" + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, 440, ": @TotalAmount") + Environment.NewLine
                           + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, 441)
                           + Environment.NewLine
                           + MessageContainerList.GetMessage(executionContext, ", ") + " @SiteName" + Environment.NewLine;

            emailTemplateDTO.EmailTemplate = msgBody;
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;
        }
        private EmailTemplateDTO CreateKioskCardDispenserErrorEmailTemplateDTO()
        {
            log.LogMethodEntry();
            EmailTemplateDTO emailTemplateDTO = new EmailTemplateDTO();
            emailTemplateDTO.Description = MessageContainerList.GetMessage(executionContext, "Error while dispensing cards");
            string msg = "Dear &1, &2 &2 Sorry, Kiosk is unable to issue all new cards for your transaction with transaction id: &3 due to some issues. &2 Please contact staff for the help. &2 &2 Thank you &2 &4";
            emailTemplateDTO.EmailTemplate = MessageContainerList.GetMessage(executionContext, msg, "@FirstName", Environment.NewLine, "@transactionId",
                "@SiteName");
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;
        }
        private EmailTemplateDTO CreateKioskWristbandPrintErrorEmailTemplateDTO()
        {
            log.LogMethodEntry();
            EmailTemplateDTO emailTemplateDTO = new EmailTemplateDTO();
            emailTemplateDTO.Description = MessageContainerList.GetMessage(executionContext, "Error while printing wristbands");
            string msg = "Dear &1, &2 &2 Sorry, Kiosk is unable to print all wristbands for your transaction with transaction id: &3 due to some issues. &2 Please contact staff for the help. &2 &2 Thank you &2 &4";
            emailTemplateDTO.EmailTemplate = MessageContainerList.GetMessage(executionContext, msg, "@FirstName", Environment.NewLine, "@transactionId",
                "@SiteName");
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;
        }
        /// <summary>
        /// GetCustomerPhoneNumber        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="transaction"></param>
        /// <param name="transactionEventContactsDTO"></param>
        /// <returns></returns>
        public static string GetCustomerPhoneNumber(ExecutionContext executionContext, Transaction transaction, TransactionEventContactsDTO transactionEventContactsDTO, List<KeyValuePair<string, string>> customerContacts = null)
        {
            log.LogMethodEntry(executionContext, "transaction", transactionEventContactsDTO);
            string phoneNumber = string.Empty;
            List<string> phoneNumberList = new List<string>();
            if (customerContacts != null)
            {
                foreach (KeyValuePair<string, string> customerContact in customerContacts)
                {
                    if (customerContact.Key.Equals("PHONENUMBER", StringComparison.InvariantCultureIgnoreCase))
                    {
                        phoneNumberList.Add(customerContact.Value);
                    }
                }
                phoneNumber = string.Join(",", phoneNumberList);
            }
            else
            {
                if (transactionEventContactsDTO != null && string.IsNullOrWhiteSpace(transactionEventContactsDTO.PhoneNumber) == false)
                {
                    phoneNumber = transactionEventContactsDTO.PhoneNumber;
                }
                else if (transaction != null)
                {
                    phoneNumber = GetPhoneNumber(transaction);
                }
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    log.Error("Transaction has no phone details");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2907, MessageContainerList.GetMessage(executionContext,"Phone Number")));
                    //&1 details are missing
                }
            }
            log.LogMethodExit(phoneNumber);
            return phoneNumber;
        }
        /// <summary>
        /// GetCustomerEmailId
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="transaction"></param>
        /// <param name="transactionEventContactsDTO"></param>
        /// <returns></returns>
        public static string GetCustomerEmailId(ExecutionContext executionContext, Transaction transaction, TransactionEventContactsDTO transactionEventContactsDTO, List<KeyValuePair<string, string>> customerContacts = null)
        {
            log.LogMethodEntry(executionContext, transaction, transactionEventContactsDTO);
            string emailAddress = string.Empty;
            List<string> emailIdList = new List<string>();
            if (customerContacts != null)
            {
                foreach (KeyValuePair<string, string> customerContact in customerContacts)
                {
                    if (customerContact.Key.Equals("EMAILID", StringComparison.InvariantCultureIgnoreCase))
                    {
                        emailIdList.Add(customerContact.Value);
                    }
                }
                emailAddress = string.Join(",", emailIdList);
            }
            else
            {
                if (transactionEventContactsDTO != null && string.IsNullOrWhiteSpace(transactionEventContactsDTO.EmailId) == false)
                {
                    emailAddress = transactionEventContactsDTO.EmailId;
                }
                else if (transaction.customerDTO != null || (transaction.PrimaryCard != null && transaction.PrimaryCard.customerDTO != null))
                {
                    CustomerDTO customerDTO = (transaction.PrimaryCard != null && transaction.PrimaryCard.customerDTO != null ? transaction.PrimaryCard.customerDTO : (transaction.customerDTO != null ? transaction.customerDTO : null));
                    if (customerDTO != null)
                    {
                        if (string.IsNullOrWhiteSpace(customerDTO.UserName) == false)
                        {
                            emailAddress = customerDTO.UserName;
                        }
                        if (string.IsNullOrWhiteSpace(emailAddress) &&
                            customerDTO.ContactDTOList != null &&
                            customerDTO.ContactDTOList.Any(x => x.ContactType == ContactType.EMAIL))
                        {
                            ContactDTO emailContactDTO = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL).OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
                            if (emailContactDTO != null)
                            {
                                emailAddress = emailContactDTO.Attribute1;
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(emailAddress))
                {
                    log.Error("Transaction has no email details");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2907, MessageContainerList.GetMessage(executionContext, "Email ID")));
                    //&1 details are missing
                }
            }
            log.LogMethodExit(emailAddress);
            return emailAddress;
        }
    }
}
