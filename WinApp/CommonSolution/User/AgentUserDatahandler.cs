/********************************************************************************************
 * Project Name - AgentUser Datahandler
 * Description  - Data handler of the AgentUser Datahandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        10-May-2016   Rakshith          Created 
 *2.70.2        16-Jul-2019   Girish Kundar     Modified :  Added LogMethodEntry() and LogMethodExit()
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public class AgentUserDatahandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM Agents AS a ";
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;


        /// <summary>
        /// Default constructor of  AgentsDataHandler class
        /// </summary>
        public AgentUserDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();

        }


        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<AgentUserDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AgentUserDTO.SearchByParameters, string>
        {
            {AgentUserDTO.SearchByParameters.PARTNER_ID, "PartnerId"},
            {AgentUserDTO.SearchByParameters.USER_ID, "Agents.User_Id"} ,
            {AgentUserDTO.SearchByParameters.AGENT_ID, "AgentId"} ,
            {AgentUserDTO.SearchByParameters.ACTIVE, "Active"},
            {AgentUserDTO.SearchByParameters.LOGINID, "loginid"}
        };


        /// <summary>
        /// return the record from the database
        /// Convert the datarow to AgentsUserDTO object
        /// </summary>
        /// <returns>return the AgentUserDTO object</returns>
        private AgentUserDTO GetAgentsUserDTO(DataRow AgentUserDataRow)
        {
            log.LogMethodEntry(AgentUserDataRow);
            AgentUserDTO agentUserDTO = new AgentUserDTO(
                                                    AgentUserDataRow["AgentId"] == DBNull.Value ? -1 : Convert.ToInt32(AgentUserDataRow["AgentId"]),
                                                    AgentUserDataRow["PartnerId"] == DBNull.Value ? -1 : Convert.ToInt32(AgentUserDataRow["PartnerId"]),
                                                    AgentUserDataRow["User_Id"] == DBNull.Value ? -1 : Convert.ToInt32(AgentUserDataRow["User_Id"]),
                                                    AgentUserDataRow["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(AgentUserDataRow["MobileNo"]),
                                                    AgentUserDataRow["Commission"] == DBNull.Value ? -1 : Convert.ToDouble(AgentUserDataRow["Commission"]),
                                                    AgentUserDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(AgentUserDataRow["CreatedBy"]),
                                                    AgentUserDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(AgentUserDataRow["CreationDate"]),
                                                    AgentUserDataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(AgentUserDataRow["LastUpdatedUser"]),
                                                    AgentUserDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(AgentUserDataRow["LastUpdatedDate"]),
                                                    AgentUserDataRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(AgentUserDataRow["Active"]),
                                                    AgentUserDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(AgentUserDataRow["Site_id"]),
                                                    AgentUserDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(AgentUserDataRow["Guid"]),
                                                    AgentUserDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(AgentUserDataRow["SynchStatus"]),
                                                    AgentUserDataRow["LoginId"] == DBNull.Value ? string.Empty : Convert.ToString(AgentUserDataRow["LoginId"]),
                                                    AgentUserDataRow["Email"] == DBNull.Value ? string.Empty : Convert.ToString(AgentUserDataRow["Email"]),
                                                    AgentUserDataRow["UserName"] == DBNull.Value ? string.Empty : Convert.ToString(AgentUserDataRow["UserName"]),
                                                    AgentUserDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(AgentUserDataRow["MasterEntityId"])
                                                 );
            log.LogMethodExit(agentUserDTO);
            return agentUserDTO;
        }

        /// <summary>
        /// Gets the List<AgentUserDTO> matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic AgentUserDTO matching the search criteria</returns>
        public List<AgentUserDTO> GetAllAgentUserList(List<KeyValuePair<AgentUserDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
                string agentsUsersQuery = @"select Agents.AgentId, Agents.PartnerId, Agents.User_Id ,Agents.MobileNo ,
                                          Agents.Commission,Agents.CreatedBy,Agents.CreationDate,Agents.LastUpdatedUser,
                                          Agents.LastUpdatedDate,Agents.Active ,Agents.site_id,Agents.Guid,Agents.SynchStatus,
                                          users.loginid,users.email, users.username  ,users.MasterEntityId     
                                          from Agents 
                                          inner join users 
                                          on Agents.User_Id=users.user_id  ";


                if ((searchParameters != null) && (searchParameters.Count > 0))
                {
                    StringBuilder query = new StringBuilder(" where ");
                    foreach (KeyValuePair<AgentUserDTO.SearchByParameters, string> searchParameter in searchParameters)
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
                    if (searchParameters.Count > 0)
                        agentsUsersQuery = agentsUsersQuery + query;
                }

                DataTable dtAgentsDTO = dataAccessHandler.executeSelectQuery(agentsUsersQuery, parameters.ToArray(),sqlTransaction);
                List<AgentUserDTO> agentUserDTOList = new List<AgentUserDTO>();
                if (dtAgentsDTO.Rows.Count > 0)
                {
                    foreach (DataRow agentsDTORow in dtAgentsDTO.Rows)
                    {
                        AgentUserDTO AgentUserDTO = GetAgentsUserDTO(agentsDTORow);
                        agentUserDTOList.Add(AgentUserDTO);
                    }
                }
                log.LogMethodExit(agentUserDTOList);
                return agentUserDTOList;
        }
    }
}
