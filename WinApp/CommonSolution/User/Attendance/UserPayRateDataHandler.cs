/********************************************************************************************
 * Project Name - UserPayRateDataHandler
 * Description  - DataHandler to setup User Pay Rate
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.130       05-Jul-2021      Nitin Pai  Added: Attendance and Pay Rate enhancement
 *2.130       11-Nov-2021      Deeksha    Modified to save/update only effective Date formate
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    ///  UserPayRate DataHandler  - Handles insert, update and select of  UserPayRate objects
    /// </summary>
    public class UserPayRateDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<UserPayRateDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<UserPayRateDTO.SearchByParameters, string>
            {
                {UserPayRateDTO.SearchByParameters.USER_PAY_RATE_ID, "up.UserPayRateId"},
                {UserPayRateDTO.SearchByParameters.USER_ID, "up.UserId"},
                {UserPayRateDTO.SearchByParameters.USER_ROLE_ID, "up.UserRoleId"},
                {UserPayRateDTO.SearchByParameters.USER_ID_IS_NULL, "up.UserId"},
                {UserPayRateDTO.SearchByParameters.USER_ROLE_ID_IS_NULL, "up.UserRoleId"},
                {UserPayRateDTO.SearchByParameters.PAY_TYPE, "up.PayType"},
                {UserPayRateDTO.SearchByParameters.EFFECTIVE_DATE,"up.EffectiveDate"},
                {UserPayRateDTO.SearchByParameters.SITE_ID, "up.site_id"},
                {UserPayRateDTO.SearchByParameters.IS_ACTIVE, "up.IsActive"}
            };
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from UserPayRate AS up";
        /// <summary>
        /// Default constructor of ProfileTypeDataHandler class
        /// </summary>
        public UserPayRateDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating UserPayRate Record.
        /// </summary>
        /// <param name="userPayRateDTO">UserPayRateDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(UserPayRateDTO userPayRateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userPayRateDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserPayRateId", userPayRateDTO.UserPayRateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserId", userPayRateDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserRoleId", userPayRateDTO.UserRoleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PayType", userPayRateDTO.PayType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RegularPayRate", userPayRateDTO.RegularPayRate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OverTimePayRate", userPayRateDTO.OverTimePayRate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EffectiveDate", userPayRateDTO.EffectiveDate.Date));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", userPayRateDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", userPayRateDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Inserts the UserPayRate record to the database
        /// </summary>
        /// <param name="userPayRateDTO">UserPayRateDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns UserPayRateDTO</returns>
        public UserPayRateDTO Insert(UserPayRateDTO userPayRateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userPayRateDTO, loginId, siteId);
            string query = @"INSERT INTO UserPayRate 
                                        ( 
                                            UserId,
                                            UserRoleId,
                                            PayType,
                                            RegularPayRate,
                                            OverTimePayRate,
                                            EffectiveDate,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            site_id,
                                            MasterEntityId

                                        ) 
                                VALUES 
                                        (
                                            @UserId,
                                            @UserRoleId,
                                            @PayType,
                                            @RegularPayRate,
                                            @OverTimePayRate,
                                            @EffectiveDate,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                        ) SELECT * FROM UserPayRate WHERE UserPayRateId  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userPayRateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserPayRateDTO(userPayRateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting UserPayRateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userPayRateDTO);
            return userPayRateDTO;
        }
        /// <summary>
        /// Updates the userPayRate record
        /// </summary>
        /// <param name="userPayRateDTO">UserPayRateDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns UserPayRateDTO</returns>
        public UserPayRateDTO Update(UserPayRateDTO userPayRateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userPayRateDTO, loginId, siteId);
            string query = @"UPDATE UserPayRate 
                             SET UserId=@UserId,
                                 UserRoleId=@UserRoleId,
                                 PayType=@PayType,
                                 RegularPayRate=@RegularPayRate,
                                 OverTimePayRate=@OverTimePayRate,
                                 EffectiveDate=@EffectiveDate,
                                 IsActive = @IsActive,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate=GETDATE(),
                                 MasterEntityId=@MasterEntityId
                             WHERE UserPayRateId = @UserPayRateId 
                       SELECT * FROM UserPayRate WHERE UserPayRateId  = @UserPayRateId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userPayRateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserPayRateDTO(userPayRateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating UserPayRateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userPayRateDTO);
            return userPayRateDTO;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="userPayRateDTO">UserPayRateDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshUserPayRateDTO(UserPayRateDTO userPayRateDTO, DataTable dt)
        {
            log.LogMethodEntry(userPayRateDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                userPayRateDTO.UserPayRateId = Convert.ToInt32(dt.Rows[0]["UserPayRateId"]);
                userPayRateDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                userPayRateDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                userPayRateDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                userPayRateDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                userPayRateDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                userPayRateDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to UserPayRateDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns UserPayRateDTO</returns>
        private UserPayRateDTO GetUserPayRateDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            UserPayRateDTO userPayRateDTO = new UserPayRateDTO(Convert.ToInt32(dataRow["UserPayRateId"]),
                                            dataRow["UserId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["UserId"]),
                                            dataRow["UserRoleId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["UserRoleId"]),
                                            dataRow["PayType"] == DBNull.Value ? string.Empty : dataRow["PayType"].ToString(),
                                            dataRow["RegularPayRate"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["RegularPayRate"]),
                                            dataRow["OverTimePayRate"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["OverTimePayRate"]),
                                            dataRow["EffectiveDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EffectiveDate"]),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(userPayRateDTO);
            return userPayRateDTO;
        }
        /// <summary>
        /// Gets the UserPayRate data of passed UserPayRate Id
        /// </summary>
        /// <param name="userPayRateId">integer type parameter</param>
        /// <returns>Returns UserPayRateDTO</returns>
        public UserPayRateDTO GetUserPayRateDTO(int userPayRateId)
        {
            log.LogMethodEntry(userPayRateId);
            UserPayRateDTO returnValue = null;
            string query = @"SELECT *
                            FROM UserPayRate
                            WHERE UserPayRateId = @UserPayRateId";
            SqlParameter parameter = new SqlParameter("@UserPayRateId", userPayRateId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetUserPayRateDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// Gets the UserPayRateDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of UserPayRateDTO matching the search criteria</returns>
        public List<UserPayRateDTO> GetUserPayRateDTOList(List<KeyValuePair<UserPayRateDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<UserPayRateDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<UserPayRateDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == UserPayRateDTO.SearchByParameters.USER_PAY_RATE_ID ||
                            searchParameter.Key == UserPayRateDTO.SearchByParameters.USER_ID ||
                            searchParameter.Key == UserPayRateDTO.SearchByParameters.USER_ROLE_ID ||
                            searchParameter.Key == UserPayRateDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UserPayRateDTO.SearchByParameters.USER_ROLE_ID_IS_NULL ||
                                 searchParameter.Key == UserPayRateDTO.SearchByParameters.USER_ID_IS_NULL)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " is null ");
                        }
                        else if (searchParameter.Key == UserPayRateDTO.SearchByParameters.PAY_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == UserPayRateDTO.SearchByParameters.EFFECTIVE_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == UserPayRateDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == UserPayRateDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<UserPayRateDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserPayRateDTO userPayRateDTO = GetUserPayRateDTO(dataRow);
                    list.Add(userPayRateDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
