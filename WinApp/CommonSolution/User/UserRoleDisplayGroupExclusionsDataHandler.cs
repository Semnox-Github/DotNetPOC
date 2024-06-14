
/********************************************************************************************
 * Project Name - UserRole DisplayGroupsDataHandler Data Handler
 * Description  - Data handler of the userRoleDisplayGroupExclusions class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00       18-May-2016   Amaresh          Created 
 *2.3.0      25-Jun-2018   Guru S A          Rename the class as per db object modifications
 *                                           For User role level product exclusion change 
 *2.60.0     03-May-2019   Divya             SQL Injection
 *2.70.0    07-Aug-2019    Mushahid Faizan   Added isActive Column.
 *2.70.2      10-Dec-2019   Jinto Thomas        Removed siteid from update query
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// User Role Display Group DataHandler - Handles insert, update and select of  userRoleDisplayGroupExclusions objects
    /// </summary>

    public class UserRoleDisplayGroupExclusionsDataHandler
    {
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters, string> DBSearchParameters = new Dictionary<UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters, string>
        {
              {UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.ROLE_DISPLAY_GROUP_ID , "RoleDisplayGroupId"},
              {UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.ROLE_ID , "Role_id"},
              {UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.SITE_ID , "site_id"},
              {UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.ISACTIVE , "IsActive"},
              {UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.ROLE_ID_LIST , "Role_id"}  ,
              {UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.MASTER_ENTITY_ID , "MasterEntityId"}
        };

        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        /// <summary>
        /// Default constructor of UserRoleDisplayGroupExclusionsDataHandler class
        /// </summary>
        public UserRoleDisplayGroupExclusionsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating UserRoleDisplayGroupExclusionsDTO Record.
        /// </summary>
        /// <param name="UserRoleDisplayGroupExclusionsDTO">UserRoleDisplayGroupExclusionsDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(UserRoleDisplayGroupExclusionsDTO userRoleDisplayGroupExclusionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userRoleDisplayGroupExclusionsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@roleDisplayGroupId", userRoleDisplayGroupExclusionsDTO.RoleDisplayGroupId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@roleId", userRoleDisplayGroupExclusionsDTO.RoleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productDisplayGroupId", userRoleDisplayGroupExclusionsDTO.ProductDisplayGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", userRoleDisplayGroupExclusionsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@user", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", userRoleDisplayGroupExclusionsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Inserts the userRole DisplayGroups record to the database
        /// </summary>
        /// <param name="userRoleDisplayGroupExclusions">UserRoleDisplayGroupExclusionsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public UserRoleDisplayGroupExclusionsDTO InsertUserRoleDisplayGroupExclusions(UserRoleDisplayGroupExclusionsDTO userRoleDisplayGroupExclusions, string loginId, int siteId)
        {
            log.LogMethodEntry(userRoleDisplayGroupExclusions, loginId, siteId);
            string query = @"insert into UserRoleDisplayGroupExclusions 
                                                        (                                                 
                                                         Role_id,
                                                         ProductDisplayGroupId,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdatedUser,
                                                         LastUpdatedDate,
                                                         site_id,
                                                         Guid,
                                                         IsActive,
                                                         MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                          @roleId,
                                                          @productDisplayGroupId,
                                                          @user,
                                                          GetDate(),
                                                          @user,
                                                          GetDate(),
                                                          @siteId,
                                                          Newid(),
                                                          @isActive,
                                                          @MasterEntityId
                                                         )  
                            SELECT * FROM UserRoleDisplayGroupExclusions WHERE RoleDisplayGroupId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userRoleDisplayGroupExclusions, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserRoleDisplayGroupExclusions(userRoleDisplayGroupExclusions, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting userRoleDisplayGroupExclusions", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userRoleDisplayGroupExclusions);
            return userRoleDisplayGroupExclusions;
        }

        private void RefreshUserRoleDisplayGroupExclusions(UserRoleDisplayGroupExclusionsDTO userRoleDisplayGroupExclusionsDTO, DataTable dt)
        {
            log.LogMethodEntry(userRoleDisplayGroupExclusionsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                userRoleDisplayGroupExclusionsDTO.RoleDisplayGroupId = Convert.ToInt32(dt.Rows[0]["RoleDisplayGroupId"]);
                userRoleDisplayGroupExclusionsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdatedDate"]);
                userRoleDisplayGroupExclusionsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                userRoleDisplayGroupExclusionsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                userRoleDisplayGroupExclusionsDTO.LastUpdatedBy = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastUpdatedUser"]);
                userRoleDisplayGroupExclusionsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                userRoleDisplayGroupExclusionsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Updates the UserRoleDisplayGroupExclusions  record
        /// </summary>
        /// <param name="UserRoleDisplayGroupExclusions">UserRoleDisplayGroupExclusionsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public UserRoleDisplayGroupExclusionsDTO UpdateUserRoleDisplayGroupExclusions(UserRoleDisplayGroupExclusionsDTO userRoleDisplayGroupExclusions, string loginId, int siteId)
        {
            log.LogMethodEntry(userRoleDisplayGroupExclusions, loginId, siteId);
            string query = @"update UserRoleDisplayGroupExclusions
                                                   set   Role_id =@roleId,
                                                         ProductDisplayGroupId =@productDisplayGroupId,
                                                         LastUpdatedDate = GetDate(),
                                                         LastUpdatedUser =@user,
                                                         MasterEntityId =@MasterEntityId, 
                                                         IsActive=@isActive                                                                              
                                                         where  RoleDisplayGroupId =@roleDisplayGroupId;
                                  SELECT* FROM UserRoleDisplayGroupExclusions WHERE RoleDisplayGroupId =@roleDisplayGroupId";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userRoleDisplayGroupExclusions, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserRoleDisplayGroupExclusions(userRoleDisplayGroupExclusions, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating userRoleDisplayGroupExclusions", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userRoleDisplayGroupExclusions);
            return userRoleDisplayGroupExclusions;
        }

        /// <summary>
        /// Converts the Data row object to UserRoleDisplayGroupExclusionsDTO class type
        /// </summary>
        /// <param name="userRoleDisplayGroupExclusionsDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns ProductDisplayGroupFormat</returns>
        private UserRoleDisplayGroupExclusionsDTO GetUserRoleDisplayGroupExclusionsDTO(DataRow userRoleDisplayGroupExclusionsDataRow)
        {
            log.LogMethodEntry(userRoleDisplayGroupExclusionsDataRow);
            UserRoleDisplayGroupExclusionsDTO userRoleDisplayGroupExclusionsDataObject = new UserRoleDisplayGroupExclusionsDTO(
                                                    Convert.ToInt32(userRoleDisplayGroupExclusionsDataRow["RoleDisplayGroupId"]),
                                                    userRoleDisplayGroupExclusionsDataRow["Role_id"] == DBNull.Value ? -1 : Convert.ToInt32(userRoleDisplayGroupExclusionsDataRow["Role_id"]),
                                                    userRoleDisplayGroupExclusionsDataRow["ProductDisplayGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(userRoleDisplayGroupExclusionsDataRow["ProductDisplayGroupId"]),
                                                    userRoleDisplayGroupExclusionsDataRow["CreatedBy"].ToString(),
                                                    userRoleDisplayGroupExclusionsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userRoleDisplayGroupExclusionsDataRow["CreationDate"]),
                                                    userRoleDisplayGroupExclusionsDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userRoleDisplayGroupExclusionsDataRow["LastUpdatedDate"]),
                                                    userRoleDisplayGroupExclusionsDataRow["LastUpdatedUser"].ToString(),
                                                    userRoleDisplayGroupExclusionsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(userRoleDisplayGroupExclusionsDataRow["site_id"]),
                                                    userRoleDisplayGroupExclusionsDataRow["Guid"].ToString(),
                                                    userRoleDisplayGroupExclusionsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(userRoleDisplayGroupExclusionsDataRow["SynchStatus"]),
                                                    userRoleDisplayGroupExclusionsDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(userRoleDisplayGroupExclusionsDataRow["IsActive"]),
                                                    userRoleDisplayGroupExclusionsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(userRoleDisplayGroupExclusionsDataRow["MasterEntityId"])
                                                    );
            log.LogMethodExit(userRoleDisplayGroupExclusionsDataRow);
            return userRoleDisplayGroupExclusionsDataObject;
        }

        /// <summary>
        /// Gets the GetUserRoleDisplayGroupExclusions data of passed displaygroup
        /// </summary>
        /// <param name="roleDisplayGroupId">integer type parameter</param>
        /// <returns>Returns UserRoleDisplayGroupExclusionsDTO</returns>
        public UserRoleDisplayGroupExclusionsDTO GetUserRoleDisplayGroupExclusions(int roleDisplayGroupId)
        {
            log.LogMethodEntry(roleDisplayGroupId);
            string selectUserRoleDisplayGroupExclusionsQuery = @"select *
                                         from UserRoleDisplayGroupExclusions
                                        where RoleDisplayGroupId = @roleDisplayGroupId";

            SqlParameter[] selectUserRoleDisplayGroupExclusionsParameters = new SqlParameter[1];
            selectUserRoleDisplayGroupExclusionsParameters[0] = new SqlParameter("@roleDisplayGroupId", roleDisplayGroupId);
            DataTable UserRoleDisplayGroupExclusions = dataAccessHandler.executeSelectQuery(selectUserRoleDisplayGroupExclusionsQuery, selectUserRoleDisplayGroupExclusionsParameters, sqlTransaction);

            if (UserRoleDisplayGroupExclusions.Rows.Count > 0)
            {
                DataRow UserRoleDisplayGroupExclusionsRow = UserRoleDisplayGroupExclusions.Rows[0];
                UserRoleDisplayGroupExclusionsDTO UserRoleDisplayGroupExclusionsDataObject = GetUserRoleDisplayGroupExclusionsDTO(UserRoleDisplayGroupExclusionsRow);
                log.LogMethodExit(UserRoleDisplayGroupExclusionsDataObject);
                return UserRoleDisplayGroupExclusionsDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the UserRoleDisplayGroupExclusionsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of UserRoleDisplayGroupExclusionsDTO matching the search criteria</returns>
        public List<UserRoleDisplayGroupExclusionsDTO> GetUserRoleDisplayGroupExclusionsList(List<KeyValuePair<UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            int count = 0;
            string selectUserRoleDisplayQuery = @"select *
                                         from UserRoleDisplayGroupExclusions";
            List<UserRoleDisplayGroupExclusionsDTO> userRoleDisplayGroupExclusionsList = new List<UserRoleDisplayGroupExclusionsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperator = (count == 0 ? " " : " and ");

                        if (searchParameter.Key.Equals(UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.ROLE_DISPLAY_GROUP_ID)
                            || searchParameter.Key.Equals(UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.SITE_ID)
                        {
                            query.Append(joinOperator + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.ROLE_ID))
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.ROLE_ID_LIST)
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.ISACTIVE))
                        {
                            query.Append(joinOperator + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joinOperator + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                if (searchParameters.Count > 0)
                    selectUserRoleDisplayQuery = selectUserRoleDisplayQuery + query;
                selectUserRoleDisplayQuery = selectUserRoleDisplayQuery + " Order by RoleDisplayGroupId";
            }

            DataTable userRoleDisplayData = dataAccessHandler.executeSelectQuery(selectUserRoleDisplayQuery, parameters.ToArray(), sqlTransaction);
            if (userRoleDisplayData.Rows.Count > 0)
            {
                foreach (DataRow userRoleDisplayDataRow in userRoleDisplayData.Rows)
                {
                    UserRoleDisplayGroupExclusionsDTO userRoleDisplayGroupExclusionsDataObject = GetUserRoleDisplayGroupExclusionsDTO(userRoleDisplayDataRow);
                    userRoleDisplayGroupExclusionsList.Add(userRoleDisplayGroupExclusionsDataObject);
                }
            }
            log.LogMethodExit(userRoleDisplayGroupExclusionsList);
            return userRoleDisplayGroupExclusionsList;
        }

        /// <summary>
        /// Based on the roleDisplayGroupId, appropriate TrxProfileTaxRules record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required 
        /// </summary>
        /// <param name="roleDisplayGroupId">roleDisplayGroupId is passed as parameter</param>
        internal void Delete(int roleDisplayGroupId)
        {
            log.LogMethodEntry(roleDisplayGroupId);
            string query = @"DELETE  
                             FROM UserRoleDisplayGroupExclusions
                             WHERE UserRoleDisplayGroupExclusions.RoleDisplayGroupId = @roleDisplayGroupId";
            SqlParameter parameter = new SqlParameter("@roleDisplayGroupId", roleDisplayGroupId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
    }

}
