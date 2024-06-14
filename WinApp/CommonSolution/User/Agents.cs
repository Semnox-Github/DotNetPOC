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
 *2.70        15-May-2019   Jagan Mohana Rao   Created Agents() with DTO and ExecutionContext
 *2.70.2        15-Jul-2019   Girish Kundar      Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.90        11-Jun-2020   Mushahid Faizan       Modified : 3 Tier Changes for Rest API.
 *2.90        18-Aug-2020   Mushahid Faizan       Modified : WMS issue fixes, added Validation in Validate()
 ********************************************************************************************/
//using Semnox.Parafait.Agents;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the Agents  data object class. This acts as data holder for the Agents  business object
    /// </summary>
    public class Agents
    {
        private AgentsDTO agentsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        private Agents(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="agentsDTO"></param>
        public Agents(ExecutionContext executionContext, AgentsDTO agentsDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, agentsDTO);
            this.agentsDTO = agentsDTO;
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander besed id
        //And return Correspond Object
        //EX: "'Agents"'  DTO  ====>  ""Agents" DataHandler
        public Agents(ExecutionContext executionContext, int agentId, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, agentsDTO, sqlTransaction);
            AgentsDataHandler agentsDataHandler = new AgentsDataHandler(sqlTransaction);
            agentsDTO = agentsDataHandler.GetAgentsDTO(agentId);
            if (agentsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Agents", agentId);
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
            try
            {
                AgentsDataHandler agentsDataHandler = new AgentsDataHandler(sqlTransaction);
                if (agentsDTO.IsChanged == false && agentsDTO.AgentId > -1)
                {
                    log.LogMethodExit(null, "No Changes to save");
                    return;
                }
                Validate(sqlTransaction);
                if (agentsDTO.AgentId < 0)
                {
                    log.LogVariableState("AgentsDTO", agentsDTO);
                    agentsDTO = agentsDataHandler.InsertAgent(agentsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    agentsDTO.AcceptChanges();
                }
                else if (agentsDTO.IsChanged)
                {
                    log.LogVariableState("AgentsDTO", agentsDTO);
                    agentsDTO = agentsDataHandler.UpdateAgent(agentsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    agentsDTO.AcceptChanges();
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Saving agentsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Validate the AgentsDTO
        /// </summary>
        /// <returns></returns>
        private void Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            AgentsDataHandler agentsDataHandler = new AgentsDataHandler(sqlTransaction);

            List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<AgentsDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<AgentsDTO.SearchByParameters, string>(AgentsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<AgentsDTO> agentsDTOList = agentsDataHandler.GetAllAgentsList(searchParams);

            if (agentsDTOList != null && agentsDTOList.Any())
            {
                if (agentsDTOList.Exists(x => x.User_Id == agentsDTO.User_Id) && agentsDTO.AgentId < 0)
                {
                    log.Debug("Duplicate entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "User already assigned.", MessageContainerList.GetMessage(executionContext, "Agents"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
                if (agentsDTOList.Exists(x => x.User_Id == agentsDTO.User_Id && x.AgentId != agentsDTO.AgentId))
                {
                    log.Debug("Duplicate update entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "User already assigned.", MessageContainerList.GetMessage(executionContext, "Agents"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
            }
             if (UserTransactionExist(agentsDTO.User_Id) > 0)
            {
                log.Debug("User has active Transcations! Cannot Assign user.");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "User has active Transactions! Cannot Assign user.", MessageContainerList.GetMessage(executionContext, "Agents"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the AgentsDTO based on Id
        /// </summary>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                AgentsDataHandler agentsDataHandler = new AgentsDataHandler(sqlTransaction);
                int id = agentsDataHandler.DeleteAgent(agentsDTO.AgentId);
                log.LogMethodExit(id);
                return id;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Deleting agentsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// UserTransactionExist method
        /// </summary>
        /// <param name="user_id">user_id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns int</returns>
        public int UserTransactionExist(int user_id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(user_id, sqlTransaction);
            AgentsDataHandler agentsDataHandler = new AgentsDataHandler(sqlTransaction);
            int id = agentsDataHandler.UserTransactionExist(user_id);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// gets the GetAgentsDTO
        /// </summary>
        public AgentsDTO GetAgentsDTO
        {
            get { return agentsDTO; }
        }

    }


    /// <summary>
    /// Manages the List of Agents
    /// </summary>
    public class AgentsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AgentsDTO> agentsDTOList = new List<AgentsDTO>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AgentsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public AgentsList(ExecutionContext executionContext, List<AgentsDTO> agentsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, agentsDTOList);
            this.agentsDTOList = agentsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns Search Request And returns List Of AgentsDTO Class  
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of AgentsDTO</returns>
        public List<AgentsDTO> GetAllAgentsList(List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {

            log.LogMethodEntry(searchParameters, sqlTransaction);
            try
            {
                AgentsDataHandler agentsDataHandler = new AgentsDataHandler(sqlTransaction);
                List<AgentsDTO> agentsDTOList = agentsDataHandler.GetAllAgentsList(searchParameters);
                log.LogMethodExit(agentsDTOList);
                return agentsDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred at GetAllAgentsList(searchParameters)", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Returns Search Request And returns List Of AgentsDTO Class  
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of AgentsDTO</returns>
        public List<AgentsDTO> GetAllAgentsLists(List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null) // added
        {

            log.LogMethodEntry(searchParameters, sqlTransaction);
            try
            {
                AgentsDataHandler agentsDataHandler = new AgentsDataHandler(sqlTransaction);
                List<AgentsDTO> agentsDTOList = agentsDataHandler.GetAgentsLists(searchParameters, currentPage, pageSize, sqlTransaction);
                log.LogMethodExit(agentsDTOList);
                return agentsDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred at GetAllAgentsList(searchParameters)", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns Search Request And returns List Of AgentUserDTO Class  
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of AgentUserDTO</returns>
        public List<AgentUserDTO> GetAllAgentUserList(List<KeyValuePair<AgentUserDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            try
            {
                AgentUserDatahandler agentUserDatahandler = new AgentUserDatahandler(sqlTransaction);
                List<AgentUserDTO> agentUserDTOList = agentUserDatahandler.GetAllAgentUserList(searchParameters);
                log.LogMethodExit(agentUserDTOList);
                return agentUserDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred at GetAllAgentUserList(searchParameters)", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Saves the agentsDTOList 
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (agentsDTOList == null ||
                agentsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < agentsDTOList.Count; i++)
            {
                var agentsDTO = agentsDTOList[i];
                if (agentsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    Agents agents = new Agents(executionContext, agentsDTO);
                    agents.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AgentsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AgentsDTO", agentsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Delete the Agents
        /// </summary>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                if (agentsDTOList != null)
                {
                    foreach (AgentsDTO agentsDTO in agentsDTOList)
                    {
                        Agents agents = new Agents(executionContext, agentsDTO);
                        agents.Delete(sqlTransaction);
                    }
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
                    throw;
                }
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit();
        }

        public int GetAgentsCount(List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AgentsDataHandler agentsDataHandler = new AgentsDataHandler(sqlTransaction);
            int agentsCount = agentsDataHandler.GetAgentsCount(searchParameters);
            log.LogMethodExit(agentsCount);
            return agentsCount;
        }

    }
}
