/********************************************************************************************
 * Project Name - Messaging Trigger BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 *********************************************************************************************
 *2.70.2      29-Nov-2019      Girish Kundar        Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Communication
{
    public class MessagingTriggerBL
    {
        private MessagingTriggerDTO messagingTriggerDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of MessagingTriggerBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public MessagingTriggerBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the MessagingTriggerBL id as the parameter
        /// Would fetch the MessagingTrigger object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public MessagingTriggerBL(ExecutionContext executionContext, int id, bool loadChildRecords=false,
                                  bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            MessagingTriggerDataHandler messagingTriggerDataHandler = new MessagingTriggerDataHandler(sqlTransaction);
            messagingTriggerDTO = messagingTriggerDataHandler.GetMessagingTriggerDTO(id);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Generate MessagingTriggerCriteria list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            MessagingTriggerCriteriaListBL messagingTriggerCriteriaListBL = new MessagingTriggerCriteriaListBL(executionContext);
            List<KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string>(MessagingTriggerCriteriaDTO.SearchByParameters.TRIGGER_ID, messagingTriggerDTO.TriggerId.ToString()));
            messagingTriggerDTO.MessagingTriggerCriteriaDTOList = messagingTriggerCriteriaListBL.GetMessagingTriggerCriteriaDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates MessagingTriggerBL object using the MessagingTriggerDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="MessagingTriggerDTO">messagingTriggerDTO object</param>
        public MessagingTriggerBL(ExecutionContext executionContext, MessagingTriggerDTO messagingTriggerDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, messagingTriggerDTO);
            this.messagingTriggerDTO = messagingTriggerDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get MessagingTriggerDTO Object
        /// </summary>
        public MessagingTriggerDTO GetMessagingTriggerDTO
        {
            get { return messagingTriggerDTO; }
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (messagingTriggerDTO == null)
            {
                //Validation to be implemented.
            }

            if (messagingTriggerDTO.MessagingTriggerCriteriaDTOList != null)
            {
                foreach (var messagingTriggerCriteriaDTO in messagingTriggerDTO.MessagingTriggerCriteriaDTOList)
                {
                    if (messagingTriggerCriteriaDTO.IsChanged)
                    {
                        MessagingTriggerCriteriaBL messagingTriggerCriteriaBL = new MessagingTriggerCriteriaBL(executionContext, messagingTriggerCriteriaDTO);
                        validationErrorList.AddRange(messagingTriggerCriteriaBL.Validate(sqlTransaction)); //calls child validation method.
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the MessagingTrigger
        /// Checks if the MessagingTriggerId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (messagingTriggerDTO.IsChangedRecursive == false
                && messagingTriggerDTO.TriggerId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            MessagingTriggerDataHandler messagingTriggerDataHandler = new MessagingTriggerDataHandler(sqlTransaction);
            if (messagingTriggerDTO.TriggerId < 0)
            {
                messagingTriggerDTO = messagingTriggerDataHandler.Insert(messagingTriggerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                messagingTriggerDTO.AcceptChanges();
            }
            else
            {
                if (messagingTriggerDTO.IsChanged)
                {
                    messagingTriggerDTO = messagingTriggerDataHandler.Update(messagingTriggerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    messagingTriggerDTO.AcceptChanges();
                }
            }
            if (messagingTriggerDTO.MessagingTriggerCriteriaDTOList != null && messagingTriggerDTO.MessagingTriggerCriteriaDTOList.Count != 0)
            {
                foreach (MessagingTriggerCriteriaDTO messagingTriggerCriteriaDTO in messagingTriggerDTO.MessagingTriggerCriteriaDTOList)
                {
                    if (messagingTriggerCriteriaDTO.IsChanged)
                    {
                        messagingTriggerCriteriaDTO.TriggerId = messagingTriggerDTO.TriggerId;
                        MessagingTriggerCriteriaBL messagingTriggerCriteriaBL = new MessagingTriggerCriteriaBL(executionContext, messagingTriggerCriteriaDTO);
                        messagingTriggerCriteriaBL.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            MessagingTriggerDataHandler messagingTriggerDataHandler = new MessagingTriggerDataHandler(sqlTransaction);
            if (messagingTriggerDTO.MessagingTriggerCriteriaDTOList != null && (messagingTriggerDTO.MessagingTriggerCriteriaDTOList.Any(x => x.IsActive == true)))
            {
                string message = MessageContainerList.GetMessage(executionContext, 1143);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ForeignKeyException(message);
            }
            log.LogVariableState("messagingTriggerDTO", messagingTriggerDTO);
            if (messagingTriggerDTO.MessagingTriggerCriteriaDTOList != null)
            {
                for (int i = 0; i < messagingTriggerDTO.MessagingTriggerCriteriaDTOList.Count; i++)
                {
                    var messagingTriggerCriteriaDTO = messagingTriggerDTO.MessagingTriggerCriteriaDTOList[i];
                    MessagingTriggerCriteriaBL messagingTriggerCriteriaBL = new MessagingTriggerCriteriaBL(executionContext, messagingTriggerCriteriaDTO);
                    messagingTriggerCriteriaBL.Delete(sqlTransaction);
                }
            }
            if (messagingTriggerDTO.TriggerId >= 0)
            {
                messagingTriggerDataHandler.Delete(messagingTriggerDTO);
            }
            messagingTriggerDTO.AcceptChanges();
        }
    }

    /// <summary>
    /// Manages the list of MessagingTriggerListBL
    /// </summary>
    public class MessagingTriggerListBL
    {
        private List<MessagingTriggerDTO> messagingTriggerDTOList;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public MessagingTriggerListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.messagingTriggerDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="messagingTriggerDTOList"></param>
        /// <param name="executionContext"></param>
        public MessagingTriggerListBL(ExecutionContext executionContext, List<MessagingTriggerDTO> messagingTriggerDTOList)
        {
            log.LogMethodEntry(messagingTriggerDTOList, executionContext);
            this.executionContext = executionContext;
            this.messagingTriggerDTOList = messagingTriggerDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or update records with inner collections
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            if (messagingTriggerDTOList == null ||
                messagingTriggerDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < messagingTriggerDTOList.Count; i++)
            {
                var messagingTriggerDTO = this.messagingTriggerDTOList[i];
                if (messagingTriggerDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        MessagingTriggerBL messagingTriggerBL = new MessagingTriggerBL(executionContext, messagingTriggerDTO);
                        messagingTriggerBL.Save(parafaitDBTrx.SQLTrx);
                    }

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
                    log.Error("Error occurred while saving messagingTriggerDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("messagingTriggerDTO", messagingTriggerDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the MessagingTrigger  List
        /// </summary>
        public List<MessagingTriggerDTO> GetAllMessagingTriggerList(List<KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>> searchParameters,
                                          bool loadChildRecords = false, bool activeChildRecords = true,
                                          SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            MessagingTriggerDataHandler messagingTriggerDataHandler = new MessagingTriggerDataHandler(sqlTransaction);
            List<MessagingTriggerDTO> messagingTriggerDTOList = messagingTriggerDataHandler.GetMessagingTriggerDTOList(searchParameters);
            if (messagingTriggerDTOList != null && messagingTriggerDTOList.Any() && loadChildRecords)
            {
                Build(messagingTriggerDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(messagingTriggerDTOList);
            return messagingTriggerDTOList;
        }

        private void Build(List<MessagingTriggerDTO> messagingTriggersDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(messagingTriggersDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, MessagingTriggerDTO> messagingTriggerIdDictionary = new Dictionary<int, MessagingTriggerDTO>();
            StringBuilder sb = new StringBuilder("");
            string messagingTriggerIdList = string.Empty;
            for (int i = 0; i < messagingTriggersDTOList.Count; i++)
            {
                if (messagingTriggersDTOList[i].TriggerId == -1 ||
                    messagingTriggerIdDictionary.ContainsKey(messagingTriggersDTOList[i].TriggerId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(messagingTriggersDTOList[i].TriggerId.ToString());
                messagingTriggerIdDictionary.Add(messagingTriggersDTOList[i].TriggerId, messagingTriggersDTOList[i]);
            }
            messagingTriggerIdList = sb.ToString();
            MessagingTriggerCriteriaListBL messagingTriggerCriteriaList = new MessagingTriggerCriteriaListBL(executionContext);
            List<KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string>(MessagingTriggerCriteriaDTO.SearchByParameters.TRIGGER_ID_LIST, messagingTriggerIdList.ToString()));

            if (activeChildRecords)
            {
                searchParams.Add(new KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string>(MessagingTriggerCriteriaDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            List<MessagingTriggerCriteriaDTO> messagingTriggerCriteriaDTOList = messagingTriggerCriteriaList.GetMessagingTriggerCriteriaDTOList(searchParams);
            if (messagingTriggerCriteriaDTOList != null && messagingTriggerCriteriaDTOList.Any())
            {
                log.LogVariableState("messagingTriggerCriteriaDTOList", messagingTriggerCriteriaDTOList);
                foreach (var messagingTriggerCriteriaDTO in messagingTriggerCriteriaDTOList)
                {
                    if (messagingTriggerIdDictionary.ContainsKey(messagingTriggerCriteriaDTO.TriggerId))
                    {
                        if (messagingTriggerIdDictionary[messagingTriggerCriteriaDTO.TriggerId].MessagingTriggerCriteriaDTOList == null)
                        {
                            messagingTriggerIdDictionary[messagingTriggerCriteriaDTO.TriggerId].MessagingTriggerCriteriaDTOList = new List<MessagingTriggerCriteriaDTO>();
                        }
                        messagingTriggerIdDictionary[messagingTriggerCriteriaDTO.TriggerId].MessagingTriggerCriteriaDTOList.Add(messagingTriggerCriteriaDTO);
                    }
                }
            }
            log.LogMethodExit();
        }
        public void Delete()
        {
            log.LogMethodEntry();
            if (messagingTriggerDTOList == null ||
                messagingTriggerDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < messagingTriggerDTOList.Count; i++)
            {
                var messagingTriggerDTO = this.messagingTriggerDTOList[i];
                if (messagingTriggerDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        MessagingTriggerBL messagingTriggerBL = new MessagingTriggerBL(executionContext, messagingTriggerDTO);
                        messagingTriggerBL.Delete(parafaitDBTrx.SQLTrx);
                    }

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
                    log.Error("Error occurred while saving messagingTriggerDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("messagingTriggerDTO", messagingTriggerDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}

