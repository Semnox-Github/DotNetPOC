/********************************************************************************************
 * Project Name - Agents Programs 
 * Description  - Data object of the Agents 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        10-May-2016   Rakshith           Created 
 *2.70.2      15-Jul-2019   Girish Kundar      Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.90        11-Jun-2020   Mushahid Faizan      Modified : 3 Tier Changes For Rest API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the Agents  data object class. This acts as data holder for the Agents  business object
    /// </summary>
    public class AgentGroupAgents
    {
        private AgentGroupAgentsDTO agentGroupAgentsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        private AgentGroupAgents(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AgentGroupAgents object using the AgentGroupAgentsDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="achievementDTO">AgentGroupAgentsDTO object is passed as parameter</param>
        public AgentGroupAgents(ExecutionContext executionContext, AgentGroupAgentsDTO agentGroupAgentsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, agentGroupAgentsDTO);
            this.agentGroupAgentsDTO = agentGroupAgentsDTO;
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        //EX: "'Agents"'  DTO  ====>  ""Agents" DataHandler
        /// <summary>
        /// Constructor with the agentGroupAgent id as the parameter
        /// Would fetch the agentGroupAgent object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="id">id of AgentGroupAgent Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AgentGroupAgents(ExecutionContext executionContext, int agentGroupAgentid, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, agentGroupAgentid, sqlTransaction);
            AgentGroupAgentsDatahandler agentGroupAgentsDatahandler = new AgentGroupAgentsDatahandler(sqlTransaction);
            agentGroupAgentsDTO = agentGroupAgentsDatahandler.GetAgentGroupAgentsDTO(agentGroupAgentid);
            if (agentGroupAgentsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AgentGroupAgentsDTO", agentGroupAgentid);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AgentGroupAgentsDatahandler agentGroupAgentsDatahandler = new AgentGroupAgentsDatahandler(sqlTransaction);
            if (agentGroupAgentsDTO.Id < 0)
            {
                log.LogVariableState("AgentGroupAgentsDTO", agentGroupAgentsDTO);
                agentGroupAgentsDTO = agentGroupAgentsDatahandler.InsertAgentGroupAgents(agentGroupAgentsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                agentGroupAgentsDTO.AcceptChanges();
            }
            else if (agentGroupAgentsDTO.IsChanged)
            {
                log.LogVariableState("AgentGroupAgentsDTO", agentGroupAgentsDTO);
                agentGroupAgentsDTO = agentGroupAgentsDatahandler.UpdateAgentGroupAgent(agentGroupAgentsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                agentGroupAgentsDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the AgentGroupAgentsDTO based on Id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>deleted Id</returns>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                AgentGroupAgentsDatahandler agentGroupAgentsDatahandler = new AgentGroupAgentsDatahandler(sqlTransaction);
                int deletedId = agentGroupAgentsDatahandler.DeleteAgentGroupsAgent(agentGroupAgentsDTO.Id);
                log.LogMethodExit(deletedId);
                return deletedId;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Deleting AgentGroupAgentDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete the AgentGroupAgentsDTO based on groupId
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>deleted Id</returns>
        public int DeleteByGroupId(int groupId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(groupId, sqlTransaction);
            try
            {
                AgentGroupAgentsDatahandler agentGroupAgentsDatahandler = new AgentGroupAgentsDatahandler(sqlTransaction);
                int deletedId = agentGroupAgentsDatahandler.DeleteAgentGroupByGroupId(groupId);
                log.LogMethodExit(deletedId);
                return deletedId;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred At DeleteByGroupId(groupId) ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the AgentGroupAgentsDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            return validationErrorList;
            // Validation Logic here 
        }

        /// <summary>
        /// gets the GetAgentGroupAgentsDTO
        /// </summary>
        public AgentGroupAgentsDTO GetAgentGroupAgentsDTO
        {
            get { return agentGroupAgentsDTO; }
        }
    }

    /// <summary>
    /// Manages the list of AgentGroupAgents
    /// </summary>
    public class AgentGroupAgentsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AgentGroupAgentsDTO> agentGroupAgentsDTOList = new List<AgentGroupAgentsDTO>();

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public AgentGroupAgentsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO Parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="agentGroupAgentsDTOList">agentGroupAgentsDTOList</param>
        public AgentGroupAgentsList(ExecutionContext executionContext, List<AgentGroupAgentsDTO> agentGroupAgentsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, agentGroupAgentsDTOList);
            this.agentGroupAgentsDTOList = agentGroupAgentsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns Search Request And returns List Of AgentGroupAgentsDTO Class  
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of AgentGroupAgentsDTO </returns>
        public List<AgentGroupAgentsDTO> GetAllAgentGroupsAgentsList(List<KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            try
            {
                AgentGroupAgentsDatahandler agentGroupAgentsDatahandler = new AgentGroupAgentsDatahandler(sqlTransaction);
                List<AgentGroupAgentsDTO> agentGroupAgentsDTOList = agentGroupAgentsDatahandler.GetAllAgentGroupsAgentsList(searchParameters);
                log.LogMethodExit(agentGroupAgentsDTOList);
                return agentGroupAgentsDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred At GetAllAgentGroupsAgentsList(searchParameters) ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Saves the AgentGroupAgents List
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (agentGroupAgentsDTOList == null ||
               agentGroupAgentsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < agentGroupAgentsDTOList.Count; i++)
            {
                var agentGroupAgentsDTO = agentGroupAgentsDTOList[i];
                if (agentGroupAgentsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AgentGroupAgents agentGroupAgents = new AgentGroupAgents(executionContext, agentGroupAgentsDTO);
                    agentGroupAgents.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AgentGroupAgentsDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AgentGroupAgentsDTOList", agentGroupAgentsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}