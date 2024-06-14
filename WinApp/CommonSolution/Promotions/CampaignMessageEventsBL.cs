/********************************************************************************************
 * Project Name - CampaignMessageEventsBL 
 * Description  -BL class of the Campaign message Events
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     12-Dec-2020    Guru S A             Created for Subscription changes                                                                               
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
using System.Linq;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// CampaignMessageEventsBL
    /// </summary>
    public class CampaignMessageEventsBL : ParafaitFunctionEventBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CAMPAIGN_MSG_REF = "Campaign Messages";
        private const string CAMPAIGN_MESSAGE_EVENT = "Campaign Message";
        private CampaignDTO campaignDTO;
        private CampaignCustomerDTO campaignCustomerDTO;
        private CustomerDTO customerDTO;

        /// <summary>
        /// CampaignMessageEventsBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="campaignDTO"></param>
        /// <param name="campaignCustomerDTO"></param>
        /// <param name="customerDTO"></param>
        /// <param name="sqlTransaction"></param>
        public CampaignMessageEventsBL(ExecutionContext executionContext, CampaignDTO campaignDTO, CampaignCustomerDTO campaignCustomerDTO, CustomerDTO customerDTO,
                                       SqlTransaction sqlTransaction = null)
            : base(executionContext)
        {
            log.LogMethodEntry(campaignDTO, campaignCustomerDTO, customerDTO, sqlTransaction);
            this.campaignDTO = campaignDTO;
            this.campaignCustomerDTO = campaignCustomerDTO;
            this.customerDTO = customerDTO;
            LoadParafaitFunctionEventDTO(ParafaitFunctionEvents.CAMPAIGN_MESSAGE_EVENT, sqlTransaction);
            //LoadAlertSetupDetails();
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
                CampaignMessageContentBuilder campaignMessageContentBuilder = new CampaignMessageContentBuilder(executionContext, this.campaignCustomerDTO, true);
                messageSubjectContent = campaignMessageContentBuilder.GenerateMessageContent(messageTemplateSubject);
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
            CampaignMessageContentBuilder campaignMessageContentBuilder = new CampaignMessageContentBuilder(executionContext, this.campaignCustomerDTO, false);
            messageBodyContent = campaignMessageContentBuilder.GenerateMessageContent(messageTemplateContent);
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
                AppNotificationContentBuilder appNotificationContentBuilder = new AppNotificationContentBuilder(executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName);
                AppNotificationContentBuilder.AppNotificationType appNotificationType = AppNotificationContentBuilder.AppNotificationType.PROMOTION;
                string promotionId = campaignDTO.CampaignId.ToString();
                string cardNumber = campaignCustomerDTO.CardNumber;
                messageBody = appNotificationContentBuilder.FormatAppNotificationContent(messageSubject, messageBody, appNotificationType, cardNumber, null, promotionId);
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
                if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
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
            if ((messagingChanelType != MessagingClientDTO.MessagingChanelType.NONE
                         && messagingChanelType == MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType))
                          || messagingChanelType == MessagingClientDTO.MessagingChanelType.NONE)
            {
                EmailTemplateDTO messageTemplateDTO = null;
                try
                {
                    messageTemplateDTO = GetEmailTemplateDTO(messagingChanelType, sqlTrx);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while retrieving the message template", ex);
                    throw;
                }
                if (messageTemplateDTO == null)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2881, messagingClientFunctionLookUpDTO.MessageType, this.campaignDTO.Name);
                    //'Unable to fetch &1 template for &2'
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                string content = messageTemplateDTO.EmailTemplate;

                string messageBody = MessageBodyFormatter(content);
                string messageSubject = MessageSubjectFormatter(messageTemplateDTO.Description);
                messageBody = AppNotificationMessageBodyFormatter(MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType), messageSubject, messageBody);
                SaveMessagingRequestDTO(messageSubject, messageBody, messagingClientFunctionLookUpDTO, sqlTrx);
            }

            log.LogMethodExit();
        }
        protected EmailTemplateDTO GetEmailTemplateDTO(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messagingChanelType, sqlTrx); 
            EmailTemplateDTO messageTemplateDTO = null;
            try
            {
                string msgSubject = (messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS ? String.Empty : this.campaignDTO.MessageSubject);
                string msgBody = this.campaignDTO.MessageTemplate;
                messageTemplateDTO = new EmailTemplateDTO(-1, CAMPAIGN_MESSAGE_EVENT, msgSubject, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), msgBody);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while generating the message template DTO", ex);
                throw;
            }
            if (messageTemplateDTO == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2881, messagingChanelType.ToString(), this.campaignDTO.Name);
                //'Unable to fetch &1 template for &2'
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit(messageTemplateDTO);
            return messageTemplateDTO;
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
            string messageReference = CAMPAIGN_MSG_REF;
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
                toEmails = campaignCustomerDTO.Email;
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
                toPhoneNumber = campaignCustomerDTO.ContactPhone1;
            }
            else if (MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.WHATSAPP_MESSAGE)
            {
                toPhoneNumber = GetCustomerWhatsAppNumber();
            }
            log.LogMethodExit(toPhoneNumber);
            return toPhoneNumber;
        }
        private string GetCustomerWhatsAppNumber()
        {
            log.LogMethodEntry();
            string toPhoneNumber = string.Empty;
            if (customerDTO != null)
            {
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
            string toDeviceName = string.Empty;
            if (campaignCustomerDTO != null)
            {
                toDeviceName = campaignCustomerDTO.NotificationToken;
            }
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
            int customerId = (campaignCustomerDTO != null ? campaignCustomerDTO.CustomerId : -1);
            log.LogMethodExit(customerId);
            return customerId;
        }
    }
}
