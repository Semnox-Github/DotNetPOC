/********************************************************************************************
 * Project Name - AgentGroups Programs 
 * Description  - Data object of the AgentGroups 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        08-June-2016   Rakshith           Created 
 *2.60        16-May-2019    Jagan Mohana Rao   Created Validate() in the save method, GetAllAgentGroupsDTOList() 
 *                                              SaveUpdateAgentAgroups() and DeleteAgentAgroups() methods in the List class.
 *2.70.2        15-Jul-2019   Girish Kundar      Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.90        11-Jun-2020   Mushahid Faizan       Modified : 3 Tier Changes for Rest API.
*2.90        18-Aug-2020   Mushahid Faizan       Modified : WMS issue fixes, added Validation in Validate()
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    // <summary>
    /// This is the AgentGroups  data object class. This acts as data holder for the AgentGroups  business object
    /// </summary>
    public class AgentGroups
    {
        private AgentGroupsDTO agentGroupsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor of AgentGroups
        /// </summary>
        private AgentGroups(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Creates AgentGroups object using the agentGroupsDTO
        /// </summary>
        /// <param name="agentGroupsDTO">agentGroupsDTO</param>
        /// <param name="executionContext">executionContext</param>
        public AgentGroups(ExecutionContext executionContext, AgentGroupsDTO agentGroupsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, agentGroupsDTO);
            this.agentGroupsDTO = agentGroupsDTO;
            log.LogMethodExit();
        }
        //Constructor Call Corresponding Data Hander besed id
        //And return Correspond Object
        //EX: "'Agents"'  DTO  ====>  ""Agents" DataHandler
        /// <summary>
        ///  Parametrized constructor gets DTO by passing Id
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="agentGroupId"></param>
        /// <param name="sqlTransaction"></param>
        public AgentGroups(ExecutionContext executionContext, int agentGroupId, bool loadChildRecords = true,
           bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, agentGroupId, sqlTransaction);
            AgentGroupsDatahandler agentGroupsDatahandler = new AgentGroupsDatahandler(sqlTransaction);
            agentGroupsDTO = agentGroupsDatahandler.GetAgentGroupsDTO(agentGroupId);
            if (agentGroupsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AgentGroups", agentGroupId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the child records for AgentGroups object.
        /// </summary>
        private void Build(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            AgentGroupAgentsList agentGroupAgentsList = new AgentGroupAgentsList(executionContext);
            List<KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>(AgentGroupAgentsDTO.SearchByParameters.AGENT_GROUP_ID, agentGroupsDTO.AgentGroupId.ToString()));
            agentGroupsDTO.AgentGroupAgentsDTOList = agentGroupAgentsList.GetAllAgentGroupsAgentsList(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AgentGroupsDatahandler agentGroupsDatahandler = new AgentGroupsDatahandler(sqlTransaction);
            try
            {
                List<ValidationError> validationErrorList = Validate();
                if (validationErrorList.Count > 0)
                {
                    throw new ValidationException("Validation Failed", validationErrorList);
                }
                if (agentGroupsDTO.AgentGroupId <= 0)
                {
                    agentGroupsDTO = agentGroupsDatahandler.InsertAgentGroup(agentGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    agentGroupsDTO.AcceptChanges();
                    SaveAgentGroupAgents(sqlTransaction);
                }
                else
                {
                    if (agentGroupsDTO.IsChanged)
                    {
                        agentGroupsDTO = agentGroupsDatahandler.UpdateAgentGroup(agentGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        agentGroupsDTO.AcceptChanges();
                    }

                    SaveAgentGroupAgents(sqlTransaction);
                    agentGroupsDTO.AcceptChanges();
                    log.LogMethodExit();
                }

            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Saving AgentGroupsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Saves the child records : AgentGroupAgentsDTOList
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        private void SaveAgentGroupAgents(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (agentGroupsDTO.AgentGroupAgentsDTOList != null &&
                agentGroupsDTO.AgentGroupAgentsDTOList.Any())
            {
                List<AgentGroupAgentsDTO> updatedAgentGroupAgentsDTOList = new List<AgentGroupAgentsDTO>();
                foreach (var agentGroupAgentsDTO in agentGroupsDTO.AgentGroupAgentsDTOList)
                {
                    if (agentGroupAgentsDTO.AgentGroupId != agentGroupsDTO.AgentGroupId)
                    {
                        agentGroupAgentsDTO.AgentGroupId = agentGroupsDTO.AgentGroupId;
                    }
                    if (agentGroupAgentsDTO.IsChanged)
                    {
                        updatedAgentGroupAgentsDTOList.Add(agentGroupAgentsDTO);
                    }
                }
                if (updatedAgentGroupAgentsDTOList.Any())
                {
                    log.LogVariableState("UpdatedAgentGroupAgentsDTOList", updatedAgentGroupAgentsDTOList);
                    AgentGroupAgentsList agentGroupAgentsList = new AgentGroupAgentsList(executionContext, updatedAgentGroupAgentsDTOList);
                    agentGroupAgentsList.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the AgentGroupsDTO based on Id
        /// </summary>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                AgentGroupsDatahandler agentGroupsDatahandler = new AgentGroupsDatahandler(sqlTransaction);
                int deletedId =  agentGroupsDatahandler.DeleteAgentGroup(agentGroupsDTO.AgentGroupId);
                log.LogMethodExit(deletedId);
                return deletedId;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Deleting AgentGroupsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Validates the AgentGroupsDTO values
        /// </summary>
        /// <returns></returns>
        private List<ValidationError> Validate()
        {
            List<ValidationError> validationErrorList = new List<ValidationError>();

            // Validation Logic here
            if (string.IsNullOrEmpty(agentGroupsDTO.GroupName))
            {
                ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Agent Group"), MessageContainerList.GetMessage(executionContext, "Group Name"), MessageContainerList.GetMessage(executionContext, 1858)); /// Please Enter the GroupName
                validationErrorList.Add(validationError);
            }

            //if (agentGroupsDTO.PartnerId < 0)
            //{
            //    ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Agent Group"), MessageContainerList.GetMessage(executionContext, "Partner Name"), MessageContainerList.GetMessage(executionContext, "Please Select Partner Name."));
            //    validationErrorList.Add(validationError);
            //}

            AgentGroupsDatahandler agentGroupsDatahandler = new AgentGroupsDatahandler();
            List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> agentGrpNameSearchParam = new List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>>();
            agentGrpNameSearchParam.Add(new KeyValuePair<AgentGroupsDTO.SearchByParameters, string>(AgentGroupsDTO.SearchByParameters.GROUP_NAME, agentGroupsDTO.GroupName));
            List<AgentGroupsDTO> agentGroupList = agentGroupsDatahandler.GetAllAgentGroupsList(agentGrpNameSearchParam);
            if (agentGroupList != null && agentGroupList.Any())
            {
                if (agentGroupList.Exists(x => x.GroupName == agentGroupsDTO.GroupName) && agentGroupsDTO.AgentGroupId < 0)
                {
                    log.Debug("Duplicate entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, " agent group"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
                if (agentGroupList.Exists(x => x.GroupName == agentGroupsDTO.GroupName && x.AgentGroupId != agentGroupsDTO.AgentGroupId))
                {
                    log.Debug("Duplicate update entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, " agent group"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
            }
            if (agentGroupsDTO.AgentGroupAgentsDTOList != null &&
                agentGroupsDTO.AgentGroupAgentsDTOList.Count > 0)
            {
                foreach (AgentGroupAgentsDTO agentGroupAgentsDTO in agentGroupsDTO.AgentGroupAgentsDTOList)
                {
                    AgentGroupAgents agentGroupAgents = new AgentGroupAgents(executionContext, agentGroupAgentsDTO);
                    validationErrorList.AddRange(agentGroupAgents.Validate());
                }
            }
            return validationErrorList;
        }

        /// <summary>
        /// gets the GetAgentGroupsDTO
        /// </summary>
        public AgentGroupsDTO GetAgentGroupsDTO
        {
            get { return agentGroupsDTO; }
        }

    }

    /// <summary>
    ///  Manages the List of AgentGroups
    /// </summary>
    public class AgentGroupsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<AgentGroupsDTO> agentGroupsDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public AgentGroupsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructors for AgentGroupsList
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="agentGroupsDTOList">agentGroupsDTOList</param>
        public AgentGroupsList(ExecutionContext executionContext, List<AgentGroupsDTO> agentGroupsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, agentGroupsDTOList);
            this.agentGroupsDTOList = agentGroupsDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns Search Request And returns List Of AgentGroupsDTO Class  
        /// </summary>
        public List<AgentGroupsDTO> GetAllAgentGroupsList(List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> searchParameters,
             bool loadChildRecords = false, bool activeChildRecords = true, System.Data.SqlClient.SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            // child records needs to build
            AgentGroupsDatahandler agentGroupsDatahandler = new AgentGroupsDatahandler(sqlTransaction);
            List<AgentGroupsDTO> agentGroupsDTOList = agentGroupsDatahandler.GetAllAgentGroupsList(searchParameters, sqlTransaction);
            if (agentGroupsDTOList != null && agentGroupsDTOList.Any() && loadChildRecords)
            {
                Build(agentGroupsDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(agentGroupsDTOList);
            return agentGroupsDTOList;
        }

        /// <summary>
        /// Returns Search Request And returns List Of AgentGroupsDTO Class  
        /// </summary>
        public List<AgentGroupsDTO> GetAllAgentGroupsLists(List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> searchParameters,
             bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            // child records needs to build
            AgentGroupsDatahandler agentGroupsDatahandler = new AgentGroupsDatahandler(sqlTransaction);
            List<AgentGroupsDTO> agentGroupsDTOList = agentGroupsDatahandler.GetAllAgentGroupsLists(searchParameters, currentPage, pageSize, sqlTransaction);
            if (agentGroupsDTOList != null && agentGroupsDTOList.Any() && loadChildRecords)
            {
                Build(agentGroupsDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(agentGroupsDTOList);
            return agentGroupsDTOList;
        }

        /// <summary>
        /// Builds the List of agentGroupsDTOList object based on the list of agentGroups id.
        /// </summary>
        /// <param name="agentGroupsDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<AgentGroupsDTO> agentGroupsDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(agentGroupsDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, AgentGroupsDTO> agentGroupIdAgentGroupDictionary = new Dictionary<int, AgentGroupsDTO>();
            StringBuilder sb = new StringBuilder(string.Empty);
            string agentGroupIdSet;
            for (int i = 0; i < agentGroupsDTOList.Count; i++)
            {
                if (agentGroupsDTOList[i].AgentGroupId == -1 ||
                    agentGroupIdAgentGroupDictionary.ContainsKey(agentGroupsDTOList[i].AgentGroupId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(agentGroupsDTOList[i].AgentGroupId);
                agentGroupIdAgentGroupDictionary.Add(agentGroupsDTOList[i].AgentGroupId, agentGroupsDTOList[i]);
            }

            agentGroupIdSet = sb.ToString();

            // Build child Records - AgentGroupAgents
            AgentGroupAgentsList agentGroupAgentsList = new AgentGroupAgentsList(executionContext);
            List<KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>> searchAgentGroupAgentParams = new List<KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>>();
            searchAgentGroupAgentParams.Add(new KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>(AgentGroupAgentsDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            if (activeChildRecords)
            {
                searchAgentGroupAgentParams.Add(new KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>(AgentGroupAgentsDTO.SearchByParameters.ISACTIVE, "1"));
            }
            searchAgentGroupAgentParams.Add(new KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>(AgentGroupAgentsDTO.SearchByParameters.AGENT_GROUP_ID_LIST, agentGroupIdSet.ToString()));
            List<AgentGroupAgentsDTO> agentGroupAgentsDTOList = agentGroupAgentsList.GetAllAgentGroupsAgentsList(searchAgentGroupAgentParams, sqlTransaction);

            if (agentGroupAgentsDTOList != null && agentGroupAgentsDTOList.Any())
            {
                log.LogVariableState("AgentGroupAgentsDTOList", agentGroupAgentsDTOList);
                foreach (var agentGroupAgentsDTO in agentGroupAgentsDTOList)
                {
                    if (agentGroupIdAgentGroupDictionary.ContainsKey(agentGroupAgentsDTO.AgentGroupId))
                    {
                        if (agentGroupIdAgentGroupDictionary[agentGroupAgentsDTO.AgentGroupId].AgentGroupAgentsDTOList == null)
                        {
                            agentGroupIdAgentGroupDictionary[agentGroupAgentsDTO.AgentGroupId].AgentGroupAgentsDTOList = new List<AgentGroupAgentsDTO>();
                        }
                        agentGroupIdAgentGroupDictionary[agentGroupAgentsDTO.AgentGroupId].AgentGroupAgentsDTOList.Add(agentGroupAgentsDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the agentGroupsDTOList List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (agentGroupsDTOList == null ||
                agentGroupsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < agentGroupsDTOList.Count; i++)
            {
                var agentGroupsDTO = agentGroupsDTOList[i];
                if (agentGroupsDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    AgentGroups agentGroups = new AgentGroups(executionContext, agentGroupsDTO);
                    agentGroups.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AgentGroupsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AgentGroupsDTO", agentGroupsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        public int GetAgentGroupsCount(List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AgentGroupsDatahandler agentGroupsDataHandler = new AgentGroupsDatahandler(sqlTransaction);
            int agentGroupsCount = agentGroupsDataHandler.GetAgentGroupsCount(searchParameters);
            log.LogMethodExit(agentGroupsCount);
            return agentGroupsCount;
        }
        /// <summary>
        /// Delete the agentGroupsDTOList  
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (agentGroupsDTOList != null && agentGroupsDTOList.Any())
            {
                foreach (AgentGroupsDTO agentGroupsDTO in agentGroupsDTOList)
                {
                    if (agentGroupsDTO.IsChangedRecursive)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                AgentGroups agentGroups = new AgentGroups(executionContext, agentGroupsDTO);
                                agentGroups.Delete(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
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
                                    throw;
                                }
                            }
                            catch (ValidationException valEx)
                            {
                                log.Error(valEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
