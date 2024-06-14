/********************************************************************************************
 * Project Name - MaintenanceEventsBL 
 * Description  -BL class of the Maintenance message Events
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     12-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// MaintenanceEventsBL
    /// </summary>
    public class MaintenanceEventsBL : ParafaitFunctionEventBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MAINTENANCE_REQUEST_MSG_REF = "Maintenance Request Messages";
        private const string MAINTENANCE_REQUEST_MESSAGE_EVENT = "Maintenance Request Message";
        private const string SERVICE_REQUEST_EMAIL_TEMPLATE = "SERVICE_REQUEST_EMAIL_TEMPLATE";
        private UserJobItemsDTO userJobItemsDTO;
        private UsersDTO requestedByUserDTO;
        private UsersDTO assignedToUserDTO;
        private readonly List<ParafaitFunctionEvents> maintenanceFunctionEventList = new ParafaitFunctionEvents[] { ParafaitFunctionEvents.SERVICE_REQUEST_STATUS_CHANGE_EVENT }.ToList();

        /// <summary>
        /// MaintenanceEventsBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parafaitFunctionEvents"></param>
        /// <param name="userJobItemsDTO"></param>
        /// <param name="sqlTransaction"></param>
        public MaintenanceEventsBL(ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents, UserJobItemsDTO userJobItemsDTO, SqlTransaction sqlTransaction = null)
            : base(executionContext)
        {
            log.LogMethodEntry(parafaitFunctionEvents, userJobItemsDTO, sqlTransaction);
            this.userJobItemsDTO = userJobItemsDTO;
            ValidMaintenanceFunctionEvent(parafaitFunctionEvents);
            LoadParafaitFunctionEventDTO(parafaitFunctionEvents, sqlTransaction);
            LoadRequestedByUserDTO(userJobItemsDTO.RequestedBy, sqlTransaction);
            LoadAssignedToUserDTO(userJobItemsDTO.AssignedUserId, sqlTransaction);
            log.LogMethodExit();
        }

        private void ValidMaintenanceFunctionEvent(ParafaitFunctionEvents parafaitFunctionEvents)
        {
            log.LogMethodEntry(parafaitFunctionEvents);
            if (maintenanceFunctionEventList.Contains(parafaitFunctionEvents) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2906, parafaitFunctionEvents));
                //"&1 is not a valid maintenance event
            }
            log.LogMethodExit();
        }
        private void LoadRequestedByUserDTO(string userName, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(userName, sqlTrx);
            string email = String.Empty;
            if (!String.IsNullOrWhiteSpace(userName))
            {
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_NAME, userName));
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                UsersList usersList = new UsersList(executionContext);

                List<UsersDTO> usersDTOList = usersList.GetAllUsers(searchParameter, false, true, sqlTrx);

                if (usersDTOList != null && usersDTOList.Any())
                {
                    requestedByUserDTO = usersDTOList.First();
                }
            }
            log.LogMethodExit();
        }
        private void LoadAssignedToUserDTO(int assignedUserId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(assignedUserId, sqlTransaction);
            if (assignedUserId > 0)
            {
                Users assignedToUser = new Users(executionContext, assignedUserId, false, false, sqlTransaction);
                assignedToUserDTO = assignedToUser.UserDTO;
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
                MaintenanceContentBuilder maintenanceMessageContentBuilder = new MaintenanceContentBuilder(executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName, this.userJobItemsDTO, true);
                messageSubjectContent = maintenanceMessageContentBuilder.GenerateMessageContent(messageTemplateSubject);
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
            MaintenanceContentBuilder maintenanceMessageContentBuilder = new MaintenanceContentBuilder(executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName, this.userJobItemsDTO, false);
            messageBodyContent = maintenanceMessageContentBuilder.GenerateMessageContent(messageTemplateContent);
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
                foreach (MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO in parafaitFunctionEventDTO.MessagingClientFunctionLookUpDTOList)
                {
                    BuildAndSendMessage(messagingChanelType, messagingClientFunctionLookUpDTO, sqlTrx);
                }
            }
            else
            {
                if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.SERVICE_REQUEST_STATUS_CHANGE_EVENT)
                {
                    SendEMail(sqlTrx);
                }
            }
            log.LogMethodExit();
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

        protected virtual void SendMsg(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messagingChanelType, sqlTrx);
            EmailTemplateDTO emailTemplateDTO = GetEmailTemplateDTO(MessagingClientDTO.MessagingChanelType.EMAIL, sqlTrx);

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
            log.LogMethodEntry(messagingChanelType, sqlTrx);
            log.LogMethodEntry(messagingChanelType);
            EmailTemplateDTO messageTemplateDTO = null;
            try
            {
                if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.SERVICE_REQUEST_STATUS_CHANGE_EVENT)
                {
                    EmailTemplate emailTemplate = new EmailTemplate(executionContext, SERVICE_REQUEST_EMAIL_TEMPLATE, sqlTrx);
                    messageTemplateDTO = emailTemplate.EmailTemplateDTO;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while generating the message template DTO", ex);
                throw;
            }
            if (messageTemplateDTO == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2881, messagingChanelType.ToString(), this.parafaitFunctionEventDTO.ParafaitFunctionEventName.ToString());
                //'Unable to fetch &1 template for &2'
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage);
            }

            log.LogMethodExit(messageTemplateDTO);
            return messageTemplateDTO;

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
            string messageReference = MAINTENANCE_REQUEST_MSG_REF;
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
                if (string.IsNullOrWhiteSpace(userJobItemsDTO.ContactEmailId) == false)
                {
                    toEmails = userJobItemsDTO.ContactEmailId;
                }
                toEmails = AppendEmailId(requestedByUserDTO, toEmails);

                toEmails = AppendEmailId(assignedToUserDTO, toEmails);
            } 
            log.LogMethodExit(toEmails);
            return toEmails;
        }

        private string AppendEmailId(UsersDTO userDTO, string toEmails)
        {
            log.LogMethodEntry(userDTO, toEmails);
            if (userDTO != null && string.IsNullOrWhiteSpace(userDTO.Email) == false)
            {
                if (string.IsNullOrWhiteSpace(toEmails))
                {
                    toEmails = userDTO.Email;
                }
                else
                {
                    toEmails = toEmails + "," + userDTO.Email;
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
                toPhoneNumber = userJobItemsDTO.ContactPhone;
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
            //to be developed
            string toPhoneNumber = string.Empty;
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
            //to be developed
            string toDeviceName = string.Empty;
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

    }
}
