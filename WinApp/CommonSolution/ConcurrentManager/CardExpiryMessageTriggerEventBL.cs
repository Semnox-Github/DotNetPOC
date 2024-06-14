/********************************************************************************************
 * Project Name - CardExpiryMessageTriggerEventBL 
 * Description  -BL class of the Link Card Expiry Message Trigger Event  
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     13-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using System; 
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.ConcurrentManager
{
    /// <summary>
    /// CardExpiryMessageTriggerEventBL
    /// </summary>
    public class CardExpiryMessageTriggerEventBL : CustomerEventsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AccountDTO accountDTO;
        private MessagingTriggerDTO messagingTriggerDTO;
        private const string CARD_VALIDITY_EXPIRY = "CARD_VALIDITY_EXPIRY";
        private const string MESSAGING_TRIGGER_DTO = "MessagingTriggerDTO";
        private const char MESSAGE_TYPE_BOTH = 'B';
        private const char MESSAGE_TYPE_EMAIL = 'E';
        private const char MESSAGE_TYPE_SMS = 'S';
        private const char MESSAGE_TYPE_APP_NOTIFICATION = 'A';

        /// <summary>
        /// CardExpiryMessageTriggerEventBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="messagingTriggerDTO"></param>
        /// <param name="customerDTO"></param>
        /// <param name="accountDTO"></param>
        /// <param name="sqlTransaction"></param>
        public CardExpiryMessageTriggerEventBL(ExecutionContext executionContext, MessagingTriggerDTO messagingTriggerDTO, CustomerDTO customerDTO, AccountDTO accountDTO, SqlTransaction sqlTransaction = null)
            : base(executionContext, ParafaitFunctionEvents.CARD_EXPIRY_MESSAGE_TRIGGER_EVENT, customerDTO, sqlTransaction)
        {
            log.LogMethodEntry(messagingTriggerDTO, accountDTO, sqlTransaction);
            this.messagingTriggerDTO = messagingTriggerDTO;
            this.accountDTO = accountDTO;
            LoadParafaitFunctionEventDTO(ParafaitFunctionEvents.CARD_EXPIRY_MESSAGE_TRIGGER_EVENT, sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// MessageBodyFormatter
        /// </summary>
        /// <param name="messageTemplateSubject"></param>
        /// <returns></returns>
        public override string MessageSubjectFormatter(string messageTemplateSubject)
        {
            log.LogMethodEntry(messageTemplateSubject);
            string messageSubjectContent = string.Empty;
            if (string.IsNullOrWhiteSpace(messageTemplateSubject) == false && messageTemplateSubject.Contains("@"))
            {
                MessagingTriggerMessageContentBuilder messagingTriggerMessageContentBuilder = new MessagingTriggerMessageContentBuilder(executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName, messagingTriggerDTO, customerDTO, accountDTO);
                messageSubjectContent = messagingTriggerMessageContentBuilder.GenerateMessageSubjectForCustomer(messageTemplateSubject);
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
            MessagingTriggerMessageContentBuilder messagingTriggerMessageContentBuilder = new MessagingTriggerMessageContentBuilder(executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName, messagingTriggerDTO, customerDTO, accountDTO);
            messageBodyContent = messagingTriggerMessageContentBuilder.GenerateMessageContentForCustomer(messageTemplateContent);
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
                AppNotificationContentBuilder.AppNotificationType appNotificationType = AppNotificationContentBuilder.AppNotificationType.CARD_VALIDITY; 
                string cardNumber = (this.accountDTO != null ? this.accountDTO.TagNumber : string.Empty); 
                messageBody = messagingTriggerAppNotificationContentBuilder.FormatAppNotificationContent(messageSubject, messageBody, appNotificationType, cardNumber, null, null);
            }
            log.LogMethodExit(messageBody);
            return messageBody;
        }

        /// <summary>
        /// SendMessage
        /// </summary>
        /// <param name="messagingChanelType"></param>
        /// <param name="sqlTrx"></param>
        public override void SendMessage(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx = null)
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
                if (messagingChanelType == MessagingClientDTO.MessagingChanelType.NONE && this.messagingTriggerDTO.MessageType == MESSAGE_TYPE_BOTH)
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
        protected override void SendMsg(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx)
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
         
        protected override EmailTemplateDTO GetEmailTemplateDTO(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messagingChanelType);
            EmailTemplateDTO messageTemplateDTO = null;
            try
            {
                string msgSubject = this.messagingTriggerDTO.EmailSubject;
                string msgBody = (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL
                                         ? this.messagingTriggerDTO.EmailTemplate : this.messagingTriggerDTO.SMSTemplate);
                messageTemplateDTO = new EmailTemplateDTO(-1, CARD_VALIDITY_EXPIRY, msgSubject, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), msgBody);
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
        

        protected override string GetToDevice(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry();
            int customerId = (accountDTO != null ? this.accountDTO.CustomerId : -1);
            string toDeviceName = GetToDeviceName(messagingClientFunctionLookUpDTO, customerId);
            log.LogMethodExit(toDeviceName);
            return toDeviceName;
        }
        protected override int? GetCardId()
        {
            log.LogMethodEntry();
            int? cardId = (this.accountDTO != null ? accountDTO.AccountId: (int?)null);
            log.LogMethodExit(cardId);
            return cardId;
        }
    }
}
