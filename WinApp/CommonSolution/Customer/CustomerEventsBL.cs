/********************************************************************************************
 * Project Name - CustomerEventsBL 
 * Description  -BL class of the Customer function Events
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     12-Dec-2020    Guru S A           Created for Subscription changes                                                                               
 **************************************************************coyntry******************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Customer
{

    /// <summary>
    /// CustomerEventsBL
    /// </summary>
    public class CustomerEventsBL : ParafaitFunctionEventBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private readonly ExecutionContext executionContext;
        protected CustomerDTO customerDTO;
        private readonly List<ParafaitFunctionEvents> customerFunctionEventList = new ParafaitFunctionEvents[]{ParafaitFunctionEvents.CAMPAIGN_MESSAGE_EVENT,
                                                                                                       ParafaitFunctionEvents.CARD_EXPIRY_MESSAGE_TRIGGER_EVENT,
                                                                                                       ParafaitFunctionEvents.CUSTOMER_VERIFICATION_EVENT,
                                                                                                       ParafaitFunctionEvents.LINK_CUSTOMER_ACCOUNT_EVENT,
                                                                                                       ParafaitFunctionEvents.NEW_REGISTRATION_EVENT,
                                                                                                       ParafaitFunctionEvents.REGISTRATION_LINK_EVENT,
                                                                                                       ParafaitFunctionEvents.RESET_PASSWORD_EVENT }.ToList();
        private const string CUSTOMER_VERIFICATION_MSG_REF = "Customer Verification";
        private const string LINK_CUSTOMER_ACCOUNT_MSG_REF = "Link Account to Customer Email";
        private const string NEW_REGISTRATION_MSG_REF = "New Customer Registration";
        private const string REGISTRATION_LINK_MSG_REF = "Customer Registration Verification";
        private const string RESET_PASSWORD_MSG_REF = "Reset Customer Password";
        /// <summary>
        /// CustomerEventsBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parafaitFunctionEvents"></param>
        /// <param name="customerDTO"></param>
        /// <param name="sqlTransaction"></param>
        public CustomerEventsBL(ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents, CustomerDTO customerDTO, SqlTransaction sqlTransaction = null)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext, parafaitFunctionEvents, customerDTO, sqlTransaction);
            this.customerDTO = customerDTO;
            ValidCustomerFunctionEvent(parafaitFunctionEvents);
            LoadParafaitFunctionEventDTO(parafaitFunctionEvents, sqlTransaction);
            log.LogMethodExit();
        }

        private void ValidCustomerFunctionEvent(ParafaitFunctionEvents parafaitFunctionEvents)
        {
            log.LogMethodEntry(parafaitFunctionEvents);
            if (customerFunctionEventList.Contains(parafaitFunctionEvents) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2882, parafaitFunctionEvents));//"&1 is not a valid customer function event
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
                CustomerMessageContentBuilder customerMessageContentBuilder = new CustomerMessageContentBuilder(executionContext, customerDTO, null, this.parafaitFunctionEventDTO.ParafaitFunctionEventName);
                messageSubjectContent = customerMessageContentBuilder.GenerateMessageSubject(messageTemplateSubject);
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
            CustomerMessageContentBuilder customerMessageContentBuilder = new CustomerMessageContentBuilder(executionContext, customerDTO, null, this.parafaitFunctionEventDTO.ParafaitFunctionEventName);
            messageBodyContent = customerMessageContentBuilder.GenerateMessageContent(messageTemplateContent);
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
                //When MessagingClientFunctionLookUp is not defined 
                if (parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.NEW_REGISTRATION_EVENT ||
                   parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.CUSTOMER_VERIFICATION_EVENT ||
                    parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.LINK_CUSTOMER_ACCOUNT_EVENT)
                {
                    if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
                    {
                        SendEMail(sqlTrx);
                    }
                    if (messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS)
                    {
                        SendSMS(sqlTrx);
                    }
                }
                if (parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.REGISTRATION_LINK_EVENT ||
                    parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.RESET_PASSWORD_EVENT )
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
            CreateMessagingRequest(messageSubject, messageBody, messagingClientFunctionLookUpDTO, messageReference, messagingClientId, messageType, toEmails, toMobile, cardId, customerId, attachFile, toDevice, sqlTrx, countryCode);
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
            switch (this.parafaitFunctionEventDTO.ParafaitFunctionEventName)
            {
                case ParafaitFunctionEvents.CAMPAIGN_MESSAGE_EVENT:
                    messageReference = ParafaitFunctionEvents.CAMPAIGN_MESSAGE_EVENT.ToString();
                    break;
                case ParafaitFunctionEvents.CARD_EXPIRY_MESSAGE_TRIGGER_EVENT:
                    messageReference = ParafaitFunctionEvents.CARD_EXPIRY_MESSAGE_TRIGGER_EVENT.ToString();
                    break;
                case ParafaitFunctionEvents.CUSTOMER_VERIFICATION_EVENT:
                    messageReference = CUSTOMER_VERIFICATION_MSG_REF;
                    break;
                case ParafaitFunctionEvents.LINK_CUSTOMER_ACCOUNT_EVENT:
                    messageReference = LINK_CUSTOMER_ACCOUNT_MSG_REF;
                    break;
                case ParafaitFunctionEvents.NEW_REGISTRATION_EVENT:
                    messageReference = NEW_REGISTRATION_MSG_REF;
                    break;
                case ParafaitFunctionEvents.REGISTRATION_LINK_EVENT:
                    messageReference = REGISTRATION_LINK_MSG_REF;
                    break;
                case ParafaitFunctionEvents.RESET_PASSWORD_EVENT:
                    messageReference = RESET_PASSWORD_MSG_REF;
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
                    case ParafaitFunctionEvents.CAMPAIGN_MESSAGE_EVENT: 
                    case ParafaitFunctionEvents.CARD_EXPIRY_MESSAGE_TRIGGER_EVENT: 
                    case ParafaitFunctionEvents.CUSTOMER_VERIFICATION_EVENT: 
                    case ParafaitFunctionEvents.LINK_CUSTOMER_ACCOUNT_EVENT:
                    case ParafaitFunctionEvents.REGISTRATION_LINK_EVENT:
                    case ParafaitFunctionEvents.RESET_PASSWORD_EVENT:
                        toEmails = this.customerDTO.Email;
                        break;
                    case ParafaitFunctionEvents.NEW_REGISTRATION_EVENT:
                        toEmails = GetEMailAddress();
                        break;   
                    default:
                        break;
                }
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
                switch (this.parafaitFunctionEventDTO.ParafaitFunctionEventName)
                {
                    case ParafaitFunctionEvents.CAMPAIGN_MESSAGE_EVENT: 
                    case ParafaitFunctionEvents.CARD_EXPIRY_MESSAGE_TRIGGER_EVENT: 
                    case ParafaitFunctionEvents.CUSTOMER_VERIFICATION_EVENT: 
                    case ParafaitFunctionEvents.LINK_CUSTOMER_ACCOUNT_EVENT:
                    case ParafaitFunctionEvents.REGISTRATION_LINK_EVENT:
                    case ParafaitFunctionEvents.RESET_PASSWORD_EVENT:
                        toPhoneNumber = this.customerDTO.PhoneNumber;
                        break;
                    case ParafaitFunctionEvents.NEW_REGISTRATION_EVENT:
                        toPhoneNumber = GetPhoneNumber();
                        break; 
                    default:
                        break;
                }
            }
            if (MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.WHATSAPP_MESSAGE)
            {
                switch (this.parafaitFunctionEventDTO.ParafaitFunctionEventName)
                {
                    case ParafaitFunctionEvents.CAMPAIGN_MESSAGE_EVENT:
                    case ParafaitFunctionEvents.CARD_EXPIRY_MESSAGE_TRIGGER_EVENT:
                    case ParafaitFunctionEvents.CUSTOMER_VERIFICATION_EVENT:
                    case ParafaitFunctionEvents.LINK_CUSTOMER_ACCOUNT_EVENT:
                    case ParafaitFunctionEvents.REGISTRATION_LINK_EVENT:
                    case ParafaitFunctionEvents.RESET_PASSWORD_EVENT:
                        toPhoneNumber = GetCustomerLevelWhatsAppsNumber(customerDTO);
                        break;
                    case ParafaitFunctionEvents.NEW_REGISTRATION_EVENT:
                        toPhoneNumber = GetWhatsAppsNumber(customerDTO);
                        break;
                    default:
                        break;
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
        private string GetEMailAddress()
        {
            log.LogMethodEntry();
            string emailAddress = string.Empty;
            if (customerDTO != null)
            {
                if (string.IsNullOrWhiteSpace(customerDTO.UserName) == false)
                {
                    emailAddress = customerDTO.UserName;
                }
                if (string.IsNullOrWhiteSpace(emailAddress) && customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Any(x => x.ContactType == ContactType.EMAIL))
                {
                    ContactDTO emailContactDTO = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL).OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
                    if (emailContactDTO != null)
                    {
                        emailAddress = emailContactDTO.Attribute1;
                    }
                }
            }
            log.LogMethodExit(emailAddress);
            return emailAddress;
        } 
        private string GetPhoneNumber()
        {
            log.LogMethodEntry();
            string phoneNumber = string.Empty;
            if (customerDTO != null)
            {
                if (string.IsNullOrWhiteSpace(customerDTO.PhoneNumber) == false)
                {
                    phoneNumber = customerDTO.PhoneNumber;
                }
                if (string.IsNullOrWhiteSpace(phoneNumber) && customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Any(x => x.ContactType == ContactType.PHONE))
                {
                    ContactDTO phoneContactDTO = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.PHONE).OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
                    if (phoneContactDTO != null)
                    {
                        phoneNumber = phoneContactDTO.Attribute1;
                    }
                }
            }
            log.LogMethodExit(phoneNumber);
            return phoneNumber;
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

        protected void SendWhatsApps(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            SendMsg(MessagingClientDTO.MessagingChanelType.WHATSAPP_MESSAGE, sqlTrx);
            log.LogMethodExit();
        }

        protected virtual void SendMsg(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx)
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
            }
            catch (Exception ex)
            {
                log.Error("Error occured while retrieving the email template for "+ templateName, ex);
            }
            if (emailTemplateDTO == null)
            {
                if (parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.RESET_PASSWORD_EVENT)
                {
                    emailTemplateDTO = CreateDefaultEmailDTOForResetPassword();
                }
                else if (parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.CUSTOMER_VERIFICATION_EVENT)
                {
                    emailTemplateDTO = CreateDefaultDTOCustomerVerification(messagingChanelType);
                }
                else
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2881, messagingChanelType.ToString(), this.ParafaitFunctionEventDTO.ParafaitFunctionEventName.ToString());
                    //'Unable to fetch &1 template for &2'
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
            }
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;

        } 
        private string GetEventBasedTemplateNames(MessagingClientDTO.MessagingChanelType messagingChanelType)
        {
            log.LogMethodEntry(messagingChanelType);
            string templateName = string.Empty;
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.NEW_REGISTRATION_EVENT)
            {
                if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
                {
                    templateName =ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_REGISTRATION_EMAIL_TEMPLATE");
                }
                else if (messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS)
                {
                    templateName =ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_REGISTRATION_SMS_TEMPLATE");
                }

            }
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.REGISTRATION_LINK_EVENT)
            {
                templateName = "ONLINE_CUSTOMER_ACTIVATION_EMAIL_TEMPLATE";
                if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
                {
                    templateName =ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_REGISTRATION_EMAIL_TEMPLATE");
                }
                else if (messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS)
                {
                    templateName =ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_REGISTRATION_SMS_TEMPLATE");
                }
            }
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.RESET_PASSWORD_EVENT)
            {
                templateName =ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ONLINE_PASSWORD_RESET_EMAIL_TEMPLATE"); 
            }
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.LINK_CUSTOMER_ACCOUNT_EVENT)
            {
                if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
                {
                    templateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ONLINE_CARD_REGISTRATION_EMAIL_TEMPLATE");
                }
                else if (messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS)
                {
                    templateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ONLINE_CARD_REGISTRATION_SMS_TEXT_TEMPLATE");
                }
            }
            log.LogMethodExit(templateName);
            return templateName;
        }
        private EmailTemplateDTO CreateDefaultEmailDTOForResetPassword()
        {
            log.LogMethodEntry();
            EmailTemplateDTO emailTemplateDTO = new EmailTemplateDTO();
            emailTemplateDTO.Description = MessageContainerList.GetMessage(executionContext, "Account Information");
            emailTemplateDTO.EmailTemplate = MessageContainerList.GetMessage(executionContext, " Hi ") + "@FirstName" +
                                               Environment.NewLine + Environment.NewLine +
                                               MessageContainerList.GetMessage(executionContext,
                                                   "<p>Password Reset Token could not be sent -  Email Template Missing </p>") +
                                               Environment.NewLine + Environment.NewLine +
                                               MessageContainerList.GetMessage(executionContext, "Thank you");
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;
        }

        private EmailTemplateDTO CreateDefaultDTOCustomerVerification(MessagingClientDTO.MessagingChanelType messagingChanelType)
        {
            log.LogMethodEntry(messagingChanelType);
            EmailTemplateDTO emailTemplateDTO = null;
            if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
            {
                emailTemplateDTO = CreateDefaultEmailDTOCustomerVerification();
            }
            if (messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS)
            {
                emailTemplateDTO = CreateDefaultSMSDTOCustomerVerification();
            }
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;
        }
        private EmailTemplateDTO CreateDefaultEmailDTOCustomerVerification()
        {
            log.LogMethodEntry();
            EmailTemplateDTO emailTemplateDTO = new EmailTemplateDTO(); 
            emailTemplateDTO.Description = "@siteName" + MessageContainerList.GetMessage(executionContext, " - customer registration verification");
            emailTemplateDTO.EmailTemplate = MessageContainerList.GetMessage(executionContext, "Dear ") + "@FirstName" + ", " +
                                              Environment.NewLine + Environment.NewLine +
                                               MessageContainerList.GetMessage(executionContext, "Your registration verification code is ") + "@verificationCode" + " ." +
                                              Environment.NewLine + Environment.NewLine +
                                               MessageContainerList.GetMessage(executionContext, "Thank you") +
                                              Environment.NewLine + "@siteName";
            return emailTemplateDTO;
        } 
        private EmailTemplateDTO CreateDefaultSMSDTOCustomerVerification()
        {
            log.LogMethodEntry();
            EmailTemplateDTO emailTemplateDTO = new EmailTemplateDTO(); 
            emailTemplateDTO.Description = MessageContainerList.GetMessage(executionContext, "Verification Code ");
            emailTemplateDTO.EmailTemplate = MessageContainerList.GetMessage(executionContext, "Dear ") + "@FirstName" + ", "+ 
                                               MessageContainerList.GetMessage(executionContext, "Your registration verification code is ") + "@verificationCode" + " ." +
                                                MessageContainerList.GetMessage(executionContext, "Thank you.") + "@siteName"; 
            return emailTemplateDTO;
        }
        /// <summary>
        /// Get Customer Level WhatsAppsNumber
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <returns></returns>
        public static string GetCustomerLevelWhatsAppsNumber(CustomerDTO customerDTO)
        {
            log.LogMethodEntry();
            string whatsAppNumber = string.Empty;

            log.Info("OptOutWhatsApp: " + (customerDTO != null && customerDTO.ProfileDTO != null ? customerDTO.ProfileDTO.OptOutWhatsApp.ToString() : string.Empty));

            if (customerDTO != null && string.IsNullOrWhiteSpace(customerDTO.PhoneNumber) == false
                 && customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Any()
                 && customerDTO.ProfileDTO != null && customerDTO.ProfileDTO.OptOutWhatsApp == false)
            {
                int index = customerDTO.ContactDTOList.FindIndex(x => x.Attribute1 == customerDTO.PhoneNumber);
                bool whatsAppEnabled = false;
                if (index > -1)
                {
                    whatsAppEnabled = customerDTO.ContactDTOList[index].WhatsAppEnabled;
                }
                if (whatsAppEnabled)
                {
                    whatsAppNumber = customerDTO.PhoneNumber;
                }
            }
            log.LogMethodExit(whatsAppNumber);
            return whatsAppNumber;
        }

        /// <summary>
        /// Get whats app number from customer level or from contacts
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <returns></returns>
        public static string GetWhatsAppsNumber(CustomerDTO customerDTO)
        {
            log.LogMethodEntry();
            string phoneNumber = string.Empty;
            if (customerDTO != null && customerDTO.ProfileDTO != null && customerDTO.ProfileDTO.OptOutWhatsApp == false)
            {
                if (string.IsNullOrWhiteSpace(customerDTO.PhoneNumber) == false )
                {
                    phoneNumber = customerDTO.PhoneNumber;
                }
                if (string.IsNullOrWhiteSpace(phoneNumber) && customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Any(x => x.ContactType == ContactType.PHONE && x.WhatsAppEnabled))
                {
                    ContactDTO phoneContactDTO = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.PHONE && x.WhatsAppEnabled).OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
                    if (phoneContactDTO != null)
                    {
                        phoneNumber = phoneContactDTO.Attribute1;
                    }
                }
            }
            log.LogMethodExit(phoneNumber);
            return phoneNumber;
        }
    }
}
