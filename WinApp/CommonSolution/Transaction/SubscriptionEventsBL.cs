/********************************************************************************************
 * Project Name - SubscriptionEventsBL 
 * Description  -BL class of the Subscription Events
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     12-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 *2.120.0     18-Mar-2021    Guru S A             For Subscription phase 2 changes
 *2.140.2     18-Apr-2022    Girish Kundar        Modified:  BOCA changes - Added new column WBModel to printer class
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Printer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// SubscriptionEventsBL
    /// </summary>
    public class SubscriptionEventsBL : ParafaitFunctionEventBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<ParafaitFunctionEvents> subscriptionEventList = new ParafaitFunctionEvents[]{ParafaitFunctionEvents.SUBSCRIPTION_PURCHASE_EVENT,
                                                                                                       ParafaitFunctionEvents.CANCEL_SUBSCRIPTION_EVENT,
                                                                                                       ParafaitFunctionEvents.BILL_SUBSCRIPTION_EVENT,
                                                                                                       ParafaitFunctionEvents.PAUSE_SUBSCRIPTION_EVENT,
                                                                                                       ParafaitFunctionEvents.UNPAUSE_SUBSCRIPTION_EVENT,
                                                                                                       ParafaitFunctionEvents.REACTIVATE_SUBSCRIPTION_EVENT,
                                                                                                       ParafaitFunctionEvents.RENEW_SUBSCRIPTION_EVENT,
                                                                                                       ParafaitFunctionEvents.SUBSCRIPTION_CARD_EXPIRY_EVENT,
                                                                                                       ParafaitFunctionEvents.SUBSCRIPTION_PAYMENT_FAILURE_EVENT,
                                                                                                       ParafaitFunctionEvents.RENEWAL_REMINDER_EVENT }.ToList();
        private const string SUBSCRIPTION_PURCHASE_MSG_REF = "Subscription Purchase";
        private const string CANCEL_SUBSCRIPTION_MSG_REF = "Cancel Subscription";
        private const string BILL_SUBSCRIPTION_MSG_REF = "Bill Subscription";
        private const string PAUSE_SUBSCRIPTION_MSG_REF = "Pause Subscription";
        private const string UNPAUSE_SUBSCRIPTION_MSG_REF = "UnPause Subscription";
        private const string REACTIVATE_SUBSCRIPTION_MSG_REF = "Reactivate Subscription";
        private const string RENEW_SUBSCRIPTION_MSG_REF = "Renew Subscription";
        private const string SUBSCRIPTION_CARD_EXPIRY_MSG_REF = "Subscription Card Expiry";
        private const string SUBSCRIPTION_PAYMENT_FAILURE_MSG_REF = "Subscription Payment Failure";
        private const string RENEWAL_REMINDER_MSG_REF = "Subscription Renewal Reminder";

        private const string PROGRAM_NAME = "Send Subscription Reminder";
        private const string IMAGE_DIRECTORY = "IMAGE_DIRECTORY";


        private SubscriptionHeaderDTO subscriptionHeaderDTO;
        private Transaction transaction;
        private CustomerCreditCardsDTO customerCreditCardsDTO;
        private Utilities utilities;
        private CustomerBL customerBL;
        private ContactBL contactBL;
        /// <summary>
        /// SubscriptionEventsBL
        /// </summary>
        /// <param name="utilities"></param> 
        /// <param name="parafaitFunctionEvents"></param>
        /// <param name="subscriptionHeaderDTO"></param>
        /// <param name="transaction"></param>
        /// <param name="sqlTransaction"></param>
        public SubscriptionEventsBL(Utilities utilities, ParafaitFunctionEvents parafaitFunctionEvents, SubscriptionHeaderDTO subscriptionHeaderDTO, Transaction transaction, SqlTransaction sqlTransaction = null)
            : base(utilities.ExecutionContext)
        {
            log.LogMethodEntry(utilities, parafaitFunctionEvents, subscriptionHeaderDTO, transaction, sqlTransaction);
            this.utilities = utilities;
            this.subscriptionHeaderDTO = subscriptionHeaderDTO;
            this.transaction = transaction;
            if (subscriptionHeaderDTO.CustomerId > -1)
            {
                this.customerBL = new CustomerBL(executionContext, subscriptionHeaderDTO.CustomerId, true, true, sqlTransaction);
            }
            else
            {
                this.customerBL = null;
            }
            if (subscriptionHeaderDTO.CustomerContactId > -1)
            {
                this.contactBL = new ContactBL(executionContext, subscriptionHeaderDTO.CustomerContactId, sqlTransaction);
            }
            else
            {
                this.contactBL = null;
            }
            ValidSubscriptionEvent(parafaitFunctionEvents);
            LoadParafaitFunctionEventDTO(parafaitFunctionEvents, sqlTransaction);
            //LoadAlertSetupDetails();
            log.LogMethodExit();
        }
        /// <summary>
        /// SubscriptionEventsBL
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="customerCreditCardsDTO"></param>
        /// <param name="sqlTransaction"></param>
        public SubscriptionEventsBL(Utilities utilities, CustomerCreditCardsDTO customerCreditCardsDTO, SqlTransaction sqlTransaction = null)
            : base(utilities.ExecutionContext)
        {
            log.LogMethodEntry(utilities, customerCreditCardsDTO, sqlTransaction);
            this.utilities = utilities;
            this.customerCreditCardsDTO = customerCreditCardsDTO;
            if (customerCreditCardsDTO.CustomerId > -1)
            {
                this.customerBL = new CustomerBL(executionContext, customerCreditCardsDTO.CustomerId, true, true, sqlTransaction);
            }
            else
            {
                this.customerBL = null;
            }
            this.contactBL = null;
            LoadParafaitFunctionEventDTO(ParafaitFunctionEvents.SUBSCRIPTION_CARD_EXPIRY_EVENT, sqlTransaction);
            //LoadAlertSetupDetails();
            log.LogMethodExit();
        }

        private void ValidSubscriptionEvent(ParafaitFunctionEvents parafaitFunctionEvents)
        {
            log.LogMethodEntry(parafaitFunctionEvents);
            if (subscriptionEventList.Contains(parafaitFunctionEvents) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2884, parafaitFunctionEvents));//"&1 is not a valid Subscription event
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
                if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.SUBSCRIPTION_CARD_EXPIRY_EVENT)
                {
                    SubscriptionMessageContentBuilder subscriptionMessageContentBuilder = new SubscriptionMessageContentBuilder(utilities, executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName, this.customerCreditCardsDTO, true);
                    messageSubjectContent = subscriptionMessageContentBuilder.GenerateMessageContent(messageTemplateSubject);
                }
                else
                {
                    SubscriptionMessageContentBuilder subscriptionMessageContentBuilder = new SubscriptionMessageContentBuilder(utilities, executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName, this.subscriptionHeaderDTO, this.transaction, true);
                    messageSubjectContent = subscriptionMessageContentBuilder.GenerateMessageContent(messageTemplateSubject);
                }
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
            //TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(executionContext, utilities, -1, transaction);
            //messageBodyContent = transactionEmailTemplatePrint.BuildContent(messageTemplateContent);
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.SUBSCRIPTION_CARD_EXPIRY_EVENT)
            {
                SubscriptionMessageContentBuilder subscriptionMessageContentBuilder = new SubscriptionMessageContentBuilder(utilities, executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName, this.customerCreditCardsDTO);
                messageBodyContent = subscriptionMessageContentBuilder.GenerateMessageContent(messageTemplateContent);
            }
            else
            {
                SubscriptionMessageContentBuilder subscriptionMessageContentBuilder = new SubscriptionMessageContentBuilder(utilities, executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName, this.subscriptionHeaderDTO, this.transaction);
                messageBodyContent = subscriptionMessageContentBuilder.GenerateMessageContent(messageTemplateContent);
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
                messageBody = messagingTriggerAppNotificationContentBuilder.FormatAppNotificationContent(messageSubject, messageBody, appNotificationType, null, null, null);
            }
            log.LogMethodExit(messageBody);
            return messageBody;
        }
        /// <summary>
        /// SendMessage
        /// </summary>
        /// <param name="messagingChanelType"></param>
        /// <param name="sqlTrx"></param>
        public virtual void SendMessage(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(messagingChanelType, sqlTrx);
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
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2885, parafaitFunctionEventDTO.ParafaitFunctionEventName.ToString()));
                //Sorry, cannot send message. Subscription event &1 is not mapped to a messaging client
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
            CreateMessagingRequest(messageSubject, messageBody, messagingClientFunctionLookUpDTO, messageReference, messagingClientId, messageType, toEmails, toMobile, cardId, customerId, attachFile, toDevice, sqlTrx);
            log.LogMethodExit();
        }
        private string GetMessageReference()
        {
            log.LogMethodEntry();
            string messageReference = string.Empty;
            switch (this.parafaitFunctionEventDTO.ParafaitFunctionEventName)
            {
                case ParafaitFunctionEvents.SUBSCRIPTION_PURCHASE_EVENT:
                    messageReference = SUBSCRIPTION_PURCHASE_MSG_REF;
                    break;
                case ParafaitFunctionEvents.CANCEL_SUBSCRIPTION_EVENT:
                    messageReference = CANCEL_SUBSCRIPTION_MSG_REF;
                    break;
                case ParafaitFunctionEvents.BILL_SUBSCRIPTION_EVENT:
                    messageReference = BILL_SUBSCRIPTION_MSG_REF;
                    break;
                case ParafaitFunctionEvents.PAUSE_SUBSCRIPTION_EVENT:
                    messageReference = PAUSE_SUBSCRIPTION_MSG_REF;
                    break;
                case ParafaitFunctionEvents.UNPAUSE_SUBSCRIPTION_EVENT:
                    messageReference = UNPAUSE_SUBSCRIPTION_MSG_REF;
                    break; 
                case ParafaitFunctionEvents.REACTIVATE_SUBSCRIPTION_EVENT:
                    messageReference = REACTIVATE_SUBSCRIPTION_MSG_REF;
                    break;
                case ParafaitFunctionEvents.RENEW_SUBSCRIPTION_EVENT:
                    messageReference = RENEW_SUBSCRIPTION_MSG_REF;
                    break;
                case ParafaitFunctionEvents.SUBSCRIPTION_CARD_EXPIRY_EVENT:
                    messageReference = SUBSCRIPTION_CARD_EXPIRY_MSG_REF;
                    break;
                case ParafaitFunctionEvents.SUBSCRIPTION_PAYMENT_FAILURE_EVENT:
                    messageReference = SUBSCRIPTION_PAYMENT_FAILURE_MSG_REF;
                    break;
                case ParafaitFunctionEvents.RENEWAL_REMINDER_EVENT:
                    messageReference = RENEWAL_REMINDER_MSG_REF;
                    break;
                default:
                    break;
            }
            log.LogMethodExit(messageReference);
            return messageReference;
        }

        private CustomerDTO GetCustomerDTO()
        {
            log.LogMethodEntry();
            CustomerDTO customerDTO = null;
            if (customerBL != null)
            { customerDTO = customerBL.CustomerDTO; }
            log.LogMethodExit(customerDTO);
            return customerDTO;
        }

        private ContactDTO GetContactDTO()
        {
            log.LogMethodEntry();
            ContactDTO contactDTO = null;
            if (contactBL != null)
            {
                contactDTO = contactBL.ContactDTO;
            }
            log.LogMethodExit(contactDTO);
            return contactDTO;
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
                toEmails = GetCustomerEmail();
            }
            log.LogMethodExit(toEmails);
            return toEmails;
        }
        private string GetCustomerEmail()
        {
            log.LogMethodEntry();
            string toEmails = string.Empty;
            ContactDTO contactDTO = GetContactDTO();
            if (contactDTO != null)
            {
                if (contactDTO.ContactType == ContactType.EMAIL)
                {
                    toEmails = contactBL.ContactDTO.Attribute1;
                }
            }
            if (string.IsNullOrWhiteSpace(toEmails))
            {
                CustomerDTO customerDTO = GetCustomerDTO();
                if (customerDTO != null)
                {
                    toEmails = customerDTO.Email;
                }
            }
            log.LogMethodExit(toEmails);
            return toEmails;
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
                toPhoneNumber = GetCusomerPhoneNumber();
            }
            else if (MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.WHATSAPP_MESSAGE)
            {
                toPhoneNumber = GetCustomerWhatsAppNumber();
            }
            log.LogMethodExit(toPhoneNumber);
            return toPhoneNumber;
        } 

        private string GetCusomerPhoneNumber()
        {
            log.LogMethodEntry();
            string toPhoneNumber = string.Empty;
            ContactDTO contactDTO = GetContactDTO();
            if (contactDTO != null)
            {
                if (contactDTO.ContactType == ContactType.PHONE)
                {
                    toPhoneNumber = contactBL.ContactDTO.Attribute1;
                }
            }
            if (string.IsNullOrWhiteSpace(toPhoneNumber))
            {
                CustomerDTO customerDTO = GetCustomerDTO();
                if (customerDTO != null)
                {
                    toPhoneNumber = customerDTO.PhoneNumber;
                }
            }
            log.LogMethodExit(toPhoneNumber);
            return toPhoneNumber;
        } 
        private string GetCustomerWhatsAppNumber()
        {
            log.LogMethodEntry();
            string toPhoneNumber = string.Empty;
            ContactDTO contactDTO = GetContactDTO();
            CustomerDTO customerDTO = GetCustomerDTO();
            if (customerDTO != null)
            {
                //if contact is populated check that info first
                toPhoneNumber = (customerDTO.ProfileDTO != null && customerDTO.ProfileDTO.OptOutWhatsApp == false
                                             && contactDTO != null && contactDTO.ContactType == ContactType.PHONE && contactDTO.WhatsAppEnabled ? contactDTO.Attribute1 : string.Empty);
                if (string.IsNullOrWhiteSpace(toPhoneNumber) && customerDTO.ProfileDTO != null && customerDTO.ProfileDTO.OptOutWhatsApp == false
                         && customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Any()
                         && customerDTO.ContactDTOList.Exists(cont => cont.ContactType == ContactType.PHONE && cont.WhatsAppEnabled && cont.Attribute1 == customerDTO.PhoneNumber))
                {

                    toPhoneNumber = customerDTO.PhoneNumber;
                }
            }
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
            int customerId = (subscriptionHeaderDTO != null ? subscriptionHeaderDTO.CustomerId : this.customerCreditCardsDTO != null ? this.customerCreditCardsDTO.CustomerId : -1);
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
            log.LogMethodEntry(messagingClientFunctionLookUpDTO);
            string attachFile = string.Empty;
            string finalFile = string.Empty;
            if (messagingClientFunctionLookUpDTO.ReceiptPrintTemplateId > -1 && this.transaction != null)
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
                    string imageFolder = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, IMAGE_DIRECTORY);
                    if (string.IsNullOrWhiteSpace(imageFolder))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2902, IMAGE_DIRECTORY));//&1 is not set
                    }
                    imageFolder = imageFolder + "\\";
                    //Bitmap receipt = new Bitmap(300, 2000);
                    //receipt = ConcLib.getTrxReceipt(receipt, Content, Graphics.FromImage(receipt));
                    string contentID = Guid.NewGuid().ToString() + ".gif";
                    attachFile = imageFolder + contentID;
                    Image attachmentCopy = GenericUtils.ConvertBase64StringToImage(base64ImageCopy);
                    attachmentCopy.Save(attachFile, System.Drawing.Imaging.ImageFormat.Gif);
                    //body += "<img src=\"cid:" + contentID + "\">";
                }
                log.Info("attachFile:" + attachFile);
                if (string.IsNullOrWhiteSpace(attachFile) == false)
                {
                    finalFile = SaveFileToServer(attachFile);
                }
            }
            log.Info("finalFile:" + finalFile);
            log.LogMethodExit(attachFile);
            return attachFile;
        }

        private string SaveFileToServer(string fileNameWithPath)
        {
            log.LogMethodEntry(fileNameWithPath);
            string filename = Path.GetFileName(fileNameWithPath);
            RandomString randomStringObj = new RandomString(3);
            string outputFile = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY") + "\\" + randomStringObj.Value + filename;
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
            int customerId = (subscriptionHeaderDTO != null ? subscriptionHeaderDTO.CustomerId : this.customerCreditCardsDTO != null ? this.customerCreditCardsDTO.CustomerId : -1);
            log.LogMethodExit(customerId);
            return customerId;
        }
    }
}
