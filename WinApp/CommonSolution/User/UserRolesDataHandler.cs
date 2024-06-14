/********************************************************************************************
 * Project Name - User Role Data Handler
 * Description  - Data handler of the User Role data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        28-Jun-2016   Raghuveera        Created 
 *2.00        04-Mar-2019   Indhu             Modified for Remote Shift Open/Close changes
 *2.70        08-May-2019   Mushahid Faizan   Added GetSQLParameters() Method & Modified InsertUserRole/UpdateUserRole.
 *2.70.2        15-Jul-2019   Girish Kundar     Modified : Added GetSQLParameter(),SQL Injection Fix,Missed Who columns
 *            05-Aug-2019   Mushahid Faizan   Added Delete method and IsActive column.
 *            04-Nov-2019   Jagan Mohana      Added new method GetFormAccessRoles()
 *2.70.2        11-Dec-2019   Jinto Thomas      Removed siteid from update query    
 *2.90.0      09-Jul-2020   Akshay Gulaganji    Modified : Added field ShiftConfigurationId
 *2.100.0     05-Sep-2020   Girish Kundar   Modified : POS UI redesign related changes 
 *2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement
  *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
  * ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// User Roles Data Handler - Handles insert, update and select of user roles data objects
    /// </summary>
    public class UserRolesDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM user_roles AS ur ";
        private static readonly Dictionary<UserRolesDTO.SearchByUserRolesParameters, string> DBSearchParameters = new Dictionary<UserRolesDTO.SearchByUserRolesParameters, string>
               {
                    {UserRolesDTO.SearchByUserRolesParameters.ROLE_ID, "ur.role_id"},
                    {UserRolesDTO.SearchByUserRolesParameters.ROLE, "ur.role"},
                    {UserRolesDTO.SearchByUserRolesParameters.SITE_ID, "ur.site_id"},
                    {UserRolesDTO.SearchByUserRolesParameters.ASSIGNED_MANAGER_ROLEID, "ur.AssignedManagerRoleId"},
                    {UserRolesDTO.SearchByUserRolesParameters.ENABLE_POS_CLOCKIN, "ur.EnablePOSClockIn"},
                    {UserRolesDTO.SearchByUserRolesParameters.MASTER_ENTITY_ID, "ur.MasterEntityId"},
                    {UserRolesDTO.SearchByUserRolesParameters.ALLOW_SHIFT_OPEN_CLOSE, "ur.AllowShiftOpenClose"},
                    {UserRolesDTO.SearchByUserRolesParameters.ISACTIVE, "ur.IsActive"},
                    {UserRolesDTO.SearchByUserRolesParameters.SHIFT_CONFIGURATION_ID, "ur.ShiftConfigurationId"},
                    {UserRolesDTO.SearchByUserRolesParameters.ROLE_ID_LIST, "ur.role_id"},
                    {UserRolesDTO.SearchByUserRolesParameters.ROLE_NAME_EXCLUSION_LIST, "ur.role"}
               };
        /// <summary>
        /// Default constructor of UserRolesDataHandler class
        /// </summary>
        public UserRolesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to UserRolesDTO class type
        /// </summary>
        /// <param name="userRoleDataRow">UserRolesDTO DataRow</param>
        /// <returns>Returns UserRolesDTO</returns>
        private UserRolesDTO GetUserRolesDTO(DataRow userRoleDataRow)
        {
            log.LogMethodEntry(userRoleDataRow);
            UserRolesDTO userRoleDataObject = new UserRolesDTO(Convert.ToInt32(userRoleDataRow["Role_Id"]),
                                            userRoleDataRow["role"] == DBNull.Value ? string.Empty : Convert.ToString(userRoleDataRow["role"]),
                                            userRoleDataRow["role_description"] == DBNull.Value ? string.Empty : Convert.ToString(userRoleDataRow["role_description"]),
                                            userRoleDataRow["manager_flag"].ToString(),
                                            userRoleDataRow["allow_pos_access"].ToString(),
                                            userRoleDataRow["DataAccessLevel"].ToString(),
                                            userRoleDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(userRoleDataRow["Guid"]),
                                            userRoleDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(userRoleDataRow["site_id"]),
                                            userRoleDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(userRoleDataRow["SynchStatus"]),
                                            userRoleDataRow["LastUpdatedBy"].ToString(),
                                            userRoleDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userRoleDataRow["LastUpdatedDate"]),
                                            userRoleDataRow["EnablePOSClockIn"] == DBNull.Value ? false : Convert.ToBoolean(userRoleDataRow["EnablePOSClockIn"]),
                                            userRoleDataRow["AllowShiftOpenClose"] == DBNull.Value ? false : Convert.ToBoolean(userRoleDataRow["AllowShiftOpenClose"]),
                                            userRoleDataRow["SecurityPolicyId"] == DBNull.Value ? -1 : Convert.ToInt32(userRoleDataRow["SecurityPolicyId"]),
                                            userRoleDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(userRoleDataRow["MasterEntityId"]),
                                            userRoleDataRow["AssignedManagerRoleId"] == DBNull.Value ? -1 : Convert.ToInt32(userRoleDataRow["AssignedManagerRoleId"]),
                                            userRoleDataRow["DataAccessRuleId"] == DBNull.Value ? -1 : Convert.ToInt32(userRoleDataRow["DataAccessRuleId"]),
                                            userRoleDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(userRoleDataRow["CreatedBy"]),
                                            userRoleDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userRoleDataRow["CreationDate"]),
                                             userRoleDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(userRoleDataRow["IsActive"]),
                                             userRoleDataRow["ShiftConfigurationId"] == DBNull.Value ? -1 : Convert.ToInt32(userRoleDataRow["ShiftConfigurationId"])
                                            );
            log.LogMethodExit(userRoleDataObject);
            return userRoleDataObject;
        }

        /// <summary>
        /// Gets the userRole data of passed user role id
        /// </summary>
        /// <param name="roleId">integer type parameter</param>
        /// <returns>Returns UserRolesDTO</returns>
        public UserRolesDTO GetUserRoles(int roleId)
        {
            log.LogMethodEntry(roleId);
            string selectUserRolesQuery = SELECT_QUERY + "  where ur. role_id = @roleId";
            UserRolesDTO userRoleDataObject = new UserRolesDTO();
            SqlParameter[] selectUserRolesParameters = new SqlParameter[1];
            selectUserRolesParameters[0] = new SqlParameter("@roleId", roleId);
            DataTable userRole = dataAccessHandler.executeSelectQuery(selectUserRolesQuery, selectUserRolesParameters, sqlTransaction);
            if (userRole.Rows.Count > 0)
            {
                DataRow userRoleRow = userRole.Rows[0];
                userRoleDataObject = GetUserRolesDTO(userRoleRow);
            }
            log.LogMethodExit(userRoleDataObject);
            return userRoleDataObject;

        }
        /// <summary>
        /// Gets the userRole data of passed user role guid
        /// </summary>
        /// <param name="roleGuid">guid type parameter</param>
        /// <returns>Returns UserRolesDTO</returns>
        public UserRolesDTO GetUserRoles(string roleGuid)
        {
            log.LogMethodEntry(roleGuid);
            string selectUserRolesQuery = SELECT_QUERY + "   where ur.guid = @guid";
            UserRolesDTO userRoleDataObject = null;
            SqlParameter[] selectUserRolesParameters = new SqlParameter[1];
            selectUserRolesParameters[0] = new SqlParameter("@guid", roleGuid);
            DataTable userRole = dataAccessHandler.executeSelectQuery(selectUserRolesQuery, selectUserRolesParameters, sqlTransaction);
            if (userRole.Rows.Count > 0)
            {
                DataRow userRoleRow = userRole.Rows[0];
                userRoleDataObject = GetUserRolesDTO(userRoleRow);
            }
            log.LogMethodExit(userRoleDataObject);
            return userRoleDataObject;

        }
        /// <summary>
        /// Returns the data table of all lower level roles with levels
        /// </summary>
        /// <param name="roleId"> The role id of the senior role</param>
        /// <param name="siteId"> The site id of the senior role belongs to</param>
        /// <param name="sqlTransaction"> sql transaction object</param>
        /// <returns>returns all role id data</returns>
        public DataTable GetLowerLevelsRoleData(int roleId, int siteId)
        {
            log.LogMethodEntry(roleId, siteId);
            string selectUserRolesQuery = @"WITH DirectReports (ManagerID, roleId, role, Level)
                                             AS
                                             (
                                             -- Anchor member definition
                                                 SELECT e.AssignedManagerRoleId, e.role_id, e.role, 0 AS Level
                                                 FROM dbo.user_roles AS e    
                                                 WHERE AssignedManagerRoleId = @roleId and (e.site_id = @siteId or @siteId=-1)
                                                 UNION ALL
                                             -- Recursive member definition
                                                 SELECT e.AssignedManagerRoleId, e.role_id, e.role,Level + 1
                                                 FROM dbo.user_roles AS e    
                                                 INNER JOIN DirectReports AS d
                                                 ON e.AssignedManagerRoleId = d.roleId and (e.site_id = @siteId or @siteId=-1)
                                             )
                                             -- Statement that executes the CTE
                                             SELECT ManagerID, roleId, role, Level
                                             FROM DirectReports";
            SqlParameter[] selectUserRolesParameters = new SqlParameter[2];
            selectUserRolesParameters[0] = new SqlParameter("@roleId", roleId);
            selectUserRolesParameters[1] = new SqlParameter("@siteId", siteId);
            DataTable userRole = dataAccessHandler.executeSelectQuery(selectUserRolesQuery, selectUserRolesParameters, sqlTransaction);
            log.LogMethodExit(userRole);
            return userRole;
        }
        /// <summary>
        /// Gets the UserRolesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of UserRolesDTO matching the search criteria</returns>
        public List<UserRolesDTO> GetUserRolesList(List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectUserRolesQuery = SELECT_QUERY;
            List<UserRolesDTO> userRoleList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joinOperartor;
                foreach (KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joinOperartor = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key.Equals(UserRolesDTO.SearchByUserRolesParameters.ROLE_ID) ||
                           searchParameter.Key.Equals(UserRolesDTO.SearchByUserRolesParameters.ASSIGNED_MANAGER_ROLEID) ||
                           searchParameter.Key.Equals(UserRolesDTO.SearchByUserRolesParameters.MASTER_ENTITY_ID) ||
                           searchParameter.Key.Equals(UserRolesDTO.SearchByUserRolesParameters.SHIFT_CONFIGURATION_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key.Equals(UserRolesDTO.SearchByUserRolesParameters.ROLE))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(UserRolesDTO.SearchByUserRolesParameters.SITE_ID))
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key) + " OR -1=" + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == UserRolesDTO.SearchByUserRolesParameters.ISACTIVE ||
                                   searchParameter.Key == UserRolesDTO.SearchByUserRolesParameters.ENABLE_POS_CLOCKIN)
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(UserRolesDTO.SearchByUserRolesParameters.ALLOW_SHIFT_OPEN_CLOSE))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(UserRolesDTO.SearchByUserRolesParameters.ROLE_ID_LIST))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(UserRolesDTO.SearchByUserRolesParameters.ROLE_NAME_EXCLUSION_LIST))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " NOT IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",' ') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                    selectUserRolesQuery = selectUserRolesQuery + query;
                selectUserRolesQuery = selectUserRolesQuery + " order by Role";
            }

            DataTable userRoleData = dataAccessHandler.executeSelectQuery(selectUserRolesQuery, parameters.ToArray(), sqlTransaction);
            if (userRoleData.Rows.Count > 0)
            {
                userRoleList = new List<UserRolesDTO>();
                foreach (DataRow userRoleDataRow in userRoleData.Rows)
                {
                    UserRolesDTO userRoleDataObject = GetUserRolesDTO(userRoleDataRow);
                    userRoleList.Add(userRoleDataObject);
                }
            }
            log.LogMethodExit(userRoleList);
            return userRoleList;
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating UserRolesDTO Record.
        /// </summary>
        /// <param name="userRolesDTO">userRolesDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(UserRolesDTO userRolesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userRolesDTO, loginId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@roleId", userRolesDTO.RoleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@role", userRolesDTO.Role));
            parameters.Add(dataAccessHandler.GetSQLParameter("@roleDescription", string.IsNullOrEmpty(userRolesDTO.Description) ? DBNull.Value : (object)userRolesDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@managerFlag", userRolesDTO.ManagerFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@allowPosAccess", userRolesDTO.AllowPosAccess));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dataAccessLevel", userRolesDTO.DataAccessLevel));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@enablePOSClockIn", userRolesDTO.EnablePOSClockIn));
            parameters.Add(dataAccessHandler.GetSQLParameter("@allowShiftOpenClose", userRolesDTO.AllowShiftOpenClose));
            parameters.Add(dataAccessHandler.GetSQLParameter("@securityPolicyId", userRolesDTO.SecurityPolicyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", userRolesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assignedManagerRoleId", userRolesDTO.AssignedManagerRoleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dataAccessRuleId", userRolesDTO.DataAccessRuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", userRolesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shiftConfigurationId", userRolesDTO.ShiftConfigurationId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Inserts the user roles record to the database
        /// </summary>
        /// <param name="userRolesDTO">userRolesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns UserRolesDTO </returns>
        public UserRolesDTO InsertUserRole(UserRolesDTO userRolesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userRolesDTO, loginId);
            string insertUserRolesQuery = @"insert into user_roles 
                                                        (                                                         
                                                        role,
                                                        role_description,
                                                        manager_flag,
                                                        allow_pos_access,
                                                        DataAccessLevel,                                                        
                                                        Guid,
                                                        site_id,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        EnablePOSClockIn,
                                                        AllowShiftOpenClose,
                                                        SecurityPolicyId,
                                                        MasterEntityId,
                                                        AssignedManagerRoleId,
                                                        DataAccessRuleId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        IsActive,
                                                        ShiftConfigurationId
                                                        ) 
                                                 values 
                                                        (                                                        
                                                        @role,
                                                        @roleDescription,
                                                        @managerFlag,
                                                        @allowPosAccess,
                                                        @dataAccessLevel,                                                        
                                                        NewId(),
                                                        @siteId,
                                                        @lastUpdatedBy,
                                                        GETDATE(),
                                                        @enablePOSClockIn,
                                                        @allowShiftOpenClose,
                                                        @securityPolicyId,
                                                        @masterEntityId,
                                                        @assignedManagerRoleId,
                                                        @dataAccessRuleId,
                                                        @createdBy,
                                                        GetDate(),
                                                        @isActive,
                                                        @shiftConfigurationId
                                            )  SELECT  * from user_roles where role_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertUserRolesQuery, GetSQLParameters(userRolesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserRolesDTO(userRolesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting userRolesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userRolesDTO);
            return userRolesDTO;
        }

        /// <summary>
        /// Updates the user role record
        /// </summary>
        /// <param name="userRolesDTO">userRolesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the UserRolesDTO</returns>
        public UserRolesDTO UpdateUserRole(UserRolesDTO userRolesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userRolesDTO, loginId);
            string updateUserRoleQuery = @"update user_roles 
                                         set role=@role,
                                             role_description= @roleDescription,                                                                                                  
                                             manager_flag = @managerFlag,
                                             allow_pos_access = @allowPosAccess,
                                             DataAccessLevel = @dataAccessLevel,                                            
                                             -- site_id = @siteId,
                                             LastUpdatedBy = @lastUpdatedBy,
                                             LastUpdatedDate = GETDATE(),
                                             EnablePOSClockIn = @enablePOSClockIn,
                                             AllowShiftOpenClose= @allowShiftOpenClose,
                                             SecurityPolicyId = @securityPolicyId,
                                             MasterEntityId = @masterEntityId,
                                             AssignedManagerRoleId = @assignedManagerRoleId,
                                             DataAccessRuleId = @dataAccessRuleId,
                                                   IsActive = @isActive ,                                                                                  
                                             ShiftConfigurationId = @shiftConfigurationId                                                                                 
                                            where role_id = @roleId 
                                  SELECT  * from user_roles where role_id = @roleId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateUserRoleQuery, GetSQLParameters(userRolesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserRolesDTO(userRolesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating userRolesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userRolesDTO);
            return userRolesDTO;
        }

        /// <summary>
        /// Based on the RoleId, appropriate User_Roles record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required 
        /// </summary>
        /// <param name="roleId">roleId is passed as parameter</param>
        internal void Delete(int roleId)
        {
            log.LogMethodEntry(roleId);
            //string query = @"DELETE  
            //                 FROM User_Roles
            //                 WHERE User_Roles.role_id = @roleId";
            string query = @"   BEGIN
                                DELETE FROM ManagementFormAccess
                                WHERE  role_id = @roleId and not exists (select 1 from users where role_id = @roleId);
                                END
                                ";
            query += @"  
                                BEGIN
                                DELETE FROM user_roles
                                WHERE role_id = @roleId
                                END
                            ";
            SqlParameter parameter = new SqlParameter("@roleId", roleId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="userRolesDTO">UserRolesDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshUserRolesDTO(UserRolesDTO userRolesDTO, DataTable dt)
        {
            log.LogMethodEntry(userRolesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                userRolesDTO.RoleId = Convert.ToInt32(dt.Rows[0]["role_id"]);
                userRolesDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                userRolesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                userRolesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                userRolesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                userRolesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                userRolesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
            }
            log.LogMethodExit();
        }
        public string GetFormAccessRoles(int roleId)
        {
            string roles = string.Empty;
            string query = @"declare @c varchar(max)
                                    set @c = '('
                                    select @c = @c + convert(varchar, u.role_id) + ', ' from user_roles u,ManagementFormAccess mfa
                                    where functiongroup = 'Data Access'
                                    and main_menu = 'User Roles'
                                    and mfa.role_id = @roleId
                                    and access_allowed = 'Y'
                                    and u.guid = mfa.functionguid
                                    select @c + '-1)'";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@roleId", roleId);
            DataTable users = dataAccessHandler.executeSelectQuery(query, selectUserParameters, sqlTransaction);
            if (users.Rows.Count > 0)
            {
                roles = users.Rows[0][0].ToString();
            }
            return roles;
        }

        public string GetDataAccessRuleLookup(int userId, int siteId)
        {
            string roles = string.Empty;
            string query = @"exec dbo.SetContextInfo @loginUserId ;declare @c varchar(max)
                                        set @c = '('
                                        select @c = @c + convert(varchar, u.role_id) + ', ' from user_roles u
                                        where (site_id = @site_id or @site_id = -1)  and u.role_Id in (select role_id from DataAccessRoleView dav where (dav.Entity='Role' and dav.DataAccessRuleId is not null)
                                                                       OR (dav.DataAccessRuleId is null ))
                                        select @c + '-1)'";
            List<SqlParameter> parameters = new List<SqlParameter>();            
            parameters.Add(new SqlParameter("@loginUserId", userId));
            parameters.Add(new SqlParameter("@site_id", siteId));
            DataTable users = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            if (users.Rows.Count > 0)
            {
                roles = users.Rows[0][0].ToString();
            }
            return roles;
        }

        internal DateTime? GetUserRoleModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastUpdatedDate) LastUpdatedDate from user_roles WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from UserRoleDisplayGroupExclusions WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from UserRolePriceList WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from ProductMenuPanelExclusion WHERE UserRoleId IS NOT NULL AND (site_id = @siteId or @siteId = -1)
                           ) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Inserts the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="functionalGuid">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void AddManagementFormAccess(string formName, string functionGuid, int siteId, bool isActive)
        {
            log.LogMethodEntry(formName, functionGuid, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'User Roles',@formName,'Data Access',@siteId,@functionGuid,@isActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", isActive));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Rename the managementFormAccess record to the database
        /// </summary>
        /// <param name="newFormName">string type object</param>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void RenameManagementFormAccess(string newFormName, string formName, int siteId, string functionGuid)
        {
            log.LogMethodEntry(newFormName, formName, siteId);
            string query = @"exec RenameManagementFormAccess @newFormName,'User Roles',@formName,'Data Access',@siteId,@functionGuid";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@newFormName", newFormName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Update the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="updatedIsActive">Site to which the record belongs</param>
        /// <param name="functionGuid">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void UpdateManagementFormAccess(string formName, int siteId, bool updatedIsActive, string functionGuid)
        {
            log.LogMethodEntry(formName, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'User Roles',@formName,'Data Access',@siteId,@functionGuid,@updatedIsActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@updatedIsActive", updatedIsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
    }
}
