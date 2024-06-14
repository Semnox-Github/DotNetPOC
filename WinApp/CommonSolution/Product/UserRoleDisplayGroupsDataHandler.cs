/********************************************************************************************
 * Project Name - UserRole DisplayGroupsDataHandler Data Handler
 * Description  - Data handler of the userRoleDisplayGroups class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       18-May-2016   Amaresh          Created 
 *2.70.2       10-Dec-2019   Jinto Thomas     Removed siteid from update query
 *2.110.0     03-Dec-2020    Prajwal S       Modiefied three tier
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Product
{
    /// <summary>
    /// User Role Display Group DataHandler - Handles insert, update and select of  userRoleDisplayGroups objects
    /// </summary>

    public class UserRoleDisplayGroupsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM UserRoleDisplayGroups AS urd ";


        private static readonly Dictionary<UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters, string> DBSearchParameters = new Dictionary<UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters, string>
        {
              {UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters.ROLE_DISPLAY_GROUP_ID , "urd.RoleDisplayGroupId"},
              {UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters.ROLE_ID , "urd.Role_id"},
               {UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters.IS_ACTIVE , "urd.IsActive"},
                {UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters.SITE_ID , "urd.site_id"},
                 {UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters.MASTER_ENTITY_ID , "urd.MasterEntityId"}
        };

        /// <summary>
        /// Default constructor of UserRoleDisplayGroupsDataHandler class
        /// </summary>
        public UserRoleDisplayGroupsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(UserRoleDisplayGroupsDTO userRoleDisplayGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userRoleDisplayGroupsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@RoleDisplayGroupId", userRoleDisplayGroupsDTO.RoleDisplayGroupId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RoleId", userRoleDisplayGroupsDTO.RoleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductDisplayGroupId", userRoleDisplayGroupsDTO.ProductDisplayGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", userRoleDisplayGroupsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", userRoleDisplayGroupsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", userRoleDisplayGroupsDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the userRole DisplayGroups record to the database
        /// </summary>
        /// <param name="userRoleDisplayGroups">UserRoleDisplayGroupsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        internal UserRoleDisplayGroupsDTO Insert(UserRoleDisplayGroupsDTO userRoleDisplayGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userRoleDisplayGroupsDTO, loginId, siteId);
           string query = @"INSERT INTO[dbo].[UserRoleDisplayGroups] 
                                                        (                                                 
                                                         Role_id,
                                                         ProductDisplayGroupId,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdatedUser,
                                                         LastUpdatedDate,
                                                         site_id,
                                                         Guid,
                                                         MasterEntityId,
                                                         IsActive,
                                                         SynchStatus
                                                        ) 
                                                values 
                                                        (
                                                          @RoleId,
                                                          @ProductDisplayGroupId,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedBy,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @IsActive,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM UserRoleDisplayGroups WHERE RoleDisplayGroupId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userRoleDisplayGroupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserRoleDisplayGroupsDTO(userRoleDisplayGroupsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userRoleDisplayGroupsDTO);
            return userRoleDisplayGroupsDTO;
        }

        private void RefreshUserRoleDisplayGroupsDTO(UserRoleDisplayGroupsDTO userRoleDisplayGroupsDTO, DataTable dt)
        {
            log.LogMethodEntry(userRoleDisplayGroupsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                userRoleDisplayGroupsDTO.RoleDisplayGroupId = Convert.ToInt32(dt.Rows[0]["RoleDisplayGroupId"]);
                userRoleDisplayGroupsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                userRoleDisplayGroupsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                userRoleDisplayGroupsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                userRoleDisplayGroupsDTO.LastUpdatedBy = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                userRoleDisplayGroupsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                userRoleDisplayGroupsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the UserRoleDisplayGroups  record
        /// </summary>
        /// <param name="UserRoleDisplayGroups">UserRoleDisplayGroupsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        internal UserRoleDisplayGroupsDTO Update(UserRoleDisplayGroupsDTO userRoleDisplayGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userRoleDisplayGroupsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[UserRoleDisplayGroups] set
                               [Role_id]                      = @RoleId,
                               [ProductDisplayGroupId]        = @ProductDisplayGroupId,
                               [MasterEntityId]               = @MasterEntityId,
                               [LastUpdatedUser]              = @LastUpdatedBy,
                               [LastUpdatedDate]               = GETDATE()
                               where RoleDisplayGroupId = @RoleDisplayGroupId
                             SELECT * FROM UserRoleDisplayGroups WHERE RoleDisplayGroupId = @RoleDisplayGroupId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userRoleDisplayGroupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserRoleDisplayGroupsDTO(userRoleDisplayGroupsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userRoleDisplayGroupsDTO);
            return userRoleDisplayGroupsDTO;
        }

        /// <summary>
        /// Converts the Data row object to UserRoleDisplayGroupsDTO class type
        /// </summary>
        /// <param name="userRoleDisplayGroupsDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns ProductDisplayGroupFormat</returns>
        private UserRoleDisplayGroupsDTO GetUserRoleDisplayGroupsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            UserRoleDisplayGroupsDTO userRoleDisplayGroupsDataObject = new UserRoleDisplayGroupsDTO(Convert.ToInt32(dataRow["RoleDisplayGroupId"]),
                                                    dataRow["Role_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Role_id"]),
                                                    dataRow["ProductDisplayGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductDisplayGroupId"]),
                                                    dataRow["CreatedBy"].ToString(),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["LastUpdatedUser"].ToString(),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["Guid"].ToString(),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])                                                    
                                                    );
            log.LogMethodExit();
            return userRoleDisplayGroupsDataObject;
        }

        /// <summary>
        /// Gets the GetUserRoleDisplayGroups data of passed displaygroup
        /// </summary>
        /// <param name="roleDisplayGroupId">integer type parameter</param>
        /// <returns>Returns UserRoleDisplayGroupsDTO</returns>
        internal UserRoleDisplayGroupsDTO GetUserRoleDisplayGroups(int roleDisplayGroupId)
        {
            log.LogMethodEntry(roleDisplayGroupId);
            UserRoleDisplayGroupsDTO result = null;
            string query = SELECT_QUERY + @" WHERE urd.RoleDisplayGroupId = @RoleDisplayGroupId";
            SqlParameter parameter = new SqlParameter("@RoleDisplayGroupId", roleDisplayGroupId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetUserRoleDisplayGroupsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }



        /// <summary>
        /// Gets the UserRoleDisplayGroupsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of UserRoleDisplayGroupsDTO matching the search criteria</returns>    
        internal List<UserRoleDisplayGroupsDTO> GetUserRoleDisplayGroupsList(List<KeyValuePair<UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
        log.LogMethodEntry(searchParameters, sqlTransaction);
        List<UserRoleDisplayGroupsDTO> userRoleDisplayGroupsDTOList = new List<UserRoleDisplayGroupsDTO>();
        List<SqlParameter> parameters = new List<SqlParameter>();
        string selectQuery = SELECT_QUERY;
        if ((searchParameters != null) && (searchParameters.Count > 0))
        {
            string joiner = string.Empty;
            int counter = 0;
            StringBuilder query = new StringBuilder(" WHERE ");
            foreach (KeyValuePair<UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters, string> searchParameter in searchParameters)
            {
                joiner = counter == 0 ? string.Empty : " and ";
                if (DBSearchParameters.ContainsKey(searchParameter.Key))
                {
                        if (searchParameter.Key == UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters.ROLE_ID ||
                            searchParameter.Key == UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                }
                else
                {
                    string message = "The query parameter does not exist " + searchParameter.Key;
                    log.LogVariableState("searchParameter.Key", searchParameter.Key);
                    log.LogMethodExit(null, "Throwing exception -" + message);
                    throw new Exception(message);
                }
                counter++;
            }
            selectQuery = selectQuery + query;
        }
        DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
        if (dataTable.Rows.Count > 0)
        {
                userRoleDisplayGroupsDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetUserRoleDisplayGroupsDTO(x)).ToList();
        }
        log.LogMethodExit(userRoleDisplayGroupsDTOList);
        return userRoleDisplayGroupsDTOList;
        }
    }
}


