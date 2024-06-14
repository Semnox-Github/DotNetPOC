/********************************************************************************************
 * Project Name - Messaging Client Function LookUp BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80        25-Jun-2020      Jinto Thomas   Created
 *2.100.0     22-Aug-2020      Vikas Dwivedi  Modified as per 3-Tier Standard Checklist 
 *2.110.0     22-Dec-2020     Guru S A        Subscription changes
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// Business Logic for MessagingClientFunctionLookUpBL
    /// </summary>
    public class MessagingClientFunctionLookUpBL
    {
        private MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor of MessagingClientFunctionLookUpBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private MessagingClientFunctionLookUpBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the MessagingClientFunctionLookUpBL id as the parameter
        /// Would fetch the MessagingClientFunctionLookUp object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="clientId">id of MessagingClientFunctionLookup object</param>
        public MessagingClientFunctionLookUpBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            MessagingClientFunctionLookUpDatahandler messagingClientFunctionLookUpDatahandler = new MessagingClientFunctionLookUpDatahandler(sqlTransaction);
            messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpDatahandler.GetMessagingClientFunctionLookUp(id);
            if (messagingClientFunctionLookUpDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "MessagingClientFunctionLookUpDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates MessagingClientBL object using the MessagingClientDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="messagingClientFunctionLookUpDTO">messagingClientDTO object</param>
        public MessagingClientFunctionLookUpBL(ExecutionContext executionContext, MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, messagingClientFunctionLookUpDTO);
            this.messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get MessagingClientDTO Object
        /// </summary>
        public MessagingClientFunctionLookUpDTO GetMessagingClientFunctionLookUpDTO
        {
            get { return messagingClientFunctionLookUpDTO; }
        }

        /// <summary>
        /// Saves the MessagingClientFunctionLookUp
        /// Checks if the MessagingClientFunctionLookUpId is not less than or equal to 0
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

            MessagingClientFunctionLookUpDatahandler messagingClientFunctionLookUpDatahandler = new MessagingClientFunctionLookUpDatahandler(sqlTransaction);

            if (messagingClientFunctionLookUpDTO.Id < 0)
            {
                messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpDatahandler.Insert(messagingClientFunctionLookUpDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                messagingClientFunctionLookUpDTO.AcceptChanges();
            }
            else
            {
                if (messagingClientFunctionLookUpDTO.IsChanged)
                {
                    messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpDatahandler.Update(messagingClientFunctionLookUpDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    messagingClientFunctionLookUpDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the child records for MessagingClientFuuntionLookupBL object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            MessagingClientListBL messagingClientListBL = new MessagingClientListBL(executionContext);
            List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.CLIENT_ID, messagingClientFunctionLookUpDTO.MessageClientId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            List<MessagingClientDTO> messagingClientDTOList = messagingClientListBL.GetMessagingClientDTOList(searchParameters, sqlTransaction);
            if (messagingClientDTOList != null && messagingClientDTOList.Any())
                messagingClientFunctionLookUpDTO.MessagingClientDTO = messagingClientDTOList[0];
            else
                messagingClientFunctionLookUpDTO.MessagingClientDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the MessagingClientFunctionLookup DTO
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            List<ValidationError> validationErrorList = new List<ValidationError>();

            ///Validation Logic
            if (string.IsNullOrWhiteSpace(messagingClientFunctionLookUpDTO.MessageClientId.ToString()))
            {
                validationErrorList.Add(new ValidationError("MessagingClientFunctionLookUp", "MessageClientId", MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Messaging Client Id"))));
            }

            if (messagingClientFunctionLookUpDTO.ParafaitFunctionEventId < 0)
            {
                validationErrorList.Add(new ValidationError("MessagingClientFunctionLookUp", "ParafaitFunctionEventId", MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Parafait Function Event Id"))));
            }

            if (MessagingClientDTO.SourceEnumFromString(messagingClientFunctionLookUpDTO.MessageType) == 0)
            {
                validationErrorList.Add(new ValidationError("MessagingClientFunctionLookUp", "MessageType", MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Message Type"))));
            }

            List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTOList;
            MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpList = new MessagingClientFunctionLookUpListBL(executionContext);

            List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.MESSAGE_TYPE, messagingClientFunctionLookUpDTO.MessageType));
            searchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID, messagingClientFunctionLookUpDTO.ParafaitFunctionEventId.ToString()));
            searchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.IS_ACTIVE, "1"));

            messagingClientFunctionLookUpDTOList = messagingClientFunctionLookUpList.GetAllMessagingClientFunctionLookUpList(searchParameters, true, true);

            if (messagingClientFunctionLookUpDTOList != null && messagingClientFunctionLookUpDTOList.Any())
            {
                if (messagingClientFunctionLookUpDTO.Id == -1)
                {
                    log.Debug("Duplicate entries detail");
                    validationErrorList.Add(new ValidationError("MessagingClientFunctionLookUp", "LookUpId", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "parafait function mapping to messaging client"))));
                }
                if (messagingClientFunctionLookUpDTOList.Exists(x => x.ParafaitFunctionEventId == messagingClientFunctionLookUpDTO.ParafaitFunctionEventId && x.Id != messagingClientFunctionLookUpDTO.Id))
                {
                    log.Debug("Duplicate update entries detail");
                    validationErrorList.Add(new ValidationError("MessagingClientFunctionLookUp", "ParafaitFunctionEventId", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "parafait function mapping to messaging client"))));
                }
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }

    /// <summary>
    /// Manages the list of MessagingClientFunctionLookUpListBL
    /// </summary>
    public class MessagingClientFunctionLookUpListBL
    {
        private List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTOList = new List<MessagingClientFunctionLookUpDTO>();
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public MessagingClientFunctionLookUpListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.messagingClientFunctionLookUpDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Paramaetrized Constructor with executionContext and DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="messagingClientFunctionLookUpDTOList">messagingClientFunctionLookUpDTOList</param>
        public MessagingClientFunctionLookUpListBL(ExecutionContext executionContext, List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, messagingClientFunctionLookUpDTOList);
            this.messagingClientFunctionLookUpDTOList = messagingClientFunctionLookUpDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or Update the MessagingClientFunctionLookUp
        /// </summary>
        public void SaveMessagingClientFunctionLookUp(SqlTransaction sqlTransaction = null)
        {
            if (messagingClientFunctionLookUpDTOList == null ||
               messagingClientFunctionLookUpDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < messagingClientFunctionLookUpDTOList.Count; i++)
            {
                var messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpDTOList[i];
                if (messagingClientFunctionLookUpDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    MessagingClientFunctionLookUpBL messagingClientFunctionLookUpBL = new MessagingClientFunctionLookUpBL(executionContext, messagingClientFunctionLookUpDTO);
                    messagingClientFunctionLookUpBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving MessagingClientFunctionLookUpDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("MessagingClientFunctionLookUpDTO", messagingClientFunctionLookUpDTO);
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the MessagingClientFunctionLookUp List
        /// </summary>
        public List<MessagingClientFunctionLookUpDTO> GetAllMessagingClientFunctionLookUpList(List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>> searchParameters, bool buildChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MessagingClientFunctionLookUpDatahandler messagingClientFunctionLookUpDatahandler = new MessagingClientFunctionLookUpDatahandler(sqlTransaction);
            messagingClientFunctionLookUpDTOList = messagingClientFunctionLookUpDatahandler.GetMessagingClientFunctionLookUpDTOList(searchParameters, sqlTransaction);

            if (messagingClientFunctionLookUpDTOList != null && messagingClientFunctionLookUpDTOList.Any() && buildChildRecords)
            {
                Build(messagingClientFunctionLookUpDTOList, activeChildRecords, sqlTransaction);
            }

            log.LogMethodExit(messagingClientFunctionLookUpDTOList);
            return messagingClientFunctionLookUpDTOList;
        }

        /// <summary>
        /// Builds the List of MessagingClientFunctionLookupBL object based on the list of Client id.
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTOList, activeChildRecords, sqlTransaction);

            Dictionary<int, MessagingClientFunctionLookUpDTO> idDictionary = new Dictionary<int, MessagingClientFunctionLookUpDTO>();
            StringBuilder sb = new StringBuilder(string.Empty);
            string messagingClientIdSet;
            for (int i = 0; i < messagingClientFunctionLookUpDTOList.Count; i++)
            {
                if (messagingClientFunctionLookUpDTOList[i].MessageClientId == -1 ||
                    idDictionary.ContainsKey(messagingClientFunctionLookUpDTOList[i].MessageClientId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(messagingClientFunctionLookUpDTOList[i].MessageClientId);
                idDictionary.Add(messagingClientFunctionLookUpDTOList[i].MessageClientId, messagingClientFunctionLookUpDTOList[i]);
            }

            messagingClientIdSet = sb.ToString();

            // Build child Records - MessagingClient
            if (string.IsNullOrWhiteSpace(messagingClientIdSet) == false)
            {
                MessagingClientListBL messagingClientListBL = new MessagingClientListBL(executionContext);
                List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.CLIENT_ID_LIST, messagingClientIdSet.ToString()));
                if (activeChildRecords)
                {
                    searchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.IS_ACTIVE, "1"));
                }

                List<MessagingClientDTO> messagingClientDTOList = messagingClientListBL.GetMessagingClientDTOList(searchParameters, sqlTransaction);
                if (messagingClientDTOList != null && messagingClientDTOList.Any())
                {
                    log.LogVariableState("messagingClientDTOList", messagingClientDTOList);
                    foreach (var messagingClientFunctionLookupDTO in messagingClientFunctionLookUpDTOList)
                    {
                        List<MessagingClientDTO> tempMessagingClientDTOList = messagingClientDTOList.Where(x => x.ClientId == messagingClientFunctionLookupDTO.MessageClientId).ToList();
                        if (tempMessagingClientDTOList != null && tempMessagingClientDTOList.Any())
                        {
                            messagingClientFunctionLookupDTO.MessagingClientDTO = tempMessagingClientDTOList[0];
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the MessagingClientFunctionLookUp List
        /// </summary>
        /// <param name="functionArea"></param>
        /// <param name="functionName"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public List<MessagingClientFunctionLookUpDTO> GetMessagingClientDTOListByFunctionName(String functionArea, String functionName, String channel)
        {
            log.LogMethodEntry(functionArea, functionName, channel);
            //revist the logic here
            //int lookupId = -1;
            //int lookUpValueId = -1;

            //List<LookupValuesDTO> lookupValuesDTOList;
            //LookupValuesList lookupValuesList = new LookupValuesList(executionContext);

            //List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValueSearchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            //lookUpValueSearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, functionArea));
            //lookUpValueSearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            //lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookUpValueSearchParameters);
            //log.LogVariableState("lookupValuesDTOList :", lookupValuesDTOList);

            //if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
            //{
            //    var lookupValuesDTO = lookupValuesDTOList.Where(x => x.LookupValue == functionName).FirstOrDefault();
            //    if (lookupValuesDTO != null)
            //        lookUpValueId = lookupValuesDTO.LookupValueId;
            //    else
            //        lookupId = lookupValuesDTOList[0].LookupId;
            //}
            //else
            //{
            //    return null;
            //}

            List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTOList;
            MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpList = new MessagingClientFunctionLookUpListBL(executionContext);
            List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.PARAFAIT_FUNCTION_NAME, functionArea));
            searchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_NAME, functionName.ToString()));
            searchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.MESSAGE_TYPE, channel));
            searchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            messagingClientFunctionLookUpDTOList = messagingClientFunctionLookUpList.GetAllMessagingClientFunctionLookUpList(searchParameters, true, true);

            //if (messagingClientFunctionLookUpDTOList != null && messagingClientFunctionLookUpDTOList.Any())
            //{
            //    if (!String.IsNullOrEmpty(channel))
            //    {
            //        messagingClientFunctionLookUpDTOList = messagingClientFunctionLookUpDTOList.Where(x => x.MessagingClientDTO.MessagingChannelCode.Equals(channel)).ToList();
            //    }
            //}

            return messagingClientFunctionLookUpDTOList;
        }
    }
}
