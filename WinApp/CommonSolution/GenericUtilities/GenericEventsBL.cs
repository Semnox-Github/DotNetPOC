/********************************************************************************************
 * Project Name - GenericEventsBL 
 * Description  - BL class of the Generic function Events
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By           Remarks          
 *********************************************************************************************   
 *2.150.0    27-Aug-2022    Yashodhara C H         Created                                                                              
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// GenericEventsBL
    /// </summary>
    public class GenericEventsBL : ParafaitFunctionEventBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<ParafaitFunctionEvents> genericFunctionEventList = new ParafaitFunctionEvents[] {ParafaitFunctionEvents.GENERIC_EVENT,
                                                                                                               ParafaitFunctionEvents.LOGIN_OTP_EVENT,
                                                                                                               ParafaitFunctionEvents.CUSTOMER_DELETE_OTP_EVENT}.ToList();
        private List<KeyValuePair<string, string>> textFields = null;
        private const string GENERIC_EVENT = "Generic Messages";
        private const string LOGIN_EVENT = "Login Messages";
        private const string CUSTOMER_DELETE_EVENT = "Customer Delete Messages";

        /// <summary>
        /// GenericEventsBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parafaitFunctionEvents"></param>
        /// <param name="genericOTPDTO"></param>
        /// <param name="sqlTransaction"></param>
        public GenericEventsBL(ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents, List<KeyValuePair<string, string>> textFields, SqlTransaction sqlTransaction = null)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext, parafaitFunctionEvents, textFields, sqlTransaction);
            this.textFields = textFields;
            ValidGenericFunctionEvent(parafaitFunctionEvents);
            LoadParafaitFunctionEventDTO(parafaitFunctionEvents, sqlTransaction);
            ValidateInputFields();
            log.LogMethodExit();
        }

        private void ValidateInputFields()
        {
            log.LogMethodEntry();
            log.Debug("Validating inputs");
            if (parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.GENERIC_EVENT)
            {
                log.Debug("Validating generic event");
                foreach(KeyValuePair<string, string> inputFields in textFields)
                {
                    if((inputFields.Key.Contains("EMAILID") || 
                       inputFields.Key.Contains("PHONE") &&
                        inputFields.Key.Contains("COUNTRYCODE") ) &&
                       inputFields.Key.Contains("SOURCE") &&
                       inputFields.Key.Contains("EXPIRYTIME") &&
                       inputFields.Key.Contains("OTPNUMBER"))
                    {
                        continue;
                    }
                }
            }
            else if(parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.LOGIN_OTP_EVENT)
            {
                log.Debug("Validating login otp event");
                foreach (KeyValuePair<string, string> inputFields in textFields)
                {
                    if ((inputFields.Key.Contains("EMAILID") ||
                       inputFields.Key.Contains("PHONE") &&
                        inputFields.Key.Contains("COUNTRYCODE")) &&
                       inputFields.Key.Contains("SOURCE") &&
                       inputFields.Key.Contains("EXPIRYTIME") &&
                        inputFields.Key.Contains("OTPNUMBER"))
                    {
                        continue;
                    }
                }
            }
            else if(parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.CUSTOMER_DELETE_OTP_EVENT)
            {
                log.Debug("Validating customer delete otp event");
                foreach (KeyValuePair<string, string> inputFields in textFields)
                {
                    if ((inputFields.Key.Contains("EMAILID") ||
                       inputFields.Key.Contains("PHONE") &&
                        inputFields.Key.Contains("COUNTRYCODE")) &&
                       inputFields.Key.Contains("SOURCE") &&
                       inputFields.Key.Contains("EXPIRYTIME") &&
                        inputFields.Key.Contains("OTPNUMBER"))
                    {
                        continue;
                    }
                }
            }
            else
            {
                log.Debug("Invalid event");
                string errorMessage = MessageContainerList.GetMessage(executionContext, "Invalid Inpuut");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, errorMessage ));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// MessageSubjectFormatter
        /// validates if the event exits in parafaitFunctionEvenList
        /// </summary>
        /// <param name="messageTemplateSubject"></param>
        /// <returns></returns>
        private void ValidGenericFunctionEvent(ParafaitFunctionEvents parafaitFunctionEvents)
        {
            log.LogMethodEntry(parafaitFunctionEvents);
            if(genericFunctionEventList.Contains(parafaitFunctionEvents) == false)
            {
                throw new ValidationException(Parafait.Languages.MessageContainerList.GetMessage(executionContext, 2882, parafaitFunctionEvents));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// MessageSubjectFormatter
        /// Replaces all the variables with the value in the message subject
        /// </summary>
        /// <param name="messageTemplateSubject"></param>
        /// <returns></returns>
        public override string MessageSubjectFormatter(string messageTemplateSubject)
        {
            log.LogMethodEntry(messageTemplateSubject);
            string messageSubjectContent = string.Empty;
            messageSubjectContent = messageTemplateSubject;
            log.LogMethodExit(messageSubjectContent);
            return messageSubjectContent;
        }

        /// <summary>
        /// MessageBodyFormatter
        /// Replaces all the variables with the value in the message body
        /// </summary>
        /// <param name="messageTemplateContent"></param>
        /// <returns></returns>
        public override string MessageBodyFormatter(string messageTemplateContent)
        {
            log.LogMethodEntry(messageTemplateContent);
            string messageBodyContent = string.Empty;
            TemplateKeywordFormatter templateKeywordFormatter = new TemplateKeywordFormatter();
            templateKeywordFormatter.Add("@Source", textFields.Find(x => x.Key.Equals("SOURCE")).Value.ToString());
            templateKeywordFormatter.Add("@ExpiryTime", textFields.Find(x => x.Key.Equals("EXPIRYTIME")).Value.ToString());
            templateKeywordFormatter.Add("@OTPNumber", textFields.Find(x => x.Key.Equals("OTPNUMBER")).Value.ToString());
            TemplateText templateText = new TemplateText(messageTemplateContent);
            messageBodyContent = templateKeywordFormatter.Format(templateText);
            log.LogMethodExit(messageBodyContent);
            return messageBodyContent;
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
                log.Debug("Messaging client is mapped, send mail using this");
                foreach (MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO in parafaitFunctionEventDTO.MessagingClientFunctionLookUpDTOList)
                {
                    log.Debug("Calling Build and Send Message for channel " + messagingChanelType + ":client:" + messagingClientFunctionLookUpDTO.Id);
                    BuildAndSendMessage(messagingChanelType, messagingClientFunctionLookUpDTO, sqlTrx);
                }
            }
            else
            {
                log.Debug("No messaging client is set up, use the default clients");
                //When MessagingClientFunctionLookUp is not defined 
                if (parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.GENERIC_EVENT ||
                   parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.LOGIN_OTP_EVENT ||
                    parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.CUSTOMER_DELETE_OTP_EVENT)
                {
                    if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
                    {
                        log.Debug("Sending email");
                        SendEMail(sqlTrx);
                    }
                    if (messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS)
                    {
                        log.Debug("Sending SMS");
                        SendSMS(sqlTrx);
                    }
                }
            }

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
            log.Debug("Messaging client id " + messagingClientId);

            string messageType = messagingClientFunctionLookUpDTO.MessageType;
            log.Debug("messageType " + messageType);

            string toEmails = GetToEmails(messagingClientFunctionLookUpDTO);
            log.Debug("toEmails " + toEmails);

            string toMobile = GetToMobileNumber(messagingClientFunctionLookUpDTO);
            log.Debug("toMobile " + toMobile);

            int? cardId = -1;
            string attachFile = GetAttachFile(messagingClientFunctionLookUpDTO);
            string toDevice = GetToDevice(messagingClientFunctionLookUpDTO);
            int customerId = GetCustomerId(messagingClientFunctionLookUpDTO);
            if (messagingClientFunctionLookUpDTO.MessageType == "S")
            {
                countryCode = GetCountryCode();
            }
            CreateMessagingRequest(messageSubject, messageBody, messagingClientFunctionLookUpDTO, messageReference, messagingClientId, messageType, toEmails, toMobile, cardId, customerId, attachFile, toDevice, sqlTrx, countryCode);
            log.LogMethodExit();
        }


        private string GetMessageReference()
        {
            log.LogMethodEntry();
            string messageReference = string.Empty;
            switch (this.parafaitFunctionEventDTO.ParafaitFunctionEventName)
            {
                case ParafaitFunctionEvents.GENERIC_EVENT:
                    messageReference = GENERIC_EVENT;
                    break;
                case ParafaitFunctionEvents.LOGIN_OTP_EVENT:
                    messageReference = LOGIN_EVENT;
                    break;
                case ParafaitFunctionEvents.CUSTOMER_DELETE_OTP_EVENT:
                    messageReference = CUSTOMER_DELETE_EVENT;
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
                    case ParafaitFunctionEvents.LOGIN_OTP_EVENT:
                    case ParafaitFunctionEvents.CUSTOMER_DELETE_OTP_EVENT:
                    case ParafaitFunctionEvents.GENERIC_EVENT:
                        toEmails = this.textFields.Find(x => x.Key.Equals("EMAILID")).Value.ToString();
                        break;
                    default:
                        break;
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
                switch (this.parafaitFunctionEventDTO.ParafaitFunctionEventName)
                {
                    case ParafaitFunctionEvents.LOGIN_OTP_EVENT:
                    case ParafaitFunctionEvents.CUSTOMER_DELETE_OTP_EVENT:
                    case ParafaitFunctionEvents.GENERIC_EVENT:
                        toPhoneNumber = this.textFields.Find(x => x.Key.Equals("PHONE")).Value.ToString();
                        break;
                    default:
                        break;
                }
            }
            log.LogMethodExit(toPhoneNumber);
            return toPhoneNumber;
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
        /// GetToDevice
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <returns></returns>
        protected override string GetToDevice(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry();
            string toDeviceName = string.Empty;
            log.LogMethodExit(toDeviceName);
            return toDeviceName;
        }

        /// <summary>
        /// GeCustomerId
        /// </summary>
        /// <returns></returns>
        protected override int GetCustomerId(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO);
            int customerId = -1;
            log.LogMethodExit(customerId);
            return customerId;
        }

        protected string GetCountryCode()
        {
            log.LogMethodEntry();
            string countryCode = string.Empty;
            var ccode = textFields.FirstOrDefault(x => x.Key == "COUNTRYCODE");
            if (ccode.Key != null && ccode.Value != null && !string.IsNullOrEmpty(ccode.Key) && !string.IsNullOrEmpty(ccode.Value))
            {
                countryCode = ccode.Value; 
            }
            log.LogMethodExit(countryCode);
            return countryCode;
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

        protected virtual void SendMsg(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messagingChanelType, sqlTrx);

            log.Debug("Getting email template dto");
            EmailTemplateDTO emailTemplateDTO = GetEmailTemplateDTO(messagingChanelType, sqlTrx);

            log.Debug("Getting message subject");
            string messageSubject = MessageSubjectFormatter(emailTemplateDTO.Description);

            log.Debug("Getting message body");
            string messageBody = MessageBodyFormatter(emailTemplateDTO.EmailTemplate);

            log.Debug("Getting app notification");
            messageBody = AppNotificationMessageBodyFormatter(messagingChanelType, messageSubject, messageBody);

            MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO = new MessagingClientFunctionLookUpDTO(-1, -1,
                                                                                           MessagingClientDTO.SourceEnumToString(messagingChanelType), -1, -1, -1, null, null);
            messagingClientFunctionLookUpDTO.MessagingClientDTO = new MessagingClientDTO();

            log.Debug("Saving message request DTO");
            SaveMessagingRequestDTO(messageSubject, messageBody, messagingClientFunctionLookUpDTO, sqlTrx);
            log.LogMethodExit();
        }

        protected virtual EmailTemplateDTO GetEmailTemplateDTO(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messagingChanelType, sqlTrx);
            EmailTemplateDTO emailTemplateDTO = null;
            log.Debug("Getting email template for channel type " + messagingChanelType);
            string templateName = GetEventBasedTemplateNames(messagingChanelType);
            log.Debug("templateName " + templateName);
            if (string.IsNullOrWhiteSpace(templateName) == false)
            {
                emailTemplateDTO = new EmailTemplate(executionContext).GetEmailTemplate(templateName, executionContext.GetSiteId(), sqlTrx);
                if(emailTemplateDTO == null)
                {
                    log.Debug("Email template not found");
                    throw new ValidationException("Email template " + templateName + " is not set up for site " + executionContext.GetSiteId());
                }
            }
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;

        }

        private string GetEventBasedTemplateNames(MessagingClientDTO.MessagingChanelType messagingChanelType)
        {
            log.LogMethodEntry(messagingChanelType);
            string templateName = string.Empty;
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.GENERIC_EVENT)
            {
                log.Debug("Getting template for generic event");
                if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
                {
                    templateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "GENERIC_EVENT_EMAIL_TEMPLATE");
                }
                else if (messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS)
                {
                    templateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "GENERIC_EVENT_SMS_TEMPLATE");
                }
            }
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.LOGIN_OTP_EVENT)
            {
                log.Debug("Getting template for login otp event");
                if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
                {
                    templateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOGIN_OTP_EVENT_EMAIL_TEMPLATE");
                }
                else if (messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS)
                {
                    templateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOGIN_OTP_EVENT_SMS_TEMPLATE");
                }
            }
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.CUSTOMER_DELETE_OTP_EVENT)
            {
                log.Debug("Getting template for customer delete otp event");
                if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
                {
                    templateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_DELETE_OTP_EVENT_EMAIL_TEMPLATE");
                }
                else if (messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS)
                {
                    templateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_DELETE_OTP_EVENT_SMS_TEMPLATE");
                }
            }
            log.Debug("template name " + templateName);
            if (String.IsNullOrEmpty(templateName))
            {
                throw new ValidationException("No Email template is set up for event " + messagingChanelType + " for site " + executionContext.GetSiteId());
            }

            log.LogMethodExit(templateName);
            return templateName;
        }
    }
}
