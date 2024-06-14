/********************************************************************************************
 * Project Name - UserToAttendanceRolesMapDataHandler
 * Description  - Data handler file for  User To Attendance Roles Map
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90.0      03-Jul-2020   Akshay Gulaganji        Created 
 *2.100.0     22-Aug-2020   Vikas Dwivedi           Resolved issues in CURD operations
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// User To Attendance Roles Map Data Handler - Handles insert, update and select of User To Attendance Roles Map objects
    /// </summary>
    public class UserToAttendanceRolesMapDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM UserToAttendanceRolesMap as uarm ";

        /// <summary>
        /// Dictionary for searching Parameters for the UserToAttendanceRolesMap object
        /// </summary>
        private static readonly Dictionary<UserToAttendanceRolesMapDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<UserToAttendanceRolesMapDTO.SearchByParameters, string>
        {
            { UserToAttendanceRolesMapDTO.SearchByParameters.USER_TO_ATTENDANCE_ROLES_MAP_ID,"uarm.UserToAttendanceRolesMapId"},
            { UserToAttendanceRolesMapDTO.SearchByParameters.ATTENDANCE_ROLE_ID,"uarm.AttendanceRoleId"},
            { UserToAttendanceRolesMapDTO.SearchByParameters.USER_ID,"uarm.UserId"},
            { UserToAttendanceRolesMapDTO.SearchByParameters.APPROVAL_REQUIRED,"uarm.ApprovalRequired"},
            { UserToAttendanceRolesMapDTO.SearchByParameters.EFFECTIVE_DATE_LESS_THAN_OR_EQUALS,"uarm.EffectiveDate"},
            { UserToAttendanceRolesMapDTO.SearchByParameters.END_DATE_GREATER_THAN,"uarm.EndDate"},
            { UserToAttendanceRolesMapDTO.SearchByParameters.IS_ACTIVE,"uarm.IsActive"},
            { UserToAttendanceRolesMapDTO.SearchByParameters.SITE_ID,"uarm.site_id"},
            { UserToAttendanceRolesMapDTO.SearchByParameters.USER_ID_LIST,"uarm.UserId"}
        };

        /// <summary>
        /// Parameterized Constructor for UserToAttendanceRolesMapDataHandler
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction Object</param>
        public UserToAttendanceRolesMapDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating User To Attendance Roles Map Record
        /// </summary>
        /// <param name="userToAttendanceRolesMapDTO">userToAttendanceRolesMapDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userToAttendanceRolesMapDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@userToAttendanceRolesMapId", userToAttendanceRolesMapDTO.UserToAttendanceRolesMapId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@attendanceRoleId", userToAttendanceRolesMapDTO.AttendanceRoleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userId", userToAttendanceRolesMapDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@approvalRequired", userToAttendanceRolesMapDTO.ApprovalRequired));
            parameters.Add(dataAccessHandler.GetSQLParameter("@effectiveDate", userToAttendanceRolesMapDTO.EffectiveDate, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@endDate", userToAttendanceRolesMapDTO.EndDate, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", userToAttendanceRolesMapDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", userToAttendanceRolesMapDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to PayConfigurationMapDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of payConfigurationMapDTO</returns>
        private UserToAttendanceRolesMapDTO GetUserToAttendanceRolesMapDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTO = new UserToAttendanceRolesMapDTO(
                                                dataRow["UserToAttendanceRolesMapId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserToAttendanceRolesMapId"]),
                                                dataRow["UserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserId"]),
                                                dataRow["AttendanceRoleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AttendanceRoleId"]),
                                                dataRow["ApprovalRequired"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ApprovalRequired"]),
                                                dataRow["EffectiveDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EffectiveDate"]),
                                                dataRow["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["EndDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"].ToString()),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                               );
            log.LogMethodExit(userToAttendanceRolesMapDTO);
            return userToAttendanceRolesMapDTO;
        }

        /// <summary>
        /// Gets the UserToAttendanceRolesMap data of passed User To Attendance Roles Map Id
        /// </summary>
        /// <param name="userToAttendanceRolesMapId">payConfigurationMapId is passed as Parameter</param>
        /// <returns>Returns UserToAttendanceRolesMapDTO</returns>
        public UserToAttendanceRolesMapDTO GetUserToAttendanceRolesMapDTO(int userToAttendanceRolesMapId)
        {
            log.LogMethodEntry(userToAttendanceRolesMapId);
            UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTO = null;
            string query = SELECT_QUERY + @" WHERE uarm.UserToAttendanceRolesMapId = @userToAttendanceRoleMapId";
            SqlParameter parameter = new SqlParameter("@userToAttendanceRoleMapId", userToAttendanceRolesMapId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                userToAttendanceRolesMapDTO = GetUserToAttendanceRolesMapDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(userToAttendanceRolesMapDTO);
            return userToAttendanceRolesMapDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="userToAttendanceRolesMapDTO">UserToAttendanceRolesMapDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshUserToAttendanceRolesMapDTO(UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTO, DataTable dt)
        {
            log.LogMethodEntry(userToAttendanceRolesMapDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                userToAttendanceRolesMapDTO.UserToAttendanceRolesMapId = Convert.ToInt32(dataRow["UserToAttendanceRolesMapId"]);
                userToAttendanceRolesMapDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                userToAttendanceRolesMapDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                userToAttendanceRolesMapDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                userToAttendanceRolesMapDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                userToAttendanceRolesMapDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                userToAttendanceRolesMapDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the UserToAttendanceRolesMap Table. 
        /// </summary>
        /// <param name="userToAttendanceRolesMapDTO">UserToAttendanceRolesMapDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PayConfigurationDetailsDTO</returns>
        public UserToAttendanceRolesMapDTO Insert(UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userToAttendanceRolesMapDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[UserToAttendanceRolesMap]
                            (
                            UserId,
                            AttendanceRoleId,
                            ApprovalRequired,
                            EffectiveDate,
                            EndDate,
                            IsActive,
                            Guid,
                            site_id,
                            MasterEntityId,
                            LastUpdatedDate,
                            LastUpdatedBy,
                            CreatedBy,
                            CreationDate
                            )
                            VALUES
                            (
                            @userId,
                            @attendanceRoleId,
                            @approvalRequired,
                            @effectiveDate,
                            @endDate,
                            @isActive,
                            NEWID(),
                            @siteId,
                            @masterEntityId,
                            GETDATE(),
                            @lastUpdatedBy,
                            @createdBy,
                            GETDATE()                      
                            )
                            SELECT uarm.* FROM UserToAttendanceRolesMap uarm WHERE uarm.UserToAttendanceRolesMapId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userToAttendanceRolesMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserToAttendanceRolesMapDTO(userToAttendanceRolesMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting UserToAttendanceRolesMapDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userToAttendanceRolesMapDTO);
            return userToAttendanceRolesMapDTO;
        }

        /// <summary>
        /// Update the record in the UserToAttendanceRolesMap Table. 
        /// </summary>
        /// <param name="userToAttendanceRolesMapDTO">UserToAttendanceRolesMapDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PayConfigurationDetailsDTO</returns>
        public UserToAttendanceRolesMapDTO Update(UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userToAttendanceRolesMapDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[UserToAttendanceRolesMap]
                             SET
                             UserId = @userId,
                             AttendanceRoleId = @attendanceRoleId,
                             ApprovalRequired = @approvalRequired,
                             EffectiveDate = @effectiveDate,
                             EndDate = @endDate,
                             IsActive = @isActive,
                             LastUpdatedDate = GETDATE(),
                             LastUpdatedBy = @lastUpdatedBy        
                             WHERE UserToAttendanceRolesMapId = @userToAttendanceRolesMapId
                            SELECT * FROM UserToAttendanceRolesMap uarm WHERE uarm.UserToAttendanceRolesMapId = @userToAttendanceRolesMapId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userToAttendanceRolesMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserToAttendanceRolesMapDTO(userToAttendanceRolesMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating UserToAttendanceRolesMapDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userToAttendanceRolesMapDTO);
            return userToAttendanceRolesMapDTO;
        }

        /// <summary>
        /// Returns the List of UserToAttendanceRolesMapDTO based on the search parameters
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of UserToAttendanceRolesMapDTO</returns>
        public List<UserToAttendanceRolesMapDTO> GetUserToAttendanceRolesMapDTOList(List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOList = new List<UserToAttendanceRolesMapDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == UserToAttendanceRolesMapDTO.SearchByParameters.USER_TO_ATTENDANCE_ROLES_MAP_ID ||
                            searchParameter.Key == UserToAttendanceRolesMapDTO.SearchByParameters.ATTENDANCE_ROLE_ID ||
                            searchParameter.Key == UserToAttendanceRolesMapDTO.SearchByParameters.USER_ID ||
                            searchParameter.Key == UserToAttendanceRolesMapDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UserToAttendanceRolesMapDTO.SearchByParameters.IS_ACTIVE ||
                                 searchParameter.Key == UserToAttendanceRolesMapDTO.SearchByParameters.APPROVAL_REQUIRED
                                )
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == UserToAttendanceRolesMapDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UserToAttendanceRolesMapDTO.SearchByParameters.USER_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == UserToAttendanceRolesMapDTO.SearchByParameters.EFFECTIVE_DATE_LESS_THAN_OR_EQUALS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == UserToAttendanceRolesMapDTO.SearchByParameters.END_DATE_GREATER_THAN)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'GETDATE()')" + " > " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTO = GetUserToAttendanceRolesMapDTO(dataRow);
                    userToAttendanceRolesMapDTOList.Add(userToAttendanceRolesMapDTO);
                }
            }
            log.LogMethodExit(userToAttendanceRolesMapDTOList);
            return userToAttendanceRolesMapDTOList;
        }
    }
}
