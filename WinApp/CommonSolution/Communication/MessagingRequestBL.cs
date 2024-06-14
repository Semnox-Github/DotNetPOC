/********************************************************************************************
 * Project Name - Messaging Request BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80        29-Nov-2019      Deeksha        Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Data.SqlClient;

namespace Semnox.Parafait.Communication
{
    public class MessagingRequestBL
    {
        private MessagingRequestDTO messagingRequestDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of MessagingRequestBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private MessagingRequestBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the MessagingRequestBL id as the parameter
        /// Would fetch the MessagingRequest object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public MessagingRequestBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            MessagingRequestDataHandler messagingRequestDataHandler = new MessagingRequestDataHandler(sqlTransaction);
            messagingRequestDTO = messagingRequestDataHandler.GetMessagingRequest(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates MessagingRequestBL object using the MessagingRequestDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="MessagingRequestDTO">messagingRequestDTO object</param>
        public MessagingRequestBL(ExecutionContext executionContext, MessagingRequestDTO messagingRequestDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, messagingRequestDTO);
            this.messagingRequestDTO = messagingRequestDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get MessagingRequestDTO Object
        /// </summary>
        public MessagingRequestDTO GetMessagingRequestDTO
        {
            get { return messagingRequestDTO; }
        }

        public List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (messagingRequestDTO == null)
            {
                //Validation to be implemented.
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the MessagingRequest
        /// Checks if the MessagingRequestId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            MessagingRequestDataHandler messagingRequestDataHandler = new MessagingRequestDataHandler(sqlTransaction);
            Validate();
            if (messagingRequestDTO.Id < 0)
            {
                messagingRequestDTO = messagingRequestDataHandler.Insert(messagingRequestDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                messagingRequestDTO.AcceptChanges();
            }
            else
            {
                if (messagingRequestDTO.IsChanged)
                {
                    messagingRequestDTO = messagingRequestDataHandler.Update(messagingRequestDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    messagingRequestDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of MessagingRequestListBL
    /// </summary>
    public class MessagingRequestListBL
    {
        private List<MessagingRequestDTO> messagingRequestDTOList;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public MessagingRequestListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.messagingRequestDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="messagingRequestDTO"></param>
        /// <param name="executionContext"></param>
        public MessagingRequestListBL(ExecutionContext executionContext, List<MessagingRequestDTO> messagingRequestDTOList)
        {
            log.LogMethodEntry(messagingRequestDTOList, executionContext);
            this.executionContext = executionContext;
            this.messagingRequestDTOList = messagingRequestDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or update records with inner collections
        /// </summary>
        public void SaveMessagingRequest(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry();
                if (messagingRequestDTOList != null)
                {
                    foreach (MessagingRequestDTO messagingRequestDTO in messagingRequestDTOList)
                    {
                        MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                        messagingRequestBL.Save(sqlTransaction);
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns the MessagingRequest  List
        /// </summary>
        public List<MessagingRequestDTO> GetAllMessagingRequestList(List<KeyValuePair<MessagingRequestDTO.SearchByParameters, string>> searchParameters, int pageNumber = 0, int pageSize = 500)
        {
            log.LogMethodEntry(searchParameters);
            MessagingRequestDataHandler messagingRequestDataHandler = new MessagingRequestDataHandler(null);
            List<MessagingRequestDTO> messagingRequestDTOList = messagingRequestDataHandler.GetMessagingRequestDTOList(searchParameters);
            log.LogMethodExit(messagingRequestDTOList);
            return messagingRequestDTOList;
        }
    }
}


