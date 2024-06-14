/********************************************************************************************
 * Project Name - PurchaseMessageTriggerEventBL 
 * Description  -BL class of the Purchase Message Trigger Event  
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     13-Dec-2020    Guru S A             Created for Subscription changes     
 *2.140.2       18-Apr-2022     Girish Kundar    Modified : Added new column Models to printer
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;

namespace Semnox.Parafait.ConcurrentManager
{
    public class PurchaseMessageTriggerEventBL : TransactionEventsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private Transaction transaction;
        private MessagingTriggerDTO messagingTriggerDTO;
        private const string PURCHASE_MESSAGE_TRIGGER = "PURCHASE_MESSAGE_TRIGGER";
        private const string MESSAGING_TRIGGER_DTO = "MessagingTriggerDTO";
        private const char MESSAGE_TYPE_BOTH = 'B'; 
        private const string IMAGE_DIRECTORY = "IMAGE_DIRECTORY";

        /// <summary>
        /// PurchaseMessageTriggerEventBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utilities"></param>
        /// <param name="messagingTriggerDTO"></param>
        /// <param name="transaction"></param>
        /// <param name="sqlTransaction"></param>
        public PurchaseMessageTriggerEventBL(ExecutionContext executionContext, Utilities utilities, MessagingTriggerDTO messagingTriggerDTO, Transaction.Transaction transaction, SqlTransaction sqlTransaction = null)
            : base(executionContext, utilities, ParafaitFunctionEvents.PURCHASE_MESSAGE_TRIGGER_EVENT, transaction, null,null, sqlTransaction)
        {
            log.LogMethodEntry(messagingTriggerDTO, transaction, sqlTransaction);
            this.messagingTriggerDTO = messagingTriggerDTO;
            //this.transaction = transaction;
            //LoadParafaitFunctionEventDTO(ParafaitFunctionEvents.CARD_EXPIRY_MESSAGE_TRIGGER_EVENT, sqlTransaction);
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
                MessagingTriggerPurchaseContentBuilder messagingTriggerPurchaseContentBuilder = new MessagingTriggerPurchaseContentBuilder(utilities, executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName, messagingTriggerDTO, this.transaction);
                messageSubjectContent = messagingTriggerPurchaseContentBuilder.GenerateMessageSubjectForTransaction(messageTemplateSubject);
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
            MessagingTriggerPurchaseContentBuilder messagingTriggerPurchaseContentBuilder = new MessagingTriggerPurchaseContentBuilder(utilities, executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName, messagingTriggerDTO, this.transaction);
            messageBodyContent = messagingTriggerPurchaseContentBuilder.GenerateMessageContentForTransaction(messageTemplateContent); 
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
                AppNotificationContentBuilder.AppNotificationType appNotificationType = AppNotificationContentBuilder.AppNotificationType.PURCHAGSE_TRIGGER;
                string transactionId = transaction.Trx_id.ToString(); 
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
        public override void SendMessage(MessagingClientDTO.MessagingChanelType messagingChanelType, List<KeyValuePair<string, string>> customerContacts = null, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(messagingChanelType, sqlTrx);
            this.customerContacts = customerContacts;

            if (this.messagingTriggerDTO == null)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1956) + " - " + MESSAGING_TRIGGER_DTO);
            }
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
                if (messagingChanelType ==  MessagingClientDTO.MessagingChanelType.NONE 
                    && this.messagingTriggerDTO.MessageType == MESSAGE_TYPE_BOTH)
                {
                    SendEMail(sqlTrx);
                    SendSMS(sqlTrx);
                    //SendAppNotification(sqlTrx);
                }
                else if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
                {
                    SendEMail(sqlTrx);
                }
                else if (messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS)
                {
                    SendSMS(sqlTrx);
                }
                else if (messagingChanelType == MessagingClientDTO.MessagingChanelType.APP_NOTIFICATION)
                {
                    SendAppNotification(sqlTrx);
                }
                else if (messagingChanelType == MessagingClientDTO.MessagingChanelType.WHATSAPP_MESSAGE)
                {
                    SendWhatsApps(sqlTrx);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// BuildAndSendMessage
        /// </summary>
        /// <param name="messagingChanelType"></param>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <param name="sqlTrx"></param>
        protected override void BuildAndSendMessage(MessagingClientDTO.MessagingChanelType messagingChanelType, MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messagingChanelType, messagingClientFunctionLookUpDTO, sqlTrx);
            //messagingChanelType == MessagingClientDTO.MessagingChanelType.NONE - means send all. Else send specified type only
            if ((messagingChanelType != MessagingClientDTO.MessagingChanelType.NONE
                && messagingChanelType == MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType))
                || messagingChanelType == MessagingClientDTO.MessagingChanelType.NONE)
            {
                EmailTemplateDTO messageTemplateDTO = GetEmailTemplateDTO(MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType), sqlTrx);
                string content = messageTemplateDTO.EmailTemplate;
                string messageBody = MessageBodyFormatter(content);
                string messageSubject = MessageSubjectFormatter(messageTemplateDTO.Description);
                messageBody = AppNotificationMessageBodyFormatter(MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType), messageSubject, messageBody);
                SaveMessagingRequestDTO(messageSubject, messageBody, messagingClientFunctionLookUpDTO, sqlTrx);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// SendMsg
        /// </summary>
        /// <param name="messagingChanelType"></param>
        /// <param name="sqlTrx"></param>
        protected override void SendMsg(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            EmailTemplateDTO emailTemplateDTO = GetEmailTemplateDTO(messagingChanelType, sqlTrx);

            string messageSubject = MessageSubjectFormatter(emailTemplateDTO.Description);
            string messageBody = MessageBodyFormatter(emailTemplateDTO.EmailTemplate);
            messageBody = AppNotificationMessageBodyFormatter(messagingChanelType, messageSubject, messageBody);
            MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO = new MessagingClientFunctionLookUpDTO(-1, -1,
                                                                                           MessagingClientDTO.SourceEnumToString(messagingChanelType), -1, -1, -1, null, null);
            messagingClientFunctionLookUpDTO.MessagingClientDTO = new MessagingClientDTO();
            SaveMessagingRequestDTO(messageSubject, messageBody, messagingClientFunctionLookUpDTO, sqlTrx);
            log.LogMethodExit();
        }
         

        protected override EmailTemplateDTO GetEmailTemplateDTO(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messagingChanelType);
            EmailTemplateDTO messageTemplateDTO = null;
            try
            {
                string msgSubject = this.messagingTriggerDTO.EmailSubject;
                string msgBody = (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL
                                         ? this.messagingTriggerDTO.EmailTemplate : this.messagingTriggerDTO.SMSTemplate);
                messageTemplateDTO = new EmailTemplateDTO(-1, PURCHASE_MESSAGE_TRIGGER, msgSubject, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), msgBody);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while generating the message template DTO", ex);
                throw;
            }
            if (messageTemplateDTO == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2881, messagingChanelType.ToString(), this.messagingTriggerDTO.TriggerName);
                //'Unable to fetch &1 template for &2'
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage);
            }

            log.LogMethodExit(messageTemplateDTO);
            return messageTemplateDTO;
        }

        /// <summary>
        /// GetToDevice
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <returns></returns>
        protected override string GetToDevice(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry(); 
            CustomerDTO customerDTO = this.transaction != null && this.transaction.customerDTO != null ? this.transaction.customerDTO
                                           : this.transaction != null && this.transaction.PrimaryCard != null && this.transaction.PrimaryCard.customerDTO != null
                                           ? this.transaction.PrimaryCard.customerDTO : null;
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
            log.Info(messagingClientFunctionLookUpDTO.MessageType);
            if (MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.EMAIL)
            {
                if (this.messagingTriggerDTO.ReceiptTemplateId != null && this.messagingTriggerDTO.ReceiptTemplateId > -1 && this.transaction != null)
                {
                    this.transaction.TransactionInfo.createTransactionInfo(this.transaction.Trx_id);

                    PrinterDTO printerDTO = new PrinterDTO(-1, "Default", "Default", 0, true, DateTime.Now, "", DateTime.Now, "", "", "", -1, PrinterDTO.PrinterTypes.ReceiptPrinter, -1, "", false, -1,-1, 0);
                    int printTemplateId = (int)messagingTriggerDTO.ReceiptTemplateId;
                    Printer.ReceiptPrintTemplateHeaderDTO receiptPrintTemplateDTO = new ReceiptPrintTemplateHeaderBL(executionContext, printTemplateId, true).ReceiptPrintTemplateHeaderDTO;
                    POS.POSPrinterDTO posPrinterDTO = new POS.POSPrinterDTO(-1, utilities.ParafaitEnv.POSMachineId, -1, -1, -1, -1, printTemplateId, printerDTO, null, receiptPrintTemplateDTO, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1, -1);
                    List<POS.POSPrinterDTO> posPrintersDTOList = new List<POS.POSPrinterDTO>();
                    posPrintersDTOList.Add(posPrinterDTO);
                    PrintTransaction printTransaction = new PrintTransaction(posPrintersDTOList);
                    string base64ImageCopy = printTransaction.printPosReceipt(this.transaction, posPrinterDTO, -1, 303, -1, false);
                    if (string.IsNullOrWhiteSpace(base64ImageCopy) == false)
                    {//ReceiptClass Content = POSPrint.PrintReceipt(this.transaction, posPrinterDTO, false);
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
                    }
                    //body += "<img src=\"cid:" + contentID + "\">";
                }
            }
            log.LogMethodExit(attachFile);
            return attachFile;
        }

    }
}
