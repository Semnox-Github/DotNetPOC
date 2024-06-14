/********************************************************************************************
 * Project Name - User Period Data Handler
 * Description  - Data handler of the User Period class
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
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.User
{
    public class UserPeriodDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM UserPeriod AS up ";

        /// <summary>
        /// Dictionary for searching Parameters for the UserPeriod object.
        /// </summary>
        private static readonly Dictionary<UserPeriodDTO.SearchByUserPeriodSearchParameters, string> DBSearchParameters = new Dictionary<UserPeriodDTO.SearchByUserPeriodSearchParameters, string>
            {
                {UserPeriodDTO.SearchByUserPeriodSearchParameters.PERIOD_ID, "up.PeriodId"},
                {UserPeriodDTO.SearchByUserPeriodSearchParameters.FROM_DATE, "up.FromDate"},
                {UserPeriodDTO.SearchByUserPeriodSearchParameters.TO_DATE, "up.ToDate"},
                {UserPeriodDTO.SearchByUserPeriodSearchParameters.SITE_ID, "up.site_id"},
                {UserPeriodDTO.SearchByUserPeriodSearchParameters.IS_ACTIVE, "up.IsActive"},
                {UserPeriodDTO.SearchByUserPeriodSearchParameters.MASTER_ENTITY_ID, "up.MasterEntityId"}
             };
        /// <summary>
        /// Parameterized Constructor for UserTargetDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public UserPeriodDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating UserPeriod Record.
        /// </summary>
        /// <param name="userPeriodDTO">UserPeriodDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(UserPeriodDTO userPeriodDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userPeriodDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PeriodId", userPeriodDTO.PeriodId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", userPeriodDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromDate", userPeriodDTO.FromDate == DateTime.MinValue ? DBNull.Value : (object)userPeriodDTO.FromDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToDate", userPeriodDTO.ToDate == DateTime.MinValue ? DBNull.Value : (object)userPeriodDTO.ToDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentId", userPeriodDTO.ParentId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", userPeriodDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", userPeriodDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Converts the Data row object to UserPeriodDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the UserPeriodDTO</returns>
        private UserPeriodDTO GetUserPeriodDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            UserPeriodDTO userPeriodDTO = new UserPeriodDTO(
                                                         dataRow["PeriodId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PeriodId"]), 
                                                         dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                                         dataRow["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["FromDate"]),
                                                         dataRow["ToDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ToDate"]),
                                                         dataRow["ParentId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentId"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"])
                                                        );
            log.LogMethodExit(userPeriodDTO);
            return userPeriodDTO;
        }
        /// <summary>
        /// Gets the UserPeriod data of passed id 
        /// </summary>
        /// <param name="id">id of UserPeriod is passed as parameter</param>
        /// <returns>Returns UserPeriod</returns>
        public UserPeriodDTO GetUserPeriodDTO(int id)
        {
            log.LogMethodEntry(id);
            UserPeriodDTO result = null;
            string query = SELECT_QUERY + @" WHERE up.PeriodId= @PeriodId";
            SqlParameter parameter = new SqlParameter("@PeriodId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetUserPeriodDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Delete the record from the UserPeriod database based on Id
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int Id)
        {
            log.LogMethodEntry(Id);
            string query = @"DELETE  
                             FROM UserPeriod
                             WHERE UserPeriod.PeriodId = @PeriodId";
            SqlParameter parameter = new SqlParameter("@Id", Id);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Inserts the UserPeriodDTO record to the database
        /// </summary>
        /// <param name="UserPeriodDTO">UserPeriodDTO type object</param>
        /// <returns>Returns inserted record id</returns>
        public UserPeriodDTO InsertUserPeriod(UserPeriodDTO userPeriodDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userPeriodDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[UserPeriod]
                                                         (
                                                            Name,
                                                            FromDate,
                                                            ToDate,
                                                            ParentId,
                                                            LastUpdatedBy,
                                                            LastUpdateDate,
                                                            Guid,
                                                            site_id,
                                                            MasterEntityId,
                                                            CreatedBy,
                                                            CreationDate,
                                                            IsActive
                                                         )
                                                       values
                                                         (
                                                            @Name,
                                                            @FromDate,
                                                            @ToDate,
                                                            @ParentId,
                                                            @LastUpdatedBy,
                                                            GetDate(),
                                                            NewId(),
                                                            @site_id,
                                                            @MasterEntityId,
                                                            @CreatedBy,
                                                            GETDATE(),
                                                            @IsActive
                                                        ) SELECT* FROM UserPeriod WHERE PeriodId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userPeriodDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserPeriodDTO(userPeriodDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting UserPeriodDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userPeriodDTO);
            return userPeriodDTO;
        }
        /// <summary>
        /// Updates the UserPeriodDTO record to the database
        /// </summary>
        /// <param name="userTargetDTO">UserPeriodDTO type object</param>
        /// <returns>Returns the count of updated rows</returns>
        public UserPeriodDTO UpdateUserPeriod(UserPeriodDTO userPeriodDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userPeriodDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[UserPeriod]
                           SET 
                                                            Name =  @Name,
                                                            FromDate =  @FromDate,
                                                            ToDate =  @ToDate,
                                                            ParentId =  @ParentId,
                                                            LastUpdatedBy =  @LastUpdatedBy, 
                                                            LastUpdateDate = GetDate(),
                                                            IsActive = @IsActive,
                                                            MasterEntityId = @MasterEntityId
                                                         WHERE PeriodId =@PeriodId 
                                    SELECT * FROM UserPeriod WHERE PeriodId = @PeriodId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userPeriodDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserPeriodDTO(userPeriodDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating UserPeriodDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userPeriodDTO);
            return userPeriodDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="userTargetDTO">UserPeriodDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshUserPeriodDTO(UserPeriodDTO userPeriodDTO, DataTable dt)
        {
            log.LogMethodEntry(userPeriodDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                userPeriodDTO.PeriodId = Convert.ToInt32(dt.Rows[0]["PeriodId"]);
                userPeriodDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                userPeriodDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                userPeriodDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                userPeriodDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                userPeriodDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                userPeriodDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of UserTarget based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of UserPeriodDTO</returns>
        public List<UserPeriodDTO> GetUserPeriodDTOList(List<KeyValuePair<UserPeriodDTO.SearchByUserPeriodSearchParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<UserPeriodDTO> userPeriodDTOList = new List<UserPeriodDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<UserPeriodDTO.SearchByUserPeriodSearchParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (
                           searchParameter.Key.Equals(UserPeriodDTO.SearchByUserPeriodSearchParameters.PERIOD_ID) ||
                           searchParameter.Key.Equals(UserPeriodDTO.SearchByUserPeriodSearchParameters.PARENT_ID) ||
                            searchParameter.Key.Equals(UserPeriodDTO.SearchByUserPeriodSearchParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(UserPeriodDTO.SearchByUserPeriodSearchParameters.NAME))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == UserPeriodDTO.SearchByUserPeriodSearchParameters.SITE_ID)

                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(UserPeriodDTO.SearchByUserPeriodSearchParameters.FROM_DATE)
                            || searchParameter.Key.Equals(UserPeriodDTO.SearchByUserPeriodSearchParameters.TO_DATE))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(UserPeriodDTO.SearchByUserPeriodSearchParameters.IS_ACTIVE))
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
                    UserPeriodDTO userPeriodDTO = GetUserPeriodDTO(dataRow);
                    userPeriodDTOList.Add(userPeriodDTO);
                }
            }
            log.LogMethodExit(userPeriodDTOList);
            return userPeriodDTOList;
        }
    }
}
