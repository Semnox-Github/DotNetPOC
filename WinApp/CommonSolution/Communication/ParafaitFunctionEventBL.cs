/********************************************************************************************
 * Project Name - ParafaitFunctionEventBL 
 * Description  -BL class of the ParafaitFunctionEvent 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     10-Dec-2020    Fiona             Created for Subscription changes                                                                               
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// ParafaitFunctionEventBL
    /// </summary>
    public class ParafaitFunctionEventBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected ParafaitFunctionEventDTO parafaitFunctionEventDTO;
        protected ExecutionContext executionContext; 

        /// <summary>
        /// Default constructor of ParafaitFunctionEventBL class
        /// </summary>
        protected ParafaitFunctionEventBL()
        {
            log.LogMethodEntry();
            parafaitFunctionEventDTO = null;
            this.executionContext = null; 
            log.LogMethodExit();
        }
        /// <summary>
        /// Default constructor of ParafaitFunctionEventBL class
        /// </summary>
        protected ParafaitFunctionEventBL(ExecutionContext executionContext) : this()
        {
            log.LogMethodEntry(executionContext);
            parafaitFunctionEventDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates ParafaitFunctionEventBL object using the ParafaitFunctionEventDTO
        /// </summary>
        /// <param name="ParafaitFunctionEventDTO">ParafaitFunctionEventDTO object</param>
        public ParafaitFunctionEventBL(ExecutionContext executionContext, ParafaitFunctionEventDTO parafaitFunctionEventDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(parafaitFunctionEventDTO);
            this.parafaitFunctionEventDTO = parafaitFunctionEventDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ParafaitFunctionEvent id as the parameter
        /// Would fetch the ParafaitFunctionEvent object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public ParafaitFunctionEventBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction); 
            ParafaitFunctionEventDatahandler parafaitFunctionEventDataHandler = new ParafaitFunctionEventDatahandler(sqlTransaction);
            parafaitFunctionEventDTO = parafaitFunctionEventDataHandler.GetParafaitFunctionEventDTO(id);
            if (parafaitFunctionEventDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ParafaitFunctionEvent", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            BuildChildDTO(sqlTransaction);
            log.LogMethodExit(parafaitFunctionEventDTO);
        }

        public ParafaitFunctionEventBL(ExecutionContext executionContext, ParafaitFunctionEvents eventName, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, eventName, sqlTransaction); 
            LoadParafaitFunctionEventDTO(eventName, sqlTransaction);
            log.LogMethodExit(parafaitFunctionEventDTO);
        }

        protected void LoadParafaitFunctionEventDTO(ParafaitFunctionEvents eventName, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(eventName, sqlTransaction); 
            ParafaitFunctionEventDatahandler parafaitFunctionEventDataHandler = new ParafaitFunctionEventDatahandler(sqlTransaction);
            parafaitFunctionEventDTO = parafaitFunctionEventDataHandler.GetParafaitFunctionEventDTO(eventName);
            if (parafaitFunctionEventDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ParafaitFunctionEvent", eventName);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            BuildChildDTO(sqlTransaction);
            log.LogMethodExit();
        }

        ///// <summary>
        ///// Saves the ParafaitFunctionEvent DTO
        ///// Checks if the  id is not less than or equal to 0
        ///// If it is less than or equal to 0, then inserts
        ///// else updates
        ///// </summary>
        ///// <param name="sqlTransaction"></param>
        //public void Save(SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(sqlTransaction);
        //    ParafaitFunctionEventDatahandler ParafaitFunctionEventDataHandler = new ParafaitFunctionEventDatahandler(sqlTransaction);
        //    if (parafaitFunctionEventDTO.IsChanged == false
        //         && parafaitFunctionEventDTO.ParafaitFunctionId > -1)
        //    {
        //        log.LogMethodExit(null, "Nothing to save.");
        //        return;
        //    }
        //    List<ValidationError> validationErrors = Validate();
        //    if (validationErrors.Any())
        //    {
        //        string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
        //        log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
        //        throw new ValidationException(message, validationErrors);
        //    }
        //    if (parafaitFunctionEventDTO.ParafaitFunctionId < 0)
        //    {
        //        parafaitFunctionEventDTO = ParafaitFunctionEventDataHandler.InsertParafaitFunctionEvent(parafaitFunctionEventDTO, executionContext.GetUserId(), executionContext.GetSiteId());
        //        parafaitFunctionEventDTO.AcceptChanges();
        //    }
        //    else
        //    {
        //        if (parafaitFunctionEventDTO.IsChanged)
        //        {
        //            parafaitFunctionEventDTO = ParafaitFunctionEventDataHandler.UpdateParafaitFunctionEvent(parafaitFunctionEventDTO, executionContext.GetUserId(), executionContext.GetSiteId());
        //            parafaitFunctionEventDTO.AcceptChanges();
        //        }
        //    }

        //    log.LogMethodExit();
        //}
        ///// <summary>
        ///// Validate the ParafaitFunctionEventDTO
        ///// </summary>
        ///// <returns></returns>
        //public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry();
        //    List<ValidationError> validationErrorList = new List<ValidationError>();
        //    if (ParafaitFunctionEventDTO.ParafaitFunctionId==-1)
        //    {
        //        validationErrorList.Add(new ValidationError("ParafaitFunctionEvent", "ParafaitFunctionId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Function Id"))));
        //    }
        //    if (ParafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.None)
        //    {
        //        validationErrorList.Add(new ValidationError("ParafaitFunctionEvent", "ParafaitFunctionEventName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Function Event Name"))));
        //    }
        //    log.LogMethodExit(validationErrorList);
        //    return validationErrorList;
        //}

        /// <summary>
        /// BuildChildDTO
        /// </summary>
        /// <returns></returns>
        private void BuildChildDTO(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(executionContext);
            List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID, parafaitFunctionEventDTO.ParafaitFunctionEventId.ToString()));
            searchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.IS_ACTIVE, "1"));
            parafaitFunctionEventDTO.MessagingClientFunctionLookUpDTOList = messagingClientFunctionLookUpListBL.GetAllMessagingClientFunctionLookUpList(searchParameters, false, false, sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// MessageSubjectFormatter
        /// </summary>
        /// <param name="messageTemplateContext"></param>
        /// <returns></returns>
        public virtual string MessageSubjectFormatter(string messageSubjectContent)
        {
            log.LogMethodEntry(messageSubjectContent);
            string templateContent = messageSubjectContent;
            log.LogMethodExit();
            return templateContent;
        }
        /// <summary>
        /// MessageBodyFormatter
        /// </summary>
        /// <param name="messageTemplateContent"></param>
        /// <returns></returns>
        public virtual string MessageBodyFormatter(string messageTemplateContent)
        {
            log.LogMethodEntry(messageTemplateContent);
            string templateContent = messageTemplateContent;
            log.LogMethodExit();
            return templateContent;
        }
        /// <summary>
        /// AppNotificationMessageBodyFormatter
        /// </summary>
        /// <param name="messagingChanelType"></param>
        /// <param name="messageSubject"></param>
        /// <param name="messageBody"></param> 
        /// <returns></returns>
        protected virtual string AppNotificationMessageBodyFormatter(MessagingClientDTO.MessagingChanelType messagingChanelType, string messageSubject, string messageBody)
        {
            log.LogMethodEntry(messagingChanelType, messageSubject, messageBody);
            if (messagingChanelType == MessagingClientDTO.MessagingChanelType.APP_NOTIFICATION)
            {
                AppNotificationContentBuilder messagingTriggerAppNotificationContentBuilder = new AppNotificationContentBuilder(executionContext, this.parafaitFunctionEventDTO.ParafaitFunctionEventName);
                messageBody = messagingTriggerAppNotificationContentBuilder.FormatAppNotificationContent(messageSubject, messageBody, AppNotificationContentBuilder.AppNotificationType.NONE, null, null, null);
            }
            log.LogMethodExit(messageBody);
            return messageBody;
        }
        /// <summary>
        /// SendMessage - child class needs to implement
        /// </summary>
        /// <param name="sqlTrx"></param>
        public virtual void SendMessage(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            log.LogMethodExit("NotImplementedException");
            throw new NotImplementedException(MessageContainerList.GetMessage(executionContext, "Child class needs to implement")); 
        }
        /// <summary>
        /// BuildAndSendMessage
        /// </summary>
        /// <param name="messagingChanelType"></param>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <param name="sqlTrx"></param>
        protected virtual void BuildAndSendMessage(MessagingClientDTO.MessagingChanelType messagingChanelType, 
            MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messagingChanelType, messagingClientFunctionLookUpDTO, sqlTrx);
            //messagingChanelType == MessagingClientDTO.MessagingChanelType.NONE - means send all. Else send specified type only
            if ((messagingChanelType != MessagingClientDTO.MessagingChanelType.NONE && messagingChanelType == MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType))
                || messagingChanelType == MessagingClientDTO.MessagingChanelType.NONE)
            {
                EmailTemplateDTO messageTemplateDTO = null;
                try
                {
                    EmailTemplate msgTemplate = new EmailTemplate(executionContext, messagingClientFunctionLookUpDTO.MessageTemplateId, sqlTrx);
                    messageTemplateDTO = msgTemplate.EmailTemplateDTO;
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while retrieving the message template", ex);
                    throw;
                }
                if (messageTemplateDTO == null)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2881, messagingClientFunctionLookUpDTO.MessageType, messagingClientFunctionLookUpDTO.MessageTemplateId); //'Unable to fetch &1 template for &2'
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
        protected virtual void SaveMessagingRequestDTO(string messageSubject, string messageBody, 
            MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messageSubject, messageBody, messagingClientFunctionLookUpDTO, sqlTrx);

            log.LogMethodExit();
        }
        /// <summary>
        /// CreateMessagingRequest
        /// </summary>
        /// <param name="messageSubject"></param>
        /// <param name="messageBody"></param>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <param name="messageReference"></param>
        /// <param name="messagingClientId"></param>
        /// <param name="messageType"></param>
        /// <param name="toEmails"></param>
        /// <param name="toMobile"></param>
        /// <param name="cardId"></param>
        /// <param name="customerId"></param>
        /// <param name="attachFile"></param>
        /// <param name="toDevice"></param>
        /// <param name="sqlTrx"></param>
        protected void CreateMessagingRequest(string messageSubject, string messageBody, MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO,
            string messageReference, int messagingClientId, string messageType, string toEmails, string toMobile, int? cardId, int customerId, string attachFile,
            string toDevice, SqlTransaction sqlTrx, string countryCode = null, int? trxId = null, string trx_no = null)
        {
            log.LogMethodEntry(messageSubject, messageBody, messagingClientFunctionLookUpDTO, messageReference, messagingClientId, messageType, toEmails, toMobile, cardId, customerId, attachFile, toDevice, sqlTrx);
            if (trxId <=0)
            {
                trxId = null;
            }
            if (MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.EMAIL && string.IsNullOrWhiteSpace(toEmails))
            {
                string msg = MessageContainerList.GetMessage(executionContext, "No valid Email found to send " + parafaitFunctionEventDTO.ParafaitFunctionEventName.ToString());
                log.Error(msg);
                log.LogMethodExit(msg);
                return;
                //throw new ValidationException(msg);
            }
            if (MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.SMS)
            {
                if (string.IsNullOrWhiteSpace(toMobile))
                { 
                    string msg = MessageContainerList.GetMessage(executionContext, "No valid Phone number found to send " + parafaitFunctionEventDTO.ParafaitFunctionEventName.ToString());
                    log.Error(msg);
                    log.LogMethodExit(msg);
                    return;
                    //throw new ValidationException(msg);
                }
                string SMSGateway = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SMS_GATEWAY");
                if (string.IsNullOrWhiteSpace(SMSGateway))
                { 
                    string msg = MessageContainerList.GetMessage(executionContext, "No SMS gateway defined ");
                    log.Error(msg);
                    throw new ValidationException(msg); 
                }
            }
            if (MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.APP_NOTIFICATION)
            {
                if (string.IsNullOrWhiteSpace(toDevice))
                {
                    string msg = MessageContainerList.GetMessage(executionContext, "No valid device details found to send " + parafaitFunctionEventDTO.ParafaitFunctionEventName.ToString());
                    log.Error(msg);
                    log.LogMethodExit(msg);
                    return;
                    //throw new ValidationException(msg);
                } 
            }

            if (string.IsNullOrWhiteSpace(toEmails) == false && MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.EMAIL)
            {
                string[] emailAddressList = toEmails.Split(',');
                foreach (string emailId in emailAddressList)
                {
                    MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, messageReference, messageType, emailId, toMobile, "", null, null, null, null,
                          messageSubject, messageBody, customerId, cardId, attachFile, true, messagingClientFunctionLookUpDTO.CCList, messagingClientFunctionLookUpDTO.BCCList, messagingClientId, false, toDevice, true, countryCode, trx_no, parafaitFunctionEventDTO.ParafaitFunctionEventId, null, trxId);
                    MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                    messagingRequestBL.Save(sqlTrx);
                }
            }
            else
            {

                MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, messageReference, messageType, toEmails, toMobile, "", null, null, null, null,
                      messageSubject, messageBody, customerId, cardId, attachFile, true, messagingClientFunctionLookUpDTO.CCList, messagingClientFunctionLookUpDTO.BCCList, messagingClientId, false, toDevice, true, countryCode, trx_no, parafaitFunctionEventDTO.ParafaitFunctionEventId, null, trxId);
                MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                messagingRequestBL.Save(sqlTrx);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ParafaitFunctionEventDTO ParafaitFunctionEventDTO
        {
            get
            {
                return parafaitFunctionEventDTO;
            }
        }

        /// <summary>
        /// BuildEmailMessage, holds subject followed by message body
        /// </summary>
        /// <param name="sqlTrx"></param>
        /// <returns></returns>
        public List<string> BuildEmailMessage(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            List<string> emailMessage = new List<string>();
            //messagingChanelType == MessagingClientDTO.MessagingChanelType.NONE - means send all. Else send specified type only
            if ( this.parafaitFunctionEventDTO != null && this.parafaitFunctionEventDTO.MessagingClientFunctionLookUpDTOList!= null 
                && this.parafaitFunctionEventDTO.MessagingClientFunctionLookUpDTOList.Exists( mcf => mcf.IsActive && MessagingClientDTO.SourceEnumFromString(mcf.MessageType) == MessagingClientDTO.MessagingChanelType.EMAIL))
            {
                MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO = this.parafaitFunctionEventDTO.MessagingClientFunctionLookUpDTOList.Find(mcf => mcf.IsActive && MessagingClientDTO.SourceEnumFromString(mcf.MessageType) == MessagingClientDTO.MessagingChanelType.EMAIL);
                EmailTemplateDTO messageTemplateDTO = null;
                try
                {
                    EmailTemplate msgTemplate = new EmailTemplate(executionContext, messagingClientFunctionLookUpDTO.MessageTemplateId, sqlTrx);
                    messageTemplateDTO = msgTemplate.EmailTemplateDTO;
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while retrieving the message template", ex);
                    throw;
                }
                if (messageTemplateDTO == null)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2881, messagingClientFunctionLookUpDTO.MessageType, messagingClientFunctionLookUpDTO.MessageTemplateId);
                    //'Unable to fetch &1 template for &2'
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                string toEMail = GetToEmails(messagingClientFunctionLookUpDTO);
                string content = messageTemplateDTO.EmailTemplate; 
                string messageBody = MessageBodyFormatter(content);
                string messageSubject = MessageSubjectFormatter(content);
                string attachment = GetAttachFile(messagingClientFunctionLookUpDTO);
                emailMessage.Add(toEMail);//toEMail 
                emailMessage.Add(messagingClientFunctionLookUpDTO.CCList);//CCList
                emailMessage.Add(messagingClientFunctionLookUpDTO.BCCList);//BCCList
                emailMessage.Add(messageSubject);//subject 
                emailMessage.Add(messageBody);//body 
                emailMessage.Add(attachment);//attachment file path 
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2902, "MessagingClientFunctionLookUp"));
                //
            }
            log.LogMethodExit(emailMessage);
            return emailMessage;
        }
        /// <summary>
        /// GetToEmails
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <returns></returns>
        protected virtual string GetToEmails(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO);
            string toEmails = string.Empty; 
            log.LogMethodExit(toEmails);
            return toEmails;
        }
        /// <summary>
        /// GetToMobileNumber
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <returns></returns>
        protected virtual string GetToMobileNumber(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO);
            string toMobiles = string.Empty;
            log.LogMethodExit(toMobiles);
            return toMobiles;
        }
        /// <summary>
        /// GetCardId
        /// </summary>
        /// <returns></returns>
        protected virtual int? GetCardId()
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
        protected virtual string GetToDevice(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO);
            string toDevices = GetToDeviceName(messagingClientFunctionLookUpDTO, -1);
            log.LogMethodExit(toDevices);
            return toDevices;
        }
        /// <summary>
        /// GetToDeviceName
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        protected string GetToDeviceName(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO, int customerId)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO, customerId);
            string toDeviceName = string.Empty;
            if (customerId > -1
                            && MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == MessagingClientDTO.MessagingChanelType.APP_NOTIFICATION
                            //&& (this.messagingTriggerDTO.MessageType == MESSAGE_TYPE_BOTH || this.messagingTriggerDTO.MessageType == MESSAGE_TYPE_APP_NOTIFICATION)
                            )
            {
                PushNotificationDeviceListBL pushNotificationDeviceListBL = new PushNotificationDeviceListBL(executionContext);
                List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>> pndSearchParameters = new List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>>();
                pndSearchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                pndSearchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.IS_ACTIVE, "1"));
                List<PushNotificationDeviceDTO> pushNotificationDeviceDTOList = pushNotificationDeviceListBL.GetPushNotificationDeviceDTOList(pndSearchParameters);

                if (pushNotificationDeviceDTOList == null || pushNotificationDeviceDTOList.Any() == false)
                {
                    log.Info("Notification token is invalid: " + "; Customer: " + customerId.ToString());
                }
                else
                {
                    //Add entry for 1 device only. The app notification method takes care of sending to all active device
                    toDeviceName = pushNotificationDeviceDTOList[0].PushNotificationToken;
                }
            }
            log.LogMethodExit(toDeviceName);
            return toDeviceName;
        }
        /// <summary>
        /// GetAttachFile
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO"></param>
        /// <returns></returns>
        protected virtual string GetAttachFile(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
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
        protected virtual int GetCustomerId(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO);
            int customerId = -1;
            log.LogMethodExit(customerId);
            return customerId;
        }

    }
    public class ParafaitFunctionEventListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ParafaitFunctionEventDTO> parafaitFunctionEventDTOList = new List<ParafaitFunctionEventDTO>();

        public ParafaitFunctionEventListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="ParafaitFunctionEventDTOList">ParafaitFunctionEventDTOList</param>
        public ParafaitFunctionEventListBL(ExecutionContext executionContext, List<ParafaitFunctionEventDTO> ParafaitFunctionEventDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.parafaitFunctionEventDTOList = ParafaitFunctionEventDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ParafaitFunctionEventDTO list
        /// </summary>
        public List<ParafaitFunctionEventDTO> GetAllParafaitFunctionEventDTOList(List<KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ParafaitFunctionEventDatahandler parafaitFunctionEventDataHandler = new ParafaitFunctionEventDatahandler(sqlTransaction);
            List<ParafaitFunctionEventDTO> ParafaitFunctionEventDTOList = parafaitFunctionEventDataHandler.GetParafaitFunctionEventDTOList(searchParameters);
            log.LogMethodExit(ParafaitFunctionEventDTOList);
            return ParafaitFunctionEventDTOList;
        }

        /// <summary>
        /// GetAllParafaitFunctionEventDTOList
        /// </summary>
        /// <param name="parafaitFunctionsIdList"></param>
        /// <param name="loadActiveChildren"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ParafaitFunctionEventDTO> GetAllParafaitFunctionEventDTOList(List<int> parafaitFunctionsIdList, bool loadActiveChildren, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(parafaitFunctionsIdList, loadActiveChildren, sqlTransaction);
            ParafaitFunctionEventDatahandler parafaitFunctionEventDataHandler = new ParafaitFunctionEventDatahandler(sqlTransaction);
            List<ParafaitFunctionEventDTO> ParafaitFunctionEventDTOList = parafaitFunctionEventDataHandler.GetParafaitFunctionEventDTOList(parafaitFunctionsIdList, loadActiveChildren);
            log.LogMethodExit(ParafaitFunctionEventDTOList);
            return ParafaitFunctionEventDTOList;
        } 
    }
}
