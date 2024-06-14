/********************************************************************************************
 * Project Name - Messaging Client BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80        25-Jun-2020      Jinto Thomas   Created 
 *2.100.0     22-Aug-2020      Vikas Dwivedi  Modified as per 3-Tier Standard CheckList
 *2.130.0     09-Aug-2021      Abhishek     Modified : modified validate
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// Business Logic for MessagingClientBL
    /// </summary>
    public class MessagingClientBL
    {
        private MessagingClientDTO messagingClientDTO;
        private static readonly logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parametrized Constructor of MessagingClientBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public MessagingClientBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the MessagingClientBL id as the parameter
        /// Would fetch the MessagingClient object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="clientId">id of MessagingClient object</param>
        public MessagingClientBL(ExecutionContext executionContext, int clientId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, clientId, sqlTransaction);
            MessagingClientDataHandler messagingClientDataHandler = new MessagingClientDataHandler(sqlTransaction);
            messagingClientDTO = messagingClientDataHandler.GetMessagingClient(clientId);
            if (messagingClientDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "MessagingClientDTO", clientId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates MessagingClientBL object using the MessagingClientDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="messagingClientDTO">messagingClientDTO object</param>
        public MessagingClientBL(ExecutionContext executionContext, MessagingClientDTO messagingClientDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, messagingClientDTO);
            this.messagingClientDTO = messagingClientDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get MessagingClientDTO Object
        /// </summary>
        public MessagingClientDTO GetMessagingClientDTO
        {
            get { return messagingClientDTO; }
        }

        /// <summary>
        /// Saves the MessagingClient
        /// Checks if the MessagingClientId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation failed", validationErrorList);
            }
            MessagingClientDataHandler messagingClientDataHandler = new MessagingClientDataHandler(sqlTransaction);

            if (messagingClientDTO.ClientId < 0)
            {
                messagingClientDTO = messagingClientDataHandler.Insert(messagingClientDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                messagingClientDTO.AcceptChanges();
            }
            else
            {
                if (messagingClientDTO.IsChanged)
                {
                    messagingClientDTO = messagingClientDataHandler.Update(messagingClientDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    messagingClientDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the MessagingClient DTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            List<ValidationError> validationErrorList = new List<ValidationError>();

            ///Validation Logic 
            MessagingClientListBL messagingClientListBL = new MessagingClientListBL(executionContext);
            List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>>();
            if (string.IsNullOrWhiteSpace(messagingClientDTO.ClientName) || string.IsNullOrWhiteSpace(messagingClientDTO.MessagingChannelCode))
            {
                validationErrorList.Add(new ValidationError("MessagingClient", "ClientName/MessagingChanelCode", MessageContainerList.GetMessage(executionContext, 246, MessageContainerList.GetMessage(executionContext, "Messaging Client Name/Messaging Chanel Code"))));
            }

            else
            {
                searchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.CLIENT_NAME, messagingClientDTO.ClientName));
                searchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.MESSAGING_CHANNEL_CODE, messagingClientDTO.MessagingChannelCode));
                searchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.IS_ACTIVE, "1"));

                List<MessagingClientDTO> messagingClientDTOList = messagingClientListBL.GetMessagingClientDTOList(searchParameters, sqlTransaction);
                if (messagingClientDTOList != null && messagingClientDTOList.Any())
                {
                    if(messagingClientDTOList.Exists(x => x.ClientName == messagingClientDTO.ClientName && x.ClientId != messagingClientDTO.ClientId))
                    {
                        log.Debug("Duplicate update entries detail");
                        validationErrorList.Add(new ValidationError("MessagingClient", "ClientName", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, messagingClientDTO.ClientName))));
                    }
                }
            }

            if (MessagingClientDTO.SourceEnumFromString(messagingClientDTO.MessagingChannelCode) == 0) 
            {
                validationErrorList.Add(new ValidationError("MessagingClient", "MessagingChanelCode", "Invalid Messaging Chanel Code"));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        public virtual MessagingRequestDTO Send(MessagingRequestDTO messagingRequestDTO)
        {
            log.LogMethodEntry();
            log.LogMethodExit(messagingRequestDTO);
            messagingRequestDTO.SendAttemptDate = ServerDateTime.Now;
            return messagingRequestDTO;
        }


        protected virtual string UpdateResults(MessagingRequestDTO messagingRequestDTO, string status, string message)
        {
            log.LogMethodEntry();
            if (messagingRequestDTO.Attempts == null)
            {
                messagingRequestDTO.Attempts = 0;
            }
            messagingRequestDTO.Attempts++;
            messagingRequestDTO.Status = status;
            messagingRequestDTO.StatusMessage = message.Substring(0, Math.Min(message.Length, 500)); ;
            log.LogMethodExit();
            return "";
        }

        public virtual bool isValidWhatsAppNumber(string phoneNumber)
        {
            log.LogMethodEntry(messagingClientDTO, phoneNumber);
            log.LogMethodExit(false);
            return false;
        }
    }

    /// <summary>
    /// Manages the list of MessagingClientListBL
    /// </summary>
    public class MessagingClientListBL
    {
        private List<MessagingClientDTO> messagingClientDTOList = new List<MessagingClientDTO>();
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public MessagingClientListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with executionContext and DTO parameter
        /// </summary>
        /// <param name="MessagingClientDTO"></param>
        /// <param name="executionContext"></param>
        public MessagingClientListBL(ExecutionContext executionContext, List<MessagingClientDTO> messagingClientDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(messagingClientDTOList, executionContext);
            this.messagingClientDTOList = messagingClientDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or Update the MessagingClient List
        /// </summary>
        public void SaveMessagingClient(SqlTransaction sqlTransaction = null)
        {
            if (messagingClientDTOList == null ||
               messagingClientDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < messagingClientDTOList.Count; i++)
            {
                var messagingClientDTO = messagingClientDTOList[i];
                if (messagingClientDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    MessagingClientBL messagingClientBL = new MessagingClientBL(executionContext, messagingClientDTO);
                    messagingClientBL.Save(sqlTransaction);
                }
                catch (SqlException ex)
                {
                    log.Error(ex);
                    if (ex.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else if (ex.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving MessagingClientDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("MessagingClientDTO", messagingClientDTO);
                    throw;
                }
            }
        }
        /// <summary>
        /// Returns the MessagingClient List
        /// </summary>
        public List<MessagingClientDTO> GetMessagingClientDTOList(List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MessagingClientDataHandler messagingClientDataHandler = new MessagingClientDataHandler(sqlTransaction);
            messagingClientDTOList = messagingClientDataHandler.GetMessagingClientDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(messagingClientDTOList);
            return messagingClientDTOList;
        }
    }
}

