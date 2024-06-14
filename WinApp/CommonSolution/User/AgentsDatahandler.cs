/********************************************************************************************
 * Project Name - Agents Data Handler
 * Description  - Data handler of the  Agents Data Handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        10-May-2016   Rakshith          Created 
 *2.00        03-May-2019   Divya             SQL Injection
 *2.70.2        15-Jul-2019   Girish Kundar     Modified : Added GetSQLParameter()
 *2.70.2        11-Dec-2019   Jinto Thomas      Removed siteid from update query 
 *2.90        11-Jun-2020   Mushahid Faizan     Modified : 3 Tier changes for Rest API.
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
    //<summary>
    // AgentsDataHandler Class
    //</summary>
    public class AgentsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM Agents AS a ";
        private DataAccessHandler dataAccessHandler;
        private List<SqlParameter> parameters = new List<SqlParameter>(); //added
        private SqlTransaction sqlTransaction = null;


        /// <summary>
        /// Default constructor of  AgentsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AgentsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();

        }

        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<AgentsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AgentsDTO.SearchByParameters, string>
        {
            {AgentsDTO.SearchByParameters.ACTIVE, "a.Active"},
            {AgentsDTO.SearchByParameters.USER_ID, "a.User_Id"} ,
            {AgentsDTO.SearchByParameters.MASTER_ENTITY_ID, "a.MasterEntityId"} ,
            {AgentsDTO.SearchByParameters.PARTNER_ID, "a.PartnerId"} ,
            {AgentsDTO.SearchByParameters.SITE_ID, "a.Site_id"} ,
            {AgentsDTO.SearchByParameters.AGENT_ID, "a.AgentId"}
        };


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Agents Record.
        /// </summary>
        /// <param name="agentsDTO">AgentsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(AgentsDTO agentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(agentsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@agentId", agentsDTO.AgentId);
            ParametersHelper.ParameterHelper(parameters, "@partnerId", agentsDTO.PartnerId);
            ParametersHelper.ParameterHelper(parameters, "@user_Id", agentsDTO.User_Id);
            ParametersHelper.ParameterHelper(parameters, "@commission", agentsDTO.Commission == 0 ? 0 : (decimal)agentsDTO.Commission);
            ParametersHelper.ParameterHelper(parameters, "@mobileNo", string.IsNullOrEmpty(agentsDTO.MobileNo) ? string.Empty : (object)agentsDTO.MobileNo);
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedUser", loginId);
            ParametersHelper.ParameterHelper(parameters, "@active", agentsDTO.Active);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@site_id", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@masterEntityId", agentsDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AgentsDTO record to the database
        /// </summary>
        /// <param name="agentsDTO">AgentsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns AgentsDTO</returns>
        public AgentsDTO InsertAgent(AgentsDTO agentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(agentsDTO, loginId, siteId);
            string insertAgentsQuery = @"insert into Agents 
                                                        (  
                                                            PartnerId,
                                                            User_Id,
                                                            MobileNo,
                                                            Commission,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedUser,
                                                            LastUpdatedDate,
                                                            Active,
                                                            Site_id,
                                                            Guid,
                                                            MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                            @partnerId,
                                                            @user_Id,
                                                            @mobileNo,
                                                            @commission,
                                                            @createdBy,
                                                            GetDate() ,
                                                            @lastUpdatedUser,
                                                            GetDate() ,
                                                            @active,
                                                            @site_id,
                                                            NEWID(),
                                                            @masterEntityId
                                                         )SELECT  * from Agents where AgentId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertAgentsQuery, BuildSQLParameters(agentsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAgentDTO(agentsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting agentsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(agentsDTO);
            return agentsDTO;

        }

        /// <summary>
        /// update the AgentsDTO record to the database
        /// </summary>
        /// <param name="agentsDTO">AgentsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>ReturnsAgentsDTO</returns>
        public AgentsDTO UpdateAgent(AgentsDTO agentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(agentsDTO, loginId, siteId);
            string iupdateAgentsQuery = @"update Agents 
                                                          set 
                                                            PartnerId=@partnerId,
                                                            User_Id=@user_Id,
                                                            MobileNo=@mobileNo,
                                                            Commission=@commission,
                                                            LastUpdatedUser=@lastUpdatedUser,
                                                            LastUpdatedDate=GetDate(),
                                                            Active=@active,
                                                            -- Site_id=@site_id,
                                                            MasterEntityId=@masterEntityId  
                                                            where AgentId = @agentId
                                                   SELECT  * from Agents where AgentId = @agentId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(iupdateAgentsQuery, BuildSQLParameters(agentsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAgentDTO(agentsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating agentsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(agentsDTO);
            return agentsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="agentsDTO">AgentsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAgentDTO(AgentsDTO agentsDTO, DataTable dt)
        {
            log.LogMethodEntry(agentsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                agentsDTO.AgentId = Convert.ToInt32(dt.Rows[0]["AgentId"]);
                agentsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                agentsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                agentsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                agentsDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                agentsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                agentsDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// return the record from the database
        /// Convert the datarow to AgentsDTO object
        /// </summary>
        /// <returns>return the AgentsDTO object</returns>
        private AgentsDTO GetAgentsDTO(DataRow agentsDataRow)
        {
            log.LogMethodEntry(agentsDataRow);
            AgentsDTO agentsDTO = new AgentsDTO(
                                                    agentsDataRow["AgentId"] == DBNull.Value ? -1 : Convert.ToInt32(agentsDataRow["AgentId"]),
                                                    agentsDataRow["PartnerId"] == DBNull.Value ? -1 : Convert.ToInt32(agentsDataRow["PartnerId"]),
                                                    agentsDataRow["User_Id"] == DBNull.Value ? -1 : Convert.ToInt32(agentsDataRow["User_Id"]),
                                                    agentsDataRow["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(agentsDataRow["MobileNo"]),
                                                    agentsDataRow["Commission"] == DBNull.Value ? 0 : Convert.ToDouble(agentsDataRow["Commission"]),
                                                    agentsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(agentsDataRow["CreatedBy"]),
                                                    agentsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(agentsDataRow["CreationDate"]),
                                                    agentsDataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(agentsDataRow["LastUpdatedUser"]),
                                                    agentsDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(agentsDataRow["LastUpdatedDate"]),
                                                    agentsDataRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(agentsDataRow["Active"]),
                                                    agentsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(agentsDataRow["site_id"]),
                                                    agentsDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(agentsDataRow["Guid"]),
                                                    agentsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(agentsDataRow["SynchStatus"]),
                                                    agentsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(agentsDataRow["MasterEntityId"])
                                                 );
            log.LogMethodExit(agentsDTO);
            return agentsDTO;
        }

        /// <summary>
        /// return the record from the database based on  agentId
        /// </summary>
        /// <returns>return the AgentsDTO object</returns>
        /// or empty AgentsDTO
        public AgentsDTO GetAgentsDTO(int agentId)
        {
            log.LogMethodEntry(agentId);
            string agentsDTOQuery = SELECT_QUERY + "   where a.AgentId = @agentId ";
            AgentsDTO agentsDTO = new AgentsDTO();
            SqlParameter[] AgentsDTOparameters = new SqlParameter[1];
            AgentsDTOparameters[0] = new SqlParameter("@agentId", agentId);

            DataTable dtAgentsDTO = dataAccessHandler.executeSelectQuery(agentsDTOQuery, AgentsDTOparameters, sqlTransaction);
            if (dtAgentsDTO.Rows.Count > 0)
            {
                DataRow agentsDTORow = dtAgentsDTO.Rows[0];
                agentsDTO = GetAgentsDTO(agentsDTORow);
            }
            log.LogMethodExit(agentsDTO);
            return agentsDTO;
        }

        /// <summary>
        /// Gets the AgentsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AgentsDTO matching the search criteria</returns>
        public List<AgentsDTO> GetAgentsLists(List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters);
            List<AgentsDTO> agentsDTOList = new List<AgentsDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters);
            if (currentPage > -1 && pageSize > 0)
            {
                selectQuery += " ORDER BY a.AgentId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                agentsDTOList = new List<AgentsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AgentsDTO AgentsDTO = GetAgentsDTO(dataRow);
                    agentsDTOList.Add(AgentsDTO);
                }
            }
            log.LogMethodExit(agentsDTOList);
            return agentsDTOList;
        }

        /// <summary>
        /// Returns the no of Agents matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetAgentsCount(List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchParameters) //added
        {
            log.LogMethodEntry(searchParameters);
            int agentsDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                agentsDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(agentsDTOCount);
            return agentsDTOCount;
        }

        /// <summary>
        /// Gets the AgentsDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic AgentsDTO matching the search criteria</returns>
        public List<AgentsDTO> GetAllAgentsList(List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<AgentsDTO> agentsDTOList = new List<AgentsDTO>();
            parameters.Clear();
            string selectAgentsDTOQuery = SELECT_QUERY;
            selectAgentsDTOQuery = selectAgentsDTOQuery + GetFilterQuery(searchParameters, sqlTransaction);
            DataTable dtAgentsDTO = dataAccessHandler.executeSelectQuery(selectAgentsDTOQuery, parameters.ToArray(), sqlTransaction);
            if (dtAgentsDTO.Rows.Count > 0)
            {
                foreach (DataRow agentsDTORow in dtAgentsDTO.Rows)
                {
                    AgentsDTO agentsDTO = GetAgentsDTO(agentsDTORow);
                    agentsDTOList.Add(agentsDTO);
                }

            }
            log.LogMethodExit(agentsDTOList);
            return agentsDTOList;
        }

        /// <summary>
        /// Gets the AgentsDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic AgentsDTO matching the search criteria</returns>
        public string GetFilterQuery(List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            parameters.Clear();
            StringBuilder query = new StringBuilder(" where ");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                foreach (KeyValuePair<AgentsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key.Equals(AgentsDTO.SearchByParameters.ACTIVE))
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key.Equals(AgentsDTO.SearchByParameters.PARTNER_ID)
                            || searchParameter.Key.Equals(AgentsDTO.SearchByParameters.USER_ID)
                            || searchParameter.Key.Equals(AgentsDTO.SearchByParameters.AGENT_ID)
                            || searchParameter.Key.Equals(AgentsDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AgentsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " = -1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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

            }
            log.LogMethodExit();
            return query.ToString();
        }

        /// <summary>
        /// Delete the record from the database based on  agentId
        /// </summary>
        /// <returns>return the int </returns>
        public int DeleteAgent(int agentId)
        {
            log.LogMethodEntry(agentId);
            string AgentsDTOQuery = @"delete  
                                          from Agents
                                          where AgentId = @agentId";

            SqlParameter[] AgentsDTOParameters = new SqlParameter[1];
            AgentsDTOParameters[0] = new SqlParameter("@agentId", agentId);

            int deleteStatus = dataAccessHandler.executeUpdateQuery(AgentsDTOQuery, AgentsDTOParameters, sqlTransaction);
            log.LogMethodExit(deleteStatus);
            return deleteStatus;

        }


        /// <summary>
        /// UserTransactionExist method
        /// </summary>
        /// <param name="user_id">user_id</param>
        /// <returns>returns int</returns>
        public int UserTransactionExist(int userId)
        {
            log.LogMethodEntry(userId);

            string agentQuery = @" select top 1 * from trx_header where user_id=@user_id ";

            SqlParameter[] agentsparameters = new SqlParameter[1];
            agentsparameters[0] = new SqlParameter("@user_id", userId);
            DataTable dtAgentsDTO = dataAccessHandler.executeSelectQuery(agentQuery, agentsparameters, sqlTransaction);
            int count = dtAgentsDTO.Rows.Count;
            log.LogMethodExit(count);
            return count;
        }
    }
}
