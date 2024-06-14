/********************************************************************************************
 * Project Name - Communications
 * Description  - MessagingTriggerCriteriaBL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.70.2      16-Nov -2019     Girish Kundar       Created.
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Communication
{
    public class MessagingTriggerCriteriaBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected static ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private MessagingTriggerCriteriaDTO messagingTriggerCriteriaDTO;
        private Utilities utilities = new Utilities();

        /// <summary>
        /// Default constructor MessagingTriggerCriteriaBL class
        /// </summary>
        public MessagingTriggerCriteriaBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            messagingTriggerCriteriaDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with one parameter
        /// </summary>
        /// <param name="Id">Id of the MessagingTriggerCriteria</param>
        public MessagingTriggerCriteriaBL(ExecutionContext executionContext, int id)
        {
            log.LogMethodEntry();
            MessagingTriggerCriteriaDataHandler messagingTriggerCriteriaDataHandler = new MessagingTriggerCriteriaDataHandler();
            messagingTriggerCriteriaDTO = messagingTriggerCriteriaDataHandler.GetMessagingTriggerCriteriaDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with messagingTriggerCriteriaDTO parameter
        /// </summary>
        /// <param name="messagingTriggerCriteriaDTO">parameter of type messagingTriggerCriteriaDTO </param>
        public MessagingTriggerCriteriaBL (ExecutionContext executionContext, MessagingTriggerCriteriaDTO messagingTriggerCriteriaDTO)
        {
            log.LogMethodEntry(messagingTriggerCriteriaDTO);
            this.messagingTriggerCriteriaDTO = messagingTriggerCriteriaDTO;
            machineUserContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the  stationMessagingTriggerCriteria details to table
        /// </summary>
        /// <param name="sqlTransaction"></param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (messagingTriggerCriteriaDTO.IsChanged == false
                     && messagingTriggerCriteriaDTO.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            MessagingTriggerCriteriaDataHandler messagingTriggerCriteriaDataHandler = new MessagingTriggerCriteriaDataHandler(sqlTransaction);
            List<ValidationError> validationErrorList = Validate();
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }
            if (messagingTriggerCriteriaDTO.Id < 0)
            {
                messagingTriggerCriteriaDTO = messagingTriggerCriteriaDataHandler.Insert(messagingTriggerCriteriaDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                messagingTriggerCriteriaDTO.AcceptChanges();
            }
            else
            {
                if (messagingTriggerCriteriaDTO.IsChanged)
                {
                    messagingTriggerCriteriaDTO = messagingTriggerCriteriaDataHandler.Update(messagingTriggerCriteriaDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    messagingTriggerCriteriaDTO.AcceptChanges();
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the Messaging TriggerCriteria DTO .
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            return validationErrorList;
        }
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            MessagingTriggerCriteriaDataHandler messagingTriggerCriteriaDataHandler = new MessagingTriggerCriteriaDataHandler(sqlTransaction);
            if (messagingTriggerCriteriaDTO.Id >= 0)
            {
                messagingTriggerCriteriaDataHandler.Delete(messagingTriggerCriteriaDTO);
            }
            messagingTriggerCriteriaDTO.AcceptChanges();
        }

        /// <summary>
        /// Gets the MessagingTriggerCriteriaDTO
        /// </summary>
        public MessagingTriggerCriteriaDTO MessagingTriggerCriteriaDTO
        {
            get { return messagingTriggerCriteriaDTO; }
        }

    }
    /// <summary>
    /// Class for MessagingTriggerCriteria List
    /// </summary>
    public class MessagingTriggerCriteriaListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MessagingTriggerCriteriaDTO> messagingTriggerCriteriaDTOList = new List<MessagingTriggerCriteriaDTO>();
        private ExecutionContext executionContext;
        public MessagingTriggerCriteriaListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with executionContext and messagingTriggerCriteriaDTOList as parameters
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="messagingTriggerCriteriaDTOList">messagingTriggerCriteriaDTOList</param>
        public MessagingTriggerCriteriaListBL(ExecutionContext executionContext, List<MessagingTriggerCriteriaDTO> messagingTriggerCriteriaDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.messagingTriggerCriteriaDTOList = messagingTriggerCriteriaDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns All active the MessagingTriggerCriteria records from the table 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of active MessagingTriggerCriteriaDTO</returns>
        public List<MessagingTriggerCriteriaDTO> GetAllMessagingTriggerCriteria(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            MessagingTriggerCriteriaDataHandler messagingTriggerCriteriaDataHandler = new MessagingTriggerCriteriaDataHandler(sqlTransaction);
            List<KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string>(MessagingTriggerCriteriaDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string>(MessagingTriggerCriteriaDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<MessagingTriggerCriteriaDTO> messagingTriggerCriteriaDTOList = messagingTriggerCriteriaDataHandler.GetMessagingTriggerCriteriaDTOList(searchParameters);
            log.LogMethodExit(messagingTriggerCriteriaDTOList);
            return messagingTriggerCriteriaDTOList;
        }

        /// <summary>
        ///  Returns All the MessagingTriggerCriteria records from the table 
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of MessagingTriggerCriteriaDTO</returns>
        public List<MessagingTriggerCriteriaDTO> GetMessagingTriggerCriteriaDTOList(List<KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MessagingTriggerCriteriaDataHandler messagingTriggerCriteriaDataHandler = new MessagingTriggerCriteriaDataHandler(sqlTransaction);
            List<MessagingTriggerCriteriaDTO> messagingTriggerCriteriaDTOList = messagingTriggerCriteriaDataHandler.GetMessagingTriggerCriteriaDTOList(searchParameters);
            log.LogMethodExit(messagingTriggerCriteriaDTOList);
            return messagingTriggerCriteriaDTOList;
        }

        /// <summary>
        /// Save and Update MessagingTriggerCriteriaDTOList Method
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (messagingTriggerCriteriaDTOList == null ||
                messagingTriggerCriteriaDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < messagingTriggerCriteriaDTOList.Count; i++)
            {
                var messagingTriggerCriteriaDTO = this.messagingTriggerCriteriaDTOList[i];
                if (messagingTriggerCriteriaDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    MessagingTriggerCriteriaBL messagingTriggerCriteriaBL = new MessagingTriggerCriteriaBL(executionContext, messagingTriggerCriteriaDTO);
                    messagingTriggerCriteriaBL.Save(sqlTransaction);


                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw new ForeignKeyException();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving messagingTriggerCriteriaDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("messagingTriggerCriteriaDTO", messagingTriggerCriteriaDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}
