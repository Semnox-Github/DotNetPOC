/********************************************************************************************
 * Project Name - RedemptionMessageTriggerEventBL
 * Description  - BL class of the Redemption Message Trigger Event   
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************  
 *2.110.0     13-Dec-2020    Guru S A             Created for Subscription changes            
 *2.140.2     18-Apr-2022     Girish Kundar    Modified : Added new column Models to printer
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Redemption;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;

namespace Semnox.Parafait.ConcurrentManager
{
    /// <summary>
    /// RedemptionMessageTriggerEventBL
    /// </summary>
    public class RedemptionMessageTriggerEventBL : ParafaitFunctionEventBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MessagingTriggerDTO messagingTriggerDTO;
        private RedemptionDTO redemptionDTO;
        private CustomerDTO customerDTO;
        private AccountDTO accountDTO;
        private Utilities utilities;

        private const string REDEMPTION_MESSAGE_TRIGGER = "REDEMPTION_MESSAGE_TRIGGER";
        private const string MESSAGING_TRIGGER_DTO = "MessagingTriggerDTO";
        private const char MESSAGE_TYPE_BOTH = 'B'; 
        private const string IMAGE_DIRECTORY = "IMAGE_DIRECTORY";
        /// <summary>
        /// RedemptionMessageTriggerEventBL
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="executionContext"></param>
        /// <param name="messagingTriggerDTO"></param>
        /// <param name="redemptionDTO"></param>
        /// <param name="customerDTO"></param>
        /// <param name="accountDTO"></param>
        public RedemptionMessageTriggerEventBL(Utilities utilities, ExecutionContext executionContext, MessagingTriggerDTO messagingTriggerDTO, RedemptionDTO redemptionDTO, CustomerDTO customerDTO, AccountDTO accountDTO)
            : base(executionContext, ParafaitFunctionEvents.REDEMPTION_MESSAGE_TRIGGER_EVENT, null)
        {
            log.LogMethodEntry(executionContext, messagingTriggerDTO, redemptionDTO, customerDTO, accountDTO);
            this.utilities = utilities;
            this.messagingTriggerDTO = messagingTriggerDTO;
            this.redemptionDTO = redemptionDTO;
            this.customerDTO = customerDTO;
            this.accountDTO = accountDTO;
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
                MessagingTriggerRedemptionContentBuilder messagingTriggerRedemptionContentBuilder = new MessagingTriggerRedemptionContentBuilder(executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName, messagingTriggerDTO, this.redemptionDTO, customerDTO, accountDTO);
                messageSubjectContent = messagingTriggerRedemptionContentBuilder.GenerateMessageSubjectForRedemption(messageTemplateSubject); 
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
            MessagingTriggerRedemptionContentBuilder messagingTriggerRedemptionContentBuilder = new MessagingTriggerRedemptionContentBuilder(executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName, messagingTriggerDTO, this.redemptionDTO, customerDTO, accountDTO);
            messageBodyContent = messagingTriggerRedemptionContentBuilder.GenerateMessageContentForRedemption(messageTemplateContent);
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
                AppNotificationContentBuilder.AppNotificationType appNotificationType = AppNotificationContentBuilder.AppNotificationType.REDEMPTION_TRIGGER;
                string redemptionId = this.redemptionDTO.RedemptionId.ToString();
                messageBody = messagingTriggerAppNotificationContentBuilder.FormatAppNotificationContent(messageSubject, messageBody, appNotificationType, null, redemptionId, null);
            }
            log.LogMethodExit(messageBody);
            return messageBody;
        }

        /// <summary>
        /// SendMessage
        /// </summary>
        /// <param name="messagingChanelType"></param>
        /// <param name="sqlTrx"></param>
        public void SendMessage(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(messagingChanelType, sqlTrx);
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
                if (messagingChanelType == MessagingClientDTO.MessagingChanelType.NONE 
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
                    SendWhatsApp(sqlTrx);
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
        /// SaveMessagingRequestDTO
        /// </summary>
        /// <param name="messageSubject"></param>
        /// <param name="messageBody"></param>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <param name="sqlTrx"></param>
        protected override void SaveMessagingRequestDTO(string messageSubject, string messageBody,
                        MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messageSubject, messageBody, messagingClientFunctionLookUpDTO, sqlTrx);
            string messageReference = GetMessageReference();
            string countryCode = null;
            int messagingClientId = messagingClientFunctionLookUpDTO.MessageClientId > -1 && messagingClientFunctionLookUpDTO.MessagingClientDTO != null
                                         ? messagingClientFunctionLookUpDTO.MessagingClientDTO.ClientId : messagingClientFunctionLookUpDTO.MessageClientId;
            string messageType = messagingClientFunctionLookUpDTO.MessageType;
            string toEmails = GetToEmails(messagingClientFunctionLookUpDTO);
            string toMobile = GetToMobileNumber(messagingClientFunctionLookUpDTO);
            int? cardId = GetCardId();
            string attachFile = GetAttachFile(messagingClientFunctionLookUpDTO);
            string toDevice = GetToDevice(messagingClientFunctionLookUpDTO);
            int customerId = GetCustomerId(messagingClientFunctionLookUpDTO);
            if (messagingClientFunctionLookUpDTO.MessageType == "S" || messagingClientFunctionLookUpDTO.MessageType == "W")
            {
                countryCode = GetCountryCode();
            }
            CreateMessagingRequest(messageSubject, messageBody, messagingClientFunctionLookUpDTO, messageReference,
                messagingClientId, messageType, toEmails, toMobile, cardId, customerId, attachFile, toDevice, sqlTrx, countryCode);
            log.LogMethodExit();
        }

        protected string GetCountryCode()
        {
            log.LogMethodEntry();
            string countryCode = null;
            if (customerDTO != null)
            {
                ContactDTO contactDTO = null;
                if (customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Count > 0)
                {
                    contactDTO = customerDTO.ContactDTOList.Where((x) => x.ContactType == ContactType.PHONE && x.IsActive).FirstOrDefault();
                    if (contactDTO == null)
                    {
                        contactDTO = customerDTO.ContactDTOList.Where((x) => x.ContactType == ContactType.PHONE && x.IsActive).OrderByDescending((x) => x.LastUpdateDate).FirstOrDefault();
                    }
                }
                if (contactDTO != null && contactDTO.CountryId != -1)
                {
                    countryCode = CountryContainerList.GetCountryCode(executionContext.GetSiteId(), contactDTO.CountryId);
                }
            }
            log.LogMethodExit(countryCode);
            return countryCode;
        }

        private string GetMessageReference()
        {
            log.LogMethodEntry();
            string messageReference = string.Empty; 
            messageReference = ParafaitFunctionEvents.REDEMPTION_MESSAGE_TRIGGER_EVENT.ToString(); 
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
                toEmails = this.customerDTO.Email; 
            }
            //toEmails = "guruprasada.adamma@semnox.com";
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
                toPhoneNumber = this.customerDTO.PhoneNumber; 
            }
            else if (MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.WHATSAPP_MESSAGE)
            {
                toPhoneNumber = CustomerEventsBL.GetCustomerLevelWhatsAppsNumber(this.customerDTO);
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
            int? cardId = (redemptionDTO.CardId > -1 ? redemptionDTO.CardId : (int?)null);
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
                if (this.messagingTriggerDTO.ReceiptTemplateId != null && this.messagingTriggerDTO.ReceiptTemplateId > -1 && this.redemptionDTO != null)
                {
                    PrinterDTO printerDTO = new PrinterDTO(-1, "Default", "Default", 0, true, DateTime.Now, "", DateTime.Now, "", "", "", -1, PrinterDTO.PrinterTypes.ReceiptPrinter, -1, "", false, -1,-1, 0);
                    int printTemplateId = (int)messagingTriggerDTO.ReceiptTemplateId;
                    Printer.ReceiptPrintTemplateHeaderDTO receiptPrintTemplateDTO = new ReceiptPrintTemplateHeaderBL(executionContext, printTemplateId, true).ReceiptPrintTemplateHeaderDTO;
                    POS.POSPrinterDTO posPrinterDTO = new POS.POSPrinterDTO(-1, utilities.ParafaitEnv.POSMachineId, -1, -1, -1, -1, printTemplateId, printerDTO, null, receiptPrintTemplateDTO, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1, -1);
                    List<POS.POSPrinterDTO> posPrintersDTOList = new List<POS.POSPrinterDTO>();
                    posPrintersDTOList.Add(posPrinterDTO);
                    PrintRedemptionReceipt printRedemptionReceipt = new PrintRedemptionReceipt(executionContext, utilities);
                    string base64ImageCopy = printRedemptionReceipt.PrintRedemptionReceiptString(redemptionDTO, posPrinterDTO, (int)this.messagingTriggerDTO.ReceiptTemplateId, 303, -1);
                    if (string.IsNullOrEmpty(base64ImageCopy) == false)
                    {
                        string imageFolder = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, IMAGE_DIRECTORY);
                        if (string.IsNullOrWhiteSpace(imageFolder))
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2902, IMAGE_DIRECTORY));//&1 is not set
                        }
                        imageFolder = imageFolder + "\\";
                        string contentID = Guid.NewGuid().ToString() + ".gif";
                        attachFile = imageFolder + contentID;
                        Image attachmentCopy = GenericUtils.ConvertBase64StringToImage(base64ImageCopy);
                        attachmentCopy.Save(attachFile, System.Drawing.Imaging.ImageFormat.Gif);
                    }
                }
            }
            log.LogMethodExit(attachFile);
            return attachFile;
        }

        /// <summary>
        /// GetCustomerId
        /// </summary>
        /// <returns></returns>
        protected override int GetCustomerId(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO);
            int customerId = (customerDTO != null ? customerDTO.Id : -1); 
            log.LogMethodExit(customerId);
            return customerId;
        }
        protected void SendEMail(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            SendMsg(MessagingClientDTO.MessagingChanelType.EMAIL, sqlTrx); 
            log.LogMethodExit();
        }

        protected void SendSMS(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            SendMsg(MessagingClientDTO.MessagingChanelType.SMS, sqlTrx); 
            log.LogMethodExit();
        }        
        protected void SendAppNotification(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            SendMsg(MessagingClientDTO.MessagingChanelType.APP_NOTIFICATION, sqlTrx);
            log.LogMethodExit();
        }

        protected void SendWhatsApp(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            SendMsg(MessagingClientDTO.MessagingChanelType.WHATSAPP_MESSAGE, sqlTrx); 
            log.LogMethodExit();
        }
        protected void SendMsg(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messagingChanelType, sqlTrx);
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
        protected EmailTemplateDTO GetEmailTemplateDTO(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messagingChanelType);
            EmailTemplateDTO messageTemplateDTO = null;
            try
            {
                string msgSubject = this.messagingTriggerDTO.EmailSubject;
                string msgBody = (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL
                                         ? this.messagingTriggerDTO.EmailTemplate : this.messagingTriggerDTO.SMSTemplate);
                messageTemplateDTO = new EmailTemplateDTO(-1, REDEMPTION_MESSAGE_TRIGGER, msgSubject, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), msgBody);
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
    }
}