/********************************************************************************************
 * Project Name - User Target Data Handler
 * Description  - Data handler of the User Target class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.80        01-Jun-2020   Vikas Dwivedi       Created                                                        
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.User
{
    public class UserTargetDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM UserTarget AS ut ";

        /// <summary>
        /// Dictionary for searching Parameters for the UserTarget object.
        /// </summary>
        private static readonly Dictionary<UserTargetDTO.SearchByUserTargetSearchParameters, string> DBSearchParameters = new Dictionary<UserTargetDTO.SearchByUserTargetSearchParameters, string>
            {
                {UserTargetDTO.SearchByUserTargetSearchParameters.USER_TARGET_ID, "ut.UserTargetId"},
                {UserTargetDTO.SearchByUserTargetSearchParameters.GAME_ID, "ut.GameId"},
                {UserTargetDTO.SearchByUserTargetSearchParameters.PERIOD_ID, "ut.PeriodId"},
                {UserTargetDTO.SearchByUserTargetSearchParameters.SITE_ID, "ut.site_id"},
                {UserTargetDTO.SearchByUserTargetSearchParameters.IS_ACTIVE, "ut.IsActive"},
                {UserTargetDTO.SearchByUserTargetSearchParameters.MASTER_ENTITY_ID, "ut.MasterEntityId"}
             };
        /// <summary>
        /// Parameterized Constructor for UserTargetDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public UserTargetDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating UserTarget Record.
        /// </summary>
        /// <param name="UserTargetDTO">UserTargetDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(UserTargetDTO userTargetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userTargetDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserTargetId", userTargetDTO.UserTargetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameId", userTargetDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PeriodId", userTargetDTO.PeriodId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Target", userTargetDTO.Target, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", userTargetDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", userTargetDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Converts the Data row object to UserTargetDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the UserTargetDTO</returns>
        private UserTargetDTO GetUserTargetDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            UserTargetDTO userTargetDTO = new UserTargetDTO(dataRow["UserTargetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserTargetId"]),
                                                         dataRow["GameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameId"]),
                                                         dataRow["PeriodId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PeriodId"]),
                                                         dataRow["Target"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Target"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"])
                                                        );
            log.LogMethodExit(userTargetDTO);
            return userTargetDTO;
        }
        /// <summary>
        /// Gets the UserTarget data of passed id 
        /// </summary>
        /// <param name="id">id of UserTarget is passed as parameter</param>
        /// <returns>Returns UserTarget</returns>
        public UserTargetDTO GetUserTargetDTO(int id)
        {
            log.LogMethodEntry(id);
            UserTargetDTO result = null;
            string query = SELECT_QUERY + @" WHERE ut.UserTargetId= @UserTargetId";
            SqlParameter parameter = new SqlParameter("@UserTargetId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetUserTargetDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Inserts the UserTargetDTO record to the database
        /// </summary>
        /// <param name="UserTargetDTO">UserTargetDTO type object</param>
        /// <returns>Returns inserted record id</returns>
        public UserTargetDTO InsertUserTarget(UserTargetDTO userTargetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userTargetDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[UserTarget]
                                                         (
                                                            GameId,
                                                            PeriodId,
                                                            Target,
                                                            LastUpdatedBy,
                                                            LastUpdatedDate,
                                                            Guid,
                                                            site_id,
                                                            MasterEntityId,
                                                            CreatedBy,
                                                            CreationDate,
                                                            IsActive
                                                         )
                                                       values
                                                         (
                                                            @GameId,
                                                            @PeriodId,
                                                            @Target,
                                                            @LastUpdatedBy,
                                                            GetDate(),
                                                            NewId(),
                                                            @site_id,
                                                            @MasterEntityId,
                                                            @CreatedBy,
                                                            GETDATE(),
                                                            @IsActive
                                                        ) SELECT* FROM UserTarget WHERE UserTargetId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userTargetDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserTargetDTO(userTargetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting UserTargetDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userTargetDTO);
            return userTargetDTO;
        }
        /// <summary>
        /// Updates the UserTargetDTO record to the database
        /// </summary>
        /// <param name="userTargetDTO">UserTargetDTO type object</param>
        /// <returns>Returns the count of updated rows</returns>
        public UserTargetDTO UpdateUserTarget(UserTargetDTO userTargetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userTargetDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[UserTarget]
                           SET 
                                                            GameId =  @GameId,
                                                            PeriodId =  @PeriodId,
                                                            Target =  @Target,
                                                            LastUpdatedBy =  @LastUpdatedBy, 
                                                            LastUpdatedDate = GetDate(),  
                                                            IsActive = @IsActive,
                                                            MasterEntityId = @MasterEntityId
                                                         WHERE UserTargetId =@UserTargetId 
                                    SELECT * FROM UserTarget WHERE UserTargetId = @UserTargetId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userTargetDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserTargetDTO(userTargetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating UserTargetDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userTargetDTO);
            return userTargetDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="userTargetDTO">UserTargetDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshUserTargetDTO(UserTargetDTO userTargetDTO, DataTable dt)
        {
            log.LogMethodEntry(userTargetDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                userTargetDTO.UserTargetId = Convert.ToInt32(dt.Rows[0]["UserTargetId"]);
                userTargetDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                userTargetDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                userTargetDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                userTargetDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                userTargetDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                userTargetDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of UserTarget based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of UserTargetDTO</returns>
        public List<UserTargetDTO> GetUserTargetDTOList(List<KeyValuePair<UserTargetDTO.SearchByUserTargetSearchParameters, string>> searchParameters,  SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<UserTargetDTO> userTargetDTOList = new List<UserTargetDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<UserTargetDTO.SearchByUserTargetSearchParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (
                           searchParameter.Key.Equals(UserTargetDTO.SearchByUserTargetSearchParameters.USER_TARGET_ID) ||
                           searchParameter.Key.Equals(UserTargetDTO.SearchByUserTargetSearchParameters.GAME_ID) ||
                           searchParameter.Key.Equals(UserTargetDTO.SearchByUserTargetSearchParameters.PERIOD_ID) ||
                            searchParameter.Key.Equals(UserTargetDTO.SearchByUserTargetSearchParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(UserTargetDTO.SearchByUserTargetSearchParameters.SITE_ID))

                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(UserTargetDTO.SearchByUserTargetSearchParameters.IS_ACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserTargetDTO userTargetDTO = GetUserTargetDTO(dataRow);
                    userTargetDTOList.Add(userTargetDTO);
                }
            }
            log.LogMethodExit(userTargetDTOList);
            return userTargetDTOList;
        }
    }
}
