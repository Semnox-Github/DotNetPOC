/********************************************************************************************
 * Project Name - UserIdTags Datahandler
 * Description  - Data handler of the user class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        13-Apr-2017   Amaresh         Created 
 *2.70.2      15-Jul-2019   Girish Kundar   Modified : Added GetSQLParameter(),SQL Injection Fix,Missed Who columns
 *            09-Aug-2019   Mushahid Faizan Modified isActive Parameter in GetUserIdentificationTagsDTO() method and
 *                                          Added delete method for hard deletion.
 *2.70.2      11-Dec-2019   Jinto Thomas    Removed siteid from update query                                          
 *2.80        09-Apr-2020   Indrajeet Kumar Modified : BuildSQLParameters() & GetUserIdentificationTagsDTO() Method
 *                                          Updated the query in InsertUserIdentificationTag & UpdateUserIdentificationTag.
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
    /// <summary>
    /// UserIdTagsDataHandler class
    /// </summary>
    public class UserIdTagsDatahandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM UserIdentificationTags AS uit ";
        private static readonly Dictionary<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string> DBSearchParameters = new Dictionary<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>
            {
                {UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ID, "uit.Id"},
                {UserIdentificationTagsDTO.SearchByUserIdTagsParameters.USER_ID, "uit.UserId"},
                {UserIdentificationTagsDTO.SearchByUserIdTagsParameters.USER_ID_LIST, "uit.UserId"},
                {UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ACTIVE_FLAG, "uit.ActiveFlag"},
                {UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ATTENDANCE_READER_TAG, "uit.AttendanceReaderTag"},
                {UserIdentificationTagsDTO.SearchByUserIdTagsParameters.CARD_ID, "uit.CardId"},
                {UserIdentificationTagsDTO.SearchByUserIdTagsParameters.CARD_NUMBER, "uit.CardNumber"},
                {UserIdentificationTagsDTO.SearchByUserIdTagsParameters.MASTER_ENTITY_ID, "uit.MasterEntityId"},
                {UserIdentificationTagsDTO.SearchByUserIdTagsParameters.SITE_ID, "uit.site_id"},
                {UserIdentificationTagsDTO.SearchByUserIdTagsParameters.FP_TEMPLATE, "uit.fp_template"}
            };

        /// <summary>
        /// Default constructor of UserIdTagsDatahandler
        /// </summary>
        public UserIdTagsDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating UserIdentificationTags Record.
        /// </summary>
        /// <param name="userIdentificationTagsDTO">UserIdentificationTagsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(UserIdentificationTagsDTO userIdentificationTagsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userIdentificationTagsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@id", userIdentificationTagsDTO.Id, true);
            ParametersHelper.ParameterHelper(parameters, "@userId", userIdentificationTagsDTO.UserId, true);
            ParametersHelper.ParameterHelper(parameters, "@cardId", userIdentificationTagsDTO.CardId, true);
            ParametersHelper.ParameterHelper(parameters, "@fingerNumber", userIdentificationTagsDTO.FingerNumber == -1 ? DBNull.Value : (object)userIdentificationTagsDTO.FingerNumber);
            ParametersHelper.ParameterHelper(parameters, "@cardNumber", string.IsNullOrEmpty(userIdentificationTagsDTO.CardNumber) ? DBNull.Value : (object)userIdentificationTagsDTO.CardNumber);
            ParametersHelper.ParameterHelper(parameters, "@fingerPrint", string.IsNullOrEmpty(userIdentificationTagsDTO.FingerPrint) ? DBNull.Value : (object)userIdentificationTagsDTO.FingerPrint);
            ParametersHelper.ParameterHelper(parameters, "@activeFlag", userIdentificationTagsDTO.ActiveFlag);
            ParametersHelper.ParameterHelper(parameters, "@startDate", userIdentificationTagsDTO.StartDate == DateTime.MinValue ? DBNull.Value : (object)userIdentificationTagsDTO.StartDate);
            ParametersHelper.ParameterHelper(parameters, "@endDate", userIdentificationTagsDTO.EndDate == DateTime.MinValue ? DBNull.Value : (object)userIdentificationTagsDTO.EndDate);
            ParametersHelper.ParameterHelper(parameters, "@attendanceReaderTag", (userIdentificationTagsDTO.AttendanceReaderTag) ? true : false);
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@siteId", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@masterEntityId", userIdentificationTagsDTO.MasterEntityId, true);            
            SqlParameter parameter = new SqlParameter("@fp_template", SqlDbType.VarBinary);
            if (userIdentificationTagsDTO.FPTemplate == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = userIdentificationTagsDTO.FPTemplate;
            }
            ParametersHelper.ParameterHelper(parameters, "@FPSalt", userIdentificationTagsDTO.FPSalt);
            parameters.Add(parameter);            
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Gets the user identification tag data of passed id
        /// </summary>
        /// <param name="userId">integer type parameter</param>
        /// <returns>Returns UsersDTO</returns>
        internal UserIdentificationTagsDTO GetUserIdentificationTagsDTO(int id)
        {
            log.LogMethodEntry(id);
            UserIdentificationTagsDTO userIdentificationTagsDTO = null ;
            string selectQuery =SELECT_QUERY + "   where uit.Id = @Id";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@Id", id);
            DataTable table = dataAccessHandler.executeSelectQuery(selectQuery, parameters, sqlTransaction);
            if (table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                userIdentificationTagsDTO = GetUserIdentificationTagsDTO(row);
            }
            log.LogMethodExit(userIdentificationTagsDTO);
            return userIdentificationTagsDTO;
        }


        /// <summary>
        /// Inserts the user record to the database
        /// </summary>
        /// <param name="userIdentificationTagsDTO">UserIdentificationTagsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns UserIdentificationTagsDTO</returns>
        public UserIdentificationTagsDTO InsertUserIdentificationTag(UserIdentificationTagsDTO userIdentificationTagsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userIdentificationTagsDTO, loginId, siteId);
            string insertUserIdTagsQuery = @"insert into UserIdentificationTags 
                                                        (     
                                                        UserId,   
                                                        CardNumber,
                                                        FingerPrint,
                                                        FingerNumber,
                                                        ActiveFlag,                                                        
                                                        StartDate,
                                                        EndDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        AttendanceReaderTag,
                                                        CardId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        fp_template,
                                                        FPSalt
                                                        ) 
                                                values 
                                                        (    
                                                        @userId,      
                                                        @cardNumber,
                                                        @fingerPrint,
                                                        @fingerNumber,
                                                        @activeFlag,                                                                                                            
                                                        @startDate,
                                                        @endDate,
                                                        @lastUpdatedBy,
                                                        GetDate(),
                                                        NewId(),
                                                        @siteId,
                                                        @masterEntityId,
                                                        @attendanceReaderTag,
                                                        @cardId,
                                                        @createdBy,
                                                        GetDate(),
                                                        @fp_template,
                                                        @FPSalt
                                            )SELECT  * from UserIdentificationTags where Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertUserIdTagsQuery, BuildSQLParameters(userIdentificationTagsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserIdentificationTagsDTO(userIdentificationTagsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting userIdentificationTagsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userIdentificationTagsDTO);
            return userIdentificationTagsDTO;
        }

        internal List<UserIdentificationTagsDTO> GetUserIdentificationTagDTOListOfUsers(List<int> userIdList, bool activeRecords)
        {
            log.LogMethodEntry(userIdList);
            List<UserIdentificationTagsDTO> list = new List<UserIdentificationTagsDTO>();
            string query = @"SELECT UserIdentificationTags.*
                            FROM UserIdentificationTags, @UserIdList List
                            WHERE UserId = List.Id ";
            if (activeRecords)
            {
                query += " AND ActiveFlag = 1 ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@UserIdList", userIdList, null, sqlTransaction);
            foreach (DataRow usersDataRow in table.Rows)
            {
                UserIdentificationTagsDTO userIdentificationTagsDTO = GetUserIdentificationTagsDTO(usersDataRow);
                list.Add(userIdentificationTagsDTO);
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Updates the user record
        /// </summary>
        /// <param name="userIdentificationTagsDTO">UserIdentificationTagsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns UserIdentificationTagsDTO</returns>
        public UserIdentificationTagsDTO UpdateUserIdentificationTag(UserIdentificationTagsDTO userIdentificationTagsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userIdentificationTagsDTO, loginId, siteId);
            string updateUserIdTagsQuery = @"update UserIdentificationTags set  
                                                        UserId = @userId,
                                                        CardNumber = @cardNumber,
                                                        FingerPrint = @fingerPrint,
                                                        FingerNumber = @fingerNumber,
                                                        ActiveFlag = @activeFlag,
                                                        StartDate = @startDate,
                                                        EndDate = @endDate,
                                                        LastUpdatedBy = @lastUpdatedBy,
                                                        LastUpdatedDate = getdate(),
                                                        -- site_id = @siteId,
                                                        MasterEntityId = @masterEntityId,
                                                        AttendanceReaderTag = @attendanceReaderTag,
                                                        CardId = @cardId,
                                                        fp_template = @fp_template
                                                        where Id = @id
                                             SELECT  * from UserIdentificationTags where Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateUserIdTagsQuery, BuildSQLParameters(userIdentificationTagsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserIdentificationTagsDTO(userIdentificationTagsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating userIdentificationTagsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userIdentificationTagsDTO);
            return userIdentificationTagsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="userIdentificationTagsDTO">UserIdentificationTagsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshUserIdentificationTagsDTO(UserIdentificationTagsDTO userIdentificationTagsDTO, DataTable dt)
        {
            log.LogMethodEntry(userIdentificationTagsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                userIdentificationTagsDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                userIdentificationTagsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                userIdentificationTagsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                userIdentificationTagsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                userIdentificationTagsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                userIdentificationTagsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                userIdentificationTagsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
            }
            log.LogMethodExit();
        }




        /// <summary>
        /// Converts the Data row object to UserIdentificationTagsDTO class type
        /// </summary>
        /// <param name="userIdtagsDataRow">UserIdentificationTagsDTO DataRow</param>
        /// <returns>Returns UserIdentificationTagsDTO</returns>
        private UserIdentificationTagsDTO GetUserIdentificationTagsDTO(DataRow userIdtagsDataRow)
        {
            log.LogMethodEntry(userIdtagsDataRow);
            UserIdentificationTagsDTO userIdTagsObject = new UserIdentificationTagsDTO(Convert.ToInt32(userIdtagsDataRow["Id"]),
                                                    Convert.ToInt32(userIdtagsDataRow["UserId"]),
                                                    userIdtagsDataRow["CardNumber"] == DBNull.Value ? string.Empty : Convert.ToString(userIdtagsDataRow["CardNumber"]),
                                                    userIdtagsDataRow["FingerPrint"] == DBNull.Value ? string.Empty : Convert.ToString(userIdtagsDataRow["FingerPrint"]),
                                                    userIdtagsDataRow["FingerNumber"] == DBNull.Value ? -1 : Convert.ToInt32(userIdtagsDataRow["FingerNumber"]),
                                                    userIdtagsDataRow["ActiveFlag"] == DBNull.Value ? true : Convert.ToBoolean(userIdtagsDataRow["ActiveFlag"]),
                                                    userIdtagsDataRow["StartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userIdtagsDataRow["StartDate"]),
                                                    userIdtagsDataRow["EndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userIdtagsDataRow["EndDate"]),
                                                    userIdtagsDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(userIdtagsDataRow["LastUpdatedBy"]),
                                                    userIdtagsDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userIdtagsDataRow["LastupdatedDate"]),
                                                    userIdtagsDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(userIdtagsDataRow["Guid"]),
                                                    userIdtagsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(userIdtagsDataRow["site_id"]),
                                                    userIdtagsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(userIdtagsDataRow["SynchStatus"]),
                                                    userIdtagsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(userIdtagsDataRow["MasterEntityId"]),
                                                    userIdtagsDataRow["AttendanceReaderTag"] == DBNull.Value ? false : Convert.ToBoolean(userIdtagsDataRow["AttendanceReaderTag"]),
                                                    userIdtagsDataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(userIdtagsDataRow["CardId"]),
                                                    userIdtagsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(userIdtagsDataRow["CreatedBy"]),
                                                    userIdtagsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userIdtagsDataRow["CreationDate"]),
                                                    userIdtagsDataRow["fp_template"] == DBNull.Value ? new Byte[0] : (Byte[])(userIdtagsDataRow["fp_template"]),
                                                    userIdtagsDataRow["FPSalt"] == DBNull.Value ? string.Empty : Convert.ToString(userIdtagsDataRow["FPSalt"])
                                                    );
            log.LogMethodExit(userIdTagsObject);
            return userIdTagsObject;
        }

        /// <summary>
        /// Gets the UserIdentificationTagsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of UserIdentificationTagsDTO matching the search criteria</returns>
        public List<UserIdentificationTagsDTO> GetUserIdTagsList(List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<UserIdentificationTagsDTO> userIdTagsList = null;
            string selectUsersIdTagsQuery = SELECT_QUERY;

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key.Equals(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ID) ||
                            searchParameter.Key.Equals(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.CARD_ID) ||
                            searchParameter.Key.Equals(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.USER_ID) ||
                            searchParameter.Key.Equals(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.MASTER_ENTITY_ID) ||
                                searchParameter.Key.Equals(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ATTENDANCE_READER_TAG))

                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.SITE_ID))
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ACTIVE_FLAG)
                            || searchParameter.Key.Equals(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ATTENDANCE_READER_TAG))
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == UserIdentificationTagsDTO.SearchByUserIdTagsParameters.USER_ID_LIST)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.FP_TEMPLATE))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.CARD_NUMBER))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                    selectUsersIdTagsQuery = selectUsersIdTagsQuery + query;
                selectUsersIdTagsQuery = selectUsersIdTagsQuery + " Order by UserId";
            }

            DataTable usersIdTagData = dataAccessHandler.executeSelectQuery(selectUsersIdTagsQuery, parameters.ToArray(), sqlTransaction);
            if (usersIdTagData.Rows.Count > 0)
            {
                userIdTagsList = new List<UserIdentificationTagsDTO>();
                foreach (DataRow usersDataRow in usersIdTagData.Rows)
                {
                    UserIdentificationTagsDTO usersDataObject = GetUserIdentificationTagsDTO(usersDataRow);
                    userIdTagsList.Add(usersDataObject);
                }
            }
            log.LogMethodExit(userIdTagsList);
            return userIdTagsList;

        }

        /// <summary>
        /// Get all available Staff Cards
        /// </summary>
        /// <returns>returns existing staff cards</returns>
        public List<UserIdentificationTagsDTO> GetStaffCards()
        {
            log.LogMethodEntry();
            try
            {
                List<UserIdentificationTagsDTO> userIdTagsList = null;
                string selectUsersIdTagsQuery = @"SELECT distinct(c.card_id), c.card_number + case when ISNULL(u.username, '') = '' then '' else ' - ' + u.username + ' ' + u.EmpLastName end CardNumber, UIT.* 
                                                    FROM cards C, UserIdentificationTags UIT ,users U
			                                      WHERE C.card_id = UIT.CardId 
				                                    AND UIT.UserId = U.user_id
				                                    AND C.valid_flag = 'Y'
				                                    AND C.technician_card = 'Y' 
				                                    AND UIT.ActiveFlag = 1
				                                    AND U.active_flag = 'Y' 
                                                    AND U.UserStatus = 'ACTIVE' 
			                                     ORDER by 1";

                DataTable usersIdTagData = dataAccessHandler.executeSelectQuery(selectUsersIdTagsQuery, null, sqlTransaction);
                if (usersIdTagData.Rows.Count > 0)
                {
                    userIdTagsList = new List<UserIdentificationTagsDTO>();
                    foreach (DataRow usersDataRow in usersIdTagData.Rows)
                    {
                        UserIdentificationTagsDTO usersDataObject = GetUserIdentificationTagsDTO(usersDataRow);
                        userIdTagsList.Add(usersDataObject);
                    }
                }
                log.LogMethodExit(userIdTagsList);
                return userIdTagsList;

            }
            catch (Exception ex)
            {
                log.Error("Error occurred at GetStaffCards()", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Based on the Id, appropriate UserIdentificationTags details record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required
        /// </summary>
        /// <param name="id">primary key of UserIdentificationTags </param>
        /// <returns>return the int </returns>
        public int Delete(int id)
        {
            log.LogMethodEntry(id);
            try
            {
                string deleteQuery = @"delete from UserIdentificationTags where Id = @id";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@id", id);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }
    }
}
