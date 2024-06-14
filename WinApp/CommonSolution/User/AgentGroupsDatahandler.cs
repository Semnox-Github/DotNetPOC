/********************************************************************************************
 * Project Name - AgentGroups Datahandler
 * Description  - Data handler of the  AgentGroupsDatahandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        08-June-2016   Rakshith          Created 
 *2.70        13-June-2019   Jagan Mohana      Added siteId search parameter
 *2.70.2        15-Jul-2019     Girish Kundar     Modified : Added GetSQLParameter() and Fix For SQL Injection Issue.
 *2.70.2        11-Dec-2019   Jinto Thomas       Removed siteid from update query 
 *2.80        11-Jun-2020   Mushahid Faizan       Modified : 3 Tier Changes for Rest API., Added IsActive Column
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
    // AgentGroupsDatahandler Class
    //</summary>
    public class AgentGroupsDatahandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM AgentGroups AS ag ";
        private DataAccessHandler dataAccessHandler;
        private List<SqlParameter> parameters = new List<SqlParameter>(); //added
        private SqlTransaction sqlTransaction = null;


        /// <summary>
        /// Default constructor of  AgentGroupsDatahandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AgentGroupsDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();

        }

        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<AgentGroupsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AgentGroupsDTO.SearchByParameters, string>
        {
            {AgentGroupsDTO.SearchByParameters.AGENT_GROUP_ID, "ag.AgentGroupId"},
            {AgentGroupsDTO.SearchByParameters.PARTNER_ID, "ag.PartnerId"} ,
            {AgentGroupsDTO.SearchByParameters.MASTER_ENTITY_ID, "ag.MasterEntityId"} ,
            {AgentGroupsDTO.SearchByParameters.SITE_ID, "ag.site_id"} ,
               {AgentGroupsDTO.SearchByParameters.GROUP_NAME, "ag.GroupName"},
            {AgentGroupsDTO.SearchByParameters.ISACTIVE, "ag.IsActive"}
        };


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AgentGroups Record.
        /// </summary>
        /// <param name="AgentGroupsDTO">AgentGroupsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(AgentGroupsDTO agentGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(agentGroupsDTO, loginId, siteId);

            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@agentGroupId", agentGroupsDTO.AgentGroupId, true);
            ParametersHelper.ParameterHelper(parameters, "@partnerId", agentGroupsDTO.PartnerId, true);
            ParametersHelper.ParameterHelper(parameters, "@groupName", string.IsNullOrEmpty(agentGroupsDTO.GroupName) ? DBNull.Value : (object)agentGroupsDTO.GroupName);
            ParametersHelper.ParameterHelper(parameters, "@remarks", string.IsNullOrEmpty(agentGroupsDTO.Remarks) ? DBNull.Value : (object)agentGroupsDTO.Remarks);
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedUser", loginId);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@site_id", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@masterEntityId", agentGroupsDTO.MasterEntityId, true);
            ParametersHelper.ParameterHelper(parameters, "@isActive", agentGroupsDTO.IsActive);
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AgentGroupsDTO record to the database
        /// </summary>
        /// <param name="agentGroupsDTO">agentGroupsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns AgentGroupsDTO</returns>
        public AgentGroupsDTO InsertAgentGroup(AgentGroupsDTO agentGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(agentGroupsDTO, loginId, siteId);
            string insertAgentsQuery = @"insert into AgentGroups 
                                                        (  
                                                            GroupName,
                                                            Remarks,
                                                            site_id,
                                                            Guid,
                                                            PartnerId,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedUser,
                                                            LastUpdatedDate ,
                                                            MasterEntityId, IsActive
                                                        ) 
                                                values 
                                                        (
                                                            @groupName,
                                                            @remarks,
                                                            @site_id,
                                                            NEWID(),
                                                            @partnerId,
                                                            @createdBy,
                                                            GetDate() ,
                                                            @lastUpdatedUser,
                                                            GetDate(),
                                                           @masterEntityId, @isActive
                                                         )SELECT  * from AgentGroups where AgentGroupId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertAgentsQuery, BuildSQLParameters(agentGroupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAgentGroupDTO(agentGroupsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting agentGroupsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(agentGroupsDTO);
            return agentGroupsDTO;

        }

        /// <summary>
        /// update the AgentGroupsDTO record to the database
        /// </summary>
        /// <param name="agentGroupsDTO">agentGroupsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns AgentGroupsDTO</returns>
        public AgentGroupsDTO UpdateAgentGroup(AgentGroupsDTO agentGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(agentGroupsDTO, loginId, siteId);
            string updateAgentQuery = @"update AgentGroups 
                                                          set 
                                                            GroupName=@groupName,
                                                            Remarks=@remarks,
                                                            -- site_id=@site_id,
                                                            PartnerId=@partnerId,
                                                            LastUpdatedUser=@lastUpdatedUser,
                                                            LastUpdatedDate=GetDate(),
                                                            MasterEntityId = @masterEntityId,  IsActive = @isActive
                                                            where AgentGroupId = @agentGroupId
                                               SELECT  * from AgentGroups where AgentGroupId =  @agentGroupId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateAgentQuery, BuildSQLParameters(agentGroupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAgentGroupDTO(agentGroupsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating agentGroupsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(agentGroupsDTO);
            return agentGroupsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="agentGroupsDTO">AgentGroupsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAgentGroupDTO(AgentGroupsDTO agentGroupsDTO, DataTable dt)
        {
            log.LogMethodEntry(agentGroupsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                agentGroupsDTO.AgentGroupId = Convert.ToInt32(dt.Rows[0]["AgentGroupId"]);
                agentGroupsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                agentGroupsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                agentGroupsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                agentGroupsDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                agentGroupsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                agentGroupsDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// return the record from the database
        /// Convert the datarow to AgentGroupsDTO object
        /// </summary>
        /// <returns>return the AgentGroupsDTO object</returns>
        private AgentGroupsDTO GetAgentGroupsDTO(DataRow agentGroupDataRow)
        {
            log.LogMethodEntry(agentGroupDataRow);
            AgentGroupsDTO agentGroupsDTO = new AgentGroupsDTO(
                                                        agentGroupDataRow["AgentGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(agentGroupDataRow["AgentGroupId"]),
                                                        agentGroupDataRow["GroupName"] == DBNull.Value ? string.Empty : Convert.ToString(agentGroupDataRow["GroupName"]),
                                                        agentGroupDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(agentGroupDataRow["Remarks"]),
                                                        agentGroupDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(agentGroupDataRow["site_id"]),
                                                        agentGroupDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(agentGroupDataRow["Guid"]),
                                                        agentGroupDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(agentGroupDataRow["SynchStatus"]),
                                                        agentGroupDataRow["PartnerId"] == DBNull.Value ? -1 : Convert.ToInt32(agentGroupDataRow["PartnerId"]),
                                                        agentGroupDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(agentGroupDataRow["CreatedBy"]),
                                                        agentGroupDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(agentGroupDataRow["CreationDate"]),
                                                        agentGroupDataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(agentGroupDataRow["LastUpdatedUser"]),
                                                        agentGroupDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(agentGroupDataRow["LastUpdatedDate"]),
                                                              agentGroupDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(agentGroupDataRow["MasterEntityId"]),
                                                        agentGroupDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(agentGroupDataRow["IsActive"])
                                                   );
            log.LogMethodExit(agentGroupsDTO);
            return agentGroupsDTO;
        }

        /// <summary>
        /// return the record from the database based on  agentGroupId
        /// </summary>
        /// <returns>return the AgentGroupsDTO object</returns>
        /// or empty AgentGroupsDTO
        public AgentGroupsDTO GetAgentGroupsDTO(int agentGroupId)
        {
            log.LogMethodEntry(agentGroupId);
            string agentGroupsDTOQuery = SELECT_QUERY + " where ag.AgentGroupId = @agentGroupId ";
            AgentGroupsDTO agentGroupsDTO = new AgentGroupsDTO();
            SqlParameter[] agentGroupsDTOparameters = new SqlParameter[1];
            agentGroupsDTOparameters[0] = new SqlParameter("@agentGroupId", agentGroupId);

            DataTable dtAgentGroupsDTO = dataAccessHandler.executeSelectQuery(agentGroupsDTOQuery, agentGroupsDTOparameters, sqlTransaction);
            if (dtAgentGroupsDTO.Rows.Count > 0)
            {
                DataRow agentGroupsDTORow = dtAgentGroupsDTO.Rows[0];
                agentGroupsDTO = GetAgentGroupsDTO(agentGroupsDTORow);
            }

            log.LogMethodExit(agentGroupsDTO);
            return agentGroupsDTO;

        }

        /// <summary>
        /// Gets the AgentGroupsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AgentGroupsDTO matching the search criteria</returns>
        public List<AgentGroupsDTO> GetAllAgentGroupsLists(List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters);
            List<AgentGroupsDTO> agentGroupsDTOList = new List<AgentGroupsDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters, sqlTransaction);
            if (currentPage > -1 && pageSize > 0)
            {
                selectQuery += " ORDER BY ag.AgentGroupId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                agentGroupsDTOList = new List<AgentGroupsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AgentGroupsDTO agentGroupsDTO = GetAgentGroupsDTO(dataRow);
                    agentGroupsDTOList.Add(agentGroupsDTO);
                }
            }
            log.LogMethodExit(agentGroupsDTOList);
            return agentGroupsDTOList;
        }

        /// <summary>
        /// Returns the no of AgentGroups matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetAgentGroupsCount(List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> searchParameters) //added
        {
            log.LogMethodEntry(searchParameters);
            int agentGroupsDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                agentGroupsDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(agentGroupsDTOCount);
            return agentGroupsDTOCount;
        }

        /// <summary>
        /// Gets the AgentGroupsDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic AgentGroupsDTO matching the search criteria</returns>
        public List<AgentGroupsDTO> GetAllAgentGroupsList(List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> searchParameters,  SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<AgentGroupsDTO> agentGroupsDTOList = new List<AgentGroupsDTO>();
            parameters.Clear();
            string selectAgentGroupsDTOQuery = SELECT_QUERY;
            selectAgentGroupsDTOQuery = selectAgentGroupsDTOQuery + GetFilterQuery(searchParameters, sqlTransaction);
            DataTable dtAgentGroupsDTO = dataAccessHandler.executeSelectQuery(selectAgentGroupsDTOQuery, parameters.ToArray(), sqlTransaction);
            if (dtAgentGroupsDTO.Rows.Count > 0)
            {
                foreach (DataRow agentGroupsDTORow in dtAgentGroupsDTO.Rows)
                {
                    AgentGroupsDTO agentGroupsDTO = GetAgentGroupsDTO(agentGroupsDTORow);
                    agentGroupsDTOList.Add(agentGroupsDTO);
                }

            }
            log.LogMethodExit(agentGroupsDTOList);
            return agentGroupsDTOList;
        }

        /// <summary>
        /// Gets the AgentGroupsDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic AgentGroupsDTO matching the search criteria</returns>
        public string GetFilterQuery(List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            parameters.Clear();
            StringBuilder query = new StringBuilder("");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                query = new StringBuilder(" where ");
                foreach (KeyValuePair<AgentGroupsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key.Equals(AgentGroupsDTO.SearchByParameters.AGENT_GROUP_ID)
                        || searchParameter.Key.Equals(AgentGroupsDTO.SearchByParameters.PARTNER_ID)
                        || searchParameter.Key.Equals(AgentGroupsDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key.Equals(AgentGroupsDTO.SearchByParameters.GROUP_NAME))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AgentGroupsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " = -1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AgentGroupsDTO.SearchByParameters.ISACTIVE)  // bit
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
            }
            log.LogMethodExit();
            return query.ToString();
        }

        /// <summary>
        /// Delete the record from the database based on  agentGroupId
        /// </summary>
        /// <returns>return the int </returns>
        public int DeleteAgentGroup(int agentGroupId)
        {
            log.LogMethodEntry(agentGroupId);
            string AgentGroupsDTOQuery = @"delete  
                                                from AgentGroups 
                                                where AgentGroupId= @agentGroupId ";

            SqlParameter[] agentGroupsDTOParameters = new SqlParameter[1];
            agentGroupsDTOParameters[0] = new SqlParameter("@agentGroupId", agentGroupId);
            int deleteStatus = dataAccessHandler.executeUpdateQuery(AgentGroupsDTOQuery, agentGroupsDTOParameters, sqlTransaction);
            log.LogMethodExit(deleteStatus);
            return deleteStatus;

        }
    }
}

