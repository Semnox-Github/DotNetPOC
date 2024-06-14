/********************************************************************************************
 * Project Name - User
 * Description  - Data Handler File for UserPasswordHistory
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        4-June-2020   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// UserPasswordHistory Data Handler - Handles insert, update and select of UserPasswordHistory objects
    /// </summary>
    public class UserPasswordHistoryDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM UserPasswordHistory AS uph ";

        /// <summary>
        /// Dictionary for searching Parameters for the UserPasswordHistory object.
        /// </summary>
        private static readonly Dictionary<UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters, string> DBSearchParameters = new Dictionary<UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters, string>
        {
            { UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters.USER_PASSWORD_HISTORY_ID,"uph.Id"},
            { UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters.USER_ID,"uph.UserId"},
            { UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters.CHANGE_DATE,"uph.ChangeDate"},
            { UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters.SITE_ID,"uph.site_id"},
            { UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters.MASTER_ENTITY_ID,"uph.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for UserPasswordHistoryDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public UserPasswordHistoryDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating UserPasswordHistory Record.
        /// </summary>
        /// <param name="userPasswordHistoryDTO">userPasswordHistoryDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(UserPasswordHistoryDTO userPasswordHistoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userPasswordHistoryDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", userPasswordHistoryDTO.UserPasswordHistoryId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserId", userPasswordHistoryDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ChangeDate", userPasswordHistoryDTO.ChangeDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PasswordSalt", userPasswordHistoryDTO.PasswordSalt));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", userPasswordHistoryDTO.MasterEntityId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", userPasswordHistoryDTO.SynchStatus));
            SqlParameter parameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
            if (userPasswordHistoryDTO.PasswordHash == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = userPasswordHistoryDTO.PasswordHash;
            }
            parameters.Add(parameter);
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to UserPasswordHistoryDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of UserPasswordHistoryDTO</returns>
        private UserPasswordHistoryDTO GetUserPasswordHistoryDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            UserPasswordHistoryDTO userPasswordHistoryDTO = new UserPasswordHistoryDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                dataRow["UserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserId"]),
                                                dataRow["PasswordHash"] == DBNull.Value ? null : dataRow["PasswordHash"] as byte[],
                                                dataRow["ChangeDate"] == DBNull.Value ? DateTime.MinValue: Convert.ToDateTime(dataRow["ChangeDate"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                dataRow["PasswordSalt"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PasswordSalt"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                                );
            return userPasswordHistoryDTO;
        }

        /// <summary>
        /// Gets the UserPasswordHistory data of passed UserPasswordHistory Id
        /// </summary>
        /// <param name="UserPasswordHistoryId">UserPasswordHistoryId of UserPasswordHistory passed as parameter</param>
        /// <returns>Returns UserPasswordHistoryDTO</returns>
        public UserPasswordHistoryDTO GetUserPasswordHistoryDTO(int UserPasswordHistoryId)
        {
            log.LogMethodEntry(UserPasswordHistoryId);
            UserPasswordHistoryDTO result = null;
            string query = SELECT_QUERY + @" WHERE userpasswordhistory.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", UserPasswordHistoryId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetUserPasswordHistoryDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="userPasswordHistoryDTO">UserPasswordHistoryDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
       
        private void RefreshUserPasswordHistoryDTO(UserPasswordHistoryDTO userPasswordHistoryDTO, DataTable dt)
        {
            log.LogMethodEntry(userPasswordHistoryDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                userPasswordHistoryDTO.UserPasswordHistoryId = Convert.ToInt32(dt.Rows[0]["Id"]);
                userPasswordHistoryDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
                userPasswordHistoryDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                userPasswordHistoryDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                userPasswordHistoryDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastUpdatedBy"]);
                userPasswordHistoryDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                userPasswordHistoryDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the UserPasswordHistory Table. 
        /// </summary>
        /// <param name="userPasswordHistoryDTO">userPasswordHistoryDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated UserPasswordHistoryDTO</returns>
        public UserPasswordHistoryDTO Insert(UserPasswordHistoryDTO userPasswordHistoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userPasswordHistoryDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[UserPasswordHistory]
                            (
                            UserId,
                            PasswordHash,
                            ChangeDate,
                            site_id,
                            Guid,
                            SynchStatus,
                            PasswordSalt,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate
                            )
                            VALUES
                            (
                            @UserId,
                            @PasswordHash,
                            @ChangeDate,
                            @site_id,
                            NEWID(),
                            @SynchStatus,
                            @PasswordSalt,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE()                           
                            )
                            SELECT * FROM UserPasswordHistory WHERE ID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userPasswordHistoryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserPasswordHistoryDTO(userPasswordHistoryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting UserPasswordHistoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userPasswordHistoryDTO);
            return userPasswordHistoryDTO;
        }

        /// <summary>
        /// Update the record in the UserPasswordHistory Table. 
        /// </summary>
        /// <param name="userPasswordHistoryDTO">userPasswordHistoryDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated UserPasswordHistoryDTO</returns>
        public UserPasswordHistoryDTO Update(UserPasswordHistoryDTO userPasswordHistoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userPasswordHistoryDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[UserPasswordHistory]
                             SET
                             UserId = @UserId,
                             PasswordHash = @PasswordHash,
                             ChangeDate = @ChangeDate,
                             site_id = @site_id,
                             SynchStatus = @SynchStatus,
                             PasswordSalt = @PasswordSalt,
                             MasterEntityId = @MasterEntityId,
                             CreatedBy = @CreatedBy,
                             LastUpdatedBy = @LastUpdatedBy    
                            SELECT * FROM UserPasswordHistory WHERE ID = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userPasswordHistoryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserPasswordHistoryDTO(userPasswordHistoryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating UserPasswordHistoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userPasswordHistoryDTO);
            return userPasswordHistoryDTO;
        }

        /// <summary>
        /// Returns the List of UserPasswordHistoryDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of UserPasswordHistoryDTO </returns>
        public List<UserPasswordHistoryDTO> GetUserPasswordHistoryDTOList(List<KeyValuePair<UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<UserPasswordHistoryDTO> userPasswordHistoryDTOList = new List<UserPasswordHistoryDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters.USER_PASSWORD_HISTORY_ID ||
                            searchParameter.Key == UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters.USER_ID ||
                            searchParameter.Key == UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters.CHANGE_DATE)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                    UserPasswordHistoryDTO userPasswordHistoryDTO = GetUserPasswordHistoryDTO(dataRow);
                    userPasswordHistoryDTOList.Add(userPasswordHistoryDTO);
                }
            }
            log.LogMethodExit(userPasswordHistoryDTOList);
            return userPasswordHistoryDTOList;
        }
    }
}
