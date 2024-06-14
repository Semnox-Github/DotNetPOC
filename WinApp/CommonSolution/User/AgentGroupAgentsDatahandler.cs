/********************************************************************************************
 * Project Name - AgentGroupAgents Data handler
 * Description  - Data handler of the  AgentGroupAgentsDatahandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        08-June-2016   Rakshith          Created 
 *2.70.2        15-Jul-2019   Girish Kundar     Modified : Added GetSQLParameter() and Fix For SQL Injection Issue. 
 *2.70.2        11-Dec-2019   Jinto Thomas      Removed siteid from update query 
 *2.90        11-Jun-2020   Mushahid Faizan      Modified : 3 Tier Changes For Rest API., Added IsActive Column.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.User
{
    ///<summary>
    /// AgentGroupAgentsDatahandler Class
    ///</summary>
    public class AgentGroupAgentsDatahandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM AgentGroupAgents AS aga ";
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;


        /// <summary>
        /// Default constructor of  AgentGroupAgentsDatahandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AgentGroupAgentsDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();

        }

        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<AgentGroupAgentsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AgentGroupAgentsDTO.SearchByParameters, string>
        {
            {AgentGroupAgentsDTO.SearchByParameters.AGENT_GROUP_ID, "aga.AgentGroupId"},
            {AgentGroupAgentsDTO.SearchByParameters.AGENT_GROUP_ID_LIST, "aga.AgentGroupId"},
            {AgentGroupAgentsDTO.SearchByParameters.AGENT_ID, "aga.AgentId"},
            {AgentGroupAgentsDTO.SearchByParameters.AGENT_ID_LIST, "aga.AgentId"},
            {AgentGroupAgentsDTO.SearchByParameters.MASTER_ENTITY_ID, "aga.MasterEntityId"} ,
            {AgentGroupAgentsDTO.SearchByParameters.SITE_ID, "aga.site_id"},
            {AgentGroupAgentsDTO.SearchByParameters.ID, "aga.Id"},
            {AgentGroupAgentsDTO.SearchByParameters.ISACTIVE, "aga.IsActive"}
        };


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AgentGroupAgents Record.
        /// </summary>
        /// <param name="agentGroupAgentsDTO">AgentGroupAgentsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(AgentGroupAgentsDTO agentGroupAgentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(agentGroupAgentsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@id", agentGroupAgentsDTO.Id, true);
            ParametersHelper.ParameterHelper(parameters, "@agentGroupId", agentGroupAgentsDTO.AgentGroupId, true);
            ParametersHelper.ParameterHelper(parameters, "@agentId", agentGroupAgentsDTO.AgentId, true);
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedUser", loginId);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@site_id", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@masterEntityId", agentGroupAgentsDTO.MasterEntityId, true);
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", agentGroupAgentsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the AgentGroupAgentsDTO record to the database
        /// </summary>
        /// <param name="agentGroupAgentsDTO">agentGroupAgentsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns AgentGroupAgentsDTO</returns>
        public AgentGroupAgentsDTO InsertAgentGroupAgents(AgentGroupAgentsDTO agentGroupAgentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(agentGroupAgentsDTO, loginId, siteId);
            string insertAgentGroupAgentsQuery = @"insert into AgentGroupAgents 
                                                        (  
                                                            AgentGroupId,
                                                            AgentId,
                                                            Guid,
                                                            site_id,
                                                            MasterEntityId,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedUser,
                                                                                    LastUpdatedDate ,
                                                            IsActive
                                                        ) 
                                                values 
                                                        (
                                                            @agentGroupId,
                                                            @agentId,
                                                            NEWID(),
                                                            @site_id,
                                                            @masterEntityId,
                                                            @createdBy,
                                                            GetDate() ,
                                                            @lastUpdatedUser,
                                                            GetDate(),   
                                                            @isActive  
                                                         )SELECT  * from AgentGroupAgents where Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertAgentGroupAgentsQuery, BuildSQLParameters(agentGroupAgentsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAgentGroupAgentsDTO(agentGroupAgentsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting agentGroupAgentsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(agentGroupAgentsDTO);
            return agentGroupAgentsDTO;
        }

        /// <summary>
        /// update the AgentGroupAgentsDTO record to the database
        /// </summary>
        /// <param name="agentGroupAgentsDTO">agentGroupAgentsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns AgentGroupAgentsDTO</returns>
        public AgentGroupAgentsDTO UpdateAgentGroupAgent(AgentGroupAgentsDTO agentGroupAgentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(agentGroupAgentsDTO, loginId, siteId);
            string updateQuery = @"update AgentGroupAgents 
                                                          set 
                                                            AgentGroupId = @agentGroupId,
                                                            AgentId = @agentId,
                                                            --site_id = @site_id,
                                                            LastUpdatedUser = @lastUpdatedUser,
                                                            MasterEntityId = @masterEntityId,
                                                            LastUpdatedDate = GetDate(),
                                                            IsActive = @isActive
                                                       where Id = @id
                                                    SELECT  * from AgentGroupAgents where Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(agentGroupAgentsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAgentGroupAgentsDTO(agentGroupAgentsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating agentGroupAgentsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(agentGroupAgentsDTO);
            return agentGroupAgentsDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="agentGroupAgentsDTO">AgentGroupAgentsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAgentGroupAgentsDTO(AgentGroupAgentsDTO agentGroupAgentsDTO, DataTable dt)
        {
            log.LogMethodEntry(agentGroupAgentsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                agentGroupAgentsDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                agentGroupAgentsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                agentGroupAgentsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                agentGroupAgentsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                agentGroupAgentsDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                agentGroupAgentsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                agentGroupAgentsDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// return the record from the database
        /// Convert the datarow to AgentGroupAgentsDTO object
        /// </summary>
        /// <returns>return the AgentGroupAgentsDTO object</returns>
        private AgentGroupAgentsDTO GetAgentGroupAgentsDTO(DataRow agentGroupAgentDataRow)
        {
            log.LogMethodEntry(agentGroupAgentDataRow);
            AgentGroupAgentsDTO agentGroupAgentsDTO = new AgentGroupAgentsDTO(
                                                    agentGroupAgentDataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(agentGroupAgentDataRow["Id"]),
                                                    agentGroupAgentDataRow["AgentGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(agentGroupAgentDataRow["AgentGroupId"]),
                                                    agentGroupAgentDataRow["AgentId"] == DBNull.Value ? -1 : Convert.ToInt32(agentGroupAgentDataRow["AgentId"]),
                                                    agentGroupAgentDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(agentGroupAgentDataRow["Guid"]),
                                                    agentGroupAgentDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(agentGroupAgentDataRow["site_id"]),
                                                    agentGroupAgentDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(agentGroupAgentDataRow["SynchStatus"]),
                                                    agentGroupAgentDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(agentGroupAgentDataRow["CreatedBy"]),
                                                    agentGroupAgentDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(agentGroupAgentDataRow["CreationDate"]),
                                                    agentGroupAgentDataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(agentGroupAgentDataRow["LastUpdatedUser"]),
                                                    agentGroupAgentDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(agentGroupAgentDataRow["LastUpdatedDate"]),
                                                     agentGroupAgentDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(agentGroupAgentDataRow["MasterEntityId"]),
                                                    agentGroupAgentDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(agentGroupAgentDataRow["IsActive"])
                                                   );
            log.LogMethodExit(agentGroupAgentsDTO);
            return agentGroupAgentsDTO;
        }

        /// <summary>
        /// return the record from the database based on  id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>return the AgentGroupAgentsDTO object</returns>
        /// or empty AgentGroupAgentsDTO
        public AgentGroupAgentsDTO GetAgentGroupAgentsDTO(int id)
        {
            log.LogMethodEntry(id);
            AgentGroupAgentsDTO agentGroupAgentsDTO = new AgentGroupAgentsDTO();
            string agentGroupAgentsDTOQuery = SELECT_QUERY + " where aga.Id= @id";

            SqlParameter[] agentGroupAgentsDTOparameters = new SqlParameter[1];
            agentGroupAgentsDTOparameters[0] = new SqlParameter("@id", id);

            DataTable dtAgentGroupAgentsDTO = dataAccessHandler.executeSelectQuery(agentGroupAgentsDTOQuery, agentGroupAgentsDTOparameters, sqlTransaction);

            if (dtAgentGroupAgentsDTO.Rows.Count > 0)
            {
                DataRow agentGroupAgentsDTORow = dtAgentGroupAgentsDTO.Rows[0];
                agentGroupAgentsDTO = GetAgentGroupAgentsDTO(agentGroupAgentsDTORow);
            }

            log.LogMethodExit(agentGroupAgentsDTO);
            return agentGroupAgentsDTO;

        }

        /// <summary>
        /// Gets the AgentGroupAgentsDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic AgentGroupAgentsDTO matching the search criteria</returns>
        public List<AgentGroupAgentsDTO> GetAllAgentGroupsAgentsList(List<KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectAgentGroupAgentsDTOQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key.Equals(AgentGroupAgentsDTO.SearchByParameters.AGENT_GROUP_ID) ||
                            searchParameter.Key.Equals(AgentGroupAgentsDTO.SearchByParameters.AGENT_ID) ||
                            searchParameter.Key.Equals(AgentGroupAgentsDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                            searchParameter.Key.Equals(AgentGroupAgentsDTO.SearchByParameters.ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AgentGroupAgentsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AgentGroupAgentsDTO.SearchByParameters.AGENT_GROUP_ID_LIST ||
                          searchParameter.Key == AgentGroupAgentsDTO.SearchByParameters.AGENT_ID_LIST)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AgentGroupAgentsDTO.SearchByParameters.ISACTIVE)  // bit
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectAgentGroupAgentsDTOQuery = selectAgentGroupAgentsDTOQuery + query;

            }
            DataTable dtAgentGroupAgentsDTO = dataAccessHandler.executeSelectQuery(selectAgentGroupAgentsDTOQuery, parameters.ToArray(), sqlTransaction);

            List<AgentGroupAgentsDTO> agentGroupAgentsDTOList = new List<AgentGroupAgentsDTO>();
            if (dtAgentGroupAgentsDTO.Rows.Count > 0)
            {
                foreach (DataRow agentGroupAgentsDTORow in dtAgentGroupAgentsDTO.Rows)
                {
                    AgentGroupAgentsDTO agentGroupAgentsDTO = GetAgentGroupAgentsDTO(agentGroupAgentsDTORow);
                    agentGroupAgentsDTOList.Add(agentGroupAgentsDTO);
                }

            }
            log.LogMethodExit(agentGroupAgentsDTOList);
            return agentGroupAgentsDTOList;
        }

        /// <summary>
        /// Delete the record from the database based on  agentGroupAgentId
        /// </summary>
        /// <returns>return the int </returns>
        public int DeleteAgentGroupsAgent(int id)
        {
            log.LogMethodEntry(id);
            string AgentGroupAgentsDTOQuery = @"delete  
                                                 from AgentGroupAgents 
                                                 where Id= @id";
            SqlParameter[] agentGroupAgentsDTOParameters = new SqlParameter[1];
            agentGroupAgentsDTOParameters[0] = new SqlParameter("@id", id);

            int deleteStatus = dataAccessHandler.executeUpdateQuery(AgentGroupAgentsDTOQuery, agentGroupAgentsDTOParameters, sqlTransaction);
            log.LogMethodExit(deleteStatus);
            return deleteStatus;
        }

        /// <summary>
        ///  Delete the record from the database based on  groupId
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <returns>deleteStatus </returns>
        public int DeleteAgentGroupByGroupId(int groupId)
        {
            log.LogMethodEntry(groupId);

            string AgentGroupAgentsDTOQuery = @"delete  
                                                 from AgentGroupAgents 
                                                 where AgentGroupId= @groupId";
            SqlParameter[] agentGroupAgentsDTOParameters = new SqlParameter[1];
            agentGroupAgentsDTOParameters[0] = new SqlParameter("@groupId", groupId);

            int deleteStatus = dataAccessHandler.executeUpdateQuery(AgentGroupAgentsDTOQuery, agentGroupAgentsDTOParameters,sqlTransaction);
            log.LogMethodExit(deleteStatus);
            return deleteStatus;
        }
    }
}

